using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace copyLogsConsole
{
    class atm
    {

        public string atmType;
        public string cashDevType;




        public string checkAtmType()
        {
            registry regKeyy = new registry();
            string atmType = "";
            try
            {
                //RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\GrgBanking\GrgSpSetting\ATMMachine", true);
                atmType = regKeyy.checkMachineType().type;
                    
                    //.GetValue("MachineType").ToString().Substring(0, 3);
            }
            catch (Exception ex)
            {

                copyLogsConsole.applicationLog.WriteDebugLog(ex.ToString());
            }

            return atmType;

        }

    }
}
