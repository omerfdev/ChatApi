using Application.Hubs;
using Application.Services.FileServices.Implementations;
using Application.Services.FileServices.Interfaces;
using Application.Services.PrivateMessageServices.Implementations;
using Application.Services.PrivateMessageServices.Interfaces;
using Application.Services.UserService.Implementations;
using Application.Services.UserService.Interfaces;
using BusinessLayer.Services.UserService.Implementations;
using ChatApi;
using Domain.Repositories;
using Infrastructure.DbContexts;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

var builder = WebApplication.CreateBuilder(args);


//UserServices
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
builder.Services.AddScoped<IUserProfileImageService, UserProfileImageService>();
builder.Services.AddScoped<IUserRetrievalService, UserRetrievalService>();
//FileServices
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IFileService, FileService>();
//MessagesServices
builder.Services.AddScoped<IPrivateMessageService, PrivateMessageService>();
//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPrivateMessageRepository, PrivateMessageRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Chat API",
        Description = "An ASP.NET Core Web API for managing Chat website",
        Contact = new OpenApiContact
        {
            Name = "omerfdev",
            Url = new Uri("https://www.linkedin.com/in/omeralmali")
        },
    });
});


//builder.Services.AddDbContext<ChatContext>(options =>
//                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//var configuration = new ConfigurationBuilder()
//    .AddEnvironmentVariables("MongoDBSettings:"); // Assuming your environment variables are prefixed with "MongoDBSettings:"
//    .Build();


//builder.Services.AddSingleton<IMongoDatabase>(_ => new Connection(configuration).GetDatabase());
//builder.Services.AddScoped<IClientSessionHandle>(_ => new Connection(configuration).StartSession());
// Add MongoDB connection
//connection to mongo db ATLAS
// Add MongoDB connection for User database

builder.Services.Configure<UserDatabaseSettings>(builder.Configuration.GetSection(nameof(UserDatabaseSettings)));
builder.Services.AddSingleton<IUserDatabaseSettings>(us => us.GetRequiredService<IOptions<UserDatabaseSettings>>().Value);
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(builder.Configuration.GetValue<string>("UserDatabaseSettings:ConnectionString")));
builder.Services.AddScoped<IMongoDatabase>(_ =>
{
    var client = _.GetService<IMongoClient>();
    return client.GetDatabase(builder.Configuration.GetValue<string>("UserDatabaseSettings:DatabaseName"));
});

// Add MongoDB connection for PrivateMessage database
builder.Services.Configure<PrivateMessageDatabaseSettings>(builder.Configuration.GetSection(nameof(PrivateMessageDatabaseSettings)));
builder.Services.AddSingleton<IPrivateMessageDatabaseSettings>(pm => pm.GetRequiredService<IOptions<PrivateMessageDatabaseSettings>>().Value);
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(builder.Configuration.GetValue<string>("PrivateMessageDatabaseSettings:ConnectionString")));
builder.Services.AddScoped<IMongoDatabase>(_ =>
{
    var client = _.GetService<IMongoClient>();
    return client.GetDatabase(builder.Configuration.GetValue<string>("PrivateMessageDatabaseSettings:DatabaseName"));
});

// Add MongoDB connection for Image database
builder.Services.Configure<ImageDatabaseSettings>(builder.Configuration.GetSection(nameof(ImageDatabaseSettings)));
builder.Services.AddSingleton<IImageDatabaseSettings>(pm => pm.GetRequiredService<IOptions<ImageDatabaseSettings>>().Value);
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(builder.Configuration.GetValue<string>("ImageDatabaseSettings:ConnectionString")));
builder.Services.AddScoped<IMongoDatabase>(_ =>
{
    var client = _.GetService<IMongoClient>();
    return client.GetDatabase(builder.Configuration.GetValue<string>("ImageDatabaseSettings:DatabaseName"));
});

// Add MongoDB connection for ConnectionInfo database
builder.Services.Configure<ConnectionInfoDatabaseSettings>(builder.Configuration.GetSection(nameof(ConnectionInfoDatabaseSettings)));
builder.Services.AddSingleton<IConnectionInfoDatabaseSettings>(pm => pm.GetRequiredService<IOptions<ConnectionInfoDatabaseSettings>>().Value);
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(builder.Configuration.GetValue<string>("ConnectionInfoDatabaseSettings:ConnectionString")));
builder.Services.AddScoped<IMongoDatabase>(_ =>
{
    var client = _.GetService<IMongoClient>();
    return client.GetDatabase(builder.Configuration.GetValue<string>("ConnectionInfoDatabaseSettings:DatabaseName"));
});
builder.Services.AddScoped<IClientSessionHandle>(_ =>
{
    var client = _.GetService<IMongoClient>();
    return client.StartSession();
});
builder.Services.AddScoped<ChatContext>();
builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:TokenKey").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddCors(c =>
{
    c.AddDefaultPolicy(options =>
    options.WithOrigins("http://localhost:3000")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
});
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/chat");
app.Run();
