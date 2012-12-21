module RestHelper

    open RestSharp

    type Username = Username of string
    type Password = Password of string
    type Credentials = Username * Password

    type GitHubState = {
        Credentials: Credentials option
    }

    let GitHubApiClient state = 
        let mutable client = new RestClient("https://api.github.com")
        match state.Credentials with
        | Some(Username(u),Password(p)) ->
            printfn "Attempting authenticated connection"
            client.Authenticator <- new HttpBasicAuthenticator(u, p)            
        | _ -> printfn "Anonymous connection"
        client

    type Request = {
        Resource: string
    }

    type Resonse = 
        | Success of string
        | Failed of string

    let Get request state = 
        let client = state |> GitHubApiClient
        let get = new RestRequest(resource=request.Resource)
        let response = client.Execute(request=get)
        match response.ResponseStatus with
        | Completed -> Success(response.Content)
        | Error -> Failed((sprintf "Error: '%s'" response.ErrorMessage))
        | Aborted -> Failed("Aborted")
        | TimedOut -> Failed("Timeout")