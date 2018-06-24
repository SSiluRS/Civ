namespace GameModel
open City

module GameModel =
    open World
   
    let createCity worldMap cellIndex = 
        { 
            name = "Moscow"; 
            currentlyBuilding = City.CurrentlyBuilding.Nothing;
            production = 0;
            population = 1;
            occupation = List.ofSeq (AssignFarmersToCell cellIndex 1 worldMap);
            food = 0;
            building =[];
            happiness = City.Happiness.Neutral;
        }

    let createWorld = 
        let worldMap = WorldMap.loadWorld @"map.sav"
        let cities = 
            let c = findCellForCity worldMap
            match c with
            | Some(i) -> Map.ofList([(i, createCity worldMap i)])
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
