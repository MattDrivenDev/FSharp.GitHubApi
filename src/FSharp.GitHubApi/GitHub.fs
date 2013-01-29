module FSharp.GitHubApi.GitHub

    open FSharp.GitHubApi

    // -------------------- //
    // Api Basics           //
    // -------------------- //
    let GetRateLimit = 
        RateLimit.Check

    // -------------------- //
    // Users Api            //
    // -------------------- //
    let GetUser = 
        User.Get

    let UpdateUser = 
        User.Update

    let GetAllUsers = 
        User.GetAll

    // -------------------- //
    // Repositories Api     //
    // -------------------- //
    let ListRepositories = 
        Repositories.List
        
    let GetRepository = 
        Repositories.Get

    let CreateRepository = 
        Repositories.Create
        
    let EditRepository = 
        Repositories.Edit

    let DeleteRepository = 
        Repositories.Delete

    let ListBranches = 
        Repositories.ListBranches

    let GetBranch =
        Repositories.GetBranch

    let ListContributors = 
        Repositories.ListContributors

    let ListTeams =
        Repositories.ListTeams

    let ListTags =
        Repositories.ListTags