using MyRecipeBook.Api.Converters;
using MyRecipeBook.Api.Filters;
using MyRecipeBook.Api.Middleware;
using MyRecipeBook.Application;
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
builder.Services.AddSwaggerGen();
builder.Services.AddMvc(opt => opt.Filters.Add(typeof(ExceptionFilter)));
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddRouting(opt => opt.LowercaseUrls = true);

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