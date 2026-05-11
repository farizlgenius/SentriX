using System;
using System.ComponentModel.DataAnnotations;

namespace SharedKernel.Domain;

public class BaseEntity
{
  [Key]
  public int id { get; set; }
  public DateTime created_at { get; set; }
  public DateTime updated_at { get; set; }
  public bool is_active { get; set; } = true;


}
