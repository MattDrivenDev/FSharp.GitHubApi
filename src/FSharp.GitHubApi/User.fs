[<AutoOpen>]
module FSharp.GitHubApi.User

    open System
    open Newtonsoft.Json
    open RestFSharp
    open Json
    open Helpers
    open FSharp.Data

    type internal _Provider = JsonProvider<"""
        {
          "login": "octocat",
          "id": 1,
          "avatar_url": "https://github.com/images/error/octocat_happy.gif",
          "gravatar_id": "somehexcode",
          "url": "https://api.github.com/users/octocat",
          "name": "monalisa octocat",
          "company": "GitHub",
          "blog": "https://github.com/blog",
          "location": "San Francisco",
          "email": "octocat@github.com",
          "hireable": false,
          "bio": "There once was...",
          "public_repos": 2,
          "public_gists": 1,
          "followers": 20,
          "following": 0,
          "html_url": "https://github.com/octocat",
          "created_at": "2008-01-14T04:33:35Z",
          "type": "User",
          "total_private_repos": 100,
          "owned_private_repos": 100,
          "private_gists": 81,
          "disk_usage": 10000,
          "collaborators": 8,
          "plan": {
            "name": "Medium",
            "space": 400,
            "collaborators": 10,
            "private_repos": 20
          }
        }""">    

    type internal _ListProvider = JsonProvider<"""
        [
         {
            "login": "octocat",
            "id": 1,
            "avatar_url": "https://github.com/images/error/octocat_happy.gif",
            "gravatar_id": "somehexcode",
            "url": "https://api.github.com/users/octocat"
          }
        ]""">

    // -------------------- //
    // Public data types    //
    // -------------------- //
    type UserId = 
        | UserId of int
        member x.Value = let (UserId v) = x in v

    type Plan = {
        Name : string
        Space : int
        Collaborators : Count
        PrivateRepositories : Count }

    type UserIdentity = {
        Login : LoginName
        Id : UserId
        AvatarUrl : WebsiteUrl
        GravatarId : Hexcode
        Url : WebsiteUrl }

    type T = {
        Identity : UserIdentity
        Name : string
        Company : string
        Blog : WebsiteUrl
        Location : string
        Email : EmailAddress
        Hireable : bool
        Bio : string
        PublicRepositories : Count
        PublicGists : Count
        Followers : Count
        Following : Count
        HtmlUrl : WebsiteUrl
        CreatedAt : string
        Type : string
        TotalPrivateRepositories : Count
        OwnedPrivateRepositories : Count
        PrivateGists : Count
        DiskUsage : int
        Collaborators : Count
        Plan : Plan }

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

    let internal CreatePlan (x:_Provider.DomainTypes.Entity1) = {
        Name = x.Name
        Space = x.Space
        Collaborators = Count(x.Collaborators)
        PrivateRepositories = Count(x.PrivateRepos) }

    let internal Create (x:_Provider.DomainTypes.Entity) = {
        Identity = {
            Login = LoginName(x.Login)
            Id = UserId(x.Id)
            AvatarUrl = WebsiteUrl(x.AvatarUrl)
            GravatarId = Hexcode(x.GravatarId)
            Url = WebsiteUrl(x.Url) }
        Name = x.Name
        Company = x.Company
        Blog = WebsiteUrl(x.Blog)
        Location = x.Location
        Email = EmailAddress(x.Email)
        Hireable = x.Hireable
        Bio = x.Bio
        PublicRepositories = Count(x.PublicRepos)
        PublicGists = Count(x.PublicGists)
        Followers = Count(x.Followers)
        Following = Count(x.Following)
        HtmlUrl = WebsiteUrl(x.HtmlUrl)
        CreatedAt = x.CreatedAt
        Type = x.Type
        TotalPrivateRepositories = Count(x.TotalPrivateRepos)
        OwnedPrivateRepositories = Count(x.OwnedPrivateRepos)
        PrivateGists = Count(x.PrivateGists)
        DiskUsage = x.DiskUsage
        Collaborators = Count(x.Collaborators)
        Plan = CreatePlan x.Plan }

    let internal CreateList (xs:_ListProvider.DomainTypes.Entity array) =
        xs |> Array.map(fun x -> {
            Login = LoginName(x.Login)
            Id = UserId(x.Id)
            AvatarUrl = WebsiteUrl(x.AvatarUrl)
            GravatarId = Hexcode(x.GravatarId)
            Url = WebsiteUrl(x.Url) })

    let internal getCurrent state = 
        let response = 
            state |> GetGitHubResponse<T> (fun x -> 
                { x with RestResource = "user"})
        let x = _Provider.Parse(response.ContentRaw)
        { response with Content = Content(Create x) }

    let internal getSpecified username state =
        let response = 
            state |> GetGitHubResponse<T> (fun x -> 
                { x with RestResource = (sprintf "users/%s" username)})        
        let x = _Provider.Parse(response.ContentRaw)
        { response with Content = Content(Create x) }

    let internal Get p state = 
        match p with
        | AuthenticatedUser -> getCurrent state
        | SpecificUser(x) -> getSpecified x state
            
    let internal Update (p:UpdateParams->UpdateParams) state = 
        let json = SerializeToJson (p(defaultUpdateParams))
        let response = 
            state |> GetGitHubResponse<T> (fun x -> 
                { x with Method = PATCH; RestResource = "user"; Content = json })
        let x = _Provider.Parse(response.ContentRaw)
        { response with Content = Content(Create x) }
        
    let internal GetAll since state = 
        let response = 
            state |> GetGitHubResponse<UserIdentity array> (fun x -> 
                { x with RestResource = (sprintf "users?since=%i" since) })        
        let x = _ListProvider.Parse(response.ContentRaw)
        { response with Content = Content(CreateList x) }