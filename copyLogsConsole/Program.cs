using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO.Packaging;
using System.Collections;
using System.Windows;
using System.Globalization;


namespace copyLogsConsole
{
    class Program
    {

        static int Main(string[] args)
        {
            #region preparation

            string logStatusReturned = applicationLog.checkLogFileSize();

            copyLogsConsole.applicationLog.WriteDebugLog("<-------------------------Start-of-log-file------------------------->");

            copyLogsConsole.applicationLog.WriteDebugLog("This package is developed for GarantiBBVABank. Supports H34,H68N and H68V series. With zip option. For both 32 and 64 bit Windows Operating Systems.");


            copyLogsConsole.applicationLog.WriteDebugLog(logStatusReturned);

            copyLogsConsole.applicationLog.WriteDebugLog("Begin.");

            var watch = System.Diagnostics.Stopwatch.StartNew();
            iniFile myIni = new iniFile();
            registry checkRegKeys = new registry();
            atm newATM = new atm();
            try
            {
                if (Environment.Is64BitOperatingSystem)
                { applicationLog.WriteDebugLog("OS is 64 bit"); }
                else { applicationLog.WriteDebugLog("OS is 32 bit"); }
            }
            catch (Exception ex)
            {

                applicationLog.WriteDebugLog(ex.ToString());
            }

            copyLogsConsole.applicationLog.WriteDebugLog("Checking ATM type...");

            string cashDevType = newATM.checkAtmType();
            copyLogsConsole.applicationLog.WriteDebugLog("ATM is " + cashDevType);
            //set timer   
            try
            {
                copyLogsConsole.applicationLog.WriteDebugLog("Checking if ini file exists...");
                string ad = System.Reflection.Assembly.GetEntryAssembly().Location;
                ad = ad.Replace(".exe", ".ini");
                myIni.IniFile(ad);
                if (!File.Exists(ad))
                {
                    myIni.createIniFile(ad);
                    copyLogsConsole.applicationLog.WriteDebugLog("Ini file not found, created");

                }
                else if (File.Exists(ad))
                {
                    //myIni.checkIniFile(myIni);

                    applicationLog.WriteDebugLog("Ini file has found");
                }
            }
            catch (Exception ex)
            {

                applicationLog.WriteDebugLog(ex.ToString());
            }

            try
            {
                applicationLog.WriteDebugLog("Checking registry keys for CRM/CDM log keys...");
                if (cashDevType == "H68N")
                {
                    if (Environment.Is64BitOperatingSystem)
                    { checkRegKeys.checkRegistryKeys(@"SOFTWARE\WOW6432Node\XFS\PHYSICAL_SERVICES\GrgCRM_CRM9250_XFS30", "H68N", myIni); applicationLog.WriteDebugLog("ATM type is H68N, checking SOFTWARE\\WOW6432Node\\XFS\\PHYSICAL_SERVICES\\GrgCRM_CRM9250_XFS30 keys."); }
                    else { checkRegKeys.checkRegistryKeys(@"SOFTWARE\XFS\PHYSICAL_SERVICES\GrgCRM_CRM9250_XFS30", "H68N", myIni); applicationLog.WriteDebugLog("ATM type is H68N, checking SOFTWARE\\XFS\\PHYSICAL_SERVICES\\GrgCRM_CRM9250_XFS30 keys."); }

                    //checkRegKeys.checkRegistryKeys(@"SOFTWARE\XFS\PHYSICAL_SERVICES\GrgCRM_CRM9250_XFS30", "H68N", myIni);
                    myIni.Write("atmType", "H68N");
                    applicationLog.WriteDebugLog("ATM type is H68N, checking SOFTWARE\\XFS\\PHYSICAL_SERVICES\\GrgCRM_CRM9250_XFS30 keys.");


                }
                if (cashDevType == "H34")
                {

                    if (Environment.Is64BitOperatingSystem)
                    { checkRegKeys.checkRegistryKeys(@"SOFTWARE\WOW6432Node\XFS\PHYSICAL_SERVICES\GrgCDM_CDM8240_XFS30", "H34", myIni); applicationLog.WriteDebugLog("ATM type is H34, checking SOFTWARE\\WOW6432Node\\XFS\\PHYSICAL_SERVICES\\GrgCDM_CDM8240_XFS30 keys."); }
                    else { checkRegKeys.checkRegistryKeys(@"SOFTWARE\XFS\PHYSICAL_SERVICES\GrgCDM_CDM8240_XFS30", "H34", myIni); applicationLog.WriteDebugLog("ATM type is H34, checking SOFTWARE\\XFS\\PHYSICAL_SERVICES\\GrgCDM_CDM8240_XFS30 keys."); }

                    //checkRegKeys.checkRegistryKeys(@"SOFTWARE\XFS\PHYSICAL_SERVICES\GrgCDM_CDM8240_XFS30", "H34", myIni);
                    myIni.Write("atmType", "H34");


                }
                if (cashDevType == "H68V")
                {
                    applicationLog.WriteDebugLog("ATM type is H68V, no need to check the registry keys.");
                }

            }
            catch (Exception ex)
            {

                copyLogsConsole.applicationLog.WriteDebugLog(ex.ToString());
            }


            string targetPath = "";

            //check that ini file has the outputDirectory
            if (myIni.Read("outputDirectory") == "")
            {
                targetPath = @"C:\GrgLogs";
                applicationLog.WriteDebugLog("Target path is: " + targetPath);
            }
            else if (myIni.Read("outputDirectory") != "")
            {
                targetPath = myIni.Read("outputDirectory");
                applicationLog.WriteDebugLog("Target path is: " + targetPath);
            }

            //delete and recreate output directory
            if (System.IO.Directory.Exists(targetPath))
            {
                applicationLog.WriteDebugLog("Delete the contents of the target directory: " + targetPath);

                try
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(targetPath);
                    Thread.Sleep(3000);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }

                    //System.IO.Directory.Delete(targetPath, true);
                    //Thread.Sleep(3000);
                    applicationLog.WriteDebugLog("The contents of the target log folder have been deleted.");
                }
                catch (Exception ex)
                {
                    applicationLog.WriteDebugLog(ex.ToString());
                }
            }

            if (!System.IO.Directory.Exists(targetPath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                    applicationLog.WriteDebugLog("Target log folder: " + targetPath + "has been created.");
                }
                catch (Exception ex)
                {
                    applicationLog.WriteDebugLog(ex.ToString());
                    applicationLog.WriteDebugLog("Target path: " + targetPath + " cannot be created");

                    targetPath = @"C:\GrgLogs";
                    applicationLog.WriteDebugLog("Target path: is C:\\GrgLogs");
                    System.IO.Directory.CreateDirectory(targetPath);

                }
            }

            applicationLog.WriteDebugLog("Log Copy begins.");
            applicationLog.WriteDebugLog("Reading date information from ini file...");
            string begDateYear = myIni.Read("logDateBegYear");
            string begDateMonth = myIni.Read("logDateBegMonth");
            string begDateDay = myIni.Read("logDateBegDay");
            string endDateYear = myIni.Read("logDateEndYear");
            string endDateMonth = myIni.Read("logDateEndMonth");
            string endDateDay = myIni.Read("logDateEndDay");

            date dateCheck = new date();
            applicationLog.WriteDebugLog("Checking beginning and ending days validity...");

            DateTime begDate = dateCheck.checkBegDate(begDateYear, begDateMonth, begDateDay, myIni);
            DateTime endDate = dateCheck.checkEndDate(endDateYear, endDateMonth, endDateDay, myIni);


            try
            {
                begDate = new DateTime(Convert.ToInt32(begDateYear), Convert.ToInt32(begDateMonth), Convert.ToInt32(begDateDay));
                endDate = new DateTime(Convert.ToInt32(endDateYear), Convert.ToInt32(endDateMonth), Convert.ToInt32(endDateDay));
            }
            catch (Exception ex)
            {

                applicationLog.WriteDebugLog(ex.ToString());
                begDate = DateTime.Today.AddDays(-7);
                endDate = DateTime.Today;

            }
            #endregion

            #region getLogDatesArray
            date logDates = new date();
            applicationLog.WriteDebugLog("Entered getLogDatesArray section. Creating log days array...");

            List<DateTime> logDaysArray = new List<DateTime>();
            try
            {
                if (Convert.ToBoolean(myIni.Read("logDateCriteria")) == true)
                {
                    logDaysArray = logDates.getlogDates(begDate, endDate);
                }
                else if (Convert.ToBoolean(myIni.Read("logIntervalCriteria")) == true)
                {
                    logDaysArray = logDates.getlogDates(Convert.ToInt32(myIni.Read("logDays")));
                }
                else if (myIni.Read("logDateCriteria") == "" && "logIntervalCriteria" == "")
                {
                    logDaysArray = logDates.getlogDates(Convert.ToInt32(myIni.Read("logDays")));
                }
            }
            catch (Exception ex)
            {

                copyLogsConsole.applicationLog.WriteDebugLog(ex.ToString());
            }


            applicationLog.WriteDebugLog("Exited getLogDaysArray section.");
            #endregion

            #region sp4LogCopy
            applicationLog.WriteDebugLog("Entered sp4Log section.");

            try
            {
                if (myIni.Read("sp4Log") == "true")
                {
                    log sp4Log = new copyLogsConsole.log();
                    List<FileInfo> sp4LogFiles = sp4Log.getLogFiles(Convert.ToBoolean(myIni.Read("sp4Log")), myIni.Read("sp4LogPath"));

                    List<FileInfo> sp4LogFilesDateFiltered = sp4Log.selectLogs(logDaysArray, sp4LogFiles, "^\\d{8}_\\d{6}_\\d{8}_\\d{6}$");

                    if (sp4LogFilesDateFiltered.Count() > 0)
                    { sp4Log.createSubFolder(Convert.ToBoolean(myIni.Read("sp4Log")), targetPath, "sp4"); }
                    sp4Log.copyLogs(sp4LogFilesDateFiltered, targetPath + "\\sp4", "SP4");

                    List<FileInfo> sp4ActualLogFile = sp4Log.getLogFiles(Convert.ToBoolean(myIni.Read("sp4Log")), myIni.Read("sp4LogPath"), "grgsp.grglog");
                    if (sp4ActualLogFile.Count > 0 && logDaysArray.Contains(DateTime.Today))
                        sp4Log.copyLogs(sp4ActualLogFile, targetPath + "\\sp4", "SP4 actual");
                    else if (!logDaysArray.Contains(DateTime.Today))
                        applicationLog.WriteDebugLog("Log dates does not contain today, so no actual sp4 log will be copied");

                }
                else if (myIni.Read("sp4Log") == "false")
                {
                    applicationLog.WriteDebugLog("Sp4Logs criteria is false. No Sp4Logs will be copied");

                }
            }
            catch (Exception ex)
            {

                copyLogsConsole.applicationLog.WriteDebugLog(ex.ToString());
            }


            applicationLog.WriteDebugLog("Exited sp4Log section");
            #endregion

            #region sp5LogCopy
            applicationLog.WriteDebugLog("Entered sp5Log section");

            try
            {
                if (myIni.Read("sp5Log") == "true")
                {
                    log sp5Log = new log();
                    List<FileInfo> sp5LogFiles = sp5Log.getLogFiles(Convert.ToBoolean(myIni.Read("sp5Log")), myIni.Read("sp5LogPath"), myIni.Read("sp5LogName"));
                    if (sp5LogFiles.Count() > 0)
                    { sp5Log.createSubFolder(Convert.ToBoolean(myIni.Read("sp5Log")), targetPath, "sp5"); }

                    sp5Log.copyLogs(sp5LogFiles, targetPath + "\\sp5", "SP5");
                }
                else if (myIni.Read("sp5Log") == "false")
                { applicationLog.WriteDebugLog("Sp5Logs criteria is false. No Sp5Logs will be copied"); }
            }
            catch (Exception ex)
            {

                copyLogsConsole.applicationLog.WriteDebugLog(ex.ToString());
            }
            applicationLog.WriteDebugLog("Exited sp5Log section");

            #endregion

            #region CRMLogs
            applicationLog.WriteDebugLog("Entered CRM/CDM log section.");

            if (myIni.Read("CRMLog") == "true")
            {
                List<FileInfo> crmLogFilesDateFiltered = new List<FileInfo>();
                log crmLog = new log();
                registry reg = new registry();

                try
                {//myIni.Read("atmType") 
                    if (cashDevType == "H34")
                    {

                        string regKeyValue = "";
                        if (Environment.Is64BitOperatingSystem)
                        {
                            regKeyValue = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\XFS\PHYSICAL_SERVICES\GrgCDM_CDM8240_XFS30").GetValue("ReadDevLogPath").ToString();
                        }
                        else { regKeyValue = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\XFS\PHYSICAL_SERVICES\GrgCDM_CDM8240_XFS30").GetValue("ReadDevLogPath").ToString(); }

                        List<FileInfo> crmLogs = crmLog.getLogFiles(Convert.ToBoolean(myIni.Read("CRMLog")), regKeyValue);

                        crmLogFilesDateFiltered = crmLog.selectCrmLogs(logDaysArray, crmLogs, "^GRGCDM8240DevLog_\\d{8}_\\d{6}", "H34");
                        applicationLog.WriteDebugLog("Cash device type is: " + cashDevType);
                    }//myIni.Read("atmType") 
                    else if (cashDevType == "H68N")
                    {

                        string regKeyValue = "";
                        if (Environment.Is64BitOperatingSystem)
                        { regKeyValue = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\XFS\PHYSICAL_SERVICES\GrgCRM_CRM9250_XFS30").GetValue("ReadDevLogPath").ToString(); }
                        else { regKeyValue = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\XFS\PHYSICAL_SERVICES\GrgCRM_CRM9250_XFS30").GetValue("ReadDevLogPath").ToString(); }
                        List<FileInfo> crmLogs = crmLog.getLogFiles(Convert.ToBoolean(myIni.Read("CRMLog")), regKeyValue);
                        crmLogFilesDateFiltered = crmLog.selectCrmLogs(logDaysArray, crmLogs, "^CRM9250DevLog_\\d{8}_\\d{6}", "H68N");
                        applicationLog.WriteDebugLog("Cash device type is: " + cashDevType);

                    }

                    else if (cashDevType == "H68V")
                    {

                        if (myIni.Read("CRMLogPath") != null)
                        {

                            List<FileInfo> crmLogs = crmLog.getLogFiles(Convert.ToBoolean(myIni.Read("CRMLog")), @"C:\GRGBanking\Log\Hardware\CRM9250NLog");
                            crmLogFilesDateFiltered = crmLog.selectCrmLogs(logDaysArray, crmLogs, "^CRM9250NLog_\\d{8}", "H68V");

                            applicationLog.WriteDebugLog("Cash device type is: " + cashDevType);

                        }


                        applicationLog.WriteDebugLog("Cash device type is: " + cashDevType);


                    }
                    if (crmLogFilesDateFiltered.Count() > 0)
                    {
                        crmLog.createSubFolder(Convert.ToBoolean(myIni.Read("CRMLog")), targetPath, "CRMLog");
                    }
                    crmLog.copyLogs(crmLogFilesDateFiltered, targetPath + "\\CRMLog", "CRMLog");

                }
                catch (Exception ex)
                {

                    copyLogsConsole.applicationLog.WriteDebugLog(ex.ToString());
                }
            }

            else if (myIni.Read("CRMLog") == "false")
            {
                applicationLog.WriteDebugLog("CRMLog criteria is false. No CRM/CDM Logs will be copied");
            }


            #endregion

            #region driverLogs
            applicationLog.WriteDebugLog("Entered driverLogs section.");

            try
            {
                if (myIni.Read("driverLog") == "true")
                {
                    log driverLog = new log();

                    string driverLogPathh = myIni.Read("driverLogPath");
                    List<FileInfo> driverLogs = driverLog.getLogFiles(Convert.ToBoolean(myIni.Read("driverLog")), myIni.Read("driverLogPath"));
                    List<FileInfo> driverLogsDateFiltered = driverLog.selectDriverLogs(logDaysArray, driverLogs, ".*\\(\\d{4}-\\d{2}-\\d{2}\\)\\..");

                    if (driverLogsDateFiltered.Count() > 0)
                    {
                        driverLog.createSubFolder(Convert.ToBoolean(myIni.Read("driverLog")), targetPath, "driverLogs");
                    }
                    driverLog.copyLogs(driverLogsDateFiltered, targetPath + "\\driverLogs", "Driver Logs");
                }
                else if (myIni.Read("driverLog") == "false")
                {
                    { applicationLog.WriteDebugLog("driverLog criteria is false. No driverLogs will be copied"); }
                }
            }
            catch (Exception ex)
            {

                copyLogsConsole.applicationLog.WriteDebugLog(ex.ToString());
            }
            applicationLog.WriteDebugLog("Exited driverLogs section.");

            #endregion

            #region duniteLogs
            applicationLog.WriteDebugLog("Entered duniteLogs section.");

            try
            {
                if (myIni.Read("duniteLog") == "true")
                {
                    log duniteLog = new log();
                    List<FileInfo> duniteLogs = duniteLog.getLogFiles(Convert.ToBoolean(myIni.Read("duniteLog")), myIni.Read("duniteLogPath"));
                    List<FileInfo> duniteLogsDateFiltered = duniteLog.selectDuniteLogs(logDaysArray, duniteLogs, ".*\\(\\d{4}-\\d{2}-\\d{2}\\).zip");

                    if (duniteLogsDateFiltered.Count() > 0)
                    {
                        duniteLog.createSubFolder(Convert.ToBoolean(myIni.Read("duniteLog")), targetPath, "duniteLogs");
                    }
                    duniteLog.copyLogs(duniteLogsDateFiltered, targetPath + "\\duniteLogs", "Dunite Logs");
                }

                else if (myIni.Read("duniteLog") == "false")
                {
                    { applicationLog.WriteDebugLog("duniteLog criteria is false. No duniteLogs will be copied"); }
                }
            }
            catch (Exception ex)
            {

                copyLogsConsole.applicationLog.WriteDebugLog(ex.ToString());
            }
            applicationLog.WriteDebugLog("Exited duniteLogs section.");

            #endregion

            #region xfsmanLogs
            applicationLog.WriteDebugLog("Entered XFS Manager Logs section.");

            try
            {
                if (myIni.Read("XFSManLog") == "true")
                {

                    log xfsManLog = new log();
                    List<FileInfo> xfsManLogs = xfsManLog.getLogFiles(Convert.ToBoolean(myIni.Read("XFSManLog")), myIni.Read("XFSManLogPath"));
                    List<FileInfo> xfsManLogsDateFiltered = xfsManLog.selectxfsManLogs(logDaysArray, xfsManLogs, "\\d{8}.txt");

                    if (xfsManLogsDateFiltered.Count() > 0)
                    {
                        xfsManLog.createSubFolder(Convert.ToBoolean(myIni.Read("XFSManLog")), targetPath, "xfsManLogs");
                    }
                    xfsManLog.copyLogs(xfsManLogsDateFiltered, targetPath + "\\xfsManLogs", "xfsManLogs");
                }
                else if (myIni.Read("XFSManLog") == "false")
                {
                    { applicationLog.WriteDebugLog("XFSManLog criteria is false. No xfsManLog will be copied"); }
                }
            }
            catch (Exception ex)
            {

                copyLogsConsole.applicationLog.WriteDebugLog(ex.ToString());
            }
            applicationLog.WriteDebugLog("Exited XFS Manager Logs section.");

            #endregion

            #region YDCLogs
            applicationLog.WriteDebugLog("Entered YDCLogs section");
            try
            {
                if (myIni.Read("YDCLogs") == "true")
                {
                    RegistryKey rKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\grgbanking\YDC");
                    log YDCLog = new log();



                    if (rKey != null)
                    {
                        applicationLog.WriteDebugLog("YDC installation found.");
                        string ydcLogPath = rKey.GetValue("PATH").ToString() + "\\ATMAPP\\log";
                        List<FileInfo> ydcLogs = YDCLog.getLogFiles(Convert.ToBoolean(myIni.Read("YDCLogs")), ydcLogPath);
                        //List<string> ydcRegexPatterns = new List<string> { @"\d{4}\.\d{2}\.\d{2}.*\.dmp", @"(COM)\d{6}\.log", @"(EJ)\d{6}\.log", @"(EMVKernel)\d{4}-\d{2}-\d{2}\.log", @"(Icc)\d{6}\.log", @"(TRACE)\d{6}\.log" };

                        List<FileInfo> ydcLogsDateFiltered = ydcLogsDateFiltered = YDCLog.selectYdcLogs(logDaysArray, ydcLogs);

                        if (ydcLogsDateFiltered.Count() > 0)
                        {
                            YDCLog.createSubFolder(Convert.ToBoolean(myIni.Read("YDCLogs")), targetPath, "YDCLogs");
                        }
                        YDCLog.copyLogs(ydcLogsDateFiltered, targetPath + "\\YDCLogs", "YDCLogs");

                    }
                    else if (rKey == null)
                        applicationLog.WriteDebugLog("YDC installation not found.");
                }

                else if (myIni.Read("YDCLogs") == "false")
                {
                    { applicationLog.WriteDebugLog("YDCLog criteria is false. No YDCLogs will be copied"); }
                }
            }
            catch (Exception ex)
            {

                ex.ToString();
            }
            applicationLog.WriteDebugLog("Exited YDCLogs section");

            #endregion

            #region Windows system log

            applicationLog.WriteDebugLog("Entered Windows system log section");

            //copy system event log


            try
            {
                bool winSystemLog = Convert.ToBoolean(myIni.Read("winSystemLog"));
                if (winSystemLog)
                {
                    log winSystemLogs = new log();
                    winSystemLogs.createSubFolder(winSystemLog, targetPath, "windowsSystemLog");
                    System.Diagnostics.ProcessStartInfo procStartInfo =
                        new System.Diagnostics.ProcessStartInfo("cmd", "/c " + "wevtutil epl system " + targetPath + "\\WindowsSystemLog\\system.evtx");

                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    //proc.StartInfo.UseShellExecute = false;
                    //proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo = procStartInfo;
                    proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    proc.Start();
                    applicationLog.WriteDebugLog("Windows system log has been copied.");
                }
                else if (myIni.Read("winSystemLog") == "false")
                {
                    { applicationLog.WriteDebugLog("winSystemLog criteria is false. Windows System event log will be copied"); }
                }
            }
            catch (Exception ex)
            {

                copyLogsConsole.applicationLog.WriteDebugLog(ex.ToString());
            }
            applicationLog.WriteDebugLog("Exited Windows system log section");

            #endregion

            #region zip

            applicationLog.WriteDebugLog("Entered zip section");

            Thread.Sleep(10000);

            try
            {



                //DirectoryInfo dir = System.IO.Directory(");

                string nowDateTime = DateTime.Now.ToLongDateString();

                string nowYear = DateTime.Now.Year.ToString();
                string nowMonth = DateTime.Now.Month.ToString();
                string nowDay = DateTime.Now.Day.ToString();
                string nowHour = DateTime.Now.Hour.ToString();
                string nowMinute = DateTime.Now.Minute.ToString();
                string nowSecond = DateTime.Now.Second.ToString();




                string zipFileName = myIni.Read("zipFileLocation") + "\\grgLogs_" + nowYear + nowMonth+nowDay+"_" + nowHour+nowMinute+nowSecond + ".zip";

                using (Package package = ZipPackage.Open(zipFileName, FileMode.Create))
                {
                    string startFolder = targetPath;

                    foreach (string currentFile in Directory.GetFiles(startFolder, "*.*", SearchOption.AllDirectories))
                    {
                        applicationLog.WriteDebugLog("Packing " + currentFile);
                        Uri relUri = zipFile.GetRelativeUri(currentFile);

                        PackagePart packagePart =
                        package.CreatePart(relUri, System.Net.Mime.MediaTypeNames.Application.Octet, CompressionOption.Maximum);
                        using (FileStream fileStream = new FileStream(currentFile, FileMode.Open, FileAccess.Read))
                        {
                            zipFile.CopyStream(fileStream, packagePart.GetStream());

                        }


                    }
                    copyLogsConsole.applicationLog.WriteDebugLog("Zip file has been created with " + Directory.GetFiles(startFolder, "*.*", SearchOption.AllDirectories).Length.ToString() + " files.");
                }
            }
            catch (Exception ex)
            {

                applicationLog.WriteDebugLog(ex.ToString());
            }
            applicationLog.WriteDebugLog("Exited zip section");

            #endregion

            #region end of program
            //Console.WriteLine("Logs have been copied successfully" + "\n" + "Please press a key to continue");
            applicationLog.WriteDebugLog("Reached the end of the program section");
            watch.Stop();
            var elapsedMs = watch.Elapsed;
            applicationLog.WriteDebugLog(elapsedMs.ToString());
            //Console.WriteLine("Total time elapsed:" + elapsedMs.ToString());
            //Console.ReadKey();
            try
            {
                if (Convert.ToInt16(registry.readDevLogReturn) != 0)
                {

                    applicationLog.WriteDebugLog("Problem with CRMLogs, registry value is different from zero.");

                }


            }
            catch (Exception ex)
            {

                applicationLog.WriteDebugLog(ex.ToString());
            }
            #endregion

            copyLogsConsole.applicationLog.WriteDebugLog("<--------------------------End-of-log-file-------------------------->");

            return 0;

        }
    }
}
