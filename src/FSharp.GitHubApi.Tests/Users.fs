namespace FSharp.GitHubApi.Users

    open NUnit.Framework
    open FSharp.GitHubApi
    open FSharp.GitHubApi.Users

    [<TestFixture>]
    [<Category("FirstPass")>]
    type Users() =

        let getUserResponse p = 
            TestHelper.AuthenticatedUser
            |> TestHelper.DefaultState
            |> GitHub.GetUser p

        let updateUserResponse p = 
            TestHelper.AuthenticatedUser
            |> TestHelper.DefaultState
            |> GitHub.UpdateUser p

        [<Test>]
        member this.``should be able to get a specified user``() =
            let userResponse = getUserResponse (SpecificUser("saxonmatt"))
            match userResponse.Content with
            | Some(user) ->
                Assert.IsTrue(user.Login.Equals("saxonmatt"))
                Assert.IsTrue(user.Name.Equals("Matt Ball"))
                Assert.IsTrue(user.Blog.Contains("www.saxonmatt.co.uk"))
            | None -> Assert.Fail()

        [<Test>]
        member this.``should be able to get current authenticated user``() =            
            let userResponse = getUserResponse AuthenticatedUser
            match userResponse.Content with
            | Some(user) ->
                Assert.IsTrue(user.Login.Equals(TestSettings.GitHubUsername))
                Assert.IsTrue(user.Name.Equals(TestSettings.GitHubName))
                Assert.IsTrue(user.Plan.Name.Equals(TestSettings.GitHubPlan))  
            | None -> Assert.Fail()         

        [<Test>]
        member this.``should be able to update the current authenticated user``() =
            let newName = sprintf "edited-%s" TestSettings.GitHubName
            let updateResponse = updateUserResponse { DefaultUpdateParams with Name = Some(newName) }
            match updateResponse.Content with
            | Some(user) -> 
                Assert.IsTrue(user.Name.Equals(newName))
                updateUserResponse { DefaultUpdateParams with Name = Some(TestSettings.GitHubName) } |> ignore
            | None -> Assert.Fail()