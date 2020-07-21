using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;

namespace copyLogsConsole
{
    public class iniFile
    {

        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public void IniFile(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName.ToString();
        }

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }

        public string returnPath()
        {
            var retVal = new iniFile();
            return retVal.Path;

        }




        public void createIniFile(string ad, int i = 0)
        {
            registry newReg = new registry();
            if (i == 1)
                File.Delete(ad);


            if (!File.Exists(ad))
            {
                atm cashDevType = new atm();
                string atmType = cashDevType.checkAtmType();
                Path = new FileInfo(ad ?? EXE + ".ini").FullName.ToString();
                var iniDosyasi = File.Create(ad);
                iniDosyasi.Close();
                //applicationLog.WriteDebugLog("Cannot find ini file, created ini file.");
                string[] iniContents = new string[61];
                iniContents[0] = "[copyLogsConsole]";
                iniContents[1] = "sp4Log=true";
                iniContents[2] = "//do you want to copy SP4 logs - true=yes false=no";
                iniContents[3] = "sp4LogPath=C:\\GrgBanking\\Log\\";
                iniContents[4] = "//where are the SP4 logs located?";
                iniContents[5] = "sp4LogName=grgsp.grglog";
                iniContents[6] = "//name of the actual SP4 log file";
                iniContents[7] = "sp5Log=false";
                iniContents[8] = "//do you want to copy SP5 logs - true=yes false=no";
                iniContents[9] = "sp5LogPath=C:\\GrgBanking\\Log\\";
                iniContents[10] = "//where is the SP5 log located?";
                iniContents[11] = "sp5LogName=GrgLog.dxdb";
                iniContents[12] = "//name of the SP5 log file";
                iniContents[13] = "CRMLog=true";
                iniContents[14] = "//do you want to copy CRM logs?	- true=yes false=no";
                iniContents[15] = "CRMLogPath=C:\\GrgBanking\\Log\\CRMLog";
                iniContents[16] = "//where are the CRM logs located?";
                iniContents[17] = "CRMLogName=";
                iniContents[18] = "//name of the CRM log files";
                iniContents[19] = "driverLog=false";
                iniContents[20] = "//do you want to copy driverlogs? - true=yes false=no";
                iniContents[21] = "driverLogPath=C:\\Program Files\\GrgBanking\\GRGXFSSP\\DevDriverLog\\";
                iniContents[22] = "//where are the driver logs located?";
                iniContents[23] = "driverLogName=";
                iniContents[24] = "//name of the driver log file";
                iniContents[25] = "duniteLog=false";
                iniContents[26] = "//do you want to copy dunite logs? - true=yes false=no";
                iniContents[27] = "duniteLogPath=C:\\GrgBanking\\Log\\hardware\\";
                iniContents[28] = "//where are the dunite logs located?";
                iniContents[29] = "duniteLogName=";
                iniContents[30] = "//what is the name of the dunitelog";
                iniContents[31] = "XFSManLog=false";
                iniContents[32] = "//do you want to copy XFS manager logs? - true=yes false=no";
                iniContents[33] = "XFSManLogPath=C:\\GrgBanking\\log\\GrgXfsManager\\";
                iniContents[34] = "//where are the XFSMan logs located?";
                iniContents[35] = "XFSMANLogName=GrgXfsManager.log";
                iniContents[36] = "//name of the XFSMan log";
                iniContents[37] = "YDCLog=false";
                iniContents[38] = "//Do you want to copy YDC logs? - true=yes false=no";
                iniContents[39] = "winSystemLog=false";
                iniContents[40] = "//Do you wantto export the Windows system log? - true=yes false=no";
                iniContents[41] = "logDateCriteria=false";
                iniContents[42] = "//True for date criteria, logDateBeg and logDateEnd will be used";
                iniContents[43] = "logDateBegYear=" + DateTime.Today.AddDays(-7).Year.ToString();
                iniContents[44] = "logDateBegMonth=" + DateTime.Today.AddDays(-7).Month.ToString();
                iniContents[45] = "logDateBegDay=" + DateTime.Today.AddDays(-7).Day.ToString();
                iniContents[46] = "//SP4 logs'will be copied based on the date criteria, this parameter will be the first day";
                iniContents[47] = "logDateEndYear=" + DateTime.Today.Year;
                iniContents[48] = "logDateEndMonth=" + DateTime.Today.Month;
                iniContents[49] = "logDateEndDay=" + DateTime.Today.Day;
                iniContents[50] = "//SP4 logs'will be copied based on the date criteria, this parameter will be the last day";
                iniContents[51] = "logIntervalCriteria=true";
                iniContents[52] = "//True for interval criteria, the logs will be copied based on the days criteria";
                iniContents[53] = "logDays=7";
                iniContents[54] = "//for ex. 10 means the the latest 10 days' SP4 logs will be copied. ";
                iniContents[55] = "outputDirectory=c:\\GRGLogs";
                iniContents[56] = "//where all the logs will be copied";
                iniContents[57] = "CRMLogDays=1";
                iniContents[58] = "//for SP package registry values, how many days for the CRM log will be generated";
                iniContents[59] = "atmType=" + atmType;
                iniContents[60] = "//What is the ATM type (H68 or H34)";

                //registry cashDevType = newReg.checkMachineType();

                File.WriteAllLines(ad, iniContents);

                //newReg.checkMachineType();
                //if (cashDevType.type == "H34")
                //    iniContents[59] = "CRM/CDM=CDM";
                //else if (cashDevType.type == "H68")
                //    iniContents[59] = "CRM/CDM=CRM";





            }

        }

        List<char> invalidChars = new List<char> { '<', '>', ':', '"', '/', '\\', '|', '?', '*' };


        public void checkIniFile(iniFile copyLogConsoleIniFile)
        {
            
            try
            {


                atm cashDevType = new atm();
                string atmType = cashDevType.checkAtmType();
                var list = new List<string>();


                using (var sr = new StreamReader(copyLogConsoleIniFile.Path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }


                List<string> iniContents = list;


                string ad = System.Reflection.Assembly.GetEntryAssembly().Location;
                ad = ad.Replace(".exe", ".ini");

                if (iniContents.Count() != 61)
                {
                    createIniFile(ad, 1);
                    applicationLog.WriteDebugLog("There is an inconsistency in the configuration file, please use the newly created one: copyLogConsole.ini.");

                }

                registry newReg = new registry();
                string CRMLogPath = newReg.readDevLogPath;
                if (String.IsNullOrEmpty(iniContents[0])) { iniContents[0] = "[copyLogsConsole]"; }
                bool outresult;
                if (iniContents[1].EndsWith("=") || !Boolean.TryParse(iniContents[1].Split('=')[1], out outresult)) { iniContents[1] = "sp4Log=true"; }
                if (String.IsNullOrEmpty(iniContents[2])) { iniContents[2] = "//do you want to copy SP4 logs - true=yes false=no"; }
                if (iniContents[3].EndsWith("=")) { iniContents[3] = @"sp4LogPath=C:\GrgBanking\Log"; }
                if (String.IsNullOrEmpty(iniContents[4])) { iniContents[4] = "//where are the SP4 logs located?"; }
                if (iniContents[5].EndsWith("=")) { iniContents[5] = "sp4LogName=grgsp.grglog"; }
                if (String.IsNullOrEmpty(iniContents[6])) { iniContents[6] = "//name of the actual SP4 log file"; }
                if (iniContents[7] == "" || !Boolean.TryParse(iniContents[7].Split('=')[1], out outresult)) { iniContents[7] = "sp5Log=false"; }
                if (String.IsNullOrEmpty(iniContents[8])) { iniContents[8] = "//do you want to copy SP5 logs - true=yes false=no"; }
                if (iniContents[9].EndsWith("=")) { iniContents[9] = @"sp5LogPath=C:\GrgBanking\Log"; }
                if (String.IsNullOrEmpty(iniContents[10])) { iniContents[10] = "//where is the SP5 log located?"; }
                if (iniContents[11].EndsWith("=")) { iniContents[11] = "sp5LogName=GrgLog.dxdb"; }
                if (String.IsNullOrEmpty(iniContents[12])) { iniContents[12] = "//name of the SP5 log file"; }
                if (iniContents[13] == "" || !Boolean.TryParse(iniContents[13].Split('=')[1], out outresult)) { iniContents[13] = "CRMLog=true"; }
                if (String.IsNullOrEmpty(iniContents[14])) { iniContents[14] = "//do you want to copy CRM logs?	- true=yes false=no"; }
                if (iniContents[15].EndsWith("=")) { iniContents[15] = @"CRMLogPath=C:\GrgBanking\log\CRMLog"; }
                if (String.IsNullOrEmpty(iniContents[16])) { iniContents[16] = "//where are the CRM logs located?"; }
                if (iniContents[17].EndsWith("=")) { iniContents[17] = "CRMLogName="; }
                if (String.IsNullOrEmpty(iniContents[18])) { iniContents[18] = "//name of the CRM log files"; }
                if (iniContents[19] == "" || !Boolean.TryParse(iniContents[19].Split('=')[1], out outresult)) { iniContents[19] = "driverLog=false"; }
                if (String.IsNullOrEmpty(iniContents[20])) { iniContents[20] = "//do you want to copy driverlogs? - true=yes false=no"; }
                if (iniContents[21].EndsWith("=")) { iniContents[21] = @"driverLogPath=C:\Program Files\GrgBanking\GRGXFSSP\DevDriverLog\"; }
                if (String.IsNullOrEmpty(iniContents[22])) { iniContents[22] = "//where are the driver logs located?"; }
                if (iniContents[23].EndsWith("=")) { iniContents[23] = "driverLogName="; }
                if (String.IsNullOrEmpty(iniContents[24])) { iniContents[24] = "//name of the driver log file"; }
                if (iniContents[25] == "" || !Boolean.TryParse(iniContents[25].Split('=')[1], out outresult)) { iniContents[25] = "duniteLog=false"; }
                if (String.IsNullOrEmpty(iniContents[26])) { iniContents[26] = "//do you want to copy dunite logs? - true=yes false=no"; }
                if (iniContents[27].EndsWith("=")) { iniContents[27] = @"duniteLogPath=C:\GrgBanking\log\hardware\"; }
                if (String.IsNullOrEmpty(iniContents[28])) { iniContents[28] = "//where are the dunite logs located?"; }
                if (iniContents[29].EndsWith("=")) { iniContents[29] = "duniteLogName="; }
                if (String.IsNullOrEmpty(iniContents[30])) { iniContents[30] = "//what is the name of the dunitelog"; }
                if (iniContents[31] == "" || !Boolean.TryParse(iniContents[31].Split('=')[1], out outresult)) { iniContents[31] = "XFSManLog=false"; }
                if (String.IsNullOrEmpty(iniContents[32])) { iniContents[32] = "//do you want to copy XFS manager logs? - true=yes false=no"; }
                if (iniContents[33].EndsWith("=")) { iniContents[33] = @"XFSManLogPath=C:\GrgBanking\log\GrgXfsManager\"; }
                if (String.IsNullOrEmpty(iniContents[34])) { iniContents[34] = "//where are the XFSMan logs located?"; }
                if (iniContents[35].EndsWith("=")) { iniContents[35] = "XFSMANLogName=GrgXfsManager.log"; }
                if (String.IsNullOrEmpty(iniContents[36])) { iniContents[36] = "//name of the XFSMan log"; }
                if (iniContents[37] == "" || !Boolean.TryParse(iniContents[37].Split('=')[1], out outresult)) { iniContents[37] = "YDCLog=false"; }
                if (String.IsNullOrEmpty(iniContents[38])) { iniContents[38] = "//Do you want to copy YDC logs? - true=yes false=no"; }
                if (iniContents[39] == "" || !Boolean.TryParse(iniContents[39].Split('=')[1], out outresult)) { iniContents[39] = "winSystemLog=false"; }
                if (String.IsNullOrEmpty(iniContents[40])) { iniContents[40] = "//Do you wantto export the Windows system log? - true=yes false=no"; }
                if (iniContents[41] == "" || !Boolean.TryParse(iniContents[41].Split('=')[1], out outresult)) { iniContents[41] = "logDateCriteria=false"; }
                if (String.IsNullOrEmpty(iniContents[42])) { iniContents[42] = "//True for date criteria, logDateBeg and logDateEnd will be used"; }

                try
                {
                    string year = iniContents[43].Split('=')[1];
                    string month = iniContents[44].Split('=')[1];
                    string day = iniContents[45].Split('=')[1];



                    DateTime begindate = new DateTime(Convert.ToInt32(year),Convert.ToInt32(month),Convert.ToInt32(day));

                }
                catch (Exception ex)
                {
                    applicationLog.WriteDebugLog(ex.ToString());
                    iniContents[43] = "logDateBegYear=" + DateTime.Today.AddDays(-7).Year.ToString();
                    iniContents[44] = "logDateBegMonth=" + DateTime.Today.AddDays(-7).Month.ToString();
                    iniContents[45] = "logDateBegDay=" + DateTime.Today.AddDays(-7).Day.ToString();
                    copyLogConsoleIniFile.Write("logDateBegYear", DateTime.Today.AddDays(-7).Year.ToString());
                    copyLogConsoleIniFile.Write("logDateBegMonth", DateTime.Today.AddDays(-7).Month.ToString());
                    copyLogConsoleIniFile.Write("logDateBegDay", DateTime.Today.AddDays(-7).Day.ToString());

                    iniContents[47] = "logDateEndYear=" + DateTime.Today.Year.ToString();
                    iniContents[48] = "logDateEndMonth=" + DateTime.Today.Month.ToString();
                    iniContents[49] = "logDateEndDay=" + DateTime.Today.Day.ToString();

                    copyLogConsoleIniFile.Write("logDateEndYear", DateTime.Today.Year.ToString());
                    copyLogConsoleIniFile.Write("logDateEndMonth", DateTime.Today.Month.ToString());
                    copyLogConsoleIniFile.Write("logDateEndDay", DateTime.Today.Day.ToString()); 
                    applicationLog.WriteDebugLog("There was a problem with the dates. Corrected to last 7 days.");
                }


                //if (iniContents[43].EndsWith("=")  || iniContents[43].Split('=')[1].Substring(0, 1) != "2" || iniContents[43].Split('=')[1].Length != 4)
                //{ iniContents[43] = "logDateBegYear=" + DateTime.Today.AddDays(-7).Year.ToString(); }

                //if (iniContents[44].EndsWith("=") || iniContents[44].Split('=')[1].Length > 2)
                //{ iniContents[44] = "logDateBegMonth=" + DateTime.Today.AddDays(-7).Month.ToString(); }

                //if (iniContents[45].EndsWith("=") || iniContents[45].Split('=')[1].Length > 2 )
                //{ iniContents[45] = "logDateBegDay=" + DateTime.Today.AddDays(-7).Day.ToString(); }


                if (String.IsNullOrEmpty(iniContents[46])) { iniContents[46] = "//SP4 logs'will be copied based on the date criteria, this parameter will be the first day"; }


                //if (iniContents[47].EndsWith("=") || iniContents[47].Split('=')[1].Substring(0, 1) != "2" || iniContents[47].Split('=')[1].Length != 4)
                //{ iniContents[47] = "logDateEndYear=" + DateTime.Today.Year.ToString(); }

                //if (iniContents[48].EndsWith("=") ||  iniContents[48].Split('=')[1].Length != 2)
                //{ iniContents[48] = "logDateEndMonth=" + DateTime.Today.Month.ToString(); }

                //if (iniContents[49].EndsWith("=") || Convert.ToInt32(iniContents[49].Split('=')[1]) > 31)
                //{ iniContents[49] = "logDateEndDay=" + DateTime.Today.Day.ToString(); }


                if (String.IsNullOrEmpty(iniContents[50])) { iniContents[50] = "//SP4 logs'will be copied based on the date criteria, this parameter will be the last day"; }

                if (iniContents[51] == "" || !Boolean.TryParse(iniContents[51].Split('=')[1], out outresult)) { iniContents[51] = "logIntervalCriteria=true"; }
                if (String.IsNullOrEmpty(iniContents[52])) { iniContents[52] = "//True for interval criteria, the logs will be copied based on the days criteria"; }
                if (iniContents[53].EndsWith("=")) { iniContents[53] = "logDays=7"; }
                if (String.IsNullOrEmpty(iniContents[54])) { iniContents[54] = "//for ex. 10 means the the latest 10 days' SP4 logs will be copied. "; }
                if (iniContents[55].EndsWith("=")) { iniContents[55] = @"outputDirectory=c:\GRGLogs"; }
                if (String.IsNullOrEmpty(iniContents[56])) { iniContents[56] = "//where all the logs will be copied"; }
                if (iniContents[57].EndsWith("=")) { iniContents[57] = "CRMLogDays=1"; }
                if (String.IsNullOrEmpty(iniContents[58])) { iniContents[58] = "//for SP package registry values, how many days for the CRM log will be generated"; }
                if (iniContents[59].EndsWith("=")) { iniContents[59] = "atmType=" + atmType; }
                if (String.IsNullOrEmpty(iniContents[60])) { iniContents[60] = "//What is the ATM type (H68 or H34)"; }

                #region comment
                //if (copyLogConsoleIniFile.Read("sp4Log") == "" || copyLogConsoleIniFile.Read("sp4Log") != "true" || copyLogConsoleIniFile.Read("sp4Log") != "false")
                //{
                //    copyLogConsoleIniFile.Write("sp4Log", "true");

                //}
                //if (copyLogConsoleIniFile.Read("sp4LogPath") == "")
                //    copyLogConsoleIniFile.Write("sp4LogPath", @"C:\GrgBanking\Log");
                //if (copyLogConsoleIniFile.Read("sp5Log") == "" || copyLogConsoleIniFile.Read("sp5Log") != "true" || copyLogConsoleIniFile.Read("sp5Log") != "false")
                //    copyLogConsoleIniFile.Write("sp5Log", "sp5Log");
                //if (copyLogConsoleIniFile.Read("sp5LogName") == "")
                //    copyLogConsoleIniFile.Write("sp5LogName", "GrgLog.dxdb");
                //if (copyLogConsoleIniFile.Read("sp5LogPath") == "")
                //    copyLogConsoleIniFile.Write("sp5LogPath", @"C:\GrgBanking\Log");
                //if (copyLogConsoleIniFile.Read("CRMLog") == "" || copyLogConsoleIniFile.Read("CRMLog") != "true" || copyLogConsoleIniFile.Read("CRMLog") != "false")
                //    copyLogConsoleIniFile.Write("CRMLog", "true");
                //if (copyLogConsoleIniFile.Read("CRMLogPath") == "")
                //    copyLogConsoleIniFile.Write("CRMLogPath", CRMLogPath);
                //if (copyLogConsoleIniFile.Read("CRMLogName") == "")
                //    copyLogConsoleIniFile.Write("CRMLogName", "");
                //if (copyLogConsoleIniFile.Read("driverLog") == "" || copyLogConsoleIniFile.Read("driverLog") != "true" || copyLogConsoleIniFile.Read("driverLog") != "false")
                //    copyLogConsoleIniFile.Write("driverLog", "false");
                //if (copyLogConsoleIniFile.Read("driverLogPath") == "")
                //    copyLogConsoleIniFile.Write("driverLogPath", @"C:\Program Files\GrgBanking\GRGXFSSP\DevDriverLog\");
                //if (copyLogConsoleIniFile.Read("driverLogName") == "")
                //    copyLogConsoleIniFile.Write("driverLogName", "");
                //if (copyLogConsoleIniFile.Read("duniteLog") == "" || copyLogConsoleIniFile.Read("duniteLog") != "true" || copyLogConsoleIniFile.Read("duniteLog") != "false")
                //    copyLogConsoleIniFile.Write("duniteLog", "false");
                //if (copyLogConsoleIniFile.Read("duniteLogPath") == "")
                //    copyLogConsoleIniFile.Write("duniteLogPath", @"C:\GrgBanking\log\hardware\");
                //if (copyLogConsoleIniFile.Read("duniteLogName") == "")
                //    copyLogConsoleIniFile.Write("duniteLogName", "");
                //if (copyLogConsoleIniFile.Read("XFSManLog") == "" || copyLogConsoleIniFile.Read("XFSManLog") != "true" || copyLogConsoleIniFile.Read("XFSManLog") != "false")
                //    copyLogConsoleIniFile.Write("XFSManLog", "false");
                //if (copyLogConsoleIniFile.Read("XFSManLogPath") == "")
                //    copyLogConsoleIniFile.Write("XFSManLogPath", @"C:\GrgBanking\log\GrgXfsManager\");
                //if (copyLogConsoleIniFile.Read("XFSMANLogName") == "")
                //    copyLogConsoleIniFile.Write("XFSMANLogName", "GrgXfsManager.log");
                //if (copyLogConsoleIniFile.Read("YDCLog") == "" || copyLogConsoleIniFile.Read("YDCLog") != "true" || copyLogConsoleIniFile.Read("YDCLog") != "false")
                //    copyLogConsoleIniFile.Write("YDCLog", "false");
                //if (copyLogConsoleIniFile.Read("winSystemLog") == "" || copyLogConsoleIniFile.Read("winSystemLog") != "true" || copyLogConsoleIniFile.Read("winSystemLog") != "false")
                //    copyLogConsoleIniFile.Write("winSystemLog", "false");
                //if (copyLogConsoleIniFile.Read("logDateCriteria") == "" || copyLogConsoleIniFile.Read("logDateCriteria") != "true" || copyLogConsoleIniFile.Read("logDateCriteria") != "false")
                //    copyLogConsoleIniFile.Write("logDateCriteria", "false");
                //if (copyLogConsoleIniFile.Read("logDateBegYear") == "")
                //    copyLogConsoleIniFile.Write("logDateBegYear", DateTime.Today.Year.ToString());
                //if (copyLogConsoleIniFile.Read("logDateBegMonth") == "")
                //    copyLogConsoleIniFile.Write("logDateBegMonth", DateTime.Today.AddDays(-7).Month.ToString());
                //if (copyLogConsoleIniFile.Read("logDateBegDay") == "")
                //    copyLogConsoleIniFile.Write("logDateBegDay", DateTime.Today.AddDays(-7).Day.ToString());
                //if (copyLogConsoleIniFile.Read("logDateEndYear") == "")
                //    copyLogConsoleIniFile.Write("logDateEndYear", DateTime.Today.Year.ToString());
                //if (copyLogConsoleIniFile.Read("logDateEndMonth") == "")
                //    copyLogConsoleIniFile.Write("logDateEndMonth", DateTime.Today.Month.ToString());
                //if (copyLogConsoleIniFile.Read("logDateEndDay") == "")
                //    copyLogConsoleIniFile.Write("logDateEndDay", DateTime.Today.Day.ToString());
                //if (copyLogConsoleIniFile.Read("logIntervalCriteria") == "" || copyLogConsoleIniFile.Read("logIntervalCriteria") != "true" || copyLogConsoleIniFile.Read("logIntervalCriteria") != "false")
                //    copyLogConsoleIniFile.Write("logIntervalCriteria", "true");
                //if (copyLogConsoleIniFile.Read("logDays") == "")
                //    copyLogConsoleIniFile.Write("logDays", "7");
                //if (copyLogConsoleIniFile.Read("outputDirectory") == "")
                //    copyLogConsoleIniFile.Write("outputDirectory", @"C:\GrgLogs");
                //if (copyLogConsoleIniFile.Read("CRMLogDays") == "")
                //    copyLogConsoleIniFile.Write("CRMLogDays", "1");
                //if (copyLogConsoleIniFile.Read("CRM/CDM") == "")
                //    copyLogConsoleIniFile.Write("CRM/CDM", "");
                #endregion



                if (!Directory.Exists(iniContents[3]))
                    iniContents[3] = @"sp4LogPath=C:\GrgBanking\Log";


                if (!Directory.Exists(iniContents[9]))
                    iniContents[9] = @"sp5LogPath=C:\GrgBanking\Log";


                if (!Directory.Exists(iniContents[15]))
                    iniContents[15] = @"CRMLogPath=C:\GrgBanking\log\CRMLog";


                if (!Directory.Exists(iniContents[21]))
                    iniContents[21] = @"driverLogPath=C:\Program Files\GrgBanking\GRGXFSSP\DevDriverLog\";


                if (!Directory.Exists(iniContents[27]))
                    iniContents[27] = @"duniteLogPath=C:\GrgBanking\log\hardware\";


                if (!Directory.Exists(iniContents[33]))
                    iniContents[33] = @"XFSManLogPath=C:\GrgBanking\log\GrgXfsManager\";

                string regexFolder = @"^.\:\\$";

                if(System.Text.RegularExpressions.Regex.IsMatch(copyLogConsoleIniFile.Read("outputDirectory"), regexFolder))
                    copyLogConsoleIniFile.Write("outputDirectory", @"C:\GrgLogs");

                try
                {
                    Directory.CreateDirectory(copyLogConsoleIniFile.Read("outputDirectory"));
                }
                catch (Exception ex)
                {
                    copyLogsConsole.applicationLog.WriteDebugLog(ex.ToString());
                    copyLogConsoleIniFile.Write("outputDirectory", @"C:\GrgLogs");
                }

                File.WriteAllText(copyLogConsoleIniFile.Path, String.Empty);

                for (int i = 0; i < iniContents.Count(); i++)
                    File.AppendAllText(copyLogConsoleIniFile.Path, iniContents[i] + "\r\n");

            }
            catch (Exception ex)
            {

                applicationLog.WriteDebugLog(ex.ToString());
            }
        }
    }






}
