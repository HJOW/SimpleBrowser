﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows;
using System.Windows.Resources;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using mshtml;

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
        public const string VERSION = "0.0.1";

        BrowserWindow win;
        List<Disposeable> resources = new List<Disposeable>();

        public BrowserCore(BrowserWindow win)
        {
            this.win = win;
            HUtilities.FixWebBrowserCompatibility();
        }

        public void Init()
        {
            NavigateToString(BuiltinHtmlContents.Start);
        }

        public void OpenInternetOption()
        {
            System.Diagnostics.Process p = HUtilities.RunProgram("InetCpl.cpl", ",4");
            p.WaitForExit();
        }

        public void GoBack()
        {
            win.getWebBrowser().GoBack();
            RefreshButtonStatuses();
        }

        public void GoForward()
        {
            win.getWebBrowser().GoForward();
            RefreshButtonStatuses();
        }

        public void Refresh()
        {
            win.getWebBrowser().Refresh();
            RefreshButtonStatuses();
        }

        public void Print()
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                IHTMLDocument2 doc = (IHTMLDocument2) win.getWebBrowser().Document;

                DocumentPaginator aDocPage = ((IDocumentPaginatorSource)doc).DocumentPaginator;
                aDocPage.PageSize = new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);
                pd.PrintDocument(aDocPage, "Print");
            }
        }

        public void RefreshButtonStatuses()
        {
            Button btn = win.getBackButton();
            if (btn != null) btn.IsEnabled = win.getWebBrowser().CanGoBack;

            btn = win.getForwardButton();
            if (btn != null) btn.IsEnabled = win.getWebBrowser().CanGoForward;
        }

        public void Navigate(string url)
        {
            Navigate(url, 0);
        }

        protected void Navigate(string url, int retryCount)
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
            RefreshButtonStatuses();
        }

        public void NavigateToString(string html)
        {
            win.getWebBrowser().NavigateToString(html);
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

        public void onNavigating(object sender, NavigatingCancelEventArgs e)
        {

        }

        public void onNavigated(object sender, NavigationEventArgs e)
        {
            string uriStr = "";
            if (e.Uri != null) uriStr = e.Uri.ToString();
            win.SetUrlFieldText(uriStr);
        }

        public void onSourceUpdated()
        {

        }

        public void onLoadCompleted()
        {
            string titles = "";

            try
            {
                titles = ((dynamic) win.getWebBrowser().Document).Title;
                titles = titles.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (!titles.Equals("")) titles = titles + " - " + "Simple Explorer" + " v" + BrowserCore.VERSION;
            else titles = "Simple Explorer" + " v" + BrowserCore.VERSION;

            win.SetTitle(titles);

            RefreshButtonStatuses();
        }
    }
}
