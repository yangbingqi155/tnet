using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCom.EF;

namespace TNet.BLL
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