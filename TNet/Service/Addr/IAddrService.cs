using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TNet.Models.Service.Com;

namespace TNet.Service.Addr
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IAddrService”。
    [ServiceContract]
    public interface IAddrService
    {//POST
        //注意:请求时必须指定请求头为以下类型，不然会报错
        //Accept: application/json
        //Content-Type: application/json

        /**************************************************/
        /// <summary>
        /// 获取某人的地址
        /// </summary>
        /// <returns></returns>
        [WebInvoke(Method = "GET", UriTemplate = "All/{iduser}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<List<TCom.EF.MyAddr>> GetAll(string iduser);

        /// <summary>
        /// 更新地址
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [WebInvoke(Method = "POST", UriTemplate = "Update", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]

        Result<long> Update(TCom.EF.MyAddr data);

    }
}
