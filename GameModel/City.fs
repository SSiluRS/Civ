namespace GameModel
module City =
    open WorldMap
    open MapGeneratorFromCS.MapGeneratorFromCS
    open Misc
    open System.Collections.Generic
    open Units
    open Unit

    type Resource = 
        | Shield //производство
        | Trade //наука либо деньги
        | Food //увеличение жителей

    (*type Building =
        | Barrack //Город строит ветеранов
        | Temple //Минус 1 недовольный житель
        | Granary // При увеличении города теряется только половина запасов еды
        | Marketplace // Одна единица торговли превращается в 1,5 единицы денег
        | Palace // Нет коррупции
        | Library // 1 единица торговли даёт 1,5 единицы науки*)
        
    type CurrentlyBuilding =
        | Building of Buildings.Building
        | Unit of UnitClass
        | TradeGoods

    type Occupation =
        | Farmer of int*int
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
            building : Buildings.Building list;
            happiness : Happiness;
            units : Unit list;
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

    let costToBuild (currentlyBuilding:CurrentlyBuilding) = 
        match currentlyBuilding with
        | Building b -> b.cost
        | Unit (u) -> u.prod
        | TradeGoods -> 2

    let CellEconomicComparision cell1 cell2 =
        let cmp getCount cell1 cell2 =
            let a = getCount cell1 Happiness.Neutral
            let b = getCount cell2 Happiness.Neutral
            if a > b then -1 else if a < b then 1 else 0
        
        let f = cmp GetFoodCount cell1 cell2
        let s = cmp GetShieldsCount cell1 cell2
        let t = cmp GetTradeCount cell1 cell2

        if f <> 0 then f else if s <> 0 then s else t

    let GetCityCells c r (worldMap : Map<int*int,LandTerrain>) =
        let zz2 c1 r1 =
            if 
                c1 = c && r1 = r || r1 < 0 || c1 < 0 ||
                c1 = c-2 && r1 = r-2 || c1 = c-2 && r1 = r+2 || 
                c1 = c+2 && r1 = r-2 || c1 = c+2 && r1 = r+2
            then None
            else Some(KeyValuePair((c1, r1), (worldMap.Item (c1,r1))))
            
        let zz = iter2d (c-2) (r-2) 5 5 zz2 
        Seq.choose (fun n -> n) zz

    let AssignFarmersToCell c r farmerCount worldMap =
        let GetSortedCityCells c r = 
            let a = GetCityCells c r worldMap
            let cmp2 (t1:KeyValuePair<int*int,LandTerrain>) (t2:KeyValuePair<int*int,LandTerrain>) = 
                CellEconomicComparision t1.Value t2.Value
            Seq.sortWith cmp2 a

        let a = GetSortedCityCells c r
        let b = Seq.take farmerCount a
        Seq.map (fun (n:KeyValuePair<int*int,LandTerrain>) -> Occupation.Farmer (fst n.Key, snd n.Key)) b

    let AddNewFarmerToCity c r (existingFarmers:Occupation list) worldMap = 
        let onlyFarmers n =
            match n with
            | Farmer _ -> true
            | _ -> false
        let farmers = List.where onlyFarmers existingFarmers

        let GetSortedCityCells c r = 
            let a = GetCityCells c r worldMap
            let cmp2 (t1:KeyValuePair<int*int,LandTerrain>) (t2:KeyValuePair<int*int,LandTerrain>) = 
                CellEconomicComparision t1.Value t2.Value
            Seq.sortWith cmp2 a
        let checkFarmers (n:KeyValuePair<int*int, LandTerrain>) = 
            let coords = n.Key
            let getFarmerCoords f =
                match f with 
                | Farmer (c,r) -> Some(c,r)
                | _ -> None
            let a = List.choose (fun n -> n) (List.map (fun n -> getFarmerCoords n) farmers)
            not (List.contains coords a)

        let a = GetSortedCityCells c r
        if List.length farmers = 20 then Artist
        else Farmer (Seq.find (fun n -> checkFarmers n) a).Key

    let ChangeOccupation (occupation: Occupation list) n (newOccupation: Occupation) =
        List.mapi (fun i e -> if i <> n then e else newOccupation) 

    let findCellForCity2 worldMap =
        let zz (c,r) = 
            let a = 
                match (getWorldMapCell worldMap c r) with
                | LandTerrain.GrassLand _ -> true
                | _ -> false
            a
        find2d 0 0 mapWidth mapHeight zz

    let findCellForCity1 worldMap c r =
        if (getWorldMapCell worldMap c r) <> (LandTerrain.GrassLand PlainUpgrades.Nothing) then None
        else
            let zz c r = 
                if c < 0 || r < 0 || c > 319 || r > 159 then LandTerrain.Unknown else getWorldMapCell worldMap c r
            let terrains = iter2d (c-2) (r-2) 5 5 zz
            let grass = Seq.where (fun n -> n = (LandTerrain.GrassLand PlainUpgrades.Nothing)) terrains
            if Seq.contains LandTerrain.Ocean terrains && Seq.length grass >= 3 
            then Some(c, r) 
            else None
        
    let findCellForCity worldMap =
        let a = iter2d 0 0 mapWidth mapHeight (findCellForCity1 worldMap)
        let b = Seq.choose (fun n -> n) a
        if Seq.length b >= 1 then Some(Seq.item 0 b) else None

    let GetFarmersYield worldMap c r (city: City) =
        let occ = city.occupation
        let zz n =
            match n with
            | Farmer(c1,r1) -> Some(getWorldMapCell worldMap c1 r1)
            | _ -> None
        let b = (getWorldMapCell worldMap c r) :: (List.map zz occ |> List.choose (fun n -> n))
        List.map (fun n -> 
                    let h = Happiness.Neutral
                    let shields = (if n = b.[0] && GetShieldsCount n h = 0 then 1 else GetShieldsCount n h)
                    let trade = (if n = b.[0] && GetTradeCount n h = 0 then 1 else GetTradeCount n h)
                    let food = (if n = b.[0] && GetFoodCount n h <= 1 then 2 else GetFoodCount n h)
                    shields, trade, food
                ) b
