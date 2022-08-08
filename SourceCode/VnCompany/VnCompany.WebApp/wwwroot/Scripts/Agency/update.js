$(".m-select2").select2({

});

$(".select-country").change(function () {
    var counter = parseInt($("#CountrySelectCounter").val());
    counter++;

    $("#CountrySelectCounter").val(counter);

    var id = $(this).val();
    var params = $.extend({}, doAjax_params_default);
    params['url'] = '/Master/GetRegionsByCountry';
    params['requestType'] = 'POST';
    params['data'] = { id: id };
    params['showloading'] = false;
    params['dataType'] = "html";

    params['successCallbackFunction'] = function (result) {
        $(".select-region").html(result);
        $(".select-prefecture").find('option').not(':first').remove();
        $(".select-city").find('option').not(':first').remove();

        if (counter === 1) {
            $(".select-region").val($("#CurrentRegion").val());
            $(".select-region").change();
        }
    };
    doAjax(params);
});

$(".select-region").change(function () {
    var counter = parseInt($("#RegionSelectCounter").val());
    counter++;

    $("#RegionSelectCounter").val(counter);

    var id = $(this).val();
    var params = $.extend({}, doAjax_params_default);
    params['url'] = '/Master/GetPrefecturesByRegion';
    params['requestType'] = 'POST';
    params['data'] = { id: id };
    params['showloading'] = false;
    params['dataType'] = "html";

    params['successCallbackFunction'] = function (result) {
        $(".select-prefecture").html(result);
        $(".select-city").find('option').not(':first').remove();

        if (counter == 1) {
            $(".select-prefecture").val($("#CurrentPrefecture").val());
            $(".select-prefecture").change();
        }
    };
    doAjax(params);
});

$(".select-prefecture").change(function () {
    var counter = parseInt($("#PrefectureSelectCounter").val());
    counter++;

    $("#PrefectureSelectCounter").val(counter);

    var id = $(this).val();
    var params = $.extend({}, doAjax_params_default);
    params['url'] = '/Master/GetCitiesByPrefecture';
    params['requestType'] = 'POST';
    params['data'] = { id: id };
    params['showloading'] = false;
    params['dataType'] = "html";

    params['successCallbackFunction'] = function (result) {
        $(".select-city").html(result);

        if (counter == 1) {
            $(".select-city").val($("#CurrentCity").val());
            $(".select-city").change();
        }
    };

    doAjax(params);
});

$(function () {
    $(".select-country").change();
});