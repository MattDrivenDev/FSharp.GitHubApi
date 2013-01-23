module TestHelper

    open FSharp.GitHubApi
    open FSharp.GitHubApi.RestFSharp

    type User = {
        Username: string option
        Password: string option
    }

    let private toCredentials user = 
        match (user.Username, user.Password) with
        | Some(u),Some(p) -> Some(Username(u),Password(p))
        | Some(u),None ->
            printfn "No password supplied"
            None
        | None,Some(p) ->
            printfn "No username supplied"
            None
        | None,None -> None

    let private defaultState = {
        Credentials = None
    }

    let AnonymousUser = {
        Username = None
        Password = None
    }

    let AuthenticatedUser = {
        Username = Some(TestSettings.GitHubUsername)
        Password = Some(TestSettings.GitHubPassword)
    }

    let DefaultState user =
        { defaultState with
            Credentials = user |> toCredentials }