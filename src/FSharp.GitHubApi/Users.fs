module FSharp.GitHubApi.Users

    open System
    open Newtonsoft.Json
    open RestHelper
    open JsonHelper

    // -------------------- //
    // Public data types    //
    // -------------------- //
    type Plan = {
        [<field: JsonProperty(PropertyName="name")>] 
        Name : string
        [<field: JsonProperty(PropertyName="space", Required=Required.Default)>]
        Space : int
        [<field: JsonProperty(PropertyName="collaborators", Required=Required.Default)>] 
        Collaborators : int
        [<field: JsonProperty(PropertyName="private_repos", Required=Required.Default)>] 
        PrivateRepos : int   
    }

    type UserSummary = {
        [<field: JsonProperty(PropertyName="login")>]
        Login: string
        [<field: JsonProperty(PropertyName="id")>]
        Id: int
        [<field: JsonProperty(PropertyName="avatar_url")>]
        AvatarUrl: string
        [<field: JsonProperty(PropertyName="gravatar_id")>]
        GravatarId: string
        [<field: JsonProperty(PropertyName="url")>]
        Url: string
    }

    type UserDetails = {
        [<field: JsonProperty(PropertyName="login")>] 
        Login : string
        [<field: JsonProperty(PropertyName="id", Required=Required.Default)>] 
        Id : int
        [<field: JsonProperty(PropertyName="avatar_url")>] 
        AvatarUrl : string
        [<field: JsonProperty(PropertyName="gravatar_id")>] 
        GravatarId : string
        [<field: JsonProperty(PropertyName="url")>] 
        Url : string
        [<field: JsonProperty(PropertyName="name")>] 
        Name : string
        [<field: JsonProperty(PropertyName="company")>] 
        Company : string
        [<field: JsonProperty(PropertyName="blog")>] 
        Blog : string
        [<field: JsonProperty(PropertyName="location")>] 
        Location : string
        [<field: JsonProperty(PropertyName="email")>] 
        Email : string
        [<field: JsonProperty(PropertyName="hireable", Required=Required.AllowNull)>] 
        Hireable : Nullable<bool>
        [<field: JsonProperty(PropertyName="bio")>] 
        Bio : string
        [<field: JsonProperty(PropertyName="public_repos", Required=Required.Default)>] 
        PublicRepos : int
        [<field: JsonProperty(PropertyName="public_gists", Required=Required.Default)>] 
        PublicGists : int
        [<field: JsonProperty(PropertyName="followers", Required=Required.Default)>] 
        Followers : int
        [<field: JsonProperty(PropertyName="following", Required=Required.Default)>] 
        Following : int
        [<field: JsonProperty(PropertyName="html_url")>] 
        HtmlUrl : string
        [<field: JsonProperty(PropertyName="type")>] 
        Type : string
        [<field: JsonProperty(PropertyName="total_private_repos", Required=Required.Default)>] 
        TotalPrivateRepos : int
        [<field: JsonProperty(PropertyName="owned_private_repos", Required=Required.Default)>] 
        OwnedPrivateRepos : int
        [<field: JsonProperty(PropertyName="private_gists", Required=Required.Default)>] 
        PrivateGists : int
        [<field: JsonProperty(PropertyName="disk_usage", Required=Required.Default)>] 
        DiskUsage : int
        [<field: JsonProperty(PropertyName="collaborators", Required=Required.Default)>] 
        Collaborators : int
        [<field: JsonProperty(PropertyName="plan")>] 
        Plan : Plan
    }
        
    type GetParameters = 
        | AuthenticatedUser
        | SpecificUser of string

    type UpdateParams = {
        [<JsonProperty(PropertyName="name", NullValueHandling=NullValueHandling.Ignore)>] 
        Name : string
        [<JsonProperty(PropertyName="email", NullValueHandling=NullValueHandling.Ignore)>] 
        Email : string
        [<JsonProperty(PropertyName="blog", NullValueHandling=NullValueHandling.Ignore)>] 
        Blog : string
        [<JsonProperty(PropertyName="company", NullValueHandling=NullValueHandling.Ignore)>] 
        Company : string
        [<JsonProperty(PropertyName="location", NullValueHandling=NullValueHandling.Ignore)>] 
        Location : string
        [<JsonProperty(PropertyName="hireable", NullValueHandling=NullValueHandling.Ignore)>] 
        Hireable : string
        [<JsonProperty(PropertyName="bio", NullValueHandling=NullValueHandling.Ignore)>] 
        Bio : string
    }

    // -------------------- //
    // Internal functions   //
    // -------------------- //
    let internal defaultUpdateParams = {
        Name        = null
        Email       = null
        Blog        = null
        Company     = null
        Location    = null
        Hireable    = null
        Bio         = null
    }

    let internal getCurrent state = 
        let request = (fun x -> { x with RestResource = "user"})
        let resolve x = 
            RestfulResponse x state
            |> ConvertResponse<UserDetails>
            |> DeserializeResponseContent<UserDetails>
        resolve request            

    let internal getSpecified username state =
        let request = (fun x -> { x with RestResource = (sprintf "users/%s" username)})
        let resolve x = 
            RestfulResponse x state
            |> ConvertResponse<UserDetails>
            |> DeserializeResponseContent<UserDetails>
        resolve request

    let internal Get p state = 
        match p with
        | AuthenticatedUser -> getCurrent state
        | SpecificUser(x) -> getSpecified x state
            
    let internal Update (p:UpdateParams->UpdateParams) state = 
        let json = SerializeToJson (p(defaultUpdateParams))
        let request = (fun x -> { x with Method = PATCH; RestResource = "user"; Content = json })
        let resolve x = 
            RestfulResponse x state
            |> ConvertResponse<UserDetails>
            |> DeserializeResponseContent<UserDetails>
        resolve request

    let internal GetAll since state = 
        let request = (fun x -> { x with RestResource = (sprintf "users?since=%i" since) })
        let resolve x = 
            RestfulResponse x state
            |> ConvertResponse<UserSummary array>
            |> DeserializeResponseContent<UserSummary array>
        resolve request