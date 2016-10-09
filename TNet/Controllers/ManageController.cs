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
using TCom.EF;
using TNet.BLL;
using TNet.Authorize;
using TNet.Util;
using log4net;
using System.Reflection;
using System;

namespace TNet.Controllers
{
    public class ManageController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ManageUserViewModel model)
        {
            ManageUser user = ManageUserService.GetManageUserByUserName(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "没有找到该用户名.");
                return View(model);
            }
            string md5Password = string.Empty;
            md5Password = Crypto.GetPwdhash(model.ClearPassword, user.MD5Salt);
            if (md5Password.ToUpper() != user.Password.ToUpper())
            {
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
        /// 商圈列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult BusinessList(int pageIndex = 0)
        {
            int pageCount = 0;
            int pageSize = 10;
            List<Business> entities = BusinessService.GetALL();
            List<Business> pageList = entities.Pager<Business>(pageIndex, pageSize, out pageCount);

            List<BusinessViewModel> viewModels = pageList.Select(model =>
            {
                BusinessViewModel viewModel = new BusinessViewModel();
                viewModel.CopyFromBase(model);
                return viewModel;
            }).ToList();

            ViewData["pageCount"] = pageCount;
            ViewData["pageIndex"] = pageIndex;

            return View(viewModels);
        }

        /// <summary>
        /// 启用或者禁用商圈
        /// </summary>
        /// <param name="idbuss"></param>
        /// <param name="enable"></param>
        /// <param name="isAjax"></param>
        /// <returns></returns>
        [HttpPost]
        [ManageLoginValidation]
        public ActionResult BusinessEnable(long idbuss, bool enable, bool isAjax)
        {
            ResultModel<BusinessViewModel> resultEntity = new ResultModel<BusinessViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "成功";
            try
            {
                Business business = BusinessService.GetBusiness(idbuss);
                business.inuse = enable;
                BusinessService.Edit(business);
            }
            catch (Exception ex)
            {
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = ex.ToString();
            }

            return Content(resultEntity.SerializeToJson());
        }

        /// <summary>
        /// 录入/编辑商圈
        /// </summary>
        /// <param name="idbuss"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult BusinessEdit(long idbuss = 0)
        {
            BusinessViewModel model = new BusinessViewModel();
            if (idbuss > 0)
            {
                Business business = BusinessService.GetBusiness(idbuss);
                if (business != null) { model.CopyFromBase(business); }
            }
            else
            {
                model.inuse = true;
            }

            return View(model);
        }

        /// <summary>
        /// 录入/编辑商圈
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpPost]
        public ActionResult BusinessEdit(BusinessViewModel model)
        {
            Business business = new Business();
            model.CopyToBase(business);
            if (business.idbuss == 0)
            {
                business.idbuss = IdentifyService.GetMaxIdentifyID<Business>(en => (int)en.idbuss) + 1;
                //新增
                business = BusinessService.Add(business);
            }
            else
            {
                //编辑
                business = BusinessService.Edit(business);
            }

            //修改后重新加载
            model.CopyFromBase(business);

            ModelState.AddModelError("", "保存成功.");
            return View(model);
        }

        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult OrderList(DateTime? startOrDate,DateTime? endOrDate,int orderTypes=0,int orderStatus=0, long orderNo =0,long userNo=0, int pageIndex = 0)
        {
            int pageCount = 0;
            int pageSize = 10;
            if (startOrDate==null) {
                startOrDate = DateTime.Now.AddDays(-1);
            }
            if (endOrDate==null) {
                endOrDate = DateTime.Now;
            }
            List<MyOrder> entities = MyOrderService.GetOrderByFilter(startOrDate,endOrDate,orderTypes,orderStatus,orderNo,userNo);
            List<MyOrder> pageList = entities.Pager<MyOrder>(pageIndex, pageSize, out pageCount);

            List<MyOrderViewModel> viewModels = pageList.Select(model =>
            {
                MyOrderViewModel viewModel = new MyOrderViewModel();
                viewModel.CopyFromBase(model);
                return viewModel;
            }).ToList();

            RouteData.Values.Add("startOrDate",startOrDate);
            RouteData.Values.Add("endOrDate", endOrDate);
            RouteData.Values.Add("orderTypes", orderTypes);
            RouteData.Values.Add("orderStatus", orderStatus);
            RouteData.Values.Add("orderNo", orderNo);
            RouteData.Values.Add("userNo", userNo);
            
            ViewData["pageCount"] = pageCount;
            ViewData["pageIndex"] = pageIndex;

            ViewData["startOrDate"] = startOrDate;
            ViewData["endOrDate"] = endOrDate;
            ViewData["orderTypes"] = orderTypes;
            ViewData["orderStatus"] = orderStatus;
            ViewData["orderNo"] = orderNo;
            ViewData["userNo"] = userNo;

            return View(viewModels);
        }

        /// <summary>
        /// 启用或者禁用商圈
        /// </summary>
        /// <param name="idbuss"></param>
        /// <param name="enable"></param>
        /// <param name="isAjax"></param>
        /// <returns></returns>
        [HttpPost]
        [ManageLoginValidation]
        public ActionResult OrderEnable(long orderno, bool enable, bool isAjax)
        {
            ResultModel<MyOrderViewModel> resultEntity = new ResultModel<MyOrderViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "成功";
            try
            {
                MyOrder order = MyOrderService.GetOrder(orderno);
                order.inuse = enable;
                MyOrderService.Edit(order);
            }
            catch (Exception ex)
            {
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = ex.ToString();
            }

            return Content(resultEntity.SerializeToJson());
        }

        /// <summary>
        /// 订单详细
        /// </summary>
        /// <param name="idbuss"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult OrderDetail(long orderno)
        {
            MyOrderViewModel model = new MyOrderViewModel();
            MyOrder order = MyOrderService.GetOrder(orderno);
            if (order != null) { model.CopyFromBase(order); }
            return View(model);
        }

        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult MercList(int pageIndex = 0)
        {
            int pageCount = 0;
            int pageSize = 10;
            List<Merc> entities = MercService.GetALL();
            List<Merc> pageList = entities.Pager<Merc>(pageIndex, pageSize, out pageCount);


            List<MercViewModel> viewModels = pageList.Select(model =>
            {
                MercViewModel viewModel = new MercViewModel();
                viewModel.CopyFromBase(model);
                return viewModel;
            }).ToList();


            //获取产品类型
            List<MercType> mercTypes = MercTypeService.GetALL();

            viewModels = viewModels.Select(model =>
            {
                model.MercTypeName = mercTypes.Where(en => en.idtype == model.idtype).FirstOrDefault().name;
                return model;
            }).ToList();

            ViewData["pageCount"] = pageCount;
            ViewData["pageIndex"] = pageIndex;


            return View(viewModels);
        }

        /// <summary>
        /// 启用或者禁用产品
        /// </summary>
        /// <param name="idmerc"></param>
        /// <param name="enable"></param>
        /// <param name="isAjax"></param>
        /// <returns></returns>
        [HttpPost]
        [ManageLoginValidation]
        public ActionResult MercEnable(int idmerc, bool enable, bool isAjax)
        {
            ResultModel<MercViewModel> resultEntity = new ResultModel<MercViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "成功";
            try
            {
                Merc merc = MercService.GetMerc(idmerc);
                merc.inuse = enable;
                MercService.Edit(merc);
            }
            catch (Exception ex)
            {
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = ex.ToString();
            }

            return Content(resultEntity.SerializeToJson());
        }

        /// <summary>
        /// 创建/编辑商品
        /// </summary>
        /// <param name="idmerc"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpGet]
        public ActionResult MercEdit(int idmerc = 0)
        {
            MercViewModel model = new MercViewModel();
            if (idmerc > 0)
            {
                Merc merc = MercService.GetMerc(idmerc);
                if (merc != null) { model.CopyFromBase(merc); }
            }
            else
            {
                model.inuse = true;
            }
            List<MercType> entities = MercTypeService.GetALL();
            List<MercTypeViewModel> mercTypes = entities.Select(en =>
            {
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
        public ActionResult MercEdit(MercViewModel model)
        {

            Merc merc = new Merc();
            model.CopyToBase(merc);
            if (merc.idmerc == 0)
            {
                merc.idmerc = IdentifyService.GetMaxIdentifyID<Merc>(en => en.idmerc) + 1;
                //新增
                merc = MercService.Add(merc);
            }
            else
            {
                //编辑
                merc = MercService.Edit(merc);
            }


            //修改后重新加载
            model.CopyFromBase(merc);
            List<MercType> entities = MercTypeService.GetALL();
            List<MercTypeViewModel> mercTypes = entities.Select(en =>
            {
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
        public ActionResult MercTypeList(int pageIndex = 0)
        {
            int pageCount = 0;
            int pageSize = 10;
            List<MercType> entities = MercTypeService.GetALL();
            List<MercType> pageList = entities.Pager<MercType>(pageIndex, pageSize, out pageCount);


            List<MercTypeViewModel> viewModels = pageList.Select(model =>
            {
                MercTypeViewModel viewModel = new MercTypeViewModel();
                viewModel.CopyFromBase(model);
                return viewModel;
            }).ToList();

            ViewData["pageCount"] = pageCount;
            ViewData["pageIndex"] = pageIndex;

            return View(viewModels);
        }


        /// <summary>
        /// 启用或者禁用产品类型
        /// </summary>
        /// <param name="idtype"></param>
        /// <param name="enable"></param>
        /// <param name="isAjax"></param>
        /// <returns></returns>
        [HttpPost]
        [ManageLoginValidation]
        public ActionResult MercTypeEnable(int idtype, bool enable, bool isAjax)
        {
            ResultModel<MercTypeViewModel> resultEntity = new ResultModel<MercTypeViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "成功";
            try
            {
                MercType mercType = MercTypeService.GetMercType(idtype);
                mercType.inuse = enable;
                MercTypeService.Edit(mercType);
            }
            catch (Exception ex)
            {
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = ex.ToString();
            }

            return Content(resultEntity.SerializeToJson());
        }

        /// <summary>
        /// 新增\编辑产品类型
        /// </summary>
        /// <param name="idtype"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpGet]
        public ActionResult MercTypeEdit(int idtype = 0)
        {
            MercTypeViewModel model = new MercTypeViewModel();
            if (idtype > 0)
            {
                MercType mercType = MercTypeService.GetMercType(idtype);
                if (mercType != null) { model.CopyFromBase(mercType); }
            }
            else
            {
                model.inuse = true;
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
                mercType.idtype = IdentifyService.GetMaxIdentifyID<MercType>(en => en.idtype) + 1;
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
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult SpecList(int idmerc, int pageIndex = 0)
        {
            int pageCount = 0;
            int pageSize = 10;
            List<Spec> entities = SpecService.GetSpecsByIdMerc(idmerc);
            List<Spec> pageList = entities.Pager<Spec>(pageIndex, pageSize, out pageCount);

            List<SpecViewModel> viewModels = pageList.Select(model =>
            {
                SpecViewModel viewModel = new SpecViewModel();
                viewModel.CopyFromBase(model);
                return viewModel;
            }).ToList();

            ViewData["pageCount"] = pageCount;
            ViewData["pageIndex"] = pageIndex;
            ViewData["mercId"] = idmerc;


            return View(viewModels);
        }

        /// <summary>
        /// 启用或者禁用产品规格
        /// </summary>
        /// <param name="idspec"></param>
        /// <param name="enable"></param>
        /// <param name="isAjax"></param>
        /// <returns></returns>
        [HttpPost]
        [ManageLoginValidation]
        public ActionResult SpecEnable(int idspec, bool enable, bool isAjax)
        {
            ResultModel<SpecViewModel> resultEntity = new ResultModel<SpecViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "成功";
            try
            {
                Spec spec = SpecService.GetSpecs(idspec);
                spec.inuse = enable;
                SpecService.Edit(spec);
            }
            catch (Exception ex)
            {
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = ex.ToString();
            }

            return Content(resultEntity.SerializeToJson());
        }

        /// <summary>
        /// 新增\编辑产品规格
        /// </summary>
        /// <param name="idspec"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpGet]
        public ActionResult SpecEdit(int idmerc, int idspec = 0)
        {
            SpecViewModel model = new SpecViewModel();
            if (idspec > 0)
            {
                Spec spec = SpecService.GetSpecs(idspec);
                if (spec != null) { model.CopyFromBase(spec); }
            }
            else
            {
                model.inuse = true;
            }

            return View(model);
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
            Spec spec = new Spec();
            model.CopyToBase(spec);
            if (spec.idspec == 0)
            {
                spec.idspec = IdentifyService.GetMaxIdentifyID<Spec>(en => en.idspec) + 1;
                //新增
                spec = SpecService.Add(spec);
            }
            else
            {
                //编辑
                spec = SpecService.Edit(spec);
            }

            //修改后重新加载
            model.CopyFromBase(spec);

            return View(model);
        }

        /// <summary>
        /// 获取产品图片
        /// </summary>
        /// <param name="mercId"></param>
        /// <param name="isAjax"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult AjaxMercImageList(int mercId,bool isAjax) {
            ResultModel<MercImageViewModel> resultEntity = new ResultModel<MercImageViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "成功";
            try {
                List<MercImage> entities = MercImageService.GetMercImagesByMercId(mercId);
                List<MercImageViewModel> viewModels = entities.Select(model =>
                {
                    MercImageViewModel viewModel = new MercImageViewModel();
                    viewModel.CopyFromBase(model);
                    viewModel.Path = Url.Content( viewModel.Path);
                    return viewModel;
                }).ToList();
                resultEntity.Content = viewModels;
            }
            catch (Exception ex) {
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = ex.ToString();
            }

            return Content(resultEntity.SerializeToJson());
        }

        /// <summary>
        /// 产品图片管理
        /// </summary>
        /// <param name="mercId"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult MercImageList(int mercId, int pageIndex = 0)
        {
            int pageCount = 0;
            int pageSize = 10;
            List<MercImage> entities = MercImageService.GetMercImagesByMercId(mercId);
            List<MercImage> pageList = entities.Pager<MercImage>(pageIndex, pageSize, out pageCount);

            List<MercImageViewModel> viewModels = pageList.Select(model =>
            {
                MercImageViewModel viewModel = new MercImageViewModel();
                viewModel.CopyFromBase(model);
                return viewModel;
            }).ToList();

            ViewData["pageCount"] = pageCount;
            ViewData["pageIndex"] = pageIndex;
            ViewData["mercId"] = mercId;

            return View(viewModels);
        }

        /// <summary>
        /// 启用或者禁用产品图片
        /// </summary>
        /// <param name="MercImageId"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        [HttpPost]
        [ManageLoginValidation]
        public ActionResult MercImageEnable(int MercImageId, bool enable, bool isAjax)
        {
            ResultModel<MercImageViewModel> resultEntity = new ResultModel<MercImageViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "成功";
            try
            {
                MercImage mercImage = MercImageService.GetMercImage(MercImageId);
                mercImage.InUse = enable;
                MercImageService.Edit(mercImage);
            }
            catch (Exception ex)
            {
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = ex.ToString();
            }

            return Content(resultEntity.SerializeToJson());
        }

        /// <summary>
        /// 产品图片信息编辑
        /// </summary>
        /// <param name="ercImageId"></param>
        /// <returns></returns>
        [HttpGet]
        [ManageLoginValidation]
        public ActionResult MercImageEdit(int idmerc, int MercImageId = 0)
        {
            MercImageViewModel mercImageModel = new MercImageViewModel();
            if (MercImageId == 0)
            {
                mercImageModel.idmerc = idmerc;
                mercImageModel.InUse = true;
            }
            else
            {
                MercImage mercImage = MercImageService.GetMercImage(MercImageId);
                mercImageModel.CopyFromBase(mercImage);
            }
            return View(mercImageModel);
        }

        /// <summary>
        /// 产品图片信息编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ManageLoginValidation]
        public ActionResult MercImageEdit(MercImageViewModel model)
        {
            MercImage mercImage = new MercImage();
            model.CopyToBase(mercImage);
            if (model.MercImageId == 0)
            {
                mercImage.MercImageId = IdentifyService.GetMaxIdentifyID<MercImage>(en => en.MercImageId) + 1;
                MercImageService.Add(mercImage);
            }
            else
            {
                MercImageService.Edit(mercImage);
            }

            return RedirectToAction("MercImageEdit", "Manage", new { idmerc = mercImage.idmerc, MercImageId = mercImage.MercImageId });
        }

        [ManageLoginValidation]
        public ActionResult UploadMercImage(int mercId=0)
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            ResultModel<MercImageViewModel> resultEntity = new ResultModel<MercImageViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "文件上传成功";
            string GUID = System.Guid.NewGuid().ToString();
            string path = "~/Resource/Images/Merc/";
            string filename = string.Empty;
            string message = string.Empty;
            try
            {
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    //if (Request.Files.Count > 1)
                    //{
                    //    resultEntity.Code = ResponseCodeType.Fail;
                    //    resultEntity.Message = "请选择文件.";
                    //    return Content(resultEntity.SerializeToJson());
                    //}
                    resultEntity.Content = new List<MercImageViewModel>();
                    int i = 0;
                    foreach (string upload in Request.Files)
                    {
                        GUID = System.Guid.NewGuid().ToString();
                        if (!Request.Files[i].HasFile())
                        {
                            resultEntity.Code = ResponseCodeType.Fail;
                            resultEntity.Message = "文件大小不能0.";
                            return Content(resultEntity.SerializeToJson());
                        }


                        if (!CheckFileType((HttpPostedFileWrapper)((HttpFileCollectionWrapper)Request.Files)[i]))
                        {
                            resultEntity.Code = ResponseCodeType.Fail;
                            resultEntity.Message = "文件类型只能是jpg,bmp,gif,PNG..";
                            return Content(resultEntity.SerializeToJson());
                        }

                        //获取文件后缀名
                        string originFileName = Request.Files[i].FileName;
                        string originFileNameSuffix = string.Empty;
                        int lastIndex = originFileName.LastIndexOf(".");
                        if (lastIndex < 0)
                        {
                            resultEntity.Code = ResponseCodeType.Fail;
                            resultEntity.Message = "文件类型只能是jpg,bmp,gif,PNG..";
                            return Content(resultEntity.SerializeToJson());
                        }
                        originFileNameSuffix = originFileName.Substring(lastIndex, originFileName.Length - lastIndex);

                        filename = GUID + originFileNameSuffix;
                        if (!Directory.Exists(Server.MapPath(path)))
                        {
                            Directory.CreateDirectory(Server.MapPath(path));
                        }

                        Request.Files[i].SaveAs(Path.Combine(Server.MapPath(path), filename));
                        MercImageViewModel model = new MercImageViewModel()
                        {
                            idmerc = mercId,
                            Path = path + filename,
                            SortID = MercImageService.MaxMercImageSortID(mercId)+1,
                            InUse = true
                        };

                        resultEntity.Content.Add(model);
                        i ++;
                    }

                }
                else
                {
                    resultEntity.Code = ResponseCodeType.Fail;
                    resultEntity.Message = "没有选择要上传的文件.";
                    return Content(resultEntity.SerializeToJson());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = "没有选择要上传的文件.";
                return Content(resultEntity.SerializeToJson());
            }
            return Content(resultEntity.SerializeToJson());

        }

        [ManageLoginValidation]
        public ActionResult DeleteMercImage(int mercImageId,int idmerc,bool isAjax)
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            ResultModel<MercImageViewModel> resultEntity = new ResultModel<MercImageViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "文件删除成功";

            try {
                MercImage image = MercImageService.GetMercImage(mercImageId);
                if (image==null) {
                    resultEntity.Code = ResponseCodeType.Fail;
                    resultEntity.Message = "该商品图片已经不存在，请刷新页面重试.";
                    return Content(resultEntity.SerializeToJson());
                }
                try {
                    if (!string.IsNullOrEmpty(image.Path) && System.IO.File.Exists(Server.MapPath(image.Path))) {
                        System.IO.File.Delete(Server.MapPath(image.Path));
                    }
                    MercImageService.Delete(mercImageId);
                    MercService.SetDefaultMercImage(idmerc);
                }
                catch (Exception ex) {
                    resultEntity.Code = ResponseCodeType.Fail;
                    resultEntity.Message = "文件删除失败.";
                    return Content(resultEntity.SerializeToJson());
                }
                
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = "文件删除失败.";
                return Content(resultEntity.SerializeToJson());
            }
            return Content(resultEntity.SerializeToJson());

        }

        [ManageLoginValidation]
        public ActionResult SortMercImage(string mercImageViewModelListJson,int idmerc,bool isAjax) {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            List<MercImageViewModel> entities = new List<MercImageViewModel>();
            ResultModel<MercImageViewModel> resultEntity = new ResultModel<MercImageViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "移动商品图片成功";
            try {
                JavaScriptSerializer js = new JavaScriptSerializer();
                entities = js.Deserialize<List<MercImageViewModel>>(mercImageViewModelListJson);

                List<MercImage> images = entities.Select(model =>
                {
                    MercImage img = new MercImage();
                    model.CopyToBase(img);
                    return img;
                }  ).ToList();

               bool result= MercImageService.BatchChangSort(images);
                if (!result) {
                    resultEntity.Code = ResponseCodeType.Success;
                    resultEntity.Message = "移动商品图片失败";
                    return Content(resultEntity.SerializeToJson());
                }
                else {
                    MercService.SetDefaultMercImage(idmerc);
                }
            }
            catch (Exception ex) {
                log.Error(ex.ToString());
                resultEntity.Code = ResponseCodeType.Success;
                resultEntity.Message = "移动商品图片失败";
                return Content(resultEntity.SerializeToJson());
            }
            
            return Content(resultEntity.SerializeToJson());
        }

        /// <summary>
        /// 判断上传文件类型
        /// </summary>
        private bool CheckFileType(HttpPostedFileWrapper postedFile)
        {

            bool result = true;
            /*  文件扩展名说明  
            jpg：255216  
            bmp：6677  
            gif：7173  
            PNG：13780
            pdf：3780  
            */
            int[] fileTypes = new int[] { 255216, 6677, 7173, 13780, 3780 };
            string fileTypeStr = "255216, 6677, 7173, 13780, 3780";
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            int fileLength = postedFile.ContentLength;
            if (fileLength <= 0)
            {
                return false;
            }
            byte[] imgArray = new byte[fileLength];

            postedFile.InputStream.Read(imgArray, 0, fileLength);

            System.IO.MemoryStream fs = new System.IO.MemoryStream();
            fs = new System.IO.MemoryStream(imgArray);
            System.IO.BinaryReader r = new System.IO.BinaryReader(fs);
            string fileclass = string.Empty;
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                fileclass = buffer.ToString();
                buffer = r.ReadByte();
                fileclass += buffer.ToString();
            }
            catch (Exception)
            {
                result = false;
                //Log.Error(ex.ToString());
            }
            finally
            {
                r.Close();
                fs.Close();
                r.Dispose();
                fs.Dispose();
            }
            if (fileTypeStr.IndexOf(fileclass) < 0)
            {
                result = false;
            }

            return result;

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
