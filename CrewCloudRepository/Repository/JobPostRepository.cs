using CrewCloudRepository.Contracts;
using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrewCloudRepository.Repository
{
    public class JobPostRepository : RepositoryBase<JobPost>, IJobPostRepository
    {
        public JobPostRepository(CrewCloudDBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<JobPost> GetAllJobPosts()
        {
            return FindAll()
                .OrderBy(x => x.DateFrom)
                .ToList();
        }

        //public IEnumerable<JobPost> GetAllJobPostsFromRestaurant( string restaurantId)
        //{
        //    return FindByCondition(x => x.RestaurantId == restaurantId).OrderBy(x => x.DateFrom).ToList();
        //}

        public JobPost GetJobPostById(int id)
        {
            return FindByCondition(x => x.Id == id).FirstOrDefault();
        }
        public void CreateJobPost(JobPost jobPost)
        {
            Create(jobPost);
        }
        public void UpdateJobPost(JobPost jobPost)
        {
            Update(jobPost);
        }
        public void DeleteJobPost(JobPost jobPost)
        {
            Delete(jobPost);
        }
    }
}
