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
using AMNApi.Dtos;
using AMNApi.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace AMNApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LoginController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly AMNDbContext _dbContext;
    protected ActiveUserAccountDoctor _userDoctor;
    protected ActiveUserAccountPatient _userPatient;

    public LoginController(IMapper mapper, IConfiguration configuration, AMNDbContext dbContext)
    {
        this._mapper = mapper;
        this._configuration = configuration;
        this._dbContext = dbContext;
        _userDoctor = new ActiveUserAccountDoctor();
        _userPatient = new ActiveUserAccountPatient();
        SettingConfigurationFile.Initialize(_configuration);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> SignIn([FromBody] LoginRequestDto requestDto)
    {
        try
        {
            var result = await IsValidUser(requestDto.UserNameOrEmail!, MD5Encrypt.GetMD5(requestDto.Password!));

            if (!result)
                return NotFound("El Usuario no es válido, revise que el Usuario/Email o la Contraseña sean correctos");

            _userDoctor = await GetDoctor(requestDto);
            if (_userDoctor.AccountType == 1)
            {
                var token = await GenerateTokenDoctor();
                return Ok(new { token });
            }

            _userPatient = await GetPatient(requestDto);
            if (_userPatient.AccountType == 2)
            {
                var token = await GenerateTokenPatient();
                return Ok(new { token });
            }

            return null!;
        }
        catch (Exception)
        {

            throw new LogicBusinessException("No se ha encontrado ningun usuario");
        }
    }

    private async Task<string> GenerateTokenDoctor()
    {
        // Header
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]!));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var header = new JwtHeader(signingCredentials);

        var lstClaims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, _userDoctor!.UserName),
                new Claim(ClaimTypes.Name, _userDoctor!.Name),
                new Claim(ClaimTypes.Email, _userDoctor.Email),
                new Claim(ClaimTypes.Sid, $"{_userDoctor.Id}"),
                new Claim(ClaimTypes.DateOfBirth, DateTime.Now.ToString()),
                //new Claim("", "") //TODO: agregar valores personalizados
                new Claim("UserAccountType", $"{_userDoctor.AccountType}"),
                new Claim("DoctorId", $"{_userDoctor.DoctorId}")
            };

        // Payload
        var elapsedTime = int.Parse(_configuration["Authentication:ExpirationMinutes"]!);

        var payload = new JwtPayload(
            issuer: _configuration["Authentication:Issuer"],
            audience: _configuration["Authentication:Audience"],
            claims: lstClaims,
            notBefore: DateTime.Now,
            expires: DateTime.UtcNow.AddMinutes(elapsedTime)
        );

        // Signature
        var token = new JwtSecurityToken(header, payload);

        await Task.CompletedTask;

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<string> GenerateTokenPatient()
    {
        // Header
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]!));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var header = new JwtHeader(signingCredentials);

        var lstClaims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, _userPatient!.UserName),
                new Claim(ClaimTypes.Name, _userPatient!.Name),
                new Claim(ClaimTypes.Email, _userPatient.Email),
                new Claim(ClaimTypes.Sid, $"{_userPatient.Id}"),
                new Claim(ClaimTypes.DateOfBirth, DateTime.Now.ToString()),
                //new Claim("", "") //TODO: agregar valores personalizados
                new Claim("UserAccountType", $"{_userPatient.AccountType}"),
                new Claim("DoctorId", $"{_userPatient.PatientId}")
            };

        // Payload
        var elapsedTime = int.Parse(_configuration["Authentication:ExpirationMinutes"]!);

        var payload = new JwtPayload(
            issuer: _configuration["Authentication:Issuer"],
            audience: _configuration["Authentication:Audience"],
            claims: lstClaims,
            notBefore: DateTime.Now,
            expires: DateTime.UtcNow.AddMinutes(elapsedTime)
        );

        // Signature
        var token = new JwtSecurityToken(header, payload);

        await Task.CompletedTask;

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<bool> IsValidUser(string UserNameOrEmail, string password)
    {
        Expression<Func<UserAccount, bool>> filters = x =>
                (x.UserName == UserNameOrEmail || x.Email == UserNameOrEmail)
                && x.Password == password
                && x.IsActive
                && x.IsAuthorized
                && !x.IsDeleted!.Value;

        var result = await _dbContext.UserAccount.AnyAsync(filters);

        return result;
    }

    private async Task<ActiveUserAccountDoctor> GetDoctor(LoginRequestDto requestDto)
    {
        Expression<Func<ActiveUserAccountDoctor, bool>> filters = x => x.UserName == requestDto.UserNameOrEmail || x.Email == requestDto.UserNameOrEmail;
        var entity = await GetUserAccountDoctorToLogin(filters);
        return entity;
    }

    private async Task<ActiveUserAccountDoctor> GetUserAccountDoctorToLogin(Expression<Func<ActiveUserAccountDoctor, bool>> filters)
    {
        var entity = await _dbContext.ActiveUserAccountDoctor.Where(filters)
        .AsNoTracking()
        .FirstOrDefaultAsync();

        return entity ?? new ActiveUserAccountDoctor();
    }

    private async Task<ActiveUserAccountPatient> GetPatient(LoginRequestDto requestDto)
    {
        Expression<Func<ActiveUserAccountPatient, bool>> filters = x => x.UserName == requestDto.UserNameOrEmail || x.Email == requestDto.UserNameOrEmail;
        var entity = await GetUserAccountPatientToLogin(filters);
        return entity;
    }

    private async Task<ActiveUserAccountPatient> GetUserAccountPatientToLogin(Expression<Func<ActiveUserAccountPatient, bool>> filters)
    {
        var entity = await _dbContext.ActiveUserAccountPatient.Where(filters)
        .AsNoTracking()
        .FirstOrDefaultAsync();

        return entity ?? new ActiveUserAccountPatient();
    }
}