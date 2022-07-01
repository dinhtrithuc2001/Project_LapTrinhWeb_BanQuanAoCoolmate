using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using BanQuanAo.Models;

namespace BanQuanAo.Controllers
{
    public class TimKiemController : Controller
    {
        QLBanQuanAoDataContext data = new QLBanQuanAoDataContext();
        [HttpPost]
        public ActionResult KetQuaTimKiem(FormCollection f, int? page)
        {
            string sTuKhoa = f["txtTimKiem"].ToString();
            if(sTuKhoa == null)
            {
                List<SANPHAM> lstKQTK = data.SANPHAMs.Where(n => n.TenSanPham.Contains((string)Session["TuKhoa"])).ToList();
                // phân trang 
                int pageNumber = (page ?? 1);
                int pageSize = 8;
                if (lstKQTK.Count == 0)
                {
                    ViewBag.ThongBaoTimKiem = "Không tìm thấy sản phẩm nào";
                    return View(data.SANPHAMs.OrderBy(n => n.TenSanPham).ToPagedList(pageNumber, pageSize));
                }
                ViewBag.ThongBaoTimKiem = "Đã tìm thấy " + lstKQTK.Count + " kết quả !";
                return View(lstKQTK.OrderBy(n => n.TenSanPham).ToPagedList(pageNumber, pageSize));
            }
            else
            {
                Session["TuKhoa"] = sTuKhoa;
                List<SANPHAM> lstKQTK = data.SANPHAMs.Where(n => n.TenSanPham.Contains(sTuKhoa)).ToList();
                // phân trang 
                int pageNumber = (page ?? 1);
                int pageSize = 8;
                if (lstKQTK.Count == 0)
                {
                    ViewBag.ThongBaoTimKiem = "Không tìm thấy sản phẩm nào";
                    return View(data.SANPHAMs.OrderBy(n => n.TenSanPham).ToPagedList(pageNumber, pageSize));
                }
                ViewBag.ThongBaoTimKiem = "Đã tìm thấy " + lstKQTK.Count + " kết quả !";
                return View(lstKQTK.OrderBy(n => n.TenSanPham).ToPagedList(pageNumber, pageSize));
            }
        }

        [HttpGet]
        public ActionResult KetQuaTimKiem(int? page, string sTuKhoa)
        {
            if(sTuKhoa != null)
            {
                Session["TuKhoa"] = sTuKhoa;
                List<SANPHAM> lstKQTK = data.SANPHAMs.Where(n => n.TenSanPham.Contains(sTuKhoa)).ToList();
                // phân trang 
                int pageNumber = (page ?? 1);
                int pageSize = 8;
                if (lstKQTK.Count == 0)
                {
                    ViewBag.ThongBaoTimKiem = "Không tìm thấy sản phẩm nào";
                    return View(data.SANPHAMs.OrderBy(n => n.TenSanPham).ToPagedList(pageNumber, pageSize));
                }
                ViewBag.ThongBaoTimKiem = "Đã tìm thấy " + lstKQTK.Count + " kết quả !";
                return View(lstKQTK.OrderBy(n => n.TenSanPham).ToPagedList(pageNumber, pageSize));
            }
            else
            {
                List<SANPHAM> lstKQTK = data.SANPHAMs.Where(n => n.TenSanPham.Contains((string)Session["TuKhoa"])).ToList();
                // phân trang 
                int pageNumber = (page ?? 1);
                int pageSize = 8;
                if (lstKQTK.Count == 0)
                {
                    ViewBag.ThongBaoTimKiem = "Không tìm thấy sản phẩm nào";
                    return View(data.SANPHAMs.OrderBy(n => n.TenSanPham).ToPagedList(pageNumber, pageSize));
                }
                ViewBag.ThongBaoTimKiem = "Đã tìm thấy " + lstKQTK.Count + " kết quả !";
                return View(lstKQTK.OrderBy(n => n.TenSanPham).ToPagedList(pageNumber, pageSize));
            }
        }
    }
}


