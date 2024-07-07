using Bulky.DataAccess.Repository;
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository :Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product product)
        {
            Product productFromDB = _db.Products.Find(product.Id)!;
            if(productFromDB is not null)
            {
                productFromDB.ISBN=product.ISBN;
                productFromDB.Title = product.Title;
                productFromDB.Author=product.Author;
                productFromDB.Description=product.Description;
                productFromDB.Category=product.Category;
                productFromDB.Price=product.Price;
                productFromDB.Price50=product.Price50;
                productFromDB.Price100=product.Price100;
                productFromDB.ListPrice=product.ListPrice;
                if(product.ImageUrl is not null)
                {
                    productFromDB.ImageUrl=product.ImageUrl;
                }
            }
        }
    }
}
