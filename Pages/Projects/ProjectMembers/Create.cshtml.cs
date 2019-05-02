using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementApp.Models;

namespace ProjectManagementApp.Pages.ProjectMembers
{
    public class CreateModel : PageModel
    {
        private readonly ProjectManagementApp.Models.WebAppContext _context;

        public CreateModel(ProjectManagementApp.Models.WebAppContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "MemberID");
        ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectID");
            return Page();
        }

        [BindProperty]
        public ProjectMember ProjectMember { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ProjectMembers.Add(ProjectMember);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}