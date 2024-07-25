namespace AMNApi.Helpers;

public class SettingConfigurationFile
{
    private static SettingConfigurationFile? _instance;
    private readonly IConfiguration _configuration;

    public static SettingConfigurationFile Instance
    {
        get
        {
            if (_instance == null)
            {
                throw new InvalidOperationException("SettingsConfigurationFile has not been initialized");
            }
            return _instance;
        }
    }

    private SettingConfigurationFile(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static void Initialize(IConfiguration configuration)
    {
        if (_instance != null)
        {
            return;
        }
        _instance = new SettingConfigurationFile(configuration);
    }

    public int DoctorAccountTypeId => int.Parse(_configuration.GetValue<string>("DefaultValues:doctorAccountTypeId")!);
    public int PatientAccountTypeId => int.Parse(_configuration.GetValue<string>("DefaultValues:patientAccountTypeId")!);
}