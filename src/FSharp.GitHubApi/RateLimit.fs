module FSharp.GitHubApi.RateLimit

    open System.IO
    open FSharp.Data
    open Helpers
    open RestFSharp

    type internal _Provider = JsonProvider<"""
        {
          "rate": {
            "remaining": 4999,
            "limit": 5000
          }
        }""">

    type Remaining = 
        | Remaining of int
        member x.Value = let (Remaining v) = x in v

    type Limit = 
        | Limit of int
        member x.Value = let (Limit v) = x in v

    type Rate = {
        Remaining : Remaining
        Limit : Limit
    }

    type T = {
        Rate : Rate
    }

    // -------------------- //
    // Internal functions   //
    // -------------------- //
    let internal Create (x:_Provider.DomainTypes.Entity) = {
        Rate = { Remaining = Remaining(x.Rate.Remaining); Limit = Limit(x.Rate.Limit) } }

    let internal Check state = 
        let response = 
            state |> GetGitHubResponse<T> (fun x -> 
                { x with RestResource = "rate_limit" }) 
        let x = _Provider.Parse(response.ContentRaw)
        { response with Content = Content(Create x) }