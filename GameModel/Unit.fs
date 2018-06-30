namespace GameModel
module Unit =
    open Units

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
 
    type Unit = 
        {
            unitClass : UnitClass; 
            veteran : VeteranStatus; 
            movesMade : int //Количество сделанных ходов, умноженых на 9 для обеспечения дробных ходов, например, по дороге
            ID : int
            roadInfo : (bool * int) * (int * int)
        }

    let getUnitMovement unitClass =
        unitClass.mov

    let getUnitAttack unitClass =
        unitClass.att

    let getUnitDefence unitClass =
        unitClass.def

    let promoteUnit unit = 
        { unit with veteran = Veteran}