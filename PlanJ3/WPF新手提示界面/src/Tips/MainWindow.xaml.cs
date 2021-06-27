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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tips.Controls;

namespace Tips
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Guide guideView;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InitGuide();
        }

        private void InitGuide()
        {
            var allGuide = new List<GuideModel>();
            allGuide.AddRange(InitWelecome());
            var akkGuides = allGuide.OrderBy(i => i.Step).ToList();
            guideView = new Guide(this, akkGuides);
            guideView.Owner = this;
            guideView.ShowDialog();
        }

        /// <summary>
        /// 添加每一个步骤
        /// </summary>
        /// <returns></returns>
        List<GuideModel> InitWelecome()
        {
            var lstWelecome = new List<GuideModel>();

            var findCtrl = FindControl<Button>("BtnCreateProject1", panelWorkspace);
            if (findCtrl != null)
            {
                var createModel = new GuideModel()
                {
                    Step = 0,
                    Direct = Direct.Visible,
                    BorderStyle = BorderStyle.Solid,
                    BulgeStyle = BulgeStyle.Solid,
                    BackgroundColor = "#FF8831",
                    BorderColor = "#FF8831",
                    FontColor = "#FFFFFF",
                    PathColor = "#FF8831",
                    Content = "1111",
                    UserControl = findCtrl,
                    Callback = () =>
                    {
                        //findCtrl.Command.Execute(null);
                    }
                };
                lstWelecome.Add(createModel);
            }

            var findCtrl1 = FindControl<Button>("BtnCreateProject2", panelWorkspace);
            var createModel1 = new GuideModel()
            {
                Step = 1,
                Direct = Direct.Visible,
                BorderStyle = BorderStyle.Dotted,
                BulgeStyle = BulgeStyle.Dotted,
                BackgroundColor = "#FF8831",
                BorderColor = "#FF8831",
                FontColor = "#FFFFFF",
                PathColor = "#FF8831",
                Content = "222222",
                UserControl = findCtrl1,
                Callback = () =>
                {
                    //findCtrl2.Command.Execute(null);
                }
            };
            lstWelecome.Add(createModel1);

            var findCtrl2 = FindControl<Button>("BtnCreateProject3", panelWorkspace);
            var createModel2 = new GuideModel()
            {
                Step = 2,
                Direct = Direct.Hidden,
                BorderStyle = BorderStyle.Solid,
                BulgeStyle = BulgeStyle.Solid,
                BackgroundColor = "#FF8831",
                BorderColor = "#FF8831",
                FontColor = "#FFFFFF",
                PathColor = "#FF8831",
                Content = "33333",
                UserControl = findCtrl2,
                Callback = () =>
                {
                    //findCtrl3.Command.Execute(null);
                }
            };
            lstWelecome.Add(createModel2);

            var findCtrl3 = FindControl<Button>("BtnCreateProject4", panelWorkspace);
            var createModel3 = new GuideModel()
            {
                Step = 3,
                Direct = Direct.Visible,
                BorderStyle = BorderStyle.Solid,
                BulgeStyle = BulgeStyle.Dotted,
                BackgroundColor = "#FF8831",
                BorderColor = "#FF8831",
                FontColor = "#FFFFFF",
                PathColor = "#FF8831",
                Content = "44444",
                UserControl = findCtrl3,
                Callback = () =>
                {
                    // findCtrl4.Command.Execute(null);
                    guideView.EndStep();
                }
            };
            lstWelecome.Add(createModel3);

            return lstWelecome;
        }

        T FindControl<T>(string controlName, FrameworkElement root) where T : FrameworkElement
        {
            var lstControl = new List<T>();
            FindVisualChild(root, ref lstControl);
            if (lstControl.Count > 0)
            {
                var findControl = lstControl.FirstOrDefault(ctrl => ctrl.Name == controlName);
                return findControl;
            }
            return null;
        }

        void FindVisualChild<T>(DependencyObject parent, ref List<T> lstT) where T : DependencyObject
        {
            if (parent != null)
            {
                T child = default(T);
                int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < numVisuals; i++)
                {
                    Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                    child = v as T;
                    if (child != null)
                    {
                        lstT.Add(child);
                    }
                    FindVisualChild<T>(v, ref lstT);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InitGuide();
        }
    }
}
