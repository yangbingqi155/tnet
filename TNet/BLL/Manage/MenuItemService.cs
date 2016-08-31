using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TNet.EF;

namespace TNet.Models.Service
{
    /// <summary>
    /// 菜单项
    /// </summary>
    public class MenuItemService
    {
        public List<MenuItem> GetALL() {
            TN db = new TN();
            return db.MenuItems.ToList();
        }
    }
}