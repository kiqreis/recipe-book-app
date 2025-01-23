using System.Collections;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;

public class MyRecipeBookClassFixture(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
  private readonly HttpClient _httpClient = factory.CreateClient();

  protected async Task<HttpResponseMessage> Post(string method, object request, string token = "", string culture = "en")
  {
    ChangeRequestCulture(culture);
    return await _httpClient.PostAsJsonAsync(method, request);
  }

  protected async Task<HttpResponseMessage> PostFormData(string method, object request, string token, string culture = "en")
  {
    ChangeRequestCulture(culture);
    AuthorizeRequest(token);

    var multipartContent = new MultipartFormDataContent();
    var requestProperties = request.GetType().GetProperties().ToList();

    foreach (var prop in requestProperties)
    {
      var propValue = prop.GetValue(request);

      if (string.IsNullOrWhiteSpace(propValue?.ToString()))
      {
        continue;
      }

      if (propValue is IList list)
      {
        AddListToMultipartContent(multipartContent, prop.Name, list);
      }
      else
      {
        multipartContent.Add(new StringContent(propValue.ToString()!), prop.Name);
      }
    }

    return await _httpClient.PostAsync(method, multipartContent);
  }

  protected async Task<HttpResponseMessage> Get(string method, string token = "", string culture = "en")
  {
    ChangeRequestCulture(culture);
    AuthorizeRequest(token);

    return await _httpClient.GetAsync(method);
  }

  protected async Task<HttpResponseMessage> Put(string method, object request, string token, string culture = "en")
  {
    ChangeRequestCulture(culture);
    AuthorizeRequest(token);

    return await _httpClient.PutAsJsonAsync(method, request);
  }

  protected async Task<HttpResponseMessage> Delete(string method, string token, string culture = "en")
  {
    ChangeRequestCulture(culture);
    AuthorizeRequest(token);

    return await _httpClient.DeleteAsync(method);
  }

  private void ChangeRequestCulture(string culture)
  {
    if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
    {
      _httpClient.DefaultRequestHeaders.Remove("Accept-Language");
    }

    _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
  }

  private void AuthorizeRequest(string token)
  {
    if (string.IsNullOrWhiteSpace(token))
    {
      return;
    }

    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
  }

  private void AddListToMultipartContent(MultipartFormDataContent multipartContent, string propName, IList list)
  {
    var itemType = list.GetType().GetGenericArguments().Single();

    if (itemType.IsClass && itemType != typeof(string))
    {
      AddClassListToMultpartContent(multipartContent, propName, list);
    }

    foreach (var item in list)
    {
      multipartContent.Add(new StringContent(item.ToString()!), propName);
    }
  }

  private void AddClassListToMultpartContent(MultipartFormDataContent multipartContent, string propName, IList list)
  {
    var index = 0;

    foreach (var item in list)
    {
      var classPropertiesInfo = item.GetType().GetProperties().ToList();

      foreach (var prop in classPropertiesInfo)
      {
        var value = prop.GetValue(item, null);

        multipartContent.Add(new StringContent(value!.ToString()!), $"{propName}[{index}][{prop.Name}]");
      }

      index++;
    }
  }
}
