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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tips.Controls
{
    /// <summary>
    /// Interaction logic for Hint.xaml
    /// </summary>
    public partial class Hint : UserControl
    {
        public delegate void NextHintDelegate();
        public event NextHintDelegate NextHintEvent;
         
        public Hint(GuideModel guide, double width = 300, double height = 56)
        {
            InitializeComponent();
            Width = width;
            Height = height;
            InitBorder(guide.BorderStyle, guide.BorderColor, width, height);
            InitTextblock(guide.Content, guide.FontColor);
        }

        /// <summary>
        /// 提示气泡边框样式修改
        /// </summary>
        /// <param name="type"></param>
        /// <param name="borderColor"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void InitBorder(BorderStyle type, string borderColor, double width, double height)
        {
            RootBrd.BorderThickness = new Thickness(3);
            RootBrd.Width = width;
            RootBrd.Height = height;
            RootBrd.CornerRadius = new CornerRadius(4);

            if (type == BorderStyle.Dotted)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.StrokeDashArray = new DoubleCollection { 4, 2 };
                rectangle.StrokeThickness = 3;
                rectangle.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(borderColor));
                rectangle.Width = RootBrd.Width;
                rectangle.Height = RootBrd.Height;
                RootBrd.BorderBrush = new VisualBrush(rectangle);
            }
            else if (type == BorderStyle.Solid)
            {
                RootBrd.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(borderColor));
            }
        }

        /// <summary>
        /// 提示气泡内容初始化
        /// </summary>
        /// <param name="text"></param>
        /// <param name="foreground"></param>
        private void InitTextblock(string text, string foreground)
        {
            ContentTxb.Text = text;
            ContentTxb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(foreground));
            ContentTxb.Margin = new Thickness(3);
            ContentTxb.HorizontalAlignment = HorizontalAlignment.Center;
            ContentTxb.VerticalAlignment = VerticalAlignment.Center;
            ContentTxb.TextWrapping = TextWrapping.WrapWithOverflow;
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Viewbox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NextHintEvent();
        }

        /// <summary>
        /// 最后一个步骤调用关闭提示界面
        /// </summary>
        public void EndStep()
        {
            Window.GetWindow(this).Close();
        }

    }
}
