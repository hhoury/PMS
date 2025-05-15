using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.Contracts;
using PMS.Application.DTOs.Policies;
using PMS.Application.Models;
using PMS.Domain.Entities;
using PMS.Domain.Enumeration;

namespace PMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoliciesController : ControllerBase
    {
        private readonly ILogger<PoliciesController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public PoliciesController(ILogger<PoliciesController> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<PolicyDto>>> GetPolicies([FromQuery] QueryParameters queryParameters)
        {
            try
            {
                var pagedPolicies = await _unitOfWork.Policies.GetAllAsync<PolicyDto>(queryParameters);
                return Ok(pagedPolicies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting policies");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PolicyDto>> GetPolicyById(int id)
        {
            try
            {
                var policy = await _unitOfWork.Policies.GetAsync(id);
                if (policy == null)
                {
                    return NotFound();
                }
                return Ok(policy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in getting policy by id {id}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("type/{policyType}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PolicyDto>>> GetPoliciesByPolicyType([FromRoute] PolicyType policyType)
        {
            try
            {
                var policies = await _unitOfWork.Policies.GetByPolicyTypeAsync(policyType);
                if (policies == null || !policies.Any())
                {
                    return NotFound();
                }
                var mapperDto = _mapper.Map<List<PolicyDto>>(policies);
                return Ok(mapperDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in getting policies by policy type {policyType}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePolicy(int id, [FromBody] PolicyDto policyDto)
        {
            try
            {
                if (id != policyDto.Id)
                {
                    return BadRequest();
                }

                var policy = await _unitOfWork.Policies.GetAsync(id);
                if (policy == null)
                {
                    return NotFound();
                }

                _mapper.Map(policyDto, policy);
                await _unitOfWork.Policies.UpdateAsync(policy);
                await _unitOfWork.CompleteAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in updating policy with id {id}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePolicy(int id)
        {
            try
            {
                var policy = await _unitOfWork.Policies.GetAsync(id);
                if (policy == null)
                {
                    return NotFound();
                }

                await _unitOfWork.Policies.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in deleting policy with id {id}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PolicyDto>> CreatePolicy([FromBody] PolicyDto policyDto)
        {
            try
            {
                if (policyDto == null)
                {
                    return BadRequest();
                }

                var policy = _mapper.Map<Policy>(policyDto);
                await _unitOfWork.Policies.AddAsync(policy);
                await _unitOfWork.CompleteAsync();

                var createdPolicyDto = _mapper.Map<PolicyDto>(policy);
                return CreatedAtAction(nameof(GetPolicyById), new { id = createdPolicyDto.Id }, createdPolicyDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in creating policy");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
