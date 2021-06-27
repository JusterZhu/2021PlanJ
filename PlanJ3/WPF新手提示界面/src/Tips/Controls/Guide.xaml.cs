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
    /// 指向显示
    /// </summary>
    public enum Direct
    {
        Visible,
        Hidden
    }

    /// <summary>
    /// 提示内框样式
    /// </summary>
    public enum BorderStyle
    {
        Dotted,
        Solid
    }

    /// <summary>
    /// 凸起指向箭头的样式
    /// </summary>
    public enum BulgeStyle
    {
        Dotted,
        Solid
    }

    /// <summary>
    /// Interaction logic for Guide.xaml
    /// </summary>
    public partial class Guide : Window
    {
        private PathGeometry borderGeometry = new PathGeometry();
        private IList<GuideModel> _guideModels;
        private int currentStep;
        private Hint hintWin;
        private Window _win;

        public Guide(Window win, IList<GuideModel> guides)
        {
            InitializeComponent();
            _win = win;
            _guideModels = guides;
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InitGuideWindow(_win);
            currentStep = 0;
            StartGuide(_guideModels[currentStep]);
        }

        private void InitGuideWindow(Window win)
        {
            Height = win.ActualHeight;
            Width = win.ActualWidth;
            WindowState = win.WindowState;
            Left = win.Left;
            Top = win.Top;
        }

        public void StartGuide(GuideModel guide)
        {
            var focusElemt = guide.UserControl;
            var point = focusElemt.TransformToAncestor(Window.GetWindow(focusElemt)).Transform(new Point(0, 0));
            var rectangleGeometry = new RectangleGeometry();
            rectangleGeometry.Rect = new Rect(0, 0, this.Width, this.Height);
            borderGeometry = Geometry.Combine(borderGeometry, rectangleGeometry, GeometryCombineMode.Union, null);
            border.Clip = borderGeometry;
            var rectangleGeometry1 = new RectangleGeometry();
            rectangleGeometry1.Rect = new Rect(point.X - 5, point.Y - 5, focusElemt.ActualWidth + 10, focusElemt.ActualHeight + 10);
            borderGeometry = Geometry.Combine(borderGeometry, rectangleGeometry1, GeometryCombineMode.Exclude, null);
            border.Clip = borderGeometry;
            InitHintControl(guide, point, focusElemt.ActualWidth, focusElemt.ActualHeight, guide.IsNear);
            currentStep++;
        }

        public void EndStep()
        {
            if (hintWin != null)
            {
                hintWin.EndStep();
            }
        }

        /// <summary>
        /// 初始化气泡内容及位置
        /// </summary>
        /// <param name="guide"></param>
        /// <param name="point"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isNear"></param>
        private void InitHintControl(GuideModel guide, Point point, double width, double height, bool isNear)
        {
            hintWin = new Hint(guide);
            hintWin.NextHintEvent -= OnNextHintEvent;
            hintWin.NextHintEvent += OnNextHintEvent;
            canvas.Children.Add(hintWin);
            var startPoint = new Point { X = point.X + width + 5, Y = point.Y + height / 2 };
            var relativePoint = CalculateRelativePoint(startPoint, 45 + guide.Angle, 100);
            if (isNear)
            {
                var nearRelativePoint = CalculateRelativePoint(point, 45 + guide.Angle, 1);
                Canvas.SetLeft(hintWin, nearRelativePoint.X);
                Canvas.SetTop(hintWin, nearRelativePoint.Y);
            }
            else
            {
                Canvas.SetLeft(hintWin, relativePoint.X - 5);
                Canvas.SetTop(hintWin, relativePoint.Y - hintWin.Height / 2);
            }

            if (guide.Direct == Direct.Visible)
            {
                InitPath(startPoint, relativePoint, 3, guide.PathColor, guide.BulgeStyle);
            }
        }

        /// <summary>
        /// 初始化连接气泡的线样式及位置
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="relativePoint"></param>
        /// <param name="pathThickness"></param>
        /// <param name="pathColor"></param>
        /// <param name="bulgeStyle"></param>
        private void InitPath(Point startPoint, Point relativePoint,
            int pathThickness = 1, string pathColor = "#000000", BulgeStyle bulgeStyle = BulgeStyle.Dotted)
        {
            var path = new Path();
            var pathGeometry = new PathGeometry();
            var pathFigure = new PathFigure();
            pathFigure.StartPoint = startPoint;
            var segmentCollection = new PathSegmentCollection();
            segmentCollection.Add(new LineSegment() { Point = relativePoint });
            pathFigure.Segments = segmentCollection;
            pathGeometry.Figures = new PathFigureCollection() { pathFigure };
            path.Data = pathGeometry;
            path.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(pathColor));
            if (bulgeStyle == BulgeStyle.Dotted)
            {
                path.StrokeDashArray = new DoubleCollection { 2, 4 };
            }
            path.StrokeThickness = pathThickness;
            canvas.Children.Add(path);
        }

        /// <summary>
        /// 计算相对位置
        /// </summary>
        /// <param name="startPoint">已知起始点坐标</param>
        /// <param name="angle">相对位置角度</param>
        /// <param name="bevel">相对距离</param>
        /// <returns></returns>
        private Point CalculateRelativePoint(Point startPoint, int angle, double bevel)
        {
            var radian = angle * Math.PI / 180;
            var xMargin = Math.Cos(radian) * bevel;
            var yMargin = Math.Sin(radian) * bevel;
            return new Point(startPoint.X + xMargin, startPoint.Y + yMargin);
        }

        /// <summary>
        /// 下一步具体实现
        /// </summary>
        private void OnNextHintEvent()
        {
            if (_guideModels.Count == 0) return;


            var beforGuide = _guideModels[currentStep - 1];

            if (_guideModels.Count == currentStep)
            {
                if (beforGuide.Callback != null)
                {
                    beforGuide.Callback.Invoke();
                }
                else
                {
                    EndStep();
                }
                return;
            }

            var currentGuide = _guideModels[currentStep];

            if (beforGuide != null)
            {
                canvas.Children.Clear();
                if (beforGuide.Callback != null)
                {
                    beforGuide.Callback.Invoke();
                }
            }

            if (currentGuide != null)
            {
                StartGuide(currentGuide);
            }
        }
    }

    public class GuideModel
    {
        private FrameworkElement _userControl;
        private string content;

        public Action Callback { get; set; }

        private string _borderColor;

        public string BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        private string _backgroundColor;

        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        private string _fontColor;

        public string FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        private string _pathColor;

        public string PathColor
        {
            get { return _pathColor; }
            set { _pathColor = value; }
        }

        public bool IsNear { get; set; }

        /// <summary>
        /// Foucse control.
        /// </summary>
        public FrameworkElement UserControl
        {
            get
            {
                return _userControl;
            }

            set
            {
                _userControl = value;
            }
        }

        /// <summary>
        /// 引导内容.
        /// </summary>
        public string Content
        {
            get
            {
                return content;
            }

            set
            {
                content = value;
            }
        }

        /// <summary>
        /// 是否显示气泡箭头
        /// </summary>
        public Direct Direct { get; set; }

        /// <summary>
        /// 提示框边框样式
        /// </summary>
        public BorderStyle BorderStyle { get; set; }

        /// <summary>
        /// 凸起样式
        /// </summary>
        public BulgeStyle BulgeStyle { get; set; }
        /// <summary>
        /// 气泡相对旋转角度
        /// </summary>
        public int Angle { get; set; }

        /// <summary>
        /// 第几步
        /// </summary>
        public int Step { get; set; }
    }
}
