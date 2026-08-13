using SharedKernel.Helpers;
using Storage.Contract.Interfaces;

namespace Host;

public sealed class StartupTask : IHostedService
{
      private readonly IServiceScopeFactory _scopeFactory;
      private readonly ILogger<StartupTask> _logger;

      public StartupTask(IServiceScopeFactory scopeFactory, ILogger<StartupTask> logger)
      {
            _scopeFactory = scopeFactory;
            _logger = logger;
      }

      public async Task StartAsync(CancellationToken cancellationToken)
      {
            await RunOnStartupAsync(cancellationToken);
      }

      public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

      private async Task RunOnStartupAsync(CancellationToken cancellationToken)
      {
            _logger.LogInformation("🚀 StartupTask started");

            try
            {

                  // ⭐ STEP 3 — Your existing RSA key generation
                  await EnsureLicenseKeysAsync();

                  _logger.LogInformation("✅ StartupTask completed");
            }
            catch (Exception ex)
            {
                  _logger.LogCritical(ex, "❌ StartupTask failed — application will stop");
                  throw; // crash app intentionally if startup fails
            }
      }

      private async Task EnsureLicenseKeysAsync()
      {
            using var scope = _scopeFactory.CreateScope();
            var services = scope.ServiceProvider;

            var storage = services.GetRequiredService<IStorage>();

            var isKeyGenerated = await storage.CheckKeyAsync();

            if (isKeyGenerated)
                  return;

            var signer = EncryptHelper.CreateSigner();

      
            await storage.SaveKeyAsync(signer.ExportSubjectPublicKeyInfo(),signer.ExportPkcs8PrivateKey());

      }
}
