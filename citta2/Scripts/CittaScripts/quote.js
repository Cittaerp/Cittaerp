$(function () {

    var head_details = $("#id_sales_quote").val();
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
        var editexch = $("#id_editexch").val();
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
                    
                    else {
                        $("#id_price").val(items);
                        $("#id_price1").val(items);
                        if (editexch=="N" && Number(items)==0 && items != -0 )
                        {
                            alert("Price Matrix not Valid")
                        }
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
                        $("#id_crate").val(items);
                    else if (state.Value == "8")
                        $("#id_cprice").val(items);
                    else if (state.Value == "9")
                        $("#id_bamt").val(items);
                    else
                        $("#id_dis_err").html(items);
                });


            }
        });
    });

    $("#ex_del_days").change(function () {
        var URL = $('#loader6').data('request-ajax6');
        var URLext = $('#ex_del_days').val();
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
                var items = '';
                $.each(data, function (i, state) {
                    items = state.Text;
                });
                $("#ex_del_date").val(items);
            }
        });
    });

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
    //        async: false,
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
    //});

    function numbersOnly(oToCheckField, oKeyEvent) {
        return oKeyEvent.charCode === 0 ||
            /\d/.test(String.fromCharCode(oKeyEvent.charCode));
    }


});