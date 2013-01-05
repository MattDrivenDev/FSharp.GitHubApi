module FSharp.GitHubApi.JsonHelper

    open System
    open System.IO
    open System.Text
    open Newtonsoft.Json

    // -------------------- //
    // Internal functions   //
    // -------------------- //
    let internal DeserializeJson<'T> json = 
        try
            Some(JsonConvert.DeserializeObject<'T>(value=json))
        with
        | ex ->
            printfn "Cannot deserialize json: %s" ex.Message
            None

    let internal SerializeToJson t =   
        try
            JsonConvert.SerializeObject(t)
        with
        | ex ->
            sprintf "Cannot serialize record type: %s" ex.Message