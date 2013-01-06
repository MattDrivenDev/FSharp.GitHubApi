#FSharp.GitHubApi#

An F# implementation of a [GitHub Api](http://developer.github.com/) wrapper.

##Progress##

The Api current supports the following:

* [Authentication (Basic)](http://developer.github.com/v3/#authentication)
* [Rate Limit](http://developer.github.com/v3/#rate-limiting)
* [Get a single user](http://developer.github.com/v3/users/#get-a-single-user)
* [Get the authenticated user](http://developer.github.com/v3/users/#get-the-authenticated-user)
* [Update the authenticated user](http://developer.github.com/v3/users/#update-the-authenticated-user)
* [Get all users](http://developer.github.com/v3/users/#get-all-users)

##Notes##

Originally intended to just be part of [OctocatTools](http://github.com/saxonmatt/OctocatTools/) - I've decided to move it out to be a separate repository and just use it as a submodule... and perhaps one day as a NuGet package or whatever.

##Development##

1. Run the `ResolveDependencies.bat` before development.
2. Edit the `TestSettings.fs` to include some valid GitHub credentials **NOTE: Do not ever commit this file and push back to GitHub**
3. Run the `build.bat` file to build/test the code base.
