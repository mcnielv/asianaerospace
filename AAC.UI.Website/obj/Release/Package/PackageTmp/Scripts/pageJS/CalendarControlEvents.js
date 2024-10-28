var calendarcontrolevents = {
    ddlaircraftOnChange : function(url){
        $('#ddlAircrafts').change(function () {
            $('#ddlRegistrations')
                    .find('option')
                    .remove()
                    .end()
                    .append('<option value="0">Select Registration #</option>')
                    .val('whatever');
            
            $.post(url, { aircraftid: $(this).val() },
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
                }, 'json');
        });
    },
    ddlcrewsOnChange: function () {
        $('#ddlcrews').change(function () {
            var str = "";
            $('#ddlcrews option:selected').each(function () {
                str += $(this).text() + ",";
            });
            return str;
        })
        .change();
    },
    getcrewids: function () {
        var str = '';
        $('#ddlcrews option:selected').each(function () {
            str += $(this).val() + ',';
        });
        return str;
    },
    cleardata: function () {
        $('#txtTitle').val('');
        $('#txtStart').val('');
        $('#txtEnd').val('');
        $('#txtWaitingStart').val('');
        $('#txtWaitingEnd').val('');
        $('#txtNotes').val('');
        $('#txtPassenger').val('');
        $('#txtFlightInfo').val('');
        $('#txtTechstops').val('');
        $('#txtEtc').val('');
        $('#ddlAircrafts').val('0');
        $('#ddlRouteStart').val('0');
        $('#ddlRouteDestination').val('0');
        $('#ddlRouteEnd').val('0');
        $('#ddlPilot').val('0');
        $('#ddlCoPilot').val('0');
        $('#ddlcrews').val('0');
        $('#txtID').val('0');
        $('#ddlRegistrations')
             .find('option')
             .remove()
             .end()
             .append('<option value="0">Select Registration #</option>')
             .val('whatever')
             .val('0');
    },
    savedata: function (addUrl,updateUrl) {
        $('#btnSave').click(function () {            
            var msg = '';
            //validate data first
            var title = $.trim($('#txtTitle').val());
            var aircraftid = $.trim($('#ddlAircrafts').val());
            var registrationid = $.trim($('#ddlRegistrations').val());
            var start = $.trim($('#txtStart').val());
            var end = $.trim($('#txtEnd').val());
            var routestart = $.trim($('#ddlRouteStart').val());
            var destination = $.trim($('#ddlRouteDestination').val());
            var routeend = $.trim($('#ddlRouteEnd').val());
            var pilot = $.trim($('#ddlPilot').val());
            var copilot = $.trim($('#ddlCoPilot').val());
            var crewids = $.trim(calendarcontrolevents.getcrewids());
            var notes = $.trim($('#txtNotes').val());
            var passenger = $.trim($('#txtPassenger').val());
            var flightinfor = $.trim($('#txtFlightInfo').val());
            var techstops = $.trim($('#txtTechstops').val());
            var etc = $.trim($('#txtEtc').val());
            var start2 = $.trim($('#txtWaitingStart').val());
            var end2 = $.trim($('#txtWaitingEnd').val());

            if (title == '') { msg += 'Title is required. <br />' }
            if (aircraftid == '0') { msg += 'Please select Aircraft. <br />' }
            if (registrationid == '0') { msg += 'Please select aircraft registration. <br />' }
            if (start == '') { msg += 'Flight start date and time is required. <br />' }
            if (end == '') { msg += 'Flight end date and time is required. <br />' }
            if (start2 == '') { msg += 'Waiting start date and time is required. <br />' }
            if (end2 == '') { msg += 'Waiting end date and time is required. <br />' }
            if (routestart == '0') { msg += 'Route start is required. <br />' }
            if (destination == '0') { msg += 'Destination is required. <br />' }
            if (routeend == '0') { msg += 'Route end is required. <br />' }
            if (pilot == '0') { msg += 'Please select pilot. <br />' }
            if (copilot == '0') { msg += 'Please select co-pilot. <br />' }
            if (crewids == '') { msg += 'Please select atleast one crew. <br />' }
            if (passenger == '') { msg += 'Passenger is required. <br />' }
            if (flightinfor == '') { msg += 'Flight info is required. <br />' }
            if (techstops == '') { msg += 'Techical stops is required. <br />' }
       
            if (msg == '') {
                $.confirm({
                    closeIcon: true,
                    title: 'AAC Data Monitoring',
                    content: 'Save Schedule?',
                    confirmButton: 'Yes',
                    cancelButton: 'No',
                    confirm: function () {
                        var id = $('#txtID').val();
                        //alert(crewids);
                        var model = {
                            ID: id,
                            Title: title,
                            AircraftID: aircraftid,
                            RegistrationID: registrationid,
                            Start: start,
                            End: end,
                            RouteStartID: routestart,
                            DestinationID: destination,
                            RouteEndID: routeend,
                            PilotID: pilot,
                            CopilotID: copilot,
                            CrewIDs: crewids,
                            Notes: notes,
                            Passengers: passenger,
                            FlightInfo: flightinfor,
                            TechnicalStops: techstops,
                            ETC: etc,
                            IsChangeSchedOnly: 0,
                            WaitingStart: start2,
                            WaitingEnd: end2
                        };
                        if (id == '0') {
                            //add
                            $.post(addUrl, model, function (data) {
                                $('#calendar').fullCalendar('refetchEvents');
                                $.alert({
                                    closeIcon: true,
                                    title: 'AAC Data Monitoring',
                                    content: data.Message
                                });
                                $('#myModal').modal('toggle');
                            }, 'json');
                        }
                        else {
                            //edit
                            $.post(updateUrl, model, function (data) {
                                $('#calendar').fullCalendar('refetchEvents');
                                $.alert({
                                    closeIcon: true,
                                    title: 'AAC Data Monitoring',
                                    content: data.Message
                                });
                                $('#myModal').modal('toggle');
                            }, 'json');
                        }
                       
                    }
                });               
            }
            else {
                $.alert({
                    closeIcon: true,
                    title: 'AAC Data Monitoring',
                    content: msg
                });
            }
        });
    }
};