var FormControl = {
    SelectedItems: [],
    PushItem: function (data) {
        let itemData = {
            info: data.info,
            sortorder: FormControl.SelectedItems.length + 1
        };
        FormControl.SelectedItems.push(itemData);
    },
    GenerateHtml: function () {
        for (let i = 0; i < FormControl.SelectedItems.length; i++) {
            if (FormControl.SelectedItems[i].info) {
                let html = ` <div class="row form-item mb15" id="block${FormControl.SelectedItems[i].info.Id}" data-id="${FormControl.SelectedItems[i].info.Id}">
                                <div class="col-md-12">
                                    <div class="row">
                                        <input type="hidden" id="form-item-fields-${FormControl.SelectedItems[i].info.Id}" class="form-item-fields" value="" />
                                        <div class="col-md-6 text-left">
                                            <h5>${FormControl.SelectedItems[i].info.Name}</h5>
                                        </div>
                                        <div class="col-md-6 text-right">
                                            <button id="btnUp-${FormControl.SelectedItems[i].info.Id}" type="button" class="btn m-btn--pill m-btn--air  btn-outline-primary btn-sm" onclick="moveUpForm(this)">
                                                <i class="fa fa-arrow-up"></i>
                                            </button>
                                            <button id="btnDown-${FormControl.SelectedItems[i].info.Id}" type="button" class="btn m-btn--pill m-btn--air btn-outline-primary btn-sm" onclick="moveDownForm(this)">
                                                <i class="fa fa-arrow-down"></i>
                                            </button>
                                            <button id="btnDelete-${FormControl.SelectedItems[i].info.Id}" type="button" class="btn m-btn--pill m-btn--air btn-danger btn-sm" onclick="deleteForm(this)">
                                                <i class="fa fa-trash-o"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="row" style="padding:15px;">
                                        <div class="col-md-12" id="form-item-${FormControl.SelectedItems[i].info.Id}"></div>
                                    </div>
                                </div>
                            </div>`
                $("#formio").append(html);

                $("#form-item-fields-" + FormControl.SelectedItems[i].info.Id).val(JSON.stringify(FormControl.SelectedItems[i].info.Fields))
                $(function () {
                    $(".form-item").each(function () {
                        var ct = $(this);
                        var formId = ct.data("id");
                        var hdFields = ct.find(".form-item-fields");
                        if (hdFields) {
                            var fields = JSON.parse(hdFields.val());
                            var allFields = [];
                            if (fields && fields.length > 0) {
                                for (var i = 0; i < fields.length; i++) {
                                    if (fields[i].TemplateInfo != null) {
                                        allFields.push(fields[i].TemplateInfo);
                                    }
                                }
                            }

                            Formio.createForm(document.getElementById("form-item-" + formId), {
                                components: allFields
                            });
                        }
                    });
                });
            }
        }
    },
    ClearHtml: function () {
        var div = document.getElementById("formio");
        while (div.firstChild) {
            div.removeChild(div.firstChild);
        }
    },
    SwapObj: function (obj1, obj2) {
        // create marker element and insert it where obj1 is
        var temp = document.createElement("div");
        obj1.parentNode.insertBefore(temp, obj1);

        // move obj1 to right before obj2
        obj2.parentNode.insertBefore(obj1, obj2);

        // move obj2 to right before where obj1 used to be
        temp.parentNode.insertBefore(obj2, temp);

        // remove temporary marker node
        temp.parentNode.removeChild(temp);
    },
    SwapArrayElements: function (arr, indexA, indexB) {
        var temp = arr[indexA];
        arr[indexA] = arr[indexB];
        arr[indexB] = temp;
    }
}