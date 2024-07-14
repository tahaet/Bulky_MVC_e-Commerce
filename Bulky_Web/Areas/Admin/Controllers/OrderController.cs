using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin)]
	public class OrderController:Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public OrderController(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
		{
			return View();
		}
        #region Api Calls
        [HttpGet]
		public IActionResult GetAll()
		{
			var orders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
			return Json(new { data = orders });
		}
		
		#endregion
	}
}
