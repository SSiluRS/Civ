namespace GameModel
open Science 
open Buildings
module Utils =

    let noRqmnts = List.filter(fun x -> x.requirements = []) allAdvances
    let pr (adv : Advance list) = List.iter (fun (a : Advance) -> printfn "%s" a.name) adv
    let prB bld = List.iter (fun (b : Building) -> printfn "%s" b.name) bld
    
    let canLearn requirements learnt =
        List.forall (fun x -> List.contains x learnt) requirements

    //let canBuild (requirement : Building) (built : Advance list) = 
    //    List.contains requirement built
    
    let notLearntAdvances learnt =
        List.where (fun adv -> (List.contains adv learnt) = false) allAdvances
        

    let allowed learnt = 
        List.choose (fun x -> if (canLearn x.requirements learnt) then Some(x) else None) (notLearntAdvances learnt)


    let allowedBuildingss (learnt : Advance list) =
        let canBuild needs adv = 
            match needs with 
            | Some(a) -> a = adv
            | None -> true
        let canBuild2 needs advList =
            List.exists (fun adv -> canBuild needs adv) advList
        List.choose (fun building -> if (canBuild2 building.technology learnt) then Some(building) else None) []


    let learnt = noRqmnts @ [Currency; Writing; Construction; Code_of_Laws]
    let built = noRqmnts
    //pr (allowed learnt)
    prB (allowedBuildingss learnt)