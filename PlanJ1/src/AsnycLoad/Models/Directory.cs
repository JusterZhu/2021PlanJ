using Juster.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AsnycLoad.Models
{
    public class Directory : BindableBase
    {
        #region Fields

        private string name;
        private bool hasChildNodes;
        private bool _isExpanded;
        private bool _isSelected;
        private string fullName;
        private ObservableCollection<Directory> _nodes;

        #endregion

        public Directory() 
        {
            Nodes = new ObservableCollection<Directory>();
        }

        #region Properties

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                OnPropertyChanged("IsExpanded");
                if (!value)
                {
                    Nodes.Clear();
                }
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
            }
        }

        public bool HasChildNodes
        {
            get { return hasChildNodes; }
            set
            {
                hasChildNodes = value;
                OnPropertyChanged("HasChildNodes");
            }
        }

        public ObservableCollection<Directory> Nodes
        {
            get { return _nodes; }
            set
            {
                _nodes = value;
                OnPropertyChanged("Nodes");
            }
        }

        #endregion
    }
}
