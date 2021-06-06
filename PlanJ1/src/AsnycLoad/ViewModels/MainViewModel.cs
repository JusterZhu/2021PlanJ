using AsnycLoad.Models;
using Juster.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsnycLoad.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public ICommand LoadAllCmd { get; set; }

        public ICommand LoadNodeDataCmd { get; set; }

        private ObservableCollection<Directory> _directories;

        public ObservableCollection<Directory> Directories 
        { 
            get => _directories;
            set 
            {
                _directories = value;
                OnPropertyChanged("Directories");
            }
        }

        private Directory _currentDir;

        public Directory CurrentDir 
        { 
            get => _currentDir;
            set 
            {
                _currentDir = value;
                OnPropertyChanged("CurrentDir");
            }
        }

        public MainViewModel() 
        {
            LoadNodeDataCmd = new AsyncCommand<object, CancellationToken>(LoadNodeDataCallback);
            LoadAllCmd = AsyncCommand.Create(LoadAllCallback);
            LoadAllCmd.Execute(null);
        }

        private async Task LoadAllCallback()
        {
            this.Directories = new ObservableCollection<Directory>(await GetDrives());
        }

        private async Task<object> LoadNodeDataCallback(object obj, CancellationToken token)
        {
            var dir = obj as Directory;
            if (dir != null && dir.HasChildNodes)
            {
                dir.Nodes = await GetDirectories(dir);
            }
            return dir;
        }

        /// <summary>
        /// 获取所有根盼复
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<Directory>> GetDrives()
        {
            return await Task.Run(()=> 
            {
                var directories = new List<Directory>();
                var driveInfos = System.IO.DriveInfo.GetDrives();
                foreach (var driveInfo in driveInfos)
                {
                    directories.Add(new Directory() { Name = driveInfo.Name, FullName = driveInfo.Name, HasChildNodes = true });
                }
                return directories;
            });
        }

        /// <summary>
        /// 获取当前选中节点下所有的内容
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private async Task<ObservableCollection<Directory>> GetDirectories(Directory directory)
        {
            return await Task.Run(()=> 
            {
                var directories = new ObservableCollection<Directory>();
                var dirInfo = new System.IO.DirectoryInfo(directory.FullName);

                foreach (System.IO.DirectoryInfo directoryInfo in dirInfo.GetDirectories())
                {
                    try
                    {
                        directories.Add(new Directory()
                        {
                            Name = directoryInfo.Name,
                            HasChildNodes = directoryInfo.GetDirectories().Length > 0 || directoryInfo.GetFiles().Length > 0,
                            FullName = directoryInfo.FullName
                        });
                    }
                    catch { }
                }
                foreach (System.IO.FileInfo fileInfo in dirInfo.GetFiles())
                {
                    directories.Add(new Directory()
                    {
                        Name = fileInfo.Name,
                        HasChildNodes = false,
                        FullName = fileInfo.FullName
                    });
                }
                directory.Nodes = new ObservableCollection<Directory>(directories);
                return directories;
            });
        }
    }
}
