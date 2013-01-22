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

    type Organization = {
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
        [<JsonProperty(PropertyName="type")>]
        Type : string
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
        [<JsonProperty(PropertyName="organization")>]
        Organization : Organization
        [<JsonProperty(PropertyName="parent")>]
        Parent : Repository
        [<JsonProperty(PropertyName="source")>]
        Source : Repository
        [<JsonProperty(PropertyName="has_issues")>]
        HasIssues : bool
        [<JsonProperty(PropertyName="has_wiki")>]
        HasWiki : bool
        [<JsonProperty(PropertyName="has_downloads")>]
        HasDownloads : bool
    }

    type Commit = {
        [<JsonProperty(PropertyName="sha")>]
        Sha : string
        [<JsonProperty(PropertyName="url")>]
        Url : string
    }

    type BranchSummary = {
        [<JsonProperty(PropertyName="name")>]
        Name : string
        [<JsonProperty(PropertyName="commit")>]
        Commit : Commit
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

    type CreateParams = {
        [<JsonProperty(PropertyName="name")>]
        RepoName : string
        [<JsonProperty(PropertyName="description")>]
        Description : string
        [<JsonProperty(PropertyName="homepage")>]
        Homepage : string
        [<JsonProperty(PropertyName="private")>]
        Private : bool
        [<JsonProperty(PropertyName="has_issues")>]
        HasIssues : bool
        [<JsonProperty(PropertyName="has_wiki")>]
        HasWiki : bool
        [<JsonProperty(PropertyName="has_downloads")>]
        HasDownloads : bool
        [<JsonProperty(PropertyName="team_id")>]
        TeamId : int
        [<JsonProperty(PropertyName="auto_init")>]
        AutoInit : bool
        [<JsonProperty(PropertyName="gitignore_template")>]
        GitIgnoreTemplate : string
    }

    type DeleteParams = {
        RepoName : string
        Owner : string
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

    let internal defaultCreateParams = {
        RepoName = null
        Description = null
        Homepage = null
        Private = false
        HasIssues = true
        HasWiki = true
        HasDownloads = true
        TeamId = 0
        AutoInit = false
        GitIgnoreTemplate = null
    }

    let internal defaultDeleteParams = {
        RepoName = null
        Owner = null
    }

    let internal buildListResource (p:ListParams) =
        let resource = 
            match p.Owner with
            | AuthenticatedUser -> "user/repos"
            | SpecifiedUser(x) -> sprintf "users/%s/repos" x
            | Organization(x) -> sprintf "orgs/%s/repos" x
        sprintf "%s?type=%A&sort=%A&direction=%A" resource p.Type p.Sort p.Direction

    let internal buildDeleteResource (p:DeleteParams) = 
        sprintf "repos/%s/%s" p.Owner p.RepoName

    let internal List (p:ListParams->ListParams) state = 
        let request = (fun x -> { x with RestResource = (buildListResource (p(defaultListParams))) })       
        let resolve x = 
            RestfulResponse x state
            |> ConvertResponse<Repository array>
            |> DeserializeResponseContent<Repository array>
        resolve request

    let internal Create (p:CreateParams->CreateParams) state =         
        let json = SerializeToJson (p(defaultCreateParams))
        let request = (fun x -> { x with RestResource = "user/repos"; Method = POST; Content = json; })
        let resolve x = 
            RestfulResponse x state
            |> ConvertResponse<Repository>
            |> DeserializeResponseContent<Repository>
        resolve request

    let internal Delete (p:DeleteParams->DeleteParams) state = 
        let request = (fun x -> { x with RestResource = (buildDeleteResource (p(defaultDeleteParams))); Method = DELETE; })
        let resolve x = 
            RestfulResponse x state
            |> ConvertResponse<Repository>
        resolve request

    let internal ListBranches owner repo state = 
        let request = (fun x -> { x with RestResource = (sprintf "repos/%s/%s/branches" owner repo) })
        let resolve x =
            RestfulResponse x state
            |> ConvertResponse<BranchSummary array>
            |> DeserializeResponseContent<BranchSummary array>
        resolve request

    let internal Get owner repo state = 
        let request = (fun x -> { x with RestResource = (sprintf "repos/%s/%s" owner repo) })
        let resolve x =
            RestfulResponse x state
            |> ConvertResponse<Repository>
            |> DeserializeResponseContent<Repository>
        resolve request