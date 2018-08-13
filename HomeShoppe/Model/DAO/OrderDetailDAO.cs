using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class OrderDetailDAO
    {
        Model1 db = null;

        public OrderDetailDAO()
        {
            db = new Model1();
        }

        public bool Insert(OrderDetail detail)
        {
            try {
                db.OrderDetails.Add(detail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
