using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TCom.Model.Task;

namespace TNet.Models.Task
{
    [NotMapped]
    public class TaskItem : TCom.EF.Task
    {
        public TaskStatusItem statusObj
        {
            get
            {
                return TaskStatus.get(this.status);
            }
            set
            {

            }
        }

        public TaskItem()
        {

        }

        public TaskItem(TCom.EF.Task data)
        {
            this.idtask = data.idtask;
            this.iduser = data.iduser;
            this.name = data.name;
            this.idsend = data.idtask;
            this.orderno = data.idtask;
            this.send = data.idtask;
            this.accpeptime = data.accpeptime;
            this.cretime = data.cretime;
            this.revctime = data.revctime;
            this.dotime = data.dotime;
            this.finishtime = data.finishtime;
            this.echotime = data.echotime;
            this.notes = data.notes;
            this.status = data.status;
            this.contact = data.contact;
            this.addr = data.addr;
            this.phone = data.phone;
            this.title = data.title;
            this.text = data.text;
            this.tasktype = data.tasktype;
            this.tasktype = data.tasktype;
            this.score = data.score;
            this.inuse = data.inuse;
        }


        public static List<TaskItem> gets(List<TCom.EF.Task> data)
        {
            if (data != null && data.Count > 0)
            {
                List<TaskItem> t = new List<TaskItem>(data.Count);
                for (int i = 0; i < data.Count; i++)
                {
                    t.Add(new TaskItem(data[i]));
                }
                return t;
            }
            return null;
        }
    }
}