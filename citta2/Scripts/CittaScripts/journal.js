$(function () {

    $(".myradio input[type=radio]").checkboxradio({ icon: false });

    var head_details = $("#id_journal").val();
    if (head_details == "H") {
        $("#head").show();
        $("#details").hide("fast");
    }
    else {
        $("#details").show();
        $("#head").hide("fast");
    }



    //$("#id_amt").change(function () {
    //    var URL = $('#loader4').data('request-ajax4');
    //    var URLext = $('#id_amt').val();
    //    var set_price = $('#d_debit').val();
    //    var set_price1 = $('#c_credit').val();
    //    URL = $.trim(URL);

    //    URLext = $.trim(URLext);
    //    URL = URL;
    //    $.ajax({
    //        type: "Post",
    //        url: URL,
    //        data: { main_amt: URLext, id_debit: set_price, id_credit: set_price1 },
    //        error: function (xhr, status, error) {
    //            alert("Error: " + xhr.status + " - " + error + " - " + URL);
    //        },
    //        success: function (data) {
    //            var items = '';
    //            $.each(data, function (i, state) {
    //                items = state.Text;
    //                if (state.Value == "debit")
    //                    $("#id_debit").val(items);
    //                else
    //                    $("#id_credit").val(items);
    //            });


    //        }
    //    });
     
    //});

    $("#sj").click(function () {
        $("#rjdisplay").hide();
        $("#ajdisplay").hide();
        $("#fjdisplay").hide();
    })

    $("#rj").click(function () {
        $("#rjdisplay").show();
        $("#ajdisplay").hide();
        $("#fjdisplay").hide();
    })

    $("#aj").click(function () {
        $("#rjdisplay").hide();
        $("#ajdisplay").show();
        $("#fjdisplay").hide();
    })

    $("#fj").click(function () {
        $("#rjdisplay").hide();
        $("#ajdisplay").hide();
        $("#fjdisplay").show();
    })
    $("#id_class").change(function () {
        var URL = $('#loader3').data('request-ajax3');
        var URLext = $('#id_class').val();
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL + "/" + URLext;
        $.ajax({
            type: "Post",
            url: URL,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = "<option value=''></option>";
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $("#id_fid").html(items);
            }
        });
    });
});