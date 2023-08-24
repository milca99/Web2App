using AutoMapper;
using Backend.Database;
using Backend.DTO;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class OrderService : IOrderServ
    {
        private readonly IOrderRepo _orderRepository;
        private readonly IUserRepo _userRepository;
        private readonly IArticleRepo _articleRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepo orderRepository, IUserRepo userRepository, IArticleRepo articleRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _articleRepository = articleRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderHistoryDto>> History(int id)
        {
            if (!await _userRepository.DoesUserExist(id))
                return null;

            await _orderRepository.UpdateStatus();

            var result = await _orderRepository.History(id);
            var history = _mapper.Map<List<OrderHistoryDto>>(result);
            return history;
        }

        public async Task<OrderGetCreatedDto> Create(OrderCreateDto newOrder)
        {
            if (!await _userRepository.DoesUserExist(newOrder.UserId))
                return null;
            if (!await _userRepository.DoesUserExist(newOrder.SellerId))
                return null;
            if (!await _articleRepository.DoesArticleExist(newOrder.Item.ArticleId))
                return null;
            var order = _mapper.Map<Order>(newOrder);
            var result = await _orderRepository.Create(order);
            if (result == null)
                return null;
            var returnValue = _mapper.Map<OrderGetCreatedDto>(result);
            return returnValue;
        }

        public async Task<List<OrderGetAllDto>> AllOrders()
        {
            await _orderRepository.UpdateStatus();
            var result = await _orderRepository.AllOrders();
            var returnValue = _mapper.Map<List<OrderGetAllDto>>(result);
            return returnValue;
        }

        public async Task<List<OrderGetActiveDto>> GetActiveOrders(int id)
        {
            await _orderRepository.UpdateStatus();
            var result = await _orderRepository.GetActiveOrders(id);
            var returnValue = _mapper.Map<List<OrderGetActiveDto>>(result);
            return returnValue;
        }

        public async Task<bool> CancelOrder(OrderCancelDto cancelOrder)
        {
            await _orderRepository.UpdateStatus();
            var result = await _orderRepository.CancelOrder(cancelOrder.orderId, cancelOrder.userId);
            return result;
        }
    }
}
