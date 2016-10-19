using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TCom.EF;

namespace TNet.Models {
    [NotMapped]
    public class NoticeViewModel:TCom.EF.Notice {
        [Required]
        [Display(Name = "通知编号")]
        [StringLength(100)]
        public new string idnotice { get; set; }

        [Required]
        [Display(Name = "发布者")]
        [StringLength(50)]
        public new string publish { get; set; }

        [Required]
        [Display(Name = "标题")]
        [StringLength(100)]
        public new string title { get; set; }

        [Required]
        [Display(Name = "发布时间")]
        public new DateTime? publish_time { get; set; }

        [Required]
        [Display(Name = "开始时间")]
        public new DateTime? start_time { get; set; }

        [Required]
        [Display(Name = "结束时间")]
        public new DateTime? end_time { get; set; }

        [Required]
        [Display(Name = "内容")]
        public new string content { get; set; }
        
        public void CopyFromBase(TCom.EF.Notice notice) {
            this.idnotice = notice.idnotice;
            this.publish = notice.publish;
            this.title = notice.title;
            this.publish_time = notice.publish_time;
            this.start_time = notice.start_time;
            this.end_time = notice.end_time;
            this.content = notice.content;
        }

        public void CopyToBase(TCom.EF.Notice notice) {
            notice.idnotice = this.idnotice;
            notice.publish = this.publish;
            notice.title = this.title;
            notice.publish_time = this.publish_time;
            notice.start_time = this.start_time;
            notice.end_time = this.end_time;
            notice.content = this.content;
        }

    }
}