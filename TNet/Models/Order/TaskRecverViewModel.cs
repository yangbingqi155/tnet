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
    public class TaskRecverViewModel:TaskRecver
    {
        [Display(Name = "接受编号")]
        [StringLength(60)]
        public new string idrecver { get; set; }

        [Display(Name = "工单编号")]
        [Required]
        [StringLength(60)]
        public new string idtask { get; set; }

        [Display(Name = "操作员编号")]
        [StringLength(60)]
        public new string mcode { get; set; }

        [Display(Name = "操作员名称")]
        [StringLength(60)]
        public new string mname { get; set; }

        [Display(Name = "创建时间")]
        public new DateTime? cretime { get; set; }

        [Display(Name = "发送者编号")]
        [StringLength(60)]
        public new string smcode { get; set; }

        [Display(Name = "发送者名称")]
        [StringLength(60)]
        public new string smname { get; set; }

        [Display(Name = "启用")]
        public new bool? inuse { get; set; }

        public void CopyFromBase(TaskRecver taskRecver)
        {
            this.idrecver = taskRecver.idrecver;
            this.idtask = taskRecver.idtask;
            this.mcode = taskRecver.mcode;
            this.mname = taskRecver.mname;
            this.cretime = taskRecver.cretime;
            this.smcode = taskRecver.smcode;
            this.smname = taskRecver.smname;
            this.inuse = taskRecver.inuse;
        }

        public void CopyToBase(TaskRecver taskRecver)
        {
            taskRecver.idrecver = this.idrecver;
            taskRecver.idtask = this.idtask;
            taskRecver.mcode = this.mcode;
            taskRecver.mname = this.mname;
            taskRecver.cretime = this.cretime;
            taskRecver.smcode = this.smcode;
            taskRecver.smname = this.smname;
            taskRecver.inuse = this.inuse;
        }
    }
}