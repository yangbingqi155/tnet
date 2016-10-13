using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCom.EF;

namespace TNet.BLL
{
    public class TaskService
    {
        public static List<Task> GetALL()
        {
            TN db = new TN();
            return db.Tasks.OrderByDescending(en => en.cretime).ToList();
        }

        public static Task Get(string idtask)
        {
            return GetALL().Where(en => en.idtask == idtask).FirstOrDefault();
        }

        public static Task Edit(Task task)
        {
            TN db = new TN();
            Task oldTask= db.Tasks.Where(en => en.idtask == task.idtask).FirstOrDefault();

            oldTask.idtask = task.idtask;
            oldTask.orderno = task.orderno;
            oldTask.idsend = task.idsend;
            oldTask.send = task.send;
            oldTask.revctime = task.revctime;
            oldTask.dotime = task.dotime;
            oldTask.finishtime = task.finishtime;
            oldTask.echotime = task.echotime;
            oldTask.notes = task.notes;
            oldTask.inuse = task.inuse;

            db.SaveChanges();
            return oldTask;
        }

        public static Task Add(Task task)
        {
            TN db = new TN();
            db.Tasks.Add(task);
            db.SaveChanges();
            return task;
        }
    }
}