var isMobile = false; //initiate as false
// device detection
if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
    || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))) {
    isMobile = true;
}

$.fn.exists = function () {
    return this.length !== 0;
};

String.prototype.endsWith = function (suffix) {
    return this.indexOf(suffix, this.length - suffix.length) !== -1;
};

function isNumeric(value) {
    return /^-?\d+$/.test(value);
}

function RoundFloat(number) {
    if (!isNumeric(number)) {
        number = Number(number);
    }

    if (isNaN(number)) {
        number = 0;
    }

    return Math.round(number * 1e12) / 1e12;
}

var doAjax_params_default = {
    'showloading': true,
    'url': null,
    'requestType': "GET",
    'contentType': 'application/x-www-form-urlencoded; charset=UTF-8',
    'dataType': 'json',
    'processData': true,
    'async': true,
    'data': {},
    'beforeSendCallbackFunction': null,
    'successCallbackFunction': null,
    'completeCallbackFunction': null,
    'errorCallBackFunction': null,
    'context': null
};

function doAjax(doAjax_params) {

    var url = doAjax_params['url'];
    var showloading = doAjax_params['showloading'];
    var requestType = doAjax_params['requestType'];
    var contentType = doAjax_params['contentType'];
    var dataType = doAjax_params['dataType'];
    var data = doAjax_params['data'];
    var context = doAjax_params['context'];
    var processData = doAjax_params['processData'];
    var async = doAjax_params['async'];
    var beforeSendCallbackFunction = doAjax_params['beforeSendCallbackFunction'];
    var successCallbackFunction = doAjax_params['successCallbackFunction'];
    var completeCallbackFunction = doAjax_params['completeCallbackFunction'];
    var errorCallBackFunction = doAjax_params['errorCallBackFunction'];

    if (context) {
        var me = $(context);

        if (me.data('requestRunning')) {
            return false;
        }

        me.data('requestRunning', true);
    }

    //data.push({ __RequestVerificationToken: $('input[name = "__RequestVerificationToken"]').val() });
    if (requestType === "POST" || requestType === "post") {
        //make sure that url ends with '/'
        if (!url.endsWith("/")) {
            url = url + "/";
        }

        data.__RequestVerificationToken = $('input[name = "__RequestVerificationToken"]').val();
    }

    $.ajax({
        url: url,
        crossDomain: true,
        async: async,
        type: requestType,
        contentType: contentType,
        dataType: dataType,
        processData: processData,
        data: data,
        context: context,
        headers: { 'X-Requested-With': 'XMLHttpRequest' },
        beforeSend: function (jqXHR, settings) {
            if (showloading) {
                showLoading();
            }

            if (typeof beforeSendCallbackFunction === "function") {
                if (context)
                    beforeSendCallbackFunction(context);
                else
                    beforeSendCallbackFunction();
            }
        },
        success: function (data, textStatus, jqXHR) {
            if (data.Error) {
                if (data.clientcallback)
                    eval(data.clientcallback);

                return false;
            }

            if (typeof successCallbackFunction === "function") {
                successCallbackFunction(data);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (typeof errorCallBackFunction === "function") {
                errorCallBackFunction(errorThrown);
            }

        },
        complete: function (jqXHR, textStatus) {
            if (showLoading) {
                hideLoading();
            }

            if (typeof completeCallbackFunction === "function") {
                completeCallbackFunction();
            }

            if (context)
                me.data('requestRunning', false);
        }
    });
}

$.aPost = function (url, data = {}, successCallback = null, dataType = "json", showLoading = false) {
    var params = $.extend({}, doAjax_params_default);
    if (data === null)
        data = {};

    params['url'] = url;
    params['requestType'] = 'POST';
    params['showloading'] = showLoading;
    params['data'] = data;
    params['dataType'] = dataType;
    params['successCallbackFunction'] = function (result) {
        if (successCallback) {
            eval(successCallback(result));
        }
    };
    doAjax(params);
};

$.aGet = function (url, data = {}, successCallback = null, dataType = "json", showLoading = false) {
    var params = $.extend({}, doAjax_params_default);
    params['url'] = url;
    params['requestType'] = 'GET';
    params['showloading'] = showLoading;
    params['data'] = data;
    params['dataType'] = dataType;
    params['successCallbackFunction'] = function (result) {
        if (successCallback) {
            eval(successCallback(result));
        }
    };
    doAjax(params);
};

$(".input-off-enter").keypress(function (e) {
    e.stopPropagation();
    if ($(this).hasClass("input-off-enter")) {
        return false;
    }
});

$('.form-off-enter input[type=text]').on('keydown', function (e) {
    if (e.which == 13) {
        return false;
    }
    else {
        return true;
    }
});

function RemoveItemFromArray(arr, item) {
    var index = arr.indexOf(item);
    if (index > -1) {
        arr.splice(index, 1);
    }
}

function ConvertStringToListArray(str, seperaterChar = ",") {
    if (str === null || str === "" || str === "NaN") {
        return [];
    }

    var arr = str.split(seperaterChar).map(function (item) {
        return parseInt(item, 10);
    });

    return arr;
}

const numberWithCommas = (x, seperateChar = ".") => {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, seperateChar);
}

function FormatNumberWithCommas(x, thousandChar = ",", decimalChar = ".") {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, thousandChar);
    return parts.join(decimalChar);
}

function FeatureCommingSoon() {
    bootbox.alert(LanguageDic["LB_FEATURE_COMMING"]);
}

function AutoSetTime() {
    $(".livetimestamp").each(function () {
        SettingElement(this);
    });
    $(".livetimestamp-title").each(function () {
        SettingElementTitle(this);
    });
}

function NeedToLogin() {
    window.location.href = "/webauth/login";
}

$("body").on("click", ".btn-back", function () {
    if ((1 < history.length) && document.referrer) {
        history.back();
    } else {
        if ($(this).data("back")) {
            window.location.href = $(this).data("back");
        } else {
            window.location.href = "/";
        }
    }
});

function NeedToLogout() {
    window.location.href = "/webauth/logout";
}

(function ($) {
    $.fn.changeElementType = function (newType) {
        var attrs = {};

        $.each(this[0].attributes, function (idx, attr) {
            attrs[attr.nodeName] = attr.nodeValue;
        });

        this.replaceWith(function () {
            return $("<" + newType + "/>", attrs).append($(this).contents());
        });
    }
})(jQuery);

function ReplaceErrorImage() {
    $("img").on("error", function () {
        $(this).attr("src", "/Content/images/no-image.png");
    });
}

function ConfirmFirst(callback, message, data) {
    if (!message)
        message = LanguageDic["LB_CONFIRM"];
    bootbox.confirm({
        message: message,
        buttons: {
            cancel: {
                label: '<i class="fa fa-remove"></i> ' + LanguageDic["BT_CANCEL"],
                className: 'btn-secondary button-cancel'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> ' + LanguageDic["BT_ALLOW"],
                className: 'btn-info'
            }
        },
        callback: function (confirmed) {
            if (confirmed)
                callback(data);
        }
    });
    return false;
}

function ConfirmDynamic(title, message) {
    if (!message)
        message = LanguageDic["LB_CONFIRM"];
    bootbox.confirm({
        title: title,
        message: message,
        buttons: {
            cancel: {
                label: '<i class="fa fa-remove"></i> ' + LanguageDic["BT_CANCEL"],
                className: 'btn-secondary button-cancel'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> ' + LanguageDic["BT_ALLOW"],
                className: 'btn-info'
            }
        },
        callback: function (confirmed) {
            if (confirmed) {
                return true;
            }
            else {
                return false;
            }
        }
    });
}

bootbox.showmessage = function (options, isSuccess, clientcallback) {
    // Set the defaults
    var className = (!isSuccess) ? 'text-danger' : 'text-info';
    var title = (!isSuccess) ? LanguageDic['LB_ERROR'] : LanguageDic['LB_NOTIFICATION'];
    var defaults = {
        className: className,
        title: title,
        message: 'An Error has occurred. Please contact your system administrator.',
        closeButton: false,
        buttons: {
            "OK": function () {
                if (clientcallback) {
                    clientcallback();
                }
            }
        }
    };

    // Extend the defaults with any passed in options
    var settings = $.extend(defaults, options);
    var iconHtml = (!isSuccess) ? '<i class="fa fa-warning"></i>' : '<i class="fa fa-check"></i>';
    // Build and show the dialog
    bootbox.dialog({
        className: settings.className,
        title: settings.title,
        message: iconHtml + ' ' + settings.message,
        closeButton: settings.closeButton,
        buttons: settings.buttons
    });
};

function ScrollToElement(el) {
    el.addClass("focus");
    $('html, body').animate({
        scrollTop: (el.offset().top)
    }, 500);

    setTimeout(function () {
        el.removeClass("focus");
    }, 3000);
}

function ScrollToElement(el, parentEl = null) {
    el.addClass("focus");

    if (parentEl) {
        parentEl.animate({
            scrollTop: (el.offset().top)
        }, 500);
    } else {
        $('html, body').animate({
            scrollTop: (el.offset().top)
        }, 500);
    }
}

function GoToElement(el) {
    //var top = el.offsetTop - (el.innerHeight / 2);
    //window.scrollTo(0, top);

    var elem_position = el.offset().top;
    var window_height = $(window).height();
    var y = elem_position - window_height / 2;

    window.scrollTo(0, y);
}

function FocusToErrorElement(el, timeout = 3000) {
    if (!el.hasClass("floating-error-focus")) {
        el.addClass("floating-error-focus");
        GoToElement(el);

        setTimeout(function () {
            el.removeClass("floating-error-focus");
        }, timeout);
    }   
}

function DoNoThing() {

}

function formatErrorMessage(jqXHR, exception) {

    if (jqXHR.status === 0) {
        return ('Not connected.\nPlease verify your network connection.');
    } else if (jqXHR.status == 404) {
        return ('The requested page not found. [404].\n' + jqXHR.responseText);
    } else if (jqXHR.status == 500) {
        var resultObj = $.parseJSON(jqXHR.responseText);
        return ('' + resultObj.message);
    } else if (exception === 'parsererror') {
        return ('Requested JSON parse failed.');
    } else if (exception === 'timeout') {
        return ('Time out error.');
    } else if (exception === 'abort') {
        return ('Ajax request aborted.');
    } else {
        return ('Uncaught Error.\n' + jqXHR.responseText);
    }
}

function ModalDisplayBack(modalId = "myModal") {
    var myModal = $("#" + modalId);
    myModal.addClass("re-show");
    myModal.modal("toggle");
}

function showLoading() {
    $('.common-loading').show();
}
function hideLoading() {
    $('.common-loading').fadeOut(1000);
}

function GetDateTimeNowByFormat() {
    var mydate = new Date();
    year = mydate.getFullYear();
    month = mydate.getMonth();

    months = new Array('01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12');
    d = mydate.getDate();
    day = mydate.getDay();
    days = new Array('Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday');
    h = mydate.getHours();

    var strMonth = "";
    var strDay = "";
    if (h < 10) {
        h = "0" + h;
    }
    m = mydate.getMinutes();
    if (m < 10) {
        m = "0" + m;
    }

    if (month < 10) {
        strMonth = "0" + month;
    }

    if (day < 10) {
        strDay = "0" + day;
    }

    s = mydate.getSeconds();
    if (s < 10) {
        s = "0" + s;
    }

    result = '' + year + months[month] + d + h + m + s;

    return result;
}

function exportTableToCSV($table, filename) {
    var universalBOM = "\uFEFF";
    var $rows = $table.find('tr:has(td),tr:has(th)'),
        // Temporary delimiter characters unlikely to be typed by keyboard
        // This is to avoid accidentally splitting the actual contents
        tmpColDelim = String.fromCharCode(11), // vertical tab character
        tmpRowDelim = String.fromCharCode(0), // null character

        // actual delimiter characters for CSV format
        colDelim = '","',
        rowDelim = '"\r\n"',

        // Grab text from table into CSV formatted string
        csv = '"' + $rows.map(function (i, row) {
            var $row = jQuery(row), $cols = $row.find('td,th');
            var regex = /<br\s*[\/]?>/gi;

            return $cols.map(function (j, col) {
                var $col = jQuery(col), text = $col.html().trim();
                text = text.replace(regex, "\n");
                return text; // escape double quotes

            }).get().join(tmpColDelim);

        }).get().join(tmpRowDelim)
            .split(tmpRowDelim).join(rowDelim)
            .split(tmpColDelim).join(colDelim) + '"',
        // Data URI        
        csvData = 'data:application/csv;charset=utf-8,' + encodeURIComponent(universalBOM + csv);

    if (window.navigator.msSaveBlob) { // IE 10+
        //alert('IE' + csv);
        window.navigator.msSaveOrOpenBlob(new Blob([csv], { type: "text/plain;charset=utf-8;" }), filename);
    }
    else {
        jQuery(this).attr({ 'download': filename, 'href': csvData, 'target': '_blank' });
    }
}

// This must be a hyperlink
$("body").on("click", ".exportcsv", function (event) {
    var fileName = "";
    fileName = $(this).data("filename");

    var title = $(this).data("title");

    if (fileName.length === 0) {
        fileName = "Export";
    }

    fileName = fileName + "_" + GetDateTimeNowByFormat();

    var tableId = "";
    tableId = $(this).data("tableid");

    if (tableId.length === 0) {
        tableId = "dataTable";
    }

    //exportTableToCSV.apply(this, [$('#' + tableId), fileName + '.csv']);

    tableToExcel(tableId, fileName);
});

function tableToExcel(id, fileName, includeInfo = false, title, info = "", subinfo = "", footer = "") {
    var sheetname = "Sheet 1";
    var tab_text = '\uFEFF';
    tab_text = tab_text + '<html xmlns:v="urn:schemas-microsoft-com:vml" xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40">';
    tab_text = tab_text + '<head>';
    tab_text = tab_text + '<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />';
    tab_text = tab_text + '<meta name="ProgId" content="Excel.Sheet" />';
    tab_text = tab_text + '<meta name="Generator" content="Microsoft Excel 11" />';
    tab_text = tab_text + '<title>Sample</title>';
    tab_text = tab_text + '<!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet>';
    tab_text = tab_text + '<x:Name>' + ((sheetname) ? sheetname : 'Sheet 1') + '</x:Name>';
    tab_text = tab_text + '<x:WorksheetOptions><x:Panes></x:Panes></x:WorksheetOptions></x:ExcelWorksheet>';
    tab_text = tab_text + '</x:ExcelWorksheets></x:ExcelWorkbook>';
    tab_text = tab_text + '</xml><![endif]--></head><body>';

    if (includeInfo) {
        if (title != null && title != "") {
            tab_text += '           <span><b>CÔNG TY TNHH XXX</b></span><br>';
            tab_text += '           <span><b>4th Floor, No. 10, Lane 45A Vong Thi, P. Buoi, Tay Ho District, Hanoi +84 383997212</b></span><br>';
            tab_text += '           <span><b>Tax: 0108896286</b><span><br><br>';

            tab_text += '<center style="font-size: 150%;"><b>' + title + '</b></center><br>';
        }
        if (info != null && info != "") {
            tab_text += info + '<br>';
        }
        if (subinfo != null && subinfo != "") {
            tab_text += subinfo;
        }
    }

    tab_text = tab_text + '<table border="1px">';
    var exportTable = $('#' + id).clone();
    exportTable.find('input').each(function (index, elem) { $(elem).remove(); });
    tab_text = tab_text + exportTable.html();
    tab_text = tab_text + '</table><br>';

    if (includeInfo) {
        if (footer != null && footer != "") {
            tab_text += footer;
        }
    }

    tab_text += '</body></html> ';

    fileName = fileName + '.xls';
    var blob = new Blob([tab_text], { type: "application/vnd.ms-excel" })
    window.saveAs(blob, fileName);
}

function PrintElem(elem) {
    var mywindow = window.open('', 'PRINT', 'height=400,width=600');

    mywindow.document.write('<html><head><title>' + document.title + '</title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write('<h1>' + document.title + '</h1>');
    mywindow.document.write(document.getElementById(elem).innerHTML);
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/

    mywindow.print();
    mywindow.close();

    return true;
}

function CatchAjaxResponseWithNotif(result) {
    var title = LanguageDic['LB_NOTIFICATION'];
    if (result && result.title) {
        title = result.title;
    }

    if (result.success && result.message) {
        $.showSuccessMessage(LanguageDic['LB_NOTIFICATION'], result.message, result.clientcallback);
    } else {
        $.showErrorMessage(LanguageDic['LB_NOTIFICATION'], result.message, result.clientcallback);
    }
}

function RedirectTo(url) {
    if (url.length > 0) {
        window.location.href = url;
    }
}

function showItemLoading(element) {
    if (element) {
        $(element).find('.common-loading-item').remove();
        var loadingHtml = $('.common-loading-item').wrap('<div/>').parent().html();
        $(element).prepend(loadingHtml);
        $(element).find('.common-loading-item').show();
    } else {
        $('.common-loading-item').show();
    }
}
function hideItemLoading(element) {
    if (element) {
        $(element).find('.common-loading-item').fadeOut(1000);
    } else {
        $('.common-loading-item').fadeOut(1000);
    }
}

var MySiteGlobal = {
    init: function () {
        this.versionVerify();

        GetResources();
    },
    bindEvents: function () {
        $("#btnViewSchedule").click(function () {
            if (!$("#scheduleModalContent").hasClass("loaded")) {
                var params = $.extend({}, doAjax_params_default);
                params['url'] = "/Schedule/ShowCalendar";
                params['requestType'] = 'GET';
                params['data'] = {};
                params['dataType'] = "html";

                params['successCallbackFunction'] = function (result) {
                    $('#scheduleModalContent').html(result).addClass("loaded");
                    $('#scheduleModal').modal("show");
                };

                doAjax(params);
            } else {
                $('#scheduleModal').modal("show");
            }
        });
    },
    bindAlertIcons: function () {
        setInterval(function () {
            $(".shake-icon .m-nav__link-icon").addClass("m-animate-shake"), $(".shake-icon .m-nav__link-badge").addClass("m-animate-blink");
        }, 3e3);

        setInterval(function () {
            $(".shake-icon .m-nav__link-icon").removeClass("m-animate-shake"), $(".shake-icon .m-nav__link-badge").removeClass("m-animate-blink");
        }, 6e3);
    },
    versionVerify: function () {
        var localVer = localStorage.getItem("CurrentVersion");
        var needUpdate = localVer !== CurrentVersion;
        if (needUpdate) {
            localStorage.setItem("CurrentVersion", CurrentVersion);

            //Clear data
            localStorage.removeItem("LanguageDic_" + CurrentLang);
        }

        var myFrmTK = localStorage.getItem("JZ-VSTK");
        if (myFrmTK !== JZ_VSTK || needUpdate) {
            var arr = []; // Array to hold the keys
            if (localStorage.length > 0) {
                for (var i = 0; i < localStorage.length; i++) {
                    if (localStorage.key(i).includes("JZ-FRMS-")) {
                        arr.push(localStorage.key(i));
                    }
                }
            }

            if (arr.length > 0) {
                // Iterate over arr and remove the items by key
                for (var j = 0; j < arr.length; j++) {
                    localStorage.removeItem(arr[j]);
                }
            }

            localStorage.setItem("JZ-VSTK", JZ_VSTK);
        }
    },
    bindClickedEvents: function (element, event) {
        $("body").on("click", element, function () {
            if (event) {
                event($(this));
            }
        });
    },
    bindModalLoading: function () {
        $(".modal").find(".modal-content").addClass("sk-loading");

        $('.modal').on('shown.bs.modal', function (e) {
            var modalBody = $(this).find(".modal-content");
            setTimeout(function () {
                modalBody.removeClass("sk-loading");
            }, 500);
        });

        $('.modal').on('hide.bs.modal', function (e) {
            var modalBody = $(this).find(".modal-content");
            setTimeout(function () {
                modalBody.addClass("sk-loading");
            }, 500);
        });
    },
    formatNumber: function () {
        $(".format-number").each(function () {
            $(this).html(FormatNumberWithCommas($(this).html()));

            $(this).removeClass("format-number");
        });
    }
};

function GetResources() {
    var langKey = "LanguageDic_" + CurrentLang;
    var langRes = localStorage.getItem("LanguageDic_" + CurrentLang);

    if (langRes === null || langRes.length === 0) {
        var params = $.extend({}, doAjax_params_default);
        params['url'] = '/Master/GetResources';
        params['requestType'] = 'POST';
        params['showloading'] = false;
        params['data'] = {};
        params['dataType'] = "json";
        params['successCallbackFunction'] = function (result) {
            LanguageDic = JSON.parse(result);

            localStorage.setItem(langKey, result);
        };
        doAjax(params);
    } else {
        LanguageDic = JSON.parse(langRes);
    }
}

function PreviewImageFromBrowseDialog(input) {
    var previewContainer = $(input).data("preview");
    var currentPreviewContainer = $("#" + previewContainer);
    var previewImg = currentPreviewContainer.children(".thumbImg").first();
    var fileUploadIcon = currentPreviewContainer.children(".file-upload-icon").first();
    var uploadBox = $(input).closest(".uploadbox");

    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            previewImg.attr("src", e.target.result);
            previewImg.removeClass("hidden");
            fileUploadIcon.addClass("hidden");
        };

        reader.readAsDataURL(input.files[0]);

        uploadBox.find(".uploadbox-cancel").removeClass("hidden");
    } else {
        if (previewImg) {
            previewImg.addClass("hidden");
            fileUploadIcon.removeClass("hidden");

            uploadBox.find(".uploadbox-cancel").addClass("hidden");
        }
    }
}

$(".file-upload-storage").change(function () {
    PreviewImageFromBrowseDialog(this);
});

$("body").on("click", ".file-upload-btn", function () {
    $(this).parent().find(".file-upload-storage").click();
});

$("body").on("click", ".uploadbox-cancel", function () {
    var uploadBox = $(this).closest(".uploadbox");

    uploadBox.find(".thumbImg").attr("src", "").addClass("hidden");
    uploadBox.find(".file-upload-icon").removeClass("hidden");
    uploadBox.find(".file-upload-storage").val("");
    uploadBox.find("input[type='hidden']").val("");

    $(this).addClass("hidden");
});

$("body").on("click", ".customFieldRemove", function () {
    var ctn = $(this).closest(".customFieldGroup");
    ctn.slideUp();
    setTimeout(function () {
        ctn.remove();

        ReflectCustomField();
    }, 300);
});

function ReflectCustomField() {
    var count = 0;
    $(".customFieldGroup").each(function () {
        var inputCtrl = $(this).find(".customFieldInput");
        inputCtrl.each(function () {
            var propName = $(this).data("prop");
            $(this).attr("name", "metadata[" + count + "][" + propName + "]");
        });

        count++;
    });
}

$(document).ready(function () {
    //DynamicRealTime();
    MySiteGlobal.init();
    MySiteGlobal.bindEvents();
    MySiteGlobal.bindAlertIcons();

    $(".page-loading").addClass("hidden");
    $(".RemovalScripts").remove();

    setTimeout(function () {
        if ($(".input-off-enter").length > 0) {
            var target = $("input[data-controller=" + $("#Controller").val() +"]");
            RenderFormSearch(target, false);
            $(".input-off-enter").removeClass("input-off-enter");
        }
    }, 200);
});

//$(document).on('keyup', '.number-format', function (e) {
//    // Get the value.
//    var input = $(e.target).val();
//    var rs = FormatNumber(input);
//    $(e.target).val(rs);
//});

//$(".number-format").bind("input", function (e) {
//    var input = $(e.target).val();
//    var value = parseFloat(input).toLocaleString(
//        undefined, // leave undefined to use the visitor's browser 
//        // locale or a string like 'en-US' to override it.
//        { minimumFractionDigits: 2 }
//    );

//    console.log(value);
//    $(e.target).val(value);
//});

function changeLocalTimezone(format = "") {
    $(".local-time").each(function () {
        var strTime = $(this).data("time");
        strTime = strTime + " UTC";
        var date = new Date(strTime);
        $(this).html(date.toLocaleString());
        $(this).removeClass("local-time");
    });
}

function FormatNumber(num, seperateChar = ".") {
    var listNum = num.split('.');
    var number = listNum[0].replace(/\./g, '');
    if (!$.isNumeric(number)) {
        number = number.substring(0, number.length - 1);
    }
    var result = "0";
    if (parseInt(number) > 0) {
        result = parseInt(number) + '';
    }
    var i = 0;
    var count = 0;
    while (i < number.length) {
        if (i % 3 == 0 && i > 0) {
            var index = number.length - i;
            result = result.insertAt(index, seperateChar);
            count++;
        }
        i++;
    }
    if (listNum.length > 1) {
        return result + "," + listNum[1];
    }

    return result;
}

String.prototype.insertAt = function (index, string) {
    return this.substr(0, index) + string + this.substr(index);
};

$("body").on("click", ".legend-ctrl", function () {
    var fs = $(this).closest("fieldset");
    var ep = fs.find(".fieldset-expand");
    if (ep.length > 0) {
        ep.remove();
    } else {
        fs.append('<div class="fieldset-expand row"><div class="col-md-12"><a href="javascript:;" class="text-info"><b>...</b></a></div></div>');
    }

    fs.find(".fieldset-content").slideToggle();
    $(this).find("i").toggleClass("fa-angle-down");
});

$("body").on("click", ".fieldset-expand", function () {
    var fs = $(this).closest("fieldset");
    $(this).remove();
    fs.find(".fieldset-content").slideToggle();
    fs.find(".legend-ctrl").find("i").toggleClass("fa-angle-down");
});

function ClearSomeLocalStorage(startsWith) {
    var myLength = startsWith.length;

    Object.keys(localStorage)
        .forEach(function (key) {
            if (key.substring(0, myLength) == startsWith) {
                localStorage.removeItem(key);
            }
        });
}

Number.prototype.round = function (p) {
    p = p || 10;
    return parseFloat(this.toFixed(p));
};

function SpamProtection(callback, wait, immediate = false) {
    let timeout = null
    let initialCall = true

    return function () {
        const callNow = immediate && initialCall
        const next = () => {
            callback.apply(this, arguments)
            timeout = null
        }

        if (callNow) {
            initialCall = false
            next()
        }

        if (!timeout) {
            timeout = setTimeout(next, wait)
        }
    }
}

function MakeSureValidNumber(num) {
    if (isNaN(num)) {
        num = 0;
    }

    return num;
}

function CatchAjaxJsonResult(result) {
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
            $.showSuccessMessage(title, msg, function () { return false; });
        } else {
            $.showSuccessMessage(title, msg, function () { eval(callback); });
        }
        //location.reload();
    } else {
        if (!callback) {
            $.showErrorMessage(title, msg, null);
        } else {
            $.showErrorMessage(title, msg, function () { eval(callback); });
        }

        // bindForm();
    }
}

$.fn.areaLoading = function (config = null, callback) {
    var defConfig = {
        colorClass: "text-success",
        position: "center",
        timeout: 1000
    };

    if (config === null) {
        config = defConfig;
    } else {
        if (config.timeout <= 0) {
            config.timeout = 1000;
        }
    }   

    var ctrl = $(this);
    if (!ctrl.hasClass("area-loading")) {
        var spHtml = '<div class="spinner-border ' + config.colorClass + ' ' + config.position + '" role="status"><span class="sr-only"></span></div>';
        ctrl.find('.spinner-border').remove();
        ctrl.prepend(spHtml);
        ctrl.addClass("area-loading");
    } else {
        ctrl.find('.spinner-border').remove();
        ctrl.removeClass("area-loading");
        if (callback) {
            setTimeout(function () {
                callback();
            }, config.timeout);
        }
    }
};

$.fn.buttonLoading = function (config = null, callback) {
    var defConfig = {
        colorClass: "",
        position: "center",
        timeout: 1000
    };

    if (config === null) {
        config = defConfig;
    } else {
        if (config.timeout <= 0) {
            config.timeout = 1000;
        }
    }

    var ctrl = $(this);
    if (!ctrl.hasClass("btn-loading")) {
        ctrl.addClass("btn-loading");
        ctrl.prop("disabled", true);
    } else {
        ctrl.removeClass("btn-loading");
        ctrl.prop("disabled", false);

        if (callback) {
            setTimeout(function () {
                callback();
            }, config.timeout);
        }
    }
};

function StringNormally(str){
    return str.normalize("NFKC");
};

function replaceUrlParam(paramName, paramValue){
    var url = window.location.href;

    if (paramValue == null) {
        paramValue = '';
    }

    var pattern = new RegExp('\\b('+paramName+'=).*?(&|#|$)');
    if (url.search(pattern)>=0) {
        return url.replace(pattern,'$1' + paramValue + '$2');
    }

    url = url.replace(/[?#]$/,'');
    return url + (url.indexOf('?')>0 ? '&' : '?') + paramName + '=' + paramValue;
}

function replaceUrlString(url, paramName, paramValue) {
    if (paramValue == null) {
        paramValue = '';
    }

    var pattern = new RegExp('\\b(' + paramName + '=).*?(&|#|$)');
    if (url.search(pattern) >= 0) {
        return url.replace(pattern, '$1' + paramValue + '$2');
    }

    url = url.replace(/[?#]$/, '');
    return url + (url.indexOf('?') > 0 ? '&' : '?') + paramName + '=' + paramValue;
}

function DragToScroll(idName = "drag-scroll") {
    const slider = document.querySelector(`#${idName}`);
    let mouseDown = false;
    let startX, scrollLeft;

    let startDragging = function (e) {
        mouseDown = true;
        startX = e.pageX - slider.offsetLeft;
        scrollLeft = slider.scrollLeft;
    };
    let stopDragging = function (event) {
        mouseDown = false;
    };

    slider.addEventListener('mousemove', (e) => {
        e.preventDefault();
        if (!mouseDown) { return; }
        const x = e.pageX - slider.offsetLeft;
        const scroll = x - startX;
        slider.scrollLeft = scrollLeft - scroll;
    });

    // Add the event listeners
    slider.addEventListener('mousedown', startDragging, false);
    slider.addEventListener('mouseup', stopDragging, false);
    slider.addEventListener('mouseleave', stopDragging, false);
}

$(".input-normally").bind("input", function () {
    var val = $(this).val();
    val = StringNormally(val);
    $(this).val(val);
});

function replaceUrl(newUrl) {
    window.history.pushState("", document.title, newUrl);
}

function StandardInputNumber(el) {
    el.bind("input", function () {
        var val = $(this).val();
        val = ConvertFullWidthNumber(val);

        //var qty = parseFloat(val);
        //if (isNaN(qty) || qty < 0) {
        //    qty = 0;
        //}

        $(this).val(val);
    });
}

function ConvertFullWidthNumber(text) {
    var search = '０１２３４５６７８９．。';
    var replace = '0123456789.';

    // Make the search string a regex.
    var regex = RegExp('[' + search + ']', 'g');
    var t = text.replace(regex,
        function (chr) {
            // Get the position of the found character in the search string.
            var ind = search.indexOf(chr);
            // Get the corresponding character from the replace string.
            var r = replace.charAt(ind);
            return r;
        });
    return t;
}