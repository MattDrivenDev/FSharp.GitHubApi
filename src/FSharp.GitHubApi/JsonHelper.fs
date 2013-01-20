module FSharp.GitHubApi.JsonHelper

    open System
    open System.Net
    open System.IO
    open System.Text
    open RestHelper
    open RestSharp
    open Newtonsoft.Json

    
    // -------------------- //
    // Internal types       //
    // -------------------- //
    type internal JsonSerializer() = 
        member this.Bind(x) = x
        member this.Delay(f) = f()
        member this.Return(x) = 
            JsonConvert.SerializeObject(x)

    type internal JsonDeserializer() =
        member this.Bind(x) = x
        member this.Delay(f) = f() 
        member this.Return(T:Type, r:GitHubResponse<'T>) = 
            match r.StatusCode with            
            | HttpStatusCode.OK ->
                { r with Content = Content(JsonConvert.DeserializeObject<'T>(r.ContentRaw)) }
            | HttpStatusCode.Created ->
                { r with Content = Content(JsonConvert.DeserializeObject<'T>(r.ContentRaw)) }
            | HttpStatusCode.Unauthorized ->
                { r with Content = Error(JsonConvert.DeserializeObject<Message>(r.ContentRaw)) }
            | _ -> { r with ResponseStatus = Some(ErrorResponse(r.StatusDescription)) }
            

    // -------------------- //
    // Internal functions   //
    // -------------------- //
    let internal deserialize = new JsonDeserializer()
    let internal serialize = new JsonSerializer()