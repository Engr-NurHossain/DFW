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
    $("#knlove_checkbox").addClass("srch_header_show");
    $("#CustomSrchHeader").addClass("srch_header_none");
    $('#flaglbl').text(totalflagcount);
    $('#favoritelbl').text(totalfavoritecount);
    console.log(totalfavoritecount);
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
    if (SearchText != null && SearchText != ''
        && typeof (SearchText) != 'undefined' && SearchText != "") {
        $("#KnowledgebaseSearchText").val(SearchText);
        var text = document.getElementById("know_base_table").innerHTML;
        var searched = document.getElementById("KnowledgebaseSearchText").value.trim();
        var re = new RegExp(searched + '(?!([^<]+)?>)', "gi");
        var newText = text.replace(re, `<mark style='background-color:#f5bd56'>${searched}</mark>`);
        document.getElementById("know_base_table").innerHTML = newText;
    }

    $(".icon_sort_timeclock").click(function () {
        var search = encodeURI($("#btn_search_Knowledge").val());
        var orderval = $(this).attr('data-val');
        loadKnowledge(pagenumber, orderval, search);
    });
});