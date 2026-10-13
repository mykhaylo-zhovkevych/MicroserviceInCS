using Microsoft.EntityFrameworkCore;
using MsStore.Stocks.Api.Clients;
using MsStore.Stocks.Api.Data;
using MsStore.Stocks.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<StockDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<UserSyncService>();

// HttpClient for talking to MsStore.Users.Api (a separate process, see its launchSettings.json for the port)
builder.Services.AddHttpClient<UserClient>(client =>
{
    var usersUrl = builder.Configuration["UsersApiUrl"] ?? "http://localhost:5154/";
    client.BaseAddress = new Uri(usersUrl);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StockDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
