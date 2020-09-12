$.fn.datepicker.defaults.format = "yyyy-mm-dd";
$.fn.datepicker.defaults.language = "lt";
$.fn.datepicker.defaults.forceParse = "true";
$.fn.datepicker.defaults.autoclose = "true";
$.fn.datepicker.defaults.clearBtn = "true";

//https://stackoverflow.com/questions/28782684/how-to-highlight-specific-dates-in-bootstrap-datepicker
//https://stackoverflow.com/questions/41830332/bootstrap-datepicker-disable-specific-dates-from-array
var datesForDisable = ["2020-04-08", "2020-04-11", "2020-04-15"]


$('#datepickerDay').datepicker({
    updateViewDate: 'false',
    datesDisabled: datesForDisable,
    startDate: new Date(),
    endDate: new Date(new Date().setDate(new Date().getDate() + 30)),
    sideBySide: true,
    multidate: false,
    inline: true,
    minDate: 0
});

$('#datepickerDay').on('blur', function () {
   if ($.inArray(this.value, datesForDisable) !== -1) {
        $('#datepickerDay').val("").datepicker("update");
   }
});

