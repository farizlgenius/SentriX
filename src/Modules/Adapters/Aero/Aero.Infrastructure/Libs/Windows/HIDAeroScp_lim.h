/*************************************************************************************
* Copyright © 2019 HID Global Corporation.  All rights reserved.
* This software is protected by copyright law and international treaties, as well as any nondisclosure agreements that you or the organization you represent have signed.  
* Any unauthorized reproduction, distribution or use of the software is prohibited.
***********************************************************************************/

#ifndef SCP_LIM_H
#define SCP_LIM_H


#define MAX_PORTS		   1024
#define MAX_SCPS		   1024
#define MAX_SIO_PER_SCP		 32
#define MAX_ACR_PER_SCP		 64
#define MAX_ACR_PER_ALVL	128
#define MAX_ALVL_STD		  6			// max access levels with  304 command
#define MAX_ALVL_EXTD		 32			// max access levels with 1304/2304/3304/4304/5304 commands
#define MAX_ALVL_128		128			// max access levels with 6304,7304 commands
#define MAX_ALVL_255		255			// max access levels with 8304 commands
#define MAX_ALVL_VALUE		MAX_ALVL_EXTD		// max access levels with 8304 for HID AERO
#define MAX_PIN_STD			  8			// max PIN digits, use with 304/1304/2304 commands
#define MAX_PIN_EXTD		 15			// max PIN digits, use with 3304 command only
#define MAX_ULVL			  8			// max user levels that may be specified
#define MAX_FREEFORM_FIELDS   8			// up to 8 fields in the freeform card block.
#define	MAX_FREEFORM_SIZE   128			// Maximum size of the freeform block
#define MAX_OAL_PARAMS_SIZE	256			// Maximum size of the parameter array
#define MAX_SIM_KEY_SIZE    32 + 1		// Max Simulated Key Size, 32 NULL Terminated
#define MAX_OSDP_BITMAP_SIZE (MAX_ACR_PER_ALVL/8)
#define MAX_CONTROL_LIST     (MAX_ACR_PER_ALVL/8)

#define MAX_ICVT			  4			// max input conversion tables
#define MAX_CFMT			  8			// max card formatter tables
#define MAX_CFMT_A			  8			// max card formatter tables: cards + assets
#define MAX_TIMEZONE		255			// max timezones
#define MAX_HOLIDAY			255			// max holidays
#define MAX_TZINT			 12			// max intervals per time zone
#define MAX_DST_TBLS		 20			// up to 20 daylight tables
#define MAX_DSTPAIRS		 20			// up to 20 daylight savings start/stop pairs

#define MAX_MPG				256			// max Monitor Point Groups
#define MAX_MPPERMPG		128			// max Monitor Points per MP Group

#define MAX_SIODC_DATA		 (255)		// max length of the SIO Direct Command cData[] array
#define MAX_NV_DATA_SIZE	(255)		

#define MAX_ASSET_SIZE		 (16)		// max size of an asset ID, in bytes
#define MAX_OWNERS			  (8)		// max owners an asset may have
#define MAX_ASSET_ARGS		 (16)		// max number of asset arguments inuse at any one time
#define MAX_ASSET_CLASS_PER_GROUP (64)	// max asset classes that may be contained in an Asset Group

#define MAX_BIO1			  (9)		// max template size of "type-1" (RSI) biometric records
#define MAX_BIO2			(512)		// max template size of "type-2" (Identix) biometric records
#define MAX_BIO3			(348)		// max template size of "type-3" (BioScrypt) biometric records
#define MAX_BIO_TEMPLATE	(1536)		// max template size of biometric templates
#define MAX_BIO_TEMPLATE_EXT (2048)		// max template size of biometric templates using binary version of command

#define MAX_OSDP_PASSTHROUGH (1024)		// max OSDP passthrough data size

#define MAX_RLEDMODE		  3			// number of Reader LED modes supported
#define MAX_RLEDID			 15			// highest Reader LED ID defined

// user command related constants
#define	MAX_UCMND_NAME			(6)		// max user command "name" length
#define MAX_UCMND_ARGS_CONFIG	(4)		// max arguments preset by HOST config
#define MAX_UCMND_ARGS_LOG		(4)		// max arguments that may be logged

// Web services realated
#define	MAX_RESOURCES_PER_LOGIN		64	// Maximum number of individual resource entries per login.

#define MAX_OPERATING_MODES			8	// Maximum number of operating modes
#define MAX_LANGUAGES               16  // Maximum number of languages

#define MAX_ISSUER_NAME			100		//Maximum character for Issuer Name




#endif
