
$("input").each(function () {
    var $this = $(this);
    if ($(this).val().trim() == "") {
        $(this).addClass("input-empty");
    } else {
        $(this).removeClass("input-empty");
    }
    $this.on("change", function () {
        if ($(this).val().trim() == "") {
            $(this).addClass("input-empty");
        } else {
            $(this).removeClass("input-empty");
        }
    });
});

$(document).ready(function () {
    $('#allorders').DataTable({
        responsive: true,
        "pageLength": 3
    });
    $('#addservices').css("display", "none");
});
function addservice() {
    $('#addservices').css("display", "block");
    $('#addservicesbutton').css("display", "none");
}
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
    $('#services').val('');
    $('#pin_code').val('');
    $('#Status').val('');
    $('#SaveVendor').css('display', 'block');
    $('#EditVendor').css('display', 'none');
});
$('#CancelVendor').click(function () {
    $('#saverecord').css('display', 'none');
});
$('#CancelsupServices').click(function () {
    $('#servicemodel').modal('hide');
})
$(document).on('change', '.chcktbl', function () {
    if ($(this).is(':checked')) {
        $('input.chcktbl').not(this).prop('checked', false);
        $(this).closest('tr').find('#btnBook').removeAttr('disabled');
        $('#saverecord').css('display', 'block');
        $('#SaveVendor').css('display', 'none');

        $('#EditVendor').css('display', 'block');
        var radataid = $(this).val();
        $('#id').val(radataid);
        $.ajax({
            url: '/ManageVendor/GetVendorDetails?id=' + radataid,
            type: 'POST',
            contentType: 'application/json',
            success: function (result) {
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
                var ks = result.services.split('&');
                var array = ks;
                var arrayValues = array;
                if (arrayValues) {
                    $.each(arrayValues, function (i, val) {
                        $("input[value='" + val + "']").prop('checked', 'checked');
                    });
                }
                $("#pin_code").val(result.pin_code);
                if (result.Status == "Active") {
                    $("input[name=Status][value='Active']").prop('checked', true);
                }
                else {
                    $("input[name=Status][value='InActive']").prop('checked', true);
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
$('#SaveVendor').click(function () {
    var val = $(this).val();
    var Id = $('#id').val();
    var Businessname = $('#Businessname').val();
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
    var pin_code = $('#pin_code').val();
    var pincodepattern = /^[0-9]{6}$/;
    var cpattren = /^[a-zA-Z\s]+$/;
    var spattren = /^[a-zA-Z\s]+$/;
    var countrypattren = /^[a-zA-Z\s]+$/;
    var servicelst = []
    $("input:checkbox[name=services]:checked").each(function () {
        servicelst.push($(this).val())
    })
    servicelst = servicelst.join('&');
    //--validation messages--//
    if (Businessname == '') { alert('Enter Supplier Business Name'); $('#Businessname').focus(); }
    else if (firstname == '') { alert('Enter Supplier First Name'); $('#firstname').focus(); }
    else if (!fnamepattren.test(firstname)) { alert("Enter only alphabates"); $("#firstname").focus(); }
    else if (lastname == '') { alert('Enter Supplier Last Name'); $('#lastname').focus(); }
    else if (!lnamepattren.test(lastname)) { alert("Enter only alphabates"); $("#lastname").focus(); }
    else if (email == '') { alert('Enter Email'); $('#email').focus(); }
    else if (!emailidpattren.test(email)) { alert("Please Enter a valid Email Address"); $("#email").focus(); }
    else if (phoneno == '') { alert('Enter Phone Number'); $('#phoneno').focus(); }
    else if (phoneno.length < 10) { alert('Enter 10 digits only'); $('#phoneno').focus(); }
    else if (!phonenopattern.test(phoneno)) { alert("Please Enter 10 digit Phone Number "); $("#phoneno").focus(); }
    else if (adress1 == '') { alert('Enter Supplier Flat Number'); $('#adress1').focus(); }
    else if (adress2 == '') { alert('Enter Supplier Locality'); $('#adress2').focus(); }
    else if (pin_code == '') { alert('Enter Pin Code'); $('#pin_code').focus(); }
    else if (pin_code.length < 6) { alert('Enter 6 digits only'); $('#pin_code').focus(); }
    else if (!pincodepattern.test(pin_code)) { alert("Please Enter 6 digit Pincode "); $("#pin_code").focus(); }
    else if (city == '') { alert('Enter City Name'); $('#city').focus(); }
    else if (!cpattren.test(city)) { alert("Enter only alphabates"); $("#city").focus(); }
    else if (state == '') { alert('Enter State Name'); $('#state').focus(); }
    else if (!spattren.test(state)) { alert("Enter only alphabates"); $("#state").focus(); }
    else if (country == '') { alert('Enter Country Name'); $('#country').focus(); }
    else if (!countrypattren.test(country)) { alert("Enter only alphabates"); $("#country").focus(); }
    else {
        var managevendor = {
            vendorId: $('#vendorId').val(),
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
            pin_code: $('#pin_code').val(),
            Status: $("input[name='Status']:checked").val(),
            services: servicelst
        }
        $.ajax({
            url: '/ManageVendor/Index?command=' + val + '&&id=' + Id,
            type: 'POST',
            data: JSON.stringify({ mngvendor: managevendor }),
            contentType: 'application/json',
            success: function (data) {
                alert(data);
                window.location.href = '/ManageVendor';
            },
            error: function (data) {
                alert("save is failed");
            }
        });
    }
});


$('#EditVendor').click(function () {
    //var status="null"
    var val = $(this).val();
    var Id = $('#id').val();
    var Businessname = $('#Businessname').val();
    var firstname = $('#firstname').val();
    var fnamepattren = /^[a-zA-Z\s]+$/;
    var lastname = $('#lastname').val();
    var lnamepattren = /^[a-zA-Z\s]+$/;
    var email = $('#email').val();
    var emailidpattren = /^([\w-\.]+\u0040([\w-]+\.)+[\w-]{2,4})?$/;
    var adress1 = $('#adress1').val();
    var adress2 = $('#adress2').val();
    var city = $('#city').val();
    var cpattren = /^[a-zA-Z\s]+$/;
    var state = $('#state').val();
    var spattren = /^[a-zA-Z\s]+$/;
    var country = $('#country').val();
    var countrypattren = /^[a-zA-Z\s]+$/;
    var phoneno = $('#phoneno').val();
    var phonenopattern = /^[0-9]{10}$/;
    var pin_code = $('#pin_code').val();
    var pincodepattern = /^[0-9]{6}$/;
    var servicelst = []
    $("input:checkbox[name=services]:checked").each(function () {
        servicelst.push($(this).val())
    })
    servicelst = servicelst.join('&');
    //--validation messages--//
    if (Businessname == '') { alert('Enter Supplier Business Name'); $('#Businessname').focus(); }
    else if (firstname == '') { alert('Enter Supplier First Name'); $('#firstname').focus(); }
    else if (!fnamepattren.test(firstname)) { alert("Enter only alphabates"); $("#firstname").focus(); }
    else if (lastname == '') { alert('Enter Supplier Last Name'); $('#lastname').focus(); }
    else if (!lnamepattren.test(lastname)) { alert("Enter only alphabates"); $("#lastname").focus(); }
    else if (email == '') { alert('Enter Email'); $('#email').focus(); }
    else if (!emailidpattren.test(email)) { alert("Please Enter a valid Email Address"); $("#email").focus(); }
    else if (phoneno == '') { alert('Enter Phone Number'); $('#phoneno').focus(); }
    else if (phoneno.length < 10) { alert('Enter 10 digits only'); $('#phoneno').focus(); }
    else if (!phonenopattern.test(phoneno)) { alert("Please Enter 10 digit Phone Number "); $("#phoneno").focus(); }
    else if (adress1 == '') { alert('Enter Supplier Flat Number'); $('#adress1').focus(); }
    else if (adress2 == '') { alert('Enter Supplier Locality'); $('#adress2').focus(); }
    else if (pin_code == '') { alert('Enter Pin Code'); $('#pin_code').focus(); }
    else if (pin_code.length < 6) { alert('Enter 6 digits only'); $('#pin_code').focus(); }
    else if (!pincodepattern.test(pin_code)) { alert("Please Enter 6 digit Pincode "); $("#pin_code").focus(); }
    else if (city == '') { alert('Enter City Name'); $('#city').focus(); }
    else if (!cpattren.test(city)) { alert("Enter only alphabates"); $("#city").focus(); }
    else if (state == '') { alert('Enter State Name'); $('#state').focus(); }
    else if (!spattren.test(state)) { alert("Enter only alphabates"); $("#state").focus(); }
    else if (country == '') { alert('Enter Country Name'); $('#country').focus(); }
    else if (!countrypattren.test(country)) { alert("Enter only alphabates"); $("#country").focus(); }
    else {
        var managevendor = {
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
            pin_code: $('#pin_code').val(),
            Status: $("input[name='Status']:checked").val(),
            services: servicelst

        }
        $.ajax({
            url: '/ManageVendor/Index?command=' + val + '&&id=' + Id,
            type: 'POST',
            data: JSON.stringify({ mngvendor: managevendor }),
            contentType: 'application/json',
            success: function (data) {
                alert(data);
                window.location.href = '/ManageVendor';
            }
        });
    }
});
function checkEmail(val) {
    vendorId = $('#vendorId').val();
    $.ajax({
        url: '/ManageVendor/checkVendoremail?email=' + val + "&&id=" + vendorId,
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
function addsupservice()
{
    $('#ServiceName').val('');
    $('#description').val('');
    $('#servicemodel').modal('show');
    $('#Updatesupservices').css('display', 'none');
    $('#ADDsupServices').css('display', 'block');
}
