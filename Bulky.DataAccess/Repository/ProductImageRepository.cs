using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using BulkyBook.DataAccess;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        private readonly AppDbContext _db;

        public ProductImageRepository(AppDbContext db):base(db) 
        {
            _db = db;
        }
       
        public void Update(ProductImage productImage)
        {
            _db.Update(productImage);
        }

    }
}
