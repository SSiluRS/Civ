namespace GameModel
module Misc =
    open WorldMap

    let NToRowCol x y w n = (x + n%w), (y + n/w)

    let iter2d x y w h fn =
        let z n =
            let c, r = NToRowCol x y w n
            fn c r
        Seq.init (w*h) z

    let find2d x y w h fn =
        let zz2 = Seq.init(w*h) (NToRowCol x y w)  
        Seq.tryFindIndex fn zz2