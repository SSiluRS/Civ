namespace GameModel
module World =
    open MapGeneratorFromCS.MapGeneratorFromCS
    open Civilization
    open System
    open Unit
    
    type UnitPack = 
        {
            units : Unit.Unit list
            civilization : Civilization
        }

    type World = 
        {
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
            List.contains unit t.units
        Map.findKey zz world.units

    let moveUnitBitweenPacks world unit c0 r0 c1 r1 =
        let packFrom = world.units.Item (c0,r0)
        let civ = packFrom.civilization
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
                        civilization = civ
                    }
        packFrom, packTo

    let getCivByUnit (world:World) unit =
        let zz key (t:UnitPack) =
            List.contains unit t.units
        let key = Map.findKey zz world.units
        let a = world.units.Item key
        a.civilization

    let attackerWins (world:World) (attacker:Unit.Unit) (defender:Unit.Unit) =
        let defencerCoords = getUnitLoc world defender
        let attackerCoords = getUnitLoc world attacker
       
        let map = Map.add defencerCoords (world.units.Item attackerCoords) world.units
        let map1 = Map.remove attackerCoords map

        {
            world with units = map1                       
        }

    let defenderWins (world:World) (attacker:Unit.Unit) (defender:Unit.Unit) =
        let defencerCoords = getUnitLoc world defender
        let attackerCoords = getUnitLoc world attacker

        let map1 = Map.remove attackerCoords world.units

        {
            world with units = map1                       
        }

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
        match getCellUnits world c r with
        | Some(pack) ->
            if pack.civilization = (getCivByUnit world unit) then
                {
                    world with units = Map.add (c,r) toPack map1
                }
                
            else attackMove world unit (world.units.Item (c,r)).units

        | None -> 
                {
                    world with units = Map.add (c,r) toPack map1                           
                }
                

    let UpdateWorld oldWorld = 
        let newWorld = oldWorld
        newWorld