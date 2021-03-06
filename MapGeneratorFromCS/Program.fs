﻿namespace MapGeneratorFromCS
module MapGeneratorFromCS =
    open MapGenerator

    type MountainUpgrades =
        | Road
        | Mine
        | Nothing

    type HillUpgrades =
        | Road
        | Irrigation
        | Mine
        | Nothing

    type PlainUpgrades =
        | Irrigation
        | Road
        | Nothing

    type BadTerrainUpgrades =
        | Road
        | Nothing

    type LandTerrain =
        | River of PlainUpgrades
        | Mountain of MountainUpgrades
        | Hill of HillUpgrades
        | Desert of PlainUpgrades
        | Forest of BadTerrainUpgrades
        | GrassLand of PlainUpgrades
        | Plain of PlainUpgrades
        | Swamp of BadTerrainUpgrades
        | Snow of BadTerrainUpgrades
        | Tundra of BadTerrainUpgrades
        | Ocean
        | Unknown

    let CreateWorldMap fileName = 
        let mg = new MapGenerator()
        //let worldMap = mg.GenerateMap1();
        let worldMap = mg.LoadMapFromFile(fileName)

        let width = Collections.Array2D.length1 worldMap
        let height = Collections.Array2D.length2 worldMap

        let GetItem n =
            let c,r = n%width,n/width
            let cell = worldMap.[c, r]
            let convCell =
                match cell with
                | Biomes.Ocean -> Ocean
                | Biomes.Desert -> Desert PlainUpgrades.Nothing
                | Biomes.Forest -> Forest BadTerrainUpgrades.Nothing
                | Biomes.Mountains -> Mountain MountainUpgrades.Nothing
                | Biomes.River -> River PlainUpgrades.Nothing
                | Biomes.Snow -> Snow BadTerrainUpgrades.Nothing
                | Biomes.Tundra -> Tundra BadTerrainUpgrades.Nothing
                | Biomes.Grass -> GrassLand PlainUpgrades.Nothing
                | _ -> Unknown
            ((c,r), convCell)
        let zz = Seq.init (width*height) GetItem
        Map.ofSeq zz

    [<EntryPoint>]
    let main argv = 
        //printfn "%A" CreateWorldMap
        0 // return an integer exit code
