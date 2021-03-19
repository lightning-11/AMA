$(document).ready(function () {
    //產生"書籍類別"下拉選單
    $("#BookClass").kendoDropDownList({
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: {
            transport: {
                read: {
                    url: "/AppMarketingAnalysis/SetDropDownListClassData",  //要加controller的位置
                    type: "post",
                    dataType: "json",
                }
            }
        },
        filter: "startswith",
        optionLabel: '請選擇',
        autoWidth: true,
    });

    //產生"借閱人"下拉選單
    $("#BookKeeper").kendoDropDownList({
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: {
            transport: {
                read: {
                    url: "/AppMarketingAnalysis/SetDropDownListMemberData",
                    type: "post",
                    dataType: "json",
                }
            }
        },
        filter: "startswith",
        optionLabel: '請選擇',
        autoWidth: true,
    });
});