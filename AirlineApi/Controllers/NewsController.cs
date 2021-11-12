using Microsoft.AspNetCore.Mvc;
using AirlineApi.Models;
using AirlineApi.Services;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        /**
         * Reference to the news service. 
         */
        private readonly NewsService _newsService;

        public NewsController(NewsService newsService)
        {
            _newsService = newsService;
        }

        /** 
         * Get the newss for a specific page.
         * <param name="amount">The amount of newss to retrieve</param>
         * <param name="page">The current page to retrieve data for</param>
         */
        [HttpGet]
        public ActionResult<NewsPageResponse> Get(int page, int amount)
        {
            var newsResponse = _newsService.Get(page, amount);

            if (newsResponse.articles == null)
            {
                return NotFound();
            }

            return newsResponse;
        }

        /**
         * Retrieve one article.
         * <param name="id">The unique identifier of an news</param>
         */
        [HttpGet("{id:length(24)}", Name = "GetNews")]
        public ActionResult<News> Get(string id)
        {
            var news = _newsService.Get(id);

            if (news == null)
            {
                return NotFound();
            }

            return news;
        }

        /**
         * Create a new news.
         * <param name="news">The news to create</param>
         */
        [HttpPost]
        public ActionResult<News> Create(News news)
        {
            _newsService.Create(news);

            return CreatedAtRoute("GetNews", new { id = news.Id.ToString() }, news);
        }

        /**
         * Update the passed news with the new values.
         * <param name="newsIn">The updated news</param>
         * <param name="id">The unique identifier of the news</param>
         */
        [HttpPut("{id:length(24)}")]
        public ActionResult<News> Update(string id, News newsIn)
        {
            var news = _newsService.Get(newsIn.Id);

            if (news == null)
            {
                return NotFound();
            }

           return _newsService.Update(newsIn.Id, newsIn);
        }

        /**
         * Remove the news with the given id.
         * <param name="id">The unique identifier of the news</param>
         */
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var news = _newsService.Get(id);

            if (news == null)
            {
                return NotFound();
            }

            _newsService.Remove(news.Id);

            return NoContent();
        }
    }
}
