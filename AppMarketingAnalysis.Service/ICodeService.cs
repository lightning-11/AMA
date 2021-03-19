using System.Collections.Generic;
using System.Web.Mvc;

namespace AppMarketingAnalysis.Service
{
    public interface ICodeService
    {
        List<SelectListItem> SetDropDownListData(string target);
    }
}