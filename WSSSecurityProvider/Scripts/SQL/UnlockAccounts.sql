USE [AspNetProvider] 

GO 

/****** Object: StoredProcedure [dbo].[aspnet_UnlockAccounts] Script Date: 04/16/2008 21:34:26 ******/ 

SET ANSI_NULLS ON 

GO 

SET QUOTED_IDENTIFIER ON 

GO 

-- ============================================= 

-- Author:    Ryan Miller 

-- Create date: 4/16/2008 

-- Description: Unlocks asp.net membership accounts that were locked out more than timeoutminutes minutes ago. 

-- ============================================= 

ALTER PROCEDURE [dbo].[aspnet_UnlockAccounts] ( 

@TimeOutMinutes int) 

AS 

BEGIN 

Update dbo.aspnet_Membership 

Set IsLockedOut = 0 

WHERE (IsLockedOut = 1) and (LastLockoutDate < (GETDATE() - (@TimeOutMinutes/1440.0))) 

END 
