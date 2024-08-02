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
using AW.Common.Helpers;
using AMNApi.Entities.Enumerations;
using AMNApi.Dtos.Request.Update;

namespace AMNApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class AppointmentController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly AMNDbContext _dbContext;
    private readonly TokenHelper _tokenHelper;

    public AppointmentController(IMapper mapper, AMNDbContext dbContext, TokenHelper tokenHelper)
    {
        this._mapper = mapper;
        this._dbContext = dbContext;
        this._tokenHelper = tokenHelper;
    }

    [HttpGet]
    [Route("")]
    [AllowAnonymous]
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

            Expression<Func<Doctor, bool>> filterDoctor = x => x.Id == requestDto.DoctorId;

            var existDoctor = await _dbContext.Doctor.AnyAsync(filterDoctor);

            if (!existDoctor)
                return BadRequest("no existe ningun Doctor");

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

    [HttpPut]
    [Route("{id:int}/status/Concluded")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<AppointmentResponseDto>))]
    public async Task<IActionResult> ChangeStatusConcluded([FromRoute] int id)
    {
        Expression<Func<Appointment, bool>> filterAppointment = x => x.Id == id;

        var existAppointment = await _dbContext.Appointment.AnyAsync(filterAppointment);

        if (!existAppointment)
            return BadRequest("No se ha encontrado ninguna coincidencia");

        var entity = await _dbContext.Appointment.FirstOrDefaultAsync(x => x.Id == id);
        entity!.Status = (short)AppoinmentStatus.Concluded;
        entity.Id = id;
        entity.LastModifiedBy = _tokenHelper.GetUserName();
        entity.LastModifiedDate = DateTime.Now;
        entity.IsDeleted = true;
        _dbContext.Appointment.Update(entity);
        await _dbContext.SaveChangesAsync();
        return Ok(true);
    }

    [HttpPut]
    [Route("{id:int}/status/Loss")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<AppointmentResponseDto>))]
    public async Task<IActionResult> ChangeStatusLoss([FromRoute] int id)
    {
        Expression<Func<Appointment, bool>> filterAppointment = x => x.Id == id;

        var existAppointment = await _dbContext.Appointment.AnyAsync(filterAppointment);

        if (!existAppointment)
            return BadRequest("No se ha encontrado ninguna coincidencia");

        var entity = await _dbContext.Appointment.FirstOrDefaultAsync(x => x.Id == id);
        entity!.Status = (short)AppoinmentStatus.Loss;
        entity.Id = id;
        entity.LastModifiedBy = _tokenHelper.GetUserName();
        entity.LastModifiedDate = DateTime.Now;
        entity.IsDeleted = true;
        _dbContext.Appointment.Update(entity);
        await _dbContext.SaveChangesAsync();
        return Ok(true);
    }

    [HttpPut]
    [Route("{id:int}/status/ReScheduled")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<AppointmentResponseDto>))]
    public async Task<IActionResult> ChangeStatusReScheduled([FromRoute] int id,[FromBody] AppointmentUpdateRequestDto requestDto)
    {
        Expression<Func<Appointment, bool>> filterAppointment = x => x.Id == id;

        var existAppointment = await _dbContext.Appointment.AnyAsync(filterAppointment);

        if (!existAppointment)
            return BadRequest("No se ha encontrado ninguna coincidencia");

        var entity = await _dbContext.Appointment.FirstOrDefaultAsync(x => x.Id == id);
        entity!.Status = (short)AppoinmentStatus.ReScheduled;
        entity.Id = id;
        entity.LastModifiedBy = _tokenHelper.GetUserName();
        entity.LastModifiedDate = DateTime.Now;
        entity.IsDeleted = false;
        entity.AppoinmentDate = requestDto.AppointmentDate;
        _dbContext.Appointment.Update(entity);
        await _dbContext.SaveChangesAsync();
        return Ok(true);
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

        if (entity.Status.HasValue)
            query = query.Where(x => x.Status == entity.Status.Value);

        if (entity.IsDeleted.HasValue)
            query = query.Where(x => x.IsDeleted == entity.IsDeleted.Value);

        if (entity.AppointmenDate.HasValue)
            query = query.Where(x => x.AppoinmentDate == entity.AppointmenDate.Value);

        if (entity.MinAppointmentDate.HasValue)
            query = query.Where(x => x.AppoinmentDate >= entity.MinAppointmentDate.Value);

        if (entity.MaxAppointmentDate.HasValue)
            query = query.Where(x => x.AppoinmentDate <= entity.MaxAppointmentDate.Value);

        return await query.ToListAsync();
    }
}