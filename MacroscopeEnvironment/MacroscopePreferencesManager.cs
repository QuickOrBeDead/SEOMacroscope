﻿/*

	This file is part of SEOMacroscope.

	Copyright 2017 Jason Holland.

	The GitHub repository may be found at:

		https://github.com/nazuke/SEOMacroscope

	Foobar is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	Foobar is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Foobar.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net;
using System.Reflection;
using System.Windows;

namespace SEOMacroscope
{

  public static class MacroscopePreferencesManager
  {

    /**************************************************************************/

    static MacroscopePreferences Preferences;
    static WebProxy wpProxy = null;

    // Application Version
    //static string AppVersion;
    
    /** Display Options ---------------------------------------------------- **/

    static Boolean PauseDisplayDuringScan;
    static Boolean ShowProgressDialogues;
    
    /** WebProxy Options --------------------------------------------------- **/

    static string HttpProxyHost;
    static int HttpProxyPort;
    //static string HttpProxyUsername;
    //static string HttpProxyPassword;

    /** Spidering Control -------------------------------------------------- **/

    static string StartUrl;
    static int MaxThreads;
    static int MaxFetchesPerWorker;
    static int Depth;
    static int PageLimit;
    static int RequestTimeout;
    static int MaxRetries;
    static int CrawlDelay;

    static Boolean IgnoreQueries;
        
    static Boolean CheckExternalLinks;

    static Boolean FollowRobotsProtocol;
    static Boolean FollowSitemapLinks;

    static Boolean FollowRedirects;
    static Boolean FollowNoFollow;
    static Boolean FollowCanonicalLinks;
    static Boolean FollowHrefLangLinks;
    static Boolean FollowListLinks;

    static Boolean FetchStylesheets;
    static Boolean FetchJavascripts;
    static Boolean FetchImages;
    static Boolean FetchAudio;
    static Boolean FetchVideo;
    static Boolean FetchXml;
    static Boolean FetchBinaries;

    /** Per-Job Spidering Options ------------------------------------------ **/

    static Boolean CrawlParentDirectories;
    static Boolean CrawlChildDirectories;

    /** Analysis Options --------------------------------------------------- **/

    static Boolean ResolveAddresses;
        
    static Boolean CheckHreflangs;

    static Boolean ProcessAudio;
    static Boolean ProcessBinaries;
    static Boolean ProcessImages;
    static Boolean ProcessJavascripts;
    static Boolean ProcessPdfs;
    static Boolean ProcessStylesheets;
    static Boolean ProcessVideo;
    static Boolean ProcessXml;

    static Boolean ScanSitesInList;
    static Boolean WarnAboutInsecureLinks;

    /** SEO Options -------------------------------------------------------- **/

    static int TitleMinLen;
    static int TitleMaxLen;
    static int TitleMinWords;
    static int TitleMaxWords;
    static int TitleMaxPixelWidth;

    static int DescriptionMinLen;
    static int DescriptionMaxLen;
    static int DescriptionMinWords;
    static int DescriptionMaxWords;

    static ushort MaxHeadingDepth;

    static Boolean AnalyzeKeywordsInText;

    static Boolean EnableLevenshteinDeduplication;
    static int MaxLevenshteinSizeDifference;
    static int MaxLevenshteinDistance;
    
    static Boolean DetectLanguage;

    /** Export Options ----------------------------------------------------- **/

    static Boolean SitemapIncludeLinkedPdfs;

    /** Advanced Settings -------------------------------------------------- **/

    static Boolean EnableMemoryGuard;

    /**************************************************************************/

    static MacroscopePreferencesManager ()
    {

      Preferences = new MacroscopePreferences ();

      //CheckAppVersion();
      
      SetDefaultValues();

      if( Preferences != null )
      {

        if( Preferences.FirstRun == true )
        {

          SetDefaultValues();
          Preferences.FirstRun = false;
          Preferences.Save();

        }
        else
        {

          PauseDisplayDuringScan = Preferences.PauseDisplayDuringScan;
          ShowProgressDialogues = Preferences.ShowProgressDialogues;
          
          HttpProxyHost = Preferences.HttpProxyHost;
          HttpProxyPort = Preferences.HttpProxyPort;

          StartUrl = Preferences.StartUrl;

          MaxThreads = Preferences.MaxThreads;
          MaxFetchesPerWorker = Preferences.MaxFetchesPerWorker;

          Depth = Preferences.Depth;
          PageLimit = Preferences.PageLimit;
          RequestTimeout = Preferences.RequestTimeout;
          MaxRetries = Preferences.MaxRetries;
          CrawlDelay = Preferences.CrawlDelay;

          IgnoreQueries = Preferences.IgnoreQueries;
          
          CheckExternalLinks = Preferences.CheckExternalLinks;

          ResolveAddresses = Preferences.ResolveAddresses;
            
          CheckHreflangs = Preferences.CheckHreflangs;
          ScanSitesInList = Preferences.ScanSitesInList;
          WarnAboutInsecureLinks = Preferences.WarnAboutInsecureLinks;
          
          EnableLevenshteinDeduplication = Preferences.EnableLevenshteinDeduplication;
          MaxLevenshteinSizeDifference = Preferences.MaxLevenshteinSizeDifference;
          MaxLevenshteinDistance = Preferences.MaxLevenshteinDistance;
          
          FollowRobotsProtocol = Preferences.FollowRobotsProtocol;
          FollowSitemapLinks = Preferences.FollowSitemapLinks;

          FollowRedirects = Preferences.FollowRedirects;
          FollowNoFollow = Preferences.FollowNoFollow;
          FollowCanonicalLinks = Preferences.FollowCanonicalLinks;
          FollowHrefLangLinks = Preferences.FollowHrefLangLinks;
          FollowListLinks = Preferences.FollowListLinks;

          FetchStylesheets = Preferences.FetchStylesheets;
          FetchJavascripts = Preferences.FetchJavascripts;
          FetchImages = Preferences.FetchImages;
          FetchAudio = Preferences.FetchAudio;
          FetchVideo = Preferences.FetchVideo;
          FetchXml = Preferences.FetchXml;
          FetchBinaries = Preferences.FetchBinaries;

          ProcessAudio = Preferences.ProcessAudio;
          ProcessBinaries = Preferences.ProcessBinaries;
          ProcessImages = Preferences.ProcessImages;
          ProcessJavascripts = Preferences.ProcessJavascripts;
          ProcessPdfs = Preferences.ProcessPdfs;
          ProcessStylesheets = Preferences.ProcessStylesheets;
          ProcessVideo = Preferences.ProcessVideo;
          ProcessXml = Preferences.ProcessXml;

          CrawlParentDirectories = Preferences.CrawlParentDirectories;
          CrawlChildDirectories = Preferences.CrawlChildDirectories;              

          TitleMinLen = Preferences.TitleMinLen;
          TitleMaxLen = Preferences.TitleMaxLen;
          TitleMinWords = Preferences.TitleMinWords;
          TitleMaxWords = Preferences.TitleMaxWords;
          TitleMaxPixelWidth = Preferences.TitleMaxPixelWidth;

          DescriptionMinLen = Preferences.DescriptionMinLen;
          DescriptionMaxLen = Preferences.DescriptionMaxLen;
          DescriptionMinWords = Preferences.DescriptionMinWords;
          DescriptionMaxWords = Preferences.DescriptionMaxWords;
          MaxHeadingDepth = Preferences.MaxHeadingDepth;
          AnalyzeKeywordsInText = Preferences.AnalyzeKeywordsInText;
    
          DetectLanguage = Preferences.DetectLanguage;
          
          SitemapIncludeLinkedPdfs = Preferences.SitemapIncludeLinkedPdfs;
          
          EnableMemoryGuard = Preferences.EnableMemoryGuard;
          
        }

      }

      SanitizeValues();

      ConfigureHttpProxy();

      DebugMsg( string.Format( "MacroscopePreferencesManager StartUrl: \"{0}\"", StartUrl ) );
      DebugMsg( string.Format( "MacroscopePreferencesManager Depth: {0}", Depth ) );
      DebugMsg( string.Format( "MacroscopePreferencesManager PageLimit: {0}", PageLimit ) );

    }

    /**************************************************************************/

    private static void CheckAppVersion ()
    {
      
      string SavedAppVersion = Preferences.AppVersion;
      Boolean DoReset = false;

      if( string.IsNullOrEmpty( SavedAppVersion ) )
      {
        DoReset = true;
      }

      if( DoReset )
      {
        SetDefaultValues();
        SavePreferences();
      }
      
    }

    /**************************************************************************/

    public static void SetDefaultValues ()
    {

      /** Display Options -------------------------------------------------- **/
      
      PauseDisplayDuringScan = false;
      ShowProgressDialogues = true;
                
      /** WebProxy Options ------------------------------------------------- **/

      HttpProxyHost = "";
      HttpProxyPort = 0;

      /** Spidering Control ------------------------------------------------ **/

      StartUrl = "";
      MaxThreads = 2;
      MaxFetchesPerWorker = 16;
      Depth = -1;
      PageLimit = -1;
      RequestTimeout = 10;
      MaxRetries = 0;
      CrawlDelay = 0;
          
      IgnoreQueries = false;
                
      CheckExternalLinks = false;

      FollowRobotsProtocol = true;
      FollowSitemapLinks = true;

      FollowRedirects = false;
      FollowNoFollow = true;
      FollowCanonicalLinks = true;
      FollowHrefLangLinks = false;
      FollowListLinks = false;

      FetchStylesheets = true;
      FetchJavascripts = true;
      FetchImages = true;
      FetchAudio = true;
      FetchVideo = true;
      FetchXml = true;
      FetchBinaries = true;

      ProcessAudio = true;
      ProcessBinaries = true;
      ProcessImages = true;
      ProcessJavascripts = true;
      ProcessPdfs = false;
      ProcessStylesheets = true;
      ProcessVideo = true;
      ProcessXml = true;

      /** Per-Job Spidering Options ---------------------------------------- **/

      CrawlParentDirectories = true;
      CrawlChildDirectories = true;

      /** Analysis Options ------------------------------------------------- **/

      ResolveAddresses = false;
      CheckHreflangs = true;
      ScanSitesInList = false;
      WarnAboutInsecureLinks = true;
      
      EnableLevenshteinDeduplication = true;
      MaxLevenshteinSizeDifference = 64;
      MaxLevenshteinDistance = 16;
      
      DetectLanguage = true;
      
      /** SEO Options ------------------------------------------------------ **/
      
      TitleMinLen = 10;
      TitleMaxLen = 70;
      TitleMinWords = 3;
      TitleMaxWords = 10;
      TitleMaxPixelWidth = 512;

      DescriptionMinLen = 10;
      DescriptionMaxLen = 150;
      DescriptionMinWords = 3;
      DescriptionMaxWords = 20;

      MaxHeadingDepth = 2;

      AnalyzeKeywordsInText = false;

      /** Export Options --------------------------------------------------- **/
            
      SitemapIncludeLinkedPdfs = false;
      
      /** Advanced Settings -------------------------------------------------- **/
      
      EnableMemoryGuard = true;
      
    }

    /**************************************************************************/

    static void SanitizeValues ()
    {

      if( StartUrl.Length > 0 )
      {
        StartUrl = Regex.Replace( StartUrl, "^\\s+", "" );
        StartUrl = Regex.Replace( StartUrl, "\\s+$", "" );
      }

      if( Depth <= 0 )
      {
        Depth = -1;
      }

      if( PageLimit <= 0 )
      {
        PageLimit = -1;
      }

      if( RequestTimeout <= 10 )
      {
        RequestTimeout = 10;
      }
      else
      if( RequestTimeout >= 50 )
      {
        RequestTimeout = 50;
      }

      if( MaxRetries <= 0 )
      {
        MaxRetries = 0;
      }
      else
      if( MaxRetries > 10 )
      {
        MaxRetries = 10;
      }

      if( CrawlDelay < 0 )
      {
        CrawlDelay = 0;
      }
      else
      if( CrawlDelay > 60 )
      {
        CrawlDelay = 60;
      }

      SavePreferences();

    }

    /**************************************************************************/

    public static void SavePreferences ()
    {

      if( Preferences != null )
      {

        Preferences.PauseDisplayDuringScan = PauseDisplayDuringScan;
        Preferences.ShowProgressDialogues = ShowProgressDialogues;
                        
        Preferences.HttpProxyHost = HttpProxyHost;
        Preferences.HttpProxyPort = HttpProxyPort;

        Preferences.StartUrl = StartUrl;

        Preferences.MaxThreads = MaxThreads;
        Preferences.MaxFetchesPerWorker = MaxFetchesPerWorker;

        Preferences.Depth = Depth;
        Preferences.PageLimit = PageLimit;
        Preferences.RequestTimeout = RequestTimeout;
        Preferences.MaxRetries = MaxRetries;
        Preferences.CrawlDelay = CrawlDelay;

        Preferences.IgnoreQueries = IgnoreQueries;
                  
        Preferences.CheckExternalLinks = CheckExternalLinks;

        Preferences.ResolveAddresses = ResolveAddresses;
        
        Preferences.CheckHreflangs = CheckHreflangs;
        Preferences.ScanSitesInList = ScanSitesInList;
        Preferences.WarnAboutInsecureLinks = WarnAboutInsecureLinks;

        Preferences.EnableLevenshteinDeduplication = EnableLevenshteinDeduplication;
        Preferences.MaxLevenshteinSizeDifference = MaxLevenshteinSizeDifference;
        Preferences.MaxLevenshteinDistance = MaxLevenshteinDistance;

        Preferences.FollowRobotsProtocol = FollowRobotsProtocol;
        Preferences.FollowSitemapLinks = FollowSitemapLinks;

        Preferences.FollowRedirects = FollowRedirects;
        Preferences.FollowNoFollow = FollowNoFollow;
        Preferences.FollowCanonicalLinks = FollowCanonicalLinks;
        Preferences.FollowHrefLangLinks = FollowHrefLangLinks;
        Preferences.FollowListLinks = FollowListLinks;

        Preferences.FetchStylesheets = FetchStylesheets;
        Preferences.FetchJavascripts = FetchJavascripts;
        Preferences.FetchImages = FetchImages;
        Preferences.FetchAudio = FetchAudio;
        Preferences.FetchVideo = FetchVideo;
        Preferences.FetchXml = FetchXml;
        Preferences.FetchBinaries = FetchBinaries;

        Preferences.ProcessAudio = ProcessAudio;
        Preferences.ProcessBinaries = ProcessBinaries;
        Preferences.ProcessImages = ProcessImages;
        Preferences.ProcessJavascripts = ProcessJavascripts;
        Preferences.ProcessPdfs = ProcessPdfs;
        Preferences.ProcessStylesheets = ProcessStylesheets;   
        Preferences.ProcessVideo = ProcessVideo;
        Preferences.ProcessXml = ProcessXml;

        Preferences.CrawlParentDirectories = CrawlParentDirectories;
        Preferences.CrawlChildDirectories = CrawlChildDirectories;

        Preferences.TitleMinLen = TitleMinLen;
        Preferences.TitleMaxLen = TitleMaxLen;
        Preferences.TitleMinWords = TitleMinWords;
        Preferences.TitleMaxWords = TitleMaxWords;
        Preferences.TitleMaxPixelWidth = TitleMaxPixelWidth;

        Preferences.DescriptionMinLen = DescriptionMinLen;
        Preferences.DescriptionMaxLen = DescriptionMaxLen;
        Preferences.DescriptionMinWords = DescriptionMinWords;
        Preferences.DescriptionMaxWords = DescriptionMaxWords;

        Preferences.MaxHeadingDepth = MaxHeadingDepth;

        Preferences.AnalyzeKeywordsInText = AnalyzeKeywordsInText;

        Preferences.DetectLanguage = DetectLanguage;
        
        Preferences.SitemapIncludeLinkedPdfs = SitemapIncludeLinkedPdfs;
                  
        Preferences.EnableMemoryGuard = EnableMemoryGuard;

        Preferences.Save();

      }

    }

    /** Display Options *******************************************************/

    public static Boolean GetPauseDisplayDuringScan ()
    {
      return( PauseDisplayDuringScan );
    }

    public static void SetPauseDisplayDuringScan ( Boolean State )
    {
      PauseDisplayDuringScan = State;
    }

    public static Boolean GetShowProgressDialogues ()
    {
      return( ShowProgressDialogues );
    }

    public static void SetShowProgressDialogues ( Boolean State )
    {
      ShowProgressDialogues = State;
    }

    /** HTTP Proxy ************************************************************/

    public static string GetHttpProxyHost ()
    {
      return( HttpProxyHost );
    }

    public static void SetHttpProxyHost ( string Value )
    {
      HttpProxyHost = Value;
    }

    public static int GetHttpProxyPort ()
    {
      return( HttpProxyPort );
    }

    public static void SetHttpProxyPort ( int Value )
    {
      HttpProxyPort = Value;
    }

    public static void ConfigureHttpProxy ()
    {

      string NewHttpProxyHost;
      int NewHttpProxyPort;

      if( HttpProxyHost.Length > 0 )
      {

        NewHttpProxyHost = HttpProxyHost;

        if( HttpProxyPort >= 0 )
        {
          NewHttpProxyPort = HttpProxyPort;
        }
        else
        {
          NewHttpProxyPort = 80;
        }

        DebugMsg( string.Format( "ConfigureHttpProxy: {0}:{1}", HttpProxyHost, HttpProxyPort ) );

        wpProxy = new WebProxy ( NewHttpProxyHost, NewHttpProxyPort );

      }
      else
      {

        DebugMsg( string.Format( "ConfigureHttpProxy: NOT USED" ) );

        wpProxy = null;

      }

    }

    public static WebProxy GetHttpProxy ()
    {
      return( wpProxy );
    }

    public static void EnableHttpProxy ( WebRequest req )
    {
      if( wpProxy != null )
      {
        req.Proxy = wpProxy;
      }
    }

    /**************************************************************************/

    public static string GetStartUrl ()
    {
      return( StartUrl );
    }

    public static void SetStartUrl ( string Url )
    {
      StartUrl = Url;
    }

    /**************************************************************************/

    public static int GetMaxThreads ()
    {
      return( MaxThreads );
    }

    public static void SetMaxThreads ( int iMaxThreads )
    {
      MaxThreads = iMaxThreads;
    }

    /**************************************************************************/

    public static int GetMaxFetchesPerWorker ()
    {
      return( MaxFetchesPerWorker );
    }

    public static void SetMaxFetchesPerWorker ( int Max )
    {
      MaxFetchesPerWorker = Max;
    }

    /**************************************************************************/

    public static int GetDepth ()
    {
      return( Depth );
    }

    public static void SetDepth ( int Max )
    {
      Depth = Max;
    }

    /**************************************************************************/

    public static int GetPageLimit ()
    {
      return( PageLimit );
    }

    public static void SetPageLimit ( int Max )
    {
      PageLimit = Max;
    }

    /** Request Timeout *******************************************************/

    public static int GetRequestTimeout ()
    {
      return( RequestTimeout );
    }

    public static void SetRequestTimeout ( int Seconds )
    {
      RequestTimeout = Seconds;
    }

    /** Maximum Retries *******************************************************/

    public static int GetMaxRetries ()
    {
      return( MaxRetries );
    }

    public static void SetMaxRetries ( int Retries )
    {
      MaxRetries = Retries;
    }

    /** Crawl Delay ***********************************************************/

    public static int GetCrawlDelay ()
    {
      return( CrawlDelay );
    }

    public static void SetCrawlDelay ( int Seconds )
    {
      CrawlDelay = Seconds;
    }

    /** Ignore Queries ********************************************************/

    public static Boolean GetIgnoreQueries ()
    {
      return( IgnoreQueries );
    }

    public static void SetIgnoreQueries ( Boolean State )
    {
      IgnoreQueries = State;
    }

    /** Domain Spidering Controls *********************************************/

    public static Boolean GetCheckExternalLinks ()
    {
      return( CheckExternalLinks );
    }

    public static void SetCheckExternalLinks ( Boolean State )
    {
      CheckExternalLinks = State;
    }

    /**************************************************************************/

    public static Boolean GetResolveAddresses ()
    {
      return( ResolveAddresses );
    }

    public static void SetResolveAddresses ( Boolean State )
    {
      ResolveAddresses = State;
    }

    /**************************************************************************/

    public static Boolean GetCheckHreflangs ()
    {
      return( CheckHreflangs );
    }

    public static void SetCheckHreflangs ( Boolean State )
    {
      CheckHreflangs = State;
    }

    /**************************************************************************/

    public static Boolean GetScanSitesInList ()
    {
      return( ScanSitesInList );
    }

    public static void SetScanSitesInList ( Boolean State )
    {
      ScanSitesInList = State;
    }

    /**************************************************************************/

    public static Boolean GetWarnAboutInsecureLinks ()
    {
      return( WarnAboutInsecureLinks );
    }

    public static void SetWarnAboutInsecureLinks ( Boolean State )
    {
      WarnAboutInsecureLinks = State;
    }

    /** Levenshtein Deduplication *********************************************/

    public static Boolean GetEnableLevenshteinDeduplication ()
    {
      return( EnableLevenshteinDeduplication );
    }

    public static void SetEnableLevenshteinDeduplication ( Boolean State )
    {
      EnableLevenshteinDeduplication = State;
    }

    public static int GetMaxLevenshteinSizeDifference ()
    {
      return( MaxLevenshteinSizeDifference );
    }

    public static void SetMaxLevenshteinSizeDifference ( int Max )
    {
      MaxLevenshteinSizeDifference = Max;
    }

    public static int GetMaxLevenshteinDistance ()
    {
      return( MaxLevenshteinDistance );
    }

    public static void SetMaxLevenshteinDistance ( int Max )
    {
      MaxLevenshteinDistance = Max;
    }

    /**************************************************************************/

    public static Boolean GetFollowRobotsProtocol ()
    {
      return( FollowRobotsProtocol );
    }

    public static void SetFollowRobotsProtocol ( Boolean State )
    {
      FollowRobotsProtocol = State;
    }

    /**************************************************************************/

    public static Boolean GetFollowSitemapLinks ()
    {
      return( FollowSitemapLinks );
    }

    public static void SetFollowSitemapLinks ( Boolean State )
    {
      FollowSitemapLinks = State;
    }

    /**************************************************************************/
    public static Boolean GetFollowRedirects ()
    {
      return( FollowRedirects );
    }

    public static void SetFollowRedirects ( Boolean State )
    {
      FollowRedirects = State;
    }

    /**************************************************************************/

    public static Boolean GetFollowNoFollow ()
    {
      return( FollowNoFollow );
    }

    public static void SetFollowNoFollow ( Boolean State )
    {
      FollowNoFollow = State;
    }

    /**************************************************************************/

    public static Boolean GetFollowCanonicalLinks ()
    {
      return( FollowCanonicalLinks );
    }

    public static void SetFollowCanonicalLinks ( Boolean State )
    {
      FollowCanonicalLinks = State;
    }

    /**************************************************************************/

    public static Boolean GetFollowHrefLangLinks ()
    {
      return( FollowHrefLangLinks );
    }

    public static void SetFollowHrefLangLinks ( Boolean State )
    {
      FollowHrefLangLinks = State;
    }

    /**************************************************************************/

    public static Boolean GetFollowListLinks ()
    {
      return( FollowListLinks );
    }

    public static void SetFollowListLinks ( Boolean State )
    {
      FollowListLinks = State;
    }

    /** CRAWL DOCUMENT TYPES **************************************************/

    public static Boolean GetFetchStylesheets ()
    {
      return( FetchStylesheets );
    }

    public static void SetFetchStylesheets ( Boolean State )
    {
      FetchStylesheets = State;
    }

    /** -------------------------------------------------------------------  **/

    public static Boolean GetFetchJavascripts ()
    {
      return( FetchJavascripts );
    }

    public static void SetFetchJavascripts ( Boolean State )
    {
      FetchJavascripts = State;
    }

    /** -------------------------------------------------------------------  **/

    public static Boolean GetFetchImages ()
    {
      return( FetchImages );
    }

    public static void SetFetchImages ( Boolean State )
    {
      FetchImages = State;
    }

    /** -------------------------------------------------------------------  **/

    public static Boolean GetFetchAudio ()
    {
      return( FetchAudio );
    }

    public static void SetFetchAudio ( Boolean State )
    {
      FetchAudio = State;
    }

    /** -------------------------------------------------------------------  **/

    public static Boolean GetFetchVideo ()
    {
      return( FetchVideo );
    }

    public static void SetFetchVideo ( Boolean State )
    {
      FetchVideo = State;
    }

    /** -------------------------------------------------------------------  **/

    public static Boolean GetFetchXml ()
    {
      return( FetchXml );
    }

    public static void SetFetchXml ( Boolean State )
    {
      FetchXml = State;
    }

    /** -------------------------------------------------------------------  **/

    public static Boolean GetFetchBinaries ()
    {
      return( FetchBinaries );
    }

    public static void SetFetchBinaries ( Boolean State )
    {
      FetchBinaries = State;
    }

    /** PROCESS DOCUMENT TYPES ************************************************/

    public static Boolean GetProcessAudio ()
    {
      return( ProcessAudio );
    }

    public static void SetProcessAudio ( Boolean State )
    {
      ProcessAudio = State;
    }

    /** -------------------------------------------------------------------  **/

    public static Boolean GetProcessBinaries ()
    {
      return( ProcessBinaries );
    }

    public static void SetProcessBinaries ( Boolean State )
    {
      ProcessBinaries = State;
    }

    /** -------------------------------------------------------------------  **/

    public static Boolean GetProcessImages ()
    {
      return( ProcessImages );
    }

    public static void SetProcessImages ( Boolean State )
    {
      ProcessImages = State;
    }
      
    /** -------------------------------------------------------------------  **/            

    public static Boolean GetProcessJavascripts ()
    {
      return( ProcessJavascripts );
    }

    public static void SetProcessJavascripts ( Boolean State )
    {
      ProcessJavascripts = State;
    }
      
    /** -------------------------------------------------------------------  **/    

    public static Boolean GetProcessPdfs ()
    {
      return( ProcessPdfs );
    }

    public static void SetProcessPdfs ( Boolean State )
    {
      ProcessPdfs = State;
    }
    
    /** -------------------------------------------------------------------  **/  

    public static Boolean GetProcessStylesheets ()
    {
      return( ProcessStylesheets );
    }

    public static void SetProcessStylesheets ( Boolean State )
    {
      ProcessStylesheets = State;
    }

    /** -------------------------------------------------------------------  **/

    public static Boolean GetProcessVideo ()
    {
      return( ProcessVideo );
    }

    public static void SetProcessVideo ( Boolean State )
    {
      ProcessVideo = State;
    }

    /** -------------------------------------------------------------------  **/

    public static Boolean GetProcessXml ()
    {
      return( ProcessXml );
    }

    public static void SetProcessXml ( Boolean State )
    {
      ProcessXml = State;
    }

    /** Per-Job Spidering Options *********************************************/

    public static Boolean GetCrawlParentDirectories ()
    {
      return( CrawlParentDirectories );
    }

    public static void SetCrawlParentDirectories ( Boolean State )
    {
      CrawlParentDirectories = State;
    }

    public static Boolean GetCrawlChildDirectories ()
    {
      return( CrawlChildDirectories );
    }

    public static void SetCrawlChildDirectories ( Boolean State )
    {
      CrawlChildDirectories = State;
    }

    /** SEO Options ***********************************************************/

    public static int GetTitleMinLen ()
    {
      return( TitleMinLen );
    }

    public static void SetTitleMinLen ( int Length )
    {
      TitleMinLen = Length;
    }

    public static int GetTitleMaxLen ()
    {
      return( TitleMaxLen );
    }

    public static void SetTitleMaxLen ( int Length )
    {
      TitleMaxLen = Length;
    }

    public static int GetTitleMinWords ()
    {
      return( TitleMinWords );
    }

    public static void SetTitleMinWords ( int Min )
    {
      TitleMinWords = Min;
    }

    public static int GetTitleMaxWords ()
    {
      return( TitleMaxWords );
    }

    public static void SetTitleMaxWords ( int Max )
    {
      TitleMaxWords = Max;
    }

    public static int GetTitleMaxPixelWidth ()
    {
      return( TitleMaxPixelWidth );
    }

    public static void SetTitleMaxPixelWidth ( int Max )
    {
      TitleMaxPixelWidth = Max;
    }

    /* ---------------------------------------------------------------------- */

    public static int GetDescriptionMinLen ()
    {
      return( DescriptionMinLen );
    }

    public static void SetDescriptionMinLen ( int Length )
    {
      DescriptionMinLen = Length;
    }

    public static int GetDescriptionMaxLen ()
    {
      return( DescriptionMaxLen );
    }

    public static void SetDescriptionMaxLen ( int Length )
    {
      DescriptionMaxLen = Length;
    }

    public static int GetDescriptionMinWords ()
    {
      return( DescriptionMinWords );
    }

    public static void SetDescriptionMinWords ( int Min )
    {
      DescriptionMinWords = Min;
    }

    public static int GetDescriptionMaxWords ()
    {
      return( DescriptionMaxWords );
    }

    public static void SetDescriptionMaxWords ( int Max )
    {
      DescriptionMaxWords = Max;
    }

    public static ushort GetMaxHeadingDepth ()
    {
      return( MaxHeadingDepth );
    }

    public static void SetMaxHeadingDepth ( ushort Depth )
    {
      MaxHeadingDepth = Depth;
    }

    public static Boolean GetAnalyzeKeywordsInText ()
    {
      return( AnalyzeKeywordsInText );
    }

    public static void SetAnalyzeKeywordsInText ( Boolean State )
    {
      AnalyzeKeywordsInText = State;
    }

    /* ---------------------------------------------------------------------- */

    public static Boolean GetDetectLanguage ()
    {
      return( DetectLanguage );
    }

    public static void SetDetectLanguage ( Boolean Detect )
    {
      DetectLanguage = Detect;
    }

    /** Export Options ********************************************************/

    public static Boolean GetSitemapIncludeLinkedPdfs ()
    {
      return( SitemapIncludeLinkedPdfs );
    }

    public static void SetSitemapIncludeLinkedPdfs ( Boolean State )
    {
      SitemapIncludeLinkedPdfs = State;
    }

    /** Advanced Settings *****************************************************/

    public static Boolean GetEnableMemoryGuard ()
    {
      return( EnableMemoryGuard );
    }

    public static void SetEnableMemoryGuard ( Boolean State )
    {
      EnableMemoryGuard = State;
    }

    /**************************************************************************/

    [Conditional( "DEVMODE" )]
    static void DebugMsg ( String sMsg )
    {
      System.Diagnostics.Debug.WriteLine( sMsg );
    }

    /**************************************************************************/

  }

}
