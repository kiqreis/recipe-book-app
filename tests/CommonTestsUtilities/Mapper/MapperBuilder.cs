using AutoMapper;
using MyRecipeBook.Application.Services.Mapping;

namespace CommonTestsUtilities.Mapper;

public class MapperBuilder
{
  public static IMapper Build() => new MapperConfiguration(opt =>
    {
      opt.AddProfile(new MappingProfile());
    }).CreateMapper();
}
