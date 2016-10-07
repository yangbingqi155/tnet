using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
using System.Web; 
using System.Xml; 

namespace TCom.Com
{
    public static class P
    {
        static long t = DateTime.Now.Ticks / 10000;
        volatile static int un = 0;
        //static int tid = Thread.CurrentThread.ManagedThreadId;
        // private volatile static object lk = new object();
        //id 生成器,CAS版本
        public static long ID()
        {
            //lock (lk)
            {
                long _t = DateTime.Now.Ticks / 10000;
                if (t == _t)
                {
                    //Interlocked.CompareExchange(ref t, _t, _t);
                    Interlocked.Increment(ref un);
                }
                else
                {
                    Interlocked.Exchange(ref t, _t);
                    Interlocked.Exchange(ref un, 0);
                }

                return long.Parse(t + "" + un);
            }
        }
         
    }
}