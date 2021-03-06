﻿using HomeShoppe.Common;
using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;
using System.Web.Mvc;

namespace HomeShoppe.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        #region method
        public ActionResult Index(string searchString, int page = 1, int sizePage = 10)
        {
            var dao = new UserDAO();
            var model = dao.ListAllPaging(searchString, page, sizePage);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var user = new UserDAO().ViewDetail(id);
            return View(user);
        }

        #endregion

        #region httpGet
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        #endregion

        #region httpPost
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDAO();
                var encrytorMD5Pass = Encrytor.MD5Hash(user.Password);
                user.Password = encrytorMD5Pass;
                long id = dao.Insert(user);
                if (id > 0)
                {
                    SetAlert("Thêm user thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm user mới không  thành công");
                }
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult Edit(User user)
       {
            if (ModelState.IsValid)
            {
                var dao = new UserDAO();
                if (!string.IsNullOrEmpty(user.Password))
                {
                    var encrytorMD5Pass = Encrytor.MD5Hash(user.Password);
                    user.Password = encrytorMD5Pass;
                }
                
                var  result= dao.Update(user);
                if (result)
                {
                    SetAlert("Cập nhật user thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật user không thành công");
                }
            }
            return View("Index");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new UserDAO().Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new UserDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        #endregion

    }
}