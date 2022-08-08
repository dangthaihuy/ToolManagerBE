function AfterSelectForm(selectedItems) {
    FormSearchTool.SearchModalHide();
    if (selectedItems && selectedItems.length > 0) {
        for (var i = 0; i < selectedItems.length; i++) {
            FormControl.PushItem(selectedItems[i]);
            if (selectedItems[i].info) {
                let html = ` <div class="row form-item mb15" id="block${selectedItems[i].info.Id}" data-id="${selectedItems[i].info.Id}">
                                <div class="col-md-12">
                                    <div class="row">
                                        <input type="hidden" id="form-item-fields-${selectedItems[i].info.Id}" class="form-item-fields" value="" />
                                        <div class="col-md-6 text-left">
                                            <h5>${selectedItems[i].info.Name}</h5>
                                        </div>
                                        <div class="col-md-6 text-right">
                                            <button id="btnUp-${selectedItems[i].info.Id}" type="button" class="btn m-btn--pill m-btn--air  btn-outline-primary btn-sm" onclick="moveUpForm(this)">
                                                <i class="fa fa-arrow-up"></i>
                                            </button>
                                            <button id="btnDown-${selectedItems[i].info.Id}" type="button" class="btn m-btn--pill m-btn--air btn-outline-primary btn-sm" onclick="moveDownForm(this)">
                                                <i class="fa fa-arrow-down"></i>
                                            </button>
                                            <button id="btnDelete-${selectedItems[i].info.Id}" type="button" class="btn m-btn--pill m-btn--air btn-danger btn-sm" onclick="deleteForm(this)">
                                                <i class="fa fa-trash-o"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="row" style="padding:15px;">
                                        <div class="col-md-12" id="form-item-${selectedItems[i].info.Id}"></div>
                                    </div>
                                </div>
                            </div>
                            `
                $("#formio").append(html);

                $("#form-item-fields-" + selectedItems[i].info.Id).val(JSON.stringify(selectedItems[i].info.Fields))
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
    }

}