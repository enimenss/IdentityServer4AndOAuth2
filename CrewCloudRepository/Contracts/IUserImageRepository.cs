using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace CrewCloudRepository.Contracts
{
    public interface IUserImageRepository : IRepositoryBase<UserImage>
    {
        IEnumerable<UserImage> GetAllImages();
        UserImage GetImageById(int id);
        void CreateImage(UserImage image);
        void UpdateImage(UserImage image);
        void DeleteImage(UserImage image);
    }
}
