using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Operator : BaseEntity
{
      public string username { get; set; } = string.Empty;
      public string password { get; set; } = string.Empty;
      public Title title { get; set; } = Title.Mr;
      public string firstname { get; set; } = string.Empty;
      public string middlename { get; set; } = string.Empty;
      public string lastname { get; set; } = string.Empty;
      public Gender gender { get; set; } = Gender.Male;
      public string phone { get; set; } = string.Empty;
      public string email { get; set; } = string.Empty;
      public DateTime joined_date { get; set; } = DateTime.UtcNow;
      public DateTime? expired_date { get; set; }
      public int role_id { get; set; }
      public Role role { get; set; } = default!;
      public ICollection<OperatorLocation> operator_locations { get; set; } = default!;
      public Operator() { }

      public Operator(Domain.Entities.Operator domain) : base(domain.Guid)
      {
            this.username = domain.Username;
            this.password = domain.Password;
            this.title = domain.Title;
            this.firstname = domain.Firstname;
            this.middlename = domain.Middlename;
            this.lastname = domain.Lastname;
            this.gender = domain.Gender;
            this.phone = domain.Phone;
            this.email = domain.Email;
            this.joined_date = domain.JoinedDate;
            this.expired_date = domain.ExpiredDate;
            this.role_id = domain.RoleId;
            this.operator_locations = domain.LocationIds.Select(x => new OperatorLocation(0, x)).ToList();
      }

}