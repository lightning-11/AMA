using System.Collections.Generic;
using AppMarketingAnalysis.Model;
using System.Web.Mvc;

namespace AppMarketingAnalysis.Dao
{
    public interface IAppMarketingAnalysisDao
    {
        List<string> GetAppNameData();
        List<AppMarketingAnalysisData> GetAppBySearch(AppMarketingAnalysisData amad);
        List<SelectListItem> SetDropDownListData(string target);
    }
}