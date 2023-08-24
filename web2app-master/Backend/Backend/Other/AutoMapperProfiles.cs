using AutoMapper;
using Backend.DTO;
using Backend.Models;

namespace Backend.Other
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            //USER
            //register user
            CreateMap<UserRegDto, User>()
                    .ForMember(dest => dest.Password, opt => opt.Ignore())
                    .ForMember(dest => dest.PasswordKey, opt => opt.Ignore());

            //update user
            CreateMap<UserUpdateDto, User>()
                    .ForMember(dest => dest.Password, opt => opt.Ignore())
                    .ForMember(dest => dest.PasswordKey, opt => opt.Ignore());

            //get all sellers
            CreateMap<User, UserSellersDto>();

            //get user details
            CreateMap<User, UserGetDto>();

            //ARTICLE
            //create article
            CreateMap<ArticleCreateDto, Article>();

            //update article
            CreateMap<ArticleUpdateDto, Article>();

            //get all articles
            CreateMap<Article, ArticleAllDto>();

            //get all seller articles
            CreateMap<Article, ArticleSellerDto>();

            //get article details
            CreateMap<Article, ArticleGetDto>();

            //ORDER
            //create order
            CreateMap<OrderCreateDto, Order>()
                .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Item))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Seller, opt => opt.Ignore());
            CreateMap<ItemForOrderDto, Item>();
            CreateMap<Order, OrderGetCreatedDto>()
                .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Item));
            CreateMap<Item, ItemGetCreatedOrderDto>()
                .ForMember(dest => dest.ArticleName, opt => opt.MapFrom(src => src.Article.Name));

            //get order history for customer and seller
            CreateMap<Order, OrderHistoryDto>()
                .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Item));
            CreateMap<Item, ItemOrderHistoryDto>()
                .ForMember(dest => dest.ArticleName, opt => opt.MapFrom(src => src.Article.Name));

            //get all orders
            CreateMap<Order, OrderGetAllDto>()
                .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Item));
            CreateMap<Item, ItemGetAllOrderDto>()
                .ForMember(dest => dest.ArticleName, opt => opt.MapFrom(src => src.Article.Name));

            //get all active orders
            CreateMap<Order, OrderGetActiveDto>()
                            .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Item));
            CreateMap<Item, ItemGetActiveOrderDto>()
                .ForMember(dest => dest.ArticleName, opt => opt.MapFrom(src => src.Article.Name));
        }
    }
}
