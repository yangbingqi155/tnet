using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TCom.EF;

namespace TNet.Models
{
    [NotMapped]
    public class TaskViewModel:Task
    {
        [Display(Name = "工单编号")]
        [StringLength(60)]
        public new string idtask { get; set; }

        [Display(Name = "订单编号")]
        [StringLength(60)]
        public new string orderno { get; set; }

        [Display(Name = "创建时间")]
        public new DateTime? cretime { get; set; }

        [Display(Name = "发送者编号")]
        [StringLength(60)]
        public new string idsend { get; set; }

        [Display(Name = "发送者")]
        [StringLength(60)]
        public new string send { get; set; }

        [Display(Name = "接受时间")]
        public new DateTime? revctime { get; set; }

        [Display(Name = "开始工作")]
        public new DateTime? dotime { get; set; }

        [Display(Name = "完成工作")]
        public new DateTime? finishtime { get; set; }

        [Display(Name = "回复时间")]
        public new DateTime? echotime { get; set; }

        [Display(Name = "备注")]
        [StringLength(50)]
        public new string notes { get; set; }

        [Display(Name = "启用")]
        public new bool? inuse { get; set; }

        public void CopyFromBase(Task task)
        {
            this.idtask = task.idtask;
            this.orderno = task.orderno;
            this.cretime = task.cretime;
            this.idsend = task.idsend;
            this.send = task.send;
            this.revctime = task.revctime;
            this.dotime = task.dotime;
            this.finishtime = task.finishtime;
            this.echotime = task.echotime;
            this.notes = task.notes;
            this.inuse = task.inuse;
        }

        public void CopyToBase(Task task)
        {
            task.idtask = this.idtask;
            task.orderno = this.orderno;
            task.cretime = this.cretime;
            task.idsend = this.idsend;
            task.send = this.send;
            task.revctime = this.revctime;
            task.dotime = this.dotime;
            task.finishtime = this.finishtime;
            task.echotime = this.echotime;
            task.notes = this.notes;
            task.inuse = this.inuse;
        }
    }
}