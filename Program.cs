using Microsoft.EntityFrameworkCore;
using NeoRxTask.Data;
using NeoRxTask.Repositories;
using NeoRxTask.Repositories.IRepositories;
using NeoRxTask.Services;
using NeoRxTask.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Register Repositories
builder.Services.AddScoped<IBusinessCardRepository, BusinessCardRepository>();

// 🔹 Register Services
builder.Services.AddScoped<IBusinessCardService, BusinessCardService>();
//Register Service 
builder.Services.AddScoped<IBusinessCardXmlService, BusinessCardXmlService>();

// 🔹 Add Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
// 🔹 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:7165")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});



var app = builder.Build();



// 🔹 Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowDev");
app.UseStaticFiles();
//app.UseCors("AllowSwagger");

app.UseAuthorization();

app.MapControllers();

app.Run();
