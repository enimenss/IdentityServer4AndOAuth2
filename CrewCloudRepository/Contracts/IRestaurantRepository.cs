using CrewCloudRepository.Models;

using System;
using System.Collections.Generic;
using System.Text;

namespace CrewCloudRepository.Contracts
{
    public interface IRestaurantRepository : IRepositoryBase<Restaurant>
    {


        IEnumerable<Restaurant> GetAllRestaurants();
        Restaurant GetRestaurantById(string id);
        void CreateRestaurant(Restaurant restaurant);
        void UpdateRestaurant(Restaurant restaurant);
        void DeleteRestaurant(Restaurant restaurant);
       
    }
}
