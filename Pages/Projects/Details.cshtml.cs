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

namespace ProjectManagementApp.Pages.Projects
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

        public Project Project { get; set; }
        public List<Member> Members {get; set;}

        public SelectList MemberDropDown {get; set;}
        
        [BindProperty]
        [Display(Name = "Member")]
        //[Required]
        public int MemberIdToAdd {get; set;}
        
        [BindProperty]
        public int MemberIdToDelete {get; set;}

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Project = await _context.Projects.Include(pm => pm.ProjectMembers).ThenInclude(m => m.Member).FirstOrDefaultAsync(m => m.ProjectID == id);
            var memberList = _context.Members.Select( p => new {ID = p.MemberID, Display = string.Format($"{p.FirstName} {p.LastName}")}).Select(p => p);
            MemberDropDown = new SelectList(await memberList.Distinct().ToListAsync(), "ID", "Display");

            if (Project == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteMemberAsync(int? id)
        {
            _log.LogWarning($"OnPost: ProjectID {id}, DROP member {MemberIdToDelete}");
            if(id == null)
            {
                return NotFound();
            }

            Project = await _context.Projects.Include(pm => pm.ProjectMembers).ThenInclude(m => m.Member).FirstOrDefaultAsync(m => m.ProjectID == id);

            if(Project == null)
            {
                return NotFound();
            }

            ProjectMember memberToDrop = _context.ProjectMembers.Find(MemberIdToDelete);

            if (memberToDrop != null)
            {
                _context.Remove(memberToDrop);
                _context.SaveChanges();
            }
            else
            {
                _log.LogWarning("Member NOT assigned project");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            _log.LogWarning($"OnPost: ProjectID {id}, ADD members {MemberIdToAdd}");
            if(id == null)
            {
                return NotFound();
            }
        Project = await _context.Projects.Include(pm => pm.ProjectMembers).ThenInclude(m => m.Member).FirstOrDefaultAsync(m => m.ProjectID == id);
        var memberList = _context.Members.Select( p => new {ID = p.MemberID, Display = string.Format($"{p.FirstName} {p.LastName}")}).Select(p => p);
        MemberDropDown = new SelectList(await memberList.Distinct().ToListAsync(), "ID", "Display");
        

        if(!_context.ProjectMembers.Any(pm => pm.MemberID == MemberIdToAdd && pm.ProjectID == id.Value))
        {
            ProjectMember memberToAdd = new ProjectMember {ProjectID = id.Value, MemberID = MemberIdToAdd};
            _context.Add(memberToAdd);
            _context.SaveChanges();
        }
        else
        {
            _log.LogWarning("Project already has this member");
        }
        MemberIdToAdd = 0;
        return RedirectToPage(new {id = id});

        }
    }
}
