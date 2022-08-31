using ExchangeOrderSelecor.Contracts.Services;
using ExchangeOrderSelecor.Services;
using ExchangeOrderSelectorAPI.Contracts;
using ExchangeOrderSelectorAPI.Repository;

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

var app = builder.Build();

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
