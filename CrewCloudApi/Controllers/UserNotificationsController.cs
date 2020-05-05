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
    
     public class UserNotificationsController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        private readonly ILogger<UserNotificationsController> _logger;

        private IMapper _mapper;

        private IHttpContextAccessor _httpContextAccessor;
        CustomResult result = new CustomResult();


       


        public UserNotificationsController(ILogger<UserNotificationsController> logger, IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        //// GET:api/UserNotifications
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Restaurant>>> GetUserNotification()
        //{
        //  //  CustomResult result = new CustomResult();
        //    PointF userLocation = new PointF()
        //    {
        //        X = Convert.ToSingle(user.Lat),
        //        Y = Convert.ToSingle(user.Long)
        //    };
        //    PointF retaurantLocation = new PointF();
        //    // var jobPosts = _repoWrapper.Restaurant.GetAllRestaurants().ToList();
        //    List<JobPostListItemVM> jobPostsResult = new List<JobPostListItemVM>();
            
        //    var nots = _repoWrapper.RestaurantUserNotifications.GetAllIRestaurantUserNotifications().Where(x => x.UserEmail == user.Email).ToList();
        //    if(!nots.Any())
        //    {
        //        result.Message = "There is no notification to show";
        //        result.StatusCode = ResultStatus.Error.ToString();
        //        return new JsonResult(result);
        //    }
        //    foreach (var item in nots)
        //    {
        //        if(item.UserVis)
        //        {
        //            var res = _repoWrapper.Restaurant.GetRestaurantById(item.RestaurantEmail);
        //            retaurantLocation.X = Convert.ToSingle(res.Lat);
        //            retaurantLocation.Y = Convert.ToSingle(res.Long);
        //            jobPostsResult.Add(new JobPostListItemVM
        //            {
        //                Email = res.Email,
        //                Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation)),
        //                City = res.City,
        //                Name = res.Name,
        //                Id= item.Id

        //            });

        //        }
        //    }
        //    if (!jobPostsResult.Any())
        //    {
        //        result.Message = "There is no notification to show";
        //        result.StatusCode = ResultStatus.Error.ToString();
        //        return new JsonResult(result);
        //    }


        //    jobPostsResult.OrderBy(x => x.Distance);
        //    result.Data = jobPostsResult;
        //    result.Message = "User notifications returned!";
        //    result.StatusCode = ResultStatus.Ok.ToString();
        //    return new JsonResult(result);
           
        //}

        //// Get: api/jobs/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Restaurant>> GetUserNotifications(string id)
        //{          


        //    try
        //    {
        //    var restaurant = _repoWrapper.Restaurant.GetRestaurantById(id);

        //    if (restaurant == null)
        //    {

        //            result.Message = "There is no notification with this data";
        //            result.StatusCode = ResultStatus.Error.ToString();

        //            return new JsonResult(result);
        //    } 
        //    restaurant = _repoWrapper.Restaurant.GetRestaurantById(id);
        //    _repoWrapper.RestaurantUserNotifications.Create(new RestaurantUserNotifications
        //         {
        //             UserEmail = user.Email,
        //             RestaurantEmail = restaurant.Email,
        //             Date = DateTime.Now
        //         });
        //     _repoWrapper.Save();
        //        result.Data = restaurant;
        //        result.Message = "User notification returned!";
        //        result.StatusCode = ResultStatus.Ok.ToString();

        //    }
        //    catch (Exception ex)
        //    {
        //        result.Message = ex.Message;
        //        result.Message = "User notification is not returned!";
        //        return new JsonResult(result);
        //    }
        //    return new JsonResult(result);           
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

        [HttpPost("{id}")]
        public async Task<ActionResult<JobPost>> SaveToNotifications(string id)
        {

           
                try
                {
                    var userLoged = _repoWrapper.User.GetUserById(User.Identity.Name);
                    var restaurant = _repoWrapper.Restaurant.GetRestaurantById(id);
                    if (restaurant == null)
                    {
                        result.Message = "There is no job with this id";
                        result.StatusCode = ResultStatus.Error.ToString();

                        return new JsonResult(result);
                    }

                    var notEx = _repoWrapper.RestaurantUserNotifications.FindAll().Where(x => x.RestaurantEmail == restaurant.Email)
                        .Where(x => x.UserEmail == userLoged.Email).FirstOrDefault();

                    if (notEx != null)
                    {
                        notEx.UserVis = true;
                        _repoWrapper.RestaurantUserNotifications.Update(notEx);
                        _repoWrapper.Save();
                        result.Data = notEx;
                        result.Message = "Restaurant added to notification screen";
                        result.StatusCode = ResultStatus.Ok.ToString();

                        return new JsonResult(result);
                    }

                    RestaurantUserNotifications not = new RestaurantUserNotifications
                    {
                        Date = DateTime.Now,
                        RestaurantEmail = id,
                        UserEmail = userLoged.Email,
                        UserVis = true,
                        RestaurantVis = false,
                        JobTitle = userLoged.Profession

                    };
                    _repoWrapper.RestaurantUserNotifications.Create(not);
                    _repoWrapper.Save();


                    var restaurantResult = _mapper.Map<RestaurantProfileScreenVM>(restaurant);

                    PointF userLocation = new PointF()
                    {
                        X = Convert.ToSingle(userLoged.Lat),
                        Y = Convert.ToSingle(userLoged.Long)
                    };


                    PointF retaurantLocation = new PointF()
                    {
                        X = Convert.ToSingle(restaurant.Lat),
                        Y = Convert.ToSingle(restaurant.Long)
                    };

                    restaurantResult.Jobs = _repoWrapper.Job.GetAllJobs().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();
                    restaurantResult.Images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();

                    var images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurant.RestaurantId);
                    foreach (var item in images)
                    {
                        restaurantResult.Images.Add(item);
                    }

                    restaurantResult.Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation));

                    result.Data = restaurantResult;
                    
                    result.StatusCode = ResultStatus.Ok.ToString();
                    result.Message = "Restaurant added to notification screen";
                    result.StatusCode = ResultStatus.Ok.ToString();

                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    result.Message = "User notification is not created!";
                    return new JsonResult(result);
                }
                return new JsonResult(result);
            
            

        }




        [HttpPost("{id}")]
        public async Task<ActionResult<JobPost>> ApplyForJob(int id)
        {


            try
            {
                var userLoged = _repoWrapper.User.GetUserById(User.Identity.Name);
                
                var job = _repoWrapper.Job.GetJobById(id);
               // var restaurant =_repoWrapper.Restaurant.GetRestaurantById(id);
                if (job == null)
                {
                    result.Message = "There is no job with this id";
                    result.StatusCode = ResultStatus.Error.ToString();

                    return new JsonResult(result);
                }



                RestaurantUserNotifications not = new RestaurantUserNotifications
                {
                    Date = DateTime.Now,
                    RestaurantEmail = _repoWrapper.Restaurant.GetRestaurantById(job.RestaurantId).Email,
                    UserEmail = User.Identity.Name,
                    UserVis = false,
                    RestaurantVis = true,
                    JobTitle = job.JobType

                };

                var notEx = _repoWrapper.RestaurantUserNotifications.FindAll().Where(x => x.RestaurantEmail == not.RestaurantEmail)
                   .Where(x => x.UserEmail ==User.Identity.Name).FirstOrDefault();

                if (notEx != null)
                {
                    notEx.UserVis = false;
                    notEx.RestaurantVis = true;
                    _repoWrapper.RestaurantUserNotifications.Update(notEx);
                    _repoWrapper.Save();
                    result.Data = notEx;
                    result.Message = "Application for job sent";
                    result.StatusCode = ResultStatus.Ok.ToString();

                    return new JsonResult(result);
                }

                _repoWrapper.RestaurantUserNotifications.Create(not);
                _repoWrapper.Save();
                var restaurant = _repoWrapper.Restaurant.GetRestaurantById(not.RestaurantEmail);

                var restaurantResult = _mapper.Map<RestaurantProfileScreenVM>(restaurant);

                restaurantResult.Jobs = _repoWrapper.Job.GetAllJobs().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();
                restaurantResult.Images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();

                var images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurant.RestaurantId);
                foreach (var item in images)
                {
                    restaurantResult.Images.Add(item);
                }
                PointF userLocation = new PointF()
                {
                    X = Convert.ToSingle(userLoged.Lat),
                    Y = Convert.ToSingle(userLoged.Long)
                };


                PointF retaurantLocation = new PointF()
                {
                    X = Convert.ToSingle(restaurant.Lat),
                    Y = Convert.ToSingle(restaurant.Long)
                };

                restaurantResult.Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation));

                result.Data = restaurantResult;
                result.Message = "Applayed for job";
                result.StatusCode = ResultStatus.Ok.ToString();

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Message = "User notification is not created!";
                return new JsonResult(result);
            }
            return new JsonResult(result);

        }

        [HttpPut]
        public async Task<ActionResult<JobPost>> DeleteAllUserNotifications()
        {
            try
            {
                var nots = _repoWrapper.RestaurantUserNotifications.GetAllIRestaurantUserNotifications().Where(x=>x.UserEmail == User.Identity.Name);
                if (nots == null)
                {
                    result.Message = "There is no notification with this data";
                    result.StatusCode = ResultStatus.Error.ToString();

                    return new JsonResult(result);
                }
                foreach(var item in nots)
                {
                    item.UserVis = false;
                    _repoWrapper.RestaurantUserNotifications.Update(item);
                    _repoWrapper.Save();
                }
               
              

                result.Data = nots;
                result.Message = "User notifications deleted!";
                result.StatusCode = ResultStatus.Ok.ToString();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Message = "User notifications are not deleted!";
                return new JsonResult(result);
            }
            return new JsonResult(result);
        }

        // DELETE: api/Messages/5
        [HttpPut("{id}")]
        public async Task<ActionResult<JobPost>> DeleteUserNotifications(int id)
        {
            try
            {
                var not = _repoWrapper.RestaurantUserNotifications.GetRestaurantUserNotificationsById(id);
                if (not == null)
                {
                    result.Message = "There is no notification with this data";
                    result.StatusCode = ResultStatus.Error.ToString();

                    return new JsonResult(result);
                }
                not.UserVis = false;
                _repoWrapper.RestaurantUserNotifications.UpdateRestaurantUserNotifications(not);
                _repoWrapper.Save();
                var nots = _repoWrapper.RestaurantUserNotifications.GetAllIRestaurantUserNotifications().Where(x => x.UserEmail == User.Identity.Name);
                result.Data = nots;
                result.Message = "User notification deleted!";
                result.StatusCode = ResultStatus.Ok.ToString();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Message = "User notification is not deleted!";
                return new JsonResult(result);
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
