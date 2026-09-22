using System.Collections.Concurrent;
using Core.Application.Interfaces;
using Core.Contract.DTOs.Device;
using Core.Contract.Interfaces;
using Core.Domain.Entities;

namespace Core.Application.Services;

public sealed class TempDeviceService : ITempDevice
{
      private readonly ConcurrentDictionary<string, TempDeviceDto> _devices =
        new(StringComparer.OrdinalIgnoreCase);
      public int Count => _devices.Count();


      public bool Contains(string macAddress)
      {
            ArgumentException.ThrowIfNullOrWhiteSpace(macAddress);

            return _devices.ContainsKey(macAddress);
      }

      public IReadOnlyCollection<TempDeviceDto> GetAll()
      {
            return _devices.Values.ToArray();
      }

      public bool TryAdd(TempDeviceDto device)
      {
            ArgumentNullException.ThrowIfNull(device);

            return _devices.TryAdd(NormalizeMac(device.Mac), device);
      }

      public bool TryGet(string macAddress, out TempDeviceDto? device)
      {
            ArgumentException.ThrowIfNullOrWhiteSpace(macAddress);

            return _devices.TryGetValue(macAddress, out device);
      }

      public bool TryRemove(string macAddress)
      {
            ArgumentException.ThrowIfNullOrWhiteSpace(macAddress);

            return _devices.TryRemove(
                NormalizeMac(macAddress),
                out _);
      }


      private static string NormalizeMac(string macAddress)
      {
            return macAddress
                .Trim()
                .ToUpperInvariant();
      }

      public void TryUpdateIp(int Id,string Ip)
      {
            var arr = _devices.Values.ToArray();

            var device = arr.Where(x => x.Id == Id).FirstOrDefault();

            ArgumentNullException.ThrowIfNull(device);

            var mac = NormalizeMac(device.Mac);

            _devices.AddOrUpdate(
                mac,
                device,
                (_, existing) =>
                {
                      existing.Ip = Ip;

                      return existing;
                });
      }

      public void TryUpdatePort(int Id,int Port)
      {
            var arr = _devices.Values.ToArray();

            var device = arr.Where(x => x.Id == Id).FirstOrDefault();
            
            ArgumentNullException.ThrowIfNull(device);

            var mac = NormalizeMac(device.Mac);

            _devices.AddOrUpdate(
                mac,
                device,
                (_, existing) =>
                {
                      existing.Port = Port;

                      return existing;
                });
      }

      public IEnumerable<int> TryGetUnavailableId()
      {
            var arr = _devices.Values.ToArray();

            return arr.Select(x => x.Id).ToArray();
      }
}