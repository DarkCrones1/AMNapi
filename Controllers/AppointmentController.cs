using System.Linq.Expressions;
using System.Net;

using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using Microsoft.EntityFrameworkCore;

using AMNApi.Data;
using AMNApi.Dtos.Response;
using AMNApi.Dtos.Request.Create;
using AMNApi.Entities;
using AMNApi.Dtos.QueryFilters;
using AMNApi.Filters.Exceptions;
using AMNApi.Entities.Base;
using Microsoft.AspNetCore.Authorization;
using AMNApi.Response;
using AMNApi.Common.Functions;

namespace AMNApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AppointmentController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly AMNDbContext _dbContext;

    public AppointmentController(IMapper mapper, AMNDbContext dbContext)
    {
        this._mapper = mapper;
        this._dbContext = dbContext;
    }

    [HttpGet]
    [Route("")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<AppointmentResponseDto>>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<IEnumerable<AppointmentResponseDto>>))]
    [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiResponse<IEnumerable<AppointmentResponseDto>>))]
    public async Task<IActionResult> GetAll([FromQuery] AppointmentQueryFilter filter)
    {
        try
        {
            var entities = await GetPageds(filter);
            var dtos = _mapper.Map<IEnumerable<AppointmentResponseDto>>(entities);
            var metaDataResponse = new MetaDataResponse(
                entities.TotalCount,
                entities.CurrentPage,
                entities.PageSize
            );
            var response = new ApiResponse<IEnumerable<AppointmentResponseDto>>(data: dtos, meta: metaDataResponse);
            return Ok(response);
        }
        catch (Exception ex)
        {
            throw new LogicBusinessException(ex);
        }
    }

    [HttpPost]
    [Route("")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<AppointmentResponseDto>))]
    public async Task<IActionResult> Create([FromBody] AppointmentCreateRequestDto requestDto)
    {
        try
        {
            Expression<Func<Consultory, bool>> filterConsultory = x => x.Id == requestDto.ConsultoryId;

            var existConsultory = await _dbContext.Consultory.AnyAsync(filterConsultory);

            if (!existConsultory)
                return BadRequest("no existe ningun consultorio");

            Expression<Func<Patient, bool>> filterPatient = x => x.Id == requestDto.PatientId;

            var existPatient = await _dbContext.Patient.AnyAsync(filterPatient);

            if (!existPatient)
                return BadRequest("no existe ningun paciente");

            Expression<Func<Doctor, bool>> filterDoctor = x => x.Id == requestDto.PatientId;

            var existDoctor = await _dbContext.Doctor.AnyAsync(filterDoctor);

            if (!existDoctor)
                return BadRequest("no existe ningun paciente");

            var entity = _mapper.Map<Appointment>(requestDto);
            await _dbContext.Appointment.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            var result = _mapper.Map<AppointmentResponseDto>(entity);
            var response = new ApiResponse<AppointmentResponseDto>(result);
            return Ok(response);
        }
        catch (Exception ex)
        {

            throw new LogicBusinessException(ex);
        }
    }

    private async Task<PagedList<Appointment>> GetPageds(AppointmentQueryFilter filter)
    {
        var result = await GetPaged(filter);
        var pagedItems = PagedList<Appointment>.Create(result, filter.PageNumber, filter.PageSize);
        return pagedItems;
    }

    private async Task<IEnumerable<Appointment>> GetPaged(AppointmentQueryFilter entity)
    {
        var query = _dbContext.Appointment.AsQueryable();

        if (entity.Id > 0)
            query = query.Where(x => x.Id == entity.Id);

        return await query.ToListAsync();
    }
}