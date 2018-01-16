$(function () {

    //account
    $('#PfolderID').change(function () {
        var URL = $('#loader1').data('request-daily');
        var URLext = $('#PfolderID').val();
        $('#kop').val(URLext);
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL + URLext;
        //$.getJSON(URL, 
        $.ajax({
            type: "Post",
            //async: false,
            url: URL,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    if (state.Value == "operand" && state.Text == "operand") {
                        $('#StatesID').html(items);
                        items = "";
                    }
                    else if (state.Value == "source" && state.Text == "source") {
                        $('#AttrbID').html(items);
                        items = "";
                    }
                    else if (state.Value == "period" && state.Text == "period") {
                        $('#periodID').html(items);
                        items = "";
                    }
                    else
                        items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                    
                });
            }

        });

    });

    $("#StatesID").change(function () {
        var ka = $('#StatesID').val();

        if (ka == "BASIC" || ka == "BONUS" || ka == "OVER" || ka == "PUBLI" || ka == "WEEK" || ka == "WKEND") {
            $("#cdebit").css("backgroundColor", "Skyblue");
            $("#ccredit").css("backgroundColor", "");
        }
        else if (ka == "TAX" || ka == "PAY") {
            $("#ccredit").css("backgroundColor", "Skyblue");
            $("#cdebit").css("backgroundColor", "");
        }
        else {
            $("#cdebit").css("backgroundColor", "");
            $("#ccredit").css("backgroundColor", "");
        }
    });

    $("#AttrbID").change(function () {
        var ka = $('#StateID').val();
        var ja = $('#AttrbID').val();
        $('#ksrc').val(ja);

        if (ja == "AMT" || ja == "DAMT" || ja == "AMT1" || ja == "AMTB" || ka == "BASIC" || ka == "BONUS") {
            $("#cdebit").css("backgroundColor", "Skyblue");
            $("#ccredit").css("backgroundColor", "");
        }
        else if (ja == "AMTT" || ja == "AMTX" || ja == "DDAMT" || ja == "EMP" || ja == "MDED" || ja == "PDED" || ja == "MINT" || ka == "TAX" || ka == "PAY") {
            $("#ccredit").css("backgroundColor", "Skyblue");
            $("#cdebit").css("backgroundColor", "");
        }
        else if (ja == "EMR") {
            $("#cdebit").css("backgroundColor", "Skyblue");
            $("#ccredit").css("backgroundColor", "Skyblue");
        }
            //else if (ja == "") {
            //    if (ka == "BASIC" || ka == "BONUS") {
            //        $("#cdebit").css("backgroundColor", "Skyblue");
            //        $("#ccredit").css("backgroundColor", "");
            //    }
            //    else {
            //        $("#cdebit").css("backgroundColor", "");
            //        $("#ccredit").css("backgroundColor", "Skyblue");
            //    }
            //}
        else {
            $("#cdebit").css("backgroundColor", "");
            $("#ccredit").css("backgroundColor", "");
        }
    });

    $('#posta').change(function () {
        var URL = $('#loader4').data('request-daily3');
        var URLext = $('#posta').val();
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL + URLext;
        //$.getJSON(URL, 
        $.ajax({
            type: "Post",
            url: URL,
            //error: function (xhr, status, error) {
            //    alert("Error: " + xhr.status + " - " + error + " - " + URL);
            //},
            //success: function (data) {
            //    var items = '';
            //    $.each(data, function (i, state) {
            //        //                        items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            //        // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
            //    });
            //    //                    $('#StatesID').html(items);

            //}
        });
    })

    //limit definition

    $("#tlimit_type").change(function () {
        var URL = $('#loader1').data('request-daily');
        URL = $.trim(URL);
        var URLext = $('#tlimit_type').val();
        URLext = $.trim(URLext);
        URL = URL + '01[]' + URLext + "[]";
        $.ajax({
            type: "Post",
            url: URL,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = "";
                $.each(data, function (i, state) {
                    if (state.Value == "textyear" && state.Text == "0") {
                        $("#tamount").html(items);
                        items = "";
                    }
                    else if (state.Value == "textidentify" && state.Text == "0") {
                        $("#tyear").html(items);
                        items = "<option></option>";
                    }
                    else
                        items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $('#tselectfield').html(items);
            }
        });
    });

    $("#tselectfield").change(function () {
        var URL = $('#loader1').data('request-daily');
        URL = $.trim(URL);
        var URLext = $('#tlimit_type').val();
        URLext = $.trim(URLext);
        URL = URL + '02[]' + URLext + "[]" + $("#tselectfield").val();
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
                });
                $('#tselectvalue').html(items);
            }
        });
    });

    $("#tlimit").change(function () {
        var URL = $('#loader1').data('request-daily');
        URL = $.trim(URL);
        var URLext = $('#tlimit').val();
        URLext = $.trim(URLext);
        URL = URL + '03[]' + URLext + "[]";
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
                });
                $('#tmap').html(items);
            }
        });
    });

    // view
    $("#id_over").click(function () {
        var item1 = get_transaction('over');
        $('#overtime').html(item1);
        table_creatennp();
    });

    $("#id_loan").click(function () {
        var item1 = get_transaction('loan');
        $('#loan').html(item1);
        table_creatennp();
    });

    $("#id_allow").click(function () {
        var item1 = get_transaction('allow');
        $('#allowance').html(item1);
        table_creatennp();
    });

    $("#id_daily").click(function () {
        var item1 = get_transaction('daily');
        $('#daily').html(item1);
        table_creatennp();
    });

    $("#id_salary").click(function () {
        var item1 = get_transaction('salary');
        $('#salary').html(item1);
        table_creatennp();
    });

    $("#id_sepa").click(function () {
        var item1 = get_transaction('sepa');
        $('#sepa').html(item1);
        table_creatennp();
    });

    $("#id_partp").click(function () {
        var item1 = get_transaction('partp');
        $('#partp').html(item1);
        table_creatennp();
    });

    $("#id_tax").click(function () {
        var item1 = get_transaction('tax');
        $('#tax').html(item1);
        table_creatennp();
    });

    $("#id_days").click(function () {
        var item1 = get_transaction('days');
        $('#days').html(item1);
        table_creatennp();
    });

})

    function get_transaction(opt1) {
        var items = '';
        var URL = $('#loader1').data('request-daily');
        URL = $.trim(URL);

        var opt = $("input[name='trans1']:checked").val();
        URL = URL + opt + "[]" + opt1
        $.ajax({
            type: "Post",
            async: false,
            url: URL,
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
    };





