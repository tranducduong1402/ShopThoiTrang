using MyClass.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class PostDAO
    {
        private MyDBContext db = new MyDBContext();
        public List<Post> getList()
        {
            return db.Posts.ToList();
        }
        // Tra ve danh sach ca mau tin
        public List<Post> getList(string status = "All", string type="Post")
        {
            List<Post> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Posts.Where(m => m.Status != 0 && m.Type == type).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Posts.Where(m => m.Status == 0 && m.Type == type).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Posts.Where(m=>m.Type==type).ToList();
                        break;
                    }
            }
            return list;
        }
        // Tra ve 1 mau tin
        public Post getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Posts.Find(id);
            }
        }

        public Post getRow(string slug)
        {
            return db.Posts.Where(m => m.Type == "Post" && m.Slug == slug && m.Status == 1).FirstOrDefault();
        }
        // them mau tin
        public int Insert(Post row)
        {
            db.Posts.Add(row);
            return db.SaveChanges();
        }
        // Cap nhat mau tin
        public int Update(Post row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        // Xoa mau tin
        public int Delete(Post row)
        {
            db.Posts.Remove(row);
            return db.SaveChanges();
        }
    }
}
