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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            Navigate(tfUrl.Text);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            GoForward();
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void tfUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Navigate(tfUrl.Text);
            }
        }

        public void GoBack()
        {
            webbrowser.GoBack();
        }

        public void GoForward()
        {
            webbrowser.GoForward();
        }

        public void Navigate(string url)
        {
            Navigate(url, 0);
        }

        private void Navigate(string url, int retryCount)
        {
            try
            {
                webbrowser.Navigate(url);
            }
            catch (UriFormatException ex)
            {
                if (retryCount == 0) Navigate("https://" + url, retryCount + 1);
                if (retryCount == 1) Navigate("http://" + url, retryCount + 1);
                if (retryCount >= 2)
                {
                    MessageBox.Show("웹 주소가 올바르지 않습니다.");
                    tfUrl.Focus();
                }
                Console.WriteLine(ex.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류가 발생하였습니다.\n" + ex.ToString());
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
