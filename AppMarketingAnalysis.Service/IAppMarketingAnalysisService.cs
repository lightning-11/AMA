using System.Collections.Generic;
using System.Web.Mvc;
using AppMarketingAnalysis.Model;

namespace AppMarketingAnalysis.Service
{
    public interface IAppMarketingAnalysisService
    {
        List<string> GetAppNameData();
        List<AppMarketingAnalysisData> GetAppBySearch(AppMarketingAnalysisData amad);
        List<SelectListItem> SetDropDownListData(string target);
    }
}