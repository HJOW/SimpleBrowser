﻿using System;
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
    /// RibbonConnectionHistoryMenuItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RibbonConnectionHistoryMenuItem : RibbonMenuItem, Disposeable
    {
        protected ConnectHistory history;
        protected BrowserCore core;
        public RibbonConnectionHistoryMenuItem(BrowserCore core, ConnectHistory hist)
        {
            InitializeComponent();
            this.core = core;
            this.history = hist;
            this.Header = hist.Title;
        }

        public ConnectHistory GetHistory()
        {
            return history;
        }

        public void dispose()
        {
            core = null;
            history = null;
        }

        private void RibbonMenuItem_Click(object sender, RoutedEventArgs e)
        {
            core.Navigate(history.Url);
        }
    }
}
