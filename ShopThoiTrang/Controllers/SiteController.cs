using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyClass.Models;
using MyClass.DAO;

namespace ShopThoiTrang.Controllers
{
    public class SiteController : Controller
    {
        LinkDAO linkDAO = new LinkDAO();
        ProductDAO productDAO = new ProductDAO();
        PostDAO postDAO = new PostDAO();
        //Url mac dinh hoac Url bat ky
        // GET: Site
        public ActionResult Index(string slug = null)
        {
            if (slug == null)
            {
                // Trang chu
                return this.Home();
            }
            else
            {
                // Tim slug co trong bang link
                Link link = linkDAO.getRow(slug);
                if (link != null)
                {
                    // Slug co trong bang link
                    string typelink = link.TypeLink;
                    switch (typelink)
                    {
                        case "category":
                            {
                                return this.ProductCategory(slug);
                            }
                        case "topic": {
                                return this.PostTopic(slug);
                            }
                        case "page": {
                                return this.PostPage(slug);
                            }
                        default:
                            {
                                return this.Error404(slug);
                            }
                    }
                }
                else
                {
                    // slug co trong bang Product
                    Product product = productDAO.getRow(slug);
                    if(product != null)
                    {
                        return this.ProductDetail(product);
                    }
                    else
                    {
                        Post post = postDAO.getRow(slug);
                        if(post != null)
                        {
                            return this.PostDetail(post);
                        }
                        else
                        {
                            return this.Error404(slug);
                        }
                    }
                    // slug co trong bang Post Post-type = post
                    // Slug khong co trong bang link
                }
            }
        }

        //Trang chu
        public ActionResult Home()
        {
            return View("Home");
        }

        // Nhom Action Product
        public ActionResult Product()
        {
            return View("Product");
        }

        public ActionResult ProductCategory(string slug)
        {
            return View("ProductCategory");
        }

        public ActionResult ProductDetail(Product product)
        {
            return View("ProductDetail");
        }


        // Nhom Post
        public ActionResult Post()
        {
            return View("Post");
        }

        public ActionResult PostTopic(string slug)
        {
            return View("PostTopic");
        }

        public ActionResult PostPage(string slug)
        {
            return View("PostPage");
        }

        public ActionResult PostDetail(Post post)
        {
            return View("PostDetail");
        }

        // Ham loi
        public ActionResult Error404(string slug)
        {
            return View("Error404");
        }
    }
}