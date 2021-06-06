using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsnycLoad.Models
{
    public class DataPages<T> where T : class, new()
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        //每页大小
        public int PageSize { get; set; }

        /// <summary>
        /// 集合数据
        /// </summary>
        public IList<T> Records { get; set; } = new List<T>();

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecords { get; set; }

        public DataPages()
        {
            this.PageIndex = 1;
            this.PageSize = 10;
        }

        //public T CacheData() 
        //{
        //    if (PageIndex)
        //    {

        //    }
        //    return default;
        //}
    }
}
