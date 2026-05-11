using System;
using SharedKernel.Helpers;

namespace Company.Domain.Entities;

public sealed class Companys
{
      public int Id { get; private set; }
      public string Name { get; private set; } = string.Empty;
      public string Address { get; private set; } = string.Empty;
      public string Description { get; private set; } = string.Empty;
      public Companys(
        int id,
        string name,
        string address,
        string description)
      {
            ValidationHelper.ValidateNotMinus(id, nameof(id));
            ValidationHelper.ValidateNotNullOrEmpty(name, nameof(name));
            Id = id;
            Name = name;
            Address = address;
            Description = description;

      }
}
