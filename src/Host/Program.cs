
using System.Net;
using System.Text;
using Adapter.Abstraction;
using Adapter.Aero;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Listener;
using Adapter.Amico;
using Adapter.Core;
using AeroAdapter.Application.Interfaces;
using Auth.Infrastructure;
using Cache.Infrastructure;
using Core.Infrastructure;
using Host.Helpers;
using Host.Logging;
using Host.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Notifier.Client;
using Notifier.Client.Hubs;
using Npgsql.Internal.Postgres;
using Scalar.AspNetCore;
using Serilog;
using Setting.Infrastructure;
using SharedKernel;
using SharedKernel.Domain;
using SharedKernel.Helpers;
using SharedKernel.Model;
using Storage;


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
        // builder.Services.AddAuth(builder.Configuration);
        builder.Services.AddCache(builder.Configuration);
        // builder.Services.AddAero(builder.Configuration);
        // builder.Services.AddAmico(builder.Configuration);
        builder.Services.AddShared(builder.Configuration);
        builder.Services.AddNotifyModule(builder.Configuration);
        builder.Services.AddStorage(builder.Configuration);
        builder.Services.AddSetting(builder.Configuration);
        builder.Services.AddCore(builder.Configuration);
        builder.Services.AddAdapter(builder.Configuration);


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

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ApiResponseFilter>();
        }).ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                 .Where(x => x.Value?.Errors.Count > 0)
                 .ToDictionary(
                     x => x.Key,
                     x => x.Value!.Errors
                         .Select(e => e.ErrorMessage)
                         .ToArray()
                 );

                return new BadRequestObjectResult(
                    new ValidateBaseResponse<object>(
                        DateTime.UtcNow,
                        HttpStatusCode.BadRequest,
                        false,
                        "Validation failed.",
                        Errors: errors
                    )
                );
            };
        });

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

        app.UseMiddleware<CorrelationMiddleware>();

        app.UseSerilogRequestLogging(options =>
{
    options.GetLevel = (httpContext, elapsed, ex) =>
    {
        if (ex != null)
        {
            return Serilog.Events.LogEventLevel.Debug;
        }

        return httpContext.Response.StatusCode >= 500
            ? Serilog.Events.LogEventLevel.Error
            : Serilog.Events.LogEventLevel.Information;
    };
});

        app.UseMiddleware<GlobalException>();

        app.UseCors("CorsPolicy");

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.MapHub<NotifierHub>("/notiHubs");

        app.MapOpenApi();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // app.UseDeveloperExceptionPage();


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
        //         var readDriver = app.Services.GetRequiredService<ReplyMessageListener>();
        //         // var writer = app.Services.GetRequiredService<ICommandWriter>();
        //         readDriver.TurnOnDebug();


        //         using (var scope = app.Services.CreateScope())
        //         {
        //             var w = scope.ServiceProvider.GetRequiredService<IDriverCommand>();
        //             var w2 = scope.ServiceProvider.GetRequiredService<IScpCommand>();

        //             // Now you can safely use sys here
        //             if (!w.SystemLevelSpecification())
        //             {
        //                 Console.WriteLine("Initial driver failed. Shutting down app...");
        //                 app.Lifetime.StopApplication(); // graceful shutdown
        //             }

        //             // Now you can safely use sys here
        //             if (!w2.CreateChannel())
        //             {
        //                 Console.WriteLine("Initial driver failed. Shutting down app...");
        //                 app.Lifetime.StopApplication(); // graceful shutdown
        //             }
        //         }

        //         app.Lifetime.ApplicationStarted.Register(() =>
        // {
        //     _ = Task.Run(() => readDriver.GetTransactionUntilShutDownAsync());
        // });


        //         app.Lifetime.ApplicationStopping.Register(async () =>
        //         {

        //             readDriver.SetShutDownFlag();
        //             readDriver.TurnOffDebug();

        //         });




        app.Run();
    }
}
