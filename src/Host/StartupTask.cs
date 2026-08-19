using System.Security.Cryptography;
using Host.Helpers;
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
                  await CreateKey();

                  _logger.LogInformation("✅ StartupTask completed");
            }
            catch (Exception ex)
            {
                  _logger.LogCritical(ex, "❌ StartupTask failed — application will stop");
                  throw; // crash app intentionally if startup fails
            }
      }

      private async Task CreateKey()
      {
            using var scope = _scopeFactory.CreateScope();
            var services = scope.ServiceProvider;

            var storage = services.GetRequiredService<IStorage>();

            if (!await storage.CheckKeyAsync())
            {
                  var key = KeyGenerator.GenerateEcdsa();

                  Console.WriteLine("Private Key:");
                  Console.WriteLine(Convert.ToBase64String(key.PrivateKey));
                  Console.WriteLine();
                  Console.WriteLine("Public Key:");
                  Console.WriteLine(Convert.ToBase64String(key.PublicKey));

                  await storage.SaveKeyAsync(key.PublicKey, key.PrivateKey);
            }

            if (!await storage.CheckEncKeyAsync())
            {
                  var encKey = EncryptionKeyGenerator.GenerateEcdh();

                  Console.WriteLine("Enc Private Key:");
                  Console.WriteLine(Convert.ToBase64String(encKey.PrivateKey));
                  Console.WriteLine();
                  Console.WriteLine("Enc Public Key:");
                  Console.WriteLine(Convert.ToBase64String(encKey.PublicKey));

                  await storage.SaveEncKeyAsync(encKey.PublicKey, encKey.PrivateKey);
            }

      }
}
