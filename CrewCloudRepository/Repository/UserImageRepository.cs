using CrewCloudRepository.Contracts;
using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrewCloudRepository.Repository
{
    public class UserImageRepository : RepositoryBase<UserImage>, IUserImageRepository
    {
        public UserImageRepository(CrewCloudDBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<UserImage> GetAllImages()
        {
            return FindAll()
                .ToList();
        }
        public UserImage GetImageById(int id)
        {
            return FindByCondition(x => x.Id == id).FirstOrDefault();
        }
        public void CreateImage(UserImage image)
        {
            Create(image);
        }
        public void UpdateImage(UserImage image)
        {
            Update(image);
        }
        public void DeleteImage(UserImage image)
        {
            Delete(image);
        }
    }
}
