using Microsoft.OpenApi.Models;
using MyRecipeBook.Api.Converters;
using MyRecipeBook.Api.Filters;
using MyRecipeBook.Api.Middleware;
using MyRecipeBook.Api.Token;
using MyRecipeBook.Application;
using MyRecipeBook.Domain.Security.Token;
using MyRecipeBook.Infrastructure;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opt =>
{
  opt.JsonSerializerOptions.Converters.Add(new StringConverter());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
  opt.OperationFilter<IdFilter>();

  opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    Description = @"Jwt Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in text input below
                  . Example: 'Bearer 123456abcdef'",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer"
  });

  opt.AddSecurityRequirement(new OpenApiSecurityRequirement
  {
    {
      new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id = "Bearer"
        },
        Scheme = "oauth2",
        Name = "Bearer",
        In = ParameterLocation.Header
      },
      new List<string>()
    }
  });
});

builder.Services.AddMvc(opt => opt.Filters.Add(typeof(ExceptionFilter)));
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenProvider>();

builder.Services.AddRouting(opt => opt.LowercaseUrls = true);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
MigrateDb();
app.Run();

void MigrateDb()
{
  if (builder.Configuration.IsUnitTestEnvironment())
  {
    return;
  }

  var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

  DbMigration.Migration(builder.Configuration.ConnectionString(), serviceScope.ServiceProvider);
}

public partial class Program;