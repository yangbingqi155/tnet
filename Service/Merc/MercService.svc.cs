using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using TNet.EF;
using TNet.Models.Service.Com;
using TNet.Models.Service.Merc;

namespace TNet.Service.Merc
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“MercService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 MercService.svc 或 MercService.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirementsAttribute(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]

    public class MercService : IMercService
    {
        public Result<MercInfo> GetAll()
        {
            Result<MercInfo> result = new Result<MercInfo>();
            try
            {
                using (TN db = new TN())
                {
                    MercInfo m = new MercInfo()
                    {
                        Merc = db.Mercs.Where(mr => mr.inuse == true).ToList(),
                        Spec = db.Specs.Where(mr => mr.inuse == true).ToList(),
                        Discount = db.Discounts.Where(mr => mr.inuse == true).ToList()
                    };
                    result.Data = m;
                    result.Code = R.Ok;
                }
            }
            catch (Exception)
            {
                result.Code = R.Error;
                result.Msg = "出现异常";
            }
            return result;
        }
    
        public Result<List<EF.Merc>> GetList()
        {
            Result<List<EF.Merc>> result = new Result<List<EF.Merc>>();
            try
            {
                using (TN db = new TN())
                {
                    result.Data = db.Mercs.Where(mr => mr.inuse == true).ToList();
                    result.Code = R.Ok;
                }
            }
            catch (Exception)
            {
                result.Code = R.Error;
            }
            return result;

        }
    }
}
