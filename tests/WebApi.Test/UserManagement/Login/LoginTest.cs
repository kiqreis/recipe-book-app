﻿using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.UserManagement.Login;

public class LoginTest : MyRecipeBookClassFixture
{
  private readonly string method = "login";
  private readonly string _email;
  private readonly string _name;
  private readonly string _password;

  public LoginTest(CustomWebApplicationFactory factory) : base(factory)
  {
    _email = factory.GetEmail();
    _name = factory.GetName();
    _password = factory.GetPassword();
  }

  [Fact]
  public async Task Success()
  {
    var request = new RequestLogin
    {
      Email = _email,
      Password = _password
    };

    var response = await Post(method: method, request: request);

    response.StatusCode.Should().Be(HttpStatusCode.OK);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);

    responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace()
      .And.Be(_name);
    responseData.RootElement.GetProperty("token").GetProperty("accessToken").GetString().Should().NotBeNullOrEmpty();
  }

  [Theory]
  [ClassData(typeof(CultureInlineData))]
  public async Task Error_Login_Invalid(string culture)
  {
    var request = LoginUserRequestBuilder.Build();

    var response = await Post(method: method, request: request, culture: culture);

    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);
    var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
    var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

    errors.Should().ContainSingle().And.Contain(e => e.GetString()!.Equals(expectedMessage));
  }
}
