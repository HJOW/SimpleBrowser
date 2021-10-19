using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.Windows.Resources;
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
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                string theme = SimpleExplorer.Properties.Settings.Default.Theme;
                if (theme == null) theme = "default";
                theme = theme.Trim().ToLower();

                if (theme == "simple") StartupUri = new Uri("SimpleWindow.xaml", UriKind.Relative);
                else StartupUri = new Uri("RibbonBasedWindow.xaml", UriKind.Relative);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Simple Explorer 를 실행하지 못하였습니다.\n" + ex.ToString());
            }
        }
    }
}
