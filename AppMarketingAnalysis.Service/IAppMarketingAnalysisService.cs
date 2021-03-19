using System.Collections.Generic;
using AppMarketingAnalysis.Model;

namespace AppMarketingAnalysis.Service
{
    public interface IAppMarketingAnalysisService
    {
        bool CheckStatus(string bookId);
        void DeleteBookById(string bookId);
        void EditBook(Book book);
        Book GetBookByBookId(string bookId);
        List<Book> GetBookByCondtioin(BookSearch arg);
        List<string> GetBookNameData();
        List<BookLendRecord> GetLendRecordById(string bookId);
        int InsertBook(Book book);
        void InsertLendRecord(Book book);
    }
}