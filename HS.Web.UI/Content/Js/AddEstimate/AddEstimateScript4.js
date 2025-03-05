
    $(document).ready(function () {

        var idlist = [{ id: ".InvEstPreview", type: 'iframe', width: 920, height: 600 }
        ];
        jQuery.each(idlist, function (i, val) {
            magnificPopupObj(val);
        });
    });
