var previewfiles = function (item) {
    console.log("Download File");
    var imgurl = $(item).attr('data-link');
    var imgcaption = encodeURIComponent($(item).attr('data-caption'));
    console.log(imgcaption);
    if (imgurl.includes(".pdf") || imgurl.includes(".doc") || imgurl.includes(".docx") || imgurl.includes(".xlsx")
        || imgurl.includes(".xls") || imgurl.includes(".mp4") || imgurl.includes(".mov") || imgurl.includes(".txt")
        || imgurl.includes(".txt") || imgurl.includes(".msg") || imgurl.includes(".pptx")) {
        window.location.href = domainurl + "/File/DownloadKnowledgeBaseFile?url=" + imgurl;
    }
    else {
        ShowImage(imgurl, imgcaption);
    }
}
function autoScrolling() {
    $(".comment_box").animate({ scrollTop: $('.comment_box').prop("scrollHeight") }, 1000);
}
String.prototype.format = String.prototype.f = function () {
    var s = this,
        i = arguments.length;

    while (i--) {
        s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
    }
    return s;
};
function GetRecentComment() {
    $('#comment_section_text').val('')
    $.ajax({
        url: '/Sales/GetCommentForDocumentLibrary',
        type: 'post',
        data: { id: $("#Id").val() },
        success: function (daata) {
            var templage = '<tr><td>{0}<div class="comment_info">Added by: <b>{1}</b> on {2}</div></td></tr>'.format(daata.Comment, daata.Name, daata.DateC);
            $('#comm_aection_items').prepend(templage)
        }
    })
}
var DocumentFileList = function (order, Id) {
    console.log("HHHHHHH");
    var url = "/Sales/SortDocumentLibraryFileList";
    var param = JSON.stringify({
        order: order,
        Id: Id
    })
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            console.log("Success");
            if (data.result) {
                if (data.ImagesList.length > 0) {
                    var searchresultstring = "";
                    var FileTile = "";
                    var Size = "";
                    for (var i = 0; i < data.ImagesList.length; i++) {

                        if (data.ImagesList[i].ImageLoc.includes(".pdf")) {
                            FileTile = '<div class="dv-preview-pic-inner">'
                                + '<img class="preview-pic project-photo" onclick="previewfiles(this)" data-link="' + data.ImagesList[i].ImageLoc + '" src="/Content/Icons/pdf.png">'
                                + '</div>'
                        }
                        else if (data.ImagesList[i].ImageLoc.includes(".doc") || data.ImagesList[i].ImageLoc.includes(".docx")) {
                            FileTile = '<div class="dv-preview-pic-inner">'
                                + '<img class="preview-pic project-photo" onclick="previewfiles(this)" data-link="' + data.ImagesList[i].ImageLoc + '" src="/Content/Icons/docx.png">'
                                + '</div>'
                        }
                        else if (data.ImagesList[i].ImageLoc.includes(".xlsx") || data.ImagesList[i].ImageLoc.includes(".xls")) {
                            FileTile = '<div class="dv-preview-pic-inner">'
                                + '<img class="preview-pic project-photo" onclick="previewfiles(this)" data-link="' + data.ImagesList[i].ImageLoc + '" src="/Content/Icons/xlsx.png">'
                                + '</div>'
                        }
                        else if (data.ImagesList[i].ImageLoc.includes(".mp4") || data.ImagesList[i].ImageLoc.includes(".mov")) {
                            FileTile = '<div class="dv-preview-pic-inner">'
                                + '<img class="preview-pic project-photo" onclick="previewfiles(this)" data-link="' + data.ImagesList[i].ImageLoc + '" src="/Content/Icons/mp4.png">'
                                + '</div>'
                        }
                        else if (data.ImagesList[i].ImageLoc.includes(".txt")) {
                            FileTile = '<div class="dv-preview-pic-inner">'
                                + '<img class="preview-pic project-photo" onclick="previewfiles(this)" data-link="' + data.ImagesList[i].ImageLoc + '" src="/Content/Icons/text.png">'
                                + '</div>'
                        }
                        else if (data.ImagesList[i].ImageLoc.includes(".msg")) {
                            FileTile = '<div class="dv-preview-pic-inner">'
                                + '<img class="preview-pic project-photo" onclick="previewfiles(this)" data-link="' + data.ImagesList[i].ImageLoc + '" src="/Content/Icons/msg.png">'
                                + '</div>'
                        }
                        else if (data.ImagesList[i].ImageLoc.includes(".pptx")) {
                            FileTile = '<div class="dv-preview-pic-inner">'
                                + '<img class="preview-pic project-photo" onclick="previewfiles(this)" data-link="' + data.ImagesList[i].ImageLoc + '" src="/Content/Icons/pptx.png">'
                                + '</div>'
                        }
                        else if (data.ImagesList[i].ImageLoc.includes(".ppt")) {
                            FileTile = '<div class="dv-preview-pic-inner">'
                                + '<img class="preview-pic project-photo" onclick="previewfiles(this)" data-link="' + data.ImagesList[i].ImageLoc + '" src="/Content/Icons/ppt.png">'
                                + '</div>'
                        }
                        else {
                            FileTile = '<div class="dv-preview-pic-inner">'
                                + '<img class="preview-pic project-photo" onclick="previewfiles(this)" data-link="' + data.ImagesList[i].ImageLoc + '" src="' + data.ImagesList[i].ImageLoc + '" data-caption="'+ data.ImagesList[i].ImageType+'">'
                                + '</div>'
                        }
                        if (data.ImagesList[i].Size > 0) {
                            Size = parseFloat(data.ImagesList[i].Size).toFixed(2) + " KB";
                        }

                        searchresultstring += '<tr id="NewFileUp_' + data.ImagesList[i].Id + '" class="dv-preview-pic  preview_' + data.ImagesList[i].Id + ' list_style_image">' +
                            '<td>' +
                            FileTile
                            + '</td>'

                            +
                            '<td>'
                            +
                            '<span class="image-caption">' + data.ImagesList[i].ImageType + '</span>'
                            +
                            '</td>'
                            +
                            '<td>'
                            +
                            '<span class="image-caption">' + Size + '</span>'
                            +
                            '</td>'
                            +
                            '<td>'
                            +
                            '<span class="image-caption">' + data.ImagesList[i].Extension + '</span>'
                            +
                            '</td>'
                            + 
                            '<td>'
                            +
                            '<span class="image-date">' + data.ImagesList[i].StrUploadedDate + '</span>'
                            +
                            '</td>'
                            +
                            '</tr>';
                        Size = "";
                    }
                    $("#Filelist").html(searchresultstring);
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var CheckFlag2 = function (Id, flg) {
    var modelval = JSON.stringify({
        Id: Id,
        IsFlag: flg,
        comments: $('#comment_section_text').val()
    });
    var x = $.ajax({
        type: "POST",
        url: domainurl + "/Sales/FlagedDocumentLibraryArtical",
        data: modelval,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (flg) {
                $('.comment_section').show(300)
            } else {
                $('.comment_section').hide()
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);

        }
    });
    x.done(function () {
        GetRecentComment()
    })
}
var ShowImage = function (imgurl, imgcaption) {
    $(".DocPreview").attr("href", "/Sales/ImagePreview?url=" + imgurl + "&caption=" + imgcaption);
    $(".DocPreview").click();
};
var ClosePopup = function () {
    $.magnificPopup.close();
}

var AddDocumentLibrary = function (id) {
    window.location.href = domainurl + "/documentlibrary?isdocumentlibrary=true" + "&Id=" + id;
}

$(document).ready(function () {
    $(".pop_height").height(window.innerHeight - 55);

    var Popupwidth = 920;
    if (window.innerWidth < 920) {
        Popupwidth = window.innerWidth;
    }
    var PopupwidthCustom = 320;
    //if (window.innerWidth < 920) {
    //    PopupwidthCustom = window.innerWidth;
    //}
    var ConditionWidth = 600;
    if (window.innerWidth < 600) {
        ConditionWidth = window.innerWidth;
    }
    var ConditionHeight = 600;
    if (window.innerHeight < 600) {
        ConditionHeight = window.innerHeight;
    }

    var ReScheduleTicketDom = 500;
    if (window.innerWidth < 500) {
        ReScheduleTicketDom = window.innerWidth;
    }

    var idlist = [{ id: ".DocPreview", type: 'iframe', width: Popupwidth, height: ConditionHeight },
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });



    if ($('#answerid a').length > 0) {
        $('#answerid a').each(function () {
            var url = $(this).attr('href');
            if (url.toLowerCase().indexOf("http") > -1) {
                $(this).attr('target', '_blank');
            }
        });
    }
    $(".backtoKnowledge").click(function () {
        location.href = '/knowledgebase';
    });
    $(".backtoDocument").click(function () {
        location.href = '/documentlibrary?isdocumentlibrary=true';
    });
    if (typeof (SearchText) != 'undefined' && SearchText != null && SearchText != ''
        && SearchText != "") {
        var text = document.getElementById("KnMessageBody").innerHTML;
        var searched = SearchText.trim();
        var re = new RegExp(searched + '(?!([^<]+)?>)', "gi");
        var newText = text.replace(re, `<mark style='background-color:#f5bd56'>${searched}</mark>`);
        document.getElementById("KnMessageBody").innerHTML = newText;
    };

    $(".Flag").change(function () {
        var flg = $(".Flag").is(":checked");
        var Id = $("#Id").val();
        if (flg) {
            CheckFlag(Id, flg);
        }
        else {
            if (RemovedFlag.toLowerCase() == "true") {
                CheckFlag(Id, flg);
            }
            else {
                $(this).prop('checked', true);
            }
        }
    });
    $(".Flag2").change(function () {
        console.log("Check Click");
        var flg = $(".Flag2").is(":checked");
        var Id = $("#Id").val();
        if (flg) {
            CheckFlag2(Id, flg);
        }
        else {
            if (RemovedFlag.toLowerCase() == "true") {
                CheckFlag2(Id, flg);
            }
            else {
                $(this).prop('checked', true);
            }
        }
    });
    $("#comment_section_save").click(function () {
        if ($('#comment_section_text').val() != '') {
            $('#comment_section_text').removeClass('required')
            CheckFlag2($("#Id").val(), $(".Flag2").is(":checked"));
        } else {
            $('#comment_section_text').addClass('required')
        }
    });
    $(".icon_sort_timeclock").click(function () {
        var Id = $("#Id").val();
        var orderval = $(this).attr('data-val');
        DocumentFileList(orderval, Id);
    });
});

$(window).resize(function () {
    $(".pop_height").height(window.innerHeight - 55);
});