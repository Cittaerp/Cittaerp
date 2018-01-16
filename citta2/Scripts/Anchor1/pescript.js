$(function () {
 
    // payroll enquiry


    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $.fn.dataTable
            .tables({ visible: true, api: true })
            .columns.adjust()
        .responsive.recalc();
    });

    $("#refexpense").click(function () {
        var URL = $('#loader2').data('request-daily');
        URL = $.trim(URL);
        var url4 = $("#snumber").val();
        var url5 = $("#docnum").val();
        var url6 = $("#defsn").val();
        $.ajax({
            type: "Post",
            url: URL,
            data: { snumber: url4, doc1: url5, type1: url6 },
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    items += state.Text;
                });
                $("#explist").html(items);
                $("#staff_number").val(url4);
                table_creatennnr();
            }
        })
    })

    $("#refincident").click(function () {
        var URL = $('#loader3').data('request-daily');
        URL = $.trim(URL);
        var url4 = $("#snumber").val();
        var url5 = $("#ticketno").val();
        var url6 = $("#defsn").val();
        $.ajax({
            type: "Post",
            url: URL,
            data: { snumber: url4, doc1: url5, type1: url6 },
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                var items = '';
                $.each(data, function (i, state) {
                    items += state.Text;
                });
                $("#inclist").html(items);
                $("#staff_number").val(url4);
                table_creatennnr();
            }
        })
    })

    $(".lx_approve").click(function () {
        if ($("#xsd").val() == "0")
            click_page_app("N");

        $("#xsd").val("1");

    })

    $(".lx_display").click(function () {
        if ($("#xsd").val() == "0")
            click_page_app("Y");

        $("#xsd").val("1");

    })

});

// hr enquiry
    function display_hr()
    {
        var URL = $('#loader1').data('request-daily');
        var sno = $('#snumber').val();
        var stype = $('#ttype1').val();
        URL = $.trim(URL);
        URL = URL + stype + "[]" + sno;
        if (sno.trim() != "") {
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
                        // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
                    });
                    $('#allowdata').html(items);
                    table_creatennp();

                }
            });
        }
    };

    function partp_rtn()
    {
        var items = '';
        var URL = $('#loader1').data('request-daily');
        var URLext = $('#snumber').val();

        $.ajax({
            type: "Post",
            url: URL,
            async: false,
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL);
            },
            success: function (data) {
                $.each(data, function (i, state) {
                    if (state.Text == "temp") {
                        $('#temp').html(items);
                        table_createpnp();
                        items = "";
                    }
                    else if (state.Text == "part") {
                        $('#partp').html(items);
                        table_createpnp();
                        items = "";
                    }
                    else
                        items += state.Text;
                });
            }
        });


    }

//verify if reqd
    function other_rtn()
    {
        var items = "";
        var URL = $('#loader1').data('request-daily');
        var url1 = $('#pyear').val();
        var url2 = $('#snumber').val();
        var errorq = function (xhr, status, error) {
            alert("Error: " + xhr.status + " - " + error + " - " + URL);
        };

        URL = $.trim(URL);
        URL = URL + url1 + "[]" + url2;
        if (url2.trim() != "") {
            $.ajax({
                type: "Post",
                url: URL + "[]1",
                success: function (data) {
                    var items = "";
                    $.each(data, function (i, state) {
                        items += state.Text;
                    });
                    $('#vacation').html(items);
                    table_creatennn();
                }
            })

            $.ajax({
                type: "Post",
                url: URL + "[]2",
                success: function (data) {
                    var items = "";
                    $.each(data, function (i, state) {
                        items += state.Text;
                    });
                    $('#medical').html(items);
                    table_creatennn();
                }
            })

            $.ajax({
                type: "Post",
                url: URL + "[]3",
                success: function (data) {
                    var items = "";
                    $.each(data, function (i, state) {
                        items += state.Text;
                    });
                    $('#train1').html(items);
                    table_creatennn();
                }
            })
        }

    }

//verify if reqd
    function appr_rtn(aflag)
    {
        var items = "";
        var URL = $('#loader1').data('request-daily');
        URL = $.trim(URL);

        $.ajax({
            type: "Post",
            url: URL + aflag+"[]1",
            error : function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL + "1");
            },
            success: function (data) {
                var items = "";
                $.each(data, function (i, state) {
                    if (state.Text == "ADL")
                    {
                        $('#lawaiting').html(items);
                        table_creatennn();
                        items = "";
                    }
                    else if (state.Text == "D") {
                        $('#ldelegated').html(items);
                        table_creatennn();
                        items = "";
                    }
                    else if (state.Text == "L") {
                        $('#lescalated').html(items);
                        table_creatennn();
                        items = "";
                    }
                    else if (state.Text == "W") {
                        $('#outstanding').html(items);
                        table_creatennn();
                        items = "";
                    }
                    else
                        items += state.Text;
                });
            }
        });


        //$.ajax({
        //    type: "Post",
        //    url: URL + aflag+"[]2",
        //    error: errorq,
        //    success: function (data) {
        //        var items = "";
        //        $.each(data, function (i, state) {
        //            items += state.Text;
        //        });
        //        $('#ldelegated').html(items);
        //        table_creatennn();
        //    }
        //});

        //$.ajax({
        //    type: "Post",
        //    url: URL + aflag+"[]3",
        //    error: errorq,
        //    success: function (data) {
        //        var items = "";
        //        $.each(data, function (i, state) {
        //            items += state.Text;
        //        });
        //        $('#lescalated').html(items);
        //        table_creatennn();
        //    }
        //});

        //$.ajax({
        //    type: "Post",
        //    url: URL + aflag+"[]4",
        //    error: errorq,
        //    success: function (data) {
        //        var items = "";
        //        $.each(data, function (i, state) {
        //            items += state.Text;
        //        });
        //        $('#outstanding').html(items);
        //        table_creatennn();
        //    }
        //});

        $.ajax({
            type: "Post",
            url: URL + aflag + "[]5",
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL + "5");
            },
            success: function (data) {
                var items = "";
                $.each(data, function (i, state) {
                    items += state.Text;
                });
                $('#iapproved').html(items);
                table_creatennn();
            }
        });

        $.ajax({
            type: "Post",
            url: URL + aflag + "[]6",
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL + "6");
            },
            success: function (data) {
                var items = "";
                $.each(data, function (i, state) {
                    items += state.Text;
                });
                $('#myrequest').html(items);
                table_creatennn();
            }
        });

    }

//verify if reqd
    function appr_rtnr(aflag) {
        var items = "";
        var URL = $('#loader1').data('request-daily');
        URL = $.trim(URL);
        var errorq = function (xhr, status, error) {
            alert("Error: " + xhr.status + " - " + error + " - " + URL);
        };

        $.ajax({
            type: "Post",
            url: URL + aflag + "[]1",
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL + "1");
            },
            success: function (data) {
                var items = "";
                $.each(data, function (i, state) {
                    if (state.Text == "ADL") {
                        $('#lawaiting').html(items);
                        table_creatennnr();
                        items = "";
                    }
                    else if (state.Text == "D") {
                        $('#ldelegated').html(items);
                        table_creatennnr();
                        items = "";
                    }
                    else if (state.Text == "L") {
                        $('#lescalated').html(items);
                        table_creatennnr();
                        items = "";
                    }
                    else if (state.Text == "W") {
                        $('#outstanding').html(items);
                        table_creatennnr();
                        items = "";
                    }
                    else
                        items += state.Text;
                });
            }
        });


        //$.ajax({
        //    type: "Post",
        //    url: URL + aflag + "[]2",
        //    error: errorq,
        //    success: function (data) {
        //        var items = "";
        //        $.each(data, function (i, state) {
        //            items += state.Text;
        //        });
        //        $('#ldelegated').html(items);
        //        table_creatennnr();
        //    }
        //});

        //$.ajax({
        //    type: "Post",
        //    url: URL + aflag + "[]3",
        //    error: errorq,
        //    success: function (data) {
        //        var items = "";
        //        $.each(data, function (i, state) {
        //            items += state.Text;
        //        });
        //        $('#lescalated').html(items);
        //        table_creatennnr();
        //    }
        //});

        //$.ajax({
        //    type: "Post",
        //    url: URL + aflag + "[]4",
        //    error: errorq,
        //    success: function (data) {
        //        var items = "";
        //        $.each(data, function (i, state) {
        //            items += state.Text;
        //        });
        //        $('#outstanding').html(items);
        //        table_creatennnr();
        //    }
        //});

        $.ajax({
            type: "Post",
            url: URL + aflag + "[]5",
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL + "5");
            },
            success: function (data) {
                var items = "";
                $.each(data, function (i, state) {
                    items += state.Text;
                });
                $('#iapproved').html(items);
                table_creatennn();
            }
        });

        $.ajax({
            type: "Post",
            url: URL + aflag + "[]6",
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL + "6");
            },
            success: function (data) {
                var items = "";
                $.each(data, function (i, state) {
                    items += state.Text;
                });
                $('#myrequest').html(items);
                table_creatennn();
            }
        });

    }

    function click_page_app(aflag) {
        var items = "";
        var URL = $('#loader1').data('request-daily');
        URL = $.trim(URL);

        $.ajax({
            type: "Post",
            url: URL + aflag + "[]5",
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL + "5");
            },
            success: function (data) {
                var items = "";
                $.each(data, function (i, state) {
                    items += state.Text;
                });
                $('#iapproved').html(items);
                table_creatennn();
            }
        });

         $.ajax({
            type: "Post",
            url: URL + aflag + "[]6",
            error: function (xhr, status, error) {
                alert("Error: " + xhr.status + " - " + error + " - " + URL + "6");
            },
            success: function (data) {
                var items = "";
                $.each(data, function (i, state) {
                    items += state.Text;
                });
                $('#myrequest').html(items);
                table_creatennn();
            }
        });


    }

//verify if reqd
    function onepage_rtn() {
        var items = '';
        var URL = $('#loader1').data('request-daily');
        var URLext = $('#snumber').val();
        var errorq = function (xhr, status, error) {
            alert("Error: " + xhr.status + " - " + error + " - " + URL);
        };

        if (URLext.trim() != "") {
            $.ajax({
                type: "Post",
                url: URL+"S",
                error: errorq,
                success: function (data) {
                    $.each(data, function (i, state) {
                        if (state.Text == "summary") {
                            $('#summary').html(items);
                            items = "";
                        }
                        else if (state.Text == "tax") {
                            $('#taxdata').html(items);
                            items = "";
                        }
                        else if (state.Text == "bio") {
                            $('#biodata').html(items);
                            items = "";
                        }
                        else if (state.Text == "bank") {
                            $('#bank').html(items);
                            items = "";
                        }
                        else if (state.Text == "perdata") {
                            $('#perdata').html(items);
                            items = "";
                        }
                        else if (state.Text == "kin") {
                            $('#kindata').html(items);
                            items = "";
                        }
                        else if (state.Text == "grat") {
                            $('#gratdata').html(items);
                            items = "";
                        }
                        else if (state.Text == "over") {
                            $('#overtime').html(items);
                            items = "";
                        }
                        else if (state.Text == "user") {
                            $('#userdef').html(items);
                            items = "";
                        }
                        else
                            items += state.Text;
                    });

                }
            })

            $.ajax({
                type: "Post",
                url: URL + "A",
                error: errorq,
                success: function (data) {
                    var items = "";
                    $.each(data, function (i, state) {
                        items += state.Text;
                    });
                    $('#allowance').html(items);
                    table_creatennn();
                    items="";
                }
            });

            $.ajax({
                type: "Post",
                url: URL + "D",
                error: errorq,
                success: function (data) {
                    var items = "";
                    $.each(data, function (i, state) {
                        items += state.Text;
                    });
                    $('#deduction').html(items);
                    table_creatennn();
                    items="";
                }
            });

            $.ajax({
                type: "Post",
                url: URL + "L",
                error: errorq,
                success: function (data) {
                    var items = "";
                    $.each(data, function (i, state) {
                        items += state.Text;
                    });
                    $('#loant').html(items);
                    table_creatennn();
                    items="";
                }
            });

            $.ajax({
                type: "Post",
                url: URL + "P",
                error: errorq,
                success: function (data) {
                    var items = "";
                    $.each(data, function (i, state) {
                        items += state.Text;
                    });
                    $('#partpayment').html(items);
                    table_creatennn();
                    items="";
                }
            });

            $.ajax({
                type: "Post",
                url: URL + "T",
                error: errorq,
                success: function (data) {
                    var items = "";
                    $.each(data, function (i, state) {
                        items += state.Text;
                    });
                    $('#temporary').html(items);
                    table_creatennn();
                    items="";
                }
            });

        }

    }

