$("#taxvaluesRpt").repeater({
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
            $("#taxvaluesRpt").find(".rpt-item-container").each(function () {
                counter++;
            });

            if (counter < 0)
                counter = 0;

            $("#TaxValueRecordsCount").html(counter);

            InitValueChange();
        }, 100);
    },
    hide: function (e) {
        $(this).slideUp(e);
        setTimeout(function () {
            var counter = 0;
            $("#taxvaluesRpt").find(".rpt-item-container").each(function () {
                counter++;
            });

            if (counter < 0)
                counter = 0;

            $("#TaxValueRecordsCount").html(counter);
        }, 500);
    }
});

$("#IsUpdateValue").change(function () {
    if ($("#IsUpdateValue").prop("checked") === true) {
        $(".taxvalue-group").removeAttr("disabled");
        $(".btn-repeater-taxvalue-add").removeClass("hidden");
        $(".btn-repeater-taxvalue-remove").removeClass("hidden");
    } else {
        $(".taxvalue-group").prop("disabled", "disabled");
        $(".btn-repeater-taxvalue-add").addClass("hidden");
        $(".btn-repeater-taxvalue-remove").addClass("hidden");
    }

    $('.taxvalue-group.selectpicker').selectpicker('refresh');
});

function InitValueChange() {
    $(".rpt-taxvalue-input").bind("input", function () {
        var el = $(this);

        el.val(StringNormally(el.val()))

        var currentVal = parseFloat(el.val());
        if (isNaN(currentVal)) {
            currentVal = 0;
        }        
    });

    
}

$("body").on("click", ".taxvalue-default", function () {
    var el = $(this);
    $(".taxvalue-default").prop("checked", false);
    el.prop("checked", true);
});

$(".btn-submit-tax").click(function () {
    var frm = $("#frmTaxUpdate");

    var isValid = frm.valid();

    if (isValid) {
        $("#taxvaluesRpt").find(".tax-value").each(function () {
            var el = $(this);
            el.val(StringNormally(el.val()));

            var value = parseFloat(el.val());
            if (isNaN(value)) {
                value = 0;
            }

            if (value === 0) {
                $.showErrorMessage(LanguageDic["LB_ERROR_OCCURED"], `税額を入力してください`, function () { el.focus(); return false; });
                isValid = false;

                return false;
            }
        });
    }

    if (isValid) {
        frm.submit();
    }
});

$(function () {
    $("#IsUpdateValue").change();

    InitValueChange();
});
