using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCom.EF;

namespace TNet.BLL
{
    public class TaskRecverService
    {
        public static List<TaskRecver> GetALL()
        {
            TN db = new TN();
            return db.TaskRecvers.OrderByDescending(en => en.cretime).ToList();
        }

        public static TaskRecver Get(string idrecver)
        {
            return GetALL().Where(en => en.idrecver == idrecver).FirstOrDefault();
        }

        public static TaskRecver Edit(TaskRecver taskRecver)
        {
            TN db = new TN();
            TaskRecver oldTaskRecver = db.TaskRecvers.Where(en => en.idtask == taskRecver.idtask).FirstOrDefault();

            oldTaskRecver.idrecver = taskRecver.idrecver;
            oldTaskRecver.idtask = taskRecver.idtask;
            oldTaskRecver.mcode = taskRecver.mcode;
            oldTaskRecver.mname = taskRecver.mname;
            oldTaskRecver.smcode = taskRecver.smcode;
            oldTaskRecver.smname = taskRecver.smname;
            oldTaskRecver.inuse = taskRecver.inuse;
           
            db.SaveChanges();
            return oldTaskRecver;
        }

        public static TaskRecver Add(TaskRecver taskRecver)
        {
            TN db = new TN();
            db.TaskRecvers.Add(taskRecver);
            db.SaveChanges();
            return taskRecver;
        }
        public static List<TaskRecver> AddMuil(List<TaskRecver> taskRecvers)
        {
            TN db = new TN();
            db.TaskRecvers.AddRange(taskRecvers);
            db.SaveChanges();
            return taskRecvers;
        }

    }
}