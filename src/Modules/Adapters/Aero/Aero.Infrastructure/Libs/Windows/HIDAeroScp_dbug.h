/*************************************************************************************
* Copyright © 2019 HID Global Corporation.  All rights reserved.
* This software is protected by copyright law and international treaties, as well as any nondisclosure agreements that you or the organization you represent have signed.  
* Any unauthorized reproduction, distribution or use of the software is prohibited.
***********************************************************************************/

#ifndef SCP_DEBUG_H
#define SCP_DEBUG_H

// debug message types
#define SCP_POLL			0
#define SCP_COMMAND			1
#define SCP_ACK				2
#define SCP_REPLY			3
#define SCP_REPLY_RETURN	4
#define SCP_MESSAGE			5
#define SCP_APP_CMND		6

// define messages for DebugThread
#define WM_SCP_SETDEBUG		(WM_USER + 101)

#define WM_SCP_POLL		    (WM_USER + 102 + SCP_POLL)
#define WM_SCP_COMMAND		(WM_USER + 102 + SCP_COMMAND)
#define WM_SCP_ACK			(WM_USER + 102 + SCP_ACK)
#define WM_SCP_REPLY		(WM_USER + 102 + SCP_REPLY)
#define WM_SCP_REPLY_RETURN	(WM_USER + 102 + SCP_REPLY_RETURN)
#define WM_SCP_MESSAGE		(WM_USER + 102 + SCP_MESSAGE)
#define WM_SCP_APP_CMND		(WM_USER + 102 + SCP_APP_CMND)


#define DBG_ERROR		0x10		// error message
#define DBG_WARN		0x20		// warning message
#define DBG_SCPCOM		0x30		// SCP communications message
#define DBG_STATUS		0x40		// status message: normal activity
#define DBG_MESSAGE		0x50		// application originated message

enum enSCPDebugLevel {		// debug level definitions
	enSCPDebugOff,
	enSCPDebugToFile,		// save debug text to a file
	enSCPDebugToFileCmnd,	// save debug text to a file and command log text to another file
	enSCPDebugToDebugger
};

//======================================//
// Debug Thread Info Data Type          //
//======================================//

#ifndef DBGTHREAD
typedef struct DBGTHREADSTRUCT
   {
   // used for the debug thread
   HANDLE         hDebugThread ;
   DWORD          dwDebugThreadID ;

   } DBGTHREAD ;
#endif

#ifndef PDBGTHREAD
typedef DBGTHREAD *PDBGTHREAD ;
#endif

#endif
