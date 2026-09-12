/*************************************************************************************
* Copyright © 2019 HID Global Corporation.  All rights reserved.
* This software is protected by copyright law and international treaties, as well as any nondisclosure agreements that you or the organization you represent have signed.  
* Any unauthorized reproduction, distribution or use of the software is prohibited.
***********************************************************************************/
#ifndef _SCP_FUNCTIONS
#define _SCP_FUNCTIONS


#ifdef __cplusplus
extern "C" {
#endif

// FUNCTIONS FROM SCP_IN.H
// Comm handler and reply return from scp
// scpGetMessage() can be used in two forms.
// In both forms, scpGetMessage() runs the driver's port handlers.
// In one form, the parameter "pReply" points to  a validSCPReply structure. scpGetMessage()
//   returns a non-zero value, signaling that "pReply" has a valid SCPReply type message.
//   The application shall process the returned data and it shall repeatedly call 
//   scpGetMessage() until it returns zero. (scpGetScpMessage() is not used with this form.)
// The other form of scpGetMessage() is invoked by setting the "pReply" parameter to NULL.
//   Messages resulting from the channels are stored for retrieval by the scpGetScpMessage()
//   function. Therefore, in this form, the application calls scpGetMessage(0,NULL) once,
//   then it will call scpGetScpMessage() for each SCP, staying on each SCP until its
//   message has been completely retrieved (return value is zero).

//   If the first argument of scpGetMessage() references a valid Channel ID, then only that
//   channel is serviced by the scpGetMessage() function.
//   Otherwise (first argument is not a valid Channel ID), then all channels are serviced.
//   This supports an application architecture with threads servicing specific channels...
//
BOOL WINAPI scpGetMessage( IntVB nChannelID, SCPReply *pReply);
//   if ((pReply is NULL) and (nChannelID is a valid Channel)) { only nChannelID is serviced }
//   else { all channels are serviced }
//
BOOL WINAPI scpGetScpMessage( IntVB nScpId, SCPReply *pReply);

/********** FUNCTIONS FROM SCP_DBUG.H *************** */
// debug control: disable, set/change level
BOOL WINAPI scpDebugSet( long nLevel);
// ...same as above, with debug file's path\name specified
BOOL WINAPI scpDebugSetFile(long nLevel, LPSTR fn);
// diagnostic output handler: puts a string into the debug file
BOOL WINAPI scpDebug( int type, LPSTR lpszMessage);
// debug filter: set filtering on scpID (setting to -1 turns off all filtering)
// returns the number of SCP IDs being filtered on, maximum of 10
long WINAPI scpDebugSetScpId( long scpId);

/********** FUNCTIONS FROM SCP_OUT.H *************** */
// generic configuration command processor: using an ASCII command string
BOOL WINAPI scpConfigCommand ( char  *command );
BOOL WINAPI scpConfigCommandWithTag ( char  *command, int lCommandTag );

// SCP status and diagnostics support: returns bit map of active channels - 1==Primary, 2==Alternate
IntVB WINAPI scpCheckAttached( short nScpId );		// check if attached to a Channel
IntVB WINAPI scpCheckOnline( short nScpId );		// check if On-Line

// generic configuration command processor: using the CFG_CMND union
BOOL WINAPI scpCfgCmnd( CFG_CMND *cp );
BOOL WINAPI scpCfgCmndWithTag( CFG_CMND *cp, int lCommandTag );

/* Command Post and Command Tag support functions */
long WINAPI scpGetPostError( void );
long WINAPI scpGetTagLastPosted( IntVB scp );
long WINAPI scpGetTagLastSent( IntVB scp );

// Download status request: return codes correspond to the command codes:
//	enCcIcvt(101) through	enCcProc (118),
//  code 300 is used to indicate any user command pending,
//  zero indicates nothing pending
IntVB WINAPI scpGetDownloadMap( IntVB nScpId, IntVB load_map[30] );

// DLL version and other "support" functions
long WINAPI scpGetDllVersion( void );		// a.b.c.d format, encoded as (a<<24)+(b<<16)+(c<<8)+(d)
long WINAPI scpGetDriverState( short nChnlId );
//	enum p_state					/* port state								*/
//	{	PS_OFF,						/* - inactive								*/
//		PS_IDLE,					/* - idle									*/
//		PS_TX,						/* - sending a message						*/
//		PS_WSYNC,					/* - waiting for the sync byte				*/
//		PS_WSYNC2,					/* - waiting for the 2'nd byte (addr+RPLY)	*/
//		PS_RXHDR,					/* - receiving the rest of the header		*/
//		PS_RXMSG,					/* - receiving the message body				*/
//		PS_DONE,					/* - complete, return to user				*/
//		PS_ERR,						/* - error, waiting for timeout				*/
//		PS_WCNCT,					/* - Waiting for connection to happen		*/
//		PS_WRECNCT					/* - Waiting for reconnection time   		*/
//	}state;

BOOL WINAPI LongToDTS(const long *letime, char *datestring);  //e_sec's to string
long WINAPI GetEtime( void );							// system's clock (GMT-based)
long WINAPI GetGMTOffset( void );						// system's offset: seconds from GMT
long WINAPI GetDaylightOffset( void );					// system's daylight offset: 3600 or 0
long WINAPI GetScpLocalTime(short nScpID, long e_time);	// apply tz and daylight offsets


// Functions for .Net Wrapper support
short WINAPI GetCfgCmndSize(void);
short WINAPI GetScpReplySize(void);

unsigned long WINAPI scpMakeAuthCode(short nNvArgType, __int64 nNvArgValue, unsigned long nCmndAuthCode, short nProductId, unsigned long nSerialNumber);
// Not used; Unsupported in this version.

// AES Support
int scpAes128_Util(	int nCommandCode, const void *pVArg0, const void *pVArg1, void *pVArg2, void *pVArg3, void *pVArg4);
// Not used; Unsupported in this version.

#ifdef __cplusplus
}
#endif //cplusplus

#endif // _SCP_FUNCTIONS
