using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Application.Services.Mapping;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<CreateUserRequest, User>()
      .ForMember(target => target.Password, opt => opt.Ignore())
      .ReverseMap();
    
    CreateMap<CreateUserResponse, User>().ReverseMap();

    CreateMap<UserProfileResponse, User>().ReverseMap();
  }
}