var bookId = "";
//產生kendoValidator
$("#Validation").kendoValidator({
    rules: {
        datepicker: function (input) {  //制定kendoDatePciker的規則
            if (input.is("[data-role=datepicker]")) {   //選擇kendoDatePicker
                return input.data("kendoDatePicker").value();
            }
            else {
                return true;
            }
        },
        bookNoteMaxLength: function (e) {   //制定BookNote最大長度的規則
            if (e.is("[id=BookNote]")) {
                return ($("#BookNote").val().length <= 2400);
            }
            else {
                return true;
            }
        },
    },
    messages: {
        required: "請輸入{0}",
        datepicker: "請輸入有效日期! 格式為:年/月/日",
        bookNoteMaxLength: "最大上限2400字",
    },
    errorTemplate: "<span style='color:red'>#=message#</span>"
});
var validator = $("#Validation").data("kendoValidator");

$(document).ready(function () {
    $("#Status").hide();   //隱藏Status
    $("#Keeper").hide();  //隱藏Keeper
    $("#SaveEditBook").hide();   //隱藏SaveEditBook的button
    $("#DeleteBook").hide();  //隱藏DeleteBook的button
    $("#EditBook").hide();  //隱藏EditBook的button
    $("#BookStatus").kendoDropDownList({     //產生"借閱狀態"下拉選單
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: {
            transport: {
                read: {
                    url: "/AppMarketingAnalysis/SetDropDownListStatusData",
                    type: "post",
                    dataType: "json",
                }
            }
        },
        filter: "startswith",
        autoWidth: true,
        change: onChange,
    });
    $("#BookBoughtDate").kendoDatePicker({  //購書日期
        format: "yyyy/MM/dd",
        max: new Date("6000/12/31"),    //詢問是否要給最大範圍,還是用預設
    });
    var url = location.href;    //取得網址字串
    if (url.indexOf('?') != -1) {   //尋找網址列中是否有資料傳遞(QueryString)
        var detailOrEdit = "";
        var ary = url.split('?')[1].split('&'); //將各自的參數資料切割放進ary中
        for (i = 0; i <= (ary.length - 1); i++) {   //下迴圈去搜尋每個資料參數
            if (ary[i].split('=')[0] == 'id') {
                bookId = ary[i].split('=')[1];
            }
            else if (ary[i].split('=')[0] == 'detailOrEdit') {
                detailOrEdit = ary[i].split('=')[1];
            }
        }
        $("#Status").show();   //顯示Status的dropdownlist
        $("#Keeper").show();  //顯示Keeper的dropdownlist
        $("#InsertBook").hide();  //隱藏InsertBook的button
        if (detailOrEdit == "Edit") {    //如果是編輯
            $.ajax({
                url: "/AppMarketingAnalysis/GetBook",
                dataType: "json",
                type: "post",
                data: "bookId=" + bookId,    //將此資料傳入EditBook
                success: function (e) {
                    if (e == "Wrong ID") {
                        alert("ID錯誤");
                        window.location.href = "/AppMarketingAnalysis/Index";
                    }
                }
            }).done(function (book) {
                var bookStatus = $("#BookStatus").data("kendoDropDownList");
                var bookKeeper = $("#BookKeeper").data("kendoDropDownList");
                $("#Title").html("編輯書籍");
                $("#SaveEditBook").show();  //顯示SaveEditBook的button
                $("#DeleteBook").show();  //顯示DeleteBook的button
                $("#BookName").val(book.BookName);
                $("#BookAuthor").val(book.BookAuthor);
                $("#BookPublisher").val(book.BookPublisher);
                $("#BookNote").val(book.BookNote);
                $("#BookBoughtDate").data("kendoDatePicker").value(book.BookBoughtDate);
                $("#BookClass").data("kendoDropDownList").value(book.BookClassId);
                bookStatus.value(book.BookStatus);
                bookKeeper.value(book.BookKeeper);
                if (bookStatus.value() == 'A' || bookStatus.value() == 'U') {   //判斷借閱人和借閱狀態的關係
                    bookKeeper.select(0);
                    bookKeeper.enable(false);
                }
                else {
                    bookKeeper.enable(true);
                    bookKeeper.select(1);
                }
            })
        }
        else if (detailOrEdit == "Detail") {   //如果是明細
            $.ajax({
                url: "/AppMarketingAnalysis/GetBook",
                dataType: "json",
                type: "post",
                data: "bookId=" + bookId,    //將此資料傳入EditBook
                success: function (e) {
                    if (e == "Wrong ID") {
                        alert("ID錯誤");
                        window.location.href = "/AppMarketingAnalysis/Index";
                    }
                }
            }).done(function (book) {
                var bookStatus = $("#BookStatus").data("kendoDropDownList");
                var bookKeeper = $("#BookKeeper").data("kendoDropDownList");
                $("#Title").html("書籍明細");
                $("#EditBook").show();  //顯示EditBook的button
                $("#BookName").val(book.BookName);
                $("#BookAuthor").val(book.BookAuthor);
                $("#BookPublisher").val(book.BookPublisher);
                $("#BookNote").val(book.BookNote);
                $("#BookBoughtDate").val(book.BookBoughtDate);
                $("#BookClass").data("kendoDropDownList").value(book.BookClassId);
                bookStatus.value(book.BookStatus);
                bookKeeper.value(book.BookKeeper);
                $("#BookName").attr("readonly", "readonly");
                $("#BookAuthor").attr("readonly", "readonly");
                $("#BookPublisher").attr("readonly", "readonly");
                $("#BookNote").attr("readonly", "readonly");
                $("#BookName").attr("readonly", "readonly");
                $("#BookBoughtDate").data("kendoDatePicker").enable(false);
                $("#BookClass").data("kendoDropDownList").enable(false);
                bookStatus.enable(false);
                bookKeeper.enable(false);
                $(".requiredMark").hide();
            })
        }
    }
});
//讓BookStatus改變時也改變BookKeeper
function onChange() {
    var bookStatus = $("#BookStatus").data("kendoDropDownList");
    var bookKeeper = $("#BookKeeper").data("kendoDropDownList");
    if (bookStatus.value() == 'A' || bookStatus.value() == 'U') {
        bookKeeper.select(0);
        bookKeeper.enable(false);
    }
    else {
        bookKeeper.enable(true);
        bookKeeper.select(1);
    }
}

//新增書籍
$("#InsertBook").click(function () {
    if (validator.validate()) {
        var insertData = {   //整合輸入的資料
            BookName: $("#BookName").val(), //命名須與model的名稱相同
            BookAuthor: $("#BookAuthor").val(),
            BookPublisher: $("#BookPublisher").val(),
            BookNote: $("#BookNote").val(),
            BookBoughtDate: $("#BookBoughtDate").data("kendoDatePicker").value(),
            BookClassId: $("#BookClass").data("kendoDropDownList").value()
        };
        $.ajax({
            url: "/AppMarketingAnalysis/InsertBook",
            dataType: "json",
            data: insertData,    //將此資料傳入InsertBook
            type: "post",
            success: function (bookId) {
                var detailOrEdit = "Edit";
                alert("新增成功");
                window.location.href = "/AppMarketingAnalysis/InsertBookView?id=" + bookId + "&detailOrEdit=" + detailOrEdit;
            },
            error: function (e) {
                alert("請勿輸入危險字元");
            }
        })
    }
});

//存檔編輯書籍
$("#SaveEditBook").click(function () {
    var bookStatus = $("#BookStatus").data("kendoDropDownList");
    var bookKeeper = $("#BookKeeper").data("kendoDropDownList");

    if ((bookStatus.value() == 'B' || bookStatus.value() == 'C') && bookKeeper.value() == "") { //判斷是否已借出卻沒有借閱人
        alert("借閱人不可為空");
    }
    else {
        if (validator.validate()) {
            var editData = {   //整合輸入的資料
                BookId: bookId,
                BookName: $("#BookName").val(), //命名須與model的名稱相同
                BookAuthor: $("#BookAuthor").val(),
                BookPublisher: $("#BookPublisher").val(),
                BookNote: $("#BookNote").val(),
                BookBoughtDate: $("#BookBoughtDate").data("kendoDatePicker").value(),
                BookClassId: $("#BookClass").data("kendoDropDownList").value(),
                BookStatus: $("#BookStatus").data("kendoDropDownList").value(),
                BookKeeper: $("#BookKeeper").data("kendoDropDownList").value()
            };
            $.ajax({
                url: "/AppMarketingAnalysis/EditBook",
                dataType: "json",
                data: editData,    //將此資料傳入EditBook
                type: "post",
                success: function (e) {
                    if (e == "編輯失敗") {
                        alert("編輯失敗");
                    }
                    else {
                        alert("編輯成功");
                        window.location.href = "/AppMarketingAnalysis/InsertBookView?id=" + bookId + "&detailOrEdit=Detail";
                    }
                },
                error: function (e) {
                    alert("請勿輸入危險字元");
                }
            })
        }
    }
});

//刪除書籍
$("#DeleteBook").click(function () {
    if (confirm('是否刪除')) {   //判斷是否刪除(會return值)
        $.ajax({
            type: "POST",
            url: "/AppMarketingAnalysis/DeleteBook",
            data: "bookId=" + bookId,
            dataType: "json",
            success: function (e) {
                if (e) {
                    alert("刪除成功");
                    window.location.href = "/AppMarketingAnalysis/Index";
                }
                else if (e == "Wrong ID") {
                    alert("ID錯誤");
                    window.location.href = "/AppMarketingAnalysis/Index";
                }
                else if (e == "This book has been lent") {
                    alert("無法刪除,此書已被借出");
                }
            },
            error: function (e) {
                alert("系統發生錯誤");
            }
        })
    }
});

//書籍明細
$("#EditBook").click(function () {
    var detailOrEdit = "Edit";
    window.location.href = "/AppMarketingAnalysis/InsertBookView?id=" + bookId + "&detailOrEdit=" + detailOrEdit;
})