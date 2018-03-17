$(document).ready(function () {


    $('#btnList').click(function () {
        $.ajax({
            type: "POST",
            url: "/ToDoes/AjaxListUpdate",
            data: { ListName: $("#txtInputList").val()},          
            success: function (response) { 
                $('#tableList').html(response);
            },
            error: function (xhr, ajaxOptions, thrownError) { alert(xhr.responseText); }
        });
    });
  
    $.ajax({
        url: '/ToDoes/showList',
        success: function (result) {
            $('#tableList').html(result);
        }
    })

    $.ajax({
        url: '/ToDoes/ToDoTable',
        success: function (result) {
            $('#tableDiv').html(result);
        }
    })

    
})