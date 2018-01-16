$(function () {

    var retval = $('#id_retgroup').val();
    if (retval == "G") {

        $('#id_asset').html("Asset Group");
        $('#id_locgroup').hide();
    }

    var temp = $('#editcount').val();

    var temp2 = $('#editcount1').val();
    var temp3 = $('#taskcount').val();
    var temp4 = $('#subcount').val();

    var temp5 = $('#miscount').val();
    var temp4i = $('#subcounti').val();

    var temp5i = $('#miscounti').val();

    for (var m = temp; m < 10; m++)
        $("#pool" + m).hide();

    for (var m = temp2; m < 10; m++)
        $("#loop" + m).hide();

    for (var m = temp3; m < 10; m++)
        $("#poolll" + m).hide();

    for (var m = temp4; m < 10; m++)
        $("#poot" + m).hide();
    for (var m = temp5; m < 10; m++)
        $("#spool" + m).hide();

    for (var m = temp4i; m < 5; m++)
        $("#pooti" + m).hide();
    for (var m = temp5i; m < 5; m++)
        $("#spooli" + m).hide();

    $("#id_mainid").change(function () {
        var URL = $('#loader6').data('request-ajax6');
        var URLext = $('#id_mainid').val();
        URL = $.trim(URL);
        URLext = $.trim(URLext);

        URL = URL + "/" + URLext;
        $.ajax({
            url: URL,
            type: 'Post',
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var tasks = '';
                $.each(data, function (i, state) {
                    tasks = state.Text;
                    if (state.Value == "nature") {
                        $("#id_nature").val(tasks);
                    }
                    else if (state.Value == "glaccc") {
                        $("#id_glacc").val(tasks);

                    }

                });

            }

        });

    });

    $("#id_fixed").change(function () {
        var URL = $('#loader4').data('request-ajax4');
        var URLext = $('#id_fixed').val();
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
                $("#id_assetss").html(items);
            }
        });
    });

    $("#id_group").change(function () {
        var URL = $('#loader4').data('request-ajax4');
        var URLext = $('#id_group').val();
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
                $("#id_assetss").html(items);
            }
        });
    });

    var counter = 0;
    $("#add").click(function () {
        counter = counter + 1;

        var k = 1;
        for (var i = 0; i < counter; i++) {
            $("#" + "pool" + k).show();
            k = k + 1;
        }
        // alert(k);
        if (k > 10) {

            alert("limit exceeded");
            return false;
        }
    });

    var counting = 0;

    $("#add2").click(function () {
        counting = counting + 1;

        var j = 1;
        for (var i = 0; i < counting; i++) {
            $("#" + "loop" + j).show();
            j = j + 1;
        }
        // alert(j);
        if (j > 10) {
            alert("limit exceeded");
            return false;
        }
    });

    var counting1 = 0;

    $("#add3").click(function () {
        counting1 = counting1 + 1;

        var j = 1;
        for (var i = 0; i < counting1; i++) {
            $("#" + "poolll" + j).show();
            j = j + 1;
        }
        // alert(j);
        if (j > 10) {
            alert("limit exceeded");
            return false;
        }
    });

    var counting2 = 0;
    $("#add4").click(function () {
        counting2 = counting2 + 1;
        var j = 1;
        for (var i = 0; i < counting2; i++) {
            $("#" + "poot" + j).show();
            j = j + 1;
        }
        // alert(j);
        if (j > 10) {
            alert("limit exceeded");
            return false;
        }
    });

    var counting3 = 0;
    $("#add5").click(function () {
        counting3 = counting3 + 1;

        var j = 1;
        for (var i = 0; i < counting3; i++) {
            $("#" + "spool" + j).show();
            j = j + 1;
        }
        // alert(j);
        if (j > 10) {
            alert("limit exceeded");
            return false;
        }
    });


    $("#add4i").click(function () {
        counting2 = counting2 + 1;
        var j = 1;
        for (var i = 0; i < counting2; i++) {
            $("#" + "pooti" + j).show();
            j = j + 1;
        }
        // alert(j);
        if (j == 5) {
            //alert("limit exceeded");
            $("#add4i").hide();
            return false;
        }
    });

    var counting3 = 0;
    $("#add5i").click(function () {
        counting3 = counting3 + 1;

        var j = 1;
        for (var i = 0; i < counting3; i++) {
            $("#" + "spooli" + j).show();
            j = j + 1;
        }
        // alert(j);
        if (j == 5) {
           // alert("limit exceeded");
            $("#add5i").hide();
            return false;
        }
    });

       $("#id_group").click(function () {
        $('#id_locgroup').hide();
        var URL = $('#loader6').data('request-ajax6');
        var URLext = $('#id_group').val();
        // alert(URLext);

        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL + "/" + URLext;
        $.ajax({
            url: URL,
            type: 'Post',
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var tasks = "<option value=''>Select</option>";
                $.each(data, function (i, state) {
                    tasks += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                    $('#id_assetgroup').html(tasks);
                    $('#id_asset').html("Asset Group");
                });

            }

        });

    });
    $("#id_assetgroup").change(function () {
        var $form = $(this).closest('form');
        $("#id_subcheck").val("RR");
        $form.trigger("submit");
    });
    var calc = 0;
    var matcost = 0

    for (var i = 0; i < 10; i++)
        $("#id_qtyy" + i).change(function () {
            for (var i = 0; i < 10; i++) {
                var $form = $(this).closest('form');
                $("#id_subcheck").val("RO");
                $form.trigger("submit");
                $("#id_tabcheck").val(1);
            }
        });


    for (var i = 0; i < 10; i++)
        $("#id_testtime" + i).change(function () {
            for (var i = 0; i < 10; i++) {
                var $form = $(this).closest('form');
                $("#id_subcheck").val("RO");
                $form.trigger("submit");
                $("#id_tabcheck").val(3);
            }
        });
    for (var i = 0; i < 10; i++)
        $("#id_strtdate" + i).change(function () {
            for (var i = 0; i < 10; i++) {
                var $form = $(this).closest('form');
                $("#id_subcheck").val("RO");
                $form.trigger("submit");
                $("#id_tabcheck").val(3);
            }
        });


    $("#id_workord").change(function () {
        var $form = $(this).closest('form');
        $("#id_subcheck").val("RO");
        $form.trigger("submit");
    });


    for (var i = 0; i < 10; i++)
        $("#id_timingemp" + i).change(function () {
            for (var i = 0; i < 10; i++) {
                var $form = $(this).closest('form');
                $("#id_subcheck").val("RO");
                $form.trigger("submit");
                $("#id_tabcheck").val(2);
            }
        });

    for (var i = 0; i < 10; i++)
        $("#id_esthour" + i).change(function () {
            for (var i = 0; i < 10; i++) {
                var $form = $(this).closest('form');
                $("#id_subcheck").val("RO");
                $form.trigger("submit");
                $("#id_tabcheck").val(2);
            }
        });

    for (var i = 0; i < 10; i++)
        $("#id_esthour1" + i).change(function () {
            for (var i = 0; i < 10; i++) {
                var $form = $(this).closest('form');
                $("#id_subcheck").val("RO");
                $form.trigger("submit");
                $("#id_tabcheck").val(3);
            }
        });


    for (var i = 0; i < 10; i++)
        $("#id_item" + i).change(function () {
            for (var i = 0; i < 10; i++) {
                var $form = $(this).closest('form');
                $("#id_subcheck").val("RO");
                $form.trigger("submit");
                $("#id_tabcheck").val(1);
            }
        });
    for (var i = 0; i < 10; i++)
        $("#id_empname" + i).change(function () {
            for (var i = 0; i < 10; i++) {
                var $form = $(this).closest('form');
                $("#id_subcheck").val("RO");
                $form.trigger("submit");
                $("#id_tabcheck").val(2);
            }
        });

    for (var i = 0; i < 10; i++)
        $("#id_job" + i).change(function () {
            for (var i = 0; i < 10; i++) {
                var $form = $(this).closest('form');
                $("#id_subcheck").val("RO");
                $form.trigger("submit");
                $("#id_tabcheck").val(2);
            }
        });


    for (var i = 0; i < 10; i++)
        $("#id_subid" + i).change(function () {
            for (var i = 0; i < 10; i++) {
                var $form = $(this).closest('form');
                $("#id_subcheck").val("RO");
                $form.trigger("submit");
                $("#id_tabcheck").val(4);
            }
        });

    for (var i = 0; i < 5; i++)
        $("#id_misamt" + i).change(function () {
            for (var i = 0; i < 10; i++) {
                var $form = $(this).closest('form');
                $("#id_subcheck").val("RO");
                $form.trigger("submit");
                $("#id_tabcheck").val(5);
            }
        });

});

