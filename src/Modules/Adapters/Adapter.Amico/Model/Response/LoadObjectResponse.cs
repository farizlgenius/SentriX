using System.Text.Json.Serialization;

namespace Adapter.Amico.Model.Response;

public sealed class LoadObjectResponse
{
      public List<Objects.TimeZone> time_zones { get; set; } = [];

      public List<Objects.Holiday> holidays { get; set; } = [];

      public List<Objects.AccessRuleTimeZone> access_rule_time_zones {get; set;} = [];
      public List<Objects.GroupAccessRule> group_access_rules {get; set;} = [];
}


