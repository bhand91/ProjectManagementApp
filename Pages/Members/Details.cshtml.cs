using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace ProjectManagementApp.Pages.Members
{
    public class DetailsModel : PageModel
    {
        private readonly ProjectManagementApp.Models.WebAppContext _context;
        private readonly ILogger _log;

        public DetailsModel(ProjectManagementApp.Models.WebAppContext context, ILogger<DetailsModel> log)
        {
            _context = context;
            _log = log;
        }

        public Member Member { get; set; }
        public List<Project> AllProjects {get; set;}
        public SelectList ProjectDropDown {get; set;}

        [BindProperty]
        [Display(Name = "Project")]
        public int ProjectIdToAdd {get; set;}

        [BindProperty]
        public int ProjectIdToDelete {get; set;}

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Member = await _context.Members.Include(pm => pm.ProjectMembers).ThenInclude(p => p.Project).FirstOrDefaultAsync(m => m.MemberID == id);
            AllProjects = await _context.Projects.ToListAsync();
            ProjectDropDown = new SelectList(AllProjects, "ProjectID", "ProjectName");

            if (Member == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteProjectAsync(int? id)
        {
            _log.LogWarning($"OnPost: MemberId {id}, DROP course {ProjectIdToDelete}");
            if (id == null)
            {
                return NotFound();
            }

            Member = await _context.Members.Include(pm => pm.ProjectMembers).ThenInclude(p => p.Project).FirstOrDefaultAsync(m => m.MemberID == id);
            AllProjects = await _context.Projects.ToListAsync();
            ProjectDropDown = new SelectList(AllProjects, "ProjectID", "ProjectName");
            
            if (Member == null)
            {
                return NotFound();
            }

            ProjectMember projectToDrop = _context.ProjectMembers.Find(ProjectIdToDelete, id.Value);

            if (projectToDrop != null)
            {
                _context.Remove(projectToDrop);
                _context.SaveChanges();
            }
            else
            {
                _log.LogWarning("Member not part of project");
            }

            return RedirectToPage(new {id = id});
        }


        public async Task<IActionResult> OnPostAsync(int? id)
        {
            _log.LogWarning($"OnPost: MemberId {id}, ADD project {ProjectIdToAdd}");
            if (ProjectIdToAdd == 0)
            {
                ModelState.AddModelError("ProjectIdToAdd", "This field is a required field.");
                return Page();
            }
            if (id == null)
            {
                return NotFound();
            }

            Member = await _context.Members.Include(pm => pm.ProjectMembers).ThenInclude(m => m.Member).FirstOrDefaultAsync(m => m.MemberID == id);            
            AllProjects = await _context.Projects.ToListAsync();
            ProjectDropDown = new SelectList(AllProjects, "ProjectID", "ProjectName");

            if (Member == null)
            {
                return NotFound();
            }

            if (!_context.ProjectMembers.Any(pm => pm.ProjectID == ProjectIdToAdd && pm.MemberID == id.Value))
            {
                ProjectMember projectToAdd = new ProjectMember { MemberID = id.Value, ProjectID = ProjectIdToAdd};
                _context.Add(projectToAdd);
                _context.SaveChanges();
            }
            else
            {
                _log.LogWarning("Member already in project");
            }

            return RedirectToPage(new {id = id});
        }
    }
}
