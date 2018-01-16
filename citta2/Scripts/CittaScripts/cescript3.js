$(function () {
    // for selection ranges
    $("#row_date1").hide();
    $("#row_date2").hide();
    $("#row_date3").hide();
    $("#row_date4").hide();
    $("#row_date5").hide();

    $("#group_list").change(function () {
        var URL = $('#loader1b').data('request-dailyb');
        URL = $.trim(URL);
        var URLext = $('#group_list').val();
        URLext = $.trim(URLext);
        URL += "/";
        URL = URL + '04' + URLext;

        $.ajax({
            type: "Post",
            url: URL,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = "<option></option>";
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $('#staff_number').html(items);
                $('#snumber').val('');
            }

        });
    });

    $('#selectid1').change(function () {
        var URLext = $('#selectid1').val();
        select_range(URLext, "1");
    });

    $('#selectid11').change(function () {
        var URLext = $('#selectid11').val();
        select_range(URLext, "1");
    });

    $('#selectid2').change(function () {
        var URLext = $('#selectid2').val();
        select_range(URLext, "2");
    });

    $('#selectid21').change(function () {
        var URLext = $('#selectid21').val();
        select_range(URLext, "2");
    });

    $('#selectid3').change(function () {
        var URLext = $('#selectid3').val();
        select_range(URLext, "3");
    });

    $('#selectid31').change(function () {
        var URLext = $('#selectid31').val();
        select_range(URLext, "3");
    });

    $('#selectid4').change(function () {
        var URLext = $('#selectid4').val();
        select_range(URLext, "4");
    });

    $('#selectid41').change(function () {
        var URLext = $('#selectid41').val();
        select_range(URLext, "4");
    });

    $('#selectid5').change(function () {
        var URLext = $('#selectid5').val();
        select_range(URLext, "5");
    });

    $('#selectid51').change(function () {
        var URLext = $('#selectid51').val();
        select_range(URLext, "5");
    });

    $('#selectid6').change(function () {
        var URLext = $('#selectid6').val();
        select_range(URLext, "6");
    });

    $(".trans_period").change(function () {
        t1pay();
    });

    $("#select_periodw").change(function () {
        var payrun = $("#select_periodw").val();
        var URL = $('#loader1b').data('request-dailyb');
        URL = $.trim(URL);
        URL += "/";
        URL = URL + "22" + payrun;

        $.ajax({
            type: "Post",
            url: URL,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                $.each(data, function (i, state) {
                    var items = '';
                    $.each(data, function (i, state) {
                        items += state.Value;
                    });
                    $("#viewperiod").html(items);
                });
            }
        });

    });

    $("#select_period").change(function () {
        var k1; var pm1; var py1; var py; var pm;
        var payrun = $("#select_period").val();
        var URL = $('#loader1b').data('request-dailyb');
        URL = $.trim(URL);
        URL += "/";
        URL = URL + "121" + payrun;

        $.ajax({
            type: "Post",
            //async : false,
            url: URL,
            error : function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    if (state.Value == 0) {
                        k1 = state.Text;
                        py = k1.substr(0, 4);
                        pm = k1.substr(4, 2);
                        pm1 = Number(pm);
                        py1 = Number(py);
                        $("#period_from_yy").val(py1);
                        $("#period_to_yy").val(py1);
                    }
                    else {
                        if (state.Value == pm1)
                            select_var1 = " selected ";
                        items += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
                        select_var1 = "";
                    }
                });
                $("#period_from_mm").html(items);
                $("#period_to_mm").html(items);
                //$("#period_from_mm").val(pm);
                //$("#period_from_yy").val(py1);
                //$("#period_to_mm").val(pm);
                //$("#period_to_yy").val(py1);
            }

        });

    });

    $("#advapp").change(function () {
        // advance approval
        var URL = $('#loader1').data('request-daily');
        URL = $.trim(URL);
        var URLext = $('#advapp').val();
        URLext = $.trim(URLext);
        URL += "/";
        URL = URL + 'GP' + URLext;

        $.ajax({
            type: "Post",
            url: URL,
//            async : false,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },

            success: function (data) {
                var items = "";
                $.each(data, function (i, state) {
                    if (state.Value == pass_var1)
                        select_var1 = " selected ";
                    items += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
                    select_var1 = "";
                });
                $('#advtrans').html(items);
            },

        });

    });

    $("#select_periodpost").change(function () {
        var k1; var pm1; var py1; var py; var pm;
        var payrun = $("#select_periodpost").val();
        var URL = $('#loader1b').data('request-dailyb');
        URL = $.trim(URL);
        URL += "/";
        URL = URL + "122" + payrun;

        $.ajax({
            type: "Post",
            url: URL,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    if (state.Value == 0) {
                        k1 = state.Text;
                        py = k1.substr(0, 4);
                        pm = k1.substr(4, 2);
                        pm1 = Number(pm);
                        py1 = Number(py);
                    }
                    else
                        items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $("#period_from_mm").html(items);
                $("#period_to_mm").html(items);
                $("#period_from_mm").val(pm);
                $("#period_from_yy").val(py1);
                $("#period_to_mm").val(pm);
                $("#period_to_yy").val(py1);
            }

        });

    });

    $("#ap_trans").change(function () {
//aprroval header details
        var URL = $('#loader1b').data('request-dailyb');
        var URLext = $('#ap_trans').val();
        var idcode = URLext.substring(0, 1);
        URL = $.trim(URL);
        URL += "/";
        URL = URL + "24" + URLext;
        $.ajax({
            type: "Post",
            url: URL,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = "<option value=''></option> ";
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $('.app_opts').html(items);

            }
        });
    });

    $("#ctype").change(function() {
        var URL = $('#loader1').data('request-daily');
        var u1 = $("#ctype").val();
        URL += "/";
        $.ajax({
            type: "Post",
            url: URL + u1,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = "<option value=''></option>";
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $('.ydata').html(items);
            },
        });
    })

});

var pass_var1 = "";
var pass_var2 = "";
var select_var1 = "";
var select_var2 = "";


function check_date(date1) {

    if (date1 == "BDATE" || date1 == "EDATE" || date1 == "LSDATE" || date1 == "SDATE" || date1 == "TDATE")
        return false;
    else
        return true;

}

function select_range(sel5, pos1) {
// range is required to be false as routine is in a loop and uses variables
    var URL = $('#loader1b').data('request-dailyb');
    var URLext = sel5;
    URL = $.trim(URL);
    URLext = $.trim(URLext);
    URL += "/";
    URL = URL + '06' + URLext;

    if (check_date(URLext)) {
        $("#selectid" + pos1).val(URLext);
        $("#row_date" + pos1).hide();
        $("#row_select" + pos1).show();
        $.ajax({
            type: "Post",
            url: URL,
            async : false,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = "<option value=''></option>";
                var items1 = "<option value=''></option>";
                $.each(data, function (i, state) {
                    if (state.Value == pass_var1)
                        select_var1 = " selected ";
                    if (state.Value == pass_var2)
                        select_var2 = " selected ";
                    items += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
                    items1 += "<option " + select_var2 + " value='" + state.Value + "'>" + state.Text + "</option>";
                    select_var1 = "";
                    select_var2 = "";
                });
                $('#select_from' + pos1).html(items);
                $('#select_to' + pos1).html(items1);
            }
        });
    }
    else {
        $("#selectid" + pos1 + "1").val(URLext);
        $("#row_date" + pos1).show();
        $("#row_select" + pos1).hide();
        $('#select_from' + pos1).val(pass_var1);
        $('#select_to' + pos1).val(pass_var2);
    };
}

function usersgrp() {
    var URL = $('#loader1b').data('request-dailyb');
    URL += "/";
    $.ajax({
        type: "Post",
        url: URL + "21",
       //async: false,
        error: function (xhr, status, error) {
            alert("Error: " + xhr.status + " - " + error + " - " + URL);
        },
        success: function (data) {
            pass_var1 = $("#xstring0").val();
            var items = "<option value=''></option>";
            $.each(data, function (i, state) {
                if (state.Value == pass_var1)
                    select_var1 = " selected ";
                items += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
                select_var1 = "";
            });
            $('#acgroup').html(items);
        },
    });

    //var num = $("#xstring0").val();
    //$("#acgroup").val(num);

}

function list12grp() {
    //list names in list tables
    var URL = $('#loader1b').data('request-dailyb');
    URL += "/";
    $.ajax({
        type: "Post",
        url: URL + "07",
        error: function (xhr, status, error) {
            alert("Error: " + xhr.status + " - " + error + " - " + URL);
        },
        success: function (data) {
            var items = "";
            $.each(data, function (i, state) {
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            });
            $('#list1').html(items);
        }
    });

    $.ajax({
        type: "Post",
        url: URL + "08",
        error: function (xhr, status, error) {
            alert("Error: " + xhr.status + " - " + error + " - " + URL);
        },
        success: function (data) {
            var items = "";
            $.each(data, function (i, state) {
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            });
            $('#list2').html(items);
        }
    });

}

function appadv() {
    //approval advance
    var URL = $('#loader1b').data('request-dailyb');
    var errorq = function (xhr, status, error) {
        alert("Error: " + xhr.status + " - " + error + " - " + URL);
    };

    
    URL += "/";
    var wecall = $.when(ajaxcall0(URL + "09"), ajaxcall0(URL + "02"));

    wecall.fail = errorq;
    wecall.done(function (reqd1, reqd2) {
        pass_var1= $("#xstring3").val();
        var items = "<option value=''></option>";
        $.each(reqd1[0], function (i, state) {
            if (state.Value == pass_var1)
                select_var1 = " selected ";
            items += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
            select_var1 = "";
        });
        $('#advapp').html(items);
        //num = $("#xstring3").val();
        //$("#advapp").val(num);
        pass_var1 = $("#xcode").val();
        //$("#advapp").trigger("change");
        //var num = $("#xcode").val();
        //$("#advtrans").val(num);


        num = $("#xstring0").val();
        var items = "<option value=''></option>";
        $.each(reqd2[0], function (i, state) {
            if (state.Value == pass_var1)
                select_var1 = " selected ";
            items += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
            select_var1 = "";
        });
        $('#selectid1').html(items);

        //num = $("#xstring0").val();
        //$("#selectid1").val(num);
        pass_var1 = $("#xstring1").val();
        pass_var2 = $("#xstring2").val();
        //$("#selectid1").trigger("change");
        //num = $("#xstring1").val();
        //$("#select_from1").val(num);
        //num = $("#xstring2").val();
        //$("#select_to1").val(num);

    });

    var wecall1 = $.when(ajaxcall0(URL + "17H47"));

    wecall1.fail = errorq;
    wecall1.done(function (reqd3) {
        var items = "<option value=''></option>";
        $.each(reqd3, function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
        });
        $(".app_opts").html(items);

        for (var wctr = 0; wctr < 10; wctr++) {
            num = $("#xstring1" + wctr.toString()).val();
            $("#ar_string1" + wctr.toString()).val(num);
        }
    });

}

function appadved() {
    //approval advance
    var URL = $('#loader1b').data('request-dailyb');
    var errorq = function (xhr, status, error) {
        alert("Error: " + xhr.status + " - " + error + " - " + URL);
    };

    URL += "/";
    var wecall = $.when(ajaxcall0(URL + "10"));

    wecall.fail = errorq;
    wecall.done(function (reqd3) {
        var items = "<option value=''></option>";
        $.each(reqd3[0], function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
        });
        $(".app_opts").html(items);

        for (var wctr = 0; wctr < 10; wctr++) {
            num = $("#xstring1" + wctr.toString()).val();
            $("#ar_string1" + wctr.toString()).val(num);
        }
    });

}

function select_payrun() {
    var URL = $('#loader1b').data('request-dailyb');
    var errorq = function (xhr, status, error) {
        alert("Error: " + xhr.status + " - " + error + " - " + URL);
    };

    URL += "/";
    var wecall1 = $.when(ajaxcall0(URL + "11"));

    wecall1.fail = errorq;
    wecall1.done(function (reqd1) {
        var items = "";
        $.each(reqd1, function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
        });
        $('#pay_run').html(items);
    });

}

function apphd()
{
    //approval header details
    var URL = $('#loader1b').data('request-dailyb');
    URL += "/";
    var errorq = function (xhr, status, error) {
        alert("Error: " + xhr.status + " - " + error + " - " + URL);
    };

    var num = $("#xcode").val();
    if (num == undefined)
        num = "";
    
    var wecall = $.when(ajaxcall0(URL + "23"));

    wecall.fail = errorq;
    wecall.done(function (reqd1) {
        var items = "";
        $.each(reqd1, function (i, state) {
            if (state.Value == num)
                select_var1 = " selected ";
            items += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
            select_var1 = "";
        });

        $('#ap_trans').html(items);

    });

    var wecall1 = $.when(ajaxcall0(URL + "24" + num));

    wecall1.fail = errorq;
    wecall1.done(function (reqd1) {
        var items = "<option value=''></option> ";
        $.each(reqd1, function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
        });

        $('.app_opts').html(items);
        for(var wctr=0;wctr<10;wctr++)
        {
            num = $("#xstring0" + wctr.toString()).val();
            $("#ar_string0" + wctr.toString()).val(num);
        }
    });


}

function default_payment() {
    var URL = $('#loader1b').data('request-dailyb');
    var errorq = function (xhr, status, error) {
        alert("Error: " + xhr.status + " - " + error + " - " + URL);
    };


    URL += "/";
    var wecall = $.when(ajaxcall0(URL + "11"));

    wecall.fail = errorq;
    wecall.done(function (reqd1) {
        var items = "";
        $.each(reqd1, function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
        });
        $('#select_period').html(items);
        var num = $("#xstring0").val();
        if (num == undefined)
            num = "P";

        $("#select_period").val(num);
        $("#select_period").trigger("change");

        var num = $("#xstring6").val();        
        if (num != undefined)
            $("#period_from_mm").val(num);

    });
        
}

function ajaxcall0(URL) {
    return $.ajax({
        type: "Post",
        url: URL ,
    });

}

function tapp() {
    //report design + others
    var URL = $('#loader1b').data('request-dailyb');
    URL += "/";
    var errorq = function (xhr, status, error) {
        alert("Error: " + xhr.status + " - " + error + " - " + URL);
    };


    $.ajax({
        type: "Post",
        url: URL + "05",
        error: function (xhr, status, error) {
            alert("Error: " + xhr.status + " - " + error + " - " + URL);
        },
        success: function (data) {
            var items = "<option value=''></option>";
            $.each(data, function (i, state) {
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            });
            $('#AppName').html(items);
        }
    })  ;

    var wecall = $.when(ajaxcall0(URL + "01"), ajaxcall0(URL + "02"), ajaxcall0(URL + "17"), ajaxcall0(URL + "04"));
    wecall.fail = errorq;

    wecall.done(function (reqd1, reqd2, reqd3, reqd4) {
        select = "";
        pass_var1 = $("#xcode").val();
        var items = "<option value=''></option>";
        $.each(reqd1[0], function (i, state) {
            if (state.Value == pass_var1)
                select_var1 = " selected ";
            items += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
            select_var1 = "";
        });
        $('#viewtrans').html(items);
        //var num = $("#xcode").val();
        //$("#viewtrans").val(num);
        
        pass_var1= $("#xstring0").val();
        var items = "<option value=''></option>";
        $.each(reqd2[0], function (i, state) {
            if (state.Value == pass_var1)
                select_var1 = " selected ";
            items += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
            select_var1 = "";
        });
        $('#selectid1').html(items);
        //num = $("#xstring0").val();
        //$("#selectid1").val(num);
        var fd1 = pass_var1;
        pass_var1 = $("#xstring1").val();
        pass_var2 = $("#xstring2").val();
        if (fd1 != "")
            select_range(fd1, "1");

        //$("#selectid1").trigger("change");
        //num = $("#xstring1").val();
        //$("#select_from1").val(num);
        //num = $("#xstring2").val();
        //$("#select_to1").val(num);

        pass_var1 = $("#xstring3").val();
        var items = "<option value=''></option>";
        $.each(reqd3[0], function (i, state) {
            if (state.Value == pass_var1)
                select_var1 = " selected ";
            items += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
            select_var1 = "";
        });
        $('#selectid2').html(items);
        //num = $("#xstring3").val();
        //$("#selectid2").val(num);
        fd1 = pass_var1;
        pass_var1 = $("#xstring4").val();
        pass_var2 = $("#xstring5").val();
        if (fd1 != "")
            select_range(fd1, "2");

        //$("#selectid2").trigger("change");
        //num = $("#xstring4").val();
        //$("#select_from2").val(num);
        //num = $("#xstring5").val();
        //$("#select_to2").val(num);

        var items = "<option value=''></option>";
        $.each(reqd4[0], function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
        });
        $('.staffclass').html(items);

        num = $("#xstring6").val();
        $("#staff1").val(num);
        num = $("#xstring7").val();
        $("#staff2").val(num);
        for (var wctr = 0; wctr < 10; wctr++) {
            num = $("#xstring8" + wctr.toString()).val();
            $("#ar_string8" + wctr.toString()).val(num);
        }
    });

}

function rep_query(query1)
{
    var URL = $('#loader1b').data('request-dailyb');
    var urlext = $("#rtype2").val();
    var errorq = function (xhr, status, error) {
        alert("Error: " + xhr.status + " - " + error + " - " + URL);
    };

    URL += "/";
    if (query1 == "MDEP" || query1 == "HREP" || query1 == "HRTRANS") {
        $.ajax({
            type: "Post",
            url: URL + "06NUMB",
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = "<option></option>";
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $('#staff1').html(items);
                $('#staff2').html(items);
            },
        });
    };

    if (query1 == "ACCT") {

        $.ajax({
            type: "Post",
            url: URL + "20",
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = "<option></option>";
                $.each(data, function (i, state) {
                    if (state.Text == state.Value && state.Text=="zxpallow") {
                        $('#allp_from').html(items);
                        $('#allp_to').html(items);
                        items = "<option></option>";
                    }
                    else if (state.Text == state.Value && state.Text == "zxpdeduct") {
                        $('#dedp_from').html(items);
                        $('#dedp_to').html(items);
                        items = "<option></option>";
                    }
                    else if (state.Text == state.Value && state.Text == "zxpdallow") {
                        $('#dallp_from').html(items);
                        $('#dallp_to').html(items);
                        items = "<option></option>";
                    }
                    else if (state.Text == state.Value && state.Text == "zxpddeduct") {
                        $('#ddedp_from').html(items);
                        $('#ddedp_to').html(items);
                        items = "<option></option>";
                    }
                    else if (state.Text == state.Value && state.Text == "zxploan") {
                        $('#lonp_from').html(items);
                        $('#lonp_to').html(items);
                        items = "<option></option>";
                    }
                    else
                        items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
            },
        });

    }

    if (query1 == "BONUS" || query1 == "YEARP" || query1 == "UPD") {
        $.when(ajaxcall0(URL + "11")).done(function (reqd1) {
            var items = "";
            $.each(reqd1, function (i, state) {
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            });
            $('#rep_payrun').html(items);
            $('#select_periodw').html(items);
            $("#select_periodw").trigger("change");
        });
    };

    $.ajax({
        type: "Post",
        url: URL + "17" + urlext,
        error: function (xhr, status, error) {
            alert("Error: " + xhr.status + " - " + error + " - " + URL);
        },
        success: function (data) {
            var items = "<option></option>";
            $.each(data, function (i, state) {
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            });
            $('#selectid1').html(items);
            $('#selectid11').html(items);
            $('#selectid2').html(items);
            $('#selectid21').html(items);
            $('#selectid3').html(items);
            $('#selectid31').html(items);
            $('#selectid4').html(items);
            $('#selectid41').html(items);
            $('#selectid5').html(items);
            $('#selectid51').html(items);
        },
    });

}

function mesca() {
    // menu,emenu and additional fields, who approves --performance
    var URL = $('#loader1b').data('request-dailyb');
    var errorq = function (xhr, status, error) {
        alert("Error: " + xhr.status + " - " + error + " - " + URL);
    };

    URL += "/";

    var wecall = $.when(ajaxcall0(URL + "13E"), ajaxcall0(URL + "13S"), ajaxcall0(URL + "15"), ajaxcall0(URL + "16"));

    wecall.fail = errorq;
    wecall.done(function (reqd1, reqd2, reqd3, reqd4) {
        pass_var1= $("#xstring11").val();
        var items = "<option value=''></option>";
        $.each(reqd1[0], function (i, state) {
            if (state.Value == pass_var1)
                select_var1 = " selected ";
            items += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
            select_var1 = "";
        });
        $('#menup').html(items);
        //num = $("#xstring11").val();
        //$("#menup").val(num);

        pass_var1 = $("#xstring12").val();
        var items = "<option value=''></option>";
        $.each(reqd2[0], function (i, state) {
            if (state.Value == pass_var1)
                select_var1 = " selected ";
            items += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
            select_var1 = "";
        });
        $('#emenup').html(items);
        //num = $("#xstring12").val();
        //$("#emenup").val(num);

        pass_var1= $("#xstring6").val();
        pass_var2 = $("#xstring7").val();
        var items = "<option value=''></option>";
        var items1 = "<option value=''></option>";
        $.each(reqd3[0], function (i, state) {
            if (state.Value == pass_var1)
                select_var1 = " selected ";
            if (state.Value == pass_var2)
                select_var2 = " selected ";
            items += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
            items1 += "<option " + select_var2 + " value='" + state.Value + "'>" + state.Text + "</option>";
            select_var1 = "";
            select_var2 = "";
        });
        $("#staff1").html(items);
        $("#staff2").html(items1);
        for (var wctr = 0; wctr < 6; wctr++) {
            $("#ar_string2" + wctr.toString()).html(items);
            $("#ar_string3" + wctr.toString()).html(items);
            num = $("#xstring2" + wctr.toString()).val();
            $("#ar_string2" + wctr.toString()).val(num);
            num = $("#xstring3" + wctr.toString()).val();
            $("#ar_string3" + wctr.toString()).val(num);
        }
        //num = $("#xstring6").val();
        //$("#staff1").val(num);
        //num = $("#xstring7").val();
        //$("#staff2").val(num);
        //for (var wctr = 0; wctr < 10; wctr++) {
        //    num = $("#xstring2" + wctr.toString()).val();
        //    $("#ar_string2" + wctr.toString()).val(num);
        //    num = $("#xstring3" + wctr.toString()).val();
        //    $("#ar_string3" + wctr.toString()).val(num);
        //}

        var items = "<option value=''></option>";
        $.each(reqd4[0], function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
        });
        $('#ar_string40').html(items);
        $('#ar_string41').html(items);
        $('#ar_string42').html(items);
        $('#ar_string43').html(items);
        num = $("#xstring40").val();
        $("#ar_string40").val(num);
        num = $("#xstring41").val();
        $("#ar_string41").val(num);
        num = $("#xstring42").val();
        $("#ar_string42").val(num);
        num = $("#xstring43").val();
        $("#ar_string43").val(num);

    });

}

function snglsel() {
    // single input for group selection
    var URL = $('#loader1b').data('request-dailyb');

    var URLext = $('#xstring0').val();
    var errorq = function (xhr, status, error) {
        alert("Error: " + xhr.status + " - " + error + " - " + URL);
    };

    URL += "/";
    var wecall = $.when(ajaxcall0(URL + "00"), ajaxcall0(URL + "04" + URLext));

    wecall.fail = errorq;
    wecall.done(function (reqd1, reqd2) {
        pass_var1= $("#xstring0").val();
        var items = "<option value=''></option>";
        $.each(reqd1[0], function (i, state) {
            if (state.Value == pass_var1)
                select_var1 = " selected ";
            items += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
            select_var1 = "";
        });
        $('#group_list').html(items);
        //num = $("#xstring0").val();
        //$("#group_list").val(num);
        
        pass_var1 = $("#xcode").val();
        var items = "<option value=''></option>";
        $.each(reqd2[0], function (i, state) {
            if (state.Value == pass_var1)
                select_var1 = " selected ";
            items += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
            select_var1 = "";
        });
        $('#staff_number').html(items);
        //num = $("#xcode").val();
        //$("#staff_number").val(num);

    });
}

function dgnrp() {
    // report design -- menu,sort field
    var URL = $('#loader1b').data('request-dailyb');
    var errorq = function (xhr, status, error) {
        alert("Error: " + xhr.status + " - " + error + " - " + URL);
    };

    var urlext = $("#rtype1").val();

    URL += "/";
    var wecall = $.when(ajaxcall0(URL + "13E"), ajaxcall0(URL + "17" + urlext));

    wecall.fail = errorq;
    wecall.done(function (reqd1, reqd2) {
        pass_var1= $("#xstring3").val();
        var items = "<option value=''></option>";
        $.each(reqd1[0], function (i, state) {
            if (state.Value == pass_var1)
                select_var1 = " selected ";
            items += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
            select_var1 = "";
        });
        $('#menup').html(items);
        //num = $("#xstring3").val();
        //$("#menup").val(num);
        
        var items = "<option value=''></option>";
        $.each(reqd2[0], function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
        });
        for (var wctr = 0; wctr < 5; wctr++) {
            $("#ar_string0" + wctr.toString()).html(items);
            num = $("#xstring0" + wctr.toString()).val();
            $("#ar_string0" + wctr.toString()).val(num);
        }

    });

}

function dgnsd() {
    //stadard reports
    var URL = $('#loader1b').data('request-dailyb');
    var errorq = function (xhr, status, error) {
        alert("Error: " + xhr.status + " - " + error + " - " + URL);
    };

    var urlext = $("#rtype1").val();
    URL += "/";

    var wecall = $.when(ajaxcall0(URL + "13T"), ajaxcall0(URL + "17" + urlext), ajaxcall0(URL + "18"));

    wecall.fail = errorq;
    wecall.done(function (reqd1, reqd2, reqd3) {
        pass_var1 = $("#xstring3").val();
        var items = "<option value=''></option>";
        $.each(reqd1[0], function (i, state) {
            if (state.Value == pass_var1)
                select_var1 = " selected ";
            items += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
            select_var1 = "";
        });
        $('#menup').html(items);
        //num = $("#xstring3").val();
        //$("#menup").val(num);

        var items = "<option value=''></option>";
        $.each(reqd2[0], function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
        });
        for (var wctr = 0; wctr < 5; wctr++) {
            $("#ar_string0" + wctr.toString()).html(items);
            num = $("#xstring0" + wctr.toString()).val();
            $("#ar_string0" + wctr.toString()).val(num);
        }
        //for (var wctr = 0; wctr < 5; wctr++) {
        //    num = $("#xstring0" + wctr.toString()).val();
        //    $("#ar_string0" + wctr.toString()).val(num);
        //}

        var items = "<option value=''></option>";
        $.each(reqd3[0], function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
        });
        for (var wctr = 0; wctr < 5; wctr++) {
            $("#ar_string6" + wctr.toString()).html(items);
            num = $("#xstring6" + wctr.toString()).val();
            $("#ar_string6" + wctr.toString()).val(num);
        }
        //for (var wctr = 0; wctr < 5; wctr++) {
        //    num = $("#xstring6" + wctr.toString()).val();
        //    $("#ar_string6" + wctr.toString()).val(num);
        //}

    });

}

function scrtrf() {
 // trasfer screen
    var URL = $('#loader1b').data('request-dailyb');
    var urlext = $("#rtype1").val();
    URL += "/";
    URL = URL + "19" + urlext;
    
    $.when(ajaxcall0(URL)).done(function (reqd1) {
        pass_var1 = $("#xstring1").val();
        var items = "<option value=''></option>";
        var items1 = "<option value=''></option>";
        $.each(reqd1, function (i, state) {
                if (state.Value == pass_var1)
                    select_var1 = " selected ";
                items1 += "<option " + select_var1 + " value='" + state.Value + "'>" + state.Text + "</option>";
                select_var1 = "";
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            });
            $('#tselect').html(items);
            $('#screen_name').html(items1);
            //var num = $("#xstring1").val();
            //$("#screen_name").val(num);
    });

}

function parep(rtype1) {
    // parameter screen
    var URL = $('#loader1b').data('request-dailyb');
    URL += "/";
    URL = URL + "26" +rtype1;

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
            $('#pselect').html(items);
        },
    });

}

function atpay()
{
    if ($("#datmode").val() !="E")
        snglsel();

    //t1pay();

}

function t1pay()
{
    var payrun = $("#snumber").val();
    if (payrun == "")
        payrun = $("#xcode").val();
    
    var URL = $('#loader1b').data('request-dailyb');
    var errorq = function (xhr, status, error) {
        alert("Error: " + xhr.status + " - " + error + " - " + URL);
    };

    URL = $.trim(URL);
    URL += "/";
    URL = URL + "123" + payrun;

    var wecall = $.when(ajaxcall0(URL));

    wecall.fail = errorq;
    wecall.done(function (reqd1) {
        var items = '';
        $.each(reqd1, function (i, state) {
            if (state.Value == 0) {
                k1 = state.Text;
                py = k1.substr(0, 4);
                pm = k1.substr(4, 2);
                py1 = Number(py);
            }
            else
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
        });
        $(".trans_period_mm").html(items);

        num = $("#xperdfm").val();
        $("#period_from_mm").val(num);
        num = $("#xperdfy").val();
        $("#period_from_yy").val(num);
        num = $("#xperdtm").val();
        $("#period_to_mm").val(num);
        num = $("#xperdty").val();
        $("#period_to_yy").val(num);
        num = $("#xperdm").val();
        $("#period_pay_mm").val(num);
        num = $("#xperdy").val();
        $("#period_pay_yy").val(num);
     });

}

function t1men(menu_type) {

    var URL = $('#loader1b').data('request-dailyb');
    var errorq = function (xhr, status, error) {
        alert("Error: " + xhr.status + " - " + error + " - " + URL);
    };

    URL = $.trim(URL);
    URL += "/";
    URL = URL + "13" + menu_type;

    var wecall = $.when(ajaxcall0(URL));

    wecall.fail = errorq;
    wecall.done(function (reqd1) {
        var items = '<option></option>';
        $.each(reqd1, function (i, state) {
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
        });
        
        if (menu_type=="E")
            $("#mmenu").html(items);

        if (menu_type == "S")
            $("#emenu").html(items);

        if (menu_type == "T")
            $("#mmenu").html(items);

        num = $("#xmenu").val();
        $("#mmenu").val(num);
        num = $("#xsmenu").val();
        $("#emenu").val(num);
    });

}

function scexp() {
    //expense templ 
    var URL = $('#loader1b').data('request-dailyb');
    URL += "/";

        $.ajax({
            type: "Post",
            url: URL + "27",
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = "<option value=''></option>";
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $('#exp' + applist1).html(items);
            }
        });

}

function users_opt()
{
    pass_var1 = $("#xstring11").val();
    pass_var2 = $("#xstring12").val();
    var fd1 = $("#selectid1").val();
    if (fd1 != "")
        select_range(fd1, "1");

    pass_var1 = $("#xstring21").val();
    pass_var2 = $("#xstring22").val();
    var fd1 = $("#selectid2").val();
    if (fd1 != "")
        select_range(fd1, "2");

    pass_var1 = $("#xstring31").val();
    pass_var2 = $("#xstring32").val();
    var fd1 = $("#selectid3").val();
    if (fd1 != "")
        select_range(fd1, "3");


    pass_var1 = $("#xstring41").val();
    pass_var2 = $("#xstring42").val();
    var fd1 = $("#selectid4").val();
    if (fd1 != "")
        select_range(fd1, "4");


    pass_var1 = $("#xstring51").val();
    pass_var2 = $("#xstring52").val();
    var fd1 = $("#selectid5").val();
    if (fd1 != "")
        select_range(fd1, "5");


    pass_var1 = $("#xstring61").val();
    pass_var2 = $("#xstring62").val();
    var fd1 = $("#selectid6").val();
    if (fd1 != "")
        select_range(fd1, "6");



}