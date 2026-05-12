using System;
using Adapter.Aero.Entities;

namespace Adapter.Aero.Interfaces;

public interface IIdReportService
{
      List<IdReport> IdReportInMemory { get; }
      List<IdReport> GetIdReport();

      void AddIdReport(IdReport reports);

      void RemoveIdReport(string mac);
      void RemoveIdReportById(int id);
      void UpdateIp(string mac, string ip);
      void UpdatePort(string mac, int port);
      bool IsMacExist(string mac);
}
