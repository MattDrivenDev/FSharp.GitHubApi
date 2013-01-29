module FSharp.GitHubApi.GitHub2

    open FSharp.GitHubApi

    // -------------------- //
    // Api Basics           //
    // -------------------- //
    let GetRateLimit = 
        RateLimit.Check