using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Configuration;
using TNet.Models;

namespace TNet.Authorize
{
    /// <summary>
    /// 管理员登录验证
    /// </summary>
    public class ManageLoginValidationAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            object obj = HttpContext.Current.Session["ManageUser"];
            if (obj == null)
            {
                string action=filterContext.RouteData.Values["action"].ToString();
                string controller = filterContext.RouteData.Values["controller"].ToString();
                if (controller.ToLower()=="manage"&&action.ToLower()=="login") {
                    return;
                }

                Object isAjax = filterContext.RouteData.Values["isAjax"];

                if (isAjax != null&&Convert.ToBoolean(isAjax))
                {
                    ResultModel<NULLViewModel> resultEntity = new ResultModel<NULLViewModel>();
                    resultEntity.Code = ResponseCodeType.LoginAgain;
                    resultEntity.Message = "登录失效，请重新登录";
                    ContentResult result = new ContentResult();
                    result.Content = resultEntity.SerializeToJson();
                    filterContext.Result = result;
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Manage/Login");
                }
            }
        }
    }
}