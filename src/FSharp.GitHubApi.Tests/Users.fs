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

        let getAllUsersResponse sinceId =
            TestHelper.AuthenticatedUser
            |> TestHelper.DefaultState
            |> GitHub.GetAllUsers sinceId

        let getUserId username = 
            let userResponse = getUserResponse (SpecificUser(username))
            userResponse.Content.Value.Id

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
            let updateResponse = updateUserResponse (fun x -> { x with Name = newName })
            match updateResponse.Content with
            | Some(user) -> 
                Assert.IsTrue(user.Name.Equals(newName))
                updateUserResponse (fun x -> { x with Name = TestSettings.GitHubName }) |> ignore
            | None -> Assert.Fail()

        [<Test>]
        member this.``should be able to get a collection of all users``() = 
            let id = "saxonmatt" |> getUserId 
            let usersResponse = getAllUsersResponse id
            match usersResponse.Content with
            | Some(c) ->
                 Assert.GreaterOrEqual(c.Length, 1)
            | None -> Assert.Fail()