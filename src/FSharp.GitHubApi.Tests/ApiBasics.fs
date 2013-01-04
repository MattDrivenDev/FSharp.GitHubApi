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
            Assert.AreEqual(60, rateLimit.Content.Limit)
            Assert.LessOrEqual(rateLimit.Content.Remaining, 60)

        [<Test>]
        member this.``should have a limit of 5000 for authenticated users``() =
            let rateLimit = 
                TestHelper.AuthenticatedUser
                |> TestHelper.DefaultState
                |> GitHub.GetRateLimit
            Assert.AreEqual(5000, rateLimit.Content.Limit)
            Assert.LessOrEqual(rateLimit.Content.Remaining, 5000)