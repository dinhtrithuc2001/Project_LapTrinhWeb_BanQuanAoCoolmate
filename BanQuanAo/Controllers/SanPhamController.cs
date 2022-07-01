using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanQuanAo.Models;
using PagedList;
using PagedList.Mvc;

namespace BanQuanAo.Controllers
{
    public class SanPhamController : Controller
    {
        QLBanQuanAoDataContext data = new QLBanQuanAoDataContext();
       
        public ActionResult Index(int ? page)
        {
            // tạo biến quy định số sản phẩm trên mỗi trang là 4
            int pageSize = 8;

            // tạo biến số trang
            int pageNum = (page ?? 1);

            // lấy tất cả sản phẩm
            var sanPham = from sp in data.SANPHAMs select sp;
            return View(sanPham.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Details(int id)
        {
            var sanPham = from s in data.SANPHAMs
                          where s.MaSanPham == id
                          select s;
            return PartialView(sanPham.Single());
        }
        public ActionResult Loai()
        {
            var loai = from sp in data.LOAIs select sp;
            return PartialView(loai);
        }
        public ActionResult SPTheoLoai(int id, int? page)
        {
            // tạo biến quy định số sản phẩm trên mỗi trang là 4
            int pageSize = 8;

            // tạo biến số trang
            int pageNum = (page ?? 1);

            var sp = from s in data.SANPHAMs where s.MaLoai == id select s;
            return View(sp.ToPagedList(pageNum, pageSize));
        }
        public ActionResult SapXepSanPham(string kieuSapXep, int? page)
        {
            // tạo biến quy định số sản phẩm trên mỗi trang là 4
            int pageSize = 8;

            // tạo biến số trang
            int pageNum = (page ?? 1);

            if(kieuSapXep == null)
            {
                kieuSapXep = Session["KieuSapXep"].ToString();
                if (kieuSapXep == "giamdan")
                {
                    Session["KieuSapXep"] = "giamdan";
                    ViewBag.KieuSanXepSanPham = "Sản phẩm theo giá giảm dần";
                    var sanphamgiamdan = data.SANPHAMs.OrderByDescending(s => s.GiaBan);
                    return View(sanphamgiamdan.ToPagedList(pageNum, pageSize));
                }
                else
                {
                    Session["KieuSapXep"] = "tangdan";
                    ViewBag.KieuSanXepSanPham = "Sản phẩm theo giá tăng dần";
                    var sanphamtangdan = data.SANPHAMs.OrderBy(s => s.GiaBan);
                    return View(sanphamtangdan.ToPagedList(pageNum, pageSize));
                }
            }
            else
            {
                if (kieuSapXep == "giamdan")
                {
                    Session["KieuSapXep"] = "giamdan";
                    ViewBag.KieuSanXepSanPham = "Sản phẩm theo giá giảm dần";
                    var sanphamgiamdan = data.SANPHAMs.OrderByDescending(s => s.GiaBan);
                    return View(sanphamgiamdan.ToPagedList(pageNum, pageSize));
                }
                else
                {
                    Session["KieuSapXep"] = "tangdan";
                    ViewBag.KieuSanXepSanPham = "Sản phẩm theo giá tăng dần";
                    var sanphamtangdan = data.SANPHAMs.OrderBy(s => s.GiaBan);
                    return View(sanphamtangdan.ToPagedList(pageNum, pageSize));
                }
            }
        }   
    }
}