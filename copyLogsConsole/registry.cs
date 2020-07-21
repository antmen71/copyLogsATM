using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace copyLogsConsole
{
    class registry
    {

        public static string readDevLogReturn;
        public static string readDevLogDays;
        public string readDevLogPath;
        public string type;

        RegistryKey machineType = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\GrgBanking\GrgSpSetting\ATMMachine", true);
        RegistryKey machineType64 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\GrgBanking\GrgSpSetting\ATMMachine", true);

        public registry checkMachineType()
        {
            registry atmMachineType = new registry();
            
            iniFile myIni = new iniFile();
            if (Environment.Is64BitOperatingSystem)
            { machineType = machineType64; }

            if (machineType.GetValue("MachineType").ToString() == "" || machineType.GetValue("MachineType").ToString() == null)
            {
                myIni.Write("CRMLog", "false");

            }

            if (machineType.GetValue("MachineType").ToString().Substring(0, 3) == "H34")
            {
                applicationLog.WriteDebugLog("ATM Machine type is " + machineType.GetValue("MachineType").ToString());
                atmMachineType.type = "H34N";
            }
            else if (machineType.GetValue("MachineType").ToString().Substring(0, 3) == "H68")
            {

                string val = machineType.GetValue("MachineType").ToString().Substring(3, 1);

                if (machineType.GetValue("MachineType").ToString().Substring(3, 1) == "V")
                {
                    applicationLog.WriteDebugLog("ATM Machine type is " + machineType.GetValue("MachineType").ToString());
                    atmMachineType.type = "H68V";
                }
                else if (machineType.GetValue("MachineType").ToString().Substring(3, 1) == "N")
                { applicationLog.WriteDebugLog("ATM Machine type is " + machineType.GetValue("MachineType").ToString());
                    atmMachineType.type = "H68N"; }
                
            }

            else if (machineType.GetValue("MachineType").ToString().Substring(0, 3) != "H34" || machineType.GetValue("MachineType").ToString().Substring(0, 3) != "H68")
            {
                atmMachineType.type = "";
                myIni.Write("CRMLog", "false");
                applicationLog.WriteDebugLog("ATM Machine type is " + machineType.ToString());

            }
            return atmMachineType;
        }

        public RegistryKey rKey(string registryKey)
        {
            RegistryKey rKey = null;
            string regKey ;
                if(Environment.Is64BitOperatingSystem)
                { regKey = rKey.OpenSubKey(@"SOFTWARE\WOW6432Node\XFS\PHYSICAL_SERVICES\GrgCRM_CRM9250_XFS30").ToString(); }
                else
                { regKey = rKey.OpenSubKey(@"SOFTWARE\XFS\PHYSICAL_SERVICES\GrgCRM_CRM9250_XFS30").ToString(); }
                
                
            return rKey;
        }

        public void checkRegistryKeys(string registryPath, string machineType, iniFile configIniFile)
        {
            RegistryKey regKeyPath = Registry.LocalMachine.OpenSubKey(registryPath, true);

            string[] valueNames = regKeyPath.GetValueNames();


            if (!valueNames.Contains("ReadDevLogDays"))
            {
                try
                {
                    regKeyPath.SetValue("ReadDevLogDays", configIniFile.Read("CRMLogDays"));
                    applicationLog.WriteDebugLog("ReadDevLogDays Registry Value Not Found, created for " + machineType);
                }
                catch (Exception ex)
                {
                    applicationLog.WriteDebugLog(ex.ToString());
                }

            }
            else if (valueNames.Contains("ReadDevLogDays"))
            {
                readDevLogDays = regKeyPath.GetValue("ReadDevLogDays").ToString();
                applicationLog.WriteDebugLog("readDevLogDays registry key found. And the value is " + regKeyPath.GetValue("ReadDevLogDays"));
            }



            if (!valueNames.Contains("ReadDevLogPath"))
            {
                try
                {
                    regKeyPath.SetValue("ReadDevLogPath", configIniFile.Read("CRMLogPath"));
                    applicationLog.WriteDebugLog("ReadDevLogPath Registry Value Not Found, created for " + machineType);
                }
                catch (Exception ex)
                {
                    applicationLog.WriteDebugLog(ex.ToString());
                }
            }

            else if (valueNames.Contains("ReadDevLogPath"))
            {
                readDevLogPath = regKeyPath.GetValue("ReadDevLogPath").ToString();
                applicationLog.WriteDebugLog("readDevLogPath registry key found. And the value is " + regKeyPath.GetValue("ReadDevLogPath"));
            }




            if (!valueNames.Contains("ReadDevLogReturn"))
            {
                try
                {
                    regKeyPath.SetValue("ReadDevLogReturn", "0");
                    applicationLog.WriteDebugLog("ReadDevLogReturn Registry Value Not Found, created for " + machineType);
                }
                catch (Exception ex)
                {
                    applicationLog.WriteDebugLog(ex.ToString());
                }

            }
            else if (valueNames.Contains("ReadDevLogReturn"))
            {
                readDevLogReturn = regKeyPath.GetValue("ReadDevLogReturn").ToString();

                applicationLog.WriteDebugLog("readDevLogDays registry key found. And the value is " + regKeyPath.GetValue("ReadDevLogReturn"));


            }
            //if (machineType == "H34")
            //{


            //    readValues = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\XFS\PHYSICAL_SERVICES\GrgCDM_CDM8240_XFS30");
            //    writeValues = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\XFS\PHYSICAL_SERVICES\GrgCDM_CDM8240_XFS30", true);

            //    //checkRegistryKeys(@"SOFTWARE\XFS\PHYSICAL_SERVICES\GrgCDM_CDM8240_XFS30", "H34", myIni);


            //    readDevLogReturn = readValues.GetValue("ReadDevLogReturn").ToString();
            //    readDevLogDays = readValues.GetValue("ReadDevLogDays").ToString();
            //    readDevLogPath = readValues.GetValue("ReadDevLogPath").ToString();


            //}
            //else if (machineType == "H68")
            //{
            //    readValues = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\XFS\PHYSICAL_SERVICES\GrgCRM_CRM9250_XFS30");
            //    writeValues = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\XFS\PHYSICAL_SERVICES\GrgCRM_CRM9250_XFS30", true);

            //    //checkRegistryKeys(@"SOFTWARE\XFS\PHYSICAL_SERVICES\GrgCRM_CRM9250_XFS30", "H68", myIni);

            //    readDevLogReturn = readValues.GetValue("ReadDevLogReturn").ToString();
            //    readDevLogDays = readValues.GetValue("ReadDevLogDays").ToString();
            //    readDevLogPath = readValues.GetValue("ReadDevLogPath").ToString();



            //}
        }
    }
}