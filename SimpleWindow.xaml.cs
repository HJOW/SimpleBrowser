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
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SimpleWindow : Window, BrowserWindow
    {
        protected BrowserCore core;
        public SimpleWindow()
        {
            InitializeComponent();
            core = new BrowserCore(this);
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            core.Navigate(tfUrl.Text);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            core.GoBack();
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            core.GoForward();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            core.Refresh();
        }

        private void MenuItem_IEOption_Click(object sender, RoutedEventArgs e)
        {
            OpenInternetOption();
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void tfUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                core.Navigate(tfUrl.Text);
            }
        }

        private void MenuItem_Print_Click(object sender, RoutedEventArgs e)
        {
            core.Print();
        }

        public WebBrowser getWebBrowser()
        {
            return webbrowser;
        }

        public TextBox getUrlField()
        {
            return tfUrl;
        }

        public Button getBackButton()
        {
            return btnBack;
        }

        public Button getForwardButton()
        {
            return btnForward;
        }

        public Button getRefreshButton()
        {
            return btnRefresh;
        }

        public ProgressBar getProgressBar()
        {
            return progBar;
        }

        public TextBox getStatusTextBox()
        {
            return tfStatus;
        }

        public Window getWindow()
        {
            return this;
        }

        public void OpenInternetOption()
        {
            core.OpenInternetOption();
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
            core.onNavigating(sender, e);
        }

        protected void webbrowser_Navigated(object sender, NavigationEventArgs e)
        {
            core.onNavigated(sender, e);
        }

        protected void webbrowser_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            core.onSourceUpdated();
        }

        protected void webbrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            core.onLoadCompleted();
        }

        public void SetTitle(string title)
        {
            this.Title = title;
        }

        public void SetUrlFieldText(string url)
        {
            if (getUrlField() != null) getUrlField().Text = url;
        }
    }
}
