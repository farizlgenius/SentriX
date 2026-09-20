using Adapter.Aero.Listener;
using Aero.Application.Interfaces;
using Core.Application.Interfaces;
using Host.Helpers;
using Setting.Contract.Interfaces;
using SharedKernel.Helpers;
using Storage.Contract.Interfaces;

namespace Host;

public sealed class StartupTask : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<StartupTask> _logger;

    public StartupTask(
        IServiceScopeFactory scopeFactory,
        ILogger<StartupTask> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    // ============================================================
    // APPLICATION STARTUP
    // ============================================================
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await RunOnStartupAsync(cancellationToken);
    }

    // ============================================================
    // APPLICATION SHUTDOWN
    // ============================================================
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🛑 ShutdownTask started");

        try
        {
            await RunOnShutdownAsync(cancellationToken);

            _logger.LogInformation("✅ ShutdownTask completed");
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("⚠️ ShutdownTask was cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ ShutdownTask failed");
        }
    }

    // ============================================================
    // STARTUP
    // ============================================================
    private async Task RunOnStartupAsync(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("🚀 StartupTask started");

        try
        {
            await CreateKey();

            await AeroDriverStartupAsync(cancellationToken);

            _logger.LogInformation("✅ StartupTask completed");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(
                ex,
                "❌ StartupTask failed — application will stop");

            throw;
        }
    }

    // ============================================================
    // SHUTDOWN
    // ============================================================
    private async Task RunOnShutdownAsync(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("🔻 Running application shutdown tasks...");

        await AeroDriverShutdownAsync(cancellationToken);

        // Add other cleanup tasks here
        // await SaveSomethingAsync(cancellationToken);
        // await CloseConnectionAsync(cancellationToken);
        // await CleanupAsync(cancellationToken);
    }

    // ============================================================
    // AERO DRIVER STARTUP
    // ============================================================
    private async Task AeroDriverStartupAsync(
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("🚀 Starting Aero Driver...");

        //Start driver here
        using var scope = _scopeFactory.CreateScope();
        var setting = scope.ServiceProvider.GetRequiredService<ISetting>();
        var aeroRead = scope.ServiceProvider.GetRequiredService<ReplyMessageListener>();
        var aero = scope.ServiceProvider.GetRequiredService<Aero.Application.Interfaces.IDeviceRepository>();

        var aeroSetting = await setting.GetAeroDriverSettingAsync();

        aeroRead.TurnOnDebug();

        aero.SystemLevelSpecification(
            (short)aeroSetting.nPorts,
            (short) aeroSetting.nScps
        );

        aero.CreateChannel(
             1,
              (short)aeroSetting.cType,
              (short)aeroSetting.cPort
        );

        await Task.CompletedTask;
    }

    // ============================================================
    // AERO DRIVER SHUTDOWN
    // ============================================================
    private async Task AeroDriverShutdownAsync(
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("🛑 Shutting down Aero Driver...");
        Console.WriteLine("🛑 Shutting down Aero Driver...");

        // // Stop / dispose driver here
        using var scope = _scopeFactory.CreateScope();
        var aeroRead = scope.ServiceProvider.GetRequiredService<ReplyMessageListener>();

        aeroRead.SetShutDownFlag();
        aeroRead.TurnOffDebug();

        await Task.CompletedTask;
    }

    // ============================================================
    // KEY CREATION
    // ============================================================
    private async Task CreateKey()
    {
        using var scope = _scopeFactory.CreateScope();

        var services = scope.ServiceProvider;

        var storage = services.GetRequiredService<IStorage>();

        // ECDSA key
        if (!await storage.CheckKeyAsync())
        {
            var key = KeyGenerator.GenerateEcdsa();

            Console.WriteLine("Private Key:");
            Console.WriteLine(
                Convert.ToBase64String(key.PrivateKey));

            Console.WriteLine();

            Console.WriteLine("Public Key:");
            Console.WriteLine(
                Convert.ToBase64String(key.PublicKey));

            await storage.SaveKeyAsync(
                key.PublicKey,
                key.PrivateKey);
        }

        // ECDH encryption key
        if (!await storage.CheckEncKeyAsync())
        {
            var encKey = EncryptionKeyGenerator.GenerateEcdh();

            Console.WriteLine("Enc Private Key:");
            Console.WriteLine(
                Convert.ToBase64String(encKey.PrivateKey));

            Console.WriteLine();

            Console.WriteLine("Enc Public Key:");
            Console.WriteLine(
                Convert.ToBase64String(encKey.PublicKey));

            await storage.SaveEncKeyAsync(
                encKey.PublicKey,
                encKey.PrivateKey);
        }
    }
}

