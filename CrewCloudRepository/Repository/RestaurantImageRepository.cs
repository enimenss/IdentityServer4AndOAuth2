using CrewCloudRepository.Contracts;
using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrewCloudRepository.Repository
{
    public class RestaurantImageRepository : RepositoryBase<RestaurantImage>, IRestaurantImageRepository
    {
        public RestaurantImageRepository(CrewCloudDBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<RestaurantImage> GetAllImages()
        {
            return FindAll()
                .ToList();
        }
        public RestaurantImage GetImageById(int id)
        {
            return FindByCondition(x => x.Id == id).FirstOrDefault();
        }
        public void CreateImage(RestaurantImage image)
        {
            Create(image);
        }
        public void UpdateImage(RestaurantImage image)
        {
            Update(image);
        }
        public void DeleteImage(RestaurantImage image)
        {
            Delete(image);
        }
    }
}
