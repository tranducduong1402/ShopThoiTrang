using MyClass.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class ContactDAO
    {
        private MyDBContext db = new MyDBContext();
        public List<Contact> getList()
        {
            return db.Contacts.ToList();
        }
        // Tra ve danh sach ca mau tin
        public List<Contact> getList(string status = "All")
        {
            List<Contact> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Contacts.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Contacts.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Contacts.ToList();
                        break;
                    }
            }
            return list;
        }
        // Tra ve 1 mau tin
        public Contact getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Contacts.Find(id);
            }
        }
        // them mau tin
        public int Insert(Contact row)
        {
            db.Contacts.Add(row);
            return db.SaveChanges();
        }
        // Cap nhat mau tin
        public int Update(Contact row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        // Xoa mau tin
        public int Delete(Contact row)
        {
            db.Contacts.Remove(row);
            return db.SaveChanges();
        }
    }
}
