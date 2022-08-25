var makerEngine, providerEngine;
$.support.cors = true;

makerEngine = new Bloodhound({
    identify: function (o) { return o.Id; },
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Name'),
    dupDetector: function (a, b) { return a.Id === b.Id; },
    remote: {
        url: '/Maker/GetSuggestion?query=%QUERY',
        wildcard: '%QUERY'
    }
});

providerEngine = new Bloodhound({
    identify: function (o) { return o.Id; },
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Name'),
    dupDetector: function (a, b) { return a.Id === b.Id; },
    remote: {
        url: '/Provider/GetSuggestion?query=%QUERY',
        wildcard: '%QUERY'
    }
});

$(".maker-suggestion").each(function () {
    var template = Handlebars.compile($("#maker-template").html());
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
                //sync(makerEngine.get('a'));
                //async([]);
                makerEngine.search(query, sync, async);
            }
            else {
                makerEngine.search(query, sync, async);
            }
        },
        displayKey: 'Name',
        templates: {
            suggestion: template
        }
    })
    .on('typeahead:selected', function (evt, item) {
        evt.preventDefault();
        var ct = $(this).closest(".m-typeahead").find(".maker-suggestion-value");
        ct.val(item.Id);
    });
});

$(".maker-suggestion").bind("input", function () {    
    var ct = $(this).closest(".m-typeahead").find(".maker-suggestion-value");
    ct.val(0);
});

$(".provider-suggestion").each(function () {
    var template = Handlebars.compile($("#provider-template").html());
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
                //sync(providerEngine.get('a'));
                //async([]);
                providerEngine.search(query, sync, async);
            }
            else {
                providerEngine.search(query, sync, async);
            }
        },
        displayKey: 'Name',
        templates: {
            suggestion: template
        }
    })
        .on('typeahead:selected', function (evt, item) {
            evt.preventDefault();
            var ct = $(this).closest(".m-typeahead").find(".provider-suggestion-value");
            ct.val(item.Id);
        });
});

$(".provider-suggestion").bind("input", function () {
    var ct = $(this).closest(".m-typeahead").find(".provider-suggestion-value");
    ct.val(0);
});

$("#CurrencyId").change(function () {
    var opt = $(this).find(":selected");
    var symbol = opt.data("symbol");

    $(".currency-symbol-label").html(symbol);
});

$("#UnitId").change(function () {
    var opt = $(this).find(":selected");
    var symbol = opt.data("symbol");

    $(".unit-symbol-label").html(symbol);
});

$(function () {
    $("#UnitId").change();
    $("#CurrencyId").change();
});