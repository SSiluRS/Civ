namespace GameModel
module Buildings =
    open Science
    type Building = {
        name: string
        technology : Advance option
        cost : int
    }

    //let Aqueduct = {
    //    name = "Aqueduct"
    //    technology = Some(Construction)
    //}
    //let Bank = {
    //    name = "Bank"
    //    technology = Some(Banking)
    //}
    let Barracks = {
        name = "Barracks"
        technology = None
        cost = 40
    }
    //let Cathedral = {
    //    name = "Cathedral"
    //    technology = Some(Religion)
    //}
    //let City_Walls = {
    //    name = "City Walls"
    //    technology = Some(Masonry)
    //}
    //let Colosseum = {
    //    name = "Colosseum"
    //    technology = Some(Construction)
    //}
    //let Courthouse = {
    //    name = "Courthouse"
    //    technology = Some(Code_of_Laws)
    //}
    //let Factory = {
    //    name = "Factory"
    //    technology = Some(Industrialization)
    //}
    let Granary = {
        name = "Granary"
        technology = Some(Pottery)
        cost = 60
    }
    //let Hydro_Plant = {
    //    name = "Hydro Plant"
    //    technology = Some(Electronics)
    //}
    let Library = {
        name = "Library"
        technology = Some(Writing)
        cost = 80
    }
    let Marketplace = {
        name = "Marketplace"
        technology = Some(Currency)
        cost = 80
    }
    //let Mass_Transit = {
    //    name = "Mass Transit"
    //    technology = Some(Mass_Production)
    //}
    //let Mfg_Plant = {
    //    name = "Manufacturing Plant"
    //    technology = Some(Robotics)
    //}
    //let Nuclear_Plant = {
    //    name = "Nuclear Plant"
    //    technology = Some(Nuclear_Power)
    //}
    let Palace = {
        name = "Palace"
        technology = Some(Masonry)
        cost = 120
    }
    //let Power_Plant = {
    //    name = "Power Plant"
    //    technology = Some(Refining)
    //}
    //let Recycling_Center = {
    //    name = "Recycling Center"
    //    technology = Some(Recycling)
    //}
    //let SDI_Defence = {
    //    name = "SDI Defence"
    //    technology = Some(Superconductor)
    //}
    let temple = {
        name = "temple"
        technology = Some(Ceremonial_Burial)
        cost = 40
    }
    //let University = {
    //    name = "University"
    //    technology = Some(University)
    //}
    //let allBuildings = [
    //    Aqueduct; Bank; Barracks; Cathedral; City_Walls; Colosseum; Courthouse; Factory; Granary; 
    //    Hydro_Plant; Library; Marketplace; Mass_Transit; Mfg_Plant; Nuclear_Plant; Palace; 
    //    Power_Plant; Recycling_Center; SDI_Defence; Temple; University;
    //]