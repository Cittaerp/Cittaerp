$(function () {

    $("table.dataprintv tbody").on("click", "tr td.details-control", function () {
        var tr = $(this).closest('tr');
        var row = qTable.row(tr);
        var data1 = row.data();
        var pos1 = $("#keyval1").val();
        var pos2 = $("#keyval2").val();


        if (row.child.isShown()) {
            tr.removeClass("details");
            row.child.hide();
        }
        else {
            tr.addClass("details");
            if (pos2 == null)
                row.child(getdetails(data1[pos1], '',data1[0])).show();
            else
                row.child(getdetails(data1[pos1], data1[pos2], data1[0])).show();
            table_inner();
        }
    });


})

function get_former_transaction_ad(ttype) {
    var URL = $('#loader2').data('request-daily');
    URL = $.trim(URL);
    var sno = $("#snumber").val();
    URL = URL + ttype + "[]" + sno;
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
                items += state.Text;
            });
            $("#vaclist").html(items);
        }
    })
}

function getdetails(dr1, dr2, dr3) {
    var URL = $('#loader2').data('request-daily');
    URL = $.trim(URL);
    var sno = $("#snumber").val();
    var dn = dr1 + "[]" + dr2 + "[]" + dr3.substr(0, dr3.length - 1) + "[]" + sno;
    var items;
    URL = URL + "10" + dn;
    $.ajax({
        type: "Post",
        url: URL,
        async: false,
        error: function (xhr, status, error) {
            alert("Error: " + xhr.status + " - " + error + " - " + URL);
        },
        success: function (data) {
            items = '';
            $.each(data, function (i, state) {
                items += state.Text;
            });
        }
    })

    return items;

}

function table_inner() {
    $("table.datainner").DataTable().destroy();
    qTableInner = $("table.datainner").DataTable({
        destroy: true,
        scrollX: false,
        fixedHeader: true,
        colReorder: false,
        stateSave: false,
        iDisplayLength: 150,
        dom: '<"clear"><"dtwrapper"rt>',
        ordering: false,
    });
    return qTableInner;
}

