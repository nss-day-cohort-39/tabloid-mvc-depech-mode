using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class UserListViewModel
    {
        public List<UserProfile> Users { get; set; }
        public UserProfile User { get; set; }
        public bool Deactivated { get; set; }
    }
}
