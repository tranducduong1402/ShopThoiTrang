using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.Models;
using MyClass.DAO;
using ShopThoiTrang.Library;
using System.IO;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private ProductDAO productDAO = new ProductDAO();
        private CategoryDAO categoryDAO = new CategoryDAO();

        // GET: Admin/Product
        public ActionResult Index()
        {
            return View(productDAO.getList("Index"));
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productDAO.getRow(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(categoryDAO.getList("Index"), "Orders", "Name", 0);
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CatId,Name,Slug,Detail,MetaKey,Metadesc,Img,Number,Price,PriceSale,Create_At,Created_By,Created_At,Update_By,Update_At,Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                // Xử lý thêm thông tin
                product.Slug = XString.str_slug(product.Name);
                // Upload file
                var img = Request.Files["img"];
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    // Kiem tra tap tin
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    {
                        // upload hinh
                        string imgName = product.Slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        product.Img = imgName;
                        string PathDir = "~/Public/images/products/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }

                product.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                product.CreatedAt = DateTime.Now;
                productDAO.Insert(product);
                TempData["message"] = new XMessage("success", "Thêm thành công");
                return RedirectToAction("Index", "Product");
            }
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(categoryDAO.getList("Index"), "Orders", "Name", 0);
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productDAO.getRow(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CatId,Name,Slug,Detail,MetaKey,Metadesc,Img,Number,Price,PriceSale,Create_At,Created_By,Created_At,Update_By,Update_At,Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                // Upload file
                product.Slug = XString.str_slug(product.Name);
                var img = Request.Files["img"];
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    // Kiem tra tap tin
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    {
                        // upload hinh
                        string imgName = product.Slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        product.Img = imgName;
                        string PathDir = "~/Public/images/products/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        // Xoa file 
                        if (product.Img.Length > 0)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), product.Img);
                            System.IO.File.Delete(DelPath); // xoa hinh
                        }
                        img.SaveAs(PathFile);
                    }
                }
                product.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
                product.UpdatedAt = DateTime.Now;
                TempData["message"] = new XMessage("success", "Cập nhật thành công");
                productDAO.Update(product);
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(categoryDAO.getList("Index"), "Orders", "Name", 0);
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productDAO.getRow(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = productDAO.getRow(id);
            string PathDir = "~/Public/images/products/";
            // Xoa hinh anh
            // Xoa file 
            if (product.Img != null)
            {
                string DelPath = Path.Combine(Server.MapPath(PathDir), product.Img);
                System.IO.File.Delete(DelPath); // xoa hinh
            }
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công");
            productDAO.Delete(product);
            return RedirectToAction("Trash", "Product");
        }
        public ActionResult Trash()
        {
            return View(productDAO.getList("Trash"));
        }

        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã sản phẩm không tồn tại");
                return RedirectToAction("Index", "Product");
            }
            Product product = productDAO.getRow(id);
            if (product == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Product");
            }
            product.Status = (product.Status == 1) ? 2 : 1;
            product.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            product.UpdatedAt = DateTime.Now;
            productDAO.Update(product);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "Product");
        }

        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã sản phẩm không tồn tại");
                return RedirectToAction("Index", "Product");
            }
            Product product = productDAO.getRow(id);
            if (product == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Product");
            }
            product.Status = 0; //Trạng thái rác = 0
            product.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            product.UpdatedAt = DateTime.Now;
            productDAO.Update(product);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành thành công");
            return RedirectToAction("Index", "Product");
        }

        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã sản phẩm không tồn tại");
                return RedirectToAction("Trash", "Product");
            }
            Product product = productDAO.getRow(id);
            if (product == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "Product");
            }
            product.Status = 2; //Trạng thái khoi phuc = 2
            product.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
            product.UpdatedAt = DateTime.Now;
            productDAO.Update(product);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Trash", "Product");
        }
    }
}
