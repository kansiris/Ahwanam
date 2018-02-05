function FetchEventAndRenderCalendar() {
    events = [];
    var date = new Date(),
        d = date.getDate(),
        m = date.getMonth() + 1,
        y = date.getFullYear()
    //alert(date + ',date:' + d + ',month:' + m + ',year:' + y);
    $.ajax({
        type: "GET",
        url: '/VendorCalendar/GetDates?id=' + $('#vid').val(),
        success: function (data) {
            $.each(data, function (i, v) {
                events.push({
                    eventID: v.Id,
                    title: v.Title,
                    description: v.Description,
                    start: moment(v.StartDate).subtract(12, 'hours').subtract(30, 'minutes'),
                    end: moment(v.EndDate).subtract(12, 'hours').subtract(30, 'minutes').add(1, 'day'),
                    color: v.Color,
                    allDay: v.IsFullDay,
                    type: v.Type
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
        timeFormat: 'H(:mm)',
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
                    $description.append($('<p/>').html('<b>Start:</b>' + moment(data.StartDate).format("DD/MMM/YYYY hh:mm A")));
                    if (calEvent.end != null) {
                        $description.append($('<p/>').html('<b>End:</b>' + moment(data.EndDate).format("DD/MMM/YYYY hh:mm A")));
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
            selectedEvent = {
                eventID: 0,
                title: '',
                description: '',
                start: start,
                end: end,
                allDay: false,
                color: '',
                type: ''
            };
            openAddEditForm();
            $('#calendar').fullCalendar('unselect');
        },
        editable: true,
        eventDrop: function (event) {
            var data = {
                EventID: event.eventID,
                Subject: event.title,
                Start: event.start.format('DD/MMM/YYYY HH:mm'),
                End: event.end.format('DD/MMM/YYYY HH:mm'),
                Description: event.description,
                ThemeColor: event.color,
                IsFullDay: event.allDay,
                type: event.type
            };
            SaveEvent(data);
        }
    })
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
            data: { 'id': selectedEvent.eventID },
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
    format: 'd/m/y H:m'
});


function openAddEditForm() {
    if (selectedEvent != null) {
        $('#hdEventID').val(selectedEvent.eventID);
        $('#subject').val(selectedEvent.title);
        $('#startdate').val(moment(selectedEvent.StartDate).format("DD/MMM/YYYY hh:mm A"));
        $('#chkIsFullDay').val(selectedEvent.allDay);
        //$('#chkIsFullDay').change();
        $('#enddate').val(moment(selectedEvent.EndDate).format("DD/MMM/YYYY hh:mm A") != null ? moment(selectedEvent.EndDate).format("DD/MMM/YYYY hh:mm A") : '');
        $('#description').val(selectedEvent.description);
        $('#color').val(selectedEvent.color);
        $('#type').val(selectedEvent.type);
    }
    $('#myModal').modal('hide');
    $('#CalenderModalNew').modal();
}
$('#btnSave').click(function () {
    //Validation/
    if ($('#subject').val().trim() == "") {
        alert('Subject required');
        return;
    }
    if ($('#startdate').val().trim() == "") {
        alert('Start date required');
        return;
    }
    if ($('#enddate').val().trim() == "") {
        alert('End date required');
        return;
    }
    else {
        var startDate = moment($('#enddate').val(), "d/m/y H:m").toDate();
        var endDate = moment($('#txtEnd').val(), "d/m/y H:m").toDate();
        if (startDate > endDate) {
            alert('Invalid end date');
            return;
        }
    }

    var data = {
        Id: $('#hdEventID').val(),
        Title: $('#subject').val().trim(),
        StartDate: $('#startdate').val().trim(),
        EndDate: $('#enddate').val().trim(),
        Description: $('#description').val(),
        Color: $('#color').val(),
        IsFullDay: $('#chkIsFullDay').val(),
        Type: $('#type').val(),
        VendorId: $('#vid').val(),
        Servicetype: 'Venue',
    }
    SaveEvent(data);
    // call function for submit data to the server
})


function SaveEvent(data) {
    $.ajax({
        type: "POST",
        url: "/VendorCalendar/SaveEvent",
        data: data,
        success: function (data) {
            if (data.status) {
                //Refresh the calender
                FetchEventAndRenderCalendar();
                $('#CalenderModalNew').modal('hide');
            }
        },
        error: function () {
            alert('Failed');
        }
    })
}