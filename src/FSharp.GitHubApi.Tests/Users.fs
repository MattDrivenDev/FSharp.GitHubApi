namespace FSharp.GitHubApi.Users

    open NUnit.Framework
    open FSharp.GitHubApi

    [<TestFixture>]
    [<Category("FirstPass")>]
    type Users() =
            
        let ``assert user details are as expected`` u n p x =
            match x.Content with
            | Content(y) ->
                Assert.IsTrue(y.Login.Equals(u))
                Assert.IsTrue(y.Name.Equals(n))
                Assert.IsTrue(y.Plan.Name.Equals(p))
            | _ -> Assert.Fail()

        let ``assert name has changed`` n (x:GitHubResponse<UserDetails>) = 
            match x.Content with
            | Content(y) ->
                Assert.IsTrue(y.Name.Equals(n)) 
            | _ -> Assert.Fail()

        let ``assert that there is a collection of users`` (x:GitHubResponse<UserSummary array>) =
            match x.Content with
            | Content(ys) ->
                Assert.GreaterOrEqual(ys.Length, 1)                
            | _ -> Assert.Fail()

        let updateUserResponse p = 
            TestHelper.AuthenticatedUser
            |> TestHelper.DefaultState
            |> GitHub.UpdateUser p          

        let getUserId username = 
            let x = 
                TestHelper.AuthenticatedUser 
                |> TestHelper.DefaultState 
                |> GitHub.GetUser (SpecificUser("saxonmatt"))
            match x.Content with
            | Content(y) -> y.Id
            | _ -> 0


        [<Test>]
        member this.``should be able to get a specified user``() =
            let x = 
                TestHelper.AuthenticatedUser 
                |> TestHelper.DefaultState 
                |> GitHub.GetUser (SpecificUser("saxonmatt"))
            x |> ``assert user details are as expected`` "saxonmatt" "Matt Ball" "free"

        [<Test>]
        member this.``should be able to get current authenticated user``() =    
            let x = 
                TestHelper.AuthenticatedUser 
                |> TestHelper.DefaultState 
                |> GitHub.GetUser AuthenticatedUser
            x |> ``assert user details are as expected`` TestSettings.GitHubUsername TestSettings.GitHubName TestSettings.GitHubPlan     

        [<Test>]
        member this.``should be able to update the current authenticated user``() =            
            let newName = sprintf "edited-%s" TestSettings.GitHubName
            let x = updateUserResponse (fun x -> { x with Name = newName })
            x |> ``assert name has changed`` newName             
            updateUserResponse (fun x -> { x with Name = TestSettings.GitHubName }) |> ignore

        [<Test>]
        member this.``should be able to get a collection of all users``() = 
            let since = getUserId "saxonmatt"
            let x = 
                TestHelper.AuthenticatedUser
                |> TestHelper.DefaultState
                |> GitHub.GetAllUsers since
            x |> ``assert that there is a collection of users``