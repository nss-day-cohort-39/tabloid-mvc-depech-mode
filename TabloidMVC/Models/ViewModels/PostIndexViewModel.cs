using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostIndexViewModel
    {
        public List<Post> Posts { get; set; }
        public int UserId { get; set; }
        public Post PostModel { get; set; }
    }
}
