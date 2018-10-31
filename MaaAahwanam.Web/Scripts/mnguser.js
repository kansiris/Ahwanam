$(document).ready(function () {
    $("#radio2").click(function () {
        $("#company").show('slow');
    });

    $("#radio1").click(function () {

        $("#company").hide('slow');
    });

    $(document).ready(function () {
        $('#allorders').DataTable();
    });

    $('#allorders').DataTable({
        responsive: true,
        "pageLength": 1
    });

   
    $('#addservices').css("display", "none");
    var vpbook = window.localStorage.getItem("vpbook");
    var date = window.localStorage.getItem("vpbookdate");
    var timeslot = window.localStorage.getItem("vpbooktimeslot");
    var packageid = window.localStorage.getItem("vpbookpid");
    if (vpbook == "" || vpbook == null) {
        $('#booknow').css('display', 'none');
        $('#booknowss').css('display', 'none');
    }
    else {
        $('#booknow').css('display', 'block');
        $('#booknowss').css('display', 'block');
    }

    $.ajax({
        url: '/ManageUser/orderdetails?select=' + vpbook + '&date=' + date + '&timeslot=' + timeslot + '&packageid=' + packageid,
        type: 'POST',
        contentType: false,
        processData: false,
        success: function (result) {
            $('.ksc').html(result);
        }
    });

    function addservice() {
        $('#addservices').css("display", "block");
        $('#addservicesbutton').css("display", "none");
    }
})
$('#btnadd').click(function () {
    $('input.chcktbl').prop('checked', false);
    $('#saverecord').css('display', 'block');
    $('#Businessname').val('');
    $('#firstname').val('');
    $('#lastname').val('');
    $('#email').val('');
    $('#phoneno').val('');
    $('#adress1').val('');
    $('#adress2').val('');
    $('#city').val('');
    $('#state').val('');
    $('#country').val('');
    $('#pincode').val('');
    $('#Status').val('');
    $('#SaveUser').css('display', 'block');
    $('#EditUser').css('display', 'none');
    //$('#booknow').css('display', 'none');
    //$('#booknowss').css('display', 'none');

});

$('#CancelUser').click(function () {
    $('#saverecord').css('display', 'none');
});

$('#CancelUser2').click(function () {
    $('.addcutomerpopup').css('display', 'none');
    $('#btnadd2').show();
});


$(document).on('change', '.chcktbl', function () {
    if ($(this).is(':checked')) {
        $('input.chcktbl').not(this).prop('checked', false);
        $(this).attr("checked", "checked");
        $(this).closest('tr').find('#btnBook').removeAttr('disabled');
        $('#saverecord').css('display', 'block');
        $('#SaveUser').css('display', 'none');
        $('#EditUser').css('display', 'block');
        $('#booknow').css('display', 'block');
        $('#booknowss').css('display', 'none');
        $('#addcustomermodel').modal('show');
        var radataid = $(this).val();
        $('#id').val(radataid);
        //alert(radataid);
        $.ajax({
            url: '/ManageUser/GetUserDetails?id=' + radataid,
            type: 'POST',
            contentType: 'application/json',
            success: function (result) {
                if (result.type == "Corporate") {
                    $("input[name=type][value='Corporate']").prop('checked', true);
                }
                else {
                    $("input[name=type][value='Individual']").prop('checked', true);
                }
                $("#usedid").val(result.id);
                $("#Businessname").val(result.Businessname);
                $("#firstname").val(result.firstname);
                $("#lastname").val(result.lastname);
                $("#email").val(result.email);
                $("#phoneno").val(result.phoneno);
                $("#adress1").val(result.adress1);
                $("#adress2").val(result.adress2);
                $("#city").val(result.city);
                $("#state").val(result.state);
                $("#country").val(result.country);
                $("#pincode").val(result.pincode);
                $("#Status").val(result.Status);
                if (result.Status == "Active") {
                    $("input[name=Status][value='Active']").prop('checked', true);
                    $('#booknow').css("display", "block");
                }
                else {
                    $("input[name=Status][value='InActive']").prop('checked', true);
                    $('#booknow').css("display", "none");
                }
            },
            error: function (result) {
                alert("failed");
            }
        })
    }
    else {
        $(this).closest('tr').find('#btnBook').attr('disabled', 'disabled');
        $('#saverecord').css('display', 'none');
    }
});
$('#SaveUser').click(function () {
    var val = $(this).val();
    var Id = $('#id').val();
    var type = $("input[name='type']:checked").val();
    if (type == 'Corporate') {
        var Businessname = $('#Businessname').val();
        if (Businessname == '') { alert('Enter Customer Business Name'); $('#Businessname').focus(); }
    }
    //var Businessname = $('#Businessname').val();
    var firstname = $('#firstname').val();
    var fnamepattren = /^[a-zA-Z\s]+$/;
    var lastname = $('#lastname').val();
    var lnamepattren = /^[a-zA-Z\s]+$/;
    var email = $('#email').val();
    var emailidpattren = /^([\w-\.]+\u0040([\w-]+\.)+[\w-]{2,4})?$/;
    var adress1 = $('#adress1').val();
    var adress2 = $('#adress2').val();
    var city = $('#city').val();
    var state = $('#state').val();
    var country = $('#country').val();
    var phoneno = $('#phoneno').val();
    var phonenopattern = /^[0-9]{10}$/;
    var pincode = $('#pincode').val();
    var pincodepattern = /^(?! )(?!0000)[0-9]{6}$/;
    var cpattren = /^[a-zA-Z\s]+$/;
    var spattren = /^[a-zA-Z\s]+$/;
    var countrypattren = /^[a-zA-Z\s]+$/;
    //--validation messages--//
    //if (Businessname == '') { alert('Enter Customer Business Name'); $('#Businessname').focus(); }
    if (firstname == '') { alert('Enter Customer First Name'); $('#firstname').focus(); }
    else if (!fnamepattren.test(firstname)) { alert("Enter only alphabates"); $("#firstname").focus(); }
    else if (lastname == '') { alert('Enter Customer Last Name'); $('#lastname').focus(); }
    else if (!lnamepattren.test(lastname)) { alert("Enter only alphabates"); $("#lastname").focus(); }
    else if (email == '') { alert('Enter Email'); $('#email').focus(); }
    else if (!emailidpattren.test(email)) { alert("Please Enter a valid Email Address"); $("#email").focus(); }
    else if (phoneno == '') { alert('Enter Phone Number'); $('#phoneno').focus(); }
    else if (phoneno.length < 10) { alert('Enter 10 digits only'); $('#phoneno').focus(); }
    else if (!phonenopattern.test(phoneno)) { alert("Please Enter 10 digit Phone Number "); $("#phoneno").focus(); }
    else if (adress1 == '') { alert('Enter Customer Flat Number'); $('#adress1').focus(); }
    else if (adress2 == '') { alert('Enter Customer Locality'); $('#adress2').focus(); }
    else if (pincode == '') { alert('Enter Pin Code'); $('#pincode').focus(); }
    else if (pincode.length < 6) { alert('Enter 6 digits only'); $('#pincode').focus(); }
    else if (!pincodepattern.test(pincode)) { alert("Please Enter 6 digit Pincode "); $("#pincode").focus(); }
    else if (city == '') { alert('Enter City Name'); $('#city').focus(); }
    else if (!cpattren.test(city)) { alert("Enter only alphabates"); $("#city").focus(); }
    else if (state == '') { alert('Enter State Name'); $('#state').focus(); }
    else if (!spattren.test(state)) { alert("Enter only alphabates"); $("#state").focus(); }
    else if (country == '') { alert('Enter Country Name'); $('#country').focus(); }
    else if (!countrypattren.test(country)) { alert("Enter only alphabates"); $("#country").focus(); }
    else {
        var manageuser = {
            vendorId: $('#vendorId').val(),
            type: $("input[name='type']:checked").val(),
            Businessname: $('#Businessname').val(),
            firstname: $('#firstname').val(),
            lastname: $('#lastname').val(),
            email: $('#email').val(),
            adress1: $('#adress1').val(),
            adress2: $('#adress2').val(),
            city: $('#city').val(),
            state: $('#state').val(),
            country: $('#country').val(),
            phoneno: $('#phoneno').val(),
            pincode: $('#pincode').val(),
            Status: $("input[name='Status']:checked").val()
        }
        $.ajax({
            url: '/ManageUser/Index?command=' + val + '&&id=' + Id,
            type: 'POST',
            data: JSON.stringify({ mnguser: manageuser }),
            contentType: 'application/json',
            success: function (data) {
                alert(data);
                $('#addcustomermodel').modal('hide');
                window.location.reload();
            },
            error: function (data) {
                alert("save is failed");
            }
        });
    }
});

$('#EditUser').click(function () {
    var val = $(this).val();
    var Id = $('#id').val();
    var type = $("input[name='type']:checked").val();
    if (type == 'Corporate') {
        var Businessname = $('#Businessname').val();
        if (Businessname == '') { alert('Enter Customer Business Name'); $('#Businessname').focus(); }
    }
    var firstname = $('#firstname').val();
    var lastname = $('#lastname').val();
    var email = $('#email').val();
    var emailidpattren = /^([\w-\.]+\u0040([\w-]+\.)+[\w-]{2,4})?$/;
    var adress1 = $('#adress1').val();
    var adress2 = $('#adress2').val();
    var city = $('#city').val();
    var state = $('#state').val();
    var country = $('#country').val();
    var phoneno = $('#phoneno').val();
    var phonenopattern = /^[0-9]{10}$/;
    var pincode = $('#pincode').val();
    var pincodepattern = /^[0-9]{6}$/;
    var lnamepattren = /^[a-zA-Z\s]+$/;
    var fnamepattren = /^[a-zA-Z\s]+$/;
    var cpattren = /^[a-zA-Z\s]+$/;
    var spattren = /^[a-zA-Z\s]+$/;
    var countrypattren = /^[a-zA-Z\s]+$/;
    //--validation messages--//
    //if (Businessname == '') { alert('Enter Customer Business Name'); $('#Businessname').focus(); }
    if (firstname == '') { alert('Enter Customer First Name'); $('#firstname').focus(); }
    else if (!fnamepattren.test(firstname)) { alert("Enter only alphabates"); $("#firstname").focus(); }
    else if (lastname == '') { alert('Enter Customer Last Name'); $('#lastname').focus(); }
    else if (!lnamepattren.test(lastname)) { alert("Enter only alphabates"); $("#lastname").focus(); }
    else if (email == '') { alert('Enter Email'); $('#email').focus(); }
    else if (!emailidpattren.test(email)) { alert("Please Enter a valid Email Address"); $("#email").focus(); }
    else if (phoneno == '') { alert('Enter Phone Number'); $('#phoneno').focus(); }
    else if (phoneno.length < 10) { alert('Enter 10 digits only'); $('#phoneno').focus(); }
    else if (!phonenopattern.test(phoneno)) { alert("Please Enter 10 digit Phone Number "); $("#phoneno").focus(); }
    else if (adress1 == '') { alert('Enter Customer Flat Number'); $('#adress1').focus(); }
    else if (adress2 == '') { alert('Enter Customer Locality'); $('#adress2').focus(); }
    else if (pincode == '') { alert('Enter Pin Code'); $('#pincode').focus(); }
    else if (pincode.length < 6) { alert('Enter 6 digits only'); $('#pincode').focus(); }
    else if (!pincodepattern.test(pincode)) { alert("Please Enter 6 digit Pincode "); $("#pincode").focus(); }
    else if (city == '') { alert('Enter City Name'); $('#city').focus(); }
    else if (!cpattren.test(city)) { alert("Enter only alphabates"); $("#city").focus(); }
    else if (state == '') { alert('Enter State Name'); $('#state').focus(); }
    else if (!spattren.test(state)) { alert("Enter only alphabates"); $("#state").focus(); }
    else if (country == '') { alert('Enter Country Name'); $('#country').focus(); }
    else if (!countrypattren.test(country)) { alert("Enter only alphabates"); $("#country").focus(); }
    else {
        var manageuser = {
            vendorId: $('#vendorId').val(),
            type: $("input[name='type']:checked").val(),
            Businessname: $('#Businessname').val(),
            firstname: $('#firstname').val(),
            lastname: $('#lastname').val(),
            email: $('#email').val(),
            adress1: $('#adress1').val(),
            adress2: $('#adress2').val(),
            city: $('#city').val(),
            state: $('#state').val(),
            country: $('#country').val(),
            phoneno: $('#phoneno').val(),
            pincode: $('#pincode').val(),
            Status: $("input[name='Status']:checked").val()
        }

        $.ajax({
            url: '/ManageUser/Index?id=' + Id + '&&command=' + val,
            type: 'POST',
            data: JSON.stringify({ mnguser: manageuser }),
            contentType: 'application/json',
            success: function (data) {
                alert(data);
                $('#addcustomermodel').modal('hide');
                window.location.reload();
            },
            error: function (data) {
                alert("update is failed");
            }
        });
    }
});

//$("input[type=text]").on('input', function () {
//    var type = $('input[name=type]:checked').val();
//    var Businessname = $('#Businessname').val();
//    if (type == "Corporate" && Businessname == '') { // || type != "Individual") {
//        alert("Please enter business name");
//    }
//})

function checkEmail(val) {
    //alert(val);
    vendorId = $('#vendorId').val();
    $.ajax({
        url: '/ManageUser/checkemail?email=' + val + "&&id=" + vendorId,
        type: 'Post',
        contentType: 'application/json',
        success: function (result) {
            if (result == "sucess1") {
                alert("email is already existed select another email");
                $('#email').val('').focus();
            }
        },
        error: function (result) {
            alert("failed");
        }
    });
}
$('#booknow').click(function () {
    var vid = $("#vendorId").val();
    var userid = $("#usedid").val();
    var packageid = window.localStorage.getItem("vpbookpid");
    var vpbook = window.localStorage.getItem("vpbook");
    var date = window.localStorage.getItem("vpbookdate");
    var timeslot = window.localStorage.getItem("vpbooktimeslot");
    var packid = packageid;
    var vpbook1 = vpbook.split(',');
    var loc = vpbook1[0];
    var eventtype = vpbook[2];
    var guests = vpbook1[1];
    var date = date;
    var timeslot = timeslot;
    if (packid == '') {
        var selectedp = window.localStorage.getItem("vpbook");
    }
    $('.overlay').show();
    $('#loadermsg').text('Booking in process....');
    var type = $("input[name='type']:checked").val();
    if (type == 'Corporate') {
        var Businessname = $('#Businessname').val();
        if (Businessname == '') { alert('Enter Customer Business Name'); $('#Businessname').focus(); }
    }
    //var Businessname = $('#Businessname').val();
    var firstname = $('#firstname').val();
    var lastname = $('#lastname').val();
    var email = $('#email').val();
    var emailidpattren = /^([\w-\.]+\u0040([\w-]+\.)+[\w-]{2,4})?$/;
    var adress1 = $('#adress1').val();
    var adress2 = $('#adress2').val();
    var city = $('#city').val();
    var state = $('#state').val();
    var country = $('#country').val();
    var phoneno = $('#phoneno').val();
    var phonenopattern = /^[0-9]{10}$/;
    var pincode = $('#pincode').val();
    var pincodepattern = /^(?! )(?!0000)[0-9]{6}$/;
    var lnamepattren = /^[a-zA-Z\s]+$/;
    var fnamepattren = /^[a-zA-Z\s]+$/;
    var cpattren = /^[a-zA-Z\s]+$/;
    var spattren = /^[a-zA-Z\s]+$/;
    var countrypattren = /^[a-zA-Z\s]+$/;
    //--validation messages--//
    //if (Businessname == '') { alert('Enter Customer Business Name'); $('#Businessname').focus(); }
    if (firstname == '') { alert('Enter Customer First Name'); $('#firstname').focus(); }
    else if (lastname == '') { alert('Enter Customer Last Name'); $('#lastname').focus(); }
    else if (email == '') { alert('Enter Email'); $('#email').focus(); }
    else if (!emailidpattren.test(email)) { alert("Please Enter a valid Email Address"); $("#email").focus(); }
    else if (phoneno == '') { alert('Enter Phone Number'); $('#phoneno').focus(); }
    else if (phoneno.length < 10) { alert('Enter 10 digits only'); $('#phoneno').focus(); }
    else if (!phonenopattern.test(phoneno)) { alert("Please Enter 10 digit Phone Number "); $("#phoneno").focus(); }
    else if (adress1 == '') { alert('Enter Customer Flat Number'); $('#adress1').focus(); }
    else if (adress2 == '') { alert('Enter Customer Locality'); $('#adress2').focus(); }
    else if (pincode == '') { alert('Enter Pin Code'); $('#pincode').focus(); }
    else if (pincode.length < 6) { alert('Enter 6 digits only'); $('#pincode').focus(); }
    else if (!pincodepattern.test(pincode)) { alert("Please Enter 6 digit Pincode "); $("#pincode").focus(); }
    else if (city == '') { alert('Enter City Name'); $('#city').focus(); }
    else if (state == '') { alert('Enter State Name'); $('#state').focus(); }
    else if (country == '') { alert('Enter Country Name'); $('#country').focus(); }
    else if (!countrypattren.test(country)) { alert("Enter only alphabates"); $("#country").focus(); }
    else if (!spattren.test(state)) { alert("Enter only alphabates"); $("#state").focus(); }
    else if (!cpattren.test(city)) { alert("Enter only alphabates"); $("#city").focus(); }
    else if (!fnamepattren.test(firstname)) { alert("Enter only alphabates"); $("#firstname").focus(); }
    else if (!lnamepattren.test(lastname)) { alert("Enter only alphabates"); $("#lastname").focus(); }
    else {
        $.ajax({
            url: '/ManageUser/booknow?uid=' + userid + '&&loc=' + loc + '&&eventtype=' + eventtype + '&&eventtype=' + eventtype + '&&guest=' + guests + '&&date=' + date + '&&pid=' + packid + '&&vid=' + vid + '&&selectedp=' + selectedp + '&&timeslot=' + timeslot,
            type: 'POST',
            contentType: 'application/json',
            success: function (result) {
                if (result == "please active the user") {
                    alert(result);
                }
                else if (result != "") {
                    alert("Order placed Successfully");
                    window.localStorage.removeItem("vpbook");
                    window.localStorage.removeItem("vpbookdate");
                    window.localStorage.removeItem("vpbooktimeslot");
                    window.localStorage.removeItem("vpbookpid");
                    window.location.href = "/vinvoice/Index?oid=" + result;
                } else {
                    alert("Error Occurred in placing order");
                }
            }
        });
    }
});

$('#booknowss').click(function () {
    var businessname = $('#Businessname').val();
    var type = $('input[name=type]:checked').val();
    var firstname = $('#firstname').val();
    var lastname = $('#lastname').val();
    var email = $('#email').val();
    var phoneno = $('#phoneno').val();
    var adress1 = $('#adress1').val();
    var adress2 = $('#adress2').val();
    var city = $('#city').val();
    var state = $('#state').val();
    var country = $('#country').val();
    var pincode = $('#pincode').val();
    var Status = $('input[name=Status]:checked').val(); //$('#Status').val();
    var vid = $("#vendorId").val();
    var packageid = window.localStorage.getItem("vpbookpid");
    var vpbook = window.localStorage.getItem("vpbook");
    var date = window.localStorage.getItem("vpbookdate");
    var timeslot = window.localStorage.getItem("vpbooktimeslot");
    var packid = packageid;
    var vpbook1 = vpbook.split(',');
    var loc = vpbook1[0];
    var eventtype = vpbook[2];
    var guests = vpbook1[1];
    var date = date;

    if (type == "Corporate" && businessname != '' || type == "Individual") {

        $('.overlay').show();
        $('#loadermsg').text('Booking in process....');
        var type = $("input[name='type']:checked").val();
        if (type == 'Corporate') {
            var Businessname = $('#Businessname').val();
            if (Businessname == '') { alert('Enter Customer Business Name'); $('#Businessname').focus(); }
        }
        //var Businessname = $('#Businessname').val();
        var firstname = $('#firstname').val();
        var lastname = $('#lastname').val();
        var email = $('#email').val();
        var emailidpattren = /^([\w-\.]+\u0040([\w-]+\.)+[\w-]{2,4})?$/;
        var adress1 = $('#adress1').val();
        var adress2 = $('#adress2').val();
        var city = $('#city').val();
        var state = $('#state').val();
        var country = $('#country').val();
        var phoneno = $('#phoneno').val();
        var phonenopattern = /^[0-9]{10}$/;
        var pin_code = $('#pin_code').val();
        var pincodepattern = /^(?! )(?!0000)[0-9]{6}$/;
        var lnamepattren = /^[a-zA-Z\s]+$/;
        var fnamepattren = /^[a-zA-Z\s]+$/;
        var cpattren = /^[a-zA-Z\s]+$/;
        var spattren = /^[a-zA-Z\s]+$/;
        var countrypattren = /^[a-zA-Z\s]+$/;
        //--validation messages--//
        //if (Businessname == '') { alert('Enter Customer Business Name'); $('#Businessname').focus(); }
        if (firstname == '') { alert('Enter Customer First Name'); $('#firstname').focus(); }
        else if (lastname == '') { alert('Enter Customer Last Name'); $('#lastname').focus(); }
        else if (email == '') { alert('Enter Email'); $('#email').focus(); }
        else if (!emailidpattren.test(email)) { alert("Please Enter a valid Email Address"); $("#email").focus(); }
        else if (phoneno == '') { alert('Enter Phone Number'); $('#phoneno').focus(); }
        else if (phoneno.length < 10) { alert('Enter 10 digits only'); $('#phoneno').focus(); }
        else if (!phonenopattern.test(phoneno)) { alert("Please Enter 10 digit Phone Number "); $("#phoneno").focus(); }
        else if (adress1 == '') { alert('Enter Customer Flat Number'); $('#adress1').focus(); }
        else if (adress2 == '') { alert('Enter Customer Locality'); $('#adress2').focus(); }
        else if (pincode == '') { alert('Enter Pin Code'); $('#pincode').focus(); }
        else if (pincode.length < 6) { alert('Enter 6 digits only'); $('#pincode').focus(); }
        else if (!pincodepattern.test(pincode)) { alert("Please Enter 6 digit Pincode "); $("#pincode").focus(); }
        else if (city == '') { alert('Enter City Name'); $('#city').focus(); }
        else if (state == '') { alert('Enter State Name'); $('#state').focus(); }
        else if (country == '') { alert('Enter Country Name'); $('#country').focus(); }
        else if (!countrypattren.test(country)) { alert("Enter only alphabates"); $("#country").focus(); }
        else if (!spattren.test(state)) { alert("Enter only alphabates"); $("#state").focus(); }
        else if (!cpattren.test(city)) { alert("Enter only alphabates"); $("#city").focus(); }
        else if (!fnamepattren.test(firstname)) { alert("Enter only alphabates"); $("#firstname").focus(); }
        else if (!lnamepattren.test(lastname)) { alert("Enter only alphabates"); $("#lastname").focus(); }
        else {
            $.ajax({
                url: '/ManageUser/booknowss?loc=' + loc + '&&eventtype=' + eventtype + '&&eventtype=' + eventtype + '&&guest=' + guests + '&&date=' + date + '&&pid=' + packid + '&&vid=' + vid + '&&businessname=' + businessname + '&&firstname=' + firstname + '&&lastname=' + lastname + '&&email=' + email + '&&phoneno=' + phoneno + '&&adress1=' + adress1 + '&&adress2=' + adress2 + '&&city=' + city + '&&state=' + state + '&&country=' + country + '&&pincode=' + pincode + '&&Status=' + Status + '&&ctype=' + type + '&&timeslot=' + timeslot, //+ '&&selectedp=' + selectedp,
                type: 'POST',
                contentType: 'application/json',
                success: function (result) {
                    if (result == "please active the user") {
                        alert(result);
                    }
                    else if (result != "") {
                        alert("Order placed Successfully");
                        window.localStorage.removeItem("vpbook");
                        window.localStorage.removeItem("vpbookdate");
                        window.localStorage.removeItem("vpbooktimeslot");
                        window.localStorage.removeItem("vpbookpid");
                        window.location.href = "/vinvoice/Index?oid=" + result;
                    } else {
                        alert("Error Occurred in placing order");
                    }
                }
            });

        }
    }
});

$('#btnadd').on('click', function () {
    $('#addcustomermodel').modal('show');
    var vpbook = window.localStorage.getItem("vpbook");
    var date = window.localStorage.getItem("vpbookdate");
    var timeslot = window.localStorage.getItem("vpbooktimeslot");
    var packageid = window.localStorage.getItem("vpbookpid");
    if (vpbook == "" || vpbook == null) {
        $('#booknow').css('display', 'none');
        $('#booknowss').css('display', 'none');
    }
    else {
        $('#booknow').css('display', 'none');
        $('#booknowss').css('display', 'block');
    }
});
