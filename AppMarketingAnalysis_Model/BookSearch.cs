using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookSystem_Model
{
    public class BookSearch
    {
        ///書籍編號
        [DisplayName("書籍編號")]
        public int BookId { get; set; }

        /// 書名
        [DisplayName("書名")]
        [AllowHtml] ///可以輸入html標籤
        public string BookName { get; set; }

        /// 圖書類別
        [DisplayName("圖書類別")]
        public string BookClassId { get; set; }

        /// 借閱人
        [DisplayName("借閱人")]
        public string BookKeeper { get; set; }

        /// 借閱狀態
        [DisplayName("借閱狀態")]
        public string BookStatus { get; set; }
    }
}
