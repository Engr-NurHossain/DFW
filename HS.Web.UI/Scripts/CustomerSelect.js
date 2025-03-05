var InitializeSuburbDropdown = function (dropdownitem) {
    $(dropdownitem).select2({
        placeholder: 'Customer',
        allowClear: true,
        minimumInputLength: 1,
        ajax: {
            url: domainurl + "/Ticket/GetCustomerList",
            dataType: 'json',
            type: "GET",
            quietMillis: 50,
            data: function (term) {
                return {
                    q: term
                };
            },
            processResults: function (data, params) {
                return {
                    results: $.map(data, function (item) {
                        if (typeof (item.Street) != "undefined" && item.Street != null && item.Street != "") {
                            if (typeof (item.BusinessName) != "undefined" && item.BusinessName != null && item.BusinessName != "") {
                                return {
                                    text: item.CustomerName + " (" + item.BusinessName + ")" + " [" + item.Street + "]",
                                    id: item.CustomerId
                                }
                            }
                            else {
                                return {
                                    text: item.CustomerName + " [" + item.Street + "]",
                                    id: item.CustomerId
                                }
                            }
                        }
                        else {
                            if (typeof (item.BusinessName) != "undefined" && item.BusinessName != null && item.BusinessName != "") {
                                return {
                                    text: item.CustomerName + " (" + item.BusinessName + ")",
                                    id: item.CustomerId
                                }
                            }
                            else {
                                return {
                                    text: item.CustomerName,
                                    id: item.CustomerId
                                }
                            }
                        }
                    })
                };
            }
        }
    });
    $(dropdownitem).on("select2:closing", function (e) {

    });


}

var InitializeContactDropdown = function (dropdownitem) {
    $(dropdownitem).select2({
        placeholder: 'Customer',
        allowClear: true,
        minimumInputLength: 1,
        ajax: {
            url: domainurl + "/Ticket/GetContactList",
            dataType: 'json',
            type: "GET",
            quietMillis: 50,
            data: function (term) {
                return {
                    q: term
                };
            },
            processResults: function (data, params) {
                return {
                    results: $.map(data, function (item) {
                        if (typeof (item.FirstName) != "undefined" && item.FirstName != null && item.FirstName != "")
                        {
                            return {
                                text: item.FirstName + " " + item.LastName ,
                                id: item.ContactId
                            }
                        }

                       
                        
                    })
                };
            }
        }
    });
    $(dropdownitem).on("select2:closing", function (e) {

    });


}
var InitializeSuburbDropdownChild = function (dropdownitem) {
    $(dropdownitem).select2({
        placeholder: 'Customer',
        allowClear: true,
        minimumInputLength: 1,
        ajax: {
            url: domainurl + "/Ticket/GetCustomerList",
            dataType: 'json',
            type: "GET",
            quietMillis: 50,
            data: function (term) {
                return {
                    q: term
                };
            },
            processResults: function (data, params) {
                return {
                    results: $.map(data, function (item) {
                        if (typeof (item.Street) != "undefined" && item.Street != null && item.Street != "") {
                            if (typeof (item.BusinessName) != "undefined" && item.BusinessName != null && item.BusinessName != "") {
                                return {
                                    text: item.CustomerName + " (" + item.BusinessName + ")" + " [" + item.Street + "]",
                                    id: item.CustomerId
                                }
                            }
                            else {
                                return {
                                    text: item.CustomerName + " [" + item.Street + "]",
                                    id: item.CustomerId
                                }
                            }
                        }
                        else {
                            if (typeof (item.BusinessName) != "undefined" && item.BusinessName != null && item.BusinessName != "") {
                                return {
                                    text: item.CustomerName + " (" + item.BusinessName + ")",
                                    id: item.CustomerId
                                }
                            }
                            else {
                                return {
                                    text: item.CustomerName,
                                    id: item.CustomerId
                                }
                            }
                        }
                    })
                };
            }
        }
    });
    $(dropdownitem).on("select2:closing", function (e) {

    });
}