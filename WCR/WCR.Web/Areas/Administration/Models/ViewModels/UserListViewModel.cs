using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCR.Models;

namespace WCR.Web.Areas.Administration.Models.ViewModels
{
    public class UserListViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string[] Roles { get; set; }

        public static Func<User, UserListViewModel> FromUser
        {
            get
            {
                return user => new UserListViewModel()
                {
                    Id = user.Id,
                    Name = $"{user.UserName} ({user.Email})"
                };
            }
        }
    }
}
