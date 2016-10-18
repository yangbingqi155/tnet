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
    public class MyOrderPressViewModel:MyOrderPress
    {
        /// <summary>
        /// 状态编号
        /// </summary>
        [Display(Name ="状态编号")]
        [StringLength(60)]
        public string idpress { get; set; }
        
        /// <summary>
        /// 订单号
        /// </summary>
        [StringLength(60)]
        [Display(Name = "订单号")]
        public string orderno { get; set; }

        /// <summary>
        /// 状态代码
        /// </summary>
        [Display(Name = "状态代码")]
        public int status { get; set; }

        [Display(Name = "状态描述")]
        [StringLength(60)]
        public string statust { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? cretime { get; set; }

        [Display(Name = "操作者")]
        [StringLength(50)]
        public string oper { get; set; }

        [Display(Name = "启用")]
        public bool? inuse { get; set; }


        public void CopyFromBase(MyOrderPress orderPress)
        {
            this.idpress = orderPress.idpress;
            this.orderno = orderPress.orderno;
            this.status = orderPress.status;
            this.statust = orderPress.statust;
            this.cretime = orderPress.cretime;
            this.oper = orderPress.oper;
            this.inuse = orderPress.inuse;
        }

        public void CopyToBase(MyOrderPress orderPress)
        {
            orderPress.idpress = this.idpress;
            orderPress.orderno = this.orderno;
            orderPress.status = this.status;
            orderPress.statust = this.statust;
            orderPress.cretime = this.cretime;
            orderPress.oper = this.oper;
            orderPress.inuse = this.inuse;
        }
    }
}