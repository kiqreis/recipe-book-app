using MyRecipeBook.Api.Filters;
using MyRecipeBook.Api.Middleware;
using MyRecipeBook.Application.SecurityConfig;
using MyRecipeBook.Application.Services.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc(opt => opt.Filters.Add(typeof(ExceptionFilter)));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<PasswordEncrypt>();

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
app.Run();