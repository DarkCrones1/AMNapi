using System.Linq.Expressions;
using System.Net;

using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using Microsoft.EntityFrameworkCore;

using AMNApi.Data;
using AMNApi.Dtos.Response;
using AMNApi.Dtos.Request.Create;
using AMNApi.Entities;
using AMNApi.Filters.Exceptions;
using AMNApi.Entities.Base;
using Microsoft.AspNetCore.Authorization;
using AMNApi.Response;
using AMNApi.Common.Functions;
using AMNApi.Dtos.QueryFilters;

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

    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<ConsultoryResponseDto>>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<IEnumerable<ConsultoryResponseDto>>))]
    [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiResponse<IEnumerable<ConsultoryResponseDto>>))]
    public async Task<IActionResult> GetAll([FromQuery] ConsultoryQueryFilter filter)
    {
        try
        {
            var entities = await GetPageds(filter);
            var dtos = _mapper.Map<IEnumerable<ConsultoryResponseDto>>(entities);
            var metaDataResponse = new MetaDataResponse(
                entities.TotalCount,
                entities.CurrentPage,
                entities.PageSize
            );
            var response = new ApiResponse<IEnumerable<ConsultoryResponseDto>>(data: dtos, meta: metaDataResponse);
            return Ok(response);
        }
        catch (Exception ex)
        {
            throw new LogicBusinessException(ex);
        }
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

    private async Task<PagedList<Consultory>> GetPageds(ConsultoryQueryFilter filter)
    {
        var result = await GetPaged(filter);
        var pagedItems = PagedList<Consultory>.Create(result, filter.PageNumber, filter.PageSize);
        return pagedItems;
    }

    private async Task<IEnumerable<Consultory>> GetPaged(ConsultoryQueryFilter entity)
    {
        var query = _dbContext.Consultory.AsQueryable();

        if (entity.Id > 0)
            query = query.Where(x => x.Id == entity.Id);

        return await query.ToListAsync();
    }
}