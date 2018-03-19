$(document).ready(function () {

    $.ajax({
        url: '/ToDoes/ToDoListTable',
        success: function (result) {
            $('#tableList').html(result);
        }
    })

    $("#toDoCreateForm").hide();
})