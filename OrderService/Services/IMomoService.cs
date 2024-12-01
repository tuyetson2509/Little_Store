using static OrderService.Model.OrderModel;

namespace OrderService.Services
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
        Task<string> CheckTransactionStatusAsync(string orderId);
    }
}
