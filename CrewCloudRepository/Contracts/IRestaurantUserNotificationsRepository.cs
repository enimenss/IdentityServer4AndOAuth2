using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrewCloudRepository.Contracts
{
    public interface IRestaurantUserNotificationsRepository : IRepositoryBase<RestaurantUserNotifications>
    {
        IEnumerable<RestaurantUserNotifications> GetAllIRestaurantUserNotifications();
        RestaurantUserNotifications GetRestaurantUserNotificationsById(int id);
        void CreateRestaurantUserNotifications(RestaurantUserNotifications restaurantUserNotifications);
        void UpdateRestaurantUserNotifications(RestaurantUserNotifications restaurantUserNotifications);
        void DeleteRestaurantUserNotifications(RestaurantUserNotifications restaurantUserNotifications);
    }
}
