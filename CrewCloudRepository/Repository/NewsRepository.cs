using CrewCloudRepository.Contracts;
using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrewCloudRepository.Repository
{
    public class NewsRepository : RepositoryBase<News>, INewsRepository
    {
        public NewsRepository(CrewCloudDBContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public IEnumerable<News> GetAllNews()
        {
            return FindAll()
                .OrderBy(x => x.Date)
                .ToList();
        }
        public News GetNewsById(int id)
        {
            return FindByCondition(x => x.Id == id).FirstOrDefault();
        }
        public void CreateNews(News news)
        {
            Create(news);
        }
        public void UpdateNews(News news)
        {
            Update(news);
        }
        public void DeleteNews(News news)
        {
            Delete(news);
        }

        

        public IEnumerable<News> GetAllNewsFromRestaurant(string userId, string restaurantId)
        {
            return FindByCondition(x => x.RestaurantId == restaurantId&& x.UserId == userId).OrderBy(x => x.Date).ToList();
        }

        public IEnumerable<News> GetAllNewsFromUser(string userId, string restaurantId)
        {
            return FindByCondition(x=>(x.RestaurantId == restaurantId && x.UserId == userId)).OrderBy(x => x.Date).ToList();
        }

        public News GetNewsById(string id)
        {
            return FindByCondition(x => x.RestaurantId == id).FirstOrDefault();
        }
        public void CreateMessages(News news)
        {
            Create(news);
        }
        public void UpdateMessages(News news)
        {
            Update(news);
        }
        public void DeleteMessages(News news)
        {
            Delete(news);
        }
    }
}
