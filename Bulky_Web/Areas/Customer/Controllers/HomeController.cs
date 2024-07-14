using Bulky.Models;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var products = unitOfWork.Product.GetAll();
            return View(products);
        }
        public IActionResult Details(int id)
        {
            ShoppingCart shoppingCart = new()
            {
                Product = unitOfWork.Product.Get(p => p.Id == id, includeProperties: "Category"),
                Count = 1,
                ProductId = id
            };
            return View(shoppingCart);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;
            var cartFromDb = unitOfWork.ShoppingCart.Get(s=>s.ApplicationUserId == userId && s.ProductId ==shoppingCart.ProductId );
            if(cartFromDb == null)
            {
                shoppingCart.Id = 0;
                unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            else
            {
                cartFromDb.Count += shoppingCart.Count;
                unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            unitOfWork.Save();
            TempData["success"] = "Cart Updated Successfully";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
