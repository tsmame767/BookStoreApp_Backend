using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using ModelLayer.DTO;
using RepositoryLayer.Database;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Service;
using RepositoryLayer.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<DBContext>();

// Configure Redis Cache
var redisConnectionString = builder.Configuration.GetValue<string>("RedisCacheSettings:ConnectionString");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

builder.Services.AddScoped<IUserBL, UserBL>();
builder.Services.AddScoped<IUserRL, UserRL>();
builder.Services.AddScoped<IEmailServiceBL, EmailServiceBL>();
builder.Services.AddScoped<IEmailServiceRL, EmailServiceRL>();
builder.Services.AddScoped<ICacheServiceBL, CacheServiceBL>();
builder.Services.AddScoped<ICacheServiceRL, CacheServiceRL>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
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

app.Run();
