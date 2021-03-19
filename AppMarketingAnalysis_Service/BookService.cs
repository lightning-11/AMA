using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSystem_Service
{
    class BookService
    {
        public List<string> GetBookNameData()
        {
            BookSystem_Dao bookDao = new BookSystem_Dao.BookDao();
            return 
        }
        public List<BookSystem_Model.Book> GetBookByCondtioin(BookSystem_Model.BookSearch arg)
        {

        }
        public bool CheckStatus(String bookId)
        {

        }
        public void DeleteBookById(string bookId)
        {

        }
        public int InsertBook(BookSystem_Model.Book book)
        {

        }
        public BookSystem_Model.Book GetBookByBookId(String bookId)
        {

        }
        public void EditBook(BookSystem_Model.Book book)
        {

        }
        public List<BookSystem_Model.BookLendRecord> GetLendRecordById(String bookId)
        {

        }
        public void InsertLendRecord(BookSystem_Model.Book book)
        {

        }

    }
}
