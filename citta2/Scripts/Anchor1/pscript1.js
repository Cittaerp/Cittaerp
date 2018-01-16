$(function () {

    // for report design
    $("#multi_line input[name='ws_string5']").click(function () {
        var radiob = $("input[name='ws_string5']:checked").val();
        if (radiob == "Y") {
            $("#row_col").show();
        }
        else {
            $("#row_col").hide();
        };
    });

    // report type standard report
    $("#rep_type_std").change(function () {
        var URLext = $('#rep_type_std').val();
        var URL = $('#loader1').data('request-daily');
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL +  URLext;
        //$.getJSON(URL, 
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
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $('#ws_string2').html(items);
            }
        });

    });

    $('#reportID').change(function () {
        $("body").addClass("waiting");
        var URL = $('#loader1').data('request-daily');
        var URLext = $('#reportID').val();
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL + URLext;
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
                    if (state.Value == "operand" && state.Text == "operand") {
                        $('#operandID').html(items);
                        items = "";
                    }
                    else if (state.Value == "source" && state.Text == "source") {
                        $('#sourceID').html(items);
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

        $("body").removeClass("waiting");
    });

    $('#operandID').change(function () {
        $("body").addClass("waiting");
        var st = $('#operandID').val();
        $('#kop').val(st);
        $('#ksrc').val("");
        $('#kperd').val("");

        var st2 = $('#ktype').val();
        if (st2 == "A") {
            //            var URL = '@Url.Action("Daily2List") ';
            var URL = $('#loader2').data('request-daily2');
            var URLext = $('#operandID').val();
            URL = $.trim(URL);
            URLext = $.trim(URLext);
            URL = URL + URLext;
            //$.getJSON(URL, 
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
                        // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
                    });
                    $('#sourceID').html(items);
                }
            });
        };
        $("body").removeClass("waiting");
    });

    $("#sourceID").change(function () {
        var st1 = $('#sourceID').val();
        $('#ksrc').val(st1);
    });

    $('#periodID').change(function () {
        var st2 = $('#periodID').val();
        $('#kperd').val(st2);
    });

    // for report design
    $("#insertd").click(function () {
        //var URL = '@Url.Action("update_view") ';
        var URL = $('#loader3').data('request-updatev');
        var op1 = $('#operandID').val();
        var src1 = $('#sourceID').val();
        var perd1 = $('#periodID').val();
        var num1 = $('#ws_decimal6').val();
        var maxp = $('#ws_decimal7').val();
        var ws_code = $('#ws_code').val();
        //var pos1 = $('#ws_string7').val();
        var pos1 = $("input:radio[name='ws_string7']:checked").val();
        var operat = "";
        $(".seqclass").each(function () {
            operat += ($(this).val() + "     ").substr(0, 4);
        });

        operat += "     ";
        var mystring = $.trim(op1) + "[]" + $.trim(src1) + "[]" + $.trim(perd1) + "[]" + $.trim(num1) + "[]";
        mystring += $.trim(pos1) + "[]" + $.trim(maxp) + "[]" + $.trim(ws_code) + "[]" + operat + "[]";

        URL = $.trim(URL); //+ mystring
        $.ajax({
            type: "POST",
            url: URL,
            data : {op:op1, src1:src1,perd1:perd1,num1:num1,pos1:pos1,max_pos:maxp,ws_code:ws_code,operat:operat},
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                $('#comp1').html(data);
            }
        });

    });

    // for calculation
    $("#insertf").click(function () {
        //var URL = '@Url.Action("update_view") ';
        var URL = $('#loader3').data('request-updatev');
        var op1 = $('#operandID').val();
        var src1 = $('#sourceID').val();
        var perd1 = $('#periodID').val();
        var num1 = $('#ws_decimal6').val();
        var maxp = $('#ws_decimal7').val();
        var ws_code = $('#ws_code').val();
        //var pos1 = $('#ws_string7').val();
        var pos1 = $("input:radio[name='ws_string7']:checked").val();
        var operat = "";
        $(".radclass").each(function () {
            operat += $(this).val();
        });

        operat += "          ";
        var mystring = $.trim(op1) + "[]" + $.trim(src1) + "[]" + $.trim(perd1) + "[]" + $.trim(num1) + "[]";
        mystring += $.trim(pos1) + "[]" + $.trim(maxp) + "[]" + $.trim(ws_code) + "[]" + operat + "[]";

        URL = $.trim(URL) + mystring
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

    });

    // for document view
    $("#insertdoc").click(function () {
        //var URL = '@Url.Action("update_view") ';
        var URL = $('#loader3').data('request-updatev');
        var op1 = $('#operandID').val();
        var src1 = $('#sourceID').val();
        var perd1 = $('#periodID').val();
        //var num1 = $('#ws_decimal6').val();
        var maxp = $('#ws_decimal7').val();
        var ws_code = $('#ws_code').val();
        //var pos1 = $('#ws_string7').val();
        var pos1 = $("input:radio[name='ws_string7']:checked").val();
        var operat = "";
        $(".mfield").each(function () {
            operat += $(this).val() + "]bb";
        });

        operat += "          ";
        var mystring = $.trim(op1) + "[]" + $.trim(src1) + "[]" + $.trim(perd1) + "[]";
        mystring += $.trim(pos1) + "[]" + $.trim(maxp) + "[]" + $.trim(ws_code) + "[]" + operat + "[]";

        URL = $.trim(URL) + mystring
        $.ajax({
            type: "POST",
            url: URL,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                $('#doc1').html(data);
            }
        });

    });

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

    // for allowance,daily, overtime
    $("#para_type input[name='ws_string2']").click(function () {
        alscrn();
    });

    $('#folderID').change(function () {
        //        var URL = '@Url.Action("DailyList") ';
        var URL = $('#loader1').data('request-daily');
        var URLext = $('#folderID').val();
        $('#kop').val(URLext);
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL + URLext;
        //$.getJSON(URL, 
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
                    // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
                });
                $('#StatesID').html(items);

            }
        });
    });

    $('#StatesID').change(function () {
        var st = $('#StatesID').val();
        $('#ksrc').val(st);
    });

    $("#insert").click(function () {
        //var URL = '@Url.Action("update_view") ';
        var URL = $('#loader3').data('request-updatev');
        var op1 = $('#StatesID').val();
        var src1 = $('#folderID').val();
        var perd1 = $('#PeriodID').val();
        var num1 = $('#ws_decimal6').val();
        var maxp = $('#ws_decimal7').val();
        var ws_code = $('#ws_code').val();
        //var pos1 = $("#ws_string7").val();
        var pos1 = $("input:radio[name='ws_string7']:checked").val();
        var operat = "";
        $(".radclass").each(function () {
            operat += $(this).val();
        });

        operat += "          ";
        var mystring = $.trim(op1) + "[]" + $.trim(src1) + "[]" + $.trim(perd1) + "[]" + $.trim(num1) + "[]";
        mystring += $.trim(pos1) + "[]" + $.trim(maxp) + "[]" + $.trim(ws_code) + "[]" + operat;

        URL = $.trim(URL) + mystring
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
    });

    // for condition inserts
    $(".condbtn").click(function () {
        var btntype = $(this).attr('name');
        var URL = $('#loader3').data('request-updatev');
        var op1 = $('#operandID').val();
        var src1 = $('#sourceID').val();
        var perd1 = $('#periodID').val();
        var num1 = $('#ws_string5').val();
        var maxp = $('#ws_decimal7').val();
        var ws_code = $('#ws_code').val();
        var pos1 = $("input:radio[name='ws_string7']:checked").val();
        
        var operat = "";
        $(".radclass").each(function () {
            operat += $(this).val();
        });
        operat += "          ";

        var comp1 = "";
        $(".compclass").each(function () {
            comp1 += $(this).val();
        });
        comp1 += "          ";

        var mystring = $.trim(op1) + "[]" + $.trim(src1) + "[]" + $.trim(perd1) + "[]" + $.trim(num1) + "[]";
        mystring += $.trim(pos1) + "[]" + $.trim(maxp) + "[]" + $.trim(ws_code) + "[]" + btntype + "[]" + operat + "[]" + comp1 + "[]";

        URL = $.trim(URL) + mystring
        $.ajax({
            type: "POST",
            url: URL,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                $('#cond1').html(data);
            }
        });

    });


    function insert_app(ind) {
        var URL = $('#loader3').data('request-updatev');
        var op1 = $('#AppName').val();
        var maxp = $('#ws_decimal7').val();
        var pos1 = $("input:radio[name='ws_string7']:checked").val();
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

        URL = $.trim(URL) + mystring
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

});

    function alscrn() {
    var radiob = $("input[name='ws_string2']:checked").val();
    if (radiob == "A") {
        $("#tax_line").show();
        $("#pay_maximum").html("Maximum Payment Per year");
        $("#when_payment").html("When Payment is Made");
        $("#allow_specific").html("Allowance Specific to Staff?");
        $("#row_loan").show();
    }
    else {
        $("#tax_line").hide();
        $("#pay_maximum").html("Maximum Deduction Per year");
        $("#when_payment").html("When Deduction is Made");
        $("#allow_specific").html("Deduction Specific to Staff?");
        $("#row_loan").hide();
    };
}


