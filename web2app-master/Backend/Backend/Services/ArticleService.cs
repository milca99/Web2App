using AutoMapper;
using Backend.Database;
using Backend.DTO;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class ArticleService : IArticleServ
    {
        private readonly IArticleRepo _articleRepository;
        private readonly IUserRepo _userRepository;
        private readonly IMapper _mapper;

        public ArticleService(IArticleRepo articleRepository, IUserRepo userRepository, IMapper mapper)
        {
            _articleRepository = articleRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> Create(ArticleCreateDto newArticle)
        {
            if (!await _userRepository.DoesSellerExist(newArticle.UserId))
                return false;

            /*var result = await _photoService.UploadPhotoAsync(newArticle.File);
            if (result.Error != null)
                return false;*/

            var article = _mapper.Map<Article>(newArticle);
            //article.Picture = result.SecureUri.AbsoluteUri;
            return await _articleRepository.Create(article);
        }

        public async Task<bool> Update(ArticleUpdateDto oldArticle)
        {
            if (!await _userRepository.DoesSellerExist(oldArticle.UserId))
                return false;
            var article = _mapper.Map<Article>(oldArticle);
            /*if (oldArticle.File != null)
            {
                var result = await _photoService.UploadPhotoAsync(oldArticle.File);
                if (result.Error != null)
                    return false;
                article.Picture = result.SecureUrl.AbsoluteUri;
            }*/

            return await _articleRepository.Update(article);
        }

        public async Task<bool> Delete(int id, int seller)
        {
            if (!await _userRepository.DoesSellerExist(seller))
                return false;

            return await _articleRepository.Delete(id, seller);
        }
        public async Task<ArticleGetDto> GetArticle(int id)
        {
            var result = await _articleRepository.GetArticle(id);
            var returnValue = _mapper.Map<ArticleGetDto>(result);
            return returnValue;
        }
        public async Task<List<ArticleAllDto>> GetAllArticles()
        {
            var result = await _articleRepository.GetAllArticles();
            var returnValue = _mapper.Map<List<ArticleAllDto>>(result);
            return returnValue;
        }
        public async Task<List<ArticleSellerDto>> GetSellerArticles(int id)
        {
            if (!await _userRepository.DoesUserExist(id))
                return null;


            var result = await _articleRepository.GetSellerArticles(id);
            var history = _mapper.Map<List<ArticleSellerDto>>(result);
            return history;
        }
    }
}
