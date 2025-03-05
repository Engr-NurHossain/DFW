var DataTablePageSize = 50;
var assEmp;
var assid;
var deleteNote = $('.item-delete').attr('id');


var NewNotesLoad = function (from) {

    //$("#NewNotes").load("/Notes/AddNotes");
    OpenRightToLeftModal(domainurl + "/Notes/AddNotes/?id=0&customerid=" + CustomerLoadId+"&from="+from);
    //$("#NewNotes").load();
    history.pushState({ urlpath: window.location.pathname }, window.location.hash, "#addNote");
}
var DeleteCustomerNote = function () {
    console.log("deletenote");
    var delitem = selectedDeleteId;
    $.ajax({
        url: domainurl + "/Notes/DeleteCustomerNote",
        data: { id: delitem, CustomerId: $('#AddReminderLeadId').val()},
        type: "Post",
        dataType: "Json",
        success: function (result) {

            if (result) {
                //var customerId = deleteNote
                //$("#NotesTab").load("/Notes/NotesPartial/?customerid=" + CustomerLoadId);
                OpenNotesTab();
            }
        },

        error: function () {
        }

    });
    console.log("from customer delete" + CustomerLoadId);
}
var AssignEmployeeSendEmail = function () {
    var url = domainurl + "/Notes/AssignEmployeeSendEmail";
    
    $.ajax({
        url: url,
        data: { employee: assEmp, id:assid },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data == true) {
                OpenSuccessMessageNew("Success!", "Assign employee customer note sent email successfully.", "");
            }
        },
        error: function () {
        }
    });
}
var AssignEmployeeSendText = function () {
    var url = domainurl + "/Notes/AssignEmployeeSendText";
    var valid = $("#btn-phn").attr('data-id');
    $.ajax({
        url: url,
        data: { id: valid },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result == true) {
                OpenConfirmationMessageNew("Confirm?", "Do you want to make a phone call to assign employee", function () {
                    $(".phnClick").attr('href', data.phnString);
                    $(".phnClick").click();
                });
            }
        },
        error: function () {
        }
    });
}
var AssignEmployeeFollowSendEmail = function () {
    var url = domainurl + "/Notes/AssignEmployeeFollowSendEmail";
    $.ajax({
        url: url,
        data: { employee: assEmp, id: assid },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data == true) {
                OpenSuccessMessageNew("Success!", "Assign employee customer follow up sent email successfully.", "");
            }
        },
        error: function () {
        }
    });
}
var Notetable = $('#tblNotes').DataTable({
    "pageLength": DataTablePageSize,
    "destroy": true,
    /* Disable initial sort */
    "aaSorting": [],
    "language": {
        "emptyTable": "No data available"
    },
});
$(document).ready(function () {
    $("#tblNotes_paginate").hide();
    $(".lessContent").hide();
    $(".nt_text_st").attr("style", "max-height: 100px !important;");
    setTimeout(function () {
        $(".nt_text_st").each(function () {
            console.log($($(this).find("div")[0]).height())
            var idval = $(this).attr('data-id');
            if (parseInt($($(this).find("div")[0]).height()) > 100) {
                $(".moreContent_" + idval).show();
            }
            else {
                $(".moreContent_" + idval).hide();
            }
        })
    }, 3000);
   

    StartDatepicker = new Pikaday({
        field: $('#StartDate')[0],
        format: 'MM/DD/YYYY'
    });

    EndDatepicker = new Pikaday({
        field: $('#EndDate')[0],
        format: 'MM/DD/YYYY'
    });

    $(".applySearch").click(function () {
        var StartDate = $("#StartDate").val();
        var EndDate = $("#EndDate").val();
        var SearchText = encodeURI($("#Searchtxt").val());
        $(".NotesTab_Load").load(domainurl + "/Notes/NotesPartial?customerid=" + CustomerLoadGuid + "&pagesize=50" + "&StartDate=" + StartDate + "&EndDate=" + EndDate + "&SearchText=" + SearchText);
    })

    $(".moreContent").click(function () {
        var IdVal = $(this).attr('id-val');
        $(".moreContent_" + IdVal).hide();
        $(".lessContent_" + IdVal).show();
        $(".nt_text_st_" + IdVal).attr("style", "max-height: unset !important;");
    })
    $(".lessContent").click(function () {
        var IdVal = $(this).attr('id-val');
        $(".moreContent_" + IdVal).show();
        $(".lessContent_" + IdVal).hide();
        $(".nt_text_st_" + IdVal).attr("style", "max-height: 100px !important;");
    })

 
    /*Notetable.order([3, "desc"]).draw();*/
    $(".item-delete").click(function () {
        selectedDeleteId = $(this).attr("data-id");
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeleteCustomerNote);
    })
    $(".item-edit").click(function () { 
        var idvalue = $(this).attr('data-id');
        OpenRightToLeftModal(domainurl + "/Notes/AddNotes/?id=" + idvalue + "&customerid=" + CustomerLoadId);
        //$("#NewNotes").load("/Notes/AddNotes/?id=" + idvalue + "&customerId=" + customerId);
    });
    $(".note-assign").load(domainurl + "/NoteAssign/NoteAssignPartial/");
    $(".btn-envelope").click(function () {
        assEmp = $(".btn-envelope").attr('data-id').toString().split(',');
        assid = $(".btn-envelope").attr('idval');
        AssignEmployeeSendEmail();
    })
    $(".btn-follow-envelope").click(function () {
        assEmp = $(this).attr('data-id').toString().split(',');
        assid = $(this).attr('idval');
        AssignEmployeeFollowSendEmail();
    })
    //$("#btn-phn").click(function () {
    //    AssignEmployeeSendText();
    //})
})