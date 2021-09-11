using System;
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
        protected List<string> history = new List<string>();

        BrowserWindow win;
        List<Disposeable> resources = new List<Disposeable>();

        public BrowserCore(BrowserWindow win)
        {
            this.win = win;
            ProcessWindow(win.getWindow());
            HUtilities.FixWebBrowserCompatibility();
        }

        public void ProcessWindow(Window win)
        {
            AeroManager.ApplyAero(win);
        }

        public void Init()
        {
            
        }

        public void OpenInternetOption()
        {
            System.Diagnostics.Process p = HUtilities.RunProgram("InetCpl.cpl", ",4");
            p.WaitForExit();
        }

        public void GoBack()
        {
            WebBrowser w = win.getWebBrowser();
            if(w != null) win.getWebBrowser().GoBack();
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
            WebBrowser w = win.getWebBrowser();
            if (btn != null && w != null) btn.IsEnabled = w.CanGoBack;

            btn = win.getForwardButton();
            if (btn != null && w != null) btn.IsEnabled = win.getWebBrowser().CanGoForward;
        }

        public void RefreshTitle()
        {
            string webheader = "";
            string titles = "";

            try
            {
                WebBrowser w = win.getWebBrowser();
                if (w != null)
                {
                    webheader = ((dynamic)w.Document).Title;
                    webheader = webheader.Trim();
                }
                titles = webheader + "";
            }
            catch (Exception ex)
            {
                Log(ex);
            }
            if (!titles.Equals("")) titles = titles + " - " + "Simple Explorer" + " v" + BrowserCore.VERSION;
            else titles = "Simple Explorer" + " v" + BrowserCore.VERSION;

            win.SetTitle(titles);

            BrowserTab t = win.getActiveBrowserTab();
            if (webheader == "") webheader = "[제목없음]";
            if (t != null) t.setHeader(webheader);
        }

        public void Navigate(string url)
        {
            Navigate(url, 0);
        }

        protected void Navigate(string url, int retryCount)
        {
            WebBrowser w = win.getWebBrowser();
            if (w == null) return;
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
                Log(ex);
            }
            RefreshButtonStatuses();
        }

        public void NavigateToString(string html)
        {
            WebBrowser w = win.getWebBrowser();
            if(w != null) w.NavigateToString(html);
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
            if (uriStr != "")
            {
                history.Add(uriStr);
            }
        }

        public void onSourceUpdated()
        {

        }

        public void onLoadCompleted()
        {
            RefreshTitle();
            RefreshButtonStatuses();
        }

        public void onTabChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshTitle();
            RefreshButtonStatuses();
        }

        public void onTabAdded(BrowserTab t)
        {
            NavigateToString(BuiltinHtmlContents.Start);
            RefreshTitle();
            RefreshButtonStatuses();
        }

        public void Shutdown()
        {
            if (win != null) { win.dispose(); win = null; }
            Properties.Settings.Default.Save();
            System.Windows.Application.Current.Shutdown();
        }

        public void Log(object obj)
        {
            if (obj == null) obj = "null";
            Console.WriteLine(obj.ToString());
        }
    }
}
