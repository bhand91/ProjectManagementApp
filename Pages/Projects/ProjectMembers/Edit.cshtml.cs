using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Models;

namespace ProjectManagementApp.Pages.ProjectMembers
{
    public class EditModel : PageModel
    {
        private readonly ProjectManagementApp.Models.WebAppContext _context;

        public EditModel(ProjectManagementApp.Models.WebAppContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProjectMember ProjectMember { get; set; }
        public SelectList EnumDropDown {get; set;}

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProjectMember = await _context.ProjectMembers
                .Include(p => p.Member)
                .Include(p => p.Project).FirstOrDefaultAsync(m => m.ID == id);

            ProjectMember = await _context.ProjectMembers.Include(m => m.Member).Include(p => p.Project).FirstOrDefaultAsync(pm => pm.ID == id);
            var roleList = _context.ProjectMembers.Select( p => new {ID = p.MemberRole, Display = string.Format($"{p.MemberRole}")}).Select(p => p);
            EnumDropDown = new SelectList(await roleList.Distinct().ToListAsync(), "ID", "Display");
            
            if (ProjectMember == null)
            {
                return NotFound();
            }
           ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "MemberID");
           ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectID");
            ViewData["MemberRole"] = new SelectList(_context.ProjectMembers, "MemberRole", "MemberRole");
                        
            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ProjectMember).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectMemberExists(ProjectMember.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ProjectMemberExists(int id)
        {
            return _context.ProjectMembers.Any(e => e.ID == id);
        }
    }
}
