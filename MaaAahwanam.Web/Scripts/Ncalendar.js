function FetchEventAndRenderCalendar(id, subvid) {
    events = [];
    var date = new Date(),
        d = date.getDate(),
        m = date.getMonth() + 1,
        y = date.getFullYear()
    $.ajax({
        type: "GET",
        url: '/NVendorCalendar/GetDates?id=' + id + '&&vid=' + subvid,
        success: function (data) {
            $.each(data, function (i, v) {
                var start1, end1 = null;
                if (moment(v.EndDate).format("A") == 'AM') {
                    end1 = moment(v.EndDate)
                }
                else {
                    end1 = moment(v.EndDate);
                }
                events.push({
                    eventID: v.Id,
                    title: v.Title,
                    description: v.Description,
                    start: moment(v.StartDate),
                    end: end1,//moment(v.EndDate),//.subtract(12, 'hours').subtract(30, 'minutes'),
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
                url: '/NVendorCalendar/GetParticularDate?id=' + selectedEvent.eventID,
                success: function (data) {
                    selectedEvent = data;
                    $('#myModal #eventTitle').text(data.Title);
                    var $description = $('<div/>');
                    $description.append($('<p/>').html('<b>Start:</b>' + moment(data.StartDate).format("DD/MMM/YYYY hh:mm A")));
                    if (data.IsFullDay == "False" || data.IsFullDay == null) {
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
            var check = moment(start).format("DD/MMM/YYYY hh:mm A"); //$('#calendar').fullCalendar.formatDate(start, 'yyyy-MM-dd');
            var today = moment(new Date()).format("DD/MMM/YYYY hh:mm A"); //$('#calendar').fullCalendar.formatDate(new Date(), 'yyyy-MM-dd');
            //if (check < today) {
            //    alert("Can't add past dates");
            //}
            //else {
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
            //}


            //alert(moment(start).format('DD/MMM/YYYY HH:mm') + ',' + moment(end).format('DD/MMM/YYYY HH:mm'));
            
        },
        editable: true,
        eventDrop: function (event) {
            var data = {
                Id: event.eventID,
                Title: event.title,
                StartDate: event.start.format('DD/MMM/YYYY hh:mm A'),
                EndDate: event.end != null ? event.end.format('DD/MM/YYYY hh:mm A') : event.start.format('DD/MMM/YYYY hh:mm A'),
                Description: event.description,
                Color: event.color,
                IsFullDay: event.allDay,
                Type: event.type,
                VendorId: $('#vendorid').val(),
                Servicetype: event.servicetype
            };
            //alert("Start Date:" + data.StartDate + "End Date:" + event.end.subtract(24, 'hours').format('DD/MMM/YYYY hh:mm A'));
            SaveEvent(data);
        },
        viewRender: function (currentView) {
            var minDate = moment()
            // Past
            if (minDate >= currentView.start && minDate <= currentView.end) {
                $(".fc-prev-button").prop('disabled', true);
                $(".fc-prev-button").addClass('fc-state-disabled');
            }
            else {
                $(".fc-prev-button").removeClass('fc-state-disabled');
                $(".fc-prev-button").prop('disabled', false);
            }
        }
    })
    //$('body.dashboard').css('padding-right', '0px');
}

$('#btnEdit').click(function () {
    //Open modal dialog for edit event
    openAddEditForm();
})

$('#btnDelete').click(function () {
    var id = $('#vendorid').val();
    var vid = $('#vendorsubid').val();
    if (selectedEvent != null && confirm('Are you sure?')) {
        $.ajax({
            type: "POST",
            url: '/NVendorCalendar/DeleteEvent',
            data: { 'id': selectedEvent.Id },
            success: function (data) {
                if (data.status) {
                    //Refresh the calender
                    FetchEventAndRenderCalendar(id, vid);
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
    format: 'DD/MM/YYYY hh:mm A'
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
        $('#subject').val(selectedEvent.Title);
        $('#chkIsFullDay').val(selectedEvent.IsFullDay);
        if (selectedEvent.eventID != 0) {
            $('#startdate').val(moment(selectedEvent.StartDate).format("DD/MM/YYYY hh:mm A"));
            $('#enddate').val(moment(selectedEvent.EndDate).format("DD/MM/YYYY hh:mm A") != null ? moment(selectedEvent.EndDate).format("DD/MM/YYYY hh:mm A") : '');
        }
        else {
            $('#startdate').val(moment(selectedEvent.start).format("DD/MM/YYYY hh:mm A"));
            $('#enddate').val(moment(selectedEvent.end).format("DD/MM/YYYY hh:mm A") != null ? moment(selectedEvent.end).format("DD/MM/YYYY hh:mm A") : '');
        }
        if (selectedEvent.IsFullDay == "True") {
            $('#divEndDate').hide();
            $('#enddate').val('');
        }
        else {
            $('#divEndDate').show();
        }
        $('#description').val(selectedEvent.Description);
        $('#color').val(selectedEvent.Color);
        $('#type').val(selectedEvent.Type);
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

    if ($('#chkIsFullDay').val() == "" || $('#chkIsFullDay').val() == null) {
        alert('Is Full Day event required');
        return;
    }
    else {
        if ($('#chkIsFullDay').val() == "True") {
            $('#enddate').val($('#startdate').val());
        }
    }


    if ($('#enddate').val().trim() == "") {
        alert('End date required');
        return;
    }
    else {
        var startDate = moment($('#startdate').val(), "DD/MM/YYYY hh:mm A").toDate();
        var endDate = moment($('#enddate').val(), "DD/MM/YYYY hh:mm A").toDate();
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
        VendorId: $('#vendorid').val(),
        Servicetype: 'Venue',
    }
    SaveEvent(data);
    // call function for submit data to the server
})


function SaveEvent(data) {
    var id = $('#vendorid').val();
    var vid = $('#vendorsubid').val();
    //alert(id);
    //alert(vid);
    $.ajax({
        type: "POST",
        url: "/NVendorCalendar/SaveEvent?vid=" + vid.toString() + "",
        data: data,
        success: function (data) {
            if (data.status) {
                //Refresh the calender
                FetchEventAndRenderCalendar(id, vid);
                $('#CalenderModalNew').modal('hide');
                //location.reload();
            }
        },
        error: function () {
            alert('Failed');
        }
    })
}
