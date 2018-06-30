namespace GameModel
open City

module GameModel =
    open World
    open System
    open Units
    open GameModel
    open Civilization
    open Unit

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
                roadInfo = (false, 0),(0,0)
            }

        let unitPack = 
            {
                units = unit :: existingUnits
            }

        let units = Map.add (c,r) unitPack world.units
        
        let newCiv = {civ with unitIDs = world.unitsCount :: civ.unitIDs}
        let newCivs = List.map (fun n -> if n = civ then newCiv else n) world.playerList

        {
            world with
                unitsCount = world.unitsCount+1
                units = units
                playerList = newCivs
        }

    let demoUnit (world:World) (playerList : Civilization list) =
        //Map.ofSeq (Seq.init 2 (fun n -> ((n,n), {units = [{unitClass = UnitClass.Catapult; veteran = VeteranStatus.Regular; movesMade = 0; ID = n}]; civilization = playerList.[n]})))
        let world1 = createUnit world playerList.[0] Units.Settlers VeteranStatus.Regular 160 80
        createUnit world1 playerList.[1] Units.Settlers VeteranStatus.Regular 1 1

    let createCity worldMap c r = 
        { 
            name = "Moscow"; 
            currentlyBuilding = City.CurrentlyBuilding.Unit Units.Settlers;
            production = 0;
            population = 1;
            occupation = List.ofSeq (AssignFarmersToCell c r 4 worldMap);
            food = 0;
            building =[Buildings.Barracks];
            happiness = City.Happiness.Neutral;
            units = []
        }

    let createWorld1 = 
        let worldMap = WorldMap.loadWorld @"map.sav"
        let cities =
            //let a = Seq.init 51000 (fun n -> findCellForCity1 worldMap 160 80)
            let c = findCellForCity1 worldMap 170 80
            let zz (c,r) = 
                //let c, r = i%mapWidth, i/mapWidth
                let z = createCity worldMap c r
                let a = [((c,r), z)]
                Map.ofList(a)
            match c with
            | Some(c,r) -> zz (c,r)
            | None -> Map.empty

        let playerList =
                [{
                    name = "Player";
                    money = 0;
                    discoveries = [];
                    taxScience = 100;
                    taxLuxury = 0;
                    cities = cities
                    currentlyDiscovering = Science.Alphabet
                    researchProgress = 0;
                    unitIDs = []
                };
                {
                    name = "Player2";
                    money = 0;
                    discoveries = [];
                    taxScience = 50;
                    taxLuxury = 50;
                    cities = Map.empty
                    currentlyDiscovering = Science.Alphabet;
                    researchProgress = 0;
                    unitIDs = []
                }]

        {
            unitsCount = 0;
            rnd = new Random();
            worldMap = worldMap;
            playerList = playerList;
            units = Map.empty
            roads = []
        }        
    
    let createWorld = createUnit createWorld1 createWorld1.playerList.[0] Units.Settlers VeteranStatus.Regular 170 80