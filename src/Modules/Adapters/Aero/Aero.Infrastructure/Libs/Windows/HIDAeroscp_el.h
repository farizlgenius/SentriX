/***************************************************************************************
* Copyright  2024 HID Global Corporation.  All rights reserved.
* This software is protected by copyright law and international treaties, as well
* as any nondisclosure agreements that you or the organization you represent have signed.
* Any unauthorized reproduction, distribution or use of the software is prohibited.
****************************************************************************************/

#ifndef SCP_EL_H
#define SCP_EL_H

#ifndef IntVB
typedef short IntVB;
#endif

#define MAX_ELALVL			256			// max elevator access levels
#define	MAX_FLOORS_PER_ACR	128			// max floors (relays) per reader
#define MAX_CABINS			255			// max elevator cabins

#define	enCcElAlvlSpc		501			// Command_501 Elevator Access Level Specification
#define	enCcElAlvl			502			// Command_502 Config an Elevator Access Level

typedef struct				// (C_501) Elevator access level spec
{	long  lastModified;			// update tag
	IntVB scp_number;			// SCP number
	IntVB max_elalvl;			// number of elevator access levels to create (<=MAX_ELALVL)
	IntVB max_floors;			// maximum number of floors (relays) per ACR (<=MAX_FLOORS_PER_ACR)
	IntVB floor_offset;			// Offset floor mapping to define negative floor numbers
	IntVB floor_flags;			// bit-0 indicates if rear floors are supported
	IntVB max_cab_access;		// Maximum number of elevator cabin access configurations to create (<=32,000)
} CC_ELALVLSPC;

typedef struct				// (C_502) Elevator Access level configuration
{	long  lastModified;				// update tag
	IntVB scp_number;				// SCP number...
	IntVB el_number;				// ...access level number
	IntVB tz[MAX_FLOORS_PER_ACR];	// make a Timezone entry for each ACR
} CC_ELALVL;


typedef struct				// (C_311)
{	IntVB scp_number;			// SCP number
	IntVB acr_number;			// ACR number
	IntVB floor;				// relay to pulse (0==all, 1==first floor, through max_floors)
} CC_UNLOCK_EL;

// The ACR's configuration setting, in structure "CC_ACR::access_cfg"
#define ACR_A_ELEVATOR1		 4		// - elevator: contiguous relays, no feedback
#define ACR_A_ELEVATOR2		 5		// - elevator: contiguous relays, and floor selector inputs

// The "CC_ACR::pair_acr_number" parameter is re-defined for an "ACR_A_ELEVATORx" type ACR:
// If nonzero, "CC_ACR::pair_acr_number" selects the elevator access level for override
// -- relays follow the state of their corresponding timezones

// The "CC_ACR::strike_mode" parameter is re-defined for an "ACR_A_ELEVATORx" type ACR:
// The number of relays assigned to the ACR is set in "CC_ACR::strike_mode" (1 to MAX_FLOORS_PER_ACR)

// The "CC_ACR::rex_tzmask[0]" parameter is re-defined for an "ACR_A_ELEVATORx" type ACR:
// If non-zero, it the elevator access level that will auto-enable outputs if the Reader is off-line
// The "CC_ACR::rex_tzmask[1]" parameter is re-defined for an "ACR_A_ELEVATORx" type ACR:
// If non-zero, it defines the elevator access level that shall be used when in Facility Code mode
// --- else (if zero) all outputs are active on a valid facility code grant
// --- the relays will be OR-ed with the relays produced by the standard override (specd's by pair_acr_number)
// --- note: this executes once per minute only - there may be 59 second delay from online/offline to relay change
//
// The "CC_UNLOCK::strk_tm, CC_UNLOCK::t_held, and CC_UNLOCK::t:held_pre can not be set for elevators in the 311 command.	
//

#endif
