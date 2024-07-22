using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = SD.Role_Admin)]
	public class UserController : Controller
    {
        private readonly AppDbContext _db;
		private readonly UserManager<IdentityUser> _userManager;
		public UserController(AppDbContext db,UserManager<IdentityUser> userManager)
        {
			_userManager = userManager;
            _db = db;
		}
        public IActionResult Index()
        {
            return View();
        }
		public IActionResult RoleManagement(string UserId)
		{
			UserVM UserVM = new()
			{
				ApplicationUser = _db.ApplicationUsers.Include(x => x.Company).FirstOrDefault(x => x.Id == UserId),
				Roles = _db.Roles.Select(r => new SelectListItem
				{
					Text = r.Name,
					Value = r.Name,
				}),
				Companies = _db.Companies.Select(x => new SelectListItem
				{
					Text = x.Name,
					Value = x.Id.ToString()
				})
			};
			UserVM.ApplicationUser.Role = _db.UserRoles.Where(u=>u.UserId==UserId).Join(_db.Roles,u=>u.RoleId,x=>x.Id ,(u,x)=> x.Name).FirstOrDefault();
			return View(UserVM);
		}
		[HttpPost]
		public IActionResult RoleManagement(UserVM userVM)
		{
			var RoleId = _db.UserRoles.FirstOrDefault(x => x.UserId == userVM.ApplicationUser.Id).RoleId;
			var oldRole = _db.Roles.FirstOrDefault(x => x.Id == RoleId).Name;
			var applicationUser = _db.ApplicationUsers.FirstOrDefault(x => x.Id == userVM.ApplicationUser.Id);
			if (userVM.ApplicationUser.Role.ToLower()!= oldRole.ToLower())
			{
				if(userVM.ApplicationUser.Role ==SD.Role_Company)
				{
					applicationUser.CompanyId =userVM.ApplicationUser.CompanyId; 
				}
				if(oldRole == SD.Role_Company)
				{
					applicationUser.CompanyId = null;
				}
				_userManager.RemoveFromRoleAsync(applicationUser,oldRole).GetAwaiter().GetResult();
				_userManager.AddToRoleAsync(applicationUser,userVM.ApplicationUser.Role).GetAwaiter().GetResult();

			}
			else
			{
				if (userVM.ApplicationUser.CompanyId != applicationUser.CompanyId) applicationUser.CompanyId = userVM.ApplicationUser.CompanyId;
			}
				_db.SaveChanges();
			
			return RedirectToAction(nameof(Index));

		}

		#region Api Calls
		[HttpGet]
        public IActionResult GetAll()
        {
			var userData = _db.ApplicationUsers.Include(u => u.Company).Join(_db.UserRoles,
			user => user.Id,
			userRole => userRole.UserId,
			(user, userRole) => new { user, userRole.RoleId }).Join(_db.Roles,
			userRole => userRole.RoleId,
			role => role.Id,
			(userRole, role) => new
			{
				userRole.user,
				RoleName = role.Name,
				Company = userRole.user.Company ?? new Company { Name = "" }
			}).Select(u => new
			{
				u.user.Id,
				u.user.PhoneNumber,
				u.user.Email,
				u.user.StreetAddress,
				u.user.City,
				u.user.State,
				u.user.PostalCode,
				u.user.Name,
				Role = u.RoleName,
				u.Company,
				u.user.LockoutEnd
			}).ToList();

			return Json(new { data = userData });
		}
        [HttpPost]
        public IActionResult LockUnlock([FromBody]string? id)
        {
           var user = _db.ApplicationUsers.FirstOrDefault(u=>u.Id == id);
			if(user is  null)
			{
				return Json(new { success = false, message = "Error While Locking/Unlocking" });

			}
			if (user.LockoutEnd is not null && user.LockoutEnd > DateTime.Now)
			{
				user.LockoutEnd = DateTime.Now;
			}
			else user.LockoutEnd = DateTime.Now.AddYears(1000);
			_db.SaveChanges();
            return Json(new { success = true, message = "Locked/Unlocked Successfully" });
        }
        #endregion
    }
}