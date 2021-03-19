using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMarketingAnalysis.Model
{
    public class BookLendRecord
    {
        ///書籍編號
        [DisplayName("書籍編號")]
        public int BookId { get; set; }

        ///借閱日期
        [DisplayName("借閱日期")]
        public string LendDate { get; set; }

        /// 借閱人員編號
        [DisplayName("借閱人員編號")]
        public string KeeperId { get; set; }

        /// 英文姓名
        [DisplayName("英文姓名")]
        public string EName { get; set; }

        /// 中文姓名
        [DisplayName("中文姓名")]
        public string CName { get; set; }
    }
}
