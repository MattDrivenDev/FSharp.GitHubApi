[<AutoOpen>]
module FSharp.GitHubApi.RateLimiting

    open Newtonsoft.Json
    open RestHelper
    open JsonHelper
    
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
        let request = (fun x -> { x with RestResource = "rate_limit" })
        let resolve x = 
            deserialize { return typeof<RateLimit>, 
            ConvertResponse<RateLimit>(restfulResponse { return x, state }) }
        resolve request