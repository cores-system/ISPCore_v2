var RickshawGraphData = {};

var RickshawGrap = function(name){
    var seriesData = [ [] ];
    
    var rlc = new Rickshaw.Graph( {
        element: document.getElementById("rickshaw-lines-"+name),
        renderer: 'line',
        series: [{color: "#34495e",data: seriesData[0],name: 'Новых'}]
    });
    
    rlc.render();    
    
    var hoverDetail = new Rickshaw.Graph.HoverDetail({graph: rlc});
    var axes = new Rickshaw.Graph.Axis.Time({graph: rlc});
    
    axes.render();
    
    var rlc_resize = function() {              
        rlc.configure({
            width: $("#rickshaw-lines-"+name).width(),
            height: $("#rickshaw-lines-"+name).height()
        });
        rlc.render();
    }
    
    window.addEventListener('resize', rlc_resize);
     
    rlc_resize();
    
    console.log(rlc)
    
    RickshawGraphData[name] = this;
    
    this.update = function(data){
        rlc.series[0].data = data;
        rlc.update();
    }
}