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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly AppDbContext _db;

        public OrderDetailRepository(AppDbContext db):base(db) 
        {
            _db = db;
        }
       
        public void Update(OrderDetail orderDetail)
        {
            _db.Update(orderDetail);
        }

    }
}
