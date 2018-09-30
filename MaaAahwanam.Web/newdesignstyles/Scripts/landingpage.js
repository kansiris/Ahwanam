function contactform(val) {
    var fname = $('#fname').val();
    var lname = $('#lname').val();
    var emailid = $('#emailid').val();
    var phoneno = $('#phoneno').val();
    var eventdate = $('#datetimepicker').val();
    var mob = /^[1-9]{1}[0-9]{9}$/;
    var eml = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    if (fname == '') {
        $('#fname').focus();
        alert("Please Enter Your First Name");
    }
    else if (fname == '') {
        $('#fname').focus();
        alert("Please Enter Your First Name");
    }
    else if (lname == '') {
        $('#lname').focus();
        alert("Please Enter Your Last Name");
    }
    else if (emailid == '') {
        $('#emailid').focus();
        alert("Please Enter Your Email ID");
    }
    else if (eml.test($.trim(emailid)) == false) {
        alert("Please enter valid email address.");
        $("#emailid").focus();
        return false;
    }
    else if (phoneno == '') {
        $('#phoneno').focus();
        alert("Please Enter Your Phone Number");
    }
    else if (mob.test(phoneno) == false) {
        $("#phoneno").focus();
        alert("Please Enter Valid Mobile Number.");
        return false;
    }
    else if (eventdate == '') {
        $('#datetimepicker').focus();
        alert("Please Select Your Event Date");
    }
    else {
        var url = '/' + val + '/SendEmail?fname=' + fname + '&&lname=' + lname + '&&emailid=' + emailid + '&&phoneno=' + phoneno + '&&eventdate=' + eventdate;
        $.ajax({
            url: url,
            method: 'POST',
            contentType: 'application-json',
            success: function (response) {
                if (response == 'success') {
                    $('#contactusdiv').css('display', 'none');
                    $('#successmsg').css('display', 'block');
                    gtag_report_conversion();
                }
            }
        });
    }
}