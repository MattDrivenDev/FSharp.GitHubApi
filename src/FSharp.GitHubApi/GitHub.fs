module GitHub

    let GetRateLimit = 
        ApiHelper.RateLimit

    let GetUser = 
        Users.Get

    let UpdateUser = 
        Users.Update