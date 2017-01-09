
        var fixeddate;
var inext = 1; var iprev = 1;
$(function () {
    $("#save").css('display', 'none');
    $("#remove").css('display', 'none');
    getdates();
});
//next month click
//$(document).on('click', '.ui-datepicker-next', function () {
//    var month = getmonthnumber($('span.ui-datepicker-month').text());
//    //$(".ui-datepicker td.myclass ").attr({ "data-month": month, "data-year": $('span.ui-datepicker-year').text() });
//})

////prev month click
//$(document).on('click', '.ui-datepicker-prev', function () {
//    var month = getmonthnumber($('span.ui-datepicker-month').text());
//    $(".ui-datepicker td.myclass ").attr({ "data-month": month, "data-year": $('span.ui-datepicker-year').text() });
//})

var unavailableDates = '';
function getdates() {
    $.ajax({
        type: "POST",
        url: "/AvailableDates/GetDates",
        data: '',
        success: function (data) {
            //debugger;
            var final = '';
            for (var i = 0; i < data.length; i++) {
                var dateslist = data[i].split('/');
                if (dateslist[0] < 10 && dateslist[1] < 10) {
                    var datepart = dateslist[0].slice(-1, 2) + '/' + dateslist[1].slice(-1, 2) + '/' + dateslist[2];
                    final = final + datepart + ",";
                }
                else if (dateslist[0] < 10 && dateslist[1] >= 10) {
                    var datepart = dateslist[0].slice(-1, 2) + '/' + dateslist[1] + '/' + dateslist[2];
                    final = final + datepart + ",";
                }
                else if (dateslist[0] >= 10 && dateslist[1] < 10) {
                    var datepart = dateslist[0] + '/' + dateslist[1].slice(-1, 2) + '/' + dateslist[2];
                    final = final + datepart + ",";
                }
                else {
                    final = final + data[i] + ",";
                }
                unavailableDates = final.slice(0, -1).split(',');
            }
            var today = new Date(); today.setDate(1); today.setHours(-1);
            var currentmonth = (today.getMonth() + 2);
            var firstDay = new Date(today.getFullYear(), today.getMonth(), 1); // prev month first day
            var lastDay = new Date(today.getFullYear(), (today.getMonth() + 3), 0); // next month last day
            $('#products_delivery').multiDatesPicker({
                altField: '#availabledate',//altField: '#removedates'
                minDate: new Date(), maxDate: lastDay, //minDate: firstDay,
                dateFormat: 'dd/mm/yyyy', 
                        
                beforeShowDay: function (date) {
                    var dmy = date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear();
                    //debugger;
                    if ($.inArray(dmy, unavailableDates) == -1) {
                        return [true, ""];
                    } else {
                        return [false, "myclass", "Unavailable"]; 
                    }
                            
                }
                , onSelect: function (data, e) {  select(); }
            });
        },
        dataType: "json",
        traditional: true,

    });
}

function getmonthnumber(month) {
    var collection = ["", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    return collection.indexOf(month);
}

    $("body").on('click', ".ui-datepicker td.myclass > span", function () {
        var month = getmonthnumber($('span.ui-datepicker-month').text());
        var date = new Date(); //alert(typeof(date));
        var today = (date.getDate() + 1) + "/" + (date.getMonth() + 1) + "/" + date.getFullYear(); 
        var value = $(this).html() + "/" + month + "/" + $('span.ui-datepicker-year').text();
        var comparetoday = new Date(date.getFullYear(), (date.getMonth() + 1), (date.getDate() + 1));
        var comparevalue = new Date($('span.ui-datepicker-year').text(), month, $(this).html());
        var removeddates = $('#removedates').val();
        var date = value;
        if (comparevalue > comparetoday) {
            $("#remove").css('display', 'block');
            $(this).css('background', 'red');
            $('#removedates').val(date);
            appendWords(this);
        }
        else {
            alert("Cannot Select This Date");
        }

    });


function appendWords(t) {
    var flag = true; //var finaldates = $("#availabledate").val();
    var resultObj = $("#availabledate");
    var outputObj = $("#removedates");
    var testing = resultObj.val().split(",");
    for (var i = 0; i < testing.length; i++) {
        if (testing[i] == outputObj.val()) {
            flag = false; 
            $(t).css('background', 'green');
            var index = testing[i].indexOf(outputObj.val());
            if (index > -1) {
                testing.splice(i, 1);
                var stringToAppend = testing;
                resultObj.val(stringToAppend + '');
                var appended = $("#availabledate").val();
                if (appended == null || appended == '') {
                    $("#remove").css('display', 'none');
                }
            }
        }
    }
    //if (finaldates == null) {
    //    $("#remove").css('display', 'none');
    //}
    if (flag) {
        var stringToAppend = resultObj.val().length > 0 ? resultObj.val() + "," : "";
        resultObj.val(stringToAppend + outputObj.val());
        var appended = $("#availabledate").val();
    }
}

    function select() {
        var val = $("#availabledate").val();
        if (val != null && val != '') {
            $("#save").css('display', 'block');
        }
        else {
            $("#save").css('display', 'none');
        }
    }

//var final = ''; //var dateslist = data.split(','); 
//for (var i = 0; i < data.length; i++) {
//    var dates = data[i].split('/');
//    var dates1 = new Date(dates[2], dates[1] - 1, dates[0]).toLocaleDateString();
//    var dates2 = dates1.split('/');
//    var dates3 = dates2[1] + '/' + dates2[0] + '/' + dates2[2];
//    final = final + dates3 + ",";
//    unavailableDates = final.slice(0, -1).split(',');
//}