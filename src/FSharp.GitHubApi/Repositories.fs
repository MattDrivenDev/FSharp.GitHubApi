module FSharp.GitHubApi.Repositories

    open System
    open Newtonsoft.Json
    open RestHelper
    open JsonHelper

    // -------------------- //
    // Public data types    //
    // -------------------- //
    type Owner = {
        [<JsonProperty(PropertyName="login")>]
        Login : string
        [<JsonProperty(PropertyName="id")>]
        Id : int
        [<JsonProperty(PropertyName="avatar_url")>]
        AvatarUrl : string
        [<JsonProperty(PropertyName="gravatar_id")>]
        GravatarId : string
        [<JsonProperty(PropertyName="url")>]
        Url : string
    }

    type Repository = {
        [<JsonProperty(PropertyName="id")>]
        Id : int        
        [<JsonProperty(PropertyName="owner")>]
        Owner : Owner
        [<JsonProperty(PropertyName="name")>]
        Name : string
        [<JsonProperty(PropertyName="full_name")>]
        FullName : string
        [<JsonProperty(PropertyName="description")>]
        Description : string
        [<JsonProperty(PropertyName="private")>]
        Private : bool
        [<JsonProperty(PropertyName="fork")>]
        Fork : bool
        [<JsonProperty(PropertyName="url")>]
        Url : string
        [<JsonProperty(PropertyName="html_url")>]
        HtmlUrl : string
        [<JsonProperty(PropertyName="clone_url")>]
        CloneUrl : string
        [<JsonProperty(PropertyName="git_url")>]
        GitUrl : string
        [<JsonProperty(PropertyName="ssh_url")>]
        SshUrl : string
        [<JsonProperty(PropertyName="svn_url")>]
        SvnUrl : string
        [<JsonProperty(PropertyName="mirror_url")>]
        MirrorUrl : string
        [<JsonProperty(PropertyName="homepage")>]
        Homepage : string
        [<JsonProperty(PropertyName="language")>]
        Language : string
        [<JsonProperty(PropertyName="forks")>]
        Forks : int
        [<JsonProperty(PropertyName="forks_count")>]
        ForkCount : int
        [<JsonProperty(PropertyName="watchers")>]
        Watchers : int
        [<JsonProperty(PropertyName="watchers_count")>]
        WatchersCount : int
        [<JsonProperty(PropertyName="size")>]
        Size : int
        [<JsonProperty(PropertyName="master_branch")>]
        MasterBranch : string
        [<JsonProperty(PropertyName="open_issues")>]
        OpenIssues : int
    }

    type UserOrOrganization = 
        | AuthenticatedUser
        | SpecifiedUser of string
        | Organization of string

    type Type = | All | Owner | Public | Private | Member
    type Direction = | Ascending | Descending
    type Sort = 
        | FullName 
        | Created 
        | Updated 
        | Pushed

    type ListParams = {        
        Owner : UserOrOrganization
        Type : Type
        Sort : Sort
        Direction : Direction
    }    

    // -------------------- //
    // Internal functions   //
    // -------------------- //
    let internal defaultListParams = {
        Owner = AuthenticatedUser
        Type = All
        Sort = FullName
        Direction = Ascending
    }

    let internal buildRequestResource p =
        let resource = 
            match p.Owner with
            | AuthenticatedUser -> "user/repos"
            | SpecifiedUser(x) -> sprintf "users/%s/repos" x
            | Organization(x) -> sprintf "orgs/%s/repos" x
        sprintf "%s?type=%A&sort=%A&direction=%A" resource p.Type p.Sort p.Direction

    let internal List (p:ListParams->ListParams) state = 
        let request = (fun x -> { x with RestResource = (buildRequestResource (p(defaultListParams))) })
        let resolve x = 
            deserialize { return typeof<Repository array>,
            ConvertResponse<Repository array>(restfulResponse { return x, state}) }
        resolve request