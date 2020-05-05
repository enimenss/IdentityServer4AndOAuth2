using CrewCloudRepository.Contracts;
using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrewCloudRepository.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(CrewCloudDBContext repositoryContext)
            : base(repositoryContext)
        {

        }
        public IEnumerable<User> GetAllUsers()
        {
            return FindAll()
                .OrderBy(x => x.FirstName)
                .ToList();
        }
        public User GetUserById(string id)
        {
            return FindByCondition(x => x.Email == id).FirstOrDefault();
        }
        public void CreateUser(User user)
        {
            Create(user);
        }
        public void UpdateUser(User user)
        {
            Update(user);
        }
        public void DeleteUser(User user)
        {
            Delete(user);
        }
    }
}
