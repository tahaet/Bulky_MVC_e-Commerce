using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext db;

        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }

		public IOrderDetailRepository OrderDetail { get; private set; }

		public IOrderHeaderRepository OrderHeader { get; private set; }
		public IProductImageRepository ProductImage { get; private set; }

		public UnitOfWork(AppDbContext db)
        {
            this.db = db;
            Category = new CategoryRepository(this.db);
            Product = new ProductRepository(this.db);
            Company = new CompanyRepository(this.db);
            ShoppingCart = new ShoppingCartRepository(this.db);
            ApplicationUser = new ApplicationUserRepository(this.db);
            OrderDetail = new OrderDetailRepository(this.db);
            OrderHeader = new OrderHeaderRepository(this.db);
            ProductImage = new ProductImageRepository(this.db);

        }
        public void Save()
        {
            db.SaveChanges();
        }
    }
}
