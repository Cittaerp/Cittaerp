
    $(function () {

        var vline;

        ctr = $("#newpos").val();
        $("#htabs").tabs("option", "active", ctr);


        ctr = $("#chgtab").val();

        $(".pftabs").click(function () {
            var newpos = $(this).data('pos');
            var oldpos = $("#oldpos").val();
//            if (newpos > 1) {
                if (oldpos != newpos) {
                    var $form = $(this).closest('form');
                    $("#newpos").val(newpos);
                    $("#id_xhrt").val("TB");
                    lgshow();
                    $form.trigger("submit");
                }
//            }
        });

        $("#id_function").click(function () {
            var doctype = $("#id_function").val();
            var URL = $('#loader1').data('request-daily');
            URL = $.trim(URL);
            doctype = $.trim(doctype);
            URL = URL + "01"+ doctype;
            $.ajax({
                type: "Post",
                url: URL,
                async: false,
                error: function (xhr, status, error) {
                    alert("Error: " + xhr.status + " - " + error + " - " + URL);
                },
                success: function (data) {
                    var items = "<option value=''>Select Job Ttile</option>";
                    $.each(data, function (i, state) {
                        items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                        // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
                    });
                    $('#id_jobtitle').html(items);
                }
            });
        })

        $("#id_reload").click(function () {
                $("body").addClass("waiting");
                var $form = $(this).closest('form');
                $("#newpos").val(0);
                $("#id_xhrt").val("RD");
                $form.trigger("submit");
        });

        // htemplate
        $(".moption").change(function () {
            var URL = $('#loader1').data('request-daily1');
            URL = $.trim(URL);
            var divid = $(this).attr("id");
            var URLext = $(this).val();
            var pos = divid.substring(3)

            URLext = $.trim(URLext);
            URL = URL + URLext;
            $.ajax({
                type: "Post",
                url: URL,
                error: function (xhr, status, error) {
                    alert("Error: " + xhr.status + " - " + error + " - " + URL);
                },
                success: function (data) {
                    var items = "";
                    $.each(data, function (i, state) {
                        items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                        // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
                    });
                    var xcode = "#xcode" + pos;
                    $(xcode).html(items);
                }
            });

        });

        $("#selectbtn").click(function () {
            var item1 = get_transactionpf("01");
            $('#pflist').html(item1);
            table_creatennn();
        });

        $("#actselectbtn").click(function () {
            var item1 = get_transactionpf("02");
            $('#pflist').html(item1);
            table_creatennn();
        });


})

    function get_transactionpf(opt) {
        var items = "";
        var url1 = opt + "[]" + $("#staff1").val() + "[]" + $("#staff2").val() + "[]";
        url1 += $("#pperiod").val() + "[]" + $("#stat1").val() + "[]" + $("#stat2").val() + "[]" + $("#optf").val();
        var URL = $('#loader1').data('request-daily');
        URL = $.trim(URL);
        URL = URL + url1;
        $.ajax({
            type: "Post",
            url: URL,
            async: false,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                $.each(data, function (i, state) {
                    items += state.Text;
                    // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
                });
            }
        });
        return items;

    }

    function get_transapplicant() {
        var items = "";
        var url1 = "P1" + $("#staff1").val() + "[]" + $("input[name='ws_string1']:checked").val();
        var URL = $('#loader1').data('request-daily');
        URL = $.trim(URL);
        URL = URL + url1;
        $.ajax({
            type: "Post",
            url: URL,
            async: false,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                $.each(data, function (i, state) {
                    items += state.Text;
                    // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
                });
            }
        });
        return items;

    }

