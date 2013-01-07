﻿module FSharp.GitHubApi.GitHub

    open FSharp.GitHubApi

    // -------------------- //
    // Api Basics           //
    // -------------------- //
    let GetRateLimit = 
        ApiHelper.RateLimit

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