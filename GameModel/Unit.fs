module Unit

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
