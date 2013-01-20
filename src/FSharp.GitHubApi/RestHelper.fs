module FSharp.GitHubApi.RestHelper

    open RestSharp

    // -------------------- //
    // Public data types    //
    // -------------------- //
    type Username = Username of string
    type Password = Password of string
    type Credentials = Username * Password

    type GitHubState = {
        Credentials: Credentials option
    }

    // --------------------- //
    // Internal data types   //
    // --------------------- //
    type internal RestMethod = 
        | GET
        | POST
        | PATCH
        | DELETE

    type internal Request = {
        RestResource: string
        Method: RestMethod
        Content: string
    }

    // -------------------- //
    // Internal functions   //
    // -------------------- //
    let internal request = {
        RestResource = ""
        Method = GET
        Content = ""
    }

    let internal GitHubApiClient state = 
        let mutable client = new RestClient("https://api.github.com")
        match state.Credentials with
        | Some(Username(u),Password(p)) ->
            printfn "Attempting authenticated connection"
            client.Authenticator <- new HttpBasicAuthenticator(u, p)            
        | _ -> printfn "Anonymous connection"
        client

    let internal Get request state = 
        let client = state |> GitHubApiClient
        let get = new RestRequest(resource=request.RestResource)
        client.Execute(request=get)

    let internal Patch request state json =
        let client = state |> GitHubApiClient
        let mutable patch = new RestRequest(request.RestResource, Method.PATCH)
        patch.RequestFormat <- DataFormat.Json
        patch.AddParameter(@"application\json", json, ParameterType.RequestBody) |> ignore        
        client.Execute(request=patch)

    type internal RestClient() =
        member this.Bind(x) = x
        member this.Delay(f) = f()
        member this.Return((x:Request->Request),s) = 
            let r = request |> x 
            match r.Method with
            | PATCH -> Patch r s r.Content
            | _ -> Get r s
                
    let internal restfulResponse = new RestClient()