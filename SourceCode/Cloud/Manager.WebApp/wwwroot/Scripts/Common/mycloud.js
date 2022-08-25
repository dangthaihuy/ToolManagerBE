// Declare a proxy to reference the hub.
var commonGateHub = $.connection.commonGateHub;
$.connection.hub.url = _mCloud + "/signalr/hubs/";
$.connection.hub.start().done(function () {
    console.log('Connected to MyCloud server successfully');
    registerGateHubEvents(commonGateHub);

    registerGateHubClientMethods(commonGateHub);
}).fail(function (error) {
    console.log("Cannot connect to MyCloud server because: " + error);
});

//Register all client methods
registerGateHubClientMethods(commonGateHub);

function registerGateHubEvents(commonGateHub) {
    commonGateHub.server.agencyConnect(_mInfo);
}

function registerGateHubClientMethods(commonGateHub) {
    // On received new notif
    commonGateHub.client.updateNotification = function (notifId) {
        AuthenticatedGlobal.counterUpNotification();
    };
}