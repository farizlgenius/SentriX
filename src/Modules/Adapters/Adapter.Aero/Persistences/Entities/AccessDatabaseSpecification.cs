using System;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class AccessDatabaseSpecification : BaseEntity
{
      public short scp_id {get; set;}
      public string mac {get; set;} = string.Empty;
      public short n_card {get; set;}
      public short n_alvl {get; set;}
      public short n_pin_digits {get; set;}
      public short b_issue_code {get; set;}
      public short b_apb_location {get; set;}
      public short b_act_date {get; set;}
      public short b_deact_date {get; set;}
      public short b_vacation_date {get; set;}
      public short b_upgrade_date {get;set;}
      public short b_user_level {get;set;}
      public short b_use_limit {get; set;}
      public short b_support_time_apb {get; set;}
      public short n_tz {get; set;}
      public short b_asset_group {get; set;}
      public short n_host_response_timeout {get; set;}
      public short n_alvl_use4arg {get; set;}
      public short n_escort_timeout {get; set;}
      public short n_multi_card_timeout {get;set;}

      public AccessDatabaseSpecification(){}

      public AccessDatabaseSpecification(short scp_id, string mac, short n_card, short n_alvl, short n_pin_digits, short b_issue_code, short b_apb_location, short b_act_date, short b_deact_date, short b_vacation_date, short b_upgrade_date, short b_user_level, short b_use_limit, short b_support_time_apb, short n_tz, short b_asset_group, short n_host_response_timeout, short n_alvl_use4arg, short n_escort_timeout, short n_multi_card_timeout)
      {
            this.scp_id = scp_id;
            this.mac = mac;
            this.n_card = n_card;
            this.n_alvl = n_alvl;
            this.n_pin_digits = n_pin_digits;
            this.b_issue_code = b_issue_code;
            this.b_apb_location = b_apb_location;
            this.b_act_date = b_act_date;
            this.b_deact_date = b_deact_date;
            this.b_vacation_date = b_vacation_date;
            this.b_upgrade_date = b_upgrade_date;
            this.b_user_level = b_user_level;
            this.b_use_limit = b_use_limit;
            this.b_support_time_apb = b_support_time_apb;
            this.n_tz = n_tz;
            this.b_asset_group = b_asset_group;
            this.n_host_response_timeout = n_host_response_timeout;
            this.n_alvl_use4arg = n_alvl_use4arg;
            this.n_escort_timeout = n_escort_timeout;
            this.n_multi_card_timeout = n_multi_card_timeout;
      }
}
