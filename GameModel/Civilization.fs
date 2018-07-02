namespace GameModel
module Civilization =
    open MapGeneratorFromCS.MapGeneratorFromCS

    type Civilization = 
        {
            name : string; 
            money : int; 
            discoveries : Science.Advance list; 
            taxScience : int; 
            taxLuxury : int;
            cities : Map<int*int,City.City>;
            currentlyDiscovering : Science.Advance;
            researchProgress : int;
            unitIDs : int list
            fogOfWar : Map<int*int, bool>
        }