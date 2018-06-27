namespace GameModel
open City

module GameModel =
    open World
    open System
    open Unit
    open WorldMap
    open GameModel
    open Civilization

    let createUnits n =
        let rnd = new Random()
        Map.ofSeq (Seq.init n (fun n -> ((rnd.Next(320), rnd.Next(160)), {unitClass = Unit.Catapult; veteran = VeteranStatus.Regular; movesMade = 0; ID = n})))

    let createUnit (world:World) (civ:Civilization) (unitClass:UnitClass) (veteran:VeteranStatus) c r =
        let existingUnitPack = (world.units.TryFind (c,r))
        let existingUnits = 
            match existingUnitPack with
            | Some (unitPack) -> unitPack.units
            | None -> List.empty
        let unit = 
            {
                unitClass = unitClass
                veteran = veteran
                movesMade = 0
                ID = world.unitsCount
            }

        let unitPack = 
            {
                units = unit :: existingUnits
                civilization = civ
            }

        let units = Map.add (c,r) unitPack world.units
        
        {
            world with
                unitsCount = world.unitsCount+1
                units = units
        }

    let demoUnit (world:World) (playerList : Civilization list) =
        //Map.ofSeq (Seq.init 2 (fun n -> ((n,n), {units = [{unitClass = UnitClass.Catapult; veteran = VeteranStatus.Regular; movesMade = 0; ID = n}]; civilization = playerList.[n]})))
        let world1 = createUnit world playerList.[0] UnitClass.Catapult VeteranStatus.Regular 0 0
        createUnit world1 playerList.[1] UnitClass.Catapult VeteranStatus.Regular 1 1

    let createCity worldMap c r = 
        { 
            name = "Moscow"; 
            currentlyBuilding = City.CurrentlyBuilding.Nothing;
            production = 0;
            population = 1;
            occupation = List.ofSeq (AssignFarmersToCell c r 4 worldMap);
            food = 0;
            building =[];
            happiness = City.Happiness.Neutral;
            units = []
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
            unitsCount = 0;
            rnd = new Random();
            worldMap = worldMap;
            playerList = playerList;
            units = Map.empty
        }        
