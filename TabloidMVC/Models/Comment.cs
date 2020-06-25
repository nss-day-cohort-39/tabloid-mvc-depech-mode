using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a subject for your comment...")]
        [StringLength(255, MinimumLength = 1)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please enter some content for your comment...")]
        public string Content { get; set; }

        [DisplayName("Created")]
        //[DataType(DataType.Date)]
        public DateTime CreateDateTime { get; set; }
        public int PostId { get; set; }
        public string DisplayName { get; set; }
        public int UserProfileId { get; set; }

    }
}
