using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectManagementApp.Pages.Projects
{
    public class IndexModel : PageModel
    {
        private readonly ProjectManagementApp.Models.WebAppContext _context;

        public IndexModel(ProjectManagementApp.Models.WebAppContext context)
        {
            _context = context;
        }

        public PaginatedList<Project> Project { get;set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public string NameSort{get; set;}
        public string DateSort {get; set;}
        public string CurrentFilter {get; set;}
        public string CurrentSort {get; set;}
        public async Task OnGetAsync(string sortOrder, string searchString, string currentFilter, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
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

            IQueryable<Project> projectQuery = from p in _context.Projects
                                                select p;
            if(!string.IsNullOrEmpty(searchString))
            {
                projectQuery = projectQuery.Where(p => p.ProjectName.ToUpper().Contains(SearchString.ToUpper()) || p.ProjectDes.ToUpper().Contains(SearchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    projectQuery = projectQuery.OrderByDescending(p => p.ProjectName);
                    break;
                case "Date":
                    projectQuery = projectQuery.OrderBy(p => p.DueDate);
                    break;
                case "date_desc":
                    projectQuery = projectQuery.OrderByDescending(p => p.DueDate);
                    break;
                default:
                    projectQuery = projectQuery.OrderBy(p => p.ProjectName);
                    break;
            }

            int pageSize = 10;

            Project = await PaginatedList<Project>.CreateAsync(projectQuery.Include(pm => pm.ProjectMembers).ThenInclude(m => m.Member).AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
