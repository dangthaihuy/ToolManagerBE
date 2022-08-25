function LoadModalFormEvents() {
    //$.ajaxSetup({ cache: false });
    $("body").on("click", ".m-checkbox-custom-all", function () {
        var checked = false;
        if ($(".m-checkbox-custom-all>input[type='checkbox']").is(":checked")) {
            checked = true;
        }
        $(".m-checkbox-item").each(function () {
            $(this).prop('checked', checked);

        })
        CheckExistSeleted();
    })

    $("body").on("click", ".m-checkbox-item", function () {
        CheckExistSeleted();
    });

    $("body").on("click", "a[data-modal]", function (e) {
        var size = $(this).data("size");
        var dataBackdrop = $(this).data("backdrop");
        var dataType = $(this).data("type");

        if (size === undefined || size.length <= 0) {
            size = "large";
        }

        if (dataType === undefined || dataType.length <= 0) {
            dataType = "json";
        }

        // hide dropdown if any
        //$(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');
        if ($(this).data("href").length === 0) {
            alert("Please provide the URL action for modal");

            return false;
        }
        showLoading();
        $('#myModalContent').load($(this).data("href"), function (response, status, xhr) {
            try {
                response = JSON.parse(response);
                $.showErrorMessage(response.title, response.message, null);
            } catch (ex) {

                if (status === "error") {
                    var msg = "Sorry but there was an error: ";
                    alert(msg + xhr.status + " " + xhr.statusText);
                }
                else {

                    if (dataBackdrop === undefined) {
                        $('#myModal').modal({
                            keyboard: true
                        }, 'show');

                    } else {
                        $('#myModal').modal({
                            backdrop: dataBackdrop,
                            keyboard: true
                        }, 'show');

                    }

                    if (size === "default") {
                        $("#myModal").find(".modal-dialog").css("max-width", "30%");
                    } else if (size === "full") {
                        $("#myModal").find(".modal-dialog").css("max-width", "95%");
                    }
                    else {
                        var dialog = $("#myModal").find(".modal-dialog").first();
                        if (dialog.hasClass("modal-lg")) {
                            dialog.css("max-width", "1024px");
                        }
                    }

                    bindForm(this, dataType);
                }
            }
            hideLoading();
        });

        return false;
    });

    function CheckExistSeleted() {
        var check = false;
        var checkall = true;
        var listIds = "";
        $(".m-checkbox-item").each(function () {
            if ($(this).is(":checked")) {
                check = true;
                if (listIds == "") {
                    listIds += $(this).attr("id");
                }
                else {
                    listIds += "," + $(this).attr("id");
                }
            }
            else {

                checkall = false;
            }
        });
        $(".btn-check").each(function () {
            var optionVal = $(this).attr("data-value");
            if (optionVal != "" && optionVal != null) {
                optionVal = "&" + optionVal;
            }
            $(this).attr("data-href", $(this).attr("data-link") + "?ids=" + listIds + optionVal);
            if (check) {
                $(this).removeClass("disabled btn-secondary").addClass("btn-danger");
            }
            else {
                $(this).removeClass("btn-danger").addClass("disabled btn-secondary");
            }
        });

        $(".m-checkbox--all input[type='checkbox']").prop('checked', checkall);
    }

    $("body").on("click", "a[data-modal-direct]", function (e) {
        var size = $(this).data("size");

        // hide dropdown if any
        //$(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');
        if ($(this).data("href").length === 0) {
            alert("Please provide the URL action for modal");

            return false;
        }
        showLoading();
        $('#myModalContent').load($(this).data("href"), function (response, status, xhr) {
            if (status === "error") {
                var msg = "Sorry but there was an error: ";
                alert(msg + xhr.status + " " + xhr.statusText);
            }
            else {
                $('#myModal').modal({
                    /*backdrop: 'static',*/
                    keyboard: true
                }, 'show');

                if (size === "default") {
                    $("#myModal").find(".modal-dialog").css("max-width", "30%");
                } else {
                    var dialog = $("#myModal").find(".modal-dialog").first();
                    if (dialog.hasClass("modal-lg")) {
                        dialog.css("max-width", "1024px");
                    }
                }

                //bindForm(this);
            }

            hideLoading();
        });

        return false;
    });
}

function ShowModal(element, classSize) {
    if ($(this).data("href").length === 0) {
        alert("Please provide the URL action for modal");

        return false;
    }

    $('#myModalContent').load($(element).data("hreft"), function (response, status, xhr) {
        if (status === "error") {
            var msg = "Sorry but there was an error: ";
            alert(msg + xhr.status + " " + xhr.statusText);
        }
        else {
            $('#myModal').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');
            bindForm(this);
        }

        if (classSize)
            $(".modal-dialog").addClass(classSize);
    });

    return false;
}

//function ShowModal(element, classSize) {
//    $('#myModalContent').load(element.href, function (response, status, xhr) {
//        if (status == "error") {
//            var msg = "Sorry but there was an error: ";
//            alert(msg + xhr.status + " " + xhr.statusText);
//        }
//        else {
//            $('#myModal').modal({
//                /*backdrop: 'static',*/
//                keyboard: true
//            }, 'show');
//            bindForm(this);
//        }

//        if (classSize)
//            $(".modal-dialog").addClass(classSize);
//    });

//    return false;
//}

function bindForm(dialog, type = "json") {

    $('form', dialog).submit(function () {
        var dataPosted = new FormData(this);
        //var dataPosted = $(this).serialize();
        var $form = $(this);
        if ($form.valid()) {
            showLoading();
            $.ajax({
                url: this.action,
                type: this.method,
                data: dataPosted,
                processData: false,  // tell jQuery not to process the data
                contentType: false,
                success: function (result) {
                    hideLoading();
                    if (type === "json" || type === "") {
                        $('#myModal').modal('hide');
                        //Refresh
                        //alert('The chosen operation is executed successfully');
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
                    } else {
                         $('#myModal').find("#myModalContent").html(result.html);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //var responseTitle = $(jqXHR.responseText).filter('title').get(0);                    
                    hideLoading();
                    var responseTitle = "Error encountered"
                    $.showErrorMessage('Error message', $(responseTitle).text() + "\n" + formatErrorMessage(jqXHR, errorThrown), null);
                },
            });
        }

        return false;
    });
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

LoadModalFormEvents();