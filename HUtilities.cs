using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
