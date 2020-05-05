using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrewCloudRepository.Contracts
{
    public interface IJobPostRepository : IRepositoryBase<JobPost>
    {
        IEnumerable<JobPost> GetAllJobPosts();
        JobPost GetJobPostById(int id);
        void CreateJobPost(JobPost jobPost);
        void UpdateJobPost(JobPost jobPost);
        void DeleteJobPost(JobPost jobPost);
    }
}
