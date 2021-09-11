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
    /// BrowserTabControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BrowserTabControl : TabControl, Disposeable
    {
        protected ObservableCollection<TabItem> browserTabs = new ObservableCollection<TabItem>();
        protected BrowserWindow win;

        protected int selectedBrowserTabIndex = -1;
        protected bool disableSelectionChangedEvent = true;

        public BrowserTabControl()
        {
            InitializeComponent();
        }
        public void Init(BrowserWindow win)
        {
            this.win = win;

            BrowserTab t = new BrowserTab(win, this);
            browserTabs.Add(t);
            selectedBrowserTabIndex = browserTabs.IndexOf(t);

            AddNewTabButton();
            disableSelectionChangedEvent = false;
            win.getCore().onTabAdded(browserTabs[0] as BrowserTab);
        }
        protected void RemoveNewTabButton(bool syncItemSource = true)
        {
            for(int idx=0; idx<browserTabs.Count; idx++)
            {
                if (browserTabs[idx] is NewTabButton)
                {
                    browserTabs.RemoveAt(idx);
                    idx = 0;
                }
            }
            if (syncItemSource)
            {
                disableSelectionChangedEvent = true;
                this.ItemsSource = browserTabs;
                this.SelectedIndex = selectedBrowserTabIndex;
                disableSelectionChangedEvent = false;
            }
        }
        protected void AddNewTabButton(bool syncItemSource = true)
        {
            RemoveNewTabButton(false);
            browserTabs.Add(new NewTabButton());
            if (syncItemSource)
            {
                disableSelectionChangedEvent = true;
                this.ItemsSource = browserTabs;
                this.SelectedIndex = selectedBrowserTabIndex;
                disableSelectionChangedEvent = false;
            }
        }
        public void NewTab()
        {
            RemoveNewTabButton(false);
            BrowserTab t = new BrowserTab(win, this);
            browserTabs.Add(t);
            SelectedIndex = browserTabs.Count - 1;
            selectedBrowserTabIndex = browserTabs.Count - 1;
            AddNewTabButton();
            win.getCore().onTabAdded(t);
        }
        public List<BrowserTab> getBrowserTabs()
        {
            List<BrowserTab> lists = new List<BrowserTab>();
            foreach (TabItem t in browserTabs)
            {
                if(t is BrowserTab) lists.Add(t as BrowserTab);
            }
            return lists;
        }

        protected TabItem getSelectedTab()
        {
            int idx = getActiveBrowserTabIndex();
            if (idx >= 0 && idx < browserTabs.Count)
            {
                return browserTabs[idx] as TabItem;
            }
            return null;
        }

        public BrowserTab getActiveBrowserTab()
        {
            TabItem t = getSelectedTab();
            if (t is NewTabButton) return null;
            return t as BrowserTab;
        }

        public int getActiveBrowserTabIndex()
        {
            return this.SelectedIndex;
        }

        public int getBrowserTabCount()
        {
            int counts = 0;
            foreach (TabItem t in browserTabs)
            {
                if (t is BrowserTab) counts++;
            }
            return counts;
        }

        public void dispose()
        {
            foreach (TabItem t in browserTabs)
            {
                try { if (t is Disposeable) (t as Disposeable).dispose(); }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            win = null;
        }

        protected void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem t = getSelectedTab();
            if (t == null) return;
            if (t is BrowserTab) selectedBrowserTabIndex = this.SelectedIndex;

            if (disableSelectionChangedEvent) return;

            if (t is NewTabButton)
            {
                if (win != null) NewTab();
            }
            else
            {
                foreach(TabItem tx in browserTabs)
                {
                    if ((tx is BrowserTab) && tx != t)
                    {
                        (tx as BrowserTab).onSelectionChanged(false);
                    }
                }
                (t as BrowserTab).onSelectionChanged(true);
                if (win != null)
                {
                    win.onTabSelectionChanged(sender, e);
                }
            }
        }

        public void RemoveBrowserTab(BrowserTab t)
        {
            if (browserTabs.IndexOf(t) < 0) return;

            browserTabs.Remove(t);
            t.dispose();

            disableSelectionChangedEvent = true;
            ItemsSource = browserTabs;
            disableSelectionChangedEvent = false;

            if (getBrowserTabCount() <= 0)
            {
                NewTab();
            }
            else
            {
                if(browserTabs.Count >= 2) SelectedIndex = browserTabs.Count - 2;
                else SelectedIndex = browserTabs.Count - 1;
            }
        }
    }
}
