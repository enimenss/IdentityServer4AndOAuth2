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
    public class MessagesController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        private readonly ILogger<WeatherForecastController> _logger;

        private IMapper _mapper;

        private IHttpContextAccessor _httpContextAccessor;

        public MessagesController(ILogger<WeatherForecastController> logger, IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        // GET: api/Messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Messages>>> GetMessages()
        {
            var messages = _repoWrapper.Messages.GetAllMessages().ToList();
            _logger.LogInformation("All Messages returned");
            var messagesResult = _mapper.Map<IEnumerable<MessageListItemVM>>(messages);
            return Ok(messagesResult);
        }

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Messages>> GetMessages(int id)
        {
            try
            {
                var messages = _repoWrapper.Messages.GetMessagesById(id);
                if (messages == null)
                {
                    _logger.LogInformation("Messages with that is not found");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned Messages with : {id}");

                    var messagesResult = _mapper.Map<MessageProfileScreenVM>(messages);
                    return Ok(messagesResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }



        }

        // PUT: api/Messages/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessages(int id, [FromBody]MessageUpdateVM messages)
        {

            try
            {
                if (messages == null)
                {
                    _logger.LogError("Messages object sent from client is null.");
                    return BadRequest("Messages object is null");
                }


                var messagesEntity = _repoWrapper.Messages.GetMessagesById(id);
                if (messagesEntity == null)
                {
                    _logger.LogError($"Messages with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                Messages messages1 = _mapper.Map(messages, messagesEntity);

                _repoWrapper.Messages.UpdateMessages(messages1);
                _repoWrapper.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Messages action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Messages

        [HttpPost]
        public async Task<ActionResult<Messages>> PostMessages([FromBody]CreateMessagesVM messages)
        {


            try
            {
                if (messages == null)
                {
                    _logger.LogError("Messages object sent from client is null.");
                    return BadRequest("Messages object is null");
                }
                Messages messagesEntity = _mapper.Map<Messages>(messages);

                _repoWrapper.Messages.CreateMessages(messagesEntity);
                _repoWrapper.Save();

                var createdMessages = _mapper.Map<CreateMessagesVM>(messagesEntity);

                return CreatedAtAction("GetMessages", new { id = messages.Id }, createdMessages);

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Something went wrong inside Messages action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }


        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Messages>> DeleteMessages(int id)
        {
            try
            {
                var messages = _repoWrapper.Messages.GetMessagesById(id);
                if (messages == null)
                {
                    _logger.LogError($"Messages with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repoWrapper.Messages.DeleteMessages(messages);
                _repoWrapper.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Messages action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
