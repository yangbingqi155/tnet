using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TNet.Models.Service.Com;
using TNet.Models.Service.Merc;

namespace TNet.Service.Merc
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IMercService”。
    [ServiceContract]
    public interface IMercService
    {
        //POST
        //注意:请求时必须指定请求头为以下类型，不然会报错
        //Accept: application/json
        //Content-Type: application/json

        /**************************************************/
        /// <summary>
        /// 所有商品信息,全部信息
        /// </summary>
        /// <returns></returns>
        [WebInvoke(Method = "GET", UriTemplate = "All", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<MercInfo> GetAll();

        /// <summary>
        /// 所有商品信息,仅精简信息
        /// </summary>
        /// <returns></returns>
        [WebInvoke(Method = "GET", UriTemplate = "List", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<List<EF.Merc>> GetList();
    }
}
