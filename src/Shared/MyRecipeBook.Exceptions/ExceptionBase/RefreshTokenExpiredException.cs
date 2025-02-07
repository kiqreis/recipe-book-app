﻿
using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionBase;

public class RefreshTokenExpiredException() : MyRecipeBookException(ResourceMessagesException.INVALID_SESSION)
{
  public override IList<string> GetErrorMessages() => [Message];

  public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
}
