using System.Globalization;

namespace MyRecipeBook.Api.Middleware;

public class CultureMiddleware(RequestDelegate next)
{
  public async Task Invoke(HttpContext context)
  {
    var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
    var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();
    var cultureInfo = new CultureInfo("en");

    if (!string.IsNullOrWhiteSpace(requestedCulture) && supportedLanguages
      .Exists(c => c.Name.Equals(requestedCulture)))
    {
      cultureInfo = new CultureInfo(requestedCulture);
    }

    CultureInfo.CurrentCulture = cultureInfo;
    CultureInfo.CurrentUICulture = cultureInfo;

    await next(context);
  }
}