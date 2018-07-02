namespace GameModel
module World =
    open MapGeneratorFromCS.MapGeneratorFromCS
    open Civilization
    open System
    open Unit
    open City
    open Science
    open GameModel
    open Units
    
    type UnitPack = 
        {
            units : Unit.Unit list
        }

    type WorldEvents =
    | NewBuilding of CurrentlyBuilding*City
    | NewDiscovery of Science.Advance*Civilization
    | NewRoad of int*int

    type World = 
        {
            unitsCount : int;
            rnd: Random;
            worldMap: Map<int*int, LandTerrain>; 
            playerList : Civilization list;
            units : Map<int*int, UnitPack>;
            currentPlayer : int;
            worldEvents : WorldEvents list
        }
        
    let getCellUnits (world:World) c r =
        world.units.TryFind (c,r) 

    let addUnitToList (unitPack : UnitPack) unit =
        unit :: unitPack.units

    let getUnitLoc (world: World) unit = 
        let zz key (t:UnitPack) =
            List.exists (fun (n:Unit) -> n.ID = unit.ID) t.units
        Map.findKey zz world.units

    let moveUnitBitweenPacks world unit c0 r0 c1 r1 =
        let packFrom = world.units.Item (c0,r0)
        let packTo = world.units.TryFind (c1,r1)
        let packFrom = 
            {
                packFrom with units = List.where (fun n -> n <> unit) packFrom.units
            }
        let packTo =
            match packTo with
            | Some (pack) ->
                    {
                        pack with units = unit :: pack.units
                    }
            | None ->
                    {
                        units = [unit]
                    }
        packFrom, packTo

    let getCivByUnit (world:World) unit =
        let zz key (t:UnitPack) =
            List.contains unit t.units
        let ID = unit.ID
        let key = Map.findKey zz world.units
        let a = world.units.Item key
        List.find (fun (n:Civilization) -> List.contains ID n.unitIDs) world.playerList

    let updFogOfWar c r fogOfWar =
        let zz c2 r2 = (c2,r2), true
        let a = Map.ofSeq (Misc.iter2d (c-1) (r-1) 3 3 zz)
        Map.ofSeq (Seq.concat [(Map.toSeq a); (Map.toSeq fogOfWar)])

    let attackerWins (world:World) (attacker:Unit.Unit) (defender:Unit.Unit) =
        let defencerCoords = getUnitLoc world defender
        let attackerCoords = getUnitLoc world attacker
        let c,r = attackerCoords
       
        let map = Map.add defencerCoords {(world.units.Item attackerCoords) with units = [{attacker with movesMade = attacker.movesMade + getMovementCost (world.worldMap.Item (c,r)) attacker; veteran = VeteranStatus.Veteran}]} world.units
        let map1 = Map.remove attackerCoords map

        let civ = getCivByUnit world defender

        let newCiv = {civ with unitIDs = List.where (fun n -> n <> defender.ID) civ.unitIDs; fogOfWar = updFogOfWar c r civ.fogOfWar}
        let newCivs = List.map (fun n -> if n = civ then newCiv else n) world.playerList

        ({ world with units = map1; playerList = newCivs }, Some({attacker with movesMade = attacker.movesMade + 1}))

    let defenderWins (world:World) (attacker:Unit.Unit) (defender:Unit.Unit) =
        let attackerCoords = getUnitLoc world attacker

        let map1 = Map.remove attackerCoords world.units

        let civ = getCivByUnit world attacker
        let newCiv = {civ with unitIDs = List.where (fun n -> n <> defender.ID) civ.unitIDs}
        let newCivs = List.map (fun n -> if n = civ then newCiv else n) world.playerList

        ({ world with units = map1; playerList = newCivs }, None)

    let attackPower (attacker:Unit.Unit) =
        let attack1 = (Unit.getUnitAttack attacker.unitClass) * 10
        let attackMultiplier = 
            match attacker.veteran with
            | VeteranStatus.Veteran -> attack1/2
            | VeteranStatus.Regular -> 0
        attack1 + attackMultiplier

    let defencePower (world:World) (defender: Unit.Unit) =
        let defenceTerrain = world.worldMap.Item (Map.findKey (fun key (t:UnitPack) -> List.contains defender t.units) world.units)
        let defence1 = (Unit.getUnitDefence defender.unitClass) * 10
        let defenceMultiplier1 =
            match defenceTerrain with //река и холмы +50 горы +100
            | LandTerrain.Hill _ -> defence1/2
            | LandTerrain.River _ -> defence1/2
            | LandTerrain.Mountain _ -> defence1
            | _ -> 0
        let defenceMultiplier2 = 
            match defender.veteran with
            | VeteranStatus.Veteran -> defence1/2
            | VeteranStatus.Regular -> 0
        defence1 + defenceMultiplier1 + defenceMultiplier2

    let attackMove (world:World) (attacker:Unit.Unit) (defenders:Unit.Unit list) =
        let defender1 = List.fold (fun acc e -> if (fst acc) < (defencePower world e) then (defencePower world e,e) else acc) (0, defenders.[0]) defenders
        let defender = snd defender1
        let attack = attackPower attacker
        let defence = defencePower world defender

        let chance = world.rnd.Next(attack + defence)
        if chance < attack 
        then attackerWins world attacker defender
        else defenderWins world attacker defender

    let moveUnit (world:World) (unit:Unit.Unit) c r =
        let c0,r0 = getUnitLoc world unit
        let civ = getCivByUnit world unit
        let fromPack,toPack = moveUnitBitweenPacks world unit c0 r0 c r 
        let map1 = 
            if List.length fromPack.units <> 0 
            then (Map.add (c0,r0) fromPack world.units) 
            else (Map.remove (c0,r0) world.units)

        let newCivNoMoves = { civ with fogOfWar = updFogOfWar c0 r0 civ.fogOfWar}
        let newCivsNoMoves = List.map (fun n -> if n = civ then newCivNoMoves else n) world.playerList

        let newCiv = { civ with fogOfWar = updFogOfWar c r (updFogOfWar c0 r0 civ.fogOfWar)}
        let newCivs = List.map (fun n -> if n = civ then newCiv else n) world.playerList

        let newWorld = ({ world with units = Map.add (c,r) {toPack with units = {unit with movesMade = unit.movesMade + getMovementCost (world.worldMap.Item (c,r)) unit} :: (List.where (fun n -> n <> unit) toPack.units)} map1; playerList = newCivs }, Some({unit with movesMade = unit.movesMade + 1}))
        match getCellUnits world c r with
        | Some(pack) ->
            if (world.worldMap.Item (c,r) = LandTerrain.Ocean) || 
               (unit.movesMade >= getUnitMovement unit.unitClass) || 
               (getCivByUnit world unit <> world.playerList.[world.currentPlayer]) ||
               ((getUnitMovement unit.unitClass - unit.movesMade) < (getMovementCost (world.worldMap.Item (c,r)) unit)) || 
               (fst (fst unit.roadInfo) = true) then ( { world with playerList = newCivsNoMoves }, Some(unit))
            else if List.length pack.units >= 1 && (getCivByUnit world pack.units.[0]) = (getCivByUnit world unit) || List.length pack.units = 0 then newWorld 
            else attackMove { world with playerList = newCivsNoMoves } unit (world.units.Item (c,r)).units

        | None -> 
            if (world.worldMap.Item (c,r) = LandTerrain.Ocean) || 
               (unit.movesMade >= getUnitMovement unit.unitClass) || 
               (getCivByUnit world unit <> world.playerList.[world.currentPlayer]) ||
               ((getUnitMovement unit.unitClass - unit.movesMade) < (getMovementCost (world.worldMap.Item (c,r)) unit)) || 
               (fst (fst unit.roadInfo) = true) then (world, Some(unit))
            else newWorld            

    let unitMakesCity (world:World) (unit:Unit) =
        let civ = getCivByUnit world unit
        let key = List.findIndex (fun n -> n = civ) world.playerList
        let c,r = getUnitLoc world unit
        let cityExists = civ.cities.TryFind (c,r)
        let city = 
            match cityExists with
            | Some(c) -> { c with population = c.population + 1}
            | None -> 
                { 
                    name = System.DateTime.Now.ToShortTimeString();
                    currentlyBuilding = City.CurrentlyBuilding.TradeGoods;
                    production = 0;
                    population = 1;
                    occupation = List.ofSeq (AssignFarmersToCell c r 1 world.worldMap);
                    food = 0;
                    building =[];
                    happiness = City.Happiness.Neutral;
                    units = []
                }
        let unitPack = (world.units.Item (c,r))
        let newUnits = 
            if List.length unitPack.units > 1 
            then Map.add (c,r) { unitPack with units = List.where (fun n -> n <> unit) unitPack.units } world.units 
            else Map.remove (c,r) world.units
        let newCities = Map.add (c,r) city world.playerList.[key].cities
        let newCiv = {civ with cities = newCities; unitIDs = List.where (fun n -> n <> unit.ID) civ.unitIDs}
        let newCivs = List.map (fun n -> if n = civ then newCiv else n) world.playerList
        {  world with playerList = (if fst (fst unit.roadInfo) = true then world.playerList else newCivs); units = newUnits }

    let getUnitWithMoves (world:World) =
        let civ = world.playerList.[world.currentPlayer]
        let allUnits = Map.fold (fun acc key n -> n @ acc) List.empty (Map.map (fun key (n: UnitPack) -> n.units) world.units)
        let checkDirections (unit:Unit) =
            let c,r = getUnitLoc world unit
            let zz c1 r1 =
                if (c1 <> r && r1 <> r) then 
                    ((getUnitMovement unit.unitClass - unit.movesMade) >= (getMovementCost (world.worldMap.Item (c1,r1)) unit))
                else false
            let a = Misc.iter2d (c-1) (r-1) 3 3 zz
            let b = Seq.tryFind (fun n -> n = true) a
            match b with
            | Some _ -> true
            | None -> false
        let civUnits = List.where (fun n -> getCivByUnit world n = civ && checkDirections n) allUnits
        if List.length civUnits >= 1 then Some(civUnits.[0]) else None

    let settlerBuildsRoad (world: World) (settler : Unit.Unit) =
        let civ = getCivByUnit world settler
        let c,r = getUnitLoc world settler
        let cityCoords = Map.tryFindKey (fun key (n: City) -> List.contains settler n.units) civ.cities
        let city = 
            match cityCoords with
            | Some (c,r) -> Some(civ.cities.Item (c,r))
            | None -> None
        let newUnit = 
            if settler.unitClass = Units.Settlers 
            then { settler with roadInfo = (true, 0),(c,r); movesMade = 9 }
            else settler
        let unitCoords = Map.findKey (fun key (n : UnitPack) -> List.contains settler n.units) world.units
        let newUnits = Map.map (fun key (n : UnitPack) -> if key = unitCoords then { n with units = List.map (fun n -> if n = settler then newUnit else n) n.units } else n) world.units   
        let newCities = 
            match city with
            | Some (city) -> 
                match cityCoords with
                | Some (coords) -> Map.add coords { city with units = List.map (fun n -> if n = settler then newUnit else n) city.units }  civ.cities
                | None -> civ.cities
            | None -> civ.cities
        let newCiv = { civ with cities = newCities }
        let newCivs = List.map (fun n -> if n = civ then newCiv else n) world.playerList
        {  world with playerList = newCivs; units = newUnits }

    let researchDestination (civ: Civilization) =
        15 + 14 * (List.length  civ.discoveries)

    let currentBuildingDestination (city : City) =
        match city.currentlyBuilding with
        | Building b -> b.cost
        | Unit u -> u.prod
        | TradeGoods -> 2

    let changeResearch (world:World) (civ: Civilization) (newResearch : Science.Advance) =
        let newCiv = { civ with currentlyDiscovering = newResearch }
        { world with playerList = List.map (fun n -> if n = civ then newCiv else n) world.playerList}

    let changeCurrentBuilding (world:World) (city: City) (newBuilding : CurrentlyBuilding) =
        let civ = List.find (fun (n:Civilization) -> (Map.tryFindKey (fun key n -> n = city) n.cities).IsSome) world.playerList
        let newCity = { city with currentlyBuilding = newBuilding }
        let newCities = Map.map (fun key n -> if n = city then newCity else n) civ.cities
        let newCiv = { civ with cities = newCities }
        { world with playerList = List.map (fun n -> if n = civ then newCiv else n) world.playerList }

    let updateCity (world : World) (city : City) = 
        //Get city civilization
        let civ = List.find (fun (n:Civilization) -> (Map.tryFindKey (fun key n -> n = city) n.cities).IsSome) world.playerList
                    
        //Get city coordinates
        let cityCoords = Map.findKey (fun key n -> n = city) civ.cities

        //Farmer's yield
        let yieldList = GetFarmersYield  world.worldMap (fst cityCoords) (snd cityCoords) city
        let shields, trade, food = 
                                List.fold (fun acc n -> 
                                            let a1, a2, a3 = acc
                                            let n1, n2, n3 = n
                                            a1+n1, a2+n2, a3+n3) (0,0,0) yieldList

        //Gain of production
        let production = city.production + shields

        //Get the cost of currently building
        let cost = costToBuild city.currentlyBuilding
        
        //Update city buildings
        let buildings, worldEvents = 
            match city.currentlyBuilding with
            | Building (b) -> if production >= cost then b :: city.building, WorldEvents.NewBuilding (CurrentlyBuilding.Building b,city) :: world.worldEvents else city.building,world.worldEvents
            | _ -> city.building, world.worldEvents

        //Buildings benefits
        let hasBarrack = List.contains Buildings.Barracks buildings
        let hasGranary = List.contains Buildings.Granary buildings
        let hasLibrary = List.contains Buildings.Library buildings
        let hasMarketplace = List.contains Buildings.Marketplace buildings

        //Get existing units in city cell
        let unitsInCell = 
            match (Map.tryFind cityCoords world.units) with
            | Some (units) -> units.units
            | None -> List.empty

        //Update TradeGoods building
        let production = 
            match city.currentlyBuilding with
            | TradeGoods -> if shields % 2 = 0 then 0 else 1
            | _ -> production

        //Update city units and general unit map
        let units, unitsMap, worldEvents =
            match city.currentlyBuilding with
            | Unit (u) ->
                    let unit = {unitClass = u; veteran = (if hasBarrack then Veteran else Regular); movesMade = 0; ID = world.unitsCount; roadInfo = (false, 0),(0,0)}
                    if production >= cost 
                    then (unit :: city.units), (Map.add cityCoords {units = unit :: unitsInCell} world.units), WorldEvents.NewBuilding (CurrentlyBuilding.Unit unit.unitClass, city) :: worldEvents
                    else city.units, world.units, worldEvents
            | _ -> (city.units, world.units, worldEvents)

        //Update currently building
        let currentlyBuilding = 
            if production >= cost 
            then 
                match city.currentlyBuilding with
                | Building b -> 
                    let a = List.where (fun (n : Buildings.Building)-> not (List.contains n buildings)) (Utils.allowedBuildings civ.discoveries)
                    if List.length a >= 1 then CurrentlyBuilding.Building a.[0] else CurrentlyBuilding.TradeGoods
                | Unit u -> CurrentlyBuilding.Unit u
                | TradeGoods -> CurrentlyBuilding.TradeGoods
            else city.currentlyBuilding

        //Gain of food 
        let food = city.food + food

        let foodDestination = 20 + 10 * (city.population - 1)

        //Update population
        let population = if (food >= foodDestination) then city.population + 1 else city.population
        let population =
            match city.currentlyBuilding with
            | Unit u -> if u = Units.Settlers && production >= cost then population - 1 else population
            | _ -> population
        //Update farmers
        let newOccupation = if (food >= foodDestination) then Some(AddNewFarmerToCity (fst cityCoords) (snd cityCoords) city.occupation world.worldMap) else None

        //Update food amount
        let food = if (food >= foodDestination) then food - (foodDestination/ (if hasGranary then 2 else 1)) else food
        
        //Update civlization's units ID's list
        let unitIDs = 
            match city.currentlyBuilding with
            | Unit _ -> 
                        if production >= cost 
                        then world.unitsCount :: civ.unitIDs
                        else civ.unitIDs
            | _ -> civ.unitIDs

        //Update amount of units ever existed
        let unitsCount = 
            match city.currentlyBuilding with
            | Unit _ -> 
                        if production >= cost
                        then world.unitsCount + 1
                        else world.unitsCount
            | _ -> world.unitsCount

        //Update production
        let production = if production >= cost then production - cost else production

        //Update current city
        let newCity = 
            { 
                city with 
                    production = production; 
                    food = food; 
                    population = population; 
                    building = buildings; 
                    units = List.map (fun n -> {n with movesMade = 0}) units; 
                    currentlyBuilding = currentlyBuilding; 
                    occupation = 
                        match newOccupation with
                        | Some(o) -> o :: city.occupation
                        | None -> city.occupation
            }

        //Update taxes
        let luxury = civ.taxLuxury
        let science = civ.taxScience
        let money = 100 - luxury - science

        let money = if hasMarketplace then trade * money * 15 / 1000 + civ.money else trade * money / 100 + civ.money
     
        //Update TradeGoods building
        let money = 
            match city.currentlyBuilding with
            | TradeGoods -> shields / 2 + money
            | _ -> money

        let researchProgress = if hasLibrary then  trade * science * 15 / 1000 + civ.researchProgress else  trade * science / 100 + civ.researchProgress
        let researchDestination = 15 + 14 * (List.length  civ.discoveries)
        let discoveries, worldEvents = 
            if (researchProgress >= researchDestination) 
            then civ.currentlyDiscovering :: civ.discoveries, WorldEvents.NewDiscovery (civ.currentlyDiscovering, civ) :: worldEvents
            else civ.discoveries, worldEvents
        let currentlyDiscovering = if (researchProgress >= researchDestination) then (Utils.allowedAdvances discoveries).[0] else civ.currentlyDiscovering
        let researchProgress = if (researchProgress >= researchDestination) then researchProgress - researchDestination else researchProgress

        //Update civilizations
        let newCiv = 
                { 
                    civ with 
                        cities = if population = 0 then Map.remove cityCoords civ.cities else Map.add cityCoords newCity civ.cities; 
                        unitIDs = if population = 0 then List.where (fun n ->List.contains n (List.map (fun n -> n.ID) city.units)) unitIDs else unitIDs; 
                        money = money; 
                        researchProgress = researchProgress;
                        discoveries = discoveries; 
                        currentlyDiscovering = currentlyDiscovering;
                }
        let newCivs = List.map (fun n -> if n = civ then newCiv else n) world.playerList

        { 
            world with 
                playerList = newCivs; 
                units = 
                    if population = 0 
                    then 
                        let a = Map.filter (fun key (n : UnitPack) -> List.length n.units <> 0) (Map.map (fun key n -> { n with units = List.where (fun t -> not (List.contains t.ID  civ.unitIDs)) n.units}) unitsMap)
                        let unit = {unitClass = Units.Settlers; veteran = Regular; movesMade = 0; ID = world.unitsCount; roadInfo = (false, 0),(0,0)}
                        if currentlyBuilding = CurrentlyBuilding.Unit Units.Settlers 
                        then Map.map (fun key (n : UnitPack) -> if key = cityCoords then { n with units = unit :: n.units} else n) a
                        else a
                    else unitsMap; 
                unitsCount = unitsCount;
                worldEvents = worldEvents
        } 

    let updateNonCities (world:World) =
        //Update Settlers who build roads
        let newSettler settler = 
            let settler1 = 
                if fst (fst settler.roadInfo) = true 
                then { settler with roadInfo = (true, snd (fst settler.roadInfo) + 1), (snd settler.roadInfo) }
                else settler
            if snd (fst settler1.roadInfo) = 2 then {settler1 with roadInfo = ((false, 0), snd settler.roadInfo)},true else settler1,false

        let upgradeLandTerrain key =
            let lt = (WorldMap.getWorldMapCell world.worldMap (fst key) (snd key))
            match lt with
            | River _ -> River PlainUpgrades.Road
            | Mountain _ -> Mountain MountainUpgrades.Road
            | Hill _ -> Hill HillUpgrades.Road
            | Desert _ -> Desert PlainUpgrades.Road
            | Forest _ -> Forest BadTerrainUpgrades.Road
            | GrassLand _ -> GrassLand PlainUpgrades.Road
            | Plain _ -> Plain PlainUpgrades.Road
            | Swamp _ -> Swamp BadTerrainUpgrades.Road
            | Snow _ -> Snow BadTerrainUpgrades.Road
            | Tundra _ -> Tundra BadTerrainUpgrades.Road
            | _ -> raise (new NotImplementedException())

        let units = Map.fold (fun acc key (n : UnitPack) -> acc @ n.units) List.empty world.units
        let a = List.map (fun n -> snd (fst (newSettler n)).roadInfo, snd (newSettler n)) (List.where (fun n -> n.unitClass = Units.Settlers) units)
        let a = List.map (fun n -> fst n) (List.where (fun n -> snd n = true) a)
        let worldEvents = List.fold (fun acc n -> WorldEvents.NewRoad n :: acc) List.empty a
        let worldMap = Map.map (fun key n -> if List.contains key a then upgradeLandTerrain key else n) world.worldMap

        let newCivs = List.map (fun n -> { n with cities = (Map.map (fun key n -> { n with units = (List.map (fun n -> if n.unitClass = Units.Settlers then fst (newSettler n) else n) n.units) }) n.cities)} ) world.playerList
        let unitsMap = Map.map (fun key (n: UnitPack) -> { n with units = (List.map (fun p -> if p.unitClass = Units.Settlers then fst (newSettler p) else p) n.units) }) world.units

        { 
            world with 
                worldMap = worldMap;
                playerList = newCivs; 
                units = unitsMap;
                currentPlayer = if world.currentPlayer = 0 then 1 else 0
                worldEvents = worldEvents @ world.worldEvents
        }

    let UpdateWorld (world : World) = 
        let updateCiv (world : World) (civ: Civilization) =
            Map.fold (fun acc key n -> updateCity acc n) world civ.cities
        let world = { world with worldEvents = List.empty }
        let a = List.fold (fun acc n -> updateCiv acc n) world world.playerList
        let b = {a with units = Map.map (fun key n -> {n with units = List.map (fun n -> {n with movesMade = 0}) n.units}) a.units}
        let c = updateNonCities b
        c

    let hasRoad (world : World) c r =
        match Map.tryFind (c, r) world.worldMap with
        | Some(c) ->
            match c with
            | River (u) -> u = PlainUpgrades.Road
            | Mountain (u) -> u = MountainUpgrades.Road
            | Hill (u) -> u = HillUpgrades.Road
            | Desert (u) ->  u = PlainUpgrades.Road
            | Forest (u) -> u = BadTerrainUpgrades.Road
            | GrassLand (u) -> u = PlainUpgrades.Road
            | Plain (u) -> u = PlainUpgrades.Road
            | Swamp (u) -> u = BadTerrainUpgrades.Road
            | Snow (u) -> u = BadTerrainUpgrades.Road
            | Tundra (u) -> u = BadTerrainUpgrades.Road
            | _ -> false
        | _ -> false
        