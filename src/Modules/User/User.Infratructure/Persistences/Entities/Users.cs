using System;
using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace User.Infratructure.Persistences.Entities;

public sealed class Users : BaseEntity
{
      [Required]
        public string identification {get; set;} = string.Empty;
        public string user_id { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string first_name { get; set; } = string.Empty;
        public string middle_name { get; set; } = string.Empty;
        public string last_name { get; set; } = string.Empty;
        public string gender { get; set; } = string.Empty;
        public DateTime date_of_birth {get; set;} 
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public int company_id { get; set; }
        public Company company { get; set; } = new Company();
        public int department_id { get; set; } 
        public Department department { get; set; } = new Department();
        public int position_id { get; set; }
        public Position position { get; set; } = new Position();
        public string address {get; set;} = string.Empty;
        public short flag { get; set; }
        public ICollection<UserAdditional> additionals { get; set; } = new List<UserAdditional>();
        public string image { get; set; } = string.Empty;
        public ICollection<Credential> credentials { get; set; } = new List<Credential>();
        // public ICollection<UserAccessLevel> user_access_levels { get; set; }
        public Users(){}
}
