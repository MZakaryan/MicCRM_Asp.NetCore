
(function ($) {
    "use strict";
    $('.column100').on('mouseover', function () {
        var table1 = $(this).parent().parent().parent();
        var table2 = $(this).parent().parent();
        var verTable = $(table1).data('vertable') + "";
        var column = $(this).data('column') + "";

        $(table2).find("." + column).addClass('hov-column-' + verTable);
        $(table1).find(".row100.head ." + column).addClass('hov-column-head-' + verTable);
    });

    $('.column100').on('mouseout', function () {
        var table1 = $(this).parent().parent().parent();
        var table2 = $(this).parent().parent();
        var verTable = $(table1).data('vertable') + "";
        var column = $(this).data('column') + "";

        $(table2).find("." + column).removeClass('hov-column-' + verTable);
        $(table1).find(".row100.head ." + column).removeClass('hov-column-head-' + verTable);
    });


})(jQuery);

window.onload = function () {
    $('.appTableBtn').click(function () {
        $.ajax({
            type: "GET",
            data: { id: this.id },
            url: "/Applicant/GetLesson",
            dataType: "json",
            success: function (obj) {
                for (var i = 0; i < obj.length; i++) {
                    $('.modal-body tbody').empty();
                    $('.modal-body tbody').append(
                        "<tr>" + 
                            "<td>" + obj[i].name + "</td>" +
                            "<td>" + obj[i].firstName + " " + obj[i].lastName + "</td>" +
                            "<td>" + obj[i].startingDate.split("T")[0] + "</td>" +
                            "<td>" + obj[i].endingDate.split("T")[0] + "</td>" +
                        "</tr>"
                    );
                }
            }
        });
    });
};