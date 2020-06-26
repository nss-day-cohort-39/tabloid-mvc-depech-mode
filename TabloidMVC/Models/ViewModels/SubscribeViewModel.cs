using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class SubscribeViewModel
    {
        public int SubscriberUserId { get; set; }

        public int ProviderUserId { get; set; }

        public int PostId { get; set; }
        public UserProfile ProviderUserProfile { get; set; }
    }
}
