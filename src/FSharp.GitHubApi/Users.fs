module FSharp.GitHubApi.Users

    open FSharp.GitHubApi.ApiHelper
    open System.Runtime.Serialization

    // -------------------- //
    // Public data types    //
    // -------------------- //
    [<DataContract>]
    type Plan = {
        [<field: DataMember(Name="name")>]
        Name: string
        [<field: DataMember(Name="space")>]
        Space: int
        [<field: DataMember(Name="collaborators")>]
        Collaborators: int
        [<field: DataMember(Name="private_repos")>]
        PrivateRepos: int        
    }

    [<DataContract>]
    type UserDetails = {
        [<field: DataMember(Name="login")>]
        Login: string
        [<field: DataMember(Name="id")>]
        Id: int
        [<field: DataMember(Name="avatar_url")>]
        AvatarUrl: string
        [<field: DataMember(Name="gravatar_id")>]
        GravatarId: string
        [<field: DataMember(Name="url")>]
        Url: string
        [<field: DataMember(Name="name")>]
        Name: string
        [<field: DataMember(Name="company")>]
        Company: string
        [<field: DataMember(Name="blog")>]
        Blog: string
        [<field: DataMember(Name="location")>]
        Location: string
        [<field: DataMember(Name="email")>]
        Email: string
        [<field: DataMember(Name="hireable")>]
        Hireable: bool
        [<field: DataMember(Name="bio")>]
        Bio: string
        [<field: DataMember(Name="public_repos")>]
        PublicRepos: int
        [<field: DataMember(Name="public_gists")>]
        PublicGists: int
        [<field: DataMember(Name="followers")>]
        Followers: int
        [<field: DataMember(Name="following")>]
        Following: int
        [<field: DataMember(Name="html_url")>]
        HtmlUrl: string
        [<field: DataMember(Name="type")>]
        Type: string
        [<field: DataMember(Name="total_private_repos")>]
        TotalPrivateRepos: int
        [<field: DataMember(Name="owned_private_repos")>]
        OwnedPrivateRepos: int
        [<field: DataMember(Name="private_gists")>]
        PrivateGists: int
        [<field: DataMember(Name="disk_usage")>]
        DiskUsage: int
        [<field: DataMember(Name="collaborators")>]
        Collaborators: int
        [<field: DataMember(Name="plan")>]
        Plan: Plan
    }
        
    type GetParameters = 
        | AuthenticatedUser
        | SpecificUser of string

    type UpdateParams = {
        Name: string option
        Email: string option
        Blog: string option
        Company: string option
        Location: string option
        Hireable: string option
        Bio: string option
    }

    let DefaultUpdateParams = {
        Name = None
        Email = None
        Blog = None
        Company = None
        Location = None
        Hireable = None
        Bio = None
    }

    // -------------------- //
    // Internal functions   //
    // -------------------- //
    let internal getUserRestResponse request state = 
        match RestHelper.Get request state with
        | RestHelper.Success(json) ->
            let userDetails = json |> JsonHelper.DeserializeJson<UserDetails>
            match userDetails with
            | Some(ud) -> { StatusCode = 200; Content = Some(ud); ErrorMessage = "" }
            | None -> { StatusCode = 200; Content = None; ErrorMessage = "Cannot deserialize User details, returned api default instead" }
        | RestHelper.Failed(reason) -> { StatusCode = 0; Content = None; ErrorMessage = reason }

    let internal getSpecificUser username state = 
        state |> getUserRestResponse { Resource = (sprintf "users/%s" username) }

    let internal getAuthenticatedUser state = 
        state |> getUserRestResponse { Resource = "user" }

    let internal Get p state = 
        match p with
        | AuthenticatedUser -> getAuthenticatedUser state 
        | SpecificUser(u) -> getSpecificUser u state

    let internal Update p state = 
        let json = JsonHelper.SerializeJson p
        let patchResponse = 
            json |> RestHelper.Patch { Resource = "user" } state
        match patchResponse with
        | RestHelper.Success(json) ->
            let userDetails = json |> JsonHelper.DeserializeJson<UserDetails>
            match userDetails with
            | Some(ud) -> { StatusCode = 200; Content = Some(ud); ErrorMessage = "" }
            | None -> { StatusCode = 200; Content = None; ErrorMessage = "Cannot deserialize User details, returned api default instead" }
        | RestHelper.Failed(reason) -> { StatusCode = 0; Content = None; ErrorMessage = reason }