using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanQuanAo.Models
{
    public class Giohang
    {
        QLBanQuanAoDataContext data = new QLBanQuanAoDataContext();

        public int iMasanpham { set; get; }
        public string sTensanpham { set; get; }
        public Double dDongia { set; get; }
        public string sAnhbia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien { 
            get { return iSoluong * dDongia; }
        }

        public Giohang(int Masanpham)
        {
            iMasanpham = Masanpham;
            SANPHAM sp = data.SANPHAMs.Single(n => n.MaSanPham == iMasanpham);
            sTensanpham = sp.TenSanPham;
            sAnhbia = sp.AnhSanPham;
            dDongia = double.Parse(sp.GiaBan.ToString());
            iSoluong = 1;
        }
    }
}