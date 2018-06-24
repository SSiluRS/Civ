module WorldMap
    open MapGeneratorFromCS.MapGeneratorFromCS
    open MapGeneratorFromCS

    let mapWidth = 320
    let mapHeight = 160

    let RowColToN row col = row * mapWidth + col
    let NToRowCol n w = (n % w), (n / w)

    let getWorldMapCell x y (worldMap:Map<int,LandTerrain>) =
        worldMap.Item (RowColToN y x)

    let applyFnToMap fn x y w n =
        let c, r = NToRowCol n w
        fn (x + c) (y + r)

    let i2c worldMap i = 
        let x,y = NToRowCol i mapWidth
        getWorldMapCell x y worldMap
    
    let loadWorld file =
        MapGeneratorFromCS.CreateWorldMap file