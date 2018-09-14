using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using PagedList;
using System.Web;

namespace Model.DAO
{
    public class ContentDAO
    {
        HomeShoppe db = null;
        public static string USER_SESSION = "USER_SESSION";
        public ContentDAO()
        {
            db = new HomeShoppe();
        }

        public IEnumerable<Content> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }

            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }
        /// <summary>
        /// List all content for client
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Content> ListAllPaging(int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }
        public IEnumerable<Content> ListAllByTag(string tag, int page, int pageSize)
        {
            var model = (from a in db.Contents
                         join b in db.ContentTabs
                         on a.ID equals b.ContentID
                         where b.TabID == tag
                         select new
                         {
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Image = a.Image,
                             Description = a.Description,
                             CreatedDate = a.CreateDate,
                             CreatedBy = a.CreateBy,
                             ID = a.ID

                         }).AsEnumerable().Select(x => new Content()
                         {
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Image = x.Image,
                             Description = x.Description,
                             CreateDate = x.CreatedDate,
                             CreateBy = x.CreatedBy,
                             ID = x.ID
                         });
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }

        public Content GetByID(long id)
        {
            return db.Contents.Find(id);
        }

        public Tab GetTag(string id)
        {
            return db.Tabs.Find(id);
        }

        public long Create(Content content)
        {
            //Xử lý alias
            if (string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }
            content.CreateDate = DateTime.Now;
            //content.ViewCount = 0;
            db.Contents.Add(content);
            db.SaveChanges();

            //Xử lý tag
            if (!string.IsNullOrEmpty(content.Tabs))
            {
                string[] tags = content.Tabs.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId);

                    //insert to to tag table
                    if (!existedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }

                    //insert to content tag
                    this.InsertContentTag(content.ID, tagId);

                }
            }

            return content.ID;
        }
        public long Edit(Content content)
        {
            //Xử lý alias
            if (string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }
            content.CreateDate = DateTime.Now;
            db.SaveChanges();

            //Xử lý tag
            if (!string.IsNullOrEmpty(content.Tabs))
            {
                this.RemoveAllContentTag(content.ID);
                string[] tags = content.Tabs.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId);

                    //insert to to tag table
                    if (!existedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }

                    //insert to content tag
                    this.InsertContentTag(content.ID, tagId);

                }
            }

            return content.ID;
        }
        public void RemoveAllContentTag(long contentId)
        {
            db.ContentTabs.RemoveRange(db.ContentTabs.Where(x => x.ContentID == contentId));
            db.SaveChanges();
        }
        public void InsertTag(string id, string name)
        {
            var tag = new Tab();
            tag.ID = id;
            tag.Name = name;
            db.Tabs.Add(tag);
            db.SaveChanges();
        }

        public void InsertContentTag(long contentId, string tagId)
        {
            var contentTag = new ContentTab();
            contentTag.ContentID = contentId;
            contentTag.TabID = tagId;
            db.ContentTabs.Add(contentTag);
            db.SaveChanges();
        }
        public bool CheckTag(string id)
        {
            return db.Tabs.Count(x => x.ID == id) > 0;
        }

        public List<Tab> ListTag(long contentId)
        {
            var model = (from a in db.Tabs
                         join b in db.ContentTabs
                         on a.ID equals b.TabID
                         where b.ContentID == contentId
                         select new
                         {
                             ID = b.TabID,
                             Name = a.Name
                         }).AsEnumerable().Select(x => new Tab()
                         {
                             ID = x.ID,
                             Name = x.Name
                         });
            return model.ToList();
        }
    }
}
