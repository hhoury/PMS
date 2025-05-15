using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.Contracts;
using PMS.Application.DTOs.Members;
using PMS.Application.Models;
using PMS.Domain.Entities;

namespace PMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly ILogger<MembersController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public MembersController(ILogger<MembersController> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<MemberDto>>> GetMembers([FromQuery] QueryParameters queryParameters)
        {
            try
            {
                var pagedMembers = await _unitOfWork.Members.GetAllAsync<MemberDto>(queryParameters);
                return Ok(pagedMembers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting members");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MemberDto>> GetMemberById(int id)
        {
            try
            {
                var member = await _unitOfWork.Members.GetAsync(id);
                if (member == null)
                {
                    return NotFound();
                }
                return Ok(member);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in getting member by id {id}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] MemberDto memberDto)
        {
            try
            {
                if (id != memberDto.Id)
                {
                    return BadRequest();
                }

                var member = await _unitOfWork.Members.GetAsync(id);
                if (member == null)
                {
                    return NotFound();
                }

                _mapper.Map(memberDto, member);
                await _unitOfWork.Members.UpdateAsync(member);
                await _unitOfWork.CompleteAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in updating member with id {id}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMember(int id)
        {
            try
            {
                var member = await _unitOfWork.Members.GetAsync(id);
                if (member == null)
                {
                    return NotFound();
                }

                await _unitOfWork.Members.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in deleting member with id {id}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MemberDto>> CreateMember([FromBody] MemberDto memberDto)
        {
            try
            {
                if (memberDto == null)
                {
                    return BadRequest();
                }

                var member = _mapper.Map<Member>(memberDto);
                await _unitOfWork.Members.AddAsync(member);
                await _unitOfWork.CompleteAsync();

                var createdMemberDto = _mapper.Map<MemberDto>(member);
                return CreatedAtAction(nameof(GetMemberById), new { id = createdMemberDto.Id }, createdMemberDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in creating member");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
