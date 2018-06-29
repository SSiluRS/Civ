namespace GameModel
module Buildings =
    open Science
    type Building = {
        name: string
        technology : Advance option
        cost : int
        maintenance : int
    }

    let Aqueduct = {
        name = "Aqueduct"
        technology = Some(Construction)
        cost = 120
        maintenance = 2
    }
    let Bank = {
        name = "Bank"
        technology = Some(Banking)
        cost = 120
        maintenance = 3
    }
    let Barracks = {
        name = "Barracks"
        technology = None
        cost = 40
        maintenance = 0
    }
    let Cathedral = {
        name = "Cathedral"
        technology = Some(Religion)
        cost = 160
        maintenance = 3
    }
    let City_Walls = {
        name = "City Walls"
        technology = Some(Masonry)
        cost = 120
        maintenance = 2
    }
    let Colosseum = {
        name = "Colosseum"
        technology = Some(Construction)
        cost = 100
        maintenance = 4
    }
    let Courthouse = {
        name = "Courthouse"
        technology = Some(Code_of_Laws)
        cost = 80
        maintenance = 1
    }
    let Factory = {
        name = "Factory"
        technology = Some(Industrialization)
        cost = 200
        maintenance = 4
    }
    let Granary = {
        name = "Granary"
        technology = Some(Pottery)
        cost = 60
        maintenance = 1
    }
    let Hydro_Plant = {
        name = "Hydro Plant"
        technology = Some(Electronics)
        cost = 240
        maintenance = 4
    }
    let Library = {
        name = "Library"
        technology = Some(Writing)
        cost = 80
        maintenance = 1
    }
    let Marketplace = {
        name = "Marketplace"
        technology = Some(Currency)
        cost = 80
        maintenance = 1
    }
    let Mass_Transit = {
        name = "Mass Transit"
        technology = Some(Mass_Production)
        cost = 160
        maintenance = 4
    }
    let Mfg_Plant = {
        name = "Manufacturing Plant"
        technology = Some(Robotics)
        cost = 320
        maintenance = 6
    }
    let Nuclear_Plant = {
        name = "Nuclear Plant"
        technology = Some(Nuclear_Power)
        cost = 160
        maintenance = 2
    }
    let Palace = {
        name = "Palace"
        technology = Some(Masonry)
        cost = 200
        maintenance = 5
    }
    let Power_Plant = {
        name = "Power Plant"
        technology = Some(Refining)
        cost = 160
        maintenance = 4
    }
    let Recycling_Center = {
        name = "Recycling Center"
        technology = Some(Recycling)
        cost = 200
        maintenance = 2
    }
    let SDI_Defence = {
        name = "SDI Defence"
        technology = Some(Superconductor)
        cost = 200
        maintenance = 4
    }
    let Temple = {
        name = "Temple"
        technology = Some(Ceremonial_Burial)
        cost = 40
        maintenance = 1
    }
    let University = {
        name = "University"
        technology = Some(University)
        cost = 160
        maintenance = 3
    }
    let allBuildings = [
        Aqueduct; Bank; Barracks; Cathedral; City_Walls; Colosseum; Courthouse; Factory; Granary; 
        Hydro_Plant; Library; Marketplace; Mass_Transit; Mfg_Plant; Nuclear_Plant; Palace; 
        Power_Plant; Recycling_Center; SDI_Defence; Temple; University;
    ]