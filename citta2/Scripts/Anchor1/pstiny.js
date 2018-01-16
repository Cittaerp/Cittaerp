
$(function () {

    var tinyloc = $("#locat").val() + "content/tinycontent.css";

    tinymce.init({
        selector: '#mytinytextarea',
        //min_width: 800,
        resize : 'both',
        plugins: "anchor autolink charmap textcolor colorpicker contextmenu fullscreen hr image imagetools insertdatetime searchreplace table wordcount autoresize nonbreaking ",
        contextmenu: "link image inserttable | cell row column deletetable | forecolor backcolor ",
        autoresize_min_height: 500,
        nonbreaking_force_tab: true,
        convert_fonts_to_spans: false,

        content_css: tinyloc,
        setup: function (editor) {
        editor.addButton('newmedia', {
            text: 'Add Field', // Text of button
            title: 'Select Field for document', // Tooltip text
            icon: 'image', // This icon comes with TinyMCE
            onclick: function() {
                $("#MediaModal").modal("show"); // When user clicks our button, open modal
            } });
        },
        toolbar: "undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | forecolor backcolor newmedia table | fontselect fontsizeselect ",
        menubar: false,
        contextmenu: " inserttable | cell row column deletetable | cut copy paste ",
    });
    

    $('#InsertField').click(function () {
        $('#MediaModal').modal("hide"); // Close the modal
        var op1 = $("#operandID").val();
        var src1 = $("#sourceID").val();
        var perd1 = $("#periodID").val();
        if (src1 == null) src1 = "";
        if (perd1 == null) perd1 = "";
        var fldname1 = "[" + op1 + ":" + src1 + ":" + perd1 + "]";
        
        /* If variable value is not empty we pass it to tinymce function and it inserts the image to post */
        tinymce.activeEditor.execCommand('mceInsertContent', false, fldname1);
        $('#MediaModal').empty();

    });
})