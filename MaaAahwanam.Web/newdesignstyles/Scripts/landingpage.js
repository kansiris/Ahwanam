function contactform(val) {
    var fname = $('#fname').val();
    var lname = $('#lname').val();
    var emailid = $('#emailid').val();
    var phoneno = $('#phoneno').val();
    var eventdate = $('#datetimepicker').val();
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
    else if (phoneno == '') {
        $('#phoneno').focus();
        alert("Please Enter Your Phone Number");
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