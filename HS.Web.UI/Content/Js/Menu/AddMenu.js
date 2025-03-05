var UserFileUploadjqXHRData;

var LoadAddNewMenuItemDiv = function () {
    console.log("menuItem");
    if ($("#id").val()>0)
    {
        console.log("menuItemFire");
        $("#AddNewMenuItemPartial").show();
        $("#AddNewMenuItemPartial").load(domainurl + "/MenuManagement/AddMenuItem?menuId=" + $("#id").val()+"&miId=0");
    }
    else
    {
        console.log("menuItemNotFire");
        OpenErrorMessageNew("Error!", "Please save menu");
    }
}

var EditMenuItemDiv = function (miId) {
    $("#AddNewMenuItemPartial").show();
    $("#AddNewMenuItemPartial").load(domainurl + "/MenuManagement/AddMenuItem?menuId=" + $("#id").val() + "&miId=" + miId);
}

var ScrollToError = function () {
    if ($(".required").length > 0) {
        $('.add_customer_wrapper').animate({
            scrollTop: ($('.add_customer_wrapper').scrollTop() + $(".required").offset().top - 100)
        }, 2000);
    }
}


var SaveCustomer = function (editmenu) {
    if ($("#UrlSlug").val() != "") {
        weburl = weburlslug + "/" + $("#UrlSlug").val();
    }
    var url = domainurl + "/MenuManagement/AddMenu/";
    var param = {
   
        id: $("#id").val(),
        MenuName: $("#MenuName").val(),
        Status: $("#MenuStatus").val() == "1" ? true : false,
        TimeFrom: $("#timeFrom").val(),
        TimeTo: $("#timeTo").val(),
        DaysAvailable: String($("#selectDaysAvailable").val()),
        Description: $('#Description').val(),
        Photo: $("#UploadedPath").val(),
        DaysAvailableOption: $("#day_available_option").val(),
        TimeAvailableOption: $("#time_available_option").val(),
        UrlSlug: $("#UrlSlug").val(),
        WebsiteURL: weburl,
        MetaTitle: $("#MetaTitle").val(),
        MetaDescription: $("#MetaDescription").val(),
      
    };

    var passparam = JSON.stringify({
        'menu': param
    });

    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: passparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result == true) {
                if (typeof (editmenu) != "undefined" && editmenu != null && editmenu != "" && editmenu == "true") {
                    $(".LoadMenuEdit").addClass("hidden");
                    $(".menu_list_allitem").removeClass("hidden");
                    OpenTopToBottomModal(domainurl + "/MenuManagement/AllMenuItemDetail/?id=" + data.menuId);
                }
                else {
                    console.log("saved");
                    OpenIeateryPopupModal("Success", "Menu saved successfully");
                    OpenTopToBottomModal("/MenuManagement/AddMenu?Id=" + data.menuId);
                    $("#id").val(data.menuId);
                    MenuListLoad(1);
                }
            }
            else if (data.result == false) {
                parent.OpenErrorMessageNew("Error!", "Menu saved failed.");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var LoadMenuItemList = function () {
    $("#MenuItemList").load(domainurl + "/MenuManagement/MenuItemListPartial?Id=" + $("#id").val());
}
var DeleteMenuById = function (mid) {
    $.ajax({
        url: domainurl + "/MenuManagement/DeleteMenu",
        data: { Id: mid },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenIeateryPopupModal("Success", data.message)
                CloseTopToBottomModal();
                MenuListLoad(1);
            } else {
                OpenErrorMessageNew("Error!", data.message)
            }
        }
    });
}

var SearchKeyUp = function (item, event) {
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13 || event.keyCode == 9)
        return false;
    var ExistItem = "";
    var ExistItemInner = "";
    $(".HasItem").each(function () {
        ExistItemInner += "'" + $(this).attr('data-id') + "',";
    });
    if (ExistItemInner.length > 0) {
        ExistItemInner = ExistItemInner.slice(0, -1);
        ExistItem = "(" + ExistItemInner + ")";
    }
    $.ajax({
        url: domainurl + "/Menu/GetMenuItemListByKey",
        data: {
            key: $(item).val(),
            ExistCourse: ExistItem
        },

        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var resultparse = JSON.parse(data.result);

            if (resultparse.length > 0) {
                var searchresultstring = "<div class='NewProjectSuggestion'>";
                for (var i = 0; i < resultparse.length; i++) {
                    searchresultstring = searchresultstring + String.format(PropertyUserSuggestiontemplate,
                        /*0*/resultparse[i].Id,
                        /*1*/ resultparse[i].ItemName.replaceAll('"', '\'\''));
                }
                searchresultstring += "</div>";
                var ttdom = $($(item).parent()).find('.tt-menu');
                var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                $(ttdomComplete).html(searchresultstring);
                $(ttdom).show();

                CrsSuggestionclickbind(item);
                if (resultparse.length > 4) {
                    $(".NewProjectSuggestion").height(352);
                    $(".NewProjectSuggestion").css('position', 'relative');
                }
            }
            if (resultparse.length == 0)
                $('.tt-menu').hide();
        }
    });
}

var SearchKeyDown = function (item, event) {
    if (event.keyCode == 13) {/*Enter*/
        var selectedTTMenu = $(event.target).parent().find('.tt-suggestion.active');
        $(selectedTTMenu).click();
        $('.tt-menu').hide();
    }
    if (event.keyCode == 40) {/*Down*/
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');
        if ($(ttSugstionDom).length > 0 && $(ttSugstionDom).is(':visible')) {
            if ($(ttSugActive).length == 0) {
                $($(ttSugstionDom).get(0)).addClass('active');
                $(item).val($($(ttSugstionDom).get(0)).attr('data-select'))
            }
            else {
                var suggestionlist = $(ttSugstionDom);
                var activesuggestion = $(ttSugActive);
                var indexactive = -1;
                for (var id = 0; id < suggestionlist.length; id++) {
                    if ($(suggestionlist[id]).hasClass('active'))
                        indexactive = id;
                }
                if (indexactive < suggestionlist.length - 1) {
                    $(ttSugstionDom).removeClass('active');
                    var possibleactive = $(ttSugstionDom).get(indexactive + 1);
                    $($(ttSugstionDom).get(indexactive + 1)).addClass('active');
                    $(possibleactive).addClass('active');
                    $(item).val($(possibleactive).attr('data-select'));
                }
            }

            event.preventDefault();
        }
        else {

            var trselected = $($(event.target).parent()).parent();
            $(trselected).removeClass('focusedItem');
            $($(trselected).next('tr')).addClass('focusedItem');
            if ($(event.target).hasClass('ItemName')) {
                $($(trselected).next('tr')).find('input.ItemName').focus();
            }
        }
    }

    if (event.keyCode == 38) {/*UP*/
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');
        if ($(ttSugstionDom).length > 0 && $(ttSugActive).length > 0 && $(ttSugstionDom).is(':visible')) {
            var suggestionlist = $(ttSugstionDom);
            var activesuggestion = $(ttSugActive);
            var indexactive = -1;
            for (var id = 0; id < suggestionlist.length; id++) {
                if ($(suggestionlist[id]).hasClass('active'))
                    indexactive = id;
            }
            if (indexactive > 0) {
                $(ttSugstionDom).removeClass('active');
                var possibleactive = $(ttSugstionDom).get(indexactive - 1);
                $(possibleactive).addClass('active');
                $(item).val($(possibleactive).attr('data-select'))
            }
            event.preventDefault();
        }
        else {
            var trselected = $($(event.target).parent()).parent();
            $(trselected).removeClass('focusedItem');
            $($(trselected).prev('tr')).addClass('focusedItem');
            $($(trselected).prev('tr')).find('input.ItemName').focus();
        }
    }
}
var PropertyUserSuggestiontemplate =
                '<div class="tt-suggestion tt-selectable" data-select="{1}"  data-id="{0}" >'
                   + "<p class='tt-sug-text'>"
                   + "{1}" + "<br />"
                   + "</p> "
                + "</div>";

var NewItemRow = "<tr>"
                        + "<td valign='top' class='rowindex'></td>"
                        + "<td valign='top'><input type='text'class='ItemName' onkeydown='SearchKeyDown(this,event)' onkeyup='SearchKeyUp(this,event)' />"
                            + "<div class='tt-menu'>"
                                + "<div class='tt-dataset tt-dataset-autocomplete'> </div> "
                            + "</div>"
                            + "<span class='spnItemName'></span>"
                        + "</td>"
                        + "<td valign='top' class='tableActions'>"
                            + "<i class='fa fa-trash-o' aria-hidden='true'></i>"
                        + "</td>"
    + "</tr>";
var GetMenuUrlSlugPermission = function () {
    var url = domainurl + "/MenuManagement/GetMenuUrlSlugPermission";
    var param;
    param = JSON.stringify({
        id: cmenuid,
        urlslug: $("#UrlSlug").val(),
    });
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data) {
                var editmenu = $(this).attr('data-editmenu');
                SaveCustomer(editmenu);
            }
            else {
                OpenErrorMessageNew("Error", "UrlSlug must be unique");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
$(document).ready(function () {
  
    $(".add_customer_wrapper").height(window.innerHeight - $('.cus-section').height() -70);
    $('.selectpicker').selectpicker();
    
    LoadMenuItemList();
    $("#DeleteThisCustomer").click(function () {
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete?", function () {
            DeleteMenuById($("#id").val())
        });
    });
    
    $("#btnMenuAddNewItem").click(function () {
        LoadAddNewMenuItemDiv();
    });

    $("#SaveCustomer").click(function () {
        
        console.log("befor CommonUiValidation");
        if (CommonUiValidation()) {
            console.log("after CommonUiValidation");
            if ($("#selectDaysAvailable").val() != null) {
                GetMenuUrlSlugPermission();
            }
            else {
                OpenErrorMessageNew("Error", "Please select menu days available");
            }
        }
        else {
            ScrollToError();
        }
    });
    $("#menu_SaveCustomer").click(function () {
        console.log("befor CommonUiValidation");
        if (CommonUiValidation()) {
            console.log("after CommonUiValidation");
            if ($("#selectDaysAvailable").val() != null) {
                GetMenuUrlSlugPermission();
            }
            else {
                OpenErrorMessageNew("Error", "Please select menu days available");
            }
        }
        else {
            ScrollToError();
        }
    });

    $(".deleteDoc").click(function () {
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this file?", function () {
            $(".Upload_Doc").removeClass('hidden');
            $("#UploadCustomerFileBtn").attr('src', '/Content/img/toppng.com-file-upload-image-icon-980x980.png');
            $(".chooseFilebtn").removeClass("hidden");
            $(".changeFilebtn").addClass("hidden");
            $(".deleteDoc").addClass("hidden");
            $("#Preview_Doc").attr('src', "");
            $("#Frame_Doc").attr('src', "");
            $("#UploadSuccessMessage").hide();
            $("#BodyContent").val("");
            $("#UploadedPath").val('');
            $(".fileborder").addClass('border_none');
            $("#UploadCustomerFileBtn").removeClass('otherfileposition');
        });
    });

    $("#UploadCustomerFileBtn").click(function () {
        console.log("sdfdsf");
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });

    $(".change-picture-logo").click(function () {
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $('#UploadedFile').fileupload({
        pasteZone: null,
        url: domainurl + '/MenuManagement/UploadMenuPhoto',
        dataType: 'json',
        add: function (e, data) {
            var ext = data.files[0].name.split(".");
            if (ext[1] == 'jpg' || ext[1] == 'png' || ext[1] == 'jpeg' || ext[1] == 'gif' || ext[1] == 'JPG' || ext[1] == 'PNG' || ext[1] == 'JPEG') {
                if (data.files[0].size <= 50000000) {
                    UserFileUploadjqXHRData = data;
                }
                else {
                    OpenErrorMessageNew("Error!", "File size is more then 50 mb.", function () {
                        $(".close").click();
                    })
                }

            }
            else {
                OpenErrorMessageNew("Error!", "File format not valid.", function () {
                    $(".close").click();
                })
            }

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
            console.log("dfdf");
            if ($("#BodyContent").val() == "") {
                var filename = data.result.FullFilePath;
                tinymce.get("BodyContent").setContent(filename);
            }

            setTimeout(function () {
                $(".file-progress").hide();
                $(".file-progress .progress-bar").animate({
                    width: 0 + "%"
                }, 0);
                $(".file-progress .progress-bar span").text(0 + '%');
            }, 500);

            if ((typeof (data.result.isUploaded) != "undefined") && data.result.isUploaded == true) {
                $("#UploadSuccessMessage").removeClass('hidden');
                $("#UploadedPath").val(data.result.FullFilePath);
                var spfile = data.result.FullFilePath.split('.');
                $(".fileborder").removeClass('red-border');
                $("#uploadfileerror").addClass("hidden");

                var index = spfile.length - 1;
                if (spfile[index] == "png" || spfile[index] == "PNG" || spfile[index] == "jpg" || spfile[index] == "JPG" || spfile[index] == "jpeg" || spfile[index] == "JPEG") {
                    $("#UploadCustomerFileBtn").attr('src', data.result.FullFilePath)
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").addClass('custom-file');
                    $("#UploadCustomerFileBtn").removeClass('otherfileposition');
                    $(".fileborder").addClass('border_none');
                }
                else if (spfile[index] == "pdf") {
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', domainurl + '/Content/Icons/pdf.png');
                    $("#UploadCustomerFileBtn").addClass('otherfileposition');
                    $("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborder").removeClass('border_none');
                }
                else if (spfile[index] == "doc" || spfile[index] == "docx") {
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/docx.png');
                    $("#UploadCustomerFileBtn").addClass('otherfileposition');
                    $("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborder").removeClass('border_none');
                }
                else if (spfile[index] == "mp4" || spfile[index] == "mov") {
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/mp4.png');
                    $(".fileborder").removeClass('border_none');
                }
                else {
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/docx.png');
                    $("#UploadCustomerFileBtn").addClass('otherfileposition');
                    $("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborder").removeClass('border_none');
                }
            }
            else {
                OpenErrorMessageNew("Error", "Image dimension should be 1400 * 930");
            }
        },
        fail: function (event, data) {
            //if (data.files[0].error) {
            //    //alert(data.files[0].error);
            //}
        }
    });
    $("#UploadedFile").on("change", function () {
        if (UserFileUploadjqXHRData) {
            UserFileUploadjqXHRData.submit();
        }
        return false;
    });

$("#UploadedPath").blur(function () {
    if ($("#UploadedPath").val() == "") {
        $("#uploadfileerror").removeClass("hidden");
        $(".fileborder").addClass('red-border');
    }
    if ($("#UploadedPath").val() != "") {
        $("#uploadfileerror").addClass("hidden");
        $(".fileborder").removeClass('red-border');
    }
})
$(".ItemTab tbody").on('click', 'tr', function (e) {
    if ($(e.target).hasClass('fa') || $(e.target).hasClass('tt-sug-text') || $(e.target).hasClass('tt-suggestion')) {
        return;
    }
    if ($(e.target).hasClass("spnItemName")) {
        $(".ItemTab tr").removeClass("focusedItem");
        $($(e.target).parent().parent()).addClass("focusedItem");
        $(e.target).parent().find('input').focus();
    }
    else if (e.target.tagName.toUpperCase() == 'INPUT') {
        return;
    }
    else {
        $(".ItemTab tr").removeClass("focusedItem");
        $($(e.target).parent()).addClass("focusedItem");
        $(e.target).find('input').focus();
    }
});
    /*Add new row*/
$(".ItemTab tbody").on('click', 'tr:last', function (e) {
    if ($(e.target).hasClass('fa')) {
        return;
    }
    $(".ItemTab tbody tr:last").after(NewItemRow);
    var i = 1;
    $(".ItemTab tbody tr td:first-child").each(function () {
        $(this).text(i);
        i += 1;
    });
});

    /*Remove last row*/
$(".ItemTab tbody").on('click', 'tr td i.fa', function (e) {
    $(this).parent().parent().remove();
    if ($(".ItemTab tbody tr").length < 2) {
        $(".ItemTab tbody tr:last").after(NewItemRow);
    }
    var i = 1;
    $(".ItemTab tbody tr td:first-child").each(function () {
        $(this).text(i);
        i += 1;
    });
});

$('#CustoerEditNav a').click(function (item) {
    $('#CustoerEditNav li').removeClass('active');
    $($(item.target).parent()).addClass('active');
});
//$(".add_customer_wrapper").on("scroll", function () {
//    var windscroll = $(".add_customer_wrapper").scrollTop();
//    $('#CustoerEditNav a').each(function (i) {
//        if ($($(this).attr("href")).position().top <= 85) {
//            $('#CustoerEditNav li').removeClass('active');
//            $('#CustoerEditNav li').eq(i).addClass('active');
//        }
//    });
    //});
    $(".Cancel_all_item").click(function () {
        $(".menu_list_allitem").removeClass("hidden");
        $(".LoadMenuEdit").addClass("hidden");
    })
});