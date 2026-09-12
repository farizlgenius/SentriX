/***************************************************************************************
* Copyright  2024 HID Global Corporation.  All rights reserved.
* This software is protected by copyright law and international treaties, as well
* as any nondisclosure agreements that you or the organization you represent have signed.
* Any unauthorized reproduction, distribution or use of the software is prohibited.
****************************************************************************************/

#ifndef SIO_COM_H
#define SIO_COM_H


/* make this VB compatible... */
#pragma pack (4)

#ifndef IntVB
#define IntVB short
#endif

/* Direct SIO Support is available only for "SCP" devices that are connected on serial comm and are
   configured CC_NEWSCP::nCommAccess set to (9+256)
*/

/* Sio Commands */
enum enSioCommands
{	enScWRU = 101,		/* - 101 - Id Request (Who-Are-You) */
	enScRTime,			/* + 102 - Reply turnaround delay spec */
	enScICvt,			/* + 103 - Input conversion table transfer */
	enScICfg,			/* + 104 - Input point configuration */
	enScOCfg,			/* + 105 - output configuration */
	enScRCfg,			/* + 106 - reader configuration */
	enScCFmt,			/* + 107 - card format configuration */
	enScOlCfg,			/* + 108 - off-line reader configuration */
	enScOlRLed,			/* + 109 - off-line LED configuration */
	enScSioHexLoad,		/* + 110 - hex download */

	enScLSrq,			/* - 111 - local status report request */
	enScISrq,			/* - 112 - input point status report request */
	enScRTSrq,			/* - 113 - reader tamper status report request */
	enScDC,				/* + 114 - direct command */
	enScOCtl,			/* + 115 - output control command */
	enScRLCtl,			/* + 116 - reader LED control command */
	enScRBCtl,			/* + 117 - reader BUZZER control command */ 
	enScRdTxt,			/* + 118 - terminal text output command */
	enScOctlArr,		/* + 119 - output control - "array" capable MR-16-OUT and MR-52 ONLY! */
	enScxWRU,			/* - 120 - extended ID report */
	enScSchOvr,			/* + 121 - scheduled override */
	enScLoadKey,		/* + 122 - Load AES key into SIO */
	enScOSrq,			/* - 123 - output point status report request */
	enScApSetTime       /* + 124 - aperio set time structure */
};
/* Note: "+" marks command codes that have associated command structures */
/*       "-" marks commands that do not have command structures */

struct sioc_rtime			/* enScRTime (102) - Reply turnaround delay spec */
{	short reply_delay;			/* reply delay, in milliseconds; valid range: 2 to 20 ms */
};

struct sioc_icvt			/* enScICvt (103) - Input conversion table transfer */
{	short nTable;				/* (table number + 128): use 128, 129, 130, or 131 */
	short pri_5pr;				/* priority: 5% (non-settling error) */
	short sts_5pr;				/* status code: 5% (non-settling err) */
	struct
	{	short priority;			/* reporting priority (0, 1, or 2 - 0 is highest) */
		short status_code;		/* status code (0-7) */
		short res_code_1;		/* resistance code, range 1 */
		short res_code_2;		/* resistance code, range 2 */
	} table[8];					/* array of range spec structures */
};

struct sioc_icfg			/* enScICfg (104) - Input point configuration */
{	short nInput;				/* input number 0, 1, ... */
	short icvt_num;				/* scan conversion table number: 0 through 3 (standard), or 128 through 131 (custom) */
	short debounce;				/* number of input scans which must agree */
	short hold_time;			/* sec's to hold before  lower pri */
	short o_time;				/* sec's to hold higher priority changes while off-line: 0-239, 255 == forever */
};

struct sioc_ocfg			/* enScOCfg (105) - output configuration */
{	short nOutputs;				/* number of outputs to set (1-32) */
	char  cDriveMode[32];		/* 0 for normal drive, 1 for inverted drive, (the first entry is for output zero) */
};

struct sioc_rcfg			/* enScRCfg (106) - reader configuration */
{	short nReader;				/* reader number 0, 1, ... */
	short dt_fmt;				/* card data format flags: */
								/* use the "IDRDR_..." flags defined for CC_RDR:dt_fmt */
	short keypad_mode;			/* keypad data filter (config to SIO)*/
								/* use the "IDRDR_K_..." flags defined for CC_RDR:keypad_mode */
	short led_drive_mode;		/* reader LED color table index (1, 2, or 3)*/
								/* use the "IDRDR_L_..." flags defined for CC_RDR:led_drive_mode */
};

struct sioc_cfmt			/* enScCFmt (107) - card format configuration */
{	short nFormat;				/* format number 0, 1, ..., 7 */
	long  facility;				/* facility code */
	long  offset;				/* cardholder ID offset */
	short function_id;			/* card formatter function to use:  */
	union						/* this is the same union as the data type "SCP_CARD_FORMAT" */
	{	struct					/* arguments if function == CFMT_F_WGND or _F_MTA */
		{	short flags;
			short bits;			/* - number of bits on the card, if _F_MTA than min bits req'd */
			short pe_ln;		/* - number of bits to sum for even parity */
			short pe_loc;		/* - ...bit address to start from (_MTA: offset from first '1') */
			short po_ln;		/* - number of bits to sum for odd parity */
			short po_loc;		/* - ...bit address to start from (_MTA: offset from first '1') */
			short fc_ln;		/* - number of facility code bits */
			short fc_loc;		/* - ...bit address to start from (ms bit), (_MTA: offset from first '1') */
			short ch_ln;		/* - number of cardholder ID bits */
			short ch_loc;		/* - ...bit address to start from (ms bit), (_MTA: offset from first '1') */
			short ic_ln;		/* - number of issue code bits */
			short ic_loc;		/* - ...bit address to start from (ms bit), (_MTA: offset from first '1') */
		} sensor;
		struct					/* arguments if function == CFMT_F_MT2 */
		{	short flags; 		/* - control flags (0) */
			short min_digits;	/* - minimum number of digits on the card */
			short max_digits;	/* - maximum number of digits on the card */
			short fc_ln;		/* - number of facility code digits */
			short fc_loc;		/* - index to the most significant digit */
			short ch_ln;		/* - number of cardholder ID digits */
			short ch_loc;		/* - index to the most significant digit */
			short ic_ln;		/* - number of issue code digits */
			short ic_loc;		/* - index to the digit */
		} mt2_acs;
	}arg;
};

struct sioc_olrcfg			/* enScOlCfg (108) - off-line reader configuration */
{	short nReader;				/* reader number 0, 1, ... */
	short nOlMode;				/* off-line reader mode: see CC_ACRMODE::acr_mode (use 1,2,3, or 4) */
	long  nCfmtMsk;				/* bit map of which formats are enabled in offlone mode */
	short nRexIn;				/* input point to use for local REX: 0, 1, ..., or -1 */
	short nStrkOut;				/* output point to use as strike relay: 0, 1, ..., -1 */
	short nStrkTm;				/* strike time, in seconds ( 0 to 255) */
};

struct sioc_olrled			/* enScOlRLed (109) - off-line LED configuration */
{	short nAction;				/*  11 - deny, 12 - grant, 1,2,3,4 - off-line modes see CC_ACRMODE::acr_mode (use 1,2,3, or 4)*/
	short on_color;				/* color during "on_time" (0=off,1=red,2=green,3=amber) */
	short off_color;			/* color to display during "off_time" */
	short on_time;				/* duration to display "on_color" (.1sec/tick) */
	short off_time;				/* duration to display "off_color" (.1sec/tick) */
	short repeat_count;			/* if transient: number of times to repeat cycle */
	short beep_count;			/* if transient: number of times to beep */
};

struct sioc_hexload			/* enScSioHexLoad (110) - hex download: one Motorola "S" Record per command */
{	char  cHexLine[256];		/* one line of Motorola Hex formatted, '\0' terminated string */
								/* supported Motorola records: S0, S2, and S9 */
};

struct sioc_dircmnd			/* enScDC (114) - direct command (mainly for development use) */
{	short nCmndId;				/* Command ID - 00 through 0xFF */
	short nDataLength;			/* Number of bytes to use from the array below (0 to 100) */
	byte  cData[MAX_SIODC_DATA+1];			/* formatted character array of length == nDataLength */
};

/* Note:
   Relay and LED commands contain a Temporary and a Permanent command.
   Either the Temporary or the Permanent command code may be a NOP.
   The Temporary command, while it is in effect, supercedes the Permanent command.
*/
struct sioc_octl			/* enScOCtl (115) - output control command */
{	short nOutput;				/* output number 0, 1, ... (0 == Relay 1) */
	short nTmpCommand;			/* command code: 0 == NOP, 1==abort an temp action, 3==single pulse, or 4==repeating pulse */
	short nTmpOnTime;			/* on time - if command == 3 then units are multiple of 2 seconds(1 is 2seconds, 2 is 4seconds , ..), if command == 4 then .1 sec */
	short nTmpOffTime;			/* off time - used for mode 4 only,  units are .1 sec */
	short nTmpRepeat;			/* on/off repeat count - used for mode 4 only */
	short nPrmCommand;			/* command code: 0 == NOP, 1==off, 2==on, 3==single pulse, or 4==repeating pulse */
	short nPrmOnTime;			/* on time - if command == 3 then units are seconds, if command == 4 then .1 sec */
	short nPrmOffTime;			/* off time - used for mode 4 only,  units are .1 sec */
								/* if nPrmCommand is 4, then it will contiously cycle through the on/off sequence */
};

struct sioc_rlctl			/* enScRLCtl (116) - reader LED control command */
{	short nReader;				/* reader number 0, 1, ... */
	short nTmpCommand;			/* command code: 0 == NOP, 1==abort an temp action, 3==single pulse, or 4==repeating pulse */
	short nTmpOnColor;			/* color code to use during on-time: 0 = off, 1 = red, 2 = green, 3 = amber */
	short nTmpOffColor;			/* color code to use during off-time: 0 = off, 1 = red, 2 = green, 3 = amber */
	short nTmpOnTime;			/* on time - if command == 3 then units are multiple of 2 seconds(1 is 2seconds, 2 is 4seconds , ..), if command == 4 then .1 sec */
	short nTmpOffTime;			/* off time - used for mode 4 only,  units are .1 sec */
	short nTmpRepeat;			/* on/off repeat count - used for mode 4 only */
	short nPrmCommand;			/* command code: 0 == NOP, 1==off, 2==on, 3==single pulse, or 4==repeating pulse */
	short nPrmOnColor;			/* color code to use during on-time: 0 = off, 1 = red, 2 = green, 3 = amber */
	short nPrmOffColor;			/* color code to use during off-time: 0 = off, 1 = red, 2 = green, 3 = amber */
	short nPrmOnTime;			/* on time - if command == 3 then units are seconds, if command == 4 then .1 sec */
	short nPrmOffTime;			/* off time - used for mode 4 only,  units are .1 sec */
								/* if nPrmCommand is 4, then it will contiously cycle through the on/off sequence */
};

struct sioc_rbctl			/* enScRBCtl (117) - reader BUZZER control command */ 
{	short nReader;				/* reader number 0, 1, ... */
	short nBeeps;				/* select the number of short beeps (1 to 15 supported) */
};

struct sioc_rdtext			/* enScRdTxt (118) - terminal text output command */
{	short nReader;				/* reader number 0, 1, ... */
	short nTextType;			/* text command type: 1==permanent, 2==temp */
	short nTempTime;			/* duration for temp text display: 1 to 31 sec */
	short nRow;					/* text location: row number (1=top row) */
	short nColumn;				/* text location: column number (1=left-most) */
	char  cText[32+1];			/* <null> terminated ASCII text */
};

struct sioc_octl2			/* enScOctl2 (119) - "array" form of the relay control command - MR16-OUT and MR-52 ONLY!!! */
{	short nOutputs;				/* number of outputs to set in this command - MAX is 16 */
	struct {
		short nOutput;				/* output number 0, 1, ... (0 == Relay 1) */
		short nOutCommand;			/* command code */
#define SIO_OC2_OFF			(1)		// - off
#define SIO_OC2_ON			(2)		// - on
#define SIO_OC2_PULSE_ON	(3)		// - pulse on for nPulseTime, then turn off
#define SIO_OC2_PULSE_OFF	(4)		// - pulse off for nPulseTime, then turn on
		short nPulseTime;			/* pulse time, in units of .1 second steps */
	}sOutCmnd[16];					/* 16 entries max, actual number specified by nOutputs */
};

struct sioc_schover				/* enScSchOvr (121) - scheduled override */
{	short rdr_num;				/* Reader number on SIO */
	short override_type;		/* 0 = unlock, 1- future use, etc. */
	long nDuration;				/* LSB-->MSB unsigned DWORD of duration in minutes. */
};

struct sioc_loadkey				/* enScLoadKey (122) - Load AES key */
{	char nKeyId;				// Key Type: 2==Mk1 or 4==Session.  All other values invalid
	char cKey[16];				// AES Key array
};

struct sioc_aperiosettime {
	byte nReader;
	long e_sec;	//elapsed seconds from base yr
#define EP_BASE_YR_1900		0x01
#define LP_BASE_YR_1970		0x02
	byte baseYear;
};

typedef struct tagSioCommand	/* C_601 Applies only to direct SIO comm */
{	short nScpId;					/* device number - the SIOs are configured as special SCPs */
	short nSioCommand;				/* command code (enum enSioCommands) */
	union {
		struct sioc_rtime			rtime;			/* enScRTime (102) - Reply turnaround delay spec */
		struct sioc_icvt			icvt;			/* enScICvt (103) - Input conversion table transfer */
		struct sioc_icfg			icfg;			/* enScICfg (104) - Input point configuration */
		struct sioc_ocfg			ocfg;			/* enScOCfg (105) - output configuration */
		struct sioc_rcfg			rcfg;			/* enScRCfg (106) - reader configuration */
		struct sioc_cfmt			cfmt;			/* enScCFmt (107) - card format configuration */
		struct sioc_olrcfg			olrcfg;			/* enScOlCfg (108) - off-line reader configuration */
		struct sioc_olrled			olrled;			/* enScOlRLed (109) - off-line LED configuration */
		struct sioc_hexload			hexload;		/* enScSioHexLoad (110) - hex download */
		struct sioc_dircmnd			dircmnd;		/* enScDC (114) - direct command */
		struct sioc_octl			octl;			/* enScOCtl (115) - output control command */
		struct sioc_rlctl			rlctl;			/* enScRLCtl (116) - reader LED control command */
		struct sioc_rbctl			rbctl;			/* enScRBCtl (117) - reader BUZZER control command */ 
		struct sioc_rdtext			rdtext;			/* enScRdTxt (118) - terminal text output command */
		struct sioc_octl2			octl2;			/* enScOCtl (119) - output control command, "array" form */
		struct sioc_schover 		schover;		/* enScSchOvr (121) - scheduled override */
		struct sioc_loadkey 		loadkey;		/* enScLoadkey (122) - Load an AES symmetric key */
		struct sioc_aperiosettime 	aperiosettime;  /* enScApSetTime (124) - aperio set time structure */
		char cmnd_buf[100];				/* reserve a free form buffer, just in case */
	}sCmnd;
}CC_SIOCOMMAND;

/*
   CC_SIOCOMMAND is incorporated into union tagSCP_COMND {} in file "scp_out.h" as Command_601:
   a)
      enCcSioCommand = 601,				// 601 Applies only to direct SIO comm
   b)
      typedef union tagSCP_CMND
      {
      ...
      #ifdef SIO_COM_H
         CC_SIOCOMMAND  sio_cmnd;	// direct SIO command (601)
      #endif
      ...
      }SCP_CMND;
*/

enum enSioReplies
{	enSrAck=101,		/* - 101 - ACK - command accepted */
	enSrNak,			/* - 102 - "NAK - checksum/sqn" (non-specific rx error) */ 
	enSrNakLength,		/* - 103 - "NAK-command" too long */
	enSrNakCommand,		/* - 104 - "NAK-command ID" cannot be processed */
	enSrIdReport,		/* + 105 - Id Report (Who-Are-You) */
	enSrDiag,			/* + 106 - diagnostic report */
	enSrLocalSr,		/* + 107 - Local Status Report: SIO tamper, power */
	enSrInputSr,		/* + 108 - Input Point Status Report */
	enSrRTamperSr,		/* + 109 - Reader Tamper Status Report */
	enSrMR50Sr,			/* + 110 - Reader Tamper Status Report */
	enSrRDataBin,		/* + 111 - card data, binary bit array */
	enSrRDataBcd,		/* + 112 - card data, BCD digit array */
	enSrRDataKey,		/* + 113 - keypad data, ASCII key codes */
	enSrXIdReport,		/* + 114 - extended id report  */
	enSrOutputSr,		/* + 115 - Output Point status report */
	enSrActState,       /* + 116 - aperio activator state */
	enSrDoorMode        /* + 117 - aperio door mode */
};
/* Note: "+" marks reply codes that have associated reply structures */
/*       "-" marks replies that do not have reply structures */

struct sior_idr			/* 105 - SIO id report */
{	short nModel;			/* SIO model number: hardware & firmware dependent */
	short nRevision;		/* firmware revision code */
	long  nSerNum;			/* serial number */
	short nRxbLen;			/* rx buffer: max command size the sio can accept	*/
};

struct sior_diag		/* 106 - diagnostic status report */
{	short sw;				/* DIP switch image */
	short nInputs;			/* number of input in this report */
	char  cA2D[32];			/* raw A/D reading: 1 byte per point */
};

struct sior_lsr			/* 107 - Local status report: */
{	char cCtSts;			/* cabinet tamper status (see "input status codes", below) */
	char cPwrSts;			/* power monitor status */
};

struct sior_isr			/* 108 - Input status report */
{	short nInputs;			/* number of input in this report */
	char  cIpSts[32];		/* input status: 1 byte per input (start with Input-00), see note below for MR-50 */
};
/* input status codes:
	0 - inactive (normal state)
	1 - active
	2 - fault: grounded circuit
	3 - fault: shorted circuit
	4 - fault: open circuit
	5 - fault: foreign voltage
	6 - fault: non-settling error
*/

struct sior_rtsr		/* 109 - Reader Tamper status report */
{	short nReaders;			/* number of readers included in the report */
	char  cRdrTmpr[8];		/* tamper status: 1 byte per reader (starts with rdr-0) */
};

struct sior_mr50sr		/* 110 - MR-50, combined status report */
{	char cCtSts;			/* cabinet tamper status (see "input status codes", below) */
	char cRdrTmpr;			/* reader tamper status */
	char cIpSts0;			/* holds the MR-50's In-0 status (Board silk screen is In 1) */
	char cIpSts1;			/* holds the MR-50's In-1 status (Board silk screen is In 2) */
	char cIpSts2;			/* holds the MR-50's In-2 status (APERIO ONLY - this is key cylinder notification*/
};


struct sior_cdb			/* 111 - Card data, bit array */
{	short nReader;			/* reader number (0, 1, ...) */
	short nBitCount;		/* bit count (0-255) */
	char  cData[256/8];	/* bit array, actual size = (b_cnt+7)/8 */
};

struct sior_cdd			/* 112 - Card data, BCD Digits forward or reverse */
{	short nReader;			/* reader number (0, 1, ...) */
	short nReadDirection;	/* 0 == normal (forward) read, else == reverse read direction */
	short nDigitCount;		/* number of BCD digit count (0-45) */
	char  cData[45];		/* array of BCD digits, actual size = nDigitCount */
};

struct sior_key			/* 113 - Keypad data, ASCII character array */
{	short nReader;			/* reader number (0, 1, ...) */
	short nKeyCount;		/* number of key characters (0-15) */
	char  cKeys[16];		/* ASCII array, actual size = nKeyCount */
};
struct sior_idrx		/* 114 - Extended ID report */
{	byte nHardwareId;			// identifies the PCB
	byte nHardwareRev;			//    PCB Revision
	byte nProductId;			// identifies the Product Type
	byte nProductVer;			//    Product Version Code
	byte nFirmwareMaj;			// firmware - major
	byte nFirmwareMin;			//          - minor
	byte nFirmwareBld;			//          - build
	int nSerNum;				// serial number
	short nOemCode;				// OEM code - factory set constant
/* this field is new: some sio's may not return the length byte */
    byte rxb_ln;				// rx buffer: max command size the sio can accept
/* added to report boot and loader revision */
	byte nBootType;		/* boot code type -> 0x01 */
	byte nBootVer;		/* 			 version, starts from 01 */
	byte nBootMaj;		/*			 firmware major */
	byte nBootMin;		/*			 firmware minor */
	byte nBootBld;		/*			 firmware build */
	byte nLdrType;		/* loader code type -> 0x10 */
	byte nLdrVer;		/* 			   version, starts from 01 */
	byte nLdrMaj;		/*			   firmware major */
	byte nLdrMin;		/*			   firmware minor */
	byte nLdrBld;		/*			   firmware build */
	byte mac_addr[6];	/* MAC ADDRESS */
	int nHardwareComponents; // Hardware Components
};
struct sior_osr			/* 115 - Output status report */
{	short nOutputs;			/* number of output in this report */
	char  cOpSts[32];		/* output status: 1 byte per input (start with output-00) */
};

//activator state message (privacy mode)
struct sioc_aperio_actstate {
	byte nReader;
	byte door_side;
	byte state;
	byte id;
};

struct sioc_aperio_doormode {
	byte nReader;
#define AP_DOORMODE_UNKNOWN				0 //unknown (offline)
#define AP_DOORMODE_FREE_OPEN			1 //The door allows free access. No credentials need to be checked to allow entry. The door is open.
#define AP_DOORMODE_UNLOCK				2 //The door allows free access. No credentials need to be checked to allow entry. The door is unlocked.
#define AP_DOORMODE_LOCK				3 //Free passage is allowed from inside to outside, but access from outside in is restricted. The door is locked.
#define AP_DOORMODE_SECURE				4 //Free passage is allowed from inside to outside, but access from outside in is restricted. The door is security locked.
#define AP_DOORMODE_RESTRICT_LOCK		5 //The entry is restricted so entry is only allowed if the ACU commands it. The door is locked.
#define AP_DOORMODE_RESTRICT_SECURE		6 //The entry is restricted so entry is only allowed if the ACU commands it. The door is security locked.
	byte mode;
};

typedef struct tagSioReply	/* R_601 Will be returned only from devices configured as direct SIOs */
{	short nSioReply;			/* command code (enum enSioReplies) */
	union {
		struct sior_idr					idr;		/* 105 - SIO id report */
		struct sior_diag				diag;		/* 106 - diagnostic status report */
		struct sior_lsr					lsr;		/* 107 - Local status report: */
		struct sior_isr					isr;		/* 108 - Input status report */
		struct sior_rtsr				rtsr;		/* 109 - Reader Tamper status report */
		struct sior_mr50sr				mr50sr;		/* 110 - MR-50, combined status report */
		struct sior_cdb					cdb;		/* 111 - Card data, bit array */
		struct sior_cdd					cdd;		/* 112 - Card data, BCD Digit array, forward or reverse */
		struct sior_key					key;		/* 113 - Keypad data, ASCII character array */
		struct sior_idrx				xidr;		/* 114 - extended id report */
		struct sior_osr					osr;		/* 115 - Output status request */
		struct sioc_aperio_actstate 	actstate;   /* 116 - aperios activator state */
		struct sioc_aperio_doormode    	doormode;   /* 117 - aperio door mode */
	}sReply;
}SCPReplySioReply;

/*
   SCPReplySioReply is incorporated into union tagSCP_COMND {} in file "scp_in.h" as Reply_601:
  
   a)
     enCcSioReply = 601,				// 601 Applies only to direct SIO comm
		// 601 Applies only to direct SIO comm
   b)
     typedef union _SCP_REPLYTYPE
     {...
     #ifdef SIO_COM_H
        SCPReplySioReply	sio_reply;	// reply returned from a "direct connect"-ed SIO
     #endif
     ...
     }SCP_REPLYTYPE;
*/

#pragma pack()


#endif
