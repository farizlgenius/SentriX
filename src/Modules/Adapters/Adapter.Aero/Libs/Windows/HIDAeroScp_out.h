/*************************************************************************************
* Copyright © 2019 HID Global Corporation.  All rights reserved.
* This software is protected by copyright law and international treaties, as well as any nondisclosure agreements that you or the organization you represent have signed.  
* Any unauthorized reproduction, distribution or use of the software is prohibited.
***********************************************************************************/

#ifndef SCP_OUT_H
#define SCP_OUT_H
#include "Scp_lim.h"


// make this VB compatible...
#pragma pack (4)

#ifndef IntVB
#define IntVB short
#endif
/* Configuration Command Codes - first field of command string */
enum enCfgCmnd
{	enCcNop = 0,			// nop

// CONTROL group
	enCcBatch,				// 001 Batch command - token is file path/name

// INITIALIZATION group
	enCcSystem = 11,					//  011 System configuration (once at startup)
	enCcCreateChannel,					//  012 create channel
	enCcCreateScp,						//  013 Create SCP 
	enCcCreateScpLn=1013,				// 1013 Create SCP 
	enCcDeleteChannel=enCcCreateScp+1,	//  014 Delete channel
	enCcDeleteScp,						//  015 Delete SCP
	SPARE_016,
	enCcPeerCertfificate,				//  017 Peer Certificate configuration
	enCcIpClientDefaults,				//  018 IP Client Default settings
	enCcIpClientIDAssignment,			//  019 IP Client ID Assignment

// CONFIGURATION group
	// Commands 101-105 apply to SCPs configured with Command 107
	// Commands 101-105 can be issued without any regard to SCP's created/present
	SPARE_101 = 101,
	SPARE_102,
	SPARE_103,
	SPARE_104,
	SPARE_105,
	// Commands 1101-1105 are the SCP specific version of commands 101 through 105
	// Commands 1101 through 1105 may be issued to SCP's that have been configured
	// with Command 1107. (They may NOT be issued to SCPs configured with Command 107!)
	enCcScpIcvt = 1101,		// 1101 Input scanner conversion table configuration
	enCcScpCfmt,			// 1102 Card formatter configuration
	enCcScpTimezone,		// 1103 Time zone configuration
	enCcScpHoliday,			// 1104 Holiday configuration
	enCcScpAdbSpec,			// 1105 Access database specification
	enCcScpScp = 1107,		// 1107 SCP configuration (this may be used in place of 107)
	enCcScpDaylight = 1116,	// 1116 Daylight Start/Stop date/time for this SCP
	SPARE_2103 = 2103,
	enCcScpTimezoneExAct = 3103, // 3103 Extended time zone configuration Act and Deact times
	// resume with the configuration commands
	SPARE_106 = 106,
	SPARE_107 = 107,
	enCcMsp1=108,			//  108 MSP1 (SIO) comm driver configuration
	enCcSio,				//  109 SIO panel configuration
	enCcInput,				//  110 Input point configuration
	enCcOutput,				//  111 Output point configuration
	enCcReader,				//  112 Card reader configuration
	enCcMP,					//  113 Monitor point configuration
	enCcCP,					//  114 Control point configuration
	enCcACR,				//  115 Access control reader configuration
	enCcAlvl = 116,				//  116 Access level configuration
	enCcAlvlEx = 2116,		// 2116 Access level configuration extended
	enCcTrgr= 117,				//  117 Trigger configuration
	enCcTrgr128= enCcTrgr+1000,	//  1117 Trigger configuration extended
	enCcProc=118,			//  118 Procedure/Action configuration
	enCcActnRem,			//  119 Removes spec'd Action from range of Proc's
	enCcMpg,				//  120 Configure a Monitor Point Group

    SPARE_2220 = 2220,
    SPARE_2221 = 2221,
    SPARE_2222 = 2222,
    SPARE_2223 = 2223,

	enCcScpLogin=2250,		// 2250	Configure a Web Service login.

	SPARE_2251 = 2251,

	enCcScpLoginUsers,		// 2252	Read back the data for a particular user
	enCcScpLoginHTS,		// 2253 HTS Broker Setup

	SPARE_121=121,
	enCcAccException = 1220,// 1220 Access Exception List
	enCcAreaSpc=1121,		// 1121 Access Control Area configuration0
	enCcRledSpc=122,		//  122 Reader Led/Buzzer Function specs
	enCcRTxtSpc,			//  123 LCD Text config and load
	enCcAlvlSpc,			//  124 Access Level Spec command
	enCcSioAesControl=127,	//	127 SIO Encryption Control

	SPARE_125=125,
	SPARE_129=129,
	SPARE_130=130,
//
enCcSioNetwork = 128,			//	128 SIO Network configuration.
enCcAcrLogDeny = 151,			// 151 Configure Logging based on deny count
//
	SPARE_1123=1123,
	SPARE_1124=1124,
	SPARE_3124=3124,
	SPARE_1125=1125,
	SPARE_1126=1126,
	SPARE_1131=1131,
	SPARE_1132=1132,
	SPARE_3132=3132,
	SPARE_3133=3133,
//
//
	enCcScpUCmnd=1141,							// 1141 User Command Configuration
	SPARE_1142=1142,
	SPARE_1143=1143,
	enCcScpUCmndBkgd,							// 1144 User Command Acr Specific Backgound Text config
//
// LOCAL COMMAND group	(obsolete commands 201,202, 204, and 205 removed)
	enScpDown = 203,							// 203 shut down all MSP2 drivers, close ports
	enCcFirmwareDown = 206,						// 206 firmware download command
	enCcAttachScp,								// 207 attach SCP to channel
	enCcDetachScp,								// 208 detach SCP from channel
	enCcConfigSave,								// 209 save the configuration settings
	enCcConfigDelta,							// 210 set the configuration "delta"-file's name
	enCcDualPortControl = 211,					// 211 dual port control command
	enCcAesControl,								// 212 AES Encryption Control Command

	SPARE_213 = 213,

	enCcPollMode = 214,							// 214 Set the "Demand Poll" mode for this SCP
	enCcFileDownload = 215,						// 215 Generic file download
												// 216 - Reserved
	enCcHexOutInternal = 217,					// 217 Internal Hex Out (factory use only)

	enCcDeleteFile     = 218,					// 218 Delete File
	enCcGetFileInfo    = 219,					// 219 Return file names of particular file type
	enCcAppDownload = 220,						// 220 App download command	

// DIRECT COMMAND group
	enCcReset = 301,							//  301 reset the SCP (complete reboot)
	enCcTime,									//  302 set the time (to the current system time)
	enCcTranIndex,								//  303 set the transaction index
	enCcAdbCardI64DTic32=5304,					// 5304 Extended Cardholder Record: enCcAdbCardI64DT with 32-bit issue code
	enCcCardDeleteI64=3305,						// 3305 delete, card_number as Int64
	enCcAdbCardI64DTic32A255FF=8304,			// 8304 Extended Cardholder Record: enCcAdbCardI64DT with 32-bit i/c, 32 access levels, freeform fields
	SPARE_304  =  304,
	SPARE_305  =  305,
	SPARE_1304 = 1304,
	SPARE_2304 = 2304,
	SPARE_3304 = 3304,
	SPARE_4304 = 4304,
	SPARE_6304 = 6304,
	SPARE_7304 = 7304,
	SPARE_2305 = 2305,
	enCcMpMask=306,								//  306 Monitor Point mask control
	enCcCpCtl,									//  307 Control Point control command
	enCcAcrMode,								//  308 Access Control Reader mode set command
	enCcForcedOpenMask,							//  309 Forced Open mask control
	enCcHeldOpenMask,							//  310 Held Open mask control
	enCcUnlock,									//  311 momentary unlock
	enCcProcedure,								//  312 Procedure control
	enCcTVCommand,								//  313 Trigger variable control command
	enCcTzCommand,								//  314 Timezone control command
	enCcAcrLedMode,								//  315 Reader LED mode control command
	enCcOemCode,								//  316 OEM code: upper 12 bits of the reported SCP serial number
	enCcPassword,								//  317 Sets the SCP's comm password
	enCcScpID,									//  318 Sets ID which is reported by SCP in the id report
    SPARE_319,
	SPARE_2319=2319,
	enCcApbFreePassI64=3319,					//  3319 issue a free pass to a) all, or b) individual card holder
	enCcHexOut=320,								//  320 hex output on SCP's SIO port (factory use only)
	enCcMpgSet,									//  321 arm/disarm monitor point group
	enCcAreaSet,								//  322 access control area command
	SPARE_323,		  
	SPARE_2323 = 2323,
	enCcUseLimitI64=3323,						// 3323 set use limit, card_number as Int64
	enCcScpOffLineTime=324,						//  324 set the off-line time (hang-up soon)
	enCcRLedTemp,								//	325 temp reader LED command
	enCcLcdText,								//	326 text output to an LCD terminal
	enCcSioDc,									//  327 direct command (Diagnostic Use)
	enCcSioHexLoad,								//  328 hex download
	enCcHostResponse,							//  329 Host Response
	enCcNvArgSet,								//  330 Set Non-Volatile Arguments - special config command
	enCcCardSim,								//  331 simulated card read
	SPARE_332 = 332,
	enCcDiag,									//  333 Send diagnostic commands to SCP
	enCcTempAcrMode,							//  334 Set temporary ACR mode
	enCcOperatingMode,							//  335 Set operating mode (threat level)
	SPARE_336 = 336,
	enCcAcrOsdpPassthrough,						//  337 ACR OSDP Passthrough
	enCcAcrOfflineAccessList,					//	338 ACR Offline Access List

	enCcKeySim,									//  339 simulated keys
	enCcOsdpReaderTransfer,						//  340 OSDP reader file transfer
	enCcControlReboot,							//  341 Control Reboot
	enCcCardSimRaw,								//  342 simulated card read raw
	enCcCardCleanup,                            //  343 allow deactivated cards to be deleted from the database

	// sanbx commands
	enCcSanbxAppCmd = 344,						//  344 Sanbx Cmds - Send command to sanbx

// STATUS REQUEST commands
	enCcIDRequest = 401,						// 401 SCP id request
	enCcTranSrq,								// 402 Transaction Log status request
	enCcMsp1Srq,								// 403 SIO comm driver status request
	enCcSioSrq,									// 404 SIO status request
	enCcMpSrq,									// 405 Monitor Point status request
	enCcCpSrq,									// 406 Control Point status request
	enCcAcrSrq,									// 407 Access Control Reader status request
	enCcTzSrq,									// 408 Timezone status request
	enCcTvSrq,									// 409 Trigger Variable status request
	SPARE_410 = 410,
	enCcMpgSrq,									// 411 Monitor Point Group Status request
	enCcAreaSrq,								// 412 Access Control Area Status request
	SPARE_413 = 413,
	SPARE_414 = 414,
	enCcSioRelayCtSrq,							// 415 SIO Relay Count status request
	enCcElevRelayInfo,							// 416 Elevator Relay status request
	enCcSioHidMfgSrq,							// 417 SIO HID Serial No and UUID request
//
#ifdef SIO_COM_H
// Direct SIO Support (used only on devices connected on serial comm with CC_NEWSCP::nCommAccess set to 9)
	enCcSioCommand = 601,						// 601 Applies only to direct SIO comm
#endif
// configuration read commands  (C_090A through C091F)  CONFIG_READ <<<
// configuration read commands     CONFIG_READ >>>
	SPARE_1701 = 1701,
	SPARE_1702 = 1702,
	enCcRdAdbSpec = 1703,						//   1703 Read the Access database specification        
	SPARE_1704 = 1704,
	SPARE_1705 = 1705,
	SPARE_1706 = 1706,
	enCcRdTimezone,								//   1707 Read the Time zone configuration
	enCcRdHoliday,								//   1708 Read the Holiday configuration
	enCcRdAdbCard,								//   1709 Read this cardholder's record
	enCcRdAccExcpt,								//   1710 Read this cardholder's Access Exception List

	SPARE_1801 = 1801,
	SPARE_1802 = 1802,
	SPARE_1803 = 1803,
	SPARE_1804 = 1804,
	SPARE_1805 = 1805,

	enCcRdMP=1811,								//   1811 Read the Monitor point configuration
	enCcRdCP,									//   1812 Read the Control point configuration
	enCcRdACR,									//   1813 Read the Access control reader configuration
	enCcRdAlvl,									//   1814 Read the Access level configuration
	enCcRdTrgr,									//   1815 Read the Trigger configuration

	SPARE_1816  = 1816,
	SPARE_1817  = 1817,
	SPARE_1818  = 1818,
	SPARE_1819  = 1819,
	SPARE_1854  = 1854,
	SPARE_1855  = 1855,
	enCcBatchTrans = 1820,						//  Batch Transactions
	enCcMemRead  = 1851,						//   1851 Diagnostic Function: Memory Read Command
	enCcRMS      = 1852,						//   1852 Remote Memory Storage
	enCcStrSRq   = 1853,						//   1853 SCP Structure Status Read Request
	enCcCertInfo = 1856,						//	 1856 Read installed certificate information

// sanbx commands
	enCcSanbxListApps = 1857,					// 1857 Sanbx Cmds - List Apps 	
// configuration read commands     CONFIG_READ <<<
//

	enCcWebConfigNotes = 901,					// 901 Web Config - Notes
	enCcWebConfigNetwork,						// 902 Web Config - Network
	enCcWebConfigHostCommPrim,					// 903 Web Config - Host Comm Primary
	SPARE_904 = 904,
	enCcWebConfigSessionTmr,					// 905 Web Config - Session Timer
	enCcWebConfigWebConn,						// 906 Web Config - Web Connection
	enCcWebConfigAutoSave,						// 907 Web Config - Auto Save
	enCcWebConfigNetworkDiag,					// 908 Web Config - Network Diag
	enCcWebConfigTimeServer,					// 909 Web Config - Time Server
	SPARE_910 = 910,
	SPARE_911 = 911,
	enCcWebConfigDiagnostics,					// 912 Web Config - Diagnostics
	enCcWebConfigApplyReboot,					// 913 Web Config - Apply/Reboot

	enCcWebConfigRead = 900,					// 900 Web Config - Read

	enCcMax
};

typedef struct				/* (C_001) Batch command specification */
{	char cfilename[256];		/* full file path/name of the command file */
} CC_BATCH;

typedef struct				// (C_011) System level specification
{	IntVB nPorts;				// max number of Channels that may be created
	IntVB nScps;				// max number of SCPs that may be created */
	IntVB nTimezones;			// max number of global timezones */
	IntVB nHolidays;			// max number of global holidays */
	IntVB bDirectMode;			// non-zero selects direct transmission of config commands
								// (affects: 1101 through 1105, 1107, 108 through 121)
	IntVB debug_rq;				// - debug dump request: 1=StrChnl, 2=StrScpDev, 3=Set Poll/Ack logging
	IntVB nDebugArg[4];			// -  if (debug_rq==1) then nDebugArg[0]==nChnlId;
								// -  if (debug_rq==2) then nDebugArg[0]==nScpId;
								// -  if (debug_rq==3) then nDeubgArg[0]==Enable(1)/Disable(0) flag
} CC_SYS;

typedef struct				// (C_012, C_014) System level specification
{	IntVB nChannelId;			// channel number to create
	IntVB cType;				// 4 = TCPIP connect to remote
                                // 7 = Concurrent multiple inbound connections from remote devices
								// Add 256 for TLS; add 512 for TLS w/ certificate authentication.
	IntVB cPort;				// for cType == 5 or == 7: Tcp/Ip PORT number the remote will try to connect to
	long  baud_rate;			// not used, set to 0
	long  timer1;				// SCP reply timeout, in milliseconds
	long  timer2;				// tcp/ip retry connect interval (used only if cType == 4)
	char  cModemId[64];			// not used, set to 0
	IntVB cRTSMode;				// not used, set to 0
} CC_CHANNEL;

typedef struct				// (C_015) Delete SCP
{	IntVB nSCPId;					// SCP number
	IntVB address;					// address
	IntVB nCommAccess;				// Not used, set to 0
	IntVB e_max;					// Not used, set to 0
	long  poll_delay;				// Not used, set to 0
	char  cCommString[32];			// Not used, set to NULL
	char  cPswdString[16];			// Not used, set to NULL
	long offline_time;				// Not used, set to 0
	short nAltPortEnable;			// Not used, set to 0
	IntVB nAltPortCommAccess;		// Not used, set to 0
	IntVB nAltPortE_max;			// Not used, set to 0
	long  nAltPortPoll_delay;		// Not used, set to 0
	char  cAltPortCommString[32];	// Not used, set to 0
} CC_NEWSCP;

// Use this structure to define IP "host names" up to 128 bytes in length (CC_NEWSCP::CommString[] is only 32 bytes long)
// other than this difference, the two commands are identical
typedef struct				// (C_1013) SCP: System level specification
{	IntVB nSCPId;					// SCP number
	IntVB address;					// address
	IntVB nCommAccess;				// type of communications on this SCP (coded per CC_CHANNEL::cType)
									// (add 256 the channel type value to reqest auto disable transaction reporting
									// app must use command C_303 to enable transaction reporting after SCP on-line)
	IntVB e_max;					// retry count before offline
	long  poll_delay;				// minimum time between "inactive" polls
	char  cCommString[128];			// winsock name
	char  cPswdString[16];			// logon password
	long offline_time;				// number of ms between messages before SCP goes "off-line"
									// set to zero to get the default(==15000) (2000 to 65000 ok)
	short nAltPortEnable;			// Set to 0
	IntVB nAltPortCommAccess;		// Set to 0
	IntVB nAltPortE_max;			// Set to 0
	long  nAltPortPoll_delay;		// Set to 0
	char  cAltPortCommString[128];	// Set to NULL
} CC_NEWSCP_LN;


typedef struct				// (C_01116) daylight savings time configuration for this SCP
{								// set nDstID to 100 to enable the use of this table
								// an SCP can hold up to 20 date pairs
	IntVB nScpID;				// SCP number
	IntVB nSYear;				// year, e.g.: 1994
	IntVB nSMonth;				// month, 1-12
	IntVB nSDay;				// day-of-month, 1-31
	IntVB nSHh;					// hours 0-23
	IntVB nSMm;					// minutes 0-59
	IntVB nSSs;					// seconds 0-59
	IntVB nEYear;				// year, e.g.: 1994
	IntVB nEMonth;				// month, 1-12
	IntVB nEDay;				// day-of-month, 1-31
	IntVB nEHh;					// hours 0-23
	IntVB nEMm;					// minutes 0-59
	IntVB nESs;					// seconds 0-59
} CC_SCP_DAYLIGHT;

typedef struct				// (C_017) peer certificate configuration
{	
	IntVB nEnable;				// 1=Enable Peer Certificate, 0=Disable Peer Certificate
	char  cIssuer[100];			// Issuer Name of Peer Certificate
	
} CC_PEERCERT;

typedef struct				// (C_018) IP Client Default settings
{	
	IntVB e_max;			// Retry count before offline
	IntVB poll_delay;		// minimum time between "inactive" polls
	IntVB offline_time;		// number of ms between messages before SCP goes "off-line"
	IntVB provisioning_time;// time the host needs to respond with SCP device specification to keep controller online
} CC_IP_CLIENT_DEFAULTS;

typedef struct				// (C_019) IP Client ID Assignment
{	
#define IP_CLIENT_ASSIGN_FLAG_DECREMENT		0x0001
#define IP_CLIENT_ASSIGN_FLAG_NO_REUSE		0x0002
#define IP_CLIENT_ASSIGN_FLAG_NO_HWID		0x0004
	long assignmentFlags;	// bit-0 = increment/decrement, bit-1 = reuse/no reuse, bit-2 = use hw id/dont use hw id
	IntVB lowScpID;			// Low SCP ID to start from
	IntVB highScpID;		// High SCP ID to start from
} CC_IP_CLIENT_ID_ASSIGN;


#define CFMT_F_NULL	0			/* - no formatting */
#define CFMT_F_WGND	1			/* - Sensor bit array to access card */
#define CFMT_F_MT2	2			/* - mag T2 nibble array to access card */
#define CFMT_F_MTA	3			/* - MTA card: like Sensor, index from first 1, bit count is min req'd */


/* use the structure that applies to the selected function */
typedef union _SCP_CARD_FORMAT
{	struct					/* arguments if function == CFMT_F_WGND or _F_MTA */
	{	IntVB flags;		/* - control flags: Bit-0 set to step parity calculation by 2 bits */
							// Bit-1 set to suppress facility code checking
							// Bit-2 set to select the "Corporate Card" mode:
							//       if set: compute the cardholder id offset by multiplying
							//       the facility_code with the value in NCC_SCP_CFMT::offset
							// Bit-3 set to enable 37-bit Parity Test with 4 Parity Bits
							// Bit-4 - obsolete - was "enable Motorola 64-bit BiStatic parity format"
							// Bit-5 set to enable 37-bit Parity Test with 2 Parity Bits in middle of card
							// Bit-6 set to format a 48-bit card number as:
							//       (bits 34 through 47)*(10**10)+(bits 20 through 33)*(10**6)+(bits 0 through 19)
							// Bit-7 set to enable "Special Card Formats", in which every resultant transaction code
							//       will by offset by 64.
							// Bit-8 set to enable "Reverse" card format.  The entire bit stream will be reversed
							//       before processing.
							// Bit-9 set to indicate card field is to be interpreted as a Large Encoded ID, and not as the actual
							//       cardholder identifier
                            // Bit-10 set to enable Reversing of BYTE order.  Only respected if bits are a multiple of 8 (whole bytes)
							// Bit-11 set to enable special 37-bit parity check
							// Bit-12 set to convert 200-bit FASCN to 128-bit version
							// Bit-13 set to require matching card number in database to use this format
							// Bit-13 set to require matching card number in database to use this format
							// Bit-14 set to enable corporate 1000 parity check (35 or 48 bits)
		IntVB bits;			/* - number of bits on the card, if _F_MTA than min bits req'd */
		IntVB pe_ln;		/* - number of bits to sum for even parity */
		IntVB pe_loc;		/* - ...bit address to start from (_MTA: offset from first '1') */
		IntVB po_ln;		/* - number of bits to sum for odd parity */
		IntVB po_loc;		/* - ...bit address to start from (_MTA: offset from first '1') */
		IntVB fc_ln;		/* - number of facility code bits */
		IntVB fc_loc;		/* - ...bit address to start from (ms bit), (_MTA: offset from first '1') */
		IntVB ch_ln;		/* - number of cardholder ID bits */
		IntVB ch_loc;		/* - ...bit address to start from (ms bit), (_MTA: offset from first '1') */
		IntVB ic_ln;		/* - number of issue code bits */
		IntVB ic_loc;		/* - ...bit address to start from (ms bit), (_MTA: offset from first '1') */
	} sensor;
	struct					/* arguments if function == CFMT_F_MT2 */
	{	IntVB flags; 		/* - control flags (0) */
							// Bit-1 set to suppress facility code checking
		IntVB min_digits;	/* - minimum number of digits on the card */
		IntVB max_digits;	/* - maximum number of digits on the card */
		IntVB fc_ln;		/* - number of facility code digits */
		IntVB fc_loc;		/* - index to the most significant digit */
		IntVB ch_ln;		/* - number of cardholder ID digits */
		IntVB ch_loc;		/* - index to the most significant digit */
		IntVB ic_ln;		/* - number of issue code digits */
		IntVB ic_loc;		/* - index to the digit */
	} mt2_acs;
} SCP_CARD_FORMAT;						/* format function specifier argument block */



typedef struct				// (C_105) Access Database Spec
{	long  lastModified;			// update tag
	long  nCards;				// number of cardholder records to allocate 
	IntVB nAlvl;				// number of access levels per cardholder
	IntVB nPinDigits;			// the lower nibble holds the number of PIN digits to store (0 to 15), AND 
								// the upper nibble holds the CardIdSize in addition to the default size:
								// ...(CardIdSizeInBytes-4)   ---- the CardholderId may be 4 to 8 bytes in size
								// --- example: 5-digit PIN, 48-bit card = ((6-4)<<4) + 5 = 0x25 = 37
	IntVB bIssueCode;			// store issue code: 0 = do not store, 1 = standard 8-bit
	IntVB bApbLocation;			// store anti-passback location
	IntVB bActDate;				// store activation date ==1, or date & time == 2
	IntVB bDeactDate;			// store deactivation date ==1, date & time == 2
	IntVB bVacationDate;		// store vacation date and duration (temporary de-activate)
	IntVB bUpgradeDate;			// store temporary access level upgrade date and duration
	IntVB bUserLevel;			// the number of user_level entries to store (0 to 7 valid)
	IntVB bUseLimit;			// store use limits
	IntVB bSupportTimedApb;		// save the time and the Acr number of last entry
	IntVB nTz;					// precision access: number of ACR's to save Tz entries for
	IntVB bAssetGroup;			// store asset group code: 2-bytes
	IntVB nHostResponseTimeout;	// time to wait for HOST Approved Access Response
								//  -- 1 second increments, 0==default==5 seconds
	IntVB nMxmTypeIndex;		// index to the user level entry that holds the MxM User Type
								// Mxm User Types: 0 = standard ("other"), 1 = "Supervisor/Service", 2 = "team member"
	IntVB nAlvlUse4Arq;			// if zero then use all (default) else,
								// if non-zero, this holds the number of access levlels to use for access request processing
								// -- this setting does not impact the card record size
								// example: if nAlvl == 32 and nAlvlUse4Arq == 22, then the access request processor will 
								//          check only the first 22 access levels. 
								//         (Note: the rest of the access levels are available for extended user command authority spec)
	IntVB nFreeformBlockSize;	// Size of the Freeform block.  If zero, then no block is created.
	struct {
		IntVB	nFieldType;		// Type of data in the field
		IntVB	nFieldSize;		// Size of the data field
	} fldsFreeform[MAX_FREEFORM_FIELDS];
	IntVB nEscortTimeout;		// time to wait in between cards in escort processing (1 second increments, 0==default==15 seconds)
	IntVB nMultiCardTimeout;	// time to wait in between cards in multi card processing (1 second increments, 0==default==15 seconds)
	IntVB nAssetTimeout;		// time to wait in between cards in asset processing (1 second increments, 0==default==10 seconds)
	IntVB bAccExceptionList;	// enable access exception list ((3 bytes))
	long adbFlags;				// access database flags
} CC_ADBS;
// --- NOTE: refer to the comment block starting with "// Guide to Cardholder record storage..." for
// discussion about the cardholder record storage requirements

// The following structures are the SCP specific version of commands 101-105

typedef struct				// (C_1101) Input conversion table
{	long  lastModified;			// update tag
	IntVB nScpID;				// the SCP's ID
	IntVB number;				// (table number + 128): use 128, 129, 130, or 131
	IntVB pri_5pr;				// priority: 5% (non-settling error)
	IntVB sts_5pr;				// status code: 5% (non-settling err)
	struct
	{	IntVB priority;			// reporting priority
		IntVB status_code;		// status code (0-7)
		IntVB res_code_1;		// resistance code, range 1
		IntVB res_code_2;		// resistance code, range 2
	} table[8];					// array of range spec structures
}CC_SCP_ICVT;

typedef struct				// (C_1102) Card format table specs
{	long  lastModified;			// update tag
	IntVB nScpID;				// the SCP's ID
	IntVB number;				// table number
	long  facility;				// facility code
	long  offset;				// cardholder ID offset
	IntVB function_id;			// card formatter function: 1==Wiegand, 2==Mag, 3==MTA
	// use the structure that applies to the selected function (CFMT_F_xxx)
	SCP_CARD_FORMAT arg;
} CC_SCP_CFMT;


typedef struct				// (C_1103) Timezone specs
{	long  lastModified;			// update tag
	IntVB nScpID;				// the SCP's ID
	IntVB number;				// time zone number
	IntVB mode;					// mode: OFF/ON/SCAN
	IntVB intervals;			// number of valid intervals in this record
	struct
	{	IntVB i_days;			// day mask: bit0==Sunday,...,bit8=Hldy0
		IntVB i_start;			// starting time: minutes (0 to 23*60+59)
		IntVB i_end;			// ending time: minutes (0 to 23*60+59)
	}i[MAX_TZINT];				// - assuming 12 intervals max per tz
} CC_SCP_TZ;

typedef struct						// (C_1820) Request Batch Transaction
{
	IntVB nScpID;					// the SCP's ID
	USHORT trigNumber;				// Trigger Number to Request Activation Summary
	USHORT actType;				   // Action Type
}CC_BATCH_TRANS;


typedef struct				// (C_3103) Timezone specs
{	long  lastModified;			// update tag
	IntVB nScpID;				// the SCP's ID
	IntVB number;				// time zone number
	IntVB mode;					// mode: OFF/ON/SCAN/OTE/RECURRING
	long actTime;
	long deactTime;
	IntVB intervals;			// number of valid intervals in this record
	struct
	{	IntVB i_days;			// day mask: bit0==Sunday,...,bit8=Hldy0
		IntVB i_start;			// starting time: minutes (0 to 23*60+59)
		IntVB i_end;			// ending time: minutes (0 to 23*60+59)
	}i[MAX_TZINT];			// - assuming 12 intervals max per tz
	
	char expTest[32];			// either datestring (YYYY-MM-DD) or
								// week of month (ascii 1-5) depending on
								// mode OTE/RECURRING

} CC_SCP_TZEX_ACT;

// NOTE: a new method of working with holidays has been added:
// You may set the "number" field to -1 to select the alternate form of operation:
//   a) (Obviously,) the holiday "number" field is not significant: 
//      holidays are searched for (by the starting date) to perform  a delete (works like cards)
//   b) if elapsed_days == 0 then clear all holidays,
//      else, if the day_mask is zero then delete this holiday,
//      else, add this holiday (into the next available slot)
//
typedef struct				// (C_1104) Holiday specs
{	long  lastModified;			// update tag
	IntVB nScpID;				// the SCP's ID
	IntVB number;				// record number, or -1, See NOTE above!!!
	IntVB year;					// year, e.g.: 1994
	IntVB month;				// month, 1-12
	IntVB day;					// day-of-month, 1-31
	IntVB extend;				// number of days in addition to start date
	IntVB type_mask;			// 0x01 == type 0, ...
}CC_SCP_HOL;

typedef struct				// (C_1105) Access Database Spec
{	long  lastModified;			// update tag
	IntVB nScpID;				// the SCP's ID
	long  nCards;				// number of cardholder records to allocate 
	IntVB nAlvl;				// number of access levels per cardholder
	IntVB nPinDigits;			// the lower nibble holds the number of PIN digits to store (0 to 15), AND 
								// the upper nibble holds the CardIdSize in addition to the default size:
								// ...(CardIdSizeInBytes-4)   ---- the CardholderId may be 4 to 8 bytes in size
								// --- example: 5-digit PIN, 48-bit card = ((6-4)<<4) + 5 = 0x25 = 37
								// The upper byte configures the PIN DURESS mode:
#define PIN_DURESS_NONE		(0x0000)	// disable PIN duress,
#define PIN_DURESS_OFFSET	(0x1000)	// enable  PIN duress, use the "Add a constant to the last digit (mod 10)" mode
#define PIN_DURESS_APPEND	(0x2000)	// enable  PIN duress, use the "Append a constant digit" mode
#define PIN_DURESS_CHAR_MSK	(0x0F00)	// this position holds the duress character: offset or appended BCD digit
								// --- bits 8 through 11 (in position 0x0F00) define the offset as the appended constant digit
								// --- bits 12 through 15 (in position 0xF000) define the PIN duress mode
								// ---- example: add 0x0n00 to the lower byte to disable the PIN duress mode
								//               add 0x1100 to the lower byte to select PIN duress, where "1" is added to the 
								//                    the last pin digit: "1234" becomes "1235", "1239" becomes "1230"
								//               add 0x2900 to the lower byte to select PIN duress, where the user appends the
								//                    digit "9" to the normal PIN code: "1234" becomes "12349" "1239" becomes "12399"
	IntVB bIssueCode;			// store issue code: 0 = do not store, 1 = standard 8-bit, 2 == extended 32-bit i/c (see C_5304)
	IntVB bApbLocation;			// store anti-passback location
	IntVB bActDate;				// store activation date ==1, or date & time == 2
	IntVB bDeactDate;			// store deactivation date ==1, date & time == 2
	IntVB bVacationDate;		// store vacation date and duration (temporary de-activate)
	IntVB bUpgradeDate;			// store temporary access level upgrade date and duration
	IntVB bUserLevel;			// the number of user_level entries to store (0 to 7 valid)
	IntVB bUseLimit;			// store use limit
	IntVB bSupportTimedApb;		// save the time and the Acr number of last entry
	IntVB nTz;					// precision access: number of ACR's to save Tz entries for
	IntVB bAssetGroup;			// store asset group code: 2-bytes
	IntVB nHostResponseTimeout;	// time to wait for HOST Approved Access Response
								//  -- 1 second increments, 0==default==5 seconds
	IntVB nMxmTypeIndex;		// (obsolete: index to the user level entry that holds the MxM User Type)
								//    (Mxm User Types: 0 = standard ("other"), 1 = "Supervisor/Service", 2 = "team member")
	IntVB nAlvlUse4Arq;			// if zero (default) then use all the access levels in the card record, as assigned by ::nAlvl, else,
								// if non-zero, this holds the number of access levlels to use for access request processing
								// -- this setting does not impact the card record size
								// example: if nAlvl == 32 and nAlvlUse4Arq == 22, then the access request processor will 
								//          check only the first 22 access levels. 
								//         (Note: the rest of the access levels are available for extended user command authority spec)
								// *** Reserved Freeform Field Types ****
#define	FFRM_FLD_ENCID		1	// Freeform Field Type large encoded ID
#define FFRM_FLD_LANGUAGE   3   // Language of Cardholder, 0-16.  Used to specify prompts.
#define	FFRM_FLD_ACCESSFLGS				8	// extended access flags (such as admin card)

	IntVB nFreeformBlockSize;	// Size of the Freeform block.  If zero, then no block is created.
	struct {
		IntVB	nFieldType;		// Type of data in the field
		IntVB	nFieldSize;		// Size of the data field
	} fldsFreeform[MAX_FREEFORM_FIELDS];
	IntVB nEscortTimeout;		// time to wait in between cards in escort processing (1 second increments, 0==default==15 seconds)
	IntVB nMultiCardTimeout;	// time to wait in between cards in multi card processing (1 second increments, 0==default==15 seconds)
	IntVB nAssetTimeout;		// Not used, set to 0
	IntVB bAccExceptionList;	// enable access exception list ((3 bytes))
	long adbFlags;
} CC_SCP_ADBS;
// <<<-----
// Limits:
// -- Max CardId size is 64-bits,
// -- Max PIN size is 15 digits,
// -- Max Access Levels per cardholder record is 8,
// -- Act/Dact/Vacation/Temp date dates are in elapsed days from 1900, January 1,
//    Act/Deact date and time settings are in elapsed seconds from 1970, January 1, 00:00:00.
// -- The maximum number of user levels that can be stored in a cardholder record is 7
//
// Guide to Cardholder record storage requirement in the SCP based on the Access Database Specs:
// -- Each Cardholder record has a 7-byte overhead (NODE linkage)
// -- The size of the CardholderId field can range from 4 to 8 bytes, with the default CardholderID size
//    of 4 bytes. Larger CardholderId is specified by setting the upper nibble of nPinDigits to the number
//    of bytes in addition to the default 4 bytes that is needed for storing the CardholderId.
//    nPinDigits is formed by adding the number of PIN digits to ((nCardIdSizeInBytes-4)<<4).
// -- The allocation for access level entries reserved in the cardholder record (CC_ADBS::nAlvl) is affected
//    by the number of access levels allocated for the SCP in CC_SCP_SCP::nAlvl (or in CC_SCP::nAlvl):
//    For CC_SCP_SCP::nAccessLevels <= 8, we need 1 byte per CC_SCP_ADBS::nAlvl;
//    Two PIN digits per byte are packed in the record. The allocation is (((nPinDigits&0xF)+1)/2) bytes.
//    Zero, one, or 4 bytes are required for storing the issue code. (::bIssueCode == 0 -> 0 bytes, == 1 -> 1 byte, == 2 -> 4 bytes)
//    One byte is required for storing the anti-passabck location. (::bApbLocation is not zero)
//    bActDate is set to 1 to store the date, or it is set to 2 to store the date & time. Storing the date
//    only requires two bytes, storing the date & time requires 4 bytes.
//    bDeactDate is set to 1 to store the date, or it is set to 2 to store the date & time. Storing the date
//    only requires two bytes, storing the date & time requires 4 bytes.
//    bVacationDate is set to 1 to select to store the date and the duration - requires 3 bytes
//    bVB bUpgradeDate is set 1 to select to store the date and the duration - requires 3 bytes
//    bUserLevel selects the number of user_level entries to store per cardholder - 1 byte is required per user_level
//    bUseLimit is set to 1 to store the current use limit and the original use limit setting - requires 2 bytes
//    bSupportTimedApb is set to 1 to store the date, time and Acr of the cardholder's last access - requires 5 bytes
//    nTz specifies the size of the array in units of the number of Acrs to store presision timezones for.
//    One byte is requires per ACR.
//    bAssetGroup is set to 1 to store an asset group entry in the cardholder record - requires 2 bytes
//    The nHostResponseTimeout has no affect on card record size;
//	  The nEscortTimeout has no affect on card record size;
//	  The nMultiCardTimeout has no affect on card record size;
//	  The nAssetTimeout has no affect on card record size;


typedef struct				/* (C_304) Access Database Card Records */
{	long  lastModified;			/* update tag */
	IntVB scp_number;			/* scp number to send this record to */
	IntVB flags;				/* user flags */
#define ADBC_ACTIVE		0x01	/* - cardholder record active */
#define ADBC_1FREE		0x02	/* - allow one free ap/b pass */
#define ADBC_VIP		0x04	/* - anti-passback exempt */
#define ADBC_ADA		0x08	/* - use handicapped timing parameters (ADA) */
#define ADBC_UNDEF		0x10	/* - PIN Exempt card & PIN mode (was "undefined") */
#define ADBC_NOAPB		0x20	/* - do not alter current apb loc */
#define ADBC_NOUSE		0x40	/* - do not alter either the "base" nor the "current" use count values */
#define ADBC_NOUSECRNT	0x80	/* - do not alter the "current" use count */
	long  card_number;			/* cardholder id number */
	IntVB issue_code;			/* issue code */
	char  pin[MAX_PIN_STD+1];	/* pin: '\0' terminated 'C' string */
	IntVB alvl[MAX_ALVL_STD];	/* access levels */
	IntVB apb_loc;				/* anti-passback location */
	IntVB use_count;			/* use count */
	IntVB act_date;				/* activation date: in elapsed days - days from 1900 */
	IntVB dact_date;			/* deactivation date - days from 1900 */
	IntVB vac_date;				/* vacation starting date - days from 1900 */
	IntVB vac_days;				/*  - number of days (in addition) */
	IntVB tmp_date;				/* temporary upgrade starting date - days from 1900 */
	IntVB tmp_days;				/*  - number of days (in addition) */
	IntVB user_level;			/* user level, for command entry */
	IntVB alvl_prec[MAX_ACR_PER_SCP];	// precision access: tz per Acr
	IntVB asset_group;			// asset group
} CC_ADBC_RECORD;

// CardID is Int64, PIN 15 digit max, Access Levels 32 max, Act/Dact date & time, multiple user levels, 32-bit issue code
typedef struct				/* (C_5304) Access Database Card Records */
{	long  lastModified;			/* update tag */
	IntVB scp_number;			/* scp number to send this record to */
	IntVB flags;				/* user flags (same as in CC_ADBC_RECORD.flags) */
	__int64  card_number;		/* cardholder id number */
	long  issue_code;			/* issue code */
	char  pin[MAX_PIN_EXTD+1];	/* pin: 15-digits plus '\0' terminated 'C' string */
	IntVB alvl[MAX_ALVL_EXTD];	/* access levels - extended */
	IntVB apb_loc;				/* anti-passback location */
	IntVB use_count;			/* use count */
	long  act_time;				/* activation date - in elapsed seconds, localtime in SCP, 1970-based */
	long  dact_time;			/* deactivation date - in elapsed seconds, localtime in SCP, 1970-based */
	IntVB vac_date;				/* vacation starting date - days from 1900 */
	IntVB vac_days;				/*  - number of days (in addition) */
	IntVB tmp_date;				/* temporary upgrade starting date - days from 1900 */
	IntVB tmp_days;				/*  - number of days (in addition) */
	IntVB user_level[MAX_ULVL];			// user level, for command entry */
	IntVB alvl_prec[MAX_ACR_PER_SCP];	// precision access: tz per Acr
	IntVB asset_group;					// Not used, set to 0
} CC_ADBC_I64DTIC32;

// NEW: CardID is Int64, PIN 15 digit max, Access Levels 255 max, Act/Dact date & time, multiple user levels, 32-bit issue code, Freeform fields.
typedef struct				/* (C_8304) Access Database Card Records */
{	long  lastModified;			/* update tag */
	IntVB scp_number;			/* scp number to send this record to */
	IntVB flags;				/* user flags (same as in CC_ADBC_RECORD.flags) */
	__int64  card_number;		/* cardholder id number */
	long  issue_code;			/* issue code */
	char  pin[MAX_PIN_EXTD+1];	/* pin: 15-digits plus '\0' terminated 'C' string */
	IntVB alvl[MAX_ALVL_VALUE];	/* access levels - 255 */
	IntVB apb_loc;				/* anti-passback location */
	IntVB use_count;			/* use count */
	long  act_time;				/* activation date - in elapsed seconds, localtime in SCP, 1970-based */
	long  dact_time;			/* deactivation date - in elapsed seconds, localtime in SCP, 1970-based */
	IntVB vac_date;				/* vacation starting date - days from 1900 */
	IntVB vac_days;				/*  - number of days (in addition) */
	IntVB tmp_date;				/* temporary upgrade starting date - days from 1900 */
	IntVB tmp_days;				/*  - number of days (in addition) */
	IntVB user_level[MAX_ULVL];			// user level, for command entry */
	IntVB alvl_prec[MAX_ACR_PER_SCP];	// precision access: tz per Acr
	IntVB asset_group;					// asset group
	char  freeform[MAX_FREEFORM_FIELDS][MAX_FREEFORM_SIZE];	// Freeform fields (max field size is the entire block size)
} CC_ADBC_I64DTIC32A255FF;		// CardId-Int64, PIN-15,AccessLevels-255, Act/Dact date & time, 32-bit issue code, Freeform fields

// The following SCP spec structure is used with Command 1107. This command causes
// separate config tables to be maintained per SCP for Commands 1101 through 1105.
typedef struct				/* (C_1107) SCP device specification */
{	long  lastModified;			/* update tag */
	IntVB number;				/* device number */
	long  ser_num_low;			/* serial number, low limit */
	long  ser_num_high;			/* serial number, high limit */
	IntVB rev_major;			/* required software rev level */
	IntVB rev_minor;			/* required software rev level */
	IntVB nMsp1Port;			/* number of RS-485 IO module ports */
	long  nTransactions;		/* number of transactions */
	IntVB nSio;					/* number of SIOs */
	IntVB nMp;					/* number of monitor points */
	IntVB nCp;					/* number of control points */
	IntVB nAcr;					/* number of access control readers */
	IntVB nAlvl;				/* number of access levels */
	IntVB nTrgr;				/* number of triggers */
	IntVB nProc;				/* number of procedures */
	// the extended argument block below overrides the system wide defaults:
	long  gmt_offset;			// gmt-timezone offset (Ex: 8*3600=28800 for Pacific Time Zone)
	IntVB nDstID;				// daylight savings ID: 0==none, else ID (see Cmnd 016)
	IntVB nTz;					// number of timezones (schedules)
	IntVB nHol;					// number of holidays
	IntVB nMpg;					// number of monitor point groups (MAX_MPPERMPG - max MPs per Group)
	long  nTranLimit;			// number of unreported transactions to log
								// transaction: tranSrcScpDiag:0, tranTypeSys:4
	IntVB nAuthModType;			// Set to 0
	IntVB nOperModes;			// Set to 0		
	IntVB oper_type;			// Set to 0 
	IntVB nLanguages;           // Set to 0
	IntVB nSrvcType;			// Set to 0
} CC_SCP_SCP;

typedef struct				// (C_108)
{	long  lastModified;			/* update tag */
	IntVB scp_number;			/* scp number */
	IntVB msp1_number;			/* SIO driver number */
	IntVB port_number;			/* physical port number on the SCP */
	long  baud_rate;			/* baud rate: 2400/9600/19200/38400 */
	long  reply_time;			/* reply timeout: 00 == default 90ms, the dll will force  50 < timeout < 150 */
	IntVB nProtocol;			// Protocol Type
#define XX00_PROTOCOL				0x00	// X100, X200, and X300 protocol
#define VERTX_PROTOCOL				0x0F	//VertX V100, V200 and V300 protocol
	IntVB nDialect;				// Dialect, 0 = default
#define MSP1_DIALECT_DEFAULT		0x00	// Default (matches controller type)
}CC_MSP1;

typedef struct				/* (C_109) SIO definition */
{	long  lastModified;			/* update tag */
	IntVB scp_number;			/* SCP number this SIO is attached to */
	IntVB sio_number;			/* ...SIO number on this SCP */
	IntVB nInputs;
	IntVB nOutputs;
	IntVB nReaders;
	IntVB model;				/* model: Ref to API manual */
	IntVB revision;				/* lowest allowed revision (0 is ok) */
	long  ser_num_low;			/* serial number, low limit */
	long  ser_num_high;			/* serial number, high limit */
	IntVB enable;				/* enable data comm to this SIO.  0 = Disable, 1 = enable, 2 = enable ONLY after AES-Encryption preference configured. */
	IntVB port;					/* RS-485 port to attach to (0-n) */
	IntVB channel_out;			/* channel to send on */
	IntVB channel_in; 			/* channel to reply on */
	IntVB address;				/* comm address */
	IntVB e_max;				/* number of errors before off-line */
	IntVB flags;				/* flags - to be assigned */
								/*  - 0x10 == Special Diagnostic mode */
								/*  - 0x20 == reverse the processing order of this Sio's inputs (ScpX_736) */
	// te following arguments may be omitted, default: 0==(SIO number+1)
	IntVB nSioNextIn;			// SIO number for continuation of inputs
	IntVB nSioNextOut;			// SIO number for continuation of outputs
	IntVB nSioNextRdr;			// SIO number for continuation of readers

	// extended connection verification setting
	IntVB nSioConnectTest;		// Not used, set to 0
	IntVB nSioOemCode;			// Not used, set to 0
	IntVB nSioOemMask;			// Not used, set to 0
} CC_SIO;



typedef struct				/* (C_110) Input Point Specification */
{	long  lastModified;			/* update tag */
	IntVB scp_number;			/* SCP number... */
	IntVB sio_number;			/* ...SIO number... */
	IntVB input;				/* ...input number on the SIO */
	IntVB icvt_num;				/* scan conversion table number: 0 through 3 (standard), or 128 through 131 (custom) */
	IntVB debounce;				/* number of input scans which must agree */
	IntVB hold_time;			/* sec's to hold before lower pri 0-15, and (off-line hold time + 15/16)*16 */
} CC_IP;

typedef struct				/* (C_111) Output point specifications */
{	long  lastModified;			/* update tag */
	IntVB scp_number;			/* SCP number... */
	IntVB sio_number;			/* SIO number,... */
	IntVB output;				/* ...output number on the SIO */
	IntVB mode;					/* output drive mode */
} CC_OP;

typedef struct				/* (C_112) Reader specification */
{	long  lastModified;			/* update tag */
	IntVB scp_number;			/* SCP number... */
	IntVB sio_number;			/* SIO number,... */
	IntVB reader;				/* ...reader number on the SIO */
	IntVB dt_fmt;				/* card data format flags: */
#define IDRDR_D1D0		0x01	/* - data 1/data 0, Wiegand pulses */
#define IDRDR_ZTRIM		0x02	/* - trim zero bits */
#define IDRDR_T2FMT		0x04	/* - mag stripe Track 2 data decode */
#define IDRDR_BIDIR		0x08	/* - allow bi-directional mag decode */
	IntVB keypad_mode;			/* keypad data filter (config to SIO)*/
#define IDRDR_K_NONE		0x00	/* - no keypad decoding defined */
#define IDRDR_K_HID			0x02	/* - HID 4-bit keypad format */
#define IDRDR_K_INDALA		0x03	/* - Motorola/Indala format */
#define IDRDR_K_4BIT_ALIVE_60     0x06   /* - 4-bit keypad with HID I'm Alive support (60 second interval) */
#define IDRDR_K_8BIT_ALIVE_60     0x07   /* - 8-bit keypad with HID I'm Alive support (60 second interval) */
#define IDRDR_K_4BIT_ALIVE_10     0x08   /* - 4-bit keypad with HID I'm Alive support (10 second interval) */
#define IDRDR_K_8BIT_ALIVE_10     0x09   /* - 8-bit keypad with HID I'm Alive support (10 second interval) */
	IntVB led_drive_mode;		/* reader LED color table index (1, 2, or 3)*/
#define IDRDR_L_BICOLOR	0x01	/* - separate red, green and buzzer conductors */
#define IDRDR_L_OSDP	0x07	/* - OSDP reader w. LCD & keypad. (9600 baud, addr-0) */
	IntVB osdp_flags;			/* - OSDP flag settings */
								/*		- No bits set mean to use default values for specific reader port */
								/*		- bits 0 - 2 = baud rates (1=9600, 2=19200, 3=38400, 4=115200) */
								/*		- bit 3 = Rdr Mode (0 = Normal, 1 = Smart Card) */
								/*		- bit 4 = Diagnostic tracing (0 = off, 1 = on) */
								/*		- bits 5 - 6 = address (0 - 3) */
								/*		- bit 7 = Secure Channel (0 = off, 1 = on) */
#define IDRDR_OSDP_BAUD_9600	0x01	/* OSDP Baud rate = 9600 */
#define IDRDR_OSDP_BAUD_19200	0x02	/* OSDP Baud rate = 19200 */
#define IDRDR_OSDP_BAUD_38400	0x03	/* OSDP Baud rate = 38400 */
#define IDRDR_OSDP_BAUD_115200	0x04	/* OSDP Baud rate = 115200 */
#define IDRDR_OSDP_TRACING		0x10	/* OSDP Diagnostic tracing flag */
#define IDRDR_OSDP_ADDR_MASK	0x60	/* OSDP address mask */
#define IDRDR_OSDP_SC			0x80	/* OSDP Secure Channel flag */
	long device_id;						/* Additional device ID information used to map to specific reader - lock */
} CC_RDR;

typedef struct				/* (C_113) Monitor Point configuration */
{	long  lastModified;			/* update tag */
	IntVB scp_number;			/* SCP number... */
	IntVB mp_number;			/* ...monitor point number */
	IntVB sio_number;			/* SIO number... */
	IntVB ip_number;			/* ...input number on this SIO */
	IntVB lf_code;				/* log function code */
#define MPLG_0		0x00		/* - log function 0 (logs all changes)*/
#define MPLG_1		0x01		/* - log function 1 (do not log contact c-o-s if masked)*/
#define MPLG_2		0x02		/* - log function 2 (1 plus no fault-to-fault changes)*/
	IntVB mode;					// MP mode: 0==normal, 1==non-latching, 2==latching 
	IntVB delay_entry;			// entry delay (seconds)
	IntVB delay_exit;			// exit delay (seconds)
} CC_MP;

typedef struct				/* (C_114) Control Point */
{	long  lastModified;			/* update tag */
	IntVB scp_number;			/* SCP number... */
	IntVB cp_number;			/* ...control point number */
	IntVB sio_number;			/* SIO number... */
	IntVB op_number;			/* ...output number on this SIO */
	IntVB dflt_pulse;			// default pulse time (1 sec per count)
} CC_CP;

typedef struct				// (C_115) Access Control Reader configuration
{	long lastModified;			// update tag
	IntVB scp_number;			// SCP number...
	IntVB acr_number;			// ...access control reader number
	IntVB access_cfg;			// configuration:
#define ACR_A_SINGLE	0		// - single, controlling the door
#define ACR_A_MASTER	1		// - paired, this is the master
#define ACR_A_SLAVE		2		// - paired, this is the slave
#define ACR_A_TURNSTILE	3		// - turnstile: pulse strk, allow acr_abort
#define ACR_A_EL1		4		// - elevator, no floor select feedback
#define ACR_A_EL2		5		// - elevator with floor select feedback
	IntVB pair_acr_number;		// access reader number of its pair
	IntVB rdr_sio;				// reader link: the sio the reader is on
	IntVB rdr_number;			// ...reader number on the sio
	IntVB strk_sio;				// strike link: the sio the relay is on
	IntVB strk_number;			// ...relay number on the sio
	IntVB strike_t_min;			// min activation time, in seconds
	IntVB strike_t_max;			// max activation, in seconds
	IntVB strike_mode;			// strike control mode
//#define ACR_S_NONE	 0		// - door has no impact on strike
#define ACR_S_OPEN		 1		// - cut short when door opens
#define	ACS_S_CLOSE		 2		// - turn off strike on door close
#define ACR_S_TAILGATE	16		// - flag, used along with ACR_S_OPEN/_CLOSE, to select tailgate mode:
								//   pulse (strk_sio:strk_number+1) relay for each user expected to enter
	IntVB door_sio;				// door contact link: the sio the input is on
	IntVB door_number;			// ...input number on the sio
	IntVB dc_held;				// delay to close, new cycle (2 sec)
	IntVB rex0_sio;				// rex-0 link: the sio the input is on
	IntVB rex0_number;			// ...input number on the sio
	IntVB rex1_sio;				// rex-1 link: the sio the input is on
	IntVB rex1_number;			// ...input number on the sio
	IntVB rex_tzmask[2];		//  timezone for disabling the REX
								// Note: the members bolt_sio, bolt_number, and bolt_spec have been reallocated
								// to define an alternate reader option: altrdr_sio, altrdr_number, and altrdr_spec
	IntVB altrdr_sio;			// alternate reader link: the sio the reader is on
	IntVB altrdr_number;		// ...reader number on the sio
	IntVB altrdr_spec;			// alternate reader configuration
#define ACR_AR_NONE			0	// - ignore data from the Alternate Reader
#define ACR_AR_NRML			1	// - normal access reader
	IntVB cd_format;			// card data formatter table mask map
	IntVB apb_mode;				// anti-passback processing mode
#define ACR_APB_NONE	0		// - do not check or alter a/pb loc
#define ACR_APB_ANY		1		// - accept any loc., change on entry
#define ACR_APB_CHK		2		// - check loc., change on entry
#define ACR_APB_DLY_L	3		// - check this reader's last valid user
#define ACR_APB_DLY_R	4		// - check user's last ACR used, no location change
#define ACR_APB_DLY_A	5		// - check user's current location, change on entry
#define ACR_APB_DLY_M_L	6		// - check this reader's last valid user (delay in minutes)
#define ACR_APB_DLY_M_R	7		// - check user's last ACR used, no location change (delay in minutes)
#define ACR_APB_DLY_M_A	8		// - check user's current location, change on entry (delay in minutes)
	IntVB apb_in;				// area number the user must be in
	IntVB apb_to;				// area the user shall be moved to
	IntVB spare;				// spare - NOW: extended actl_flags (2006/06/28)
#define ACR_FE_NOEXTEND	 0x0001	// - on a new grant: do not resume to extended door held time, set new parm's
#define ACR_FE_NOPINCARD 0x0002	// - CARD & PIN - do not accept PIN followed by CARD in "CARD & PIN" mode
#define ACR_FE_DFO_FLTR  0x0008	// - enable the "door forced open filter" (opening within 3 seconds of closing is not DFO)
#define ACR_FE_NO_ARQ	 0x0010	// - do not allow any access request processing (20/08/02/21) */
#define	ACR_FE_SHNTRLY	 0x0020	// - relay (strike_rly+1) becomes the "shunt relay" (2008/04/30)
#define ACR_FE_FLOOR_PIN 0x0040 // - Output selection tracking (PIN-based floor/bin entry)
#define ACR_FE_LINK_MODE 0x0080 // - flag indicating if in link mode, acr_mode==29 sets it, acr_mode==30, timeout, or linking clears it
#define ACR_FE_DCARD     0x0100 // - enable Double-Card Event (card presentation of the same card within 5 seconds of valid acceess)
#define ACR_FE_OVERRIDE  0x0200 // - flag indicating if in Temp ACR Override
#define ACR_FE_CRD_OVR_EN	  0x0400 // - flag indicating if credential overrides at this lock
#define ACR_FE_ELV_DISABLE	  0x0800 // - flag indicating if allows the ACR to enable/disable the disable floors
#define ACR_FE_LINK_MODE_ALT  0x1000 // - flag indicating if in link mode Alternate, acr_mode==32 sets it, acr_mode==33, timeout, or linking clears it
#define ACR_FE_REX_HOLD		  0x2000  // - flag to enable extending REX 'Grant time' while REX input is active 
#define ACR_FE_HOST_BYPASS	  0x4000  // - flag to enable host grant to bypass local validation first
#define ACR_FE_REX_EARLYTXN   0x8000  // - flag to enable generating REX transactions at the beginning of a REX cycle. 
	IntVB actl_flags;			// control flags:
#define ACR_F_DCR		0x0001	// - decrement use limits on access
#define ACR_F_CUL		0x0002	// - require use limit to be non-zero
#define ACR_F_DRSS		0x0004	// - set to deny a duress request
#define ACR_F_ALLUSED	0x0008	// - log all access requests as used
#define ACR_F_QEXIT		0x0010	// - do not pulse the door strike on rex
#define ACR_F_FILTER	0x0020	// - filter CosDoor transactions
#define ACR_F_2CARD		0x0040	// - require two-card control at this reader
#define ACR_F_SETMPR	0x0080	// - asset readers: enable Tamper Flag set function <<-- obsolete HID Asset control flag
#define ACR_F_NOPIPE	0x0080	// - disables pipelining access requests (discard an access request if one already pending)
#define ACR_F_HOST_CBG	0x0400	// - if on-line, check with HOST before GRANTING access
#define ACR_F_HOST_SFT	0x0800	// - if HOST is not available (off-line or timeout) proceed with GRANT
#define ACR_F_CIPHER	0x1000	// - enable cipher mode (if user command fits a card format then use it as card)
#define ACR_F_LOG_EARLY	0x4000	// - if set, log access grant transaction right away, then log used/not-used
#define ACR_F_CNIF_WAIT	0x8000	// - show "wait" on "card not in file" (not the std "denied" response)
#define ACR_F_NOEXTEND	(0x010000)	/* - do not resume to extended door held time, set new parm's */
#define ACR_F_NOPINCARD	(0x020000)	/* - do not accept PIN followed by card in CARD & PIN mode */
#define ACR_F_ASSETREQD	(0x040000)	/* - asset is required with each access request (20070716) */
#define ACR_F_DFO_FLTR	(0x080000)	/* - enable Door Forced Open Filter processing (20080221) */
#define ACR_F_NO_ARQ		(0x100000)	/* - do not allow any access request processing (20080221) */
#define ACR_F_SHNTRLY	(0x200000)	/* - configure the door shunt mode via the reader off-line config command */
#define ACR_F_FLOOR_PIN	(0x400000)	/* - use the PIN to only enable a specific floor/bin/door number*/
#define ACR_F_LINK		(0x800000)	/* - link IR-PIM or OSDP reader */
#define ACR_F_DCARD	  (0x01000000)	/* - Double-Card mode enabled */
#define ACR_F_OVERRIDE (0x02000000)	/* - Override ACR Mode in effect (read-only) */
#define ACR_F_CRD_OVR_EN (0x04000000)	/* - enables credential overrides at this lock (card can gain access to locked acr) */
	IntVB offline_mode;			// off-line acc. mode (0==no change)
	IntVB default_mode;			// default acc. mode (0==no change)
								// ((if bit 7 is set and firmware rev is newer than 3.1.2, then ACR current mode is set))
	IntVB default_led_mode;		// led mode (0==no change) 1-3 ok
/* new fields */
	IntVB pre_alarm;			// number of 2 sec. ticks before held open
	IntVB apb_delay;			// anti-passback delay, in seconds for ACR_APB_DLY_* modes and minutes for ACR_APB_DLY_M_* modes
/* handicapped access - ADA compliance - invoked for users with ADA flag set */
	IntVB strk_t2;				// strike time, "special door cycle" (ADA)
	IntVB dc_held2;				// held open time, "special door cycle" (ADA)
	IntVB strk_follow_pulse;	// Strike Follower enable and duration (strk_sio:strk_number+1). 0 = Disable, -1 = Same duration as strike,
								// 1 through 20 = # of .1 second tics.
	IntVB strk_follow_delay;	// Strike Follower delay from strike activation, in .1 second tics. 0 = No delay.
								// Should door open, before delay expires, or if delay is less than pulse time, no strike follow delay will occur.
	long nAuthModFlags;			// Not used, set to 0
	IntVB	nExtFeatureType;	// (0=None, 1=Classroom, 2=Office, 3=Privacy, 4=Apartment, ..)
	union
	{
		struct
		{
			IntVB	iIPB_sio;			// SIO ID for Int. Push Button (Not needed for native locksets)
			IntVB	iIPB_number;		// Input number for Int. Push Button (Not needed for native locksets)
			IntVB	iIPB_long_press;	// IPB long-press 0-15 sec. (if applicable)
			IntVB	iIPB_out_sio;		// SIO ID for IPB indicator output (Not needed for native locksets)
			IntVB	iIPB_out_num;		// Output number for IPB indicator output (Not needed for native locksets)
		} sIPBoverrides;
	} uExtFeatureInfo;
	IntVB dfofFilterTime;
} CC_ACR;
//  Note:  Any SIOs referenced in the CC_ACR must be configured prior to issuing the CC_ACR. 
//         if they are not configured at this time, it is the same as using a -1 for the SIO number.
//

typedef struct				// (C_151) Configure Logging based on deny count
{	IntVB scp_number;			// SCP number...
	IntVB acr_number;			// ...access control reader number
	long  nLpdFlags;			// control flags */
#define LDF_PIN		(0x0001)	// enable counting Not-In-File violations while in PIN ONLY mode
#define LDF_CIPHER	(0x0002)	// enable counting Not-In-File violations if entry was made in cipher mode
#define LDF_CAP		(0x0004)	// enable counting Wrong PIN" violations in Card and PIN mode
#define LDF_DACT	(0x0100)	// deactivate the card if CardAndPin, bad pin
	IntVB nLpdLimit;			// trip value of log entry
	IntVB nLpdTime;				// in seconds, reset counts after this much time since last error
}CC_ACRLOGDENY;

typedef struct				// (C_116) Access level configuration
{	long  lastModified;			// update tag
	IntVB scp_number;			// SCP number...
	IntVB alvl_number;			// ...access level number
	IntVB tz[MAX_ACR_PER_ALVL];	// make a Timezone entry for each ACR
}CC_ALVL;

typedef struct				// (C_2116) Access level configuration Extended
{	long  lastModified;			// update tag
	IntVB scp_number;			// SCP number...
	IntVB alvl_number;			// ...access level number
	IntVB oper_mode;			// Operating mode
	IntVB tz[MAX_ACR_PER_ALVL];	// make a Timezone entry for each ACR
}CC_ALVL_EX;

typedef struct				// (C_124) Access Level Spec Block configuration
{	IntVB nScpNumber;			// SCP number
	IntVB nAlvlNumber;			// access level number
	IntVB nActYear;				// Activation year, e.g.: 2004
	IntVB nActMonth;			//   month, 1-12
	IntVB nActDay;				//   day-of-month, 1-31
	IntVB nActHh;				//   hours 0-23
	IntVB nActMm;				//   minutes 0-59
	IntVB nActSs;				//   seconds 0-59
	IntVB nDactYear;			// Deactivation year, e.g.: 2004
	IntVB nDactMonth;			//   month, 1-12
	IntVB nDactDay;				//   day-of-month, 1-31
	IntVB nDactHh;				//   hours 0-23
	IntVB nDactMm;				//   minutes 0-59
	IntVB nDactSs;				//   seconds 0-59
	IntVB nEscortCode;			// escort code of user at readers where tz is active
								// 0 == not an escort, 1 == is an escort, 2 == escort is required
	IntVB oper_mode;			// Operating Mode
}CC_ALVL_SPC;

typedef struct				// (C_117) Trigger Specification
{	long  lastModified;			// update tag
	IntVB scp_number;			// SCP number...
	IntVB trgr_number;			// ...trigger number
	IntVB command;				// command to issue to the procedure
	IntVB proc_num;				// procedure to invoke
	IntVB src_type;				// source type (event processor code)
	IntVB src_number;			// source number, or -1
	IntVB tran_type;			// transaction type
	long code_map;				// transaction code map: bit array
	IntVB timezone;				// timezone
	IntVB trig_var[4];			// trigger variables - 4 signed chars
	IntVB arg[4];				// trl type dependent arguments
								// for trl type "tranTypeCardID" (06)
								//	 arg[0] - non-zero to accept any user level, zero to require match
								//          new definition to allow extension
								//			- index to user level to match (0==first user level)
								//			  user level is not checked if a nonexistent user level is specified
								//	 arg[1] - user level to match (checked only if arg[0] indexes a valid user level)
								//   arg[2] - floor number selected (-1==any floor)
								//   arg[3] - special C_ACR::actl_flags based control
								//          -- ignored unless its value is between 0x40 (64) to 0x7F (127)
								//			-- 0x41 == require that actl_flags::ACR_F_MODE_TRIG is set
								// for tran_type==tranTypeCoS (7) AND tran_code == 3 AND arg[0] is one of the following:
					            //   arg[0] == 1 - trip ONLY if status is now inactive (was active)
								//   arg[0] == 2 - trip ONLY if status is now active (was inactive)
								//   arg[0] == 3 - trip ONLY if last state was ALARM
								//   arg[0] == 4 - trip ONLY if last state was FAULT
								//   arg[0] == 5 - trip ONLY if last state was ALARM or FAULT
								// for trl type "tranTypeCoSDoor" (09),
								//   if tran code is 3, then further qualify the trigger based on the previous state
								//   arg[0] - 1 == trigger on held open pre-alarm only
								//			- 2 == trigger if Forced Open is being cancelled
								//			- 3 == trigger if Held Open is being cancelled
								//			- 4 == trigger if Held or Forced is being cancelled
								//			- 5 == trigger if the door just closed
								//			- 6 == trigger if the door just opened (no alarm)
								//          - any other value == trigger on any door safe message
								//   if tran code is 4, then the combination of Forced and/or Held Open is selected
								//   arg[0] - 1 == Forced Open only (held open after forced does not trigger
								//          - 2 == Held Open only (only after normal access cycle)
								//          - 3 == Both (Forced then held open)
								//          - 4 == Held Open, regardless of forced open status
								//          - 5 == Either (forced or held open, only one trigger)
								//          - any other value == trigger on any reader alarm
								// for trl type "tranTypeUserCmnd" (11)
								//   arg[0] - (1'st user code digit * 16) + (2'nd user code digit)
								//   arg[1] - (3'rd user code digit * 16) + (4'th user code digit)
								//   arg[2] - (5'th user code digit * 16) + (6'th user code digit)
								//   arg[3] - (7'th user code digit * 16) + (8'th user code digit)
								//     note: shorter than 8-digit codes are to be padded with '15'
								// for trl type "tranTypeAcrExtFeatureCoS" (65)
								//   arg[0] - pseudo point number
} CC_TRGR;

typedef struct				// (C_1117) Trigger Specification
{	long  lastModified;			// update tag
	IntVB scp_number;			// SCP number...
	IntVB trgr_number;			// ...trigger number
	IntVB command;				// command to issue to the procedure
	IntVB proc_num;				// procedure to invoke
	IntVB src_type;				// source type (event processor code)
	IntVB src_number;			// source number, or -1
	IntVB tran_type;			// transaction type
	__int64 code_map[2];		// transaction code map: bit array LSB first.
	IntVB timezone;				// timezone
	IntVB trig_var[4];			// trigger variables - 4 signed chars
	IntVB arg[4];				// trl type dependent arguments
	IntVB trigger_flags;		// extra flags for trigger
} CC_TRGR_128;

typedef struct				// (C_119) Remove Matching Actions from Range of Proc's
{	long  lastModified;			// update tag
	IntVB scp_number;			// SCP number
	IntVB cProcFirst;			// first procedure to check
	IntVB cProcLast;			// last procedure in the range
	IntVB cActionType;			// action type (as spec's in SCP_ACTION)
	long  arg;					// type dependent argument
								// -   0: this action type is ignored
								// -   1: arg == MP number
								// -   2: arg == CP number
								// -   3: arg == ACR number
								// -   4: arg == ACR number
								// -   5: arg == ACR number
								// -   6: arg == ACR number
								// -   7: arg == proc number
								// -   8: arg == TV number
								// -   9: arg == TZ number
								// -  10: arg == ACR number
								// -  11: arg == CardholderID/-1
								// -  13: arg == ACR
								// -  14: arg == Monitor Point Group number
								// -  15: arg == Monitor Point Group number
								// -  16: arg == Monitor Point Group number
								// -  17: arg == Area number
								// -  18: arg == ACR number
								// -  19: arg == ACR number
								// -  20: arg == Terminal (ACR) number
								// -  22: arg == ACR number
								// -  24: arg == ACR number
								// -  25: arg == ACR number
								// -  26: arg == CardholderID/-1
								// -  27: arg == Operating Mode
								// -  28: arg == ACR number
								// -  29: this action type is ignored
								// - 126: this action type is ignored
								// - 127: this action type is ignored
} CC_ACTNREM;

typedef struct				// 120 Configure a Monitor Point Group
{	long  lastModified;				// update tag
	IntVB scp_number;				// SCP number
	IntVB mpg_number;				// Monitor Point Group number
	IntVB nMpCount;					// number of (Monitor) Points in this Group
	IntVB nMpList[MAX_MPPERMPG*2];	// point list pairs: {point type, point number}
									// - nMpCount defines the number of valid pairs in the list
									// - point types:
									//		1: Monitor Point
									//		2: Forced Open
									//		3: Held Open
} CC_MPG;




/* The extended area command supports the "Special 1-Man Rule" the and "Special 2-Man Rule" */
typedef struct				// (C_1121) - area configuration command - with Special 1-Man/2-Man Option
{	long  lastModified;			// update tag
	IntVB scp_number;			// SCP number
	IntVB area_number;			// access area number (0 to 127)
	IntVB multi_occupancy;		// multi-occupancy mode:
#define AREA_MO_NONE	(0)		// - none (no multi-occupancy rules apply)
#define AREA_MO_STD		(1)		// - standard (multi-occupancy required, all users are same)
#define AREA_MO_M1M		(2)		// - Special 1-Man Rule (separate spec)
#define AREA_MO_M2M		(3)		// - Special 2-Man Rule (separate spec)
	IntVB access_control;		// 0==NOP, 1==disable Access, 2==enable Access
	IntVB occ_control;			// the flag bits in the entry control the setting of the occupancy counts
#define AREA_OC_STD		(1)		// - set the area's "standard user" count to the value in occ_set
#define AREA_OC_SPC		(2)		// - set the area's "special  user" count to the value in occ_set_sp
	long    occ_set;			// set "standard user" occupancy count to this value if AREA_OC_STD is set
	long    occ_max;			// maximum occupancy
	long    occ_up;				// log transaction when this count is reached, counting up
	long    occ_down;			// log transaction when this count is reached, counting down
	long    occ_set_spc;		// set "special user" occupancy count to this value if AREA_OC_SPC is set
	__int64 custodian;			// user ID of the "assigned team member"
	IntVB nAppRqRlySio;			// Approval Request Indicator Relay: Sio Number
	IntVB nAppRqRlyNum;			// Approval Request Indicator Relay: Sio Number
	IntVB nAppRqRlyDly;			// Approval Request Indicator Relay: wait time, in seconds (1-255)
	IntVB area_flags;			// Area Flags.
#define	AREA_F_AIRLOCK		0x0001	// Enable Airlock/Mantrap control: Area can only have open thresholds to one other Area.
#define	AREA_F_AIRLOCK_ODO	0x0002	// Enable Airlock/Mantrap control: Just (O)ne (D)oor (O)nly is allowed to be open into this Area (AREA_F_AIRLOCK must also be set).
} CC_AREA_SPC;

typedef struct				// (C_1220) Access Exception List
{	long lastModified;				// update tag
	IntVB scp_number;				// ACP number...
	__int64 nCardholderId;			// cardholder id
	IntVB nEntries;					// actual number of valid entries in the following array
	struct {
		IntVB nAcrNumber;			// ACR number
		IntVB nTimezone;			// timezone at this ACR (0 == Never Allowed, overrides access level)
	}nList[MAX_ACR_PER_SCP];		// alow for max entries
} CC_ACCEXCEPTION;

typedef struct				// (C_122) - Reader Led Function Spec
{	long  lastModified;			// update tag
	IntVB scp_number;			// SCP number
	IntVB led_mode;				// led mode this setting applies to (1, 2, or 3 valid)
	IntVB rled_id;				// reader LED/buzzer function ID (1 to 8, or 11 to 17)
	IntVB on_color;				// color during "on_time" (0=off,1=red,2=green,3=amber)
	IntVB off_color;			// color to display during "off_time"
	IntVB on_time;				// duration to display "on_color" (.1sec/tick)
	IntVB off_time;				// duration to display "off_color" (.1sec/tick)
	IntVB repeat_count;			// if transient: number of times to repeat cycle
	IntVB beep_count;			// if transient: number of times to beep
	// optional (new) arguments: index to LCD text table - use zero to stay with the default
	IntVB rdiLine1;				// text index for Line1 (top)   - use zero to stay with the default
	IntVB rdiLine2;				// text index for Line2 (lower) - use zero to stay with the default
} CC_RLEDSPC;
//  Notes:
//  "led_mode" is set by Command-115, CC_ACR::default_led_mode,
//     ...and it is also set by Command-315 CC_ACRLEDMODE::led_mode
//  "rled_id" codes define when the following is to be applied
//    reader mode based display codes (steady state)
//      1 - disabled			- reader mode (Command 308) set to '1'
//      2 - unlocked 			- reader mode (Command 308) set to '2'
//      3 - locked 				- reader mode (Command 308) set to '3'
//      4 - facility 			- reader mode (Command 308) set to '4'
//      5 - card only 			- reader mode (Command 308) set to '5'
//      6 - pin only			- reader mode (Command 308) set to '6'
//      7 - card and pin 		- reader mode (Command 308) set to '7'
//      8 - pin or card			- reader mode (Command 308) set to '8'
//    transient display codes
//     11 - rled_deny           - rejecting an access request
//     12 - rled_admit          - granting an access request
//     13 - rled_ucmd           - prompting for user command entry
//     14 - rled_2ndcard		- prompting for 2'nd card
//     15 - rled_2ndpin			- prompting for 2'nd PIN
//     16 - rled_wait           - wait (for host response)
//
//
// text line index assignment used by CC_RTXTSPC::nLineIndex 
// and referenced by CC_RLEDSPC::rdiLine1/2, and also used by BURG related extended user commands
//      /-- default text -\           /- line index and description
//	"    HH:MM AM    "		// 01 - rdiCLOCK ("HH:MM AM")
//	"Locked          "		// 02 - rdiLOCKED
//	"Unlocked        "		// 03 - rdiUNLOCKED
//	"Ready           "		// 04 - rdiREADY
//	"Enter PIN       "		// 05 - rdiPINREQ
//	"Invalid         "		// 06 - rdiDENY
//	"Valid           "		// 07 - rdiADMIT
//	"Enter Command   "		// 08 - rdiUCMD
//	"Next Request    "		// 09 - rdi2NDCRD
//	"Enter Next PIN  "		// 10 - rdi2NDPIN --- not used?
//	"?               "		// 11 - rdiPROMPT
//	"Command Accepted"		// 12 - rdiCMD_OK
//	"Not Authorized  "		// 13 - rdiCMD_EA
//	"Invalid Data    "		// 14 - rdiCMD_ED
//	"Timed Door Open:"		// 15 - rdiTMD_OPEN1
//	"  0:00 to Alarm "		// 16 - rdiTMD_OPEN2
//	"...             "		// 17 - rdiWAIT
//	"Biometric Test  "		// 18 - rdiBIORQ
//	"Enter Badge     "		// 19 - rdiCARDRQ
//#

typedef struct				// (C_123) - Reader LCD Text config
{	IntVB scp_number;			// SCP number
	IntVB nMaxLines;			// max number of text lines
	long  nArg;					// command argument, set to zero for now - to be defined later
	IntVB nLineIndex;			// index (zero-based) of this line
	char  cTextLine[16+1];		// text line: ASCI string - zero terminated string
    IntVB nLanguageIndex;       // The language number to which this string should apply, 0-based, with 0 being the default language
} CC_RTXTSPC;

// General Discussion about the asset management feature:
//    Assets are store in a sepaprate database, keyed by asset ID.
//    Asset records may have several pointers to Owners (Cardholders)
//    The CC_ASDBS struture defines the fields that will be used (stored) in an asset record.
// Asset IDs will be passed as an array of right justified hex characters (in the C structure)
// --- in the command string form, they will be passed as a list of (MAX_ASSET_SIZE) numbers
//     using standard C notation, allowing the usage of decimal, octal, or hex representation


// Biometric Database Specifications
// SIO Types for the Biometric Gateways:
#define SIO_MODEL_RSI		(0x62)		// - RSI_HandKey-II system
#define SIO_MODEL_IDX		(0x63)		// - Identix FingerScan system
#define SIO_MODEL_BSCR		(0x64)		// - BioScrypt System
#define SIO_MODEL_IRD		(0x65)		// - Recognition Source (Wyreless) Gateway

// Biometric Types used for template database and loadin
#define BIO_TYPE_RSI		(101)		// - RSI_HandKey-II system
#define BIO_TYPE_IDX		(102)		// - Identix FingerScan system
#define BIO_TYPE_BSCR		(103)		// - BioScrypt System
#define BIO_TYPE_IRD		(104)		// - Iridian Technologies
#define BIO_TYPE_OSDP_1		(105)		// - OSDP biometric type 1
#define BIO_TYPE_OSDP_2		(106)		// - OSDP biometric type 2
#define BIO_TYPE_OSDP_3		(107)		// - OSDP biometric type 3
#define BIO_TYPE_OSDP_4		(108)		// - OSDP biometric type 4


// Command Descriptions:
// UCMND_XTEND: on an authorized command entry with valid parameters, the command handler generates
//       an extended door unlock command (CC_UNLOCK) with the held open time (in 2 second/unit) set to:
//       = (3-digit command argument) * nArgs[0], and pre-alarm time set to nArgs[1]
// UCMND_DIRECT: on an authorized command entry with valid parameters, the command handler generates
//       a procedure execute command (CC_PROCEDURE), of procedure number computed as follows:
//       = nArgs[0] + AcrNumber*nArgs[1] + (1-digit command argument). The command code is 2 == EXECUTE.
//

typedef struct				// (C_1141) - User Command Configuration
{	long lastModified;
	IntVB scp_number;			// scp number
	IntVB nUCmndId;						// UCMND_xxx: ID of this User Command
#define	UCMND_XTEND	    (1001)			// Command ID for Set Extended Door Time (3-digit time)
#define	UCMND_DIRECT	(1002)			// Command ID for Direct Procedure Execute Command (1-digit action code)
	char cCmndCode[MAX_UCMND_NAME+1];	// string: to invoke this command 
										// if the first char is NOT '0' to '9', then process the cAcrListOnly!
										// --- to allow alteration of cAcrList without affecting the other  settings
	char cAcrList[MAX_ACR_PER_SCP+1];	// string: ACRs that can accept this command: 0==NO,1==YES,2==DoNotChange
										// for ACR n: if (cAcrList[n] & 1) { accept } else { not_accept }
	IntVB nIdMode;						// method of identification rq'd (encode as ACR mode)
										// -- 1 = locked == command disabled
										// -- 2 = unlocked == no identification rq'd
										// -- 5-8 = standard controlled access modes

	long nAuthorityFlags;				// Id and Command Authority Requirement
#define UCAC_ACRMODE	(0x001)			// - use the current ACR mode if it is "controlled"
#define UCAC_VALID		(0x002)			// - accept any valid user record in database
#define UCAC_ADMIT		(0x004)			// - user must have access rights "now" (any access level)

	IntVB nAccessDelay;					// may use last access at ACR if it was less than nAccessDelay seconds ago
	IntVB nUsrLevel;					// ((user_level_index*256)+(user level to match)) (-1 == do not test user_level)
	IntVB nAccLevel;					// index to cardholder's access level entry to use:
										// the TZ in this access level entry must be ACTIVE (-1==do not test)
	long nArgs[MAX_UCMND_ARGS_CONFIG];	// command dependent argument block (dwrd)
										// for nUCmndId==UCMND_XTEND:
										//    nArgs[0] is used to multiply the user entered 3-digit number 
										//         to create the held open time (in 2 second ticks)
										//         example: value of 30 will make user's entry into minutes
										//    nArgs[1] is the pre-alarm time, in 2 second units
										//    nArgs[2] is the low limit: entry must be same or larger
										//    nArgs[3] is the high limit: entry must be less than
										// for nUCmndId==UCMND_DIRECT:
										//    nArgs[0] is the BASE Procedure Number
										//    nArgs[1] is the ACR multiplier
										//    nArgs[2] is the low limit: entry must be same or larger (0-9)
										//    nArgs[3] is the high limit: entry must be less than (0-9)
										//    Example: Handle commands "101", "102", "103", "104", "105",
										//      This will need 5 procedure entries per ACR. Start with Procedure-100.
										//      Then set: cCmndCode[]={"10"}; nArgs[]={100, 5, 1, 5};
										//      Then, commands "*101#" through "*105#" at ACR-17 will execute
										//      Procedure-(100 + 17*5 + (n-1)):
										//		101->Proc-185, 102->Proc-186, 103->Proc-187, 104->Proc-188, 105->Proc-189
}CC_UCMND;


// this command specifies the text to be shown on the lower line of the display while the ACR is idle
typedef struct				// (C_1144) User Command Acr Specific Background Text config
{	IntVB nScpId;				// SCP number
	IntVB nAcr;					// the ACR this command applies to 
	IntVB nBkgdSpec;			// specify the background display operation
								// bit-0 = change settings. (This bit must be set for this field to be processed)
								// bit-1 = clear all background text line buffers
								// bit-2 = display the time of day
								// bit-3 = selects the time display mode: 0 == 12 hour AM/PM, 1 == 24-hour mode
								// bit-4 = display the User Specified Background text (cycle through all valid entries)
								// bit-5 = display the Status of the MPG associated with this Acr
								// bit-6 = display the Name of Active Points
								// bit-7 = select "slow" update cycle time (3 sec vs. 2 seconds)
	IntVB nBkgdIndex;			// selects which line to load. 0 to 7 are valid, otherwise no text is loaded
	char  cBkgdText[16];		// text to show for that line. A line where cBlgdText[0] == '\0' is not displayed
}CC_UCMND_BKGD;

typedef struct				// (C_203)
{	IntVB dummy;				// shutdown does not require arguments
}CC_SCP_DOWN;

typedef struct				// (C_206)
{	IntVB scp_number;			// SCP number
	char  file_name[200];		// (path) file name to the .bin file
} CC_FIRMWARE;

typedef struct				// (C_207, C_208) Attach/Detach SCP to channel
{	IntVB nSCPId;				// SCP number
	IntVB nChannelId;			// channel ID (as assigned during enCcCreateChannel)
	IntVB nToAltPort;			// Not used, set to 0
} CC_ATTACHSCP;

typedef struct				// (C_209)
{	char  file_name[100];		// (path) file name to the file to save to
	IntVB nMode;				// save mode: 0, 1, 2, or 3
//		   nMode == 0 >>> save ALL settings (system and all Scp's, nScpId is ignored)
//		   nMode == 1 >>> save ONLY the system settings (nScpId is ignored)
//		   nMode == 2 >>> save only the settings for the Scp specified by "nScpId"
//         nMode == 3 >>> save the 'skeleton' configuration parameters only for the specified SCP
	IntVB nScpId;				// if nMode == 2, specified the SCP to be saved
} CC_CONFIGSAVE;

typedef struct				// (C_210)
{	char  file_name[100];		// (path) file name of the config "delta" file
} CC_CONFIGDELTA;

typedef struct				// (C_211) (enCcDualPortControl)
// Notes: the "active" port accepts configuration and transaction retrieval
// the "standby" port confirms operation, can read status, cannot control or set
// this command will return a "enSCPReplyDualPort" reply message
{	IntVB scp_number;			// SCP number
	IntVB nHcpDriver;			// host comm driver number (0==primary, 1==alternate)
	IntVB command;				// 0 - NOP (status request only)
								// 1 - set to "standby" (clear the "active" state of this port)
								// 2 - if the other port in not "active" set this to the "active" port
								// 3 - set this as the "active" port (other port becomes standby)
								// overloaded for autosave control
								// 4 - save the controller configuration (no cardholders)
								// 5 - save the controllers database
								// 127 - clear card database
} CC_DUALPORTCONTROL;

typedef struct				// (C_212) Encryption Control Command
{	IntVB nScpId;
	IntVB nAesCommand;			// command code
#define AESCMND_OFF		(0)		// - disable encryption to this SCP
#define AESCMND_SET_MK1	(1)		// - Load Master Key 1
#define AESCMND_SET_MK2	(2)		// - Load Master Key 2
#define AESCMND_ENA_MK1	(3)		// - enable encryption to this SCP, using Master Key 1
#define AESCMND_ENA_MK2	(4)		// - enable encryption to this SCP, using Master Key 2
#define AESCMND_ENA_TST (5)		// - enable TEST MODE: std encryption protocol, plain text, use Mk-1
#define AESCMND_NEW_SK	(6)		// - set new session key
#define AESCMND_MK12SCP	(7)		// - transfer Master Key 1 to the Scp
#define AESCMND_MK22SCP	(8)		// - transfer Master Key 2 to the Scp
#define	AESCMND_ENA_MK256 (9)	// enable encryption to this EP, using AES 256 (with Master Keys 1 and 2 as a 256 bit Master Key).
	char  cAesKey[128/8];		// optional field: 128-bit "Master Key"
								// --- In text mode, use two Ascii Characters per byte notation:
								// --- example: for nAesKey[]={0x12,0x34,...}, use "1234..."
}CC_AESCTL;

typedef struct				// (C_127) SIO Encryption Control Command
{	IntVB nScpId;
	IntVB sio_number;
	IntVB nAesCommand;			// command code
#define AESCMND_SIO_OFF				(0)		// - disable encryption to this SIO
#define AESCMND_SIO_SET_MK			(1)		// - Load Master Key (Secret Key)
#define AESCMND_SIO_ENA_MK			(2)		// - enable encryption to this SIO, using Master (Secret) Key
#define	AESCMND_SIO_ENA_PKI			(3)		// - enable encryption to this SIO, using Public Key Infrastructure
#define	AESCMND_SIO_ENA_ANY 		(4)		// - enable encryption using any available key (precedence: PKI, Secret, Default)
#define	AESCMND_SIO_ENA_ANYORNONE	(5)		// - enable encryption using any available key; revert to plaintext if all keys fail.
#define AESCMND_SIO_ENA_TST 		(6)		// - enable TEST MODE: std encryption protocol, plain text, use Secret Key
#define	AESCMND_SIO_TST_KEYS		(7)		// - test all keys (via new session key generation)
#define AESCMND_SIO_NEW_SK			(8)		// - set new session key
#define AESCMND_SIO_MK2SIO			(9)		// - transfer secret key to SIO
#define AESCMND_SIO_SEC_CH_AES256	(10)	// - enable encryption using AES256
	char  cAesKey[128/8];		// optional field: 128-bit "Secret Key"
								// --- In text mode, use two Ascii Characters per byte notation:
								// --- example: for nAesKey[]={0x12,0x34,...}, use "1234..."
}CC_SIOAESCTL;

typedef struct				/* (C_128) SIO Network configuration */
{	IntVB nScpId;			/* SCP number this SIO is attached to */
	IntVB sio_number;		/* ...SIO number on this SCP */
	char  cIpAddr[22];		/* null terminated string with IP address, dotted quad notation (192.168.0.251) */
	char  cMacAddr[18];		/* null terminated string with MAC Address six groups of two hexadecimal digits, */
							/* separated by colons (:) such as 00:0F:E5:00:06:3E  */
	short nMode;			/* Addressing Mode.  0 = Mercury DHCP, 1 = Public DHCP , 2 = static IP */
	char  cHostname[64];
	IntVB iPort;			/* Network Port 0-65535 */
} CC_SIO_NETWORK;


typedef struct				// (C_214) Set the "Demand Poll" mode for this SCP
{	IntVB nScpId;				// SCP number
	IntVB nPollMode;			// poll mode
#define DPOLL_ENABLE	(0)		// -- enable demand poll: transaction retrieval has priority (default mode)
#define DPOLL_DISABLE	(1)		// -- disable demand poll: user commands have priority
#define DPOLL_ALTERNATE	(2)		// -- alternate the priority between polls and user commands
}CC_POLLMODE;
// Notes:
// The initial state of the polling mode is set to DPOLL_ENABLE when the SCP is attached.
// This command can be set any time after attaching the SCP, and it stays in effect 
// until the polling mode is altered by re-issuing this command with a new value.

typedef struct				// (C_215 & C_216) Generic file download and upload
{	IntVB scp_number;			// SCP number
	IntVB file_type;			// type of file to load, 0 = Certificate, 1 = user defined, 2 = license, 3 = peer cert
	char src_file[255 + 1];	// (path) file name to the file on the host
	char dest_file[255 + 1];	// destination file name for upload
	char filePass[128 + 1];	// Password for cert chains
}CC_FILETRANSFER;

typedef struct				// (C_217) Raw byte output
{	IntVB scp_number;			// SCP number
	IntVB data_len;				// data length
	byte  data[2048];			// byte array
} CC_HEX_OUT_INTERNAL;

typedef struct				// (C_218) Delete file
{
	IntVB scp_number;			// SCP number...
	IntVB file_type;				// type of file to delete
	char file_name[100];		// local file name
} CC_DELETE_FILE;

typedef struct			//(C_219) return list of files
{
	IntVB scp_number;	// SCP number
	IntVB file_type;	// type of files to return
} CC_FS_FILE_INFO;

typedef struct				// (C_220)
{	IntVB scp_number;			// SCP number
	char  file_name[200];		// (path) file name to the App bundle file
} CC_APP;

typedef struct				// (C_301)
{	IntVB scp_number;			// SCP number
} CC_RESET;

typedef struct				// (C_302)
{	IntVB scp_number;			// SCP number
	UINT32 custom_time;
} CC_TIME;
typedef struct				// (C_303)
{	IntVB scp_number;			// SCP number
	long tran_index;			// transaction index to report from
								// special:
								//  -1 to disable reporting,
								//  -2 to enable reporting from "last_reported"
} CC_TRANINDEX;
typedef struct				// (C_3305)
{	IntVB scp_number;			// SCP number
	__int64 cardholder_id;		// cardholder to remove from this SCP
} CC_CARDDELETEI64;

typedef struct              // (C_343)
{
	IntVB scp_number;       // SCP number
} CC_CARDCLEANUP;

typedef struct				// (C_306)
{	IntVB scp_number;			// SCP number
	IntVB mp_number;			// MP number
	IntVB set_clear;			// mask control: non-zero to set mask
} CC_MPMASK;

typedef struct				// (C_307)
{	IntVB scp_number;			// SCP number
	IntVB cp_number;			// CP number
	IntVB command;				// command code: 1==off, 2==on, 3==single pulse, or 4==repeating puse
	IntVB on_time;				// on time - used for mode 3 or 4
	IntVB off_time;				// off time - used for mode 4 only
	IntVB repeat;				// on/off repeat count - used for mode 4 only
} CC_CPCTL;


typedef struct				// (C_308)
{	IntVB scp_number;			// SCP number
	IntVB acr_number;			// ACR number
	IntVB acr_mode;				// access mode: 1 through 8
								//   1 == disable the ACR
								//   2 == unlock (unlimited access)
								//   3 == locked (no access, REX active)
								//   4 == facility code only
								//   5 == card only
								//   6 == PIN only
								//   7 == card and PIN
								//   8 == card or PIN
								//  16 == disable "2-card" mode: clear actl_flags::ACR_F_2CARD
								//  17 == enable  "2-card" mode: set actl_flags::ACR_F_2CARD
								//  26 == clear   "ACR_FE_NO_ARQ" flag
								//  27 == set     "ACR_FE_NO_ARQ" flag
								//  29 == set     "link mode": set extended actl_flags::ACR_FE_LINK_MODE
								//  30 == abort   "link mode": clear extended actl_flags::ACR_FE_LINK_MODE
								//  31 == change Extended features specified in nExtFeatureType and nExtFeatureData
								//  32 == set     "link mode Alternate": set extended actl_flags::ACR_FE_LINK_MODE_ALT
								//  33 == abort   "link mode Alternate": clear extended actl_flags::ACR_FE_LINK_MODE_ALT
	long  nAuthModFlags;		// Not used, set to 0
	IntVB nExtFeatureType;		// Ext. Feature Type (0=None, ...) and Feature info. Used only with acr_mode 31
	union
	{
		struct
		{
			IntVB	iIPB_sio;			// SIO ID for Int. Push Button (Not needed for native locksets)
			IntVB	iIPB_number;		// Input number for Int. Push Button (Not needed for native locksets)
			IntVB	iIPB_long_press;	// IPB long-press 0-15 sec. (if applicable)
			IntVB	iIPB_out_sio;		// SIO ID for IPB indicator output (Not needed for native locksets)
			IntVB	iIPB_out_num;		// Output number for IPB indicator output (Not needed for native locksets)
		} sIPBoverrides;
	} uExtFeatureInfo;

} CC_ACRMODE;

typedef struct				// (C_309)
{	IntVB scp_number;			// SCP number
	IntVB acr_number;			// ACR number
	IntVB set_clear;			// forced open mask control: non-zero to set mask
} CC_FOMASK;

typedef struct				// (C_310)
{	IntVB scp_number;			// SCP number
	IntVB acr_number;			// ACR number
	IntVB set_clear;			// held open mask control: non-zero to set mask
} CC_HOMASK;

typedef struct				// (C_311)
{	IntVB scp_number;			// SCP number
	IntVB acr_number;			// ACR number
	IntVB floor_number;			// floor index, 0-based (for elevator readers only)
	IntVB strk_tm;				// 0, or strike time - in seconds
	IntVB t_held;				// 0, or held open time - in 2 second ticks
	IntVB t_held_pre;			// 0, or held open time pre-alarm - in 2 second ticks
} CC_UNLOCK;

typedef struct				// (C_312)
{	IntVB scp_number;			// SCP number
	IntVB proc_number;			// Procedure number
	IntVB command;				// procedure command:
								//  1 = abort a delayed procedure
								//  2 = execute (prefix    0 actions)
								//  3 = resume a delayed procedure
								//  4 = execute (prefix  256 actions)
								//  5 = execute (prefix  512 actions)
								//  6 = execute (prefix 1024 actions)
								//  7 = resume  (prefix  256 actions)
								//  8 = resume  (prefix  512 actions)
								//  9 = resume  (prefix 1024 actions)
} CC_PROCEDURE;

typedef struct				// (C_313)
{	IntVB scp_number;			// SCP number
	IntVB tv_number;			// Trigger Variable number (1 to 63)
	IntVB set_clear;			// TV control: non-zero to set it
} CC_TVCOMMAND;

typedef struct				// (C_314)
{	IntVB scp_number;			// SCP number
	IntVB tz_number;			// Timezone number (1 to n)
	IntVB command;				// 1==TmpClr, TmpSet, OvrClr, OvrSet, Release, Refresh
} CC_TZCOMMAND;

typedef struct				// (C_315)
{	IntVB scp_number;			// SCP number
	IntVB acr_number;			// ACR number (0 to n)
	IntVB led_mode;				// 1, 2, or 3 sets LED table index
} CC_ACRLEDMODE;

typedef struct				// (C_316)
{	IntVB scp_number;			// SCP number
	IntVB oem_code;				// oem code to set (upper 12-bits of the serial number)
} CC_OEMCODE;

typedef struct				// (C_317)
{	IntVB scp_number;			// SCP number
	char password[16];			// password for establishing comm
} CC_PASSWORD;

typedef struct				// (C_318)
{	IntVB scp_number;			// SCP number
	IntVB scp_id;				// id to set, for reporting in the ID reply
} CC_SCPID;

typedef struct				// (C_3319)
{	IntVB scp_number;			// SCP number
	__int64  cardholder_id;		// -1 == all cardholders, else only the specified id
	IntVB apb_area;				// 0 == free pass, 1-127 == specific area to set
	IntVB flags;				// bit-0 = Do not fail if card does not exist, skip
} CC_APBFREEPASSI64;
typedef struct				// (C_320) Raw character output
{	IntVB scp_number;			// SCP number
	long  baud;					// baud rate (2400, 9600, 19200, 38400)
	IntVB port;					// MSP1 port number (currently, only 0 is valid)
	IntVB channel;				// 0, 1, 2, or 3
	char  data[128+1];			// null terminated string, 2 chars per byte (up to 64 bytes)
} CC_HEXOUT;

//  A variation of enCcHexOut enables Access Activity Log.
//  Once this command is loaded, access requests are formatted per this spec and sent out
//  as specified by <baud>, <port>, <channel> in this command.
//  the hex data output format is specified as follows:
//  - the first hex character is set to "FF" to designate this as a "format control command"
//  - hex character "FE" is replaced with <YYYY Mmm dd, hh:mm> example : "2000 Feb 24, 17:24"
//  - hex character "FD" is replaced with the ScpId (as set by enCcScpID)
//  - hex character "FC" is replaced with the ACR number where the access request is pending
//  - hex character "FB" is replaced with the (TransactionType*10+TracnsactionCode)
//  - hex character "FA" is replaced with the CardholderId
//  - any other character is passed through

typedef struct				// (C_321) Monitor Point Group arm/disarm command
{	IntVB scp_number;			// SCP number
	IntVB mpg_number;			// Monitor Point Group number
	IntVB command;				// command
								// 1 - access: if(!mask_count){mask all MPs}mask_count++;
								// 2 - override: set mask_count to "arg1", further...
								//   --- if arg1 is zero then all points get unmasked
								//   --- else (if arg1 is not zero) then all points get masked
								// 3 - force-arm:
								//		if(mask_count > 1) { mask_count-- }
								//		else if (mask_count == 1 ) {
								//			unmask all non-active MPs; mask_count = 0;
								//		}
								// 4 - arm
								//		if(mask_count > 1) { mask_count-- }
								//		else if (mask_count == 1 && no MPs  are active ) {
								//			unmask all MPs; mask_count = 0;
								//		}
								// 5 - override arm:
								//		if(mask_count > 1) { mask_count-- }
								//		else if (mask_count == 1 ) {
								//			unmask all MPs; mask_count = 0;
								//		}
	IntVB arg1;					// command dependent argument: see "command 2"
} CC_MPGSET;

typedef struct				// (for "action_15") Monitor Point Group::mask_count test
{	IntVB scp_number;			// SCP number
	IntVB mpg_number;			// Monitor Point Group number
	IntVB action_prefix_ifz;	// value to set the action type prefix to if mask_count == 0
	IntVB action_prefix_ifnz;	// value to set the action type prefix to if mask_count != 0
								// Note: this command may modify the "action_type" prefix that
								//  determine which of the following actions execute
} CC_MPGTESTMASK;

typedef struct				// (for "action_16") Monitor Point Group: active point test
{	IntVB scp_number;				// SCP number
	IntVB mpg_number;				// Monitor Point Group number
	IntVB action_prefix_ifnoactive;	// value to set the action type prefix to if no points are active
	IntVB action_prefix_ifactive;	// value to set the action type prefix to if at least one active
									// Note: this command may modify the "action_type" prefix that
									//  determine which of the following actions execute
} CC_MPGTESTACTIVE;

typedef struct				// Area Set (C_322, and "action_17")
{	IntVB scp_number;			// SCP number
	IntVB area_number;			// access area number (0 to 127)
	IntVB command;				// command code
								// - 1 = disable Access
								// - 2 = enable Access,
								// - 3 = set occupancy count (sets count of "standard" users)
								// --- control codes 4 through 9 support the Special 1-Man/2-Man Rules
								// - 4 = set occupancy count of the "special" users)
								// - 5 = clear occupancy counts of the "standard" and of the "special" users
								// - 6 = multi-occupancy mode control: disable multi-occupancy rules
								// - 7 = multi-occupancy mode control: enable standard multi-occupancy processing
								// - 8 = multi-occupancy mode control: enable "Modified 1-Man Rule" processing
								// - 9 = multi-occupancy mode control: enable "Modified 2-Man Rule" processing
	long  occ_set;				// set occupancy count to this value (if command==3 or ==4)
} CC_AREASET;
typedef struct				// Use Limit Set (C_3323, not implemented via "action" )
{	IntVB scp_number;			// SCP number
	_int64 cardholder_id;		// -1 == all cardholders, else only the specified id
	IntVB new_limit;			// 0==none, 1 through 254 valid use counts, 255==unlimited
} CC_USELIMITI64;
typedef struct				// Set off-line time (C_324, not implemented via "action" )
{	IntVB scp_number;			// SCP number
	IntVB offline_time;			// in units of milliseconds, 0==default (2000 milliseconds)
} CC_OFFLINETIME;				// send before hanging up on the SCP, allow quick disconnect

typedef struct				// Temp Reader LED control (C_325)
{	IntVB scp_number;			// SCP number
	IntVB acr_number;			// (word) access control reader number
	IntVB color_on;				// color code to use during on-time.  For Lockset interior pushbutton, for Interior Pushbutton (IPB) color on, shift 2 bits left (mult x 4) and add to this value.  Example: IPB Red-on is color 4.
	IntVB color_off;			// color code to use during 'off-time'. For Lockset interior pushbutton, for Interior Pushbutton (IPB) color off, shift 2 bits left (mult x 4) and add to this value.  Example: IPB green-off is color 8.
	IntVB ticks_on;				// ticks for on-color, .1 sec per tick
	IntVB ticks_off;			// ticks for off-color, .1 sec per tick
	IntVB repeat;				// number of times to do the on/off cycle.  Setting hi-bit (OR-in 0x80 or adding 128), combined with and IPB color command, signifies vendor-specific LED instead.
	IntVB beeps;				// beep count
    IntVB LED_number;           // 0 = standard, 1 = lightframe
    long color_on_RGB;
    long color_off_RGB;
    IntVB flags;                // bit-0 indicates to use RGB colors
} CC_RLED_TMP;

typedef struct				//	Text output to an LCD terminal (C_326)
{	IntVB scp_number;			// SCP number
	IntVB term_number;			// ACR (terminal) number
	IntVB type;					// text command type: 1==permanent, 2==temp, 3==offline text (not used), 4==Indexed, language-specific permanent text, 5==Indexed, language-specific temp text
	IntVB temp_time;			// duration for temp text display: 1 to 31 sec
	IntVB tone;					// Not used, set to 0
	IntVB tone_time;			// Not used, set to 0
	IntVB row;					// text location: row number (0=top row)
	IntVB column;				// text location: column number (0=left-most)
	char  text[64];				// <null> terminated ASCII text
    IntVB nLineIndex;			// index (zero-based) of this line
    IntVB nLanguageIndex;       // The language number to which this string should apply, 0 = language of current cardholder (or default language if no languages defined). 1-15 = explicit language. 255 = explicitly use controller default language.
}CC_LCDTEXT;

typedef struct				//	Diagnostic Use Only! - Direct Command to an SIO (C_327)
{	IntVB scp_number;				// SCP number
	IntVB sio_number;				// sio number
	long  cmnd_tag;					// command tag - arbitrary integer value
	IntVB cmnd_id;					// command code
	char  data[MAX_SIODC_DATA*2+1];	// null terminated string, 2 chars per byte
}CC_SIODC;

typedef struct				//	Hex Load Output to an SIO (C_328)
{	IntVB scp_number;				// SCP number
	IntVB sio_number;				// sio number
	char  file_name[200];			// <path/name> of the hex file
}CC_SIOHEX;

typedef struct				// send the host response to tranTypeCardFulll-TranCode==10 (C_329)
{	IntVB scp_number;							// SCP number
	IntVB acr_number;							// ACR number
	IntVB command;								// 0==deny access, 1==allow access
	__int64 cardholder_id;						// cardholder
	byte enc_id_length;							// Length Encoded ID, 0 if not an encoded ID
	unsigned char enc_id[MAX_FREEFORM_SIZE];	// Large encoded ID array
}CC_HOSTRESPONSE;

typedef struct				// send a NonVolatile Argument to an SCP or to one of its SIOs (C_330)
{	IntVB nScpId;					// SCP number
	IntVB nNvArgType;				// specifies which NV argument is beig set
									// 13 = SCP's OEM code, (11, 12, and 14 are reserved for factory settings)
									// 33 = SIO's OEM code, (31, 32 and 34 are reserved for factory settings)
	IntVB   nSioNumber;				// SIO number (if applicable, else it is ignored)
	__int64 nNvArgValue;			// the argument value to set
	long    nNvAuthCode;			// authorization code to set this Argument
	IntVB   nNvDataLen;				//
	char data[MAX_NV_DATA_SIZE];
}CC_NV_ARG_SET;

typedef struct				// send a host simulated card read (C_331)
{	IntVB nScp;						// SCP number
	IntVB nCommand;					// 1 == process this as card data input
	IntVB nAcr;						// ACR number
	long  e_time;					// time, in elapsed seconds. Scp time must be less than this time!
	IntVB nFmtNum;					// card format number
	long  nFacilityCode;			// facility code
	__int64 nCardholderId;			// cardholder id
	long nIssueCode;
}CC_CARD_SIM;


typedef struct				// (C_333)
{	IntVB nScpId;				// SCP number
	long diag_code;				// diagnostic code
#define DIAG_FORCE_EXCEPTION	(0)
#define DIAG_BULK_ERASE			(1)
} CC_DIAG;

typedef struct					// (C_334)
{	IntVB scp_number;			// SCP number
	IntVB acr_number;			// ACR number
	IntVB acr_mode;				// access mode: 1 through 8
								//   1 == disable the ACR
								//   2 == unlock (unlimited access)
								//   3 == locked (no access, REX active)
								//   4 == facility code only
								//   5 == card only
								//   6 == PIN only
								//   7 == card and PIN
								//   8 == card or PIN
								//	28 == change auth mod flags specified in nAuthModFlags
	IntVB time;					// time in minutes, bits 0-13 (Upper 2 bits indicate: 00 = number of minutes, 01 = number of minutes since last midnight, 10 = indefinite override)
	long nAuthModFlags;			//  Not used, set to 0
} CC_TEMP_ACR_MODE;

typedef struct					// (C_335)
{	IntVB scp_number;			// SCP number
	IntVB oper_mode;			// Operating mode to set
	IntVB enforce_existing;		// 0 = do not enforce, 1 = enforce
	IntVB existing_mode;		// Existing operating mode
} CC_OPER_MODE;


typedef struct				// ACR OSDP Passthrough (C_337)
{	IntVB	scp_number;		// SCP Number
	IntVB	acr_number;		// ACR number
	long	sequence_num;	// Sequence number for matching commands to their responses
	IntVB	reader_role;	// 0 for primary reader, 1 for alternate reader on the ACR
	IntVB	msg_type;		// 0 for osdp_MFG, 1 for osdp_XWR
	IntVB	data_len;		// Length of the nData message;
	byte	data[MAX_OSDP_PASSTHROUGH];		// Data portion of message
} CC_ACR_OSDP_PASSTHROUGH;


typedef struct				// ACR Offline Access List (C_338)
{	IntVB	scp_number;		// SCP number
	IntVB	acr_number;		// ACR number
	IntVB	protocol;		// protocol of ACR (simons voss, aperio, etc)
	IntVB	action;			// action code
	IntVB	params_len;		// length of action params
	byte	params_data[MAX_OAL_PARAMS_SIZE];	// action parameters based on action type (unsigned to not get FFFFFF added during conversion)
} CC_ACR_OAL;

typedef struct				// Host Simulated Keys (C_339)
{	IntVB scp_number;		// SCP number	
	IntVB nAcr;				// ACR number
	long  e_time;			// time, in elapsed seconds. Scp time must be less than this time!
	char  keys[MAX_SIM_KEY_SIZE];
} CC_KEYS_SIM;

typedef struct				// OSDP transfer to readers (C_340)
{	IntVB scp_number;		// SCP number	
	byte  acr_pri[16];		// Bitmap for primary ACR readers to transfer file to
	byte  acr_alt[16];		// Bitmap for alternate ACR readers to transfer file to
	char src_file[100];		// OSDP source file on controller
} CC_OSDP_TRANSFER;

typedef struct				// Control Reboot (C_341)
{
	IntVB scp_number;		// SCP number	
	IntVB sio_number;		//SIO Number of reader list
	IntVB target_type;      // target type
	IntVB reset_type;       // reset type
	byte  ctl_list[16];		// Bitmap for Controls to reboot
} CC_CTL_REBOOT;

typedef struct				// send a host simulated card read raw (C_342)
{
	IntVB nScp;						// SCP number
	IntVB nCommand;					// 1 == binary, 2 == foward nibble, 3 == reverse nibble
	IntVB nAcr;						// ACR number
	long  e_time;					// time, in elapsed seconds. Scp time must be less than this time!
	IntVB count;					// number of bits/nibbles
	char  raw_data[100];			// raw data
}CC_CARD_SIM_RAW;

typedef struct				// send a command requesting the list of apps
{
	IntVB scp_number;							// SCP number
}CC_SANBX_LIST_APPS;

// sanbx Driver Command Structure for App Action and Control Command
#define SANBX_APP_VER_MAX_LEN 10
typedef struct				// send a command requesting the list of apps
{
	IntVB scp_number;						// SCP number
	UINT32  appCode; //App number
	char  version[SANBX_APP_VER_MAX_LEN]; //App version
	byte nCommand;	//App Action				// 
}CC_SANBX_APP_CMD;

typedef union	_SCP_ACTION		// based on action_type:
{								// -   0: delete all actions from this procedure
								//        (this command is void)
	CC_MPMASK  mp_mask;			// -   1: MP mask
	CC_CPCTL   cp_ctl;			// -   2: CP control
	CC_ACRMODE acr_mode;		// -   3: ACR mode control
	CC_FOMASK  fo_mask;			// -   4: Forced Open Mask control
	CC_HOMASK  ho_mask;			// -   5: Held Open Mask control
	CC_UNLOCK  unlock;			// -   6: ACR unlock timed
								// -  22: unlock-timed with execution-time supplied held open delay
	CC_PROCEDURE proc;			// -   7: Procedure execution control
	CC_TVCOMMAND tv_ctl;		// -   8: Trigger Variable control
	CC_TZCOMMAND tz_ctl;		// -   9: Timezone control
	CC_ACRLEDMODE led_mode;		// -  10: Reader LED mode control
	char dial_str[24];			// -  12: Not used
	CC_HEXOUT hexout;			// -  13: Raw hex character output
	CC_MPGSET mpg_set;			// -  14: Monitor Point Group set
	CC_MPGTESTMASK mpg_test_mask;		// -  15: set "action_type" prefix based on "mask_count"
	CC_MPGTESTACTIVE mpg_test_active;	// -  16: set "action_type" prefix based on "active points"
	CC_AREASET area_set;				// -  17: access area control command
	CC_UNLOCK acr_abort;				// -  18: abort the Wait-For-Door-Open state
	CC_RLED_TMP rled_tmp;				// -  19: temporary reader LED control
	CC_LCDTEXT lcd_text;				// -  20: LCD text output
	char dial_str_alt[24];				// -  21: Not used
										// -  22: --- reserved ---
										// -  23: Not used
	CC_TEMP_ACR_MODE temp_acr_mode;		// -  24: Temporary ACR mode control
	CC_CARD_SIM card_sim;				// -  25: Host Simulated Card Read
	CC_USELIMITI64 use_limit;			// -  26: Use Limit
	CC_OPER_MODE oper_mode;				// -  27: Operating Mode
	CC_KEYS_SIM key_sim;				// -  28: Host Simulated Keys
	IntVB unused;						// -  29: Filter Transaction
	CC_BATCH_TRANS batch_trans;			// -  30: Send Batch Summary
	IntVB delay;						// - 127: delay, in 1 second units
										// - 126: delay, in .1 second units
}SCP_ACTION;

typedef struct				// (C_118) Action Specification
{	long  lastModified;			// update tag
	IntVB scp_number;			// SCP number...
	IntVB proc_number;			// procedure number this action assigned to
	IntVB action_type;			// action type
	// The action argument field below is a union of direct commands, defined
	// by the action_type argument above. (The SCP number field in the direct
	// commands is now used for Regional I/O.  If you set it to a different
	// SCP number then the action will be executed on the remote SCP if Regional I/O
	// is properly configured.  If not using Regional I/O, then the SCP number in
	// the direct command should either be set to 0 or to the same SCP number)
	// The "action_type" can have a prefix of 0, 256, 512, and 1024.
	// the prefix restricts when the action executes, based on the command
	// that was used to "start" the procedure.
	// start command - prefix operational rules:
	//   2 - only actions with prefix    0 are executed (default)
	//   4 - only actions with prefix  256 are executed
	//   5 - only actions with prefix  512 are executed
	//   6 - only actions with prefix 1024 are executed
	SCP_ACTION	arg;
} CC_ACTN;

typedef struct				// (C_401)
{	IntVB scp_number;			// SCP number
} CC_IDREQUEST;

typedef struct				// (C_402)
{	IntVB scp_number;			// SCP number
} CC_TRANSRQ;

typedef struct				// (C_403)
{	IntVB scp_number;			// SCP number
} CC_MSP1SRQ;

typedef struct				// (C_404)
{	IntVB scp_number;			// SCP number
	IntVB first;				// first SIO to report
	IntVB count;				// number of SIOs to report (up to 4 ok)
} CC_SIOSRQ;

typedef struct				// (C_405)
{	IntVB scp_number;			// SCP number
	IntVB first;				// first MP to report
	IntVB count;				// number of MPs to report (up to 100 ok)
} CC_MPSRQ;

typedef struct				// (C_406)
{	IntVB scp_number;			// SCP number
	IntVB first;				// first CP to report
	IntVB count;				// number of CPs to report (up to 100 ok)
} CC_CPSRQ;

typedef struct				// (C_407)
{	IntVB scp_number;			// SCP number
	IntVB first;				// first ACR to report
	IntVB count;				// number of ACRs to report (up to 32 ok)
} CC_ACRSRQ;

typedef struct				// (C_408)
{	IntVB scp_number;			// SCP number
	IntVB first;				// first Timezones to report
	IntVB count;				// number of TZs to report (up to 100 ok)
} CC_TZSRQ;

typedef struct				// (C_409)
{	IntVB scp_number;			// SCP number
	IntVB first;				// first Trigger Variable to report
	IntVB count;				// number of CPs to report (up to 100 ok)
} CC_TVSRQ;


typedef struct				// (C_411) Monitor Point Group status request
{	IntVB scp_number;			// SCP number
	IntVB first;				// first Monitor Point Group to report
	IntVB count;				// number of Monitor Point Groups to report
} CC_MPGSRQ;

typedef struct				// (C_412) Access Area status request
{	IntVB scp_number;			// SCP number
	IntVB first;				// first Access Area to report
	IntVB count;				// number of Access Area to report
} CC_AREASRQ;


typedef struct				// (C_415) SIO Relay Count Status request
{	IntVB scp_number;			// SCP number
	IntVB sio_number;			// SIO number
} CC_SIO_RLYCT_SRQ;

typedef struct				// (C_417) SIO HID Serial No  request
{	IntVB scp_number;			// SCP number
	IntVB sio_number;			// SIO number
}CC_SIO_HID_MFG;

// configuration read commands     CONFIG_READ >>>
// generic configuration read command

// the CC_READ structure has been implemented for the following items.
// Use these command types in CFG_CMND::command, with the cfg_read (CC_READ) member of the SCP_CMND union:

// enCcRdScpAdbSpec	  Read the Access Database Specification
// enCcRdScpCfmt      Read the Card formatter configuration
// enCcRdScpTimezone  Read the Time zone configuration             ****
// enCcRdScpHoliday   Read the Holiday configuration               ****
// enCcRdAdbCard      Read this cardholder's record                ****
// enCcRdAccExcpt     Read this cardholder's Access Exception List ****
// enCcRdMP=1811      Read the Monitor point configuration         ****
// enCcRdCP           Read the Control point configuration         ****
// enCcRdACR          Read the Access control reader configuration ****
// enCcRdAlvl         Read the Access level configuration          ****
// enCcRdTrgr         Read the Trigger configuration               ****
// enCcMemRead        Memory Read Request                          ****
// enCcRMS            Remote Memory Storage                        ****
// enCcStrSRq         SCP Structure Status Read Request            ****
// enCcPkgInfo		  Read installed package information
// enCcRdBioTemplate  Read the biometric template

typedef struct
{	IntVB nScpID;				// the SCP's ID
	__int64  nFirst;			// specify which item(s) to return, special options:
								//  0 <= nFirst - start with the item specified (0==first)
								// -1 == nFirst - continue with the 'next' item
								// if item is card record, then nFirst is the cardholder ID
	IntVB nCount;				// number of items to return,
								// ... where zero means to return as many items as fit in the reply
								// if item is Access Database Specification, then nFirst and nCount
								// are ignored.
}CC_READ;


typedef struct
{	IntVB nScpID;				// (C_1852) the SCP's ID
	long  nRmsSize;				// amount of memory to allocate, or zero if this is a remote block write command
								// --- the write command is processed only if nRmsSize is set to zero
	long  nOffset;				// byte offset into the Remote Memory Block for writing
	IntVB nDataLength;			// number of bytes to write
	char  nData[64];			// array of data to write, only the first "nDataLength" bytes are used
}CC_RMS;

typedef struct
{	IntVB nScpID;					// (C_1853) the SCP's ID
	IntVB nListLength;				// - number of structures to read back status for (max 32)
	IntVB nStructId[32];			// - list of SCP structure IDs to read status for
#define SCPSID_TRAN			 (1)	//  1 - transactions
#define SCPSID_TZ			 (2)	//  2 - Timezones
#define SCPSID_HOL			 (3)	//  3 - Holidays
#define SCPSID_MSP1			 (4)	//  4 - SIO ports
#define SCPSID_SIO			 (5)	//  5 - SIOs
#define SCPSID_MP			 (6)	//  6 - Monitor Points
#define SCPSID_CP			 (7)	//  7 - Control Points
#define SCPSID_ACR			 (8)	//  8 - Access Control Readers
#define SCPSID_ALVL			 (9)	//  9 - Access Levels
#define SCPSID_TRIG			(10)	// 10 - Triggers
#define SCPSID_PROC			(11)	// 11 - Procedures
#define SCPSID_MPG			(12)	// 12 - Monitor Point Groups
#define SCPSID_AREA			(13)	// 13 - Access Areas
#define SCPSID_EAL			(14)	// 14 - Elevator Access Levels
#define SCPSID_CRDB			(15)	// 15 - Cardholder Database
#define SCPSID_FLASH		(20)	// 20 - FLASH specs: nRecords==MfgID, nRecSize==BlockSize, nActive==FlashSize
#define SCPSID_BSQN			(21)	// 21 - build sequence number - incremented each build (intended for internal use)
#define SCPSID_SAVE_STAT	(22)	// 22 - flash save status (EP only) (Bit-0: normal, Bit-1: dirty flag, Bit-2:auto save triggered)
#define SCPSID_MAB1_FREE	(23)	// 23 - Memory Alloc Block 1 (Host Config)   free memory
#define SCPSID_MAB2_FREE	(24)	// 24 - Memory Alloc Block 2 (Card Database) free memory
#define SCPSID_ARQ_BUFFER   (26)	// 26 - Access Request Buffers
#define SCPSID_PART_FREE_CNT	(27)	// 27 - Partition Memory Free Info
#define SCPSID_FEATURECODE	(28)	// 28 - Read feature code
#define SCPSID_LOGIN_STANDARD (33)  // 33 - Web logins - Standard
#define SCPSID_FILE_SYSTEM  (35)	// 35 - File System Version Info
}CC_STRSRQ;



typedef struct						// (C_1856) Read Cert Info
{
	IntVB nScpID;					// the SCP's ID
	IntVB certType;				// Type of cert to read
}CC_CERT_INFO;

typedef struct						// (C_416) Elevator Floor Status
{
	IntVB nScpID;					// the SCP's ID
	IntVB first;					// First ACR to Read
	IntVB count;						// Last ACR to read
}CC_ELEV_RELAY_INFO;

// Web Services commands
typedef struct			// (C_2250) Web Services Login
{
	IntVB	nScpId;			// SCP number
	IntVB	nLoginNumber;	// Login number (0 through 63)
	IntVB	nLoginType;		// 0 = Standard Login, 4 = SNMP, .... -1 Inactive
	union
	{
		struct
		{
			char   cName[10+1];			// Login Name, null terminated
			char   cPassword[64 + 1];		// Password, null terminated.
			IntVB  nAcctType;			// Existing Login Level.
			char   cNotes[32+1];		// Notes, null terminated.
		} sWebConfig;
		struct
		{
			char   cName[16+1];			// Not Used
			char   cPassword[16+1];		// Not Used
		} sPSIA;
        struct
        {
            long   iDevInstance;          // Not Used
            IntVB  wBACnetPort;           // Not Used
            char   cFDBBMDAddress[15+1];  // Not Used
            IntVB  wFDLifetime;           // Not Used
        } sBACnet;
		struct
		{
			char   cName[10+1];           // SNMPv2c: Community Name, SNMPv3: Login Name, null terminated
			char   cAuthKey[16+1];        // SNMPv2c: , SNMPv3: User Password, null terminated. (must be longer than 8 char)
			char   cPrivKey[16+1];        // SNMPv2c: , SNMPv3: Encryption Key, null terminated. (must be longer than 8 char)
			IntVB  wSecurityInfo;         // SNMP security Information (See flags below)
#define SNMP_V2C             (0x0001)     // Enable user for v2c
#define SNMP_V3_USM          (0x0002)     // Enable user for v3 User Security Model
#define SNMPV3_AUTH_ENABLE   (0x0010)     // SNMPv3 Authentication Enable (noAuthNoPriv, authNoPriv and authPriv are valid security level combinations, this encompasses the AUTH_ENABLE and PRIV_ENABLE flags)
#define SNMPV3_PRIV_ENABLE   (0x0020)     // SNMPv3 Encryption Enable 
#define SNMPV3_AUTHTYPE      (0x0040)     // SNMPv3 AuthType (0: MD5, 1:SHA)
#define SNMPV3_PRIVTYPE      (0x0080)     // SNMPv3 PrivType (0: AES, 1:DES)
			char   cContextName[10+1];    // SNMPv3 ContextName, null terminated. (optional)
		} sSnmp;
		struct
		{
			char   cName[16+1];			// Not Used
			char   cPassword[16+1];		// Not Used
			char   cBrokerAddress[15+1];	// Not Used
			IntVB   cPort;				// Not Used
			IntVB   cFlags;				// Not Used
		} sMQTT;
	} uLoginInfo;
} CC_SCP_LOGIN;


typedef struct				// (C_2252)
{	IntVB scp_number;		// SCP number
	IntVB userType;			// user type (0=standard web, 1=psia)
	IntVB userIndex;		// user index to read back
} CC_SCP_LOGIN_USERS;

typedef struct				// (C_2253)
{	IntVB scp_number;		// SCP number
	byte conn_type;
	char broker_connstring[256]; // Connection String for HTS Broker e.g. ssl://brokeraddress:brokerport
	char username[256];     // Broker Username
	char password[256];     // Broker Password
	char pub_topic[256];	// Publish Topic (To Broker)
	char sub_topic[256];	// Subscribe Topic (From Broker)
	char qos[2];            // QoS
} CC_SCP_LOGIN_HTS;

typedef struct				// (C_901)
{	IntVB scp_number;		// SCP number
	char notes[251];		// notes
} CC_WEB_CONFIG_NOTES;

typedef struct				// (C_902)
{	IntVB scp_number;		// SCP number
	IntVB method;
#define WEB_CONFIG_NETWORK_METHOD_DHCP		(1)
#define WEB_CONFIG_NETWORK_METHOD_STATIC	(2)
	long cIpAddr;
	long cSubnetMask;
	long cDfltGateway;
	char cHostName[64];
	IntVB dnsType;
#define WEB_CONFIG_NETWORK_DNS_DYNAMIC		(1)
#define WEB_CONFIG_NETWORK_DNS_STATIC		(2)
	long cDns;
	char cDnsSuffix[256];
	IntVB method2;
	long cIpAddr2;
	long cSubnetMask2;
	long cDfltGateway2;
	long cDns2;
	IntVB TnlEnable;
	long cIpTnl;
	long cPortTnl;
} CC_WEB_CONFIG_NETWORK;

#define HOST_COMM_CONN_NONE			(0)
#define HOST_COMM_CONN_IPSERVER		(1)
#define HOST_COMM_CONN_IPCLIENT		(2)

#define HOST_COMM_DATA_SECURITY_NONE		(0)
#define HOST_COMM_DATA_SECURITY_AES			(2)
#define HOST_COMM_DATA_SECURITY_TLS_REQ		(3)
#define HOST_COMM_DATA_SECURITY_TLS_AVAIL	(4)
#define HOST_COMM_DATA_SECURITY_PEER_CERT	(16)


#define HOST_COMM_IP_CLIENT_RETRY_5		5
#define HOST_COMM_IP_CLIENT_RETRY_10	10
#define HOST_COMM_IP_CLIENT_RETRY_20	20

#define HOST_COMM_IP_CLIENT_MODE_CONT		0
#define HOST_COMM_IP_CLIENT_MODE_ON_DEMAND	1

typedef struct				// (C_903)
{	IntVB scp_number;		// SCP number
	IntVB address;			// Address (0-7)
	IntVB dataSecurity;		// Data Security Options
	IntVB cType;			// Connection Type
	union {
		struct {
			long cAuthIP1;		// Authorized IP address 1
			long cAuthIP2;		// Authorized IP address 2
			IntVB nPort;		// Port number
			IntVB enableAuthIP;	// Enable Authorized IP addresses (0=disable, 1=enable)
			IntVB nNicSel;			// 1 == nic1
		} ipserver;
		struct {
			long cHostIP;			// Host IP address
			IntVB nPort;			// Port number
			IntVB rqIntvl;			// Request interval (5, 10, 20)
			IntVB connMode;			// Connection Mode ( 0=Continuous, 1=OnDemand)
			char cHostName[256];	// Host name
			IntVB nNicSel;			// 1 == nic1
		} ipclient;
	} conn;
} CC_WEB_CONFIG_HOST_COMM_PRIM;


typedef struct				// (C_905)
{	IntVB scp_number;		// SCP number
	IntVB minutes;			// Minutes for the session timer
#define WEB_SESSION_TIMER_MINUTES_5			5
#define WEB_SESSION_TIMER_MINUTES_10		10
#define WEB_SESSION_TIMER_MINUTES_15		15
#define WEB_SESSION_TIMER_MINUTES_20		20
#define WEB_SESSION_TIMER_MINUTES_25		25
#define WEB_SESSION_TIMER_MINUTES_30		30
#define WEB_SESSION_TIMER_MINUTES_35		35
#define WEB_SESSION_TIMER_MINUTES_40		40
#define WEB_SESSION_TIMER_MINUTES_45		45
#define WEB_SESSION_TIMER_MINUTES_50		50
#define WEB_SESSION_TIMER_MINUTES_55		55
#define WEB_SESSION_TIMER_MINUTES_60		60
	IntVB pswd_strength;	// Password Strength
#define WEB_PASSWORD_STRENGTH_LOW		1
#define WEB_PASSWORD_STRENGTH_MEDIUM	2
#define WEB_PASSWORD_STRENGTH_HIGH		3
} CC_WEB_CONFIG_SESSION_TMR;

typedef struct					// (C_906)
{	IntVB scp_number;			// SCP number
	IntVB web_server;			// Web Server (0=disabled, 1=enabled)
	IntVB zeroconf;				// Zeroconf (0=disabled, 1=enabled)
	IntVB dfo_filter;			// DFO Filter (0=disabled, 1=enabled)
	IntVB diag_log;				// Diagnostic Log (0=disabled, 1=enabled)
	IntVB snmp;					// SNMP (0=disabled, 1=v2c enabled, 2=v3 enabled, 3=v2c/v3 enabled)
	IntVB disable_default_user; // Disable Default User (0=enabled, 1=disabled)
	IntVB disable_sd_int;		// Disable SD Card Interface (0=enabled, 1=disabled)
	IntVB disable_usb_int;		// Disable USB Port Interface (0=enabled, 1=disabled)
	IntVB enGratArp;			// Enable Gratuitous ARP (0=disabled, 1=enabled)
	IntVB disable_legacy;		// Disable legacy mode
} CC_WEB_CONFIG_WEB_CONN;

typedef struct				// (C_907)
{	IntVB scp_number;		// SCP number
	IntVB auto_save;		// Auto save enabled (0=disabled, 1=enabled)
	IntVB seconds;			// Number of seconds for auto save timer
#define AUTO_SAVE_SECONDS_30	30
#define AUTO_SAVE_SECONDS_60	60
#define AUTO_SAVE_SECONDS_90	90
#define AUTO_SAVE_SECONDS_120	120
#define AUTO_SAVE_SECONDS_300	300
#define AUTO_SAVE_SECONDS_600	600
#define AUTO_SAVE_SECONDS_1200	1200
#define AUTO_SAVE_SECONDS_1800	1800
	IntVB restore;			// Restore Type (0=Clear All, 1=Restore from last known)
#define AUTO_SAVE_RESTORE_CLEAR_ALL		0
#define AUTO_SAVE_RESTORE_RESTORE		1
} CC_WEB_CONFIG_AUTO_SAVE;

typedef struct				// (C_908)
{	IntVB scp_number;		// SCP number
	IntVB flag;				// flag
} CC_WEB_CONFIG_NETWORK_DIAG;

typedef struct				// (C_909)
{	IntVB scp_number;		// SCP number
	IntVB ts_disabled;		// Time Server Disabled (1=disabled, 0=enabled)
	IntVB index;			// Index of Time Server to Use
#define TIME_SERVER_USER_SPECIFIED_NAME		0
#define TIME_SERVER_USER_SPECIFIED_IP		1
#define TIME_SERVER_POOL_NTP_ORG			2
#define TIME_SERVER_TIME_NIST_GOV			3
	char cTmServ[65];		// User Specified Time Server
	IntVB port;
	IntVB interval;
} CC_WEB_CONFIG_TIME_SERVER;


typedef struct				// (C_912)
{	IntVB scp_number;			// SCP number
	IntVB dump_files_enabled;	// Dump Files Enabled (0=disabled, 1=enabled)
	IntVB num_dump_files;		// Number of Dump Files (read only)
} CC_WEB_CONFIG_DIAGNOSTICS;

typedef struct				// (C_913)
{	IntVB scp_number;		// SCP number
} CC_WEB_CONFIG_APPLY_REBOOT;

typedef struct				// (C_900)
{	IntVB scp_number;		// SCP number
	IntVB read_type;		// Read Type
#define WEB_CONFIG_READ_NOTES				1
#define WEB_CONFIG_READ_NETWORK				2
#define WEB_CONFIG_READ_HOST_COMM_PRIM		3
#define WEB_CONFIG_READ_SESSION_TMR			5
#define WEB_CONFIG_READ_WEB_CONN			6
#define WEB_CONFIG_READ_AUTO_SAVE			7
#define WEB_CONFIG_READ_NET_DIAG			8
#define WEB_CONFIG_READ_TIME_SERVER			9
#define WEB_CONFIG_READ_DIAGNOSTICS			12
} CC_WEB_CONFIG_READ;


typedef union tagSCP_CMND
{	
	CC_BATCH batch;				// batch command (1)
	CC_SYS	system;				// system configuration (11)
	CC_CHANNEL channel;			// create a new channel (12,14)
	CC_NEWSCP    newscp;		// create a new SCP (13,15)
	CC_NEWSCP_LN newscp_ln;		// create a new SCP (1013)
	CC_PEERCERT peer_cert;		// Peer Certificate (17)
	CC_IP_CLIENT_DEFAULTS ip_client_defaults;		// IP Client Defaults (18)
	CC_IP_CLIENT_ID_ASSIGN ip_client_assign;		// IP Client ID Assignment (19)
	CC_ADBC_I64DTIC32 adbcI64dtic32;			// card database record (add/modify) (5304)
	// the SCP specific version of the config commands
	CC_SCP_SCP	scp_scp;		// SCP configuration (1107)
	CC_SCP_ICVT	scp_icvt;		// input conversion table (1101)
	CC_SCP_CFMT	scp_cfmt;		// card format (1102)
	CC_SCP_TZ	scp_timezone;	// timezone (1103)
	CC_SCP_TZEX_ACT	scp_timezone_ex_act_time;	// extended timezone (3103)
	CC_SCP_HOL	scp_holiday;	// holiday (1104)
	CC_SCP_ADBS	scp_adbs;		// access database spec (1105)
	CC_SCP_DAYLIGHT scp_dst;	// set daylight savings table (1116)
	CC_MSP1 msp1;				// MSP1 driver configuration (108)
	CC_SIO	sio;				// sio configuration (109)
	CC_IP	ip;					// input point (110)
	CC_OP	op;					// output point (111)
	CC_RDR	rdr;				// reader (112)
	CC_MP	mp;					// monitor point (113)
	CC_CP	cp;					// control point (114)
	CC_ACR	acr;				// access control reader (115)
	CC_ALVL	alvl;				// access level (116)
	CC_ALVL_EX alvl_ex;			// access level extended (2116)
	CC_ALVL_SPC alvl_spc;		// access level spec (124)
	CC_TRGR	trgr;				// trigger (117)	
	CC_TRGR_128	trgr128;		// trigger (1117)	
	CC_ACTN	actn;				// action (118)
	CC_ACTNREM actn_rem;		// remove action (119)
	CC_MPG mpg;					// Configure a Monitor Point Group (120)
	CC_SCP_LOGIN			scp_login;				// configure a web services login (2250)
    CC_SCP_LOGIN_USERS		scp_login_users;		// read back login user information
	CC_AREA_SPC area_spc;		// configure an access area - "special" (1121)
	CC_ACCEXCEPTION acc_excpt;		// Access Exception List (1220)
	CC_RLEDSPC rled_spc;		// configure the reader led specs (122)
	CC_RTXTSPC rtxt_spc;		// configure the reader text specs (123)
	CC_SIOAESCTL sio_aes_ctl;	// Aes Encryption control (127)
	CC_ACRLOGDENY acr_logdeny;	// configure logging keypad-based deny (151)
	CC_UCMND ucmnd;						// configure user commands (1141)
	CC_UCMND_BKGD  ucmnd_bkgd;			// configure the background text operation (1144)
	CC_SCP_DOWN  down;					// MSP2 driver: shut down (203)
	CC_APP  app;				        // SCP app download (220)	
	CC_FIRMWARE  firmware;				// SCP firmware download (206)
	CC_ATTACHSCP attach;				// attach channel to scp (207,208)
	CC_CONFIGSAVE configsave;			// save the systems configuration to a file (209)
	CC_CONFIGDELTA configdelta;			// set the configuration "delta" file's name (210)
	CC_DUALPORTCONTROL dual_prt;		// send Dual Port control command (211)
	CC_AESCTL aes_ctl;					// Aes Encryption control (212)
	CC_POLLMODE poll_mode;				// Set Poll Mode (214)
	CC_FILETRANSFER file_download;		// Generic file download (215)
	CC_HEX_OUT_INTERNAL hex_out_internal;		// Hex Out Internal (217)
	CC_RESET reset;								// SCP reset (301)
	CC_TIME  time;								// send the systemtime (302)
	CC_TRANINDEX tran_idx;						// set the transaction log reporting index (303)
	CC_CARDDELETEI64 crd_deli64;				// delete (card number is "Int64")  (3305)
	CC_CARDCLEANUP crd_clean;                   // cleanup deactivated cards (343)
	CC_MPMASK mp_mask;							// Monitor Point mask control (306)
	CC_CPCTL cp_ctl;							// Control Point command (307)
	CC_ACRMODE acr_mode;						// ACR mode control (308)
	CC_FOMASK fo_mask;							// forced open mask control (309)
	CC_HOMASK ho_mask;							// held open mask control (310)
	CC_UNLOCK unlock;							// temoprary unlock command (311)
	CC_PROCEDURE proc_ctl;						// Procedure control command (312)
	CC_TVCOMMAND tv_ctl;						// Trigger Variable control command (313)
	CC_TZCOMMAND tz_ctl;						// Timezone control command (314)
	CC_ACRLEDMODE led_mode;						// Reader LED mode set command (315)
	CC_OEMCODE   oem_code;						// SCP OEM code set (upper 12 bits of the s/n) (316)
	CC_PASSWORD  pswd;							// SCP's comm password (317)
	CC_SCPID	 scp_id;						// sets SCP's reported ID (318)0
	CC_APBFREEPASSI64 apb_freei64;				// Free pass to all/individual (3319)
	CC_MPGSET mpg_set;							// Arm/Disarm a monitor point group (321)
	CC_AREASET area_set;						// access area control command (322)
	CC_USELIMITI64 use_limiti64;				// Set use limit to all/individual (3323)
	CC_HEXOUT  hexout;							// Raw hex character output (320)
	CC_OFFLINETIME scp_oltm;					// set the SCP/host off-line time (324)
	CC_RLED_TMP rled_tmp;						// temporary reader LED control (325)
	CC_LCDTEXT lcd_text;						// LCD text output (326)
	CC_SIODC sio_dc;							// SIO Direct Command (327)
	CC_SIOHEX sio_hex;							// SIO HEX Load Command (328)
	CC_HOSTRESPONSE host_response;				// Access grant/deny from HOST (329)
	CC_NV_ARG_SET nv_arg_set;					// Nv Argument Set (330)
	CC_CARD_SIM card_sim;						// card simulator (331)
	CC_DIAG diag;								// Diagnostic commands (333)
	CC_TEMP_ACR_MODE temp_acr_mode;				// Temporary ACR mode control (334)
	CC_OPER_MODE oper_mode;						// Operating Mode (335)
	CC_ACR_OSDP_PASSTHROUGH acr_osdp_pt;		// OSDP Passthrough (337)
	CC_KEYS_SIM key_sim;						// key simulator (339)
	CC_CTL_REBOOT ctl_reboot;					// Control Reboot (341)
	CC_CARD_SIM_RAW card_sim_raw;				// card simulator raw (342)
	CC_IDREQUEST idrq;			// SCP ID report request 401)
	CC_TRANSRQ tran_srq;		// transaction log status request (402)
	CC_MSP1SRQ msp1_srq;		// SIO comm driver status request (403)
	CC_SIOSRQ sio_srq;			// SIO status request (404)
	CC_MPSRQ mp_srq;			// MP status request (405)
	CC_CPSRQ cp_srq;			// CP status request (406)
	CC_ACRSRQ acr_srq;			// ACR status request (407)
	CC_TZSRQ tz_srq;			// Timezone status request (408)
	CC_TVSRQ tv_srq;			// Trigger Variable status request (409)
	CC_MPGSRQ mpg_srq;			// Monitor Point Group status request (411)
	CC_AREASRQ area_srq;		// access area status request (412)
	CC_SIO_RLYCT_SRQ sio_rlyct;	// SIO relay count status request (415)
	CC_SIO_HID_MFG sio_hid_mfg; // SIO HID Serial No and UUID
	CC_ELEV_RELAY_INFO elev_relay_info; //(416)
	CC_ELALVLSPC elalvl_spc;	// Configure the Elevator Access specs (501)
	CC_ELALVL elalvl;			// Configure one elevator access levels (502)
	CC_READ   cfg_read;			// configuration read commands CONFIG_READ
	CC_STRSRQ sts_read;			// scp structure status read
	CC_CERT_INFO cert_info;		// cert info
	CC_BATCH_TRANS batch_trans; // Batch Transactions
	CC_WEB_CONFIG_NOTES web_notes;	// Web config - notes
	CC_WEB_CONFIG_NETWORK web_network;
	CC_WEB_CONFIG_HOST_COMM_PRIM web_host_comm_prim;
	CC_WEB_CONFIG_WEB_CONN web_conn;
	CC_WEB_CONFIG_SESSION_TMR web_session_tmr;
	CC_WEB_CONFIG_AUTO_SAVE web_auto_save;
	CC_WEB_CONFIG_NETWORK_DIAG web_net_diag;
	CC_WEB_CONFIG_TIME_SERVER web_time_server;
	CC_WEB_CONFIG_DIAGNOSTICS web_diagnostics;
	CC_WEB_CONFIG_APPLY_REBOOT web_apply_reboot;
	CC_WEB_CONFIG_READ web_read;
	CC_RMS    rms;				// remote storage (1852)
	CC_ADBC_RECORD  adbc;		// card database record (add/modify) (304)
	CC_SIO_NETWORK sio_network;	// SIO Network configuration (128)

#ifdef SIO_COM_H
// direct SIO support
	CC_SIOCOMMAND  sio_cmnd;	// direct SIO command (601)
#endif

	CC_ADBC_I64DTIC32A255FF adbcI64dtic32a255ff;// card database record (add/modify) (8304) 
	CC_SCP_LOGIN_HTS		scp_login_hts;          // Configure a MQTT HTS Connnection

	CC_DELETE_FILE delete_file;			// Delete file (218)
	CC_FS_FILE_INFO fs_file_info;		// Get file names of particular type (219)
	CC_OSDP_TRANSFER osdp_transfer;				// OSDP Reader File Transfer (340)

	CC_ACR_OAL acr_oal;							// Offline Access List (338)

// Sanbx
	CC_SANBX_LIST_APPS sanbx_list_apps;	// Get the list of apps in sanbx
	CC_SANBX_APP_CMD sanbx_apps_cmd;	// Send a command to sanbx

}SCP_CMND;

typedef struct tagCFG_CMND
{  //enum enCfgCmnd command;
	IntVB command;				// command code: VB compatible
	union tagSCP_CMND cu;
} CFG_CMND;


#pragma pack()

// procedure command codes
enum prc_cmnd { prcNOP, prcCANCEL, prcSTART, prcRESUME };

// control point command codes
enum cp_cmnd { cpcNOP, cpcOFF, cpcON, cpcPULSE, cpcREPEAT };

// command post function's error codes (updated after "3xx and 4xx" commands)
enum enCmndPost
{	enCpNoError = 0,		// no error: the last post was OK
	enCpScpNum,				// invalid SCP number
	enCpOffLine,			// the SCP is off-line
	enCpBusy,				// busy: no buffers available for posting
	enCpMissingPrereq,		// Prerequisite command has not been sent
	enCpInvalidData			// Invalid data was detected in command
};


#endif
