using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace copyLogsConsole
{
    class date
    {
        
        public DateTime checkBegDate(string year,string month,string day, iniFile myIni)
        {
            int result;
            DateTime begDateCorrected = new DateTime();

            if (year == "" || year == null || year.Substring(0, 1) != "2" || year.Length != 4 || Convert.ToInt32(year) > DateTime.Today.Year)
            {
                if(String.IsNullOrEmpty(myIni.Read("logDays")) || !Int32.TryParse(myIni.Read("logDays"),out result))
                {
                year = DateTime.Today.AddDays(-7).Year.ToString();
                }
               
                }
            if (month == "" || month == null || month.Length > 2 || Convert.ToInt32(month) > 12)
            {
                month = DateTime.Today.AddDays(-7).Month.ToString();
            }
            if (day == "" || day == null || day.Length > 2 || Convert.ToInt32(day) > 31)
            { 
             day = DateTime.Today.AddDays(-7).Day.ToString();
            }
               

            begDateCorrected = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
            return begDateCorrected;
        }

        public DateTime checkEndDate(string year, string month, string day, iniFile myIni)
        {
            DateTime begEndCorrected = new DateTime();

            if (year == "" || year == null || year.Substring(0, 1) != "2" || year.Length != 4 || Convert.ToInt32(year) > DateTime.Today.Year)
            {
                year = DateTime.Today.Year.ToString();
                //myIni.Write("logDateEndYear", DateTime.Today.Year.ToString()); 
            }
            if (month == "" || month == null || month.Length > 2 || Convert.ToInt32(month) > 12)
            {
                month = DateTime.Today.Month.ToString();
                //myIni.Write("logDateEndMonth", DateTime.Today.Month.ToString());
            }
            if (day == "" || day == null || day.Length > 2 || Convert.ToInt32(day) > 31)
            {
                day = DateTime.Today.Day.ToString();
                //myIni.Write("logDateEndDay", DateTime.Today.Day.ToString());
            }
            begEndCorrected = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
            return begEndCorrected;
        }

        public List<DateTime> getlogDates(DateTime begDate, DateTime endDate)
        {
            List<DateTime> logDates = new List<DateTime>();
            try
            {
                if (begDate == null)
                {
                    begDate = DateTime.Now.AddDays(-7);
                    applicationLog.WriteDebugLog("begDate was null. Is now 7 days before.");
                }
                else if (endDate == null)
                {
                    endDate = DateTime.Now;
                    applicationLog.WriteDebugLog("endDate was null. Is now today.");
                }

                if (begDate > endDate)
                {
                    begDate = endDate.AddDays(-7);
                    applicationLog.WriteDebugLog("endDate was later than begDate. Is now 7 days before.");
                }

                applicationLog.WriteDebugLog("Beginning date is: " + begDate.ToShortDateString());
                applicationLog.WriteDebugLog("Ending date is: " + endDate.ToShortDateString());



                for (DateTime i = begDate; i <= endDate; i = i.AddDays(1))
                {
                    logDates.Add(i);
                }
            }
            catch (Exception ex)
            {

                applicationLog.WriteDebugLog(ex.ToString());
            }


            return logDates;
        }

        public List<DateTime> getlogDates(int interval)
        {
            List<DateTime> logDates = new List<DateTime>();
            try
            {
                if (interval < 0 ||  interval.ToString() ==null || interval.ToString() =="")
                { interval = 7; }

                DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                DateTime begDate = DateTime.Today.AddDays(-7);
                try
                {
                    begDate = today.AddDays(-1 * interval);

                }
                catch (Exception ex)
                {

                    applicationLog.WriteDebugLog(ex.ToString());
                }




                for (DateTime i = begDate; i <= today; i = i.AddDays(1))
                {
                    logDates.Add(i);
                }

                applicationLog.WriteDebugLog("Beginning date is: " + begDate.ToShortDateString());
                applicationLog.WriteDebugLog("Ending date is: " + DateTime.Today.ToShortDateString());
            }
            catch (Exception ex)
            {

                applicationLog.WriteDebugLog(ex.ToString());
            }


            return logDates;
        }

    }
}
