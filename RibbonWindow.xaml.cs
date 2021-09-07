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
    /// RibbonWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RibbonWindow : BrowserWindow
    {
        public RibbonWindow()
        {
            InitializeComponent();
        }

        private void RibbonApplicationMenuItem_ap_exit_Click(object sender, ExecutedRoutedEventArgs e)
        {
            Shutdown();
        }

        private void RibbonButton_ap_ie_option_Click(object sender, ExecutedRoutedEventArgs e)
        {
            OpenInternetOption();
        }

        private void RibbonButton_ap_print_Click(object sender, ExecutedRoutedEventArgs e)
        {
            core.Print();
        }

        private void RibbonButton_home_back_Click(object sender, ExecutedRoutedEventArgs e)
        {
            core.GoBack();
        }

        private void RibbonButton_home_front_Click(object sender, ExecutedRoutedEventArgs e)
        {
            core.GoForward();
        }

        private void RibbonButton_home_refresh_Click(object sender, ExecutedRoutedEventArgs e)
        {
            core.Refresh();
        }

        private void RibbonButton_home_go_Click(object sender, ExecutedRoutedEventArgs e)
        {
            core.Navigate(tfUrl.Text);
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

        private void RibbonCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RibbonCommand_CanGoBack(object sender, CanExecuteRoutedEventArgs e)
        {
            core.RefreshButtonStatuses();
            e.CanExecute = getWebBrowser().CanGoBack;
        }

        private void RibbonCommand_CanGoForward(object sender, CanExecuteRoutedEventArgs e)
        {
            core.RefreshButtonStatuses();
            e.CanExecute = getWebBrowser().CanGoForward;
        }

        private void tfUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                core.Navigate(tfUrl.Text);
            }
        }
    }
}
