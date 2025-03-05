var ShowDetailsPopup = function (ev) {
    //console.log(ev.currentTarget+"qwerty");
    //var detailsData = ev.currentTarget.dataset.details;
    //detailsData = '<span class="calerrorpopupclose" onclick="HidePopupDetails()">&times;</span>' + detailsData;
    //var clientX = parseInt(ev.clientX);
    //var clientY = parseInt(ev.clientY);
    //clientX = clientX - 105;
    //if (clientY > 319) {
    //    clientY = clientY - 265;
    //}
    //else {
    //    clientY = clientY + 10;
    //}
    //ev.stopPropagation();
    //ev.cancelBubble = true;
    //$('#detailsdata1').html('');
    //$('#detailsdata1').html(detailsData);
    //$("#detailspopup1").css({
    //    "left": clientX + "px", "top": clientY + "px"
    //});
    //var options = { direction: "top" };
    //$('#detailspopup1').toggle('slide', options, 1);
    //HidePopupDetails();
    $('.popup_cl').addClass("hidden");

    var a = ev.currentTarget.id;
    $('#popup_' + a).removeClass("hidden");

    
}
var HidePopupDetails = function () {
   // $('.popup_cl').html('');
    $('.popup_cl').addClass("hidden");
} 