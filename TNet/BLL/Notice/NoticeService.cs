using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCom.EF;

namespace TNet.BLL {
    public class NoticeService {

        public static List<Notice> GetALL() {
            TN db = new TN();
            return db.Notices.OrderBy(en=>en.publish_time).ToList();
        }

        public static Notice Get(string idnotice) {
            return GetALL().Where(en => en.idnotice == idnotice).FirstOrDefault();
        }

        public static Notice Edit(Notice notice) {
            TN db = new TN();
            Notice oldNotice = db.Notices.Where(en => en.idnotice == notice.idnotice).FirstOrDefault();

            oldNotice.idnotice = notice.idnotice;
            oldNotice.publish = notice.publish;
            oldNotice.title = notice.title;
            oldNotice.publish_time = notice.publish_time;
            oldNotice.start_time = notice.start_time;
            oldNotice.end_time = notice.end_time;
            oldNotice.content = notice.content;

            db.SaveChanges();
            return oldNotice;
        }

        public static Notice Add(Notice notice) {
            TN db = new TN();
            db.Notices.Add(notice);
            db.SaveChanges();
            return notice;
        }
    }
}