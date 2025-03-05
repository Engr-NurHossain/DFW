var valid = false;
var hyperlinkvalid = false;
var jqXHRProjectData;
var PhotoIndexNumber = 0;
var jqXHR = [];
var UserFileUploadjqXHRData;
var NewEquipmentRow = "<tr>"
    + "<td valign='top'><input type='text' class='form-control titlename' placeholder='Title' />"
    + "</td>"
    + "<td valign='top'>"
    + "<input type='text' class='form-control addlink'  placeholder='https://url link' />"
    + "<span class='spnProductDesc'></span>"
    + "</td>"
    + "<td valign='top' class='tableActions'>"
    + "<button class='btn red_button'><i class='fa fa-trash-o' aria-hidden='true'></i></button>"
    + "</td>";

var NewRelatedRow = "<tr>"
    + "<td valign='top'>"
    + "<input type='text' class='form-control addlink'  placeholder='https://url link' />"
    + "<span class='spnProductDesc'></span>"
    + "</td>"
    + "<td valign='top' class='tableActions'>"
    + "<button class='btn red_button'><i class='fa fa-trash-o' aria-hidden='true'></i></button>"
    + "</td>";

var validationcheck = function () {
    var cn = $("#Title").val();
    var an = tinyMCE.get('BodyMessage').getContent();
    var flag = $("#IsDocumentLibrary").val();
    var result = false;
    if (cn != 'undefined' && cn != '' && cn != null && cn.length > 1) {
        $("#Title").css({ "border": "1px solid #ccc" });
        result = true;
    }
    else {
        $("#Title").css({ "border": "1px solid red" });
        return result = false;
    }

    if (an != "<!DOCTYPE html>\n<html>\n<head>\n</head>\n<body>\n\n</body>\n</html>" && an != null) {
        $("#BodyMessage").css({ "border": "1px solid #ccc" });
        result = true;
    }
    else if (flag =='True') {
        result = true;
    }
    else {
        $("#BodyMessage").css({ "border": "1px solid red" });
        return result = false;
    }
    return result;
}
var SaveKnowledge = function () {
    if (tinyMCE.get('BodyMessage').getContent() == "<!DOCTYPE html>\n<html>\n<head>\n</head>\n<body>\n\n</body>\n</html>") {
        $("#lblcomments").html(" (Required)");
    }
    else {
        $("#lblcomments").html("");
    }
    if (!validationcheck()) {
        $('#SaveKnowledge').removeAttr('disabled');
        return false;
    }
    var liinklist = [];
    var relatedliinklist = [];
    var Images = [];
    $(".dv-preview-pic").each(function () {
        Images.push({
            Location: $(this).find('img.project-photo').attr('data-link') == null || $(this).find('img.project-photo').attr('data-link') == "" ? $(this).find('img.project-photo').attr('src') : $(this).find('img.project-photo').attr('data-link'),
            Description: $($(this).find('.image-caption')).val(),
            Size: $(this).find('img.project-photo').attr('data-size')
        });
    });
    $("#hiperlink_table tbody tr").each(function () {
        console.log("hiperlink");
        var title = $(this).find('.titlename').val();
        var link = $(this).find('.addlink').val();

        if (title != '' && title != 'undefined' && title != null && link != '' && link != 'undefined' && link != null) {
            if (link.length > 5 && link.toLowerCase().includes('http')) {
                liinklist.push({ Title: encodeURI(title), Link: encodeURI(link) });
                $(this).find('.addlink').css({ "border": "1px solid #ccc" });
                hyperlinkvalid = true;
            }
            else {
                liinklist.push({ Title: encodeURI(title), Link: encodeURI(link) });
                $(this).find('.addlink').css({ "border": "1px solid red" });
                $(this).find('.titlename').css({ "border": "1px solid #ccc" });
                hyperlinkvalid = false;
                return false;
            }
            $(this).find('.titlename').css({ "border": "1px solid #ccc" });
        }
        else if ((title == '' || title == null) && link.length > 0) {
            liinklist.push({ Title: encodeURI(title), Link: encodeURI(link) });
            $(this).find('.titlename').css({ "border": "1px solid red" });
            if (link.toLowerCase().includes('http')) {
                $(this).find('.addlink').css({ "border": "1px solid #ccc" });
            }
            hyperlinkvalid = false;
            return false;
        }
        else if (title != '' && title != null && link.length < 1) {
            liinklist.push({ Title: encodeURI(title), Link: encodeURI(link) });
            $(this).find('.addlink').css({ "border": "1px solid red" });
            $(this).find('.titlename').css({ "border": "1px solid #ccc" });
            hyperlinkvalid = false;
            return false;
        }
        else {
            hyperlinkvalid = true;
        }
    });
    $("#relatedlink_table tbody tr").each(function () {
        console.log("relatedlink");
        var link = $(this).find('.addlink').val();

        if (link != '' && link != 'undefined' && link != null) {
            if (link.length > 5 && link.toLowerCase().includes('http')) {
                relatedliinklist.push({ Link: encodeURI(link) });
                $(this).find('.addlink').css({ "border": "1px solid #ccc" });
                valid = true;
            }
            else if (link.length > 0) {
                relatedliinklist.push({ Link: encodeURI(link) });
                $(this).find('.addlink').css({ "border": "1px solid red" });
                valid = false;
                return false;
            }
            else {
                relatedliinklist.push({ Link: encodeURI(link) });
                $(this).find('.addlink').css({ "border": "1px solid red" });
                valid = false;
            }
        }
        else {
            valid = true;
        }
    });
    if ((!valid || !hyperlinkvalid) && (liinklist.length > 0 || relatedliinklist.length > 0)) {
        $('#SaveKnowledge').removeAttr('disabled');
        return false;
    }
    var Param = {
        "Title": $("#Title").val(),
        "Answer": encodeURI(tinyMCE.get('BodyMessage').getContent()),
        "IsHidden": $("#IsHidden").prop("checked"),
        "TagsStr": $("#Tag").tagit('assignedTags'),
        "Id": $("#Id").val(),
        "IsDocumentLibrary": $("#IsDocumentLibrary").val(),
        "KnowledgeWeblinkList": liinklist,
        "RelatedArticleList": relatedliinklist,
        "IsDefault": $("#IsDefault").prop("checked"),
        "UserGroups": $("#UserGroups").val(),
        "Images": Images
    };
    var url = domainurl + "/Sales/SaveKnowledgeBase";
    $.ajax({
        type: "POST",
        url: url,
        data: Param,
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "Save successfully!");
            CloseTopToBottomModal();
            //window.location.reload();
            window.location.href = domainurl + "/knowledgebase";
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })
}
var TagSuggestionclickbind = function (item) {
    $('.tt-menu .tt-suggestion').click(function () {
        var clickitem = this;
        $('.tt-menu').hide();
        var selectval = $(clickitem).attr('data-name');
        var addvalue = '';
        var alarr = [];
        var allcount = $("ul.tagit li.tagit-choice").length;
        $("ul.tagit li.tagit-choice").each(function () {
            allcount--;
            if (allcount == 0) {
                if (!alarr.includes(selectval)) {
                    addvalue += selectval;
                    $(this).find('span.tagit-label').html(selectval);
                }
                else {
                    $(this).find('a.tagit-close').click();
                    addvalue = addvalue.slice(0, addvalue.length - 1);
                }
            }
            else {
                addvalue += $(this).find('span.tagit-label').html().trim() + ',';
                alarr.push($(this).find('span.tagit-label').html().trim());
            }
        });
        if (addvalue != '') {
            $('#Tag').val(addvalue);
        }
    });
    $('.tt-menu .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
    $('.AddKnowledgeBaseContainer').click(function () {
        $('.tt-menu').hide();
    })
}
var DeleteKnowledgeBase = function (id) {
    OpenConfirmationMessageNew("Confirmation", "Do you want to delete?", function () {
        var url = domainurl + "/Sales/DeleteKnowledgebase?id=" + id;
        $.ajax({
            type: "GET",
            url: url,
            dataType: "json",
            success: function (data) {
                window.location.reload();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    });

}
$(document).ready(function () {
    $(".addcarrier_inner").height(window.innerHeight - 100);
    $("#Tag").tagit();
    $("#UserGroups").selectpicker('val', UserVal);
    $('#BodyMessage').tinymce({
        height: 500,
        menubar: true,
        convert_urls: false,
        relative_urls: false,
        remov_script_host: false,
        document_base_url: domainurl + "/",
        quickbars_insert_toolbar: true,
        mode: "specific_textareas",
        editor_selector: "mceEditor",
        plugins: [
            'advlist', 'autolink', 'code',
            'lists', 'link', 'image', 'charmap', 'preview', 'anchor', 'searchreplace', 'visualblocks',
            'fullscreen', 'insertdatetime', 'media', 'table', 'wordcount', 'fullscreen','fullpage',
        ],
        toolbar: 'undo redo | casechange blocks | bold italic backcolor | alignleft aligncenter alignright alignjustify | bullist numlist checklist outdent indent | removeformat | link | image | insertdatetime |fullscreen |quickbars',
        fullpage_default_doctype: '<!DOCTYPE html>'
    });

    $("#btnProjectUpload").click(function () {
        $('#fu-my-project-upload').click();
        $("#UploadSuccessMessage").addClass('hidden');
    });

    $('.upload-gallary').delegate('.image-delete', 'click', function (evt) {
        evt.preventDefault();
        var id = $(this).attr('data-id');
        parent.OpenConfirmationMessageNew("Delete", "Do you want to delete this item?", function () {
            $(".preview_" + id).remove();
            $("#preview_" + id).remove();
            var piccountProject = $(".upload-gallary img.preview-pic").length;
            if (piccountProject < 20) {
                $(".project-blank-thumb").show();
            }
        })
    });

    $('#fu-my-project-upload').fileupload({
        pasteZone: null,
        url: '/File/KnowledgeBaseFileUpload',
        dataType: 'json',
        progress: function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $(".project-progress_" + data.files[0].uploadID).show();
            $(".project-progress_" + data.files[0].uploadID + " .progress-bar").animate({
                width: progress + "%"
            }, 40);
            $(".progressbar-project-percentage_" + data.files[0].uploadID + " span").text(progress + "%");
        },
        //add: function (e, data) {
        //    console.log("Upload Knowledgebase");
        //    if (data.originalFiles.length > 100) {
        //        parent.OpenErrorMessageNew("", "You can upload upto 100 files.");
        //        return false;
        //    };
        //    if (data.originalFiles.length > 0) {
        //        for (var u = 0; u < data.originalFiles.length; u++) {
        //            var size = data.originalFiles[u].size;
        //            if (size > 104857600) {
        //                parent.OpenErrorMessageNew("Failed.", "File size upto 100MB.");
        //                return false;
        //            }
        //        }
        //    }
        //    if (data.originalFiles.length > 0) {
        //        for (var u = 0; u < data.originalFiles.length; u++) {
        //            var Name = data.originalFiles[u].name.toLowerCase();
        //            console.log(Name);
        //            if (!Name.includes(".doc") && !Name.includes(".docx") && !Name.includes(".gif") && !Name.includes(".jpeg") && !Name.includes(".jpg")
        //                && !Name.includes(".msg") && !Name.includes(".png") && !Name.includes(".ppt") && !Name.includes(".pptx") && !Name.includes(".pdf")
        //                && !Name.includes(".txt") && !Name.includes(".xls") && !Name.includes(".xlsx")) {
        //                parent.OpenErrorMessageNew("Failed.", "Unallowed file type. Please re-check your file's format.");
        //                return false;
        //            }
        //        }
        //    }
        //    jqXHRProjectData = data;
        //    PhotoUploadXHRData = [];
        //    PhotoUploadXHRData.push(jqXHRProjectData);
        //    var validationPass = true;

        //    //var piccount = $(".upload-gallary img.preview-pic").length;
        //    //if ((piccount + PhotoUploadXHRData.length) > 5) {
        //    //    parent.OpenErrorMessageNew("", "You can upload upto 20 photos.");
        //    //    validationPass = false;
        //    //}
        //    //else if(data.files[0].size > (100 * 1024 * 1024)) {
        //    //    parent.OpenErrorMessageNew("Failed.", "File size upto 100MB.");
        //    //    validationPass = false;
        //    //}
        //    if (validationPass) {
        //        data.files[0].uploadID = PhotoIndexNumber;

        //        var ProgressBarString =
        //            "<div class='progressbar-div progress-container project-progress_" + PhotoIndexNumber + "' style='position: relative;margin-top:-6px;'>" +
        //            "<div class='progress progress-striped active progressbar-bar'>" +
        //            "<div class='progress-bar progress-bar-danger' style='width: 0%;'>" +
        //            "</div>" +
        //            "</div>" +
        //            "<div class='progressbar-project-percentage_" + PhotoIndexNumber + "'>" +
        //            "<span></span>" +
        //            "</div>" +
        //            "</div>";

        //        var CaptionText = "Add caption";
        //        if (typeof (CaptionTextTranslation) != "undefined") {
        //            CaptionText = CaptionTextTranslation;
        //        }
        //        var CaptionErrorText = "The content have exceeded 30 characters";
        //        if (typeof (CaptionErrorTextTranslation) != "undefined") {
        //            CaptionErrorText = CaptionErrorTextTranslation;
        //        }
        //        $(".upload-gallary").append(" <div id='NewFileUp_" + PhotoIndexNumber + "' class='dv-preview-pic '><div class='image-delete' style='cursor:pointer;' newImgDataId='" + PhotoIndexNumber + "'><img src='/Content/Icons/cross_update.png' /></div><div class='dv-preview-pic-inner'><img class='preview-pic project-photo' src='' /></div><input type='text' propertycontentid='' class='image-caption' placeholder='" + CaptionText + "' /><label class='label hidden red lblCaptionError' id=''>" + CaptionErrorText + ".</label>" + ProgressBarString + "</div></div>");
        //        jqXHR[PhotoIndexNumber] = data.submit();
        //        PhotoIndexNumber++;
        //    }
        //    else {
        //        parent.OpenErrorMessageNew("", "You can upload upto 100 files.");
        //    }
        //},
        add: function (e, data) {
            console.log("Upload Knowledgebase");
            if (data.originalFiles.length > 5) {
                parent.OpenErrorMessageNew("", "You can upload upto 5 files at a time.");
                return false;
            };
            if (data.originalFiles.length > 0) {
                for (var u = 0; u < data.originalFiles.length; u++) {
                    var size = data.originalFiles[u].size;
                    if (size > 104857600) {
                        parent.OpenErrorMessageNew("Failed.", "File size upto 100MB.");
                        return false;
                    }
                }
            }
            if (data.originalFiles.length > 0) {
                for (var u = 0; u < data.originalFiles.length; u++) {
                    var Name = data.originalFiles[u].name.toLowerCase();
                    console.log(Name);
                    if (!Name.includes(".doc") && !Name.includes(".docx") && !Name.includes(".gif") && !Name.includes(".jpeg") && !Name.includes(".jpg")
                        && !Name.includes(".msg") && !Name.includes(".png") && !Name.includes(".ppt") && !Name.includes(".pptx") && !Name.includes(".pdf")
                        && !Name.includes(".txt") && !Name.includes(".xls") && !Name.includes(".xlsx")) {
                        parent.OpenErrorMessageNew("Failed.", "Unallowed file type. Please re-check your file's format.");
                        return false;
                    }
                }
            }
            jqXHRProjectData = data;
            PhotoUploadXHRData = [];
            PhotoUploadXHRData.push(jqXHRProjectData);
            var validationPass = true;

            //var piccount = $(".upload-gallary img.preview-pic").length;
            //if ((piccount + PhotoUploadXHRData.length) > 5) {
            //    parent.OpenErrorMessageNew("", "You can upload upto 20 photos.");
            //    validationPass = false;
            //}
            //else if(data.files[0].size > (100 * 1024 * 1024)) {
            //    parent.OpenErrorMessageNew("Failed.", "File size upto 100MB.");
            //    validationPass = false;
            //}
            if (validationPass) {
                data.files[0].uploadID = PhotoIndexNumber;

                var ProgressBarString =
                    "<div class='progressbar-div progress-container project-progress_" + PhotoIndexNumber + "' style='position: relative;margin-top:-6px;'>" +
                    "<div class='progress progress-striped active progressbar-bar'>" +
                    "<div class='progress-bar progress-bar-danger' style='width: 0%;'>" +
                    "</div>" +
                    "</div>" +
                    "<div class='progressbar-project-percentage_" + PhotoIndexNumber + "'>" +
                    "<span></span>" +
                    "</div>" +
                    "</div>";

                var CaptionText = "Add caption";
                if (typeof (CaptionTextTranslation) != "undefined") {
                    CaptionText = CaptionTextTranslation;
                }
                var CaptionErrorText = "The content have exceeded 30 characters";
                if (typeof (CaptionErrorTextTranslation) != "undefined") {
                    CaptionErrorText = CaptionErrorTextTranslation;
                }
                $(".upload-gallary").append(" <div id='NewFileUp_" + PhotoIndexNumber + "' class='dv-preview-pic '><div class='image-delete' style='cursor:pointer;' newImgDataId='" + PhotoIndexNumber + "'><img src='/Content/Icons/cross_update.png' /></div><div class='dv-preview-pic-inner'><img class='preview-pic project-photo' src='' /></div><input type='text' propertycontentid='' class='image-caption' placeholder='" + CaptionText + "' /><label class='label hidden red lblCaptionError' id=''>" + CaptionErrorText + ".</label>" + ProgressBarString + "</div></div>");
                jqXHR[PhotoIndexNumber] = data.submit();
                PhotoIndexNumber++;
            }
            else {
                parent.OpenErrorMessageNew("", "You can upload upto 5 files at a time.");
            }
        },
        done: function (event, data) {
            console.log(data)
            if (data.result.status.length > 0) {
                for (var u = 0; u < data.result.status.length; u++) {
                    var resultdata = data.result.status[u];
                    $("#hf-uploaded-cover-path").val(resultdata.File);
                    fullImagePath = resultdata.filepath;
                    var filename = resultdata.filename;
                    var spfile = resultdata.filename.split('.');
                    var size = resultdata.size;
                    var index = spfile.length - 1;
                    var NewIdName = "#NewFileUp_" + data.files[u].uploadID;
                    $(NewIdName).addClass("preview_" + data.files[u].uploadID);
                    $(NewIdName + " .image-delete").attr('data-id', data.files[u].uploadID);
                    if (spfile[index] == "pdf") {
                        $(NewIdName + " .preview-pic").attr('src', domainurl + '/Content/Icons/pdf.png');
                        $(NewIdName + " .preview-pic").attr('data-link', fullImagePath);
                    }
                    else if (spfile[index] == "doc" || spfile[index] == "docx") {
                        $(NewIdName + " .preview-pic").attr('src', domainurl + '/Content/Icons/docx.png');
                        $(NewIdName + " .preview-pic").attr('data-link', fullImagePath);
                    }
                    else if (spfile[index] == "mp4" || spfile[index] == "mov") {
                        $(NewIdName + " .preview-pic").attr('src', domainurl + '/Content/Icons/mp4.png');
                        $(NewIdName + " .preview-pic").attr('data-link', fullImagePath);
                    }
                    else if (spfile[index] == "xlsx" || spfile[index] == "xls") {
                        $(NewIdName + " .preview-pic").attr('src', domainurl + '/Content/Icons/xlsx.png');
                        $(NewIdName + " .preview-pic").attr('data-link', fullImagePath);
                    }
                    else if (spfile[index] == "txt") {
                        $(NewIdName + " .preview-pic").attr('src', domainurl + '/Content/Icons/text.png');
                        $(NewIdName + " .preview-pic").attr('data-link', fullImagePath);
                    }
                    else if (spfile[index] == "msg") {
                        $(NewIdName + " .preview-pic").attr('src', domainurl + '/Content/Icons/msg.png');
                        $(NewIdName + " .preview-pic").attr('data-link', fullImagePath);
                    }
                    else if (spfile[index] == "pptx") {
                        $(NewIdName + " .preview-pic").attr('src', domainurl + '/Content/Icons/pptx.png');
                        $(NewIdName + " .preview-pic").attr('data-link', fullImagePath);
                    }
                    else if (spfile[index] == "ppt") {
                        $(NewIdName + " .preview-pic").attr('src', domainurl + '/Content/Icons/ppt.png');
                        $(NewIdName + " .preview-pic").attr('data-link', fullImagePath);
                    }
                    else {
                        $(NewIdName + " .preview-pic").attr('src', fullImagePath);
                        $(NewIdName + " .preview-pic").attr('data-link', fullImagePath);
                    }
                    $(NewIdName + " .image-caption").val(filename.substring(0, filename.lastIndexOf('.')));
                    $(NewIdName + " .lblCaptionError").attr('id', "lblCaptionErr_" + resultdata.Id);
                    $(NewIdName + " .preview-pic").attr('data-size', size);
                }
                var piccount = $(".upload-gallary img.preview-pic").length;
                if (piccount > 0) {
                    $(".project-blank-thumb-img").attr("src", "/Content/Icons/blank_thumb_file.png");
                }
                if (piccount == 20) {
                    $(".project-blank-thumb").hide();
                }
                setTimeout(function () {
                    $(".project-progress_" + data.files[0].uploadID).hide();
                    $(".project-progress .progress-bar").animate({
                        width: "0%"
                    }, 10);
                    $(".progressbar-project-percentage span").text(+ "0%");
                }, 1000);
            }
            $('.addcarrier_inner').scrollTop($('.addcarrier_inner')[0].scrollHeight);
        },
        fail: function (event, data) {
            if (data.files[0].error) {
                alert(data.files[0].error);
            }
        }
    })

    $("#fu-my-project-upload").on("change", function () {
        console.log("ajkbdj");
        if (jqXHRProjectData) {
            var piccount = $(".upload-gallary img.preview-pic").length;
            piccount += PhotoUploadXHRData.length;
            var uploadPhoto = PhotoUploadXHRData.length;
            var sizeLimit = false;
            for (var ur = 0; ur < PhotoUploadXHRData.length; ur++) {
                if (PhotoUploadXHRData[ur].files[0].size > 104857600)
                    sizeLimit = true;
            }
            if (!sizeLimit || uploadPhoto > 1) {
                if (piccount > 100) {
                    parent.OpenErrorMessageNew("", "Upto 100 files");
                }
                else {
                    var items = PhotoUploadXHRData[0];
                }
            }
            else {
                $("#lblProjectMgs").text("File size not more than 100MB.");
                $("#dvProjectMgsbox").show();
                setTimeout(function () {
                    $("#dvProjectMgsbox").hide();
                }, 3000);
            }
            PhotoUploadXHRData = [];
        }
        return false;
    });

    $("#hiperlink_table tbody").on('click', 'tr:last', function (e) {
        if ($(e.target).hasClass('fa')) {
            return;
        }
        $("#hiperlink_table tbody tr:last").after(NewEquipmentRow);
    });

    $("#relatedlink_table tbody").on('click', 'tr:last', function (e) {
        if ($(e.target).hasClass('fa')) {
            return;
        }
        $("#relatedlink_table tbody tr:last").after(NewRelatedRow);
    });

    $("#hiperlink_table tbody").on('click', 'tr td button.red_button', function (e) {
        $(this).parent().parent().remove();
        if ($("#hiperlink_table tbody tr").length < 2) {
            $("#hiperlink_table tbody tr:last").after(NewEquipmentRow);
        }
    });

    $("#relatedlink_table tbody").on('click', 'tr td button.red_button', function (e) {
        $(this).parent().parent().remove();
        if ($("#relatedlink_table tbody tr").length < 2) {
            $("#relatedlink_table tbody tr:last").after(NewRelatedRow);
        }
    });

    $("#SaveKnowledge").click(function () {
        $('#SaveKnowledge').attr("disabled", "disabled");
        SaveKnowledge();
    });
    $(".project-photo").click(function () {
        console.log("Download File");
        var imgurl = $(this).attr('data-link');
        window.location.href = domainurl + "/File/DownloadKnowledgeBaseFile?url=" + imgurl;
        
    });
});
