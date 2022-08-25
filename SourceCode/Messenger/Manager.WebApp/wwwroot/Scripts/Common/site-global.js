var AuthenticatedGlobal = {
    getNotifications: function (pageNum = 0) {
        if (pageNum <= 0) {
            pageNum = 1;
        }
        showItemLoading(".box-noti");

        var params = $.extend({}, doAjax_params_default);
        params['url'] = '/AuthedGlobal/GetNotification';
        params['requestType'] = 'POST';
        params['data'] = { page: pageNum };
        params['dataType'] = "json";
        params['showloading'] = false;

        params['successCallbackFunction'] = function (data) {
            var container = $(".box-noti");
            var btnControl = $("#btnViewNotification");
            var counterLabel = btnControl.find(".m-nav__link-badge");
            btnControl.find(".m-nav__link-icon").removeClass("m-animate-shake");
            btnControl.removeClass("shake-icon");
            counterLabel.removeClass("m-badge m-badge--danger");
            counterLabel.html("");

            if (data !== "" && data) {
                if (data.success) {
                    if (data.html) {
                        container.html(data.html);
                        container.addClass("loaded");
                        // $(".notif-dropdown").find("dropdown").addClass("open");
                    }
                }
                else {
                    _hasNoData = true;
                }
            }

            hideItemLoading(".box-noti");
        };
        doAjax(params);
    },
    getNotifCounter: function () {
        var btnControl = $("#btnViewNotification");
        if (btnControl.hasClass("shake-icon"))
            return false;

        var params = $.extend({}, doAjax_params_default);
        params['url'] = '/AuthedGlobal/GetNotifCount';
        params['requestType'] = 'POST';
        params['data'] = {};
        params['dataType'] = "json";
        params['showloading'] = false;

        params['successCallbackFunction'] = function (data) {
            if (data) {
                if (data.success) {
                    var counterLabel = btnControl.find(".m-nav__link-badge");

                    var notifCount = parseInt(data.data);
                    if (notifCount > 0) {
                        if (notifCount < 99) {
                            counterLabel.html(notifCount);
                        } else {
                            counterLabel.html("99+");
                        }

                        btnControl.addClass("shake-icon");
                        counterLabel.addClass("m-badge m-badge--danger");
                    } else {
                        btnControl.removeClass("shake-icon");
                        counterLabel.removeClass("m-badge m-badge--danger");
                    }
                }
            }
        };
        doAjax(params);
    },
    getNewNotification: function (id) {
        var params = $.extend({}, doAjax_params_default);
        params['url'] = '/Member/GetNewNotification';
        params['requestType'] = 'POST';
        params['data'] = { id: id };
        params['showloading'] = false;
        params['dataType'] = "html";

        params['successCallbackFunction'] = function (result) {
            $("body").append(result);
        };

        doAjax(params);
    },
    counterUpNotification: function () {
        var btnControl = $("#btnViewNotification");
        var counterLabel = btnControl.children(".m-nav__link-badge");
        var ht = counterLabel.html();
        var currentVal = 0;
        if (ht) {
            currentVal = parseInt(ht.replace("+", ""));
        }

        var notifCount = parseInt(currentVal + 1);
        if (notifCount > 0) {
            if (notifCount < 99) {
                counterLabel.html(notifCount);
            } else {
                counterLabel.html("99+");
            }

            btnControl.addClass("shake-icon");
            counterLabel.addClass("m-badge m-badge--danger");
        } else {
            btnControl.removeClass("shake-icon");
            counterLabel.removeClass("m-badge m-badge--danger");
        }

        var container = $(".box-noti");
        container.removeClass("loaded");
        container.html("");
    },
    getMailBoxCounter() {
        var btnControl = $("#btnViewEmail");
        if (btnControl.hasClass("shake-icon"))
            return false;

        var params = $.extend({}, doAjax_params_default);
        params['url'] = '/MyEmail/GetMailBoxCounter';
        params['requestType'] = 'POST';
        params['data'] = {};
        params['showloading'] = false;
        params['dataType'] = "json";

        params['successCallbackFunction'] = function (result) {
            if (result) {
                var counterLabel = btnControl.find(".m-nav__link-badge");

                if (result.hasData) {
                    btnControl.addClass("shake-icon");
                    //btnControl.find(".m-nav__link-badge").addClass("m-badge m-badge--dot m-badge--dot-small m-badge--danger");
                    counterLabel.addClass("m-badge m-badge--danger");

                    var totalCount = parseInt(result.total);
                    if (totalCount > 0) {
                        if (totalCount < 99) {
                            counterLabel.html(totalCount);
                        } else {
                            counterLabel.html("99+");
                        }
                    }

                    //if ($("#email_child_data_ajax").length > 0) {
                    //    FetchEmailBox();
                    //}
                } else {
                    btnControl.removeClass("shake-icon");
                    btnControl.find(".m-nav__link-badge").removeClass("m-badge m-badge--dot m-badge--dot-small m-badge--danger");
                }
            }
        };

        doAjax(params);
    },
    getScheduleCounter: function () {
        var dt = new Date();
        var localTime = dt.toISOString();
        //var tzOffset = dt.getTimezoneOffset();
        var btnControl = $("#btnViewSchedule");
        if (btnControl.hasClass("shake-icon"))
            return false;

        var params = $.extend({}, doAjax_params_default);
        params['url'] = '/AuthedGlobal/GetScheduleCounter';
        params['requestType'] = 'POST';
        params['data'] = { local_time: localTime, time_zone_number: 9 };
        params['dataType'] = "json";
        params['showloading'] = false;

        params['successCallbackFunction'] = function (data) {
            if (data) {
                if (data.success) {
                    var counterLabel = btnControl.find(".m-nav__link-badge");

                    var notifCount = parseInt(data.data);
                    if (notifCount > 0) {
                        if (notifCount < 99) {
                            counterLabel.html(notifCount);
                        } else {
                            counterLabel.html("99+");
                        }

                        btnControl.addClass("shake-icon");
                        counterLabel.addClass("m-badge m-badge--danger");
                    } else {
                        btnControl.removeClass("shake-icon");
                        counterLabel.removeClass("m-badge m-badge--danger");
                    }
                }
            }
        };
        doAjax(params);
    },
};

$(function () {
    $(".notif-icon").parent().on('show.bs.dropdown', function () {
        var container = $(".box-noti");
        if (!container.hasClass("loaded")) {
            AuthenticatedGlobal.getNotifications();
        }

        $(".count-noti-str").addClass("hidden");
    });

    $("#btnViewNotification").click(function () {
        var container = $(".box-noti");
        if (!container.hasClass("loaded")) {
            AuthenticatedGlobal.getNotifications();
        }

        $(".count-noti-str").addClass("hidden");
    });
});
