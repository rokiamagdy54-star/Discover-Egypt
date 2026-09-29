using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Features.Nationalities.DTOs;

namespace Tourism.Core.Mapping
{
    public class NationalityProfile : Profile
    {
        public NationalityProfile()
        {
            CreateMap<Nationality, NationalityDto>();
            CreateMap<CreateNationalityDto, Nationality>();
        }
    }
}