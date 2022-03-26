using Microsoft.EntityFrameworkCore;
using ProcessManager.Application.ProcessManager;
using ProcessManager.Infrastructure.Interfaces;
using ProcessManager.Infrastructure.Persistence;
using ProcessManager.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<CardDbContext>(opt => 
    opt.UseInMemoryDatabase(databaseName: "Card"));

builder.Services.AddControllers();

builder.Services.AddScoped<CardProcessManager>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
