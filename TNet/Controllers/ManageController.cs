﻿using System.Web;
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
using TNet.BLL.User;
using TNet.Authorize;
using TNet.Util;
using log4net;
using System.Reflection;
using System;
using System.Text;

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
            if (user == null|| user.UserType==(int)ManageUserType.Worker)
            {
                ModelState.AddModelError("", "没有找到该用户名或者帐号被禁用或者没有权限登录.");
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

        [ManageLoginValidation]
        public ActionResult SignOut() {
            Session["ManageUser"] = null;
            return RedirectToAction("Login", "Manage");
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
        public ActionResult BusinessEdit(BusinessViewModel model, string bussImages = "")
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


            BussImageService.DeleteBussImages(business.idbuss);

            if (!string.IsNullOrEmpty(bussImages)) {
                List<BussImage> list = new List<BussImage>();
                string[] imgs = bussImages.Split(',');
                int i = 0;
                foreach (var item in imgs) {
                    list.Add(new BussImage() {
                        BussImageId=Pub.ID(),
                        idbuss = business.idbuss,
                        InUse = true,
                        Path = item,
                        SortID = i + 1
                    });
                    i++;
                }
                if (list.Count > 0) {
                    BussImageService.AddMuti(list);
                }
            }
            BusinessService.SetDefaultBussImage(business.idbuss);


            ModelState.AddModelError("", "保存成功.");
            return View(model);
        }


        /// <summary>
        /// 获取商圈图片
        /// </summary>
        /// <param name="idbuss"></param>
        /// <param name="isAjax"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult AjaxBussImageList(int idbuss, bool isAjax) {
            ResultModel<BussImageViewModel> resultEntity = new ResultModel<BussImageViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "成功";
            try {
                List<BussImage> entities = BussImageService.GetBussImagesByBussId(idbuss);
                List<BussImageViewModel> viewModels = entities.Select(model => {
                    BussImageViewModel viewModel = new BussImageViewModel();
                    viewModel.CopyFromBase(model);
                    viewModel.Path = Url.Content(viewModel.Path);
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

        [ManageLoginValidation]
        public ActionResult UploadBussImage(int idbuss = 0) {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            ResultModel<BussImageViewModel> resultEntity = new ResultModel<BussImageViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "文件上传成功";
            string GUID = System.Guid.NewGuid().ToString();
            string path = "~/Resource/Images/Buss/";
            string filename = string.Empty;
            string message = string.Empty;
            try {
                if (Request.Files != null && Request.Files.Count > 0) {
                    //if (Request.Files.Count > 1)
                    //{
                    //    resultEntity.Code = ResponseCodeType.Fail;
                    //    resultEntity.Message = "请选择文件.";
                    //    return Content(resultEntity.SerializeToJson());
                    //}
                    resultEntity.Content = new List<BussImageViewModel>();
                    int i = 0;
                    foreach (string upload in Request.Files) {
                        GUID = System.Guid.NewGuid().ToString();
                        if (!Request.Files[i].HasFile()) {
                            resultEntity.Code = ResponseCodeType.Fail;
                            resultEntity.Message = "文件大小不能0.";
                            return Content(resultEntity.SerializeToJson());
                        }


                        if (!CheckFileType((HttpPostedFileWrapper)((HttpFileCollectionWrapper)Request.Files)[i])) {
                            resultEntity.Code = ResponseCodeType.Fail;
                            resultEntity.Message = "文件类型只能是jpg,bmp,gif,PNG..";
                            return Content(resultEntity.SerializeToJson());
                        }

                        //获取文件后缀名
                        string originFileName = Request.Files[i].FileName;
                        string originFileNameSuffix = string.Empty;
                        int lastIndex = originFileName.LastIndexOf(".");
                        if (lastIndex < 0) {
                            resultEntity.Code = ResponseCodeType.Fail;
                            resultEntity.Message = "文件类型只能是jpg,bmp,gif,PNG..";
                            return Content(resultEntity.SerializeToJson());
                        }
                        originFileNameSuffix = originFileName.Substring(lastIndex, originFileName.Length - lastIndex);

                        filename = GUID + originFileNameSuffix;
                        if (!Directory.Exists(Server.MapPath(path))) {
                            Directory.CreateDirectory(Server.MapPath(path));
                        }

                        Request.Files[i].SaveAs(Path.Combine(Server.MapPath(path), filename));
                        BussImageViewModel model = new BussImageViewModel() {
                            idbuss = idbuss,
                            Path = path + filename,
                            SortID = BussImageService.MaxBussImageSortID(idbuss) + 1,
                            InUse = true
                        };

                        resultEntity.Content.Add(model);
                        i++;
                        StringBuilder initialPreview = new StringBuilder();
                        StringBuilder initialPreviewConfig = new StringBuilder();
                        initialPreviewConfig.Append(",\"initialPreviewConfig\":[");
                        initialPreview.Append("{\"initialPreview\":[");
                        for (int k = 0; k < resultEntity.Content.Count; k++) {
                            if (k == 0) {
                                initialPreview.AppendFormat("\"{0}\"", Url.Content(resultEntity.Content[k].Path));
                                initialPreviewConfig.Append("{\"url\":\"" + Url.Action("DeleteBussImage", "Manage") + "\"}");
                            }
                            else {
                                initialPreview.AppendFormat("\",{0}\"", Url.Content(resultEntity.Content[k].Path));
                                initialPreviewConfig.Append(",{\"url\":\"" + Url.Action("DeleteBussImage", "Manage") + "\"}");
                            }
                        }
                        initialPreview.Append("]");
                        initialPreviewConfig.Append("]");
                        initialPreview.Append(initialPreviewConfig.ToString());
                        initialPreview.Append("}");
                        return Content(initialPreview.ToString());
                    }

                }
                else {
                    resultEntity.Code = ResponseCodeType.Fail;
                    resultEntity.Message = "文件上传失败.";
                    return Content(resultEntity.SerializeToJson());
                }
            }
            catch (Exception ex) {
                log.Error(ex.ToString());
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = "没有选择要上传的文件.";
                return Content(resultEntity.SerializeToJson());
            }
            return Content(resultEntity.SerializeToJson());


        }

        [ManageLoginValidation]
        public ActionResult DeleteBussImage(int BussImageId = 0, int idbuss = 0, bool isAjax = true) {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            ResultModel<BussImageViewModel> resultEntity = new ResultModel<BussImageViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "文件删除成功";
            return Content(resultEntity.SerializeToJson());
        }


        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult OrderList(DateTime? startOrDate, DateTime? endOrDate, int orderTypes = 0, int orderStatus = 0, long orderNo = 0, long userNo = 0, int pageIndex = 0)
        {
            int pageCount = 0;
            int pageSize = 10;
            if (startOrDate == null)
            {
                startOrDate = DateTime.Now.AddDays(-1);
            }
            if (endOrDate == null)
            {
                endOrDate = DateTime.Now;
            }
            List<MyOrderViewModel> entities = MyOrderService.GetOrdersViewModelByFilter(startOrDate, endOrDate, orderTypes, orderStatus, orderNo, userNo);
            List<MyOrderViewModel> viewModels = entities.Pager<MyOrderViewModel>(pageIndex, pageSize, out pageCount);

            RouteData.Values.Add("startOrDate", startOrDate);
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
        /// 启用或者禁用订单
        /// </summary>
        /// <param name="orderno"></param>
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
            MyOrderViewModel model = MyOrderService.GetViewModel(orderno);
            return View(model);
        }

        /// <summary>
        /// 派单任务列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult TaskList(DateTime? startDate, DateTime? endDate, string orderno = "", string idsend = "", string idrevc = "", int pageIndex = 0)
        {
            int pageCount = 0;
            int pageSize = 10;
            if (startDate == null)
            {
                startDate = DateTime.Now.AddDays(-1);
            }
            if (endDate == null)
            {
                endDate = DateTime.Now;
            }

            List<TaskViewModel> entities = TaskService.Search(startDate, endDate, orderno, idsend, idrevc);
            List<TaskViewModel> viewModels = entities.Pager<TaskViewModel>(pageIndex, pageSize, out pageCount);
           
            List<ManageUser> manageUsers = new List<ManageUser>();
            manageUsers = ManageUserService.GetALL();
            List<SelectItemViewModel<string>> idsendSelectItems = manageUsers.Select(mod => {
                SelectItemViewModel<string> model = new SelectItemViewModel<string>();
                model.DisplayValue = mod.ManageUserId.ToString();
                model.DisplayText = mod.UserName;
                return model;
            }).ToList();
            idsendSelectItems.Insert(0,new SelectItemViewModel<string>() {
                 DisplayText="选择派单发送者",
                 DisplayValue=""
            });

            List<SelectItemViewModel<string>> idrevcSelectItems = manageUsers.Select(mod => {
                SelectItemViewModel<string> model = new SelectItemViewModel<string>();
                model.DisplayValue = mod.ManageUserId.ToString();
                model.DisplayText = mod.UserName;
                return model;
            }).ToList();

            idrevcSelectItems.Insert(0, new SelectItemViewModel<string>()
            {
                DisplayText = "选择派单接收者",
                DisplayValue = ""
            });

            RouteData.Values.Add("startDate", startDate);
            RouteData.Values.Add("endDate", endDate);
            RouteData.Values.Add("orderno", orderno);
            RouteData.Values.Add("idsend", idsend);
            RouteData.Values.Add("idrevc", idrevc);

            ViewData["pageCount"] = pageCount;
            ViewData["pageIndex"] = pageIndex;

            ViewData["startDate"] = startDate;
            ViewData["endDate"] = endDate;
            ViewData["orderno"] = orderno;
            ViewData["idsend"] = idsend;
            ViewData["idrevc"] = idrevc;
            ViewData["idsendSelectItems"] = idsendSelectItems;
            ViewData["idrevcSelectItems"] = idrevcSelectItems;
            return View(viewModels);
        }
        /// <summary>
        /// 任务详细
        /// </summary>
        /// <param name="idtask"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult TaskDetail(string idtask)
        {
            TaskViewModel model= TaskService.GetViewModel(idtask);
            return View(model);
        }

        /// <summary>
        /// 启用或者禁用订单
        /// </summary>
        /// <param name="orderno"></param>
        /// <param name="enable"></param>
        /// <param name="isAjax"></param>
        /// <returns></returns>
        [HttpPost]
        [ManageLoginValidation]
        public ActionResult TaskEnable(string idtask, bool enable, bool isAjax)
        {
            ResultModel<TaskViewModel> resultEntity = new ResultModel<TaskViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "成功";
            try
            {
                Task task = TaskService.Get(idtask);
                task.inuse = enable;
                TaskService.Edit(task);
            }
            catch (Exception)
            {
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = "操作失败";
            }

            return Content(resultEntity.SerializeToJson());
        }

        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="idtype"></param>
        /// <param name="merc"></param>
        /// <param name="netype"></param>
        /// <param name="isetup"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult MercList(int idtype = 0,string idcity="", string merc = "", int netype = -1, int isetup = -1, int pageIndex = 0)
        {
            int pageCount = 0;
            int pageSize = 10;
            List<Merc> entities = MercService.Search(idtype, idcity, merc,netype,isetup);
            List<Merc> pageList = entities.Pager<Merc>(pageIndex, pageSize, out pageCount);
            
            List<MercViewModel> viewModels = pageList.Select(model =>
            {
                MercViewModel viewModel = new MercViewModel();
                viewModel.CopyFromBase(model);
                return viewModel;
            }).ToList();

            //获取城市列表
            List<SelectItemViewModel<string>> citySelects=CityService.SelectItems();
            citySelects.Insert(0, new SelectItemViewModel<string>()
            {
                DisplayText = "所有城市",
                DisplayValue = ""
            });

            //获取产品类型
            List<MercType> mercTypes = MercTypeService.GetALL();

            viewModels = viewModels.Select(model =>
            {
                model.MercTypeName = mercTypes.Where(en => en.idtype == model.idtype).FirstOrDefault().name;
                return model;
            }).ToList();

            List<SelectItemViewModel<int>> netypeSelects = MercViewModel.GetNeTypeSelectItems();
            netypeSelects.Insert(0,new SelectItemViewModel<int>() {
                DisplayText = "所有接入方式",
                DisplayValue = -1
            });

            List<SelectItemViewModel<string>> mercTypeSelects= MercTypeService.SelectItems();
            mercTypeSelects.Insert(0,new SelectItemViewModel<string>() {
                 DisplayText="所有类型",
                 DisplayValue="0"
            });
            List<SelectItemViewModel<int>> isetupSelects = new List<SelectItemViewModel<int>>();
            isetupSelects.Add(new SelectItemViewModel<int>() {
                 DisplayValue=-1,
                 DisplayText="所有"
            });
            isetupSelects.Add(new SelectItemViewModel<int>() {
                DisplayValue = 0,
                DisplayText = "不能报装"
            });
            isetupSelects.Add(new SelectItemViewModel<int>() {
                DisplayValue = 1,
                DisplayText = "可报装"
            });

            ViewData["mercTypeSelects"] = mercTypeSelects;
            ViewData["isetupSelects"] = isetupSelects;
            ViewData["netypeSelects"] = netypeSelects;

            RouteData.Values.Add("idtype", idtype);
            RouteData.Values.Add("merc", merc);
            RouteData.Values.Add("netype", netype);
            RouteData.Values.Add("isetup", isetup);

            ViewData["idtype"] = idtype;
            ViewData["merc"] = merc;
            ViewData["netype"] = netype;
            ViewData["isetup"] = isetup;
            ViewData["citySelects"] = citySelects;

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
        /// <param name="mercImages"></param>
        /// <param name="idcity"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MercEdit(string[] idcity, MercViewModel model,string mercImages="")
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

            //保存商品城市关系
            List<CityRelation> cityRelations = new List<CityRelation>();
            if (idcity!=null&& idcity.Count()>0) {
                for (int i = 0; i < idcity.Length; i++)
                {
                    cityRelations.Add(new CityRelation() {
                         idcity= idcity[i],
                         idmodule= merc.idmerc.ToString(),
                          moduletype=(int)ModuleType.Merc,
                          inuse=true
                    });
                }
            }
            if (cityRelations!=null&& cityRelations.Count>0) {
                CityRelationService.Save(cityRelations);
            }
            
            MercImageService.DeleteMercImages(merc.idmerc);
            
            if (!string.IsNullOrEmpty(mercImages)) {
                List<MercImage> list = new List<MercImage>();
                string[] imgs = mercImages.Split(',');
                int i = 0;
                foreach (var item in imgs) {
                    list.Add(new MercImage() {
                         idmerc=merc.idmerc,
                          InUse=true,
                           Path=item,
                            SortID=i+1
                    });
                    i++;
                }
                if (list.Count>0) {
                    MercImageService.AddMuti(list);
                }
            }

            MercService.SetDefaultMercImage(merc.idmerc);

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

            List<string> idtypes = viewModels.Select(mod=> {
                return mod.idtype.ToString();
            }).ToList();

            List<Setup> setups= SetupService.GetByIdTypes(idtypes);

            viewModels = viewModels.Select(mod=> {
                List<Setup> temps= setups.Where(en => en.idtype == mod.idtype.ToString()).ToList();
                if ((temps != null && temps.Count > 0)) {
                    SetupViewModel viewModel = new SetupViewModel();
                    viewModel.CopyFromBase(temps.First());
                    mod.Setup = viewModel;
                }
                return mod;
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

            ModelState.AddModelError("", "保存成功.");

            return View(model);
        }

        /// <summary>
        /// 新增\编辑报装
        /// </summary>
        /// <param name="idtype"></param>
        /// <param name="idsetup"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpGet]
        public ActionResult SetupEdit(string idtype,string idsetup = "")
        {
            SetupViewModel model = new SetupViewModel();
            if (!string.IsNullOrEmpty(idsetup))
            {
                Setup setup = SetupService.Get(idsetup);
                if (setup != null) { model.CopyFromBase(setup); }
            }
            else
            {
                model.idtype = idtype;
                model.inuse = true;
            }
            MercType mercType= MercTypeService.GetMercType(Convert.ToInt32(idtype));
            if (mercType!=null) {
                MercTypeViewModel viewModel = new MercTypeViewModel();
                viewModel.CopyFromBase(mercType);
                model.merctype = viewModel;
            }

            return View(model);
        }

        /// <summary>
        /// 新增\编辑报装
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpPost]
        public ActionResult SetupEdit(SetupViewModel model)
        {
            Setup setup = new Setup();
            model.CopyToBase(setup);
            if (string.IsNullOrEmpty(setup.idsetup))
            {
                setup.idsetup = Pub.ID().ToString();
                //新增
                setup = SetupService.Add(setup);
            }
            else
            {
                //编辑
                setup = SetupService.Edit(setup);
            }

            //修改后重新加载
            model.CopyFromBase(setup);

            ModelState.AddModelError("", "保存成功.");

            return View(model);
        }


        /// <summary>
        /// 报装地址列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult SetupAddrList(int pageIndex = 0)
        {
            int pageCount = 0;
            int pageSize = 10;
            List<SetupAddrViewModel> entities = SetupAddrService.GetALLViewModels();
            List<SetupAddrViewModel> viewModels = entities.Pager<SetupAddrViewModel>(pageIndex, pageSize, out pageCount);
            
            ViewData["pageCount"] = pageCount;
            ViewData["pageIndex"] = pageIndex;

            return View(viewModels);
        }


        /// <summary>
        /// 新增\编辑报装地址
        /// </summary>
        /// <param name="idaddr"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpGet]
        public ActionResult SetupAddrEdit(string idaddr="")
        {
            SetupAddrViewModel model = new SetupAddrViewModel();
            if (!string.IsNullOrEmpty(idaddr))
            {
                model = SetupAddrService.GetViewModel(idaddr);
            }
            else
            {
                model.inuse = true;
            }

            ViewData["SetupSelectItems"] = SetupService.SelectItems();
            ViewData["MercTypeSelectItems"] = MercTypeService.SelectItems();

            return View(model);
        }

        /// <summary>
        /// 新增\编辑报装地址
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpPost]
        public ActionResult SetupAddrEdit(SetupAddrViewModel model)
        {
            SetupAddr setupAddr = new SetupAddr();
            model.CopyToBase(setupAddr);
            Setup setup = SetupService.Get(model.idsetup);
            setupAddr.idtype = setup.idtype;
            if (string.IsNullOrEmpty(setupAddr.idaddr))
            {
                setupAddr.idaddr = Pub.ID().ToString();
                
                //新增
                setupAddr = SetupAddrService.Add(setupAddr);
            }
            else
            {
                //编辑
                setupAddr = SetupAddrService.Edit(setupAddr);
            }

            //修改后重新加载
            model.CopyFromBase(setupAddr);

            ModelState.AddModelError("", "保存成功.");

            ViewData["SetupSelectItems"] = SetupService.SelectItems();
            ViewData["MercTypeSelectItems"] = MercTypeService.SelectItems();

            return View(model);
        }


        /// <summary>
        /// 启用或者禁用报装地址
        /// </summary>
        /// <param name="idaddr"></param>
        /// <param name="enable"></param>
        /// <param name="isAjax"></param>
        /// <returns></returns>
        [HttpPost]
        [ManageLoginValidation]
        public ActionResult SetupAddrEnable(string idaddr, bool enable, bool isAjax)
        {
            ResultModel<SetupAddrViewModel> resultEntity = new ResultModel<SetupAddrViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "成功";
            try
            {
                SetupAddr setupAddr = SetupAddrService.Get(idaddr);
                setupAddr.inuse = enable;
                SetupAddrService.Edit(setupAddr);
            }
            catch (Exception ex)
            {
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = ex.ToString();
            }

            return Content(resultEntity.SerializeToJson());
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
            List<SpecViewModel> entities = SpecService.GetSpecsByIdMerc(idmerc);
            List<SpecViewModel> viewModels = entities.Pager<SpecViewModel>(pageIndex, pageSize, out pageCount);
            
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
        public ActionResult SpecEnable(string idspec, bool enable, bool isAjax)
        {
            ResultModel<SpecViewModel> resultEntity = new ResultModel<SpecViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "成功";
            try
            {
                Spec spec = SpecService.Get(idspec);
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
        /// <param name="idmerc"></param>
        /// <param name="idspec"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpGet]
        public ActionResult SpecEdit(int idmerc, string idspec = "")
        {
            SpecViewModel model = new SpecViewModel();
            if (!string.IsNullOrEmpty(idspec))
            {
                model = SpecService.GetSpec(idspec);
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
            if (string.IsNullOrEmpty( spec.idspec) )
            {
                spec.idspec = Pub.ID().ToString();
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
                        StringBuilder initialPreview = new StringBuilder();
                        StringBuilder initialPreviewConfig = new StringBuilder();
                        initialPreviewConfig.Append(",\"initialPreviewConfig\":[");
                        initialPreview.Append("{\"initialPreview\":[");
                        for (int k = 0; k < resultEntity.Content.Count; k++)
                        {
                            if (k == 0) {
                                initialPreview.AppendFormat("\"{0}\"",Url.Content(resultEntity.Content[k].Path));
                                initialPreviewConfig.Append("{\"url\":\"" + Url.Action("DeleteMercImage", "Manage") + "\"}");
                            }
                            else {
                                initialPreview.AppendFormat("\",{0}\"", Url.Content(resultEntity.Content[k].Path));
                                initialPreviewConfig.Append(",{\"url\":\""+ Url.Action("DeleteMercImage", "Manage") + "\"}");
                            }
                        }
                        initialPreview.Append("]");
                        initialPreviewConfig.Append("]");
                        initialPreview.Append(initialPreviewConfig.ToString());
                        initialPreview.Append("}");
                        return Content(initialPreview.ToString());
                    }

                }
                else
                {
                    resultEntity.Code = ResponseCodeType.Fail;
                    resultEntity.Message = "文件上传失败.";
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
        public ActionResult DeleteMercImage(int mercImageId=0,int idmerc=0,bool isAjax=true)
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            ResultModel<MercImageViewModel> resultEntity = new ResultModel<MercImageViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "文件删除成功";

            //try {
            //    MercImage image = MercImageService.GetMercImage(mercImageId);
            //    if (image==null) {
            //        resultEntity.Code = ResponseCodeType.Fail;
            //        resultEntity.Message = "该商品图片已经不存在，请刷新页面重试.";
            //        return Content(resultEntity.SerializeToJson());
            //    }
            //    try {
            //        if (!string.IsNullOrEmpty(image.Path) && System.IO.File.Exists(Server.MapPath(image.Path))) {
            //            System.IO.File.Delete(Server.MapPath(image.Path));
            //        }
            //        MercImageService.Delete(mercImageId);
            //        MercService.SetDefaultMercImage(idmerc);
            //    }
            //    catch (Exception ex) {
            //        resultEntity.Code = ResponseCodeType.Fail;
            //        resultEntity.Message = "文件删除失败.";
            //        return Content(resultEntity.SerializeToJson());
            //    }
                
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex.ToString());
            //    resultEntity.Code = ResponseCodeType.Fail;
            //    resultEntity.Message = "文件删除失败.";
            //    return Content(resultEntity.SerializeToJson());
            //}
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
        /// 管理员列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult ManageUserList(int pageIndex = 0)
        {
            int pageCount = 0;
            int pageSize = 10;
            List<ManageUser> entities = ManageUserService.GetALL();
            List<ManageUser> pageList = entities.Pager<ManageUser>(pageIndex, pageSize, out pageCount);
            
            List<ManageUserViewModel> viewModels = pageList.Select(model =>
            {
                ManageUserViewModel viewModel = new ManageUserViewModel();
                viewModel.CopyFromBase(model);
                return viewModel;
            }).ToList();

            ViewData["pageCount"] = pageCount;
            ViewData["pageIndex"] = pageIndex;


            return View(viewModels);
        }


        /// <summary>
        /// 创建/编辑管理员
        /// </summary>
        /// <param name="idmerc"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpGet]
        public ActionResult ManageUserEdit(int manageUserId = 0)
        {
            ManageUserViewModel model = new ManageUserViewModel();
            if (manageUserId > 0)
            {
                ManageUser manageUser = ManageUserService.Get(manageUserId);
                if (manageUser != null) { model.CopyFromBase(manageUser); }
            }
            else
            {
                model.inuse = true;
            }
            
            
            return View(model);
        }

        /// <summary>
        /// 创建/编辑管理员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpPost]
        public ActionResult ManageUserEdit(ManageUserViewModel model)
        {

            ManageUser manageUser = new ManageUser();
            model.CopyToBase(manageUser);
            if (manageUser.ManageUserId == 0)
            {
                //新增
                manageUser = ManageUserService.Add(manageUser);
            }
            else
            {
                //编辑
                manageUser = ManageUserService.Edit(manageUser);
            }

            //修改后重新加载
            model.CopyFromBase(manageUser);

            ModelState.AddModelError("", "保存成功.");
            return View(model);
        }

        /// <summary>
        /// 搜索工人
        /// </summary>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult SearchManageUsers(string userName, string bindOrderNo, bool isAjax) {
            List<ManageUser> entities = ManageUserService.SearchByUserName(userName);
            List<ManageUserViewModel> viewModels = entities.Select(model => {
                ManageUserViewModel viewModel = new ManageUserViewModel();
                viewModel.CopyFromBase(model);
                return viewModel;
            }).ToList();

            ViewData["bindOrderNo"] = bindOrderNo;

            return View(viewModels);
        }

        /// <summary>
        /// 指派任务
        /// </summary>
        /// <param name="bindOrderNo"></param>
        /// <param name="manageUserIds"></param>
        /// <param name="isAjax"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult AssignTask(string bindOrderNo,string manageUserIds, bool isAjax,string notes="") {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            ResultModel<ManageUserViewModel> resultEntity = new ResultModel<ManageUserViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "报装订单指派工人成功";
            try {
                MyOrder order = MyOrderService.GetOrder(Convert.ToInt64(bindOrderNo));
                User user = null;
                if (order!=null) {
                    user = UserBll.Get(order.iduser);
                }

                Task task = new Task();
                task.idtask = Pub.ID().ToString();
                task.orderno = bindOrderNo;
                task.cretime = DateTime.Now;
                task.iduser= (user!=null?user.iduser.ToString():"");
                task.name = (user != null ? user.name : "");
                task.contact= (user != null ? user.name : "");
                task.phone = (user != null ? user.phone : "");
                task.idsend = ((ManageUser)Session["ManageUser"]).ManageUserId.ToString();
                task.send = ((ManageUser)Session["ManageUser"]).UserName;
                task.inuse = true;
                task.notes = notes;
                task.text = "报装";
                task.title = order != null ? order.merc + "-报装" : "报装";
                if (order != null) {
                    task.accpeptime = order.cretime;
                }
                task.status = TCom.Model.Task.TaskStatus.WaitPress;
                task.tasktype = TCom.Model.Task.TaskType.Setup;

                Task newTask= TaskService.Add(task);
                List<ManageUser> manageUsers = ManageUserService.GetALL();
                if (newTask!=null&&!string.IsNullOrEmpty(manageUserIds)) {
                    List<TaskRecver> taskRecvers = new List<TaskRecver>();
                    string[] userArray = manageUserIds.Split(',');
                    for (int i = 0; i < userArray.Count(); i++) {
                        ManageUser manageUser = manageUsers.Where(en => en.ManageUserId == Convert.ToInt32(userArray[i])).First();
                        taskRecvers.Add(new TaskRecver() {
                            idrecver = Pub.ID().ToString(),
                            idtask = newTask.idtask,
                            mcode = userArray[i],
                            mname= manageUser==null?"": manageUser.UserName,
                            cretime=DateTime.Now,
                            inuse=true
                        });
                    }
                    TaskRecverService.AddMuil(taskRecvers);
                }

            }
            catch (Exception ex) {
                log.Error(ex.ToString());
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = "报装订单指派工人失败";
            }
            return Content(resultEntity.SerializeToJson());
        }

        /// <summary>
        /// 启用或者禁用管理员
        /// </summary>
        /// <param name="ManageUserId"></param>
        /// <param name="enable"></param>
        /// <param name="isAjax"></param>
        /// <returns></returns>
        [HttpPost]
        [ManageLoginValidation]
        public ActionResult ManageUserEnable(int ManageUserId, bool enable, bool isAjax)
        {
            ResultModel<ManageUserViewModel> resultEntity = new ResultModel<ManageUserViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "成功";
            try
            {
                ManageUser manageUser = ManageUserService.Get(ManageUserId);
                manageUser.inuse = enable;
                ManageUserService.Edit(manageUser);
            }
            catch (Exception ex)
            {
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = ex.ToString();
            }

            return Content(resultEntity.SerializeToJson());
        }

        /// <summary>
        /// 搜索用户(用于绑定工人微信)
        /// </summary>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult SearchUsers(string phone,int bindManageUserId,bool isAjax) {
            List<TCom.EF.User> entities = UserBll.SearchByPhone(phone);
            List<UserViewModel> viewModels = entities.Select(model => {
                UserViewModel viewModel = new UserViewModel();
                viewModel.CopyFromBase(model);
                return viewModel;
            }).ToList();

            ViewData["bindManageUserId"] = bindManageUserId;

            return View(viewModels);
        }

        [ManageLoginValidation]
        public ActionResult BindManageUserWechat(int manageUserId,long iduser,bool isAjax) {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            ResultModel<ManageUserViewModel> resultEntity = new ResultModel<ManageUserViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "绑定微信成功";
            try {
                ManageUser manageUser = ManageUserService.Get(manageUserId);
                TCom.EF.User user = UserBll.Get(iduser);
                manageUser.idweixin = user.idweixin;
                ManageUserService.Edit(manageUser);
            }
            catch (Exception ex) {
                log.Error(ex.ToString());
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = "绑定微信失败";
            }
            return Content(resultEntity.SerializeToJson());
        }


        /// <summary>
        /// 公告通知列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult NoticeList(int pageIndex = 0) {
            int pageCount = 0;
            int pageSize = 10;
            List<Notice> entities = NoticeService.GetALL();
            List<Notice> pageList = entities.Pager<Notice>(pageIndex, pageSize, out pageCount);


            List<NoticeViewModel> viewModels = pageList.Select(model => {
                NoticeViewModel viewModel = new NoticeViewModel();
                viewModel.CopyFromBase(model);
                return viewModel;
            }).ToList();

            ViewData["pageCount"] = pageCount;
            ViewData["pageIndex"] = pageIndex;

            return View(viewModels);
        }

        /// <summary>
        /// 创建/编辑公告通知
        /// </summary>
        /// <param name="idnotice"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpGet]
        public ActionResult NoticeEdit(string idnotice = "") {
            NoticeViewModel model = new NoticeViewModel();
            if (!string.IsNullOrEmpty(idnotice)) {
                Notice notice = NoticeService.Get(idnotice);
                if (notice != null) { model.CopyFromBase(notice); }
            }
            else {
          
            }
            return View(model);
        }

        /// <summary>
        /// 编辑公告通知
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [ManageLoginValidation]
        [HttpPost]
        public ActionResult NoticeEdit(NoticeViewModel model) {

            Notice notice = new Notice();
            model.CopyToBase(notice);
            if (string.IsNullOrEmpty(notice.idnotice) ) {
                notice.publish_time = DateTime.Now;
                notice.idnotice = Pub.ID().ToString();
                //新增
                notice = NoticeService.Add(notice);
            }
            else {
                //编辑
                notice = NoticeService.Edit(notice);
            }

            ModelState.AddModelError("", "保存成功.");
            return View(model);
        }

        /// <summary>
        /// 广告类型列表
        /// </summary>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult AdvertiseTypeList()
        {
            List<AdvertiseType> entities = AdvertiseTypeService.GetALL();
   
            List<AdvertiseTypeViewModel> viewModels = entities.Select(model =>
            {
                AdvertiseTypeViewModel viewModel = new AdvertiseTypeViewModel();
                viewModel.CopyFromBase(model);
                return viewModel;
            }).ToList();

            return View(viewModels);
        }


        /// <summary>
        /// 启用或者禁用广告类型
        /// </summary>
        /// <param name="idat"></param>
        /// <param name="enable"></param>
        /// <param name="isAjax"></param>
        /// <returns></returns>
        [HttpPost]
        [ManageLoginValidation]
        public ActionResult AdvertiseTypeEnable(string idat, bool enable, bool isAjax)
        {
            ResultModel<AdvertiseTypeViewModel> resultEntity = new ResultModel<AdvertiseTypeViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "成功";
            try
            {
                AdvertiseType advertiseType = AdvertiseTypeService.Get(idat);
                advertiseType.inuse = enable;
                AdvertiseTypeService.Edit(advertiseType);
            }
            catch (Exception ex)
            {
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = ex.ToString();
            }

            return Content(resultEntity.SerializeToJson());
        }

        /// <summary>
        /// 新增\编辑广告类型
        /// </summary>
        /// <param name="idat"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpGet]
        public ActionResult AdvertiseTypeEdit(string idat = "")
        {
            AdvertiseTypeViewModel model = new AdvertiseTypeViewModel();
            if (!string.IsNullOrEmpty(idat))
            {
                AdvertiseType advertiseType = AdvertiseTypeService.Get(idat);
                if (idat != null) { model.CopyFromBase(advertiseType); }
            }
            else
            {
                model.inuse = true;
            }

            return View(model);
        }

        /// <summary>
        /// 新增\编辑广告类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpPost]
        public ActionResult AdvertiseTypeEdit(AdvertiseTypeViewModel model)
        {
            AdvertiseType advertiseType = new AdvertiseType();
            model.CopyToBase(advertiseType);
            if (string.IsNullOrEmpty( advertiseType.idat))
            {
                advertiseType.idat = Pub.ID().ToString();
                advertiseType.createtime = DateTime.Now;
                //新增
                advertiseType = AdvertiseTypeService.Add(advertiseType);
            }
            else
            {
                //编辑
                advertiseType = AdvertiseTypeService.Edit(advertiseType);
            }

            //修改后重新加载
            model.CopyFromBase(advertiseType);

            ModelState.AddModelError("", "保存成功.");

            return View(model);
        }


        /// <summary>
        /// 广告列表
        /// </summary>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult AdvertiseList(DateTime? sdate, DateTime? edate, string idat = "", string title = "",int pageIndex=0)
        {
            int pageCount = 0;
            int pageSize = 10;
           
            List<AdvertiseViewModel> entities = AdvertiseService.SearchViewModels(sdate, edate, idat, title);
            List<AdvertiseViewModel> viewModels = entities.Pager<AdvertiseViewModel>(pageIndex, pageSize, out pageCount);

            RouteData.Values.Add("sdate", sdate);
            RouteData.Values.Add("edate", edate);
            RouteData.Values.Add("idat", idat);
            RouteData.Values.Add("title", title);

            ViewData["pageCount"] = pageCount;
            ViewData["pageIndex"] = pageIndex;

            ViewData["sdate"] = sdate;
            ViewData["edate"] = edate;
            ViewData["idat"] = idat;
            ViewData["title"] = title;

            List<SelectItemViewModel<string>> advertiseTypes= AdvertiseTypeService.SelectItems();
            ViewData["advertiseTypes"] = advertiseTypes;

            return View(viewModels);
        }


        /// <summary>
        /// 启用或者禁用广告
        /// </summary>
        /// <param name="idav"></param>
        /// <param name="enable"></param>
        /// <param name="isAjax"></param>
        /// <returns></returns>
        [HttpPost]
        [ManageLoginValidation]
        public ActionResult AdvertiseEnable(string idav, bool enable, bool isAjax)
        {
            ResultModel<AdvertiseViewModel> resultEntity = new ResultModel<AdvertiseViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "成功";
            try
            {
                Advertise advertise = AdvertiseService.Get(idav);
                advertise.inuse = enable;
                AdvertiseService.Edit(advertise);
            }
            catch (Exception ex)
            {
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = ex.ToString();
            }

            return Content(resultEntity.SerializeToJson());
        }

        /// <summary>
        /// 新增\编辑广告
        /// </summary>
        /// <param name="idav"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpGet]
        public ActionResult AdvertiseEdit(string idav = "")
        {
            AdvertiseViewModel model = new AdvertiseViewModel();
            if (!string.IsNullOrEmpty(idav))
            {
                Advertise advertise = AdvertiseService.Get(idav);
                if (advertise != null) { model.CopyFromBase(advertise); }
            }
            else
            {
                model.inuse = true;
            }
            List<SelectItemViewModel<string>> advertiseTypes = AdvertiseTypeService.SelectItems();
            ViewData["advertiseTypes"] = advertiseTypes;

            return View(model);
        }

        /// <summary>
        /// 新增\编辑广告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpPost]
        public ActionResult AdvertiseEdit(AdvertiseViewModel model)
        {
            Advertise advertise = new Advertise();
            model.CopyToBase(advertise);
            if (string.IsNullOrEmpty(advertise.idav))
            {
                advertise.idav = Pub.ID().ToString();
                advertise.cretime = DateTime.Now;
                //新增
                advertise = AdvertiseService.Add(advertise);
            }
            else
            {
                //编辑
                advertise = AdvertiseService.Edit(advertise);
            }

            //修改后重新加载
            model.CopyFromBase(advertise);

            List<SelectItemViewModel<string>> advertiseTypes = AdvertiseTypeService.SelectItems();
            ViewData["advertiseTypes"] = advertiseTypes;

            ModelState.AddModelError("", "保存成功.");

            return View(model);
        }

        [ManageLoginValidation]
        public ActionResult UploadAdvertiseImage(string idav="")
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            ResultModel<AdvertiseViewModel> resultEntity = new ResultModel<AdvertiseViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "文件上传成功";
            string GUID = System.Guid.NewGuid().ToString();
            string path = "~/Resource/Images/Advertise/";
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
                    resultEntity.Content = new List<AdvertiseViewModel>();
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
                        AdvertiseViewModel model = new AdvertiseViewModel()
                        {
                            idav = idav,
                            img = path + filename
                        };

                        resultEntity.Content.Add(model);
                        i++;
                        StringBuilder initialPreview = new StringBuilder();
                        StringBuilder initialPreviewConfig = new StringBuilder();
                        initialPreviewConfig.Append(",\"initialPreviewConfig\":[");
                        initialPreview.Append("{\"initialPreview\":[");
                        for (int k = 0; k < resultEntity.Content.Count; k++)
                        {
                            if (k == 0)
                            {
                                initialPreview.AppendFormat("\"{0}\"", Url.Content(resultEntity.Content[k].img));
                                initialPreviewConfig.Append("{\"url\":\"" + Url.Action("DeleteAdvertiseImage", "Manage") + "\"}");
                            }
                            else
                            {
                                initialPreview.AppendFormat("\",{0}\"", Url.Content(resultEntity.Content[k].img));
                                initialPreviewConfig.Append(",{\"url\":\"" + Url.Action("DeleteAdvertiseImage", "Manage") + "\"}");
                            }
                        }
                        initialPreview.Append("]");
                        initialPreviewConfig.Append("]");
                        initialPreview.Append(initialPreviewConfig.ToString());
                        initialPreview.Append("}");
                        return Content(initialPreview.ToString());
                    }

                }
                else
                {
                    resultEntity.Code = ResponseCodeType.Fail;
                    resultEntity.Message = "文件上传失败.";
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
        public ActionResult DeleteAdvertiseImage(string idav = "", bool isAjax = true)
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            ResultModel<AdvertiseViewModel> resultEntity = new ResultModel<AdvertiseViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "文件删除成功";
            return Content(resultEntity.SerializeToJson());

        }


        /// <summary>
        /// 城市列表
        /// </summary>
        /// <returns></returns>
        [ManageLoginValidation]
        public ActionResult CityList(int pageIndex = 0)
        {
            int pageCount = 0;
            int pageSize = 10;
            
            List<City> entities = CityService.GetALL();
            
            List<City> pageList = entities.Pager<City>(pageIndex, pageSize, out pageCount);
            List<CityViewModel> viewModels = pageList.Select(mod => {
                CityViewModel viewModel = new CityViewModel();
                viewModel.CopyFromBase(mod);
                return viewModel;
            }).ToList();

            ViewData["pageCount"] = pageCount;
            ViewData["pageIndex"] = pageIndex;
            
            return View(viewModels);
        }

        /// <summary>
        /// 启用或者禁用广告
        /// </summary>
        /// <param name="idcity"></param>
        /// <param name="enable"></param>
        /// <param name="isAjax"></param>
        /// <returns></returns>
        [HttpPost]
        [ManageLoginValidation]
        public ActionResult CityEnable(string idcity, bool enable, bool isAjax)
        {
            ResultModel<CityViewModel> resultEntity = new ResultModel<CityViewModel>();
            resultEntity.Code = ResponseCodeType.Success;
            resultEntity.Message = "成功";
            try
            {
                City city = CityService.Get(idcity);
                city.inuse = enable;
                CityService.Edit(city);
            }
            catch (Exception ex)
            {
                resultEntity.Code = ResponseCodeType.Fail;
                resultEntity.Message = ex.ToString();
            }

            return Content(resultEntity.SerializeToJson());
        }

        /// <summary>
        /// 新增\编辑广告
        /// </summary>
        /// <param name="idav"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpGet]
        public ActionResult CityEdit(string idcity = "")
        {
            CityViewModel model = new CityViewModel();
            if (!string.IsNullOrEmpty(idcity))
            {
                City city = CityService.Get(idcity);
                if (city != null) { model.CopyFromBase(city); }
            }
            else
            {
                model.inuse = true;
            }

            return View(model);
        }

        /// <summary>
        /// 新增\编辑广告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ManageLoginValidation]
        [HttpPost]
        public ActionResult CityEdit(CityViewModel model)
        {
            City city = new City();
            model.CopyToBase(city);
            if (string.IsNullOrEmpty(city.idcity))
            {
                city.idcity = Pub.ID().ToString();
                //新增
                city = CityService.Add(city);
            }
            else
            {
                //编辑
                city = CityService.Edit(city);
            }

            //修改后重新加载
            model.CopyFromBase(city);
            ModelState.AddModelError("", "保存成功.");
            return View(model);
        }


        public ActionResult CitiesCheckBoxList(string idmodule,ModuleType moduleType) {
            List<CityRelation> cityRelations= CityRelationService.GetByModuleId(idmodule, moduleType);
            List<CityViewModel> viewModels = new List<CityViewModel>();
            List<City> city = CityService.GetALL();
            if (city.Count>0&& city != null) {
                viewModels = city.Select(mod => {
                    CityViewModel model = new CityViewModel();
                    model.CopyFromBase(mod);
                    model.IsCheck= cityRelations.Where(en => en.idcity == mod.idcity).Count() > 0 ? true : false;
                    return model;
                }).ToList();
            }

            return View(viewModels);
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
