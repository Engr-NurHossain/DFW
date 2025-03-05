
var TinyMCEHeight = 430;
if (typeof (TinyMCEHeightSet) != "undefined") {
    TinyMCEHeight = TinyMCEHeightSet;
}
tinymce.init({
    /* replace textarea having class .tinymce with tinymce editor */
    selector: "textarea.tinymce",
    branding: false,



    /* theme of the editor */
    theme: "modern",
    skin: "lightgray",
    convert_urls: false,
    document_base_url: parent.SiteUrl,
    /* width and height of the editor */
    width: "100%",
    height: TinyMCEHeight,

    /* display statusbar */
    statubar: true,

    /* plugin */
    plugins: [
        "code",
        "lists hr anchor pagebreak",
		//"visualblocks visualchars code nonbreaking"
    ],

    //plugins: [
    // "advlist autolink link image lists charmap print preview hr anchor pagebreak",
    // "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
    // "save table contextmenu directionality emoticons template paste textcolor"
    //],


    /* toolbar */
    toolbar: "undo redo | styleselect | bold italic forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent",
    //toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview media fullpage | forecolor backcolor emoticons",

    /* style */
    style_formats: [
		{
		    title: "Headers", items: [
               { title: "Header 1", format: "h1" },
               { title: "Header 2", format: "h2" },
               { title: "Header 3", format: "h3" },
               { title: "Header 4", format: "h4" },
               { title: "Header 5", format: "h5" },
               { title: "Header 6", format: "h6" }
		    ]
		},
		{
		    title: "Inline", items: [
               { title: "Bold", icon: "bold", format: "bold" },
               { title: "Italic", icon: "italic", format: "italic" },
               { title: "Underline", icon: "underline", format: "underline" },
               { title: "Strikethrough", icon: "strikethrough", format: "strikethrough" },
               { title: "Superscript", icon: "superscript", format: "superscript" },
               { title: "Subscript", icon: "subscript", format: "subscript" },
               { title: "Code", icon: "code", format: "code" }
		    ]
		},
		{
		    title: "Blocks", items: [
               { title: "Paragraph", format: "p" },
               { title: "Blockquote", format: "blockquote" },
               { title: "Div", format: "div" },
               { title: "Pre", format: "pre" }
		    ]
		},
		{
		    title: "Alignment", items: [
               { title: "Left", icon: "alignleft", format: "alignleft" },
               { title: "Center", icon: "aligncenter", format: "aligncenter" },
               { title: "Right", icon: "alignright", format: "alignright" },
               { title: "Justify", icon: "alignjustify", format: "alignjustify" }
		    ]
		}
    ],
    setup: function (ed) {
        // This function works for checkboxes
        ed.on('init', function (e) {
            $(ed.getBody()).on("change", ":checkbox", function (el) {
                if (el.target.checked) {
                    $(el.target).attr('checked', 'checked');
                } else {
                    $(el.target).removeAttr('checked');
                }
            });
            // Radiobuttons
            $(ed.getBody()).on("change", "input:radio", function (el) {
                var name = 'input:radio[name="' + el.target.name + '"]';
                $(ed.getBody()).find(name).removeAttr('checked');
                $(el.target).attr('checked', 'checked');
                $(el.target).prop('checked', true);
            });
            // Selects
            $(ed.getBody()).on("change", "select", function (el) {
                $(el.target).children('option').each(function (index) {
                    if (this.selected) {
                        $(this).attr('selected', 'selected');
                    } else {
                        $(this).removeAttr('selected');
                    }
                });
            });
        });
    }

});
