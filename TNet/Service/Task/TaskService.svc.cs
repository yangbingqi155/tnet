using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using TCom.EF;
using TNet.Models.Service.Com;
using TNet.Models.Task;

namespace TNet.Service.Task
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“TaskService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 TaskService.svc 或 TaskService.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirementsAttribute(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]

    public class TaskService : ITaskService
    {
        public Result<List<TaskItem>> List(string mgcode)
        {

            Result<List<TaskItem>> result = new Result<List<TaskItem>>();
            try
            {
                if (!string.IsNullOrWhiteSpace(mgcode))
                {

                    using (TN db = new TN())
                    {
                        List<TCom.EF.Task> ts = (from t in db.Tasks
                                                 where ((from tr in db.TaskRecvers
                                                         where tr.mcode == mgcode
                                                         select tr.idtask).Contains(t.idtask))
                                                 select t).ToList();
                        result.Data = TaskItem.gets(ts);

                        result.Code = R.Ok;
                    }
                }
            }
            catch (Exception e)
            {
                result.Code = R.Error;
                result.Msg = "出现异常"+e.Message;
            }
            return result;
        }
    }
}
