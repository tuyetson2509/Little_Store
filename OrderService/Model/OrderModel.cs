using System.ComponentModel.DataAnnotations;

namespace OrderService.Model
{
    public class OrderModel
    {
        public class DonHang
        {
            [Key]
            public int MaDon { get; set; }
            public int MaNguoiDung { get; set; }
            public DateTime NgayDat { get; set; }
            public int ThanhToan { get; set; }
            public string DiaChiNhanHang { get; set; }
            public decimal? TongTien { get; set; }
            public int? TinhTrang { get; set; }
        }

        public class ChiTietDonHang
        {
            [Key]
            public int MaDon { get; set; }
            public int MaSP { get; set; }
            public int SoLuong { get; set; }
            public decimal DonGia { get; set; }
            public decimal ThanhTien { get; set; }
            public int PhuongThucThanhToan { get; set; }
        }
        public class OrderWithDetailsDto
        {
            public int MaNguoiDung { get; set; }
            public int ThanhToan { get; set; }
            public string DiaChiNhanHang { get; set; }
        }

        public class OrderDetailDto
        {
            public int MaSP { get; set; }
            public int SoLuong { get; set; }
            public decimal DonGia { get; set; }
            public decimal ThanhTien { get; set; }
        }
        public class MomoCreatePaymentResponseModel
        {
            public string RequestId { get; set; }
            public int ErrorCode { get; set; }
            public string OrderId { get; set; }
            public string Message { get; set; }
            public string LocalMessage { get; set; }
            public string RequestType { get; set; }
            public string PayUrl { get; set; }
            public string Signature { get; set; }
            public string QrCodeUrl { get; set; }
            public string Deeplink { get; set; }
            public string DeeplinkWebInApp { get; set; }
        }
        public class MomoExecuteResponseModel
        {
            public string OrderId { get; set; }
            public string Amount { get; set; }
            public string FullName { get; set; }
            public string OrderInfo { get; set; }
        }
        public class MomoOptionModel
        {
            public string MomoApiUrl { get; set; }
            public string SecretKey { get; set; }
            public string AccessKey { get; set; }
            public string ReturnUrl { get; set; }
            public string NotifyUrl { get; set; }
            public string PartnerCode { get; set; }
            public string RequestType { get; set; }
        }
        public class OrderInfoModel
        {
            public string FullName { get; set; }
            public string OrderId { get; set; }
            public string OrderInfo { get; set; }
            public double Amount { get; set; }
        }
        public class MomoTransactionStatusResponse
        {
            public string PartnerCode { get; set; } 
            public string OrderId { get; set; }  
            public string RequestId { get; set; }  
            public string ExtraData { get; set; }  
            public double Amount { get; set; }  
            public long TransId { get; set; }  
            public string PayType { get; set; }  
            public long ResultCode { get; set; }  
            public string Message { get; set; }  
            public long ResponseTime { get; set; }  
            public long LastUpdated { get; set; }  
            public string Signature { get; set; }  
        }

        public class PaymentStatusResponse
        {
            public string status { get; set; }
            public string message { get; set; }
            public string orderId { get; set; }
            public decimal amount { get; set; }
            public string transactionId { get; set; }
        }
        public class PaymentStatusRequest
        {
            public string OrderId { get; set; }
        }
        public class TinhTrangModel
        {
            public int TinhTrang { get; set; }
        }
    }
}
