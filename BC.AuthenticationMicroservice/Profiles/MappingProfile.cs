using AutoMapper;
using BC.AuthenticationMicroservice.Boundary.Request;
using BC.AuthenticationMicroservice.Boundary.Response;
using BC.AuthenticationMicroservice.Models;

namespace BC.AuthenticationMicroservice.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserWithRole>()
                .ForMember(dest => 
                    dest.Role,
                    opt => opt.MapFrom(src => src.UserRoles.FirstOrDefault().Role.Name));
            
            CreateMap<RegisterRequest, User>()
                .ForMember(dest =>
                    dest.UserName,
                    opt => opt.MapFrom(src => $"{src.FirstName}_{src.SecondName}"));

            CreateMap<UserUpdateDto, User>();
            CreateMap<Role, GetRole>();
        }
    }
}
