using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Util
{
    public static class ListExtension
    {
        public static List<T> Pager<T>(this List<T> list, int pageIndex, int pageSize, out int pageCount) {
            pageCount = 1;
            if (pageSize == 0) {
                throw new Exception("每页记录条数不能为0.");
            }

            if (list.Count == 0) {
                return list;
            }

           
            int rest = list.Count % pageSize;
            int times = list.Count / pageSize;
            pageCount = (rest != 0) ? (times + 1) : times;

            if (pageCount<pageIndex+1) {
                throw new Exception("当前分页索引超过总页数.");
            }

            List<T> entities = new List<T>();
            if (rest == 0||(rest != 0 && pageCount != (pageIndex + 1))) {
                entities = list.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            else if (rest != 0 && pageCount == (pageIndex + 1)) {
                entities = list.Skip(pageIndex * pageSize).Take(rest).ToList();
            }

            return entities;
        }
    }
}