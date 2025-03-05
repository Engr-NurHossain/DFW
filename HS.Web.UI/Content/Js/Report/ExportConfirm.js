var count = 0;
var count1 = 1;
var IsDatabaseColumn = "False";
var IdList = [];
var SelectIdList = "";
$(document).ready(function() {
    if (window.innerWidth < 421) {
        $(".export_confirm_container").height(window.innerHeight - 20);
        $(".contents").height(window.innerHeight - 410);
    } else {
        $(".export_confirm_container").css("height", "498px");
        $(".contents").css("height", "295px");
    };
    $('.contents').perfectScrollbar();
    $("#SelectAll").change(function() {
        $("#ExportDatabseSelected").prop('checked', false);
        if ($(this).is(':checked')) {
            $(".CheckItems").each(function() {
                $(this).prop('checked', true);
            });
        } else {
            $(".CheckItems").each(function() {
                $(this).prop('checked', false);
            });
        }
    });

    $("#ExportDatabseSelected").change(function() {
        $("#SelectAll").prop('checked', false);
        $(".CheckItems").each(function() {
            $(this).prop('checked', false);
        });
        if ($(this).is(':checked')) {
            IsDatabaseColumn = "True";
        } else {
            IsDatabaseColumn = "False";
        }
    });

    if ($("#ExportDatabseSelected").is(':checked')) {
        IsDatabaseColumn = "True";
    }
    $("#SelectAllIds").change(function() {
        if ($(this).is(':checked')) {
            $(".CheckItems").each(function() {
                $(this).prop('checked', true);
            });
        } else {
            $(".CheckItems").each(function() {
                $(this).prop('checked', false);
            });
        }
    });

    $("#OkButton").click(function() {
        console.log("hlw");
        var ColumnNames = [];

        var ColumnNameList = "";
        var Ids = "";
        var ReportFor = $("#ReportFor").val();
        var NumberPrefix = $("#ListNumberPrefix").val();
        var allids = "true";
        var activecustomer = $("#CancelCustomerFilter").val();

        $(".CheckItems").each(function() {
            if ($(this).is(':checked')) {
                ColumnNames.push($(this).val());
                ColumnNameList += $(this).val() + ",";
            }
        });
        parent.$(".CheckItemsCustomer:checked").each(function() {
            IdList.push($(this).attr("idval"));
            SelectIdList += $(this).attr("idval") + ",";
        })

        if ($("#ExportSelected").is(':checked')) {

            var ExportType = $("input[name='exporttype']:checked").val();
            var radioValue = $("input[name='ImportType']:checked").val();


            if (radioValue) {
                if (radioValue == "excel") {
                    console.log("1");
                    var DownloadUrl = domainurl + "/Reports/NewReport?&ColumnNames=" + ColumnNameList + "&ReportFor=" + ReportFor + "&NumberPrefix=" + NumberPrefix + "&SelectAllIds=" + SelectAllIds + "&activeOrinactive=" + activecustomer + "&UserList=" + SelectAllIds + "&ExportType=" + ExportType + "&FilterUser=" + FilterUser + "&isSelected=true" + "&IsDatabaseColumn=" + IsDatabaseColumn + "&SelectedIdList=" + SelectIdList;
                } else {
                    var DownloadUrl = domainurl + "/Reports/CustomerPdf?&ColumnNames=" + ColumnNameList + "&ReportFor=" + ReportFor + "&NumberPrefix=" + NumberPrefix + "&SelectAllIds=" + SelectAllIds + "&activeOrinactive=" + activecustomer + "&UserList=" + SelectAllIds + "&ExportType=" + ExportType + "&FilterUser=" + FilterUser + "&isSelected=true" + "&IsDatabaseColumn=" + IsDatabaseColumn;
                }

            } else {
                console.log("2");
                var DownloadUrl = domainurl + "/Reports/NewReport?&ColumnNames=" + ColumnNameList + "&ReportFor=" + ReportFor + "&NumberPrefix=" + NumberPrefix + "&SelectAllIds=" + SelectAllIds + "&activeOrinactive=" + activecustomer + "&UserList=" + SelectAllIds + "&ExportType=" + ExportType + "&FilterUser=" + FilterUser + "&isSelected=true" + "&IsDatabaseColumn=" + IsDatabaseColumn + "&SelectedIdList=" + SelectIdList;
            }

            parent.window.open(DownloadUrl, '_blank');
            parent.$.magnificPopup.close();
        } else if ($("#ExportFilter").is(':checked')) {
            var ExportType = $("input[name='exporttype']:checked").val();
            var radioValue = $("input[name='ImportType']:checked").val();
            if (radioValue) {
                if (radioValue == "excel") {
                    console.log("3");
                    var DownloadUrl = domainurl + "/Reports/NewReport?ColumnNames=" + ColumnNameList + "&ReportFor=" + ReportFor + "&NumberPrefix=" + NumberPrefix + "&SelectAllIds=" + "filtered" + "&activeOrinactive=" + activecustomer + "&UserList=" + UsersList + "&ExportType=" + ExportType + "&FilterUser=" + FilterUser + "&IsDatabaseColumn=" + IsDatabaseColumn;
                } else {
                    var DownloadUrl = domainurl + "/Reports/CustomerPdf?ColumnNames=" + ColumnNameList + "&ReportFor=" + ReportFor + "&NumberPrefix=" + NumberPrefix + "&SelectAllIds=" + "filtered" + "&activeOrinactive=" + activecustomer + "&UserList=" + UsersList + "&ExportType=" + ExportType + "&FilterUser=" + FilterUser + "&IsDatabaseColumn=" + IsDatabaseColumn;
                }

            } else {
                console.log("4");
                var DownloadUrl = domainurl + "/Reports/NewReport?ColumnNames=" + ColumnNameList + "&ReportFor=" + ReportFor + "&NumberPrefix=" + NumberPrefix + "&SelectAllIds=" + "filtered" + "&activeOrinactive=" + activecustomer + "&UserList=" + UsersList + "&ExportType=" + ExportType + "&FilterUser=" + FilterUser + "&IsDatabaseColumn=" + IsDatabaseColumn;

            }
            parent.window.open(DownloadUrl, '_blank');
            parent.$.magnificPopup.close();

        } else {
            var ExportType = $("input[name='exporttype']:checked").val();
            var radioValue = $("input[name='ImportType']:checked").val();
            if (radioValue) {
                if (radioValue == "excel") {
                    console.log("5");
                    var DownloadUrl = domainurl + "/Reports/NewReport?ColumnNames=" + ColumnNameList + "&ReportFor=" + ReportFor + "&NumberPrefix=" + NumberPrefix + "&SelectAllIds=" + "true" + "&activeOrinactive=" + activecustomer + "&UserList=" + UsersList + "&ExportType=" + ExportType + "&FilterUser=" + FilterUser + "&IsDatabaseColumn=" + IsDatabaseColumn;
                } else {
                    var DownloadUrl = domainurl + "/Reports/CustomerPdf?ColumnNames=" + ColumnNameList + "&ReportFor=" + ReportFor + "&NumberPrefix=" + NumberPrefix + "&SelectAllIds=" + "true" + "&activeOrinactive=" + activecustomer + "&UserList=" + UsersList + "&ExportType=" + ExportType + "&FilterUser=" + FilterUser + "&IsDatabaseColumn=" + IsDatabaseColumn;
                }
            } else {
                console.log("6");
                console.log(SelectIdList);
                var DownloadUrl = domainurl + "/Reports/NewReport?ColumnNames=" + ColumnNameList + "&ReportFor=" + ReportFor + "&NumberPrefix=" + NumberPrefix + "&SelectAllIds=" + "true" + "&activeOrinactive=" + activecustomer + "&UserList=" + UsersList + "&ExportType=" + ExportType + "&FilterUser=" + FilterUser + "&IsDatabaseColumn=" + IsDatabaseColumn + "&SelectedIdList=" + SelectIdList;

            }

            parent.window.open(DownloadUrl, '_blank');
            parent.$.magnificPopup.close();
        }
    });
});