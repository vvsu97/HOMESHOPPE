﻿
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ContentDAO
    {
        HomeShoppe db = null;

        public ContentDAO()
        {
            db = new HomeShoppe();
        }

        public Content GetById(long id)
        {
            return db.Contents.Find(id);
        }
    }
}
