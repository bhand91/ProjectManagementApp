using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApp.Models
{
    public class Project
    {
        public int ProjectID {get; set;}

        [Display(Name = "Project Name")]
        [StringLength(50, ErrorMessage = "Project name cannot be longer than 50 characters.")]

        public string ProjectName {get; set;}
        
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate {get; set;}
        
        [StringLength(160, ErrorMessage = "Project Description cannot be longer than 50 characters.")]
        [Display(Name= "Project Description")]
        public string ProjectDes {get; set;}
        public List<ProjectMember> ProjectMembers {get; set;}
    }
}