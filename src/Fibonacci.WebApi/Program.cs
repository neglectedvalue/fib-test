using Abstractions.Fibonacci;
using Abstractions.MessageSender;
using Configuration;
using FibonacciGenerator;
using MessageSender.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IMessageSender, RabbitMqMessageSender>();
builder.Services.AddSingleton<IFibonacciGenerator<FibonacciValueDto>, StatefulFibonacciGenerator>();

builder.Configuration.AddAllSettingFiles();

builder.Services.AddRabbitMq(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();