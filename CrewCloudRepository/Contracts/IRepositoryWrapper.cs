using System;
using System.Collections.Generic;
using System.Text;

namespace CrewCloudRepository.Contracts
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IUserRatingRepository UserRating { get; }
        IRestaurantRepository Restaurant { get; }
        IRestaurantRatingRepository RestaurantRating { get; }
        INewsRepository News { get; }
        IMessagesRepository Messages { get; }
        IJobRepository Job { get; }
        IJobPostRepository JobPost { get; }
        IJobApplicationRepository JobApplication { get; }
        IUserImageRepository UserImage { get;  }
        IRestaurantImageRepository RestaurantImage { get;  }
        IRestaurantUserNotificationsRepository RestaurantUserNotifications { get; }
        void Save();
    }
}
