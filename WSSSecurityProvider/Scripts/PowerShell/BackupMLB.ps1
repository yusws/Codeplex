#If documentation or updates become available, you'll find them at
#http://www.codeplex.com/mylocalbroadband


# ---------------------------------------------------------------------------
# SharePoint Functions
#
[Reflection.Assembly]::Load("Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c")

function Get-LocalFarm
{
   return [Microsoft.SharePoint.Administration.SPFarm]::Local
}

filter Get-WebService
{
   $webServices = new-object  Microsoft.SharePoint.Administration.SPWebServiceCollection($_)
   $webServices
}

filter Get-WebApplication
{
   $_.WebApplications
}

filter Get-SiteCollection 
{
   $_.Sites
}

filter Get-Web 
{
   $_.AllWebs
}
filter Get-Response
{
	$_
}
filter Get-DefaultPage()
{
	$url = $_.url
	$client= new-object System.Net.WebClient
	$client.UseDefaultCredentials = $True

	$start = [DateTime]::Now
	$content = $client.DownloadString($url)
	$end = [DateTime]::Now

	$timetaken = $end - $start
	$url + " - " + $timetaken.TotalSeconds + " seconds."
}
function Get-AllDefaultPages()
{
	Get-localfarm |get-webservice | get-webapplication | get-sitecollection | Get-DefaultPage
	
}
function Get-WebApplicationFromURL($url)
{
	return [Microsoft.SharePoint.Administration.SPWebApplication]::Lookup($url)
}
function Get-BackupDirectory($startDir)
{
	$d = get-date -format yyyyMMdd
	$path = $startDir + "\" + $d

	if (!(Test-Path -path $path))
	{
		$res = new-item -path $path -type directory 
	}

	return $startDir + "\" + $d
	
}
function Get-BackupFileName($site)
{
	$fileName = $site.url
	$fileName = $fileName.substring($fileName.indexof("//")+2)
	$fileName = $fileName.replace("/","-")
	return $fileName + ".bak"
}
filter New-SiteBackup ($backupDirectory)
{
	$bDir = Get-BackupDirectory($backupDirectory) 

	$fileName= Get-BackupFileName($_)
	$fileName = $bDir + "\" + $fileName

	$url = $_.url

	$url + " >> " + $fileName
	stsadm -o backup -url $url -filename $fileName
}
function New-WebAppBackup ($appURL, $backupDirectory)
{
	Get-WebApplicationFromURL($appURL) | get-sitecollection | New-SiteBackup $backupDirectory
}

function Remove-ArchiveDirectory($backupDirectory, $daystokeep)
{
	$d = [DateTime]::Today
	$d = $d.subtract([TimeSpan]::FromDays($daystokeep))	
	$path = $backupDirectory + "\" + $d.tostring("yyyyMMdd")

	if (Test-Path -path $path)
	{
		remove-item -path $path -recurse 
		return "Remove: " + $path
	}
	return "No Directory to Remove: " + $path
	
}


# -------------------------------------------------
# Script

New-WebAppBackup -appURL http://mylocalbroadband -backupDirectory c:\backups
Remove-ArchiveDirectory -backupDirectory \\<Server>\<directory> -daystokeep 30

