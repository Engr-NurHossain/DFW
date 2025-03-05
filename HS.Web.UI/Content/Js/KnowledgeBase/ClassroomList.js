var ShowFullMessage = function (Fullurl) {
    window.open(Fullurl, '_blank');
};

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
var UndoKnowledgeBase = function (id) {
    OpenConfirmationMessageNew("Confirmation", "Do you want to revert this artical?", function () {
        var url = domainurl + "/Sales/UndoKnowledgebase?id=" + id;
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
var CopyLink = function (e) {
    var copyText = $(e).attr("data-url");
    navigator.clipboard.writeText(copyText);
    $(e).find(".lblcomment").show().delay(1000).fadeOut();

}
$(document).ready(function () {
    if (count > 0) {
        $("#btnKnowledgebaseDownload").removeClass("hidden");
    }
    $("#CustomSrch").removeClass("center_srch_block");
    $("#SrchSugg").addClass("srch_suggestion_hide");
    $("#srch_header_hide").addClass("srch_header_show");
    $("#CustomSrchHeader").addClass("srch_header_none");
    $('#flaglbl').text(totalflagcount);
    /*$('#favoritelbl').text(totalfavoritecount);*/
    $("#deletedknw").click(function () {
        $("#KnowledgebaseSearchText").val('');
        $(".deletelbl").css("display", "unset");
        $(".btn-add-booking").addClass('hidden');
        loadKnowledge(1, null, null, null, true);
    });
    if (CheckContact == "true") {
        if (CheckNavList.length != 0) {
            $("#favtag").selectpicker('val', CheckNavList);
        }
        else {
            $("#favtag").selectpicker('val', '');
        }
    }
    else {
        $("#favtag").selectpicker('val', TagList);
    }

    if (Flagged.toLocaleLowerCase() == "true") {
        $(".flag_filter").prop("checked", true);
    }
    $(".icon_sort_timeclock2").click(function () {
        var search = encodeURI($("#AssignedSearchText").val());
        var orderval = $(this).attr('data-val');
        loadclassroom(pagenumber, orderval, search, '', IsAdmin);
    });
    $(".icon_sort_timeclock3").click(function () {
        var search = encodeURI($("#CompletedSearchText").val());
        var orderval = $(this).attr('data-val');
        loadcompletedclassroom(pagenumber, orderval, search, '', IsAdmin);
    });
});