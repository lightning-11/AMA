var detailOrEdit = "";
$(document).ready(function () {
    $("#NotFind").hide();   //隱藏NotFind
    $("#BookGrid").hide();  //隱藏Grid

    //產生"借閱狀態"下拉選單
    $("#BookStatus").kendoDropDownList({
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
        optionLabel: '請選擇',
        autoWidth: true,
    });

    //初始查詢的kendoGrid
    $("#BookGrid").kendoGrid({
        height: 540,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
            { field: "BookId", title: "書籍編號", hidden: true },
            { field: "BookClassId", title: "圖書類別", width: "10%" },
            { field: "BookName", title: "書名", 'template': '<a href=\"#= "/AppMarketingAnalysis/InsertBookView?id=" + BookId + "&detailOrEdit=Detail"#\">#=BookName#</a>', width: "24%" },  //超連結到明細畫面
            { field: "BookBoughtDate", title: "購買日期", width: "10%" },
            { field: "BookStatus", title: "借閱狀態", width: "8%" },
            { field: "BookKeeper", title: "借閱人", width: "8%" },
            { command: { text: "借閱紀錄", click: LendRecord }, width: "8%" },    //借閱紀錄欄位,按下後執行LendRecord()
            { command: { text: "編輯", click: EditBook }, width: "8%" },    //編輯欄位,按下後執行EditBook()
            { command: { text: "刪除", click: DeleteBook }, width: "8%" }    //刪除欄位,按下後執行DeleteBook()
        ],
    });
});

//產生bookNameAutoComplete
$("#BookName").kendoAutoComplete({
    dataSource: {
        transport: {
            read: {
                url: "/AppMarketingAnalysis/AutoCompleteBookName",
                type: "post",
                dataType: "json",
            }
        }
    },
    filter: "startswith",
    autoWidth: true,
    height: 100,
});

//查詢按鈕
$("#Search").click(function () {
    var inputData = {   //整合輸入的資料
        BookName: $("#BookName").val(), //命名須與model的名稱相同
        BookClassId: $("#BookClass").data("kendoDropDownList").value(),
        BookKeeper: $("#BookKeeper").data("kendoDropDownList").value(),
        BookStatus: $("#BookStatus").data("kendoDropDownList").value()
    };
    $.ajax({
        url: "/AppMarketingAnalysis/Search",
        dataType: "json",
        data: inputData,    //將此資料傳入Models.BookSearch
        type: "post",
    }).done(function (searchResult) {   //建立查詢Grid
        var dataSource = new kendo.data.DataSource({
            data: searchResult,
            schema: {
                model: {
                    fields: {
                        BookId: { type: "int" },
                        BookClassId: { type: "string" },
                        BookName: { type: "string" },
                        BookBoughtDate: { type: "string" },
                        BookStatus: { type: "string" },
                        BookKeeper: { type: "string" }
                    }
                }
            },
            pageSize: 20,
            sort: { field: "BookBoughtDate", dir: "desc" }   //排序
        });
        $("#BookGrid").data("kendoGrid").setDataSource(dataSource);
        if ($("#BookGrid").data("kendoGrid").dataSource.data().length == 0) {
            $("#BookGrid").hide();  //隱藏Grid
            $("#NotFind").show();   //顯示NotFind
        }
        else {
            $("#BookGrid").show();  //顯示Grid
            $("#NotFind").hide();   //隱藏NotFind
        }
        $("#BookGrid").data("kendoGrid").refresh();
    });
});

//刪除書籍
function DeleteBook(e) {
    var row = $(e.target).closest("tr"); //被選取的row
    var selectedId = $("#BookGrid").data("kendoGrid").dataItem(row).BookId;//被選取的row的BookId
    if (confirm('是否刪除')) {   ///判斷是否刪除(會return值)
        e.preventDefault();
        $.ajax({
            type: "POST",
            url: "/AppMarketingAnalysis/DeleteBook",
            data: "bookId=" + selectedId,
            dataType: "json",
            success: function (e) {
                if (e) {    //成功
                    $(row).remove();
                    alert("刪除成功");
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
        });
    }
};

//編輯書籍
function EditBook(e) {
    var row = $(e.target).closest("tr"); //被選取的row
    var selectedId = $("#BookGrid").data("kendoGrid").dataItem(row).BookId;//被選取的row的BookId
    detailOrEdit = "Edit";
    window.location.href = "/AppMarketingAnalysis/InsertBookView?id=" + selectedId + "&detailOrEdit=" + detailOrEdit;
};

//借閱紀錄
function LendRecord(e) {
    var row = $(e.target).closest("tr"); //被選取的row
    var selectedId = $("#BookGrid").data("kendoGrid").dataItem(row).BookId;//被選取的row的BookId
    window.location.href = "/AppMarketingAnalysis/LendRecordView?id=" + selectedId;
};