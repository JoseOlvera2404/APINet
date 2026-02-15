using Microsoft.EntityFrameworkCore;
using TrufasAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Obtener connection string (local o Railway)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    )
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger (lo dejamos activo también en producción porque es práctica)
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

// 🔥 IMPORTANTE PARA RAILWAY
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

app.Run();