module FSharp.GitHubApi.ApiHelper

    open Newtonsoft.Json
        
    // -------------------- //
    // Public data types    //
    // -------------------- //
    type GitHubResponse<'T> = {
        StatusCode: int
        Content: 'T
        ErrorMessage: string
    }

    type Rate = {
        [<field: JsonProperty(PropertyName="remaining")>] Remaining : int
        [<field: JsonProperty(PropertyName="limit")>] Limit         : int
    }

    type RateLimit = {        
        [<field: JsonProperty(PropertyName="rate")>] Rate           : Rate
    }

    // -------------------- //
    // Internal functions   //
    // -------------------- //
    let internal RateLimit state =         
        match RestHelper.Get { Resource = "rate_limit" } state with
        | RestHelper.Success(json) ->
            let rateLimit = json |> JsonHelper.DeserializeJson<RateLimit>
            match rateLimit with
            | Some(rl) -> { StatusCode = 200; Content = rl.Rate; ErrorMessage = ""; }
            | None -> { StatusCode = 200; Content = { Remaining = 0; Limit = 0; }; ErrorMessage = "Cannot deserialize RateLimit" }
        | RestHelper.Failed(reason) -> { StatusCode = 0; Content = { Remaining = 0; Limit = 0; }; ErrorMessage = reason; }