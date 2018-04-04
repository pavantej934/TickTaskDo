// Load the Visualization API and the piechart package.  
google.load('visualization', '1.0', { 'packages': ['corechart'] });

// Set a callback to run when the Google Visualization API is loaded.  
$(document).ready(function () {
   
    LoadGraph(570, 600, '/Home/GetPostLoginTopCategory', 'divGraphsTopCategory','Your Task Progress So Far');
    LoadGraph(570, 600, '/Home/GetPostLoginStatus', 'divGraphsStatus','Your Top Lists');

    $(window).resize(function () {
        if ($(window).width() > 570) {
            Height = 600;
            Width = 500;
        }
        else {
            Height = $(window).height() - 400;
            Width = $(window).width() - 70;
        }
        LoadGraph(Width, Height, '/Home/GetPostLoginTopCategory', 'divGraphsTopCategory','Your Task Progress So Far');
        LoadGraph(Width, Height, '/Home/GetPostLoginStatus', 'divGraphsStatus', 'Your Top Lists');
    });

});



function LoadGraph(Width, Height, Url, elementId, title) {
    $.ajax(
        {
            type: 'POST',
            dataType: 'JSON',
            url: Url,
            success:
            function (response) {
                // Set chart options  
                var options =
                    {
                        width: Width,
                        height: Height,
                        sliceVisibilityThreshold: 0,
                        legend: { position: "top", alignment: "end" },
                        pieHole: 0.3,
                        title: title
                        //  pieSliceTextStyle: { color: white;},
                    };

                // Draw.  
                drawGraph(response, options, elementId);
            }
        });
}




// Callback that creates and populates a data table,  
// instantiates the pie chart, passes in the data and  
// draws it.  
function drawGraph(dataValues, options, elementId) {


    var dt = new google.visualization.DataTable();
    dt.addColumn('string', 'label');
    dt.addColumn('number', 'count');

    for (var i = 0; i < dataValues.length; i++) {
        dt.addRow([dataValues[i].label, dataValues[i].count]);
    }  
  
    chartData = dt;

    // Instantiate and draw our chart, passing in some options.  
    var chart = new google.visualization.PieChart(document.getElementById(elementId));

    // Draw chart.  
    chart.draw(chartData, options);
}  