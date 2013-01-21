module FSharp.GitHubApi.GitHub

    open FSharp.GitHubApi

    // -------------------- //
    // Api Basics           //
    // -------------------- //
    let GetRateLimit = 
        RateLimiting.Check

    // -------------------- //
    // Users Api            //
    // -------------------- //
    let GetUser = 
        Users.Get

    let UpdateUser = 
        Users.Update

    let GetAllUsers = 
        Users.GetAll

    // -------------------- //
    // Repositories Api     //
    // -------------------- //
    let ListRepositories = 
        Repositories.List

    let CreateRepository = 
        Repositories.Create

    let DeleteRepository = 
        Repositories.Delete

    let ListBranches = 
        Repositories.ListBranches