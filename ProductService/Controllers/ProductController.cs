using Microsoft.AspNetCore.Mvc;
using ProductService.Model;
using ProductService.UnitOfWork;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SanPham>>> GetProducts()
        {
            var products = await _unitOfWork.ProductRepository.GetAllProductsAsync();
            return Ok(products);
        }
        [HttpGet("LoaiHang")]
        public async Task<ActionResult<IEnumerable<LoaiHang>>> GetLoaiHang()
        {
            var loaiHang = await _unitOfWork.ProductRepository.GetAllLoaiHangAsync();
            return Ok(loaiHang);
        }
        [HttpPost("AddProduct")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddProduct([FromBody] SanPham product)
        {
            await _unitOfWork.ProductRepository.AddProductAsync(product);
            await _unitOfWork.CompleteAsync();
            return Ok(new { message = "Product added successfully." });
        }
        [HttpPut("UpdateSoluong")]
        public async Task<IActionResult> UpdateSoluong([FromBody] List<UpdateQuantityRequest> products)
        {
            // Kiểm tra nếu danh sách sản phẩm rỗng
            if (products == null || !products.Any())
            {
                return BadRequest(new { message = "No products to update." });
            }

            // Lặp qua từng sản phẩm trong danh sách
            foreach (var item in products)
            {
                var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(item.MaSP);

                // Kiểm tra nếu sản phẩm không tồn tại
                if (product == null)
                {
                    return NotFound(new { message = $"Product with MaSP {item.MaSP} not found." });
                }

                // Cập nhật số lượng
                product.Soluong = item.SoLuong;

                // Lưu thay đổi vào cơ sở dữ liệu
                await _unitOfWork.CompleteAsync();
            }

            return Ok(new { message = "Product quantities updated successfully." });
        }
    }
}
