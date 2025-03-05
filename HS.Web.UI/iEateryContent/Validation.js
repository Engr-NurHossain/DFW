var Validation = {
    IsEmpty: function (ctrl) {
        var result = true;
        var value = $.trim($(ctrl).val());
        $(ctrl).val(value);
        var label;
        var labels = $($(ctrl).parent().parent()).find('label');
        if (labels.length > 1) {
            var cltrName = $(ctrl).attr('name');
            for (var ind = 0; ind < labels.length; ind++) {
                if ($(labels[ind]).attr('rel') == cltrName) {
                    label = $(labels[ind]);
                    break;
                }
            }
        }
        else
            label = labels;

        if (value == "") {
            $(ctrl).addClass('required');
            $(label).removeClass('hidden');
            if (typeof $.magnificPopup != 'undefined')
                magnific_helpers.SetContentHight();
            return true;
        }
        $(label).addClass('hidden');
        $(ctrl).removeClass('required');
        if (typeof $.magnificPopup != 'undefined')
            magnific_helpers.SetContentHight();
        return false;
    },
    IsEmail: function (ctrl) {
        var value = $.trim($(ctrl).val());

        var label;
        var labels = $($(ctrl).parent().parent()).find('label');
        if (labels.length > 1) {
            var cltrName = $(ctrl).attr('name');
            for (var ind = 0; ind < labels.length; ind++) {
                if ($(labels[ind]).attr('rel') == cltrName) {
                    label = $(labels[ind]);
                    break;
                }
            }
        }
        else
            label = labels;

        $(ctrl).val(value);
        if ($(ctrl).val() != "") {
            var regexp = /^[\+a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
            if (RegularExpressionCheck($(ctrl).val(), regexp)) {
                $(ctrl).removeClass('required');
                $(label).addClass('hidden');
                if (typeof $.magnificPopup != 'undefined')
                    magnific_helpers.SetContentHight();
                return true;
            }
        }
        $(ctrl).addClass('required');
        $(label).removeClass('hidden');
        if (typeof $.magnificPopup != 'undefined')
            magnific_helpers.SetContentHight();
        return false;
    },
    IsPassord: function (ctrl) {
        var value = $(ctrl).val();

        var label;
        var labels = $($(ctrl).parent().parent()).find('label');
        if (labels.length > 1) {
            var cltrName = $(ctrl).attr('name');
            for (var ind = 0; ind < labels.length; ind++) {
                if ($(labels[ind]).attr('rel') == cltrName) {
                    label = $(labels[ind]);
                    break;
                }
            }
        }
        else
            label = labels;

        if ($(ctrl).val() != "") {

            if (value.length > 5) {
                $(ctrl).removeClass('required');
                $(label).addClass('hidden');
                if (typeof $.magnificPopup != 'undefined')
                    magnific_helpers.SetContentHight();
                return true;
            }
        }
        $(ctrl).addClass('required');
        $(label).removeClass('hidden');
        if (typeof $.magnificPopup != 'undefined')
            magnific_helpers.SetContentHight();
        return false;
    },
    IsPhone: function (ctrl) {
        var value = $.trim($(ctrl).val());
     
        var pCountry = $(ctrl).parent().parent();
        var plabel;
        var labels = $($(ctrl).parent().parent()).find('label');
        if (labels.length > 1) {
            var cltrName = $(ctrl).attr('name');
            for (var ind = 0; ind < labels.length; ind++) {
                if ($(labels[ind]).attr('rel') == cltrName) {
                    plabel = $(labels[ind]);
                    break;
                }
            }
        }
        else
            plabel = labels;

        var pSelect = $(pCountry).find('select');
        if (typeof pSelect != 'undefined') {
            if ($(pSelect).val() == 'Others') {
                value = '+' + value;
            }
        }

        if ($(ctrl).val() != "") {
            if (this.IsPhoneNumber(value, $(pSelect).val())) {
                $(plabel).addClass('hidden');
                $(ctrl).removeClass('required');
                if (typeof $.magnificPopup != 'undefined')
                    magnific_helpers.SetContentHight();
                return true;
            }
        }
        $(plabel).removeClass('hidden');
        $(ctrl).addClass('required');
        if (typeof $.magnificPopup != 'undefined')
            magnific_helpers.SetContentHight();
        return false;

    },
   IsPhoneNumber: function (phone) {
   
    try {
       
        if (phone != undefined && phone != null && phone != "")
        {
            phone = phone.replace(/[-  ]/g, '');
                
                
            if (phone.length == 10) {
                   
                return true;
            }
            else if (phone.length > 10) {
                   
                return false;
            }
            else if (phone.length < 10) {
                  
                return false;
            }
            else{
                return false;
            }
                
        }
    }
    catch (e) {
        return false;
    }
   },
   IsSSN: function (ctrl) {
       var value = $.trim($(ctrl).val());
       var pCountry = $(ctrl).parent().parent();
       var plabel;
       var labels = $($(ctrl).parent().parent()).find('label');
       if (labels.length > 1) {
           var cltrName = $(ctrl).attr('name');
           for (var ind = 0; ind < labels.length; ind++) {
               if ($(labels[ind]).attr('rel') == cltrName) {
                   plabel = $(labels[ind]);
                   break;
               }
           }
       }
       else
           plabel = labels;

       var pSelect = $(pCountry).find('select');
       if (typeof pSelect != 'undefined') {
           if ($(pSelect).val() == 'Others') {
               value = '+' + value;
           }
       }

       if ($(ctrl).val() != "") {
           if (this.IsSSNNumber(value, $(pSelect).val())) {
               $(plabel).addClass('hidden');
               $(ctrl).removeClass('required');
               if (typeof $.magnificPopup != 'undefined')
                   magnific_helpers.SetContentHight();
               return true;
           }
       }
       $(plabel).removeClass('hidden');
       $(ctrl).addClass('required');
       if (typeof $.magnificPopup != 'undefined')
           magnific_helpers.SetContentHight();
       return false;

   },
   IsSSNNumber: function (phone) {

       try {

           if (phone != undefined && phone != null && phone != "") {
               phone = phone.replace(/[-  ]/g, '');


               if (phone.length == 10) {

                   return true;
               }
               else if (phone.length > 10) {

                   return false;
               }
               else if (phone.length < 10) {

                   return false;
               }
               else {
                   return false;
               }

           }
       }
       catch (e) {
           return false;
       }
   },
   IsCardEX: function (ctrl) {
       var value = $.trim($(ctrl).val());
       var pCountry = $(ctrl).parent().parent();
       var plabel;
       var labels = $($(ctrl).parent().parent()).find('label');
       if (labels.length > 1) {
           var cltrName = $(ctrl).attr('name');
           for (var ind = 0; ind < labels.length; ind++) {
               if ($(labels[ind]).attr('rel') == cltrName) {
                   plabel = $(labels[ind]);
                   break;
               }
           }
       }
       else
           plabel = labels;

       var pSelect = $(pCountry).find('select');
       if (typeof pSelect != 'undefined') {
           if ($(pSelect).val() == 'Others') {
               value = '+' + value;
           }
       }

       if ($(ctrl).val() != "") {
           if (this.IsCardNumberEX(value, $(pSelect).val())) {
               $(plabel).addClass('hidden');
               $(ctrl).removeClass('required');
               if (typeof $.magnificPopup != 'undefined')
                   magnific_helpers.SetContentHight();
               return true;
           }
       }
       $(plabel).removeClass('hidden');
       $(ctrl).addClass('required');
       if (typeof $.magnificPopup != 'undefined')
           magnific_helpers.SetContentHight();
       return false;

   },
   IsCardNumberEX: function (cardno) {
           try {
               if (cardno != undefined && cardno != null && cardno != "" ) {
                   cardno = cardno.replace(/[/  ]/g, '');
                   if (cardno.length == 4) {

                       return true;
                   }

                   else {
                       return false;
                   }

               }
           }
           catch (e) {
               return false;
           }       
   },
   IsSecurity: function (ctrl) {
       var value = $.trim($(ctrl).val());
       var pCountry = $(ctrl).parent().parent();
       var plabel;
       var labels = $($(ctrl).parent().parent()).find('label');
       if (labels.length > 1) {
           var cltrName = $(ctrl).attr('name');
           for (var ind = 0; ind < labels.length; ind++) {
               if ($(labels[ind]).attr('rel') == cltrName) {
                   plabel = $(labels[ind]);
                   break;
               }
           }
       }
       else
           plabel = labels;

       var pSelect = $(pCountry).find('select');
       if (typeof pSelect != 'undefined') {
           if ($(pSelect).val() == 'Others') {
               value = '+' + value;
           }
       }

       if ($(ctrl).val() != "") {
           if (this.IsSecurityCode(value, $(pSelect).val())) {
               $(plabel).addClass('hidden');
               $(ctrl).removeClass('required');
               if (typeof $.magnificPopup != 'undefined')
                   magnific_helpers.SetContentHight();
               return true;
           }
       }
       $(plabel).removeClass('hidden');
       $(ctrl).addClass('required');
       if (typeof $.magnificPopup != 'undefined')
           magnific_helpers.SetContentHight();
       return false;

   },
   IsSecurityCode: function (cardno) {
       try {
           if (cardno != undefined && cardno != null && cardno != "") {
               
               if (cardno.length == 3) {

                   return true;
               }

               else {
                   return false;
               }

           }
       }
       catch (e) {
           return false;
       }
   }, 
    MaxLengthCheck :function(ctrl){
        var label;
        var labels = $($(ctrl).parent().parent()).find('label');
        if (labels.length > 1) {
            var cltrName = $(ctrl).attr('name');
            for (var ind = 0; ind < labels.length; ind++) {
                if ($(labels[ind]).attr('rel').hasClass('maxlenghterr') == cltrName) {
                    label = $(labels[ind]);
                    break;
                }
            }
        }
        else {
            label = labels;
        }
        if ($(ctrl).attr('maxlengthcheck') < $(ctrl).val().length) {
            $(ctrl).addClass('required');
            $(label).removeClass('hidden');
            return false;
        }
        $(ctrl).removeClass('required');
        $(label).addClass('hidden');
        return true;
    },
    ForTextbox: function (inputctl) {
        var result = true;
        var check = false;
        if ($(inputctl).attr('datarequired') == 'true') {
            check = !Validation.IsEmpty($(inputctl));
            if (!check)
                result = check;
        }
        if ($(inputctl).attr('dataformat') == 'email') {
            check = Validation.IsEmail($(inputctl));
            if (!check)
                result = check;
        }
        if ($(inputctl).attr('dataformat') == 'phone') {
        
            check = Validation.IsPhone($(inputctl));
            if (!check)
                result = check;
        }
        if ($(inputctl).attr('dataformat') == 'ssn') {
            check = Validation.IsSSN($(inputctl));
            if (!check)
                result = check;
        }
        if ($(inputctl).attr('dataformat') == 'cardex') {
            check = Validation.IsCardEX($(inputctl));
            if (!check)
                result = check;
        }
        if ($(inputctl).attr('dataformat') == 'security') {
            check = Validation.IsSecurity($(inputctl));
            if (!check)
                result = check;
        }
        if ($(inputctl).attr('dataformat') == 'password') {
            check = Validation.IsPassord($(inputctl));
            if (!check)
                result = check;
        }
        if ($(inputctl).attr('dataformat') == 'price') {
            check = Validation.IsPrice($(inputctl));
            if (!check)
                result = check;
        }
        if ($(inputctl).attr('dataformat') == 'builtuparea') {
            check = Validation.IsBuiltUpArea($(inputctl));
            if (!check)
                result = check;
        }
        if (result && $(inputctl).attr('maxlengthcheck') > 0) {
            check = Validation.MaxLengthCheck($(inputctl));
            if (!check)
                result = check;
        }
        return result;
    },
    ForSelect: function (ctl) {
        var tempCtl = ctl;
        var formctl = $(ctl).parent().parent();
        var inputvalue = $(formctl).find('select');
        var labelmgs;
        var labels = $($(ctl).parent().parent()).find('label');
        if (labels.length > 1) {
            var cltrName = $(ctl).attr('name');
            for (var ind = 0; ind < labels.length; ind++) {
                if ($(labels[ind]).attr('rel') == cltrName) {
                    labelmgs = $(labels[ind]);
                    break;
                }
            }
        }
        else
            labelmgs = labels;
        var selectedvalue = '';
        for (var i = 0; i < inputvalue.length; i++) {
            if ($(inputvalue[i]).attr('name') == $(ctl).attr('name'))
                selectedvalue = $(inputvalue[i]).val();
        }
        if (selectedvalue == '' || selectedvalue == '-1' || selectedvalue == null || typeof (selectedvalue)=="undefined") {
            $(ctl).addClass('required');
            $(labelmgs).removeClass('hidden');
            return false;
        }
        else {
            $(ctl).removeClass('required');
            $(labelmgs).addClass('hidden');
            return true;
        }
    },
    ForCheckBox: function (ctl) {
        var labelmgs;
        var labels = $($(ctl).parent().parent()).find('label');
        if (labels.length > 1) {
            var cltrName = $(ctl).attr('name');
            for (var ind = 0; ind < labels.length; ind++) {
                if ($(labels[ind]).attr('rel') == cltrName) {
                    labelmgs = $(labels[ind]);
                    break;
                }
            }
        }
        else
            labelmgs = labels;
        if (!$(ctl).is(':checked') && $(ctl).attr('checkedrequired') == 'true') {
            $(ctl).addClass('required');
            $(labelmgs).removeClass('hidden');
            return false;
        }
        else {
            $(ctl).removeClass('required');
            $(labelmgs).addClass('hidden');
            return true;
        }
    }
}
var RegularExpressionCheck = function (inputs, expression) {
    return expression.test(inputs);
};

var CommonUiValidation = function () {
 
    var result = true;
    var inputlist = $("input");
    //var inputPhone = $("input-phone-format");
    var textarealist = $("textarea");
    var selectList = $("select");
    for (var i = 0; i < selectList.length; i++) {
        if ($(selectList[i]).attr('datarequired') == 'true' && $(selectList[i]).is(":visible")) {
            var check = Validation.ForSelect($(selectList[i]));
            if (!check)
                result = check;
        }
    }
    for (var i = 0; i < inputlist.length; i++) {
        if ($(inputlist[i]).attr('type') == 'checkbox' && $(inputlist[i]).is(":visible") && $(inputlist[i]).attr('checkedrequired') == 'true') {
            var check = Validation.ForCheckBox($(inputlist[i]));
            if (!check)
                result = check;
        }
        if (($(inputlist[i]).attr('datarequired') == 'true' || $(inputlist[i]).attr('daterequired') == 'true') && $(inputlist[i]).is(":visible")) {
            var check = Validation.ForTextbox($(inputlist[i]));
            if (!check)
                result = check;
        }
    }
    for (var i = 0; i < textarealist.length; i++) {
        
        if (($(textarealist[i]).attr('datarequired') == 'true' || $(textarealist[i]).attr('daterequired') == 'true') && $(textarealist[i]).is(":visible")) {
            var check = Validation.ForTextbox($(textarealist[i]));
            if (!check)
                result = check;
        }
    }
    return result;
}

$(document).ready(function () {
    $('input').focus(function () {
        var inputctl = this;
        if ($(inputctl).hasClass('required')) {
            $(inputctl).removeClass('required');
        }
    });
    $('input').blur(function () { 
        var inputctl = this;
        if ($(inputctl).attr('datarequired') == 'true') {
            Validation.IsEmpty($(inputctl));
        }
        if ($(inputctl).attr('dataformat') == 'email') {
            Validation.IsEmail($(inputctl));
        }
        if ($(inputctl).attr('dataformat') == 'phone') {
            Validation.IsPhone($(inputctl));
        }
        if ($(inputctl).attr('dataformat') == 'ssn') {
            Validation.IsSSN($(inputctl));
        }
        if ($(inputctl).attr('dataformat') == 'cardex') {
            Validation.IsCardEX($(inputctl));
        }
        if ($(inputctl).attr('dataformat') == 'security') {
            Validation.IsSecurity($(inputctl));
        }
        if ($(inputctl).attr('dataformat') == 'password') {
            Validation.IsPassord($(inputctl));
        }
        if ($(inputctl).attr('dataformat') == 'price') {
            Validation.IsPrice($(inputctl));
        }
        if ($(inputctl).attr('dataformat') == 'builtuparea') {
            Validation.IsBuiltUpArea($(inputctl));
        }
        if ($(inputctl).attr('dataformat') == 'propertyname') {
            Validation.IsPropertyName($(inputctl));
        }
        if ($(inputctl).attr('maxlengthcheck') > 0) {
            Validation.MaxLengthCheck($(inputctl));
        }
        if ($(inputctl).attr('loginformat') == 'login') {
            if ($(this).val() == "") {
                $("#lblLoginCommonError").addClass("hidden");
                $("#lblLoginError").addClass("hidden");
            }
        }
        if ($(inputctl).attr('datasignupformate') == 'user') {
            if ($(this).val() == "") {
                $("#lblSignUpError").addClass("hidden");
            }
        }
        if ($(inputctl).attr('dataformat') == 'devcompany') {
            if ($(this).val() == "") {
                $("#lblDevCompanyAlready").addClass("hidden");
            }
        }
    });
    $('textarea').blur(function () {
        var inputctl = this;
        if ($(inputctl).attr('datarequired') == 'true') {
            Validation.IsEmpty($(inputctl));
        }
    });
    $('select').change(function () {
        var selectItem = this;
        if ($(this).attr('datarequired') == 'true')
            Validation.ForSelect($(selectItem));
        //if (typeof $.magnificPopup != 'undefined')
        //    magnific_helpers.SetContentHight();
    });
});