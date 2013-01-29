[<AutoOpen>]
module FSharp.GitHubApi.RateLimiting



    open Newtonsoft.Json
    open RestFSharp
    open Json
    open Helpers
    
    // -------------------- //
    // Public data types    //
    // -------------------- //
    type Rate = {
        [<field: JsonProperty(PropertyName="remaining")>] 
        Remaining : int
        [<field: JsonProperty(PropertyName="limit")>] 
        Limit : int
    }

    type RateLimit = {        
        [<field: JsonProperty(PropertyName="rate")>] 
        Rate : Rate
    }    

    // -------------------- //
    // Internal functions   //
    // -------------------- //
    let internal Check state = 
        state |> GetDeserializedGitHubResponse<RateLimit> (fun x -> 
            { x with RestResource = "rate_limit" })        