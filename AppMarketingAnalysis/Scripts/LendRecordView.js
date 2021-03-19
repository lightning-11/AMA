$(document).ready(function () {
    $("#NotFind").hide();   //隱藏NotFind
    var url = location.href;    //取得網址字串
    var bookId = "";
    if (url.indexOf('?') != -1) {   //尋找網址列中是否有資料傳遞(QueryString)
        var ary = url.split('?')[1].split('&'); //將各自的參數資料切割放進ary中
        for (i = 0; i <= (ary.length - 1); i++) {   //下迴圈去搜尋每個資料參數
            if (ary[i].split('=')[0] == 'id') {
                bookId = ary[i].split('=')[1];
            }
        }
        $.ajax({
            url: "/AppMarketingAnalysis/LendRecord",
            dataType: "json",
            type: "post",
            data: "bookId=" + bookId,    //將此資料傳入LendRecord
            success: function (e) {
                if (e == "Wrong ID") {
                    alert("ID錯誤");
                    window.location.href = "/AppMarketingAnalysis/Index";
                }
            }
        }).done(function (lendRecordData) {
            //初始查詢的kendoGrid
            $("#LendRecordGrid").kendoGrid({
                dataSource: {
                    data: lendRecordData,
                    schema: {
                        model: {
                            fields: {
                                BookId: { type: "int" },
                                LendDate: { type: "string" },
                                KeeperId: { type: "string" },
                                EName: { type: "string" },
                                CName: { type: "string" }
                            }
                        }
                    },
                    pageSize: 5,
                },
                height: 550,
                sortable: true,
                pageable: {
                    input: true,
                    numeric: false
                },
                columns: [
                    { field: "BookId", title: "書籍編號", hidden: true },
                    { field: "LendDate", title: "借閱日期", width: "10%" },
                    { field: "KeeperId", title: "借閱人員編號", width: "50%" },
                    { field: "EName", title: "英文姓名", width: "10%" },
                    { field: "CName", title: "中文姓名", width: "15%" },
                ]
            });
            if ($("#LendRecordGrid").data("kendoGrid").dataSource.data().length == 0) {
                $("#LendRecordGrid").hide();  //隱藏Grid
                $("#NotFind").show();   //顯示NotFind
            }
        })
    }
});