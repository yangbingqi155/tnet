using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TCom.EF;
using TNet.Models.Notice;
using TNet.Models.Service.Com;

namespace TNet.Service.Notice
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“NoticeService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 NoticeService.svc 或 NoticeService.svc.cs，然后开始调试。
    public class NoticeService : INoticeService
    {
        public Result<List<NoticeBaseItem>> GetList()
        {
            Result<List<NoticeBaseItem>> result = new Result<List<NoticeBaseItem>>();
            try
            {
                using (TN db = new TN())
                {
                    DateTime s = DateTime.Now;// DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                   
                    result.Data = (from n in db.Notices
                                   where (n.start_time <= s && n.end_time >= s)
                                   select new NoticeBaseItem()
                                   {
                                       idnotice = n.idnotice,
                                       title = n.title,
                                       publish_time = n.publish_time

                                   }).ToList();

                    result.Code = R.Ok;
                }
            }
            catch (Exception)
            {
                result.Code = R.Error;
            }
            return result;


        }

        public Result<NoticeDetail> GetDetail(string idNotice)
        {
            Result<NoticeDetail> result = new Result<NoticeDetail>();
            try
            {
                if (!string.IsNullOrWhiteSpace(idNotice))
                {

                    using (TN db = new TN())
                    {
                        result.Data = (from n in db.Notices
                                       where (n.idnotice == idNotice)
                                       select new NoticeDetail()
                                       {
                                           idnotice = n.idnotice,
                                           title = n.title,
                                           publish_time = n.publish_time,
                                           content = n.content

                                       }).FirstOrDefault();

                        result.Code = R.Ok;
                    }
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
