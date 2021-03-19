using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AppMarketingAnalysis.Service;
using AppMarketingAnalysis.Model;

namespace AppMarketingAnalysis.Controllers
{
    public class AppMarketingAnalysisController : Controller
    {
        private ICodeService codeService { get; set; }
        private IAppMarketingAnalysisService AppMarketingAnalysisService { get; set; }

        // GET: Book
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 拿取BOOK_CLASS資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetDropDownListClassData()
        {
            List<SelectListItem> classData = codeService.SetDropDownListData("Class");  //拿取BOOK_CLASS資料
            return Json(classData);
        }

        /// <summary>
        /// 拿取MEMBER資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetDropDownListMemberData()
        {
            List<SelectListItem> memberData = codeService.SetDropDownListData("Member"); //拿取MEMBER資料
            return Json(memberData);
        }

        /// <summary>
        /// 拿取BOOK_STATUS資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetDropDownListStatusData()
        {
            List<SelectListItem> statusData = codeService.SetDropDownListData("Status"); //拿取BOOK_STATUS資料
            return Json(statusData);
        }

        /// <summary>
        /// 拿取BookNameAutoComplete的資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AutoCompleteBookName()
        {
            var BookNameData = AppMarketingAnalysisService.GetBookNameData(); //拿取BOOK_NAME資料
            return Json(BookNameData);
        }
        /// <summary>
        /// 查詢書籍
        /// </summary>
        /// <param name="bookSearch"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Search(BookSearch bookSearch) //接收search的資料
        {
            List<Book> searchResult =  AppMarketingAnalysisService.GetBookByCondtioin(bookSearch);
            return Json(searchResult);
        }

        /// <summary>
        /// 刪除書籍
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteBook(string bookId)
        {
            if (!int.TryParse(bookId, out int testNum)) //如果bookId不是數字的話
            {
                return Json("wrongId");
            }
            try
            {
                if (AppMarketingAnalysisService.CheckStatus(bookId))    //檢查借閱狀態是否為可以借出或不可借出
                {
                    AppMarketingAnalysisService.DeleteBookById(bookId); 
                    return this.Json(true);
                }
                else
                {
                    return this.Json("This book has been lent");
                }
            }
            catch (Exception)
            {
                return this.Json(false);
            }
        }

        /// <summary>
        /// 書籍新增的畫面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InsertBookView()
        {
            return View();
        }

        /// <summary>
        /// 新增書籍
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult InsertBook(Book book)
        {
            var BookIdByInsertResult = AppMarketingAnalysisService.InsertBook(book);
            return Json(BookIdByInsertResult);
        }

        /// <summary>
        /// 書籍編輯的畫面
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetBook(string bookId)
        {
            if (!int.TryParse(bookId, out int testNum))  //如果bookId不是數字的話
            {
                return Json("wrongId");
            }
            var book = AppMarketingAnalysisService.GetBookByBookId(bookId); ///前往獲得那筆資料的完整資訊
            return Json(book);
        }
       
        /// <summary>
        /// 編輯書籍
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditBook(Book book)
        {
            if(book.BookStatus=="B"|| book.BookStatus == "C")    //判斷是否為已借出或已借出(未領)
            {
                if(book.BookKeeper == null)
                {
                    return Json("編輯失敗");
                }
                else
                {
                    AppMarketingAnalysisService.InsertLendRecord(book); //新增借閱紀錄
                    AppMarketingAnalysisService.EditBook(book); //編輯書籍
                    return Json(true);
                }
            }
            else
            {
                AppMarketingAnalysisService.EditBook(book); //編輯書籍
                return Json(true);
            }
        }

        /// <summary>
        /// 借閱紀錄畫面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LendRecordView()
        {
            return View();
        }

        /// <summary>
        /// 獲取借閱紀錄資料
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LendRecord(string bookId)
        {
            if(!int.TryParse(bookId,out int testNum))    //如果bookId不是數字的話
            {
                return Json("wrongId");
            }
            var LendRecordResult = AppMarketingAnalysisService.GetLendRecordById(bookId);
            return Json(LendRecordResult);
        }
    }
}