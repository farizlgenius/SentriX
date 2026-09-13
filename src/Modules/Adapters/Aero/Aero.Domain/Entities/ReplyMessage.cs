using System;

namespace Aero.Domain.Entities;


public sealed class ReplyMessage
{
  public int SCPId { get; set; }
  public int ReplyType { get; set; }
  public object AddrofReplyBuffer { get; set; } = default!;
  public object ReplyBuffer { get; set; } = default!;
  public int ChannelNo { get; set; }
  public SCPReplyCommStatus comm { get; set; } = default!;
  public SCPReplyNAK nak { get; set; } = default!;
  public SCPReplyIDReport id { get; set; } = default!;
  public SCPReplyUTAGReport utags { get; set; } = default!;
  public SCPReplyTranStatus tran_sts { get; set; } = default!;
  public SCPReplyTransaction tran { get; set; } = default!;
  public SCPReplySrMsp1Drvr sts_drvr { get; set; } = default!;
  public SCPReplySrSio sts_sio { get; set; } = default!;
  public SCPReplySrMp sts_mp { get; set; } = default!;
  public SCPReplySrCp sts_cp { get; set; } = default!;
  public SCPReplySrAcr sts_acr { get; set; } = default!;
  public SCPReplySrTz sts_tz { get; set; } = default!;
  public SCPReplySrTv sts_tv { get; set; } = default!;
  public SCPReplyCmndStatus cmnd_sts { get; set; } = default!;
  public SCPReplySrMpg sts_mpg { get; set; } = default!;
  public SCPReplySrArea sts_area { get; set; } = default!;
  public SCPReplyDualPort dual_prt { get; set; } = default!;
  public SCPReplyBioAddResult bio_result { get; set; } = default!;
  public SCPReplyLoginInfo login_info { get; set; } = new SCPReplyLoginInfo();
  public SCPReplyPkgInfo pkg_info { get; set; } = new SCPReplyPkgInfo();
  public SCPReplyFileInfo file_info { get; set; } = new SCPReplyFileInfo();
  public SCPReplyCertInfo cert_info { get; set; } = new SCPReplyCertInfo();
  public SCPReplyElevRelayInfo elev_relay_info { get; set; } = new SCPReplyElevRelayInfo();
  // public CC_WEB_CONFIG_NOTES web_notes { get; set; }
  public CC_WEB_CONFIG_NETWORK web_network { get; set; } = new CC_WEB_CONFIG_NETWORK();
  public CC_WEB_CONFIG_HOST_COMM_PRIM web_host_comm_prim { get; set; } = new CC_WEB_CONFIG_HOST_COMM_PRIM();
  // public CC_WEB_CONFIG_SESSION_TMR web_session_tmr { get; set; }
  // public CC_WEB_CONFIG_WEB_CONN web_conn { get; set; }
  // public CC_WEB_CONFIG_AUTO_SAVE web_auto_save { get; set; }
  // public CC_WEB_CONFIG_NETWORK_DIAG web_net_diag { get; set; }
  // public CC_WEB_CONFIG_TIME_SERVER web_time_server { get; set; }
  // public CC_WEB_CONFIG_DIAGNOSTICS web_diagnostics { get; set; }
  public SCPReplyOsdpPassthrough osdp_passthrough { get; set; } = default!;
  public SCPReplySioRelayCounts sio_relay_counts { get; set; } = default!;
  public SCPReplySioHidMfgInfo hid_mfg_info { get; set; } = default!;
  public SCPReplyCmndStatusExt cmnd_sts_ext { get; set; } = default!;
  public SCPReplySioReply sio_reply { get; set; } = default!;
  public SCPReplySioHeader sio_reply_header { get; set; } = default!;
  // public CC_SCP_ADBS adbs { get; set; }
  // public CC_ACCEXCEPTION acc_excpt { get; set; }
  // public CC_MP mp { get; set; }
  // public CC_CP cp { get; set; }
  // public CC_ACR acr { get; set; }
  // public CC_SCP_TZ tz { get; set; }
  // public CC_SCP_HOL hldy { get; set; }
  // public CC_TRGR trgr { get; set; }
  // public CC_TRGR_128 trgr128 { get; set; }
  // public CC_ALVL alvl { get; set; }
  // public CC_ADBC_I64DTIC32 adbc { get; set; }
  // public CC_ADBC_I64DTIC32A255FF adbc255FF { get; set; }
  public SCPReplyMemRead mem_read { get; set; } = default!;
  public SCPReplyStrStatus str_sts { get; set; } = default!;
  public SCPReplySanbxAppList sanbx_app_list { get; set; } = default!;

  public sealed class SCPReplyCommStatus
  {
    public int status { get; set; }
    public int error_code { get; set; }
    public short nChannelId { get; set; }
    public short current_primary_comm { get; set; }
    public short previous_primary_comm { get; set; }
    public short current_alternate_comm { get; set; }
    public short previous_alternate_comm { get; set; }
  }

  public sealed class SCPReplyNAK
  {
    public short reason { get; set; }
    public int data { get; set; }
    public byte[] command { get; set; } = default!;
    public int description_code { get; set; }
  }

  public sealed class SCPReplyIDReport
  {
    public short device_id { get; set; }
    public short device_ver { get; set; }
    public short sft_rev_major { get; set; }
    public short sft_rev_minor { get; set; }
    public int serial_number { get; set; }
    public int ram_size { get; set; }
    public int ram_free { get; set; }
    public int e_sec { get; set; }
    public int db_max { get; set; }
    public int db_active { get; set; }
    public byte dip_switch_pwrup { get; set; }
    public byte dip_switch_current { get; set; }
    public short scp_id { get; set; }
    public short firmware_advisory { get; set; }
    public short scp_in_1 { get; set; }
    public short scp_in_2 { get; set; }
    public int adb_max { get; set; }
    public int adb_active { get; set; }
    public int bio1_max { get; set; }
    public int bio1_active { get; set; }
    public int bio2_max { get; set; }
    public int bio2_active { get; set; }
    public short nOemCode { get; set; }
    public byte config_flags { get; set; }
    public byte[] mac_addr { get; set; } = default!;
    public byte tls_status { get; set; }
    public byte oper_mode { get; set; }
    public short scp_in_3 { get; set; }
    public int cumulative_bld_cnt { get; set; }
    public byte hardware_id { get; set; }
    public byte hardware_revision { get; set; }
    public int hardware_component_id { get; set; }

    public sealed class HARDWARE_COMP_PHY
    {
      public int value__ { get; set; }
    }

    public sealed class HARDWARE_COMP_CRYPTO
    {
      public int value__ { get; set; }
    }

    public sealed class HARDWARE_COMP_TYPE
    {
      public int value__ { get; set; }
    }
  }

  public sealed class SCPReplyUTAGReport
  {
    public short nCount { get; set; }
    public short nFirst { get; set; }
    public int[] list { get; set; } = default!;
  }

  public sealed class SCPReplyTranStatus
  {
    public int capacity { get; set; }
    public int oldest { get; set; }
    public int last_rprtd { get; set; }
    public int last_loggd { get; set; }
    public short disabled { get; set; }
  }

  public sealed class TypeSys
  {
    public short error_code { get; set; }
  }

  public sealed class TypeSysComm
  {
    public short error_code { get; set; }
    public short current_primary_comm { get; set; }
    public short previous_primary_comm { get; set; }
    public short current_alternate_comm { get; set; }
    public short previous_alternate_comm { get; set; }
  }

  public sealed class TypeSioComm
  {
    public short comm_sts { get; set; }
    public byte model { get; set; }
    public byte revision { get; set; }
    public int ser_num { get; set; }
    public short nExtendedInfoValid { get; set; }
    public short nHardwareId { get; set; }
    public short nHardwareRev { get; set; }
    public short nProductId { get; set; }
    public short nProductVer { get; set; }
    public short nFirmwareBoot { get; set; }
    public short nFirmwareLdr { get; set; }
    public short nFirmwareApp { get; set; }
    public short nOemCode { get; set; }
    public byte nEncConfig { get; set; }
    public byte nEncKeyStatus { get; set; }
    public byte[] mac_addr { get; set; } = default!;
    public int nHardwareComponents { get; set; }
  }

  public sealed class TypeCardBin
  {
    public short bit_count { get; set; }
    public byte[] bit_array { get; set; } = default!;
  }

  public sealed class TypeCardBcd
  {
    public short digit_count { get; set; }
    public byte[] bcd_array { get; set; } = default!;
  }

  public sealed class TypeCardFull
  {
    public short format_number { get; set; }
    public int facility_code { get; set; }
    public int cardholder_id { get; set; }
    public short issue_code { get; set; }
    public short floor_number { get; set; }
    public byte[] encoded_card { get; set; } = default!;
  }

  public sealed class TypeDblCardFull
  {
    public short format_number { get; set; }
    public int facility_code { get; set; }
    public double cardholder_id { get; set; }
    public short issue_code { get; set; }
    public short floor_number { get; set; }
    public byte[] encoded_card { get; set; } = default!;
  }

  public sealed class TypeI64CardFull
  {
    public short format_number { get; set; }
    public int facility_code { get; set; }
    public long cardholder_id { get; set; }
    public short issue_code { get; set; }
    public short floor_number { get; set; }
    public byte[] encoded_card { get; set; } = default!;
  }

  public sealed class TypeI64CardFullIc32
  {
    public short format_number { get; set; }
    public int facility_code { get; set; }
    public long cardholder_id { get; set; }
    public int issue_code { get; set; }
    public short floor_number { get; set; }
    public byte[] encoded_card { get; set; } = default!;
  }

  public sealed class TypeHostCardFullPin
  {
    public short format_number { get; set; }
    public int facility_code { get; set; }
    public long cardholder_id { get; set; }
    public int issue_code { get; set; }
    public short floor_number { get; set; }
    public byte[] pin { get; set; } = default!;
    public byte[] encoded_card { get; set; } = default!;
  }

  public sealed class TypeCardID
  {
    public short format_number { get; set; }
    public int cardholder_id { get; set; }
    public short floor_number { get; set; }
    public short card_type_flags { get; set; }
    public short elev_cab { get; set; }
  }

  public sealed class TypeDblCardID
  {
    public short format_number { get; set; }
    public double cardholder_id { get; set; }
    public short floor_number { get; set; }
    public short card_type_flags { get; set; }
    public short elev_cab { get; set; }
  }

  public sealed class TypeI64CardID
  {
    public short format_number { get; set; }
    public long cardholder_id { get; set; }
    public short floor_number { get; set; }
    public short card_type_flags { get; set; }
    public short elev_cab { get; set; }
  }

  public sealed class TypeCoS
  {
    public byte status { get; set; }
    public byte old_sts { get; set; }
  }

  public sealed class TypeREX
  {
    public short rex_number { get; set; }
  }

  public sealed class TypeCoSDoor
  {
    public byte door_status { get; set; }
    public byte ap_status { get; set; }
    public byte ap_prior { get; set; }
    public byte door_prior { get; set; }
  }

  public sealed class TypeUserCmnd
  {
    public short nKeys { get; set; }
    public char[] keys { get; set; } = default!;
  }

  public sealed class TypeActivate
  {
    public object activationCount { get; set; } = default!;
  }

  public sealed class TypeProcedure
  {
  }

  public sealed class TypeAcr
  {
    public short actl_flags { get; set; }
    public short prior_flags { get; set; }
    public short prior_mode { get; set; }
    public short actl_flags_e { get; set; }
    public short prior_flags_e { get; set; }
    public int auth_mod_flags { get; set; }
    public int prior_auth_mod_flags { get; set; }
  }

  public sealed class TypeMPG
  {
    public short mask_count { get; set; }
    public short nActiveMps { get; set; }
    public short[] nMpList { get; set; } = default!;
  }

  public sealed class sIpsCos_Cos
  {
    public short nIpsCosSourceType { get; set; }
    public short nIpsCosSourceNumber { get; set; }
    public short nIpsCosSourceStateCurrent { get; set; }
    public short nIpsCosSourceStatePrior { get; set; }
  }

  public sealed class sIpsCos_Acr
  {
    public short nIpsCosAction { get; set; }
    public short nIpsCosActionSrcType { get; set; }
    public short nIpsCosActionSrcNumber { get; set; }
    public long nCardholderId { get; set; }
  }

  public sealed class sIpsPtSet
  {
    public short nIpsSourceType { get; set; }
    public short nIpsSourceNumber { get; set; }
    public short nIpsPointType { get; set; }
    public short nIpsPointNumber { get; set; }
    public short nPointModeCurrent { get; set; }
    public short nPointModePrior { get; set; }
  }

  public sealed class TypeArea
  {
    public short status { get; set; }
    public int occupancy { get; set; }
    public int occ_spc { get; set; }
    public short prior_status { get; set; }
  }

  public sealed class TypeUseLimit
  {
    public short use_count { get; set; }
    public long cardholder_id { get; set; }
  }

  public sealed class TypeAsci
  {
    public char[] bfr { get; set; } = default!;
  }

  public sealed class TypeSioDiag
  {
    public short length { get; set; }
    public byte[] bfr { get; set; } = default!;
  }

  public sealed class TypeAcrExtFeatureStls
  {
    public short nExtFeatureType { get; set; }
    public short nHardwareType { get; set; }
    public byte[] nExtFeatureData { get; set; } = default!;
    public byte[] nExtFeatureStatus { get; set; } = default!;
  }

  public sealed class TypeAcrExtFeatureCoS
  {
    public short nExtFeatureType { get; set; }
    public short nHardwareType { get; set; }
    public short nExtFeaturePoint { get; set; }
    public byte nStatus { get; set; }
    public byte nStatusPrior { get; set; }
    public byte[] nExtFeatureData { get; set; } = default!;
    public byte[] nExtFeatureStatus { get; set; } = default!;
  }

  public sealed class TypeWebActivity
  {
    public byte iType { get; set; }
    public byte iCurUserId { get; set; }
    public byte iObjectUserId { get; set; }
    public char[] szObjectUser { get; set; } = default!;
    public int ipAddress { get; set; }
  }

  public sealed class TypeOperatingMode
  {
    public byte prev_oper { get; set; }
  }

  public sealed class TypeOAL
  {
    public byte nReasonCode { get; set; }
    public byte[] nData { get; set; } = default!;
  }

  public sealed class TypeCoSFloor
  {
    public byte prevFloorStatus { get; set; }
    public byte floorNumber { get; set; }
  }

  public sealed class TypeFileDownloadStatus
  {
    public byte fileType { get; set; }
    public char[] fileName { get; set; } = default!;
  }

  public sealed class TypeBatchReport
  {
    public object triggerNumber { get; set; } = default!;
    public object activationCount { get; set; } = default!;
    public byte sourceType { get; set; }
    public object sourceNumber { get; set; } = default!;
    public byte tranType { get; set; }
    public byte[] tranCodeMap { get; set; } = default!;
  }

  public sealed class TypeCoSElevatorAccess
  {
    public long cardholder_id { get; set; }
    public byte[] floors { get; set; } = default!;
    public byte nCardFormat { get; set; }
  }

  public sealed class SCPReplyTransactionHeader
  {
    public int ser_num { get; set; }
    public int time { get; set; }
    public short source_type { get; set; }
    public short source_number { get; set; }
    public short tran_type { get; set; }
    public short tran_code { get; set; }
  }

  public sealed class SCPReplyTransaction
  {
    public int ser_num { get; set; }
    public int time { get; set; }
    public short source_type { get; set; }
    public short source_number { get; set; }
    public short tran_type { get; set; }
    public short tran_code { get; set; }
    public TypeSys sys { get; set; } = default!;
    public TypeSysComm sys_comm { get; set; } = default!;
    public TypeSioComm s_comm { get; set; } = default!;
    public TypeCardBin c_bin { get; set; } = default!;
    public TypeCardBcd c_bcd { get; set; } = default!;
    public TypeCardFull c_full { get; set; } = default!;
    public TypeCardID c_id { get; set; } = default!;
    public TypeDblCardFull c_fulldbl { get; set; } = default!;
    public TypeDblCardID c_iddbl { get; set; } = default!;
    public TypeI64CardFull c_fulli64 { get; set; } = default!;
    public TypeI64CardFullIc32 c_fulli64i32 { get; set; } = default!;
    public TypeHostCardFullPin c_fullHostPin { get; set; } = default!;
    public TypeI64CardID c_idi64 { get; set; } = default!;
    public TypeCoS cos { get; set; } = default!;
    public TypeREX rex { get; set; } = default!;
    public TypeCoSDoor door { get; set; } = default!;
    public TypeProcedure proc { get; set; } = default!;
    public TypeUserCmnd usrcmd { get; set; } = default!;
    public TypeActivate act { get; set; } = default!;
    public TypeAcr acr { get; set; } = default!;
    public TypeMPG mpg { get; set; } = default!;
    public TypeOAL oal { get; set; } = default!;
    public TypeArea area { get; set; } = default!;
    public TypeUseLimit c_uselimit { get; set; } = default!;
    public TypeAsci t_diag { get; set; } = default!;
    public TypeSioDiag s_diag { get; set; } = default!;
    public TypeAcrExtFeatureStls extfeat_stls { get; set; } = default!;
    public TypeAcrExtFeatureCoS extfeat_cos { get; set; } = default!;
    public TypeWebActivity web_activity { get; set; } = default!;
    public TypeOperatingMode oper_mode { get; set; } = default!;
    public TypeCoSFloor floor { get; set; } = default!;
    public TypeFileDownloadStatus file_download { get; set; } = default!;
    public TypeCoSElevatorAccess elev_access { get; set; } = default!;
    public TypeBatchReport batch_report { get; set; } = default!;
  }

  public sealed class SCPReplyDualPort
  {
    public short number { get; set; }
    public short stat_this { get; set; }
    public short stat_primary { get; set; }
    public short stat_alternate { get; set; }
  }

  public sealed class SCPReplyBioAddResult
  {
    public short nBioType { get; set; }
    public short nResult { get; set; }
    public long nCardId { get; set; }
    public int nCommandTag { get; set; }
  }

  public sealed class SCPReplyLoginInfo
  {
    public char[] name { get; set; } = default!;
    public char[] notes { get; set; } = default!;
    public short acctType { get; set; }
    public short userType { get; set; }
    public short userId { get; set; }
  }

  public sealed class SCPReplyPkgInfo
  {
    public char[] pkgName { get; set; } = default!;
    public char[] pkgVersion { get; set; } = default!;
    public long installDate { get; set; }
  }

  public sealed class SCPReplyFileInfo
  {
    public short file_type { get; set; }
    public short file_index { get; set; }
    public short num_files { get; set; }
    public char[] fileName { get; set; } = default!;
  }

  public sealed class SCPReplyCertInfo
  {
    public char[] issuedTo { get; set; } = default!;
    public char[] issuedBy { get; set; } = default!;
    public char[] issuedStart { get; set; } = default!;
    public char[] issuedExpire { get; set; } = default!;
  }

  public sealed class SCPReplyElevRelayInfo
  {
    public short acr_number { get; set; }
    public byte[] status { get; set; } = default!;
  }

  public sealed class SCPReplyOsdpPassthrough
  {
    public short acr_number { get; set; }
    public int sequence_num { get; set; }
    public short reader_role { get; set; }
    public short msg_type { get; set; }
    public short data_len { get; set; }
    public byte[] data { get; set; } = default!;
  }

  public sealed class SCPReplySioRelayCounts
  {
    public short sio_number { get; set; }
    public short num_relays { get; set; }
    public int[] data { get; set; } = default!;
  }

  public sealed class SCPReplySioHidMfgInfo
  {
    public short sio_number { get; set; }
    public byte[] serial_no { get; set; } = default!;
    public byte[] uuid { get; set; } = default!;
  }

  public sealed class SCPReplySrMsp1Drvr
  {
    public short number { get; set; }
    public short port { get; set; }
    public short mode { get; set; }
    public int baud_rate { get; set; }
    public short throughput { get; set; }
  }

  public sealed class SCPReplySrSio
  {
    public short number { get; set; }
    public short com_status { get; set; }
    public short msp1_dnum { get; set; }
    public int com_retries { get; set; }
    public short ct_stat { get; set; }
    public short pw_stat { get; set; }
    public short model { get; set; }
    public short revision { get; set; }
    public int serial_number { get; set; }
    public short inputs { get; set; }
    public short outputs { get; set; }
    public short readers { get; set; }
    public short[] ip_stat { get; set; } = default!;
    public short[] op_stat { get; set; } = default!;
    public short[] rdr_stat { get; set; } = default!;
    public short nExtendedInfoValid { get; set; }
    public short nHardwareId { get; set; }
    public short nHardwareRev { get; set; }
    public short nProductId { get; set; }
    public short nProductVer { get; set; }
    public short nFirmwareBoot { get; set; }
    public short nFirmwareLdr { get; set; }
    public short nFirmwareApp { get; set; }
    public short nOemCode { get; set; }
    public byte nEncConfig { get; set; }
    public byte nEncKeyStatus { get; set; }
    public byte[] mac_addr { get; set; } = default!;
    public short emg_stat { get; set; }
  }

  public sealed class SCPReplySrMp
  {
    public short first { get; set; }
    public short count { get; set; }
    public short[] status { get; set; } = default!;
  }

  public sealed class SCPReplySrCp
  {
    public short first { get; set; }
    public short count { get; set; }
    public short[] status { get; set; } = default!;
  }

  public sealed class SCPReplySrAcr
  {
    public short number { get; set; }
    public short mode { get; set; }
    public short rdr_status { get; set; }
    public short strk_status { get; set; }
    public short door_status { get; set; }
    public short ap_status { get; set; }
    public short rex_status0 { get; set; }
    public short rex_status1 { get; set; }
    public short led_mode { get; set; }
    public short actl_flags { get; set; }
    public short altrdr_status { get; set; }
    public short actl_flags_extd { get; set; }
    public short nExtFeatureType { get; set; }
    public short nHardwareType { get; set; }
    public byte[] nExtFeatureStatus { get; set; } = default!;
    public int nAuthModFlags { get; set; }
  }

  public sealed class SCPReplySrTz
  {
    public short first { get; set; }
    public short count { get; set; }
    public short[] status { get; set; } = default!;
  }

  public sealed class SCPReplySrTv
  {
    public short first { get; set; }
    public short count { get; set; }
    public short[] status { get; set; } = default!;
  }

  public sealed class SCPReplySrMpg
  {
    public short number { get; set; }
    public short mask_count { get; set; }
    public short num_active { get; set; }
    public short[] active_mp_list { get; set; } = default!;
  }

  public sealed class SCPReplySrArea
  {
    public short number { get; set; }
    public short flags { get; set; }
    public int occupancy { get; set; }
    public int occ_spc { get; set; }
  }

  public sealed class SCPReplyCmndStatus
  {
    public short status { get; set; }
    public int sequence_number { get; set; }
    public SCPReplyNAK nak { get; set; } = default!;
  }

  public sealed class SCPReplyCmndStatusExt
  {
    public short status { get; set; }
    public int lSequenceFirst { get; set; }
    public int lSequenceLast { get; set; }
    public SCPReplyNAK nak { get; set; } = default!;
  }

  public sealed class SCPReplyMemRead
  {
    public short nType { get; set; }
    public int nBase { get; set; }
    public short nSize { get; set; }
    public byte[] nData { get; set; } = default!;
  }

  public sealed class SanbxApp
  {
    public int appCode { get; set; }
    public byte[] version { get; set; } = default!;
    public short state { get; set; }
  }

  public sealed class SCPReplySanbxAppList
  {
    public short nApps { get; set; }
    public object[] apps { get; set; } = default!;
  }

  public sealed class StrSpec
  {
    public short nStrType { get; set; }
    public int nRecords { get; set; }
    public int nRecSize { get; set; }
    public int nActive { get; set; }
  }

  public sealed class SCPReplyStrStatus
  {
    public short nListLength { get; set; }
    public StrSpec[] sStrSpec { get; set; } = default!;
  }



  public sealed class sior_idr
  {
    public short nModel { get; set; }
    public short nRevision { get; set; }
    public int nSerNum { get; set; }
    public short nRxbLen { get; set; }
  }

  public sealed class sior_diag
  {
    public short sw { get; set; }
    public short nInputs { get; set; }
    public byte[] cA2D { get; set; } = default!;
  }

  public sealed class sior_lsr
  {
    public byte cCtSts { get; set; }
    public byte cPwrSts { get; set; }
  }

  public sealed class sior_isr
  {
    public short nInputs { get; set; }
    public byte[] cIpSts { get; set; } = default!;
  }

  public sealed class sior_rtsr
  {
    public short nReaders { get; set; }
    public byte[] cRdrTmpr { get; set; } = default!;
  }

  public sealed class sior_mr50sr
  {
    public byte cCtSts { get; set; }
    public byte cRdrTmpr { get; set; }
    public byte cIpSts0 { get; set; }
    public byte cIpSts1 { get; set; }
    public byte cIpSts2 { get; set; }
    public byte[] dummy { get; set; } = default!;
  }

  public sealed class sior_cdb
  {
    public short nReader { get; set; }
    public short nBitCount { get; set; }
    public byte[] cData { get; set; } = default!;
  }

  public sealed class sior_cdd
  {
    public short nReader { get; set; }
    public short nReadDirection { get; set; }
    public short nDigitCount { get; set; }
    public byte[] cData { get; set; } = default!;
  }

  public sealed class sior_key
  {
    public short nReader { get; set; }
    public short nKeyCount { get; set; }
    public byte[] cKeys { get; set; } = default!;
  }

  public sealed class sior_idrx
  {
    public byte nHardwareId { get; set; }
    public byte nHardwareRev { get; set; }
    public byte nProductId { get; set; }
    public byte nProductVer { get; set; }
    public byte nFirmwareMaj { get; set; }
    public byte nFirmwareMin { get; set; }
    public byte nFirmwareBld { get; set; }
    public int nSerNum { get; set; }
    public short nOemCode { get; set; }
    public byte rxb_ln { get; set; }
    public byte nBootType { get; set; }
    public byte nBootVer { get; set; }
    public byte nBootMaj { get; set; }
    public byte nBootMin { get; set; }
    public byte nBootBld { get; set; }
    public byte nLdrType { get; set; }
    public byte nLdrVer { get; set; }
    public byte nLdrMaj { get; set; }
    public byte nLdrMin { get; set; }
    public byte nLdrBld { get; set; }
    public byte[] mac_addr { get; set; } = default!;
  }

  public sealed class sior_osr
  {
    public short nOutputs { get; set; }
    public byte[] cOpSts { get; set; } = default!;
  }

  public sealed class sioc_aperio_actstate
  {
    public byte nReader { get; set; }
    public byte door_side { get; set; }
    public byte state { get; set; }
    public byte id { get; set; }
  }

  public sealed class sioc_aperio_doormode
  {
    public byte nReader { get; set; }
    public byte mode { get; set; }
  }

  public sealed class SCPReplySioReply
  {
    public short nSioReply { get; set; }
    public sior_idr idr { get; set; } = default!;
    public sior_diag diag { get; set; } = default!;
    public sior_lsr lsr { get; set; } = default!;
    public sior_isr isr { get; set; } = default!;
    public sior_rtsr rtsr { get; set; } = default!;
    public sior_mr50sr mr50sr { get; set; } = default!;
    public sior_cdb cdb { get; set; } = default!;
    public sior_cdd cdd { get; set; } = default!;
    public sior_key key { get; set; } = default!;
    public sior_idrx xidr { get; set; } = default!;
    public sior_osr osr { get; set; } = default!;
    public sioc_aperio_actstate actstate { get; set; } = default!;
    public sioc_aperio_doormode doormode { get; set; } = default!;
  }

  public sealed class SCPReplySioHeader
  {
    public short nSioReply { get; set; }
    public byte[] dummy { get; set; } = default!;
  }

  public sealed class CC_WEB_CONFIG_NETWORK
  {

    public short scp_number { get; set; }

    public short method { get; set; }
    public int cIpAddr { get; set; }

    public int cSubnetMask { get; set; }

    public int cDfltGateway { get; set; }

    public char[] cHostName { get; set; } = default!;

    public short dnsType { get; set; }

    public int cDns { get; set; }

    public char[] cDnsSuffix { get; set; } = default!;

    public short method2 { get; set; }
    public int cIpAddr2 { get; set; }

    public int cSubnetMask2 { get; set; }

    public int cDfltGateway2 { get; set; }

    public int cDns2 { get; set; }

    public short TnlEnable { get; set; }

    public int cIpTnl { get; set; }

    public int cPortTnl { get; set; }
  }

  public sealed class CC_WEB_CONFIG_HOST_COMM_PRIM
  {
    public short scp_number { get; set; }

    public short address { get; set; }

    public short dataSecurity { get; set; }

    public short cType { get; set; }

    public HostCommIpServer ipserver { get; set; } = new HostCommIpServer();

    public HostCommIpClient ipclient { get; set; } = new HostCommIpClient();
  }

  public sealed class HostCommIpServer
  {
    public int cAuthIP1 { get; set; }

    public int cAuthIP2 { get; set; }

    public short nPort { get; set; }

    public short enableAuthIP { get; set; }

    public short nNicSel { get; set; }
  }

  public sealed class HostCommIpClient
  {
    public int cHostIP { get; set; }

    public short nPort { get; set; }

    public short rqIntvl { get; set; }

    public short connMode { get; set; }

    public char[] cHostName { get; set; } = default!;

    public short nNicSel { get; set; }
  }
}

