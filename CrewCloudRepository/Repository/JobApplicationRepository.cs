using CrewCloudRepository.Contracts;
using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrewCloudRepository.Repository
{
    public class JobApplicationRepository : RepositoryBase<JobApplication>, IJobApplicationRepository
    {
        public JobApplicationRepository(CrewCloudDBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<JobApplication> GetAllJobApplications()
        {
            return FindAll()
                .OrderBy(x => x.Id)
                .ToList();
        }

        //public IEnumerable<JobApplication> GetAllJobsApplicationFromRestaurant(string userId, string restaurantId)
        //{
        //    return FindByCondition(x => x.RestaurantId == restaurantId&& x.UserId == userId).OrderBy(x => x.Id).ToList();
        //}

        //public IEnumerable<JobApplication> GetAllJobsApplicationFromUser(string userId, string restaurantId)
        //{
        //    return FindByCondition(x=> x.RestaurantId == restaurantId&& x.UserId == userId).OrderBy(x => x.Id).ToList();
        //}

        public JobApplication GetJobApplicationById(int id)
        {
            return FindByCondition(x => x.Id == id).FirstOrDefault();
        }
        public void CreateJobApplication(JobApplication jobApplication)
        {
            Create(jobApplication);
        }
        public void UpdateJobApplication(JobApplication jobApplication)
        {
            Update(jobApplication);
        }
        public void DeleteJobApplication(JobApplication jobApplication)
        {
            Delete(jobApplication);
        }
    }
}
