using ExchangeOrderSelecor.Contracts.Services;
using ExchangeOrderSelecor.Services;
using ExchangeOrderSelectorAPI.Contracts;
using ExchangeOrderSelectorAPI.Repository;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IFindBestOrderService, FindBestOrderService>();
builder.Services.AddSingleton<IGenerateSampleCustomerService, GenerateSampleCustomerService>();
builder.Services.AddSingleton<IGenerateCustomerOrderService, GenerateCustomerOrderService>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//lowercase Url's
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var app = builder.Build();

//Localization 
var supportedCultures = new[] { "en-US"};
//var localizationOptions =
//    new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
//    .AddSupportedCultures(supportedCultures)
//    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(supportedCultures);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
