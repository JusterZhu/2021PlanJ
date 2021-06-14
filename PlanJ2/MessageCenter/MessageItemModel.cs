using Juster.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCenter
{
    /// <summary>
    /// 消息处理结果
    /// </summary>
    public enum SysMessageResult
    {
        /// <summary>
        /// 未处理
        /// </summary>
        Unhandled = 0,
        /// <summary>
        /// 同意
        /// </summary>
        Processed = 1
    }

    /// <summary>
    /// 消息类型
    /// </summary>
    public enum SysMessageType
    {
        /// <summary>
        /// 通知类型
        /// </summary>
        NoticeType = 0,
        /// <summary>
        /// 其他类型
        /// </summary>
        OtherType = 1
    }

    public class MessageItemModel : BindableBase
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public string CreateTime { get; set; }

        public SysMessageType SystemMessageType { get; set; }

        public bool NoticeType { get; set; }

        public bool InvitationType { get; set; }

        public dynamic Parameter { get; set; }

        private bool _myCheckBoxState;
        public bool CheckBoxState
        {
            get { return _myCheckBoxState; }
            set
            {
                if (_myCheckBoxState == value) return;
                _myCheckBoxState = value;
                OnPropertyChanged("CheckBoxState");
            }
        }
    }
}
