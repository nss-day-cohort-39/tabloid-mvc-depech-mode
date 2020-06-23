using System.ComponentModel.DataAnnotations;

namespace TabloidMVC.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Please enter a name for your category...")]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
    }
}