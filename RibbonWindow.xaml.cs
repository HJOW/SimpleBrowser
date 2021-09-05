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

        private void BrowserWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Shutdown();
        }

        private void RibbonApplicationMenuItem_ap_exit_Click(object sender, ExecutedRoutedEventArgs e)
        {
            Shutdown();
        }

        private void RibbonButton_ap_ie_option_Click(object sender, ExecutedRoutedEventArgs e)
        {
            OpenInternetOption();
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

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            core.Navigate(tfUrl.Text);
        }

        private void webbrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {

        }

        private void webbrowser_Navigated(object sender, NavigationEventArgs e)
        {
            tfUrl.Text = e.Uri.ToString();
        }

        private void webbrowser_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void webbrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {

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

        public override WebBrowser getWebBrowser()
        {
            return webbrowser;
        }

        public override TextBox getUrlField()
        {
            return tfUrl;
        }

        private void RibbonCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
