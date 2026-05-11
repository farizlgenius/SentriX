using System;

namespace Cache.Domain.Entities;

public sealed class Redis
{
      public string? ConnectionString { get; set; }
      public bool Enabled { get; set; }
}
