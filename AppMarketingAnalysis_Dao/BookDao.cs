using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSystem_Dao
{
    public class BookDao
    {
        /// 取得DB連線字串
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return BookSystem_Common.ConfigTool.GetDBConnectionString("DBConn");
        }

        /// <summary>
        /// 拿去BookName的資料
        /// </summary>
        /// <returns></returns>
        public List<string> GetBookNameData()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT bd.BOOK_NAME
                           FROM BOOK_DATA as bd";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            List<string> bookName = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                bookName.Add(row.Field<String>("BOOK_NAME"));
            }
            return bookName;
        }

        /// <summary>
        /// 依照條件取得書籍資料
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public List<BookSystem_Model.Book> GetBookByCondtioin(BookSystem_Model.BookSearch arg)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT bd.BOOK_ID,bc.BOOK_CLASS_NAME, bd.BOOK_NAME, 
                                        CONVERT(varchar, bd.BOOK_BOUGHT_DATE, 111) as BOOK_BOUGHT_DATE,
                                        bcd.CODE_NAME, (m.USER_ENAME + '-' + m.USER_CNAME) AS USER_NAME, 
                                        bd.BOOK_PUBLISHER ,bd.BOOK_NOTE,bd.BOOK_AUTHOR
                                      FROM BOOK_DATA as bd 
	                                    LEFT JOIN MEMBER_M as m
	                                        ON (bd.BOOK_KEEPER = m.USER_ID)
	                                    INNER JOIN BOOK_CLASS as bc
	                                        ON (bd.BOOK_CLASS_ID = bc.BOOK_CLASS_ID)
                                        INNER JOIN BOOK_CODE as bcd
	                                        ON (bd.BOOK_STATUS = bcd.CODE_ID AND bcd.CODE_TYPE = 'BOOK_STATUS')
                                      Where (UPPER(bd.BOOK_NAME) LIKE UPPER('%'+@BookName+'%') OR @BookName = '') AND
                                        (bc.BOOK_CLASS_ID = @BookClassId OR @BookClassId='') AND
                                        (m.USER_ID  = @BookKeeper OR @BookKeeper='') AND
                                        (bcd.CODE_ID = @BookStatus OR @BookStatus='')
                                      ORDER BY BOOK_BOUGHT_DATE DESC";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookName", arg.BookName == null ? string.Empty : arg.BookName));
                cmd.Parameters.Add(new SqlParameter("@BookClassId", arg.BookClassId == null ? string.Empty : arg.BookClassId));
                cmd.Parameters.Add(new SqlParameter("@BookKeeper", arg.BookKeeper == null ? string.Empty : arg.BookKeeper));
                cmd.Parameters.Add(new SqlParameter("@BookStatus", arg.BookStatus == null ? string.Empty : arg.BookStatus));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookDataToList(dt);
        }

        /// <summary>
        /// 檢查借閱狀態
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public bool CheckStatus(String bookId)
        {
            DataTable dt = new DataTable();
            string checkSql = @"SELECT bd.BOOK_STATUS
                                                FROM BOOK_DATA bd
                                                WHERE bd.BOOK_ID=@BookId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(checkSql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookId", bookId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            if (dt.Rows[0]["BOOK_STATUS"].ToString() == "A" || dt.Rows[0]["BOOK_STATUS"].ToString() == "U")
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 刪除書籍
        /// </summary>
        /// <param name="bookId"></param>
        public void DeleteBookById(string bookId)
        {
            string sql = @"DELETE 
                                      FROM BOOK_DATA 
                                      WHERE BOOK_ID=@BookId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlTransaction sqltrans = conn.BeginTransaction();
                cmd.Parameters.Add(new SqlParameter("@BookId", bookId));
                cmd.Transaction = sqltrans;
                try
                {
                    cmd.ExecuteNonQuery();
                    sqltrans.Commit();
                }
                catch
                {
                    sqltrans.Rollback();
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 新增書籍
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public int InsertBook(BookSystem_Model.Book book)
        {
            string sql = @" INSERT INTO BOOK_DATA
						 (
							 BOOK_NAME, BOOK_AUTHOR, BOOK_PUBLISHER, BOOK_NOTE, 
                             BOOK_BOUGHT_DATE, BOOK_CLASS_ID, BOOK_STATUS
						 )
						VALUES
						(
							 @BookName,@BookAuthor, @BookPublisher, 
                             @BookNote, @BookBoughtDate, @BookClassId,'A'
						)
						Select SCOPE_IDENTITY()";   ///SCOPE_IDENTITY() 抓最新的流水號
            int bookId;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlTransaction sqltrans = conn.BeginTransaction();
                cmd.Parameters.Add(new SqlParameter("@BookName", book.BookName));
                cmd.Parameters.Add(new SqlParameter("@BookAuthor", book.BookAuthor));
                cmd.Parameters.Add(new SqlParameter("@BookPublisher", book.BookPublisher));
                cmd.Parameters.Add(new SqlParameter("@BookNote", book.BookNote));
                cmd.Parameters.Add(new SqlParameter("@BookBoughtDate", book.BookBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@BookClassId", book.BookClassId));
                cmd.Transaction = sqltrans;
                try
                {
                    bookId = Convert.ToInt32(cmd.ExecuteScalar());
                    sqltrans.Commit();
                }
                catch
                {  ///有return要throw
                    sqltrans.Rollback();
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
            return bookId;
        }

        /// <summary>
        /// 獲取欲編輯的圖書資料
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public BookSystem_Model.Book GetBookByBookId(String bookId)
        {
            BookSystem_Model.Book book = new BookSystem_Model.Book();
            DataTable dt = new DataTable();
            string sql = @"SELECT bd.BOOK_ID,bc.BOOK_CLASS_ID,bd.BOOK_NAME,
                                        CONVERT(varchar, bd.BOOK_BOUGHT_DATE, 111) as BOOK_BOUGHT_DATE,
                                        bd.BOOK_KEEPER, CODE_ID,bd.BOOK_AUTHOR,
                                        bd.BOOK_PUBLISHER ,bd.BOOK_NOTE
                                     FROM BOOK_DATA as bd 
	                                    LEFT JOIN MEMBER_M as m
	                                        ON (bd.BOOK_KEEPER = m.USER_ID)
	                                    INNER JOIN BOOK_CLASS as bc
	                                        ON (bd.BOOK_CLASS_ID = bc.BOOK_CLASS_ID)
                                        INNER JOIN BOOK_CODE as bcd
	                                        ON (bd.BOOK_STATUS = bcd.CODE_ID AND bcd.CODE_TYPE = 'BOOK_STATUS')
                                     Where bd.BOOK_ID = @BookId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookId", bookId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            foreach (DataRow row in dt.Rows)
            {
                book = new BookSystem_Model.Book
                {
                    BookClassId = row["BOOK_CLASS_ID"].ToString(),
                    BookName = row["BOOK_NAME"].ToString(),
                    BookBoughtDate = row["BOOK_BOUGHT_DATE"].ToString(),
                    BookStatus = row["CODE_ID"].ToString(),
                    BookKeeper = row["BOOK_KEEPER"].ToString(),
                    BookId = (int)row["BOOK_ID"],
                    BookPublisher = row["BOOK_PUBLISHER"].ToString(),
                    BookNote = row["BOOK_NOTE"].ToString(),
                    BookAuthor = row["BOOK_AUTHOR"].ToString(),
                };
            }
            return book;
        }

        /// <summary>
        /// 編輯書籍
        /// </summary>
        /// <param name="book"></param>
        public void EditBook(BookSystem_Model.Book book)
        {
            string sql = @"Update BOOK_DATA
                                        SET BOOK_NAME = @BookName, BOOK_AUTHOR = @BookAuthor,
                                                BOOK_PUBLISHER = @BookPublisher, BOOK_NOTE = @BookNote,
                                                BOOK_BOUGHT_DATE = @BookBoughtDate,
                                                BOOK_CLASS_ID = @BookClassId,
                                                BOOK_STATUS = @BookStatus,
                                                BOOK_KEEPER = @BookKeeper
                                        WHERE BOOK_ID = @BookId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlTransaction sqltrans = conn.BeginTransaction();
                cmd.Parameters.Add(new SqlParameter("@BookId", book.BookId));
                cmd.Parameters.Add(new SqlParameter("@BookName", book.BookName == null ? string.Empty : book.BookName));
                cmd.Parameters.Add(new SqlParameter("@BookAuthor", book.BookAuthor == null ? string.Empty : book.BookAuthor));
                cmd.Parameters.Add(new SqlParameter("@BookPublisher", book.BookPublisher == null ? string.Empty : book.BookPublisher));
                cmd.Parameters.Add(new SqlParameter("@BookNote", book.BookNote == null ? string.Empty : book.BookNote));
                cmd.Parameters.Add(new SqlParameter("@BookBoughtDate", book.BookBoughtDate == null ? string.Empty : book.BookBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@BookClassId", book.BookClassId == null ? string.Empty : book.BookClassId));
                cmd.Parameters.Add(new SqlParameter("@BookStatus", book.BookStatus == null ? string.Empty : book.BookStatus));
                cmd.Parameters.Add(new SqlParameter("@BookKeeper", book.BookKeeper == null ? string.Empty : book.BookKeeper));
                cmd.Transaction = sqltrans;
                try
                {
                    cmd.ExecuteNonQuery();
                    sqltrans.Commit();
                }
                catch
                {
                    sqltrans.Rollback();
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 借閱紀錄資料
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public List<BookSystem_Model.BookLendRecord> GetLendRecordById(String bookId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT  bd.BOOK_ID,blr.KEEPER_ID,
                                        CONVERT(varchar, blr.LEND_DATE, 111) as LEND_DATE,
                                        m.USER_ENAME,m.USER_CNAME
                                      FROM BOOK_DATA as bd 
	                                    INNER JOIN BOOK_LEND_RECORD as blr
		                                    ON (bd.BOOK_ID = blr.BOOK_ID)
	                                    LEFT JOIN MEMBER_M as m
		                                    ON (blr.KEEPER_ID = m.USER_ID)
                                      WHERE bd.BOOK_ID = @BookId
                                      ORDER BY blr.LEND_DATE DESC";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookId", bookId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookLendRecordToList(dt);
        }

        /// <summary>
        /// 新增借閱紀錄(修改)
        /// </summary>
        /// <param name="book"></param>
        public void InsertLendRecord(BookSystem_Model.Book book)
        {
            string sql = @"INSERT INTO BOOK_LEND_RECORD
						                    (
							                    BOOK_ID, KEEPER_ID, 
                                                LEND_DATE, CRE_DATE
						                    )
						               VALUES
						                    (
							                    @BookId, @BookKeeper, 
                                                GETDATE(), GETDATE()
						                    )";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlTransaction sqltrans = conn.BeginTransaction();
                cmd.Parameters.Add(new SqlParameter("@BookId", book.BookId));
                cmd.Parameters.Add(new SqlParameter("@BookKeeper", book.BookKeeper));
                cmd.Transaction = sqltrans;
                try
                {
                    cmd.ExecuteNonQuery();
                    sqltrans.Commit();
                }
                catch
                {
                    sqltrans.Rollback();
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// Map資料進List (BookData)
        /// <param name="BookDataTable"></param>
        /// <returns></returns>
        private List<BookSystem_Model.Book> MapBookDataToList(DataTable BookDataTable)
        {
            List<BookSystem_Model.Book> result = new List<BookSystem_Model.Book>();
            foreach (DataRow row in BookDataTable.Rows)
            {
                result.Add(new BookSystem_Model.Book()
                {
                    BookClassId = row["BOOK_CLASS_NAME"].ToString(),
                    BookName = row["BOOK_NAME"].ToString(),
                    BookBoughtDate = row["BOOK_BOUGHT_DATE"].ToString(),
                    BookStatus = row["CODE_NAME"].ToString(),
                    BookKeeper = row["USER_NAME"].ToString(),
                    BookId = (int)row["BOOK_ID"],
                    BookPublisher = row["BOOK_PUBLISHER"].ToString(),
                    BookNote = row["BOOK_NOTE"].ToString(),
                    BookAuthor = row["BOOK_AUTHOR"].ToString()
                });
            }
            return result;
        }

        /// Map資料進List (LendRecord)
        /// <param name="LendRecordTable"></param>
        /// <returns></returns>
        private List<BookSystem_Model.BookLendRecord> MapBookLendRecordToList(DataTable LendRecordTable)
        {
            List<BookSystem_Model.BookLendRecord> result = new List<BookSystem_Model.BookLendRecord>();
            foreach (DataRow row in LendRecordTable.Rows)   //讀每一個row
            {
                result.Add(new BookSystem_Model.BookLendRecord()   //將每一行row 加進去
                {
                    LendDate = row["LEND_DATE"].ToString(),
                    KeeperId = row["KEEPER_ID"].ToString(),
                    EName = row["USER_ENAME"].ToString(),
                    CName = row["USER_CNAME"].ToString(),
                    BookId = (int)row["BOOK_ID"]
                });
            }
            return result;
        }
    }
}
