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
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return View(products);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM =
                new()
                {
                    Product = new(),
                    Categories = _unitOfWork
                        .Category.GetAll()
                        .Select(i => new SelectListItem { Text = i.Name, Value = i.Id.ToString() })
                };

            if (id == null || id == 0)
            {
                //create product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.Get(
                    u => u.Id == id,
                    includeProperties: "ProductImages"
                );
                return View(productVM);

                //update product
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, List<IFormFile>? files)
        {
            if (ModelState.IsValid)
            {
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                _unitOfWork.Save();
                string wwwRootPath = webHostEnvironment.WebRootPath;
                if (files != null)
                {
                    foreach (IFormFile file in files)
                    {
                        string fileName =
                            Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var folderPath = $@"images\products\product-{productVM.Product.Id}";
                        var filePath = Path.Combine(wwwRootPath, folderPath);
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        using (
                            var fileStreams = new FileStream(
                                Path.Combine(filePath, fileName),
                                FileMode.Create
                            )
                        )
                        {
                            file.CopyTo(fileStreams);
                        }
                        ProductImage productImage = new ProductImage()
                        {
                            ImageUrl = $@"\{folderPath}\{fileName}",
                            ProductId = productVM.Product.Id
                        };
                        if (productVM.Product.ProductImages is null)
                            productVM.Product.ProductImages = new List<ProductImage>();
                        productVM.Product.ProductImages.Add(productImage);
                    }
                    _unitOfWork.Product.Update(productVM.Product);
                    _unitOfWork.Save();
                }
                var result = productVM.Product.Id == 0 ? "created" : "updated";
                TempData["success"] = $"Product {result} successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.Categories = _unitOfWork
                    .Category.GetAll()
                    .Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() });

                return View(productVM);
            }
        }

        public IActionResult DeleteImage(int ImageId)
        {
            var imageToBeDeleted = _unitOfWork.ProductImage.Get(x => x.Id == ImageId);
            if (imageToBeDeleted is null)
                return NotFound();
            var id = imageToBeDeleted.ProductId;
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldPath = Path.Combine(
                        webHostEnvironment.WebRootPath,
                        imageToBeDeleted.ImageUrl.TrimStart('\\')
                    );
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                _unitOfWork.ProductImage.Remove(imageToBeDeleted);
                _unitOfWork.Save();
                TempData["success"] = "Deleted Successfully";
            }
            return RedirectToAction(nameof(Upsert), new { id = id });
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var product = _unitOfWork.Product.Get(p => p.Id == id);
            if (product == null)
                return NotFound();
            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            TempData["success"] = "Product was deleted successfully";
            return RedirectToAction("Index");
        }

        #region Api Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var product = _unitOfWork.Product.Get(p => p.Id == id);
            if (product == null)
                return NotFound();
            var folderPath = $@"images\products\product-{id}";
            var filePath = Path.Combine(webHostEnvironment.WebRootPath, folderPath);
            if (Directory.Exists(filePath))
            {
                var files = Directory.GetFiles(filePath);
                foreach (var file in files)
                {
                    System.IO.File.Delete(file);
                }
                Directory.Delete(filePath);
            }
            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Product was deleted successfully" });
        }
        #endregion
    }
}
