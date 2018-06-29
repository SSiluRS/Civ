namespace GameModel
open Science 
open Units
open Buildings
module Utils =

    let noRqmnts = List.filter(fun x -> x.requirements = []) allAdvances
    let prA (adv : Advance list) = List.iter (fun (a : Advance) -> printfn "%s" a.name) adv
    let prB bld = List.iter (fun (b : Building) -> printfn "%s" b.name) bld
    let prU (unit : Unit list) = List.iter (fun (u : Unit) -> printfn "%s" u.name) unit
    
    let canLearn requirements learnt =
        List.forall (fun x -> List.contains x learnt) requirements
    
    let notLearntAdvances learnt =
        List.where (fun adv -> (List.contains adv learnt) = false) allAdvances
        

    let allowedAdvances learnt = 
        List.choose (fun x -> if (canLearn x.requirements learnt) then Some(x) else None) (notLearntAdvances learnt)


    let allowedBuildings (learnt : Advance list) =
        let canBuild needs adv = 
            match needs with 
            | Some(a) -> a = adv
            | None -> true
        let canBuild2 needs advList =
            match needs with
            | Some(x) -> List.exists (fun adv -> canBuild needs adv) advList
            | _ -> true
        List.choose (fun building -> if (canBuild2 building.technology learnt) then Some(building) else None) allBuildings

    let allowedUnits (learnt : Advance list) =
        let canGet needs adv = 
            match needs with
            | Some(a) -> a = adv
            | None -> true
        let canGet2 needs advList =
            match needs with
            | Some (x) -> List.exists (fun adv -> canGet needs adv) advList
            | _ -> true
        List.choose (fun unit -> if (canGet2 unit.advance learnt) then Some(unit) else None) allUnits

    let learnt = []
    let built = noRqmnts
    printfn"%s" "Advances:"
    prA (allowedAdvances learnt)
    printfn"%s" ""
    printfn"%s" "Buildings:"
    prB (allowedBuildings learnt)
    printfn"%s" ""
    printfn"%s" "Units:"
    prU (allowedUnits learnt)