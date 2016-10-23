using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Config 的摘要说明
/// </summary>
public static class Config
{
    private static JObject BuildItems()
    {
        var json = File.ReadAllText(HttpContext.Current.Server.MapPath("~/ueditor/net/config.json"));
        JObject j = JObject.Parse(json);
        getPath("imageUrlPrefix", j);
        //getPath("imagePathFormat", j);
        return j;
    }

    public static void getPath(string key, JObject j)
    {
        string app = HttpContext.Current.Request.ApplicationPath;
        if (app[app.Length - 1] == '/')
        {
            app = app.Substring(0, app.Length - 1);
        }
        j[key] = app + j[key];




    }

    public static JObject Items
    {
        get
        {
            if (_Items == null)
            {
                _Items = BuildItems();
            }
            return _Items;
        }
    }
    private static JObject _Items;


    public static T GetValue<T>(string key)
    {
        return Items[key].Value<T>();
    }

    public static String[] GetStringList(string key)
    {
        return Items[key].Select(x => x.Value<String>()).ToArray();
    }

    public static String GetString(string key)
    {
        return GetValue<String>(key);
    }

    public static int GetInt(string key)
    {
        return GetValue<int>(key);
    }
}