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


function getApplicantLesson(source) {
    $.ajax({
        type: "GET",
        data: { id: source.id },
        url: "/Applicant/GetLesson",
        dataType: "json",
        success: function (obj) {
            $('#myModal .modal-body tbody').empty();
            for (var i = 0; i < obj.length; i++) {
                $('#myModal .modal-body tbody').append(
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
}


function getLessons() {
    $.ajax({
        type: "GET",
        url: "/Lesson/GetLessons",
        dataType: "json",
        success: function (obj) {
            $('#addLessonModal .modal-body tbody').empty();
            for (var i = 0; i < obj.length; i++) {
                $('#addLessonModal .modal-body tbody').append(
                    "<tr>" +
                    "<td>" +
                    "<input type=" + "radio" + " id=" + obj[i].id + " name=" + "check" + ">" +
                    "</td>" +
                    "<td>" + obj[i].name + "</td>" +
                    "<td>" + obj[i].firstName + " " + obj[i].lastName + "</td>" +
                    "<td>" + obj[i].startingDate.split("T")[0] + "</td>" +
                    "<td>" + obj[i].endingDate.split("T")[0] + "</td>" +
                    "</tr>"
                );
            }
        }
    });
}

function getStudentLessons(source) {
    $.ajax({
        type: "GET",
        data: { id: source.id },
        url: "/Student/GetLessons",
        dataType: "json",
        success: function (obj) {
            $('#myModal .modal-body tbody').empty();
            for (var i = 0; i < obj.length; i++) {
                $('#myModal .modal-body tbody').append(
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
}

function addLesson() {
    var id = getIdSelectedRadioButton();
    var array = getIdSelectedRows();
    if (array.length == 0 || id == null) {
        alert("You have not chosen a student or lesson.");
        return;
    }
    $.ajax({
        type: "post",
        data: {
            lessonId: id,
            arrayOfStudentId: array
        },
        url: "/Student/AddLessonToStudent",
        dataType: "json",
        success: function (obj) {
            if (obj == true) {
                alert("Lesson Added.");
            }
            else {
                alert("Invalid Operation!!!");
            }
        },
        error: function (obj) {
            alert("The student already has this lesson. Try again.");
        }
    });
}

function getIdSelectedRadioButton() {
    var radio = $('[name="check"]:checked');
    if (radio.length == 0) {
        return;
    }
    return radio[0].id;
}

function toggle(source) {
    var checkboxes = $('[name="app"]');
    for (var i = 0; i < checkboxes.length; i++) {
        checkboxes[i].checked = source.checked;
    }
}

function makeStudent() {
    var arrayOfId = getIdSelectedRows();
    if (arrayOfId.length == 0) {
        return;
    }
    //$.ajax({
    //    type: "post",
    //    data: { arrayOfId },
    //    url: "/Applicant/MakeStudent",
    //    success: function () {
    //        refreshApplicantsTable(arrayOfId);
    //    }
    //});
    refreshApplicantsTable(arrayOfId);
}

function refreshApplicantsTable(arrayOfId) {
    for (var i = 0; i < arrayOfId.length; i++) {
        $("tr").remove('#' + arrayOfId[i]);
    }

    //$.ajax({
    //    type: "GET",
    //    url: "/Applicant/GetApplicantsForPartial",
    //    dataType: "html",
    //    success: function (obj) {
    //        $('.myTbody').empty();
    //        $('.myTbody').append(obj);
    //    }
    //});
}

//get array of id of the selected rows
function getIdSelectedRows() {
    var checkboxes = $('[name="app"]');
    var id = [];
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].checked) {
            id.push(checkboxes[i].id);
        }
    }
    return id;
}