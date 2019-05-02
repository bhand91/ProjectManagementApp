using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApp.Models
{
    public class Project
    {
        public int ProjectID {get; set;}

        [Display(Name = "Project Name")]
        public string ProjectName {get; set;}
        
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate {get; set;}
        
        [Display(Name= "Project Description")]
        public string ProjectDes {get; set;}
        public List<ProjectMember> ProjectMembers {get; set;}
    }
}