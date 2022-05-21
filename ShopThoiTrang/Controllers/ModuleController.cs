using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Models;

namespace ShopThoiTrang.Controllers
{
    public class ModuleController : Controller
    {
        private MenuDAO menuDAO = new MenuDAO();
        private SliderDAO sliderDAO = new SliderDAO();
        // GET: Module
        public ActionResult MainMenu()
        {
            List<Menu> list = menuDAO.getListByParentId("mainmenu",0);
            return View("MainMenu", list);
        }

        public ActionResult MainMenuSub(int id)
        {
            Menu menu = menuDAO.getRow(id);
            List<Menu> list = menuDAO.getListByParentId("mainmenu",id);
            if (list.Count == 0)
            {
                // khong co cap con

                return View("MainMenuSub1", menu);
            }
            else
            {
                // co cap con
                ViewBag.Menu = menu;
                return View("MainMenuSub2", list);
            }
        }
        //Slideshow
        public ActionResult Slideshow()
        {
            List<Slider> list = sliderDAO.getListByPosition("Slideshow");
            return View("Slideshow", list);
        }

    }
}