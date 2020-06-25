﻿using Microsoft.Data.SqlClient.Server;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;

namespace TabloidMVC.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [DisplayName("Username")]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
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