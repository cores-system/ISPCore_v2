var TeGraf = function(selector){
    this.conteiner = $(selector);
    this.input     = $('input',this.conteiner);
    this.width     = this.conteiner.actual('width');
    this.height    = 300;
    this.days      = 7;
    this.daysMinus = this.days - 1;
    this.points    = [];
    this.traffic   = 10000;
    this.dayName   = ["Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"];
    
    var scope = this;
    
    this.init = function(){
        this.content = $('<div class="clearfix"><div class="TeGraf_Types clearfix"><div><b></b> Трафик</div><div><b class="red"></b> Результат трафика</div></div><canvas width="100" height="100" class="TeGraf"></canvas><div class="TeGraf_Dates"></div></div>').appendTo(this.conteiner);
        this.canvas  = $('canvas',this.content);
        
        this.context = this.canvas.get(0).getContext('2d');
        
        this.canvas.on('click',function(e){
            var offset    = $(this).offset();
            var relativeX = (e.pageX - offset.left);
            var relativeY = (e.pageY - offset.top);
            
            for(var i = 0; i < scope.days; i++){
                var point = scope.points[i]
                    x     = scope.getX(i),
                    p     = 20;
                
                if(relativeX < x + p && relativeX > x - p){
                    scope.points[i] = 1 - (relativeY/scope.height);
                    
                    scope.draw();
                    
                    scope.input.val(JSON.stringify(scope.points));
                    
                    break;
                }
            }
        })
        
        try{
            this.points = JSON.parse(this.input.val());
        }
        catch(e){
            this.createPoints();
        }
        
        var procent = this.width / this.daysMinus / this.width * 100;
        
        for(var i = 0; i < this.daysMinus; i++){
            $('<div>'+this.dayName[i]+'</div>').css('width',procent+'%').appendTo($('.TeGraf_Dates',this.content));
        }
        
        $(window).resize(function(){
            scope.update();
        })
        
        this.update();
    }
    
    this.update = function(){
        this.width = this.conteiner.actual('width');

        this.context.canvas.width  = this.width;
        this.context.canvas.height = this.height;
        
        this.draw();
    }
    
    this.getX = function(i){
        return (this.width/this.daysMinus)*i;
    }
    
    this.getY = function(y){
        return this.height - (this.height*y);
    }
    
    this.draw = function(){
        this.context.clearRect(0, 0, this.width, this.height);
        
        this.drawBoard();
        this.drawPoints();
        this.drawTraffic();
    }
    
    this.createPoints = function(){
        this.points = [];
        
        for(var i = 0; i <= this.days; i++){
            this.points[i] = 0.5;
        }
        
        this.input.val(JSON.stringify(this.points));
    }
    
    this.drawBoard = function(){
        this.context.beginPath();
        
        var wb = Math.round(this.width/this.daysMinus);
        var hb = Math.round(this.height/8);
        
        for (var x = wb; x < this.width; x += wb) {
            this.context.moveTo(0.5 + x, 0);
            this.context.lineTo(0.5 + x, this.height);
        }
    
        for (var x = hb; x <= this.height; x += hb) {
            this.context.moveTo(0, 0.5 + x);
            this.context.lineTo(this.width, 0.5 + x);
        }
    
        this.context.strokeStyle = "#ddd";
        this.context.stroke();
    }
    
    this.drawPoints = function(){
        if(!this.points.length) this.createPoints();
        
        this.context.beginPath();
        
        for (i = 0; i < this.points.length - 1; i ++){
            this.context.moveTo(this.getX(i), this.getY(this.points[i]));
            this.context.lineTo(this.getX(i+1), this.getY(this.points[i+1]));
        }
        
        this.context.strokeStyle = "rgb(31, 119, 180)";
        this.context.stroke();
        
        for (i = 0; i < this.points.length - 1; i ++){
            this.drawCircle(this.getX(i), this.getY(this.points[i]),"rgb(31, 119, 180)")
        }
    }
    
    this.randow = function(min,max){
        return Math.random() * (max - min) + min;
    }
    
    this.drawTraffic = function(){
        var max = 1000;
        var newPoints = [];
        
        for (i = 0; i < this.points.length ; i ++){
            var m = 1 + this.randow(0,0.6),
                b = this.traffic * m,
                c = b * (this.points[i]);
            
            newPoints.push(c);
            
            max = Math.max(max,c);
        }
        
        this.context.beginPath();
        
        var x = this.getX(0),
            y = this.height - ((newPoints[0]/max) * this.height);
        
        this.context.moveTo(x,y);
        
        this.drawText(newPoints[0],x,y);
        
        for (i = 1; i < newPoints.length ; i ++){
            x = this.getX(i);
            y = this.height - ((newPoints[i]/max) * this.height);
            
            this.context.lineTo(x,y);
            
            this.drawText(newPoints[i],x,y);
        }
        
        this.context.strokeStyle = "rgba(255,0,0,0.3)";
        this.context.stroke();
    }
    
    this.drawText = function(n,x,y){
        this.context.fillStyle = "#000";
        this.context.font      = "11px Arial";
        this.context.fillText(Math.round(n),x + 5,y+15);
    }
    
    this.drawCircle = function(x,y,color){
        this.context.beginPath();
        this.context.arc(x, y, 3, 0, 2 * Math.PI, false);
        this.context.fillStyle = color;
        this.context.fill();
    }
}