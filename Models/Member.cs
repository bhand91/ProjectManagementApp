using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApp.Models
{
    public class Member
    {
        public int MemberID {get; set;}

        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName {get; set;}

        [StringLength(50, ErrorMessage = "last name cannot be longer than 50 characters.")]
        [Display(Name = "Last Name")]
        public string LastName {get; set;}

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }

        }

        public List<ProjectMember> ProjectMembers {get; set;}
    }
}