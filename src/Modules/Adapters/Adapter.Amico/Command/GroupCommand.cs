using System.Text.RegularExpressions;
using Adapter.Abstraction.Interfaces;
using Adapter.Amico.Enums;
using Adapter.Amico.Helper;
using Adapter.Amico.Interface;
using Adapter.Amico.Model.Objects;
using Adapter.Amico.Model.Request;
using Adapter.Amico.Model.Response;

namespace Adapter.Amico.Command;

public sealed class GroupCommand(IAmicoSetting setting, IHttpClient client) : BaseCommand(client, setting), IGroupCommand
{
      public async Task<CreateObjectResponse> CreateAccessRulesAsync(string ip, string session, string name, int type)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<CreateObjectRequest<AccessRule>, CreateObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.CREATE_OBJECT,
                  new CreateObjectRequest<AccessRule>(
                        ObjectConstant.AccessRule,
                        new List<AccessRule>
                        {
                              new AccessRule(
                        name,
                        type,
                        0
                  )
                        }
                  ),
                  queryParams: queryParams
            ) ?? new CreateObjectResponse();
      }

      public async Task<CreateObjectResponse> CreateAccessRuleTimeZoneAsync(string ip, string session, int timezone_id, int access_rule_id)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<CreateObjectRequest<AccessRuleTimeZone>, CreateObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.CREATE_OBJECT,
                  new CreateObjectRequest<AccessRuleTimeZone>(
                        ObjectConstant.AccessRuleTimeZone,
                        new List<AccessRuleTimeZone>
                  {
                        new AccessRuleTimeZone(
                        access_rule_id,
                        timezone_id
                  )
                  }
                  ),
                  queryParams: queryParams
            ) ?? new CreateObjectResponse();
      }

      public async Task<CreateObjectResponse> CreateGroupAccessRuleAsync(string ip, string session, int group_id, int access_rule_id)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<CreateObjectRequest<GroupAccessRule>, CreateObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.CREATE_OBJECT,
                  new CreateObjectRequest<GroupAccessRule>(
                        ObjectConstant.GroupAccessRule,
                        new List<GroupAccessRule>
                        {
                              new GroupAccessRule(
                                    group_id,
                                    access_rule_id
                              )
                        }
                  ),
                  queryParams: queryParams
            ) ?? new CreateObjectResponse();
      }

      public async Task<CreateObjectResponse> CreateGroupAsync(string ip, string session, string name)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<CreateObjectRequest<Model.Objects.Group>, CreateObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.CREATE_OBJECT,
                  new CreateObjectRequest<Model.Objects.Group>(
                        ObjectConstant.Group,
                        new List<Model.Objects.Group>
                        {
                              new Model.Objects.Group(
                                    name
                              )
                        }
                  ),
                  queryParams: queryParams
            ) ?? new CreateObjectResponse();
      }

      public async Task<DeleteObjectResponse> DeleteAccessRuleAsync(string ip, string session, int id)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<DeleteObjectRequest, DeleteObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.DELETE_OBJECT,
                  new DeleteObjectRequest(
                        ObjectConstant.AccessRule,
                        new
                        {
                              access_rules = new
                              {
                                    id = id
                              }
                        }
                        ),
                  queryParams: queryParams
            ) ?? new DeleteObjectResponse();
      }

      public async Task<DeleteObjectResponse> DeleteAccessRuleTimeZoneAsync(string ip, string session, int access_rule_id)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<DeleteObjectRequest, DeleteObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.DELETE_OBJECT,
                  new DeleteObjectRequest(
                        ObjectConstant.AccessRuleTimeZone,
                        new
                        {
                              access_rule_time_zones = new
                              {
                                    access_rule_id=access_rule_id
                              }
                        }
                        ),
                  queryParams: queryParams
            ) ?? new DeleteObjectResponse();
      }


      public async Task<DeleteObjectResponse> DeleteGroupAccessRuleAsync(string ip, string session, int group_id, int access_rule_id)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<DeleteObjectRequest, DeleteObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.DELETE_OBJECT,
                  new DeleteObjectRequest(
                        ObjectConstant.GroupAccessRule,
                        new
                        {
                              group_access_rules = new
                              {
                                    group_id=group_id,
                                    access_rule_id=access_rule_id
                              }
                        }
                        ),
                  queryParams: queryParams
            ) ?? new DeleteObjectResponse();
      }

      public async Task<DeleteObjectResponse> DeleteGroupAsync(string ip, string session, int id)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<DeleteObjectRequest, DeleteObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.DELETE_OBJECT,
                  new DeleteObjectRequest(
                        ObjectConstant.Group,
                        new
                        {
                              groups = new
                              {
                                    id = id
                              }
                        }
                        ),
                  queryParams: queryParams
            ) ?? new DeleteObjectResponse();
      }

      public async Task<UpdateObjectResponse> UpdateAccessRulesAsync(string ip, string session, int id, string name, int type)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<UpdateObjectRequest<AccessRule>, UpdateObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.UPDATE_OBJECT,
                  new UpdateObjectRequest<AccessRule>(
                        ObjectConstant.AccessRule,
                        new List<AccessRule>
                        {
                              new AccessRule(
                                    name,
                                    type,
                                    0
                              )
                        },
                        new
                        {
                              access_rules = new
                              {
                                    id = id
                              }
                        }
                        ),
                  queryParams: queryParams
            ) ?? new UpdateObjectResponse();
      }


      public async Task<UpdateObjectResponse> UpdateAccessRuleTimeZoneAsync(string ip, string session, int timezone_id, int access_rule_id)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            var old = await Client.SendAsync<LoadObjectRequest,LoadObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.LOAD_OBJECT,
                  new LoadObjectRequest(
                        ObjectConstant.AccessRuleTimeZone,
                        new List<string>
                        {
                               ObjectConstant.AccessRuleId,
                               ObjectConstant.TimeZoneId
                        }
                  ),
                  queryParams: queryParams
            ) ?? new LoadObjectResponse();

            if(old.access_rule_time_zones.Count() == 0)
                  return new UpdateObjectResponse();

            return await Client.SendAsync<UpdateObjectRequest<AccessRuleTimeZone>, UpdateObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.UPDATE_OBJECT,
                  new UpdateObjectRequest<AccessRuleTimeZone>(
                        ObjectConstant.AccessRuleTimeZone,
                        new List<AccessRuleTimeZone>
                        {
                              new AccessRuleTimeZone(
                                    access_rule_id,
                                    timezone_id
                              )
                        },
                        new
                        {
                              access_rule_time_zones = new
                              {
                                   old.access_rule_time_zones.ElementAt(0).acccess_rule_id,
                                   old.access_rule_time_zones.ElementAt(0).time_zone_id 
                              }
                        }
                        ),
                  queryParams: queryParams
            ) ?? new UpdateObjectResponse();
      }

      public async Task<UpdateObjectResponse> UpdateGroupAccessRuleAsync(string ip, string session, int group_id, int access_rule_id)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            var old = await Client.SendAsync<LoadObjectRequest,LoadObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.LOAD_OBJECT,
                  new LoadObjectRequest(
                        ObjectConstant.GroupAccessRule,
                        new List<string>
                        {
                               ObjectConstant.GroupId,
                               ObjectConstant.AccessRuleId
                        }
                  ),
                  queryParams: queryParams
            ) ?? new LoadObjectResponse();

            if(old.group_access_rules.Count() == 0)
                  return new UpdateObjectResponse();

            return await Client.SendAsync<UpdateObjectRequest<GroupAccessRule>, UpdateObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.UPDATE_OBJECT,
                  new UpdateObjectRequest<GroupAccessRule>(
                        ObjectConstant.GroupAccessRule,
                        new List<GroupAccessRule>
                        {
                              new GroupAccessRule(
                                    access_rule_id,
                                    group_id
                              )
                        },
                        new
                        {
                              group_access_rules = new
                              {
                                   old.group_access_rules.ElementAt(0).group_id,
                                   old.group_access_rules.ElementAt(0).access_rule_id 
                              }
                        }
                        ),
                  queryParams: queryParams
            ) ?? new UpdateObjectResponse();
      }

      public async Task<UpdateObjectResponse> UpdateGroupAsync(string ip, string session, int id, string name)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendAsync<UpdateObjectRequest<Model.Objects.Group>, UpdateObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.UPDATE_OBJECT,
                  new UpdateObjectRequest<Model.Objects.Group>(
                        ObjectConstant.Group,
                        new List<Model.Objects.Group>
                        {
                              new Model.Objects.Group(
                                    name
                              )
                        },
                        new
                        {
                              groups = new
                              {
                                    id = id
                              }
                        }
                        ),
                  queryParams: queryParams
            ) ?? new UpdateObjectResponse();
      }
}