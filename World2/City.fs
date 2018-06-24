module City

type Building =
    | Barrack //Город строит ветеранов
    | Temple //Минус 1 недовольный житель
    | Granary // При увеличении города теряется только половина запасов еды
    | Marketplace // Одна единица торговли превращается в 1,5 единицы денег
    | Palace // Нет коррупции
    | Library // 1 единица торговли даёт 1,5 единицы науки
        
type CurrentlyBuilding =
    | Building of Building
    | Unit of Unit
    | Nothing

type Occupation =
    | Farmer of int
    | Scientist
    | TaxCollector
    | Artist

type Happiness =
    | Neutral
    | Happy
    | Unhappy

type City = 
    {
        name : string; 
        currentlyBuilding : CurrentlyBuilding;
        production : int;
        population : int;
        occupation : Occupation list; //номера квадратов вокруг города, которые обрабатывают фермеры
        food : int;
        building : Building list;
        happiness : Happiness;
    }     
