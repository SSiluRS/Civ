﻿namespace SaveNLoad

module SaveNLoad =
    open XmlToolkit
    open GameModel
    open City
    open World
    open Microsoft.FSharp.Reflection

    let GetUnionCaseName (x:'a) = 
        match FSharpValue.GetUnionFields(x, typeof<'a>) with
        | case, _ -> case.Name  

    let saveUnit (u : Unit.Unit) =
        XElement (GetUnionCaseName u.unitClass) [
            XElement "ID" [u.ID]
            XElement "VeteranStatus" [u.veteran] 
            XElement "madeMoves" [u.movesMade]
        ]

    let saveWorldUnits (units : Map<int * int, UnitPack>) = 
        let saveUnitPack ((c, r), (unitPack : UnitPack)) =
            XElement "UnitPack"  [
                XElement "c" [c]
                XElement "r" [r]
                XElement "Units" (List.map saveUnit unitPack.units)
            ]
        Seq.map saveUnitPack (Map.toSeq units)

    let saveUnits (units : Unit.Unit list) = 
        List.map saveUnit units

    let saveIDS unitIDS = 
        List.map (fun id -> XElement "id" [id]) unitIDS

    let saveOccupation (occupants : City.Occupation list) =
        let saveOccupant (o : City.Occupation) =
            match o with
            | Farmer (c, r) -> XElement "Farmer" [XElement "c" [c]; XElement "r" [r]]
            | Scientist -> XElement "Scientist" []
            | TaxCollector -> XElement "TaxCollector" []
            | Artist -> XElement "Artist" []
            
        List.map saveOccupant occupants

    let saveCivDiscoveries (discoveries : Science.Advance list) = 
        List.map (fun (discovery:Science.Advance) -> XElement "discovery" [XElement "name" [discovery.name]]) discoveries

    let saveCivCities (cities : Map<int*int, City.City>) =
        let mapToSeq = Map.toSeq cities
        let z ((c, r), (city:City.City)) =
            XElement "city" [
                XElement "c" [c]
                XElement "r" [r]
                XElement "name" [city.name]
                XElement "currentlyBuilding" [city.currentlyBuilding]
                XElement "production" [city.production]
                XElement "population" [city.population]
                XElement "occupation" (saveOccupation city.occupation)
                XElement "food" [city.food]
                XElement "building" [city.building]
                XElement "happiness" [city.happiness]
                XElement "units" (saveUnits city.units)
            ]
        Seq.map z mapToSeq

    let saveCivilization (civ : Civilization.Civilization) = 
        XElement "civilization" [
            XElement "name" [civ.name]
            XElement "money" [civ.money]
            XElement "discoveries" (saveCivDiscoveries civ.discoveries)
            XElement "taxScience" [civ.taxScience]
            XElement "taxLuxury" [civ.taxLuxury]
            XElement "cities" (saveCivCities civ.cities)
            XElement "currentlyDiscovering" (saveCivDiscoveries [civ.currentlyDiscovering])
            XElement "researchProgress" [civ.researchProgress]
            XElement "unitIDs" (saveIDS civ.unitIDs)
        ]

    let saveWorld world = 
        let doc =
            XDocument (XDeclaration "1.0" "UTF-8" "yes") [
                XElement "World" [
                    XElement "unitsCount" [world.unitsCount]
                    XElement "civilizations" (List.map saveCivilization world.playerList)
                    XElement "units" (saveWorldUnits world.units)
                ]
            ]
        doc