using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ContentDAO
    {
        HomeShoppeDB db = null;

        public ContentDAO()
        {
            db = new HomeShoppeDB();
        }

        public Content GetById(long id)
        {
            return db.Contents.Find(id);
        }
    }
}
