using Adapter.Amico.Interfaces;
using Adapter.Amico.Model.Response;

namespace Adapter.Amico.Interface;

public interface IGroupCommand : IBaseCommand
{
      Task<CreateObjectResponse> CreateGroupAsync(string ip,string session,int id,string name);
      Task<UpdateObjectResponse> UpdateGroupAsync(string ip,string session,int id,string name);
      Task<DeleteObjectResponse> DeleteGroupAsync(string ip,string session,int id);
      Task<CreateObjectResponse> CreateAccessRulesAsync(string ip,string session,int id,string name,int type);
      Task<UpdateObjectResponse> UpdateAccessRulesAsync(string ip,string session,int id,string name,int type);
      Task<DeleteObjectResponse> DeleteAccessRuleAsync(string ip,string session,int id);
      Task<CreateObjectResponse> CreateGroupAccessRuleAsync(string ip,string session,int group_id,int access_rule_id);
      Task<UpdateObjectResponse> UpdateGroupAccessRuleAsync(string ip,string session,int group_id,int access_rule_id);
      Task<DeleteObjectResponse> DeleteGroupAccessRuleAsync(string ip,string session,int group_id,int access_rule_id);
      Task<CreateObjectResponse> CreateAccessRuleTimeZoneAsync(string ip,string session,int timezone_id,int access_rule_id);
      Task<UpdateObjectResponse> UpdateAccessRuleTimeZoneAsync(string ip,string session,int timezone_id,int access_rule_id);
      Task<DeleteObjectResponse> DeleteAccessRuleTimeZoneAsync(string ip,string session,int access_rule_id);
}