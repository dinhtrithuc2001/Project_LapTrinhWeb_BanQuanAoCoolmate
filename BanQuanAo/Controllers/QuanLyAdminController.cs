using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanQuanAo.Models;
using PagedList;

namespace BanQuanAo.Controllers
{
    public class QuanLyAdminController : Controller
    {
        // GET: QuanLyAdmin
        QLBanQuanAoDataContext db = new QLBanQuanAoDataContext();
        public ActionResult SanPham(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int pagesize = 10;
            int pageNum = (page ?? 1);
            var list = db.SANPHAMs.OrderByDescending(s => s.MaSanPham).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }
        [HttpGet]
        public ActionResult ThemmoiSanPham()
        {
            ViewBag.MaLoai = new SelectList(db.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            return View();
        }
        [HttpPost]
        public ActionResult ThemmoiSanPham(SANPHAM sANPHAM, HttpPostedFileBase fileupload)
        {
            ViewBag.MaLoai = new SelectList(db.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Public/images/HinhSanPham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sANPHAM.AnhSanPham = fileName;

                    db.SANPHAMs.InsertOnSubmit(sANPHAM);
                    db.SubmitChanges();
                }
                return RedirectToAction("SanPham");
            }
        }
        public ActionResult ChitietSanPham(int id)
        {
            var sANPHAM = from s in db.SANPHAMs
                          where s.MaSanPham == id
                          select s;

            if (sANPHAM == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return PartialView(sANPHAM.Single());
        }
        [HttpGet]
        public ActionResult XoaSanPham(int id)
        {
            SANPHAM sANPHAM = db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == id);
            ViewBag.MaSanPham = sANPHAM.MaSanPham;
            if (sANPHAM == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sANPHAM);
        }
        [HttpPost, ActionName("XoaSanPham")]
        public ActionResult XacNhanXoa(int id)
        {
            SANPHAM sANPHAM = db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == id);
            ViewBag.MaSanPham = sANPHAM.MaSanPham;
            if (sANPHAM == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            CHITIETDONTHANG chiTit = db.CHITIETDONTHANGs.SingleOrDefault(n => n.MaSanPham == id);

            if (chiTit == null)
            {
                ViewBag.MaSanPham = "Co tao ne";
                db.SANPHAMs.DeleteOnSubmit(sANPHAM);
                db.SubmitChanges();
            }
            return RedirectToAction("SanPham");
           
        }
        [HttpGet]
        public ActionResult SuaSanPham(int id)
        {
          
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == id);
            ViewBag.MaloaiSP = new SelectList(db.LOAIs.ToList().OrderBy(n => n.MaLoai), "MaLoai", "TenLoai", sp.MaLoai);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }

        [HttpPost, ActionName("SuaSanPham")]
        public ActionResult XacNhanSuasanpham(FormCollection collection, int id, HttpPostedFileBase fileupload)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == id);
            var img = "";
            if (fileupload != null)
            {
                img = Path.GetFileName(fileupload.FileName);
                var path = Path.Combine(Server.MapPath("~/Public/images/HinhSanPham"), img);
                if (!System.IO.File.Exists(path))//Sản Phẩm Chưa Tồn Tại
                {
                    fileupload.SaveAs(path);
                }
            }
            else
            {
                img = sp.AnhSanPham;
            }
           
            sp.AnhSanPham = img;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            UpdateModel(sp);
            db.SubmitChanges();
            return RedirectToAction("SanPham");
        }
        public ActionResult LoaiSanPham(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int pagesize = 10;
            int pageNum = (page ?? 1);
            var list = db.LOAIs.OrderByDescending(s => s.MaLoai).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }
        [HttpGet]
        public ActionResult ThemmoiloaiSanPham()
        {
            ViewBag.MaLoai = new SelectList(db.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            return View();
        }
        [HttpPost]
        public ActionResult ThemmoiloaiSanPham(LOAI lOAI, HttpPostedFileBase fileupload)
        {
            ViewBag.MaLoai = new SelectList(db.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            db.LOAIs.InsertOnSubmit(lOAI);
            db.SubmitChanges();
            return RedirectToAction("LoaiSanPham");
            
        }
        [HttpGet]
        public ActionResult SuaLoaiSanPham(int id)
        {

            LOAI ls = db.LOAIs.SingleOrDefault(n => n.MaLoai == id);
            ViewBag.MaloaiSP = new SelectList(db.LOAIs.ToList().OrderBy(n => n.MaLoai), "MaLoai", "TenLoai", ls.MaLoai);
            if (ls == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ls);
        }

        [HttpPost, ActionName("SuaLoaiSanPham")]
        public ActionResult XacNhanSuaLoaiSanPham(FormCollection collection, int id, HttpPostedFileBase fileupload)
        {
            var img = "";
            if (fileupload != null)
            {
                img = Path.GetFileName(fileupload.FileName);
                var path = Path.Combine(Server.MapPath("~/Public/images/HinhSanPham"), img);
                if (!System.IO.File.Exists(path))//Sản Phẩm Chưa Tồn Tại
                {
                    fileupload.SaveAs(path);
                }
            }
            else
            {
                img = collection["Anh"];
            }
            LOAI ls = db.LOAIs.SingleOrDefault(n => n.MaLoai == id);
            UpdateModel(ls);
            db.SubmitChanges();
            return RedirectToAction("LoaiSanPham");
        }
        public ActionResult ChitietloaiSanPham(int id)
        {
            var lOAI = from s in db.LOAIs
                          where s.MaLoai == id
                          select s;

            if (lOAI == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return PartialView(lOAI.Single()); ;
        }
            
        public ActionResult DonHang(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int pagesize = 10;
            int pageNum = (page ?? 1);
            var list = db.DONDATHANGs.OrderByDescending(s => s.MaDonHang).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }
        public ActionResult ChiTietDonHang(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int pagesize = 10;
            int pageNum = (page ?? 1);
            var list = db.CHITIETDONTHANGs.OrderByDescending(s => s.MaSanPham).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }
    }
}