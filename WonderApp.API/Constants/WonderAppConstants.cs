using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WonderApp.Constants
{
    public static class WonderAppConstants
    {
        public const int DefaultRadius = 100;
        public const int DefaultMaxNumberOfWonders = 20;
        public const string Facebook = "facebook";
        public const string UserAlreadyRegisted = "User is registered with different facebook credentials";
        public const string CreateUserError = "Error creating user";
        public const string CreateFacebookDetailsError = "Error adding Facebook details";
        public const string FacebookTokenUrl = "https://graph.facebook.com/me?access_token=";
    }
}