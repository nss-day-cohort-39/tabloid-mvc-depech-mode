using Microsoft.Data.SqlClient.Server;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;

namespace TabloidMVC.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DisplayName("Username")]
        public string DisplayName { get; set; }
        public string Email { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        [DisplayName("Created")]
        public DateTime CreateDateTime { get; set; }
        public string ImageLocation { get; set; }
        public int UserTypeId { get; set; }
        [DisplayName("User Type")]
        public UserType UserType { get; set; }
        public bool Active { get; set; }
        [DisplayName("Full Name")]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}