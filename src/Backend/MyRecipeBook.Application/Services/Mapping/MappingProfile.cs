using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using Sqids;

namespace MyRecipeBook.Application.Services.Mapping;

public class MappingProfile : Profile
{
  private readonly SqidsEncoder<long> _encoder;

  public MappingProfile(SqidsEncoder<long> encoder)
  {
    _encoder = encoder;

    CreateMap<CreateUserRequest, User>()
      .ForMember(target => target.Password, opt => opt.Ignore())
      .ReverseMap();

    CreateMap<CreateUserResponse, User>().ReverseMap();

    CreateMap<UserProfileResponse, User>().ReverseMap();

    CreateMap<RecipeRequest, Recipe>()
      .ForMember(dest => dest.Instructions, opt => opt.Ignore())
      .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(source => source.Ingredients.Distinct()))
      .ForMember(dest => dest.DishTypes, opt => opt.MapFrom(source => source.DishTypes.Distinct()));

    CreateMap<string, Ingredient>()
      .ForMember(dest => dest.Item, opt => opt.MapFrom(source => source));

    CreateMap<Communication.Enums.DishType, DishType>()
      .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source));

    CreateMap<InstructionRequest, Instruction>();

    CreateMap<Recipe, CreatedRecipeResponse>()
      .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _encoder.Encode(source.Id)));
  }
}