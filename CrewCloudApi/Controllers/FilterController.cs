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
    [AllowAnonymous]
    public class FilterController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        private readonly ILogger<FilterController> _logger;

        private IMapper _mapper;

        private IHttpContextAccessor _httpContextAccessor;

        private CustomResult result = new CustomResult();

        public User user = new User()
        {
            CoverLetter = "asdsad",
            Long = 0,
            Lat = 0,
            FirstName = "Joca",
            LastName = "Pesic"
        };


        public FilterController(ILogger<FilterController> logger, IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        // GET: api/Filter
        // [Route("api/[controller]/[action]")]
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetFilter()
        {
            try
            {
                FilterVM item = new FilterVM
                {
                    Cities = _repoWrapper.Restaurant.GetAllRestaurants().Select(x => x.City).Distinct().ToList(),
                    Countries = _repoWrapper.Restaurant.GetAllRestaurants().Select(x => x.Country).Distinct().ToList(),
                 //   DailyPaids = _repoWrapper.Job.GetAllJobs().Select(x => x.DailyPaid).Distinct().ToList(),
                    JobTypes = _repoWrapper.Job.GetAllJobs().Select(x => x.JobType).Distinct().ToList(),
                   // MonthlyPaids = _repoWrapper.Job.GetAllJobs().Select(x => x.MonthlyPaid).Distinct().ToList()
                };
                result.Data = item;
                result.Message = "Restaurants returned!";
                result.StatusCode = ResultStatus.Ok.ToString();

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();
            }
                     

            return new JsonResult(result);
        }

      //  [Route("GetRestaurant")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurant([FromQuery]RestaurantParameters restaurantParameters)
        {

            PointF userLocation = new PointF()
            {
                X = Convert.ToSingle(user.Lat),
                Y = Convert.ToSingle(user.Long)
            };

            PointF retaurantLocation = new PointF();

            var restaurants = _repoWrapper.Restaurant.GetAllRestaurants().ToList();

           
            if (restaurants == null)
            {
                result.Message = "There are no restaurants in database";
                result.StatusCode = ResultStatus.Error.ToString();
                return new JsonResult(result);
            }



            List<RestaurantListItemVM> restaurantsResult = new List<RestaurantListItemVM>();

            //RestaurantListItemVM listItem = new RestaurantListItemVM();
            

            foreach (var item in restaurants)
            {
                retaurantLocation.X = Convert.ToSingle(item.Lat);
                retaurantLocation.Y = Convert.ToSingle(item.Long);

                var jobs = _repoWrapper.Job.GetAllJobs().Where(x => x.RestaurantId == item.RestaurantId).ToList();
                var a = jobs.Select(x => x.MonthlyPaid) ;
             //   int b = Convert.ToInt32(restaurantParameters.MonthlyPaid);
                if (jobs.Any())
                {
                    if (
                        ((jobs.Select(x => x.JobType).Contains(restaurantParameters.JobType)) || (restaurantParameters.JobType == ""))
                        && ((Convert.ToInt32(GetDistance(userLocation, retaurantLocation)) <= restaurantParameters.Radius) || (restaurantParameters.Radius == 0))
                       && ((item.City == restaurantParameters.City) || (restaurantParameters.City == ""))
                       && ((item.Country == restaurantParameters.Country) || (restaurantParameters.Country == ""))
                          && ((jobs.Any(x => x.MonthlyPaid < Convert.ToDouble(restaurantParameters.MonthlyPaid)) || (restaurantParameters.MonthlyPaid == 0)))
                           && ((jobs.Any(x => x.DailyPaid < Convert.ToDouble(restaurantParameters.DailyPaid)) || (restaurantParameters.DailyPaid == 0)))
                        )
                    {
                        RestaurantListItemVM listItem = new RestaurantListItemVM
                        {
                            Date = item.Date,
                            Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation)),
                            City = item.City,
                            Name = item.Name

                        };

                        restaurantsResult.Add(listItem);
                    }



                    /*&& Convert.ToInt32(GetDistance(userLocation, retaurantLocation)) <= restaurantParameters.Radius*/


                }



                //var Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation));
                //RestaurantListItemVM listItem = new RestaurantListItemVM
                //{
                //    Date = item.Date,
                //    Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation)),
                //    Location = item.Location,
                //    Name = item.Name

                //};

                //restaurantsResult.Add(listItem);

            }
            restaurantsResult.OrderBy(x => x.Distance);


            result.Data = restaurantsResult;
            result.Message = "Restaurants returned!";
            result.StatusCode = ResultStatus.Ok.ToString();
            return new JsonResult(result);
            // return Ok(restaurantsResult);
        }

        // GET: api/Restaurants/Name
        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetRestaurant(string id)
        {
            try
            {
                var restaurant = _repoWrapper.Restaurant.GetRestaurantById(id);
                if (restaurant == null)
                {
                    result.Message = $"There is no restaurant with Name {id}";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                _logger.LogInformation($"Returned restaurant with Name: {id}");

                PointF userLocation = new PointF()
                {
                    X = Convert.ToSingle(user.Lat),
                    Y = Convert.ToSingle(user.Long)
                };

                PointF retaurantLocation = new PointF()
                {
                    X = Convert.ToSingle(restaurant.Lat),
                    Y = Convert.ToSingle(restaurant.Long)
                };

                var restaurantResult = _mapper.Map<RestaurantProfileScreenVM>(restaurant);

                restaurantResult.Jobs = _repoWrapper.Job.GetAllJobs().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();
                restaurantResult.Images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();

                //foreach(var item in images)
                //{
                //    restaurantResult.Images.Add(item);
                //}
                restaurantResult.Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation));

                result.Data = restaurantResult;
                result.Message = "Restaurant returned!";
                result.StatusCode = ResultStatus.Ok.ToString();
                // return Ok(restaurantResult);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();
            }

            return new JsonResult(result);

        }

        // PUT: api/Restaurants/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRestaurant(string id, [FromBody]RestaurantUpdateVM restaurant)
        {

            try
            {
                if (restaurant == null)
                {
                    result.Message = $"Restaurant with Email {id} not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }


                var restaurantEntity = _repoWrapper.Restaurant.GetRestaurantById(id);
                if (restaurantEntity == null)
                {
                    result.Message = $"Restaurant with Email {id} not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                Restaurant restaurant1 = _mapper.Map(restaurant, restaurantEntity);

                _repoWrapper.Restaurant.UpdateRestaurant(restaurant1);
                _repoWrapper.Save();

                result.Data = restaurant1;
                result.Message = "Restaurant updated!";
                result.StatusCode = ResultStatus.Ok.ToString();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();
            }
            return new JsonResult(result);
        }

        // POST: api/Restaurant

        //[HttpPost]
        //public async Task<ActionResult<Restaurant>> PostRestaurant([FromBody]CreateRestaurantVM restaurant)
        //{


        //    try
        //    {
        //        if (restaurant == null)
        //        {
        //            result.Message = "Restaurant not found";
        //            result.StatusCode = ResultStatus.Error.ToString();
        //            return new JsonResult(result);
        //        }
        //        Restaurant restaurantEntity = _mapper.Map<Restaurant>(restaurant);

        //        _repoWrapper.Restaurant.CreateRestaurant(restaurantEntity);
        //        _repoWrapper.Save();

        //        var createdRestaurant = _mapper.Map<CreateRestaurantVM>(restaurantEntity);
        //        result.Data = createdRestaurant;
        //        result.Message = "User Created!";
        //        result.StatusCode = ResultStatus.Ok.ToString();
        //        //return CreatedAtAction("GetRestaurant", new { id = restaurant.Name }, createdRestaurant);

        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        result.Message = ex.Message;
        //        result.StatusCode = ResultStatus.Error.ToString();
        //    }
        //    return new JsonResult(result);


        //}

        // DELETE: api/Restaurants/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Restaurant>> DeleteRestaurant(string id)
        {
            try
            {
                var restaurant = _repoWrapper.Restaurant.GetRestaurantById(id);
                if (restaurant == null)
                {
                    result.Message = $"Restaurant not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                _repoWrapper.Restaurant.DeleteRestaurant(restaurant);
                _repoWrapper.Save();
                result.Data = restaurant;
                result.Message = "Restaurant deleted!";
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
