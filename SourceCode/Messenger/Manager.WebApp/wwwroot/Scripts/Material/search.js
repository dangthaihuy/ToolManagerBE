var MaterialSearchTool = {
    SelectedItems: [],
    SearchLoaded: false,
    SelectMultiple: true,
    SearchPageIdx: 1,    
    SearchCallbackFunc: null,
    SearchCallbackFuncName: "",
    Search: function (idx = 1, more = false) {
        var ctn = $("#modalSearchMaterialContent");
        MaterialSearchTool.SearchPageIdx = idx;

        if (!more) {
            if (!MaterialSearchTool.SearchLoaded) {
                ctn.html("");
                ctn.areaLoading();
            }
        }
        var sIncludePack = $("#frmMaterialSearch").find(".search-include-pack");
        var include_pack = true;
        if (sIncludePack) {
            include_pack = sIncludePack.val();
            if (include_pack == null || include_pack === "") {
                include_pack = true;
            }
        }

        var frmData = $("#frmMaterialSearch").serializeArray();
        frmData.push({ name: "Page", value: idx });
        frmData.push({ name: "IncludePack", value: include_pack });
        frmData.push({ name: "CallbackFunction", value: MaterialSearchTool.SearchCallbackFuncName });

        $.aPost("/Material/Search", frmData, function (result) {
            if (result) {
                if (!MaterialSearchTool.SearchLoaded) {
                    ctn.html(result);
                } else {
                    if (!more) {
                        ctn.html(result);
                    } else {
                        ctn.find("#MaterialSearchResults").append(result);
                    }
                }

                $("#modalSearchMaterial").find(".selectpicker").selectpicker().removeClass("selectpicker");

                MaterialSearchTool.SearchModalBindEvents();

                MaterialSearchTool.SearchLoaded = true;

                $("#modalSearchMaterial").modal("show");
            }
        }, "html", true);
    },
    SearchModalShow: function () {
        if (!MaterialSearchTool.SearchLoaded) {
            MaterialSearchTool.Search();
        }
        else {
            $("#modalSearchMaterial").modal("show");
        }
    },
    SearchModalHide: function () {
        $("#modalSearchMaterial").modal("hide");
    },
    SearchModalBindEvents: function () {
        //const ctn = document.querySelector('#modalSearchMaterial .search-result-container');
        //if (ctn) {
        //    ctn.addEventListener('scroll',
        //        SpamProtection(function () {
        //            if (ctn.offsetHeight + ctn.scrollTop >= ctn.scrollHeight) {
        //                var page = MaterialSearchTool.SearchPageIdx + 1;
        //                MaterialSearchTool.Search(page, true);
        //            }
        //        }, 500)
        //    );
        //}

        $(".search-material-item-cbx.new-item").on("click", function () {            
            MaterialSearchTool.ItemCheckedEvent($(this));            
        });
    },    
    Detail: function (link) {
        $.aGet(link, null, function (result) {
            if (result) {
                $("#modalMaterialDetailContent").html(result);
                MaterialSearchTool.DetailModalShow();
            }
        }, "html", true);

        MaterialSearchTool.SearchModalHide();
    },
    DetailModalShow: function () {
        $("#modalMaterialDetail").modal("show");
    },
    DetailModalHide: function () {
        $("#modalMaterialDetail").modal("hide");
    },
    SearchAllowSelect: function (callbackFuncName) {
        if (callbackFuncName !== "") {           
            window[callbackFuncName](MaterialSearchTool.SelectedItems);
            $(".search-material-item-cbx").prop("checked", false);

            MaterialSearchTool.ClearSelectedItems();
        } else {
            alert("Please define CallbackFunctionName");
        }
    },    
    ItemCheckedEvent: function (el) {
        if (!MaterialSearchTool.SelectMultiple) {
            MaterialSearchTool.ClearSelectedItems();
        }

        var hasCheckedItems = (MaterialSearchTool.SelectedItems.length > 0);
        var id = el.data("id");
        var info = el.data("info");
        let itemData = {
            id: id,
            info: info
        };

        if (el.is(":checked")) {
            if (hasCheckedItems) {
                var matched = false;
                for (var key in MaterialSearchTool.SelectedItems) {
                    if (MaterialSearchTool.SelectedItems[key]["id"] == id) {
                        matched = true;

                        break;
                    }
                }
                if (!matched) {
                    MaterialSearchTool.SelectedItems.push(itemData);
                }
            }
            else {
                MaterialSearchTool.SelectedItems.push(itemData);
            }
        } else {
            if (hasCheckedItems) {
                for (var key in MaterialSearchTool.SelectedItems) {
                    if (MaterialSearchTool.SelectedItems[key]["id"] == id) {
                        MaterialSearchTool.SelectedItems.splice(key, 1);
                        break;
                    }
                }
            }
        }

        el.removeClass("new-item");
    },
    ClearSelectedItems: function () {
        MaterialSearchTool.SelectedItems = [];
    }
}

MySiteGlobal.bindClickedEvents(".btn-search-material", function () {
    MaterialSearchTool.SearchModalShow();
});

MySiteGlobal.bindClickedEvents(".btn-search-material-show", function () {
    MaterialSearchTool.DetailModalHide();

    MaterialSearchTool.SearchModalShow();
});

MySiteGlobal.bindClickedEvents(".btn-filter-material", function () {
    MaterialSearchTool.Search(1);
});