using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrewCloudRepository.Contracts
{
    public interface IJobRepository : IRepositoryBase<Job>
    {
        IEnumerable<Job> GetAllJobs();
        Job GetJobById(int id);
        void CreateJob(Job job);
        void UpdateJob(Job job);
        void DeleteJob(Job job);
    }
}
