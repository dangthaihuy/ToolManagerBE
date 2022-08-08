var DashboardGlobal = {
    getStatisticsAppWeek: function () {
        GetAndDrawAppWeekChartData();
    },
    getStatisticsWeek: function () {
        GetAndDrawWeekChartData();
    },
    getTodayTasks: function () {
        GetTodayTasks();
    },
    getNotifications: function () {
       GetNotifications();
    },
    notifPage: 1,
    notifPagingEnded: false
};

$(function () {
    $('#WidgetNotif').on('scroll', function () {
        if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
            DashboardGlobal.getNotifications();
        }
    });

    //setTimeout(function () {
    //    DashboardGlobal.getStatisticsWeek();
    //}, 500);   

    //setTimeout(function () {
    //    DashboardGlobal.getStatisticsAppWeek();
    //}, 1000);

    //setTimeout(function () {
    //    DashboardGlobal.getTodayTasks();
    //}, 1500);

    setTimeout(function () {
        DashboardGlobal.getNotifications();
    }, 500);
});

function GetAndDrawWeekChartData() {
    var params = $.extend({}, doAjax_params_default);
    params['url'] = 'Home/DashBoardStatisticsByWeek';
    params['requestType'] = 'POST';
    params['showloading'] = false;
    params['data'] = {};
    params['dataType'] = "json";
    params['successCallbackFunction'] = function (result) {
        if (result) {
            //Chart by week
            DrawWeekLineChart(result);
        }
    };
    doAjax(params);
}

function GetAndDrawAppWeekChartData() {
    var params = $.extend({}, doAjax_params_default);
    params['url'] = 'Home/DashBoardStatisticsAppByWeek';
    params['requestType'] = 'POST';
    params['showloading'] = false;
    params['data'] = {};
    params['dataType'] = "json";
    params['successCallbackFunction'] = function (result) {
        if (result) {
            //Chart by week
            DrawAppWeekLineChart(result);
        }
    };
    doAjax(params);
}

function GetTodayTasks() {
    var params = $.extend({}, doAjax_params_default);
    params['url'] = 'Schedule/GetTodayTasks';
    params['requestType'] = 'POST';
    params['showloading'] = false;
    params['data'] = {};
    params['dataType'] = "json";
    params['successCallbackFunction'] = function (result) {
        if (result) {
            $("#WidgetTodayTasks").html(result.html).fadeIn();            
            $("#WidgetTodayTasks").fadeIn();            
        }
    };
    doAjax(params);
}

//function GetNotifications() {
//    var page = parseInt($("#WidgetNotif").data("page"));
//    var isEnded = $("#WidgetNotif").data("ended");
//    if (isNaN(page)) {
//        page = 0;
//    }
//    page = page + 1;
//    $("#WidgetNotif").attr("data-page", page);

//    if (isEnded) {
//        return false;
//    }

//    showItemLoading("#WidgetNotif");
//    var params = $.extend({}, doAjax_params_default);
//    params['url'] = 'AuthedGlobal/GetDashboardNotification';
//    params['requestType'] = 'POST';
//    params['showloading'] = false;
//    params['data'] = { page: page};
//    params['dataType'] = "json";
//    params['successCallbackFunction'] = function (result) {
//        if (result) {
//            if (!result.success) {
//                $("#WidgetNotif").attr("data-ended", true);

//                if (page === 0 || page === 1 || isNaN(page)) {
//                    $("#WidgetNotif").append(result.html);
//                }
//            }
//            else {
//                $("#WidgetNotif").append(result.html); 
//            }          
//        }

//        if (page === 0 || page === 1 || isNaN(page)) {
//            $("#WidgetNotif").fadeIn();
//        } 

//        hideItemLoading("#WidgetNotif");
//    };
//    doAjax(params);
//}

function GetNotifications() {
    var isEnded = DashboardGlobal.notifPagingEnded;

    if (isEnded) {
        return false;
    }

    if (!isEnded) {
        $("#WidgetNotif").areaLoading();
        var params = $.extend({}, doAjax_params_default);

        params['url'] = 'AuthedGlobal/GetDashboardNotification';
        params['requestType'] = 'POST';
        params['showloading'] = false;
        params['data'] = { page: DashboardGlobal.notifPage };
        params['dataType'] = "json";
        params['successCallbackFunction'] = function (result) {
            if (result) {
                if (!result.html) {
                    DashboardGlobal.notifPagingEnded = true;

                    if (DashboardGlobal.notifPage === 0 || DashboardGlobal.notifPage === 1) {
                        $("#WidgetNotif").height = 50;
                        $("#WidgetNotif").html("<div class='text-center'><span class='text-danger'>" + LanguageDic['LB_HAS_NO_NOTIFICATION'] +" !</span></div>");
                    }
                }
                else {
                    if (DashboardGlobal.notifPage === 0 || DashboardGlobal.notifPage === 1) {
                        $("#WidgetNotif").html(result.html);
                    } else {
                        $("#WidgetNotif").append(result.html);
                    }

                    $(".btn-all-notif").removeClass("hidden");
                }
            }

            //if (page === 0 || page === 1 || isNaN(page)) {
            //    $("#WidgetNotif").fadeIn();
            //}

            DashboardGlobal.notifPage++;

            $("#WidgetNotif").areaLoading();
        };
    }

    doAjax(params);
}

function DrawWeekLineChart(rootData) {
    var DAYS = [
        LanguageDic['LB_DATE_MONDAY'],
        LanguageDic['LB_DATE_TUESDAY'],
        LanguageDic['LB_DATE_WEDNESDAY'],
        LanguageDic['LB_DATE_THUSDAY'],
        LanguageDic['LB_DATE_FRIDAY'],
        LanguageDic['LB_DATE_SATURDAY'],
        LanguageDic['LB_DATE_SUNDAY']
    ];

    //var progress = document.getElementById('animationProgress');
    var color = Chart.helpers.color;
    Chart.defaults.global.animation.duration = 3000;
    //var note = LanguageDic['LB_UPDATE'] + ': UPDATED_AT, ' + LanguageDic['LB_UPDATE_NEXT_TIME'] + ': UPDATE_NEXT';
    //note = note.replace('UPDATED_AT', rootData.UpdatedTime).replace('UPDATE_NEXT', rootData.NextUpdate);

    var note = LanguageDic['LB_DATA_DAILY_UPDATE_AUTO'];
    var dateRangeStr = rootData.FromDateStr + " - " + rootData.ToDateStr;

    var lineChartData = {
        labels: DAYS,
        datasets: [
        {
            label: LanguageDic['LB_JOB_STATUS_PUBLIC'] + ' (' + rootData.InWeekData.successTotal +')',
            backgroundColor: 'rgba(0, 0, 0, 0)',
            borderColor: window.chartColors.blue,
            borderWidth: 2,
            data: rootData.InWeekData.successData
        },
        {
            label: LanguageDic['LB_JOB_STATUS_DRAFT'] + ' (' + rootData.InWeekData.processingTotal + ')',
            backgroundColor: 'rgba(0, 0, 0, 0)',
            borderColor: window.chartColors.orange,
            borderWidth: 2,
            data: rootData.InWeekData.processingData
        },
        {
            label: LanguageDic['LB_JOB_STATUS_PENDING'] + ' (' + rootData.InWeekData.failedTotal + ')',
            backgroundColor: 'rgba(0, 0, 0, 0)',
            borderColor: window.chartColors.red,
            borderWidth: 2,
            data: rootData.InWeekData.failedData
            //hidden: true,
        }
        ]

    };
    var ctx = document.getElementById("WeekStatisticsChartCanvas").getContext("2d");

    $("#WeekStatisticsChart").fadeIn(1000);
    $("#WeekStatisticsChart").find(".m-chart-title").append(": " + dateRangeStr);

    window.myBar = new Chart(ctx, {
        type: 'line',
        data: lineChartData,
        options: {
            responsive: true,
            maintainAspectRatio: false,
            legend: {
                position: 'top'
            },
            title: {
                display: true,
                text: LanguageDic['LB_STATISTICS_JOB_IN_WEEK']
            },
            scales: {
                xAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        padding: 20,
                        fontStyle: "bold",
                        fontColor: "#ff1818",
                        labelString: note
                    }
                }],
                yAxes: [{
                    ticks: {
                        beginAtZero: true,
                        //callback: function (value) { if (Number.isInteger(value)) { return value; } },
                        //stepSize: 1
                    },
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: LanguageDic['LB_JOB_COUNT']
                    }
                }]
            },
            //animation: {
            //    duration: 1000,
            //    onProgress: function (animation) {
            //        progress.value = animation.currentStep / animation.numSteps;
            //    },
            //    onComplete: function () {
            //        window.setTimeout(function () {
            //            progress.value = 0;
            //        }, 1000);

            //        progress.className = "hidden";
            //    }
            //}
        }
    });
}

function DrawAppWeekLineChart(rootData) {
    var DAYS = [
        LanguageDic['LB_DATE_MONDAY'],
        LanguageDic['LB_DATE_TUESDAY'],
        LanguageDic['LB_DATE_WEDNESDAY'],
        LanguageDic['LB_DATE_THUSDAY'],
        LanguageDic['LB_DATE_FRIDAY'],
        LanguageDic['LB_DATE_SATURDAY'],
        LanguageDic['LB_DATE_SUNDAY']
    ];

    //var progress = document.getElementById('animationProgress');
    var color = Chart.helpers.color;
    Chart.defaults.global.animation.duration = 3000;
    //var note = LanguageDic['LB_UPDATE'] + ': UPDATED_AT, ' + LanguageDic['LB_UPDATE_NEXT_TIME'] + ': UPDATE_NEXT';
    //note = note.replace('UPDATED_AT', rootData.UpdatedTime).replace('UPDATE_NEXT', rootData.NextUpdate);
    var note = LanguageDic['LB_DATA_DAILY_UPDATE_AUTO'];
    var dateRangeStr = rootData.FromDateStr + " - " + rootData.ToDateStr;

    var lineChartData = {
        labels: DAYS,
        datasets: [
            {
                label: LanguageDic['LB_APPLICATION_STATUS_AWAITING'] + ' (' + rootData.InWeekData.waitingTotal +')',
                backgroundColor: color(window.chartColors.orange).alpha(0.5).rgbString(),
                borderColor: window.chartColors.orange,
                borderWidth: 2,
                data: rootData.InWeekData.waitingData
            },
            {
                label: LanguageDic['LB_APPLICATION_STATUS_APPROVED'] + ' (' + rootData.InWeekData.approvedTotal + ')',
                backgroundColor: color(window.chartColors.blue).alpha(0.5).rgbString(),
                borderWidth: 2,
                data: rootData.InWeekData.approvedData
            },
            {
                label: LanguageDic['LB_APPLICATION_STATUS_AGENCY_IGNORED'] + ' (' + rootData.InWeekData.ignoredTotal + ')',
                backgroundColor: color(window.chartColors.red).alpha(0.5).rgbString(),
                borderColor: window.chartColors.red,
                borderWidth: 2,
                data: rootData.InWeekData.ignoredData
                //hidden: true,
            }
        ]

    };
    var ctx = document.getElementById("AppWeekStatisticsChartCanvas").getContext("2d");

    $("#AppWeekStatisticsChart").fadeIn(1000);
    $("#AppWeekStatisticsChart").find(".m-chart-title").append(": " + dateRangeStr);

    window.myBar = new Chart(ctx, {
        type: 'bar',
        data: lineChartData,
        options: {
            responsive: true,
            maintainAspectRatio: false,
            legend: {
                position: 'top'
            },
            title: {
                display: true,
                text: LanguageDic['LB_STATISTICS_APP_IN_WEEK']
            },
            scales: {
                xAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        padding: 20,
                        fontStyle: "bold",
                        fontColor: "#ff1818",
                        labelString: note
                    }
                }],
                yAxes: [{
                    ticks: {
                        beginAtZero: true,
                        //callback: function (value) { if (Number.isInteger(value)) { return value; } }
                    },
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: LanguageDic['LB_APPLICATION_COUNT']
                    }
                }]
            },
            //animation: {
            //    duration: 1000,
            //    onProgress: function (animation) {
            //        progress.value = animation.currentStep / animation.numSteps;
            //    },
            //    onComplete: function () {
            //        window.setTimeout(function () {
            //            progress.value = 0;
            //        }, 1000);

            //        progress.className = "hidden";
            //    }
            //}
        }
    });
}