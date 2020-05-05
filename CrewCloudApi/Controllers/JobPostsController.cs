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
using System.Drawing;

namespace CrewCloudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    class JobPostsController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        private readonly ILogger<JobPostsController> _logger;

        private IMapper _mapper;

        private IHttpContextAccessor _httpContextAccessor;

        public User user = new User()
        {
            CoverLetter = "asdsad",
            Long = 0,
            Lat = 0,
            FirstName = "Joca",
            LastName = "Pesic"
        };


        public JobPostsController(ILogger<JobPostsController> logger, IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        // GET:api/JobPosts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetJobPosts()
        {
            PointF userLocation = new PointF()
            {
                X = Convert.ToSingle(user.Lat),
                Y = Convert.ToSingle(user.Long)
            };

            PointF retaurantLocation = new PointF();

            var jobPosts = _repoWrapper.Restaurant.GetAllRestaurants().ToList();


            _logger.LogInformation("All Restaurants returned");
            //var restaurantsResult = _mapper.Map<IEnumerable<RestaurantListItemVM>>(restaurants);

            List<JobPostListItemVM> jobPostsResult = new List<JobPostListItemVM>();

            //RestaurantListItemVM listItem = new RestaurantListItemVM();

            foreach (var item in jobPosts)
            {
                retaurantLocation.X = Convert.ToSingle(item.Lat);
                retaurantLocation.Y = Convert.ToSingle(item.Long);

                //var Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation));
                JobPostListItemVM listItem = new JobPostListItemVM()
                {
                    Email = item.Email,
                    Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation)),
                    City = item.City,
                    Name = item.Name

                };

                jobPostsResult.Add(listItem);

            }
            jobPostsResult.OrderBy(x => x.Distance);
            return Ok(jobPostsResult);
        }

        // GET: api/jobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobPost>> GetJobPosts(int id)
        {
            try
            {
                var jobsPost = _repoWrapper.JobPost.GetJobPostById(id);
                if (jobsPost == null)
                {
                    _logger.LogInformation("job with that is not found");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned jobsPost with : {id}");

                    var jobPostResult = _mapper.Map<JobPostListItemVM>(jobsPost);
                    return Ok(jobPostResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }



        }

        // PUT: api/Job/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobPosts(int id, [FromBody]JobPostListItemVM job)
        {

            try
            {
                if (job == null)
                {
                    _logger.LogError("Job object sent from client is null.");
                    return BadRequest("Job object is null");
                }


                var JobPostEntity = _repoWrapper.JobPost.GetJobPostById(id);
                if (JobPostEntity == null)
                {
                    _logger.LogError($"jobPost with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                JobPost jobPost1 = _mapper.Map(job, JobPostEntity);

                _repoWrapper.JobPost.UpdateJobPost(jobPost1);
                _repoWrapper.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Job action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Messages

        [HttpPost]
        public async Task<ActionResult<JobPost>> PostJobPosts([FromBody]JobPostListItemVM job)
        {


            try
            {
                if (job == null)
                {
                    _logger.LogError("jobPost object sent from client is null.");
                    return BadRequest("jobPost object is null");
                }
                JobPost jobPostEntity = _mapper.Map<JobPost>(job);

                _repoWrapper.JobPost.CreateJobPost(jobPostEntity);
                _repoWrapper.Save();

                var createdJobPost = _mapper.Map<JobPostListItemVM>(jobPostEntity);

                return CreatedAtAction("GetJobsPost", new { id = jobPostEntity.Id }, createdJobPost);

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Something went wrong inside Messages action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }


        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<JobPost>> DeleteJobPosts(int id)
        {
            try
            {
                var jobPost = _repoWrapper.JobPost.GetJobPostById(id);
                if (jobPost == null)
                {
                    _logger.LogError($"job with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repoWrapper.JobPost.DeleteJobPost(jobPost);
                _repoWrapper.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Messages action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private static double GetDistance(PointF point1, PointF point2)
        {
            //pythagorean theorem c^2 = a^2 + b^2
            //thus c = square root(a^2 + b^2)
            double a = (double)(point2.X - point1.X);
            double b = (double)(point2.Y - point1.Y);

            return Math.Sqrt(a * a + b * b);
        }

    }
}
