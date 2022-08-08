var CustomerGlobal = {
    OfferPageIdx: 1,
    GetOfferList: function (idx) {
        var ctn = $("#OfferList");               

        if (!ctn.hasClass("loaded")) {
            CustomerGlobal.idx = idx;
            ctn.html("");
            ctn.areaLoading("#JobListTab");

            var frmData = $("#frmOfferList").serializeArray();
            frmData.push({name: 'Page', value: idx });
            frmData.push({ name: 'CustomerId', value: $("#CustomerId").val() });   
                    
            $.aPost("/Customer/GetOfferList", frmData, function (result){
                if (result) {                    
                    ctn.html(result);
                }
            },"html");
        }
    }
};

$("body").on("click", ".OfferListTab", function () {
    CustomerGlobal.GetOfferList(1);
});        