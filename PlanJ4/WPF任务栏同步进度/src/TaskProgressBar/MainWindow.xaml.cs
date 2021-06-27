using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;

namespace TaskProgressBar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //参考文档
        //https://docs.microsoft.com/zh-cn/dotnet/api/system.windows.shell.taskbariteminfo?f1url=%3FappId%3DDev16IDEF1%26l%3DZH-CN%26k%3Dk(System.Windows.Shell.TaskbarItemInfo);k(VS.XamlEditor)%26rd%3Dtrue&view=net-5.0

        public MainWindow()
        {
            InitializeComponent();
            MyProgressBar.Minimum = 0;
            MyProgressBar.Maximum = 100;
        }

        private async void Update()
        {
            while (true)
            {
                if (MyProgressBar.Value == MyProgressBar.Maximum)
                {
                    break;
                }
                MyProgressBar.Value += 10;
                TaskbarItemInfo.ProgressValue = MyProgressBar.Value / MyProgressBar.Maximum;
                await Task.Delay(500);
            }
        }

        private void MyBtn_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }
    }
}
