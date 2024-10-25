using ClienSVH.XMLParser;
using ClientSWH.Application.Interfaces.Auth;
using ClientSWH.Application.Interfaces;
using ClientSWH.Application.Services;
using ClientSWH.Core.Abstraction.Repositories;
using ClientSWH.Core.Abstraction.Services;
using ClientSWH.DataAccess.Mapping;
using ClientSWH.DataAccess.Repositories;
using ClientSWH.DataAccess;
using ClientSWH.DocsRecordCore.Abstraction;
using ClientSWH.DocsRecordDataAccess;
using ClientSWH.Extensions;
using ClientSWH.Infrastructure;
using ClientSWH.SendReceivServer.Consumer;
using ClientSWH.SendReceivServer.Producer;
using ClientSWH.SendReceivServer.Settings;
using ClientSWH.SendReceivServer;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;

services.AddApiAuthentication(configuration);
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
//postgresql db
Console.WriteLine(configuration.GetConnectionString("DefaultConnection"));
services.AddDbContext<ClientSWHDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
});
services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

//mongodb
services.Configure<Settings>(configuration.GetSection("MongoConnection"));


services.AddTransient<IMongoClient>(_ =>
{
    var connectionString = configuration.GetSection("MongoConnection:ConnectionString")?.Value;

    return new MongoClient(connectionString);
});
services.Configure<JwtOptions>(configuration.GetSection("JWT"));
services.AddTransient<IUsersService, UsersService>();
services.AddTransient<IPackagesServices, PackagesServices>();
services.AddTransient<IDocumentsServices, DocumentsServices>();
services.AddTransient<IHistoryPkgRepository, HistoryPkgRepository>();

services.AddTransient<IUsersRepository, UsersRepository>();
services.AddTransient<IPackagesRepository, PackagesRepository>();
services.AddTransient<IDocumentsRepository, DocumentsRepository>();
services.AddTransient<IDocRecordRepository, DocRecordRepository>();
services.AddTransient<IJwtProvider, JwtProvider>();
services.AddTransient<IPasswordHasher, PasswordHasher>();

services.AddTransient<ILoadFromFile, LoadFromFile>();
services.AddTransient<IRabbitMQBase, RabbitMQBase>();
services.AddTransient<IMessagePublisher, RabbitMQProducer>();
services.AddTransient<IRabbitMQConsumer, RabbitMQConsumer>();
services.AddTransient<ISendToServer, SendToServer>();
services.AddTransient<IReceivFromServer, ReceivFromServer>();

services.AddAutoMapper(typeof(MapperProfile));
services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
