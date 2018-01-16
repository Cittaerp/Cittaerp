$(function () {

    var head_details = $("#id_transt").val();
    if (head_details == "H") {
        $("#head").show();
        $("#details").hide("fast");
    }
    else {
        $("#details").show();
        $("#head").hide("fast");
    }

    $("#id_strantype").change(function () {
        var URL = $('#loader4').data('request-ajax4');
        var URLext = $('#id_strantype').val();
        URL = $.trim(URL);
        $.ajax({
            type: "Post",
            url: URL,
            data: { typecode: "03", id: URLext },
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    if (state.Value == "99X" && state.Text == "postcode") {
                        $("#id_atype").html(items);
                        items = "";
                    }
                    else if (state.Value == "99Z" && state.Text == "actcode") {
                        $("#id_acode").html(items);
                        items = "";
                    }
                    else
                        items += "<option value='" + state.Value + "'>" + state.Text + "</option>";

                });


            }
        });

    });

    var amtcheck = $("#curflag").val();
    if (amtcheck == "N") {
        $("#changer").show();
        $("#id_base").show();
    }
    else {
        $("#changer").hide();
        $("#id_base").hide();
    }


});