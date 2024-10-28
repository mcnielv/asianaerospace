var calendarsetup = {
    textboxdatetime: function (dateNow) {
        //**********************
        //START DATE TIME PICKER
        $.datetimepicker.setLocale('en');
        $('#txtStart').datetimepicker({
            dateFormat: 'yyyy-MM-dd',
            dayOfWeekStart: 1,
            lang: 'en',
            disabledDates: ['1986/01/08', '1986/01/09', '1986/01/10'],
            startDate: dateNow//,'1986/01/05',
            //minDateTime: dateNow,
            //minDate: dateNow
        });
        $('#txtEnd').datetimepicker({
            dayOfWeekStart: 1,
            lang: 'en',
            disabledDates: ['1986/01/08', '1986/01/09', '1986/01/10'],
            startDate: dateNow//,'1986/01/05',
            //minDateTime: dateNow,
            //minDate: dateNow
        });
        $('#txtWaitingEnd').datetimepicker({
            dayOfWeekStart: 1,
            lang: 'en',
            disabledDates: ['1986/01/08', '1986/01/09', '1986/01/10'],
            startDate: dateNow//,'1986/01/05',
            //minDateTime: dateNow,
            //minDate: dateNow
        });
        $('#txtWaitingStart').datetimepicker({
            dayOfWeekStart: 1,
            lang: 'en',
            disabledDates: ['1986/01/08', '1986/01/09', '1986/01/10'],
            startDate: dateNow//,'1986/01/05',
            //minDateTime: dateNow,
            //minDate: dateNow
        });
        $('.some_class').datetimepicker();
        //END DATE TIME PICKER
        //********************
    },
    createcalendar: function (dateNow, jsonurl, detailurl, urlReloadRegistration, updateurl) {
        //*****************START    C A L E N D A R*********************************************************
        
        $('#calendar').fullCalendar({
            height: 650,
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'//'month,agendaWeek,agendaDay'
            },
            eventLimit: true,
            theme: true,
            defaultDate: dateNow, //'2016-05-12',
            businessHours: true, // display business hours
            editable: true, // ability to drag and drop data.
            dayClick: function (date, jsEvent, view) {
                calendarcontrolevents.cleardata();

                var today = new Date();
                var dt = new Date(date);
                var mos = (dt.getMonth() + 1);
                if (mos < 10)
                    mos = '0' + mos;
                var day = dt.getDate()
                if (day < 10)
                    day = '0' + day;
                //alert(mos.length)
                var strDtTime = dt.getFullYear() + '/' + mos + '/' + day + ' '
                    + today.getHours() + ':' + today.getMinutes();
                $('#txtStart').val(strDtTime);
                $('#txtEnd').val(strDtTime);
                $("#myModal").modal();
            },
            viewDisplay: function (view) {
                $('.fc-view').find('td').css('cursor', 'pointer');
            },
            //disableDragging: false,
            eventClick: function (calEvent, jsEvent, view) {
                $('#ddlRegistrations')
                    .find('option')
                    .remove()
                    .end()
                    .append('<option value="0">Select Registration #</option>')
                    .val('whatever');

                $.post(detailurl, { id: calEvent.id }, function (data) {
                    var aircraftID = data.AircraftID;
                    var regID = data.RegistrationID;
                   
                    $('#txtID').val(data.ID);
                    $('#txtTitle').val(data.Title);
                    $('#txtStart').val(data.Start);
                    $('#txtEnd').val(data.End);
                    $('#txtWaitingStart').val(data.WaitingStart);
                    $('#txtWaitingEnd').val(data.WaitingEnd);
                    $('#txtNotes').val(data.Notes);
                    $('#txtPassenger').val(data.Passengers);
                    $('#txtFlightInfo').val(data.FlightInfo);
                    $('#txtTechstops').val(data.TechnicalStops);
                    $('#txtEtc').val(data.ETC);
                    $('#ddlAircrafts').val(aircraftID);
                    $('#ddlRouteStart').val(data.RouteStartID);
                    $('#ddlRouteDestination').val(data.DestinationID);
                    $('#ddlRouteEnd').val(data.RouteEndID);
                    $('#ddlPilot').val(data.PilotID);
                    $('#ddlCoPilot').val(data.CopilotID);
                    $('#ddlcrews').val('0');
                    if (data.ScheduleCrews != null) {
                        //var selCrews = $('#ddlcrews');
                        var a = '';
                        var items = [];
                        var selected = $("#ddlcrews option:selected").map(function () { return this.value }).get();
                        $.each(data.ScheduleCrews, function (key, value) {
                            selected.push(value.CrewID);                           
                        });
                        $('#ddlcrews').val(selected);
                    }
                    
                    //setup ddlregistration
                    //alert(regID);
                     $('#ddlRegistrations')
                    .find('option')
                    .remove()
                    .end()
                    .append('<option value="0">Select Registration #</option>')
                    .val('whatever');
                              
                     
                     $.post(urlReloadRegistration, { aircraftid: aircraftID },
                        function (data) {
                            if ($.trim( data ) == ''){
                                $('#ddlRegistrations')
                                        .find('option')
                                        .remove()
                                        .end()
                                        .append('<option value="0">Select Registration #</option>')
                                        .val('whatever');
                            }
                            //alert(data)
                            $.each(data, function (key, value) {
                                $('#ddlRegistrations').append($('<option>', { value: value.ID, text: value.Name }));
                            });
                            $('#ddlRegistrations').val(regID);
                        }, 'json');
                    
                    //end registration setup
                    $("#myModal").modal();
                }, 'json');

            },//when event is clicked
            eventDrop: function (event, delta, revertFunc) {
                //var start = $('#calendar').fullCalendar('formatDate', event.start, "yyyy-MM-dd HH:mm");
                //var end = $('#calendar').fullCalendar('formatDate', event.end, "yyyy-MM-dd HH:mm");
                //alert(start);
                //alert(end)

                //if (!confirm("Are you sure about this change?")) {
                //    revertFunc();
                //}
                $.confirm({
                    closeIcon: true,
                    title: 'AAC Data Monitoring',
                    content: event.title + " was dropped on " + event.start.format() + '.<br /> Do you want to continue?' ,
                    confirmButton: 'Yes',
                    cancelButton: 'No',
                    confirm: function () {
                        var model = {
                            ID: event.id,
                            Start: event.start.format(),
                            IsChangeSchedOnly: true
                        };
                        //alert(updateurl)
                        $.post(updateurl, model, function (data) {
                            $('#calendar').fullCalendar('refetchEvents');
                            $.alert({
                                closeIcon: true,
                                title: 'AAC Data Monitoring',
                                content: data.Message
                            });
                        }, 'json');
                        
                    }
                });
                $('#calendar').fullCalendar('refetchEvents');
            },
            //events: [{ "title": "Taeng Yan", "start": "5/25/2016 6:03:10 PM" },
            //       { "title": "Taeng Yan 2", "start": "5/27/2016 6:03:12 PM" }]//}
            //events: jsonurl
            events: function (start, end, timezone, callback) {
                $.post(jsonurl, {}, function (doc) {
                    var events = [];
                    var obj = jQuery.parseJSON(doc);
                    $.each(obj, function (key, value) {
                        //alert(value.title);
                        events.push({
                            title: $(this).attr('title') + '\n [' + $(this).attr('aircrafname') + '-' + $(this).attr('registrationname') + '] \n',
                            start: $(this).attr('start'),
                            end: $(this).attr('end'),
                            id: $(this).attr('id'),
                            tip: 'Aircraft Regstration: \n [' + $(this).attr('aircrafname') + '-' + $(this).attr('registrationname') + '] \n'
                        });

                    });
                    callback(events);
                }, 'json');
            },
            eventRender: function (event, element) {
                element.attr('title', event.tip);
            }
        });
        //*****************E N D    C A L E N D A R*********************************************************
    },
    createcalendardelete: function (dateNow, jsonurl, deleteurl) {
        //*****************START    C A L E N D A R*********************************************************

        $('#calendar').fullCalendar({
            height: 650,
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'//'month,agendaWeek,agendaDay'
            },
            eventLimit: true,
            theme: true,
            defaultDate: dateNow, //'2016-05-12',
            businessHours: true, // display business hours
            editable: false,
            viewDisplay: function (view) {
                $('.fc-view').find('td').css('cursor', 'pointer');
            },
            eventClick: function (calEvent, jsEvent, view) {
                $.confirm({
                    closeIcon: true,
                    title: 'AAC Data Monitoring',
                    content: 'Delete Schedule?',
                    confirmButton: 'Yes',
                    cancelButton: 'No',
                    confirm: function () {
                        alert(calEvent.id)
                        $.post(deleteurl, { id: calEvent.id }, function (data) {
                            $('#calendar').fullCalendar('refetchEvents');
                            $.alert({
                                closeIcon: true,
                                title: 'AAC Data Monitoring',
                                content: data.Message
                            });
                        }, 'json');

                    }
                });

            },//when event is clicked
            events: function (start, end, timezone, callback) {
                $.post(jsonurl, {}, function (doc) {
                    var events = [];
                    var obj = jQuery.parseJSON(doc);
                    $.each(obj, function (key, value) {
                        //alert(value.title);
                        events.push({
                            title: $(this).attr('title') + '\n [' + $(this).attr('aircrafname') + '-' + $(this).attr('registrationname') + '] \n',
                            start: $(this).attr('start'),
                            id: $(this).attr('id'),
                            tip: 'Aircraft Regstration: \n [' + $(this).attr('aircrafname') + '-' + $(this).attr('registrationname') + '] \n'
                        });

                    });
                    callback(events);
                }, 'json');
            }
        });
        //*****************E N D    C A L E N D A R*********************************************************
    }
};