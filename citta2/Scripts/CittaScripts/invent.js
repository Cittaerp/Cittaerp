/// <reference path="invent.js" />
$(function () {

    var head_details = $("#id_inventory").val();
    if (head_details == "H") {
        $("#head").show();
        $("#details").hide("fast");
    }
    else {
        $("#details").show();
        $("#head").hide("fast");
    }



    $("#id_item").change(function () {
        var URL = $('#loader4').data('request-ajax4');
        var URLext = $('#id_item').val();
        URL = $.trim(URL);

        URLext = $.trim(URLext);
        URL = URL;
        $.ajax({
            type: "Post",
            url: URL,
            data: { item_code: URLext },
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    items = state.Text;
                    if (state.Value == "sku")
                        $("#id_sku").html(items);
                    else if (state.Value == "qty")
                        $("#id_hqty").html(items);
                    else
                        $("#id_itm_des").html(items);
                    
                });


            }
        });
     
    });


    $(".class_price").change(function () {
        var URL = $('#loader1').data('request-ajax1');
        var URLext = $('#id_unit').val();
        var qtyt = $('#id_qty').val();
        URL = $.trim(URL);

        URLext = $.trim(URLext);
        URL = URL;
        $.ajax({
            type: "Post",
            url: URL,
            data: { unit_cost: URLext, item_qty: qtyt},
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    items = state.Text;
                    $("#id_ext_cost").val(items);
                   
                });


            }
        });
    });

    $("#id_trantype").change(function () {
        var trantype = $("#id_trantype").val();
        if (trantype == "02") {
            $("#hide_cost").show();
            $("#show_cost").hide();
        }
        else if (trantype == "04") {
            $("#hide_cost").show();
            $("#show_cost").hide();
        }
        else {
            $("#hide_cost").hide();
            $("#show_cost").show();
        }
    })


    var trantype = $("#id_trantype").val();
    if (trantype == "02") {
        $("#hide_cost").show();
        $("#show_cost").hide();
    }
    else if (trantype == "04") {
        $("#hide_cost").show();
        $("#show_cost").hide();
    }
    else {
        $("#hide_cost").hide();
        $("#show_cost").show();
    }
});