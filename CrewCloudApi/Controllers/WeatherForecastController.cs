using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CrewCloudRepository.Contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CrewCloudApi.Controllers
{

    public class testt
    {
        public string Email { get; set; }
        public string Username { get; set; }
       public string Password { get; set; }
    }
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private IRepositoryWrapper _repoWrapper;

        private readonly ILogger<WeatherForecastController> _logger;


        private IHttpContextAccessor _httpContextAccessor;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }



        [HttpGet]
    
      
        public IEnumerable<WeatherForecast> Get(string Id)
        {
            Exception ex = new Exception("omg");
            _logger.LogInformation("Info logging OKK");
            _logger.LogError(ex, "errOrTU");
            var test= User.Identity.Name;
           //var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            //var roles = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var ok = User.IsInRole("Admincina");
            var ok1 = User.IsInRole("Ok");
            var ok2 = User.IsInRole("Restaurant");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }


        //[HttpPost]
        //[AllowAnonymous]
        //public IEnumerable<WeatherForecast> Post([FromForm]testt ok3)
        //{
        //    var test = User.Identity.Name;
        //    //var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
        //    //var roles = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
        //    var ok = User.IsInRole("Admincina");
        //    var ok1 = User.IsInRole("Admincinaa");
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpPost]
        [AllowAnonymous]
        public IEnumerable<WeatherForecast> Post([FromForm]testt ok3)
        {
            var test = User.Identity.Name;
            //var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            //var roles = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var ok = User.IsInRole("Admincina");
            var ok1 = User.IsInRole("Admincinaa");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        //[HttpGet("{nameContained}")]       
        //public IEnumerable<RestaurantVM> GetRestaurants(string nameContained)
        //{
        //    return _repoWrapper.Restaurant.GetRestaurants(nameContained);
        //}
        [HttpGet("{nameContained}")]
        public string GetRestaurants(string nameContained)
        {
            return User.Identity.Name; 
        }
    }
}
