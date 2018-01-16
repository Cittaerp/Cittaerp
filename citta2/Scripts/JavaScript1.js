function ConfirmYesNo(title, msg) {


    var dfd = jQuery.Deferred();
    var $confirm = $('#myModal');
    $confirm.modal('show');
    $('#myModalLabel').html(title);
    $('#myModalText').html(msg);


    $('#name').off('click').click(function () {
        $confirm.modal('hide');
        dfd.resolve(1);
        return 1;
    });


    $('#btnNo').off('click').click(function () {
        $confirm.modal('hide');
        return 0;
    });

    return dfd.promise();
}
function ValidationWarning(WarningMsg) {
    var a = ConfirmYesNo('dddd', ' WarningMsg');
    a.then(function (b) {
        console.log(b);
        alert(b)
    })
}
ValidationWarning()