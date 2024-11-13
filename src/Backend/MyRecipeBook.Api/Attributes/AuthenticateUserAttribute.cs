using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Api.Filters;

namespace MyRecipeBook.Api.Attributes;

public class AuthenticateUserAttribute() : TypeFilterAttribute(typeof(AuthenticatedUserFilter));
