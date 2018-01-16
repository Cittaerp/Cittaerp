$(function () {

    var head_details = $("#id_budget").val();
    if (head_details == "H") {
        $("#head").show();
        $("#details").hide("fast");
    }
    else {
        $("#details").show();
        $("#head").hide("fast");
    }

  

    $("#id_batype").change(function () {
        var URL = $('#loader4').data('request-ajax4');
        var URLext = $('#id_batype').val();
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
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $("#id_bacode").html(items);
            }
        });
    });
   

    $("#id_bacode").change(function () {
        var items = '';
        var shw_rat = "N"
        var URL = $('#loader9').data('request-ajax9');
        var URLext = $('#id_bacode').val();
        var atype = $('#id_batype').val();
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL + "/" + URLext;
        $.ajax({
            type: "Post",
            url: URL,
            async: false,
            data: { ac_code: URLext, id: atype },
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                $.each(data, function (i, state) {
                    if (state.Value == "1") {
                        document.getElementById("id_curen").value = state.Text;
                        items = state.Text
                    }
                    else if (state.Value == "2") {
                        shw_rat = state.Text
                    }
                });
            }
        })
        if (items == shw_rat)
            $("#changer").hide();
        else
            $("#changer").show();

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
                    else
                        $("#id_price").val(items);
                    
                });


            }
        });
     
    });


    $(".class_price").change(function () {
        var URL = $('#loader1').data('request-ajax1');
        var URLext = $('#id_amt').val();
        var rate_e = $('#id_rate').val();
        URL = $.trim(URL);

        URLext = $.trim(URLext);
        URL = URL;
        $.ajax({
            type: "Post",
            url: URL,
            data: { item_amt: URLext, rate_code: rate_e},
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    items = state.Text;
                        $("#id_bamt").val(items);
                });


            }
        });
    });

});