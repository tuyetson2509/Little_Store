using Azure.Core;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Cryptography;
using System.Text;
using static OrderService.Model.OrderModel;

namespace OrderService.Services
{
    public class MomoService : IMomoService
    {
        private readonly IOptions<MomoOptionModel> _options;
        public MomoService(IOptions<MomoOptionModel> options)
        {
            _options = options;
        }
        public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model)
        {
            model.OrderId = DateTime.UtcNow.Ticks.ToString();
            model.OrderInfo = "Khách hàng: " + model.FullName + ". Nội dung: " + model.OrderInfo;
            var rawData =
                $"partnerCode={_options.Value.PartnerCode}" +
                $"&accessKey={_options.Value.AccessKey}" +
                $"&requestId={model.OrderId}" +
                $"&amount={model.Amount}" +
                $"&orderId={model.OrderId}" +
                $"&orderInfo={model.OrderInfo}" +
                $"&returnUrl={_options.Value.ReturnUrl}" +
                $"&notifyUrl={_options.Value.NotifyUrl}" +
                $"&extraData=";

            var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

            var client = new RestClient(_options.Value.MomoApiUrl);
            var request = new RestRequest() { Method = Method.Post };
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");

            // Create an object representing the request data
            var requestData = new
            {
                accessKey = _options.Value.AccessKey,
                partnerCode = _options.Value.PartnerCode,
                requestType = _options.Value.RequestType,
                notifyUrl = _options.Value.NotifyUrl,
                returnUrl = _options.Value.ReturnUrl,
                orderId = model.OrderId,
                amount = model.Amount.ToString(),
                orderInfo = model.OrderInfo,
                requestId = model.OrderId,
                extraData = "",
                signature = signature
            };

            request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);
            var momoResponse = JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
            return momoResponse;

        }

        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            var amount = collection.First(s => s.Key == "amount").Value;
            var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
            var orderId = collection.First(s => s.Key == "orderId").Value;

            return new MomoExecuteResponseModel()
            {
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo

            };
        }
        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }
        public async Task<string> CheckTransactionStatusAsync(string orderId)
        {
            // Cấu hình các tham số cần thiết
            var partnerCode = _options.Value.PartnerCode;
            var accessKey = _options.Value.AccessKey;
            var secretKey = _options.Value.SecretKey;

            // Dữ liệu yêu cầu
            var rawData = $"accessKey={accessKey}&orderId={orderId}&partnerCode={partnerCode}&requestId={orderId}";


            // Tính toán chữ ký (hash) bằng HMAC-SHA256
            var hash = ComputeHmacSha256(rawData, secretKey);

            // Tạo đối tượng dữ liệu yêu cầu
            var requestData = new
            {
                partnerCode = partnerCode,
                requestId = orderId,
                orderId = orderId,
                signature = hash,                     
                lang = "vi"                      
            };

            // Tạo RestClient và RestRequest
            var url = "https://test-payment.momo.vn/v2/gateway/api/query";
            var client = new RestClient(url);  // Địa chỉ URL API MoMo kiểm tra trạng thái
            var request = new RestRequest() { Method = Method.Post };
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");

            // Thêm dữ liệu vào body của yêu cầu
            request.AddJsonBody(requestData);

            // Gửi yêu cầu đến API MoMo và nhận phản hồi
            var response = await client.ExecuteAsync(request);
            // Kiểm tra kết quả trả về
            if (response.IsSuccessful)
            {
                var momoResponse = JsonConvert.DeserializeObject<MomoTransactionStatusResponse>(response.Content);

                return momoResponse.Message;
            }
            else
            {
                // Trả về 0 nếu yêu cầu không thành công
                return null;
            }
        }
    }
}
