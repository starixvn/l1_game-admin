using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Account.Results
{
    public class AddUsersResult
    {
        public AddUsersResult(string username, string name, string password)
        {
            Username = username;
            Name = name;
            Password = password;
        }

        public string Username { get; }

        public string Name { get; }

        public string Password { get; }

        public string Error { get; set; }

        public bool IsSuccess => string.IsNullOrWhiteSpace(Error);
    }
}
