using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using TCom.EF;
using TNet.Models.Merc;
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

        public Result<MercList> GetList()
        {
            Result<MercList> result = new Result<MercList>();
            try
            {
                using (TN db = new TN())
                {
                    result.Data = new MercList()
                    {
                        Mercs = db.Mercs.Where(mr => mr.inuse == true).OrderByDescending(m => m.idtype).ThenByDescending(m => m.sortno).ToList(),
                        Types = db.MercTypes.Where(m => m.inuse == true).OrderByDescending(m => m.sortno).ToList()

                    };
                    result.Code = R.Ok;
                }
            }
            catch (Exception)
            {
                result.Code = R.Error;
            }
            return result;

        }

        public Result<MercDataSingle> GetMercSingle(string idmerc)
        {
            Result<MercDataSingle> result = new Result<MercDataSingle>();
            try
            {
                if (!string.IsNullOrWhiteSpace(idmerc))
                {
                    int _idmerc = int.Parse(idmerc);
                    using (TN db = new TN())
                    {
                        MercDataSingle m = new MercDataSingle()
                        {
                            Merc = db.Mercs.First(mr => mr.inuse == true && mr.idmerc == _idmerc),
                            Spec = db.Specs.Where(mr => mr.inuse == true && mr.idmerc == _idmerc).ToList(),
                            Discount = db.Discounts.Where(mr => mr.inuse == true && mr.idmerc == _idmerc).ToList(),
                            Imgs = db.MercImages.Where(mr => mr.idmerc == _idmerc).ToList()
                        };
                        if (m.Merc != null)
                        {
                            m.Setups = db.Setups.Where(mr => mr.inuse == true && mr.idtype == m.Merc.idtype.ToString()).ToList();
                            m.SetupAddrs = db.SetupAddrs.Where(mr => mr.inuse == true && mr.idtype == m.Merc.idtype.ToString()).ToList();
                        }
                        result.Data = m;
                        result.Code = R.Ok;
                    }
                }
            }
            catch (Exception)
            {
                result.Code = R.Error;
                result.Msg = "出现异常";
            }
            return result;
        }



        public Result<SetupList> GetSetupList()
        {
            Result<SetupList> result = new Result<SetupList>();
            try
            {
                using (TN db = new TN())
                {
                    result.Data = new SetupList()
                    {
                        Mercs = db.Mercs.Where(m => m.inuse == true && m.isetup == true).OrderByDescending(m => m.idtype).ThenByDescending(m => m.sortno).ToList(),
                        Types = db.MercTypes.Where(m => m.inuse == true).OrderByDescending(m => m.sortno).ToList(),
                        Setups = db.Setups.Where(m => m.inuse == true).ToList(),
                        SetupAddrs = db.SetupAddrs.Where(m => m.inuse == true).ToList(),
                    };
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
