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

    // [HttpPut]
    // [Route("{id:int}")]
    // [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<ConsultoryResponseDto>))]
    // public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PatientUpdateRequestDto requestDto)
    // {
    //     try
    //     {
    //         Expression<Func<Patient, bool>> filter = x => x.Id == id;

    //         var existPatient = await _dbContext.Patient.AnyAsync(filter);

    //         if (!existPatient)
    //             return BadRequest("No se encontró ningun paaciente");

    //         var newEntity = _mapper.Map<Patient>(requestDto);
    //         newEntity.Id = id;
    //         newEntity.IsDeleted = false;
    //         newEntity.LastModifiedDate = DateTime.Now;
    //         newEntity.LastModifiedBy = _tokenHelper.GetUserName();

    //         var patientAddress = _mapper.Map<PatientAddress>(requestDto);
    //         patientAddress.PatientId = id;
    //         newEntity.PatientAddress.Add(patientAddress);

    //         await UpdatePatient(newEntity);

    //         var dto = _mapper.Map<PatientResponseDto>(newEntity);
    //         var response = new ApiResponse<PatientResponseDto>(dto);
    //         return Ok(response);
    //     }
    //     catch (Exception ex)
    //     {

    //         throw new LogicBusinessException(ex);
    //     }
    // }

    [HttpPut]
    [Route("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<PatientResponseDto>))]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PatientUpdateRequestDto requestDto)
    {
        try
        {
            // Recupera el paciente existente junto con su dirección
            var existPatient = await _dbContext.Patient
                .Include(p => p.PatientAddress)
                .ThenInclude(pa => pa.Address)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existPatient == null)
                return BadRequest("No se encontró ningún paciente");

            // Actualiza la información del paciente
            _mapper.Map(requestDto, existPatient);
            existPatient.LastModifiedDate = DateTime.Now;
            existPatient.LastModifiedBy = _tokenHelper.GetUserName();

            // Manejo de dirección
            if (requestDto != null)
            {
                var existingPatientAddress = existPatient.PatientAddress.FirstOrDefault(pa => pa.IsDefault);

                if (existingPatientAddress != null)
                {
                    // Actualiza la dirección existente
                    existingPatientAddress.Address.Street = requestDto.Street;
                    existingPatientAddress.Address.ExternalNumber = requestDto.ExternalNumber;
                    existingPatientAddress.Address.InternalNumber = requestDto.InternalNumber;
                    existingPatientAddress.Address.ZipCode = requestDto.ZipCode;
                    existingPatientAddress.Address.Address1 = requestDto.Address1;
                    existingPatientAddress.Address.Address2 = requestDto.Address2;
                    existingPatientAddress.IsDefault = true;
                    existingPatientAddress.RegisterDate = DateTime.Now;

                    _dbContext.PatientAddress.Update(existingPatientAddress);
                    _dbContext.Address.Update(existingPatientAddress.Address);
                }
                else
                {
                    // Crea una nueva dirección si no existe una dirección
                    var newAddress = new Address
                    {
                        Street = requestDto.Street,
                        ExternalNumber = requestDto.ExternalNumber,
                        InternalNumber = requestDto.InternalNumber,
                        ZipCode = requestDto.ZipCode,
                        Address1 = requestDto.Address1,
                        Address2 = requestDto.Address2
                    };

                    var newPatientAddress = new PatientAddress
                    {
                        Address = newAddress,
                        AddressId = newAddress.Id,
                        PatientId = id,
                        IsDefault = true,
                        RegisterDate = DateTime.Now
                    };

                    _dbContext.PatientAddress.Add(newPatientAddress);
                    _dbContext.Address.Add(newAddress);
                }
            }

            // Marca el paciente para actualización
            _dbContext.Patient.Update(existPatient);

            // Guarda los cambios
            await _dbContext.SaveChangesAsync();

            // Mapea el resultado y devuelve la respuesta
            var resultDto = _mapper.Map<PatientResponseDto>(existPatient);
            var response = new ApiResponse<PatientResponseDto>(resultDto);
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

    // private async Task UpdatePatient(Patient entity)
    // {
    //     var oldEntity = await _dbContext.Patient.FirstOrDefaultAsync(x => x.Id == entity.Id);
    //     var patienAddress = oldEntity!.PatientAddress.FirstOrDefault();
    //     var address = patienAddress!.Address ?? new Address();

    //     var newPatienAddress = entity.PatientAddress.FirstOrDefault()!;
    //     var newAddress = newPatienAddress.Address ?? new Address();

    //     patienAddress.PatientId = patienAddress.PatientId;
    //     patienAddress.AddressId = patienAddress.AddressId;
    //     patienAddress.IsDefault = true;
    //     patienAddress.RegisterDate = DateTime.Now;
    //     patienAddress.Id = patienAddress.Id;

    //     address.Id = address.Id;
    //     address.Address1 = newAddress.Address1;
    //     address.Address2 = newAddress.Address2;
    //     address.Street = newAddress.Street;
    //     address.ExternalNumber = newAddress.ExternalNumber;
    //     address.InternalNumber = newAddress.InternalNumber;
    //     address.ZipCode = newAddress.ZipCode;

    //     _dbContext.PatientAddress.Update(patienAddress);
    //     _dbContext.Address.Update(address);

    //     entity.Code = oldEntity.Code;

    //     _dbContext.Patient.Update(entity);
    //     await _dbContext.SaveChangesAsync();
    // }
}