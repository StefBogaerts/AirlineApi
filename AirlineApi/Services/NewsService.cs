using AirlineApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace AirlineApi.Services
{
    public class NewsService
    {
        /** The news article collection of the MongoDB */
        private readonly IMongoCollection<News> _news;

        public NewsService(IAirlineDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _news = database.GetCollection<News>("News");
        }

        /**
         * Retrieve all articles.
         */
        public List<News> Get()
        {
            return _news.Find(news => true).ToList();
        }

        /**
         * Retrieve a set amount of News.
         * <param name="page">The page to retrieve data for</param>
         * <param name="amount">The amount of articles to retrieve</param>
         */
        public NewsPageResponse Get(int page, int amount)
        {
            // Filter dataset.
            var newsArticles = _news.Find(news => true);
            var totalAmountOfArticles = newsArticles.ToList().Count;

            // Set page data.
            var amountToSkip = page * amount;
            var newsArticlesToReturn = newsArticles.Skip(amountToSkip).Limit(amount).ToList();

            return new NewsPageResponse(newsArticlesToReturn, totalAmountOfArticles);
        }

        /**
         * Retrieve one article.
         * <param name="id">The unique identifier</param>
         */
        public News Get(string id) =>
            _news.Find(news => news.Id == id).FirstOrDefault();

        /**
         *  Create a new news.
         *  <param name="news">The article to create</param>
         */
        public News Create(News news)
        {
            _news.InsertOne(news);
            return news;
        }

        /**
         * Update the given news.
         * <param name="id">The unique identifier</param>
         * <param name="news">The new news</param>
         */
        public News Update(string id, News news) {
            _news.ReplaceOne(a => a.Id == id, news);
            return news;
        }

        /**
         * Delete an news.
         * <param name="news">The news to remove</param>
         */
        public void Remove(News news) =>
            _news.DeleteOne(a => a.Id == news.Id);

        /**
         * Delete an news based on the id.
         * <param name="id">The unique identifier</param>
         */
        public void Remove(string id) =>
            _news.DeleteOne(a => a.Id == id);
    }
}

/**
 * The API response for a page.
 */
public class NewsPageResponse
{
    public NewsPageResponse(List<News> articles, long TotalAmount)
    {
        this.articles = articles;
        this.totalAmount = TotalAmount;
    }

    /** The news articles to send. */
    public List<News> articles { get; set; }

    /** The total amount of news articles. */
    public long totalAmount { get; set; }
}