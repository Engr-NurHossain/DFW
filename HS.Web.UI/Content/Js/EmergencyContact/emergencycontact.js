$(document).ready(function () {
    $("#emergencybtn").click(function () {
        var url = domainurl + "/Leads/EmergencyContactPartial/";
        var param = {
            CrossStreet: $("#txtCrossStreet").val(),
            FirstName: $("#txtFirstName").val(),
            LastName: $("#txtLastName").val(),
            RelationShip: $("#txtRelation").val(),
            PhoneNo: $("#txtPhoneNo").val(),
            Haskey: $("#txtHaskey").val()
        };
        $.ajax({
            type:"POST"
        })
    })
})