module FSharp.GitHubApi.ApiHelper

    open Newtonsoft.Json
    open System.IO
    open FSharp.Data
        
    // -------------------- //
    // Public data types    //
    // -------------------- //
    type GitHubResponse<'T> = {
        StatusCode: int
        Content: 'T option
        ErrorMessage: string
    }

    type RateLimit = JsonProvider<""" { "rate": { "remaining": 4999, "limit": 5000 } }""">    

    // -------------------- //
    // Internal functions   //
    // -------------------- //
    let internal RateLimit state =         
        match RestHelper.Get { Resource = "rate_limit" } state with
        | RestHelper.Success(json) ->
            try
                let rateLimit = json |> RateLimit.Parse
                { StatusCode = 200; Content = Some(rateLimit); ErrorMessage = ""; }
            with
            | ex ->
                { StatusCode = 200; Content = None; ErrorMessage = ex.Message }
        | RestHelper.Failed(reason) -> { StatusCode = 0; Content = None; ErrorMessage = reason; }