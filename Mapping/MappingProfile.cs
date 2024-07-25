using AMNApi.Dtos.Request.Create;
using AMNApi.Dtos.Response;
using AMNApi.Entities;
using AMNApi.Entities.Enumerations;
using AMNApi.Helpers;
using AutoMapper;

namespace AMNApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        // ResponseMapping

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

        // CreateRequestMapping

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

        // UpdateMapping

        // QueryFilterMapping
    }
}