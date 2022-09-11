using ExchangeOrderSelecor.Contracts.Services;
using ExchangeOrderSelecor.Services;
using ExchangeOrderSelectorAPI.Contracts;
using ExchangeOrderSelectorAPI.Repository;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IFindBestOrderService, FindBestOrderService>();
builder.Services.AddSingleton<IGenerateSampleCustomerService, GenerateSampleCustomerService>();
builder.Services.AddSingleton<IGenerateCustomerOrderService, GenerateCustomerOrderService>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//lowercase Url's
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var app = builder.Build();

//Localization 
var supportedCultures = new[] { "en-US" };
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


//app.Use(async (context, next) =>
//{
    
//    var hasSecrete = context.Request.Headers.TryGetValue("secrete", out var secrete);
//    if (hasSecrete == false)
//    {
//        await context.Response.WriteAsync("secrete header is missing.");
//        context.Response.StatusCode = 401;
//        return;
//    }

//    // If SecreteValue from header not equals SecreteValue from properties then reuturn also 401

//    await next.Invoke();
//});

app.MapControllers();

app.Run();
