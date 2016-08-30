using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Util;
using System.IO;
using WeChatApp.Models;

namespace TNet.Controllers
{
    public class ManageController : Controller
    {
        [HttpGet]
        public ActionResult Login() {
            
            return View();
        }


        [HttpPost]
        public ActionResult Login(string UserName,string Password) {
            return View();
        }


        //
        // GET: /Manage/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Menu()
        {
            HttpHelp bll = new HttpHelp();
            string url = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token=" + Pub.accessToken;
            string data = bll.HttpGet(url);
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

            HttpHelp bll = new HttpHelp();
            string url = "https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=" + Pub.accessToken;
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            string responseContent = bll.HttpPost(url, Serializer.Serialize(mode));
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
            HttpHelp bll = new HttpHelp();
            string url = "https://api.weixin.qq.com/cgi-bin/material/get_material?access_token=" + Pub.accessToken;
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            string responseContent = bll.HttpPost(url, Serializer.Serialize(mode));
            return responseContent;
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public JsonResult UpdateMenu(string menu)
        {
            HttpHelp bll = new HttpHelp();
            string url = " https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + Pub.accessToken;
            
            string responseContent = bll.HttpPost(url, menu);
            return Json(responseContent);
        }
    }
}
