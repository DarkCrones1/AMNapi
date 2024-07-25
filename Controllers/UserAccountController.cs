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
public class UserAccountController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly AMNDbContext _dbContext;

    public UserAccountController(IMapper mapper, AMNDbContext dbContext)
    {
        this._mapper = mapper;
        this._dbContext = dbContext;
    }

    [HttpPost]
    [Route("Customer")]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UserAccountPatientResponseDto>))]
    public async Task<IActionResult> Create([FromBody] UserAccountPatientCreateRequestDto requestDto)
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
}