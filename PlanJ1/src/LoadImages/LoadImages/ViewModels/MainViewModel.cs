using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using LoadImages.Models;

namespace LoadImages.ViewModels
{
    public class MainViewModel
    {
        private ObservableCollection<ImageObj> _lstImages;

        const string _filePath = @"E:\temp";

        private BackgroundWorker _backgroundWorker;

        private Task _task;

        public ObservableCollection<ImageObj> LstImages
        {
            get { return _lstImages; }
            set { _lstImages = value; }
        }

        public MainViewModel()
        {
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += _backgroundWorker_DoWork;
            _backgroundWorker.RunWorkerAsync();

            //_task = new Task(new Action(() => { Application.Current.Dispatcher.Invoke(() => { GetImages(_filePath); }); }));
            
            //_task.Start();
        }

        void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Application.Current.Dispatcher.Invoke(() => {  GetImages(_filePath); });
               GetImages(_filePath);
        }

        private void GetImages(string path)
        {
            LstImages = new ObservableCollection<ImageObj>();
            var directoryInfo = new DirectoryInfo(path);
            var pathArry = directoryInfo.GetFiles();
            foreach (var item in pathArry)
            {
                LstImages.Add(new ImageObj() { ImageName = item.Name, ImagePath = item.DirectoryName + "\\"+ item.Name }); 
            }
            MessageBox.Show(LstImages.Count.ToString());
        }
    }
}
