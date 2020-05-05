using AutoMapper;
using CrewCloudApi.ViewModels;
using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrewCloudApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User Maps
            CreateMap<User, UserListVM>();
            CreateMap<User, UserProfileScreenVM>();
            CreateMap<CreateUserVM, User>();
            CreateMap<User, CreateUserVM>();
            CreateMap<UserUpdateVM, User>();
            CreateMap<User, UserUpdateVM > ();
            //Restaurants Maps
            CreateMap<Restaurant, RestaurantListItemVM>();
            CreateMap<Restaurant, RestaurantProfileScreenVM>();
            CreateMap<CreateRestaurantVM, Restaurant>();
            CreateMap<Restaurant, CreateRestaurantVM>();
            CreateMap<RestaurantUpdateVM, Restaurant>();
            CreateMap<Restaurant, RestaurantUpdateVM> ();
            //News Maps
            CreateMap<News, NewsListItemVM>();
            CreateMap<News, NewsProfileScreenVM>();
            CreateMap<CreateNewsVM, News>();
            CreateMap<News, CreateNewsVM>();
            CreateMap<NewsUpdateVM, News>();
            CreateMap<News, NewsUpdateVM> ();
            //Messages Maps
            CreateMap<Messages, MessageListItemVM>();
            CreateMap<Messages, MessageProfileScreenVM>();
            CreateMap<CreateMessagesVM, Messages>();
            CreateMap<Messages, CreateMessagesVM>();
            CreateMap<MessageUpdateVM, Messages>();
            CreateMap<Messages, MessageUpdateVM> ();
            //Jobs Maps
            CreateMap<Job, JobListItemVM>();
            CreateMap<JobListItemVM , Job>();
            CreateMap<Job, CreateJobVM>();
            CreateMap<CreateJobVM, Job>();

            //Restaurant Notifications Maps
            
            CreateMap<User, RestaurantNotificationListItemVM>(); 
            CreateMap<RestaurantNotificationListItemVM,User>();
            //Images Maps
            CreateMap<ImageListItemVM, UserImage>();
            CreateMap<UserImage, ImageListItemVM>();
            CreateMap<RestaurantImage, ImageListItemVM>();
            CreateMap<ImageListItemVM, RestaurantImage> ();


        }
    }
}
