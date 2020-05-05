using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrewCloudRepository.Contracts
{
    public interface IJobApplicationRepository : IRepositoryBase<JobApplication>
    {
        IEnumerable<JobApplication> GetAllJobApplications();
        JobApplication GetJobApplicationById(int id);
        void CreateJobApplication(JobApplication jobApplication);
        void UpdateJobApplication(JobApplication jobApplication);
        void DeleteJobApplication(JobApplication jobApplication);
    }
}
