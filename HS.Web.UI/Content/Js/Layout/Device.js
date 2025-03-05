var Device = {
    Android: function () {
        return navigator.userAgent.match(/Android/i) ? true : false;
    },
    BlackBerry: function () {
        return navigator.userAgent.match(/BlackBerry/i) || navigator.userAgent.match(/BB/i) ? true : false;
    },
    iOS: function () {
        return navigator.userAgent.match(/iPhone|iPod/i) ? true : false;
    },
    iPad: function () {
        return navigator.userAgent.match(/iPad/i) ? true : false;
    },
    Windows: function () {
        return navigator.userAgent.match(/IEMobile/i) ? true : false;
    },
    All: function () {
        return (Device.Android() || Device.BlackBerry() || Device.iOS() || Device.Windows() || Device.iPad() || Device.isIpadOS() || Device.isIOS());
    },
    Mobile: function () {
        return (Device.Android() || Device.BlackBerry() || Device.iOS() || Device.Windows());
    },
    MobileGadget: function () {
        if (navigator.userAgent.match(/SmartTV/i)) {
            return false;
        }
        if (Device.Tablet()) return false;

        return (Device.Android() || Device.BlackBerry() || Device.iOS() || Device.Windows() || window.innerWidth < 769);
    },
    Tablet: function () {
        return (navigator.userAgent.match(/iPad/i) != null || ((navigator.userAgent.match(/Mobile/i) == null && navigator.userAgent.match(/Android/i) != null)));
    },
    Desktop: function () {
        return (!Device.Android() && !Device.BlackBerry() && !Device.iOS() && !Device.Windows() && !Device.iPad() && !(window.innerWidth > 768));
    },
    Potrait: function () {
        if (window.innerHeight > window.innerWidth) {
            return true;
        }
    },
    Lanscape: function () {
        if (window.innerHeight < window.innerWidth) {
            return true;
        }
    },
    isIOS: function () {
        if (/iPad|iPhone|iPod/.test(navigator.platform)) {
            return true;
        } else {
            return navigator.maxTouchPoints &&
                navigator.maxTouchPoints > 2 &&
                /MacIntel/.test(navigator.platform);
        }
    },
    isIpadOS: function () {
        return navigator.maxTouchPoints &&
            navigator.maxTouchPoints > 2 &&
            /MacIntel/.test(navigator.platform);
    }
};
 