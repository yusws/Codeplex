/****** ===========================================================================
This script is a modified version of the one you'll find as part of the
Microsoft Hosted Application & Templates for Sharepoint
http://www.codeplex.com/SharePointHosters
============================================================================== ******/

USE [SecProviderDB]
GO
/****** Object:  Table [dbo].[aspnet_Sitemaps] ******/
-- This will create the aspnet_Sitemaps table required for site/domain scoping of users to applications
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Sitemaps](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[FQDN] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_aspnet_Sitemap_1] PRIMARY KEY CLUSTERED 
(
	[FQDN] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** USE [SecProviderDB]******/
GO
ALTER TABLE [dbo].[aspnet_Sitemaps]  WITH CHECK ADD  CONSTRAINT [FK_aspnet_Sitemap_aspnet_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])

/****** USE [SecProviderDB]******/
--GO
/****** Object:  StoredProcedure [dbo].[aspnet_Sitemaps_CreateMapping] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[aspnet_Sitemaps_CreateMapping]
	@ApplicationName	nvarchar(256),
	@FQDN				nvarchar(256)	
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ApplicationId uniqueidentifier
	SELECT @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    
	INSERT  dbo.aspnet_Sitemaps (ApplicationId, FQDN)
	VALUES  (@ApplicationId, @FQDN)

	RETURN 0

END

/****** USE [SecProviderDB]******/
--GO
/****** Object:  StoredProcedure [dbo].[aspnet_Sitemaps_DeleteMapping] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[aspnet_Sitemaps_DeleteMapping]
	@FQDN	nvarchar(256)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM dbo.aspnet_Sitemaps WHERE @FQDN = FQDN

END

/****** USE [SecProviderDB]******/
--GO
/****** Object:  StoredProcedure [dbo].[aspnet_Sitemaps_GetApplicationNameByFQDN] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[aspnet_Sitemaps_GetApplicationNameByFQDN]
    @FQDN      nvarchar(256)
AS
BEGIN

	SELECT Apps.ApplicationName 
	FROM aspnet_Applications Apps JOIN aspnet_Sitemaps Maps
	ON (Apps.ApplicationId = Maps.ApplicationId)
	WHERE Maps.FQDN = @FQDN

END

/****** USE [SecProviderDB]******/
--GO
/****** Object:  StoredProcedure [dbo].[aspnet_Sitemaps_GetApplicationsForUser] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[aspnet_Sitemaps_GetApplicationsForUser]
    @UserName      nvarchar(256)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Apps.ApplicationName 
	FROM aspnet_Applications Apps JOIN aspnet_Users Users
	ON (Apps.ApplicationId = Users.ApplicationId)
	WHERE Users.UserName = @UserName

END

GO

/****** Object:  DatabaseRole [aspnet_SiteMap_FullAccess] ******/
CREATE ROLE [aspnet_SiteMap_FullAccess] AUTHORIZATION [dbo]
GO

GRANT EXECUTE ON OBJECT::aspnet_Sitemaps_CreateMapping
	TO [aspnet_SiteMap_FullAccess];
GRANT EXECUTE ON OBJECT::aspnet_Sitemaps_DeleteMapping 
	TO [aspnet_SiteMap_FullAccess];
GRANT EXECUTE ON OBJECT::aspnet_Sitemaps_GetApplicationNameByFQDN
	TO [aspnet_SiteMap_FullAccess];
GRANT EXECUTE ON OBJECT::aspnet_Sitemaps_GetApplicationsForUser 
	TO [aspnet_SiteMap_FullAccess];
GO  

/****** Object:  DatabaseRole [aspnet_SiteMap_BasicAccess]   ******/
CREATE ROLE [aspnet_SiteMap_BasicAccess] AUTHORIZATION [dbo]
GO

GRANT EXECUTE ON OBJECT::aspnet_Sitemaps_GetApplicationNameByFQDN
	TO [aspnet_SiteMap_BasicAccess];
GRANT EXECUTE ON OBJECT::aspnet_Sitemaps_GetApplicationsForUser 
	TO [aspnet_SiteMap_BasicAccess];
GO  