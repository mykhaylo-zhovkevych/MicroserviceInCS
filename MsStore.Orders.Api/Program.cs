using Microsoft.EntityFrameworkCore;
using MsStore.Orders.Api.Client;
using MsStore.Orders.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<OrdersDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// HttpClient for talking to Store.Stocks.Api (a separate process, see its launchSettings.json for the port)
builder.Services.AddHttpClient<IStockClient, StockClient>(client =>
{ 
    var stockUrl = builder.Configuration["StockApiUrl"] ?? "http://localhost:5040/";
    client.BaseAddress = new Uri(stockUrl);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    var content = services.GetRequiredService<OrdersDbContext>();
    content.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders API v1");
        c.RoutePrefix = string.Empty;
    });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
