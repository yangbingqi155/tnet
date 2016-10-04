using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using TCom.EF;
using System.Data.Entity;
using System.Linq.Expressions;

namespace TNet.BLL
{
    /// <summary>
    /// 增长列服务类
    /// </summary>
    public class IdentifyService
    {
        /// <summary>
        /// 获取表中的最大的主键值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static int GetMaxIdentifyID<T>(Expression<Func<T, int>> expression) where T:class {
            TN db = new TN();
            Type type = typeof(T);
            string efModelTypeName = type.Name;
            string propertyName = string.Empty;

            Type dbType = typeof(TN);
            if (efModelTypeName == "Business")
            {
                propertyName = string.Format("{0}es", efModelTypeName);
            }
            else
            {
                propertyName = string.Format("{0}s", efModelTypeName);
            }
            DbSet<T> efModelList= (DbSet<T>)dbType.GetProperty(propertyName).GetValue(db);

            if (efModelList==null|| efModelList.Count()==0) {
                return 999;
            }
            return efModelList.Max(expression);
        }
    }
}