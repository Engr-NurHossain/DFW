tinymce.init({
    /* replace textarea having class .tinymce with tinymce editor */
    selector: "textarea.tinymce",
    branding: false,

    /* theme of the editor */
    theme: "modern",
    skin: "lightgray",
    convert_urls: false,
    document_base_url: 'https://dev.rmrcloud.com/',
    //init_instance_callback: function (editor) {
    //    editor.on('keyup', function (e) {
    //        var body = tinymce.get("txtObjective").getContent(), text = tinymce.trim(body.innerText || body.textContent);
    //        var id_txt = tinymce.activeEditor.id;
    //        var txt = tinyMCE.activeEditor.getContent();
    //        //txt = txt.replace(/(<([^>]+)>)/ig,"");
    //        //txt = txt.replace(/&nbsp;/g, "");
    //        //alert(body);
    //        var len = txt.length;
    //        if (len >= 500) {
    //            txt = body.substring(0, 490);
    //            //alert(txt);
    //            //tinyMCE.activeEditor.setContent(txt.substring(0, 490));
    //            tinymce.get("txtObjective").setContent(body.substring(0, 490))
    //            editor.focus();
    //            editor.selection.select(editor.getBody(), true);
    //            editor.selection.collapse(false);
    //            //return false;
    //        } else {
    //            var res = (500 - len) + '/500';
    //            $('#lblcarrier').text(res);
    //        }
    //    });
    //},
    /* width and height of the editor */
    width: "100%",
    height: 100,

    /* display statusbar */
    statubar: true,

    /* plugin */
    plugins: [
		"lists hr anchor pagebreak"
		//"visualblocks visualchars code nonbreaking"
    ],

    //plugins: [
    // "advlist autolink link image lists charmap print preview hr anchor pagebreak",
    // "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
    // "save table contextmenu directionality emoticons template paste textcolor"
    //],


    /* toolbar */
    toolbar: "undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | forecolor backcolor",
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
    ]
});
