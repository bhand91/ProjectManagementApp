using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Models;

namespace ProjectManagementApp.Pages.ProjectMembers
{
    public class DeleteModel : PageModel
    {
        private readonly ProjectManagementApp.Models.WebAppContext _context;

        public DeleteModel(ProjectManagementApp.Models.WebAppContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProjectMember ProjectMember { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProjectMember = await _context.ProjectMembers
                .Include(p => p.Member)
                .Include(p => p.Project).FirstOrDefaultAsync(m => m.ID == id);

            if (ProjectMember == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProjectMember = await _context.ProjectMembers.FindAsync(id);

            if (ProjectMember != null)
            {
                _context.ProjectMembers.Remove(ProjectMember);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
