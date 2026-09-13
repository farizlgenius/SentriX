namespace Core.Infrastructure.Persistences.Entities;

public sealed class AeroDriverSetting : BaseEntity
{
  // System Level
  public int n_port { get; set; }
  public int n_scps { get; set; }
  // Create Channel
  public int c_type { get; set; }
  public int c_port { get; set; }

  // Scp Device Specification
  public int n_msp1_port { get; set; }
  public int n_trasaction { get; set; }
  public int n_sio { get; set; }
  public int n_mp { get; set; }
  public int n_cp { get; set; }
  public int n_acr { get; set; }
  public int n_alvl { get; set; }
  public int n_trgr { get; set; }
  public int n_proc { get; set; }
  public int gmt_offset { get; set; }
  public bool is_daylight_saving { get; set; }
  public int n_tz { get; set; }
  public int n_hol { get; set; }
  public int n_mpg { get; set; }
  public int n_tran_limit { get; set; }

  // Access Database Specification
  public int n_cards { get; set; }
  public int n_alvl_per_card { get; set; }
  public int pin_duress_mode { get; set; }
  public int duress_const_digit { get; set; }
  public int card_id_size { get; set; }
  public int pin_digit { get; set; }
  public int issue_code_bit { get; set; }
  public bool apb_location { get; set; }
  public int store_act_date { get; set; }
  public int store_deact_date { get; set; }
  public bool used_limit { get; set; }
  public int escort_timeout { get; set; }
  public int multi_card_timeout { get; set; }

  public AeroDriverSetting() { }

}