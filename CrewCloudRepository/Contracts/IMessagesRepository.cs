using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrewCloudRepository.Contracts
{
    public interface IMessagesRepository : IRepositoryBase<Messages>
    {
        IEnumerable<Messages> GetAllMessages();
        Messages GetMessagesById(int id);
        void CreateMessages(Messages messages);
        void UpdateMessages(Messages messages);
        void DeleteMessages(Messages messages);
    }
}
