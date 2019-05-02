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
    public class DetailsModel : PageModel
    {
        private readonly ProjectManagementApp.Models.WebAppContext _context;

        public DetailsModel(ProjectManagementApp.Models.WebAppContext context)
        {
            _context = context;
        }

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
    }
}
