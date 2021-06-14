using Juster.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MessageCenter
{
    public class MessageViewModel : BindableBase
    {

        #region Attribute

        public ObservableCollection<MessageItemModel> MessageItem { get; set; }

        private bool _myCheckBoxValue;
        public bool CheckBoxValue
        {
            get { return _myCheckBoxValue; }
            set
            {
                if (_myCheckBoxValue == value) return;
                _myCheckBoxValue = value;
                OnPropertyChanged("CheckBoxValue");
            }
        }

        private bool _myViewVisibility;
        public bool ViewVisibility
        {
            get { return _myViewVisibility; }
            set
            {
                if (_myViewVisibility == value) return;
                _myViewVisibility = value;
                OnPropertyChanged("ViewVisibility");
            }
        }

        private MessageItemModel _mySelectItemModel;
        public MessageItemModel SelectItemModel
        {
            get { return _mySelectItemModel; }
            set
            {
                if (_mySelectItemModel == value) return;
                _mySelectItemModel = value;
                OnPropertyChanged("SelectItemModel");
            }
        }

        private DateTime _lasTime;

        #endregion

        #region  ICommand
        int i = 0;
        private ICommand _addCommand;

        public ICommand AddCommand 
        {
            get 
            {
                return _addCommand ?? (_addCommand = new RelayCommand(()=> 
                {
                    MessageItem.Add(new MessageItemModel { CreateTime = DateTime.Now.ToString(), Content = "重大新闻，刚刚点击按钮的男子买彩票获得了百万大奖。", Name = "News", Id = i++, InvitationType = true });
                    UpdateDefault();
                }));
            }
        }

        private ICommand _clickAcceptCommand;

        public ICommand ClickAcceptCommand
        {
            get
            {
                return _clickAcceptCommand ?? (_clickAcceptCommand = new RelayCommand<MessageItemModel>((e) => {
                    var request = MessageItem.FirstOrDefault(p => p.Id == e.Id);
                    if (request != null)
                    {
                        MessageItem.Remove(request);
                    }
                    UpdateDefault();
                }));
            }
        }

        private ICommand _clickRefuseCommand;

        public ICommand ClickRefuseCommand
        {
            get
            {
                return _clickRefuseCommand ?? (_clickRefuseCommand = new RelayCommand<MessageItemModel>((e) => {
                    var request = MessageItem.FirstOrDefault(p => p.Id == e.Id);
                    if (request != null)
                    {
                        MessageItem.Remove(request);
                    }
                    UpdateDefault();
                }));
            }
        }

        private ICommand _clickAgreeCommand;

        public ICommand ClickAgreeCommand
        {
            get
            {
                return _clickAgreeCommand ?? (_clickAgreeCommand = new RelayCommand<MessageItemModel>((e) => {
                    var request = MessageItem.FirstOrDefault(p => p.Id == e.Id);
                    if (request != null)
                    {
                        MessageItem.Remove(request);
                    }
                    UpdateDefault();
                }));
            }
        }

        private ICommand _clickDeleteCommand;

        public ICommand ClickDeleteCommand
        {
            get
            {
                return _clickDeleteCommand ?? (_clickDeleteCommand = new RelayCommand<long>((e) => {
                    var request = MessageItem.FirstOrDefault(p => p.Id == e);
                    if (request != null)
                    {
                        MessageItem.Remove(request);
                    }
                    UpdateDefault();
                }));
            }
        }
        
        private ICommand _clickBatchDeleteCommand;

        public ICommand ClickBatchDeleteCommand
        {
            get
            {
                return _clickBatchDeleteCommand ?? (_clickBatchDeleteCommand = new RelayCommand(() => {
                    for (var i = 0; i < MessageItem.Count; i++)
                    {
                        if (!MessageItem[i].CheckBoxState)
                            continue;
                        MessageItem.Remove(MessageItem[i]);
                        i--;
                    }
                    CheckBoxValue = false;
                    UpdateDefault();
                }));
            }
        }

        private ICommand _clickCheckBoxCommand;

        public ICommand ClickCheckBoxCommand
        {
            get
            {
                return _clickCheckBoxCommand ?? (_clickCheckBoxCommand = new RelayCommand(() => {
                    for (var i = 0; i < MessageItem.Count; i++)
                    {
                        MessageItem[i].CheckBoxState = CheckBoxValue;
                    }
                }));
            }
        }

        #endregion


        public MessageViewModel()
        {
            MessageItem = new ObservableCollection<MessageItemModel>();
            ViewVisibility = false;
            CheckBoxValue = false;
        }

        #region Methods

        private void ClearMessageByName(string messageName)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                for (var i = 0; i < MessageItem.Count; i++)
                {
                    if (!MessageItem[i].Name.Equals(messageName))
                        continue;
                    MessageItem.Remove(MessageItem[i]);
                    i--;
                }
                UpdateDefault();
            });
        }

        private void UpdateDefault()
        {
            if (MessageItem != null && MessageItem.Count > 0)
            {
                SelectItemModel = MessageItem[0];
            }
            if (MessageItem == null)
                return;
            ViewVisibility = MessageItem.Count > 0;
        }

        #endregion
    }
}
