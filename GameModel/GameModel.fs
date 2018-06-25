namespace GameModel
open City

module GameModel =
    open World
    open System
    open Unit
    open WorldMap

    let createUnits n =
        let rnd = new Random()
        Map.ofSeq (Seq.init n (fun n -> ((rnd.Next(320), rnd.Next(160)), {unitClass = Unit.Catapult; veteran = VeteranStatus.Regular})))

    let createCity worldMap c r = 
        { 
            name = "Moscow"; 
            currentlyBuilding = City.CurrentlyBuilding.Nothing;
            production = 0;
            population = 1;
            occupation = List.ofSeq (AssignFarmersToCell c r 1 worldMap);
            food = 0;
            building =[];
            happiness = City.Happiness.Neutral;
        }

    let createWorld = 
        let worldMap = WorldMap.loadWorld @"map.sav"
        let cities = 
            let c = findCellForCity worldMap
            let zz i = 
                let c, r = i%mapWidth, i/mapWidth
                let z = createCity worldMap c r
                let a = [((c,r), z)]
                Map.ofList(a)
            match c with
            | Some(i) -> zz i
            | None -> Map.empty
        {
            worldMap = worldMap;
            playerList = 
                [{
                    name = "Player";
                    money = 0;
                    discoveries = [];
                    taxScience = 50;
                    taxLuxury = 50;
                    cities = cities
                    currentlyDiscovering = Science.Discovery.Nothing;
                    researchProgress = 0;
                    units = Map.empty;
                }]
        }        
