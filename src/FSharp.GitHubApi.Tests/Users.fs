namespace FSharp.GitHubApi.Users

    open NUnit.Framework

    [<TestFixture>]
    [<Category("FirstPass")>]
    type Users() =

        [<Test>]
        member this.``should be able to get a specified user``() =
            let getQuery = Users.SpecificUser("saxonmatt")
            let user = 
                TestHelper.AuthenticatedUser
                |> TestHelper.DefaultState
                |> Users.Get getQuery
            printfn "%s" user.ErrorMessage
            Assert.AreEqual(200, user.StatusCode)
            Assert.IsTrue(user.Content.Login.Equals("saxonmatt"))
            Assert.IsTrue(user.Content.Name.Equals("Matt Ball"))
            Assert.IsTrue(user.Content.Blog.Contains("www.saxonmatt.co.uk"))