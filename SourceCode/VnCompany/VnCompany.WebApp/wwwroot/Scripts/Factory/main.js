var OrderControl = {
    Confirm: function (id) {
        ConfirmFirst(OrderControl.AllowConfirmation, "この注文を確認してもよろしいですか?", id);
    },
    AllowConfirmation: function (id) {
        var frmData = $("#frmFactoryOrder").serialize();
        $.aPost("/Factory/MaterialsStockChecking", frmData, function (result){
            if (result.html) {
                $("#modalStockCheckingContent").html(result.html);
                $("#myModal").modal("hide");
                $("#modalStockChecking").modal("show");
            } else {
                $.aPost("/Factory/ConfirmOrder", frmData, function (data) {
                    if (data.success) {
                         $.showSuccessMessage(LanguageDic["LB_NOTIFICATION"], data.message, function () { window.location.reload() });
                    } else {            
                        $.showErrorMessage(LanguageDic["LB_NOTIFICATION"], data.message, function () { return false; });
                    }   
                },"json", true);
            }   
        },"json", true);
    },
    SelectJanCode: function () {
        $.aGet("/JanCode/SelectJanCode", {}, function (result) {
            if (result.success) {
                $("#OfferInfo_JanCodeFinal").val(result.jancode);
            }
        }, "json", true);
    },
}

$("body").on("click", ".btn-material-order", SpamProtection(function () {
    $(this).buttonLoading();

    $("#frmMaterialStockChecking").submit();

    return false;
}, 300)
);