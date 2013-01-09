namespace FSharp.GitHubApi.Tests

    open NUnit.Framework
    open FSharp.GitHubApi

    [<TestFixture>]
    [<Category("FirstPass")>]
    type ``Rate limiting``() =

        [<Test>]
        member this.``should have a limit of 60 for anonymous users``() =
            let rateLimit = 
                TestHelper.AnonymousUser
                |> TestHelper.DefaultState 
                |> GitHub.GetRateLimit
            match rateLimit.Content with
            | Some(x) ->
                Assert.AreEqual(60, x.Rate.Limit)
                Assert.LessOrEqual(x.Rate.Remaining, 60)
            | None -> Assert.Fail()

        [<Test>]
        member this.``should have a limit of 5000 for authenticated users``() =
            let rateLimit = 
                TestHelper.AuthenticatedUser
                |> TestHelper.DefaultState
                |> GitHub.GetRateLimit
            match rateLimit.Content with
            | Some(x) ->
                Assert.AreEqual(5000, x.Rate.Limit)
                Assert.LessOrEqual(x.Rate.Remaining, 5000)
            | None -> Assert.Fail()