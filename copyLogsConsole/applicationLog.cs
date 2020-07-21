using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Packaging;


namespace copyLogsConsole
{
    public class applicationLog
    {


        public static void WriteDebugLog(String Message)
        {
            StreamWriter sw = null;
            try
            {
                //Console.Write(Message + "\n");
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFileClC.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                WriteDebugLog(ex.ToString());
            }

        }

        public static string checkLogFileSize()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;
            int second = DateTime.Now.Second;
            string status = "";
            string logSize = "";
            string logStatus = "The log file has not been deleted";


            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\LogFileClC.txt"))
            {
                long fileSize = new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\LogFileClC.txt").Length;
                logSize = fileSize.ToString();
                if (fileSize > 10000000)
                {

                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\archiveLogFiles");

                    using (Package package = ZipPackage.Open(AppDomain.CurrentDomain.BaseDirectory + "\\archiveLogFiles\\LogFileClC" + "_" + year.ToString() +
                        month.ToString() + day.ToString() + "_" + hour + minute + second + ".zip", FileMode.Create))
                    {
                        string startFolder = AppDomain.CurrentDomain.BaseDirectory;

                        applicationLog.WriteDebugLog("Packing " + startFolder + @"\LogFileClC.txt");
                        Uri relUri = zipFile.GetRelativeUri(startFolder + "LogFileClC.txt");
                        PackagePart packagePart =
                        package.CreatePart(relUri, System.Net.Mime.MediaTypeNames.Application.Octet, CompressionOption.Maximum);
                        using (FileStream fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\LogFileClC.txt", FileMode.Open, FileAccess.Read))
                        {
                            zipFile.CopyStream(fileStream, packagePart.GetStream());

                        }

                    }
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\LogFileClC.txt");
                    logStatus = "Log File has been deleted.";
                }

            }
            else if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\LogFileClC.txt"))
            {
                logStatus = "Log file not found. Created.";
            }
            return status = "Log file size was: " + logSize + ". " + logStatus;   
        }
    }
}

