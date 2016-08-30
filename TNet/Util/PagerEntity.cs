using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Util
{
    /// <summary>
    /// 分页实体
    /// </summary>
    public class PagerEntity
    {
        /// <summary>
        /// 每页记录条数
        /// </summary>
        public int pageSize { get; set; }
        
        /// <summary>
        /// 总页数
        /// </summary>
        public int pageCount { get; set; }

        /// <summary>
        /// 当前页码数(页码数从0开始)
        /// </summary>
        public int pageIndex { get; set; }
    }
}