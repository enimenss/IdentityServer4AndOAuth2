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

namespace CrewCloudApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class RestaurantImagesController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        private readonly ILogger<RestaurantImagesController> _logger;

        private IMapper _mapper;

        private IHttpContextAccessor _httpContextAccessor;

        private CustomResult result = new CustomResult();
        public RestaurantImagesController(ILogger<RestaurantImagesController> logger, IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public Restaurant restaurant = new Restaurant()
        {
            Email = "piksla@gmail.com",
            Name = "asdsad",
            Long = 0,
            Lat = 0,

        };

        // GET: api/Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantImage>>> GetImage()
        {
            var images = _repoWrapper.RestaurantImage.GetAllImages().ToList();
            if (images == null)
            {
                result.Message = "There are no images";
                result.StatusCode = ResultStatus.Error.ToString();
                return new JsonResult(result);
            }


            var imagesResult = _mapper.Map<IEnumerable<ImageListItemVM>>(images);
            
            result.Data = imagesResult;
            result.Message = "Images returned!";
            result.StatusCode = ResultStatus.Ok.ToString();
            return new JsonResult(result);
        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantImage>> GetImage(int id)
        {
            try
            {
                var image = _repoWrapper.RestaurantImage.GetImageById(id);
                if (image == null)
                {
                    result.Message = $"There is no Image with id {id}";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                _logger.LogInformation($"Returned image with email: {id}");
                var imageResult = _mapper.Map<ImageListItemVM>(image);
                var today = DateTime.Today;
                
                result.Data = imageResult;
                result.Message = "Image returned!";
                result.StatusCode = ResultStatus.Ok.ToString();

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();
                

            }

            return new JsonResult(result);

        }

        // PUT: api/Images/5       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage(int id, [FromBody]ImageListItemVM image)
        {
            try
            {
                if (image == null)
                {
                    result.Message = $"Image with id {id} not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }


                var imageEntity = _repoWrapper.RestaurantImage.GetImageById(id);
                if (imageEntity == null)
                {
                    result.Message = $"Image with id {id} not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                RestaurantImage image1 = _mapper.Map(image, imageEntity);
                
                _repoWrapper.RestaurantImage.UpdateImage(image1);
                _repoWrapper.Save();

                result.Data = image1;
                result.Message = "Image updated!";
                result.StatusCode = ResultStatus.Ok.ToString();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();
                
            }
            return new JsonResult(result);
        }

        // POST: api/Images
        [HttpPost]
        public async Task<ActionResult<RestaurantImage>> UploadImage([FromBody]ImageListItemVM image)
        {
            try
            {
                if (image == null)
                {
                    result.Message = "Image not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }
                // RestaurantImage imageEntity = _mapper.Map<RestaurantImage>(image);

                RestaurantImage imageEntity = new RestaurantImage
                {
                    Date = DateTime.Now,
                    ImageUrl = image.ImageUrl,
                    Name = _repoWrapper.Restaurant.GetRestaurantById(User.Identity.Name).Name + DateTime.Now,
                    Picture = "blob",
                    RestaurantId = User.Identity.Name

                };
                _repoWrapper.RestaurantImage.CreateImage(imageEntity);
                _repoWrapper.Save();

                restaurant = _repoWrapper.Restaurant.GetRestaurantById(User.Identity.Name);

                var restaurantResult = _mapper.Map<RestaurantProfileScreenVM>(restaurant);
                restaurantResult.Jobs = _repoWrapper.Job.GetAllJobs().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();
                restaurantResult.Images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();


                result.Data = restaurantResult;
                result.Message = "Image Uploaded!";
                result.StatusCode = ResultStatus.Ok.ToString();
            }
            catch (DbUpdateException ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();
               
            }

            return new JsonResult(result);
        }

        // DELETE: api/Images/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RestaurantImage>> DeleteImage(int id)
        {
            try
            {
                var image = _repoWrapper.RestaurantImage.GetImageById(id);
                if (image == null)
                {
                    result.Message = $"Image not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                _repoWrapper.RestaurantImage.DeleteImage(image);
                _repoWrapper.Save();

                restaurant = _repoWrapper.Restaurant.GetRestaurantById(User.Identity.Name);

                var restaurantResult = _mapper.Map<RestaurantProfileScreenVM>(restaurant);
                restaurantResult.Jobs = _repoWrapper.Job.GetAllJobs().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();
                restaurantResult.Images = _repoWrapper.RestaurantImage.GetAllImages().Where(x => x.RestaurantId == restaurant.RestaurantId).ToList();


                result.Data = restaurantResult;
                result.Message = "Image deleted!";
                result.StatusCode = ResultStatus.Ok.ToString();
                // return NoContent();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();
            }
            return new JsonResult(result);
        }

        private bool ImageExists(int id)
        {
            return _repoWrapper.RestaurantImage.FindByCondition(e => e.Id == id).Any();
        }
    }
}
