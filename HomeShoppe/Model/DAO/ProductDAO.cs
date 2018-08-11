using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ProductDAO
    {
        Model1 db = null;

        public ProductDAO()
        {
            db = new Model1();
        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }

        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreateDate).Take(top).ToList(); 
        }

        public List<Product> ListRelatedProducts(long productId)
        {
            var product = db.Products.Find(productId);
            return db.Products.Where(x => x.ID != productId && x.CategoryID==product.CategoryID).ToList();
        }

        public List<Product> ListByCategoryID(long categoryID, ref int totalRecord, int pageIndex =1, int pageSize = 1)
        {
            totalRecord = db.Products.Where(x => x.CategoryID == categoryID).Count();
            var model = db.Products.Where(x => x.CategoryID == categoryID).OrderByDescending(x=>x.CreateDate).Skip((pageIndex - 1)* pageSize).Take(pageSize).ToList();
            return model;
        }
        public Product ViewDetail(long id) 
        {
            return db.Products.Find(id);
        }
    }
}
