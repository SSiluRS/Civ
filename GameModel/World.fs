module World
open MapGeneratorFromCS.MapGeneratorFromCS
open Civilization

type World = 
    {
        worldMap: Map<int, LandTerrain>; 
        playerList : Civilization list;
    }

let UpdateWorld oldWorld = 
    let newWorld = oldWorld
    newWorld