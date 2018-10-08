function contactform(val) {
    var fname = $('#fname').val();
    var lname = $('#lname').val();
    var emailid = $('#emailid').val();
    var phoneno = $('#phoneno').val();
    var eventdate = $('#datetimepicker').val();
    //var mob = /^[1-9]{1}[0-9]{9}$/;
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
    //else if (mob.test(phoneno) == false) {
    //    $("#phoneno").focus();
    //    alert("Please Enter Valid Mobile Number.");
    //    return false;
    //}
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

var input = document.querySelector("#phoneno"), errorMsg = document.querySelector("#error-msg");
var iti = window.intlTelInput(input, {
    initialCountry: "auto",
    geoIpLookup: function (success, failure) {

        $.get("https://ipinfo.io", function () { }, "jsonp").always(function (resp) {
            var countryCode = (resp && resp.country) ? resp.country : "";
            success(countryCode);
        });
    },
    utilsScript: "newdesignstyles/Scripts/utils.js",
});

var errorMap = ["Invalid number", "Invalid country code", "Too short", "Too long", "Invalid number"];

input.addEventListener('blur', function () {
    reset();
    if (input.value.trim()) {
        if (iti.isValidNumber()) {
            //validMsg.classList.remove("hide");
            $('#btnsubmit').removeAttr('disabled');
        } else {
            input.classList.add("error");
            var errorCode = iti.getValidationError();
            errorMsg.innerHTML = errorMap[errorCode];
            errorMsg.classList.remove("hide");
            if (errorMap[errorCode] == 'Invalid number' || errorMap[errorCode] == 'Invalid country code') {
                $("#phoneno").val('').focus();
            }
            else {
                $("#phoneno").focus();
            }
            $('#btnsubmit').attr('disabled', 'disabled');
        }
    }
});

var reset = function () {
    input.classList.remove("error");
    errorMsg.innerHTML = "";
    errorMsg.classList.add("hide");
};