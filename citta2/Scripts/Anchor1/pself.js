$(function () {

    $("#start_date1").change(function () {
        $("body").addClass("waiting");
        //$("#end_date1").val("");
        check_date("S");
        $("body").removeClass("waiting");
    });

    $("#duration1").change(function () {
        $("body").addClass("waiting");
        //$("#end_date1").val("");
        check_date("D");
        $("body").removeClass("waiting");
    });

    $("#end_date1").change(function () {
        $("body").addClass("waiting");
        //$("#duration1").val("0");
        check_date("E");
        $("body").removeClass("waiting");
    });

    $("#vac_year").click(function () {
        $("body").addClass("waiting");
        var selradio = $("input[name=string9]:checked").val();
        if (selradio == "Y")
            compute_allowance();
        $("body").removeClass("waiting");
    });

    $("#payal_yesno").click(function () {
        if ($("#payal_yesno").prop("checked")) {
            $("body").addClass("waiting");
            compute_allowance();
            $("body").removeClass("waiting");
        }
        else 
            $("#vac_text").html("Allowance Amount : 0.00");
    });

    $("#pay_no").click(function () {
        $("#vac_text").html("Allowance Amount : 0.00");
    });

    $("#dep_type").change(function () {
        $("#depname").hide();
        $(".newdep").hide();
        var selt = $("#dep_type").val();
        if (selt == "D" || selt == "A" )
            $("#depname").show();
        if (selt == "A" || selt == "N")
            $(".newdep").show();
    });

    var vline;
    $("#staff_number").change(function () {
        lgshow();
        var $form = $(this).closest('form');
        $("#id_xhrt").val("id_line0");
        $form.trigger("submit");
    });

    $("#snumber").change(function () {
        lgshow();
        var $form = $(this).closest('form');
        $("#id_xhrt").val("id_line0");
        $form.trigger("submit");
    });

    $("#vac_type").change(function () {
        lgshow();
        var $form = $(this).closest('form');
        $("#id_xhrt").val("id_line0");
        $form.trigger("submit");
    });

    $(".cls_line").change(function () {
        lgshow();
        vline = "L";
        var idstr = $(this).attr('id');
        var $form = $(this).closest('form');
        $("#id_xhrt").val(idstr);
        $form.trigger("submit");
    });

    $(".adamount").change(function () {
        amt_balance();
    });

    $(".adamount1").change(function () {
        amt_balance();
    });

    $(".adamountb").change(function () {
        amt_total();
    });

    $(".roast_start").change(function () {
        $("body").addClass("waiting");
        var idstr = $(this).attr('id');
        var numstr = idstr.substr(idstr.length - 1, 1);
        //$("#end_roast" + numstr).val("");
        check_roast_date(numstr, "S");
        $("body").removeClass("waiting");
    });

    $(".roast_dura").change(function () {
        $("body").addClass("waiting");
        var idstr = $(this).attr('id');
        var numstr = idstr.substr(idstr.length - 1, 1);
        //$("#end_roast" + numstr).val("");
        check_roast_date(numstr, "D");
        $("body").removeClass("waiting");
    });

    $(".roast_end").change(function () {
        $("body").addClass("waiting");
        var idstr = $(this).attr('id');
        var numstr = idstr.substr(idstr.length - 1, 1);
        //$("#dura_roast" + numstr).val("0");
        check_roast_date(numstr, "E");
        $("body").removeClass("waiting");
    });

    $("#roast_year").change(function () {
        var URLext = $('#roast_year').val();
        var url2 = $('#snumber').val();
        var URL = $('#loader1').data('request-daily');
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL + "04" + URLext + "[]" + url2;
        $.ajax({
            type: "Post",
            url: URL,
            async: false,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    items = state.Text;
                });
                $('#totamt').val(items);
            }
        });

    });


})

function compute_allowance() {
    var URL = $('#loader1').data('request-daily');
    URL = $.trim(URL);
    var url1 = $("#start_date1").val();
    var url2 = $("#duration1").val();
    var url3 = $("#vac_year").val();
    var url4 = $("#snumber").val();
    url1 = $.trim(url1);
    url2 = $.trim(url2);
    url3 = $.trim(url3);
    URL = URL + '03' + url1 + "[]" + url2 + "[]" + url3 + "[]" + url4;
    $.ajax({
        type: "Post",
        url: URL,
        error: function (xhr, status, error) {
            alert("Error: " + xhr.status + " - " + error + " - " + URL);
        },
        success: function (data) {
            var items = '';
            $.each(data, function (i, state) {
                // items = html(state.Value);
                if (state.Value != "") {
                    $("#para_type").show();
                    $("#vac_text").html(state.Value);
                }
                else
                    $("#para_type").hide();
            });

        }
    });
}

function check_date(ctype)
{
    var URL = $('#loader1').data('request-daily');
    URL = $.trim(URL);
    var url1 = $("#start_date1").val();
    var url2 = $("#duration1").val();
    var url3 = $("#end_date1").val();
    var url4 = $("#vac_type").val();
    url1 = $.trim(url1);
    url2 = $.trim(url2);
    url3 = $.trim(url3);
    var testn = $.isNumeric(url2);
    if (testn == true) {
        URL = URL + '02' + url1 + "[]" + url2 + "[]" + url3 + "[]" + ctype;
        $.ajax({
            type: "Post",
            url: URL,
            async: false,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    if (url1 != "" && url2 == "0" && url3 != "")
                        $("#duration1").val(state.Text);
                    else if (url1 != "" && url2 != "0" && url3 == "")
                        $("#end_date1").val(state.Text);
                    else if (url1 == "" && url2 != "0" && url3 != "")
                        $("#start_date1").val(state.Text);
                    else if (url1 != "" && url2 != "0" && url3 != "" && ctype == "D")
                        $("#end_date1").val(state.Text);
                    else if (url1 != "" && url2 != "0" && url3 != "" && ctype == "E")
                        $("#duration1").val(state.Text);
                    else if (url1 != "" && url2 != "0" && url3 != "" && ctype == "S")
                        $("#end_date1").val(state.Text);

                });

                var vac1 = $("#vactype").val();
                if (vac1 == "R") {
                    var recdays = $("#recalldays").html();
                    diff = recdays - url2;
                    if (diff < 0) {
                        alert("You can't recall more than vacation days initially approved - " + recdays + " days")
                    };
                }
                else {
                    var outst = $("#outst").html();
                    var diff = outst - url2;
                    if (diff < 0 && url4 != "MATER") {
                        alert("The days requested exceed your balance - " + outst + " days");
                    };
                }
            }
        });
    };
};

function check_roast_date(numstr, ctype) {
    var URL = $('#loader1').data('request-daily');
    URL = $.trim(URL);
    var url1 = $("#start_roast" + numstr).val();
    var url2 = $("#dura_roast" + numstr).val();
    var url3 = $("#end_roast" + numstr).val();
    var url4 = "ROAST";
    url1 = $.trim(url1);
    url2 = $.trim(url2);
    url3 = $.trim(url3);
    var testn = $.isNumeric(url2);
    if (testn == true) {
        URL = URL + '02' + url1 + "[]" + url2 + "[]" + url3 + "[]" + ctype;
        $.ajax({
            type: "Post",
            url: URL,
            async: false,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    // items = html(state.Value);
                    if (url1 == "" && url2 != "0" && url3 != "")
                        $("#start_roast" + numstr).val(state.Text);
                    else if (url1 != "" && url2 == "0" && url3 != "")
                        $("#dura_roast" + numstr).val(state.Text);
                    else if (url1 != "" && url2 != "0" && url3 == "")
                        $("#end_roast" + numstr).val(state.Text);
                    else if (url1 != "" && url2 != "0" && url3 != "" && ctype == "D")
                        $("#end_roast" + numstr).val(state.Text);
                    else if (url1 != "" && url2 != "0" && url3 != "" && ctype == "E")
                        $("#dura_roast" + numstr).val(state.Text);
                    else if (url1 != "" && url2 != "0" && url3 != "" && ctype == "S")
                        $("#end_roast" + numstr).val(state.Text);

                    amt_total();
                });
            }
        });
    };
};


function get_former_transaction(ttype) {
    var URL = $('#loader2').data('request-daily');
    URL = $.trim(URL);
    var url4 = $("#snumber").val();
    URL = URL + ttype + "[]" + url4;
    $.ajax({
        type: "Post",
        url: URL,
        error: function (xhr, status, error) {
            alert("Error: " + xhr.status + " - " + error + " - " + URL);
        },
        success: function (data) {
            var items = '';
            $.each(data, function (i, state) {
                items += state.Text;
            });
            $("#vaclist").html(items);
            table_creatennn();
        }
    });
};

function amt_total()
{
    var tamt = 0;
    var samt = 0;
    $(".adamountb").each(function () {
        samt = parseFloat($(this).val().replace(/,/g, ''));
        tamt += samt;
    });
    var t2 = (tamt).toFixed(2).replace(/./g, function (c, i, a) {
        return i && c !== "." && ((a.length - i) % 3 === 0) ? ',' + c : c;
    });
    $("#rbal").val(t2);

}

function amt_balance() {
    var tamt = 0;
    var samt = 0;
    $(".adamount").each(function () {
        samt = parseFloat($(this).val().replace(/,/g, ''));
        tamt += samt;
    });
    var t3 = parseFloat($("#totamt").val().replace(/,/g, ''));
    var t2 = (t3 - tamt).toFixed(2).replace(/./g, function (c, i, a) {
        return i && c !== "." && ((a.length - i) % 3 === 0) ? ',' + c : c;
    });
    $("#rtotal").html(t2);

}
