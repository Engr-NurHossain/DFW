var DefaultConfirmationfunc = null;
var DefaultConfirmationRejectfunc = null;
var DefaultSuccessfunc = null;
var DefaultErrorfunc = null;
var DefaultCautionfunc = null;
var DefaultCancelCustomerfunc = null;
var OpenConfirmationMessageNew = function (HeaderMessage, BodyMessage, ToDoFunc,RejectFunc) {
    $("#ModalConfirmationMessage .modal-title").html(HeaderMessage);
     
    $('#ModalConfirmationMessage p').html(BodyMessage);
    if (typeof (ToDoFunc) == "function") {
        DefaultConfirmationfunc = ToDoFunc;
        $("#eqp_checkbox").prop('checked', true);
        $("#ser_checkbox").prop('checked', true);
        $("#eqp_checkbox").change(function () {
            if ($("#eqp_checkbox").prop('checked')) {
                $("#eqp_checkbox").prop('checked', true);
                $("#ser_checkbox").prop('checked', true);
            }
            else {
                $("#eqp_checkbox").prop('checked', false);
                $("#ser_checkbox").prop('checked', false);
            }
        })
        $("#ser_checkbox").change(function () {
            if ($("#ser_checkbox").prop('checked')) {
                $("#eqp_checkbox").prop('checked', true);
                $("#ser_checkbox").prop('checked', true);
            }
            else {
                $("#eqp_checkbox").prop('checked', false);
                $("#ser_checkbox").prop('checked', false);
            }
        })
    } else {
        DefaultConfirmationfunc = function () { };
    }

    if (typeof (RejectFunc) == "function") {
        DefaultConfirmationRejectfunc = RejectFunc;
    } else {
        DefaultConfirmationRejectfunc = function () { };
    }
         
    $("#ConfirmationMessageModal").click();
}

var OpenSuccessMessageNew = function (HeaderMessage, BodyMessage, ToDoFunc) {

    $("#ModalSuccessMessage .message_header_title").html(HeaderMessage);
    $("#ModalSuccessMessage p").html(BodyMessage);
    if (typeof (ToDoFunc) == "function") {
        DefaultSuccessfunc = ToDoFunc;
        $(".close").unbind();
            
    } else {
        DefaultSuccessfunc = function () { };
    }
    $("#SuccessMessageModal").click();
}
var OpenTextModal = function (HeaderMessage, BodyMessage, ToDoFunc) {
    console.log("dsf");
    $("#ModalOpenText .message_header_title").text("Give Your Verbal Password!");
    $("#ModalOpenText p").text(BodyMessage);
    if (typeof (ToDoFunc) == "function") { 
        DefaultSuccessfunc = ToDoFunc;
        $(".close").unbind();

    } else {
        DefaultSuccessfunc = function () { };
    }
    $("#OpenTextModal").click();
}


var OpenCancelCustomer = function (HeaderMessage, BodyMessage, ToDoFunc) {
    $("#ModalCancelCustomer .cancel-title").text(HeaderMessage);
    $("#ModalCancelCustomer cancel-body p").text(BodyMessage);
    if (typeof (ToDoFunc) == "function") {
        DefaultCancelCustomerfunc = ToDoFunc;

    } else {
        DefaultCancelCustomerfunc = function () { };
    }
    $("#CancelCustomerModal").click();
}
var OpenErrorMessageNew= function (HeaderMessage, BodyMessage, ToDoFunc) {
    $("#ModalErrorMessage .message_header_title").text('Error!');
    $('#ModalErrorMessage p').html(BodyMessage);
    if (typeof (ToDoFunc) == "function") {
        DefaultErrorfunc = ToDoFunc;
    } else {
        DefaultErrorfunc = function () { };
    } 
    $("#ErrorMessageModal").click();
    /*setTimeout(function () {
        if ($("#ModalErrorMessage").is(":visible")) {
            if ($(".modal-backdrop").length > 1) {
                $(".modal-backdrop").each(function () {
                    $(this).css("z-index", "999999");
                    return 0;
                }); 
            } else if ($(".modal-backdrop").is(":visible")) {
                $(".modal-backdrop").css("z-index", "1040!important");
            }
            
        }
    },1000);*/
}
var OpenCautionMessageNew = function (HeaderMessage, BodyMessage, ToDoFunc) {
    $("#ModalCautionMessage .message_header_title").text('Caution!');
    $('#ModalCautionMessage p').html(BodyMessage);
    if (typeof (ToDoFunc) == "function") {
        
        DefaultCautionfunc = ToDoFunc;
    } else {
        DefaultCautionfunc = function () { };
    }
    $("#CautionMessageModal").click();
}
var OpenTopToBottomModal = function (url) {
    var windowHeight = $(window).height();
    $(".TopToBottomModal").css('top', -window.innerHeight);
    $(".TopToBottomModal").show(); 
    $(".TopToBottomModal").animate({
        top: 0
    }, 200);
    
    var InvoiceLoaderText = "<div class='invoice-loader' style='position: fixed;left: 0px;top: 0px;width: 100%;height: 100%;'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
    $(".TopToBottomModal .ContentsDiv").html(InvoiceLoaderText);
    setTimeout(function () {
        $(".top_to_bottom_modal_container").css("height", window.innerHeight);
    }, 300);
    setTimeout(function () {
        $(".TopToBottomModal .ContentsDiv").load(url);
    }, 700);
}
var CloseTopToBottomModal = function () {
    $(".TopToBottomModal").animate({
        top: -window.innerHeight
    }, 500);
    setTimeout(function () {
        $(".TopToBottomModal").hide();
        $(".TopToBottomModal .ContentsDiv").html("")
    }, 510);
}

var OpenFullScreenLoginModal = function (url) {
    var windowHeight = $(window).height();
    $("#FullScreenLoginModal").css('top', -window.innerHeight);
    $("#FullScreenLoginModal").show();
    $("#FullScreenLoginModal").animate({
        top: 0
    }, 200); 
    setTimeout(function () {
        $(".top_to_bottom_modal_container").css("height", window.innerHeight);
    }, 300); 
}
var CloseFullScreenLoginModal = function () {
    $("#FullScreenLoginModal").animate({
        top: -window.innerHeight
    }, 500);
    setTimeout(function () {
        $("#FullScreenLoginModal").hide();
    }, 510);
}
var CloseRightToLeftModal = function (url) {
    $("#Right-To-Left-Modal-Body .modal-body").html("");
    $("#RightToLeftModal").click();
}
var OpenRightToLeftModal = function (url) {
    $("#RightToLeftModal").click();
    var InvoiceLoaderText = "<div class='invoice-loader' style='position: fixed;left: 0px;top: 0px;width: 100%;height: 100%;'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
    $("#Right-To-Left-Modal-Body .modal-body").html(InvoiceLoaderText);
    if ( typeof(url)!="undefined" && url != "") {
        $("#Right-To-Left-Modal-Body .modal-body").load(url);
    }   
}
var OpenRightToLeftModalMMR = function (url) {
    $("#RightToLeftModalMMR").click();
    var InvoiceLoaderText = "<div class='invoice-loader' style='position: fixed;left: 0px;top: 0px;width: 100%;height: 100%;'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
    $("#Right-To-Left-Modal-Body-MMR .modal-body").html(InvoiceLoaderText);
    if (typeof (url) != "undefined" && url != "") {
        $("#Right-To-Left-Modal-Body-MMR .modal-body").load(url);
    }
}
var OpenRightToLeftLgModal = function (url) {
    $("#RightToLeftBigModal").click();
    var InvoiceLoaderText = "<div class='invoice-loader' style='position: fixed;left: 0px;top: 0px;width: 100%;height: 100%;'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
    $("#Right-To-Left-big-Modal-Body .modal-body").html(InvoiceLoaderText);
    if (typeof (url) != "undefined" && url != "") {
        $("#Right-To-Left-big-Modal-Body .modal-body").load(url);
    }
}
var OpenCreditAmountModal = function (HeaderMessage, BodyMessage, ToDoFunc) {
    $("#ModalOpenCreditAndAmmount span").html(HeaderMessage);
    $("#ModalOpenCreditAndAmmount p").html(BodyMessage);
    if (typeof (ToDoFunc) == "function") {
        DefaultSuccessfunc = ToDoFunc;
        $(".closenew").unbind();

    } else {
        DefaultSuccessfunc = function () { };
    }
    $("#OpenCreditAndAmmountModal").click();
}

