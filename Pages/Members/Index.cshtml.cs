using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Models;

namespace ProjectManagementApp.Pages.Members
{
    public class IndexModel : PageModel
    {
        private readonly ProjectManagementApp.Models.WebAppContext _context;

        public IndexModel(ProjectManagementApp.Models.WebAppContext context)
        {
            _context = context;
        }

        public IList<Member> Member { get;set; }

        public async Task OnGetAsync()
        {
            Member = await _context.Members.ToListAsync();
        }
    }
}
