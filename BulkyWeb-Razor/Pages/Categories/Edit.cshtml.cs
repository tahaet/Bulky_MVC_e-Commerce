using BulkyWeb_Razor.Data;
using BulkyWeb_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWeb_Razor.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
		private readonly AppDbContext db;

		public Category? Category { get; set; }
        public EditModel(AppDbContext db)
        {
			this.db = db;
		}
        public void OnGet(int? id)
        {
            if(id!=null && id!=0)
            {
                Category = db.Categories.Find(id);
            }
        }
        public IActionResult OnPost()
        {
            if(ModelState.IsValid)
            {
               
                db.Categories.Update(Category);
                db.SaveChanges();
				TempData["success"] = "Category was edited successfully";
				return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
