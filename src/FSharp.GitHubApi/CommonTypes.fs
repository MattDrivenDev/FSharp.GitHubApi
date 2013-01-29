[<AutoOpen>]
module FSharp.GitHubApi.CommonTypes

    type EmailAddress = 
        | EmailAddress of string
        member x.Value = let (EmailAddress v) = x in v

    type WebsiteUrl = 
        | WebsiteUrl of string
        member x.Value = let (WebsiteUrl v) = x in v

    type LoginName = 
        | LoginName of string
        member x.Value = let (LoginName v) = x in v

    type Hexcode = 
        | Hexcode of string
        member x.Value = let (Hexcode v) = x in v

    type Count = 
        | Count of int
        member x.Value = let (Count v) = x in v