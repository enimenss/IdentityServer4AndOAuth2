using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace CrewCloudRepository.Contracts
{
    public interface IRestaurantImageRepository : IRepositoryBase<RestaurantImage>
    {
        IEnumerable<RestaurantImage> GetAllImages();
        RestaurantImage GetImageById(int id);
        void CreateImage(RestaurantImage image);
        void UpdateImage(RestaurantImage image);
        void DeleteImage(RestaurantImage image);
    }
}
