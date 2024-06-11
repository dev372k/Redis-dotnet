using Microsoft.EntityFrameworkCore;
using TestApp.Entities;
using TestApp.Services;
using WatchDog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDBContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("cs")));
builder.Services.AddTransient<IUserRepo, UserRepo>();
builder.Services.AddTransient<ICacheService, CacheService>();
builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration.GetConnectionString("redis");
    opt.InstanceName = "stg-app";
});
builder.Services.AddWatchDogServices(opt =>
{
    opt.IsAutoClear = true; // clears the logs after a specific duration
    opt.ClearTimeSchedule = WatchDog.src.Enums.WatchDogAutoClearScheduleEnum.Monthly; // specify when it will automatically clear the logs
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseWatchDogExceptionLogger();
app.UseWatchDog(opt =>
{
    opt.WatchPageUsername = "admin";
    opt.WatchPagePassword = "Pass123#";
});
app.Run();
