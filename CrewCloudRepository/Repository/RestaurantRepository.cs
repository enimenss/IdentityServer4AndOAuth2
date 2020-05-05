using CrewCloudRepository.Contracts;
using CrewCloudRepository.Models;

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Drawing;

namespace CrewCloudRepository.Repository
{
    public class RestaurantRepository : RepositoryBase<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(CrewCloudDBContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public IEnumerable<Restaurant> GetAllRestaurants()
        {

            return  FindAll()
                .OrderBy(x => x.Name)
                .ToList();
                          
        }
        public Restaurant GetRestaurantById(string id)
        {
            return FindByCondition(x => x.RestaurantId == id).FirstOrDefault();
        }
        public void CreateRestaurant(Restaurant restaurant)
        {
            Create(restaurant);
        }
        public void UpdateRestaurant(Restaurant restaurant)
        {
            Update(restaurant);
        }
        public void DeleteRestaurant(Restaurant restaurant)
        {
            Delete(restaurant);
        }

        private static double GetDistance(PointF point1, PointF point2)
        {

            double a = (double)(point2.X - point1.X);
            double b = (double)(point2.Y - point1.Y);

            return Math.Sqrt(a * a + b * b);
        }


    }
}

