using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TNet.Models.File;
using TNet.Models.Service.Com;

namespace TNet.Service.File
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IFileService”。
    [ServiceContract]
    public interface IFileService
    {
        //POST
        //注意:请求时必须指定请求头为以下类型，不然会报错
        //Accept: application/json
        //Content-Type: application/json

        /**************************************************/
        [WebInvoke(Method = "POST", UriTemplate = "Upload", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<UploadDataResult> Upload(UploadData data);
    }
}