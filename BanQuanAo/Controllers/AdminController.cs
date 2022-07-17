using BanQuanAo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Security.Cryptography;

namespace BanQuanAo.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        QLBanQuanAoDataContext db = new QLBanQuanAoDataContext();
        
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var Username = collection["Username"];
            var Password = collection["Password"];
         
           
                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.Username == Username && n.Password == Password);

            if (ad != null)
            {
                ViewBag.ThongBaoDangNhap = "Chúc mừng đăng nhập thành công";
                Session["Taikhoanadmin"] = ad;
            }
            else
            {
                ViewBag.ThongBaoDangNhap = "Tên đăng nhập hoặc mật khẩu không đúng";
                return this.Login();
            }
            
            return RedirectToAction("Index", "DashBoard");
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}