module City
open WorldMap
open MapGeneratorFromCS.MapGeneratorFromCS
open Misc

type Resource = 
    | Shield //производство
    | Trade //наука либо деньги
    | Food //увеличение жителей

type Building =
    | Barrack //Город строит ветеранов
    | Temple //Минус 1 недовольный житель
    | Granary // При увеличении города теряется только половина запасов еды
    | Marketplace // Одна единица торговли превращается в 1,5 единицы денег
    | Palace // Нет коррупции
    | Library // 1 единица торговли даёт 1,5 единицы науки
        
type CurrentlyBuilding =
    | Building of Building
    | Unit of Unit
    | Nothing

type Occupation =
    | Farmer of int
    | Scientist
    | TaxCollector
    | Artist

type Happiness =
    | Neutral
    | Happy
    | Unhappy

type City = 
    {
        name : string; 
        currentlyBuilding : CurrentlyBuilding;
        production : int;
        population : int;
        occupation : Occupation list; //номера квадратов вокруг города, которые обрабатывают фермеры
        food : int;
        building : Building list;
        happiness : Happiness;
    } 
   
let GetShieldsCount cell happiness =
    match cell with
        | (LandTerrain.Ocean) -> 0
        | (LandTerrain.Desert _) -> 1
        | (LandTerrain.Forest _) -> 2
        | (LandTerrain.Mountain _) -> 1
        | (LandTerrain.River _) -> 1
        | (LandTerrain.Snow _) -> 0
        | (LandTerrain.Tundra _) -> 0
        | (LandTerrain.GrassLand _) -> 1
        | _ -> 0

let GetFoodCount cell happiness =
    match cell with
        | (LandTerrain.Ocean) -> 1
        | (LandTerrain.Desert _) -> 0
        | (LandTerrain.Forest _) -> 1
        | (LandTerrain.Mountain _) -> 0
        | (LandTerrain.River _) -> 2
        | (LandTerrain.Snow _) -> 0
        | (LandTerrain.Tundra _) -> 1
        | (LandTerrain.GrassLand _) -> 2
        | _ -> 0

let GetTradeCount cell happiness =
    match cell with
        | (LandTerrain.Ocean) -> 2
        | (LandTerrain.Desert _) -> 0
        | (LandTerrain.Forest _) -> 0
        | (LandTerrain.Mountain _) -> 0
        | (LandTerrain.River _) -> 1
        | (LandTerrain.Snow _) -> 0
        | (LandTerrain.Tundra _) -> 0
        | (LandTerrain.GrassLand _) -> 0
        | _ -> 0

let CellEconomicComparision cell1 cell2 =
    let cmp getCount cell1 cell2 =
        let a = getCount cell1 Happiness.Neutral
        let b = getCount cell2 Happiness.Neutral
        if a > b then -1 else if a < b then 1 else 0
        
    let f = cmp GetFoodCount cell1 cell2
    let s = cmp GetShieldsCount cell1 cell2
    let t = cmp GetTradeCount cell1 cell2

    if f <> 0 then f else if s <> 0 then s else t

let GetCityCells cellIndex (worldMap : Map<int,LandTerrain>) =
    let x, y = NToRowCol cellIndex mapWidth
        
    let zz2 c r =
        let n = RowColToN r c
        if 
            c = x && r = y || r < 0 || c < 0 ||
            c = x-2 && r = y-2 || c = x-2 && r = y+2 || 
            c = x+2 && r = y-2 || c = x+2 && r = y+2
        then None
        else Some(n, worldMap.Item n)
            
    let zz = iter2d (x-2) (y-2) 5 5 zz2 
    Seq.choose (fun n -> n) zz

let AssignFarmersToCell cellIndex farmerCount worldMap =
    let GetSortedCityCells cellIndex = 
        let a = GetCityCells cellIndex worldMap
        let cmp2 t1 t2 = 
            CellEconomicComparision (snd t1) (snd t2)
        Seq.sortWith cmp2 a

    let a = GetSortedCityCells cellIndex
    let b = Seq.take farmerCount a
    Seq.map (fun n -> Occupation.Farmer (fst n)) b

let findCellForCity worldMap =
    let zz x y = 
        let a = 
            match (getWorldMapCell x y worldMap) with
            | LandTerrain.GrassLand _ -> true
            | _ -> false
        a
    find2d 0 0 mapWidth mapHeight zz

let GetFarmersYield worldMap cellIndex (city: City) =
    let occ = city.occupation
        
    let zz n =
        match n with
        | Farmer(i) -> Some(i2c worldMap i)
        | _ -> None
    let b = (i2c worldMap cellIndex) :: (List.map zz occ |> List.choose (fun n -> n))

    List.map (fun n -> 
                let h = Happiness.Neutral
                GetShieldsCount n h, GetTradeCount n h, GetFoodCount n h) b
