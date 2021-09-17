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
    public class ConnectHistory
    {
        protected string inputDate;
        public ConnectHistory()
        {
            inputDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }
        public ConnectHistory(string stringifiedContent) : this()
        {
            if (stringifiedContent == null) return;
            string[] splits = stringifiedContent.Split('\n');
            this.Url = splits[0];
            if (splits.Length >= 2) inputDate = splits[1];
            this.Title = "";
            if (splits.Length >= 3)
            {
                string lineCollectors = "";

                for (int idx = 2; idx < splits.Length; idx++)
                {
                    string lineOne = splits[idx];
                    lineCollectors += lineOne + "\n";
                }

                this.Title = lineCollectors.Trim();
            }
        }
        public string Url { get; set; }
        public string Title { get; set; }
        public string InputDate
        {
            get
            {
                return inputDate;
            }
        }
        public override string ToString()
        {
            return this.Url + "\n" + this.inputDate + "\n" + this.Title;
        }
    }
}
