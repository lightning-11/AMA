using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMarketingAnalysis.Model
{
    public class AppMarketingAnalysisData
    {
        //APP排序(由資料庫流水號自動產生)
        //[DisplayName("APP排序")]
        public int APP_SORT { get; set; }

        //APP名稱
        [DisplayName("APP名稱")]
        //[Required(ErrorMessage = "此欄位必填")]
        public string APP_NAME { get; set; }

        //APP分類
        [DisplayName("APP分類")]
        public string CATEGORY { get; set; }

        //平均評分
        [DisplayName("平均評分")]
        public int RATING { get; set; }

        //評分數量
        [DisplayName("評分數量")]
        public int RATING_COUNT { get; set; }

        //下載數量範圍
        [DisplayName("下載數量範圍")]
        public string INSTALLS_RANGE { get; set; }

        //下載數量
        [DisplayName("下載數量")]
        public int INSTALLS_COUNT { get; set; }

        //APP是免費還是付費
        [DisplayName("APP是免費還是付費")]
        public bool FREE { get; set; }

        //APP價格
        [DisplayName("APP價格")]
        public int PRICE { get; set; }

        //貨幣種類
        [DisplayName("貨幣種類")]
        public string CURRENCY { get; set; }

        //APP檔案大小
        [DisplayName("APP檔案大小")]
        public string SIZE { get; set; }

        //最小支援安卓版本
        [DisplayName("最小支援安卓版本")]
        public string MIN_ANDROID { get; set; }

        //開發者名稱
        [DisplayName("開發者名稱")]
        public string DEVELOPER_ID { get; set; }

        //APP發行日期
        [DisplayName("APP發行日期")]
        public string RELEASED { get; set; }

        //APP最後更新日期
        [DisplayName("APP最後更新日期")]
        public string LAST_UPDATED { get; set; }

        //APP分級
        [DisplayName("APP分級")]
        public string CONTENT_RATING { get; set; }

        //APP內是否有廣告
        [DisplayName("APP內是否有廣告")]
        public bool AD_SUPPORTED { get; set; }

        //APP內是否有購買機制
        [DisplayName("APP內是否有購買機制")]
        public bool IN_APP_PURCHASES { get; set; }

        //是否為編輯精選
        [DisplayName("是否為編輯精選")]
        public bool EDITOR_CHOICE { get; set; }

    }
}
