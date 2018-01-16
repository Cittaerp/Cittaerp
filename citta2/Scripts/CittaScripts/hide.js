/// <reference path="hide.js" />
/// <reference path="hide.js" />
$(document).ready(function () {

    $(".datet").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat : "dd/mm/yy"
    });

 

    $("#display").click(function () {
        $("#head").show();
        $("#details").hide("fast");
    });


    $("#direct").click(function () {
        $("#details").show();
        $("#head").hide("fast");
    });

    $("#submit").hide();

    $("#hide_details").click(function () {
        $("#submit").show();
        $("#hide_details").hide();
    });

    $("#submit").click(function () {
        $("#details").show();
        $("#submit").hide();
        $("#hide_details").show();

    });
  

    $("table.myTable1").DataTable({
        destroy: true,
        order: [],
        scrollX: false,
        fixedHeader: true,
        colReorder: true,
        stateSave: true,
        iDisplayLength: 25,
        lengthMenu: [[25, 50, 75, 100, -1], [25, 50, 75, 100, "All"]],
        dom: '<"clear">l<"centered"i><"dtwrapper"frtip>',
        "columnDefs": [{ "targets": [0], "orderable": false }]

    });

   // $(".myradio input[type=radio]").checkboxradio({ icon: false });

    $(".myradio input[type=radio],.mycheckq input[type=checkbox]").checkboxradio({
        icon: false
    });


    $("input[type=checkbox]").click(function () {
        var lblid = $(this).attr('id');
        if (!(lblid == null || lblid == "")) {
            var pos_yes = lblid.indexOf("_yesno");
            if (pos_yes >= 0) {
                var ab = $("label[for=" + lblid + "]").text().trim();
                if (ab == "On")
                    $("label[for=" + lblid + "]").text("Off");
                else
                    $("label[for=" + lblid + "]").text("On");
            }
        }

    })

   

    $(".year").datepicker({
        changeYear: true,
        showButtonPanel: true,
        dateFormat: 'yy',
        onClose: function (dateText, inst) {
            var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            $(this).datepicker('setDate', new Date(year, 1));
        }
        });

        $(".year").focus(function () {
            $(".ui-datepicker-month").hide();
        });
    
     
        $("#n").click(function () {
            var pal = "Save your Record and proceed to Address Maintainance Table through the Listing Screen"
            $("#deladd_alert").html(pal);
        })
        $("#y").click(function () {
            $("#deladd_alert").hide();
        })


    $("#yes").click(function () {
        clickyeshide();
        $("#click").show('slow');

    })
    $("#no").click(function () {
        clickyeshide();

    })

    $("#not").click(function () {
        clickyeshide();

    })
    $("#noc").click(function () {
        clickyeshide();

    })


    function clickyeshide() {
        $("#click").hide('slow');
    }

    $("#yes1").click(function () {
        click1yeshide();
        $("#click1").show('slow');

    })
    $("#no1").click(function () {
        click1yeshide();

    })

    function click1yeshide() {
        $("#click1").hide('slow');
    }

    $("#yes2").click(function () {
        click2yeshide();
        $("#click2").show('slow');

    })
    $("#no2").click(function () {
        click2yeshide();

    })

    function click2yeshide() {
        $("#click2").hide('slow');
    }

    $("#yes3").click(function () {
        click3yeshide();
        $("#click3").show('slow');

    })
    $("#no3").click(function () {
        click3yeshide();

    })

    function click3yeshide() {
        $("#click3").hide('slow');
    }

    $("#yes4").click(function () {
        click4yeshide();
        $("#click4").show('slow');

    })
    $("#no4").click(function () {
        click4yeshide();
    })

    function click4yeshide() {
        $("#click4").hide('slow');
    }


    $("#yes5").click(function () {
        $("#click5").show('slow');

    })
    $("#no5").click(function () {
        $("#click5").hide('slow');

    })


    $("#yess").click(function () {
        click1yeshide();
        click2yeshide();
        $("#click").show('slow');
       

    })
    $("#noo").click(function () {
        clickyeshide();
        click2yeshide();
        $("#click1").show('slow');

    })
    $("#noy").click(function () {
        clickyeshide();
        click1yeshide();
        $("#click2").show('slow');

    })
    $("#noy2").click(function () {
        click2yeshide();
        $("#click").show('slow');
        $("#click1").show('slow');



    })

    $("#now").click(function () {
        $("#click6").show('slow');
        $("#click7").show('slow');
        $("#click8").show('slow');

    })
    $(".now1").click(function () {
        $("#click7").hide('slow');
        $("#click6").hide('slow');
        $("#click8").hide('slow');
    

    })
 
    $(".now2").click(function () {
        $("#click7").hide('slow');
        $("#click6").hide('slow');
        $("#click8").hide('slow');


    })

    $(".now3").click(function () {

        $("#click10").hide('slow');
        $("#click11").hide('slow');
    })
   
   
    $("#now6").click(function () {
        click10hide();
        $("#click10").show('slow');

    })

    $("#hide_details").click(function () {
        $("#details").hide();
    })
    $("#submit").click(function () {
        $("#details").show();
    })
    
    // delete in edit
    $("#id_delete").click(function () {
        var $form = $(this).closest('form');
        bootbox.confirm("Are you sure you want to delete?", function (a) {
            if (a) {
                $("#id_xhrt").val("D");
                alert("johny was here");
                alert($form);
                $form.trigger("submit");
            }
            else {
                $("#id_xhrt").val("X");
            }
        });
    });


 
    $(".myTable1").on('click', '.deleteitem', function () {
        var ispan = $(this).closest("td");
        var keyg = ispan.children("[data-keyg]").data('keyg');
        bootbox.confirm("Are you sure you want to delete?", function (a) {
            if (a) {

                delete_ajax_rtn(keyg);
                location.reload();
            }
            else {
                $("#id_xhrt").val("X");
            }
        });
    });

    function delete_ajax_rtn (keyg)
    {
        
        var URL = $('#loader2').data('request-ajax2');
        if (isNaN(keyg)) {
            keyg = keyg.replace(/&/g, "[]");
        }
        var URLext = keyg;
        var Acct = " "
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL + "/" + URLext;
        $.ajax({
            type: "Post",
            async:false,
            url: URL,
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    items = state.Text;
                    if (state.Value == "1") {
                            alert(state.Text);
                    }
                });
            }
           
        });
       // location.reload();
    }

    $(".myTable1").on('click', '.voiditem', function () {
        var ispan = $(this).closest("td");
        var keyg = ispan.children("[data-keyg]").data('keyg');
        bootbox.confirm("Are you sure you want to void?", function (a) {
            if (a) {

                void_ajax_rtn(keyg);
                location.reload();
            }
            else {
                $("#id_xhrt").val("v");
            }
        });
    });

    function void_ajax_rtn(keyg) {

        var URL = $('#loader21').data('request-ajax21');
        if (isNaN(keyg)) {
            keyg = keyg.replace(/&/g, "[]");
        }
        var URLext = keyg;
        var Acct = " "
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL + "/" + URLext;
        $.ajax({
            type: "Post",
            async: false,
            url: URL,
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    items = state.Text;
                    if (state.Value == "1") {
                        alert(state.Text);
                    }
                });
            }

        });
        // location.reload();
    }

    $(".myTable1").on('click', '.confirmitem', function () {
        var ispan = $(this).closest("td");
        var keyg = ispan.children("[data-keyg]").data('keyg');
        bootbox.confirm("Are you sure you want to Confirm Order?", function (a) {
            if (a) {

                conform_ajax_rtn(keyg);
                location.reload();
            }
            else {
                $("#id_xhrt").val("R");
            }
        });
    });

    function confirm_ajax_rtn(keyg) {

        var URL = $('#loader20').data('request-ajax20');
        if (isNaN(keyg)) {
            keyg = keyg.replace(/&/g, "[]");
        }
        var URLext = keyg;
        var Acct = " "
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL + "/" + URLext;
        $.ajax({
            type: "Post",
            async: false,
            url: URL,
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    items = state.Text;
                    if (state.Value == "1") {
                        alert(state.Text);
                    }
                });
            }

        });
        // location.reload();
    }

    $(".myTable1").on('click', '.activateitem', function () {
        var ispan = $(this).closest("td");
        var keyg = ispan.children("[data-keyg]").data('keyg');
        bootbox.confirm("Are you sure you want to Reactivate?", function (a) {
            if (a) {

                activate_ajax_rtn(keyg);
                location.reload();
            }
            else {
                $("#id_xhrt").val("R");
            }
        });
    });

    function activate_ajax_rtn(keyg) {

        var URL = $('#loader20').data('request-ajax20');
        if (isNaN(keyg)) {
            keyg = keyg.replace(/&/g, "[]");
        }
        var URLext = keyg;
        var Acct = " "
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL + "/" + URLext;
        $.ajax({
            type: "Post",
            async: false,
            url: URL,
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    items = state.Text;
                    if (state.Value == "1") {
                        alert(state.Text);
                    }
                });
            }

        });
        // location.reload();
    }

    $(".nexttab").click(function () {
        $(".nav-tabs > .active").next("li").find("a").trigger("click");
    });

    $(".prevtab").click(function () {
        $(".nav-tabs > .active").prev("li").find("a").trigger("click");
    });

    $("#tab2cty").change(function () {
        var cty = $("#tab2cty").val();
        var items = stateval2(cty);
        $("#tab2state").html(items);
    })
    $("#tab3cty").change(function () {
        var cty = $("#tab3cty").val();
        var items = stateval2(cty);
        $("#tab3state").html(items);
    })

    function stateval2(county) {
        var URL = $('#loader1').data('request-ajax1');
        var URLext = county
        var items = '';
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        URL = URL + "/" + URLext;
        $.ajax({
            type: "Post",
            url: URL,
            async : false,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                
            }
        });
        return items;
    };

    $("#acctype").change(function () {
        var URL = $('#loader3').data('request-ajax3');
        var URLext = $('#acctype').val();
        URL = $.trim(URL);
        
        URLext = $.trim(URLext);
        URL = URL + "/" + URLext;
        $.ajax({
            type: "Post",
            url: URL,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    items = state.Text;
                });
                $("#accat").val(items);
            }
        });
    });

    $("#id_atype").change(function () {
            var URL = $('#loader4').data('request-ajax4');
            var URLext = $('#id_atype').val();
            URL = $.trim(URL);
            URLext = $.trim(URLext);
            
            $.ajax({
                type: "Post",
                url: URL,
                data: {typecode: "02",id: URLext },
                error: function (xhr, status, error) {
                    alert("Error: " + xhr.status + " - " + error + " - " + URL);
                },
                success: function (data) {
                    var items = "<option value=''></option>";
                    $.each(data, function (i, state) {
                        items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                    });
                    $("#id_acode").html(items);
                }
            });
        });

    $("#id_acode").change(function () {
        var items = '';
        var shw_rat = "N"
        var rate_amt = "";
        var cur_code = "";
        var curen_name = "";
        var editexch = $("#id_editexch").val();
        var exhrate = "0";
        var ctr = 0;
        var URL = $('#loader4').data('request-ajax4');
        var URLext = $('#id_acode').val();
        var atype = $('#id_atype').val();
        var urtdate = $('#id_transdate').val();
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        
        $.ajax({
            type: "Post",
            url: URL,
            async: false,
            data: {typecode:"01", ac_code1: URLext, id: atype,tdate: urtdate },
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                $.each(data, function (i, state) {
                    if (ctr == 0) {
                        cur_code = state.Value;
                        cur_name = state.Text;

                        //if (state.Value == "1")
                        //    items = state.Text;
                        //else if (state.Value == "2")
                        //    shw_rat = state.Text;
                        //else if (state.Value == "3")
                        //    rate_amt = state.Text;
                        //else
                        //    curen_name = state.Text;
                        ctr++;
                    }
                    else {
                        exhrate = state.Text;
                       }
                });
                $("#curren_name").val(cur_name);
                $("#id_hcuren").val(cur_code);
                $(".cls_rate2").val(exhrate);

            }
        })
        if (editexch == "N" && Number(exhrate) == 0) {
            alert("Exchange rate can not be zero")
        }

       });

    $("#id_amt").change(function () {
            var items = '';
            var URLext = $('#id_amt').val();
            var atype = $('#id_rate').val();
            var amt = parseFloat((URLext).replace(/,/g, '')) * parseFloat((atype).replace(/,/g, ''));
            $("#id_bamt").val(amt);
    });

    $("#edit_show").click(function () {
      
        $("#id_createh").css({ "display":"none"});
        $("#id_edith").show();
       
    });


    function clickhide() {
        $("#click1").hide('slow');
        $("#click2").hide('slow');
        $("#click3").hide('slow');
        $("#click").hide('slow');

    }

    $("#id_cust_code").change(function () {
        var items = '';
        var shw_rat = "N"
        var rate_amt = "";
        var curen_name = "";
        var item = '';
        var URL = $('#loader5').data('request-ajax5');
        var URLext = $('#id_cust_code').val();
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        $.ajax({
            type: "Post",
            url: URL,
            data: { cust_code: URLext },
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                $.each(data, function (i, state) {
                    if (state.Value == "1")
                        items = state.Text;
                    else if (state.Value == "2")
                        shw_rat = state.Text;
                    else if (state.Value == "3")
                        rate_amt = state.Text;
                    else if (state.Value == "qty")
                        item = state.Text;
                    else if (state.Value == "4")
                        curen_name = state.Text;
                });
            
        $("#curren_name").val(curen_name);
        $("#id_hcuren").val(items);
        $("#id_rate").val(rate_amt);
        $("#fx_flag").val(shw_rat);
        $("#id_srep").html(item);
            }

        })

        if (shw_rat == "Y") {
            $("#changer").show();
        }
        else {
            $("#changer").hide();
        }
    });

    $(".get_currency").change(function () {
        var editexch = $("#id_editexch").val();
        var exhrate = "0";
        var cur_code = "";
        var cur_name = "";
        var ctr = 0;

        var URL = $('#loader5').data('request-ajax5');
        var URLext = $('#get_currency1').val();
        var urtdate = $('#id_transdate').val();
        URL = $.trim(URL);
        URLext = $.trim(URLext);

        $.ajax({
            type: "Post",
            async: false,
            url: URL,
            data: { cust_code: URLext, tdate: urtdate },
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                $.each(data, function (i, state) {
                    if (ctr == 0) {
                        cur_code = state.Value;
                        cur_name = state.Text;
                        ctr++;
                    }
                    else
                    {
                        exhrate = state.Text;
                    }
                });

                $("#curren_name").val(cur_name);
                $("#id_hcuren").val(cur_code);
                $(".cls_rate2").val(exhrate);

            }

        })

        if (editexch == "N" && Number(exhrate) == 0) {
            alert("Exchange rate can not be zero")
        }


    });

  

    $(".id_cont").change(function () {
        var items = ''; var nsvl = ''; var spec1 = '';
        var item = ''; var bal = ''; var outbal = '';
        var damt = ''; var spec = ''; var qtty = '';
        var mamt = ''; var curen = ''; var promo_name = '';
        var npyt = ''; var curen1 = '';
        var URL = $('#loader5').data('request-ajax5');
        var URLext = $('#id_contr').val();
        //var dep = $('#id_depamt').val();
        //var prc = $('#id_price').val();
        var qty = $('#id_qty').val();
        URL = $.trim(URL);
        URLext = $.trim(URLext);
        $.ajax({
            type: "Post",
            url: URL,
            data: { item_code: URLext, qty:qty },
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                $.each(data, function (i, state) {
                    if (state.Value == "1")
                        items = (state.Text);
                    else if (state.Value == "2")
                        item = (state.Text);
                    else if (state.Value == "3")
                        damt = (state.Text);
                    else if (state.Value == "4")
                        mamt = (state.Text);
                    else if (state.Value == "5")
                        npyt = (state.Text);
                    else if (state.Value == "6")
                        nsvl = (state.Text);
                    else if (state.Value == "7")
                        bal = (state.Text);
                    else if (state.Value == "8")
                        spec = (state.Text);
                    else if (state.Value == "9")
                        curen = (state.Text);
                    else if (state.Value == "10")
                        curen1 = (state.Text);
                    else if (state.Value == "11")
                        spec1 = (state.Text);
                    else if (state.Value == "12")
                        outbal = state.Text;
                    else if (state.Value == "13")
                        qtty = state.Text;
                    else if (state.Value == "14")
                        promo_name = state.Text;
                });
                $("#id_scom").val(items);
                $("#id_price").val(item);
                $("#id_damt").val(damt);
                $("#id_mamt").val(mamt);
                $("#id_npyt").val(npyt);
                $("#id_sval").val(nsvl);
                $("#id_bal").val(bal);
                $("#id_spec").val(spec);
                $("#curen").val(curen);
                $("#id_scom1").val(items);
                $("#id_price1").val(item);
                $("#id_damt1").val(damt);
                $("#id_mamt1").val(mamt);
                $("#id_npyt1").val(npyt);
                $("#id_sval1").val(item);
                $("#id_bal1").val(bal);
                $("#id_spec1").val(spec1);
                $("#curen1").val(curen1);
                $("#id_outbal").val(outbal);
                $("#id_qtty").val(qtty);
                $("#id_promo_name").val(promo_name);
            }

        })

    });

 
    $(".range_calc").change(function () {
        var getname1 = $(this).attr('data-pos');
        var linnum = Number(getname1) + 1;
        var valper = $("#id_rangecalto" + getname1).val();
        var pernum = Number(valper) + 1;
        $("#id_rangecalfrm" + linnum).val(pernum);
    });


    $(function ($) {
        $(".numformat").autoNumeric('init', { mDec: 2 });
        $(".myTable1.numformat").autoNumeric('init', { mDec: 2 });
    });

});

