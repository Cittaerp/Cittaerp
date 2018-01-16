
// table with print option,no ordering, page
function table_createpnp() {
    var ptext = $("#ptitle").val();
    $("table.dataprintv").DataTable().destroy();
    qTable = $("table.dataprintv").DataTable({
            destroy : true,
            order : [],
            scrollX: true,
            fixedHeader: false,
            colReorder: true,
            stateSave: true,
            iDisplayLength: 25,
            lengthMenu: [[25, 50, 75, 100, -1], [25, 50, 75, 100, "All"]],
            dom: '<"clear"><"dtwrapper"B>l<"centered"i><"dtwrapper"frtip>',
            ordering: false,
            responsive: true,
            buttons: [
                {
                    extend: "colvis",
                    text: "Hide/Show"
                },
                {
                    extend: "collection",
                    text: "Options",
                    buttons: [
                        {
                            extend: "copy",
                            text: "<u>C</u>opy to Clipboard",
                            key: {
                                key: "c",
                                altKey: true
                            },
                            exportOptions: {
                                columns: ':visible'
                            },
                        },
                        {
                            extend: "print",
                            text: '<u>P</u>rint',
                            key: {
                                key: 'p',
                                altKey: true
                            },
                            exportOptions: {
                                columns: ':visible'
                            },
                            title: ptext,
                            filename : ptext
                        },
                        {
                            extend: "csv",
                            text: "CSV File",
                            exportOptions: {
                                columns: ':visible'
                            },
                            filename: ptext
                        },
                        {
                            extend: "excel",
                            text: "Excel File",
                            exportOptions: {
                                columns: ':visible'
                            },
                            title: ptext,
                            filename: ptext
                        },
                        {
                            extend: "pdf",
                            text: "PDF File",
                            exportOptions: {
                                columns: ':visible'
                            },
                            title: ptext,
                            filename: ptext
                        },

                    ]
                }
            ]

        })
        return qTable;
    }

// table with print , no-ordering, no page
function table_createpnn() {
    var ptext = $("#ptitle").val();
    $("table.dataprintv").DataTable().destroy();
    qTable = $("table.dataprintv").DataTable({
        destroy: true,
        order : [],
        scrollX: true,
        fixedHeader: false,
        colReorder: true,
        stateSave: true,
        iDisplayLength: 15,
        dom: '<"clear"><"dtwrapper"Brtip>',
        ordering: false,
        buttons: [
            {
                extend: "colvis",
                text: "Hide/Show"
            },
            {
                extend: "collection",
                text: "Options",
                buttons: [
                    {
                        extend: "copy",
                        text: "<u>C</u>opy to Clipboard",
                        key: {
                            key: "c",
                            altKey: true
                        },
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                    {
                        extend: "print",
                        text: '<u>P</u>rint',
                        key: {
                            key: 'p',
                            altKey: true
                        },
                        exportOptions: {
                            columns: ':visible'
                        },
                        title: ptext,
                        filename: ptext
                    },
                    {
                        extend: "csv",
                        text: "CSV File",
                        exportOptions: {
                            columns: ':visible'
                        },
                        filename: ptext
                    },
                    {
                        extend: "excel",
                        text: "Excel File",
                        exportOptions: {
                            columns: ':visible'
                        },
                        title: ptext,
                        filename: ptext
                    },
                    {
                        extend: "pdf",
                        text: "PDF File",
                        exportOptions: {
                            columns: ':visible'
                        },
                        title: ptext,
                        filename: ptext
                    },

                ]
            }
        ]
    });

    return qTable;
};

// table no print, no ordering, no page
function table_creatennn() {
    $("table.dataprintv").DataTable().destroy();
    qTable = $("table.dataprintv").DataTable({
        destroy: true,
        order : [],
//        scrollX: "100%",
        fixedHeader: true,
        colReorder: true,
        stateSave: true,
        iDisplayLength: 15,
        dom: '<"clear"><"dtwrapper"rtip>',
//        ordering: false,
        responsive : true,
//        scrollY: '70vh',
//        scrollCollapse: true
    })
    $("table.dataprintv").wrap("<div class='ScrollTable'></div>");
    return qTable;
}

// table no print, no ordering, no page, no responsive
function table_creatennnr() {
    $("table.dataprintv").DataTable().destroy();
    qTable = $("table.dataprintv").DataTable({
        destroy: true,
        order: [],
        //        scrollX: "100%",
        fixedHeader: true,
        colReorder: true,
        stateSave: true,
        iDisplayLength: 15,
        dom: '<"clear"><"dtwrapper"rtip>',
//        ordering: false,
        //        scrollY: '70vh',
        //        scrollCollapse: true
    })
    $("table.dataprintv").wrap("<div class='ScrollTable'></div>");
    return qTable;
}

// table no print, no ordering, page
function table_creatennp() {
    $("table.dataprintv").DataTable().destroy();
    qTable = $("table.dataprintv").DataTable({
        destroy: true,
        order : [],
//        scrollX: true,
        fixedHeader: true,
        colReorder: true,
        stateSave: true,
        iDisplayLength: 25,
        lengthMenu: [[25, 50, 75, 100, -1], [25, 50, 75, 100, "All"]],
        dom: '<"clear">l<"centered"i><"dtwrapper"frtp>',
        ordering: false,
        responsive : true,
//        scrollY: '70vh',
//        scrollCollapse: true
    })
    $("table.dataprintv").wrap("<div class='ScrollTable'></div>");
    return qTable;
}

// table para screen and fixed header
function table_create_para() {
    $("table.datadisplay").DataTable().destroy();
    qTable = $("table.datadisplay").DataTable({
        destroy: true,
        order : [],
        scrollX: false,
        fixedHeader: true,
        colReorder: true,
        stateSave: true,
        iDisplayLength: 25,
        lengthMenu: [[25, 50, 75, 100, -1], [25, 50, 75, 100, "All"]],
        dom: '<"clear">l<"centered"i><"dtwrapper"frtip>',
        "columnDefs": [{ "targets": [0], "orderable": false }]

    })
    return qTable;
}

//view table with fixed header
function table_viewpnp() {
    var ptext = $("#ptitle").val();
    $("table.dataprintv").DataTable().destroy();
    qTable = $("table.dataprintv").DataTable({
            destroy: true,
            order : [],
            scrollX: false,
            fixedHeader: true,
            colReorder: true,
            stateSave: true,
            iDisplayLength: 25,
            lengthMenu: [[25, 50, 75, 100, -1], [25, 50, 75, 100, "All"]],
            dom: '<"clear"><"dtwrapper"B>l<"centered"i><"dtwrapper"frtip>',
            ordering: false,
            buttons: [
                {
                    extend: "colvis",
                    text: "Hide/Show"
                },
                {
                    extend: "collection",
                    text: "Options",
                    buttons: [
                        {
                            extend: "copy",
                            text: "<u>C</u>opy to Clipboard",
                            key: {
                                key: "c",
                                altKey: true
                            },
                            exportOptions: {
                                columns: ':visible'
                            }
                        },
                        {
                            extend: "print",
                            text: '<u>P</u>rint',
                            key: {
                                key: 'p',
                                altKey: true
                            },
                            exportOptions: {
                                columns: ':visible'
                            },
                            title: ptext,
                            filename: ptext
                        },
                        {
                            extend: "csv",
                            text: "CSV File",
                            exportOptions: {
                                columns: ':visible'
                            },
                            filename : ptext
                        },
                        {
                            extend: "excelHtml5",
                            text: "Excel File",
                            exportOptions: {
                                columns: ':visible'
                            },
                            title: ptext,
                            filename: ptext
                        },
                        {
                            extend: "pdf",
                            text: "PDF File",
                            exportOptions: {
                                columns: ':visible'
                            },
                            title: ptext,
                            filename: ptext
                        },

                    ]
                }
            ]

        })
        return qTable;
    }

// view and fixed header
function table_viewnnp() {
    $("table.dataprintv").DataTable().destroy();
    qvTable = $("table.dataprintv").DataTable({
        destroy: true,
        order : [],
        scrollX: false,
        fixedHeader: true,
        colReorder: true,
        stateSave: true,
        iDisplayLength: 25,
        lengthMenu: [[25, 50, 75, 100, -1], [25, 50, 75, 100, "All"]],
        dom: '<"clear">l<"centered"i><"dtwrapper"frtip>',
        ordering: false
    })

    return qvTable;
}

// table for data columns
function table_datacols() {
    $("table.dataprintv").DataTable().destroy();
    qTable = $("table.dataprintv").DataTable({
        destroy: true,
        scrollX: true,
        fixedHeader: false,
        colReorder: true,
        stateSave: true,
        iDisplayLength: 15,
        dom: '<"clear"><"dtwrapper"rtip>',
        ordering: false,
        columnDefs: [
            { "name": "col1", "targets": 0 },
            { "name": "col2", "targets": 1 },
            { "name": "col3", "targets": 2 },
            { "name": "col4", "targets": 3 },
            { "name": "col5", "targets": 4 },
            { "name": "col6", "targets": 5 },
            { "name": "col7", "targets": 6 },
            { "name": "col8", "targets": 7 },
            { orderable : false, targets : [0] }
        ]

    })
    return qTable;
}

// table for data columns index
function table_datacolp() {
    $("table.dataprintv").DataTable().destroy();
    qTable = $("table.dataprintv").DataTable({
        destroy: true,
        scrollX: false,
        fixedHeader: true,
        colReorder: true,
        stateSave: true,
        iDisplayLength: 25,
        lengthMenu: [[25, 50, 75, 100, -1], [25, 50, 75, 100, "All"]],
        dom: '<"clear">l<"centered"i><"dtwrapper"frtip>',
        ordering: false,
        columnDefs: [
            { "name": "col1", "targets": 0 },
            { "name": "col2", "targets": 1 },
            { "name": "col3", "targets": 2 },
            { "name": "col4", "targets": 3 },
            { "name": "col5", "targets": 4 },
            { "name": "col6", "targets": 5 },
            { orderable : false, targets : [0] }
        ]

    })
    return qTable;
}

function user_font()
{
    var fsize = $("#fs").val();
    if (fsize != "") {
        $("body").css("font-size", fsize);
        fsize = $("#ff").val();
        $("body").css("font-family", fsize);
        $(".hmenu").css("font-family", fsize);        
    }
    else
    {
        $(".hmenu").css("font-family", "inherit");
    }
}
$(function () {

    //$('input[type=text], textarea, select').not(" .noboot, table textarea, table select ").addClass(' form-control mypadding ');
    //$(' .noboot, table textarea, table select  ').addClass(' round-corners ')

    var fsize = $("#fs").val();
    if (fsize != "") {
        $("body").css("font-size", fsize);
        fsize = $("#ff").val();
        $("body").css("font-family", fsize);
        $(".hmenu").css("font-family", fsize);
    }

    var vdob = $(".dob")
    if (vdob.length != 0) {
        $(".dob").datepicker({
            dateFormat: 'dd/mm/yy',
            changeMonth: true,
            changeYear: true
        })
    };

    $.fn.DataTable.Buttons.swfPath = '../../DataTables/swf/flashExport.swf'

    var oTable = table_create_para()
    $('[data-toggle="tooltip"]').tooltip();
 
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $.fn.dataTable
            .tables({ visible: true, api: true })
            .columns.adjust()
        .responsive.recalc();
    });


    $("#id_delete").click(function () {
        var $form = $(this).closest('form');
        bootbox.confirm("<p><h3><font color=red>Do you want to delete the Record?</font></h3></p>", function (a) {
            if (a) {
                $("#id_xhrt").val("D");
                $form.trigger("submit");
            }
            else
            {
                $("#id_xhrt").val("X");
            }
        });
    });

    var baseloc = $("#locat").val();

    $.sessionTimeout({
        title: 'Expiring : Your Current Session ',
        keepAliveButton : ' Continue ',
        keepAliveUrl: baseloc + 'home/KeepAlive',
        logoutUrl: baseloc + 'home/signout',
        redirUrl: baseloc + 'home/signout',
        warnAfter: 1500000,
        redirAfter: 1800000,
        countdownMessage: 'Redirecting in {timer} seconds.'
    });

    $('.btnNext').click(function () {
        $('.nav-tabs > .active').next('li').find('a').trigger('click');
    });

    $('.btnPrevious').click(function () {
        $('.nav-tabs > .active').prev('li').find('a').trigger('click');
    });

    $(".myradioq input[type=radio], .mycheckq input[type=checkbox]").checkboxradio({
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

    $("#multi_yesno").click(function () {
        $("#line1_off").toggle();

    })

    $(".cls_formsubmit").change(function () {
        var $form = $(this).closest('form');
        $("#id_xhrt").val("N");
        $form.trigger("submit");

    });


});

$(function ($) {
    $(".numform").autoNumeric("init");
    $(".digitform").autoNumeric("init", { mDec: 0 });
})
