var empid = $("#AssignName").val();
var CustomerId = '@ViewBag.CustomerId';

$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $("#NoteType").change(function () {
        console.log("changedropdown");
        if ($("#NoteType").val() == "-1" || $("#NoteType").val() == "") {
            $(".NoteTypeError").removeClass("hidden");
        }
        else {
            $(".NoteTypeError").addClass("hidden");

        }
    })
    $("#AssignName").change(function () {
        empid = $(this).val();
    })

    $(".btn-leadnote").click(function () {
        console.log("enter");
     if ($("#NoteType").val() == "-1" || $("#NoteType").val() == "") {
            $(".NoteTypeError").removeClass("hidden");
        }
        else {
            $(".NoteTypeError").addClass("hidden");
            if (CommonUiValidation()) {
                if ($("#Leadnote").val() != "") {
                    if ($("#IsEmailReminder").is(':checked') != false || $("#IsTextReminder").is(':checked') != false || $("#AssignName").val() == null) {
                        var url = domainurl + "/Leads/AddLeadNotes/";
                        console.log("test");
                        var param = JSON.stringify({
                            id: $("#Id").val(),
                            notes: $("#Leadnote").val(),
                            noteType: $('#NoteType option:selected').attr('originalvalue'),
                            customerId: CustomerGuid,
                            
                            IsPin: $("#IsPinned").is(':checked'),
                            IsEmail: $("#IsEmailReminder").is(':checked'),
                            IsText: $("#IsTextReminder").is(':checked'),
                            EmpIdList: $("#AssignName").val(),
                            IsOverview: $("#isoverview").prop('checked')
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
                                $('.close').trigger('click');

                                if (From == "" || From == "undefined") {
                                    OpenSuccessMessageNew("Success!", "Successfully Added Lead Note", function () {
                                        console.log("A");
                                        openLeadNoteTab();
                                        LeadDetailTabCount();
                                    })
                                }
                                else {
                                    OpenSuccessMessageNew("Success!", "Successfully Added Lead Note", function () {
                                        console.log("C");
                                        $(".lead_detail_tab_list li").removeClass("active");
                                        $(".followUpTab").addClass("active");
                                        $(".lead-tabcontent .tab-pane").removeClass("active");
                                        $("#followUpTab").addClass("active");
                                        openLeadNoteTab();
                                        LeadDetailTabCount();
                                    })

                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                $(".loader-div").hide();
                                console.log(errorThrown);
                            }
                        });
                    }
                    else {
                        OpenErrorMessageNew("Error!", "Please select any follow up method");
                    }
                }
                else {
                    OpenErrorMessageNew("Error!", "Please fill up Notes with any Note Type");
                }
            }
        }
        
    })

    $(".btn-leadnoteLeadform").click(function () {
        console.log("enter");
       
        if ($("#NoteType").val() == "-1" || $("#NoteType").val() == "") {
            $(".NoteTypeError").removeClass("hidden");
        }
        else {
            $(".NoteTypeError").addClass("hidden");
            
                if ($("#Leadnote").val() != "") {
                    if ($("#IsEmailReminder").is(':checked') != false || $("#IsTextReminder").is(':checked') != false || $("#AssignName").val() == null) {
                        var url = domainurl + "/Leads/AddLeadNotes/";
                        console.log("test");
                        var param = JSON.stringify({
                            id: $("#Id").val(),
                            notes: $("#Leadnote").val(),
                            noteType: $('#NoteType option:selected').attr('originalvalue'),
                            customerId: CustomerId,
                            IsPin: $("#IsPinned").is(':checked'),
                            IsEmail: $("#IsEmailReminder").is(':checked'),
                            IsText: $("#IsTextReminder").is(':checked'),
                            EmpIdList: $("#AssignName").val(),
                            IsOverview: $("#isoverview").prop('checked')
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
                                $('.close').trigger('click');

                                if (From == "" || From == "undefined") {
                                    OpenSuccessMessageNew("Success!", "Successfully Added Lead Note", function () {
                                        //openLeadNoteTab();
                                        //LeadDetailTabCount();
                                    })
                                }
                                else {
                                    OpenSuccessMessageNew("Success!", "Successfully Added Lead Note", function () {
                                        //$(".Notes-Box").load("/Leads/LeadNoteBoxes?CustomerId=" + CustomerId);
                                        //LeadDetailTabCount();
                                    })

                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                $(".loader-div").hide();
                                console.log(errorThrown);
                            }
                        });
                    }
                    else {
                        OpenErrorMessageNew("Error!", "Please select any follow up method");
                    }
                }
                else {
                    OpenErrorMessageNew("Error!", "Please fill up Notes with any Note Type");
                }
            
        }

    })
})


