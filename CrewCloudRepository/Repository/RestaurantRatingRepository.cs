using CrewCloudRepository.Contracts;
using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrewCloudRepository.Repository
{
    class RestaurantRatingRepository : RepositoryBase<RestaurantRating>, IRestaurantRatingRepository
    {
        public RestaurantRatingRepository(CrewCloudDBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<RestaurantRating> GetAllRestaurantRating()
        {
            return FindAll()
                .OrderBy(x => x.Date)
                .ToList();
        }

        public IEnumerable<RestaurantRating> GetAllRestaurantRatingForRestaurant(string id)
        {
            return FindByCondition(x => x.RestaurantId == id).ToList();
        }

        public IEnumerable<RestaurantRating> GetAllRestaurantRatingFromUser(string id)
        {
            return FindByCondition(x => x.UserId == id).ToList();
        }

        public RestaurantRating GetRestaurantRatingById(int id)
        {
            return FindByCondition(x => x.Id == id).FirstOrDefault();
        }
        public void CreateRestaurantRating(RestaurantRating RestaurantRating)
        {
            Create(RestaurantRating);
        }
        public void UpdateRestaurantRating(RestaurantRating RestaurantRating)
        {
            Update(RestaurantRating);
        }
        public void DeleteRestaurantRating(RestaurantRating RestaurantRating)
        {
            Delete(RestaurantRating);
        }
    }
}
