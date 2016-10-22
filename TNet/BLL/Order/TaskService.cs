using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.SqlServer;
using TCom.EF;
using TNet.Models;

namespace TNet.BLL
{
    public class TaskService
    {
        public static List<Task> GetALL()
        {
            TN db = new TN();
            return db.Tasks.OrderByDescending(en => en.cretime).ToList();
        }

        private static IQueryable<Task> CommonSearch(DateTime? startDate, DateTime? endDate, string orderno = "", string idsend = "", string idrevc = "")
        {
            TN db = new TN();
            return (from ta in db.Tasks
                                   join ord in db.MyOrders on ta.orderno equals ord.orderno.ToString()
                                   join tre in db.TaskRecvers on ta.idtask equals tre.idtask
                                   where
                                    (!string.IsNullOrEmpty(orderno) && ord.orderno.ToString() == orderno)
                                    ||
                                    (
                                    string.IsNullOrEmpty(orderno)
                                    && (startDate == null || SqlFunctions.DateDiff("dd", startDate.Value, ta.cretime) >= 0)
                                    && (endDate == null || SqlFunctions.DateDiff("dd", endDate.Value, ta.cretime) <= 0)
                                    && (string.IsNullOrEmpty(idsend) || ta.idsend == idsend)
                                    && (string.IsNullOrEmpty(idrevc) || tre.mcode == idrevc)
                                    )
                                   orderby ta.cretime descending
                                   select ta);
          
        }


        private static List<Task> SearchList(DateTime? startDate, DateTime? endDate, string orderno = "", string idsend = "", string idrevc = "")
        {
            return CommonSearch(startDate, endDate, orderno, idsend, idrevc).ToList();;
        }

        public static List<TaskViewModel> Search(DateTime? startDate, DateTime? endDate, string orderno = "", string idsend = "", string idrevc = "")
        {
            IQueryable<Task> queryable= CommonSearch(startDate, endDate, orderno, idsend, idrevc);
            List<Task> entities = queryable.ToList();
            List<TaskViewModel> viewModels = new List<TaskViewModel>();
            viewModels = entities.Select(mod =>
            {
                TaskViewModel viewModel = new TaskViewModel();
                viewModel.CopyFromBase(mod);
                return viewModel;
            }).ToList();
            viewModels = viewModels.Distinct(new TaskViewModel()).ToList();

            TN db = new TN();
            List<TaskRecver> taskRecvers = db.TaskRecvers.ToList();
            List<TaskRecverViewModel> taskRecverViewModels = taskRecvers.Select(mod =>
            {
                TaskRecverViewModel viewModel = new TaskRecverViewModel();
                viewModel.CopyFromBase(mod);
                return viewModel;
            }).ToList();

            viewModels = viewModels.Select(mod =>
            {
                mod.TaskRecvers = taskRecverViewModels.Where(en => en.idtask == mod.idtask).ToList();
                return mod;
            }).ToList();

            return viewModels;
        }

        public static TaskViewModel GetViewModel(string idtask) {
            TN db = new TN();
            TaskViewModel viewModel = null;
            Task task= db.Tasks.Where(en=> en.idtask==idtask).FirstOrDefault();
            if (task!=null) {
                viewModel = new TaskViewModel();
                viewModel.CopyFromBase(task);
                List<TaskRecver> taskRecvers= db.TaskRecvers.Where(en => en.idtask == idtask).ToList();
                viewModel.TaskRecvers = taskRecvers.Select(mod=> {
                    TaskRecverViewModel model = new TaskRecverViewModel();
                    model.CopyFromBase(mod);
                    return model;
                }).ToList();
            }

            return viewModel;

        }

        public static Task Get(string idtask)
        {
            return GetALL().Where(en => en.idtask == idtask).FirstOrDefault();
        }

        public static Task Edit(Task task)
        {
            TN db = new TN();
            Task oldTask = db.Tasks.Where(en => en.idtask == task.idtask).FirstOrDefault();

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