using AutoMapper;
using CommonTestsUtilities.IdEncrypt;
using MyRecipeBook.Application.Services.Mapping;

namespace CommonTestsUtilities.Mapper;

public class MapperBuilder
{
  public static IMapper Build()
  {
    var idEncrypt = IdEncryptBuilder.Build();
    var mapper = new MapperConfiguration(opt =>
    {
      opt.AddProfile(new MappingProfile(idEncrypt));
    }).CreateMapper();

    return mapper;
  }
}
