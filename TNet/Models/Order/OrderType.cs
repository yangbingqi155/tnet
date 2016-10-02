using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TNet.Util;

namespace TNet.Models.Order
{
    /// <summary>
    /// 订单类型
    /// </summary>
    public class OrderType
    {
        /// <summary>
        /// 商品订单
        /// </summary>
        public static int Merc = 1;


        /// <summary>
        /// 报装业务
        /// </summary>
        public static int Setup = 2;
        

        public static List<SelectItemViewModel<int>> GetSelectItems()
        {
            List<SelectItemViewModel<int>> list = new List<SelectItemViewModel<int>>();

            SelectItemViewModel<int> model = new SelectItemViewModel<int>();
            model.DisplayValue = 0;
            model.DisplayText = "所有订单类型";
            list.Add(model);

            SelectItemViewModel<int> model1 = new SelectItemViewModel<int>();
                model1.DisplayValue = Merc;
            model1.DisplayText = "商品订单";
                list.Add(model1);

            SelectItemViewModel<int> model2 = new SelectItemViewModel<int>();
            model2.DisplayValue = Setup;
            model2.DisplayText = "报装业务";
            list.Add(model2);

            return list;
        }
    }
}