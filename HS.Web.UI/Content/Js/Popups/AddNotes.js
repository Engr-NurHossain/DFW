
var empid = $("#EmpId").val();
var SaveNotes = function () {
    if (CommonUiValidation()) {
        if ($("#IsEmailReminder").is(':checked') != false || $("#IsTextReminder").is(':checked') != false || $("#EmpId2").val() == null) {
        var url = domainurl + "/Notes/AddNotes";
        var param = {
            Id: $("#Id").val(),
   
            NoteType: $('#NoteType option:selected').attr('originalvalue'),
            //NoteType: $("#optionnotetype").attr('originalvalue'),
            Notes:  tinymce.get('Notes').getContent(),
            TeamSettingId: $("#TeamSettingId").val(),
            ReminderDate: datepicker.getDate(),
            CustomerId: $("#CustomerIdVal").val(),
            CompanyId: $("#CompanyId").val(),
            cusName: $("#cusName").val(),
            empName: $("#empName").val(),
            //[Shariful-18-9-19]
            //IsEmail: $(".checkitemail").is(':checked'),
            //IsText: $(".checkitemfortext").is(':checked'),
            //[~Shariful-18-9-19]
            //[Shariful2-18-9-19]
            IsEmail: $("#IsEmailReminder").is(':checked'),
            IsText: $("#IsTextReminder").is(':checked'),
            //[~Shariful2-18-9-19]
       
            IsPin: $("#IsPinned").is(":checked"),
            cusIdVal: $("#cusIdVal").val(),
            IsOverview: $("#isoverview").prop('checked')
        };
        var noteassign = {
            EmployeeIdVal: $("#EmpId").val(),
        }
        var passedparam = JSON.stringify({
            'cn': param, 'noteassign': noteassign
        });

        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: passedparam,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                $('.close').trigger('click');
                console.log("sdfd");
                if (From == "" || From == "undefined") {
                    console.log("A");
                    OpenSuccessMessageNew("Success!", "Successfully Added Note", function () {
                        UpdateCustomerTabCounter();
                        OpenNotesTab();
                    })
                }
                else {
                    console.log("B");
                    OpenSuccessMessageNew("Success!", "Successfully Added Note", function () {
                        //var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
                        //$(LoadCustomerDiv + ".Notes-Box").load(domainurl + "/Customer/CustomerNoteBoxes?customerId=" + CustomerLoadGuid);
                        UpdateCustomerTabCounter();
                        OpenNotesTab();
                    });
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })
    }
    else {
        OpenErrorMessageNew("Error!", "Please select any follow up method");
    }

    }
}
$(document).ready(function () {
    $("#EmpId2").change(function () {
        empid = $(this).val();
    })
    
   

    //var saveButton = $("#SaveNotes").addClass("hidden").prop('disabled', true);

    //$("#NoteType").change(function () {
    //    var noteTypeValue = $(this).val();
    //    saveButton.toggleClass("hidden", noteTypeValue == "-1" || noteTypeValue == "").prop('disabled', noteTypeValue == "-1" || noteTypeValue == "");
    //    $(".NoteTypeError").toggleClass("hidden", !(noteTypeValue == "-1" || noteTypeValue == ""));
    //});

    $("#NoteType").change(function () {
        if ($("#NoteType").val() == "-1" || $("#NoteType").val() == "") {
            $(".NoteTypeError").removeClass("hidden");
        }
        else {
            $(".NoteTypeError").addClass("hidden");
          
        }
    })

    $("#SaveNotes").click(function () {
        console.log("note click")

        if ($("#NoteType").val() == "-1" || $("#NoteType").val() == "") {
            $(".NoteTypeError").removeClass("hidden");
        }
        else {
            $(".NoteTypeError").addClass("hidden");
            $(this).attr('disabled', 'disabled');
            SaveNotes();
        }
       
    });
});