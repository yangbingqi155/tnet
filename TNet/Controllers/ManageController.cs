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
using TNet.BLL;
using TNet.Authorize;
using TNet.Util;

namespace TNet.Controllers
{
    public class ManageController : Controller
    {
        [HttpGet]
        public ActionResult Login() {
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
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult MercList(int pageIndex=0) {
            int pageCount = 0;
            int pageSize = 10;
            List<Merc> entities = MercService.GetALL();
            List<Merc> pageList=  entities.Pager<Merc>(pageIndex, pageSize, out pageCount);


            List<MercViewModel> viewModels = pageList.Select(model => {
                MercViewModel viewModel = new MercViewModel();
                viewModel.CopyFromBase(model);
                return viewModel;
            }).ToList();


            //获取产品类型
            List<MercType> mercTypes = MercTypeService.GetALL();

            viewModels =viewModels.Select(model => {
                model.MercTypeName = mercTypes.Where(en => en.idtype == model.idtype).FirstOrDefault().name;
                return model;
            }).ToList();

            ViewData["pageCount"] = pageCount;
            ViewData["pageIndex"] = pageIndex;
            

            return View(viewModels);
        }

        /// <summary>
        /// 创建/编辑商品
        /// </summary>
        /// <param name="idmerc"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpGet]
        public ActionResult MercEdit(int idmerc=0) {
            MercViewModel model = new MercViewModel();
            if (idmerc>0) {
                Merc merc = MercService.GetMerc(idmerc);
                if ( merc!=null) { model.CopyFromBase(merc); }
            }
            List<MercType> entities = MercTypeService.GetALL();
            List<MercTypeViewModel> mercTypes= entities.Select(en => {
                MercTypeViewModel viewModel = new MercTypeViewModel();
                viewModel.CopyFromBase(en);
                return viewModel;
            }).ToList();

            model.mercTypes = mercTypes;
            return View(model);
        }

        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpPost]
        public ActionResult MercEdit(MercViewModel model) {

            Merc merc = new Merc();
            model.CopyToBase(merc);
            if (merc.idmerc == 0)
            {
                //新增
                merc = MercService.Add(merc);
            }
            else {
                //编辑
                merc = MercService.Edit(merc);
            }
            

            //修改后重新加载
            model.CopyFromBase(merc); 
            List<MercType> entities = MercTypeService.GetALL();
            List<MercTypeViewModel> mercTypes = entities.Select(en => {
                MercTypeViewModel viewModel = new MercTypeViewModel();
                viewModel.CopyFromBase(en);
                return viewModel;
            }).ToList();

            model.mercTypes = mercTypes;

            ModelState.AddModelError("", "保存成功.");
            return View(model);
        }

        /// <summary>
        /// 产品类型列表
        /// </summary>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult MercTypeList(int pageIndex=0) {
            int pageCount = 0;
            int pageSize = 10;
            List<MercType> entities = MercTypeService.GetALL();
            List<MercType> pageList = entities.Pager<MercType>(pageIndex, pageSize, out pageCount);


            List<MercTypeViewModel> viewModels = pageList.Select(model => {
                MercTypeViewModel viewModel = new MercTypeViewModel();
                viewModel.CopyFromBase(model);
                return viewModel;
            }).ToList();

            ViewData["pageCount"] = pageCount;
            ViewData["pageIndex"] = pageIndex;

            return View(viewModels);
        }

        /// <summary>
        /// 新增\编辑产品类型
        /// </summary>
        /// <param name="idtype"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpGet]
        public ActionResult MercTypeEdit(int idtype=0)
        {
            MercTypeViewModel model = new MercTypeViewModel();
            if (idtype > 0)
            {
                MercType mercType = MercTypeService.GetMercType(idtype);
                if (mercType != null) { model.CopyFromBase(mercType); }
            }
           
            return View(model);
        }

        /// <summary>
        /// 新增\编辑产品类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpPost]
        public ActionResult MercTypeEdit(MercTypeViewModel model)
        {
            MercType mercType = new MercType();
            model.CopyToBase(mercType);
            if (mercType.idtype == 0)
            {
                //新增
                mercType = MercTypeService.Add(mercType);
            }
            else
            {
                //编辑
                mercType = MercTypeService.Edit(mercType);
            }

            //修改后重新加载
            model.CopyFromBase(mercType);
           
            return View(model);
        }

        /// <summary>
        /// 产品规格列表
        /// </summary>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult SpecList() {
            return View();
        }

        /// <summary>
        /// 新增\编辑产品规格
        /// </summary>
        /// <param name="idspec"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpGet]
        public ActionResult SpecEdit(int idspec = 0)
        {
            return View();
        }

        /// <summary>
        /// 新增\编辑产品规格
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpPost]
        public ActionResult SpecEdit(SpecViewModel model)
        {
            return View(model);
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
