using CrewCloudRepository.Contracts;
using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrewCloudRepository.Repository
{
    public class JobRepository : RepositoryBase<Job>, IJobRepository
    {
        public JobRepository(CrewCloudDBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<Job> GetAllJobs()
        {
            return FindAll()
                //.OrderBy(x => x.DateFrom)
                .ToList();
        }

        //public IEnumerable<Job> GetAllJobsFromRestaurant(string userId, string restaurantId)
        //{
        //    return FindByCondition(x => x.RestaurantId == restaurantId&& x.UserId == userId).OrderBy(x => x.DateFrom).ToList();
        //}

        //public IEnumerable<Job> GetAllJobsFromUser(string userId, string restaurantId)
        //{
        //    return FindByCondition(x=> x.RestaurantId == restaurantId && x.UserId == userId).OrderBy(x => x.DateFrom).ToList();
        //}

        public Job GetJobById(int id)
        {
            return FindByCondition(x => x.Id == id).FirstOrDefault();
        }
        public void CreateJob(Job job)
        {
            Create(job);
        }
        public void UpdateJob(Job job)
        {
            Update(job);
        }
        public void DeleteJob(Job job)
        {
            Delete(job);
        }
    }
}
