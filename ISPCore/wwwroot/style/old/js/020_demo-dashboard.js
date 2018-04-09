"use strict";

var dashboardChartsData  = {};
var dashboardChartUpdate = [];

var dashboard = {
    init: function(selector,data,nameChart,stacked){
        
        /* dashboard chart */        
        var myColors = ["#34495e","#82b440","#F3BC65","#85d6de","#E74E40","#3B5998",
                        "#80CDC2","#A6D969","#D9EF8B","#FFFF99","#F7EC37","#F46D43",
                        "#E08215","#D73026","#A12235","#8C510A","#14514B","#4D9220",
                        "#542688", "#4575B4", "#74ACD1", "#B8E1DE", "#FEE0B6","#FDB863",                                                
                        "#C51B7D","#DE77AE","#EDD3F2"];

        d3.scale.myColors = function() {
            return d3.scale.ordinal().range(myColors);
        };
        
        nv.addGraph({
            generate: function() {                

                var chart = nv.models.multiBarChart()                    
                    .stacked(stacked !== undefined ? stacked : false)
                    .color(d3.scale.myColors().range())
                    .margin({top: 0, right: 0, bottom: 20, left: 20})
                
				chart.yAxis.tickFormat(d3.format(''))
                
                var svg = d3.select(selector).datum(data);
                
                svg.transition().duration(0).call(chart);
                
                $( window ).resize(chart.update);
                
                var date = new Date(),
                    minu = date.getMinutes()+'',
                    minu = (10 - parseInt(minu[1]))+1;
                
                var update = function(){
                    if(dashboardChartsData[nameChart]){
                        d3.select(selector).datum(dashboardChartsData[nameChart]);
                        svg.transition().duration(0).call(chart);
                    }
                }
                
                dashboardChartUpdate.push(update);
                
                setTimeout(function(){
                    update();
                    
                    setInterval(update,1000*60*10)
                },1000*60*minu)
                
                return chart;
            }
        });                
        /* ./dashboard chart */                
        
    }
};

  
//dashboard.init('#dashboard-chart svg');
