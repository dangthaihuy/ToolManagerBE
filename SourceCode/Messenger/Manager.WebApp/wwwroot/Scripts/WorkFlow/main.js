function AfterSelectWorkFlow(selectedItems) {
    WorkFlowSearchTool.SearchModalHide();
    if (selectedItems && selectedItems.length > 0) {
        for (var i = 0; i < selectedItems.length; i++) {
            WorkflowControl.PushItem(selectedItems[i]);
        }
        WorkflowControl.ClearHtmlStep();
        WorkflowControl.GenerateHtmlStep();
    }
}