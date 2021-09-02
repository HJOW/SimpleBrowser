using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleExplorer
{
    public class HUtilities
    {
        public static Boolean isEmpty(object obj)
        {
            if (obj == null) return true;

            string str = obj.ToString();
            str = str.Trim();
            if (str.Length <= 0) return true;
            return false;
        }
        public static System.Diagnostics.Process RunProgram(String str)
        {
            System.Diagnostics.ProcessStartInfo pri = new System.Diagnostics.ProcessStartInfo();
            System.Diagnostics.Process proc = new System.Diagnostics.Process();

            pri.FileName = str;
            pri.CreateNoWindow = false;
            pri.UseShellExecute = false;
            pri.RedirectStandardInput  = true;
            pri.RedirectStandardOutput = true;
            pri.RedirectStandardError  = true;

            proc.StartInfo = pri;
            proc.Start();
            return proc;
        }
        public static string RunCommand(String str)
        {
            System.Diagnostics.Process proc = RunProgram("cmd.exe");

            proc.StandardInput.Write(str);
            proc.StandardInput.Close();

            string res = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            proc.Close();

            return res;
        }
    }
}
