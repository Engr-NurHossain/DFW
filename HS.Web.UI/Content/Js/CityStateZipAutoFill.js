var itemval;
var selectedZip;
var Invalidzipmsg = "Invalid Zip Code";
var CityStateSuggestiontemplate =
               "<div class='tt-suggestion tt-selectable' data-zip = '{0}' data-city = '{1}' data-state = '{2}' data-county = '{3}'>"
                   + "<p class='tt-sug-text'>"
                   + "<span class='tt-sug-zip'>{0}</span>"
                   + "<span class='tt-eq-state'>{2}</span><br />"
                   + "<span class='tt-eq-city'>{1}&nbsp;</span>"
                   + "</p> "
    + "</div>";
var FirstLetterUppercase = function (str) {
    str = str.toLowerCase().replace(/\b[a-z]/g, function (letter) {
        return letter.toUpperCase();
    });
    return str;
}
var CityStateSearchKeyDown = function (item, event) {

    if (event.keyCode == 13) {
        var selectedTTMenu = $(event.target).parent().find('.tt-suggestion.active');
        if (selectedTTMenu.length > 0) {
            setTimeout(function () { $(selectedTTMenu).click(); }, 10)
            $('.tt-menu').hide();
        }
    }
    if (event.keyCode == 40) {
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');
        if ($(ttSugstionDom).length > 0) {
            if ($(ttSugActive).length == 0) {
                $($(ttSugstionDom).get(0)).addClass('active');
                $(item).val($($(ttSugstionDom).get(0)).attr('data-select'))
            }
            else {
                var suggestionlist = $(ttSugstionDom);
                var activesuggestion = $(ttSugActive);
                var indexactive = -1;
                for (var id = 0; id < suggestionlist.length; id++) {
                    if ($(suggestionlist[id]).hasClass('active'))
                        indexactive = id;
                }
                if (indexactive < suggestionlist.length - 1) {
                    $(ttSugstionDom).removeClass('active');
                    var possibleactive = $(ttSugstionDom).get(indexactive + 1);
                    $($(ttSugstionDom).get(indexactive + 1)).addClass('active');
                    $(possibleactive).addClass('active');
                    $(item).val($(possibleactive).attr('data-select'));
                }
            }
            event.preventDefault();
        }
    }
    if (event.keyCode == 38) {
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');
        if ($(ttSugstionDom).length > 0 && $(ttSugActive).length > 0) {
            var suggestionlist = $(ttSugstionDom);
            var activesuggestion = $(ttSugActive);
            var indexactive = -1;
            for (var id = 0; id < suggestionlist.length; id++) {
                if ($(suggestionlist[id]).hasClass('active'))
                    indexactive = id;
            }
            if (indexactive > 0) {
                $(ttSugstionDom).removeClass('active');
                var possibleactive = $(ttSugstionDom).get(indexactive - 1);
                $(possibleactive).addClass('active');
                $(item).val($(possibleactive).attr('data-select'))
            }
            event.preventDefault();
        }
    }
}
var CityStateSearchKeyUp = function (item, event) {
    console.log(event.keyCode);
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13)
        return false;

    if ($(item).val().toString().length >= 3) {
        $.ajax({
            url: domainurl + "/Leads/GetCityStateZipListByKey",
            data: {
                key: $(item).val()
            },
            contentType: 'application/x-www-form-urlencoded',
            dataType: "json",
            success: function (data) {
                var resultparse = JSON.parse(data.result);
                console.log(resultparse);
                if (resultparse.length > 0) {
                    var searchresultstring = "<div class='CityStateZipSuggestion'>";
                    for (var i = 0; i < resultparse.length; i++) {
                        searchresultstring = searchresultstring + String.format(CityStateSuggestiontemplate,
                            resultparse[i].ZipCode,/*{0}*/
                            resultparse[i].City,/*{1}*/
                            resultparse[i].State, /*{2}*/
                            resultparse[i].County /*3*/);
                    }
                    searchresultstring += "</div>";
                    var ttdom = $($(item).parent()).find('.tt-menu');
                    var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                    $(ttdomComplete).html(searchresultstring);
                    $(ttdom).show();

                    CityStateclickbind(item);
                    if (resultparse.length > 5) {
                        $(".CityState_insert_div .CityState_insert_div_previous .CityStateZipSuggestion").height(200);
                        //$(".NewProjectSuggestion").css('position', 'relative');
                    }
                }
                if (resultparse.length == 0)
                    $('.tt-menu').hide();
            }
        });
    }
    else {
        $('.tt-menu').hide();
    }
}
var CityStateSearchKeyUpwithrestrictedzipcode = function (item, event) {
    console.log(event.keyCode);
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13)
        return false;

    if ($(item).val().toString().length >= 3) {
        $.ajax({
            url: domainurl + "/Leads/GetCityStateZipListByKey",
            data: {
                key: $(item).val()
            },
            contentType: 'application/x-www-form-urlencoded',
            dataType: "json",
            success: function (data) {
                var resultparse = JSON.parse(data.result);
                console.log(resultparse);
                if (resultparse.length > 0) {
                    var searchresultstring = "<div class='CityStateZipSuggestion'>";
                    for (var i = 0; i < resultparse.length; i++) {
                        searchresultstring = searchresultstring + String.format(CityStateSuggestiontemplate,
                            resultparse[i].ZipCode,/*{0}*/
                            resultparse[i].City,/*{1}*/
                            resultparse[i].State, /*{2}*/
                            resultparse[i].County /*3*/);
                    }
                    searchresultstring += "</div>";
                    var ttdom = $($(item).parent()).find('.tt-menu');
                    var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                    $(ttdomComplete).html(searchresultstring);
                    $(ttdom).show();

                    CityStateclickbindwithrestrictedzipcode(item);
                    if (resultparse.length > 5) {
                        $(".CityState_insert_div .CityState_insert_div_previous .CityStateZipSuggestion").height(200);
                        //$(".NewProjectSuggestion").css('position', 'relative');
                    }
                }
                if (resultparse.length == 0)
                    $('.tt-menu').hide();
            }
        });
    }
    else {
        $('.tt-menu').hide();
    }
}
var checkNumber = function (e) {
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||
        // Allow: Ctrl+A, Command+A
           (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
        // Allow: home, end, left, right, down, up
           (e.keyCode >= 35 && e.keyCode <= 40)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
}
var CityStateclickbind = function (item) {
    $('.CityState_insert_div .tt-suggestion').click(function () {
        var clickitem = this;
        $('.CityState_insert_div .tt-menu').hide();
        selectedZip = $(clickitem).attr("data-zip").trim();
        $("#ZipCode").val(selectedZip);
        var selectedCity = $(clickitem).attr("data-city").trim();
        $("#City").val(FirstLetterUppercase(selectedCity));
        $("#City").attr("style", "border-color:#ccc;");
        var selectedState = $(clickitem).attr("data-state").trim();
        $("#State").val(selectedState);
        $("#State").attr("style", "border-color:#ccc;");
        ////  customerAdditionalinfo
        $("#BillingZipCode").val(selectedZip);
        var selectedCity = $(clickitem).attr("data-city").trim();
        $("#lBillingCity").val(FirstLetterUppercase(selectedCity));
        var selectedState = $(clickitem).attr("data-state").trim();
        $("#lBillingState").val(selectedState);
        //// end  customerAdditionalinfo
        var selectedCounty = $(clickitem).attr("data-county").trim();
        if (selectedCounty == "null") {
            selectedCounty = "";
        }
        $("#County").val(selectedCounty);

      
    });




    $('.CityState_insert_div_previous .tt-suggestion').click(function () {
        var clickitem = this;
        $('.CityState_insert_div_previous .tt-menu').hide();
        var selectedZip = $(clickitem).attr("data-zip").trim();
        $("#ZipCodePrevious").val(selectedZip);
        var selectedCity = $(clickitem).attr("data-city").trim();
        $("#CityPrevious").val(FirstLetterUppercase(selectedCity));
        var selectedState = $(clickitem).attr("data-state").trim();
        $("#StatePrevious").val(selectedState);


        ////  customerAdditionalinfo
        $("#CorpZipCode").val(selectedZip);
        var selectedCity = $(clickitem).attr("data-city").trim();
        $("#lCorpCity").val(selectedCity);
        var selectedState = $(clickitem).attr("data-state").trim();
        $("#lCorpState").val(selectedState);
        //// end  customerAdditionalinfo


    });
    $('.CoCityState_insert_div .tt-suggestion').click(function () {
        var clickitem = this;
        $('.CoCityState_insert_div .tt-menu').hide();
        var selectedZip = $(clickitem).attr("data-zip").trim();
        $("#CoZipCode").val(selectedZip);
        var selectedCity = $(clickitem).attr("data-city").trim();
        $("#CoCity").val(selectedCity);
        var selectedState = $(clickitem).attr("data-state").trim();
        $("#CoState").val(selectedState);


        ////  customerAdditionalinfo
        $("#CoZipCode").val(selectedZip);
        var selectedCity = $(clickitem).attr("data-city").trim();
        $("#lCorpCity").val(selectedCity);
        var selectedState = $(clickitem).attr("data-state").trim();
        $("#lCorpState").val(selectedState);
        //// end  customerAdditionalinfo
        var selectedCounty = $(clickitem).attr("data-county").trim();
        if (selectedCounty == "null") {
            selectedCounty = "";
        }
        $("#CoCounty").val(selectedCounty);

    });
    $('.CityState_insert_div .CoCityState_insert_div .CityState_insert_div_previous .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}
var CityStateclickbindwithrestrictedzipcode = function (item) {
    $('.CityState_insert_div .tt-suggestion').click(function () {
        var clickitem = this;
        $('.CityState_insert_div .tt-menu').hide();
        selectedZip = $(clickitem).attr("data-zip").trim();
        $("#ZipCode").val(selectedZip);
        var selectedCity = $(clickitem).attr("data-city").trim();
        $("#City").val(selectedCity);
        $("#City").attr("style", "border-color:#ccc;");
        var selectedState = $(clickitem).attr("data-state").trim();
        $("#State").val(selectedState);
        $("#State").attr("style", "border-color:#ccc;");
        ////  customerAdditionalinfo
        $("#BillingZipCode").val(selectedZip);
        var selectedCity = $(clickitem).attr("data-city").trim();
        $("#lBillingCity").val(selectedCity);
        var selectedState = $(clickitem).attr("data-state").trim();
        $("#lBillingState").val(selectedState);
        //// end  customerAdditionalinfo
        var selectedCounty = $(clickitem).attr("data-county").trim();
        if (selectedCounty == "null") {
            selectedCounty = "";
        }
        $("#County").val(selectedCounty);

        //region restrictedzipcode
      
        var url = domainurl + "/CityTax/CheckZipCode";
        var param = JSON.stringify({

            Zipcode: selectedZip,


        });
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data == true) {
                    OpenErrorMessageNew("Error!", Invalidzipmsg);
                    $("#ZipCode").val("");
                    $("#City").val("");
                    $("#State").val("");
                    $("#County").val("");
                }
                else {
                    $(".close").click();
                  

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
///
    });
   

    
  
    $('.CityState_insert_div_previous .tt-suggestion').click(function () {
        var clickitem = this;
        $('.CityState_insert_div_previous .tt-menu').hide();
        var selectedZip = $(clickitem).attr("data-zip").trim();
        $("#ZipCodePrevious").val(selectedZip);
        var selectedCity = $(clickitem).attr("data-city").trim();
        $("#CityPrevious").val(selectedCity);
        var selectedState = $(clickitem).attr("data-state").trim();
        $("#StatePrevious").val(selectedState);


        ////  customerAdditionalinfo
        $("#CorpZipCode").val(selectedZip);
        var selectedCity = $(clickitem).attr("data-city").trim();
        $("#lCorpCity").val(selectedCity);
        var selectedState = $(clickitem).attr("data-state").trim();
        $("#lCorpState").val(selectedState);
        //// end  customerAdditionalinfo

        //region restrictedzipcode

        var url = domainurl + "/CityTax/CheckZipCode";
        var param = JSON.stringify({

            Zipcode: selectedZip,


        });
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data == true) {
                    OpenErrorMessageNew("Error!", Invalidzipmsg);
                    $("#ZipCodePrevious").val("");
                    $("#CityPrevious").val("");
                    $("#StatePrevious").val("");
                  
                  
                }
                else {
                    $(".close").click();


                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
///


    });
    $('.CoCityState_insert_div .tt-suggestion').click(function () {
        var clickitem = this;
        $('.CoCityState_insert_div .tt-menu').hide();
        var selectedZip = $(clickitem).attr("data-zip").trim();
        $("#CoZipCode").val(selectedZip);
        var selectedCity = $(clickitem).attr("data-city").trim();
        $("#CoCity").val(selectedCity);
        var selectedState = $(clickitem).attr("data-state").trim();
        $("#CoState").val(selectedState);


        ////  customerAdditionalinfo
        $("#CoZipCode").val(selectedZip);
        var selectedCity = $(clickitem).attr("data-city").trim();
        $("#lCorpCity").val(selectedCity);
        var selectedState = $(clickitem).attr("data-state").trim();
        $("#lCorpState").val(selectedState);
        //// end  customerAdditionalinfo
        var selectedCounty = $(clickitem).attr("data-county").trim();
        if (selectedCounty == "null") {
            selectedCounty = "";
        }
        $("#CoCounty").val(selectedCounty);

    });
    $('.CityState_insert_div .CoCityState_insert_div .CityState_insert_div_previous .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}

$(document).ready(function () {
    $("#State").attr("MaxLength", "2");
    $("#StatePrevious").attr("MaxLength", "2");

    $("#ZipCode").attr("MaxLength", "5");
    $("#CoZipCode").attr("MaxLength", "5");
    $("#ZipCodePrevious").attr("MaxLength", "5");
    //$("#ZipCode").attr("type", "number");
    //$("#ZipCodePrevious").attr("type", "number");

    $("#State").focusout(function () {
        $("#State").val($("#State").val().toUpperCase());
    });
    $("#StatePrevious").focusout(function () {
        $("#StatePrevious").val($("#StatePrevious").val().toUpperCase());
    });

  

    $('#State').keydown(function (e) {
        var key = e.keyCode;
        if (!((key == 8) || (key == 9) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {
            e.preventDefault();
        }
    });
    $('#StatePrevious').keydown(function (e) {
        var key = e.keyCode;
        if (!((key == 8) || (key == 9) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {
            e.preventDefault();
        }
    });

    $("#ZipCode").keydown(function (e) {
        checkNumber(e);
    })
    $("#CoZipCode").keydown(function (e) {
        checkNumber(e);
    })
    $("#ZipCodePrevious").keydown(function (e) {
        checkNumber(e);
    })


    ////  customerAdditionalinfo
    $("#lBillingState").attr("MaxLength", "2");
    $("#lCorpState").attr("MaxLength", "2");

    $("#BillingZipCode").attr("MaxLength", "5");
    $("#CorpZipCode").attr("MaxLength", "5");

    $("#lBillingState").focusout(function () {
        $("#lBillingState").val($("#lBillingState").val().toUpperCase());
    });
    $("#lCorpState").focusout(function () {
        $("#lCorpState").val($("#lCorpState").val().toUpperCase());
    });

    $('#lBillingState').keydown(function (e) {
        var key = e.keyCode;
        if (!((key == 8) || (key == 9) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {
            e.preventDefault();
        }
    });
    $('#lCorpState').keydown(function (e) {
        var key = e.keyCode;
        if (!((key == 8) || (key == 9) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {
            e.preventDefault();
        }
    });

    $("#BillingZipCode").keydown(function (e) {
        checkNumber(e);
    })
    $("#CorpZipCode").keydown(function (e) {
        checkNumber(e);
    })

    $("#BillingZipCode").focusout(function (e) {
        setTimeout(function () {
            $(".CityState_insert_div .tt-menu").hide();
        }, 200);
    });
    $("#CorpZipCode").focusout(function () {
        setTimeout(function () {
            $(".CityState_insert_div .CityState_insert_div_previous .tt-menu").hide();
        }, 200);
    });


    //// end customerAdditionalinfo


    $("#ZipCode").focusout(function (e) {
        setTimeout(function () {
            $(".CityState_insert_div .tt-menu").hide();
        }, 200);
    });
    $("#ZipCodePrevious").focusout(function () {
        setTimeout(function () {
            $(".CityState_insert_div .CityState_insert_div_previous .CoCityState_insert_div .tt-menu").hide();
        }, 200);
    });
    $("#CoZipCode").focusout(function () {
        setTimeout(function () {
            $(".CityState_insert_div .CityState_insert_div_previous .CoCityState_insert_div .tt-menu").hide();
        }, 200);
    });
})