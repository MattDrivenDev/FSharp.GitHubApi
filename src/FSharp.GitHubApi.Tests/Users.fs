namespace FSharp.GitHubApi.Users

    open NUnit.Framework

    [<TestFixture>]
    [<Category("FirstPass")>]
    type Users() =

        [<Test>]
        member this.``should be able to get a specified user``() =
            let getQuery = Users.SpecificUser("saxonmatt")
            let userResponse = 
                TestHelper.AuthenticatedUser
                |> TestHelper.DefaultState
                |> Users.Get getQuery
            printfn "%s" userResponse.ErrorMessage
            Assert.AreEqual(200, userResponse.StatusCode)
            match userResponse.Content with
            | Some(user) ->
                Assert.IsTrue(user.Login.Equals("saxonmatt"))
                Assert.IsTrue(user.Name.Equals("Matt Ball"))
                Assert.IsTrue(user.Blog.Contains("www.saxonmatt.co.uk"))
            | None -> Assert.Fail()

        [<Test>]
        member this.``should be able to get current authenticated user``() =
            let getQuery = Users.AuthenticatedUser
            let userResponse = 
                TestHelper.AuthenticatedUser
                |> TestHelper.DefaultState
                |> Users.Get getQuery
            printfn "%s" userResponse.ErrorMessage
            Assert.AreEqual(200, userResponse.StatusCode)
            match userResponse.Content with
            | Some(user) ->
                Assert.IsTrue(user.Login.Equals(TestSettings.GitHubUsername))
                Assert.IsTrue(user.Name.Equals(TestSettings.GitHubName))
                Assert.IsTrue(user.Plan.Name.Equals(TestSettings.GitHubPlan))  
            | None -> Assert.Fail()         