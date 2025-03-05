var step = parseInt( $("#step").val());
var CalActive = function () {
    step =  parseInt( $("#step").val());
    var lilist = $(".lilist");
    var activeCount = $(".lilist.current").length;
    //if (activeCount == 4) return false;
    for (var icoun = 0; icoun < lilist.length; icoun++) {
        var itemtarget = lilist[icoun];
        if ($(itemtarget).hasClass('current')) { 
            if (step == icoun + 1) {
                console.log(step);
                if (step != 4) {
                    $("#LoadLeadDetail").html(LoaderDom);
                }
                $($(itemtarget).next().find('a').parent()).addClass("current");
                $($(itemtarget).next().find('a').parent()).addClass("activeli");
                $($(itemtarget).find('a').parent()).removeClass("activeli");
                $("#LoadLeadDetail").load($($(itemtarget).next()).attr('data-url'));
                if (step == 3) {
                    $("#btnSavandNex span").text("Save & Sign");
                    $("#btnSavandClose").removeClass('hidden');
                }
                return false;
            }
        }
    }
}
var ClosePopup = function () {
    $.magnificPopup.close();
}
$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    $("#LoadPackage").addClass("activeli");
    $("#btnSavandNex").click(function () {
        console.log('btnSavandNex2')
        if (CommonUiValidation()) {
            CalActive();
        }
    });
    $(".cd-breadcrumb li").click(function (item) {
        var SelectVal = 0;
        var liselected = $(item.target).parent();
        var liselectedspan = $(item.target).parent().parent();
        var selectedindex = -1;
        if ($(liselectedspan).hasClass('current'))
            liselected = liselectedspan;
        if ($(liselected).hasClass('current')) {
            console.log(liselected);
            $(liselected).addClass('activeli');
            var liList = $(".custom_container li");
            for (var ilicount = 0; ilicount < liList.length; ilicount++) {
                if ($(liList[ilicount]).attr('id') == $(liselected).attr('id')) {
                    selectedindex = ilicount;
                    $("#LoadLeadDetail").html(LoaderDom);
                    $("#LoadLeadDetail").load($(liselected).attr('data-url'));
                    $("#btnSavandNex span").text("Save & Next");
                }
            }
            selectedindex = selectedindex + 1;
            for (var ilicount2 = 0; ilicount2 < selectedindex; ilicount2++) {
                $(liList[ilicount2]).addClass('current');
            }
            for (var ilicount = 0; ilicount < liList.length; ilicount++) {
                if ($(liList[ilicount]).attr('id') != $(liselected).attr('id')) {
                    $(liList[ilicount]).removeClass('activeli');
                }
            }
            if (selectedindex == 4) {
                $("#btnSavandNex span").text("Save & Sign");
                $("#btnSavandClose").removeClass('hidden');
            }
            else {
                $("#btnSavandClose").addClass('hidden');
            }
        }
    });
});

