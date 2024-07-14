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
    public class ShoppingCartRepository :Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly AppDbContext _db;

        public ShoppingCartRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ShoppingCart ShoppingCart)
        {
           _db.Update(ShoppingCart);
        }
    }
}
