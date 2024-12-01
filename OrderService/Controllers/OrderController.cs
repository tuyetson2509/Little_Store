using Microsoft.AspNetCore.Mvc;
using OrderService.UnitOfWork;
using static OrderService.Model.OrderModel;
using Microsoft.EntityFrameworkCore;
using OrderService.DBContext;
using Microsoft.AspNetCore.Authorization;
using OrderService.Services;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderContext _orderContext;
        private readonly IUnitOfWork _unitOfWork;
        private IMomoService _momoService;
        public OrderController(OrderContext orderContext, IUnitOfWork unitOfWork, IMomoService momoService)
        {
            _orderContext = orderContext;
            _unitOfWork = unitOfWork;
            _momoService = momoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonHang>>> GetOrders()
        {
            var orders = await _unitOfWork.OrderRepository.GetAllOrdersAsync();
            return Ok(orders);
        }

        [Authorize]
        //[AutoValidateAntiforgeryToken]
        [HttpPost("AddOrder")]

        public async Task<IActionResult> AddOrder([FromBody] OrderWithDetailsDto orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest(new { message = "Dữ liệu đơn hàng không hợp lệ." });
            }

            try
            {
                // Tạo đối tượng đơn hàng
                var order = new DonHang
                {
                    NgayDat = DateTime.Now,
                    ThanhToan = orderDto.ThanhToan,
                    DiaChiNhanHang = orderDto.DiaChiNhanHang,
                    MaNguoiDung = orderDto.MaNguoiDung,
                    TongTien = 0 // Tổng tiền sẽ được cập nhật sau khi thêm chi tiết đơn hàng
                };

                // Thêm đơn hàng vào cơ sở dữ liệu
                await _unitOfWork.OrderRepository.AddOrderAsync(order);
                await _unitOfWork.CompleteAsync();

                // Trả về mã đơn hàng vừa tạo
                return Ok(new { message = "Đơn hàng đã được tạo thành công.", MaDon = order.MaDon });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi xử lý yêu cầu.", details = ex.Message });
            }
        }
        [Authorize]
        //[AutoValidateAntiforgeryToken]
        [HttpPost("AddDetailsOrder/{maDon}")]

        public async Task<IActionResult> AddDetailsOrder(int maDon, [FromBody] OrderDetailDto detailsDto)
        {
            if (detailsDto == null)
            {
                return BadRequest(new { message = "Dữ liệu chi tiết đơn hàng không hợp lệ." });
            }

            try
            {
                // Lấy đơn hàng từ repository thông qua UnitOfWork
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(maDon);
                if (order == null)
                {
                    return NotFound(new { message = "Đơn hàng không tồn tại." });
                }

                // Tạo danh sách chi tiết đơn hàng
                var orderDetails = new ChiTietDonHang
                {
                    MaDon = maDon,
                    MaSP = detailsDto.MaSP,
                    SoLuong = detailsDto.SoLuong,
                    DonGia = detailsDto.DonGia,
                    ThanhTien = detailsDto.ThanhTien,
                    PhuongThucThanhToan = order.ThanhToan // Sử dụng phương thức thanh toán từ đơn hàng
                };

                // Thêm chi tiết đơn hàng thông qua repository
                await _unitOfWork.OrderRepository.AddAsync(orderDetails);

                // Cập nhật tổng tiền của đơn hàng
                order.TongTien += orderDetails.ThanhTien;
                _unitOfWork.OrderRepository.UpdateOrderAsync(order);

                // Lưu thay đổi vào cơ sở dữ liệu thông qua UnitOfWork
                await _unitOfWork.CompleteAsync();

                return Ok(new { message = "Chi tiết đơn hàng đã được thêm thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi xử lý yêu cầu.", details = ex.Message });
            }
        }
        [HttpPost]
        [Route("CreatePaymentUrl")]
        public async Task<IActionResult> CreatePaymentUrl(OrderInfoModel model)
        {
            var a = model;
            var response = await _momoService.CreatePaymentAsync(model);
            return Ok(new {response });
        }
        [HttpPost("CheckPaymentStatus")]
        public async Task<IActionResult> CheckPaymentStatus(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return BadRequest(new { status = 0, message = "Bạn chưa có OrderId" });
            }
            try
            {
                var status = await _momoService.CheckTransactionStatusAsync(orderId);
                return Ok(new { message = status });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 0, message = $"Đã xảy ra lỗi trong quá trình kiểm tra trạng thái thanh toán: {ex.Message}" });
            }
        }
        [HttpPut("orders/{MaDon}")]
        public async Task<IActionResult> UpdateOrderStatus(int MaDon, [FromBody] int? TinhTrang)
        {
            // Kiểm tra giá trị TinhTrang có hợp lệ không
            if (TinhTrang != null && TinhTrang != 1 && TinhTrang != 0)
            {
                // Nếu TinhTrang không hợp lệ, trả về BadRequest với thông điệp lỗi
                return BadRequest("TinhTrang must be 1, 0, or null.");
            }

            // Tìm đơn hàng theo MaDon
            var donHang = await _unitOfWork.OrderRepository.GetOrderByIdAsync(MaDon);
            if (donHang == null)
            {
                // Nếu không tìm thấy đơn hàng, trả về NotFound
                return NotFound("DonHang not found.");
            }

            // Cập nhật trạng thái TinhTrang của đơn hàng
            donHang.TinhTrang = TinhTrang;

            // Cập nhật đơn hàng trong cơ sở dữ liệu
            _unitOfWork.OrderRepository.UpdateOrderAsync(donHang);
            await _unitOfWork.CompleteAsync();

            // Trả về kết quả thành công
            return Ok(new { message = "Order status updated successfully." });
        }

    }
}
