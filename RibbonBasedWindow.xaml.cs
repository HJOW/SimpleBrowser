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
using Microsoft.Windows.Controls.Ribbon;

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
    public partial class RibbonBasedWindow : RibbonWindow, BrowserWindow
    {
        protected BrowserCore core;

        public RibbonBasedWindow()
        {
            InitializeComponent();
            core = new BrowserCore(this);
            HideStatusBar();
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

        private void RibbonCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RibbonCommand_CanGoBack(object sender, CanExecuteRoutedEventArgs e)
        {
            core.RefreshButtonStatuses();
            if (getWebBrowser() == null) return;
            e.CanExecute = getWebBrowser().CanGoBack;
        }

        private void RibbonCommand_CanGoForward(object sender, CanExecuteRoutedEventArgs e)
        {
            core.RefreshButtonStatuses();
            if (getWebBrowser() == null) return;
            e.CanExecute = getWebBrowser().CanGoForward;
        }

        private void tfUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                core.Navigate(tfUrl.Text);
            }
        }

        public void OpenInternetOption()
        {
            core.OpenInternetOption();
        }

        public void Shutdown()
        {
            core.Shutdown();
        }

        protected void windowMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Simple Explorer" + " v" + BrowserCore.VERSION;
            tabctrl.Init(this);
            core.Init();
        }

        protected void BrowserWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(core != null) Shutdown();
        }

        public void SetTitle(string title)
        {
            this.Title = title;
        }

        public void SetUrlFieldText(string url)
        {
            getUrlField().Text = url;
        }

        public void dispose()
        {
            tabctrl.dispose();
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

        public void onTabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            core.onTabChanged(sender, e);
        }

        private void RibbonCommand_ThemeSimpleClicked(object sender, ExecutedRoutedEventArgs e)
        {
            btnThemeRibbon.IsChecked = false;
            btnThemeSimple.IsChecked = true;
            Properties.Settings.Default.Theme = "simple";
        }

        private void RibbonCommand_ThemeRibbonClicked(object sender, ExecutedRoutedEventArgs e)
        {
            btnThemeRibbon.IsChecked = true;
            btnThemeSimple.IsChecked = false;
            Properties.Settings.Default.Theme = "ribbon";
        }

        private void RibbonCommand_AboutClicked(object sender, ExecutedRoutedEventArgs e)
        {
            AboutWindow abWin = new AboutWindow();
            core.ProcessWindow(abWin);
            abWin.ShowDialog();
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
    }
}
