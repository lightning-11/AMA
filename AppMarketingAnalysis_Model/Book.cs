using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookSystem_Model
{
    public class Book
    {
        //書籍編號
        [DisplayName("書籍編號")]
        public int BookId { get; set; }

        // 書名
        [DisplayName("書名")]
        [Required(ErrorMessage = "此欄位必填")]
        //[AllowHtml]
        public string BookName { get; set; }
        // 圖書類別
        [DisplayName("圖書類別")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookClassId { get; set; }

        // 作者
        [DisplayName("作者")]
        [Required(ErrorMessage = "此欄位必填")]
        //[AllowHtml]
        public string BookAuthor { get; set; }

        // 購書日期
        [DisplayName("購書日期")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookBoughtDate { get; set; }

        // 出版商
        [DisplayName("出版商")]
        [Required(ErrorMessage = "此欄位必填")]
        //[AllowHtml]
        public string BookPublisher { get; set; }


        // 內容簡介
        [DisplayName("內容簡介")]
        [Required(ErrorMessage = "此欄位必填")]
        //[AllowHtml]
        public string BookNote { get; set; }

        // 借閱狀態
        [DisplayName("借閱狀態")]
        //[Required(ErrorMessage = "此欄位必填")]
        public string BookStatus { get; set; }

        // 借閱人
        [DisplayName("借閱人")]
        //[Required(ErrorMessage = "此欄位必填")]
        public string BookKeeper { get; set; }
    }
}
