// Popup Logic
var regClick = $(".registerBtn"), loginClick = $(".loginBtn"), forgotClick = $(".forgotPasswordBtn"),
            loginBlock = $(".login"), regBlock = $(".register"), forgotBlock = $(".forgotPassword");
$(regBlock).css("display", "none");
$(regClick).click(function () {
    $(".authenticationBlocks > div").css("display", "none");
    $(regBlock).css("display", "block");
});
$(loginClick).click(function () {
    $(".forgotks").css("display", "none");
    $(".register").css("display", "none");
    $(".authenticationBlocks > div").css("display", "block");
});

$(forgotClick).click(function () {
    $(".authenticationBlocks > div").css("display", "none");
    $(forgotBlock).css("display", "block");
});

// Handling inputs on Load
$(document).ready(function () {
    var guests = window.localStorage.getItem("guests");
    var location = window.localStorage.getItem("location");
    var eventtype = window.localStorage.getItem("eventtype");
    var date = window.localStorage.getItem("eventdate");
    //alert('Guests:' + guests + ',loc=' + location + ',event=' + eventtype + ',date=' + date);
    if (eventtype != '' && eventtype != null && eventtype != 'eventtype')
        $("span.current").text(eventtype);
    else
        $("span.current").text('Wedding');

    if (guests == '' || guests == null && guests != 'guests') {
        window.localStorage.setItem("guests", 100);
        $('#guests').val(100);
    }
    else { $('#guests').val(guests); }

    if (location != '' && location != null && location != 'location') { $('#loc').val(location); }
    else { $('#loc').val("Hyderabad"); }

    if (new Date(date) < new Date()) {
        var today = new Date();
        var formatdate = '' + today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear() + '';
        $('#datetimepicker1').val(formatdate);
    }
    else { $('#datetimepicker1').val(date); }
});

// Search
$('#btnsearch').click(function () {
    var location = $('#loc').val();
    var eventtype = $('.current').text();
    var guests = $('#guests').val();
    var eventdate = $('#datetimepicker1').val();
    if (location == '') {
        alert('Enter Location');
        $('#loc').focus();
    }
    else if (eventtype == '' || eventtype == null || eventtype == 'Select Event') {
        alert('Select Event Type');
    }
    else if (guests == '') {
        alert('Enter Guests');
        $('#guests').focus();
    }
    else if (eventdate == '') {
        alert('Select date');
        $('#datetimepicker1').focus();
    }
    else {
        window.localStorage.setItem("guests", guests);
        window.localStorage.setItem("location", location);
        window.localStorage.setItem("eventtype", eventtype);
        window.localStorage.setItem("eventdate", eventdate);
        window.location.href = '/results/Index?loc=' + location + '&&eventtype=' + eventtype + '&&count=' + guests + '&&date=' + eventdate;
    }
});

// New Registration
function savecustomer(clickedvalue) {
    var CustomerPhoneNumber = $("#txtphone").val();
    var CustomerName = $("#txtname").val();
    var Password = $("#Password").val();
    var Email = $("#txtemail").val();
    var emailReg = /^([\w-\.]+\u0040([\w-]+\.)+[\w-]{2,4})?$/;
    var phonepattern = /^[0-9]{10}$/;
    var passwordpattern = /^[a-z0-9]+$/i;
    var emailaddressVal = $("#txtemail").val();
    var url = document.referrer;

    if (CustomerName == '') {
        alert("Please Enter Name");
        $("#txtname").focus();
    }
        //else if (!passwordpattern.test(CustomerName)) {
        //    alert("Please Enter valid 10 digit Phonenumber");
        //    $("#txtphone").focus();
        //}
    else if (CustomerName.length < 5) {
        alert("Name must be 5 characters");
        $("#txtname").focus();
    }
    else if (Password == '') {
        alert("Please Enter Password");
        $("#txtpassword").focus();
    }
    else if (Password.length < 5) {
        alert("Password length must be 5 characters");
        $("#txtpassword").focus();
        $("#txtpassword").alphanum();
    }
    else if (!passwordpattern.test(Password)) {
        alert("Please Enter valid 10 digit Phonenumber");
        $("#txtphone").focus();
    }
    else if (CustomerPhoneNumber == '') {
        alert("Please Enter PhoneNumber");
        $("#txtphone").focus();
    }
        //else if (!phonepattern.test(CustomerPhoneNumber)) {
        //    alert("Please Enter valid 10 digit Phonenumber");
        //    $("#txtphone").focus();
        //}
    else if (emailaddressVal == '') {
        alert("Please Enter Email Address");

        $("#txtemail").focus();
    }
    else if (!emailReg.test(emailaddressVal)) {
        alert("Please Enter a valid Email Address");
        $("#txtemail").focus();
    }
    else {
        if (clickedvalue == 'Register') {
            $('.overlay').show();
            $('#loadermsg').text('Registration in process....');
            $.ajax({
                url: '/Home/register',//?CustomerPhoneNumber=' + CustomerPhoneNumber + '&CustomerName=' + CustomerName + '&Password=' + Password + '&Address=' + Address + '&Email=' + Email,
                type: 'POST',
                data: JSON.stringify({ CustomerPhoneNumber: CustomerPhoneNumber, CustomerName: CustomerName, Password: Password, Email: Email }),
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    if (data == "success") {
                        alert("Check your email to active your account to login");
                        location.reload();
                    }
                    else if (data == "unique") {
                        alert("E-Mail ID Already Registered!!! Try Logging with your Password");
                    }
                    else { //{if (data == "Failed" || data == "unique1") {
                        alert("Registration Failed");
                        window.location.reload();
                    }
                    $('.overlay').hide();
                },
                error: function (data) {
                    alert("Something Went Wrong!!! Try Again Later");
                    $('.overlay').hide();
                }
            });
        }
    }
}

// Authentication
function login(clickedvalue) {
    var url1 = document.referrer;
    if (url1 == '' || url1 == undefined) { url1 = location.href; }
    var Password = $("#Password1").val();
    Password = Password.replace(/\s/g, '');
    var Email = $("#UserName").val();
    Email = Email.replace(/\s/g, '');
    var emailReg = /^([\w-\.]+\u0040([\w-]+\.)+[\w-]{2,4})?$/;
    var passwordpattern = /^[a-z0-9]+$/i;
    var emailaddressVal = $("#UserName").val();
    if (emailaddressVal == '') {
        alert("Please Enter Email Address");
        $("#txtemail").focus();
    }
    else if (Password == '') {
        alert("Please Enter Password");
        $("#txtpassword").focus();
    }
        //else if (!emailReg.test(emailaddressVal)) {
        //    alert("Please Enter a valid Email Address");
        //    $("#txtpassword").focus();
        //}
    else {
        if (clickedvalue == 'Login') {
            $('.overlay').show();
            $('#loadermsg').text('Authenticating....');
            $.get("/Home/login", { Password: Password, Email: Email, url1: url1 }, function (data) {
                if (data == "success")
                    window.location.reload();
                else if (data == "Failed" || data == "success1")
                { alert("Wrong Credentials,Check Username and password"); $('.overlay').hide(); }
            }).error(function (data) { alert("Something Went Wrong!!! Try again Later") });
        }
    }
}

// Forgot Password
function forgot(clickedvalue) {
    var Email = $("#email_forgot").val();
    Email = Email.replace(' ', '');
    var emailReg = /^([\w-\.]+\u0040([\w-]+\.)+[\w-]{2,4})?$/;
    var emailaddressVal = $("#email_forgot").val();
    if (emailaddressVal == '') {
        alert("Please Enter Email Address");
        $("#txtemail").focus();
    }
    else if (!emailReg.test(emailaddressVal)) {
        alert("Please Enter a valid Email Address");
        $("#txtemail").focus();
    }
    else {
        if (clickedvalue == 'Reset Password') {
            $('.overlay').show();
            $('#loadermsg').text('Locating your Email Address in database....');
            $.get("/Home/forgotpass", { Email: Email }, function (data) {
                if (data == "success") {
                    alert("A mail is sent to your email to change password Please check your email");
                    window.location.reload();
                }
                else if (data == "success1") {
                    alert("Email ID Not Found.please enter registered Email ID");
                    $("#email_forgot").focus();
                }
            }).error(function (data) { alert("Something Went Wrong!!! Try again Later") });
        }
    }
}

function forgot1(clickedvalue) {
    var Email = $("#email_forgot1").val();
    var emailReg = /^([\w-\.]+\u0040([\w-]+\.)+[\w-]{2,4})?$/;
    var emailaddressVal = $("#email_forgot1").val();
    if (emailaddressVal == '') {
        alert("Please Enter Email Address");
        $("#txtemail").focus();
    }
    else if (!emailReg.test(emailaddressVal)) {
        alert("Please Enter a valid Email Address");
        $("#txtemail").focus();
    }
    else {
        if (clickedvalue == 'Reset Password') {
            $.ajax({
                url: '/Home/forgotpass',//?CustomerPhoneNumber=' + CustomerPhoneNumber + '&CustomerName=' + CustomerName + '&Password=' + Password + '&Address=' + Address + '&Email=' + Email,
                type: 'POST',
                data: JSON.stringify({ Email: Email }),
                //data: JSON.stringify({ CustomerPhoneNumber, CustomerName, Password, Address, Email }),
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    if (data == "success") {
                        alert("A mail is sent to your email to change password Please check your email");
                        window.location.reload();
                    }

                    else if (data == "Failed") {

                        alert("Email ID Not Found");
                        window.location.reload();
                    }
                    else if (data == "success1") {

                        alert("Email ID Not Found");
                        window.location.reload();
                    }
                },
                error: function (data) {
                    alert("Something Went Wrong!!! Try Again Later");
                }
            });
        }


    }
}
function savecustomer1(clickedvalue) {
    var CustomerPhoneNumber = $("#txtphone1").val();
    var CustomerName = $("#txtname1").val();
    var Password = $("#Password3").val();
    var Email = $("#txtemail1").val();
    var emailReg = /^([\w-\.]+\u0040([\w-]+\.)+[\w-]{2,4})?$/;
    var phonepattern = /^[0-9]{10}$/;
    var passwordpattern = /^[a-z0-9]+$/i;
    var emailaddressVal = $("#txtemail1").val();

    //else if (!phonepattern.test(CustomerPhoneNumber)) {
    //    alert("Please Enter valid 10 digit Phonenumber");
    //    $("#txtphone1").focus();
    //}
    if (CustomerName == '') {
        alert("Please Enter Name");
        $("#txtname1").focus();
    }
    else if (!passwordpattern.test(CustomerName)) {
        alert("Please Enter valid 10 digit Phonenumber");
        $("#txtphone1").focus();
    }
    else if (CustomerName.length < 5) {
        alert("Name must be 5 characters");
        $("#txtname1").focus();
    }
    else if (Password == '') {
        alert("Please Enter Password");
        $("#Password3").focus();
    }
    else if (Password.length < 5) {
        alert("Password length must be 5 characters");
        $("#Password3").focus();
        $("#Password3").alphanum();
    }
    else if (!passwordpattern.test(Password)) {
        alert("Please Enter valid 10 digit Phonenumber");
        $("#txtphone1").focus();
    }
    else if (emailaddressVal == '') {
        alert("Please Enter Email Address");
        $("#txtemail1").focus();
    }
    else if (!emailReg.test(emailaddressVal)) {
        alert("Please Enter a valid Email Address");
        $("#txtemail1").focus();
    }
    else if (CustomerPhoneNumber == '') {
        alert("Please Enter PhoneNumber");
        $("#txtphone1").focus();
    }

    else {
        if (clickedvalue == 'Register') {
            $('.overlay').show();
            $('#loadermsg').text('Registration in process....');
            $.ajax({
                url: '/Home/register', type: 'POST',
                data: JSON.stringify({ CustomerPhoneNumber: CustomerPhoneNumber, CustomerName: CustomerName, Password: Password, Email: Email }),
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    if (data == "success") {
                        alert("Check your email to active your account to login");
                        window.location.reload();

                    }
                    if (data == "unique") {
                        alert("E-Mail ID Already Registered!!! Try Logging with your Password");
                        window.location.reload();

                    }
                    else if (data == "unique1") {
                        alert("Registration fails");
                        $('.overlay').hide();
                    }
                    else if (data == "Failed") {

                        alert("Registration Failed");
                        window.location.reload();
                    }
                    else {
                        alert("Registration Failed");
                        // $("#txtphone").val('');
                        $("#txtname").val('');
                        $("#txtpassword").val('');
                        $("#txtdeladdress").val('');
                        $("#txtemail").val('');
                        // window.location.reload();
                        $("#otpopup").css("display", "block");
                        $("#txtotp").focus();
                        $('.overlay').hide();
                    }
                },
                error: function (data) {
                    alert("Something Went Wrong!!! Try Again Later");
                    $('.overlay').hide();
                }
            });
        }
    }
}

function login1(clickedvalue) {
    var Password = $("#Password2").val();
    var Email = $("#UserName1").val();
    Email = Email.replace(/\s/g, '');
    Password = Password.replace(/\s/g, '');
    var emailReg = /^([\w-\.]+\u0040([\w-]+\.)+[\w-]{2,4})?$/;
    var passwordpattern = /^[a-z0-9]+$/i;
    var emailaddressVal = $("#UserName1").val();
    var url1 = document.referrer;
    if (emailaddressVal == '') {
        alert("Please Enter Email Address");
        $("#UserName1").focus();
    }
    else if (!emailReg.test(emailaddressVal)) {
        alert("Please Enter a valid Email Address");
        $("#UserName1").focus();
    }
    else if (Password == '') {
        alert("Please Enter Password");
        $("#Password2").focus();
    }

    else {
        if (clickedvalue == 'Login') {
            $.ajax({
                url: '/Home/login',
                type: 'POST',
                data: JSON.stringify({ Password: Password, Email: Email, url1: url1 }),
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    if (data == "success") {
                        //alert("Login Sucessfull");
                        window.location.reload();
                    }
                    if (data == "unique") {
                        alert("vendorlogin successfull");
                        //vendorlogin
                    }
                    else if (data == "Failed") {

                        alert("Wrong Credentials,Check Username and password");
                        window.location.reload();
                    }
                    else if (data == "success1") {

                        alert("Wrong Credentials,Check Username and password");
                        window.location.reload();
                    }
                },
                error: function (data) {
                    alert("Something Went Wrong!!! Try Again Later");
                }
            });
        }
    }
}