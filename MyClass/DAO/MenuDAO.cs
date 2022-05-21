using MyClass.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class MenuDAO
    {
        private MyDBContext db = new MyDBContext();
        // tra ve danh sach - trang nguoi dung
        public List<Menu> getListByParentId(string position, int parentid=0)
        {
            return db.Menus
                .Where(m => m.ParentId == parentid && m.Status == 1 && m.Position == position)
                .OrderBy(m=>m.Orders)
                .ToList();
        }
        // Tra ve danh sach ca mau tin
        public List<Menu> getList(string status = "All")
        {
            List<Menu> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Menus.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Menus.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Menus.ToList();
                        break;
                    }
            }
            return list;
        }
        // Tra ve 1 mau tin
        public Menu getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Menus.Find(id);
            }
        }
        // them mau tin
        public int Insert(Menu row)
        {
            db.Menus.Add(row);
            return db.SaveChanges();
        }
        // Cap nhat mau tin
        public int Update(Menu row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        // Xoa mau tin
        public int Delete(Menu row)
        {
            db.Menus.Remove(row);
            return db.SaveChanges();
        }
    }
}
