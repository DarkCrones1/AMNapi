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

namespace AMNApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
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

    [HttpPost]
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
}