using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ExceptionMessages
{
    public static class UserExceptionMessages
    {
        public const string NotFoundUserById = "No user with this id";

        public const string NotFoundUserByUsername = "No user with this username";

        public const string UsernameAlreadyExsist = "This username is already exist";

        public const string IncorrectPassword = "Password is not correct";

        public const string AlreadyHaveProfilePicture = "User already have profile picture";

        public const string DoNotHaveProfilePicture = "User don't have profile picture";
    }
}
