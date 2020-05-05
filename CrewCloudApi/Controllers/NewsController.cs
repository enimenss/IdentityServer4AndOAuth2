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

namespace CrewCloudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class NewsController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        private readonly ILogger<WeatherForecastController> _logger;

        private IMapper _mapper;

        private IHttpContextAccessor _httpContextAccessor;

        public NewsController(ILogger<WeatherForecastController> logger, IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        // GET: api/News
        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
            var news = _repoWrapper.News.GetAllNews().ToList();
            _logger.LogInformation("All News returned");
            var newsResult = _mapper.Map<IEnumerable<NewsListItemVM>>(news);
            return Ok(newsResult);
        }

        // GET: api/News/5
        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNews(int id)
        {
            try
            {
                var news = _repoWrapper.News.GetNewsById(id);
                if (news == null)
                {
                    _logger.LogInformation("News with that is not found");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned News with : {id}");

                    var newsResult = _mapper.Map<NewsProfileScreenVM>(news);
                    return Ok(newsResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }



        }

        // PUT: api/News/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNews(int id, [FromBody]NewsUpdateVM news)
        {

            try
            {
                if (news == null)
                {
                    _logger.LogError("News object sent from client is null.");
                    return BadRequest("News object is null");
                }


                var newsEntity = _repoWrapper.News.GetNewsById(id);
                if (newsEntity == null)
                {
                    _logger.LogError($"News with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                News news1 = _mapper.Map(news, newsEntity);

                _repoWrapper.News.UpdateNews(news1);
                _repoWrapper.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside News action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/News
      
        [HttpPost]
        public async Task<ActionResult<News>> PostNews([FromBody]CreateNewsVM news)
        {


            try
            {
                if (news == null)
                {
                    _logger.LogError("News object sent from client is null.");
                    return BadRequest("News object is null");
                }
                News newsEntity = _mapper.Map<News>(news);

                _repoWrapper.News.CreateNews(newsEntity);
                _repoWrapper.Save();

                var createdNews = _mapper.Map<CreateNewsVM>(newsEntity);

                return CreatedAtAction("GetNews", new { id = news.Name }, createdNews);

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Something went wrong inside News action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }


        }

        // DELETE: api/News/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<News>> DeleteNews(int id)
        {
            try
            {
                var news = _repoWrapper.News.GetNewsById(id);
                if (news == null)
                {
                    _logger.LogError($"News with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repoWrapper.News.DeleteNews(news);
                _repoWrapper.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside News action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

       
    }
}
