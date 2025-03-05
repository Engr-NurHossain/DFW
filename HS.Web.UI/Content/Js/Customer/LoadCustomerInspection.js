
$(document).ready(function () {
   
    if (window.innerWidth < 415) {
        $(".inspection_container_inner").height(window.innerHeight - ($(".inpection_header").height() + $(".inpection_footer").height() + 91));
    }
    else {
        $(".inspection_container_inner").height(window.innerHeight - ($(".inpection_header").height() + $(".inpection_footer").height() + 41));
    }

    //if (typeof (CurrentOutsideConditions) != "undefined" && CurrentOutsideConditions != null && CurrentOutsideConditions != "" && CurrentOutsideConditions != "-1") {
    //    $("#CurrentOutsideConditions").val(CurrentOutsideConditions);
    //}
    //if (typeof (Heat) != "undefined" && Heat != null && Heat != "" && Heat != "-1") {
    //    $("#Heat").val(Heat);
    //}
    //if (typeof (Air) != "undefined" && Air != null && Air != "" && Air != "-1") {
    //    $("#Air").val(Air);
    //}
    //if (typeof (BasementDehumidifier) != "undefined" && BasementDehumidifier != null && BasementDehumidifier != "" && BasementDehumidifier != "-1") {
    //    $("#BasementDehumidifier").val(BasementDehumidifier);
    //}
    //if (typeof (FoundationType) != "undefined" && FoundationType != null && FoundationType != "" && FoundationType != "-1") {
    //    $("#FoundationType").val(FoundationType);
    //}
    //if (typeof (BasementGoDown) != "undefined" && BasementGoDown != null && BasementGoDown != "" && BasementGoDown != "-1") {
    //    console.log(InstalledStatusval);
    //    $("#GoDownBasement").val(BasementGoDown);
    //}
    //if (typeof (RemoveWater) != "undefined" && RemoveWater != null && RemoveWater != "" && RemoveWater != "-1") {
    //    $("#RemoveWater").val(RemoveWater);
    //}
    //if (typeof (PlansForBasementOnce) != "undefined" && PlansForBasementOnce != null && PlansForBasementOnce != "" && PlansForBasementOnce != "-1") {
    //    $("#PlansForBasement").val(PlansForBasementOnce);
    //}
    //if (typeof (LosePower) != "undefined" && LosePower != null && LosePower != "" && LosePower != "-1") {
    //    $("#LosePower").val(LosePower);
    //}

    new Pikaday({ format: 'MM/DD/YYYY', field: $('#PMSignatureDate')[0] });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#HomeOwnerSignatureDate')[0] });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#CreatedDate')[0] });

    var canvaspm = $('#signature-pad-pm');
    var ctxpm = $('#signature-pad-pm')[0].getContext('2d');
    var signaturePadpm = new SignaturePad($('#signature-pad-pm')[0]);

    var canvashw = $('#signature-pad-homeowner');
    var ctxhw = $('#signature-pad-homeowner')[0].getContext('2d');
    var signaturePadhw = new SignaturePad($('#signature-pad-homeowner')[0]);

    var canvasdr = $('#drawing-pad');
    var ctxdr = $('#drawing-pad')[0].getContext('2d');
    var drawingPaddr = new SignaturePad($('#drawing-pad')[0]);

    function resizeCanvaspm() {
        var ratio = Math.max(1, 1);
        canvaspm.width = canvaspm.offsetWidth * ratio;
        canvaspm.height = canvaspm.offsetHeight * ratio;
        canvaspm[0].getContext("2d").scale(ratio, ratio);
    }
    resizeCanvaspm();

    function resizeCanvashw() {
        var ratio = Math.max(1, 1);
        canvashw.width = canvashw.offsetWidth * ratio;
        canvashw.height = canvashw.offsetHeight * ratio;
        canvashw[0].getContext("2d").scale(ratio, ratio);
    }
    resizeCanvashw();

    function resizeCanvasdr() {
        var ratio = Math.max(1, 1);
        canvasdr.width = canvasdr.offsetWidth * ratio;
        canvasdr.height = canvasdr.offsetHeight * ratio;
        canvasdr[0].getContext("2d").scale(ratio, ratio);
    }
    resizeCanvasdr();

    var imgpm = new Image();
    imgpm.onload = function () {
        ctxpm.drawImage(imgpm, 0, 0);
    };

    var imghw = new Image();
    imghw.onload = function () {
        ctxhw.drawImage(imghw, 0, 0);
    };

    var imgdr = new Image();
    imgdr.onload = function () {
        ctxdr.drawImage(imghw, 0, 0);
    };

    imgpm.crossOrigin = 'anonymous';
    imghw.crossOrigin = 'anonymous';
    imgdr.crossOrigin = 'anonymous';

    $('#clear-pm').click(function () {
        signaturePadpm.clear();
    });
    $('#clear-homeowner').click(function () {
        signaturePadhw.clear();
    });
    $('#clear-dr').click(function () {
        drawingPaddr.clear();
    });
    $("#SaveInspection").click(function () {
        if (CommonUiValidation()){
        var datapm = signaturePadpm.toDataURL('image/png');
        var datahw = signaturePadhw.toDataURL('image/png');
        var datadr = drawingPaddr.toDataURL('image/png');
        var url = domainurl + "/Customer/AddCustomerInspection";
        var param = {
            Id: $("#Id").val(),
            CusId: $("#CusId").val(),
            CustomerId: $("#CustomerId").val(),
            CompanyId: $("#CompanyId").val(),
            CreatedBy: $("#CreatedBy").val(),
            CreatedDate: $("#CreatedDate").val(),
            CurrentOutsideConditions: $("#CurrentOutsideConditions").val(),
            OutsideRelativeHumidity: $("#OutsideRelativeHumidity").val(),
            OutsideTemperature: $("#OutsideTemperature").val(),
            FirstFloorRelativeHumidity: $("#FirstFloorRelativeHumidity").val(),
            FirstFloorTemperature: $("#FirstFloorTemperature").val(),
            RelativeOther1: $("#RelativeOther1").val(),
            RelativeOther2: $("#RelativeOther2").val(),
            Heat: $("#Heat").val(),
            Air: $("#Air").val(),
            BasementRelativeHumidity: $("#BasementRelativeHumidity").val(),
            BasementTemperature: $("#BasementTemperature").val(),
            BasementDehumidifier: $("#BasementDehumidifier").val(),
            GroundWater: $("#GroundWater").val(),
            GroundWaterRating: parseInt($("#GroundWaterRating").val()),
            IronBacteria: $("#IronBacteria").val(),
            IronBacteriaRating: parseInt($("#IronBacteriaRating").val()),
            Condensation: $("#Condensation").val(),
            CondensationRating: parseInt($("#CondensationRating").val()),
            WallCracks: $("#WallCracks").val(),
            WallCracksRating: parseInt($("#WallCracksRating").val()),
            FloorCracks: $("#FloorCracks").val(),
            FloorCracksRating: parseInt($("#FloorCracksRating").val()),
            ExistingSumpPump: $("#ExistingSumpPump").val(),
            ExistingDrainageSystem: $("#ExistingDrainageSystem").val(),
            ExistingRadonSystem: $("#ExistingRadonSystem").val(),
            DryerVentToCode: $("#DryerVentCode").val(),
            FoundationType: $("#FoundationType").val(),
            Bulkhead: $("#Bulkhead").val(),
            VisualBasementOther: $("#VisualBasementOther").val(),
            NoticedSmellsOrOdors: $("#NoticedSmellsOrOdors").val(),
            NoticedSmellsOrOdorsComment: $("#NoticedSmellsOrOdorsComment").val(),
            NoticedMoldOrMildew: $("#NoticedMoldOrMildew").val(),
            NoticedMoldOrMildewComment: $("#NoticedMoldOrMildewComment").val(),
            BasementGoDown: $("#GoDownBasement").val(),
            HomeSufferForRespiratory: $("#HomeSufferForRespiratoryProblems").val(),
            HomeSufferForrespiratoryComment: $("#HomeSufferForRespiratoryProblemsComment").val(),
            ChildrenPlayInBasement: $("#ChildrenPlay").val(),
            ChildrenPlayInBasementComment: $("#ChildrenPlayComment").val(),
            PetsGoInBasement: $("#PetsGoInBasement").val(),
            PetsGoInBasementComment: $("#PetsGoInBasementComment").val(),
            NoticedBugsOrRodents: $("#NoticedBugsOrRodents").val(),
            NoticedBugsOrRodentsComment: $("#NoticedBugsOrRodentsComment").val(),
            GetWater: $("#GetWater").val(),
            GetWaterComment: $("#GetWaterComment").val(),
            RemoveWater: $("#RemoveWater").val(),
            SeeCondensationPipesDripping: $("#SeePipesDripping").val(),
            SeeCondensationPipesDrippingComment: $("#SeePipesDrippingComment").val(),
            RepairsProblems: $("#RepairsTryAndFix").val(),
            RepairsProblemsComment: $("#RepairsTryAndFixComment").val(),
            LivingPlan: $("#LivingPlan").val(),
            SellPlaning: $("#PlaningSell").val(),
            PlansForBasementOnce: $("#PlansForBasement").val(),
            HomeTestForPastRadon: $("#HomeTestedForRadon").val(),
            HomeTestForPastRadonComment: $("#HomeTestedForRadonComment").val(),
            LosePower: $("#LosePower").val(),
            LosePowerHowOften: $("#LosePowerHowOften").val(),
            CustomerBasementOther: $("#CustomerBasementOther").val(),
            Drawing: datadr,
            DrawingImagePath: $("#DrawingImagePath").val(),
            Notes: $("#Notes").val(),
            PMSignature: datapm,
            PMSignatureImagePath:$("#PMSignatureImagePath").val(),
            PMSignatureDate: $("#PMSignatureDate").val(),
            HomeOwnerSignature: datahw,
            HomeOwnerSignatureImagePath:$("#HomeOwnerSignatureImagePath").val(),
            HomeOwnerSignatureDate: $("#HomeOwnerSignatureDate").val()
        };

        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                $('.close').trigger('click');
                if (data.result == false) {
                    parent.OpenErrorMessageNew("Error!", data.message);
                }
                else if (data.result == true) {
                    var Customerid = data.customerid;
                    gid = $("#id").val(Customerid);
                    parent.OpenSuccessMessageNew("Success!", "Customer Inspection saved successfully.", function () {
                        CloseTopToBottomModal();
                    });
                    parent.LoadCustomerDetail(Customerid, true);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })
    }
    else{
            OpenErrorMessageNew("Error!", "Please fill up required(*) field");
        }
    });
});
$(window).resize(function () {
    if (window.innerWidth < 415) {
        $(".inspection_container_inner").height(window.innerHeight - ($(".inpection_header").height() + $(".inpection_footer").height() + 91));
    }
    else {
        $(".inspection_container_inner").height(window.innerHeight - ($(".inpection_header").height() + $(".inpection_footer").height() + 41));
    }
})