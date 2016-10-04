using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.Net;
using System.Web.Security;
using Util;
using Newtonsoft.Json.Linq;

namespace WxPayAPI
{
    public sealed class JsApiPay
    {



        /**
         * 调用统一下单，获得下单结果
         * @return 统一下单结果
         * @失败时抛异常WxPayException
         */
        public static string PayOrder(TCom.EF.MyOrder order, TCom.EF.User user)
        {
            try
            {
                //统一下单
                WxPayData data = new WxPayData();
                data.SetValue("body", order.merc + "-" + (order.otype == 1 ? "宽带" : "报装"));
                data.SetValue("attach", order.spec);
                data.SetValue("out_trade_no", order.orderno + "");
                double total_fee = order.totalfee != null ? order.totalfee.Value : 0;
                 
                data.SetValue("total_fee", (int)(total_fee * 100));
                data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
                data.SetValue("time_expire", DateTime.Now.AddDays(1).ToString("yyyyMMddHHmmss"));
                data.SetValue("goods_tag", "test");
                data.SetValue("trade_type", "JSAPI");
                data.SetValue("openid", user.idweixin);
                WxPayData result = WxPayApi.UnifiedOrder(data);
                if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
                {
                    return null;
                }
                return GetJsOrder(result);
            }
            catch (Exception)
            {
            }
            return null;
        }

        /**
        *  
        * 从统一下单成功返回的数据中获取微信浏览器调起jsapi支付所需的参数，
        * 微信浏览器调起JSAPI时的输入参数格式如下：
        * {
        *   "appId" : "wx2421b1c4370ec43b",     //公众号名称，由商户传入     
        *   "timeStamp":" 1395712654",         //时间戳，自1970年以来的秒数     
        *   "nonceStr" : "e61463f8efa94090b1f366cccfbbb444", //随机串     
        *   "package" : "prepay_id=u802345jgfjsdfgsdg888",     
        *   "signType" : "MD5",         //微信签名方式:    
        *   "paySign" : "70EA570631E4BB79628FBCA90534C63FF7FADD89" //微信签名 
        * }
        * @return string 微信浏览器调起JSAPI时的输入参数，json格式可以直接做参数用
        * 更详细的说明请参考网页端调起支付API：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7
        * 
        */
        private static string GetJsOrder(WxPayData data)
        {
            try
            {
                WxPayData jsApiParam = new WxPayData();
                jsApiParam.SetValue("appId", data.GetValue("appid"));
                jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
                jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
                jsApiParam.SetValue("package", "prepay_id=" + data.GetValue("prepay_id"));
                jsApiParam.SetValue("signType", "MD5");
                jsApiParam.SetValue("paySign", jsApiParam.MakeSign());

                return jsApiParam.ToJson();
            }
            catch (Exception)
            {

            }
            return null;

        }

        

    }
}