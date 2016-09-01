using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Util;
using System.IO;
using WeChatApp.Models;
using TNet.Models;
using TNet.EF;
using TNet.Models.Service;
using TNet.Authorize;
using TNet.Util;

namespace TNet.Controllers
{
    public class ManageController : Controller
    {
        [HttpGet]
        public ActionResult Login() {
            string salt = string.Empty;
            string clearPassword = "admin";
            string md5Password = string.Empty;
            Crypto.GetPwdhashAndSalt(clearPassword, out salt, out md5Password);

            return View();
        }


        [HttpPost]
        public ActionResult Login(ManageUserViewModel model) {
            ManageUser user = ManageUserService.GetManageUserByUserName(model.UserName);
            if (user==null) {
                ModelState.AddModelError("","没有找到该用户名.");
                return View(model);
            }
            string md5Password = string.Empty;
            md5Password= Crypto.GetPwdhash(model.ClearPassword, user.MD5Salt);
            if (md5Password.ToUpper()!=user.Password.ToUpper()) {
                ModelState.AddModelError("", "密码不正确.");
                return View(model);
            }
            Session["ManageUser"] = user;
            return RedirectToAction("Index", "Manage");
        }

        /// <summary>
        /// 后台管理首页
        /// </summary>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult Index()
        {
            
            return View();
        }


        /// <summary>
        /// 商品列表
        /// </summary>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult MercList(int pageIndex=0) {
            int pageCount = 0;
            int pageSize = 10;
            List<Merc> entities = MercService.GetALL();
            List<Merc> pageList=  entities.Pager<Merc>(pageIndex, pageSize, out pageCount);
            
            ViewData["pageCount"] = pageCount;
            ViewData["pageIndex"] = pageIndex;
            

            return View(pageList);
        }

        [HttpGet]
        public ActionResult Menu()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token=" + Pub.accessToken;
            string data = HttpHelp.Get(url);
            ViewBag.Menu = new HtmlString(data);
            return View();
        }


        public ActionResult GetMenu()
        {

            return View();
        }

        /// <summary>
        /// 获取素材列表
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public JsonResult GetMaterialList(RMaterialListParamM mode)
        {
             
            string url = "https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=" + Pub.accessToken;
            JavaScriptSerializer s = new JavaScriptSerializer();
            string responseContent = HttpHelp.Post(url, s.Serialize(mode));
            return Json(responseContent);
        }

        /// <summary>
        /// 获取指定图片信息
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns>存储路径</returns>
        public string GetImageById(RMaterialItemParamM mode)
        {
            string filePath = Server.MapPath("~/Content/Images/Material/" + mode.media_id + ".jpg");
            if (!System.IO.File.Exists(filePath))
            {
                HttpHelp bll = new HttpHelp();
                string url = "https://api.weixin.qq.com/cgi-bin/material/get_material?access_token=" + Pub.accessToken;
                JavaScriptSerializer Serializer = new JavaScriptSerializer();
                filePath = bll.DownLoad(url, Serializer.Serialize(mode), mode.media_id);
            }
            return Pub.urlconvertor(filePath);
        }


        /// <summary>
        /// 获取指定的素材信息
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public string GetMaterialDetail(RMaterialItemParamM mode)
        { 
            string url = "https://api.weixin.qq.com/cgi-bin/material/get_material?access_token=" + Pub.accessToken;
            JavaScriptSerializer s = new JavaScriptSerializer();
            string responseContent = HttpHelp.Post(url, s.Serialize(mode));
            return responseContent;
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public JsonResult UpdateMenu(string menu)
        { 
            string url = " https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + Pub.accessToken;
            string responseContent = HttpHelp.Post(url, menu);
            return Json(responseContent);
        }
    }
}
