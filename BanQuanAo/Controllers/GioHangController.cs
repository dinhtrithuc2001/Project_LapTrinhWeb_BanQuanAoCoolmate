using BanQuanAo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanQuanAo.Controllers
{
    public class GioHangController : Controller
    {

        QLBanQuanAoDataContext data = new QLBanQuanAoDataContext();

        // Phương thức lấy giỏ hàng
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if(lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        // Phương thức thêm hàng vào giỏ
        public ActionResult Themgiohang(int iMasanpham, string strURL)
        {
            // lấy ra Session giỏ hàng
            List<Giohang> lstGiohang = Laygiohang();
            // kiểm tra sản phẩm đã tồn tại trong Session giỏ hàng chưa
            Giohang sanpham = lstGiohang.Find(n => n.iMasanpham == iMasanpham);
            if(sanpham == null)
            {
                sanpham = new Giohang(iMasanpham);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            } 
        }
        // Phương thức tính tổng số lượng
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if(lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }
        // Phương thức tính tổng tiền
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if(lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }

        // Phương thức xử lý hiển thị giỏ hàng
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "SanPham");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        // tạo Partial View 
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        // Cap nhat gio hang
        public ActionResult CapnhatGiohang(int iMaSp, FormCollection f)
        {
            //lay gio hang tu session
            List<Giohang> lstGioHang = Laygiohang();
            // kiem tra ton tai
            Giohang sp = lstGioHang.SingleOrDefault(n => n.iMasanpham == iMaSp);
            // Neu ton tai thi sua so luong
            if(sp != null)
            {
                sp.iSoluong = int.Parse(f["txtSoLuong"].ToString());    
            }
            return RedirectToAction("GioHang");
        }

        // xoa san pham trong gio

        public ActionResult XoaGioHang(int iMaSp)
        {
            //lay gio hang tu session
            List<Giohang> lstGioHang = Laygiohang();
            // kiem tra ton tai
            Giohang sp = lstGioHang.SingleOrDefault(n => n.iMasanpham == iMaSp);
            // Neu ton tai thi sua so luong
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.iMasanpham == iMaSp);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }

        // Xoa tat ca gio hang 
        public ActionResult XoaTatCaGioHang()
        {
            //lay gio hang tu session
            List<Giohang> lstGioHang = Laygiohang();
            lstGioHang.Clear();
            return RedirectToAction("Index","SanPham");
        }
        
        // phương thức đặt hàng
        [HttpGet]
        public ActionResult Dathang()
        {
            if(Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Nguoidung");
            }
            if(Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "SanPham");
            }
            // lay gio hang tu session
            List<Giohang> lstgiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            return View(lstgiohang);
        }
        public ActionResult Dathang(FormCollection collection)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            foreach(var item in gh)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaSanPham = item.iMasanpham;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;
                data.CHITIETDONTHANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang","Giohang");
        }

        // phương thức xác nhận đơn hàng
        public ActionResult Xacnhandonhang()
        {
            return PartialView();
        }
    }
}