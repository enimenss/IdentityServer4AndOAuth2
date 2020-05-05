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
using CrewCloudApi.Enums;

namespace CrewCloudApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
   // [AllowAnonymous]
    public class RestaurantNotificationsController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        private readonly ILogger<RestaurantNotificationsController> _logger;

        private IMapper _mapper;

        private IHttpContextAccessor _httpContextAccessor;

        private CustomResult result = new CustomResult();

        public Restaurant restaurant = new Restaurant()
        {
            Email = "pitanga@palma.com",
            Name = "asdsad",
            Long = 0,
            Lat = 0,
            
        };


        public RestaurantNotificationsController(ILogger<RestaurantNotificationsController> logger, IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        // GET:api/UserNotifications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantNotificationListItemVM>>> GetRestaurantNotifications()
        {

            var con =_repoWrapper.RestaurantUserNotifications.FindAll();

            var users = _repoWrapper.User.GetAllUsers().ToList();
            if (users == null)
            {
                result.Message = "There are no users";
                result.StatusCode = ResultStatus.Error.ToString();
                return new JsonResult(result);
            }
                
            _logger.LogInformation("All users returned");
            var usersResult = _mapper.Map<IEnumerable<RestaurantNotificationListItemVM>>(users).ToList();
            List<RestaurantNotificationListItemVM> usersResult1 = new List<RestaurantNotificationListItemVM>();
            

            foreach (var item in usersResult)
            {
                var today = DateTime.Today;

                var age = today.Year - item.DateOfBirth.Year;

                if (item.DateOfBirth > today.AddYears(-age))
                    age--;

                item.Years = age;
                
                foreach (var c in con)
                {
                    if ((c.RestaurantEmail == restaurant.Email) && (item.Email == c.UserEmail) && (c.RestaurantVis == true))
                    {
                        item.NotificationId = c.Id;
                        usersResult1.Add(item);
                    }

                }
            }
            result.Data = usersResult1;
            result.Message = "Notifications returned!";
            result.StatusCode = ResultStatus.Ok.ToString();
            return new JsonResult(result);
            
        }

        // GET: api/jobs/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<JobPost>> GetUserNotifications(int id)
        //{
        //    try
        //    {
        //        var jobsPost = _repoWrapper.JobPost.GetJobPostById(id);
        //        if (jobsPost == null)
        //        {
        //            _logger.LogInformation("job with that is not found");
        //            return NotFound();
        //        }
        //        else
        //        {
        //            _logger.LogInformation($"Returned jobsPost with : {id}");

        //            var jobPostResult = _mapper.Map<JobPostListItemVM>(jobsPost);
        //            return Ok(jobPostResult);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Something went wrong inside GetOwnerById action: {ex.Message}");
        //        return StatusCode(500, "Internal server error");
        //    }



        //}

        //// PUT: api/Job/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUserNotifications(int id, [FromBody]JobPostListItemVM job)
        //{

        //    try
        //    {
        //        if (job == null)
        //        {
        //            _logger.LogError("Job object sent from client is null.");
        //            return BadRequest("Job object is null");
        //        }


        //        var JobPostEntity = _repoWrapper.JobPost.GetJobPostById(id);
        //        if (JobPostEntity == null)
        //        {
        //            _logger.LogError($"jobPost with id: {id}, hasn't been found in db.");
        //            return NotFound();
        //        }

        //        JobPost jobPost1 = _mapper.Map(job, JobPostEntity);

        //        _repoWrapper.JobPost.UpdateJobPost(jobPost1);
        //        _repoWrapper.Save();

        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Something went wrong inside Job action: {ex.Message}");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //// POST: api/Messages

        //[HttpPost]
        //public async Task<ActionResult<JobPost>> PostUserNotifications([FromBody]JobPostListItemVM job)
        //{


        //    try
        //    {
        //        if (job == null)
        //        {
        //            _logger.LogError("jobPost object sent from client is null.");
        //            return BadRequest("jobPost object is null");
        //        }
        //        JobPost jobPostEntity = _mapper.Map<JobPost>(job);

        //        _repoWrapper.JobPost.CreateJobPost(jobPostEntity);
        //        _repoWrapper.Save();

        //        var createdJobPost = _mapper.Map<JobPostListItemVM>(jobPostEntity);

        //        return CreatedAtAction("GetJobsPost", new { id = jobPostEntity.Id }, createdJobPost);

        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        _logger.LogError($"Something went wrong inside Messages action: {ex.Message}");
        //        return StatusCode(500, "Internal server error");
        //    }


        //}

        // DELETE: api/RestaurantNotifications/5

        [HttpDelete]
        public async Task<ActionResult<JobPost>> DeleteAllRestaurantNotifications()
        {
            try
            {
                var notifications = _repoWrapper.RestaurantUserNotifications.GetAllIRestaurantUserNotifications()
                    .Where(x=>x.RestaurantEmail == User.Identity.Name).ToList();
                if (!notifications.Any())
                {
                    result.Message = $"There are no notifications";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }
                foreach(var item in notifications)
                {
                    item.RestaurantVis = false;
                    _repoWrapper.RestaurantUserNotifications.Update(item);
                    _repoWrapper.Save();
                }
                

                result.Data = notifications;
                result.Message = "Notifications has been removed!";
                result.StatusCode = ResultStatus.Ok.ToString();

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();
            }
            return new JsonResult(result);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<JobPost>> DeleteRestaurantNotification(int id)
        {
            try
            {
                var notification = _repoWrapper.RestaurantUserNotifications.GetRestaurantUserNotificationsById(id);
                if (notification == null)
                {
                    result.Message = $"There is no notification with id {id}";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                notification.RestaurantVis = false;
                _repoWrapper.RestaurantUserNotifications.Update(notification);
                _repoWrapper.Save();

                result.Data = notification;
                result.Message = "Notification has been removed!";
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
            //pythagorean theorem c^2 = a^2 + b^2
            //thus c = square root(a^2 + b^2)
            double a = (double)(point2.X - point1.X);
            double b = (double)(point2.Y - point1.Y);

            return Math.Sqrt(a * a + b * b);
        }

    }
}
