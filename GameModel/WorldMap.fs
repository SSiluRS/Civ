namespace GameModel
module WorldMap =
    open MapGeneratorFromCS.MapGeneratorFromCS
    open MapGeneratorFromCS

    let mapWidth = 320
    let mapHeight = 160

    let getWorldMapCell (worldMap:Map<int*int,LandTerrain>) c r =
        worldMap.Item (c, r)
    
    let loadWorld file =
        MapGeneratorFromCS.CreateWorldMap file