var InitializeSuburbDropdown = function (dropdownitem) {
    $(dropdownitem).select2({
        placeholder: 'Suburbs', allowClear: true,
        minimumInputLength: 1,
        ajax: {
            url: domainurl + "/Hr/GetCustomarList",
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

                        return {
                            text: item.CompleteName,
                            id: item.Value
                        }
                    })
                };
            }
        }
    });
    $(dropdownitem).on("select2:closing", function (e) {
        InitializeSuburbDropdown($('.dropdown-search-agent-suburb'));
    });
}
var InitializeSuburbWithStateDropdown = function (dropdownitem) {
    $(dropdownitem).select2({
        minimumInputLength: 1, placeholder: 'Customars', allowClear: true,
        ajax: {
            url: domainurl + "/Hr/GetCustomarList",
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

                        return {
                            text: item.CompleteName,
                            id: item.State + '#' + item.Value
                        }
                    })
                };
            }
        }
    });
    $(dropdownitem).on("select2:closing", function (e) {
        InitializeSuburbDropdown($('.dropdown_customar'));
    });
}