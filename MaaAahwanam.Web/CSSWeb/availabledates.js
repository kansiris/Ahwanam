
        var fixeddate;
var inext = 1; var iprev = 1;
$(function () {
    $("#save").css('display', 'none');
    $("#remove").css('display', 'none');
    //getdates();
    $("#vendorservicelist").attr('title','Select Service');
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


$("body").on('change', "#vendorservicelist", function () {
    //$("#products_delivery").multiDatesPicker('resetDates', 'disabled');
    $("#availabledate").val('');
    getdates(this.value); //$("#availabledate").val('');
    //alert($("#availabledate").val());
    //$('#products_delivery').multiDatesPicker().find(".ui-datepicker-current-day a ").removeClass("ui-state-highlight ui-state-active");
    //$("#products_delivery").multiDatesPicker("refresh");
});

var unavailableDates = '';
function getdates(id) {
    
    $.ajax({
        type: "POST",
        url: "/AvailableDates/GetDates?id="+id+"",
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
            var todaydate=new Date();
            var today = new Date(); today.setDate(1); today.setHours(-1);
            var currentmonth = (today.getMonth() + 2);
            var firstDay = new Date(today.getFullYear(), today.getMonth(), 1); // prev month first day
            var lastDay = new Date(today.getFullYear(), (today.getMonth() + 3), 0); // next month last day
            var yesterday = new Date(todaydate.getFullYear(), (todaydate.getMonth()), (todaydate.getDate() - 1));
            //alert(yesterday);
            //$("#products_delivery").multiDatesPicker('resetDates', 'disabled');
            //$('#products_delivery').multiDatesPicker("destroy");//.find("td.ui-datepicker-current-day").removeClass("ui-state-highlight");
            //$('#products_delivery').multiDatesPicker().multiDatesPicker("setDate", yesterday);
            $('#products_delivery').multiDatesPicker({
                //$('#products_delivery'+id+'').multiDatesPicker({
                altField: '#availabledate',//altField: '#removedates'
                minDate: new Date(), maxDate: lastDay, //minDate: firstDay,
                dateFormat: 'dd/mm/yyyy',
                todayHighlight: false,//defaultDate: null,//defaultDate:yesterday,
                beforeShowDay: function (date) {
                    var dmy = date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear();
                    //debugger;
                    if ($.inArray(dmy, unavailableDates) == -1) {
                        return [true, ""];
                    } else {
                        return [false, "myclass", "Unavailable"];
                    }
                    //$.find('.ui-state-highlight.ui-state-hover').removeClass('ui-state-highlight ui-state-hover')
                    //$("#products_delivery a.ui-state-highlight").removeClass("ui-state-highlight");
                }
                , onSelect: function (data, e) {
                    select();
                },
                //defaultDate: yesterday,
            });//.multiDatesPicker("setDate", yesterday);//.find("td.ui-datepicker-today").removeClass("ui-state-highlight"); //,'resetDates', 'disabled'
        },
        dataType: "json",
        traditional: true,

    });
    unavailableDates = '';
    //$("#products_delivery .ui-state-active").removeClass("ui-state-active");
    //$("#products_delivery .ui-state-highlight").removeClass("ui-state-highlight");
    //$("#products_delivery .ui-datepicker-today").removeClass("ui-datepicker-today");
    //$("#products_delivery").multiDatesPicker("refresh");
    //$('#products_delivery').multiDatesPicker("destroy");
    //$('#products_delivery').multiDatesPicker().find(".ui-datepicker-current-day a ").removeClass("ui-state-highlight ui-state-active");
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
            $('#vendorsubid').val($('#vendorservicelist').val());
            $('#vendorservicelist').prop("disabled", true).prop("title","Make Sure that below dates are not selected");
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
                    $('#vendorservicelist').prop("disabled", false).prop("title", "Select Service");
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
            //$('#vendorservicelist').prop("disabled", true).prop("title", "Make Sure that below dates are not selected");
        }
        else {
            $("#save").css('display', 'none');
            //$('#vendorservicelist').prop("disabled", false);
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