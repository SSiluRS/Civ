module Civilization

type Civilization = 
    {
        name : string; 
        money : int; 
        discoveries : Science.Discovery list; 
        taxScience : int; 
        taxLuxury : int;
        cities : Map<int*int,City.City>;
        currentlyDiscovering : Science.Discovery;
        researchProgress : int;
        units : Map<int*int, Unit>;
    }
