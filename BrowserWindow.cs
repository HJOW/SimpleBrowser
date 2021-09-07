using System;
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
    /**
     * 브라우저 창 컨트롤의 상위 클래스
     *    각 타입별 브라우저 창이 이 클래스와 상속관계
     */
    public class BrowserWindow : Window
    {
        protected BrowserCore core;

        public BrowserWindow()
        {
            core = new BrowserCore(this);
        }

        public virtual WebBrowser getWebBrowser()
        {
            return null;
        }

        public virtual TextBox getUrlField()
        {
            return null;
        }

        public virtual Button getBackButton()
        {
            return null;
        }

        public virtual Button getForwardButton()
        {
            return null;
        }

        public virtual Button getRefreshButton()
        {
            return null;
        }

        public virtual ProgressBar getProgressBar()
        {
            return null;
        }

        public virtual TextBox getStatusTextBox()
        {
            return null;
        }

        public virtual Window getWindow()
        {
            return this;
        }

        public void OpenInternetOption()
        {
            System.Diagnostics.Process p = HUtilities.RunProgram("InetCpl.cpl", ",4");
            p.WaitForExit();
        }

        public void Shutdown()
        {
            if (core != null) { core.dispose(); core = null; }
            System.Windows.Application.Current.Shutdown();
        }

        protected void windowMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Simple Explorer" + " v" + BrowserCore.VERSION;
            core.Init();
        }

        protected void BrowserWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Shutdown();
        }

        protected void webbrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {

        }

        protected void webbrowser_Navigated(object sender, NavigationEventArgs e)
        {
            string uriStr = "";
            if (e.Uri != null) uriStr = e.Uri.ToString();
            if (getUrlField() != null) getUrlField().Text = uriStr;
        }

        protected void webbrowser_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        protected void webbrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            string titles = "";

            try
            {
                titles = ((dynamic)getWebBrowser().Document).Title;
                titles = titles.Trim();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (!titles.Equals("")) titles = titles + " - " + "Simple Explorer" + " v" + BrowserCore.VERSION;
            else titles = "Simple Explorer" + " v" + BrowserCore.VERSION;

            this.Title = titles;
        }
    }
}
