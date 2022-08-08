var FormSearchTool = {
    SelectedItems: [],
    SearchLoaded: false,
    SelectMultiple: true,
    SearchPageIdx: 1,
    SearchCallbackFunc: null,
    SearchCallbackFuncName: "",
    Search: function (idx = 1, more = false) {
        var ctn = $("#modalSearchFormContent");
        FormSearchTool.SearchPageIdx = idx;

        if (!more) {
            if (!FormSearchTool.SearchLoaded) {
                ctn.html("");
                ctn.areaLoading();
            }
        }
        var sIncludePack = $("#frmFormSearch").find(".search-include-pack");
        var include_pack = true;
        if (sIncludePack) {
            include_pack = sIncludePack.val();
            if (include_pack == null || include_pack === "") {
                include_pack = true;
            }
        }

        var frmData = $("#frmFormSearch").serializeArray();
        frmData.push({ name: "CurrentPage", value: idx });
        frmData.push({ name: "IncludePack", value: include_pack });
        frmData.push({ name: "CallbackFunction", value: FormSearchTool.SearchCallbackFuncName });

        $.aPost("/WorkFlow/Search", frmData, function (result) {
            if (result) {
                if (!FormSearchTool.SearchLoaded) {
                    ctn.html(result);
                } else {
                    if (!more) {
                        ctn.html(result);
                    } else {
                        ctn.find("#FormSearchResults").append(result);
                    }
                }

                $("#modalSearchForm").find(".selectpicker").selectpicker().removeClass("selectpicker");

                FormSearchTool.SearchModalBindEvents();
                FormSearchTool.BindItemDetailEvents();

                FormSearchTool.SearchLoaded = true;

                $("#modalSearchForm").modal("show");
            }
        }, "html", true);
    },
    SearchModalShow: function () {
        if (!FormSearchTool.SearchLoaded) {
            FormSearchTool.Search();
        }
        else {
            $("#modalSearchForm").modal("show");
        }
    },
    SearchModalHide: function () {
        $("#modalSearchForm").modal("hide");
    },
    SearchModalBindEvents: function () {
        $(".search-form-item-cbx.new-item").on("click", function () {
            FormSearchTool.ItemCheckedEvent($(this));
        });
    },
    Detail: function (link) {
        $.aGet(link, null, function (result) {
            if (result) {
                $("#modalFormDetailContent").html(result);
                FormSearchTool.DetailModalShow();
            }
        }, "html", true);

        FormSearchTool.SearchModalHide();
    },
    DetailModalShow: function () {
        let template = $("#template-form").val();
        if (template) {
            Formio.createForm(document.getElementById("formio-detail"), {
                components: JSON.parse(template)
            });    
        }
        
        $("#modalFormDetail").modal("show");
    },
    DetailModalHide: function () {
        $("#modalFormDetail").modal("hide");
    },
    SearchAllowSelect: function (callbackFuncName) {
        if (!FormSearchTool.SelectedItems || FormSearchTool.SelectedItems.length === 0) {
            $.showErrorMessage(LanguageDic["LB_NOTIFICATION"], "少なくとも1つの材料を選択してください", function () {
                ModalDisplayBack("modalSearchForm");
            });
        }

        if (callbackFuncName !== "") {
            window[callbackFuncName](FormSearchTool.SelectedItems);
            $(".search-form-item-cbx").prop("checked", false);

            FormSearchTool.ClearSelectedItems();

            if (!FormSearchTool.SelectedItems || FormSearchTool.SelectedItems.length === 0) {
                ModalDisplayBack("modalSearchForm");
            }
        } else {
            alert("Please define CallbackFunctionName");
        }
    },
    ItemCheckedEvent: function (el) {
        if (!FormSearchTool.SelectMultiple) {
            FormSearchTool.ClearSelectedItems();
        }

        var hasCheckedItems = (FormSearchTool.SelectedItems.length > 0);
        var id = el.data("id");
        var info = el.data("info");
        let itemData = {
            id: id,
            info: info
        };

        if (el.is(":checked")) {
            if (hasCheckedItems) {
                var matched = false;
                for (var key in FormSearchTool.SelectedItems) {
                    if (FormSearchTool.SelectedItems[key]["id"] == id) {
                        matched = true;

                        break;
                    }
                }
                if (!matched) {
                    FormSearchTool.SelectedItems.push(itemData);
                }
            }
            else {
                FormSearchTool.SelectedItems.push(itemData);
            }
        } else {
            if (hasCheckedItems) {
                for (var key in FormSearchTool.SelectedItems) {
                    if (FormSearchTool.SelectedItems[key]["id"] == id) {
                        FormSearchTool.SelectedItems.splice(key, 1);
                        break;
                    }
                }
            }
        }

        el.removeClass("new-item");
    },
    ClearSelectedItems: function () {
        FormSearchTool.SelectedItems = [];
    },
    BindItemDetailEvents: function () {
        $(".search-form-item-view.new-item").each(function () {
            var el = $(this);
            el.on("click", function () {
                var link = el.attr("data-detail");
                if (link) {
                    FormSearchTool.Detail(link);
                }
            });

            el.removeClass("new-item");
        });
    }
}

MySiteGlobal.bindClickedEvents(".btn-search-form", function () {
    FormSearchTool.SearchModalShow();
});

MySiteGlobal.bindClickedEvents(".btn-search-form-show", function () {
    FormSearchTool.DetailModalHide();

    FormSearchTool.SearchModalShow();
});

MySiteGlobal.bindClickedEvents(".btn-filter-form", function () {
    FormSearchTool.Search(1);
});