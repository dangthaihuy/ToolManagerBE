function NotificationResponse(result) {
    var msg = 'The chosen operation is executed successfully.';
    var title = LanguageDic["LB_NOTIFICATION"];
    var callback = null;
    if (result.hasOwnProperty('message'))
        msg = result.message;

    if (result.hasOwnProperty('title'))
        title = result.title;

    if (result.hasOwnProperty('clientcallback'))
        callback = result.clientcallback;

    if (result.success) {
        if (!callback) {
            $.showSuccessMessage(title, msg, function () { location.reload(); });
        } else {
            $.showSuccessMessage(title, msg, function () { eval(callback); });
        }
    } else {
        if (!callback) {
            $.showErrorMessage(title, msg, null);
        } else {
            $.showErrorMessage(title, msg, function () { eval(callback); });
        }
    }
}
(function ($) {

    $.showErrorMessage = function (title, text, afterCloseEvent) {
        swal({
            title: "<div class='text-danger'>" + title + "</div>",
            //text: text,
            html: "<div class='text-danger'>" + text + "</div>",
            type: "error",
            confirmButtonText: "OK",
            reverseButtons: !0
        }).then(function (e) {
            if (e.value) {
                if (afterCloseEvent && (typeof afterCloseEvent == "function")) {
                    afterCloseEvent();
                } else {
                    eval(afterCloseEvent);
                }
            }

        });

        return false;
    };


    $.showSuccessMessage = function (title, text, afterCloseEvent) {

        //toastr.options = {
        //    "closeButton": true,
        //    "debug": false,
        //    "newestOnTop": false,
        //    "progressBar": true,
        //    "positionClass": "toast-top-right",
        //    "preventDuplicates": true,
        //    "onclick": afterCloseEvent,
        //    "showDuration": "300",
        //    "hideDuration": "1000",
        //    "timeOut": "2000",
        //    "extendedTimeOut": "1000",
        //    "showEasing": "swing",
        //    "hideEasing": "linear",
        //    "showMethod": "fadeIn",
        //    "hideMethod": "fadeOut"
        //};

        //toastr.info(title, text);

        swal({
            title: title,
            //text: text,
            html : text,
            type: "success",            
            confirmButtonText: "OK",
            reverseButtons: !0
        }).then(function (e) {
            if (e.value) {
                if (afterCloseEvent && (typeof afterCloseEvent == "function")) {
                    afterCloseEvent();
                } else {
                    eval(afterCloseEvent);
                }
            }
             
            });

        return false;
    };

    $.showWarningMessage = function (title, text, afterCloseEvent) {
        swal({
            title: "<div class='text-warning'>" + title + "</div>",
            //text: text,
            html: "<div class='text-warning'>" + text + "</div>",
            type: "warning",
            confirmButtonText: "OK",
            reverseButtons: !0
        }).then(function (e) {
            if (e.value) {
                if (afterCloseEvent && (typeof afterCloseEvent == "function")) {
                    afterCloseEvent();
                } else {
                    eval(afterCloseEvent);
                }
            }

        });

        return false;
    };
})(jQuery);
