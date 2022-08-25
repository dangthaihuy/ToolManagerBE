var WorkflowControl = {
    SelectedItems: [],
    PushItem: function (data) {
        let itemData = {
            info: data.info,
            sortorder: WorkFlowSearchTool.SelectedItems.length + 1
        };
        WorkflowControl.SelectedItems.push(itemData);
    },
    GenerateHtmlWorkflowDetail: function (data, idDiv) {
        if (data) {
            let html = `<div class="row form-item mb15" id="block${data.Id}" data-id="${data.Id}" style="border">
                            <div class="col-md-12">
                                <div class="row" style="padding:15px;">
                                    <input type="hidden" id="form-item-fields-${data.Id}" class="form-item-fields" value="" />
                                    <h4>${data.Name}</h4>
                                </div>
                                <div class="row" style="padding:15px;">
                                    <div class="col-md-12" id="form-item-${data.Id}">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="m-separator m-separator--dashed m-separator--lg"></div>`
            $("#" + idDiv).append(html);

            $("#form-item-fields-" + data.Id).val(JSON.stringify(data.Fields))
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
    },
    ClearHtml: function () {
        var div = document.getElementById("formio_id");
        while (div.firstChild) {
            div.removeChild(div.firstChild);
        }
    },
    ClearHtmlStep: function () {
        var div = document.getElementById("demo1");
        while (div.firstChild) {
            div.removeChild(div.firstChild);
        }
    },
    GenerateHtmlStep: function () {
        for (let i = 0; i < WorkflowControl.SelectedItems.length; i++) {
            let item = WorkflowControl.SelectedItems[i];

            let done = '';
            if (i == 0) {
                done = "m-wizard__step--current";
            }
            let html = `<div class="m-wizard__step ${done}" m-wizard-target="m_wizard_form_step_${i + 1}" id="step_${item.info.Id}">
						    <div class="m-wizard__step-info">
							    <a class="m-wizard__step-number" onclick="changeWorkflow(this)" id="detail_${item.info.Id}">
								    <span>
									    <span>
										    ${i + 1}
									    </span>
								    </span>
							    </a>
							    <div class="m-wizard__step-label">
								    ${item.info.Name}
							    </div>
                                <div class="m-wizard__step-icon>
                                    <i class="la la-check"></i>
                                </div>
						    </div>
					    </div>`;
            $("#demo1").append(html);
            if (i == 0) {
                for (let j = 0; j < WorkflowControl.SelectedItems[0].info.Forms.length; j++) {

                    WorkflowControl.GenerateHtmlWorkflowDetail(WorkflowControl.SelectedItems[0].info.Forms[j], "formio_id")

                }
            }
        }
    }
}