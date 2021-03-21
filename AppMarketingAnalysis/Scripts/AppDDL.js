$(document).ready(function () {
    //產生下拉選單
    var DDL = ["Category", "InstallsRange", "Currency"];     //下拉選單清單
    for (var i = 0; i < DDL.length; i++) {
        $("#" + DDL[i]).kendoDropDownList({
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: {
                transport: {
                    read: {
                        url: "/AppMarketingAnalysis/GetDropDownListClassData",  //要加controller的位置
                        type: "post",
                        dataType: "json",
                        data: DDL[i]
                    }
                }
            },
            filter: "startswith",
            optionLabel: '請選擇',
            autoWidth: true,
        });
    }
});