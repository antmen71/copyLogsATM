using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace copyLogsConsole
{
    class zipFile
    {


        public static void CopyStream(Stream source, Stream target)
        {
            try
            {
                const int bufSize = 16384;
                byte[] buf = new byte[bufSize];
                int bytesRead = 0;
                while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
                    target.Write(buf, 0, bytesRead);
            }
            catch (Exception ex)
            {

                applicationLog.WriteDebugLog(ex.ToString());
            }

        }

        public static Uri GetRelativeUri(string currentFile)
        {
            string relPath = currentFile.Substring(currentFile
            .IndexOf('\\')).Replace('\\', '/').Replace(' ', '_');

            int howMAny = CountCharacters('/', relPath);
            int index = GetNthIndex(relPath, '/', howMAny-1);
            relPath = relPath.Substring(index, relPath.Length - index);
            relPath = RemoveAccents(relPath);
            return new Uri(RemoveAccents(relPath), UriKind.Relative);
        }

        public static int GetNthIndex(string s, char t, int n)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == t)
                {
                    count++;
                    if (count == n)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        protected static int CountCharacters(char target, string text)
        {
            int returnVal = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].ToString().ToUpper() == target.ToString().ToUpper())
                    returnVal++;
            }
            return returnVal;
        }
        
        
        public static string RemoveAccents(string input)
        {
            string normalized = input.Normalize(NormalizationForm.FormKD);
            Encoding removal = Encoding.GetEncoding(Encoding.ASCII.CodePage, new EncoderReplacementFallback(""), new DecoderReplacementFallback(""));
            byte[] bytes = removal.GetBytes(normalized);
            return Encoding.ASCII.GetString(bytes);

        }


        public static string RemoveInvalidFileNameChars(string fileName)
        {
            char[] invalidFileChars = Path.GetInvalidFileNameChars();

            foreach (char invalidFChar in invalidFileChars)
            {
                fileName = fileName.Replace(invalidFChar.ToString(), "");
            }
            return fileName;

        }
    }
}
