using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using TCom.EF;
using TNet.Models.Service.Com;

namespace TNet.Service.City
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“CityService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 CityService.svc 或 CityService.svc.cs，然后开始调试。

    [AspNetCompatibilityRequirementsAttribute(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]

    public class CityService : ICityService
    {
        public Result<List<TCom.EF.City>> GetCity()
        {
            Result<List<TCom.EF.City>> result = new Result<List<TCom.EF.City>>();
            result.Data = Cache.Mgr.Get<List<TCom.EF.City>>(Cache.Key.City);
            if(result.Data == null)
            {
                try
                { 
                    using (TN db = new TN())
                    {
                        result.Data = db.Cities.Where(mr => mr.inuse == true).OrderByDescending(m=>m.isdefault).OrderByDescending(m => m.sortno).ToList();
                        result.Code = R.Ok;
                    }
                }
                catch (Exception)
                {
                    result.Code = R.Error;
                    result.Msg = "出现异常";
                }
            }else
            {
                result.Code = R.Ok;
            }
            return result;

        }
    }
}
