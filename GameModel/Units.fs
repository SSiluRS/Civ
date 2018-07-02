namespace GameModel    
open Science
module Units =

    [<StructuredFormatDisplay("{name}")>]
    type UnitClass = {
        name: string
        advance : Advance option
        att : int
        def : int
        mov : int
        prod : int
        cost : int
    }

    let Settlers = { name = "Settlers"; advance = None; att = 0; def = 1; mov = 1; prod = 40; cost = 320}
    let Riflemen = { name = "Riflemen"; advance = Some(Conscription); att = 3; def = 5; mov = 1; prod = 30; cost = 210}
    let Phalanx = { name = "Phalanx"; advance = Some(Bronze_Working); att = 1; def = 2; mov = 1; prod = 20; cost = 120}
    let Musketeers = { name = "Musketeers"; advance = Some(Gunpowder); att = 2; def = 3; mov = 1; prod = 30; cost = 210}
    let Militia = { name = "Militia"; advance = None; att = 1; def = 1; mov = 1; prod = 10; cost = 50}
    let Mech_Inf = { name = "Mech. Inf."; advance = Some(Labor_Union); att = 6; def = 6; mov = 3; prod = 50; cost = 450}
    let Legion = { name = "Legion"; advance = Some(Iron_Working); att = 3; def = 1; mov = 1; prod = 20; cost = 120}
    let Knights = { name = "Knights"; advance = Some(Chivalry); att = 4; def = 2; mov = 2; prod = 40; cost = 320}
    let Diplomat = { name = "Diplomat"; advance = Some(Writing); att = 0; def = 0; mov = 2; prod = 30; cost = 210}
    let Chariot = { name = "Chariot"; advance = Some(The_Wheel); att = 4; def = 1; mov = 2; prod = 40; cost = 320}
    let Cavalry = { name = "Cavalry"; advance = Some(Horseback_Riding); att = 2; def = 1; mov = 2; prod = 20; cost = 120}
    let Catapult = { name = "Catapult"; advance = Some(Mathematics); att = 6; def = 1; mov = 1; prod = 40; cost = 320}
    let Caravan = { name = "Caravan"; advance = Some(Trade); att = 0; def = 1; mov = 1; prod = 50; cost = 450}
    let Cannon = { name = "Cannon"; advance = Some(Metallurgy); att = 8; def = 1; mov = 1; prod = 40; cost = 320}
    let Artillery = { name = "Artillery"; advance = Some(Robotics); att = 12; def = 2; mov = 2; prod = 60; cost = 600}
    let Armor = { name = "Armor"; advance = Some(Automobile); att = 10; def = 5; mov = 3; prod = 80; cost = 960}
    let Trireme = { name = "Trireme"; advance = Some(Map_Making); att = 1; def = 0; mov = 3; prod = 40; cost = 320}
    let Transport = { name = "Transport"; advance = Some(Industrialization); att = 0; def = 3; mov = 4; prod = 50; cost = 450}
    let Submarine = { name = "Submarine"; advance = Some(Mass_Production); att = 8; def = 2; mov = 3; prod = 50; cost = 450}
    let Sail = { name = "Sail"; advance = Some(Navigation); att = 1; def = 1; mov = 3; prod = 40; cost = 320}
    let Ironclad = { name = "Ironclad"; advance = Some(Steam_Engine); att = 4; def = 4; mov = 4; prod = 60; cost = 600}
    let Frigate = { name = "Frigate"; advance = Some(Magnetism); att = 2; def = 2; mov = 3; prod = 40; cost = 320}
    let Cruiser = { name = "Cruiser"; advance = Some(Combustion); att = 6; def = 6; mov = 6; prod = 80; cost = 960}
    let Carrier = { name = "Carrier"; advance = Some(Advanced_Flight); att = 1; def = 12; mov = 5; prod = 160; cost = 3200}
    let Battleship = { name = "Battleship"; advance = Some(Steel); att = 18; def = 12; mov = 4; prod = 160; cost = 3200}
    let Bomber = { name = "Bomber"; advance = Some(Advanced_Flight); att = 12; def = 1; mov = 8; prod = 120; cost = 1920}
    let Fighter = { name = "Fighter"; advance = Some(Flight); att = 4; def = 2; mov = 10; prod = 60; cost = 600}
    let Nuclear = { name = "Nuclear"; advance = Some(Rocketry); att = 99; def = 0; mov = 16; prod = 160; cost = 3200}

    let allUnits = [
        Settlers; Riflemen; Phalanx; Musketeers; Militia; Mech_Inf; Legion; Knights; Diplomat; Chariot; Cavalry; Catapult; Caravan; 
        Cannon; Artillery; Armor; Trireme; Transport; Submarine; Sail; Ironclad; Frigate; Cruiser; Carrier; Battleship; Bomber; 
        Fighter; Nuclear; 
    ]