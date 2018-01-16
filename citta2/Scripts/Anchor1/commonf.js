
    function check_password_same() {
        var numeric_flag = false;
        var alphacap_flag = false;
        var alphasmall_flag = false;
        var other_flag = false;

        var newpassword = document.all.NewPassword.value;
        var confirmpassword = document.all.ConfirmPassword.value;
        var oldpassword = document.all.OldPassword.value;
        var passlen = document.all.pass1.value;
        var passecure = document.all.pass2.value;

        if (newpassword != confirmpassword) {
            alert("Password are not same ...");
            return false;
        }

        if (newpassword.length < passlen) {
            alert("Invalid Password.... Minimum Length is " + passlen);
            return false;
        }

        if (passecure == "Y") {
            for (var ws_count = 0; ws_count < newpassword.length; ws_count++) {
                if ((newpassword.charCodeAt(ws_count) - newpassword.charCodeAt(ws_count + 1)) == 1 |
                    (newpassword.charCodeAt(ws_count) - newpassword.charCodeAt(ws_count + 1)) == 0 |
                    (newpassword.charCodeAt(ws_count) - newpassword.charCodeAt(ws_count + 1)) == -1) {
                    alert("Invalid Password.... Password too simple");
                    return false;
                }
            }
        }
            //' 0-9 48-57
            //' A-Z 65-90
            //' a-z 97-122

            if (passecure == "Y" || passecure == "L") {
                for (var ws_count = 0; ws_count < newpassword.length ; ws_count++) {
                    if (newpassword.charCodeAt(ws_count) >= 48 && newpassword.charCodeAt(ws_count) <= 57)
                        numeric_flag = true;
                    else if (newpassword.charCodeAt(ws_count) >= 65 && newpassword.charCodeAt(ws_count) <= 90)
                        alphacap_flag = true;
                    else if (newpassword.charCodeAt(ws_count) >= 97 && newpassword.charCodeAt(ws_count) <= 122)
                        alphasmall_flag = true;
                    else
                        other_flag = true;
                }

                if (!(numeric_flag && alphacap_flag && alphasmall_flag && other_flag)) {
                    alert("Invalid Password.... Password too simple ");
                    return false;
                }
            }

        


        var seed = document.all.pass3.value;
        document.all.NewPassword.value = (MD5(newpassword + oldpassword));
        document.all.ConfirmPassword.value = (MD5(confirmpassword + seed));
        document.all.OldPassword.value = (MD5(oldpassword + seed));
        if (document.all.pass5.value!="")
            document.all.pass5.value = (MD5(document.all.pass5.value + seed));

        return true;
    }

    function log_password() {
        document.all.ws_string2.value = MD5(MD5(document.all.ws_string0.value + document.all.ws_code.value) + document.all.ws_string3.value);
        document.all.ws_string0.value = 'xadfgkhrgt109878'
        return true;
    }

    function lostp_password() {
        document.all.ws_string2.value = (MD5(document.all.ws_string2.value + document.all.ws_code.value));
        return true;

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


    $(function () {

        var vdob = $(".dob")
        if (vdob.length != 0) {
            $(".dob").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true
            })
        };

        $(".lgtime").click(function () {
            lgshow();
        });

    })
