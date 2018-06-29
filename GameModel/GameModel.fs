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
        let world1 = createUnit world playerList.[0] UnitClass.Catapult VeteranStatus.Regular 160 80
        createUnit world1 playerList.[1] UnitClass.Catapult VeteranStatus.Regular 1 1

    let createCity worldMap c r = 
        { 
            name = "Moscow"; 
            currentlyBuilding = City.CurrentlyBuilding.Unit UnitClass.Settlers;
            production = 0;
            population = 1;
            occupation = List.ofSeq (AssignFarmersToCell c r 4 worldMap);
            food = 0;
            building =[];
            happiness = City.Happiness.Neutral;
            units = []
        }

    let createWorld1 = 
        let worldMap = WorldMap.loadWorld @"map.sav"
        let cities =
            //let a = Seq.init 51000 (fun n -> findCellForCity1 worldMap 160 80)
            //let c = findCellForCity1 worldMap 2 2
            let zz (c,r) = 
                //let c, r = i%mapWidth, i/mapWidth
                let z = createCity worldMap c r
                let a = [((c,r), z)]
                Map.ofList(a)
            //match c with
            zz (19, 10)

        let playerList =
                [{
                    name = "Player";
                    money = 0;
                    discoveries = [Science.Pottery; Science.The_Wheel; Science.Alphabet];
                    taxScience = 100;
                    taxLuxury = 0;
                    cities = cities
                    currentlyDiscovering = Science.Ceremonial_Burial
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
        }        
    
    let createWorld2 = createUnit createWorld1 createWorld1.playerList.[0] UnitClass.Settlers VeteranStatus.Regular 170 80
    let createWorld = createUnit createWorld2 createWorld2.playerList.[0] UnitClass.Settlers VeteranStatus.Regular 170 80