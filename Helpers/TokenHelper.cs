using System.Security.Claims;

namespace AW.Common.Helpers;

public class TokenHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenHelper(IHttpContextAccessor httpContextAccessor)
    {
        this._httpContextAccessor = httpContextAccessor;
    }

    public string GetFullName()
    {
        var identity = _httpContextAccessor.HttpContext!.User.Identity as ClaimsIdentity;
        var userName = identity!.FindFirst(ClaimTypes.Name);

        return userName!.Value;
    }

    public string GetUserName()
    {
        var identity = _httpContextAccessor.HttpContext!.User.Identity as ClaimsIdentity;
        var nameIdentifier = identity!.FindFirst(ClaimTypes.NameIdentifier);

        return nameIdentifier!.Value;
    }

    public int GetAccountId()
    {
        var identity = _httpContextAccessor.HttpContext!.User.Identity as ClaimsIdentity;
        var accountId = identity!.FindFirst(ClaimTypes.Sid);

        return int.Parse(accountId!.Value);
    }

    public string GetUserAccountType()
    {
        var identity = _httpContextAccessor.HttpContext!.User.Identity as ClaimsIdentity;
        var accountType = identity!.FindFirst("UserAccountType");

        return accountType!.Value;
    }

    public int GetDoctorrId()
    {
        var identity = _httpContextAccessor.HttpContext!.User.Identity as ClaimsIdentity;
        var doctorId = identity!.FindFirst("DoctorId");

        return int.Parse(doctorId!.Value);
    }

    public int GetPatientId()
    {
        var identity = _httpContextAccessor.HttpContext!.User.Identity as ClaimsIdentity;
        var patientId = identity!.FindFirst("PatientId");

        return int.Parse(patientId!.Value);
    }
}