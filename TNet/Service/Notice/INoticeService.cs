using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TNet.Models.Notice;
using TNet.Models.Service.Com;

namespace TNet.Service.Notice
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“INoticeService”。
    [ServiceContract]
    public interface INoticeService
    {

        //POST
        //注意:请求时必须指定请求头为以下类型，不然会报错
        //Accept: application/json
        //Content-Type: application/json

        /**************************************************/
        /// <summary>
        /// 所有公告列表
        /// </summary>
        /// <returns></returns>
        [WebInvoke(Method = "GET", UriTemplate = "List/{city}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<List<NoticeBaseItem>> GetList(string city);

        /// <summary>
        /// 获取指定公告明细
        /// </summary>
        /// <returns></returns>
        [WebInvoke(Method = "GET", UriTemplate = "Detail/{idnotice}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<NoticeDetail> GetDetail(string idNotice);
    }
}
