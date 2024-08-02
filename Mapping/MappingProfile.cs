using AMNApi.Dtos.QueryFilters;
using AMNApi.Dtos.Request.Create;
using AMNApi.Dtos.Request.Update;
using AMNApi.Dtos.Response;
using AMNApi.Entities;
using AMNApi.Entities.Enumerations;
using AMNApi.Helpers;
using AutoMapper;
using AW.Common.Helpers;

namespace AMNApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        // ResponseMapping

        CreateMap<Appointment, AppointmentResponseDto>()
        .ForMember(
            dest => dest.IsDeleted,
            opt => opt.MapFrom(src => src.IsDeleted)
        ).ForMember(
            dest => dest.StatusName,
            opt => opt.MapFrom(src => EnumHelper.GetDescription<AppoinmentStatus>((AppoinmentStatus)src.Status))
        ).ForMember(
            dest => dest.IsActive,
            opt => opt.MapFrom(src => StatusDeletedHelper.GetStatusDeletedEntity(src.IsDeleted))
        ).ForMember(
            dest => dest.AppoinmentDate,
            opt => opt.MapFrom(src => src.AppoinmentDate)
        );

        CreateMap<Consultory, ConsultoryResponseDto>()
        .ForMember(
            dest => dest.Code,
            opt => opt.MapFrom(src => Guid.NewGuid())
        )
        .AfterMap(
            (src, dest) =>
            {
                var address = src.Address.FirstOrDefault() ?? new Address();

                dest.Address1 = address.Address1;
                dest.Address2 = address.Address2;
                dest.Street = address.Street;
                dest.ExternalNumber = address.ExternalNumber;
                dest.InternalNumber = address.InternalNumber;
                dest.ZipCode = address.ZipCode;
                dest.FullAddress = address.FullAddress;

                var mapLocation = src.MapLocation.FirstOrDefault() ?? new MapLocation();

                dest.Latitude = mapLocation.Latitude;
                dest.Longitude = mapLocation.Longitude;
            }
        );

        CreateMap<MapLocation, MapLocationResponseDto>();

        CreateMap<Patient, PatientResponseDto>()
        .ForMember(
            dest => dest.GenderName,
            opt => opt.MapFrom(src => EnumHelper.GetDescription<Gender>((Gender)src.Gender!))
        );

        CreateMap<UserAccount, UserAccountResponseDto>()
        .ForMember(
            dest => dest.UserName,
            opt => opt.MapFrom(src => src.UserName)
        ).ForMember(
            dest => dest.IsDeleted,
            opt => opt.MapFrom(src => src.IsDeleted)
        ).ForMember(
            dest => dest.AccountTypeName,
            opt => opt.MapFrom(src => EnumHelper.GetDescription<UserAccountType>((UserAccountType)src.AccountType))
        );

        CreateMap<UserAccount, UserAccountPatientResponseDto>()
        .ForMember(
            dest => dest.UserName,
            opt => opt.MapFrom(src => src.UserName)
        ).ForMember(
            dest => dest.IsDeleted,
            opt => opt.MapFrom(src => src.IsDeleted)
        ).AfterMap(
            (src, dest) =>
            {
                var patient = src.Patient.FirstOrDefault() ?? new Patient();
                dest.PatientId = patient.Id;
                dest.FullName = patient.FullName;
                dest.CellPhone = patient.CellPhone;
                dest.UserAccountType = src.AccountType;
                dest.UserAccountTypeName = EnumHelper.GetDescription<UserAccountType>((UserAccountType)src.AccountType);
            }
        );

        CreateMap<UserAccount, UserAccountDoctorResponseDto>()
        .ForMember(
            dest => dest.UserName,
            opt => opt.MapFrom(src => src.UserName)
        ).ForMember(
            dest => dest.IsDeleted,
            opt => opt.MapFrom(src => src.IsDeleted)
        ).AfterMap(
            (src, dest) =>
            {
                var doctor = src.Doctor.FirstOrDefault() ?? new Doctor();
                dest.DoctorId = doctor.Id;
                dest.FullName = doctor.FullName;
                dest.UserAccountType = src.AccountType;
                dest.UserAccountTypeName = EnumHelper.GetDescription<UserAccountType>((UserAccountType)src.AccountType);

                var consultory = doctor.Consultory ?? new Consultory();
                dest.ConsultoryId = consultory.Id;
                dest.ConsultoryName = consultory.Name;
            }
        );

        // CreateRequestMapping

        CreateMap<AppointmentCreateRequestDto, Appointment>()
        .ForMember(
            dest => dest.Status,
            opt => opt.MapFrom(src => (short)AppoinmentStatus.Scheduled)
        ).ForMember(
            dest => dest.AppoinmentDate,
            opt => opt.MapFrom(src => src.AppointmentDate)
        ).ForMember(
            dest => dest.IsDeleted,
            opt => opt.MapFrom(src => ValuesStatusPropertyEntity.IsNotDeleted)
        );

        CreateMap<ConsultoryCreateRequestDto, Consultory>()
        .AfterMap(
            (src, dest) =>
            {
                dest.Code = Guid.NewGuid();
                dest.Name = src.Name;
                dest.Phone = src.Phone;
                dest.IsDeleted = ValuesStatusPropertyEntity.IsNotDeleted;
                dest.CreatedDate = DateTime.Now;

                var address = new Address
                {
                    Address1 = src.Address1,
                    Address2 = src.Address2,
                    Street = src.Street,
                    ExternalNumber = src.ExternalNumber,
                    InternalNumber = src.InternalNumber,
                    ZipCode = src.ZipCode,
                };
                dest.Address.Add(address);

                var mapLocation = new MapLocation {
                    Latitude = src.Latitude,
                    Longitude = src.Longitude
                };
                dest.MapLocation.Add(mapLocation);
            }
        );

        CreateMap<MapLocationCreateRequestDto, MapLocation>();

        CreateMap<UserAccountPatientCreateRequestDto, Patient>()
        .AfterMap(
            (src, dest) =>
            {
                dest.FirstName = "Asignar";
                dest.LastName = "Asignar";
                dest.CellPhone = "Asignar";
                dest.Code = Guid.NewGuid();
                dest.IsDeleted = ValuesStatusPropertyEntity.IsNotDeleted;
                dest.CreatedDate = DateTime.Now;
                dest.Gender = (short)Gender.DontSay;
                dest.BirthDate = DateTime.Now;

                var customerAddress = new PatientAddress
                {
                    RegisterDate = DateTime.Now,
                    IsDefault = true,
                    Address = new Address
                    {
                        Address1 = "Asignar",
                        Address2 = "Asignar",
                        Street = "Asignar",
                        ExternalNumber = "Asignar",
                        InternalNumber = "Asignar",
                        ZipCode = "Asignar",
                    }
                };
                dest.PatientAddress.Add(customerAddress);
            }
        );

        CreateMap<UserAccountPatientCreateRequestDto, UserAccount>()
        .ForMember(
            dest => dest.IsDeleted,
            opt => opt.MapFrom(src => ValuesStatusPropertyEntity.IsNotDeleted)
        ).ForMember(
            dest => dest.IsActive,
            opt => opt.MapFrom(src => true)
        ).ForMember(
            dest => dest.IsAuthorized,
            opt => opt.MapFrom(src => true)
        ).ForMember(
            dest => dest.CreatedDate,
            opt => opt.MapFrom(src => DateTime.Now)
        ).ForMember(
            dest => dest.AccountType,
            opt => opt.MapFrom(src => (short)UserAccountType.Patient)
        ).ForMember(
            dest => dest.Email,
            opt => opt.MapFrom(src => src.Email)
        );

        CreateMap<UserAccountDoctorCreateRequestDto, Doctor>()
        .AfterMap(
            (src, dest) =>
            {
                dest.FirstName = src.FirstName;
                dest.MiddleName = src.MiddleName;
                dest.LastName = src.LastName;
                dest.Code = Guid.NewGuid();
                dest.IsDeleted = ValuesStatusPropertyEntity.IsNotDeleted;
                dest.CreatedDate = DateTime.Now;
                dest.Gender = (short)Gender.DontSay;
                dest.BirthDate = DateTime.Now;
                dest.ConsultoryId = src.ConsultoryId;
            }
        );

        CreateMap<UserAccountDoctorCreateRequestDto, UserAccount>()
        .ForMember(
            dest => dest.IsDeleted,
            opt => opt.MapFrom(src => ValuesStatusPropertyEntity.IsNotDeleted)
        ).ForMember(
            dest => dest.IsActive,
            opt => opt.MapFrom(src => true)
        ).ForMember(
            dest => dest.IsAuthorized,
            opt => opt.MapFrom(src => true)
        ).ForMember(
            dest => dest.CreatedDate,
            opt => opt.MapFrom(src => DateTime.Now)
        ).ForMember(
            dest => dest.AccountType,
            opt => opt.MapFrom(src => (short)UserAccountType.Doctor)
        ).ForMember(
            dest => dest.Email,
            opt => opt.MapFrom(src => src.Email)
        );

        // UpdateMapping

        CreateMap<PatientUpdateRequestDto, PatientAddress>()
        .AfterMap(
            (src, dest) => 
            {
                dest.Address = new Address
                {
                    Address1 = src.Address1,
                    Address2 = src.Address2,
                    Street = src.Street,
                    ExternalNumber = src.ExternalNumber,
                    InternalNumber = src.InternalNumber,
                    ZipCode = src.ZipCode
                };
            }
        );

        CreateMap<PatientUpdateRequestDto, Patient>();

        // QueryFilterMapping

        CreateMap<ConsultoryQueryFilter, Consultory>();

        CreateMap<MapLocationQueryFilter, MapLocation>();

        CreateMap<UserAccountQueryFilter, UserAccount>();
    }
}