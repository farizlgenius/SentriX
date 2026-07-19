using Adapter.Abstraction.Interfaces;
using Adapter.Amico.Enums;
using Adapter.Amico.Helper;
using Adapter.Amico.Interface;
using Adapter.Amico.Model.Objects;
using Adapter.Amico.Model.Request;
using Adapter.Amico.Model.Response;

namespace Adapter.Amico.Command;

public sealed class TimeCommand(IHttpClient client, IAmicoSetting setting) : BaseCommand(client, setting), ITimeCommand
{
      public async Task ClearTimeAsync(string ip, string session)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            var request = new DeleteObjectAllRequest(
                  ObjectConstant.TimeZone
            );

            await Client.SendAsync<DeleteObjectAllRequest, DeleteObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.DELETE_OBJECT,
                  request,
                  queryParams: queryParams
            );

            request = new DeleteObjectAllRequest(
                  ObjectConstant.TimeSpan
            );

            await Client.SendAsync<DeleteObjectAllRequest, DeleteObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.DELETE_OBJECT,
                  request,
                  queryParams: queryParams
            );
      }

      public async Task<CreateObjectResponse> CreateHolidayAsync(
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
            )
      {
            var request = new CreateObjectRequest<Model.Objects.Holiday>(
                  ObjectConstant.Holiday,
                  new List<Model.Objects.Holiday>
                  {
                        new Model.Objects.Holiday(
                              componentId,
                              name,
                              start,
                              end,
                              hol1,
                              hol2,
                              hol3,
                              repeats
                              )
                  }
            );

            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<CreateObjectRequest<Model.Objects.Holiday>, CreateObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.CREATE_OBJECT,
                  request,
                  queryParams: queryParams
            ) ?? new CreateObjectResponse();
      }

      public async Task<CreateObjectResponse> CreateTimeSpanAsync(
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
            int hol3)
      {
            var request = new CreateObjectRequest<Model.Objects.TimeSpan>(
                  ObjectConstant.TimeSpan,
                  new List<Model.Objects.TimeSpan>
                  {
                        new Model.Objects.TimeSpan(
                        componentId,
                        tz_id,
                        start,
                        end,
                        sun,
                        mon,
                        tue,
                        wed,
                        thu,
                        fri,
                        sat,
                        hol1,
                        hol2,
                        hol3
                  )
                  }
            );

            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<CreateObjectRequest<Model.Objects.TimeSpan>, CreateObjectResponse>(
                 HttpMethod.Post,
                 UriHelper.UriBuilder(ip, Setting.Secure),
                 Endpoint.CREATE_OBJECT,
                 request,
                 queryParams: queryParams
           ) ?? new CreateObjectResponse();
      }

      public async Task<CreateObjectResponse> CreateTimeZoneAsync(
            string ip,
            string session,
            string name,
            int componentId
            )
      {
            var request = new CreateObjectRequest<Model.Objects.TimeZone>(
                  ObjectConstant.TimeZone,
                  new List<Model.Objects.TimeZone>
                  {
                        new Model.Objects.TimeZone(
                              componentId,
                              name
                              )
                  }
            );

            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<CreateObjectRequest<Model.Objects.TimeZone>, CreateObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.CREATE_OBJECT,
                  request,
                  queryParams: queryParams
            ) ?? new CreateObjectResponse();
      }

      public async Task<DeleteObjectResponse> DeleteHolidayAsync(
            string ip,
            string session,
            int hol_id
      )
      {
            var request = new DeleteObjectRequest(
                  ObjectConstant.Holiday,
                  new
                  {
                        holidays = new
                        {
                              id = hol_id
                        }
                  }
            );

            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<DeleteObjectRequest, DeleteObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.DELETE_OBJECT,
                  request,
                  queryParams: queryParams
            ) ?? new DeleteObjectResponse();
      }

      public async Task<DeleteObjectResponse> DeleteTimeSpanAsync(string ip, string session, int componentId)
      {
            var request = new DeleteObjectRequest(
                 ObjectConstant.TimeSpan,
                 new
                 {
                       time_spans = new
                       {
                             id = componentId
                       }
                 }
           );

            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<DeleteObjectRequest, DeleteObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.DELETE_OBJECT,
                  request,
                  queryParams: queryParams
            ) ?? new DeleteObjectResponse();
      }

      public async Task<DeleteObjectResponse> DeleteTimeZoneAsunc(string ip, string session, int componentId)
      {
            var request = new DeleteObjectRequest(
                  ObjectConstant.TimeZone,
                  new
                  {
                        time_zones = new
                        {
                              id = componentId
                        }
                  }
            );

            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<DeleteObjectRequest, DeleteObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.DELETE_OBJECT,
                  request,
                  queryParams: queryParams
            ) ?? new DeleteObjectResponse();
      }

      public async Task<UpdateObjectResponse> UpdateHolidayAsync(string ip, string session, string name, int componentId, int start, int end, int hol1, int hol2, int hol3, int repeats)
      {
            var request = new UpdateObjectRequest<Model.Objects.Holiday>(
                   ObjectConstant.TimeZone,
                   new List<Model.Objects.Holiday>
                   {
                        new Model.Objects.Holiday(
                              componentId,
                              name,
                              start,
                              end,
                              hol1,
                              hol2,
                              hol3,
                              repeats
                              )
                   },
                   new
                   {
                         time_zones = new
                         {
                               id = componentId
                         }
                   }
             );

            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<UpdateObjectRequest<Model.Objects.Holiday>, UpdateObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.UPDATE_OBJECT,
                  request,
                  queryParams: queryParams
            ) ?? new UpdateObjectResponse();
      }

      public async Task<UpdateObjectResponse> UpdateTimeSpanAsync(string ip, string session, int tz_id, int componentId, int start, int end, int sun, int mon, int tue, int wed, int thu, int fri, int sat, int hol1, int hol2, int hol3)
      {
            var request = new UpdateObjectRequest<Model.Objects.TimeSpan>(
                 ObjectConstant.TimeSpan,
                 new List<Model.Objects.TimeSpan>
                 {
                        new Model.Objects.TimeSpan(
                        componentId,
                        tz_id,
                        start,
                        end,
                        sun,
                        mon,
                        tue,
                        wed,
                        thu,
                        fri,
                        sat,
                        hol1,
                        hol2,
                        hol3
                  )
                 },
                 new
                 {
                       time_spans = new
                       {
                             id = componentId
                       }
                 }
           );

            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<UpdateObjectRequest<Model.Objects.TimeSpan>, UpdateObjectResponse>(
                 HttpMethod.Post,
                 UriHelper.UriBuilder(ip, Setting.Secure),
                 Endpoint.CREATE_OBJECT,
                 request,
                 queryParams: queryParams
           ) ?? new UpdateObjectResponse();
      }

      public async Task<UpdateObjectResponse> UpdateTimeZoneAsync(string ip, string session, string name, int componentId)
      {
            var request = new UpdateObjectRequest<Model.Objects.TimeZone>
              (
                    ObjectConstant.TimeZone,
                   new List<Model.Objects.TimeZone>
                   {
                        new Model.Objects.TimeZone(
                                    componentId,
                                    name
                              )
                   },
                   new
                   {
                         time_zones = new
                         {
                               id = componentId
                         }
                   }
             );

            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<UpdateObjectRequest<Model.Objects.TimeZone>, UpdateObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.UPDATE_OBJECT,
                  request,
                  queryParams: queryParams
            ) ?? new UpdateObjectResponse();
      }
}