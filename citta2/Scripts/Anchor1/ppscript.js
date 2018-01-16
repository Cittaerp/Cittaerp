$(function () {

//--allowances
    $("#al_code").change(function () {
        var URL = $('#loader1').data('request-daily');
        URL = $.trim(URL);
        var URLext = $('#al_code').val();
        var URLext1 = $('#snumber').val();
        URLext = $.trim(URLext);
        URL = URL + '02' + URLext + "[]" + URLext1;
        $.ajax({
            type: "Post",
            url: URL,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    if (state.Value == "temp") {
                        $("#al_bal").html(state.Text);
                    }
                    else {
                        if (state.Value == "D") {
                            $("#al_staff").html("Staff");
                            $("#al_tax").html("Company");
                        }
                        else {
                            $("#al_staff").html("Allowance");
                            $("#al_tax").html("Taxable");
                        }
                    }
                });
            }
        });

    });

// loan create
    $("#row_rate").hide();

    $("#ln_indicator").change(function () {
        validate_loan();
    });

    $("#ln_code").change(function () {
        validate_loan();
    });

    function validate_loan() {
        var lind = $("#ln_indicator").val();
        if (lind == "N") {
            $("#row_amount").show();
            $("#row_tenor").show();
        }
        else if (lind == "R") {
            $("#row_amount").show();
            $("#row_tenor").hide();
        }
        else if (lind == "T") {
            $("#row_amount").hide();
            $("#row_tenor").hide();
        }
        else if (lind == "A") {
            $("#row_amount").show();
            $("#row_tenor").show();
        }

        var URL = $('#loader1').data('request-daily');
        URL = $.trim(URL);
        var URLext = $('#snumber').val();
        var URLext1 = $('#ln_code').val();
        URLext = $.trim(URLext);
        URLext1 = $.trim(URLext1);
        URL = URL + URLext1 + '[]' + URLext;
        $.ajax({
            type: "Post",
            url: URL,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    if (state.Value == "rate") {
                        if (state.Text == "N")
                            $("#row_rate").hide();
                        else
                            $("#row_rate").show();
                    }
                    if (state.Value == "bal")
                        $("#bal").html(state.Text);

                });
            }
        });


    };

// part payment

    hideline();

    $("#indicatora").change(function () {
        var URL = $('#loader1').data('request-daily');
        URL = $.trim(URL);
        var URLext = $('#indicatora').val();
        $(".group-pay").hide();
        if (URLext == "O")
            $(".group-pay").show();

        URLext = $.trim(URLext);
        URL = URL + '02' + URLext;
        $.ajax({
            type: "Post",
            url: URL,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = "<option value=''>Select Transaction Name</option>";
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                    // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
                });
                $('#codea').html(items);
            }
        });

    });

    $("#date_range").change(function () {
        hideline();
    });

    function hideline() {
        var drange = $("#date_range").val();
        if (drange == "P") {
            $("#row_date").hide();
            $("#row_days").hide();
            $("#row_period").show();
        }
        else if (drange == "D") {
            $("#row_date").show();
            $("#row_days").hide();
            $("#row_period").hide();
        }
        else {
            $("#row_date").hide();
            $("#row_days").show();
            $("#row_period").hide();
        }
    };

    // promotion step
    $('#prom_category').change(function () {
        var URL = $('#loader1').data('request-daily');
        var URLext = $('#prom_category').val();
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL + 'sp' + URLext;
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
                $('#prom_step').html(items);
            }
        });
    });

    $('#prom_step').change(function () {
        var URL = $('#loader1').data('request-daily');
        var URLext = $('#prom_category').val();
        var URL2 = $('#prom_step').val();
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL + 'as' + URLext + "[]" + URL2;
        $.ajax({
            type: "Post",
            url: URL,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    items += state.Value;
                });
                $('#prom_salary').val(items);
            }
        });
    });

    // tabular report
    $("#trans_type").change(function () {
        var URL = '@Url.Action("Daily3List") ';
        var URL = $('#loader4').data('request-daily4');
        URL = $.trim(URL);
        var URLext = $('#trans_type').val();
        URLext = $.trim(URLext);
        URL = URL + URLext;
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
                $(".radclass").html(items);
            }
        });

    });


    // staff record
    $("#payment input[name='payment_mode']").click(function () {
        var radiob = $("input[name='payment_mode']:checked").val();
        if (radiob == "B") {
            $("#bank").show();
            $("#account").show();
        }
        else {
            $("#bank").hide();
            $("#account").hide();
        };
    });

    $('#nationality').change(function () {
        var URL = $('#loader1').data('request-daily');
        var URLext = $('#nationality').val();
        //var oldstate = $('state').val();
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL + 'st' + URLext;
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
                items += "<option value=''>None</option>";
                $('#state').html(items);

            }
        });
    });

    $('#state').change(function () {
        var URL = $('#loader1').data('request-daily');
        var URLext = $('#state').val();
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL + 'lg' + URLext;
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
                items += "<option value=''>None</option>";
                $('#lga').html(items);

            }
        });
    });



});

