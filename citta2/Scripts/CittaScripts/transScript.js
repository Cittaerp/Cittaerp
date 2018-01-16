$(function () {
    $("#selectid1").change(function () {
        selectp1(1);
    })

    $("#selectid2").change(function () {
        selectp1(2);
    })

    $("#selectid3").change(function () {
        selectp1(3);
    })

    $("#selectid4").change(function () {
        selectp1(4);
    })

     $("#selectid5").change(function () {
        selectp1(5);
    })

    

    // aproval setup for transaction
    $("#insertap").click(function () {
        insert_app("1");
    });

    $("#insertal").click(function () {
        insert_app("2");
    });

    $("#insertin").click(function () {
        insert_app("3");
    });


    $("#btnx-right").click(function () {
        $("#list1 > option:selected").each(function () {
            $(this).remove().appendTo("#list2");
        });
    });

    $("#btnx-left").click(function () {
        $("#list2 > option:selected").each(function () {
            $(this).remove().appendTo("#list1");
        });
    });

    $("#id_trans").change(function () {
        var URL = $('#loader5').data('request-daily');
        var URLext = $('#id_trans').val();
        URL = $.trim(URL);
        URLext = $.trim(URLext);

        $.ajax({
            type: "Post",
            url: URL,
            data: {  id: URLext },
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = "<option value=''></option>";
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $("#selectid1").html(items);
            }
        });
    });

});
function insert_app(ind) {
    var URL = $('#loader3').data('request-updatev');
    var op1 = $('#AppName').val();
    var maxp = $('#vwdecimal7').val();
    var pos1 = $("input:radio[name='vwstring7']:checked").val();

    var operat = "";
    $(".radclass").each(function () {
        operat += $(this).val();
   });
    //        operat += "          ";
    var amtvar = "";
    $(".amtclass").each(function () {
        amtvar += $(this).val() + "bb";
    });

    var mystring = ind + "[]" + $.trim(op1) + "[]" + $.trim(pos1) + "[]" + $.trim(maxp) + "[]" + operat + "[]" + amtvar;

    URL = $.trim(URL) + "/" + mystring
    $.ajax({
        type: "POST",
        url: URL,
        error: function (xhr, status, error) {
            alert("Error: " + xhr.status + " - " + error + " - " + URL);
        },
        success: function (data) {
            $('#comp1').html(data);
        }
    });

};

function selectp1(ind) {
    var URL = $('#loader1').data('request-ajax1');
    var sel1 = "#selectid" + ind;
    var self="#select_from"+ind;
    var selt="#select_to"+ind;
    var URLext = $(sel1).val();
    URL = $.trim(URL);
    URLext = $.trim(URLext);
    $.ajax({
        type: "Post",
        url: URL,
        data: { typecode: "02", id: URLext },
        error: function (xhr, status, error) {
            alert("Error: " + xhr.status + " - " + error + " - " + URL);
        },
        success: function (data) {
            var items = "<option value=''></option>";
            $.each(data, function (i, state) {
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            });
            $(self).html(items);
            $(selt).html(items);
        }
    });
};
