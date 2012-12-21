module GitHub

    open System.Runtime.Serialization

    type GitHubResponse<'T> = {
        StatusCode: int
        Content: 'T
        ErrorMessage: string
    }

    [<DataContract>]
    type Rate = {
        [<field: DataMember(Name="remaining")>]
        Remaining: int
        [<field: DataMember(Name="limit")>]
        Limit: int
    }

    [<DataContract>]
    type RateLimit = {        
        [<field: DataMember(Name="rate")>]
        Rate: Rate
    }

    let RateLimit state =         
        match RestHelper.Get { Resource = "rate_limit" } state with
        | RestHelper.Success(json) ->
            let rateLimit = json |> JsonHelper.DeserializeJson<RateLimit>
            match rateLimit with
            | Some(rl) -> { StatusCode = 200; Content = rl.Rate; ErrorMessage = ""; }
            | None -> { StatusCode = 200; Content = { Remaining = 0; Limit = 0; }; ErrorMessage = "Cannot deserialize RateLimit" }
        | RestHelper.Failed(reason) -> { StatusCode = 0; Content = { Remaining = 0; Limit = 0; }; ErrorMessage = reason; }