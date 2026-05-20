
using System.Text;
using Adapter.Abstraction;
using Adapter.Aero;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Listener;
using Adapter.Amico;
using AeroAdapter.Application.Interfaces;
using Auth.Infrastructure;
using Cache.Infrastructure;
using Device.Infrastructure;
using Events.Infrastructure;
using Host.Helpers;
using Host.Logging;
using Host.Middlewares;
using Location.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Notifier.Client;
using Notifier.Client.Hubs;
using Operator.Infrastructure;
using Output.Infrastructure;
using Role.Infrastructure;
using Scalar.AspNetCore;
using Serilog;
using SharedKernel;
using Time.Infrastructure;


namespace Host;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ==========================
        // Add SignalR
        // ==========================
        builder.Services.AddSignalR();

        // // ==========================
        // // Read config from appsetting.json
        // // ==========================
        // ReadConfigHelper.ReadConfig(builder);

        // // ==========================
        // // Cache service setting
        // // ==========================
        // CacheSettingHelper.RedisConfiguration(builder);

        // ==========================
        // Setting Routing option
        // ==========================
        builder.Services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true; // optional
        });

        // ==========================
        // Dependency Injection
        // ==========================

        builder.Services.AddHost(builder.Configuration);
        builder.Services.AddAuth(builder.Configuration);
        builder.Services.AddCache(builder.Configuration);
        builder.Services.AddOperator(builder.Configuration);
        builder.Services.AddLocation(builder.Configuration);
        builder.Services.AddAero(builder.Configuration);
        builder.Services.AddAmico(builder.Configuration);
        builder.Services.AddRole(builder.Configuration);
        builder.Services.AddShared(builder.Configuration);
        builder.Services.AddDevice(builder.Configuration);
        builder.Services.AddNotifyModule(builder.Configuration);
        builder.Services.AddEvents(builder.Configuration);
        builder.Services.AddAdapter(builder.Configuration);
        builder.Services.AddOutput(builder.Configuration);
        builder.Services.AddTime(builder.Configuration);


        // Replace default logging with Serilog
            // builder.Host.UseSerilog();
            builder.Host.ConfigureEnterpriseLogging();

        // ==========================
        // MediatR
        // ==========================
        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(
        AppDomain.CurrentDomain.GetAssemblies()));


        // // ==========================
        // // Adding App Dependency Injection
        // // ==========================
        // DISettingHelper.DISetting(builder);

        // ==========================
        // Adding App Dependency Injection
        // ==========================
        CorsHelper.Cors(builder);

        // // ==========================
        // // Add Authorization
        // // ==========================
        // builder.Services.AddAuthorization();

        // ==========================
        // Add Controllers
        // ==========================
        builder.Services.AddControllers();

        builder.Services.AddAuthorization();

        var jwtSection = builder.Configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSection["Secret"]!);

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        var path = context.HttpContext.Request.Path;

                        // ⭐ 1) Normal API requests → read header
                        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                        {
                            context.Token = authHeader.Substring("Bearer ".Length);
                        }

                        // ⭐ 2) SignalR requests → read query string
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/notiHubs"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },

                        OnChallenge = async context =>
                        {
                              context.HandleResponse(); // stop default 401 page

                              var message = "Jwt Token invalid or missing";

                              if (context.AuthenticateFailure != null)
                                    message = context.AuthenticateFailure.Message;

                              await AuthResponseHelper.Write401(context.Response, message);
                        },

                        OnForbidden = async context =>
                        {
                              await AuthResponseHelper.Write403(context.Response);
                        },

                        OnAuthenticationFailed = context =>
                        {
                              Console.WriteLine("❌ JWT FAILED: " + context.Exception.Message);
                              return Task.CompletedTask;
                        },

                        OnTokenValidated = context =>
                        {
                              Console.WriteLine("✅ TOKEN VALIDATED");
                              return Task.CompletedTask;
                        },

                    
                };
            });


        // ==========================
        // Scalar Setting
        // ==========================
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddOpenApi();




        var app = builder.Build();

        app.UseMiddleware<GlobalException>();
        app.UseMiddleware<CorrelationMiddleware>();

        app.MapOpenApi();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {


            app.MapScalarApiReference(options =>
            {
                options.Title = "Scalar Demo API";
                options.Theme = ScalarTheme.Default;
                options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);

                // ⭐ THIS LINE FIXES EVERYTHING
                options.OpenApiRoutePattern = "/openapi/{documentName}.json";
            });

        }

        /// ==========================
        /// Driver
        /// ==========================
        var readDriver = app.Services.GetRequiredService<ReplyMessageListener>();
        // var writer = app.Services.GetRequiredService<ICommandWriter>();
        readDriver.TurnOnDebug();


        using (var scope = app.Services.CreateScope())
        {
            var w = scope.ServiceProvider.GetRequiredService<IDriverCommand>();
            var w2 = scope.ServiceProvider.GetRequiredService<IScpCommand>();

            // Now you can safely use sys here
            if (!w.SystemLevelSpecification())
            {
                Console.WriteLine("Initial driver failed. Shutting down app...");
                app.Lifetime.StopApplication(); // graceful shutdown
            }

            // Now you can safely use sys here
            if (!w2.CreateChannel())
            {
                Console.WriteLine("Initial driver failed. Shutting down app...");
                app.Lifetime.StopApplication(); // graceful shutdown
            }
        }

        app.Lifetime.ApplicationStarted.Register(() =>
{
_ = Task.Run(() => readDriver.GetTransactionUntilShutDownAsync());
});


        app.Lifetime.ApplicationStopping.Register(async () =>
        {

            readDriver.SetShutDownFlag();
            readDriver.TurnOffDebug();

        });

        app.UseSerilogRequestLogging();


        app.UseCors("CorsPolicy");

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.MapHub<NotifierHub>("/notiHubs");

        app.Run();
    }
}
