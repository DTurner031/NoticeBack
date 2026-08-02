using Microsoft.EntityFrameworkCore;
using PortalNoticiasAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PortalNoticiasContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("PortalNoticiasDb"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("PortalNoticiasDb"))
    )
);

// Add services to the container.

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

app.UseStaticFiles();

app.Run();
