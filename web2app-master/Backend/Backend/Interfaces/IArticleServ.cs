using Backend.DTO;

namespace Backend.Interfaces
{
    public interface IArticleServ
    {
        public Task<bool> Create(ArticleCreateDto newArticle);
        public Task<bool> Update(ArticleUpdateDto oldArticle);
        public Task<bool> Delete(int id, int seller);
        public Task<ArticleGetDto> GetArticle(int id);
        public Task<List<ArticleAllDto>> GetAllArticles();
        public Task<List<ArticleSellerDto>> GetSellerArticles(int id);
    }
}
