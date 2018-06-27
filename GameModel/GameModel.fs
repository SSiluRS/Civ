namespace GameModel
open City

module GameModel =
    open World
    open System
    open Unit
    open WorldMap
    open System.Collections.Generic
    open GameModel
    open Civilization

    let createUnits n =
        let rnd = new Random()
        Map.ofSeq (Seq.init n (fun n -> ((rnd.Next(320), rnd.Next(160)), {unitClass = Unit.Catapult; veteran = VeteranStatus.Regular; movesMade = 0; ID = n})))

    let demoUnit (playerList : Civilization list) =
        Map.ofSeq (Seq.init 2 (fun n -> ((n,n), {units = [{unitClass = UnitClass.Catapult; veteran = VeteranStatus.Regular; movesMade = 0; ID = n}]; civilization = playerList.[n]})))

    let createCity worldMap c r = 
        { 
            name = "Moscow"; 
            currentlyBuilding = City.CurrentlyBuilding.Nothing;
            production = 0;
            population = 3;
            occupation = List.ofSeq (AssignFarmersToCell c r 3 worldMap);
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

        let playerList =
                [{
                    name = "Player";
                    money = 0;
                    discoveries = [];
                    taxScience = 50;
                    taxLuxury = 50;
                    cities = cities
                    currentlyDiscovering = Science.Discovery.Nothing;
                    researchProgress = 0;
                };
                {
                    name = "Player2";
                    money = 0;
                    discoveries = [];
                    taxScience = 50;
                    taxLuxury = 50;
                    cities = cities
                    currentlyDiscovering = Science.Discovery.Nothing;
                    researchProgress = 0;
                }]

        {
            rnd = new Random();
            worldMap = worldMap;
            playerList = playerList
            units = demoUnit playerList
        }        
