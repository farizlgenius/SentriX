using Adapter.Amico.Interfaces;
using Adapter.Amico.Model.Response;

namespace Adapter.Amico.Interface;

public interface ITimeCommand : IBaseCommand
{
      Task<CreateObjectResponse> CreateHolidayAsync(
            string ip,
            string session,
            string name,
            int start,
            int end,
            int hol1,
            int hol2,
            int hol3,
            int repeats
      );

      Task<UpdateObjectResponse> UpdateHolidayAsync(
            string ip,
            string session,
            string name,
            int componentId,
            int start,
            int end,
            int hol1,
            int hol2,
            int hol3,
            int repeats
      );

      Task<DeleteObjectResponse> DeleteHolidayAsync(
            string ip,
            string session,
            int hol_id
      );

      Task<CreateObjectResponse> CreateTimeZoneAsync(
            string ip,
            string session,
            string name
      );

      Task<CreateObjectResponse> CreateTimeSpanAsync(
            string ip,
            string session,
            int tz_id,
            int componentId,
            int start,
            int end,
            int sun,
            int mon,
            int tue,
            int wed,
            int thu,
            int fri,
            int sat,
            int hol1,
            int hol2,
            int hol3
      );

      Task<UpdateObjectResponse> UpdateTimeZoneAsync(
            string ip,
            string session,
            string name,
            int componentId
      );

      Task<DeleteObjectResponse> DeleteTimeZoneAsunc(
            string ip,
            string session,
            int componentId
      );

      Task<UpdateObjectResponse> UpdateTimeSpanAsync(
            string ip,
            string session,
            int tz_id,
            int componentId,
            int start,
            int end,
            int sun,
            int mon,
            int tue,
            int wed,
            int thu,
            int fri,
            int sat,
            int hol1,
            int hol2,
            int hol3
      );

      Task<DeleteObjectResponse> DeleteTimeSpanAsync(
            string ip,
            string session,
            int componentId
      );

      Task ClearTimeAsync(
            string ip,
            string session
      );


}