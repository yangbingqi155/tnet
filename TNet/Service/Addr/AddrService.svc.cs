using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using TCom.EF;
using TNet.Models.Service.Com;
using Util;

namespace TNet.Service.Addr
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“AddrService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 AddrService.svc 或 AddrService.svc.cs，然后开始调试。

    [AspNetCompatibilityRequirementsAttribute(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AddrService : IAddrService
    {
        public Result<List<TCom.EF.MyAddr>> GetAll(string iduser)
        {
            Result<List<TCom.EF.MyAddr>> result = new Result<List<TCom.EF.MyAddr>>();
            try
            {
                long _iduser = long.Parse(iduser);
                using (TN db = new TN())
                {
                    result.Data = db.MyAddrs.Where(mr => mr.iduser == _iduser).OrderByDescending(m=>m.idaddr).ToList();
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



        public Result<long> Update(TCom.EF.MyAddr data)
        {
            Result<long> result = new Result<long>();

            try
            {
                using (TN db = new TN())
                {
                    if (data.idaddr <= 0)
                    {
                        data.idaddr = Pub.ID();
                        db.MyAddrs.Add(data);
                    }
                    else
                    {
                        DbEntityEntry<TCom.EF.MyAddr> entry = db.Entry<TCom.EF.MyAddr>(data);
                        if (data.inuse == false)
                        {
                            entry.State = EntityState.Deleted;
                        }
                        else
                        {
                            entry.State = EntityState.Unchanged;
                            entry.Property("contact").IsModified = true;
                            entry.Property("phone").IsModified = true;
                            entry.Property("province").IsModified = true;
                            entry.Property("city").IsModified = true;
                            entry.Property("district").IsModified = true;
                            entry.Property("street").IsModified = true;
                            entry.Property("tag").IsModified = true;
                            entry.Property("isdv").IsModified = true;
                            entry.Property("notes").IsModified = true;
                            entry.Property("inuse").IsModified = true;
                        }
                    }
                    if (db.SaveChanges() > 0)
                    {
                        result.Data = data.idaddr;
                    }
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
    }
}
