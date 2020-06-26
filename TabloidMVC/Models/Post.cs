using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TabloidMVC.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a title for your post...")]
        [StringLength(255, MinimumLength = 1)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter some content for your post...")]
        public string Content { get; set; }

        [DisplayName("Header Image URL")]
        [StringLength(255, MinimumLength = 1)]
        public string ImageLocation { get; set; }

        [DisplayName("Created")]
        [DataType(DataType.DateTime)]
        public DateTime CreateDateTime { get; set; }

        [DisplayName("Published")]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime? PublishDateTime { get; set; }

        public bool IsApproved { get; set; }

        
        [DisplayName("Category")]
        [Required(ErrorMessage = "Please select a category...")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [DisplayName("Author")]
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        public List<Tag> Tags { get; set; }

        public bool IsSubscribed { get; set; }
    }
}
