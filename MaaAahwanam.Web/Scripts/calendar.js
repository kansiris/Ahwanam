function FetchEventAndRenderCalendar(subvid) {
    events = [];
    var date = new Date(),
        d = date.getDate(),
        m = date.getMonth() + 1,
        y = date.getFullYear()
    $.ajax({
        type: "GET",
        url: '/VendorCalendar/GetDates?id=' + $('#vid').val() + '&&vid=' + $('#vsid').val(),
        success: function (data) {
            $.each(data, function (i, v) {
                //var start1, end1 = null;
                //if (moment(v.EndDate).subtract(12, 'hours').subtract(30, 'minutes').format("A") == 'AM') {
                //    end1 = moment(v.EndDate).add(12, 'hours').add(30, 'minutes')
                //}
                //else {
                //    end1 = moment(v.EndDate);
                //}
                events.push({
                    eventID: v.Id,
                    title: v.Title,
                    description: v.Description,
                    start: moment(v.StartDate).subtract(12, 'hours').subtract(30, 'minutes'),
                    end: moment(v.EndDate),//.subtract(12, 'hours').subtract(30, 'minutes'),
                    color: v.Color,
                    allDay: v.IsFullDay,
                    type: v.Type,
                    servicetype: v.Servicetype
                });
            })

            GenerateCalender(events);
        },
        error: function (error) {
            alert('failed');
        }
    })
}

function GenerateCalender(events) {
    $('#calendar').fullCalendar('destroy');
    $('#calendar').fullCalendar({
        contentHeight: 600,
        timezone: 'India Standard Time',
        defaultDate: new Date(),
        timeFormat: 'h(:mm)a',
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,basicWeek,basicDay'
        },
        eventLimit: true,
        eventColor: '#378006',
        events: events,
        eventClick: function (calEvent, jsEvent, view) {
            selectedEvent = calEvent;
            $.ajax({
                type: "GET",
                url: '/VendorCalendar/GetParticularDate?id=' + selectedEvent.eventID,
                success: function (data) {
                    selectedEvent = data;
                    $('#myModal #eventTitle').text(data.Title);
                    var $description = $('<div/>');
                    $description.append($('<p/>').html('<b>Start:</b>' + moment(data.StartDate).subtract(12, 'hours').subtract(30, 'minutes').format("DD/MMM/YYYY")));
                    if (data.IsFullDay == "False" || data.IsFullDay == null) {
                        $description.append($('<p/>').html('<b>End:</b>' + moment(data.EndDate).subtract(12, 'hours').subtract(30, 'minutes').format("DD/MMM/YYYY")));
                    }
                    $description.append($('<p/>').html('<b>Description:</b>' + data.Description));
                    $('#myModal #pDetails').empty().html($description);
                    $('#myModal').modal();
                },
                error: function (error) {
                    alert('Failed to Fetch data');
                }
            })
        },
        selectable: true,
        select: function (start, end) {
            //alert(moment(start).format('DD/MMM/YYYY HH:mm') + ',' + moment(end).format('DD/MMM/YYYY HH:mm'));
            $('#divEndDate').show();
            selectedEvent = {
                eventID: 0,
                title: '',
                description: '',
                start: start,
                end: end,
                allDay: false,
                color: '',
                type: '',
                servicetype: ''
            };
            openAddEditForm();
            $('#calendar').fullCalendar('unselect');
        },
        editable: true,
        droppable: true, // this allows things to be dropped onto the calendar
        drop: function (date) {
            var type = $(this).text();
            var color = ''; var category = ''; var timeslot = '';
            if (type == 'Morning Availability') { color = 'Red'; category = 'Availability'; timeslot = 'Morning'; }
            else if (type == 'Evening Availability') { color = 'Green'; category = 'Availability'; timeslot = 'Evening'; }
            
            var data = {
                Id: $('#hdEventID').val(),
                Title: type,
                StartDate: moment(date).format('DD/MM/YYYY hh:mm:ss'),
                EndDate: moment(date).format('DD/MM/YYYY hh:mm:ss'),
                Description: type,
                Color: color,
                IsFullDay: timeslot,
                Type: category,
                VendorId: $('#vid').val(),
                Servicetype: 'Venue',
            }           
            SaveEvent(data);
        },
        eventDrop: function (event) {
            var data = {
                Id: event.eventID,
                Title: event.title,
                StartDate: event.start.format('DD/MMM/YYYY'),
                EndDate: event.end != null ? event.end.format('DD/MM/YYYY') : null,
                Description: event.description,
                Color: event.color,
                IsFullDay: event.allDay,
                Type: event.type,
                VendorId: $('#vid').val(),
                Servicetype: event.servicetype
            };
            //alert("Start Date:" + data.StartDate + "End Date:" + event.end.subtract(24, 'hours').format('DD/MMM/YYYY hh:mm A'));
            SaveEvent(data);
        }
    })
    //$('body.dashboard').css('padding-right', '0px');
}

$('#btnEdit').click(function () {
    //Open modal dialog for edit event
    openAddEditForm();
})

$('#btnDelete').click(function () {
    if (selectedEvent != null && confirm('Are you sure?')) {
        $.ajax({
            type: "POST",
            url: '/VendorCalendar/DeleteEvent',
            data: { 'id': selectedEvent.Id },
            success: function (data) {
                if (data.status) {
                    //Refresh the calender
                    FetchEventAndRenderCalendar();
                    $('#myModal').modal('hide');
                }
            },
            error: function () {
                alert('Failed');
            }
        })
    }
})

$('#startdate,#enddate').datetimepicker({
    format: 'DD/MM/YYYY',
    minDate:new Date()
    //step: 15
});


$('#chkIsFullDay').change(function () {
    if ($(this).val() == "True") {
        $('#divEndDate').hide();
        $('#enddate').val('');
    }
    else {
        $('#divEndDate').show();
    }
});

function openAddEditForm() {
    if (selectedEvent != null) {
        $('#hdEventID').val(selectedEvent.Id);
        //$('#subject').val(selectedEvent.Title);
        $('#chkIsFullDay').val(selectedEvent.IsFullDay);
        if (selectedEvent.eventID != 0) {
            $('#startdate').val(moment(selectedEvent.StartDate).subtract(12, 'hours').subtract(30, 'minutes').format("DD/MM/YYYY"));
            $('#enddate').val(moment(selectedEvent.EndDate).subtract(12, 'hours').subtract(30, 'minutes').format("DD/MM/YYYY") != null ? moment(selectedEvent.EndDate).subtract(12, 'hours').subtract(30, 'minutes').format("DD/MM/YYYY") : '');
        }
        else {
            $('#startdate').val(moment(selectedEvent.start).format("DD/MM/YYYY"));
            $('#enddate').val(moment(selectedEvent.end).format("DD/MM/YYYY") != null ? moment(selectedEvent.end).format("DD/MM/YYYY") : '');
        }
        //if (selectedEvent.IsFullDay == "True") {
        //    $('#divEndDate').hide();
        //    $('#enddate').val('');
        //}
        //else {
        //    $('#divEndDate').show();
        //}
        $('#description').val(selectedEvent.Description);
        $('#color').val(selectedEvent.Color);
        $('#type').val(selectedEvent.Type);
        if (selectedEvent.Type == 'Packages') {
            $('#pkgdays').css('display', 'block');
            $('#title').val(selectedEvent.Title);
            $('#title1').val(selectedEvent.IsFullDay);
        }
        else {
            $('#pkgdays').css('display', 'none');
        }
    }
    $('#myModal').modal('hide');
    $('#CalenderModalNew').modal();
}
$('#btnSave').click(function () {
    //Validation/
    //if ($('#subject').val().trim() == "") {
    //    alert('Subject required');
    //    return;
    //}
    if ($('#startdate').val().trim() == "") {
        alert('Start date required');
        return;
    }
    
    //if ($('#chkIsFullDay').val() == "" || $('#chkIsFullDay').val() == null) {
    //    alert('Is Full Day event required');
    //    return;
    //}
    //else {
    //    if ($('#chkIsFullDay').val() == "True") {
    //        $('#enddate').val($('#startdate').val());
    //    }
    //}

   
    if ($('#enddate').val().trim() == "") {
        alert('End date required');
        return;
    }
    else {
        var startDate = moment($('#startdate').val(), "DD/MM/YYYY").toDate();
        var endDate = moment($('#enddate').val(), "DD/MM/YYYY").toDate();
        if (startDate > endDate) {
            alert('Invalid end date');
            return;
        }
    }

    var data = {
        Id: $('#hdEventID').val(),
        Title: $('#type').val(),//$('#subject').val().trim(),
        StartDate: $('#startdate').val().trim(),
        EndDate: $('#enddate').val().trim(),
        Description: $('#title').val(),
        Color: $('#color').val(),
        IsFullDay: $('#title1').val(),//$('#chkIsFullDay').val(),
        Type: $('#type').val(),
        VendorId: $('#vid').val(),
        Servicetype: 'Venue',
    }
    SaveEvent(data);
    // call function for submit data to the server
})


function SaveEvent(data) {
    var vid = $('#vsid').val();
    
    $.ajax({
        type: "POST",
        url: "/VendorCalendar/SaveEvent?vid=" + vid.toString() + "",
        data: data,
        success: function (data) {
            if (data.status) {
                //Refresh the calender
                FetchEventAndRenderCalendar(vid);
                $('#CalenderModalNew').modal('hide');
                $('#btnSave').attr("disabled", "disabled");
                //location.reload();
            }
        },
        error: function () {
            alert('Failed');
        }
    })
}

$('#refresh').click(function () {
    var vid = $('#vsid').val();
    FetchEventAndRenderCalendar(vid);
});