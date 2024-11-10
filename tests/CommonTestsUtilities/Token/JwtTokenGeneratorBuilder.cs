using MyRecipeBook.Domain.Security.Token;
using MyRecipeBook.Infrastructure.Security.Token.Access.Generator;

namespace CommonTestsUtilities.Token;

public class JwtTokenGeneratorBuilder
{
  public static IAccessTokenGenerator Build() => new JwtTokenGenerator(5, "E)e/xkdqJ@RtnX#$9h<8B([sF_gp*W!Y");
}
