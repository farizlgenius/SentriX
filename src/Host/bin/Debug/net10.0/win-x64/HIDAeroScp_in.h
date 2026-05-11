/*************************************************************************************
* Copyright © 2019 HID Global Corporation.  All rights reserved.
* This software is protected by copyright law and international treaties, as well as any nondisclosure agreements that you or the organization you represent have signed.  
* Any unauthorized reproduction, distribution or use of the software is prohibited.
***********************************************************************************/

#ifndef SCP_IN_H
#define SCP_IN_H


// make this VB compatible...
#pragma pack (4)
#ifndef IntVB
#define IntVB short
#endif


// definitions for the reply message structures
enum enSCPComm	{ enSCPCommUnknown, enSCPCommFailed, enSCPCommOk };
enum enSCPCommErr {
	enSCPCommNoError,				//  0 - no error condition
	// the following errors are cause for terminating comm (after retries)
	enSCPCommTimeout,				//  1 - timeout 
	enSCPCommReplyTooLong,			//  2 - invalid reply packet - too long
	enSCPCommCommandTooLong,		//  3 - invalid command packet - too long (rejected by the SCP)
	enSCPCommCRCError,				//  4 - invalid checksum - either SCP rejected the command or bad reply
	enSCPCommSequenceNumber,		//  5 - the reply did not match the transmitted sequence number - out of sync
	enSCPCommDetached,				//  6 - the SCP was detached
	enSCPCommDeleted,				//  7 - the SCP was deleted
	enSCPCommCipher,				//  8 - decryption error of the reply packet 

	// connection progress status - technically not errors,
	enSCPCommLogOn=20,				// 20 - Connection sequence in progress, started log-on (and Aes sync.)
	enSCPCommNoErrAes,				// 21 - Connection OK, AES encryption enabled (same as enSCPCommNoError, with AES)
	enSCPCommTlsHandshake,			// 22 - TLS handshake in-progress. (Might not see this, except on channels being "promoted" having started as cleartext.)
	enSCPCommNoErrTls,				// 23 - Connection OK, TLS encryption enabled.

	// connecton related errors
	enSCPCommScpType=30,			// 30 - device mismatch - invalid SCP type
	enSCPCommNoPassword,			// 31 - SCP requires password, none was set
	enSCPCommNoAesSet,				// 32 - SCP requires encrypted comm, the dll is not configured for it
	enSCPCommNoAesScp,				// 33 - dll is required to run encrypted comm, the SCP is not configured for it
	enSCPCommAesKey					// 34 - tried encrypted comm, failed key

};


// the SCP reply message structures that form the union tagSCPReply
typedef struct tagSCPReplyCommStatus
{	enum enSCPComm status;
	enum enSCPCommErr error_code;
	IntVB  nChannelId;			// channel number - valid if status==enSCPComNoError
// extended reporting for DualPort applications
	IntVB	current_primary_comm;		// 0 == detached, 1 == not tried, 2 == off-line, 3 == on-line
	IntVB	previous_primary_comm;		// 0 == detached, 1 == not tried, 2 == off-line, 3 == on-line
	IntVB	current_alternate_comm;		// Not used
	IntVB	previous_alternate_comm;	// Not used
}SCPReplyCommStatus;

typedef struct tagSCPReplyNAK
{	IntVB  reason;				// the reason the SCP rejected the command
//									- 0 == invalid packet header,
//									- 1,2,3 == invalid command type (firmware rev mismatch)
//									- 4 == command content error << app may be at fault
//									- 5 == cannot execute: requires password logon
//									- 6 == this port is in standby mode and cannot execute this command
//									- 7 == failed logon: password and/or encryption key
//									- 8 == ICD is Running in Degraded Mode
	long   data;				// argument - depends on 'reason'
//									- if reason==4, then data==index of rejected record
	char   command[20];			// the first 20 bytes of the command
	long   description_code;	// Unique error code for reason description
}SCPReplyNAK;

typedef struct tagSCPReplyIDReport
{	IntVB  device_id;			// identification of the replying device (3=HID)
	IntVB  device_ver;			// hardware version: 27==X1100
	IntVB  sft_rev_major;		// software revision, major
	IntVB  sft_rev_minor;		// software revision, (minor * 10 + build)
	long   serial_number;		// serial number
	long   ram_size;			// amount of ram installed
	long   ram_free;			// amount of ram available
	long   e_sec;				// current clock
	long   db_max;				// access database size
	long   db_active;			// number of active records
	BYTE   dip_switch_pwrup;	// DIP switch at power-up: diagnostic
	BYTE   dip_switch_current;	// DIP switch current value: diagnostic
	IntVB  scp_id;				// the SCP's ID, as set by the host << new 02/03/97 <<
	IntVB  firmware_advisory;	// 0==no firmware action, 1==must reset first, 2==starting load
	IntVB  scp_in_1;			// Scp local monitor "IN 1" state
	IntVB  scp_in_2;			// Scp local monitor "IN 2" state
	long   adb_max;				// Not used
	long   adb_active;			// Not used
	long   bio1_max;			// Not used
	long   bio1_active;			// Not used
	long   bio2_max;			// Not used
	long   bio2_active;			// Not used
	IntVB  nOemCode;			// Not used
    BYTE   config_flags;        // Configuration flags.  (Bit-0 = Needs CC_SCP_SCP configuration, SCP not yet known to driver)
	BYTE   mac_addr[6];			// MAC Address, if applicable, LSB first.
	BYTE   tls_status;			// TLS status
	BYTE   oper_mode;			// Current Operating Mode
	IntVB  scp_in_3;			// Scp local monitor "IN 3" state
	long   cumulative_bld_cnt;	// Cumulative build count
	char hardware_id; 			// Hardware ID
	char hardware_rev;			// Hardware Rev
	long hardware_component_id;			// Hardware Components
}SCPReplyIDReport;

typedef struct tagSCPReplyUTAGReport
{	IntVB	nCount;				// number UTAGs in the UTAG array
	IntVB	nFirst;				// index of first UTAG in the UTAG array
	long	list[64];			// UTAG array - size defined by nCount
}SCPReplyUTAGReport;

#if 0  // this is only a "comment block" - for informational use only
enum enScpUtag		// index, corresponding command
{	utgNULL,		//  0 - indicates an invalid/null resource
	utgScp,			//  1 - 107 SCP control structure
	utgTimezone,	//  2 - 103 time: time zone
	utgHoliday,		//  3 - 104 time: holiday
	utgIcvt,		//  4 - 101 input conversion table
	utgCfmt,		//  5 - 102 card format tables
	utgAdbs,		//  6 - 105 card database - config.
	utgAdbc,		//  7 - 106/304 card database - download
	utgMsp1,		//  8 - 108 SIO comm driver
	utgSio,			//  9 - 109 SIO panel
	utgIp,			// 10 - 110 input point
	utgOp,			// 11 - 111 output point
	utgRdr,			// 12 - 112 reader
	utgMp,			// 13 - 113 EP, monitor point
	utgCp,			// 14 - 114 EP, control output point
	utgAcr,			// 15 - 115 EP, access control reader
	utgAlvl,		// 16 - 116 access levels
	utgTrgr,		// 17 - 117 triggers
	utgProc,		// 18 - 118 procedures
	utgMpg,			// 19 - 120 Monitor Point Groups
	utgArea			// 20 - 121 Access Control Areas
}
#endif

typedef struct tagSCPReplyTranStatus
{	long	capacity;		// the transaction buffer size (allocated)
	long	oldest;			// serial number of the oldest TR in the file
	long	last_rprtd;		// serial number of the last reported TR
	long	last_loggd;		// serial number of the last logged TR
	IntVB	disabled;		// non-zero if disabled with (Command_303)
}SCPReplyTranStatus;

// transaction related definitions
// - transaction source type definitions
#define tranSrcScpDiag		0x00	// SCP diagnostics
#define tranSrcScpCom		0x01	// SCP to HOST comm driver
#define tranSrcScpLcl		0x02	// SCP local monitor points
#define tranSrcSioDiag		0x03	// SIO diagnostics
#define tranSrcSioCom		0x04	// SIO comm driver
#define tranSrcSioTmpr		0x05	// SIO cabinet tamper
#define tranSrcSioPwr		0x06	// SIO power monitor
#define tranSrcMP			0x07	// alarm Monitor Point
#define tranSrcCP			0x08	// output Control Point
#define tranSrcACR			0x09	// Access Control Reader (ACR)
#define tranSrcAcrTmpr		0x0A	// ACR: reader tamper monitor
#define tranSrcAcrDoor		0x0B	// ACR: door position sensor
#define tranSrcAcrRex0		0x0D	// ACR: 1'st "Request to exit" input
#define tranSrcAcrRex1		0x0E	// ACR: 2'nd "Request to exit" input
#define tranSrcTimeZone		0x0F	// Timezone
#define tranSrcProcedure	0x10	// procedure (action list)
#define tranSrcTrigger		0x11	// trigger
#define tranSrcTrigVar		0x12	// trigger variable
#define tranSrcMPG			0x13	// Monitor Point Group
#define tranSrcArea			0x14	// Access Control Area
#define tranSrcAcrTmprAlt	0x15	// ACR: the alternate reader's tamper monitor
#define tranSrcSioEmg		0x17	// SIO Emergency Switch
#define tranSrcLoginService	0x18	// Login Service

// - transaction type definitions - specifies the transaction argument also
#define tranTypeSys						0x01	// system
#define tranTypeSioComm					0x02	// SIO communication status report
#define tranTypeCardBin					0x03	// binary card data
#define tranTypeCardBcd					0x04	// card data
#define tranTypeCardFull				0x05	// formatted card: f/c, c/n, i/c
#define tranTypeCardID					0x06	// formatted card: card number only
#define tranTypeCoS						0x07	// change-of-state
#define tranTypeREX						0x08	// exit request
#define tranTypeCoSDoor					0x09	// Door Sts Monitor Change-Of-State
#define tranTypeProcedure				0x0A	// procedure (command list) log
#define tranTypeUserCmnd				0x0B	// User Command Request report
#define tranTypeActivate				0x0C	// change of state: tv, tz, trig
#define tranTypeAcr						0x0D	// ACR mode change
#define tranTypeMpg						0x0E	// Monitor Point Group status change
#define tranTypeArea					0x0F	// Access Control Area
#define tranTypeOAL						0x19	// Offline Access List
#define tranTypeUseLimit				0x13	// Use Limit report
#define tranTypeWebActivity				0x14	// Web activity
#define tranTypeDblCardFull				0x15	// formatted card: f/c, c/n-64-bit, i/c
#define tranTypeDblCardID				0x16	// formatted card: 52-bit card number only
#define tranTypeOperatingMode			0x18	// Operating Mode change
#define tranTypeCoSElevator				0x1A	// Elevator Floor Status CoS
#define tranTypeFileDownloadStatus		0x1B	// File Download Status
#define tranTypeBatchReport				0x1C	// Batch Report
#define tranTypeCoSElevatorAccess		0x1D	// Elevator Floor Access Transaction
#define tranTypeI64CardFull				0x25	// formatted card: f/c, c/n-64-bit, i/c
#define tranTypeI64CardID				0x26	// formatted card: 64-bit card number only
#define tranTypeI64CardFullIc32			0x35	// formatted card: f/c, c/n-64-bit, 32-bit i/c
#define	tranTypeAcrExtFeatureStls		0x40	// Extended Lockset Stateless event
#define	tranTypeAcrExtFeatureCoS		0x41	// Extended Locket Statefull transaction
#define tranTypeHostCardFullPin			0x42	// formatted card: f/c, c/n-64-bit, 32-bit i/c, pin
#define tranTypeAsci					0x7E	// ASCII diagnostic message
#define tranTypeSioDiag					0x7F	// SIO comm diagnostics



// - transaction type dependent structures
typedef struct tagTypeSys			// system
{	IntVB	error_code;				// non-zero indicates an error
}TypeSys;

typedef struct tagTypeSysComm		// system - extended comm report
{	IntVB	error_code;					// non-zero indicates an error unless it's tran_code == 1 then it's power up diagnostics
										// Power Up diagnostics are interpreted as follows:
										// bit 0 = Loss of lock
										// bit 1 = Loss of clock
										// bit 2 = External Reset
										// bit 3 = Power on Clock
										// bit 4 = Watchdog Timer
										// bit 5 = Software
										// bit 6 = Low Voltage
										// bit 7 = Fault (Software Fault)
	IntVB	current_primary_comm;		// 0 == off-line, 1 == active, 2 == standby
	IntVB	previous_primary_comm;		// 0 == off-line, 1 == active, 2 == standby
	IntVB	current_alternate_comm;		// Not used
	IntVB	previous_alternate_comm;	// Not used
}TypeSysComm;
// transaction codes for tranTypeSys:
//			1 - SCP power-up diagnostics
//			2 - host comm, off-line --> formats to TypeSytsComm
//			3 - host comm, on-line ---> formats to TypeSytsComm
//			4 - Transaction count exceeds the preset limit
//			5 - Autosave - Configuration save complete
//			6 - Autosave - Database Complete
//			7 - Card Database cleared due to SRAM buffer overflow

typedef struct tagTypeSioComm			// SIO communication status report
{	IntVB	comm_sts;			// comm status
								//  0 - not configured
								//  1 - not tried: active, have not tried to poll it
								//  2 - off-line
								//  3 - on-line
    BYTE model;					// sio model number (VALID ONLY IF "ON-LINE")
    BYTE revision;				// sio firmware revision number (VALID ONLY IF "ON-LINE")
    long ser_num;				// sio serial number (VALID ONLY IF "ON-LINE")
	// extended SIO info: the following block is valid ONLY if nExtendedInfoValid is non-zero:
	IntVB nExtendedInfoValid;	// use the data below only if this field is non-zero
	IntVB nHardwareId;
	IntVB nHardwareRev;
	IntVB nProductId;			// same as model
	IntVB nProductVer;			// -- application specific version of this ProductId
	IntVB nFirmwareBoot;		// BOOT code version info:   (maj(4) << 12)+(min(8) << 4) + (bld(4))
	IntVB nFirmwareLdr;			// Loader code version info: (maj(4) << 12)+(min(8) << 4) + (bld(4))
	IntVB nFirmwareApp;			// App code version info:    (maj(4) << 12)+(min(8) << 4) + (bld(4))
	IntVB nOemCode;				// Not used
	BYTE  nEncConfig;			// Master/Secret key currently in use on this SIO: 0=None, 1=AES Default Key, 2=AES Master/Secret Key, 3= PKI, 6=AES256 session key
	BYTE  nEncKeyStatus;		// Status of Master/Secret Key; 0=Not Loaded, 1=Loaded, unverified, 2=Loaded, conflicts w/SIO, 3=Loaded, Verified, 4=AES256 Verified.
	BYTE  mac_addr[6];			// MAC Address, if applicable, LSB first.
	long nHardwareComponents;
}TypeSioComm;
//  Transaction codes for tranTypeSioComm:
//  	1	- comm disabled (result of host command)
//  	2	- off-line: timeout (no/bad response from unit)
//  	3	- off-line: invalid identification from SIO
//  	4	- off-line: Encryption could not be established
//  	5	- on-line: normal connection
//		6   - hexLoad report: ser_num is address loaded (-1 == last record)

typedef struct tagTypeCardBin	// binary card data
{	IntVB bit_count;			// number of valid data bits
	char	bit_array[100];		// first bit is (0x80 & bit_array[0])
}TypeCardBin;
// transaction codes for tranTypeCardBin:
//		1 - access denied, invalid card format
// 		2 - access granted, invalid card format

typedef struct tagTypeCardBcd	// card data
{	IntVB digit_count;			// number of valid digits (0-9 plus A-F)
	char	bcd_array[100];		// each entry holds a hex digit: 0x0 - 0xF
}TypeCardBcd;
// transaction codes for tranTypeCardBcd:
//		1 - access denied, invalid card format, forward read
//		2 - access denied, invalid card format, reverse read

typedef struct tagTypeCardFull	// formatted card: f/c, c/n, i/c
{	IntVB format_number;		// index to the format table that was used, negative if reverse
	long	facility_code;		// facility code
	long	cardholder_id;		// cardholder ID number
	IntVB	issue_code;			// issue code
	IntVB	floor_number;		// zero if not available (or not supported), else 1==first floor, ...
	char	encoded_card[32];	// Large encoded ID. (up to 32 bytes reported)
}TypeCardFull;
// transaction codes for tranTypeCardFull:
//		 1	- request rejected: access point "locked"
//		 2	- request accepted: access point "unlocked"
//		 3	- request rejected: invalid facility code
//		 4	- request rejected: invalid f/c extension
//		 5	- request rejected: not in card file
//		 6	- request rejected: invalid issue code
//		 7	- request granted: f/c verified, not used
//		 8	- request granted: f/c verified, door used
//		 9	- access denied - asked for host approval, then timed out
//		10	- reporting that this card is "about to get access granted" (expecting C_329 Host Response)
//		11  - access denied count exceeded
//		12	- access denied - asked for host approval, then host denied
//		13  - request rejected: Airlock is Busy
// 		16 - request granted: card granted access from cache, not in card file
// 		17 - request granted: card granted access from cache, invalid facility code
// 		18 - request granted: card granted access from cache, invalid issue code

typedef struct tagTypeDblCardFull	// formatted card: f/c, c/n, i/c
{	IntVB format_number;			// index to the format table that was used, negative if reverse
	long	facility_code;			// facility code
	double	cardholder_id;			// cardholder ID number
	IntVB	issue_code;				// issue code
	IntVB	floor_number;			// zero if not available (or not supported), else 1==first floor, ...
	char	encoded_card[32];		// Large encoded ID. (up to 32 bytes reported)
}TypeDblCardFull;

typedef struct tagTypeI64CardFull	// formatted card: f/c, c/n, i/c
{	IntVB format_number;			// index to the format table that was used, negative if reverse
	long	facility_code;			// facility code
	__int64	cardholder_id;			// cardholder ID number
	IntVB	issue_code;				// issue code
	IntVB	floor_number;			// zero if not available (or not supported), else 1==first floor, ...
	char	encoded_card[32];	// Large encoded ID. (up to 32 bytes reported)
}TypeI64CardFull;

typedef struct tagTypeI64CardFullIc32	// formatted card: f/c, c/n, i/c
{	IntVB format_number;				// index to the format table that was used, negative if reverse
	long	facility_code;				// facility code
	__int64	cardholder_id;				// cardholder ID number
	long	issue_code;					// issue code
	IntVB	floor_number;				// zero if not available (or not supported), else 1==first floor, ...
	char	encoded_card[32];	// Large encoded ID. (up to 32 bytes reported)
}TypeI64CardFullIc32;

typedef struct tagTypeHostCardFullPin	// formatted card: f/c, c/n, i/c, pin
{	IntVB format_number;				// index to the format table that was used, negative if reverse
	long	facility_code;				// facility code
	__int64	cardholder_id;				// cardholder ID number
	long	issue_code;					// issue code
	IntVB	floor_number;				// zero if not available (or not supported), else 1==first floor, ...
	char 	pin[8];		    			// pin entered, BCD, 0xF terminated
	char	encoded_card[32];			// Large encoded ID. (up to 32 bytes reported)
}TypeHostCardFullPin;


typedef struct tagTypeCardID		// formatted card: card number only
{	IntVB	format_number;			// index to the format table that was used, negative if reverse read
	long	cardholder_id;			// cardholder ID number
	IntVB	floor_number;			// zero if not available (or not supported), else 1==first floor, ...
	IntVB	card_type_flags;		// Card type flags (bit-0 = escort, bit-1 = escort required)
	IntVB	elev_cab;				// Elevator cab number
}TypeCardID;
// transaction codes for tranTypeCardID:
//		 1 - request rejected: de-activated card
//		 2 - request rejected: before activation date
//		 3 - request rejected: after expiration date
//		 4 - request rejected: invalid time
//		 5 - request rejected: invalid PIN
//		 6 - request rejected: anti-passback violation
//		 7 - request granted:  apb violation, not used
//		 8 - request granted:  apb violation, used
//		 9 - request rejected: duress code detected
//		10 - request granted:  duress, used
//		11 - request granted:  duress, not used
//		12 - request granted:  full test, not used
//		13 - request granted:  full test, used
//		14 - request denied:   never allowed at this reader (all Tz's == 0)
//		15 - request denied:   no second card presented
//		16 - request denied:   occupancy limit reached
//		17 - request denied:   the area is NOT enabled
//		18 - request denied:   use limit
//		19 - request denied:   unauthorized assets
//		20 - request denied:   biometric verification error
//		21 - granting access:  used/not-used transaction will follow
//      22 - request rejected: failed the bio test: no bio record
//      23 - request rejected: failed the bio test: no bio device
//      24 - request rejected: no escort card presented
//      25 - request rejected: obsolete >> m1m/m2m last special user may not exit
//      26 - request rejected: obsolete >> m1m/m2m area has no special users yet
//      27 - request rejected: obsolete >> m1m/m2m supervisor approval timeout
//      28 - request rejected: not used
//		29 - request rejected: Airlock is Busy
//		30 - request rejected: Incomplete CARD & PIN sequence
//		31 - request granted: Double-card event.
//		32 - request granted: Double-card event while in uncontrolled state (locked/unlocked).
//		33 - request rejected: Elevators - floor not in floors served
//		34 - request rejected: Elevators - floor request not authorized
//		35 - request rejected: Elevators - timeout
//		36 - request rejected: Elevators - unknown error
//		37 - request granted: local verification: access grant, not used
//		38 - request granted: local verification: access grant, used
//		39 - granting access: requires escort, pending escort card
//      40 - request denied: violates minimum occupancy count
//      41 - request denied: card pending at another reader
// 		42 - request granted: known card granted access from cache
// 		43 - request denied: known card denied access from cache


typedef struct tagTypeDblCardID		// formatted card: card number only
{	IntVB	format_number;			// index to the format table that was used, negative if reverse read
	double	cardholder_id;			// cardholder ID number
	IntVB	floor_number;			// zero if not available (or not supported), else 1==first floor, ...
	IntVB	card_type_flags;		// Card type flags (bit-0 = escort, bit-1 = escort required)
	IntVB	elev_cab;				// Elevator cab number
}TypeDblCardID;

typedef struct tagTypeI64CardID		// formatted card: card number only
{	IntVB	format_number;			// index to the format table that was used, negative if reverse read
	__int64	cardholder_id;			// cardholder ID number
	IntVB	floor_number;			// zero if not available (or not supported), else 1==first floor, ...
	IntVB	card_type_flags;		// Card type flags (bit-0 = escort, bit-1 = escort required)
	IntVB	elev_cab;				// Elevator cab number
}TypeI64CardID;


typedef struct tagTypeCoS			// change-of-state
{	BYTE	status;					// new status
// status code byte encoding:
//		0x07 - status mask: 0=inactive, 1=active, 2-7=supervisory fault codes:
//				2==ground, 3==short, 4==open, 5==foreign voltage, 6==non-settling
//		0x08 - off-line: comm to the input point is not valid
//		0x10 - mask flag: set if the Monitor Point is MASK-ed
//		0x20 - local mask flag: entry or exit delay in progress
//		0x40 - entry delay in progress
//		0x80 - not attached (the Monitor Point is not linked to an Input)
	BYTE	old_sts;				// previous status (prior to this CoS report)
}TypeCoS;

typedef struct tagTypeActivate			// change-of-state
{
	__int32	activationCount;
}TypeActivate;
// transaction codes for tranTypeCoS:
//		1 - disconnected (from an input point ID)
//		2 - unknown (off-line): no report from the ID
//		3 - secure (or de-activate relay)
//		4 - alarm (or activated relay: perm or temp)
//		5 - fault
//		6 - exit delay in progress
//		7 - entry delay in progress

typedef struct tagTypeREX			// exit request
{	IntVB	rex_number;				// rex that initiated the request (0 or 1)
}TypeREX;
// transaction codes for tranTypeREX
//		1 - exit cycle: door use not verified
//		2 - exit cycle: door not used
//		3 - exit cycle: door used
//		4 - host initiated request: door use not verified
//		5 - host initiated request: door not used
//		6 - host initiated request: door used
//		7 -	exit request denied: Airlock Busy
//		8 - host request: Cannot complete due to Airlock Busy.
//      9 - exit cycle: started

typedef struct tagTypeCoSDoor	// Door Sts Monitor Change-Of-State
{	BYTE door_status;			// door status map
// door_status byte encoding is the same as tagTypeCoS::status
	BYTE ap_status;				// supplemental access point status
	BYTE ap_prior;				// previous ap status
	BYTE door_prior;			// previous door status map
// ap_status byte encoding
//		0x01 - flag: set if a/p is unlocked
//		0x02 - flag: access (exit) cycle in progress
//		0x04 - flag: forced open status
//		0x08 - flag: forced open mask status
//		0x10 - flag: held open status
//		0x20 - flag: held open mask status
//		0x40 - flag: held open pre-alarm condition
//		0x80 - flag: door is in "extended held open" mode
  }TypeCoSDoor;
// transaction codes for tranTypeCoSDoor:
//		1 - disconnected
//		2 - unknown (_RS bits: last known status)
//		3 - secure
//		4 - alarm (forced, held, or both)
//		5 - fault (fault type is encoded in door_status byte

//struct tagTypeProcedure			// procedure (command list) log
//									// no arguments required
// transaction codes for tranTypeProcedure:
//		1 - cancel (abort delay), see command enCcProcedure (312)
//		2 - execute (start new), see command enCcProcedure (312)
//		3 - resume, if paused, see command enCcProcedure (312)
//		4 = execute (prefix  256 actions), see command enCcProcedure (312)
//		5 = execute (prefix  512 actions), see command enCcProcedure (312)
//		6 = execute (prefix 1024 actions), see command enCcProcedure (312)
//		7 = resume  (prefix  256 actions), see command enCcProcedure (312)
//		8 = resume  (prefix  512 actions), see command enCcProcedure (312) 
//		9 = resume  (prefix 1024 actions), see command enCcProcedure (312)
//	   10 = command was issued to procedure with no actions - nop

typedef struct tagTypeUserCmnd	// User Command Request report
{	IntVB nKeys;				// number of user command digits entered
	char  keys[16];				// null terminated string: '0' through '9'
}TypeUserCmnd;
// transaction codes for tranTypeUserCmnd:
//		1 - command entered by the user...

//struct tagTypeActivate		// change of state: tv, tz, trig
//								// no arguments required
// transaction codes for tranTypeActivate:
//		1 - became inactive
//		2 - became active 

typedef struct tagTypeAcr		// ACR mode change
{	IntVB actl_flags;			// image of CC_ACR::actl_flags
	IntVB prior_flags;			// flags prior to mode set
	IntVB prior_mode;			// mode prior to mode set
	IntVB actl_flags_e;			// image of CC_ACR::spare flags
	IntVB prior_flags_e;		// prior image of CC_ACR::spare flags
	long  auth_mod_flags;		// Not used
	long  prior_auth_mod_flags; // Not used
}TypeAcr;
// transaction codes for tranTypeAcr: (same as ACR mode set command codes)
//		1 - disabled
//		2 - unlocked
//		3 - locked (exit request enabled)
//		4 - facility code only
//		5 - card only
//		6 - PIN only
//		7 - card and PIN
//		8 - PIN or card

typedef struct tagTypeMPG		// Monitor Point Group Change of state
{	IntVB mask_count;			// current mask count of this MPG
	IntVB nActiveMps;			// number of active Monitor Points
	IntVB nMpList[10*2];		// list of the first 10 active Point Pairs: "Type-Number"
}TypeMPG;
// transaction codes for tranTypeMpg
//		1 - first disarm command executed (mask_count was 0, all MPs got masked)
//		2 - subsequent disarm command executed (mask_count incremented, MPs already masked)
//		3 - override command: armed (mask_count cleard, all points unmasked)
//		4 - override command: disarmed (mask_count set, unmasked all points)
//		5 - force arm command, MPG armed, (may have active zones, mask_count is now zero)
//		6 - force arm command, MPG not armed (mask_count decremented)
//		7 - standard arm command, MPG armed (did not have active zones, mask_count is now zero)
//		8 - standard arm command, MPG did not arm, (had active zones, mask_count unchanged)
//		9 - standard arm command, MPG still armed, (mask_count decremented)
//	   10 - override arm command, MPG armed (mask_count is now zero)
//	   11 - override arm command, MPG did not arm, (mask_count decremented)


// The point types used by the PtLink format (nIpsCosSourceType, nIpsCosActionSrcType, etc.)
#define PTLINK_TYPE_MP		(0x1)		// - MP
#define PTLINK_TYPE_FORCED	(0x2)		// - F/O
#define PTLINK_TYPE_HELD	(0x3)		// - H/O
#define PTLINK_TYPE_DOOR	(0x4)		// - AcrDoor
#define PTLINK_TYPE_ACR		(0x5)		// - Acr
#define PTLINK_TYPE_PROC	(0x6)		// - Procedure
#define PTLINK_TYPE_HOST	(0xF)		// - Host


typedef struct tagTypeArea		// Access Control Area Change of state
{	IntVB status;				// area status mask flags
//		1 - set if area is enabled (open)
//     -- - the multi-occupancy mode is coded using bit-1 and bit 2
//      0 - (both bit-1 and bit-2 are zero) multi-occupancy mode not enabled
//      2 - (bit-1 is set, bit-2 is zero)  "standard" multiple occupancy rules
//      4 - (bit-2 is set, bit-1 is zero)  "modified-1-man" multiple occupancy rules
//      6 - (both bit-1 and bit 2 are set) "modified-2-man" multiple occupancy rules
//	  128 - set if this area has NOT been configured (no area checks are made!!!)
	long  occupancy;			// occupancy count - standard users
	long  occ_spc;				// occupancy count - special users
	IntVB prior_status;			// flags before change
}TypeArea;
// transaction codes for tranTypeArea
//		1 - area disabled
//		2 - area enabled
//		3 - occupancy count reached zero
//		4 - occupancy count reached the "downward-limit"
//		5 - occupancy count reached the "upward-limit"
//		6 - occupancy count reached the "max-occupancy-limit"
//      7 - multi-occupancy mode change
//		8 - multi-occupancy mode change could not be made - the area is not empty


typedef struct tagTypeUseLimit
{	IntVB	use_count;				// the updated use count as a result of this access
	__int64	cardholder_id;			// cardholder ID number
}TypeUseLimit;
// transaction codes for tranUseLimit:
//		 1 - use limit changed, reporting new limit

typedef struct tagTypeAsci
{	char bfr[100];				// null terminated string
}TypeAsci;

typedef struct tagTypeSioDiag
{	IntVB length;
	char bfr[100];			// raw hex dump - for SCP factory test support
}TypeSioDiag;

typedef struct tagTypeAcrExtFeatureStls	// Ext. Feature Stateless
{	
	IntVB	nExtFeatureType;		// Ext. Feature Type (0=None, ...)
	IntVB	nHardwareType;			// Hardware Type in Use.
	BYTE	nExtFeatureData[16];	// Associated Data (by feature type)
    BYTE    nExtFeatureStatus[32];	// Extended Feature Status
} TypeAcrExtFeatureStls;
// transaction codes for TypeAcrExtFeatureStls
//    1 - Extended Status Updated

typedef struct tagTypeAcrExtFeatureCoS	// Ext. Feature C-o-S
{	
	IntVB	nExtFeatureType;		// Ext. Feature Type (0=None, ...)
	IntVB	nHardwareType;			// Hardware Type in Use.
	IntVB	nExtFeaturePoint;		// Point (0=Deadbolt, etc.)
	BYTE	nStatus;				// Current Status (trl07 encoded)
	BYTE	nStatusPrior;			// Prior Status (trl07 encoded)
	BYTE	nExtFeatureData[16];	// Associated Data (by feature type)
    BYTE    nExtFeatureStatus[32];	// Extended Feature Status
} TypeAcrExtFeatureCoS;
// transaction codes for TypeAcrExtFeatureCoS:
//		1 - disconnected
//		2 - unknown (nStatus bits: last known status)
//		3 - secure
//		4 - alarm 
//		5 - fault (fault type is encoded in nStatus byte)

typedef struct tagTypeWebActivity  // Web Activity
{
#define WEB_ACTIVITY_TYPE_WEBPAGE     0
#define WEB_ACTIVITY_TYPE_PSIA        1
	BYTE	iType;		// 0 = Web pages 1 = PSIA
	BYTE	iCurUserId;                               //Current Logged in User, -1 = Invalid
    BYTE	iObjectUserId;  //Modified or Action User, -1 = Invalid
	char	szObjectUser[16];  //Modified or Action User Name
	long    ipAddress;    // IP Address that requested Action
} TypeWebActivity;
// transaction codes for TypeWebActivity:
//		1 - Save Home Notes 
//		2 - Save Network Settings 
//		3 - Save Host Comm 
//		4 - Add User
//		5 - Delete User
//		6 - Modify User
//		7 - Save Password Strength and Session Timer
//		8 - Save Web Server Options
//		9 - Save Time Server Settings
//		10 - Auto Save Timer Settings
//		11 - Load Certificate
//		12 - Logged Out By Link
//		13 - Logged Out By Timeout
//		14 - Logged Out By User
//		15 - Logged Out By Apply
//		16 - Invalid Login
//		17 - Successful Login
//		18 - Network Diagnostic Saved
//		19 - Card DB Size Saved
//		20 - Central Station Settings Saved
//		21 - Diagnostic Page Saved
//		22 - Security Options Saved
//		23 - Add-On Package Saved
//		24 - Not used
//		25 - Not used
//		26 - Not used
//		27 - Invalid login limit reached
//		28 - Firmware download initiated
//		29 - Advanced Networking Routes Saved
//		30 - Advanced Networking Reversion Timer Started
//		31 - Advanced Networking Reversion Timer Elapsed
//		32 - Advanced Networking Route Changes Reverted
//		33 - Advanced Networking Route Changes Cleared
//		34 - Certificate Generation Started

typedef struct tagTypeOperatingMode  // Operating Mode
{
	BYTE	prev_oper;      // Previous operating mode
} TypeOperatingMode;
// transaction codes for TypeOperatingMode:
//		1 - Operating Mode 0
//		2 - Operating Mode 1
//		3 - Operating Mode 2
//		4 - Operating Mode 3
//		5 - Operating Mode 4
//		6 - Operating Mode 5
//		7 - Operating Mode 6
//		8 - Operating Mode 7


typedef struct tagTypeOAL	// Offline Access List
{
	BYTE	nReasonCode;      // Reason Code
	BYTE	nData[4];		  // OAL Data
} TypeOAL;
// transaction codes for tagTypeOAL:
//		1 - OAL Enabled
//		2 - OAL Disabled
//		3 - OAL Credential Added
//		4 - OAL Credential Deleted
//		5 - OAL List Clear
//		6 - OAL Count for Local Verification accesses while the lock was offline
//		7 - OAL Count for Offline Access List(whitelist)
//		8 - OAL Dynamic List Cleared
//		9 - OAL Static List Cleared
//		10 - OAL Credential Add Failure
//		11 - OAL Credential Delete Failure


typedef struct tagTypeCoSElevator {
	BYTE prevFloorStatus;
	BYTE floorNumber;
}TypeCoSElevator;
// transaction codes for tagTypeCoSElevator:
//		1 - Floor Status is Secure
//		2 - Floor Status is Public
//		3 - Floor Status is Disabled

typedef struct tagTypeFileDownloadStatus {
	BYTE fileType;
	char fileName[100+1];
}TypeFileDownloadStatus;
// transaction codes for TypeFileTransfer:
//		1 - File transfer complete 
//		2 - File transfer aborted/error
//		3 - File successfully deleted 
//		4 - File Delete Error

typedef struct tagTypeBatchReport {
	IntVB trigNum;
	__int32 activationCount;
	BYTE sourceType;
	IntVB sourceNumber;
	BYTE tranType;
	BYTE tranCodeMap[16];
}TypeBatchReport;
// transaction codes for TypeFileTransfer:
//		1 - Batch Status Read
//      2 - Batch Status Read (Clear Activations)
//		3 - Batch Cleared


typedef struct tagTypeCoSElevatorAccess {
	__int64	cardholder_id;			// cardholder ID number
	BYTE floors[(MAX_FLOORS_PER_ACR + 7) / 8];
	BYTE nCardFormat;
}TypeCoSElevatorAccess;
// transaction codes for tagTypeCoSElevatorAccess:
//		1 - Elevator Access


typedef union _SCP_TRANS_TYPE						// content defined by "tran_type"
{	
	TypeOAL				oal;			// Offline Access List

	TypeSys				sys;			// system
	TypeSysComm			sys_comm;		// system with extended comm status fields
	TypeSioComm			s_comm;			// SIO communication status report
	TypeCardBin			c_bin; 			// binary card data
	TypeCardBcd			c_bcd;			// card data
	TypeCardFull		c_full;			// formatted card: f/c, c/n, i/c
	TypeCardID			c_id;			// formatted card: card number only
	TypeDblCardFull		c_fulldbl;		// formatted card: f/c, c/n (double), i/c
	TypeDblCardID		c_iddbl;		// formatted card: card number (double) only
	TypeI64CardFull		c_fulli64;		// formatted card: f/c, c/n (Int64), i/c
	TypeI64CardFullIc32	c_fulli64i32;	// formatted card: f/c, c/n (Int64), i/c
	TypeHostCardFullPin	c_fullHostPin;	// formatted card: f/c, c/n (Int64), i/c, pin
	TypeI64CardID		c_idi64;		// formatted card: card number (Int64) only
	TypeCoS				cos;			// change-of-state
	TypeREX				rex;			// exit request
	TypeCoSDoor			door;			// Door Sts Monitor Change-Of-State
//	TypeProcedure		proc;			// procedure (command list) log
	TypeUserCmnd		usrcmd;			// User Command Request report
	TypeActivate		act;			// change of state: tv, tz, trig
	TypeAcr				acr;			// ACR mode
	TypeMPG				mpg;			// Monitor Point Group
	TypeArea			area;			// Access Area
	TypeUseLimit		c_uselimit;		// Use Limit update
	TypeAsci			t_diag;			// ASCII diagnostic message
	TypeSioDiag			s_diag;			// SIO comm diagnostics
	TypeAcrExtFeatureStls	extfeat_stls;	// Extended Feature Stateless Transaction
	TypeAcrExtFeatureCoS	extfeat_cos;	// Extended Feature Change-of-state
	TypeWebActivity		web_activity;	// Web Activity
	TypeOperatingMode	oper_mode;		// Operating mode
	TypeCoSElevator		floor;          // Elevator Floor Status
	TypeFileDownloadStatus file_download; // File Download Status
	TypeCoSElevatorAccess elev_access;
	TypeBatchReport       batch_report;
}SCP_TRANS_TYPE;

typedef struct tagSCPReplyTransaction
{	long	ser_num;			// serial number of this transaction
	long	time;				// time of the transaction, seconds, 1970-based
	IntVB	source_type;		// see the "tranSrc..." definitions
	IntVB	source_number;		// ...defines the element of tranSrc...
	IntVB	tran_type;			// see the "tranType..." definitions
	IntVB	tran_code;			// ...defines the reason
	SCP_TRANS_TYPE arg;
}SCPReplyTransaction;

typedef struct tagSCPReplyDualPort
{	IntVB	number;				// Reply Source - Scp Port ID: 0==primary, 1==alternate
	IntVB	stat_this;			// Status of THIS Port: ACTIVE ? TRUE : FALSE;
	IntVB	stat_primary;		// Primary Port Status: 0 == off-line, 1 == active, 2 == standby
	IntVB	stat_alternate;		// Alternate Port Status: 0 == off-line, 1 == active, 2 == standby
}SCPReplyDualPort;

typedef struct tagSCPReplyBioAddResult
{	IntVB	nBioType;			// bio type, as defined in enCcScpBioDbSpec1 (C_1131)
	IntVB	nResult;			// Result Code:
								// -1 == General Error: record format
								// -2 == No Cardholder for this record
								// -3 == No Room for this record
								//  1 == ok, record accepted for add/modify
								//  2 == ok, delete processed
	__int64	nCardId;			// Cardholder ID the command was sent to
	long    nCommandTag;		// command tag this record was part of (SCPReplyCmndStatus::sequence_number)
}SCPReplyBioAddResult;

typedef struct tagSCPReplyLoginInfo
{	char	name[17];		// user name
	char	notes[33];		// notes on user (only for standard login types)
	IntVB   acctType;		// user level
	IntVB	userType;		// user type (0=standard web, 1=PSIA)
	IntVB	userId;			// user id
}SCPReplyLoginInfo;

typedef struct tagSCPReplyPkgInfo
{	char	pkgName[33];		// pkgName
	char	pkgVersion[33];		// pkgVersion
	__int64 installDate;		// install date of package
}SCPReplyPkgInfo;

typedef struct	tagSCPReplyCertInfo
{
	char issuedTo[100];		// Cert issued to  
	char issuedBy[100];		// Cert issued by this
	char issuedStart[12];		// Cert issued Start Date
	char issuedExpire[12];		// Cert issued Expire Date
} SCPReplyCertInfo;

typedef struct	tagSCPReplyElevRelayInfo
{
	IntVB acr_number;			// Elevator(ACR) number
	BYTE status[128];
} SCPReplyElevRelayInfo;

typedef struct tagSCPReplyOsdpPassthrough
{	IntVB	acr_number;		// ACR for the reader from which this message originated
	long	sequence_num;	// Sequence number to match commands up with responses
	IntVB	reader_role;	// 0 for primary reader, 1 for alternate reader on the ACR
	IntVB	msg_type;		// 0=MFG, 1=XRD, 2=ACK, 3=NAK, 4=BUSY
	IntVB	data_len;		// Length of the nData message;
	BYTE	data[1024];		// Data portion of message
}SCPReplyOsdpPassthrough;

typedef struct tagSCPReplySioRelayCounts
{	IntVB	sio_number;			// SIO
	IntVB	num_relays;			// Number of relays on SIO
	long	relay_counts[32];	// Relay activation counts (relay # is index)
}SCPReplySioRelayCounts;

typedef struct tagSCPReplyHidMfgInfo
{
	IntVB	sio_number;			// SIO
	BYTE	serial_no[30];	//  HID Serial No
	BYTE	uuid[16];		// HID UUID
}SCPReplyHidMfgInfo;

// Status report structures
typedef struct tagSCPReplySrMsp1Drvr
{	IntVB number;				// MSP1 driver number (always 0 for SCP-2)
	IntVB port;					// actual hardware port number (0, 1, ...)
	IntVB mode;					// mode: 0 == disabled, 1 == enabled
	long  baud_rate;			// (word) baud rate  eg.: 1200, ..., 38400
  	IntVB throughput;			// i/o transactions per second (approx)
}SCPReplySrMsp1Drvr;

typedef struct tagSCPReplySrFileInfo
{
	IntVB file_type;			// File type
	IntVB file_index;			// Index to current file (1...num_files)
	IntVB num_files;			// Total number of files of current type
	char fileName[100];			// Name of file
}SCPReplySrFileInfo;

typedef struct tagSCPReplySrSio
{	IntVB number;				// SIO number
   	IntVB com_status;			// comm status: encoded per tran codes for tranTypeSioComm
   	IntVB msp1_dnum;			// MSP1 driver number (0, 1, ...)
   	// the following block is valid only if the SIO is on-line
 	long  com_retries;			// retries since power-up, cumulative
	IntVB ct_stat;				// cabinet tamper status: TranCoS::status encoded
	IntVB pw_stat;				// power monitor status: TranCoS::status encoded
	IntVB model;				// identification: see C03_02
	IntVB revision;				// firmware revision number: see C03_02
	long  serial_number;		// serial number
	IntVB inputs;				// number of inputs
	IntVB outputs;				// number of outputs
	IntVB readers;				// number of readers
	IntVB ip_stat[32];			// input point status: TranCoS::status encoded
	IntVB op_stat[16];			// output point status: TranCoS::status encoded
	IntVB rdr_stat[8];			// reader tamper status: TranCoS::status encoded
	// extended Sio ID info --- fields added 2006/05/15
	IntVB nExtendedInfoValid;	// use the data below only if this field is non-zero
	IntVB nHardwareId;			// MR-50 == , MR-52 == , MR-16In == , MR-16Out == , MR-DT == ,
	IntVB nHardwareRev;
	IntVB nProductId;			// same as model
	IntVB nProductVer;			// -- application specific version of this ProductId
	IntVB nFirmwareBoot;		// BOOT code version info:   (maj(4) << 12)+(min(8) << 4) + (bld(4))
	IntVB nFirmwareLdr;			// Loader code version info: (maj(4) << 12)+(min(8) << 4) + (bld(4))
	IntVB nFirmwareApp;			// App code version info:    (maj(4) << 12)+(min(8) << 4) + (bld(4))
	IntVB nOemCode;				// OEM code assigned to this SIO (0 == none)
	BYTE  nEncConfig;			// SIO comm encryption support: 0=None, 1=AES Default Key, 2=AES Master/Secret Key, 3= PKI, 6=AES256 Session key
	BYTE  nEncKeyStatus;		// Status of Master/Secret Key; 0=Not Loaded to EP, 1=Loaded, unverified, 2=Loaded, conflicts w/SIO, 3=Loaded, Verified, 4=AES256 Verified.
	BYTE  mac_addr[6];			// MAC Address, if applicable, LSB first.
	IntVB emg_stat;				// emergency switch status: TranCoS::status encoded
}SCPReplySrSio;

typedef struct tagSCPReplySrMp
{	IntVB first;				// number of the first Monitor Point
	IntVB count;				// number of MP status entries
	IntVB status[100];			// MP status (trl07 encoded)
}SCPReplySrMp;

typedef struct tagSCPReplySrCp
{	IntVB first;				// number of the first Control Point
	IntVB count;				// number of CP status entries
	IntVB status[100];			// CP status (trl07 encoded)
}SCPReplySrCp;

typedef struct tagSCPReplySrAcr
{	IntVB number;				// ACR number
	IntVB mode;					// access control mode: C_308 encoded
	IntVB rdr_status;			// reader tamper  (TypeCoS::status)
	IntVB strk_status;			// strike relay   (TypeCoS::status)
	IntVB door_status;			// door status map  (TypeCoSDoor::door_status)
	IntVB ap_status;			// access point status (TypeCoSDoor::ap_status)
	IntVB rex_status0;			// rex-0 contact  (TypeCoS::status)
	IntVB rex_status1;			// rex-1 contact  (TypeCoS::status)
	IntVB led_mode;				// reader led mode:  C_315 encoded
	IntVB actl_flags;			// acr config flags (CC_ACR::actl_flags)
	IntVB altrdr_status;		// alternate reader tamper  (TypeCoS::status)
	IntVB actl_flags_extd;		// extended flags (same as CC_ACR::spare)
	IntVB nExtFeatureType;			// Ext. Feature Type (0=None, 1=Classroom, 2=Office, 3=Privacy, 4=Apartment, ..)
	IntVB nHardwareType;			// Hardware Type in use.
	BYTE  nExtFeatureStatus[32];	// Features variable by type, first byte hardware-specific binary inputs by convention.
	long  nAuthModFlags;
}SCPReplySrAcr;

typedef struct tagSCPReplySrTz
{	IntVB first;				// number of the first Timezone
	IntVB count;				// number of TZ status entries
	IntVB status[100];			// TZ status is bit-mapped:
								// 0x01 mask == tz active
								// 0x02 mask == time based scan state
								// 0x04 mask == time scan override
}SCPReplySrTz;

typedef struct tagSCPReplySrTv
{	IntVB first;				// number of the first Trigger Variable
	IntVB count;				// number of TV status entries
	IntVB status[100];			// TV status: set/clear
}SCPReplySrTv;

typedef struct tagSCPReplySrMpg
{	IntVB number;							// MPG number
	IntVB mask_count;						// mask count
	IntVB num_active;						// number of active MPs
	IntVB active_mp_list[MAX_MPPERMPG*2];	// list of the active point pairs (Type-Num) (MAX_MP_PER_MPG)
}SCPReplySrMpg;

typedef struct tagSCPReplySrArea
{	IntVB number;				// Area number
	IntVB flags;				// status map
//		1 - set if area is enabled (open)
//     -- - the multi-occupancy mode is coded using bit-1 and bit 2
//      0 - (both bit-1 and bit-2 are zero) multi-occupancy mode not enabled
//      2 - (bit-1 is set, bit-2 is zero)  "standard" multiple occupancy rules
//      4 - (bit-2 is set, bit-1 is zero)  "modified-1-man" multiple occupancy rules
//      6 - (both bit-1 and bit 2 are set) "modified-2-man" multiple occupancy rules
//	  128 - set if this area has NOT been configured (no area checks are made!!!)
	long  occupancy;			// occupancy count - standard users
	long  occ_spc;				// occupancy count - special users
}SCPReplySrArea;


typedef struct tagSCPReplyCmndStatus
{	IntVB	status;				// command delivery status:
								// - 0 = FAILED (could not send, SCP off-line)
								// - 1 = OK (delivered and accepted),
								// - 2 = NAK'd (command rejected by the SCP)
	long	sequence_number;	// sequence number assigned when posted

	// the following block is included only if status==2 (NAK)
	// this extension includes the information that will be returned in the following NAK reply
	SCPReplyNAK		nak;		// supplemental info - NAK details
}SCPReplyCmndStatus;

typedef struct tagSCPReplyCmndStatusExt
{	IntVB	status;				// command delivery status:
								// - 0 = FAILED (could not send, SCP off-line)
								// - 1 = OK (delivered and accepted),
								// - 2 = NAK'd (command rejected by the SCP)
    long	lSequenceFirst;     // sequence number assigned when posted
    long    lSequenceLast;

	// the following block is included only if status==2 (NAK)
	// this extension includes the information that will be returned in the following NAK reply
	SCPReplyNAK		nak;		// supplemental info - NAK details
}SCPReplyCmndStatusExt;

// sanbx Driver Reply Structure for App List
#define SANBX_MAX_NUM_APPS 40
#define SANBX_APP_VER_MAX_LEN 10
typedef struct tagSCPReplySanbxAppList
{	
	IntVB	nApps;
	struct {
		long appCode;				
		char  version[SANBX_APP_VER_MAX_LEN];			
		IntVB  state;				
	}apps[SANBX_MAX_NUM_APPS];					
}SCPReplySanbxAppList;

// configuration read commands     CONFIG_READ >>>
typedef struct tagSCPReplyMemRead
{	IntVB	nType;				// Block Type: 0 == raw memory dump, else it's SCP struct type
	long    nBase;				// physical address, or record number of this SCP struct type
	IntVB	nSize;				// size of the data record, in bytes
	char	nData[256];			// up to 256 bytes of data;
}SCPReplyMemRead;

typedef struct tagSCPReplyStrStatus
{	IntVB	nListLength;
	struct {
		IntVB nStrType;				// structure type - defined for CC_STRSRQ::nStructId[] in scp_out.h
		long  nRecords;				// number of records of this struct type
		long  nRecSize;				// the size of each record - in bytes
		long  nActive;				// the number of records that are active (valid for databse types)
	}sStrSpec[32];					// array of structures holding status for each structure type
}SCPReplyStrStatus;

// configuration read commands     CONFIG_READ <<<

// the SCP reply types: determines the part of the union to use
enum enSCPReplyType
{	enSCPReplyUnknown,			// 00 - not recognized: passed on as is
	enSCPReplyACK,				// 01 - ACK
	enSCPReplyCommStatus,		// 02 - comm_status
	enSCPReplyNAK,				// 03 - NAK
	enSCPReplyIDReport,			// 04 - ID report
	enSCPReplyUTAGReport,		// 05 - UTAG report
	enSCPReplyTranStatus,		// 06 - transaction log status
	enSCPReplyTransaction,		// 07 - transaction log event
	enSCPReplySrMsp1Drvr,		// 08 - status: MSP1 (SIO comm) driver
	enSCPReplySrSio,			// 09 - status: SIO
	enSCPReplySrMp,				// 10 - status: Monitor Point
	enSCPReplySrCp,				// 11 - status: Control Point
	enSCPReplySrAcr,			// 12 - status: Access Control Reader
	enSCPReplySrTz,				// 13 - status: Timezone
	enSCPReplySrTv,				// 14 - status: Trigger Variable
	enSCPReplyCmndStatus,		// 15 - Direct command delivery status
	enSCPReplySrMpg,			// 16 - status: Monitor Point Group
	enSCPReplySrArea,			// 17 - status: Access Area
	enSCPReplyDualPort,			// 18 - Dual Port Status
	enSCPReplyBioAddResult,		// 19 - Bio Add/Modify/Delete Result
	enSCPReplyStrStatus,		// 20 - SCP Structure Status report
	enSCPReplySrIps,			// 21 - IPS Group Status report
	enSCPReplySrIpsPts,			// 22 - IPS Group Status with Point List report
    enSCPReplyCmndStatusExt,    // 23 - Extended Direct command delivery status
    enSCPReplyLoginInfo,		// 24 - Login info
	enSCPReplyPkgInfo,			// 25 - Installed package information
	enSCPReplyWebConfigNotes,			// 26 - Web Config Notes
	enSCPReplyWebConfigNetwork,			// 27 - Web Config Network
	enSCPReplyWebConfigHostCommPrim,	// 28 - Web Config Host Comm
	enSCPReplyWebConfigHostCommAlt,		// 29 - Web Config Host Comm
	enSCPReplyWebConfigSessionTmr,		// 30 - Web Config Session Timer
	enSCPReplyWebConfigWebConn,			// 31 - Web Config Web Connection
	enSCPReplyWebConfigAutoSave,		// 32 - Web Config Auto Save
	enSCPReplyWebConfigNetDiag,			// 33 - Web Config Network Diagnostic
	enSCPReplyWebConfigTimeServer,		// 34 - Web Config Time Server
	enSCPReplyWebConfigCentralStation,	// 35 - Web Config Central station
	enSCPReplyWebConfigCardDBSize,		// 36 - Web Config Card DB Size
	enSCPReplyWebConfigDiagnostics,		// 37 - Web Config Diagnostics
	enSCPReplyCertInfo,					// 38 - Web Config Cert Info
	enSCPReplyElevRelayInfo,				// 39 - Elevator Relay Status Info
	enSCPReplySrFileInfo,				//40 - File Information
	enSCPReplyOsdpPassthrough = 50,	// 50 - OSDP passthrough
	enSCPReplySioRelayCounts,		// 51 - SIO relay counts
	enSCPReplyHidMfgInfo,			//52 - HID Serial No and UUID

// Sanbx - Add reply structure. 
	enSCPReplySanbxAppList=60,	

#ifdef SIO_COM_H
// Direct SIO Support (used only on devices connected on serial comm with CC_NEWSCP::nCommAccess set to 9)
	enCcSioReply = 601,			// 601 Applies only to direct SIO comm
#endif

// configuration read commands     CONFIG_READ >>>
	enSCPReplyAdbs=enCcScpAdbSpec,
	enSCPReplyAccException=enCcAccException,
	enSCPReplyMP=enCcMP,
	enSCPReplyCP=enCcCP,
	enSCPReplyACR=enCcACR,
	enSCPReplyTimezone=enCcScpTimezone,
	enSCPReplyHoliday=enCcScpHoliday,
	enSCPReplyTrgr=enCcTrgr,
	enSCPReplyTrgr128=enCcTrgr128,
	enSCPReplyCard=enCcAdbCardI64DTic32,
	enSCPReplyAlvl=enCcAlvl,
	enSCPReplyMemRead = enCcMemRead,
	enSCPReplyCard255FF = enCcAdbCardI64DTic32A255FF,
	enSCPReplyBioTemplate=SPARE_3133,

// configuration read commands     CONFIG_READ <<<
};

typedef union _SCP_REPLYTYPE
{	SCPReplyCommStatus		comm;			// enSCPReplyCommStatus
	SCPReplyNAK				nak;			// enSCPReplyNAK
	SCPReplyIDReport		id;				// enSCPReplyIDReport
	SCPReplyUTAGReport		utags;			// enSCPreplyUTAGReport
	SCPReplyTranStatus		tran_sts;		// enSCPReplyTranStatus
	SCPReplyTransaction		tran;			// enSCPReplyTransaction
	SCPReplySrMsp1Drvr		sts_drvr;		// SIO comm driver
	SCPReplySrSio			sts_sio;		// SIO status
	SCPReplySrMp			sts_mp;			// MP status
	SCPReplySrCp			sts_cp;			// CP status
	SCPReplySrAcr			sts_acr;		// ACR status
	SCPReplySrTz			sts_tz;			// Timezone status
	SCPReplySrTv			sts_tv;			// Trigger Variable status
	SCPReplyCmndStatus		cmnd_sts;		// Direct command delivery status
	SCPReplySrMpg			sts_mpg;		// Monitor Point Group Status
	SCPReplySrArea			sts_area;		// - status: Access Area
	SCPReplyDualPort		dual_prt;		// enSCPDualPort
	SCPReplyBioAddResult	bio_result;		// Bio add/mod/del result
    SCPReplyCmndStatusExt   cmnd_sts_ext;   // Extended Direct command delivery status
	SCPReplyLoginInfo		login_info;		// Login info
	SCPReplyPkgInfo			pkg_info;		// Package info
	SCPReplyCertInfo		cert_info;		// Certificate info
	SCPReplyElevRelayInfo   elev_relay_info;            // Elevator Floor Relay Status
	SCPReplySrFileInfo		sts_file;		// File Info
	CC_WEB_CONFIG_NOTES				web_notes;			// Web Config Notes
	CC_WEB_CONFIG_NETWORK			web_network;		// Web Config Network
	CC_WEB_CONFIG_HOST_COMM_PRIM	web_host_comm_prim;	// Web Config Host Comm
	CC_WEB_CONFIG_SESSION_TMR		web_session_tmr;	// Web Config Session Timer
	CC_WEB_CONFIG_WEB_CONN			web_conn;			// Web Config Web Connection
	CC_WEB_CONFIG_AUTO_SAVE			web_auto_save;		// Web Config Auto Save
	CC_WEB_CONFIG_NETWORK_DIAG		web_net_diag;		// Web Config Network Diagnostic
	CC_WEB_CONFIG_TIME_SERVER		web_time_server;	// Web Config Time Server
	CC_WEB_CONFIG_DIAGNOSTICS		web_diagnostics;	// Web Config Diagnostics

	SCPReplyOsdpPassthrough			osdp_passthrough;	// OSDP Passthrough
	SCPReplySioRelayCounts			sio_relay_counts;	// SIO Relay Counts
	SCPReplyHidMfgInfo				hid_mfg_info;

#ifdef SIO_COM_H
// Direct SIO Support (used only on devices connected on serial comm with CC_NEWSCP::nCommAccess set to 9)
	SCPReplySioReply		sio_reply;	// reply returned from a "direct connect"-ed SIO
#endif
// configuration read commands     CONFIG_READ >>>
	CC_SCP_ADBS				adbs;
	CC_MP					mp;
	CC_CP					cp;
	CC_ACR					acr;
	CC_SCP_TZ				tz;
	CC_SCP_HOL				hldy;
	CC_TRGR					trgr;
	CC_TRGR_128				trgr128;
	CC_ALVL_EX				alvlEx;
	CC_ALVL					alvl;
	CC_ADBC_I64DTIC32		adbc;
	CC_ADBC_I64DTIC32A255FF	adbc255FF;
	SCPReplyMemRead			mem_read;
	CC_ACCEXCEPTION			acc_excpt;
	SCPReplyStrStatus		str_sts;

	// sanbx
	SCPReplySanbxAppList	sanbx_app_list;

// configuration read commands     CONFIG_READ <<<
	char reply_buffer[300];				// raw data - used for enSCPReplyUnknown
}SCP_REPLYTYPE;



typedef struct tagSCPReply
{	IntVB nSCPNumber;
	enum enSCPReplyType type;
	SCP_REPLYTYPE u;
}SCPReply;

#pragma pack()


#endif
