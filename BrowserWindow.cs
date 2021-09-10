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
    /**
     * 브라우저 창 컨트롤의 상위 클래스
     *    각 타입별 브라우저 창이 이 클래스와 상속관계
     */
    public interface BrowserWindow
    {
        WebBrowser getWebBrowser();
        TextBox getUrlField();
        Button getBackButton();
        Button getForwardButton();
        Button getRefreshButton();
        ProgressBar getProgressBar();
        TextBox getStatusTextBox();
        Window getWindow();
        void OpenInternetOption();
        void Shutdown();
        void SetTitle(string title);
        void SetUrlFieldText(string url);
    }
}
