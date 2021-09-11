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

namespace SimpleExplorer
{
    /// <summary>
    /// BrowserTab.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BrowserTab : TabItem, Disposeable
    {
        protected BrowserWindow win;
        public BrowserTab(BrowserWindow win)
        {
            InitializeComponent();
            this.win = win;
        }
        public void dispose()
        {
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
    }
}
