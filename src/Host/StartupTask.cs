using System.Security.Cryptography;
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

            var isKeyGenerated = await storage.CheckKeyAsync();

            if (isKeyGenerated)
                  return;

            using var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);

            var privateKey = ecdsa.ExportPkcs8PrivateKey();
            var publicKey = ecdsa.ExportSubjectPublicKeyInfo();

            Console.WriteLine("Private Key:");

            Console.WriteLine(

                Convert.ToBase64String(privateKey));

            Console.WriteLine();

            Console.WriteLine("Public Key:");

            Console.WriteLine(Convert.ToBase64String(publicKey));

            await storage.SaveKeyAsync(publicKey, privateKey);

      }
}
