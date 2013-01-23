module FSharp.GitHubApi.Json

    open System
    open System.Net
    open System.IO
    open System.Text
    open RestSharp
    open RestFSharp
    open Newtonsoft.Json


    // -------------------- //
    // Internal functions   //
    // -------------------- //
    let internal DeserializeResponseContent (response:GitHubResponse<'T>) = 
        match response.StatusCode with            
        | HttpStatusCode.OK | HttpStatusCode.Created ->
            { response with Content = Content(JsonConvert.DeserializeObject<'T>(response.ContentRaw)) }
        | HttpStatusCode.Unauthorized ->
            { response with Content = Error(JsonConvert.DeserializeObject<Message>(response.ContentRaw)) }
        | _ -> { response with ResponseStatus = ErrorResponse(response.StatusDescription) }

    let internal SerializeToJson x = 
        JsonConvert.SerializeObject(x)