var Holiday = {
    getHoliday: function () {
        $.get("/Holiday", function (holidaysData) {
            $(".datepicker-holiday").datepicker({
                //language: "ja",
                todayBtn: "linked",
                clearBtn: !0,
                todayHighlight: !0,
                format: "yyyy/mm/dd",
                beforeShowDay: function (date) {
                    if (date.getDay() == 0) {
                        return {
                            classes: 'day-sunday'
                        };
                    } else if (date.getDay() == 6) {
                        return {
                            classes: 'day-saturday'
                        };
                    }

                    var holidays = holidaysData.holidays;
                    if (typeof holidays !== 'undefined') {
                        for (var i = 0; i < holidays.length; i++) {
                            var holiday = new Date(Date.parse(holidays[i].DateStr));
                            if (holiday.getYear() == date.getYear() &&
                                holiday.getMonth() == date.getMonth() &&
                                holiday.getDate() == date.getDate()) {
                                return {
                                    classes: 'day-holiday'
                                };
                            }
                        }
                    }

                    return {
                        classes: 'day-weekday'
                    };
                }
            }).on('change', function () {
                $('.datepicker').hide();
            });;
        });
    }
}

$(function () {
/*    $.fn.datepicker.dates['ja'] = {
        days: ["日曜", "月曜", "火曜", "水曜", "木曜", "金曜", "土曜"],
        daysShort: ["日", "月", "火", "水", "木", "金", "土"],
        daysMin: ["日", "月", "火", "水", "木", "金", "土"],
        months: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
        monthsShort: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
        today: "今日",
        format: "yyyy/mm/dd",
        titleFormat: "yyyy年mm月",
        clear: "クリア"
    };*/

    Holiday.getHoliday();
});