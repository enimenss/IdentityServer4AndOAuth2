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
    public class JobApplicationControler
 : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        private readonly ILogger<JobApplicationControler> _logger;

        private IMapper _mapper;

        private IHttpContextAccessor _httpContextAccessor;

        public JobApplicationControler(ILogger<JobApplicationControler> logger, IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        // GET: api/Jobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobApplication>>> GetJobsApplication()
        {
            var JobsApplication = _repoWrapper.JobApplication.GetAllJobApplications().ToList();
            _logger.LogInformation("All Jobs returned");
            var jobsResult = _mapper.Map<IEnumerable<JobApplicationVM>>(JobsApplication);
            return Ok(jobsResult);
        }

        // GET: api/jobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobApplication>> GetJobsApplication(int id)
        {
            try
            {
                var jobApplications = _repoWrapper.JobApplication.GetJobApplicationById(id);
                if (jobApplications == null)
                {
                    _logger.LogInformation("jobApplication with that is not found");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned jobsApplication with : {id}");

                    var jobApplicationResault = _mapper.Map<JobApplicationVM>(jobApplications);
                    return Ok(jobApplicationResault);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }



        }

        // PUT: api/Job/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobsApplication(int id, [FromBody]JobApplicationVM jobApplication)
        {

            try
            {
                if (jobApplication == null)
                {
                    _logger.LogError("JobApplication object sent from client is null.");
                    return BadRequest("JobApplication object is null");
                }


                var JobApplicationEntity = _repoWrapper.JobApplication.GetJobApplicationById(id);
                if (JobApplicationEntity == null)
                {
                    _logger.LogError($"jobApplication with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                JobApplication jobApplication1 = _mapper.Map(jobApplication, JobApplicationEntity);

                _repoWrapper.JobApplication.UpdateJobApplication(jobApplication1);
                _repoWrapper.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Job action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Messages

        [HttpPost]
        public async Task<ActionResult<JobApplication>> PostJobsApplication([FromBody]JobApplicationVM job)
        {


            try
            {
                if (job == null)
                {
                    _logger.LogError("job object sent from client is null.");
                    return BadRequest("job object is null");
                }
                JobApplication jobEntity = _mapper.Map<JobApplication>(job);

                _repoWrapper.JobApplication.CreateJobApplication(jobEntity);
                _repoWrapper.Save();

                var createdJob = _mapper.Map<JobListItemVM>(jobEntity);

                return CreatedAtAction("GetJobs", new { id = createdJob.Id }, createdJob);

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Something went wrong inside Messages action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }


        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<JobApplication>> DeleteJobsApplication(int id)
        {
            try
            {
                var job = _repoWrapper.JobApplication.GetJobApplicationById(id);
                if (job == null)
                {
                    _logger.LogError($"jobApplication with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repoWrapper.JobApplication.DeleteJobApplication(job);
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
