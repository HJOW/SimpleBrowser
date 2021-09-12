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
using System.Windows.Shapes;

namespace SimpleExplorer
{
    /// <summary>
    /// LogWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();
        }

        public void Log(object msg)
        {
            if (msg == null) msg = "null";
            taLog.AppendText(msg + "\n");
        }

        private void bt_close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void bt_clear_Click(object sender, RoutedEventArgs e)
        {
            taLog.Text = "";
        }
    }
}
