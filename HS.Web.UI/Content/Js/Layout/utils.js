var newUrl = '/';
var magnificPopupObj = function (item) {
    var topcal = $(window).height() / 2 - item.height / 2;
    var leftcal = $(window).width() / 2 - item.width / 2;

    var markUpTags = '';
    var isFixedContentPosition = true;
    if (item.isAbsolute) {
        isFixedContentPosition = false;
        topcal = item.top ? item.top : 0;
        var heightCSS = item.height ? "height: " + item.height + "px;" : "";
        markUpTags = '<div style="margin-left:auto;margin-right:auto;right: 0px;' + heightCSS + 'width: ' + item.width + 'px;top:' + topcal + 'px;left:0px;position:absolute;opacity: 1; overflow: hidden;display: block;background-color: #fff;border-radius: 3px;padding:25px;">' +
                '<div class="mfp-iframe-scaler" >' +
                '<iframe class="mfp-iframe" frameborder="0" allowfullscreen></iframe>' +
                '</div></div>';
    }
    else {
        markUpTags = '<div style="width: ' + item.width + 'px;height: ' + item.height + 'px;top:' + topcal + 'px;left:' + leftcal + 'px;position: fixed;opacity: 1; overflow: hidden;display: block;background-color: #fff;border-radius: 3px;">' +
               '<div class="mfp-iframe-scaler" >' +
               '<iframe class="mfp-iframe" frameborder="0" allowfullscreen></iframe>' +
               '</div></div>';
    }

    $(item.id).magnificPopup({
        preloader: true,
        type: item.type,
        iframe: {
            markup: markUpTags
        },
        modal: true,
        fixedContentPos: isFixedContentPosition,
        callbacks: {
            beforeOpen: function () {
                if (typeof (item.beforeopenCallback) == "function") item.beforeopenCallback();
            },
            open: function () {
                if (typeof (item.openCallback) == "function") item.openCallback();
            },
            beforeClose: function () {
                if (typeof (item.beforeCloseCallback) == "function") item.beforeCloseCallback();
                /*console.log("before close");*/
            },
            close: function () {
                if (typeof (item.closeCallback) == "function") {
                    item.closeCallback();
                    /*console.log(" close");*/
                    topcal = $(window).height() / 2 - item.height / 2;
                    leftcal = $(window).width() / 2 - item.width / 2;
                }
            },
            afterClose: function () {
                if (typeof (item.aftercloseCallback) == "function") item.aftercloseCallback();
                /*console.log("after close");*/
            },
            change: function () {
                if (typeof (item.changeCallback) == "function") {
                    item.changeCallback();
                    /*console.log("change");*/
                    topcal = $(window).height() / 2 - item.height / 2;
                    leftcal = $(window).width() / 2 - item.width / 2;
                }
            }
        }
    });
}
$(window).resize(function () {
    var $magnificSection = $(".mfp-content > div");

    if ($magnificSection.css("position") === "absolute") {
        $magnificSection.css('left', '0px');
        /*$magnificSection.css('top', parseInt($(window).height() * -.35));*/
    }
    else {
        $magnificSection.css('left', (window.innerWidth / 2 - $magnificSection.width() / 2) + 'px');
        $magnificSection.css('top', (window.innerHeight / 2 - $magnificSection.height() / 2) + 'px');
    }
});

var magnific_helpers = {
    SetContentHight: function () {
        if (typeof $.magnificPopup != 'undefined' || typeof parent.$.magnificPopup != 'undefined') {
            var magnificPopup = typeof $.magnificPopup != 'undefined' ? $.magnificPopup.instance : parent.$.magnificPopup.instance;
            if (magnificPopup) {
                var bodyHeight = $(magnificPopup.content).find("iframe").contents().find("body").height();
                if (typeof $.magnificPopup != 'undefined')
                    $('.mfp-content>div').animate({ 'height': bodyHeight }, 100);
                else
                    parent.$('.mfp-content>div').animate({ 'height': bodyHeight }, 100);
            }
        }
    },
    SetContentHeightForForm: function () {
        if (typeof $.magnificPopup != 'undefined' || typeof parent.$.magnificPopup != 'undefined') {
            var magnificPopup = typeof $.magnificPopup != 'undefined' ? $.magnificPopup.instance : parent.$.magnificPopup.instance;
            if (magnificPopup) {
                var bodyHeight = 0;
                if ($('form').length > 0)
                    bodyHeight = $('form').height() + 60;
                else
                    bodyHeight = $('body>div').height() + 60;

                if (typeof $.magnificPopup != 'undefined')
                    $('.mfp-content>div').animate({ 'height': bodyHeight }, 100);
                else
                    parent.$('.mfp-content>div').animate({ 'height': bodyHeight }, 100);
            }
        }
    }

}

var ClearURL = function () {
    $.magnificPopup.close();
    history.pushState({ old_url: "/", new_url: newUrl }, null, newUrl);
}
if (!String.format) {
    String.format = function (format) {
        var args = Array.prototype.slice.call(arguments, 1);
        return format.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
              ? args[number]
              : match
            ;
        });
    };
}