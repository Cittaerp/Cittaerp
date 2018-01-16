$(function () {

    var head_details = $("#id_sales_order").val();
    if (head_details == "H") {
        $("#head").show();
        $("#details").hide("fast");
    }
    else {
        $("#details").show();
        $("#head").hide("fast");
    }

    $("#display").click(function () {
        $("#head").show();
        $("#details").hide("fast");
    });

    $("#direct").click(function () {
        $("#details").show();
        $("#head").hide("fast");
    });


    $("#id_item").change(function () {
        var URL = $('#loader4').data('request-ajax4');
        var URLext = $('#id_item').val();
        var set_price = "N"
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
                    else if (state.Value == "price_flag") {
                        set_price = items;
                        if (set_price == "Y")
                            $('#id_price').attr('disabled', 'disabled');
                        else
                            $('#id_price').removeAttr('disabled');

                    }
                    else {
                        $("#id_price").val(items);
                        $("#id_price1").val(items);
                    }
                });


            }
        });
     
    });

    $(".class_price").change(function () {
        var URL = $('#loader1').data('request-ajax1');
        var URLext = $('#id_price').val();
        var qtyt = $('#id_qty').val();
        var itemt = $('#id_item').val();
        var disct = $('#id_dis').val();
        var disct1 = $('#id_dis1').val();
        var show_dist = "N"
        URL = $.trim(URL);

        URLext = $.trim(URLext);
        URL = URL;
        $.ajax({
            type: "Post",
            url: URL,
            data: { item_price: URLext, item_qty: qtyt, item_code: itemt, disc_amt: disct, disc_per: disct1 },
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    items = state.Text;
                    if (state.Value == "1")
                        $("#id_ext_price").val(items);
                    else if (state.Value == "2")
                        $("#id_tax").val(items);
                    else if (state.Value == "3")
                        $("#id_net_amt").val(items);
                    else if (state.Value == "4")
                        $("#id_quote_amt").val(items);
                    else if (state.Value == "5")
                        $("#id_act_dis").val(items);
                    else if (state.Value == "7")
                        $("#tax_id0").val(items);
                    else if (state.Value == "8")
                        $("#tax_id1").val(items);
                    else if (state.Value == "9")
                        $("#tax_id2").val(items);
                    else if (state.Value == "10")
                        $("#tax_id3").val(items);
                    else if (state.Value == "11")
                        $("#tax_id4").val(items);
                    else if (state.Value == "12")
                        $("#id_flat").val(items);
                    else if (state.Value == "13")
                        $("#id_act_dis1").val(items);
                    else if (state.Value == "14") {
                        show_dist = items;
                        if (show_dist == "Y")
                            $("#show_dis").show();
                        else
                            $("#show_dis").hide();
                    }
                    else
                        $("#id_dis_err").html(items);
                });


            }
        });
    });

    //$("#ex_del_days").change(function () {
       
    //})
    //$("#id_cust_code").change(function () {
    //    var items = '';
    //    var shw_rat = "N"
    //    var rate_amt = "";
    //    var curen_name = "";
    //    var URL = $('#loader5').data('request-ajax5');
    //    var URLext = $('#id_cust_code').val();
    //    URL = $.trim(URL);
    //    URLext = $.trim(URLext);
    //    $.ajax({
    //        type: "Post",
    //        url: URL,
    //        data: {cust_code: URLext },
    //        error: function (xhr, status, error) {
    //            alert("Error: " + xhr.status + " - " + error + " - " + URL);
    //        },
    //        success: function (data) {
    //            $.each(data, function (i, state) {
    //                if (state.Value == "1")
    //                    items = state.Text;
    //                else if (state.Value == "2")
    //                    shw_rat = state.Text;
    //                else if (state.Value == "3")
    //                    rate_amt = state.Text;
    //                else if (state.Value == "qty")
    //                    items = state.Text;

    //                else
    //                    curen_name = state.Text;
    //            });
    //        }
    //    })

    //    if (shw_rat == "Y") {
    //        $("#changer").show();
    //    }
    //    else {
    //        $("#changer").hide();
    //    }

    //    $("#curren_name").val(curen_name);
    //    $("#id_hcuren").val(items);
    //    $("#id_rate").val(rate_amt);
    //    $("#fx_flag").val(shw_rat);
    //    $("#id_srep").html(items);
    //});

    $("#id_ware").change(function () {
        var URL = $('#loader6').data('request-ajax6');
        var URLext = $('#id_ware').val();
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL;
        $.ajax({
            type: "Post",
            url: URL,
            data: { ware_code: URLext },
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    items = state.Text;
                });
                $("#id_phyqty").html(items);

            }
        });

    });

    //$("#id_cust_code").change(function () {
    //    var URL = $('#loader7').data('request-ajax7');
    //    var URLext = $('#id_cust_code').val();
    //    URL = $.trim(URL);
    //    URLext = $.trim(URLext);
    //    URL = URL;
    //    alert(URLext);
    //    $.ajax({
    //        type: "Post",
    //        url: URL,
    //        data: { cust_code: URLext },
    //        error: function (xhr, status, error) {
    //            alert("Error: " + xhr.status + " - " + error + " - " + URL);
    //        },
    //        success: function (data) {
    //            var items = '';
    //            $.each(data, function (i, state) {
    //                items = state.Text;
    //            });
    //            $("#id_srep").html(items);

    //        }
    //    });

    //});

    function numbersOnly(oToCheckField, oKeyEvent) {
        return oKeyEvent.charCode === 0 ||
            /\d/.test(String.fromCharCode(oKeyEvent.charCode));
    }


});