using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMarketingAnalysis.Service
{
    public class AppMarketingAnalysisService : IAppMarketingAnalysisService
    {
        private AppMarketingAnalysis.Dao.IAppMarketingAnalysisDao AppMarketingAnalysisDao { get; set; }

        public List<string> GetBookNameData()
        {
            return AppMarketingAnalysisDao.GetBookNameData();
        }

        public List<AppMarketingAnalysis.Model.Book> GetBookByCondtioin(AppMarketingAnalysis.Model.BookSearch arg)
        {
            return AppMarketingAnalysisDao.GetBookByCondtioin(arg);
        }

        public bool CheckStatus(String bookId)
        {
            return AppMarketingAnalysisDao.CheckStatus(bookId);
        }

        public void DeleteBookById(string bookId)
        {
            AppMarketingAnalysisDao.DeleteBookById(bookId);
        }

        public int InsertBook(AppMarketingAnalysis.Model.Book book)
        {
            return AppMarketingAnalysisDao.InsertBook(book);
        }

        public AppMarketingAnalysis.Model.Book GetBookByBookId(String bookId)
        {
            return AppMarketingAnalysisDao.GetBookByBookId(bookId);
        }

        public void EditBook(AppMarketingAnalysis.Model.Book book)
        {
            AppMarketingAnalysisDao.EditBook(book);
        }

        public List<AppMarketingAnalysis.Model.BookLendRecord> GetLendRecordById(String bookId)
        {
            return AppMarketingAnalysisDao.GetLendRecordById(bookId);
        }

        public void InsertLendRecord(AppMarketingAnalysis.Model.Book book)
        {
            AppMarketingAnalysisDao.InsertBook(book);
        }
    }
}
