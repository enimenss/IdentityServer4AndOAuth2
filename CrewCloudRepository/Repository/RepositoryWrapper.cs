using CrewCloudRepository.Contracts;
using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrewCloudRepository.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private CrewCloudDBContext _repoContext;
        private IUserRepository _user;
        private IUserRatingRepository _userRating;
        private IRestaurantRepository _restaurant;
        private IRestaurantRatingRepository _restaurantRating;
        private INewsRepository _news;
        private IMessagesRepository _messages;
        private IJobApplicationRepository _jobApplication;
        private IJobPostRepository _jobPost;
        private IJobRepository _job;
        private IUserImageRepository _userImage;
        private IRestaurantImageRepository _restaurantImage;
        private IRestaurantUserNotificationsRepository _restaurantUserNotifications;

        public IUserRatingRepository UserRating
        {
            get
            {
                if (_userRating == null)
                {
                    _userRating = new UserRatingRepository(_repoContext);
                }

                return _userRating;
            }
        }
        public IRestaurantRepository Restaurant
        {
            get
            {
                if (_restaurant == null)
                {
                    _restaurant = new RestaurantRepository(_repoContext);
                }

                return _restaurant;
            }
        }
        public IRestaurantRatingRepository RestaurantRating
        {
            get
            {
                if (_restaurantRating == null)
                {
                    _restaurantRating = new RestaurantRatingRepository(_repoContext);
                }

                return _restaurantRating;
            }
        }
        public INewsRepository News
        {
            get
            {
                if (_news == null)
                {
                    _news = new NewsRepository(_repoContext);
                }

                return _news;
            }
        }
        public IMessagesRepository Messages
        {
            get
            {
                if (_messages == null)
                {
                    _messages = new MessagesRepository(_repoContext);
                }

                return _messages;
            }
        }
        public IJobApplicationRepository JobApplication
        {
            get
            {
                if (_jobApplication == null)
                {
                    _jobApplication = new JobApplicationRepository(_repoContext);
                }

                return _jobApplication;
            }
        }
        public IJobPostRepository JobPost
        {
            get
            {
                if (_jobPost == null)
                {
                    _jobPost = new JobPostRepository(_repoContext);
                }

                return _jobPost;
            }
        }
        public IJobRepository Job
        {
            get
            {
                if (_job == null)
                {
                    _job = new JobRepository(_repoContext);
                }

                return _job;
            }
        }
        public IUserImageRepository UserImage
        {
            get
            {
                if (_userImage == null)
                {
                    _userImage = new UserImageRepository(_repoContext);
                }

                return _userImage;
            }
        }
        public IRestaurantImageRepository RestaurantImage
        {
            get
            {
                if (_restaurantImage == null)
                {
                    _restaurantImage = new RestaurantImageRepository(_repoContext);
                }

                return _restaurantImage;
            }
        }


        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }

                return _user;
            }
        }
        public IRestaurantUserNotificationsRepository RestaurantUserNotifications
        {
            get
            {
                if (_restaurantUserNotifications == null)
                {
                    _restaurantUserNotifications = new RestaurantUserNotificationsRepository(_repoContext);
                }

                return _restaurantUserNotifications;
            }
        }

       

        public RepositoryWrapper(CrewCloudDBContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
