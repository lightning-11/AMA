using System.Collections.Generic;
using System.Web.Mvc;

namespace AppMarketingAnalysis.Dao
{
    public interface ICodeDao
    {
        List<SelectListItem> SetDropDownListData(string target);
    }
}