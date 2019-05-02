using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApp.Models
{
    public class Member
    {
        public int MemberID {get; set;}

        [Display(Name = "First Name")]
        public string FirstName {get; set;}

        [Display(Name = "Last Name")]
        public string LastName {get; set;}

        public List<ProjectMember> ProjectMembers {get; set;}
    }
}