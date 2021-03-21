//Table的清單,要修改Table的話修改這裡
var TableList = [
    { Table: "AppGrid", TableName: "表格" },
    { Table: "Chart1", TableName: "圖表一" },
    { Table: "Char2", TableName: "圖表二" },
    { Table: "Chart3", TableName: "圖表三" }
];

$(document).ready(function () {
    $("#NotFind").hide();   //一開始隱藏NotFind
    $("#NotSearchYet").show();   //一開始顯示NotSearchYet

    //跑出選取Table的按鈕和Table顯示的區塊
    for (var i = 0; i < TableList.length; i++) {
        //選取Table的按鈕
        $("#Btn_Table").append("<button type='button' id='Btn_" + TableList[i].Table + "' class='btn btn-info' value='" + TableList[i].TableName + "' onclick='SelectedTable(this); '>" + TableList[i].TableName + "</button>");
        //Table顯示的區塊
        $("#Div_Table").append("<div id = '" + TableList[i].Table + "' style='border: 1px black solid;'></div>");
        //初次隱藏Table顯示的區塊
        $("#" + TableList[i].Table).hide();
    }

    //初次產生AppGrid框架
    $("#AppGrid").kendoGrid({
        height: 540,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
            { field: "AppName", title: "APP名稱", width: "25%" },
            { field: "Category", title: "圖書類別", width: "25%" },
            { field: "InstallsRange", title: "書名", width: "25%" },
            { field: "Currency", title: "書名", width: "25%" },
            /*
            { field: "Rating", title: "平均評分", hidden: true },
            { field: "RatingCount", title: "評分數量", width: "10%" },
            { field: "InstallsCount", title: "下載數量", width: "24%" },
            { field: "Free", title: "APP是免費還是付費", width: "10%" },
            { field: "Price", title: "APP價格", width: "8%" },
            { field: "Size", title: "APP檔案大小", width: "8%" },
            */
        ],
    });
});

//產生bookNameAutoComplete
$("#AppName").kendoAutoComplete({
    dataSource: {
        transport: {
            read: {
                url: "/AppMarketingAnalysis/AutoCompleteAppName",
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
    $("#NotSearchYet").hide(); //隱藏NotSearchYet

    var inputData = {   //整合輸入的資料
        APP_NAME: $("#AppName").val(), //命名須與model的名稱相同
        CATEGORY: $("#Category").data("kendoDropDownList").value(),
        INSTALLS_RANGE: $("#InstallsRange").data("kendoDropDownList").value(),
        CURRENCY: $("#Currency").data("kendoDropDownList").value()
    };
    $.ajax({
        url: "/AppMarketingAnalysis/Search",
        dataType: "json",
        data: inputData,    //將此資料傳入Models.AppMarketingAnalysisData
        type: "post",
    }).done(function (searchResult) { 
        //AppGrid接收資料
        GetAppGrid(searchResult);

        //Chart1接收資料
        //.
        //.
        //.
        
    });
});

//選擇圖表清單按鈕
function SelectedTable(table) {
    var TableId = table.id.substring(4, table.id.length);
    //選擇到的Table顯示,未選擇的Table隱藏
    for (var i = 0; i < TableList.length; i++) {
        if (TableList[i].Table == TableId) {
            $("#" + TableList[i].Table).show();
            //如果是表格的話,需判斷資料是否為空
            if (TableList[i].Table == "AppGrid") {
                if ($("#AppGrid").data("kendoGrid").dataSource.data().length == 0) {
                    $("#AppGrid").hide();  //隱藏Grid
                }
            }
        } else {
            $("#" + TableList[i].Table).hide();
        }
    }
}

//AppGrid接收資料
function GetAppGrid(searchResult) {
    var dataSource = new kendo.data.DataSource({
        data: searchResult,
        schema: {
            model: {
                fields: {
                    APP_NAME: { type: "string" },
                    CATEGIRY: { type: "string" },
                    INSTALLS_RANGE: { type: "string" },
                    CURRENCY: { type: "string" },
                }
            }
        },
        pageSize: 20,
        sort: { field: "APP_NAME", dir: "desc" }   //排序
    });
    $("#AppGrid").data("kendoGrid").setDataSource(dataSource);

    //判斷是否有資料
    if ($("#AppGrid").data("kendoGrid").dataSource.data().length == 0) {
        $("#AppGrid").hide();  //隱藏Grid
        $("#NotFind").show();   //顯示NotFind
    }
    else {
        $("#AppGrid").show();  //顯示Grid
        $("#NotFind").hide();   //隱藏NotFind
    }
    $("#AppGrid").data("kendoGrid").refresh();  //刷新Grid
}

/*
//選取gridview function
function AppDetail(e) {
    var row = $(e.target).closest("tr"); //被選取的row
    var selectedId = $("#AppGrid").data("kendoGrid").dataItem(row).APP_SORT;//被選取的row的APP_SORT
    window.location.href = "/AppMarketingAnalysis/AppDetailView?id=" + selectedId;
};
*/