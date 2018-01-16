$(function () {

    var mode1 = $("#datmode").val();
    if (mode1 != "E") {
        var cde = $("#defsn").val();
        if (cde == null || cde == undefined || cde == "undefined")
            cde = "";

        defaultsn(cde);

        var num = $("#snumber").val();
        $("#staff_number").val(num);
    }

} );

function defaultsn(def1) {
        var URL = $('#loader1b').data('request-dailyb');
        $.ajax({
            type: "Post",
            async: false,
            url: URL + "04"+ def1,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = "<option value=''></option>";
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $('#staff_number').html(items);
                $('#staff1').html(items);
                $('#staff2').html(items);

            }
        });
    }

