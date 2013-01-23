module FSharp.GitHubApi.RestFSharp

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

    let internal Post request state json =
        let client = state |> GitHubApiClient
        let mutable post = new RestRequest(request.RestResource, Method.POST)
        post.RequestFormat <- DataFormat.Json
        post.AddParameter(@"application\json", json, ParameterType.RequestBody) |> ignore        
        client.Execute(request=post)

    let internal Delete request state = 
        let client = state |> GitHubApiClient
        let delete = new RestRequest(request.RestResource, Method.DELETE)
        client.Execute(request=delete)

    let internal RestfulResponse (x:Request->Request) s = 
        let r = x(request)
        match r.Method with
        | PATCH -> Patch r s r.Content
        | POST -> Post r s r.Content
        | DELETE -> Delete r s
        | _ -> Get r s