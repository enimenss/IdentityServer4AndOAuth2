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
using Microsoft.AspNetCore.Cors;

namespace CrewCloudApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    [EnableCors("AllowOrigin")]
    public class UsersController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        private readonly ILogger<UsersController> _logger;

        

        private IMapper _mapper;

        private IHttpContextAccessor _httpContextAccessor;

        private CustomResult result = new CustomResult();
        public UsersController(ILogger<UsersController> logger, IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repoWrapper, IMapper mapper)
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
            var users = _repoWrapper.User.GetAllUsers().ToList();
            if (users == null)
            {
                result.Message = "There are no users";
                result.StatusCode = ResultStatus.Error.ToString();
                return new JsonResult(result);
            }
            

            var usersResult = _mapper.Map<IEnumerable<UserListVM>>(users);
            foreach(var item in usersResult)
            {
                var today = DateTime.Today;

                var age = today.Year - item.DateOfBirth.Year;

                if (item.DateOfBirth > today.AddYears(-age))
                    age--;

                item.Years = age;
            }
            result.Data = usersResult;
            result.Message = "Users returned!";
            result.StatusCode = ResultStatus.Ok.ToString();
            return new JsonResult(result);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            try
            {
                var user = _repoWrapper.User.GetUserById(id);
                if (user == null)
                {
                    result.Message = $"There is no user with email {id}";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }
                
                 _logger.LogInformation($"Returned user with email: {id}");
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


        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody]UserUpdateVM user)
        {
            try
            {
                if (user == null)
                {
                    result.Message = $"User with found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }


                var userEntity = _repoWrapper.User.GetUserById(User.Identity.Name);
                if (userEntity == null)
                {
                    result.Message = $"User not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }
             

                User user1 = _mapper.Map(user, userEntity);
                User user2 = _mapper.Map<User>(userEntity);
                _repoWrapper.User.UpdateUser(user1);
                _repoWrapper.Save();

                var today = DateTime.Today;
                var age = today.Year - user.DateOfBirth.Year;
                if (user.DateOfBirth.Date > today.AddYears(-age))
                    age--;
                
                var images = _repoWrapper.UserImage.GetAllImages().Where(x => x.UserId == User.Identity.Name).ToList();
                var userReturn = _repoWrapper.User.GetUserById(User.Identity.Name);
                var userResult = _mapper.Map<UserProfileScreenVM>(userReturn);
                userResult.Images = images;
                userResult.Years = age;

                result.Data = userResult;

                result.Message = "User updated!";
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

        [HttpPut]
        public async Task<IActionResult> UpdateCoverLetter([FromBody]CoverLetterVM coverLetter)
        {
            try
            {
                if (coverLetter == null)
                {
                    result.Message = $"CoverLetter with found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }


                var userEntity = _repoWrapper.User.GetUserById(User.Identity.Name);
                if (userEntity == null)
                {
                    result.Message = $"User not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }
                userEntity.CoverLetter = coverLetter.CoverLetter;

                
                _repoWrapper.User.UpdateUser(userEntity);
                _repoWrapper.Save();

                var today = DateTime.Today;
                var age = today.Year - userEntity.DateOfBirth.Year;
                if (userEntity.DateOfBirth.Date > today.AddYears(-age))
                    age--;
                var images = _repoWrapper.UserImage.GetAllImages().Where(x => x.UserId == User.Identity.Name).ToList();
                var userReturn = _repoWrapper.User.GetUserById(User.Identity.Name);
                var userResult = _mapper.Map<UserProfileScreenVM>(userReturn);
                userResult.Images = images;
                userResult.Years = age;
                result.Data = userResult;

                result.Message = "User updated!";
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



        // PUT: api/Users/5       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, [FromBody]UserUpdateVM  user)
        {
            try
            {
                if (user == null)
                {
                    result.Message = $"User with Email {id} not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }


                var userEntity = _repoWrapper.User.GetUserById(id);
                if (userEntity == null)
                {
                    result.Message = $"User with Email {id} not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }
                

                User user1 =_mapper.Map(user, userEntity);
                User user2 = _mapper.Map<User>(userEntity);
                _repoWrapper.User.UpdateUser(user1);
                _repoWrapper.Save();
                
                result.Data = user1;
                result.Message = "User updated!";
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


        [HttpPut]
        public async Task<IActionResult> ChangeJobStatus()
        {
            try
            {
                var user = _repoWrapper.User.GetUserById(User.Identity.Name);
                if (user == null)
                {
                    result.Message = $"User not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }

                if(user.Status)
                {
                    user.Status = false;
                }
                else
                {
                    user.Status = true;
                }

                _repoWrapper.User.UpdateUser(user);
                _repoWrapper.Save();
                
                var images = _repoWrapper.UserImage.GetAllImages().Where(x => x.UserId == User.Identity.Name).ToList();
                var userReturn = _repoWrapper.User.GetUserById(User.Identity.Name);
                var userResult = _mapper.Map<UserProfileScreenVM>(userReturn);
                userResult.Images = images;
                result.Data = userResult;
                
                result.Message = "Job status changed!";
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

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody]CreateUserVM user)
        {
            try
            {
                if (user == null)
                {
                    result.Message = "User not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }
                User userEntity = _mapper.Map<User>(user);
                if(user.ProfilePicture!=null)
                {
                    UserImage i = new UserImage
                    {
                        Date = DateTime.Now,
                        IsProfile = true,
                        Name = user.FirstName.ToString() + "Profile Picture",
                        Picture = user.ProfilePicture,
                        UserId = user.Email
                    };
                    _repoWrapper.UserImage.Create(i);
                    _repoWrapper.Save();
                    userEntity.ProfilPictureId = _repoWrapper.UserImage.GetAllImages().Single(x=>x.UserId ==userEntity.Email).Id;
                }


                userEntity.UserName = user.Email;
                _repoWrapper.User.CreateUser(userEntity);
                
                _repoWrapper.Save();
                
                var createdUser = _mapper.Map<CreateUserVM>(userEntity);

                // return CreatedAtAction("GetUser", new { id = user.Email }, createdUser);
                result.Data = userEntity;
                result.Message = "User Created!";
                result.StatusCode = ResultStatus.Ok.ToString();
            }
            catch (DbUpdateException ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ResultStatus.Error.ToString();
                //   result.Message = "User is not created!";
            }

            return new JsonResult(result);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(string id)
        {
            try
            {
                var user = _repoWrapper.User.GetUserById(id);
                if (user == null)
                {
                    result.Message = $"User not found";
                    result.StatusCode = ResultStatus.Error.ToString();
                    return new JsonResult(result);
                }
                
                foreach(var item in _repoWrapper.UserImage.GetAllImages().Where(x => x.UserId == user.Email).ToList())
                {
                    _repoWrapper.UserImage.Delete(item);
                    _repoWrapper.Save();
                }
                _repoWrapper.User.DeleteUser(user);
                _repoWrapper.Save();
                result.Data = user;
                result.Message = "User deleted!";
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

        private bool UserExists(string id)
        {
            return _repoWrapper.User.FindByCondition(e => e.Email == id).Any();
        }
    }
}
