using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Core.Application.Interfaces;

namespace Core.Application.Services;

public sealed class MachineService : IMachine
{
      public string Get()
      {
            var raw = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? GetWindowsFingerprint()
                : GetLinuxFingerprint();

            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
            return Convert.ToHexString(hash);
      }

      // ---------------- Windows ----------------

      private string GetWindowsFingerprint()
      {
            return string.Join("|",
                GetWmi("Win32_Processor", "ProcessorId"),
                GetWmi("Win32_BaseBoard", "SerialNumber"),
                GetWmi("Win32_PhysicalMedia", "SerialNumber"));
      }

#if WINDOWS
    private string GetWmi(string table, string field)
    {
        try
        {
            using var searcher = new ManagementObjectSearcher($"SELECT {field} FROM {table}");
            foreach (var obj in searcher.Get())
                return obj[field]?.ToString()?.Trim() ?? "UNKNOWN";
        }
        catch { }
        return "UNKNOWN";
    }
#else
      private string GetWmi(string _, string __) => "N/A";
#endif

      // ---------------- Linux ----------------

      private string GetLinuxFingerprint()
      {
            return string.Join("|",
                ReadFile("/etc/machine-id"),
                Run("lsblk", "-o UUID -n | head -n 1"),
                ReadFile("/proc/sys/kernel/hostname"));
      }

      // ---------------- Helpers ----------------

      private string ReadFile(string path)
      {
            try
            {
                  return File.Exists(path) ? File.ReadAllText(path).Trim() : "UNKNOWN";
            }
            catch { return "UNKNOWN"; }
      }

      private string Run(string cmd, string args)
      {
            try
            {
                  var psi = new ProcessStartInfo(cmd, args)
                  {
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                  };
                  using var p = Process.Start(psi);
                  return p?.StandardOutput.ReadLine()?.Trim() ?? "UNKNOWN";
            }
            catch { return "UNKNOWN"; }
      }
}