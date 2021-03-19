using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AppMarketingAnalysis.Dao
{
    public class CodeDao : ICodeDao
    {
        /// 取得DB連線字串
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return AppMarketingAnalysis.Common.ConfigTool.GetDBConnectionString("DBConn");
        }

        /// <summary>
        /// 設定下拉式選單資料
        /// </summary>
        /// <param name="target">要拿取的資料</param>
        /// <returns></returns>
        public List<SelectListItem> SetDropDownListData(string target)
        {
            DataTable dt = new DataTable();
            string sql = "";
            if (target == "Class")  //拿取書籍類別資料
            {
                sql = @"Select bc.BOOK_CLASS_NAME as Text, bc.BOOK_CLASS_ID as Value
                               From BOOK_CLASS as bc";
            }
            else if (target == "Status")    //拿取借閱狀態資料
            {
                sql = @"Select bcd.CODE_NAME as Text, bcd.CODE_ID as Value
                               From BOOK_CODE as bcd
                               Where bcd.CODE_TYPE = 'BOOK_STATUS'";
            }
            else if (target == "Member")    //拿取USER資料
            {
                sql = @"Select (m.USER_ENAME + '-' + m.USER_CNAME ) as Text, m.USER_ID as Value
                               From MEMBER_M as m";
            }
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapDropDownListData(dt);
        }

        /// <summary>
        /// Maping 代碼資料
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<SelectListItem> MapDropDownListData(DataTable dt)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["Text"].ToString(),
                    Value = row["Value"].ToString()
                });
            }
            return result;
        }
    }
}
