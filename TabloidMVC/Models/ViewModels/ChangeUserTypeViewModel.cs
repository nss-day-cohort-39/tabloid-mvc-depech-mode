using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class ChangeUserTypeViewModel
    {
        public UserProfile User { get; set; }
        public Exception Exception { get; set; }
    }
}
