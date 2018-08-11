using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class MenuDAO
    {
        Model1 db = null;

        public MenuDAO()
        {
            db = new Model1();
        }

        public List<Menu> ListByGroupID(int groupId)
        {
            return db.Menus.Where(x => x.TypeID == groupId && x.Status== true).OrderBy(x=>x.DisplayOrder).ToList();
        }
    }
}
