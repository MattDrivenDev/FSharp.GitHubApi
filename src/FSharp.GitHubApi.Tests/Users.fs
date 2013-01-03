namespace FSharp.GitHubApi.Users

    open NUnit.Framework

    [<TestFixture>]
    [<Category("FirstPass")>]
    type Users() =

        [<Test>]
        member this.``should be able to get a specified user``() =
            let getParams = Users.SpecificUser("saxonmatt")
            let userResponse = 
                TestHelper.AuthenticatedUser
                |> TestHelper.DefaultState
                |> GitHub.GetUser getParams
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
            let getParams = Users.AuthenticatedUser
            let userResponse = 
                TestHelper.AuthenticatedUser
                |> TestHelper.DefaultState
                |> GitHub.GetUser getParams
            printfn "%s" userResponse.ErrorMessage
            Assert.AreEqual(200, userResponse.StatusCode)
            match userResponse.Content with
            | Some(user) ->
                Assert.IsTrue(user.Login.Equals(TestSettings.GitHubUsername))
                Assert.IsTrue(user.Name.Equals(TestSettings.GitHubName))
                Assert.IsTrue(user.Plan.Name.Equals(TestSettings.GitHubPlan))  
            | None -> Assert.Fail()         

        [<Test>]
        member this.``should be able to update the current authenticated user``() =
            let newName = sprintf "edited-%s" TestSettings.GitHubName
            let updateParams = { Users.DefaultUpdateParams with Name = Some(newName) }
            let updateResponse = 
                TestHelper.AuthenticatedUser
                |> TestHelper.DefaultState
                |> GitHub.UpdateUser updateParams
            printfn "%s" updateResponse.ErrorMessage
            Assert.AreEqual(200, updateResponse.StatusCode)
            match updateResponse.Content with
            | Some(user) -> 
                Assert.IsTrue(user.Name.Equals(newName))
                TestHelper.AuthenticatedUser
                |> TestHelper.DefaultState
                |> GitHub.UpdateUser { updateParams with Name = Some(TestSettings.GitHubName) }
                |> ignore
            | None -> Assert.Fail()