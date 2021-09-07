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
    public partial class SimpleWindow : BrowserWindow
    {
        public SimpleWindow()
        {
            InitializeComponent();
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

        public override WebBrowser getWebBrowser()
        {
            return webbrowser;
        }

        public override TextBox getUrlField()
        {
            return tfUrl;
        }

        public override Button getBackButton()
        {
            return btnBack;
        }

        public override Button getForwardButton()
        {
            return btnForward;
        }

        public override Button getRefreshButton()
        {
            return btnRefresh;
        }

        public override ProgressBar getProgressBar()
        {
            return progBar;
        }

        public override TextBox getStatusTextBox()
        {
            return tfStatus;
        }

        public override Window getWindow()
        {
            return this;
        }
    }
}
