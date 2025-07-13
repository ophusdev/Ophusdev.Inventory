using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

using Inventory.Business;
using Inventory.Business.Abstraction;
using Inventory.Repository;
using Inventory.Repository.Abstraction;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.Services.AddDbContext<InventoryDbContext>(options => options.UseSqlServer("name=ConnectionStrings:InventoryDbContext", b => b.MigrationsAssembly("Ophusdev.Inventory.Api")));

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IBusiness, Business>();

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

await app.RunAsync();
