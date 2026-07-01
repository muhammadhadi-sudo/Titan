using Titan.Controllers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Services
// =========================
builder.Services.AddControllers();

// Swagger (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TITAN API",
        Version = "v1",
        Description = "Threat Intelligence & Tactical Access Network API"
    });
});

var app = builder.Build();

// =========================
// Middleware
// =========================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Titan API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// =========================
// HID Aero Init
// =========================
try
{
    StartAero.Communication();
    Console.WriteLine("HID Aero initialized successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"HID Aero init failed: {ex.Message}");
}

app.Run();