using Application.Operations.Appointments;
using Application.Operations.Roles;
using Application.Operations.Services;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Role, RoleDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => CapitalizeFirstLettter(s.Name)));

            CreateMap<Service, ServiceDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => CapitalizeFirstLettter(s.Name)));

            CreateMap<Appointment, AppointmentDto>()
                .ForMember(d => d.PatientId, o => o.MapFrom(s => s.Patient.IdDocument))
                .ForMember(d => d.PatientName, o => o.MapFrom(s =>
                    CapitalizeFirstLettter(s.Patient.Name) + " " + CapitalizeFirstLettter(s.Patient.FirstLastname)))
                .ForMember(d => d.Service, o => o.MapFrom(s => s.Service.Name));
        }

        private static string CapitalizeFirstLettter(string word)
        {
            return char.ToUpper(word[0]) + word.Substring(1);
        }
    }
}