using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AppMarketingAnalysis.Service
{
    public class CodeService : ICodeService
    {
        private AppMarketingAnalysis.Dao.ICodeDao codeDao { get; set; }

        public List<SelectListItem> SetDropDownListData(string target)
        {
            return codeDao.SetDropDownListData(target);
        }
    }
}
