namespace GameModel
module Misc =
    open WorldMap

    let NToRowCol x y w n = (x + n%w), (y + n/w)

    let iter2d x y w h fn =
        Seq.init(w*h) (fun n -> fn (fst (NToRowCol x y w n)) (snd (NToRowCol x y w n)))

    let find2d x y w h fn =
        let zz2 = Seq.init(w*h) (NToRowCol x y w)  
        Seq.tryFindIndex fn zz2