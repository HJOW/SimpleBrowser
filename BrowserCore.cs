﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    public class BrowserCore : Disposeable
    {
        BrowserWindow win;
        List<Disposeable> resources = new List<Disposeable>();

        public BrowserCore(BrowserWindow win)
        {
            this.win = win;
        }

        public void OpenInternetOption()
        {
            System.Diagnostics.Process p = HUtilities.RunProgram("InetCpl.cpl", ",4");
            p.WaitForExit();
        }

        public void GoBack()
        {
            win.getWebBrowser().GoBack();
        }

        public void GoForward()
        {
            win.getWebBrowser().GoForward();
        }

        public void Refresh()
        {
            win.getWebBrowser().Refresh();
        }

        public void Navigate(string url)
        {
            Navigate(url, 0);
        }

        private void Navigate(string url, int retryCount)
        {
            try
            {
                win.getWebBrowser().Navigate(url);
            }
            catch (UriFormatException ex)
            {
                if (retryCount == 0) Navigate("https://" + url, retryCount + 1);
                if (retryCount == 1) Navigate("http://" + url, retryCount + 1);
                if (retryCount >= 2)
                {
                    MessageBox.Show("웹 주소가 올바르지 않습니다.");
                    if (win.getUrlField() != null) win.getUrlField().Focus();
                }
                Console.WriteLine(ex.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류가 발생하였습니다.\n" + ex.ToString());
                Console.WriteLine(ex.ToString());
            }
        }

        public void dispose()
        {
            disposeAll();
            this.win = null;
        }

        public void disposeAll()
        {
            if (resources == null) return;
            for (int idx = 0; idx < resources.Count; idx++)
            {
                resources[idx].dispose();
            }
            resources.Clear();
        }
    }
}
