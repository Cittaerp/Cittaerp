$(function () {

    //transactions
    $("#staff_number").change(function () {
        $("#snumber").val($("#staff_number").val())
    });

    $(".lgtime").click(function () {
        lgshow();
    });


    $("#cbtnv").click(function () {
        vline = "X";
        $("#id_xhrt").val(vline);
    });

    $(".cbtnv").click(function () {
        vline = "X";
        $("#id_xhrt").val(vline);
    });

    //initial transaction
    $("#init_extract").click(function () {
        var item1 = opt_check();
        $('#initplace').html(item1);
        table_viewnnp();
    });

    $("#app_number").change(function () {
        $("#appnumber").val($("#app_number").val())
    });

    // advice slip layout for document
    $("#advice_print input[name='ws_string4']").click(function () {
        var doctype = $("input[name='ws_string4']:checked").val();
        var URL = $('#loader1').data('request-daily');
        URL = $.trim(URL);
        doctype = $.trim(doctype);
        URL = URL + '02' + doctype;
        $.ajax({
            type: "Post",
            url: URL,
            async: false,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = "<option value=''>Select Document</option>";
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                    // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
                });
                $('#advice_document').html(items);
            }
        });

    });

    // for add lines, default show
    var ctr = $("#rcount").val();
    show_lines(ctr);

    ctr = $("#rcount1").val();
    show_lines1(ctr);

    $("#btone").click(function () {

        var rc1 = $("#rcount").val();
        rc1++;
        show_lines(rc1);
        $("#rcount").val(rc1);

    });

    $("#btone1").click(function () {

        var rc1 = $("#rcount1").val();
        rc1++;
        show_lines1(rc1);
        $("#rcount").val(rc1);

    });

    // tax computation for expense
    $("#taxper").change(function () {
        var doctype = $("#taxper").val();
        var URL = $('#loader1').data('request-daily');
        URL = URL + '02' + doctype;
        $.ajax({
            type: "Post",
            url: URL,
            async: false,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = "";
                $.each(data, function (i, state) {
                    if (state.Value == "tax")
                        $("#taxamt").val(state.Text);

                    if (state.Value == "bal")
                        $("#taxbal").val(state.Text);
                });
            }
        });

    });

    $("#doc_printbtn").click(function () {
        var URL = $('#loader1p').data('request-daily');
        $.ajax({
            type: "Post",
            url: URL,
            async: false,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
        });

        var l3 = $("#locat").val() + "RepScrn/View1/";
        var mywin = window.open(l3, '_blank');

    })

    function prt_document(det1) {
        var URL = $('#loader1p').data('request-daily');
        $.ajax({
            type: "Post",
            url: URL + det1,
            async: false,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
        });

        var l3 = $("#locat").val() + "RepScrn/View1/";
        var mywin = window.open(l3, '_blank');
        //window.open(l3, '_blank', 'left=100,top=50,width=1000,height=650,toolbar=0,resizable=1,scrollbars=1');

    }


});

function opt_check() {
    var items = "";
    var URL = $('#loader1').data('request-daily');
    URL = $.trim(URL);

    var allow = $("#allow:checked").val();
    var daily = $("#daily:checked").val();
    var over = $("#over:checked").val();
    var loan = $("#loan:checked").val();
    var partp = $("#partp:checked").val();
    var prom = $("#prom:checked").val();
    var sepa = $("#sepa:checked").val();
    var tax = $("#tax:checked").val();
    var dinput = $("#dinput:checked").val();
    var snumber = $("#snumber").val();
    var opt_str;
    if (typeof allow === 'undefined')
        allow = " ";
    if (typeof daily === 'undefined')
        daily = " ";
    if (typeof over === 'undefined')
        over = " ";
    if (typeof loan === 'undefined')
        loan = " ";
    if (typeof partp === 'undefined')
        partp = " ";
    if (typeof prom === 'undefined')
        prom = " ";
    if (typeof sepa === 'undefined')
        sepa = " ";
    if (typeof tax === 'undefined')
        tax = " ";
    if (typeof dinput === 'undefined')
        dinput = " ";
    opt_str = allow + daily + over + loan + partp + prom + sepa + tax + dinput + snumber;

    URL = URL + '01' + opt_str;
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
            });
        }
    });

    return items;
};

function lgshow1() {
// spinner for forms after validation is done and okay
    var result2 = $(".valshow").closest('form');
    var result = result2.valid();
    if (result) {
        $("#loading").fadeIn();
        var opts = {
            lines: 12, // The number of lines to draw
            length: 12, // The length of each line
            width: 4, // The line thickness
            radius: 10, // The radius of the inner circle
            color: '#000', // #rgb or #rrggbb
            speed: 1, // Rounds per second
            trail: 60, // Afterglow percentage
            shadow: false, // Whether to render a shadow
            hwaccel: false // Whether to use hardware acceleration
        };
        var target = document.getElementById('loading');
        var spinner = new Spinner(opts).spin(target);
        $(target).data('spinner', spinner);
    }
}

function lgshow() {
    $("#loading").fadeIn();
    var opts = {
        lines: 12, // The number of lines to draw
        length: 12, // The length of each line
        width: 4, // The line thickness
        radius: 10, // The radius of the inner circle
        color: '#000', // #rgb or #rrggbb
        speed: 1, // Rounds per second
        trail: 60, // Afterglow percentage
        shadow: false, // Whether to render a shadow
        hwaccel: false // Whether to use hardware acceleration
    };
    var target = document.getElementById('loading');
    var spinner = new Spinner(opts).spin(target);
    $(target).data('spinner', spinner);
}

function lgstop() {
    $('#loading').data('spinner').stop();
    $("#loading").fadeOut();

}

function show_lines(ctr1) {
    var maxc = $("#rmaxcount").val();
    var ctr3 = parseInt(maxc, 10);
    var ctr2 = parseInt(ctr1, 10);
    if (!(isNaN(ctr3) || isNaN(ctr2))) {
        var q = "#query" + ctr2;
        $(q).show();
        ctr2++;
        for (i = ctr2; i < ctr3; i++) {
            var q = "#query" + i;
            $(q).hide();
        }
        if (ctr2 == ctr3) {
            $("#btnline").hide();
        }
    }
}

function show_lines1(ctr1) {
    var ctr2 = parseInt(ctr1, 10);
    var q = "#querya" + ctr2;
    $(q).show();
    ctr2++;
    for (i = ctr2; i < 20; i++) {
        var q = "#querya" + i;
        $(q).hide();
    }
    if (ctr2 == 20) {
        $("#btnline1").hide();
    }

}

function select_optionup() {
    var URL = $('#loader1').data('request-daily');
    $.ajax({
        type: "Post",
        url: URL + "optsel",
        error: function (xhr, status, error) {
            alert("Error: " + xhr.status + " - " + error + " - " + URL);
        },
        success: function (data) {
            var items = "<option value=''></option>";
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
        }
    });
}

function mastercheck() {
    $("table.dataprintv .chkcheckitem").prop('checked', $("table.dataprintv #chkCheckAll").prop('checked'));
};

