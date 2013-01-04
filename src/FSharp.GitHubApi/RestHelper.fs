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
    type internal Request = {
        Resource: string
    }

    type internal Response = 
        | Success of string
        | Failed of string

    // -------------------- //
    // Internal functions   //
    // -------------------- //
    let internal GitHubApiClient state = 
        let mutable client = new RestClient("https://api.github.com")
        match state.Credentials with
        | Some(Username(u),Password(p)) ->
            printfn "Attempting authenticated connection"
            client.Authenticator <- new HttpBasicAuthenticator(u, p)            
        | _ -> printfn "Anonymous connection"
        client

    let internal handleRestResponse (r:IRestResponse) = 
        match r.ResponseStatus with
        | Completed -> Success(r.Content)
        | Error -> Failed((sprintf "Error: '%s'" r.ErrorMessage))
        | Aborted -> Failed("Aborted")
        | TimedOut -> Failed("Timeout")

    let internal Get request state = 
        let client = state |> GitHubApiClient
        let get = new RestRequest(resource=request.Resource)
        client.Execute(request=get) |> handleRestResponse

    let internal Patch request state json =
        let client = state |> GitHubApiClient
        let mutable patch = new RestRequest(request.Resource, Method.PATCH)
        patch.AddParameter("RequestBody", json)
        client.Execute(request=patch) |> handleRestResponse