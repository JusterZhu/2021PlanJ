using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MessageCenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var da = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                To = ((Label)sender).Content.Equals("取消") ? 0d : 180d
            };
            var ar = FindName("Aar") as AxisAngleRotation3D;
            if (ar != null)
                ar.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);
        }
    }
}
