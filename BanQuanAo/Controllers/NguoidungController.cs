using BanQuanAo.Helper;
using BanQuanAo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BanQuanAo.Controllers
{
    public class NguoidungController : Controller
    {
        QLBanQuanAoDataContext db = new QLBanQuanAoDataContext();

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = AppHelper.GetMd5Hash(collection["Matkhau"]);
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var dienthoai = collection["Dienthoai"];


            var TK = from sp in db.KHACHHANGs select sp;
            foreach (var item in TK)
            {
                if (item.Email == email)
                {
                    ViewBag.Thongbaodangky = "Email đã tồn tại !! Vui lòng nhập Email khác";
                    return this.DangKy();
                    
                }
                if (item.Taikhoan == tendn)
                {
                    ViewBag.Thongbaodangky = "Tài khoản đã tồn tại !! Vui lòng nhập tên tài khoản khác";
                    return this.DangKy();
                    
                }
            }
                kh.HoTen = hoten;
                kh.Taikhoan = tendn;
                kh.Matkhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachi;
                kh.DienthoaiKH = dienthoai;
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = AppHelper.GetMd5Hash(collection["Matkhau"]);
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);
            if (kh != null)
            {
                ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                Session["Taikhoan"] = kh;
                Session["TenTaikhoan"] = kh.HoTen;
            }
            else
            {
                ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng !!";
                return this.DangNhap();
            }
            return RedirectToAction("Index", "TrangChu");
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "TrangChu");
        }

        [HttpGet]
        public ActionResult DoiMatKhau()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoiMatKhau(FormCollection collection)
        {
            if(Session["Taikhoan"] != null)
            {
                KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
                KHACHHANG kh2 = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == kh.MaKH);
                var matKhauCu = AppHelper.GetMd5Hash(collection["Matkhau"]);
                if(matKhauCu != kh2.Matkhau)
                {
                    ViewBag.Thongbaothaydoimatkhau = "Sai mật khẩu! Vui lòng nhập chính xác mật khẩu";
                    return this.DoiMatKhau();
                }
                var matKhauMoi = collection["MatkhauMoi"];
                var matKhauMoiNhapLai = collection["MatkhauMoiNhapLai"];
                if(matKhauMoi != matKhauMoiNhapLai)
                {
                    ViewBag.Thongbaothaydoimatkhau = "Mật khẩu mới và Nhập lại mật khẩu mới không trùng nhau";
                    return this.DoiMatKhau();
                }

                kh2.Matkhau = AppHelper.GetMd5Hash(collection["MatkhauMoi"]);
                kh2.HoTen = kh.HoTen;
                kh2.Taikhoan = kh.Taikhoan;
                kh2.Email = kh.Email;
                kh2.DiachiKH = kh.DiachiKH;
                kh2.DienthoaiKH = kh.DienthoaiKH;
                if (kh.MaKH > 0)
                {
                    var existing = db.KHACHHANGs.Single(user => user.MaKH == kh.MaKH);

                    db.KHACHHANGs.DeleteOnSubmit(existing);
                }
               
                db.KHACHHANGs.InsertOnSubmit(kh2);
                Session["Taikhoan"] = kh2;
                Session["TenTaikhoan"] = kh2.HoTen;
                db.SubmitChanges();
                return RedirectToAction("Index", "TrangChu");
            }
            else
            {
                return RedirectToAction("DangNhap");
            }
        }

        public ActionResult ThongTinNguoiDung()
        {
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            return View(kh);
        }
    }
}