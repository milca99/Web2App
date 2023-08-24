using Backend.DTO;
using Backend.Interfaces;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleServ _articleService;

        public ArticlesController(IArticleServ articleService)
        {
            _articleService = articleService;
        }

        [HttpPost("create")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Seller")]
        public async Task<IActionResult> Create([FromForm] ArticleCreateDto newArticle)
        {
            if (!await _articleService.Create(newArticle))
                return BadRequest("User not valid");
            return Ok();
        }

        [HttpPatch("update")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Seller")]
        public async Task<IActionResult> Update([FromForm] ArticleUpdateDto updatedArticle)
        {
            if (!await _articleService.Update(updatedArticle))
                return BadRequest("Updated article not valid");
            return Ok();
        }

        [HttpDelete("delete/{id}/{sellerId}")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Seller")]
        public async Task<IActionResult> Delete(int id, int sellerId)
        {
            if (!await _articleService.Delete(id, sellerId))
                return BadRequest("Invalid id-s");
            return Ok();
        }

        [HttpGet("details/{id}")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Seller")]
        public async Task<IActionResult> GetArticle(int id)
        {
            if (id < 1)
                return BadRequest("Invalid id");
            var result = await _articleService.GetArticle(id);
            if (result == null)
                return BadRequest("No user found");
            return Ok(result);
        }

        //get all articles
        [HttpGet()]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Customer")]
        public async Task<IActionResult> GetAllArticles()
        {
            var result = await _articleService.GetAllArticles();
            return Ok(result);
        }
        //get only articles for seller
        [HttpGet("{id}")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Seller")]
        public async Task<IActionResult> GetSellerArticles(int id)
        {
            var result = await _articleService.GetSellerArticles(id);
            if (result == null)
                return BadRequest("Wrong Id");
            return Ok(result);
        }
    }
}
