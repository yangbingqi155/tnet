using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace TNet.Models
{
    /// <summary>
    /// 接口返回通用实体
    /// </summary>
    public class ResultModel<T> 
    {
        /// <summary>
        /// 返回的数据内容
        /// </summary>
        public List<T> Content { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public ResponseCodeType Code { get; set; }

        /// <summary>
        /// 状态消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 记录总条数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 每页显示记录条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get
            {
                if (RecordCount == 0 || PageSize == 0) { return 0; }

                return RecordCount % PageSize == 0 ? (RecordCount / PageSize) : (RecordCount / PageSize) + 1;
            }
        }

        /// <summary>
        /// 把实体序列化成Json格式
        /// </summary>
        /// <returns></returns>
        public string SerializeToJson()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }
    }
}