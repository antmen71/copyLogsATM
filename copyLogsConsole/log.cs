using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace copyLogsConsole
{
    class log
    {

        public log()
        {
        }

        public List<FileInfo> getLogFiles(bool wBc, string logSourcePath)
        {
            if (Directory.Exists(logSourcePath))
            {
                if (wBc)
                    try
                    {
                        System.IO.DirectoryInfo dir = new DirectoryInfo(logSourcePath);

                        List<FileInfo> logFilesList = dir.GetFiles("*", SearchOption.AllDirectories).ToList();

                        return logFilesList;

                       
                    }
                    catch (Exception ex)
                    {

                        applicationLog.WriteDebugLog(ex.ToString());
                    }

                else if (!wBc)
                {
                    List<FileInfo> logFileListEmpty = null;
                    return logFileListEmpty;
                }
            }
            else if (!Directory.Exists(logSourcePath))
            { applicationLog.WriteDebugLog("Source log folder not found."); }
            List<FileInfo> logFilesListError = new List<FileInfo>();
            return logFilesListError;




        }

        public List<FileInfo> getLogFiles(bool wBc, string logSourcePath, string fileName)
        {

            if (Directory.Exists(logSourcePath))
            {
                if (wBc)
                    try
                    {
                        System.IO.DirectoryInfo dir = new DirectoryInfo(logSourcePath);

                        List<FileInfo> logFilesList = dir.GetFiles(fileName, SearchOption.AllDirectories).ToList();

                        return logFilesList;
                    }
                    catch (Exception ex)
                    {

                        applicationLog.WriteDebugLog(ex.ToString());
                    }

                else if (!wBc)
                {
                    List<FileInfo> logFileListEmpty = null;
                    return logFileListEmpty;
                }
            }
            else if (!Directory.Exists(logSourcePath))
            { applicationLog.WriteDebugLog("Source log folder not found."); }
            List<FileInfo> logFilesListError = new List<FileInfo>();
            return logFilesListError;




        }

        public List<FileInfo> selectLogs(List<DateTime> logDates, List<FileInfo> LogFiles, string sPattern)
        {

            List<FileInfo> fileList = new List<FileInfo>();
            try
            {
                if (LogFiles != null)
                {
                    foreach (FileInfo flInf in LogFiles)
                    {
                        //string sPattern = "";

                        if (System.Text.RegularExpressions.Regex.IsMatch(Path.GetFileNameWithoutExtension(flInf.FullName), sPattern))
                        {
                            DateTime begdate = new DateTime(Convert.ToInt16(flInf.Name.Substring(0, 4)), Convert.ToInt16(flInf.Name.Substring(4, 2)), Convert.ToInt16(flInf.Name.Substring(6, 2)));
                            DateTime enddate = new DateTime(Convert.ToInt16(flInf.Name.Substring(16, 4)), Convert.ToInt16(flInf.Name.Substring(20, 2)), Convert.ToInt16(flInf.Name.Substring(22, 2)));

                            string sBegDate = begdate.ToShortDateString();
                            string sEndDate = enddate.ToShortDateString();

                            DateTime fileCreation = Convert.ToDateTime(sBegDate);
                            DateTime fileLastMod = Convert.ToDateTime(sEndDate);

                            foreach (DateTime logDate in logDates)
                            {

                                string sLogDate = logDate.ToShortDateString();


                                if (sLogDate == sBegDate || sLogDate == sEndDate)
                                {
                                    while (!fileList.Contains(flInf))
                                        fileList.Add(flInf);


                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                applicationLog.WriteDebugLog(ex.ToString());
            }


            return fileList;


        }

        public List<FileInfo> selectCrmLogs(List<DateTime> logDates, List<FileInfo> LogFiles, string sPattern, string machineType)
        {

            List<FileInfo> fileList = new List<FileInfo>();
            try
            {
                foreach (FileInfo flInf in LogFiles)
                {

                    //string sPattern = "";
                    if (System.Text.RegularExpressions.Regex.IsMatch(Path.GetFileNameWithoutExtension(flInf.FullName), sPattern))
                    {

                        DateTime begDate = new DateTime ();
                        if (machineType == "H68N")
                        {
                            begDate = new DateTime(Convert.ToInt16(flInf.Name.Substring(14, 4)), Convert.ToInt16(flInf.Name.Substring(18, 2)), Convert.ToInt16(flInf.Name.Substring(20, 2)));
                        } 
                            //DateTime enddate = new DateTime(Convert.ToInt16(flInf.Name.Substring(16, 4)), Convert.ToInt16(flInf.Name.Substring(20, 2)), Convert.ToInt16(flInf.Name.Substring(22, 2)));
                        else if (machineType == "H34")
                        {
                           

                            begDate = new DateTime(Convert.ToInt16(flInf.Name.Substring(17, 4)), Convert.ToInt16(flInf.Name.Substring(21, 2)), Convert.ToInt16(flInf.Name.Substring(23, 2)));
                        }
                        else if(machineType=="H68V")
                        {
                        
                        begDate = new DateTime(Convert.ToInt16(flInf.Name.Substring(12,4)), Convert.ToInt16(flInf.Name.Substring(16,2)), Convert.ToInt16(flInf.Name.Substring(18,2)));
                        
                        }
                        string sBegDate = begDate.ToShortDateString();
                        //string sEndDate = enddate.ToShortDateString();
                        
                        foreach (DateTime logDate in logDates)
                        {

                            string sLogDate = logDate.ToShortDateString();


                            if (sLogDate == sBegDate)
                            {
                                while (!fileList.Contains(flInf))
                                    fileList.Add(flInf);


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                applicationLog.WriteDebugLog(ex.ToString());
            }

            return fileList;
        }

        public List<FileInfo> selectDriverLogs(List<DateTime> logDates, List<FileInfo> LogFiles, string sPattern)
        {
            List<FileInfo> fileList = new List<FileInfo>();

            try
            {
                foreach (FileInfo flInf in LogFiles)
                {
                    //{CARDREADER(2018-02-13).log}

                    if (System.Text.RegularExpressions.Regex.IsMatch(flInf.Name, sPattern))
                    {
                        try
                        {
                            int firstParanthesesOccurence = Convert.ToInt16(flInf.Name.IndexOf('('));
                            int year = Convert.ToInt16(flInf.Name.Substring(firstParanthesesOccurence + 1, 4));
                            int month = Convert.ToInt16(flInf.Name.Substring(firstParanthesesOccurence + 6, 2));
                            int day = Convert.ToInt16(flInf.Name.Substring(firstParanthesesOccurence + 9, 2));


                            DateTime begdate = new DateTime(year, month, day);

                            string sBegDate = begdate.ToShortDateString();

                            foreach (DateTime logDate in logDates)
                            {

                                string sLogDate = logDate.ToShortDateString();


                                if (sLogDate == sBegDate)
                                {
                                    while (!fileList.Contains(flInf))
                                        fileList.Add(flInf);


                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            copyLogsConsole.applicationLog.WriteDebugLog(ex.ToString());
                        }

                    }


                }
            }
            catch (Exception ex)
            {

                applicationLog.WriteDebugLog(ex.ToString());
            }

            return fileList;
        }

        public List<FileInfo> selectDuniteLogs(List<DateTime> logDates, List<FileInfo> LogFiles, string sPattern)
        {
            List<FileInfo> fileList = new List<FileInfo>();
            try
            {
                foreach (FileInfo flInf in LogFiles)
                {


                    if (System.Text.RegularExpressions.Regex.IsMatch(flInf.Name, sPattern))
                    {

                        DateTime begdate = new DateTime(Convert.ToInt16(flInf.Name.Substring(flInf.Name.IndexOf('(') + 1, 4)), Convert.ToInt16(flInf.Name.Substring(flInf.Name.IndexOf('(') + 6, 2)), Convert.ToInt16(flInf.Name.Substring(flInf.Name.IndexOf('(') + 9, 2)));

                        string sBegDate = begdate.ToShortDateString();

                        foreach (DateTime logDate in logDates)
                        {

                            string sLogDate = logDate.ToShortDateString();


                            if (sLogDate == sBegDate)
                            {
                                while (!fileList.Contains(flInf))
                                    fileList.Add(flInf);


                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                applicationLog.WriteDebugLog(ex.ToString());
            }

            return fileList;
        }

        public List<FileInfo> selectxfsManLogs(List<DateTime> logDates, List<FileInfo> LogFiles, string sPattern)
        {
            List<FileInfo> fileList = new List<FileInfo>();
            try
            {
                foreach (FileInfo flInf in LogFiles)
                {


                    if (System.Text.RegularExpressions.Regex.IsMatch(flInf.Name, sPattern))
                    {

                        string year = flInf.Name.Substring(0, 4);
                        string month = flInf.Name.Substring(4, 2);
                        string DayOfWeek = flInf.Name.Substring(6, 2);


                        DateTime begdate = new DateTime(Convert.ToInt32(flInf.Name.Substring(0, 4)), Convert.ToInt32(flInf.Name.Substring(4, 2)), Convert.ToInt32(flInf.Name.Substring(6, 2)));

                        string sBegDate = begdate.ToShortDateString();

                        foreach (DateTime logDate in logDates)
                        {

                            string sLogDate = logDate.ToShortDateString();


                            if (sLogDate == sBegDate)
                            {
                                while (!fileList.Contains(flInf))
                                    fileList.Add(flInf);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                applicationLog.WriteDebugLog(ex.ToString());
            }

            return fileList;
        }

        public List<FileInfo> selectYdcLogs(List<DateTime> logDates, List<FileInfo> LogFiles)
        {
            List<FileInfo> fileList = new List<FileInfo>();

            try
            {
                foreach (FileInfo flInf in LogFiles)
                {

                    if (System.Text.RegularExpressions.Regex.IsMatch(flInf.Name, @"\d{4}\.\d{2}\.\d{2}.*\.dmp"))
                    {

                        DateTime begDate = new DateTime(Convert.ToInt32(flInf.Name.Substring(0, 4)), Convert.ToInt32(flInf.Name.Substring(5, 2)), Convert.ToInt32(flInf.Name.Substring(8, 2)));
                        string sBegDate = begDate.ToShortDateString();

                        foreach (DateTime logDate in logDates)
                        {

                            string sLogDate = logDate.ToShortDateString();


                            if (sLogDate == sBegDate)
                            {
                                while (!fileList.Contains(flInf))
                                    fileList.Add(flInf);

                            }
                        }

                    }
                    else if (System.Text.RegularExpressions.Regex.IsMatch(flInf.Name, @"(COM)\d{6}\.log"))
                    {
                        DateTime begDate = new DateTime(Convert.ToInt32("20" + flInf.Name.Substring(3, 2)), Convert.ToInt32(flInf.Name.Substring(5, 2)), Convert.ToInt32(flInf.Name.Substring(7, 2)));
                        string sBegDate = begDate.ToShortDateString();

                        foreach (DateTime logDate in logDates)
                        {

                            string sLogDate = logDate.ToShortDateString();


                            if (sLogDate == sBegDate)
                            {
                                while (!fileList.Contains(flInf))
                                    fileList.Add(flInf);

                            }
                        }

                    }

                    else if (System.Text.RegularExpressions.Regex.IsMatch(flInf.Name, @"(EJ)\d{6}\.log"))
                    {
                        DateTime begDate = new DateTime(Convert.ToInt32("20" + flInf.Name.Substring(2, 2)), Convert.ToInt32(flInf.Name.Substring(4, 2)), Convert.ToInt32(flInf.Name.Substring(6, 2)));
                        string sBegDate = begDate.ToShortDateString();

                        foreach (DateTime logDate in logDates)
                        {

                            string sLogDate = logDate.ToShortDateString();


                            if (sLogDate == sBegDate)
                            {
                                while (!fileList.Contains(flInf))
                                    fileList.Add(flInf);

                            }
                        }

                    }
                    else if (System.Text.RegularExpressions.Regex.IsMatch(flInf.Name, @"(EMVKernel)\d{4}-\d{2}-\d{2}\.log"))
                    {
                        string year = flInf.Name.Substring(9, 4);
                        string month = flInf.Name.Substring(14, 2);
                        string DayOfWeek = flInf.Name.Substring(17, 2);

                        DateTime begDate = new DateTime(Convert.ToInt32(flInf.Name.Substring(9, 4)), Convert.ToInt32(flInf.Name.Substring(14, 2)), Convert.ToInt32(flInf.Name.Substring(17, 2)));
                        string sBegDate = begDate.ToShortDateString();

                        foreach (DateTime logDate in logDates)
                        {

                            string sLogDate = logDate.ToShortDateString();


                            if (sLogDate == sBegDate)
                            {
                                while (!fileList.Contains(flInf))
                                    fileList.Add(flInf);

                            }
                        }

                    }

                    else if (System.Text.RegularExpressions.Regex.IsMatch(flInf.Name, @"(Icc)\d{6}\.log"))
                    {
                        DateTime begDate = new DateTime(Convert.ToInt32("20" + flInf.Name.Substring(3, 2)), Convert.ToInt32(flInf.Name.Substring(5, 2)), Convert.ToInt32(flInf.Name.Substring(7, 2)));
                        string sBegDate = begDate.ToShortDateString();

                        foreach (DateTime logDate in logDates)
                        {

                            string sLogDate = logDate.ToShortDateString();


                            if (sLogDate == sBegDate)
                            {
                                while (!fileList.Contains(flInf))
                                    fileList.Add(flInf);

                            }
                        }

                    }

                    else if (System.Text.RegularExpressions.Regex.IsMatch(flInf.Name, @"(TRACE)\d{6}\.log"))
                    {
                        DateTime begDate = new DateTime(Convert.ToInt32("20" + flInf.Name.Substring(5, 2)), Convert.ToInt32(flInf.Name.Substring(7, 2)), Convert.ToInt32(flInf.Name.Substring(9, 2)));
                        string sBegDate = begDate.ToShortDateString();

                        foreach (DateTime logDate in logDates)
                        {

                            string sLogDate = logDate.ToShortDateString();


                            if (sLogDate == sBegDate)
                            {
                                while (!fileList.Contains(flInf))
                                    fileList.Add(flInf);

                            }
                        }

                    }

                    else if (System.Text.RegularExpressions.Regex.IsMatch(flInf.Name, @"\w*_\d{4}-\d{2}-\d{2}\.txt") && !flInf.Name.Contains("BRIEF"))
                    {
                        string year = flInf.Name.Substring(flInf.Name.IndexOf("_") + 1, 4);
                        string month = flInf.Name.Substring(flInf.Name.IndexOf("_") + 6, 2);
                        string day = flInf.Name.Substring(flInf.Name.IndexOf("_") + 9, 2);



                        DateTime begDate = new DateTime(Convert.ToInt32(flInf.Name.Substring(flInf.Name.IndexOf("_") + 1, 4)), Convert.ToInt32(flInf.Name.Substring(flInf.Name.IndexOf("_") + 6, 2)), Convert.ToInt32(flInf.Name.Substring(flInf.Name.IndexOf("_") + 9, 2)));
                        string sBegDate = begDate.ToShortDateString();

                        foreach (DateTime logDate in logDates)
                        {

                            string sLogDate = logDate.ToShortDateString();


                            if (sLogDate == sBegDate)
                            {
                                while (!fileList.Contains(flInf))
                                    fileList.Add(flInf);

                            }
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                applicationLog.WriteDebugLog(ex.ToString());
            }


            return fileList;

        }       

        public void copyLogs(List<FileInfo> logsToBeCopied, string outputPath, string logType)
        {

            if (logsToBeCopied.Count() == 0 || logsToBeCopied == null)
                applicationLog.WriteDebugLog(logType + " list is empty. No logs will be copied.");
            else if (logsToBeCopied.Count > 0)
            {
                try
                {
                    if (!Directory.Exists(outputPath))
                    {
                        Directory.CreateDirectory(outputPath);
                    }
                    foreach (FileInfo flInf in logsToBeCopied)
                    {
                        System.IO.File.Copy(flInf.FullName, outputPath + "\\" + flInf.Name, true);
                        applicationLog.WriteDebugLog(flInf.Name + " is copied from " + flInf.FullName + " to " + outputPath);
                    }
                }
                catch (Exception ex)
                {
                    applicationLog.WriteDebugLog(ex.ToString());
                }

            }
        }

        public void createSubFolder(bool ok, string targetPath, string subFolderName)
        {
            try
            {
                System.IO.Directory.CreateDirectory(targetPath + "\\" + subFolderName);
                applicationLog.WriteDebugLog(subFolderName + " output folder is created under " + targetPath);

            }
            catch (Exception ex)
            {

                applicationLog.WriteDebugLog(ex.ToString());
            }

        }


    }
}
