namespace GameModel
module Unit =
    open Units
    open MapGeneratorFromCS.MapGeneratorFromCS

    type VeteranStatus =
        | Regular 
        | Veteran 

    (*type UnitClass =
        | Settlers
        | Militia
        | Phalanx
        | Chivalery 
        | Legion
        | Chariot 
        | Catapult
        | Knight *)
 
    [<StructuredFormatDisplay("{unitClass}")>]
    type Unit = 
        {
            unitClass : UnitClass; 
            veteran : VeteranStatus; 
            movesMade : int //Количество сделанных ходов, умноженых на 9 для обеспечения дробных ходов, например, по дороге
            ID : int
            roadInfo : (bool * int) * (int * int)
        }

    let getUnitMovement unitClass =
        unitClass.mov*9

    let getUnitAttack unitClass =
        unitClass.att

    let getUnitDefence unitClass =
        unitClass.def

    let getMovementCost (landTerrain : LandTerrain) (unit : Unit) =
        match landTerrain with
        | (LandTerrain.Desert PlainUpgrades.Road) -> 3
        | (LandTerrain.Desert _) -> 9
        | (LandTerrain.Forest BadTerrainUpgrades.Nothing) -> 9
        | (LandTerrain.Forest BadTerrainUpgrades.Road) -> 3
        | (LandTerrain.Mountain MountainUpgrades.Road) -> 3
        | (LandTerrain.Mountain _) -> if unit.movesMade < 1 then 1 else getUnitMovement unit.unitClass - unit.movesMade
        | (LandTerrain.River PlainUpgrades.Road) -> 3
        | (LandTerrain.River _) -> 9
        | (LandTerrain.Snow BadTerrainUpgrades.Nothing) -> 9
        | (LandTerrain.Snow BadTerrainUpgrades.Road) -> 3
        | (LandTerrain.Tundra BadTerrainUpgrades.Nothing) -> 9
        | (LandTerrain.Tundra BadTerrainUpgrades.Road) -> 3
        | (LandTerrain.GrassLand PlainUpgrades.Road) -> 3
        | (LandTerrain.GrassLand _) -> 9
        | _ -> 0

    let promoteUnit unit = 
        { unit with veteran = Veteran}