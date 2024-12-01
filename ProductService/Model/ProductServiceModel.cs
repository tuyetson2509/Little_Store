using System.ComponentModel.DataAnnotations;

namespace ProductService.Model
{
    public class SanPham
    {
        [Key]
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public decimal? GiaBan { get; set; }
        public int Soluong { get; set; }
        public string MoTa { get; set; }
        public int MaLoai { get; set; }
        public int MaNCC { get; set; }
        public string AnhSP { get; set; }
    }

    public class LoaiHang
    {
        [Key]
        public int MaLoai { get; set; }
        public string TenLoai { get; set; }
    }

    public class NhaCungCap
    {
        [Key]
        public int MaNCC { get; set; }
        public string TenNCC { get; set; }
    }

    public class UpdateQuantityRequest
    {
        public int MaSP { get; set; }
        public int SoLuong { get; set; }
    }

}
