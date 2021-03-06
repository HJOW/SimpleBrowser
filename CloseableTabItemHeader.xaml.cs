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
    /// CloseableTabItemHeader.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CloseableTabItemHeader : UserControl, Disposeable
    {
        protected BrowserTabControl tabCtrl;
        protected BrowserTab tab;
        public string Header
        {
            get
            {
                return label_TabTitle.Content.ToString();
            }
            set
            {
                label_TabTitle.Content = value.ToString();
            }
        }
        public void Init(BrowserTabControl tabCtrl, BrowserTab tab)
        {
            this.tabCtrl = tabCtrl;
            this.tab = tab;
        }
        public CloseableTabItemHeader()
        {
            InitializeComponent();
        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            tabCtrl.RemoveBrowserTab(tab);
        }

        public void dispose()
        {
            tabCtrl = null;
            tab = null;
        }

        public void onSelectionChanged(bool selecteds)
        {
            if (selecteds) label_TabTitle.FontWeight = FontWeights.Bold;
            else label_TabTitle.FontWeight = FontWeights.Medium;
        }
    }
}
