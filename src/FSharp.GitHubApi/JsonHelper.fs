module FSharp.GitHubApi.JsonHelper

    open System
    open System.IO
    open System.Text
    open System.Runtime.Serialization    

    // -------------------- //
    // Internal functions   //
    // -------------------- //
    let internal DeserializeJson<'T> json = 
        try
            let buffer = Encoding.UTF8.GetBytes(s=json)
            let deserializer = Json.DataContractJsonSerializer(typeof<'T>)
            Some(deserializer.ReadObject(new MemoryStream(buffer)) :?> 'T)
        with
        | _ ->
            printfn "Cannot deserialize json"
            None

    let internal SerializeJson T = 
        try
            Some("")
        with
        | _ ->
            printfn "Cannot serialize to json"
            None