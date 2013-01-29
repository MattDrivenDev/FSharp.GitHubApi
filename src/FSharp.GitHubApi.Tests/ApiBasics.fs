namespace FSharp.GitHubApi.Tests

    open NUnit.Framework
    open FSharp.GitHubApi
    open FSharp.GitHubApi.RateLimit

    [<TestFixture>]
    [<Category("FirstPass")>]
    type ``Rate limiting``() =

        let ``assert rate limit response is as expected`` i x =
            match x.Content with
                | Content(y) ->
                    Assert.AreEqual(i, y.Rate.Limit.Value)
                    Assert.LessOrEqual(y.Rate.Remaining.Value, i)
                | Error(y) -> 
                    printfn "%s" y.Message
                    Assert.Fail()
                | _ -> Assert.Fail()

        [<Test>]
        member this.``should have a limit of 60 for anonymous users``() =
            let x = TestHelper.AnonymousUser |> TestHelper.DefaultState |> GitHub2.GetRateLimit
            x |> ``assert rate limit response is as expected`` 60

        [<Test>]
        member this.``should have a limit of 5000 for authenticated users``() =
            let x = TestHelper.AuthenticatedUser |> TestHelper.DefaultState |> GitHub2.GetRateLimit
            x |> ``assert rate limit response is as expected`` 5000