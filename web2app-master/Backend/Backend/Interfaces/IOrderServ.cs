using Backend.DTO;

namespace Backend.Interfaces
{
    public interface IOrderServ
    {
        public Task<List<OrderHistoryDto>> History(int id);
        public Task<OrderGetCreatedDto> Create(OrderCreateDto newArticle);
        public Task<List<OrderGetAllDto>> AllOrders();
        public Task<List<OrderGetActiveDto>> GetActiveOrders(int id);
        public Task<bool> CancelOrder(OrderCancelDto cancelOrder);
    }
}
