﻿using MassTransit;
using RabbitMQ.Share.Model;
using RabbitMQ.Share.Service;
using RabbitMQ.Share.Service.IService;
using RabbitMQSender;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserRequestSender>();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        config.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
       
        
        config.ReceiveEndpoint("user_queue", ep =>
        {
           
            ep.Lazy = true;
            ep.Bind<GetUserRequest>();
            ep.ConfigureConsumer<UserRequestSender>(provider);
        });
        
       // config.ConfigureEndpoints(provider);

    }));
});

//builder.Services.AddScoped<ISubscriber, Subscriber>();
builder.Services.AddMassTransitHostedService();




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

app.Run();

