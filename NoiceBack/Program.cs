using Microsoft.EntityFrameworkCore;
using PortalNoticiasAPI.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration
    .GetConnectionString("PortalNoticiasDb");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "No se encontró la cadena de conexión 'PortalNoticiasDb'. " +
        "Configúrala mediante User Secrets o variables de entorno."
    );
}

builder.Services.AddDbContext<PortalNoticiasContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    )
);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();