$(function () {

    var head_details = $("#id_stock_count").val();
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
                        $("#id_qty").val(items);
                    else if (state.Value == "loc")
                        $("#id_bin").html(items);
                    else
                        $("#id_itm_des").html(items);
                    
                });


            }
        });
     
    });


    $(".stock_count").change(function () {
        var URL = $('#loader1').data('request-ajax1');
        var URLext = $('#id_phy_count').val();
        var qtyt = $('#id_qty').val();
        URL = $.trim(URL);

        URLext = $.trim(URLext);
        URL = URL;
        $.ajax({
            type: "Post",
            url: URL,
            data: { phy_count: URLext, item_qty: qtyt},
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    items = state.Text;
                    $("#id_dif").val(items);
                   
                });


            }
        });
    });

});