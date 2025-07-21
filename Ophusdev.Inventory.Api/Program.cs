using Inventory.Business;
using Inventory.Business.Abstraction;
using Inventory.Repository;
using Inventory.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using Ophusdev.Inventory.Api.Extensions;
using Ophusdev.Inventory.Business.Abstraction;
using Ophusdev.Inventory.Business.Service;
using Ophusdev.Inventory.Business.Services;
using Ophusdev.Kafka.Abstraction;
using Ophusdev.Kafka.Configuration;
using Ophusdev.Kafka.Consumer;
using Ophusdev.Kafka.Extensions;
using Ophusdev.Kafka.Producer;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.Services.AddDbContext<InventoryDbContext>(options => options.UseSqlServer("name=ConnectionStrings:InventoryDbContext", b => b.MigrationsAssembly("Ophusdev.Inventory.Api")));

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IBusiness, Business>();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddSingleton<ITopicTranslator, TopicTranslator>();
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
builder.Services.AddSingleton<IKafkaConsumer, KafkaConsumer>();

builder.Services.AddScoped<IInventoryService, InventoryService>();

builder.Services.AddKafkaTopicHandlers();

builder.Services.AddHostedService<SagaConsumerService>();

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
