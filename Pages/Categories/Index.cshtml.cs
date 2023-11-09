using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Negru_Luminita_Lab2.Data;
using Negru_Luminita_Lab2.Models;
using Negru_Luminita_Lab2.Models.ViewModels;

namespace Negru_Luminita_Lab2.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly Negru_Luminita_Lab2.Data.Negru_Luminita_Lab2Context _context;

        public IndexModel(Negru_Luminita_Lab2.Data.Negru_Luminita_Lab2Context context)
        {
            _context = context;
        }
        public IList<Category> Category { get; set; } = default!;
        public CategoryIndexData CategoryData { get; set; }
        public int CategoryID { get; set; }
        public int BookID { get; set; }
        public async Task OnGetAsync(int? id, int? bookID)
        {
            CategoryData = new CategoryIndexData();

            CategoryData.Categories = await _context.Category
            .Include(i => i.BookCategories)
            .ThenInclude(c => c.Book)
            .ThenInclude(b => b.Author)
            .OrderBy(i => i.CategoryName)
            .ToListAsync();

            if (CategoryData.Categories == null)
            {
                CategoryData.Categories = new List<Category>();
            }

            if (id != null)
            {
                CategoryID = id.Value;
                Category category = CategoryData.Categories
                .Where(i => i.ID == id.Value).SingleOrDefault();

                if (category != null && category.BookCategories != null)
                {
                    CategoryData.Books = category.BookCategories;
                }
                else
                {
                    CategoryData.Books = new List<BookCategory>();
                }
            }
            else
            {
                // Dacă nu este specificat un ID, preluați toate editurile.
                Category = await _context.Category.ToListAsync();

                if (Category == null)
                {
                    Category = new List<Category>();
                }
            }
        }

    }
}
