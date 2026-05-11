using System;

namespace Adapter.Aero.Model;


public sealed class SCPReplyMessageDto
{
    public int SCPId { get; set; }
    public int ReplyType { get; set; }
    public object AddrofReplyBuffer { get; set; } = default!;
    public object ReplyBuffer { get; set; } = default!;
    public int ChannelNo { get; set; }
    public SCPReplyCommStatusDto comm { get; set; } = default!;
    public SCPReplyNAKDto nak { get; set; } = default!;
    public SCPReplyIDReportDto id { get; set; } = default!;
    public SCPReplyUTAGReportDto utags { get; set; } = default!;
    public SCPReplyTranStatusDto tran_sts { get; set; } = default!;
    public SCPReplyTransactionDto tran { get; set; } = default!;
    public SCPReplySrMsp1DrvrDto sts_drvr { get; set; } = default!;
    public SCPReplySrSioDto sts_sio { get; set; }= default!;
    public SCPReplySrMpDto sts_mp { get; set; } = default!;
    public SCPReplySrCpDto sts_cp { get; set; } = default!;
    public SCPReplySrAcrDto sts_acr { get; set; } = default!;
    public SCPReplySrTzDto sts_tz { get; set; } = default!;
    public SCPReplySrTvDto sts_tv { get; set; } = default!;
    public SCPReplyCmndStatusDto cmnd_sts { get; set; } = default!;
    public SCPReplySrMpgDto sts_mpg { get; set; } = default!;
    public SCPReplySrAreaDto sts_area { get; set; } = default!;
    public SCPReplyDualPortDto dual_prt { get; set; } = default!;
    public SCPReplyBioAddResultDto bio_result { get; set; } = default!;
    public SCPReplyLoginInfoDto login_info { get; set; } = new SCPReplyLoginInfoDto();
    public SCPReplyPkgInfoDto pkg_info { get; set; } = new SCPReplyPkgInfoDto();
    public SCPReplyFileInfoDto file_info { get; set; } = new SCPReplyFileInfoDto();
    public SCPReplyCertInfoDto cert_info { get; set; } = new SCPReplyCertInfoDto();
    public SCPReplyElevRelayInfoDto elev_relay_info { get; set; } = new SCPReplyElevRelayInfoDto();
    // public CC_WEB_CONFIG_NOTESDto web_notes { get; set; }
    public CC_WEB_CONFIG_NETWORKDto web_network { get; set; } = new CC_WEB_CONFIG_NETWORKDto();
    public CC_WEB_CONFIG_HOST_COMM_PRIMDto web_host_comm_prim { get; set; } = new CC_WEB_CONFIG_HOST_COMM_PRIMDto();
    // public CC_WEB_CONFIG_SESSION_TMRDto web_session_tmr { get; set; }
    // public CC_WEB_CONFIG_WEB_CONNDto web_conn { get; set; }
    // public CC_WEB_CONFIG_AUTO_SAVEDto web_auto_save { get; set; }
    // public CC_WEB_CONFIG_NETWORK_DIAGDto web_net_diag { get; set; }
    // public CC_WEB_CONFIG_TIME_SERVERDto web_time_server { get; set; }
    // public CC_WEB_CONFIG_DIAGNOSTICSDto web_diagnostics { get; set; }
    public SCPReplyOsdpPassthroughDto osdp_passthrough { get; set; } = default!;
    public SCPReplySioRelayCountsDto sio_relay_counts { get; set; } = default!;
    public SCPReplySioHidMfgInfoDto hid_mfg_info { get; set; } = default!;
    public SCPReplyCmndStatusExtDto cmnd_sts_ext { get; set; } = default!;
    public SCPReplySioReplyDto sio_reply { get; set; } = default!;
    public SCPReplySioHeaderDto sio_reply_header { get; set; } = default!;
    // public CC_SCP_ADBSDto adbs { get; set; }
    // public CC_ACCEXCEPTIONDto acc_excpt { get; set; }
    // public CC_MPDto mp { get; set; }
    // public CC_CPDto cp { get; set; }
    // public CC_ACRDto acr { get; set; }
    // public CC_SCP_TZDto tz { get; set; }
    // public CC_SCP_HOLDto hldy { get; set; }
    // public CC_TRGRDto trgr { get; set; }
    // public CC_TRGR_128Dto trgr128 { get; set; }
    // public CC_ALVLDto alvl { get; set; }
    // public CC_ADBC_I64DTIC32Dto adbc { get; set; }
    // public CC_ADBC_I64DTIC32A255FFDto adbc255FF { get; set; }
    public SCPReplyMemReadDto mem_read { get; set; } = default!;
    public SCPReplyStrStatusDto str_sts { get; set; } = default!;
    public SCPReplySanbxAppListDto sanbx_app_list { get; set; } = default!;

    public class SCPReplyCommStatusDto
    {
        public int status { get; set; }
        public int error_code { get; set; }
        public short nChannelId { get; set; }
        public short current_primary_comm { get; set; }
        public short previous_primary_comm { get; set; }
        public short current_alternate_comm { get; set; }
        public short previous_alternate_comm { get; set; }
    }

    public class SCPReplyNAKDto
    {
        public short reason { get; set; }
        public int data { get; set; }
        public byte[] command { get; set; } = default!;
        public int description_code { get; set; }
    }

    public class SCPReplyIDReportDto
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

        public class HARDWARE_COMP_PHYDto
        {
            public int value__ { get; set; }
        }

        public class HARDWARE_COMP_CRYPTODto
        {
            public int value__ { get; set; }
        }

        public class HARDWARE_COMP_TYPEDto
        {
            public int value__ { get; set; }
        }
    }

    public class SCPReplyUTAGReportDto
    {
        public short nCount { get; set; }
        public short nFirst { get; set; }
        public int[] list { get; set; } = default!;
    }

    public class SCPReplyTranStatusDto
    {
        public int capacity { get; set; }
        public int oldest { get; set; }
        public int last_rprtd { get; set; }
        public int last_loggd { get; set; }
        public short disabled { get; set; }
    }

    public class TypeSysDto
    {
        public short error_code { get; set; }
    }

    public class TypeSysCommDto
    {
        public short error_code { get; set; }
        public short current_primary_comm { get; set; }
        public short previous_primary_comm { get; set; }
        public short current_alternate_comm { get; set; }
        public short previous_alternate_comm { get; set; }
    }

    public class TypeSioCommDto
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

    public class TypeCardBinDto
    {
        public short bit_count { get; set; }
        public byte[] bit_array { get; set; } = default!;
    }

    public class TypeCardBcdDto
    {
        public short digit_count { get; set; }
        public byte[] bcd_array { get; set; } = default!;
    }

    public class TypeCardFullDto
    {
        public short format_number { get; set; }
        public int facility_code { get; set; }
        public int cardholder_id { get; set; }
        public short issue_code { get; set; }
        public short floor_number { get; set; }
        public byte[] encoded_card { get; set; } = default!;
    }

    public class TypeDblCardFullDto
    {
        public short format_number { get; set; }
        public int facility_code { get; set; }
        public double cardholder_id { get; set; }
        public short issue_code { get; set; }
        public short floor_number { get; set; }
        public byte[] encoded_card { get; set; } = default!;
    }

    public class TypeI64CardFullDto
    {
        public short format_number { get; set; }
        public int facility_code { get; set; }
        public long cardholder_id { get; set; }
        public short issue_code { get; set; }
        public short floor_number { get; set; }
        public byte[] encoded_card { get; set; } = default!;
    }

    public class TypeI64CardFullIc32Dto
    {
        public short format_number { get; set; }
        public int facility_code { get; set; }
        public long cardholder_id { get; set; }
        public int issue_code { get; set; }
        public short floor_number { get; set; }
        public byte[] encoded_card { get; set; } = default!;
    }

    public class TypeHostCardFullPinDto
    {
        public short format_number { get; set; }
        public int facility_code { get; set; }
        public long cardholder_id { get; set; }
        public int issue_code { get; set; }
        public short floor_number { get; set; }
        public byte[] pin { get; set; } = default!;
        public byte[] encoded_card { get; set; } = default!;
    }

    public class TypeCardIDDto
    {
        public short format_number { get; set; }
        public int cardholder_id { get; set; }
        public short floor_number { get; set; }
        public short card_type_flags { get; set; }
        public short elev_cab { get; set; }
    }

    public class TypeDblCardIDDto
    {
        public short format_number { get; set; }
        public double cardholder_id { get; set; }
        public short floor_number { get; set; }
        public short card_type_flags { get; set; }
        public short elev_cab { get; set; }
    }

    public class TypeI64CardIDDto
    {
        public short format_number { get; set; }
        public long cardholder_id { get; set; }
        public short floor_number { get; set; }
        public short card_type_flags { get; set; }
        public short elev_cab { get; set; }
    }

    public class TypeCoSDto
    {
        public byte status { get; set; }
        public byte old_sts { get; set; }
    }

    public class TypeREXDto
    {
        public short rex_number { get; set; }
    }

    public class TypeCoSDoorDto
    {
        public byte door_status { get; set; }
        public byte ap_status { get; set; }
        public byte ap_prior { get; set; }
        public byte door_prior { get; set; }
    }

    public class TypeUserCmndDto
    {
        public short nKeys { get; set; }
        public char[] keys { get; set; } = default!;
    }

    public class TypeActivateDto
    {
        public object activationCount { get; set; } = default!;
    }

    public class TypeProcedureDto
    {
    }

    public class TypeAcrDto
    {
        public short actl_flags { get; set; }
        public short prior_flags { get; set; }
        public short prior_mode { get; set; }
        public short actl_flags_e { get; set; }
        public short prior_flags_e { get; set; }
        public int auth_mod_flags { get; set; }
        public int prior_auth_mod_flags { get; set; }
    }

    public class TypeMPGDto
    {
        public short mask_count { get; set; }
        public short nActiveMps { get; set; }
        public short[] nMpList { get; set; } = default!;
    }

    public class sIpsCos_CosDto
    {
        public short nIpsCosSourceType { get; set; }
        public short nIpsCosSourceNumber { get; set; }
        public short nIpsCosSourceStateCurrent { get; set; }
        public short nIpsCosSourceStatePrior { get; set; }
    }

    public class sIpsCos_AcrDto
    {
        public short nIpsCosAction { get; set; }
        public short nIpsCosActionSrcType { get; set; }
        public short nIpsCosActionSrcNumber { get; set; }
        public long nCardholderId { get; set; }
    }

    public class sIpsPtSetDto
    {
        public short nIpsSourceType { get; set; }
        public short nIpsSourceNumber { get; set; }
        public short nIpsPointType { get; set; }
        public short nIpsPointNumber { get; set; }
        public short nPointModeCurrent { get; set; }
        public short nPointModePrior { get; set; }
    }

    public class TypeAreaDto
    {
        public short status { get; set; }
        public int occupancy { get; set; }
        public int occ_spc { get; set; }
        public short prior_status { get; set; }
    }

    public class TypeUseLimitDto
    {
        public short use_count { get; set; }
        public long cardholder_id { get; set; }
    }

    public class TypeAsciDto
    {
        public char[] bfr { get; set; } = default!;
    }

    public class TypeSioDiagDto
    {
        public short length { get; set; }
        public byte[] bfr { get; set; } = default!;
    }

    public class TypeAcrExtFeatureStlsDto
    {
        public short nExtFeatureType { get; set; }
        public short nHardwareType { get; set; }
        public byte[] nExtFeatureData { get; set; } = default!;
        public byte[] nExtFeatureStatus { get; set; } = default!;
    }

    public class TypeAcrExtFeatureCoSDto
    {
        public short nExtFeatureType { get; set; }
        public short nHardwareType { get; set; }
        public short nExtFeaturePoint { get; set; }
        public byte nStatus { get; set; }
        public byte nStatusPrior { get; set; }
        public byte[] nExtFeatureData { get; set; } = default!;
        public byte[] nExtFeatureStatus { get; set; } = default!;
    }

    public class TypeWebActivityDto
    {
        public byte iType { get; set; }
        public byte iCurUserId { get; set; }
        public byte iObjectUserId { get; set; }
        public char[] szObjectUser { get; set; } = default!;
        public int ipAddress { get; set; }
    }

    public class TypeOperatingModeDto
    {
        public byte prev_oper { get; set; }
    }

    public class TypeOALDto
    {
        public byte nReasonCode { get; set; }
        public byte[] nData { get; set; } = default!;
    }

    public class TypeCoSFloorDto
    {
        public byte prevFloorStatus { get; set; }
        public byte floorNumber { get; set; }
    }

    public class TypeFileDownloadStatusDto
    {
        public byte fileType { get; set; }
        public char[] fileName { get; set; } = default!;
    }

    public class TypeBatchReportDto
    {
        public object triggerNumber { get; set; } = default!;
        public object activationCount { get; set; } = default!;
        public byte sourceType { get; set; }
        public object sourceNumber { get; set; } = default!;
        public byte tranType { get; set; }
        public byte[] tranCodeMap { get; set; } = default!;
    }

    public class TypeCoSElevatorAccessDto
    {
        public long cardholder_id { get; set; }
        public byte[] floors { get; set; } = default!;
        public byte nCardFormat { get; set; }
    }

    public class SCPReplyTransactionHeaderDto
    {
        public int ser_num { get; set; }
        public int time { get; set; }
        public short source_type { get; set; }
        public short source_number { get; set; }
        public short tran_type { get; set; }
        public short tran_code { get; set; }
    }

    public class SCPReplyTransactionDto
    {
        public int ser_num { get; set; }
        public int time { get; set; }
        public short source_type { get; set; }
        public short source_number { get; set; }
        public short tran_type { get; set; }
        public short tran_code { get; set; }
        public TypeSysDto sys { get; set; } = default!;
        public TypeSysCommDto sys_comm { get; set; } = default!;
        public TypeSioCommDto s_comm { get; set; } = default!;
        public TypeCardBinDto c_bin { get; set; } = default!;
        public TypeCardBcdDto c_bcd { get; set; } = default!;
        public TypeCardFullDto c_full { get; set; } = default!;
        public TypeCardIDDto c_id { get; set; } = default!;
        public TypeDblCardFullDto c_fulldbl { get; set; } = default!;
        public TypeDblCardIDDto c_iddbl { get; set; } = default!;
        public TypeI64CardFullDto c_fulli64 { get; set; } = default!;
        public TypeI64CardFullIc32Dto c_fulli64i32 { get; set; } = default!;
        public TypeHostCardFullPinDto c_fullHostPin { get; set; } = default!;
        public TypeI64CardIDDto c_idi64 { get; set; } = default!;
        public TypeCoSDto cos { get; set; } = default!;
        public TypeREXDto rex { get; set; } = default!;
        public TypeCoSDoorDto door { get; set; } = default!;
        public TypeProcedureDto proc { get; set; } = default!;
        public TypeUserCmndDto usrcmd { get; set; } = default!;
        public TypeActivateDto act { get; set; } = default!;
        public TypeAcrDto acr { get; set; } = default!;
        public TypeMPGDto mpg { get; set; } = default!;
        public TypeOALDto oal { get; set; } = default!;
        public TypeAreaDto area { get; set; } = default!;
        public TypeUseLimitDto c_uselimit { get; set; } = default!;
        public TypeAsciDto t_diag { get; set; } = default!;
        public TypeSioDiagDto s_diag { get; set; } = default!;
        public TypeAcrExtFeatureStlsDto extfeat_stls { get; set; } = default!;
        public TypeAcrExtFeatureCoSDto extfeat_cos { get; set; } = default!;
        public TypeWebActivityDto web_activity { get; set; } = default!;
        public TypeOperatingModeDto oper_mode { get; set; } = default!;
        public TypeCoSFloorDto floor { get; set; } = default!;
        public TypeFileDownloadStatusDto file_download { get; set; } = default!;
        public TypeCoSElevatorAccessDto elev_access { get; set; } = default!;
        public TypeBatchReportDto batch_report { get; set; } = default!;
    }

    public class SCPReplyDualPortDto
    {
        public short number { get; set; }
        public short stat_this { get; set; }
        public short stat_primary { get; set; }
        public short stat_alternate { get; set; }
    }

    public class SCPReplyBioAddResultDto
    {
        public short nBioType { get; set; }
        public short nResult { get; set; }
        public long nCardId { get; set; }
        public int nCommandTag { get; set; }
    }

    public class SCPReplyLoginInfoDto
    {
        public char[] name { get; set; } = default!;
        public char[] notes { get; set; } = default!;
        public short acctType { get; set; }
        public short userType { get; set; }
        public short userId { get; set; }
    }

    public class SCPReplyPkgInfoDto
    {
        public char[] pkgName { get; set; } = default!;
        public char[] pkgVersion { get; set; } = default!;
        public long installDate { get; set; }
    }

    public class SCPReplyFileInfoDto
    {
        public short file_type { get; set; }
        public short file_index { get; set; }
        public short num_files { get; set; }
        public char[] fileName { get; set; } = default!;
    }

    public class SCPReplyCertInfoDto
    {
        public char[] issuedTo { get; set; } = default!;
        public char[] issuedBy { get; set; } = default!;
        public char[] issuedStart { get; set; } = default!;
        public char[] issuedExpire { get; set; } = default!;
    }

    public class SCPReplyElevRelayInfoDto
    {
        public short acr_number { get; set; }
        public byte[] status { get; set; } = default!;
    }

    public class SCPReplyOsdpPassthroughDto
    {
        public short acr_number { get; set; }
        public int sequence_num { get; set; }
        public short reader_role { get; set; }
        public short msg_type { get; set; }
        public short data_len { get; set; }
        public byte[] data { get; set; } = default!;
    }

    public class SCPReplySioRelayCountsDto
    {
        public short sio_number { get; set; }
        public short num_relays { get; set; }
        public int[] data { get; set; } = default!;
    }

    public class SCPReplySioHidMfgInfoDto
    {
        public short sio_number { get; set; }
        public byte[] serial_no { get; set; } = default!;
        public byte[] uuid { get; set; } = default!;
    }

    public class SCPReplySrMsp1DrvrDto
    {
        public short number { get; set; }
        public short port { get; set; }
        public short mode { get; set; }
        public int baud_rate { get; set; }
        public short throughput { get; set; }
    }

    public class SCPReplySrSioDto
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

    public class SCPReplySrMpDto
    {
        public short first { get; set; }
        public short count { get; set; }
        public short[] status { get; set; } = default!;
    }

    public class SCPReplySrCpDto
    {
        public short first { get; set; }
        public short count { get; set; }
        public short[] status { get; set; } = default!;
    }

    public class SCPReplySrAcrDto
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

    public class SCPReplySrTzDto
    {
        public short first { get; set; }
        public short count { get; set; }
        public short[] status { get; set; } = default!;
    }

    public class SCPReplySrTvDto
    {
        public short first { get; set; }
        public short count { get; set; }
        public short[] status { get; set; } = default!;
    }

    public class SCPReplySrMpgDto
    {
        public short number { get; set; }
        public short mask_count { get; set; }
        public short num_active { get; set; }
        public short[] active_mp_list { get; set; } = default!;
    }

    public class SCPReplySrAreaDto
    {
        public short number { get; set; }
        public short flags { get; set; }
        public int occupancy { get; set; }
        public int occ_spc { get; set; }
    }

    public class SCPReplyCmndStatusDto
    {
        public short status { get; set; }
        public int sequence_number { get; set; }
        public SCPReplyNAKDto nak { get; set; } = default!;
    }

    public class SCPReplyCmndStatusExtDto
    {
        public short status { get; set; }
        public int lSequenceFirst { get; set; }
        public int lSequenceLast { get; set; }
        public SCPReplyNAKDto nak { get; set; } = default!;
    }

    public class SCPReplyMemReadDto
    {
        public short nType { get; set; }
        public int nBase { get; set; }
        public short nSize { get; set; }
        public byte[] nData { get; set; } = default!;
    }

    public class SanbxAppDto
    {
        public int appCode { get; set; }
        public byte[] version { get; set; } = default!;
        public short state { get; set; }
    }

    public class SCPReplySanbxAppListDto
    {
        public short nApps { get; set; }
        public object[] apps { get; set; } = default!;
    }

    public class StrSpecDto
    {
        public short nStrType { get; set; }
        public int nRecords { get; set; }
        public int nRecSize { get; set; }
        public int nActive { get; set; }
    }

    public class SCPReplyStrStatusDto
    {
        public short nListLength { get; set; }
        public StrSpecDto[] sStrSpec { get; set; } = default!;
    }



    public class sior_idrDto
    {
        public short nModel { get; set; }
        public short nRevision { get; set; }
        public int nSerNum { get; set; }
        public short nRxbLen { get; set; }
    }

    public class sior_diagDto
    {
        public short sw { get; set; }
        public short nInputs { get; set; }
        public byte[] cA2D { get; set; } = default!;
    }

    public class sior_lsrDto
    {
        public byte cCtSts { get; set; }
        public byte cPwrSts { get; set; }
    }

    public class sior_isrDto
    {
        public short nInputs { get; set; }
        public byte[] cIpSts { get; set; } = default!;
    }

    public class sior_rtsrDto
    {
        public short nReaders { get; set; }
        public byte[] cRdrTmpr { get; set; } = default!;
    }

    public class sior_mr50srDto
    {
        public byte cCtSts { get; set; }
        public byte cRdrTmpr { get; set; }
        public byte cIpSts0 { get; set; }
        public byte cIpSts1 { get; set; }
        public byte cIpSts2 { get; set; }
        public byte[] dummy { get; set; } = default!;
    }

    public class sior_cdbDto
    {
        public short nReader { get; set; }
        public short nBitCount { get; set; }
        public byte[] cData { get; set; } = default!;
    }

    public class sior_cddDto
    {
        public short nReader { get; set; }
        public short nReadDirection { get; set; }
        public short nDigitCount { get; set; }
        public byte[] cData { get; set; } = default!;
    }

    public class sior_keyDto
    {
        public short nReader { get; set; }
        public short nKeyCount { get; set; }
        public byte[] cKeys { get; set; } = default!;
    }

    public class sior_idrxDto
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

    public class sior_osrDto
    {
        public short nOutputs { get; set; }
        public byte[] cOpSts { get; set; } = default!;
    }

    public class sioc_aperio_actstateDto
    {
        public byte nReader { get; set; }
        public byte door_side { get; set; }
        public byte state { get; set; }
        public byte id { get; set; }
    }

    public class sioc_aperio_doormodeDto
    {
        public byte nReader { get; set; }
        public byte mode { get; set; }
    }

    public class SCPReplySioReplyDto
    {
        public short nSioReply { get; set; }
        public sior_idrDto idr { get; set; } = default!;
        public sior_diagDto diag { get; set; } = default!;
        public sior_lsrDto lsr { get; set; } = default!;
        public sior_isrDto isr { get; set; } = default!;
        public sior_rtsrDto rtsr { get; set; } = default!;
        public sior_mr50srDto mr50sr { get; set; } = default!;
        public sior_cdbDto cdb { get; set; } = default!;
        public sior_cddDto cdd { get; set; } = default!;
        public sior_keyDto key { get; set; } = default!;
        public sior_idrxDto xidr { get; set; } = default!;
        public sior_osrDto osr { get; set; } = default!;
        public sioc_aperio_actstateDto actstate { get; set; } = default!;
        public sioc_aperio_doormodeDto doormode { get; set; } = default!;
    }

    public class SCPReplySioHeaderDto
    {
        public short nSioReply { get; set; }
        public byte[] dummy { get; set; } = default!;
    }

    public class CC_WEB_CONFIG_NETWORKDto 
    {
    
        public short scp_number  { get; set; }

      public short method  {get; set;}
      public int cIpAddr  {get; set;}

      public int cSubnetMask  {get; set;}

      public int cDfltGateway  {get; set;}

      public char[] cHostName {get; set;} = default!;

      public short dnsType  {get; set;}

      public int cDns  {get; set;}

      public char[] cDnsSuffix  {get; set;} = default!;

      public short method2 {get; set;}
      public int cIpAddr2  {get; set;}

      public int cSubnetMask2 {get; set;}

      public int cDfltGateway2  {get; set;}

      public int cDns2  {get; set;}

      public short TnlEnable  {get; set;}

      public int cIpTnl  {get; set;}

      public int cPortTnl  {get; set;}
    }

    public class CC_WEB_CONFIG_HOST_COMM_PRIMDto
    {
         public short scp_number { get; set; }

      public short address { get; set; }

      public short dataSecurity { get; set; }

      public short cType { get; set; }

      public HostCommIpServerDto ipserver { get; set; } = new HostCommIpServerDto();

      public HostCommIpClientDto ipclient { get; set; } = new HostCommIpClientDto();
    }

    public class HostCommIpServerDto
    {
        public int cAuthIP1 {get; set;}

      public int cAuthIP2 {get; set;}

      public short nPort {get; set;}

      public short enableAuthIP {get; set;}

      public short nNicSel {get; set;}
    }

    public class HostCommIpClientDto
    {
          public int cHostIP { get; set;}

      public short nPort  { get; set;}

      public short rqIntvl  { get; set;}

      public short connMode  { get; set;}

      public char[] cHostName  { get; set;} = default!;

      public short nNicSel  { get; set;}
    }
}

