module World
open MapGeneratorFromCS.MapGeneratorFromCS
open Civilization

type Resource = 
    | Shield //производство
    | Trade //наука либо деньги
    | Food //увеличение жителей

type World = 
    {
        worldMap: Map<int, LandTerrain>; 
        playerList : Civilization list;
    }