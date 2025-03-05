//var deleteincome = $('.item-delete').attr('id');
var DataTablePageSize = 50;
var table = $('#tblFunding').DataTable({
    "pageLength": DataTablePageSize,
    "destroy": true,
    "language": {
        "emptyTable": "No data available"
    }
});
var DeletIncome = function () {
    var delitem = selectedDeleteId;
    $.ajax({
        url: domainurl + "/Funding/DeleteAddIncome",
        data: { id: delitem },
        type: "Post",
        dataType: "Json",
        success: function (result) {
            if (result) {
                $("#FundingTab").load(domainurl + "/Funding/FundingPartial?customerid=" + CustomerGuid);
            } else {

            }
        },
        error: function () {
        }

    });
    //console.log("from customer delete" + customerId);
}

$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$("#Right-To-Left-Modal-Body .modal-body").html('');
    })
    $('#tblFunding tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    $("#addincome").click(function () {
        OpenRightToLeftModal(domainurl + "/Funding/AddIncome?id=0&customerid=" + customerId);
    });
    $("#addexpense").click(function () {
        OpenRightToLeftModal(domainurl + "/Funding/AddExpense?id=0&customerid=" + customerId);
    });
    $(".item-delete").click(function () {
        selectedDeleteId = $(this).attr("data-id");
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeletIncome);
    });

    $(".addexpense").click(function () {
        OpenPayrollTab();
    })
});