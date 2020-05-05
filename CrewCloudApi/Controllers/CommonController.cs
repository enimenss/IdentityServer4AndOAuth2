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
using Microsoft.AspNetCore.Identity;
using System.Drawing;
//using CrewCloudMVC.IdentityModels;

namespace CrewCloudApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class CommonController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        private readonly ILogger<CommonController> _logger;

      //  private UserManager<AppUser> _userManager { get; }
        //(UserManager<IdentityUser> userManager,
        private IMapper _mapper;

        private IHttpContextAccessor _httpContextAccessor;

        private CustomResult result = new CustomResult();

        
        public CommonController( ILogger<CommonController> logger, IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            
            _repoWrapper = repoWrapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var name = User.Identity.Name;
            var res = _repoWrapper.Restaurant.GetRestaurantById(name);
            if (res!=null)
            {
                result.Message = "restaurant returned!";
                result.StatusCode = ResultStatus.Ok.ToString();
                return new JsonResult(result);
            }
            var s = User.Identity.Name;
           var k = User.IsInRole("Restaurant");
          //  result.Data = User.Identity;
          var j =  result.Message = "Users returned!";
            result.StatusCode = ResultStatus.Ok.ToString();
            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantUserNotifications>>> NotificationScreen()
        {
           
            if (User.IsInRole("User"))
            {
                User user = _repoWrapper.User.GetUserById(User.Identity.Name);
                //  CustomResult result = new CustomResult();
                PointF userLocation = new PointF()
                {
                    X = Convert.ToSingle(user.Lat),
                    Y = Convert.ToSingle(user.Long)
                };
                PointF retaurantLocation = new PointF();
                // var jobPosts = _repoWrapper.Restaurant.GetAllRestaurants().ToList();
                List<JobPostListItemVM> jobPostsResult = new List<JobPostListItemVM>();

                var nots = _repoWrapper.RestaurantUserNotifications.GetAllIRestaurantUserNotifications().Where(x => x.UserEmail == user.Email).ToList();
                if (!nots.Any())
                {
                    result.Message = "There is no notification to show";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }
                foreach (var item in nots)
                {
                    if (item.UserVis)
                    {
                        var res = _repoWrapper.Restaurant.GetRestaurantById(item.RestaurantEmail);
                        retaurantLocation.X = Convert.ToSingle(res.Lat);
                        retaurantLocation.Y = Convert.ToSingle(res.Long);
                        jobPostsResult.Add(new JobPostListItemVM
                        {
                            Email = res.Email,
                            Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation)),
                            City = res.City,
                            Name = res.Name,
                            Id = item.Id

                        });

                    }
                }
                if (!jobPostsResult.Any())
                {
                    result.Message = "There is no notification to show";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }


                jobPostsResult.OrderBy(x => x.Distance);
                result.Data = jobPostsResult;
                result.Message = "User notifications returned!";
                result.StatusCode = ResultStatus.Ok.ToString();
                return new JsonResult(result);

            }

            Restaurant restaurant = _repoWrapper.Restaurant.GetRestaurantById(User.Identity.Name);
            var notsRes = _repoWrapper.RestaurantUserNotifications.FindAll().Where(x=>x.RestaurantEmail == User.Identity.Name).Where(x=>x.RestaurantVis == true)
                .ToList();

           
            if (!notsRes.Any())
            {
                result.Message = "There are no notifications";
                result.StatusCode = ResultStatus.Error.ToString();
                return new JsonResult(result);
            }

            List<RestaurantNotificationVM> list =new List<RestaurantNotificationVM>();

           

            foreach (var item in notsRes)
            {
                var userEntity = _repoWrapper.User.GetUserById(item.UserEmail);

                var today = DateTime.Today;

                var age = today.Year - userEntity.DateOfBirth.Year;

                if (userEntity.DateOfBirth > today.AddYears(-age))
                    age--;

               

                list.Add(new RestaurantNotificationVM
                {
                    City = userEntity.City,
                    Email = userEntity.Email,
                    FirstName = userEntity.FirstName,
                    LastName = userEntity.LastName,
                    Profession = item.JobTitle,
                    Years = age,
                    Id = item.Id
                    

                });
            }

            result.Data = list;
            result.Message = "Notifications returned!";
            result.StatusCode = ResultStatus.Ok.ToString();
            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<ActionResult> ProfileScreen()
        {
            
            if (User.IsInRole("User"))
            {
                try
                {
                    User user = _repoWrapper.User.GetUserById(User.Identity.Name);
                    if (user == null)
                    {
                        result.Message = $"There is no user";
                        result.StatusCode = ResultStatus.Error.ToString();
                        return new JsonResult(result);
                    }

                   
                    var userResult = _mapper.Map<UserProfileScreenVM>(user);
                    var today = DateTime.Today;
                    var age = today.Year - user.DateOfBirth.Year;
                    if (user.DateOfBirth.Date > today.AddYears(-age))
                        age--;
                    userResult.Years = age;

                    userResult.Images = _repoWrapper.UserImage.GetAllImages().Where(x => x.UserId == user.Email).ToList();
                    result.Data = userResult;
                    result.Message = "User returned!";
                    result.StatusCode = ResultStatus.Ok.ToString();

                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    result.StatusCode = ResultStatus.Error.ToString();
                    //  result.Message = "User notification is not returned!";

                }

                return new JsonResult(result);

            }
           
            try
            {
                Restaurant restaurant = _repoWrapper.Restaurant.GetRestaurantById(User.Identity.Name);
                if (restaurant == null)
                {
                    result.Message = $"There is no restaurant!";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                PointF userLocation = new PointF()
                {
                    X = Convert.ToSingle(restaurant.UserLat),
                    Y = Convert.ToSingle(restaurant.UserLong)
                };


                PointF retaurantLocation = new PointF()
                {
                    X = Convert.ToSingle(restaurant.Lat),
                    Y = Convert.ToSingle(restaurant.Long)
                };


                var restaurantResult = _mapper.Map<RestaurantProfileScreenVM>(restaurant);

                restaurantResult.Jobs = _repoWrapper.Job.GetAllJobs().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();
                restaurantResult.Images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();

                var images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurant.RestaurantId);
                foreach (var item in images)
                {
                    restaurantResult.Images.Add(item);
                }

                restaurantResult.Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation));

                result.Data = restaurantResult;
                result.Message = "Restaurant returned!";
                result.StatusCode = ResultStatus.Ok.ToString();


            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();
            }

            return new JsonResult(result);

        }

        [HttpPost]
        public async Task<ActionResult> PostLocation([FromBody]LocationVM location)
        {
            
            if (User.IsInRole("User"))
            {
                
                try
                {
                    User user = _repoWrapper.User.GetUserById(User.Identity.Name);
                    if (user == null)
                    {
                        result.Message = $"There is no user logged";
                        result.StatusCode = ResultStatus.Error.ToString();
                        return new JsonResult(result);
                    }

                    user.Long = location.Long;
                    user.Lat = location.Lat;
                    _repoWrapper.User.UpdateUser(user);
                    _repoWrapper.Save();
                    var userResult = _mapper.Map<UserProfileScreenVM>(user);
                    var today = DateTime.Today;
                    var age = today.Year - user.DateOfBirth.Year;
                    if (user.DateOfBirth.Date > today.AddYears(-age))
                        age--;
                    userResult.Years = age;

                    userResult.Images = _repoWrapper.UserImage.GetAllImages().Where(x => x.UserId == user.Email).ToList();
                    result.Data = userResult;
                    result.Message = "LocationUpdated";
                    result.StatusCode = ResultStatus.Ok.ToString();

                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    result.StatusCode = ResultStatus.Error.ToString();
                    //  result.Message = "User notification is not returned!";

                }

                return new JsonResult(result);

            }
           
            try
            {
                Restaurant restaurant = _repoWrapper.Restaurant.GetRestaurantById(User.Identity.Name);
                if (restaurant == null)
                {
                    result.Message = $"There is no restaurant!";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                restaurant.UserLat = location.Lat;
                restaurant.UserLong = location.Long;
                _repoWrapper.Restaurant.UpdateRestaurant(restaurant);
                _repoWrapper.Save();

                var restaurantResult = _mapper.Map<RestaurantProfileScreenVM>(restaurant);

                restaurantResult.Jobs = _repoWrapper.Job.GetAllJobs().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();
                restaurantResult.Images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();

                var images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();
                

                

                result.Data = restaurantResult;
                result.Message = "Location updated!";
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
