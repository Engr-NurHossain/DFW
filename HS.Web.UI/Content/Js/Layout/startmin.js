$(function() {

    $('#side-menu').metisMenu();

});

//Loads the correct sidebar on window load,
//collapses the sidebar on window resize.
// Sets the min-height of #page-wrapper to window size
$(function() {
    $(window).bind("load resize", function() {
        topOffset = 50;
        width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            $('div.navbar-collapse').addClass('collapse');
            topOffset = 100; // 2-row-menu
        } else {
            $('div.navbar-collapse').removeClass('collapse');
        }

        height = ((this.window.innerHeight > 0) ? this.window.innerHeight : this.screen.height) - 1;
        height = height - topOffset;
        if (height < 1) height = 1;
        if (height > topOffset) {
            //$(".page-wrapper-contents").css("min-height", (height) + "px");
        }
        if (window.innerWidth < 768) {
            $(".PrivateLayoutContents").width(window.innerWidth);
        }
        else {
            $(".PrivateLayoutContents").width(window.innerWidth - (235 + 1)); /*1 is for error fixing for 1920*1080*/
        }
        
    });
    if (window.innerWidth < 768) {
        $(".PrivateLayoutContents").width(window.innerWidth);
    }
    else {
        $(".PrivateLayoutContents").width(window.innerWidth - (235 +1));/*1 is for error fixing for 1920*1080*/
    }
    var url = window.location;
    var element = $('ul.nav a').filter(function() {
        return this.href == url || url.href.indexOf(this.href) == 0;
    }).addClass('active').parent().parent().addClass('in').parent();
    if (element.is('li')) {
        element.addClass('active');
    }
});
$(window).resize(function ()
{
    setTimeout(function () {
        if (window.innerWidth < 768) {
            $(".PrivateLayoutContents").width(window.innerWidth);
        }
        else {
            $(".PrivateLayoutContents").width(window.innerWidth - (235 + 1)); /*1 is for error fixing for 1920*1080*/
        }
    }, 50);
});