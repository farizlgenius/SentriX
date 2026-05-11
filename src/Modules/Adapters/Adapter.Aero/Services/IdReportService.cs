using System;
using Adapter.Aero.Entities;
using Adapter.Aero.Interfaces;

namespace Adapter.Aero.Services;

public sealed class IdReportService : IIdReportService
{
      public List<IdReport> IdReportInMemory { get; set; } = new List<IdReport>();

      public List<IdReport> GetIdReport()
      {

            return IdReportInMemory;
      }

      public void AddIdReport(IdReport reports)
      {
            if (IdReportInMemory.Any(x => x.Mac.Equals(reports.Mac)))
                  return;

            IdReportInMemory.Add(reports);


      }

      public void RemoveIdReport(string mac)
      {
            var data = IdReportInMemory.Where(x => x.Mac.Equals(mac)).FirstOrDefault();
            if (data == null)
                  return;

            IdReportInMemory.Remove(data);
      }

      public void UpdateIp(string mac, string ip)
      {
            var idreport = IdReportInMemory.FirstOrDefault(x => x.Mac.Equals(mac));
            if (idreport == null)
                  return;

            idreport.UpdateIp(ip);
      }

      public void UpdatePort(string mac, int port)
      {
            var idreport = IdReportInMemory.FirstOrDefault(x => x.Mac.Equals(mac));
            if (idreport == null)
                  return;

            idreport.UpdatePort(port);
      }

      public bool IsMacExist(string mac)
      {
            return IdReportInMemory.Any(x => x.Mac.Equals(mac));
      }
}
