using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TCom.EF;
using TNet.Models.Service.Com;

namespace TNet.Service.Buss
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“BussService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 BussService.svc 或 BussService.svc.cs，然后开始调试。
    public class BussService : IBussService
    {
        public Result<List<TCom.EF.Business>> List()
        {
            Result<List<TCom.EF.Business>> result = new Result<List<TCom.EF.Business>>();
            try
            {
                using (TN db = new TN())
                {
                    result.Data = db.Businesses.Where(mr => mr.inuse == true).ToList();
                    result.Code = R.Ok;
                }
            }
            catch (Exception)
            {
                result.Msg = "获取周边商圈错误";
                result.Code = R.Error;
            }
            return result;
        }


        public Result<TCom.EF.Business> Detail(string idbuss)
        {
            Result<TCom.EF.Business> result = new Result<TCom.EF.Business>();
            try
            {
                if (!string.IsNullOrWhiteSpace(idbuss))
                {
                    long _idbuss = long.Parse(idbuss);
                    using (TN db = new TN())
                    {
                        result.Data = db.Businesses.Where(m => m.inuse == true && m.idbuss == _idbuss).FirstOrDefault();
                        result.Code = R.Ok;
                    }
                }
            }
            catch (Exception)
            {
                result.Msg = "获取商家错误";
                result.Code = R.Error;
            }
            return result;
        }
    }
}
