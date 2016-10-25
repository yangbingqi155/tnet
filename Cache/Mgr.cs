using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public sealed class Mgr
    {
        public static T Get<T>(string key)
        {
            T d = (T)System.Web.HttpContext.Current.Application[key];
            return d;
        }

        public static bool Clear(string key)
        {
            System.Web.HttpContext.Current.Application.Remove(key);
            return true;
        }

        public static bool Save<T>(string key, T data)
        {
            System.Web.HttpContext.Current.Application[key] = data;
            return true;
        }
    }
}
