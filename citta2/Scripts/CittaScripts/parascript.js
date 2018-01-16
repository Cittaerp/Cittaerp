$(document).ready(function () {

  
    $(".dis_percentt").change(function () {
        var getname1 = $(this).attr('data-pos');
        var valper = $("#sdisper" + getname1).val();
        var valamt = $("#sdisamt" + getname1).val();
        var pernum = Number(valper);
        var amtnum = Number(valamt);
        var linnum = Number(getname1) + 1;
        if (pernum > 0 && amtnum > 0) {
            alert("Line" + linnum + " is Invalid");
        }
    });

    $(".range_calc").change(function () {
        var getname1 = $(this).attr('data-pos');
        var linnum = Number(getname1) + 1;
        var valper = $("#id_rangecalto" + getname1).val();
        var pernum = Number(valper) + 1;
        $("#id_rangecalfrm" + linnum).val(pernum);
    });


    $("#id_steppeddiscount").click(function () {
        var a = $(".fcheck").val();
        var b = $(".pcheck").val();
        if (a != 0) {
            alert("Flat Discount is filled already");
        }
        if (b != 0) {
            alert("Promotion is filled already");
        }
        $("#click").hide('slow');
        $("#click2").hide('slow');
        $("#click1").show('slow');
    });
    $("#id_flatdiscount").click(function () {
        var a = $(".range_cal").val();
        var c = $(".dis_percentt").val();
        var b = $(".pcheck").val();
        if ((a != 0) || (c != 0)) {
            alert("Step Discount is filled already");
        }
        if (b != 0) {
            alert("Promotion is filled already");
        }
        $("#click").show('slow');
        $("#click2").hide('slow');
        $("#click1").hide('slow');

    });
    $("#id_promotion").click(function () {
        var a = $(".fcheck").val();
        var b = $(".range_cal").val();
        var c = $(".dis_percentt").val();
        if (a != 0) {
            alert("Flat Discount is filled already");
        }
        if ((b != 0) || (c != 0)) {
            alert("Step Discount is filled already");
        }
        $("#click").hide('slow');
        $("#click2").show('slow');
        $("#click1").hide('slow');
    });


    $("#id_num_inst").change(function () {
        var items = '';
        var URLext = $('#id_num_inst').val();
        var atype = $('#price0').val();
        var amt = parseFloat((URLext).replace(/,/g, '')) * parseFloat((atype).replace(/,/g, ''));
        $("#id_inst_amt").val(amt);
    });
 
    $("#id_qtybased").click(function () {
        $("#shw_gift").hide();
        $("#shw_qty").show();
        $("#shw_flatgift").hide();

    });

    $("#id_gift").click(function () {
        $("#shw_gift").show();
        $("#shw_qty").hide();
        $("#shw_flatgift").hide();

    });

    $("#id_flatgift").click(function () {
        $("#shw_gift").show();
        $("#shw_qty").hide();
        $("#shw_flatgift").show();

    });
    //$("#id_itmgrp").change(function () {
    //    var URL = $('#loader3').data('request-ajax3');
    //    var URLext = $('#id_itmgrp').val();
    //    URL = $.trim(URL);

    //    URLext = $.trim(URLext);
    //    URL = URL;
    //    $.ajax({
    //        type: "Post",
    //        async : false,
    //        url: URL,
    //        data: { itemgroup: URLext},
    //        error: function (xhr, status, error) {
    //            alert("Error: " + xhr.status + " - " + error + " - " + URL);
    //        },
    //        success: function (data) {
    //            var items = '';
    //            $.each(data, function (i, state) {
    //                items = state.Text;
    //                if (state.Value == "1") {
    //                    if (state.Text == "I")
    //                        $("#now").prop('checked', true)
    //                    else if (state.Text == "S")
    //                        $("#serve").prop('checked', true)
    //                    else
    //                        $("#rate").prop('checked', true)

    //                    //$("input[name='vwstrarray0[1]'][value='" + state.Text + "']").prop("checked", true);
    //                   }
    //                else if (state.Value == "2")
    //                    $("#id_uom").val(items);
    //                else if (state.Value == "3")
    //                    $("#id_itmgrpm").val(items);
    //                else if (state.Value == "4")
    //                {
    //                    if (state.Text == "F")
    //                        $("#id_fifo").prop('checked', true)
    //                    else if (state.Text == "A")
    //                        $("#id_ave").prop('checked', true)
    //                    else
    //                        $("#now6").prop('checked', true)

    //                 }
    //                else if (state.Value == "5")
    //                    $("#id_pref_vend").val(items);
    //                else if (state.Value == "6")  {
    //                    if (state.Text == "Y")
    //                        $("#taxy").prop('checked', true)
    //                    else
    //                        $("#taxn").prop('checked', true)

    //                  }
    //                else if (state.Value == "7")
    //                    $("#tax1").val(items);
    //                else if (state.Value == "8")
    //                    $("#tax2").val(items);
    //                else if (state.Value == "9")
    //                    $("#tax3").val(items);
    //                else if (state.Value == "10")
    //                    $("#tax4").val(items);
    //                else if (state.Value == "11")
    //                    $("#tax5").val(items);
    //                else if (state.Value == "12")
    //                    $("#glppv").val(items);
    //                else if (state.Value == "13")
    //                    $("#glinv").val(items);
    //                else if (state.Value == "14")
    //                    $("#glscv").val(items);
    //                else if (state.Value == "15")
    //                    $("#glinc").val(items);
    //                else if (state.Value == "16")
    //                    $("#glcos").val(items);
              
    //            });
    //           // $(".myradio").checkboxradio("refresh");
    //             $("input[name='vwstrarray0[1]'], input[name='vwstrarray0[6]'], input[name='vwstrarray0[17]']").checkboxradio("refresh");
    //             //$("input[name='vwstrarray0[6]']").checkboxradio("refresh");
    //             //$("input[name*='vwstrarray0[17]']").checkboxradio("refresh");


    //        }
    //    });
    //});

   
});

