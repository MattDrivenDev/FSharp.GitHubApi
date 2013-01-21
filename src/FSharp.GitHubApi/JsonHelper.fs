module FSharp.GitHubApi.JsonHelper

    open System
    open System.Net
    open System.IO
    open System.Text
    open RestHelper
    open RestSharp
    open Newtonsoft.Json


    // -------------------- //
    // Internal functions   //
    // -------------------- //
    let internal DeserializeResponseContent<'T> response = 
        match response.StatusCode with            
        | HttpStatusCode.OK ->
            { response with Content = Content(JsonConvert.DeserializeObject<'T>(response.ContentRaw)) }
        | HttpStatusCode.Created ->
            { response with Content = Content(JsonConvert.DeserializeObject<'T>(response.ContentRaw)) }
        | HttpStatusCode.Unauthorized ->
            { response with Content = Error(JsonConvert.DeserializeObject<Message>(response.ContentRaw)) }
        | _ -> { response with ResponseStatus = Some(ErrorResponse(response.StatusDescription)) }

    let internal SerializeToJson x = 
        JsonConvert.SerializeObject(x)