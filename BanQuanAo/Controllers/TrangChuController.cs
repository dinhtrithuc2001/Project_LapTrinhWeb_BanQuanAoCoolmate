using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanQuanAo.Models;

namespace BanQuanAo.Controllers
{
    public class TrangChuController : Controller
    {
        // Tạo 1 đối tượng chứa toàn bộ CSDL từ DB
        QLBanQuanAoDataContext data = new QLBanQuanAoDataContext();

        private List<SANPHAM> LaySanPhamMoi(int count)
        {
            return data.SANPHAMs.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        public ActionResult Index()
        {
            var sanphammoi = LaySanPhamMoi(4);
            return View(sanphammoi);
        }
    }
}