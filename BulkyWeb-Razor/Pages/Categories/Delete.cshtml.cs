using BulkyWeb_Razor.Data;
using BulkyWeb_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWeb_Razor.Pages.Categories
{
	[BindProperties]
    public class DeleteModel : PageModel
    {
		private readonly AppDbContext db;

		public Category Category { get; set; }
		public DeleteModel(AppDbContext db)
		{
			this.db = db;
		}
		public void OnGet(int? id)
		{
			if (id != null && id != 0)
				Category = db.Categories.Find(id);
		}
		public IActionResult OnPost()
		{
			var category = db.Categories.Find(Category.Id);
			if (category == null) return NotFound();
			db.Categories.Remove(category);
			db.SaveChanges();
			TempData["success"] = "Category was deleted successfully";

			return RedirectToPage("Index");
		}
	}
}
