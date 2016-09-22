using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Hosting;
using TNet.Models.File;
using TNet.Models.Service.Com;
using Util;

namespace TNet.Service.File
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“FileService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 FileService.svc 或 FileService.svc.cs，然后开始调试。
    public class FileService : IFileService
    {
        public Result<UploadDataResult> Upload(UploadData data)
        {
            Result<UploadDataResult> result = new Result<UploadDataResult>();
            try
            {
                if (data != null && !string.IsNullOrWhiteSpace(data.data))
                {
                    string ffn = "data:image/";
                    ///data:image/jpeg;base64,
                    int li = data.data.IndexOf(';');
                    string lfn = data.data.Substring(ffn.Length, li - ffn.Length);
                    if (string.IsNullOrWhiteSpace(lfn))
                    {
                        lfn = "jpeg";
                    }
                    li = data.data.IndexOf(',');
                    data.data = data.data.Substring(++li);
                    byte[] fData = Pub.DeBase64ToBytes(data.data);
                    if (fData != null && fData.Length > 0)
                    {
                        string fp = HostingEnvironment.MapPath("~/Resource/Images/r");
                        if (!Directory.Exists(fp))
                        {
                            Directory.CreateDirectory(fp);
                        }
                        string f = "/"+Pub.ID() + "." + lfn;
                        fp += f;
                        f = "Resource/Images/r" + Pub.ID() + "." + lfn;
                        FileStream imgFile = System.IO.File.Open(fp, FileMode.OpenOrCreate);
                        imgFile.Write(fData, 0, fData.Length);
                        imgFile.Flush();
                        imgFile.Close();
                        fData = null;
                        result.Data = new UploadDataResult();
                        result.Data.name = f;
                        result.Code = R.Ok;
                    }
                }
            }
            catch (Exception)
            {
                result.Msg = "保存文件失败";
            }

            return result;
        }
    }
}
