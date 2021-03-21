using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AppMarketingAnalysis.Service
{
    public class AppMarketingAnalysisService : IAppMarketingAnalysisService
    {
        private AppMarketingAnalysis.Dao.IAppMarketingAnalysisDao AppMarketingAnalysisDao { get; set; }

        //拿取APP_NAME的資料
        public List<string> GetAppNameData()
        {
            return AppMarketingAnalysisDao.GetAppNameData();
        }

        //依照搜尋條件取得App資料
        public List<AppMarketingAnalysis.Model.AppMarketingAnalysisData> GetAppBySearch(AppMarketingAnalysis.Model.AppMarketingAnalysisData amad)
        {
            return AppMarketingAnalysisDao.GetAppBySearch(amad);
        }

        //設定下拉式選單資料
        public List<SelectListItem> SetDropDownListData(string target)
        {
            return AppMarketingAnalysisDao.SetDropDownListData(target);
        }
    }
}
