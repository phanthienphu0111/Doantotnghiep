﻿using BS_Project.DAO;
using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BS_Project.Areas.Store.Controllers
{
    public class PublisherController : Controller
    {
        // GET: Store/Publisher
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new PublisherDAO();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                var dao = new PublisherDAO();
                int result = dao.Insert(publisher);
                if (result > 0)
                {
                    return RedirectToAction("Index", "Publisher");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm Category không thành công");
                }
            }
            return View("Index");
        }

        public ActionResult Edit(int id)
        {
            var publisherDetail = new PublisherDAO().ViewDetail(id);
            return View(publisherDetail);
        }

        [HttpPost]
        public ActionResult Edit(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                var dao = new PublisherDAO();
                var result = dao.Update(publisher);
                if (result)
                {
                    return RedirectToAction("Index", "Publisher");
                }
                else
                {
                    ModelState.AddModelError("", "Publisher không cập nhật thành công");
                }
            }
            return View("Index");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var result = new PublisherDAO().Delete(id);
            return RedirectToAction("Index", "Publisher");
        }
    }
}