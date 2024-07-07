using BulkyWeb_Razor.Data;
using BulkyWeb_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWeb_Razor.Pages.categories
{
    public class IndexModel : PageModel
    {
		private readonly AppDbContext _db;
        public List<Category> Categories { get; set; }
		public IndexModel(AppDbContext db)
        {
			_db = db;
		}
        public void OnGet()
        {
            Categories=_db.Categories.ToList();
        }
    }
}
