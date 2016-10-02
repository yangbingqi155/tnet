using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TNet.EF;
using TNet.Models.File;
using TNet.Models.Order;
using TNet.Models.Service.Com;

namespace TNet.Service.Order
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IOrderService”。
    [ServiceContract]
    public interface IOrderService
    {
        //POST
        //注意:请求时必须指定请求头为以下类型，不然会报错
        //Accept: application/json
        //Content-Type: application/json

        /**************************************************/
        /// <summary>
        /// 所有商品信息,全部信息
        /// </summary>
        /// <returns></returns>
        [WebInvoke(Method = "POST", UriTemplate = "Create", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<CreateOrderResult> Create(CreateOrderData data);


        [WebInvoke(Method = "GET", UriTemplate = "List/{iduser}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<OrderListInfo> List(string iduser);

        [WebInvoke(Method = "GET", UriTemplate = "Cancel/{iduser}/{orderno}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<bool> Cancel(string iduser, string orderno);



        [WebInvoke(Method = "GET", UriTemplate = "Detail/{iduser}/{orderno}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<MyOrderDetail> Detail(string iduser,string orderno);

        [WebInvoke(Method = "GET", UriTemplate = "DetailFoyPay/{iduser}/{orderno}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]

        Result<EF.MyOrder> DetailFoyPay(string iduser, string orderno);




    }
}
