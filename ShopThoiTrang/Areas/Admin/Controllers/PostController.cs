using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Models;
using ShopThoiTrang.Library;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class PostController : Controller
    {
        private PostDAO postDAO = new PostDAO();
        private TopicDAO topicDAO = new TopicDAO();

        // GET: Admin/Post
        public ActionResult Index()
        {
            return View(postDAO.getList("Index", "Post"));
        }

        // GET: Admin/Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Admin/Post/Create
        public ActionResult Create()
        {
            ViewBag.ListTopic = new SelectList(topicDAO.getList("Index"), "Id", "Name", 0);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                // Upload file
                var img = Request.Files["img"];
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    // Kiem tra tap tin
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    {
                        // upload hinh
                        string imgName = post.Slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        post.Img = imgName;
                        string PathDir = "~/Public/images/posts/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }
                post.Type = "Post";
                post.Slug = XString.str_slug(post.Title);
                post.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                post.CreatedAt = DateTime.Now;
                postDAO.Insert(post);
                TempData["message"] = new XMessage("success", "Thêm thành công");
                return RedirectToAction("Index", "Post");
            }
            ViewBag.ListTopic = new SelectList(topicDAO.getList("Index"), "Id", "Name", 0);
            return View(post);
        }

        // GET: Admin/Post/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListTopic = new SelectList(topicDAO.getList("Index"), "Id", "Name", 0);
            return View(post);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Post post)
        {
            if (ModelState.IsValid)
            {
                // Upload file
                var img = Request.Files["img"];
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    // Kiem tra tap tin
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    {
                        // upload hinh
                        string imgName = post.Slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        post.Img = imgName;
                        string PathDir = "~/Public/images/posts/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        // Xoa file 
                        if (post.Img.Length > 0)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), post.Img);
                            System.IO.File.Delete(DelPath); // xoa hinh
                        }
                        img.SaveAs(PathFile);
                    }
                }
                post.Type = "Post";
                post.Slug = XString.str_slug(post.Title);
                post.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
                post.UpdatedAt = DateTime.Now;
                postDAO.Update(post);
                TempData["message"] = new XMessage("success", "Cập nhật thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListTopic = new SelectList(topicDAO.getList("Index"), "Id", "Name", 0);
            return View(post);
        }

        // GET: Admin/Post/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Admin/Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = postDAO.getRow(id);
            string PathDir = "~/Public/images/posts/";
            // Xoa hinh anh
            // Xoa file 
            if (post.Img != null)
            {
                string DelPath = Path.Combine(Server.MapPath(PathDir), post.Img);
                System.IO.File.Delete(DelPath); // xoa hinh
            }
            postDAO.Delete(post);
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công");
            return RedirectToAction("Index", "Post");
        }

        public ActionResult Trash()
        {
            return View(postDAO.getList("Trash"));
        }

        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại sản phẩm không tồn tại");
                return RedirectToAction("Index", "Post");
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Post");
            }
            post.Status = (post.Status == 1) ? 2 : 1;
            post.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            post.UpdatedAt = DateTime.Now;
            postDAO.Update(post);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "Post");
        }

        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại sản phẩm không tồn tại");
                return RedirectToAction("Index", "Post");
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Post");
            }
            post.Status = 0; //Trạng thái rác = 0
            post.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            post.UpdatedAt = DateTime.Now;
            postDAO.Update(post);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành thành công");
            return RedirectToAction("Index", "Post");
        }

        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại sản phẩm không tồn tại");
                return RedirectToAction("Trash", "Post");
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "Post");
            }
            post.Status = 2; //Trạng thái khoi phuc = 2
            post.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            post.UpdatedAt = DateTime.Now;
            postDAO.Update(post);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Trash", "Post");
        }
    }
}
