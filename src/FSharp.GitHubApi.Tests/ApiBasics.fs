namespace FSharp.GitHubApi.Tests

    open NUnit.Framework

    [<TestFixture>]
    [<Category("FirstPass")>]
    type ``Rate limiting``() =

        [<Test>]
        member this.``should have a limit of 60 for anonymous users``() =
            let rateLimit = 
                TestHelper.AnonymousUser
                |> TestHelper.DefaultState 
                |> GitHub.RateLimit
            printfn "%s" rateLimit.ErrorMessage
            Assert.AreEqual(200, rateLimit.StatusCode)
            Assert.AreEqual(60, rateLimit.Content.Limit)
            Assert.LessOrEqual(rateLimit.Content.Remaining, 60)
