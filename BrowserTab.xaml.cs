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
    /// <summary>
    /// BrowserTab.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BrowserTab : TabItem, Disposeable
    {
        protected BrowserWindow win;
        protected BrowserTabControl tabCtrl;
        protected CloseableTabItemHeader headerObj;
        public BrowserTab(BrowserWindow win, BrowserTabControl tabCtrl)
        {
            InitializeComponent();
            this.win = win;
            this.tabCtrl = tabCtrl;

            headerObj = new CloseableTabItemHeader();
            headerObj.Init(tabCtrl, this);
            Header = this.headerObj;
        }
        public void dispose()
        {
            if (headerObj != null) headerObj.dispose();
            this.headerObj = null;
            this.win = null;
        }
        protected void webbrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            win.getCore().onNavigating(sender, e);
        }

        protected void webbrowser_Navigated(object sender, NavigationEventArgs e)
        {
            win.getCore().onNavigated(sender, e);
        }

        protected void webbrowser_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            win.getCore().onSourceUpdated();
        }

        protected void webbrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            win.getCore().onLoadCompleted();
        }

        public WebBrowser getWebBrowser()
        {
            return webbrowser;
        }

        public void onSelectionChanged(bool selecteds)
        {
            if (headerObj != null) headerObj.onSelectionChanged(selecteds);
        }

        public void setHeader(string text)
        {
            if (headerObj == null) Header = text;
            else headerObj.Header = text;
        }
    }
}
