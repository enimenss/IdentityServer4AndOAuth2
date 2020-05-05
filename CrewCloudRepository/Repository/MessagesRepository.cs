using CrewCloudRepository.Contracts;
using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrewCloudRepository.Repository
{
    public class MessagesRepository : RepositoryBase<Messages>, IMessagesRepository
    {
        public MessagesRepository(CrewCloudDBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<Messages> GetAllMessages()
        {
            return FindAll()
                .OrderBy(x => x.Date)
                .ToList();
        }

        public IEnumerable<Messages> GetAllMessagesFromRestaurant(string userId, string restaurantId)
        {
            return FindByCondition(x => x.RestaurantId == restaurantId&& x.UserId == userId).OrderBy(x => x.Date).ToList();
        }

        public IEnumerable<Messages> GetAllMessagesFromUser(string userId, string restaurantId)
        {
            return FindByCondition(x=> x.RestaurantId == restaurantId && x.UserId == userId).OrderBy(x => x.Date).ToList();
        }

        public Messages GetMessagesById(int id)
        {
            return FindByCondition(x => x.Id == id).FirstOrDefault();
        }
        public void CreateMessages(Messages messages)
        {
            Create(messages);
        }
        public void UpdateMessages(Messages messages)
        {
            Update(messages);
        }
        public void DeleteMessages(Messages messages)
        {
            Delete(messages);
        }
    }
    
}
