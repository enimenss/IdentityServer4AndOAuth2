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
 //   [AllowAnonymous]
    public class RestaurantsController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        private readonly ILogger<RestaurantsController> _logger;

        private IMapper _mapper;

        private IHttpContextAccessor _httpContextAccessor;

        private CustomResult result = new CustomResult();

        public User user = new User()
        {
            CoverLetter = "asdsad",
            Long = 0,
            Lat = 0,
            FirstName = "Joca",
            LastName = "Pesic",
            City = "Nis"
        };


        public RestaurantsController(ILogger<RestaurantsController> logger, IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Restaurant>>> GetFilter()
        {
            try
            {
                FilterVM item = new FilterVM
                {
                    Radiuses = new List<int>(){500, 1000, 1500, 2500, 3500, 5000, 10000 },
                    Cities = _repoWrapper.Restaurant.GetAllRestaurants().Select(x => x.City).Distinct().ToList(),
                    Countries = _repoWrapper.Restaurant.GetAllRestaurants().Select(x => x.Country).Distinct().ToList(),
                    //   DailyPaids = _repoWrapper.Job.GetAllJobs().Select(x => x.DailyPaid).Distinct().ToList(),
                    JobTypes = _repoWrapper.Job.GetAllJobs().Select(x => x.JobType).Distinct().ToList(),
                    // MonthlyPaids = _repoWrapper.Job.GetAllJobs().Select(x => x.MonthlyPaid).Distinct().ToList()
                };
                result.Data = item;
                result.Message = "Filter parameters returned!";
                result.StatusCode = ResultStatus.Ok.ToString();

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();
            }


            return new JsonResult(result);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> FilterResult([FromQuery]RestaurantParameters restaurantParameters)
        {
            var logedUser = _repoWrapper.User.GetUserById(User.Identity.Name);
            PointF userLocation = new PointF()
            {
                X = Convert.ToSingle(logedUser.Lat),
                Y = Convert.ToSingle(logedUser.Long)
            };

            PointF retaurantLocation = new PointF();

            var restaurants = _repoWrapper.Restaurant.GetAllRestaurants().ToList();

            if (restaurants == null)
            {
                result.Message = "There are no restaurants in database";
                result.StatusCode = ResultStatus.Error.ToString();
                return new JsonResult(result);
            }

            if(restaurantParameters.City == "")
            {
                restaurantParameters.City = logedUser.City;
            }
            List<RestaurantListItemVM> restaurantsResult = new List<RestaurantListItemVM>();

            //RestaurantListItemVM listItem = new RestaurantListItemVM();


            foreach (var item in restaurants)
            {
                retaurantLocation.X = Convert.ToSingle(item.Lat);
                retaurantLocation.Y = Convert.ToSingle(item.Long);

                var jobs = _repoWrapper.Job.GetAllJobs().Where(x => x.RestaurantId == item.RestaurantId).ToList();
                var a = jobs.Select(x => x.MonthlyPaid);
                //   int b = Convert.ToInt32(restaurantParameters.MonthlyPaid);
               
                    if (
                        ((jobs.Select(x => x.JobType).Contains(restaurantParameters.JobType)) || (restaurantParameters.JobType == ""))
                        && ((Convert.ToInt32(GetDistance(userLocation, retaurantLocation)) >= restaurantParameters.Radius) || (restaurantParameters.Radius == 0))
                       && ((item.City == restaurantParameters.City) || (restaurantParameters.City == ""))
                       && ((item.Country == restaurantParameters.Country) || (restaurantParameters.Country == ""))
                          && ((jobs.Any(x => x.MonthlyPaid >= Convert.ToDouble(restaurantParameters.MonthlyPaid)) || (restaurantParameters.MonthlyPaid == 0)))
                           && ((jobs.Any(x => x.DailyPaid >= Convert.ToDouble(restaurantParameters.DailyPaid)) || (restaurantParameters.DailyPaid == 0)))
                        )
                    {
                        RestaurantListItemVM listItem = new RestaurantListItemVM
                        {
                            RestaurantId = item.RestaurantId,
                            Date = item.Date,
                            Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation)),
                            City = item.City,
                            Name = item.Name

                        };

                        restaurantsResult.Add(listItem);
                    }

            }
            restaurantsResult.OrderBy(x => x.Distance);

            if (!restaurantsResult.Any())
            {
                result.Message = "No restraurants with these parameters";
                result.StatusCode = ResultStatus.Error.ToString();
                return new JsonResult(result);
            }
            result.Data = restaurantsResult;
            result.Message = "Restaurants returned!";
            result.StatusCode = ResultStatus.Ok.ToString();
            return new JsonResult(result);
            // return Ok(restaurantsResult);
        }



        // GET: api/Restaurants/GetAllRestaurants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetAllRestaurants()
        {
            var logedUser = _repoWrapper.User.GetUserById(User.Identity.Name);

            PointF userLocation = new PointF()
            {
                X = Convert.ToSingle(logedUser.Lat),
                Y = Convert.ToSingle(logedUser.Long)
            };

            PointF retaurantLocation = new PointF();

            var restaurants = _repoWrapper.Restaurant.GetAllRestaurants().ToList();

            //foreach(var item in restaurants)
            //{
            //    _repoWrapper.Job.GetAllJobs().Where(x => x.RestaurantId == item.RestaurantId);
            //}
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


                RestaurantListItemVM listItem = new RestaurantListItemVM
                {
                    RestaurantId = item.RestaurantId,
                    Date = item.Date,
                    Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation)),
                    City = item.City,
                    Name = item.Name

                };

                restaurantsResult.Add(listItem);
            }
           
            restaurantsResult.OrderBy(x => x.Distance);
            result.Data = restaurantsResult;
            result.Message = "Restaurants returned!";
            result.StatusCode = ResultStatus.Ok.ToString();
            return new JsonResult(result);

        }

        // GET: api/Restaurants/GetRestaurantsById
        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetRestaurantById(string id)
        {

            try
            {
                var logedUser = _repoWrapper.User.GetUserById(User.Identity.Name);
                var restaurant = _repoWrapper.Restaurant.GetRestaurantById(id);
                if (restaurant == null)
                {
                    result.Message = $"There are no restaurants with id {id}";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                PointF userLocation = new PointF()
                {
                    X = Convert.ToSingle(logedUser.Lat),
                    Y = Convert.ToSingle(logedUser.Long)
                };
                
                
                    PointF retaurantLocation = new PointF()
                    {
                        X = Convert.ToSingle(restaurant.Lat),
                        Y = Convert.ToSingle(restaurant.Long)
                    };

  
                  var restaurantResult = _mapper.Map<RestaurantProfileScreenVM>(restaurant);

                restaurantResult.Jobs=   _repoWrapper.Job.GetAllJobs().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();
                restaurantResult.Images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();

                //var images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurant.RestaurantId);
                //foreach(var item in images)
                //{
                //    restaurantResult.Images.Add(item);
                //}
               
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


        // GET: api/Restaurants/GetRestaurantsByName
        [HttpGet]
        public async Task<ActionResult<Restaurant>> GetRestaurantsByName([FromBody]RestaurantName restaurantName)
        {
            
            try
            {
                var logedUser = _repoWrapper.User.GetUserById(User.Identity.Name);
                var restaurants = _repoWrapper.Restaurant.GetAllRestaurants().Where(x=>x.Name.ToLower().Contains(restaurantName.name.ToLower())).ToList();
                if (!restaurants.Any())
                {
                    result.Message = $"There are no restaurants with Name {restaurantName.name}";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }
                                                  
                    PointF userLocation = new PointF()
                    {
                        X = Convert.ToSingle(logedUser.Lat),
                        Y = Convert.ToSingle(logedUser.Long)
                    };
                List<RestaurantListItemVM> restaurantsItems = new List<RestaurantListItemVM>();
                foreach(var item in restaurants)
                {
                    PointF retaurantLocation = new PointF()
                    {
                        X = Convert.ToSingle(item.Lat),
                        Y = Convert.ToSingle(item.Long)
                    };

                    RestaurantListItemVM restaurantListItemVM = new RestaurantListItemVM
                    {
                        RestaurantId = item.RestaurantId,
                        City = item.City,
                        Date = item.Date,
                        Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation)),
                        Name = item.Name
                    };
                    restaurantsItems.Add(restaurantListItemVM);

                }

                    

                 //   var restaurantResult = _mapper.Map<RestaurantProfileScreenVM>(restaurant);

                //restaurantResult.Jobs=   _repoWrapper.Job.GetAllJobs().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();
                //restaurantResult.Images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();

                //foreach(var item in images)
                //{
                //    restaurantResult.Images.Add(item);
                //}
               //restaurantResult.Distance = Convert.ToInt32(GetDistance(userLocation, retaurantLocation));

                result.Data = restaurantsItems;
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
        
        [HttpPut]
        public async Task<IActionResult> ChangeAddress([FromBody]RestaurantUpdateVM restaurant)
        {
            
            try
            {
                if (restaurant == null)
                {
                    result.Message = $"Restaurant with Email  not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }


                var restaurantEntity = _repoWrapper.Restaurant.GetRestaurantById(User.Identity.Name);
                if (restaurantEntity == null)
                {
                    result.Message = $"Restaurant with Email  not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                restaurantEntity.Address = restaurant.Address;
                restaurantEntity.City = restaurant.City;
                restaurantEntity.City = restaurant.Contry;

                _repoWrapper.Restaurant.UpdateRestaurant(restaurantEntity);
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
        [HttpPut]
        public async Task<IActionResult> ChangeDescription([FromBody]ChangeDescription restaurant)
        {

            try
            {
                if (restaurant == null)
                {
                    result.Message = $"Description is empty";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }


                var restaurantEntity = _repoWrapper.Restaurant.GetRestaurantById(User.Identity.Name);
                if (restaurantEntity == null)
                {
                    result.Message = $"Restaurant with Email  not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                restaurantEntity.Description = restaurant.Description;
                

                _repoWrapper.Restaurant.UpdateRestaurant(restaurantEntity);
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

        [HttpPut]
        public async Task<IActionResult> ChangeRestaurantName([FromBody]ChangeRestaurantNameVM name)
        {

            try
            {
                if (name == null)
                {
                    result.Message = $"Name is empty";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }


                var restaurantEntity = _repoWrapper.Restaurant.GetRestaurantById(User.Identity.Name);
                if (restaurantEntity == null)
                {
                    result.Message = $"Restaurant with Email  not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                restaurantEntity.Name = name.Name;

                _repoWrapper.Restaurant.UpdateRestaurant(restaurantEntity);
                _repoWrapper.Save();

                

                var restaurantResult = _mapper.Map<RestaurantProfileScreenVM>(restaurantEntity);
                restaurantResult.Jobs = _repoWrapper.Job.GetAllJobs().Where(x => x.RestaurantId == restaurantEntity.RestaurantId).ToList();
                restaurantResult.Images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurantEntity.RestaurantId).ToList();


                
                result.Data = restaurantResult;
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

        [HttpPost]
        public async Task<ActionResult<Restaurant>> PostRestaurant([FromBody]CreateRestaurantVM restaurant)
        {

           
            try
            {
                if (restaurant == null)
                {
                    result.Message = "Restaurant not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }
                Restaurant restaurantEntity = _mapper.Map<Restaurant>(restaurant);

                _repoWrapper.Restaurant.CreateRestaurant(restaurantEntity);
                _repoWrapper.Save();

                var createdRestaurant = _mapper.Map<CreateRestaurantVM>(restaurantEntity);
                result.Data = createdRestaurant;
                result.Message = "User Created!";
                result.StatusCode = ResultStatus.Ok.ToString();
                //return CreatedAtAction("GetRestaurant", new { id = restaurant.Name }, createdRestaurant);

            }
            catch (DbUpdateException ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();
            }
            return new JsonResult(result);


        }

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
               var images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == id);
                foreach(var item in images)
                {
                    _repoWrapper.RestaurantImage.Delete(item);
                }
                var jobs = _repoWrapper.Job.GetAllJobs().Where(x => x.RestaurantId == id);
                foreach (var item in jobs)
                {
                    _repoWrapper.Job.Delete(item);
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
