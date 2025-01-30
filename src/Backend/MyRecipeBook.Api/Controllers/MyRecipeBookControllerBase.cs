using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Domain.Extensions;

namespace MyRecipeBook.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class MyRecipeBookControllerBase : ControllerBase
{
  protected static bool IsNotAuthenticated(AuthenticateResult authenticate)
  {
    return authenticate.Succeeded.IsFalse() || authenticate.Principal == null || authenticate.Principal.Identities.Any(id =>
      id.IsAuthenticated.IsFalse()
    );
  }
}
