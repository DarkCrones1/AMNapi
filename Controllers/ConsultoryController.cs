using System.Linq.Expressions;
using System.Net;

using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using Microsoft.EntityFrameworkCore;

using AMNApi.Data;
using AMNApi.Dtos.Response;
using AMNApi.Dtos.Request.Create;
using AMNApi.Entities;
// using AMNApi.Dtos.QueryFilters;
using AMNApi.Filters.Exceptions;
using AMNApi.Entities.Base;
using Microsoft.AspNetCore.Authorization;
using AMNApi.Response;
using AMNApi.Common.Functions;

namespace AMNApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ConsultoryController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly AMNDbContext _dbContext;

    public ConsultoryController(IMapper mapper, AMNDbContext dbContext)
    {
        this._mapper = mapper;
        this._dbContext = dbContext;
    }

    [HttpPost]
    [Route("")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<ConsultoryResponseDto>))]
    public async Task<IActionResult> Create([FromBody] ConsultoryCreateRequestDto requestDto)
    {
        try
        {
            Expression<Func<Consultory, bool>> filterConsultory = x => !x.IsDeleted!.Value && x.Name == requestDto.Name;

            var existConsultory = await _dbContext.Consultory.AnyAsync(filterConsultory);

            if (existConsultory)
                return BadRequest("No pueden haber consultorios con el nombre duplicado");

            var entity = _mapper.Map<Consultory>(requestDto);
            await _dbContext.Consultory.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            var result = _mapper.Map<ConsultoryResponseDto>(entity);
            var response = new ApiResponse<ConsultoryResponseDto>(result);
            return Ok(response);
        }
        catch (Exception ex)
        {

            throw new LogicBusinessException(ex);
        }
    }
}