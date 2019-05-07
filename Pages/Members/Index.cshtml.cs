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

        public PaginatedList<Member> Member { get;set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public string LastNameSort {get; set;}
        public string FirstNameSort {get; set;}
        public string CurrentFilter {get; set;}
        public string CurrentSort {get; set;}
        public async Task OnGetAsync(string sortOrder, string searchString, string currentFilter, int? pageIndex)
        {
            CurrentSort = sortOrder;
            LastNameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            CurrentFilter = searchString;

            if(searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Member> memberQuery = from m in _context.Members
                                                select m;
            if(!string.IsNullOrEmpty(searchString))
            {
                memberQuery = memberQuery.Where(m => m.FirstName.ToUpper().Contains(SearchString.ToUpper()) || m.LastName.ToUpper().Contains(SearchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    memberQuery = memberQuery.OrderByDescending(m => m.LastName);
                    break;
                default:
                    memberQuery = memberQuery.OrderBy(m => m.LastName);
                    break;
            }

            int pageSize = 10;

            Member = await PaginatedList<Member>.CreateAsync(memberQuery.Include(pm => pm.ProjectMembers).ThenInclude(p => p.Project).AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
