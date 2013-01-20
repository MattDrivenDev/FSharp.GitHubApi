namespace FSharp.GitHubApi.Tests.Repositories

    open NUnit.Framework
    open FSharp.GitHubApi
    open FSharp.GitHubApi.Repositories

    [<TestFixture>]
    [<Category("FirstPass")>]
    type Repositories() =
        
        let getUserRepositoriesResponse p = 
            TestHelper.AuthenticatedUser
            |> TestHelper.DefaultState
            |> GitHub.ListRepositories p

        [<Test>]
        member this.``should be able to get current authenticated user's repositories``() =
            let x = getUserRepositoriesResponse (fun p -> { p with Owner = AuthenticatedUser; })
            match x.Content with
            | Content(y) -> Assert.GreaterOrEqual(y.Length, 1)
            | _ -> Assert.Fail()

        [<Test>]
        member this.``should be able to get a specified user's repositories``() =
            let x = getUserRepositoriesResponse (fun p -> { p with Owner = SpecifiedUser("saxonmatt"); })
            match x.Content with
            | Content(y) -> Assert.GreaterOrEqual(y.Length, 1)
            | _ -> Assert.Fail()

        [<Test>]
        member this.``should be able to get a list of organization's repositories``() =
            let x = getUserRepositoriesResponse (fun p -> { p with Owner = Organization("fsharp"); })
            match x.Content with
            | Content(y) -> Assert.GreaterOrEqual(y.Length, 1)
            | _ -> Assert.Fail()