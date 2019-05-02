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
    public class IndexModel : PageModel
    {
        private readonly ProjectManagementApp.Models.WebAppContext _context;

        public IndexModel(ProjectManagementApp.Models.WebAppContext context)
        {
            _context = context;
        }

        public IList<ProjectMember> ProjectMember { get;set; }

        public async Task OnGetAsync()
        {
            ProjectMember = await _context.ProjectMembers
                .Include(p => p.Member)
                .Include(p => p.Project).ToListAsync();
        }
    }
}
