var WorkFlowSearchTool = {
    SelectedItems: [],
    SearchLoaded: false,
    SelectMultiple: true,
    SearchPageIdx: 1,
    SearchCallbackFunc: null,
    SearchCallbackFuncName: "",
    Search: function (idx = 1, more = false) {
        var ctn = $("#modalSearchWorkFlowContent");
        WorkFlowSearchTool.SearchPageIdx = idx;

        if (!more) {
            if (!WorkFlowSearchTool.SearchLoaded) {
                ctn.html("");
                ctn.areaLoading();
            }
        }
        var sIncludePack = $("#frmWorkFlowSearch").find(".search-include-pack");
        var include_pack = true;
        if (sIncludePack) {
            include_pack = sIncludePack.val();
            if (include_pack == null || include_pack === "") {
                include_pack = true;
            }
        }

        var frmData = $("#frmWorkFlowSearch").serializeArray();
        frmData.push({ name: "CurrentPage", value: idx });
        frmData.push({ name: "IncludePack", value: include_pack });
        frmData.push({ name: "CallbackFunction", value: WorkFlowSearchTool.SearchCallbackFuncName });

        $.aPost("/MenuService/Search", frmData, function (result) {
            if (result) {
                if (!WorkFlowSearchTool.SearchLoaded) {
                    ctn.html(result);
                } else {
                    if (!more) {
                        ctn.html(result);
                    } else {
                        ctn.find("#WorkFlowSearchResults").append(result);
                    }
                }

                $("#modalSearchWorkFlow").find(".selectpicker").selectpicker().removeClass("selectpicker");

                WorkFlowSearchTool.SearchModalBindEvents();
                WorkFlowSearchTool.BindItemDetailEvents();

                WorkFlowSearchTool.SearchLoaded = true;

                $("#modalSearchWorkFlow").modal("show");
            }
        }, "html", true);
    },
    SearchModalShow: function () {
        if (!WorkFlowSearchTool.SearchLoaded) {
            WorkFlowSearchTool.Search();
        }
        else {
            $("#modalSearchWorkFlow").modal("show");
        }
    },
    SearchModalHide: function () {
        $("#modalSearchWorkFlow").modal("hide");
    },
    SearchModalBindEvents: function () {
        $(".search-workflow-item-cbx.new-item").on("click", function () {
            WorkFlowSearchTool.ItemCheckedEvent($(this));
        });
    },
    Detail: function (link) {
        $.aGet(link, null, function (result) {
            if (result) {
                $("#modalWorkFlowDetailContent").html(result);
                WorkFlowSearchTool.DetailModalShow();
            }
        }, "html", true);

        WorkFlowSearchTool.SearchModalHide();
    },
    DetailModalShow: function () {
        let data = JSON.parse($("#template-workflow").val());
        for (let i = 0; i < data.length; i++) {
            WorkflowControl.GenerateHtmlWorkflowDetail(data[i], "formio-detail");
        }
        $("#modalWorkFlowDetail").modal("show");
    },
    DetailModalHide: function () {
        $("#modalWorkFlowDetail").modal("hide");
    },
    SearchAllowSelect: function (callbackFuncName) {
        if (!WorkFlowSearchTool.SelectedItems || WorkFlowSearchTool.SelectedItems.length === 0) {
            $.showErrorMessage(LanguageDic["LB_NOTIFICATION"], "少なくとも1つの材料を選択してください", function () {
                ModalDisplayBack("modalSearchWorkFlow");
            });
        }

        if (callbackFuncName !== "") {
            window[callbackFuncName](WorkFlowSearchTool.SelectedItems);
            $(".search-workflow-item-cbx").prop("checked", false);

            WorkFlowSearchTool.ClearSelectedItems();

            if (!WorkFlowSearchTool.SelectedItems || WorkFlowSearchTool.SelectedItems.length === 0) {
                ModalDisplayBack("modalSearchWorkFlow");
            }
        } else {
            alert("Please define CallbackFunctionName");
        }
    },
    ItemCheckedEvent: function (el) {
        if (!WorkFlowSearchTool.SelectMultiple) {
            WorkFlowSearchTool.ClearSelectedItems();
        }

        var hasCheckedItems = (WorkFlowSearchTool.SelectedItems.length > 0);
        var id = el.data("id");
        var info = el.data("info");
        let itemData = {
            id: id,
            info: info
        };

        if (el.is(":checked")) {
            if (hasCheckedItems) {
                var matched = false;
                for (var key in WorkFlowSearchTool.SelectedItems) {
                    if (WorkFlowSearchTool.SelectedItems[key]["id"] == id) {
                        matched = true;

                        break;
                    }
                }
                if (!matched) {
                    WorkFlowSearchTool.SelectedItems.push(itemData);
                }
            }
            else {
                WorkFlowSearchTool.SelectedItems.push(itemData);
            }
        } else {
            if (hasCheckedItems) {
                for (var key in WorkFlowSearchTool.SelectedItems) {
                    if (WorkFlowSearchTool.SelectedItems[key]["id"] == id) {
                        WorkFlowSearchTool.SelectedItems.splice(key, 1);
                        break;
                    }
                }
            }
        }

        el.removeClass("new-item");
    },
    ClearSelectedItems: function () {
        WorkFlowSearchTool.SelectedItems = [];
    },
    BindItemDetailEvents: function () {
        $(".search-workflow-item-view.new-item").each(function () {
            var el = $(this);
            el.on("click", function () {
                var link = el.attr("data-detail");
                if (link) {
                    WorkFlowSearchTool.Detail(link);
                }
            });

            el.removeClass("new-item");
        });
    }
}

MySiteGlobal.bindClickedEvents(".btn-search-workflow", function () {
    WorkFlowSearchTool.SearchModalShow();
});

MySiteGlobal.bindClickedEvents(".btn-search-workflow-show", function () {
    WorkFlowSearchTool.DetailModalHide();

    WorkFlowSearchTool.SearchModalShow();
});

MySiteGlobal.bindClickedEvents(".btn-filter-workflow", function () {
    WorkFlowSearchTool.Search(1);
});