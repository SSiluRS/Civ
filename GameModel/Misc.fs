module Misc
open WorldMap

let iter2d x y w h fn =
    Seq.init(w*h) (fun n -> applyFnToMap fn x y w n)

let find2d x y w h fn =
    Seq.tryFindIndex (applyFnToMap fn x y w) (Seq.init (w*h) (fun n -> n))