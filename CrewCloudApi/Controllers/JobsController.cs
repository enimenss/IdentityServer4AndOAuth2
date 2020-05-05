using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrewCloudRepository.Models;
using CrewCloudRepository.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using CrewCloudApi.ViewModels;
using CrewCloudApi.Enums;
using System.Drawing;

namespace CrewCloudApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[AllowAnonymous]
    public class JobsController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        private readonly ILogger<JobsController> _logger;

        private IMapper _mapper;

        private IHttpContextAccessor _httpContextAccessor;


        private CustomResult result = new CustomResult();

        public JobsController(ILogger<JobsController> logger, IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        // GET: api/Jobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        {
            try
            {
             var jobs = _repoWrapper.Job.GetAllJobs().ToList();
                if (!jobs.Any())
                {
                    result.Message = "There are no jobs";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                var jobsResult = _mapper.Map<IEnumerable<JobListItemVM>>(jobs);
                result.Data = jobsResult;
                result.Message = "Jobs returned!";
                result.StatusCode = ResultStatus.Ok.ToString();
            }
            catch (DbUpdateException ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();
            }
            return new JsonResult(result);
        }

        // GET: api/jobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(int id)
        {
            try
            {
                var jobs = _repoWrapper.Job.GetJobById(id);
                if (jobs == null)
                {
                    result.Message = $"There is no job with id {id}";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }
                
                    _logger.LogInformation($"Returned job with : {id}");

                var jobResult = _mapper.Map<JobListItemVM>(jobs);
                result.Data = jobResult;
                result.Message = "Job returned!";
                result.StatusCode = ResultStatus.Ok.ToString();

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();
            }
            return new JsonResult(result);


        }

             
        [HttpPut]
        public async Task<IActionResult> UpdateJob([FromBody]JobListItemVM job)
        {
           try
            {
                if (job == null)
                {
                    result.Message = $"There is no job";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }


                var JobEntity = _repoWrapper.Job.GetJobById(job.Id);
                if (JobEntity == null)
                {
                    result.Message = $"There is no job";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                Job job1 = _mapper.Map(job, JobEntity);
                job1.Date = DateTime.Now;
                job1.RestaurantId = User.Identity.Name;
                _repoWrapper.Job.UpdateJob(job1);
                _repoWrapper.Save();

                Restaurant restaurantEntity = _repoWrapper.Restaurant.GetRestaurantById(User.Identity.Name);
                var restaurantResult = _mapper.Map<RestaurantProfileScreenVM>(restaurantEntity);
                restaurantResult.Jobs = _repoWrapper.Job.GetAllJobs().Where(x => x.RestaurantId == restaurantEntity.RestaurantId).ToList();
                restaurantResult.Images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurantEntity.RestaurantId).ToList();

                PointF userLocation = new PointF()
                {
                    X = Convert.ToSingle(job.Lat),
                    Y = Convert.ToSingle(job.Long)
                };


                PointF retaurantLocation = new PointF()
                {
                    X = Convert.ToSingle(restaurantEntity.Lat),
                    Y = Convert.ToSingle(restaurantEntity.Long)
                };

                restaurantResult.Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation));
                result.Data = restaurantResult;
                result.Message = "Job updated!";
                result.StatusCode = ResultStatus.Ok.ToString();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();
            }
            return new JsonResult(result);

        }

        // POST: api/Messages


        [HttpPost]
        public async Task<ActionResult<Job>> CreateJob([FromBody]CreateJobVM job)
        {

           Restaurant restaurantEntity = _repoWrapper.Restaurant.GetRestaurantById(User.Identity.Name);
            
            try
            {
                if (job == null)
                {
                    result.Message = "Job not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }
                if (restaurantEntity == null)
                {
                    result.Message = "Restaurant not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }
                Job jobEntity = _mapper.Map<Job>(job);
                jobEntity.RestaurantId = User.Identity.Name;
                jobEntity.Date = DateTime.Now;

                _repoWrapper.Job.CreateJob(jobEntity);
                _repoWrapper.Save();

                var restaurantResult = _mapper.Map<RestaurantProfileScreenVM>(restaurantEntity);
                restaurantResult.Jobs = _repoWrapper.Job.GetAllJobs().Where(x => x.RestaurantId == restaurantEntity.RestaurantId).ToList();
                restaurantResult.Images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurantEntity.RestaurantId).ToList();

                PointF userLocation = new PointF()
                {
                    X = Convert.ToSingle(restaurantEntity.UserLat),
                    Y = Convert.ToSingle(restaurantEntity.UserLong)
                };


                PointF retaurantLocation = new PointF()
                {
                    X = Convert.ToSingle(restaurantEntity.Lat),
                    Y = Convert.ToSingle(restaurantEntity.Long)
                };

                restaurantResult.Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation));
                result.Data = restaurantResult;
                result.Message = "Job created!";
                result.StatusCode = ResultStatus.Ok.ToString();
                //return CreatedAtAction("GetJobs", new { id = createdJob.Id }, createdJob);

            }
            catch (DbUpdateException ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();
            }
            return new JsonResult(result);

        }

        // DELETE: api/Messages/5
        [HttpDelete]
        public async Task<ActionResult<Job>> DeleteJob([FromBody]DeleteJobVM job)
        {
            try
            {
                var jobEntity = _repoWrapper.Job.GetJobById(job.Id);
                if (job == null)
                {
                    result.Message = "Job not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                _repoWrapper.Job.DeleteJob(jobEntity);
                _repoWrapper.Save();

                Restaurant restaurantEntity = _repoWrapper.Restaurant.GetRestaurantById(User.Identity.Name);
                var restaurantResult = _mapper.Map<RestaurantProfileScreenVM>(restaurantEntity);
                restaurantResult.Jobs = _repoWrapper.Job.GetAllJobs().Where(x => x.RestaurantId == restaurantEntity.RestaurantId).ToList();
                restaurantResult.Images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurantEntity.RestaurantId).ToList();

                PointF userLocation = new PointF()
                {
                    X = Convert.ToSingle(restaurantEntity.UserLat),
                    Y = Convert.ToSingle(restaurantEntity.UserLong)
                };


                PointF retaurantLocation = new PointF()
                {
                    X = Convert.ToSingle(restaurantEntity.Lat),
                    Y = Convert.ToSingle(restaurantEntity.Long)
                };

                restaurantResult.Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation));
                result.Data = restaurantResult;
                result.Message = "Job deleted!";
                result.StatusCode = ResultStatus.Ok.ToString();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();
            }
            return new JsonResult(result);
        }
        private static double GetDistance(PointF point1, PointF point2)
        {

            double a = (double)(point2.X - point1.X);
            double b = (double)(point2.Y - point1.Y);

            return Math.Sqrt(a * a + b * b);
        }

    }
}
