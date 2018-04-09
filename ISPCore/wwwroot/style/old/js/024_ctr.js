/** Генератор CTR **/

function countMax(a){
    var m = 0;
    
    for(var i = 0; i < a.length; i++) m = Math.max(m,a[i]);
    
    return m;
}

function toSimple(ob){
    var a = [];
    
    for(var i in ob){
        a.push(ob[i].y);
    }
    
    return a;
}

var c_wave = 2; //количество волн


function stream_layers(n, m, o) {
    if (arguments.length < 3)
        o = 0;
    function bump(a) {
        var x = 1 / (.1 + Math.random()),
                y = 2 * Math.random() - .5,
                z = c_wave / (.1 + Math.random());
        for (var i = 0; i < m; i++) {
            var w = (i / m - y) * z;
            a[i] += x * Math.exp(-w * w);
        }
    }
    return d3range(n).map(function () {
        var a = [], i;
        for (i = 0; i < m; i++)
            a[i] = o + o * Math.random();
        for (i = 0; i < 245; i++)
            bump(a);
        return a.map(stream_index);
    });
}

/* Another layer generator using gamma distributions. */
function stream_waves(n, m) {
    return d3range(n).map(function (i) {
        return d3range(m).map(function (j) {
            var x = 20 * j / m - i / 3;
            return 2 * x * Math.exp(-.5 * x);
        }).map(stream_index);
    });
}

function stream_index(d, i) {
    return {x: i, y: Math.max(0, d)};
}

function d3range(start, stop, step) {
    if (arguments.length < 3) {
      step = 1;
      if (arguments.length < 2) {
        stop = start;
        start = 0;
      }
    }
    if ((stop - start) / step === Infinity) throw new Error("infinite range");
    var range = [], k = d3_range_integerScale(Math.abs(step)), i = -1, j;
    start *= k, stop *= k, step *= k;
    if (step < 0) while ((j = start + step * ++i) > stop) range.push(j / k); else while ((j = start + step * ++i) < stop) range.push(j / k);
    return range;
}

function d3_range_integerScale(x) {
    var k = 1;
    while (x * k % 1) k *= 10;
    return k;
}


/** CTR **/

var CTRChar = function(selector,points){
    this.conteiner = $(selector);
    this.width     = this.conteiner.actual('width');
    this.inProcent = true;
    this.average   = true;
    this.height    = 350;
    this.sections  = 24;
    this.division  = 10;
    this.points    = points;
    this.addPoints = [];
    
    var scope = this;
    
    
    
    this.init = function(){
        this.content = $('<div class="ctr clearfix"><canvas width="100" height="100" class="ctr_canvas"></canvas></div>').appendTo(this.conteiner);
        this.canvas  = $('canvas',this.content);
        
        this.context = this.canvas.get(0).getContext('2d');
        
        $(window).resize(function(){
            scope.update();
        })
        
        if(this.average){
            var max = this.calculate_average(this.points)*2;
            
            for(var i = 0; i < this.points.length; i++){
                if(this.points[i] > max) this.points[i] = max;
            }
        }
        
        this.update();
    }
    
    this.add = function(points,color){
        //var lar = stream_layers(2,1440,1.8);
    
        //points = toSimple(lar[0]);
        //color = 'rgb(131, 119, 180)';
    
    
        this.addPoints.push({points:points,color:color});
        this.draw();
    }
    
    this.addTimePoints = function(points,color){
        this.addPoints.push({points:points,color:color,type:'time'});
        this.draw();
    }
    
    this.calculate_average = function(arr) {
        var
            x, correctFactor = 0,
            sum = 0
        ;
        for (x = 0; x < arr.length; x++) {
            arr[x] = +arr[x];
            if (!isNaN(arr[x])) {
                sum += arr[x];
            } else {
                correctFactor++;
            }
        }
        return (sum / (arr.length - correctFactor)).toFixed(2);
    }
    
    this.update = function(){
        this.width = this.conteiner.actual('width');

        this.context.canvas.width  = this.width;
        this.context.canvas.height = this.height;
        
        this.draw();
    }
    
    this.draw = function(){
        this.context.clearRect(0, 0, this.width, this.height);
        
        this.drawBoard();
        this.drawPoints();
        
        for(var i = 0; i < this.addPoints.length; i++) this.drawPoints(this.addPoints[i]);
    }
    
    this.drawBoard = function(){
        this.context.beginPath();
        
        var wb = Math.round(this.width/this.sections);
        var hb = Math.round(this.height/this.division);
        
        for(var x = wb,i = 1; x < this.width; x += wb,i++) {
            this.context.moveTo(0.5 + x, 0);
            this.context.lineTo(0.5 + x, this.height);
            
            this.drawText(i+'H', x+4, 10);
        }
        
        var max    = countMax(this.points),
            secNum = max/this.division;
    
        for(var x = hb,i = 0; x <= this.height; x += hb,i++) {
            this.context.moveTo(0, 0.5 + x);
            this.context.lineTo(this.width, 0.5 + x);
            
            this.drawText(this.inProcent ? 100-(x/this.height*100)+'%' : (max - (secNum*i)).toFixed(1), 4, x-4);
        }
    
        this.context.strokeStyle = "#ddd";
        this.context.lineWidth   = .5;
        this.context.stroke();
    }
    
    this.drawPoints = function(otherPoints){
        this.context.beginPath();
        
        var points = otherPoints ? otherPoints.points : this.points,
            color  = otherPoints && otherPoints.color ? otherPoints.color : "rgb(31, 119, 180)",
            type   = otherPoints && otherPoints.type  ? otherPoints.type  : 'none',
            max    = countMax(points);
        
        if(type == 'time'){
            var stime = 0;
            
            for(var i = 0; i < points.length; i++){
                var pointTime = points[i];
                
                stime += pointTime;
                
                var siw = (this.width/(this.sections*60)),
                    sih = this.height,
                    px1 = siw*stime,
                    vl1 = this.height,
                    vl2 = 0;
        
                this.context.moveTo(px1, vl1);
                this.context.lineTo(px1, vl2);
                
            }
        }
        else{
            for(var i = 0; i < points.length-1; i++){
                var siw = (points.length/60) * (this.width/this.sections) / points.length,
                    sih = this.height/max,
                    px1 = siw*i,
                    px2 = siw*(i+1),
                    vl1 = this.height-(points[i]*sih),
                    vl2 = this.height-(points[i+1]*sih);
        
                this.context.moveTo(px1, vl1-1);
                this.context.lineTo(px2, vl2-1);
                
            }
        }
        
        this.context.strokeStyle = color;
        this.context.stroke();
    }
    
    
    this.drawText = function(n,x,y){
        this.context.fillStyle = "#000";
        this.context.font      = "10px Arial";
        this.context.fillText(n,x,y);
    }
}