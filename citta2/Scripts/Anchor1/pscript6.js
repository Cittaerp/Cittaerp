$(function () {

    $(".formsubmitx1").change(function () {
        var $form = $(this).closest('form');
        $("#id_xhrt").val("N");
        $form.trigger("submit");

    });



})

