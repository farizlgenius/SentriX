using System;

namespace Cache.Domain.Entities;

public sealed class RedisSetting
{
      public string? ConnectionString { get; set; }
      public bool Enabled { get; set; }
}
