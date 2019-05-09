using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApp.Models
{
    public enum Role
    {
        Manager, Task_Worker, Researcher, Specialist
    }
    public class ProjectMember
    {
        public int ID {get; set;}
        public int ProjectID {get; set;}
        public int MemberID {get; set;}

        [Display(Name = "Member Role")]
        public Role MemberRole {get; set;}

        [StringLength(160, ErrorMessage = "Assignment Description cannot be longer than 160 characters.")]
        [Display(Name = "Assignment Description")]
        public string AssignmentDes {get; set;}
        public Member Member {get; set;}
        public Project Project {get; set;}
    }
}