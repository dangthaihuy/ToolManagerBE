var materialEngine = new Bloodhound({
    identify: function (o) { return o.materialid; },
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Name'),
    dupDetector: function (a, b) { return a.Id === b.Id; },
    remote: {
        url: '/Material/GetSuggestion?query=%QUERY',
        wildcard: '%QUERY',
        cache: true,
        replace: function (url, uriEncodedQuery) {
            var exceptIds = $("#ExceptMaterials").val();
            return url.replace("%QUERY", uriEncodedQuery) + '&excepts=' + encodeURIComponent(exceptIds)
        },
    }
});

Dropzone.autoDiscover = false;
Dropzone.autoProcessQueue = false;

// Get the template HTML and remove it from the doument
var previewNode = document.querySelector("#dropzoneTemplate");
previewNode.id = "";
var previewTemplate = previewNode.parentNode.innerHTML;
previewNode.parentNode.removeChild(previewNode);

var myDropzone = new Dropzone(document.body, { // Make the whole body a dropzone
    url: "/Material/MaterialAttachFile", // Set the url
    thumbnailWidth: 70,
    thumbnailHeight: 50,
    parallelUploads: 20,
    previewTemplate: previewTemplate,
    autoQueue: false, // Make sure the files aren't queued until manually added
    previewsContainer: "#previews", // Define the container to display the previews
    clickable: ".fileinput-button", // Define the element that should be used as click trigger to select files.
    init: function () {
        this.on("success", function (file, response) {
            var attachments = [];
            var jsonAtts = $("#AllAttachments").val();
            if (jsonAtts != undefined && jsonAtts.length > 0) {
                attachments = JSON.parse(jsonAtts);
            }

            if (attachments == null) {
                attachments = [];
            }

            if (response) {
                if (response.data) {
                    attachments.push(response.data);
                }
            }

            $("#AllAttachments").val(JSON.stringify(attachments));
        });
    }
});

//Hide the total progress bar when nothing's uploading anymore
myDropzone.on("queuecomplete", function (progress, response) {
    document.querySelector("#total-progress").style.opacity = "0";
    $(".fileupload-process").addClass("hidden");

    $("#frmMaterial").submit();
    return false;
});

$("body").on("click", ".btn-remove-attached", function () {
    var el = $(this);
    var id = el.data("id");
    var jsonAtts = $("#AllAttachments").val();
    if (jsonAtts != undefined && jsonAtts.length > 0) {
        attachments = JSON.parse(jsonAtts);
    }

    if (attachments == null) {
        attachments = [];
    }
    attachments = attachments.filter(x => x.Id != id);
    $("#AllAttachments").val(JSON.stringify(attachments));

    el.closest("tr").remove();
});


function SubMaterialsRptTypeAhead() {
    $(".material-suggestion-new").each(function () {
        var template = Handlebars.compile($("#material-template").html());
        var el = $(this);
        el.removeClass("material-suggestion-new");
        var myTypeahead = el.typeahead({
            highlight: true,
            minLength: 0,            
        },
        {
            limit: 10,
            source: function (query, sync, async) {
                if (query === '') {
                    //sync(makerEngine.get('a'));
                    //async([]);
                    materialEngine.search(query, sync, async);
                }
                else {
                    materialEngine.search(query, sync, async);
                }
            },
            displayKey: 'DisplayName',
            templates: {
                suggestion: template
            }
        })
        .on('typeahead:selected', function (evt, item) {
            var ct = $(this).closest(".rpt-item-container");
            ct.find(".material_id").val(item.Id);
            //var exceptMaterials = JSON.parse("[" + $("#ExceptMaterials").val() + "]");
            //exceptMaterials.push(item.Id);

            //exceptMaterials = [...new Set(exceptMaterials)];
            //$("#ExceptMaterials").val(exceptMaterials.join(","));
        });
    });
}

$("#ddlMaterialType").change(function () {
    var mixed = $(this).find(':selected').data('mixed');
    if (mixed === "True") {
        $("#NutritionArea").removeClass("hidden");
    } else {
        $("#NutritionArea").addClass("hidden");
    }

    $("#IsMixed").val(mixed);
});

$("#submaterialsRpt").repeater({
    initEmpty: !1,
    show: function () {
        $(this).slideDown();
        SubMaterialsRptTypeAhead();

        var rpInputDefault = $(this).find(".rp-input-default-new");
        rpInputDefault.removeClass("rp-input-default-new");
        rpInputDefault.val(rpInputDefault.data("default"));

        setTimeout(function () {
            var counter = 0;
            $("#submaterialsRpt").find(".rpt-item-container").each(function () {
                counter++;
            });

            if (counter < 0)
                counter = 0;

            $("#SubRecordsCount").html(counter);
        }, 100);
    },
    hide: function (e) {
        $(this).slideUp(e);
        setTimeout(function () {
            var counter = 0;
            $("#submaterialsRpt").find(".rpt-item-container").each(function () {
                counter++;
            });

            if (counter < 0)
                counter = 0;

            $("#SubRecordsCount").html(counter);
        }, 500);
    }
});

$("#nutritionsRpt").repeater({
    initEmpty: !1,
    show: function () {
        $(this).slideDown();

        var rpInputDefault = $(this).find(".rp-input-default-new");
        rpInputDefault.removeClass("rp-input-default-new");
        rpInputDefault.val(rpInputDefault.data("default"));

        NutritionSelectPickerInit();      

        setTimeout(function () {
            var counter = 0;
            $("#nutritionsRpt").find(".rpt-item-container").each(function () {
                counter++;
            });

            if (counter < 0)
                counter = 0;

            $("#NutritionRecordsCount").html(counter);
        }, 100);
    },
    hide: function (e) {
        $(this).slideUp(e);
        setTimeout(function () {
            var counter = 0;
            $("#nutritionsRpt").find(".rpt-item-container").each(function () {
                counter++;
            });

            if (counter < 0)
                counter = 0;

            $("#NutritionRecordsCount").html(counter);
        }, 500);
    }
});

function BindProviderSuggestion() {
    $(".provider-suggestion-new").each(function () {
        var template = Handlebars.compile($("#provider-template").html());
        var el = $(this);
        var ctVal = $(this).closest(".m-typeahead").find(".provider-suggestion-value");
        var ctName = $(this).closest(".m-typeahead").find(".provider-name");
        if (ctVal.val() == null || ctVal.val().trim() == "") {
            ctVal.val(0);
            ctName.val("");
        }
        var myTypeahead = el.typeahead({
            highlight: true,
            minLength: 0
        }, {
            limit: 10,
            source: function (query, sync, async) {
                if (query === '') {
                    //sync(providerEngine.get('a'));
                    //async([]);
                    providerEngine.search(query, sync, async);
                }
                else {
                    providerEngine.search(query, sync, async);
                }
            },
            displayKey: 'Name',
            templates: {
                suggestion: template
            }
        })
        .on('typeahead:selected', function (evt, item) {
            evt.preventDefault();
            var ct = $(this).closest(".m-typeahead").find(".provider-suggestion-value");
            ct.val(item.Id);
        });
    });

    $(".provider-suggestion-new").bind("input", function () {
        var ct = $(this).closest(".m-typeahead").find(".provider-suggestion-value");
        ct.val(0);
    });

    $(".provider-suggestion-new").removeClass("provider-suggestion-new");
}

function BindMakerSuggestion() {
    $(".maker-suggestion-new").each(function () {
        var template = Handlebars.compile($("#maker-template").html());
        var el = $(this);
        var ctVal = $(this).closest(".m-typeahead").find(".maker-suggestion-value");
        var ctName = $(this).closest(".m-typeahead").find(".maker-name");
        if (ctVal.val() == null || ctVal.val().trim() == "") {
            ctVal.val(0);
            ctName.val("");
        }
        var myTypeahead = el.typeahead({
            highlight: true,
            minLength: 0,
            //classNames: {
            //    input: 'display-block',
            //    hint: 'dummy',
            //    open: 'is-open',
            //    empty: 'is-empty',
            //    cursor: 'is-active'
            //}
        }, {
            limit: 10,
            source: function (query, sync, async) {
                if (query === '') {
                    //sync(makerEngine.get('a'));
                    //async([]);
                    makerEngine.search(query, sync, async);
                }
                else {
                    makerEngine.search(query, sync, async);
                }
            },
            displayKey: 'Name',
            templates: {
                suggestion: template
            }
        })
        .on('typeahead:selected', function (evt, item) {
            evt.preventDefault();
            var ct = $(this).closest(".m-typeahead").find(".maker-suggestion-value");
            ct.val(item.Id);
        });
    });

    $(".maker-suggestion-new").bind("input", function () {
        var ct = $(this).closest(".m-typeahead").find(".maker-suggestion-value");
        ct.val(0);
    });

    $(".maker-suggestion-new").removeClass("maker-suggestion-new");
}

function BindProviderRepeater() {
    $('.provider-repeater').repeater({
        show: function () {
            var el = $(this);
            el.slideDown();

            var rpInputDefault = el.find(".rp-input-default-new");
            rpInputDefault.removeClass("rp-input-default-new");
            rpInputDefault.val(rpInputDefault.data("default"));

            var opt = $("#UnitId").find(":selected");
            var symbol = opt.data("symbol");

            el.find(".unit-symbol-label").html(symbol);

            setTimeout(function () {
                BindProviderSuggestion();
                BindMakerSuggestion();

                //$("#IsUpdatePrice").change();
                InitPriceChange();

                el.find(".rpt-item-container:not(:first)").remove();
                el.find(".label-price").html("");
            }, 100);
        },
        hide: function (e) {
            $(this).slideUp(e);            
        },
        repeaters: [{
            selector: '.provider-inner-repeater',
            show: function () {
                var el = $(this);
                el.slideDown();

                var rpInputDefault = $(this).find(".rp-input-default-new");
                rpInputDefault.removeClass("rp-input-default-new");
                rpInputDefault.val(rpInputDefault.data("default"));

                var rpLabelDefault = $(this).find(".rp-label-default-new");
                rpLabelDefault.removeClass("rp-label-default-new");
                rpLabelDefault.html(rpLabelDefault.data("default"));

                setTimeout(function () {
                    $("#UnitId").change();
                    InitPriceChange();
                }, 100);
            },
            hide: function (e) {
                $(this).slideUp(e);
            }
        }]
    });
}


$("#IsUpdatePrice").change(function () {
    if ($("#IsUpdatePrice").prop("checked") === true) {
        $(".price-group").removeAttr("disabled");
        $(".btn-repeater-price-add").removeClass("hidden");
        $(".btn-repeater-price-remove").removeClass("hidden");
        $(".provider").removeClass("hidden");
        $(".provider-current-name").addClass("hidden");
        $(".btn-control").removeClass("hidden");
    } else {
        $(".price-group").prop("disabled", "disabled");
        $(".btn-repeater-price-add").addClass("hidden");
        $(".btn-repeater-price-remove").addClass("hidden");
        $(".provider").addClass("hidden");
        $(".provider-current-name").removeClass("hidden");
        $(".btn-control").addClass("hidden");
    }

    $('.price-group.selectpicker').selectpicker('refresh');
});

function NutritionSelectPickerInit() {
    $("#nutritionsRpt").find(".selectpicker-new").each(function () {
        var firstVal = $(this).find("option:first").val();
        $(this).val(firstVal);
        $(this).selectpicker("refresh");
        $(this).removeClass("selectpicker-new");
    });
}

$("#NatriMg").bind("input", function () {
    var saltFormula = parseFloat($("#NatriMg").attr("data-formula-salt"));
    var natri = parseFloat($(this).val());
    if (natri === undefined || isNaN(natri)) {
        natri = 0;
    }

    $("#Salt").val(natri * saltFormula);
});

function InitPriceChange() {
    $(".rpt-price-input").bind("input", function () {
        var el = $(this);

        var currentVal = parseFloat(el.val());
        if (isNaN(currentVal)) {
            currentVal = 0;
        }

        var rptItem = el.closest(".rpt-item-container");
        var affectedItem = el.data("affected");
        var affectedLabel = el.data("label-affected");
        var formula = parseInt(el.data("formula-percent"));
        if (formula === undefined) {
            formula = 0;
        }

        var myVal = parseFloat(currentVal * formula / 100).toFixed(5).replace(/([0-9]+(\.[1-9]+)?)(\.?0+$)/, "$1");
        rptItem.find("." + affectedItem).val(myVal);
        rptItem.find("." + affectedLabel).html(myVal);

        rptItem.find("." + affectedItem).change();
    });

    $(".rpt-price-input-hidden").change(function () {
        var el = $(this);

        var currentVal = parseFloat(el.val());
        if (currentVal === undefined) {
            currentVal = 0;
        }

        var rptItem = el.closest(".rpt-item-container");
        var affectedItem = el.data("affected");
        var affectedLabel = el.data("label-affected");
        var formula = parseInt(el.data("formula-percent"));
        if (formula === undefined) {
            formula = 0;
        }

        var myVal = parseFloat(currentVal * formula / 100).toFixed(5).replace(/([0-9]+(\.[1-9]+)?)(\.?0+$)/, "$1");
        rptItem.find("." + affectedItem).val(myVal);
        rptItem.find("." + affectedLabel).html(myVal);
    });
}

$("body").on("click", ".btn-material-submit", function () {
    var ctrl = $(this);
    var isValid = true;
    var pricesRptCount = 0;

    isValid = $("#frmMaterial").valid();

    if (!isValid) {
        var firstError = $("#frmMaterial").find(".input-validation-error").first();
        GoToElement(firstError);
        firstError.focus();

        return false;
    }

    $(".price-provider2ft").each(function () {
        pricesRptCount++;
    });

    if (pricesRptCount === 0) {
        $.showErrorMessage(LanguageDic["LB_ERROR_OCCURED"], `材料価格を入力してください`, function () { return false; });

        isValid = false;
        return false;
    }

    if (isValid) {
        $(".price-provider2ft").each(function () {
            var el = $(this);
            var pr = el.val();
            pr = parseFloat(pr);
            if (isNaN(pr)) {
                pr = 0;
            }

            if (pr === 0) {
                $.showErrorMessage(LanguageDic["LB_ERROR_OCCURED"], `材料価格を入力してください`, function () { el.focus(); return false; });

                isValid = false;
                return false;
            }            
        });
    }

    if (isValid) {
        var newFilesUpload = myDropzone.getFilesWithStatus(Dropzone.ADDED);
        if (newFilesUpload.length > 0) {
            myDropzone.enqueueFiles(newFilesUpload);
        } else {
            ctrl.buttonLoading();
            $("#frmMaterial").submit();
        }
    }
})

$('.select-tag').select2({
    placeholder: LanguageDic['LB_KEYWORD_SEARCH'],
    width: '100%',
    "language": {
        "noResults": function () {
            return "キーワードが見つかりません";
        }
    },
    ajax: {
        url: '/Material/GetSuggestionTag',
        type: 'POST',
        quietMillis: 1000,
        data: function (params) {
            var query = {
                query: params.term,
                page: params.page || 1,
                __RequestVerificationToken: $('input[name = "__RequestVerificationToken"]').val()
            };

            return query;
        },
        processResults: function (data, page) {
            return {
                results: data.data.map(function (item) {
                    return {
                        id: item.Id,
                        text: item.Tag
                    };
                })
            };
        },
        cache: true
    }
});

$(function () {
    $("#ddlMaterialType").change();

    SubMaterialsRptTypeAhead();

    //if ($("#IsUpdatePrice").attr("enable") == false) {
    //    $(".price-group").removeAttr("disabled");
    //    $('.price-group.selectpicker').selectpicker('refresh');
    //    $(".btn-repeater-price-add").removeClass("hidden");
    //    $(".btn-repeater-price-remove").removeClass("hidden");
    //    $(".provider").addClass("hidden");
    //    $(".provider-name").removeClass("hidden");
    //} else {
    //    $(".price-group").prop("disabled", "disabled");
    //    $('.price-group.selectpicker').selectpicker('refresh');
    //    $(".btn-repeater-price-add").addClass("hidden");
    //    $(".btn-repeater-price-remove").addClass("hidden");
    //    $(".provider").addClass("hidden");
    //    $(".provider-name").removeClass("hidden");
    //}    


    $("#nutritionsRpt").find(".selectpicker-new").each(function () {
        $(this).selectpicker();
        $(this).removeClass("selectpicker-new");
    });

    BindProviderRepeater();

    //$("#IsUpdatePrice").change();

    InitPriceChange();

    BindProviderSuggestion();
    BindMakerSuggestion();
});
