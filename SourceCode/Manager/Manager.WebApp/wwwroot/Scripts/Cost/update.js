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
            displayKey: 'Name',
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

$("#attributesRpt").repeater({
    initEmpty: !1,
    show: function () {
        $(this).slideDown();

        var rpInputDefault = $(this).find(".rp-input-default-new");
        rpInputDefault.removeClass("rp-input-default-new");
        rpInputDefault.val(rpInputDefault.data("default"));

        AttributeSelectPickerInit();

        setTimeout(function () {
            var counter = 0;
            $("#attributesRpt").find(".rpt-item-container").each(function () {
                counter++;
            });

            if (counter < 0)
                counter = 0;

            $("#AttributeRecordsCount").html(counter);
        }, 100);
    },
    hide: function (e) {
        $(this).slideUp(e);
        setTimeout(function () {
            var counter = 0;
            $("#attributesRpt").find(".rpt-item-container").each(function () {
                counter++;
            });

            if (counter < 0)
                counter = 0;

            $("#AttributeRecordsCount").html(counter);
        }, 500);
    }
});

$("#pricesRpt").repeater({
    initEmpty: !1,
    show: function () {
        $(this).slideDown();

        var rpInputDefault = $(this).find(".rp-input-default-new");
        rpInputDefault.removeClass("rp-input-default-new");
        rpInputDefault.val(rpInputDefault.data("default"));

        var rpLabelDefault = $(this).find(".rp-label-default-new");
        rpLabelDefault.removeClass("rp-label-default-new");
        rpLabelDefault.html(rpLabelDefault.data("default"));

        setTimeout(function () {
            var counter = 0;
            $("#pricesRpt").find(".rpt-item-container").each(function () {
                counter++;
            });

            if (counter < 0)
                counter = 0;

            $("#PriceRecordsCount").html(counter);

            $("#UnitId").change();

            InitPriceChange();
        }, 100);
    },
    hide: function (e) {
        $(this).slideUp(e);
        setTimeout(function () {
            var counter = 0;
            $("#pricesRpt").find(".rpt-item-container").each(function () {
                counter++;
            });

            if (counter < 0)
                counter = 0;

            $("#PriceRecordsCount").html(counter);
        }, 500);
    }
});

$("#IsUpdatePrice").change(function () {
    if ($("#IsUpdatePrice").prop("checked") === true) {
        $(".price-group").removeAttr("disabled");
        $(".btn-repeater-price-add").removeClass("hidden");
        $(".btn-repeater-price-remove").removeClass("hidden");
    } else {
        $(".price-group").prop("disabled", "disabled");
        $(".btn-repeater-price-add").addClass("hidden");
        $(".btn-repeater-price-remove").addClass("hidden");
    }

    $('.price-group.selectpicker').selectpicker('refresh');
});

function AttributeSelectPickerInit() {
    $("#attributesRpt").find(".ddl-color-picker-new").each(function () {
        var firstVal = $(this).find("option:first").val();
        $(this).val(firstVal);
        $(this).selectpicker("refresh");
        $(this).removeClass("ddl-color-picker-new");

        $(this).change(function () {
            var color = $(this).find("option:selected").data("value");
            $(this).closest(".rpt-item-container").find(".rpt-color-preview").css("background-color", color);           
        });

        $(this).change();
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

$(function () {
    if ($("#IsUpdatePrice").data("enable") == false) {
        $(".price-group").removeAttr("disabled");
        $('.price-group.selectpicker').selectpicker('refresh');
        $(".btn-repeater-price-add").removeClass("hidden");
        $(".btn-repeater-price-remove").removeClass("hidden");
    } else {
        $(".price-group").prop("disabled", "disabled");
        $('.price-group.selectpicker').selectpicker('refresh');
        $(".btn-repeater-price-add").addClass("hidden");
        $(".btn-repeater-price-remove").addClass("hidden");
    }    

    $("#attributesRpt").find(".ddl-color-picker-new").each(function () {
        $(this).selectpicker();
        $(this).removeClass("ddl-color-picker-new");
    });

    $("#IsUpdatePrice").change();

    InitPriceChange();
});
