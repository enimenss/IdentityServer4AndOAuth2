using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrewCloudRepository.Contracts
{
    public interface INewsRepository : IRepositoryBase<News>
    {
        IEnumerable<News> GetAllNews();
        News GetNewsById(int id);
        void CreateNews(News news);
        void UpdateNews(News news);
        void DeleteNews(News news);
    }
}
