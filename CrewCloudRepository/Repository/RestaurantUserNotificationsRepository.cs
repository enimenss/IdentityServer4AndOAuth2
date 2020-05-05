using CrewCloudRepository.Contracts;
using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrewCloudRepository.Repository
{
    public class RestaurantUserNotificationsRepository : RepositoryBase<RestaurantUserNotifications>, IRestaurantUserNotificationsRepository
    {
        public RestaurantUserNotificationsRepository(CrewCloudDBContext repositoryContext)
            : base(repositoryContext)
        {

        }
        public IEnumerable<RestaurantUserNotifications> GetAllIRestaurantUserNotifications()
        {
            return FindAll()
                .OrderBy(x => x.Date)
                .ToList();
        }
        public RestaurantUserNotifications GetRestaurantUserNotificationsById(int id)
        {
            return FindByCondition(x => x.Id == id).FirstOrDefault();
        }
        public void CreateRestaurantUserNotifications(RestaurantUserNotifications restaurantUserNotifications)
        {
            Create(restaurantUserNotifications);
        }
        public void UpdateRestaurantUserNotifications(RestaurantUserNotifications restaurantUserNotifications)
        {
            Update(restaurantUserNotifications);
        }
        public void DeleteRestaurantUserNotifications(RestaurantUserNotifications restaurantUserNotifications)
        {
            FindByCondition(x => x.Id == restaurantUserNotifications.Id).SingleOrDefault().UserVis = false;
        }
    }
}
