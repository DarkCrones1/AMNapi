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
using AMNApi.Dtos.Request.Update;
using AW.Common.Helpers;
using AMNApi.Dtos.QueryFilters;

namespace AMNApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class PatientController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly AMNDbContext _dbContext;
    private readonly TokenHelper _tokenHelper;

    public PatientController(IMapper mapper, AMNDbContext dbContext, TokenHelper tokenHelper)
    {
        this._mapper = mapper;
        this._dbContext = dbContext;
        this._tokenHelper = tokenHelper;
    }

    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PatientResponseDto>>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<IEnumerable<PatientResponseDto>>))]
    [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiResponse<IEnumerable<PatientResponseDto>>))]
    public async Task<IActionResult> GetAll([FromQuery] PatientQueryFilter filter)
    {
        try
        {
            var entities = await GetPageds(filter);
            var dtos = _mapper.Map<IEnumerable<PatientResponseDto>>(entities);
            var metaDataResponse = new MetaDataResponse(
                entities.TotalCount,
                entities.CurrentPage,
                entities.PageSize
            );
            var response = new ApiResponse<IEnumerable<PatientResponseDto>>(data: dtos, meta: metaDataResponse);
            return Ok(response);
        }
        catch (Exception ex)
        {
            throw new LogicBusinessException(ex);
        }
    }

    [HttpPost]
    [Route("")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<PatientResponseDto>))]
    public async Task<IActionResult> Create([FromBody] PatientCreateRequestDto requestDto)
    {
        try
        {
            Expression<Func<Patient, bool>> filter = x => !x.IsDeleted!.Value && x.CellPhone == requestDto.CellPhone;

            var existPatient = await _dbContext.Patient.AnyAsync(filter);

            if (existPatient)
                return BadRequest("Ese numero de celular ya ha sido usado");

            var entity = _mapper.Map<Patient>(requestDto);
            entity.CreatedBy = _tokenHelper.GetUserName();
            await _dbContext.Patient.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            var result = _mapper.Map<PatientResponseDto>(entity);
            var response = new ApiResponse<PatientResponseDto>(result);
            return Ok(response);
        }
        catch (Exception ex)
        {

            throw new LogicBusinessException(ex);
        }
    }

    [HttpPut]
    [Route("{Id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<ConsultoryResponseDto>))]
    public async Task<IActionResult> Update([FromRoute] int Id, [FromBody] PatientUpdateRequestDto requestDto)
    {
        try
        {
            Expression<Func<Patient, bool>> filter = x => x.Id == Id;

            var existPatient = await _dbContext.Patient.AnyAsync(filter);

            if (!existPatient)
                return BadRequest("No se encontró ningun paaciente");

            var newEntity = _mapper.Map<Patient>(requestDto);
            newEntity.Id = Id;
            newEntity.IsDeleted = false;
            newEntity.LastModifiedDate = DateTime.Now;
            newEntity.LastModifiedBy = _tokenHelper.GetUserName();

            var patientAddress = _mapper.Map<PatientAddress>(requestDto);
            patientAddress.PatientId = Id;
            newEntity.PatientAddress.Add(patientAddress);

            _dbContext.Update(newEntity);

            var dto = _mapper.Map<PatientResponseDto>(newEntity);
            var response = new ApiResponse<PatientResponseDto>(dto);
            return Ok(response);

        }
        catch (Exception ex)
        {

            throw new LogicBusinessException(ex);
        }
    }


    private async Task<PagedList<Patient>> GetPageds(PatientQueryFilter filter)
    {
        var result = await GetPaged(filter);
        var pagedItems = PagedList<Patient>.Create(result, filter.PageNumber, filter.PageSize);
        return pagedItems;
    }

    private async Task<IEnumerable<Patient>> GetPaged(PatientQueryFilter entity)
    {
        var query = _dbContext.Patient.AsQueryable();

        if (entity.Id > 0)
            query = query.Where(x => x.Id == entity.Id);

        return await query.ToListAsync();
    }
}