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
   // [AllowAnonymous]
    public class UserImagesController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        private readonly ILogger<UserImagesController> _logger;

        private IMapper _mapper;

        private IHttpContextAccessor _httpContextAccessor;

        private CustomResult result = new CustomResult();
        public UserImagesController(ILogger<UserImagesController> logger, IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        // GET: api/Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserImage>>> GetImage()
        {
            var images = _repoWrapper.UserImage.GetAllImages().ToList().Where(x=>x.UserId == User.Identity.Name);
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
        [HttpPost]
        public async Task<IActionResult> AddProfilePicture(CreateUserImageVM image)
        {
            try
            {
                if (image == null)
                {
                    result.Message = "Image not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }
                var currentProfile = _repoWrapper.UserImage.GetAllImages()
                    .Where(x => x.UserId == User.Identity.Name)
                    .Where(x => x.IsProfile == true).SingleOrDefault();
                if (currentProfile!=null)
                {
                    currentProfile.IsProfile = false;
                    _repoWrapper.UserImage.Update(currentProfile);
                    _repoWrapper.Save();
                }

                UserImage imageEntity = new UserImage
                {
                    Date = DateTime.Now,
                    ImageUrl = image.ImageUrl,
                    IsProfile = true,
                    Name = _repoWrapper.User.GetUserById(User.Identity.Name).FirstName + " " + DateTime.Now.ToString(),
                    Picture = "Blob",
                    UserId = User.Identity.Name

                };
                _repoWrapper.UserImage.CreateImage(imageEntity);
                _repoWrapper.Save();
                var u = _repoWrapper.User.GetUserById(User.Identity.Name);
                u.ProfilPictureId = _repoWrapper.UserImage.GetAllImages()
                    .Where(x => x.UserId == User.Identity.Name)
                    .Where(x=>x.IsProfile == true).SingleOrDefault().Id;

                _repoWrapper.User.Update(u);
                _repoWrapper.Save();





                var images = _repoWrapper.UserImage.GetAllImages().Where(x => x.UserId == User.Identity.Name).ToList();
                var userReturn = _repoWrapper.User.GetUserById(User.Identity.Name);
                var userResult = _mapper.Map<UserProfileScreenVM>(userReturn);
                userResult.Images = images;
                
                result.Data = userResult;
                result.Message = "Image Created!";
                result.StatusCode = ResultStatus.Ok.ToString();
            }
            catch (DbUpdateException ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();

            }

            return new JsonResult(result);
        }
    
        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeProfilePicture(int id)
        {
            try
            {
                var images = _repoWrapper.UserImage.GetAllImages().Where(x => x.UserId == User.Identity.Name).ToList();
                //if(!images.Any())
                //{
                //    var newImage = 
                //    _repoWrapper.UserImage.Create();
                //}
                if (images.Any())
                {
                    var profilePicture = _repoWrapper.UserImage.GetAllImages().Where(x => x.UserId == User.Identity.Name).Where(x => x.IsProfile == true).First();
                    if (profilePicture == null)
                    {
                        result.Message = $"Profile image  not found";
                        result.StatusCode = ResultStatus.Error.ToString();
                        return new JsonResult(result);
                    }
                    var image = _repoWrapper.UserImage.GetImageById(id);
                    if (image == null)
                    {
                        result.Message = $"Image with id {id} not found";
                        result.StatusCode = ResultStatus.Error.ToString();
                        return new JsonResult(result);
                    }
                    image.IsProfile = true;
                    _repoWrapper.UserImage.Update(image);
                    profilePicture.IsProfile = false;
                    _repoWrapper.UserImage.Update(profilePicture);

                    
                    _repoWrapper.Save();

                    //var images = _repoWrapper.UserImage.GetAllImages().Where(x => x.UserId == User.Identity.Name).ToList();
                    var userReturn = _repoWrapper.User.GetUserById(User.Identity.Name);
                    var userResult = _mapper.Map<UserProfileScreenVM>(userReturn);
                    userResult.Images = images;
                    result.Data = userResult;
                    result.Message = "Image updated!";
                    result.StatusCode = ResultStatus.Ok.ToString();
                }
                
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();

            }
            return new JsonResult(result);
        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserImage>> GetImage(int id)
        {
            try
            {
                var image = _repoWrapper.UserImage.GetImageById(id);
                if (image == null)
                {
                    result.Message = $"There is no Image with id {id}";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                _logger.LogInformation($"Returned image with id: {id}");
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


                var imageEntity = _repoWrapper.UserImage.GetImageById(id);
                if (imageEntity == null)
                {
                    result.Message = $"Image with id {id} not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                UserImage image1 = _mapper.Map(image, imageEntity);
                
                _repoWrapper.UserImage.UpdateImage(image1);
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
        public async Task<ActionResult<UserImage>> PostImage([FromBody]CreateUserImageVM image)
        {
            try
            {
                if (image == null)
                {
                    result.Message = "Image not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }
                UserImage imageEntity = new UserImage
                {
                    Date = DateTime.Now,
                    ImageUrl = image.ImageUrl,
                    IsProfile = false,
                    Name = _repoWrapper.User.GetUserById(User.Identity.Name).FirstName + " " + DateTime.Now.ToString(),
                    Picture = "Blob",
                    UserId = User.Identity.Name

                };
                _repoWrapper.UserImage.CreateImage(imageEntity);
                _repoWrapper.Save();




                var images = _repoWrapper.UserImage.GetAllImages().Where(x => x.UserId == User.Identity.Name).ToList();
                var userReturn = _repoWrapper.User.GetUserById(User.Identity.Name);
                var today = DateTime.Today;
                var age = today.Year - userReturn.DateOfBirth.Year;
                if (userReturn.DateOfBirth.Date > today.AddYears(-age))
                    age--;
                var userResult = _mapper.Map<UserProfileScreenVM>(userReturn);
                userResult.Images = images;
                userResult.Years = age;
                result.Data = userResult;
                result.Message = "Image Created!";
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
        public async Task<ActionResult<UserImage>> DeleteImage(int id)
        {
            try
            {
                var image = _repoWrapper.UserImage.GetImageById(id);
                if (image == null)
                {
                    result.Message = $"Image not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                _repoWrapper.UserImage.DeleteImage(image);
                _repoWrapper.Save();
                var images = _repoWrapper.UserImage.GetAllImages().Where(x => x.UserId == User.Identity.Name).ToList();
                var userReturn = _repoWrapper.User.GetUserById(User.Identity.Name);
                var userResult = _mapper.Map<UserProfileScreenVM>(userReturn);
                userResult.Images = images;
                result.Data = userResult;
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
            return _repoWrapper.UserImage.FindByCondition(e => e.Id == id).Any();
        }
    }
}
