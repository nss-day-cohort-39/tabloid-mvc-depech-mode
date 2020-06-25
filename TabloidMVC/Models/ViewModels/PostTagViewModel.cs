using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostTagViewModel
    {
        public Post Post { get; set; }

        public List<Tag> TagList { get; set; } //list of all possible tags
        public List<Tag> Tags { get; set; } //list of all the tags for this particular post
        public string TagString { get; set; } //list of tags as a string separated by commas
    }
}
