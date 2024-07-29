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
public class UserAccountController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly AMNDbContext _dbContext;

    public UserAccountController(IMapper mapper, AMNDbContext dbContext)
    {
        this._mapper = mapper;
        this._dbContext = dbContext;
    }

    [HttpGet]
    [Route("")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UserAccountResponseDto>>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<IEnumerable<UserAccountResponseDto>>))]
    [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiResponse<IEnumerable<UserAccountResponseDto>>))]
    public async Task<IActionResult> GetAll([FromQuery] UserAccountQueryFilter filter)
    {
        try
        {
            var entities = await GetPageds(filter);
            var dtos = _mapper.Map<IEnumerable<UserAccountResponseDto>>(entities);
            var metaDataResponse = new MetaDataResponse(
                entities.TotalCount,
                entities.CurrentPage,
                entities.PageSize
            );
            var response = new ApiResponse<IEnumerable<UserAccountResponseDto>>(data: dtos, meta: metaDataResponse);
            return Ok(response);
        }
        catch (Exception ex)
        {
            throw new LogicBusinessException(ex);
        }
    }

    [HttpPost]
    [Route("Patient")]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UserAccountPatientResponseDto>))]
    public async Task<IActionResult> CreatePatient([FromBody] UserAccountPatientCreateRequestDto requestDto)
    {
        try
        {
            Expression<Func<UserAccount, bool>> filterUserName = x => !x.IsDeleted!.Value && x.UserName == requestDto.UserName;

            var existUser = await _dbContext.UserAccount.AnyAsync(filterUserName);

            if (existUser)
                return BadRequest("Ya existe un perfil con este nombre de usuario");

            Expression<Func<UserAccount, bool>> filterEmail = x => !x.IsDeleted!.Value && x.Email == requestDto.Email;

            var existEmail = await _dbContext.UserAccount.AnyAsync(filterEmail);

            if (existEmail)
                return BadRequest("Ya existe un usuario con este correo electrónico");

            var entity = await PopulateUserAccountPatient(requestDto);
            entity.Password = MD5Encrypt.GetMD5(requestDto.Password);
            await _dbContext.UserAccount.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            var result = _mapper.Map<UserAccountPatientResponseDto>(entity);
            var response = new ApiResponse<UserAccountPatientResponseDto>(result);
            return Ok(response);
        }
        catch (Exception ex)
        {

            throw new LogicBusinessException(ex);
        }
    }

    [HttpPost]
    [Route("Doctor")]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UserAccountDoctorResponseDto>))]
    public async Task<IActionResult> CreateDoctor([FromBody] UserAccountDoctorCreateRequestDto requestDto)
    {
        try
        {
            Expression<Func<UserAccount, bool>> filterUserName = x => !x.IsDeleted!.Value && x.UserName == requestDto.UserName;

            var existUser = await _dbContext.UserAccount.AnyAsync(filterUserName);

            if (existUser)
                return BadRequest("Ya existe un perfil con este nombre de usuario");

            Expression<Func<UserAccount, bool>> filterEmail = x => !x.IsDeleted!.Value && x.Email == requestDto.Email;

            var existEmail = await _dbContext.UserAccount.AnyAsync(filterEmail);

            if (existEmail)
                return BadRequest("Ya existe un usuario con este correo electrónico");

            var entity = await PopulateUserAccountDoctor(requestDto);
            entity.Password = MD5Encrypt.GetMD5(requestDto.Password);
            await _dbContext.UserAccount.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            var result = _mapper.Map<UserAccountDoctorResponseDto>(entity);
            var response = new ApiResponse<UserAccountDoctorResponseDto>(result);
            return Ok(response);
        }
        catch (Exception ex)
        {

            throw new LogicBusinessException(ex);
        }
    }


    private async Task<UserAccount> PopulateUserAccountPatient(UserAccountPatientCreateRequestDto requestDto)
    {
        Expression<Func<UserAccount, bool>> filter = x => !x.IsDeleted!.Value && x.Email == requestDto.Email;

        var existUser = await _dbContext.UserAccount.AnyAsync(filter);

        var userAccount = _mapper.Map<UserAccount>(requestDto);

        var patient = _mapper.Map<Patient>(requestDto);

        patient.UserAccount.Add(userAccount);
        userAccount.Patient.Add(patient);

        return userAccount;
    }

    private async Task<UserAccount> PopulateUserAccountDoctor(UserAccountDoctorCreateRequestDto requestDto)
    {
        Expression<Func<UserAccount, bool>> filter = x => !x.IsDeleted!.Value && x.Email == requestDto.Email;

        var existUser = await _dbContext.UserAccount.AnyAsync(filter);

        var userAccount = _mapper.Map<UserAccount>(requestDto);

        var doctor = _mapper.Map<Doctor>(requestDto);

        doctor.UserAccount.Add(userAccount);
        userAccount.Doctor.Add(doctor);

        return userAccount;
    }

    private async Task<PagedList<UserAccount>> GetPageds(UserAccountQueryFilter filter)
    {
        var result = await GetPaged(filter);
        var pagedItems = PagedList<UserAccount>.Create(result, filter.PageNumber, filter.PageSize);
        return pagedItems;
    }

    private async Task<IEnumerable<UserAccount>> GetPaged(UserAccountQueryFilter entity)
    {
        var query = _dbContext.UserAccount.AsQueryable();

        if (entity.Id > 0)
            query = query.Where(x => x.Id == entity.Id);

        if (!string.IsNullOrEmpty(entity.UserName) && !string.IsNullOrWhiteSpace(entity.UserName))
            query = query.Where(x => x.UserName.Contains(entity.UserName));

        if (!string.IsNullOrEmpty(entity.Email) && !string.IsNullOrWhiteSpace(entity.Email))
            query = query.Where(x => x.Email.Contains(entity.Email));

        return await query.ToListAsync();
    }
}