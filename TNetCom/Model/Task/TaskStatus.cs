using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCom.Model.Task
{
    public sealed class TaskStatus
    {
        /// <summary>
        /// 等待处理
        /// </summary>
        public readonly static int WaitPress = 1;

        /// <summary>
        /// 处理中...
        /// </summary>
        public readonly static int Pressing = 20;

        /// <summary>
        /// 暂停...
        /// </summary>
        public readonly static int Pause = 30;

        /// <summary>
        /// 继续处理
        /// </summary>
        public readonly static int RePressing = 40;

        /// <summary>
        /// 完成处理,等待验收
        /// </summary>
        public readonly static int DoFinish = 50;

        /// <summary>
        /// 验收完成
        /// </summary>
        public readonly static int Confirm = 60;

        /// <summary>
        /// 回访完毕
        /// </summary>
        public readonly static int Review = 70;

        /// <summary>
        /// 关闭
        /// </summary>
        public readonly static int Close = 80;

        /// <summary>
        /// 转移中...
        /// </summary>
        public readonly static int Transfering = 100;

        //public readonly static int ConfirmTransfer = 110;

        /// <summary>
        /// 工人取消
        /// </summary>
        public readonly static int WorkerCancel = 120;

        //public readonly static int ConfirmWorkerCancel = 130;
        /// <summary>
        /// 抢单中...
        /// </summary>
        public readonly static int Looting = 130;

        /// <summary>
        /// 超时未处理
        /// </summary>
        public readonly static int Timeout = 999;

        public static Dictionary<int, TaskStatusItem> s = new Dictionary<int, TaskStatusItem>()
        {
             {
                WaitPress,
                new TaskStatusItem()
                {
                    text = "等待处理",
                    ops = ""
                }
            },
            {
                Pressing,
                new TaskStatusItem()
                {
                    text = "处理中...",
                    ops = ""
                }
            },
            {
                Pause,
                new TaskStatusItem()
                {
                    text = "暂停...",
                    ops = ""
                }
            },
            {
                RePressing,
                new TaskStatusItem()
                {
                    text = "继续处理",
                    ops = ""
                }
            },
            {
                DoFinish,
                new TaskStatusItem()
                {
                    text = "完成处理,等待验收",
                    ops = ""
                }
            },
            {
                Confirm,
                new TaskStatusItem()
                {
                    text = "验收完成",
                    ops = ""
                }
            },
            {
                Review,
                new TaskStatusItem()
                {
                    text = "回访完毕",
                    ops = ""
                }
            },
            {
                Close,
                new TaskStatusItem()
                {
                    text = "关闭",
                    ops = ""
                }
            },
            {
                Transfering,
                new TaskStatusItem()
                {
                    text = "转移中...",
                    ops = ""
                }
            },
            {
                WorkerCancel,
                new TaskStatusItem()
                {
                    text = "工人取消...",
                    ops = ""
                }
            },
            {
                Looting,
                new TaskStatusItem()
                {
                    text = "抢单中...",
                    ops = ""
                }
            },
            {
                Timeout,
                new TaskStatusItem()
                {
                    text = "超时",
                    ops = ""
                }
            },
            {
                0,
                new TaskStatusItem()
                {
                    text = "未知",
                    ops = ""
                }
            }
        };


        public static Dictionary<int, TaskStatusItem> get()
        {
            return s;
        }

        public static TaskStatusItem get(int? status)
        {
            return status != null && s.ContainsKey(status.Value) ? s[status.Value] : s[0];
        }
    }
}
