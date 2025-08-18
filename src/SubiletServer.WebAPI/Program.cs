using SubiletServer.Infrastructure;
using SubiletServer.Application;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.OData;
using Scalar.AspNetCore;
using SubiletServer.WebAPI;
using SubiletServer.WebAPI.Modules;
using SubiletServer.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Serilog.Events;
using SubiletServer.Infrastructure.Context;
using SubiletServer.WebAPI.Middleware;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File("logs/subilet-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
    .CreateLogger();

try
{
    Log.Information("🚀 SubiletServer başlatılıyor...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();

    builder.Services.AddApplication();
    builder.Services.AddOpenApi();
    builder.Services.AddInfrastructure(builder.Configuration);

    // JWT Authentication
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "your-super-secret-key-with-at-least-32-characters"))
            };
        });

    // Authorization
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminOnly", policy =>
            policy.RequireRole("Admin"));
    });

    builder.Services.AddRateLimiter(cfr =>
    {
        cfr.AddFixedWindowLimiter("fixed", opt =>
        {
            opt.PermitLimit = 100;
            opt.QueueLimit = 100;
            opt.Window = TimeSpan.FromSeconds(1);
            opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        });
    });

    builder.Services
        .AddControllers()
        .AddOData(opt =>
            opt.Select().Filter().Count().Expand().OrderBy().SetMaxTop(null)
        );

    builder.Services.AddCors();
    builder.Services.AddSwaggerGen();

    builder.Services.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>("Database");

    var app = builder.Build();

    // app.UseHttpsRedirection(); // Geçici olarak kaldırıldı
    app.UseJwtMiddleware();

    app.UseCors(x => x
        .AllowAnyHeader()
        .WithOrigins(
            "http://localhost:4200", 
            "https://localhost:4200",
            "http://localhost:4201",
            "https://localhost:4201",
            "http://localhost:3000",
            "https://localhost:3000"
        )
        .AllowAnyMethod()
        .AllowCredentials()
        .SetPreflightMaxAge(TimeSpan.FromMinutes(10))
    );

    app.UseAuthentication();
    app.UseAuthorization();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapHealthChecks("/health");
    app.MapControllers().RequireRateLimiting("fixed");
    app.MapAuth();

    // 👇 Admin kullanıcı oluşturuluyor (loglu)
    try
    {
        Log.Information("👤 Admin kullanıcısı oluşturuluyor...");
        await app.CreateAdminUser();
        Log.Information("✅ Admin kullanıcısı oluşturuldu.");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "❌ Admin kullanıcısı oluşturulurken hata oluştu.");
    }

    Log.Information("🌐 Uygulama çalıştırılıyor...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "❌ SubiletServer başlatılamadı.");
}
finally
{
    Log.CloseAndFlush();
}
