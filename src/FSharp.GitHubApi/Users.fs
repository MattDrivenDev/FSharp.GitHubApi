module Users

    open GitHub
    open System.Runtime.Serialization

    [<DataContract>]
    type UserDetails = {
        [<field: DataMember(Name="login")>]
        Login: string
        [<field: DataMember(Name="name")>]
        Name: string
        [<field: DataMember(Name="blog")>]
        Blog: string
    }

    let internal defaultUserDetails = {
        Login = "octocat"
        Name = "monalisa octocat"
        Blog = "https://github.com/blog"
    }

    type GetParameters = 
        | AuthenticatedUser
        | SpecificUser of string

    let internal getSpecificUser username state = 
        match RestHelper.Get { Resource = (sprintf "users/%s" username) } state with
        | RestHelper.Success(json) ->
            let userDetails = json |> JsonHelper.DeserializeJson<UserDetails>
            match userDetails with
            | Some(ud) -> { StatusCode = 200; Content = ud; ErrorMessage = "" }
            | None -> { StatusCode = 200; Content = defaultUserDetails; ErrorMessage = "Cannot deserialize User details, returned api default instead" }
        | RestHelper.Failed(reason) -> { StatusCode = 0; Content = defaultUserDetails; ErrorMessage = reason }

    let Get query state = 
        match query with
        | AuthenticatedUser ->
            { 
                StatusCode = 0
                ErrorMessage = "Not implemented yet"
                Content = { Login = ""; Name = ""; Blog = ""; }
            }   
        | SpecificUser(u) ->
            getSpecificUser u state