var InitializeSuburbDropdown = function (dropdownitem, packId, equipmentClassId) {
    $(dropdownitem).select2({
        placeholder: 'Equipment And Service',
        allowClear: true,
        minimumInputLength: 1,
        ajax: {
            url: domainurl + "/SmartPackageSetup/LoadEquipmentAndServiceSearch/?id=" + packId + "&equipmentClassId=" + equipmentClassId,
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

                        if (item.ManufacturerName != null) {
                            return {
                                text: item.ManufacturerName + " (" + item.Name + ")",
                                id: item.EquipmentId
                            }
                        }
                        else {
                            return {
                                text: " (" + item.Name + ")",
                                id: item.EquipmentId
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