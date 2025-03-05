var UserFileUploadjqXHRData; 
var SaveCompanyBranch = function () {
    if (CommonUiValidation()) {
        $.ajax({
                type: "POST",
                url: "CompanyBranch/AddCompanyBranch/",
                data: {
                    Id: $("#Id").val(),
                    Name: $("#BranceName").val(),
                    Division: $("#Division").val(),
                    Region: $("#Region").val(),
                    Street: $("#Street").val(),
                    City: $("#City").val(),
                    State: $("#State").val(),
                    ZipCode: $("#ZipCode").val(),
                    TimeZone: $("#TimeZone").val(),
                    IsMainBranch: $("#IsMainBranch_Value").is(":checked"),
                    Tax: $("#Tax").val(),
                    Logo: $("#UploadedPath").val(),
                    ColorLogo: $("#UploadedPathColored").val(),
                    EmailLogo: $("#UploadedPathEmail").val(),

                },
                success: function () {
                    $(".close").trigger("click");
                    OpenSuccessMessageNew("Success!", "Branch data saved successfully.", function () {
                        LoadCompanyBranch(true);
                    }) 
                }
            });

    }
}
var AddBranchHeightCalc = function () {
    setTimeout(function () {
        $('.add_company_branch_inner').height(window.innerHeight - ($('.add_company_branch_header').height() + $('.footer-section').height() + 21));
    }, 100);
}
$(document).ready(function () {
    $("#SaveBranch").click(function () {
        SaveCompanyBranch();
    });
    /*$(".select-list").select2({});*/
    $("#UploadCompanyBranchBtn").click(function () {
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $('#UploadedFile').fileupload({
        pasteZone: null,
        url: domainurl + '/CompanyBranch/UploadCompanyBranchLogo/',
        dataType: 'json',
        add: function (e, data) {
            UserFileUploadjqXHRData = data;
        },
        progress: function (e, data) {
            var percentVal = parseInt(data.loaded / data.total * 100, 10);
            $(".file-progress").show();
            $(".file-progress .progress-bar").animate({
                width: percentVal + "%"
            }, 40);
            $(".file-progress .progress-bar span").text(percentVal + '%');
        },
        done: function (event, data) {
            setTimeout(function () {
                $(".file-progress").hide();
                $(".file-progress .progress-bar").animate({
                    width: 0 + "%"
                }, 0);
                $(".file-progress .progress-bar span").text(0 + '%');
            }, 500);
            console.log(data.result);
            if ((typeof (data.result.isUploaded) != "undefined") && data.result.isUploaded == true) {
                $("#UploadSuccessMessage").removeClass('hidden');
                //$("#UploadCustomerFileBtn").addClass('hidden');
                $("#UploadedPath").val(data.result.filePath);

            }
        },
        fail: function (event, data) {
            if (data.files[0].error) {
                alert(data.files[0].error);
            }
        }
    });
    $("#UploadedFile").on("change", function () {
        if (UserFileUploadjqXHRData) {
            UserFileUploadjqXHRData.submit();
        }
        return false;
    });



    $("#UploadCompanyBranchBtnEmail").click(function () {
        $("#UploadedFileEmail").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $('#UploadedFileEmail').fileupload({
        pasteZone: null,
        url: domainurl + '/CompanyBranch/UploadCompanyBranchEmailLogo/',
        dataType: 'json',
        add: function (e, data) {
            UserFileUploadjqXHRData = data;
        },
        progress: function (e, data) {
            var percentVal = parseInt(data.loaded / data.total * 100, 10);
            $(".file-progressEmail").show();
            $(".file-progressEmail .progress-bar").animate({
                width: percentVal + "%"
            }, 40);
            $(".file-progressEmail .progress-bar span").text(percentVal + '%');
        },
        done: function (event, data) {
            setTimeout(function () {
                $(".file-progressEmail").hide();
                $(".file-progressEmail .progress-bar").animate({
                    width: 0 + "%"
                }, 0);
                $(".file-progressEmail .progress-bar span").text(0 + '%');
            }, 500);
            console.log(data.result);
            if ((typeof (data.result.isUploaded) != "undefined") && data.result.isUploaded == true) {
                $("#UploadSuccessMessageEmail").removeClass('hidden');
                //$("#UploadCustomerFileBtn").addClass('hidden');
                $("#UploadedPathEmail").val(data.result.filePath);

            }
        },
        fail: function (event, data) {
            if (data.files[0].error) {
                alert(data.files[0].error);
            }
        }
    });
    $("#UploadedFileEmail").on("change", function () {
        if (UserFileUploadjqXHRData) {
            UserFileUploadjqXHRData.submit();
        }
        return false;
    });


    $("#UploadCompanyBranchBtnColored").click(function () {
        $("#UploadedFileColored").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $('#UploadedFileColored').fileupload({
        pasteZone: null,
        url: domainurl + '/CompanyBranch/UploadCompanyBranchColordLogo/',
        dataType: 'json',
        add: function (e, data) {
            UserFileUploadjqXHRData = data;
        },
        progress: function (e, data) {
            var percentVal = parseInt(data.loaded / data.total * 100, 10);
            $(".file-progressColored").show();
            $(".file-progressColored .progress-bar").animate({
                width: percentVal + "%"
            }, 40);
            $(".file-progressColored .progress-bar span").text(percentVal + '%');
        },
        done: function (event, data) {
            setTimeout(function () {
                $(".file-progressColored").hide();
                $(".file-progressColored .progress-bar").animate({
                    width: 0 + "%"
                }, 0);
                $(".file-progressColored .progress-bar span").text(0 + '%');
            }, 500);
            console.log(data.result);
            if ((typeof (data.result.isUploaded) != "undefined") && data.result.isUploaded == true) {
                $("#UploadSuccessMessageColored").removeClass('hidden');
                //$("#UploadCustomerFileBtn").addClass('hidden');
                $("#UploadedPathColored").val(data.result.filePath);

            }
        },
        fail: function (event, data) {
            if (data.files[0].error) {
                alert(data.files[0].error);
            }
        }
    });
    $("#UploadedFileColored").on("change", function () {
        if (UserFileUploadjqXHRData) {
            UserFileUploadjqXHRData.submit();
        }
        return false;
    });
    AddBranchHeightCalc();
});
$(window).resize(function () {
    AddBranchHeightCalc();
});