namespace FSharp.GitHubApi.Tests.Repositories

    open System.Net
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

        [<Test>]
        member this.``should be able to create and delete a repository``() =
            let state = TestHelper.AuthenticatedUser |> TestHelper.DefaultState
            
            let deleteRepository n = 
                state |> GitHub.DeleteRepository (fun p -> { p with Owner = TestSettings.GitHubUsername; RepoName = n })
                            
            let x = state |> GitHub.CreateRepository (fun p -> { p with RepoName = TestSettings.TestRepoName })
            match x.Content with
            | Content(y) -> 
                Assert.IsTrue(y.Name.Equals(TestSettings.TestRepoName))            
                let d = deleteRepository TestSettings.TestRepoName
                match d.StatusCode with
                | HttpStatusCode.NoContent -> Assert.Pass()
                | _ -> Assert.Fail()
            | _ -> Assert.Fail()

        [<Test>]
        member this.``should be able to get a list of branches from a repository``() =
            let x = 
                TestHelper.AuthenticatedUser
                |> TestHelper.DefaultState
                |> GitHub.ListBranches "saxonmatt" "FSharp.GitHubApi"
            match x.Content with
            | Content(y) -> Assert.GreaterOrEqual(y.Length, 1)
            | _ -> Assert.Fail()

        [<Test>]
        member this.``should be able to get full repository details``() =
            let x = 
                TestHelper.AuthenticatedUser
                |> TestHelper.DefaultState
                |> GitHub.GetRepository "saxonmatt" "FSharp.GitHubApi"
            match x.Content with
            | Content(y) ->
                Assert.IsTrue(y.Owner.Login.Equals("saxonmatt"))
                Assert.IsTrue(y.Name.Equals("FSharp.GitHubApi"))
            | _ -> Assert.Fail()

        [<Test>]
        member this.``should be able to update a repository``() =
            let state = TestHelper.AuthenticatedUser |> TestHelper.DefaultState
            let repoName = sprintf "%s.%s" TestSettings.TestRepoName (System.DateTime.Now.ToString("ddHHmmss"))
            let editedRepoName = sprintf "edited-%s" repoName
            let createTestRepo = GitHub.CreateRepository (fun p -> { p with RepoName = repoName })
            let deleteTestRepo = GitHub.DeleteRepository (fun p -> { p with Owner = TestSettings.GitHubUsername; RepoName = editedRepoName ;})

            createTestRepo state |> ignore
            let x = 
                state 
                |> GitHub.EditRepository TestSettings.GitHubUsername repoName (fun p -> { p with EditParams.RepoName = editedRepoName })
            match x.Content with
            | Content(y) ->
                Assert.IsTrue(y.Name.Equals(editedRepoName))
                deleteTestRepo state |> ignore
            | _ ->                     
                deleteTestRepo state |> ignore
                Assert.Fail()

        [<Test>]
        member this.``should be able to list repository contributors``() =
            let x = 
                TestHelper.AuthenticatedUser
                |> TestHelper.DefaultState
                |> GitHub.ListContributors "fsharp" "FAKE" false
            match x.Content with
            | Content(ys) ->
                Assert.GreaterOrEqual(ys.Length, 1)
            | _ -> Assert.Fail()

        [<Test>]
        member this.``should be able to get full branch details``() =
            let x =
                TestHelper.AuthenticatedUser
                |> TestHelper.DefaultState
                |> GitHub.GetBranch "saxonmatt" "FSharp.GitHubApi" "master"
            match x.Content with
            | Content(y) ->
                Assert.IsTrue(y.Name.Equals("master"))
            | _ -> Assert.Fail()

        [<Test>]
        member this.``should be able to get a list of teams associated with a repository``() =
            let x = 
                TestHelper.AuthenticatedUser
                |> TestHelper.DefaultState
                |> GitHub.ListTeams "IWDev" "IWDev3-FSharp"
            match x.Content with
            | Content(ys) ->
                Assert.GreaterOrEqual(ys.Length, 1)
            | _ -> Assert.Fail()