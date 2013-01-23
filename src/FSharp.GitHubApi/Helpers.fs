module FSharp.GitHubApi.Helpers

    open RestFSharp
    open Json

    let internal GetGitHubResponse<'T> state = 
        state 
        |> RestfulResponse 
        >> ConvertResponse<'T>

    let internal GetDeserializedGitHubResponse<'T> state = 
        state
        |> RestfulResponse
        >> ConvertResponse<'T>
        >> DeserializeResponseContent