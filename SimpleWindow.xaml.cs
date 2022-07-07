using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        protected LogWindow logViewer = new LogWindow();

        public SimpleWindow()
        {
            InitializeComponent();
            core = new BrowserCore(this);
            HideStatusBar();
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
            Shutdown();
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
            BrowserTab tabOne = getActiveBrowserTab();
            if (tabOne == null) return null;
            return tabOne.getWebBrowser();
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
            if(core != null) core.Shutdown();
        }

        protected void windowMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Simple Explorer" + " v" + BrowserCore.VERSION;
            tabctrl.Init(this);
            core.Init();
        }

        protected void BrowserWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Shutdown();
        }

        public void onTabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            core.onTabChanged(sender, e);
        }

        public void SetTitle(string title)
        {
            this.Title = title;
        }

        public void SetUrlFieldText(string url)
        {
            if (getUrlField() != null) getUrlField().Text = url;
        }

        public void dispose()
        {
            tabctrl.dispose();
            logViewer = null;
            core = null;
        }

        public List<BrowserTab> getBrowserTabs()
        {
            return tabctrl.getBrowserTabs();
        }

        public BrowserTab getActiveBrowserTab()
        {
            return tabctrl.getActiveBrowserTab();
        }

        public int getActiveBrowserTabIndex()
        {
            return tabctrl.getActiveBrowserTabIndex();
        }

        public int getBrowserTabCount()
        {
            return tabctrl.getBrowserTabCount();
        }

        public BrowserCore getCore()
        {
            return core;
        }

        public void NewTab()
        {
            tabctrl.NewTab();
        }

        private void MenuItem_Click_Theme_Simple(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Theme = "simple";
            menu_setting_theme_simple.IsChecked = true;
            menu_setting_theme_ribbon.IsChecked = false;
        }

        private void MenuItem_Click_Theme_Ribbon(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Theme = "ribbon";
            menu_setting_theme_simple.IsChecked = false;
            menu_setting_theme_ribbon.IsChecked = true;
        }

        private void MenuItem_Click_Help_About(object sender, RoutedEventArgs e)
        {
            AboutWindow abWin = new AboutWindow(core);
            core.ProcessWindow(abWin);
            abWin.Show();
        }

        public void HideStatusBar()
        {
            dockStatusBar.Visibility = System.Windows.Visibility.Hidden;
            rowdefStatus.Height = new GridLength(0, GridUnitType.Star);
        }

        public void ShowStatusBar()
        {
            dockStatusBar.Visibility = System.Windows.Visibility.Visible;
            rowdefStatus.Height = new GridLength(20, GridUnitType.Star);
        }

        public void Log(object msg)
        {
            if (logViewer != null) logViewer.Log(msg);
        }

        private void MenuItem_LogViewer_Click(object sender, RoutedEventArgs e)
        {
            if (logViewer != null) logViewer.Show();
        }

        public void RefreshConnectHistory(List<ConnectHistory> lists)
        {

        }

        public void RefreshFavorites(List<ConnectHistory> lists)
        {

        }

        private void menu_iever_new_Click(object sender, RoutedEventArgs e)
        {
            bool res = HUtilities.setFeatureBrowserEmulation(false, 11001);
            if (res)
            {
                MessageBox.Show("엔진 버전이 설정되었습니다.");
            }
            else
            {
                MessageBox.Show("엔진 버전 설정에 실패하였습니다.");
            }
        }

        private void menu_iever_7_Click(object sender, RoutedEventArgs e)
        {
            bool res = HUtilities.setFeatureBrowserEmulation(false, 7000);
            if (res)
            {
                MessageBox.Show("엔진 버전이 설정되었습니다.");
            }
            else
            {
                MessageBox.Show("엔진 버전 설정에 실패하였습니다.");
            }
        }

        private void menu_iever_lm_new_Click(object sender, RoutedEventArgs e)
        {
            bool res = HUtilities.setFeatureBrowserEmulation(true, 11001);
            if (res)
            {
                MessageBox.Show("엔진 버전이 설정되었습니다.");
            }
            else
            {
                MessageBox.Show("엔진 버전 설정에 실패하였습니다.");
            }
        }

        private void menu_iever_lm_7_Click(object sender, RoutedEventArgs e)
        {
            bool res = HUtilities.setFeatureBrowserEmulation(true, 7000);
            if (res)
            {
                MessageBox.Show("엔진 버전이 설정되었습니다.");
            }
            else
            {
                MessageBox.Show("엔진 버전 설정에 실패하였습니다.");
            }
        }
    }
}
