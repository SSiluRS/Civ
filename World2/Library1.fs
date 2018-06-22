namespace World
open MapGenerator
open MapGeneratorFromCS

module Unit =
    type VeteranStatus =
        | Regular 
        | Veteran 
    type UnitClass =
        | Settlers
        | Militia
        | Phalanx
        | Chivalery 
        | Legion
        | Chariot 
        | Catapult
        | Knight 
    type Unit = {unitClass : UnitClass; veteran : VeteranStatus}

module City =
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

open City

module Science =
    type Discovery =
        | BronzeWorking
        | IronWorking
        | Wheel
        | Math
        | HorsebackRiding   
        | Feudalism
        | Monarchy
        | CeremonialBurial
        | Pottery
        | Currency
        | Alphabet
        | Literacy
        | Nothing

module Player =
    type Player = 
        {
            name : string; 
            money : int; 
            discoveries : Science.Discovery list; 
            taxScience : int; 
            taxLuxury : int;
            cities : Map<int,City.City>;
            currentlyDiscovering : Science.Discovery;
            researchProgress : int;
            units : Map<int, Unit>;
        }
 
module World = 
    open MapGeneratorFromCS
    type Resource = 
        | Shield //производство
        | Trade //наука либо деньги
        | Food //увеличение жителей

    type World = 
        {
            worldMap: Map<int, LandTerrain>; 
            playerList : Player.Player list;
        } 
      
module WorldUpdate =
    open MapGeneratorFromCS.MapGeneratorFromCS
    open World

    let mapWidth = 320
    let mapHeight = 160

    let RowColToN row col = row * mapWidth + col
    let NToRowCol n w = n % w, n / w


    let getWorldMapCell x y (worldMap:Map<int,LandTerrain>) =
        worldMap.Item (RowColToN y x)

    let applyFnToMap fn x y w n =
        let r, c = NToRowCol n w
        fn (x + c) (y + r)

    let iter2d x y w h fn =
        Seq.init(w*h) (fun n -> applyFnToMap fn x y w n)

    let find2d x y w h fn =
        Seq.tryFindIndex (applyFnToMap fn x y w) (Seq.init(w*h) (fun n -> n))

    let findCellForCity worldMap =
        let zz x y = 
            match (getWorldMapCell x y worldMap) with
            | LandTerrain.GrassLand _ -> true
            | _ -> false
        find2d 0 0 mapWidth mapHeight zz
        

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


    let GetCityCells cellIndex (worldMap:Map<int,LandTerrain>) =
        let zz2 r c =
            if 
                c = 2 && r = 2 || 
                c = 0 && r = 0 || c = 0 && r = 4 || 
                c = 4 && r = 0 || c = 4 && r = 4
            then None
            else Some(worldMap.Item (RowColToN r c))
            
        let x, y = NToRowCol cellIndex 5
        let zz = iter2d (x-2) (y-2) 5 5 zz2 
        Seq.choose (fun n -> n) zz

    let CellEconomicComparision cell1 cell2 =
        let cmp getCount cell1 cell2 =
            let a = getCount cell1 Happiness.Neutral
            let b = getCount cell2 Happiness.Neutral
            if a > b then -1 else if a < b then 1 else 0
        
        let f = cmp GetFoodCount cell1 cell2
        let s = cmp GetShieldsCount cell1 cell2
        let t = cmp GetTradeCount cell1 cell2

        if f <> 0 then f else if s <> 0 then s else t

    let GetSortedCityCells cellIndex = 
        let a = GetCityCells cellIndex
        let b = Seq.sortWith CellEconomicComparision a
        Seq.init(5*5) (fun n -> (n, Seq.item n b))
    
    let AssignFarmersToCell cellIndex farmerCount =
        let a = GetSortedCityCells cellIndex
        Seq.where (fun n -> 
                    let n1, n2 = n
                    n1 < farmerCount) a

    let worldMap = MapGeneratorFromCS.MapGeneratorFromCS.CreateWorldMap

    WorldUpdate.AssignFarmersToCell

    let city = 
        { 
            name = "Moscow"; 
            currentlyBuilding = City.CurrentlyBuilding.Nothing;
            production = 0;
            population = 1;
            occupation = [];
            food = 0;
            building =[];
            happiness = City.Happiness.Neutral;
        }

    let currentWorld = 
        {
            worldMap = MapGeneratorFromCS.MapGeneratorFromCS.CreateWorldMap;
            playerList = 
                [{
                    name = "Player";
                    money = 0;
                    discoveries = [];
                    taxScience = 50;
                    taxLuxury = 50;
                    cities = Map.ofList([(10000, city)])
                    currentlyDiscovering = Science.Discovery.Nothing;
                    researchProgress = 0;
                    units = Map.empty;
                }]
        }        

    let UpdateWorld oldWorld = 
        let newWorld = oldWorld
        


        newWorld
