using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models
{
    /// <summary>
    /// 下拉框实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SelectItemViewModel<T>
    {
        /// <summary>
        /// 下拉值
        /// </summary>
        public T DisplayValue { get; set; }

        /// <summary>
        /// 下拉显示文字
        /// </summary>
        public string DisplayText { get; set; }
    }
}