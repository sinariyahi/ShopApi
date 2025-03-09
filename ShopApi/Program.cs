using Application;
using Domain;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.GlobalExceptionHandling;
using Infrastructure.Models.PaymentGateway;
using Microsoft.Extensions.DependencyInjection;
using ShopApi.Installers;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Path.GetFullPath(Directory.GetCurrentDirectory()),
    WebRootPath = Path.GetFullPath(Directory.GetCurrentDirectory()),
    Args = args
});

builder.Services.AddOptions();
builder.Services.Configure<Configs>(builder.Configuration.GetSection("Configs"));



builder.Services.Configure<PaymentConfigs>(builder.Configuration.GetSection("PaymentConfigs"));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicaiton();
builder.Services.AddCore();
builder.Services.AddInfrastructure();
builder.Services.AddSwagger();
builder.Services.AddJWT();
builder.Services.AddCustomCors();
builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

#region AutoMapper
var config = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new Application.AutoMapper());
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);
#endregion


builder.Services.AddMemoryCache();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop.API v1"));
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("ShopAPI");

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();