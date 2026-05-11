/***************************************************************************************
* Copyright  2024 HID Global Corporation.  All rights reserved.
* This software is protected by copyright law and international treaties, as well
* as any nondisclosure agreements that you or the organization you represent have signed.
* Any unauthorized reproduction, distribution or use of the software is prohibited.
****************************************************************************************/

#ifndef XLATERPY_H
#define XLATERPY_H

#ifndef Int16
typedef short Int16;
#endif

BOOL WINAPI scpXlateRpyCommStatus 
	( SCPReply * scpreply, SCPReplyCommStatus * rpyCommStatus);

BOOL WINAPI scpXlateRpyNAK 
		( SCPReply * scpreply,SCPReplyNAK * rpyNAK);

BOOL WINAPI scpXlateRpyIDReport
	 ( SCPReply * scpreply,SCPReplyIDReport * rpyIDReport);

BOOL WINAPI scpXlateRpyUTAGReport
	 ( SCPReply * scpreply,SCPReplyUTAGReport * rpyUTAGReport);

BOOL WINAPI scpXlateRpySrMsp1Drvr 
		( SCPReply * scpreply, struct tagSCPReplySrMsp1Drvr * rpySrMsp1Drvr);
BOOL WINAPI scpXlateRpySrSio
		( SCPReply * scpreply, struct  tagSCPReplySrSio * rpySrSio);
BOOL WINAPI scpXlateRpySrMp 
		( SCPReply * scpreply, struct tagSCPReplySrMp * rpySrMp);
BOOL WINAPI scpXlateRpySrCp 
		( SCPReply * scpreply, struct tagSCPReplySrCp * rpySrCp);
BOOL WINAPI scpXlateRpySrACR
		( SCPReply * scpreply, struct tagSCPReplySrAcr * rpySrACR);
BOOL WINAPI scpXlateRpySrTz
		( SCPReply * scpreply, struct tagSCPReplySrTz * rpySrTz);
BOOL WINAPI scpXlateRpySrTv
		( SCPReply * scpreply, struct tagSCPReplySrTv * rpySrTv);
BOOL WINAPI scpXlateRpyCmndStatus
		( SCPReply * scpreply, SCPReplyCmndStatus * rpyCmndStatus);
BOOL WINAPI scpXlateRpySrMpg
		( SCPReply * scpreply, struct tagSCPReplySrMpg * rpySrMpg);
BOOL WINAPI scpXlateRpySrArea
		( SCPReply * scpreply, struct tagSCPReplySrArea * rpySrArea);
BOOL WINAPI scpXlateRpySrFileInfo
		( SCPReply * scpreply, struct tagSCPReplySrFileInfo * rpySrFileInfo);
BOOL WINAPI scpXlateRpyTranStatus
	( SCPReply * scpreply, SCPReplyTranStatus * rpyTranStatus);

BOOL WINAPI scpGetTransType 
	( SCPReply * scpreply, Int16 * nTransType );

BOOL WINAPI scpXlateTransSys 
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransTypeSys );

BOOL WINAPI scpXlateDualPortReply
	( SCPReply * scpreply, struct tagSCPReplyDualPort * pReplyDualPort );

BOOL WINAPI scpXlateTransSioComm 
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransSioComm );

BOOL WINAPI scpXlateTransCardBin 
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransCardBin );

BOOL WINAPI scpXlateTransCardBCD 
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransCardBCD );

BOOL WINAPI scpXlateTransCardFull 
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransCardFull );

BOOL WINAPI scpXlateTransDblCardFull
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransDblCardFull );

BOOL WINAPI scpXlateTransCardID
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransCardID );

BOOL WINAPI scpXlateTransDblCardID
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransDblCardID );

BOOL WINAPI scpXlateTransUseLimit
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransUseLimit );

BOOL WINAPI scpXlateTransCoS 
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransCoS );

BOOL WINAPI scpXlateTransREX 
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransREX );

BOOL WINAPI scpXlateTransCoSDoor 
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransCoSDoor );

BOOL WINAPI scpXlateTransProcedure
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransProcedure );
	
BOOL WINAPI scpXlateTransUserCmd 
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransUserCmd );

BOOL WINAPI scpXlateTransActivate
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransActivate );
	
BOOL WINAPI scpXlateTransAcr
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransAcr );

BOOL WINAPI scpXlateTransMpg
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransMpg );

BOOL WINAPI scpXlateTransArea
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransArea );

BOOL WINAPI scpXlateTransUserCmdX
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransUserCmdX);
BOOL WINAPI scpXlateTransBio1
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransBio1);
BOOL WINAPI scpXlateTransAsci
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransAsci);
BOOL WINAPI scpXlateTransSioDiag
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransSioDiag);

BOOL WINAPI scpXlateTransAcrExtFeatureStls
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransAcr );

BOOL WINAPI scpXlateTransAcrExtFeatureCoS
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransAcr );

BOOL WINAPI scpXlateTransWebActivity
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransWebActivity );

BOOL WINAPI scpXlateTransAsset
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransAsset );

BOOL WINAPI scpXlateTransMpgIps
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransMpgIps );

BOOL WINAPI scpXlateTransAssetI64
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransAssetI64 );

BOOL WINAPI scpXlateTransBio1I64
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransBio1I64 );

BOOL WINAPI scpXlateTransI64CardFull
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransI64CardFull );

BOOL WINAPI scpXlateTransI64CardID
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransI64CardID );

BOOL WINAPI scpXlateTransI64CardFullIc32
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransI64CardFullIc32 );

BOOL WINAPI scpXlateTransHostCardFullPin
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransHostCardFullPin );

BOOL WINAPI scpXlateTransI64CardFullAAM
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransI64CardFullAAM );

BOOL WINAPI scpXlateTransI64CardIDAAM
	( SCPReply * scpreply, struct tagSCPReplyTransaction *  scpTransI64CardIDAAM );

BOOL WINAPI scpXlateRpyBioAddResult
		( SCPReply * scpreply, struct tagSCPReplyBioAddResult * rpyBioAddResult);

BOOL WINAPI scpXlateRpyLoginInfo
		( SCPReply * scpreply, struct tagSCPReplyLoginInfo * rpyLoginInfo);

BOOL WINAPI scpXlateRpyPkgInfo
		( SCPReply * scpreply, struct tagSCPReplyPkgInfo * rpyPkgInfo);

BOOL WINAPI scpXlateRpyOsdpPassthrough
		( SCPReply * scpreply, struct tagSCPReplyOsdpPassthrough * rpyOsdpPassthrough);

BOOL WINAPI scpXlateRpyStrStatus
		( SCPReply * scpreply, struct tagSCPReplyStrStatus * rpyStrStatus);

BOOL WINAPI scpXlateRpySrIps
		( SCPReply * scpreply, struct tagSCPReplySrIps * rpySrIps);

BOOL WINAPI scpXlateRpySrIpsPts
		( SCPReply * scpreply, struct tagSCPReplySrIpsPts * rpySrIpsPts);


#endif
