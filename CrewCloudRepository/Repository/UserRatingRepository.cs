using CrewCloudRepository.Contracts;
using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Dynamic;

namespace CrewCloudRepository.Repository
{
    public class UserRatingRepository : RepositoryBase<UserRating>, IUserRatingRepository
    {
        public UserRatingRepository(CrewCloudDBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<UserRating> GetAllUsersRating()
        {
            return FindAll()
                .OrderBy(x => x.Date)
                .ToList();
        }

        public IEnumerable<UserRating> GetAllUsersRatingForUser(string id)
        {
            return FindByCondition(x => x.UserId == id).ToList();
        }

        public IEnumerable<UserRating> GetAllUsersRatingFromRestaurant(string id)
        {
            return FindByCondition(x => x.RestaurantId == id).ToList();
        }

        public UserRating GetUserRatingById(int id)
        {
            return FindByCondition(x => x.Id == id).FirstOrDefault();
        }
        public void CreateUserRating(UserRating userRating)
        {
            Create(userRating);
        }
        public void UpdateUserRating(UserRating userRating)
        {
            Update(userRating);
        }
        public void DeleteUserRating(UserRating userRating)
        {
            Delete(userRating);
        }


    }
}
