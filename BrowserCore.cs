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
using MSHTML;

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
    public class BrowserCore : Disposeable, Logger
    {
        public const string  VERSION = "0.0.1";
        private static List<Logger> LOG_EVENTS = new List<Logger>();

        protected int historyMaximumCount = 100;
        protected List<ConnectHistory> history = new List<ConnectHistory>();
        protected List<ConnectHistory> favorites = new List<ConnectHistory>();

        BrowserWindow win;
        List<Disposeable> resources = new List<Disposeable>();

        public BrowserCore(BrowserWindow win)
        {
            this.win = win;
            ProcessWindow(win.getWindow());
            RegisterLogger(this);
            HUtilities.FixWebBrowserCompatibility();
        }

        public void ProcessWindow(Window win)
        {
            AeroManager.ApplyAero(win);
        }

        public void Init()
        {
            LoadResources(false);
            win.RefreshConnectHistory(history);
            win.RefreshFavorites(favorites);
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
                Log(ex.ToString());
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

            Log("[navigated]" + uriStr + "[/navigated]");

            if (uriStr != "")
            {
                // 이미 히스토리에 이력이 있으면 넣지 않기
                foreach(ConnectHistory h in history)
                {
                    if (h.Url == uriStr) return;
                }

                ConnectHistory hist = new ConnectHistory();
                hist.Url   = uriStr;
                hist.Title = uriStr;
                try
                {
                    hist.Title = (win.getWebBrowser().Document as dynamic).Title;
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                }
                history.Add(hist);
                DeleteOldHistory();
                win.RefreshConnectHistory(history);
            }
        }

        public void DeleteOldHistory()
        {
            history.Sort((h1, h2) => h1.InputDate.CompareTo(h2.InputDate));
            while (history.Count > historyMaximumCount)
            {
                history.RemoveAt(0);
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

        public int GetConnectionHistoryCount()
        {
            return history.Count;
        }

        public void Shutdown()
        {
            if (win != null) { win.dispose(); win = null; }
            UnregisterLogger(this);
            SaveResources();
            System.Windows.Application.Current.Shutdown();
        }

        public void SaveResources()
        {
            Properties.Settings.Default.Save();
            SaveHistories();
            SaveFavorites();
        }

        public void LoadResources(bool triggerUIEvent = false)
        {
            LoadHistories(triggerUIEvent);
            LoadFavorites(triggerUIEvent);
        }

        protected void SaveHistoryContent(string fileName, List<ConnectHistory> histList)
        {
            string contentsString = "";
            foreach (ConnectHistory his in histList)
            {
                string line = his.ToString().Replace("\n", "\\" + "\n");
                contentsString += line + "\n";
            }
            HUtilities.SaveTextResource(fileName, contentsString.Trim());
        }

        protected List<string> LoadHistoryContent(string fileName)
        {
            string fileContent = HUtilities.ReadTextResource(fileName);
            if (fileContent == null) return new List<string>();

            List<string> lines = HUtilities.SplitLineWithoutEscaped(fileContent);
            fileContent = null;

            return lines;
        }

        protected void SaveHistories()
        {
            DeleteOldHistory();
            SaveHistoryContent("history.txt", history);
        }

        protected void SaveFavorites()
        {
            SaveHistoryContent("favorites.txt", favorites);
        }

        protected void LoadHistories(bool triggerUIEvent = false)
        {
            List<string> lines = LoadHistoryContent("history.txt");

            history.Clear();
            foreach (string line in lines)
            {
                if (history.Count >= historyMaximumCount) break;
                history.Add(new ConnectHistory(line));
            }

            if (triggerUIEvent) win.RefreshConnectHistory(history);
        }

        protected void LoadFavorites(bool triggerUIEvent = false)
        {
            List<string> lines = LoadHistoryContent("favorites.txt");

            favorites.Clear();
            foreach (string line in lines)
            {
                favorites.Add(new ConnectHistory(line));
            }

            if (triggerUIEvent) win.RefreshFavorites(favorites);
        }

        public void Log(object obj)
        {
            if (obj == null) obj = "null";
            Console.WriteLine(obj.ToString());
        }

        public static void RegisterLogger(Logger logger)
        {
            if (LOG_EVENTS.Contains(logger)) return;
            LOG_EVENTS.Add(logger);
        }

        public static void UnregisterLogger(Logger logger)
        {
            if(LOG_EVENTS.Contains(logger)) LOG_EVENTS.Remove(logger);
        }

        public static void PrintLog(object obj)
        {
            if (obj == null) obj = "null";
            Console.WriteLine(obj.ToString());
            foreach (Logger l in LOG_EVENTS)
            {
                try { l.Log(obj); }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
        }
    }
}
