namespace GameModel
module World =
    open MapGeneratorFromCS.MapGeneratorFromCS
    open Civilization
    open System
    open Unit
    open City
    
    type UnitPack = 
        {
            units : Unit.Unit list
        }

    type World = 
        {
            unitsCount : int;
            rnd: Random;
            worldMap: Map<int*int, LandTerrain>; 
            playerList : Civilization list;
            units : Map<int*int, UnitPack>;
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

    let attackerWins (world:World) (attacker:Unit.Unit) (defender:Unit.Unit) =
        let defencerCoords = getUnitLoc world defender
        let attackerCoords = getUnitLoc world attacker
       
        let map = Map.add defencerCoords {(world.units.Item attackerCoords) with units = [{attacker with movesMade = attacker.movesMade + 1; veteran = VeteranStatus.Veteran}]} world.units
        let map1 = Map.remove attackerCoords map

        let civ = getCivByUnit world defender
        let newCiv = {civ with unitIDs = List.where (fun n -> n <> defender.ID) civ.unitIDs}
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
        let fromPack,toPack = moveUnitBitweenPacks world unit c0 r0 c r 
        let map1 = 
            if List.length fromPack.units <> 0 
            then (Map.add (c0,r0) fromPack world.units) 
            else (Map.remove (c0,r0) world.units)
        let newWorld = ({ world with units = Map.add (c,r) {toPack with units = {unit with movesMade = unit.movesMade + 1} :: (List.where (fun n -> n <> unit) toPack.units)} map1 }, Some({unit with movesMade = unit.movesMade + 1}))
        match getCellUnits world c r with
        | Some(pack) ->
            if (world.worldMap.Item (c,r) = LandTerrain.Ocean) || (unit.movesMade >= getUnitMovement unit.unitClass) then (world, Some(unit))
            else if List.length pack.units >= 1 && (getCivByUnit world pack.units.[0]) = (getCivByUnit world unit) || List.length pack.units = 0 then newWorld 
            else attackMove world unit (world.units.Item (c,r)).units

        | None -> 
            if (world.worldMap.Item (c,r) = LandTerrain.Ocean) || (unit.movesMade >= getUnitMovement unit.unitClass) then (world, Some(unit))
            else newWorld            

    let unitMakesCity (world:World) (unit:Unit) =
        let civ = getCivByUnit world unit
        let key = List.findIndex (fun n -> n = civ) world.playerList
        let c,r = getUnitLoc world unit
        let city = 
            { 
                name = System.DateTime.Now.ToShortTimeString();
                currentlyBuilding = City.CurrentlyBuilding.Nothing;
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
        {  world with playerList = newCivs; units = newUnits }
         

    let updateCity (world : World) (city : City) = 
        let onlyFarmers n =
            match n with
            | Farmer _ -> true
            | _ -> false
        let farmers = List.where onlyFarmers city.occupation

        let getYield fn farmerList happiness =
            let production farmer = 
                match farmer with
                | Farmer (c,r) -> fn (WorldMap.getWorldMapCell world.worldMap c r) happiness
                | _ -> 0
            List.fold (fun acc n -> acc + (production n)) 0 farmerList
            
        let shields = getYield GetShieldsCount farmers Happiness.Neutral
        let trade = getYield GetTradeCount farmers Happiness.Neutral
        let food = getYield GetFoodCount farmers Happiness.Neutral

        city

    let UpdateWorld (world : World) = 
        let a = List.map (fun n -> {n with cities = (Map.map (fun key n -> updateCity world n) n.cities)}) world.playerList
        { world with playerList = a }
