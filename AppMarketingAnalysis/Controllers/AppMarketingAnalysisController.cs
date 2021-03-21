using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AppMarketingAnalysis.Service;
using AppMarketingAnalysis.Model;

namespace AppMarketingAnalysis.Controllers
{
    public class AppMarketingAnalysisController : Controller
    {
        private IAppMarketingAnalysisService AppMarketingAnalysisService { get; set; }

        /// <summary>
        /// 開啟Index首頁
        /// GET
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 拿取下拉式選單資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDropDownListClassData(string target)
        {
            List<SelectListItem> DDLData = AppMarketingAnalysisService.SetDropDownListData(target);  //拿取下拉式選單資料
            return Json(DDLData);
        }

        /// <summary>
        /// 拿取AppNameAutoComplete的資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AutoCompleteBookName()
        {
            var AppNameData = AppMarketingAnalysisService.GetAppNameData(); //拿取APP_NAME資料
            return Json(AppNameData);
        }

        /// <summary>
        /// 查詢APP
        /// </summary>
        /// <param name="amad"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Search(AppMarketingAnalysisData amad) //接收search的資料
        {
            List<AppMarketingAnalysisData> searchResult =  AppMarketingAnalysisService.GetAppBySearch(amad);
            return Json(searchResult);
        }
    }
}