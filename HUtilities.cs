using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;
/*
 Copyright 2021 HJOW

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
 */
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
        public static System.Diagnostics.Process RunProgram(string str)
        {
            return System.Diagnostics.Process.Start(str);
        }
        public static System.Diagnostics.Process RunProgram(string str, string param)
        {
            return System.Diagnostics.Process.Start(str, param);
        }
        public static void FixWebBrowserCompatibility()
        {
            setFeatureBrowserEmulation();
        }

        public static bool setFeatureBrowserEmulation(bool localMachine = false, int versionCode = 11001)
        {
            try
            {
                if (localMachine)
                {
                    using (RegistryKey Key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree))
                        if (Key.GetValue(CurrentProcess.ProcessName + ".exe") == null)
                            Key.SetValue(CurrentProcess.ProcessName + ".exe", versionCode, RegistryValueKind.DWord);
                }
                else
                {
                    using (RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree))
                        if (Key.GetValue(CurrentProcess.ProcessName + ".exe") == null)
                            Key.SetValue(CurrentProcess.ProcessName + ".exe", versionCode, RegistryValueKind.DWord);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public static void SetBrowserFeatureControlKey(string feature, string appName, uint value)
        {
            using (var key1 = Registry.CurrentUser.CreateSubKey(string.Concat(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\", feature), RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                key1.SetValue(appName, (uint)value, RegistryValueKind.DWord);
            }
        }

        public static List<string> SplitLineWithoutEscaped(string contents)
        {
            List<string> lines = new List<string>();
            char[] chars = contents.ToCharArray();
            char lasts = ' ';
            string collectors = "";

            foreach (char c in chars)
            {
                if (c == '\n')
                {
                    if (lasts == '\\')
                    {
                        collectors += "\n";
                    }
                    else
                    {
                        lines.Add(collectors);
                        collectors = "";
                    }
                }
                else
                {
                    collectors += c.ToString();
                }

                lasts = c;
            }

            return lines;
        }

        public static string GetOSHomeDirectory()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }

        public static void SaveTextFile(string fileFullPath, string contents)
        {
            if (contents == null) contents = "";
            List<string> lines = SplitLineWithoutEscaped(contents);

            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(fileFullPath, false, System.Text.Encoding.UTF8);
                foreach (string line in lines)
                {
                    writer.WriteLine(line);
                }

                writer.Close();
                writer.Dispose();
                writer = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                    writer = null;
                }
            }
        }

        public static string ReadTextFile(string fileFullPath)
        {
            string contents = null;
            StreamReader reader = null;

            try
            {
                reader = new StreamReader(fileFullPath);
                contents = "";
                string line;
                while (true)
                {
                    line = reader.ReadLine();
                    if (line == null) break;
                    contents += line + "\n";
                }

                reader.Close();
                reader.Dispose();
                reader = null;

                contents = contents.Trim();
                return contents;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
            }

            return contents;
        }

        public static void SaveTextResource(string fileName, string contents)
        {
            string dir = GetOSHomeDirectory() + Path.DirectorySeparatorChar + ".simpleBrowser";
            if (!File.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            SaveTextFile(dir + Path.DirectorySeparatorChar + fileName, contents);
        }

        public static string ReadTextResource(string fileName)
        {
            string dir = GetOSHomeDirectory() + Path.DirectorySeparatorChar + ".simpleBrowser";
            if (!File.Exists(dir))
            {
                return null;
            }
            return ReadTextFile(dir + Path.DirectorySeparatorChar + fileName);
        }

        public static System.Diagnostics.Process CurrentProcess
        {
            get
            {
                return System.Diagnostics.Process.GetCurrentProcess();
            }
        }
    }
}
