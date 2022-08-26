var customerEngine;
$.support.cors = true;

customerEngine = new Bloodhound({
    identify: function (o) { return o.Id; },
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Name'),
    dupDetector: function (a, b) { return a.Id === b.Id; },
    remote: {
        url: '/Customer/GetSuggestion?query=%QUERY',
        wildcard: '%QUERY'
    }
});

$(".customer-suggestion").each(function () {
    var template = Handlebars.compile($("#customer-template").html());
    var el = $(this);
    var myTypeahead = el.typeahead({
        highlight: true,
        minLength: 0,
        //classNames: {
        //    input: 'display-block',
        //    hint: 'dummy',
        //    open: 'is-open',
        //    empty: 'is-empty',
        //    cursor: 'is-active'
        //}
    }, {
        limit: 10,
        source: function (query, sync, async) {
            if (query === '') {
                //sync(customerEngine.get('a'));
                //async([]);
                customerEngine.search(query, sync, async);
            }
            else {
                customerEngine.search(query, sync, async);
            }
        },
        displayKey: 'Name',
        templates: {
            suggestion: template
        }
    })
    .on('typeahead:selected', function (evt, item) {
        evt.preventDefault();
        var ct = $(this).closest(".m-typeahead").find(".customer-suggestion-value");
        ct.val(item.Id);
    });
});

$(".customer-suggestion").bind("input", function () {    
    var ct = $(this).closest(".m-typeahead").find(".customer-suggestion-value");
    ct.val(0);
});