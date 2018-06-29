namespace GameModel
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
 
    [<StructuredFormatDisplay("{unitClass}")>]
    type Unit = 
        {
            unitClass : UnitClass; 
            veteran : VeteranStatus; 
            movesMade : int //Количество сделанных ходов, умноженых на 9 для обеспечения дробных ходов, например, по дороге
            ID : int
        }

    let getUnitMovement unitClass =
        match unitClass with
        | UnitClass.Settlers ->  10
        | UnitClass.Militia ->   1
        | UnitClass.Phalanx ->   1
        | UnitClass.Chivalery -> 2
        | UnitClass.Legion ->    1
        | UnitClass.Chariot ->   2
        | UnitClass.Catapult ->  1
        | UnitClass.Knight ->    2

    let getUnitAttack unitClass =
        match unitClass with
        | UnitClass.Settlers ->  0
        | UnitClass.Militia ->   1
        | UnitClass.Phalanx ->   1
        | UnitClass.Chivalery -> 2
        | UnitClass.Legion ->    3
        | UnitClass.Chariot ->   4
        | UnitClass.Catapult ->  6
        | UnitClass.Knight ->    4

    let getUnitDefence unitClass =
        match unitClass with
        | UnitClass.Settlers ->  1
        | UnitClass.Militia ->   1
        | UnitClass.Phalanx ->   2
        | UnitClass.Chivalery -> 1
        | UnitClass.Legion ->    1
        | UnitClass.Chariot ->   1
        | UnitClass.Catapult ->  1
        | UnitClass.Knight ->    2

    let promoteUnit unit = 
        { unit with veteran = Veteran}