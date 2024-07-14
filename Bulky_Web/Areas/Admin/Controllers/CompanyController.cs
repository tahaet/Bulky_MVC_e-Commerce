using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = SD.Role_Admin)]
	public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

		public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
		}
        public IActionResult Index()
        {
            
            var companies = _unitOfWork.Company.GetAll().ToList();
          
			
            return View(companies);
        }

        public IActionResult Upsert(int? id)
        {
			Company company = new();

			if (id == null || id == 0)
			{
				//create product
				//ViewBag.CategoryList = CategoryList;
				//ViewData["CoverTypeList"] = CoverTypeList;
				return View(company);
			}
			else
			{
				company = _unitOfWork.Company.Get(c => c.Id == id);
				if (company is null) return NotFound();
				return View(company);

				//update product
			}

		}

        [HttpPost]
        public IActionResult Upsert(Company company)
        {


			if (ModelState.IsValid)
			{
				if (company.Id == 0)
				{
					_unitOfWork.Company.Add(company);
				}
				else
				{
					_unitOfWork.Company.Update(company);
				}
				_unitOfWork.Save();
				var result = company.Id==0?"created": "updated";
				TempData["success"] = $"Company {result} successfully";
				return RedirectToAction("Index");
			}
			return View(company);
		}

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var company = _unitOfWork.Company.Get(p => p.Id == id);
            if (company == null) return NotFound();
            _unitOfWork.Company.Remove(company);
            _unitOfWork.Save();
            TempData["success"] = "Company was deleted successfully";
            return RedirectToAction("Index");
        }
        #region Api Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var companies = _unitOfWork.Company.GetAll();
            return Json(new { data = companies });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var company = _unitOfWork.Company.Get(p => p.Id == id);
            if (company == null) return NotFound();
            
            _unitOfWork.Company.Remove(company);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Company was deleted successfully" });
        }
        #endregion
    }
}