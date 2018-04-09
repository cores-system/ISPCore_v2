$(function(){
    /* ION Range Slider Samples */
    
    
    
    var group_sliders  = $('.group-sliders input');
    var availableTotal = 100;
    var asliders       = [];
    var lastInx        = 0;
    
    function updateVal(){
        var total = 0;

        for(var i = 0; i < asliders.length; i++){
            if(i == lastInx) continue;
            
            total += parseInt(asliders[i].data('from'));
        };
        
        var max = availableTotal - total;
            max = max < 0 ? 0 : max > availableTotal ? availableTotal : max;
        
        var slid = asliders[lastInx],
            moc  = parseInt(slid.data('from'));
            
        if(moc >= max){
            slid.data("ionRangeSlider").update({from:max})
            slid.data("ionRangeSlider").reset();
        } 
    }
    
    group_sliders.each(function(){
        var inx = asliders.length;
        
        var slid = $(this).ionRangeSlider({
            step: 1,
            min: 0,
            max: 100,
            from: parseInt($(this).val()),
            onFinish: updateVal,
            onChange:function(option){
                
                lastInx = inx;
                /*
                var total = 0;

                for(var i = 0; i < asliders.length; i++){
                    if(i == inx) continue;
                    
                    total += parseInt(asliders[i].data('from'));
                };
                
                var max = availableTotal - total;
                    max = max < 0 ? 0 : max > availableTotal ? availableTotal : max;
                
                if(option.from > max){
                    slid.data("ionRangeSlider").update({value:max})
                }
                
                console.log(max,option.from)
                */
                /*
                var total = 0;

                for(var i = 0; i < asliders.length; i++){
                    total += parseInt(asliders[i].data('from'));
                };

                var delta = availableTotal - total;
                
                // Update each slider
                for(var i = 0; i < asliders.length; i++){
                    if(i == inx) continue;
                    
                    var sld   = asliders[i],
                        value = parseInt(sld.data('from'));
    
                    var new_value = value + (delta/2);
                    
                    if (new_value < 0 || option.from == 100)  new_value = 0;
                    if (new_value > 100)                      new_value = 100;
                    
                    sld.data("ionRangeSlider").update({
                        from:new_value
                    })
                    
                    group_sliders.eq(i).val(new_value);
                    
                };*/
            }
        })
        
        asliders.push(slid);
    })
    
    $('.group-sliders').on('mouseup',updateVal);
    
    
 /*   
    
    var sliders        = $("#sliders .slider");
    var availableTotal = 100;
    var asliders       = [];
    var initSliders    = false;
    
    function initSlider(){
        if(initSliders) return;
        
        sliders.each(function(){
            var inx = asliders.length;
            
            var slid = $(this).freshslider({
                range: false, // true or false
                step: 1,
                text: true,
                min: 0,
                max: 100,
                unit: '%', // the unit which slider is considering
                enabled: true, // true or false
                value: parseInt($(this).text()), // a number if unranged , or 2 elements array contains low and high value if ranged
                onchange:function(svalue){
                    var total = 0;
                    
                    for(var i = 0; i < asliders.length; i++){
                        total += parseInt(asliders[i].getValue()[0]);
                    }
        
                    var delta = availableTotal - total;
                    
                    for(var i = 0; i < asliders.length; i++){
                        if(i == inx) continue;
                        
                        var sld   = asliders[i],
                            value = parseInt(sld.getValue()[0]);
        
                        var new_value = value + (delta/2);
                        
                        if(new_value < 0 || svalue == 100)  new_value = 0;
                        if(new_value > 100)  new_value = 100;
                        
                        sld.setValue(new_value,true)
                    }
                }
            })
            
            asliders.push(slid);
            
            slid.name = $(this).attr('name');
            slid.value = parseInt($(this).text());
        })
        
        initSliders = true;
        
        for(var i = 0; i < asliders.length; i++){
            asliders[i].setValue(asliders[i].value,true)
        }
    }
    
    
    */
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    //Default
    $("#ise_default").ionRangeSlider();
    //End Default
    
    //Min Max Value
    $(".ise_min_maxsss").ionRangeSlider({
        min: 100,
        max: 1000,
        from: 550
    });
    //End Min Max Value
    
    //Prefix
    $("#ise_prefix").ionRangeSlider({
        type: "double",
        grid: true,
        min: 0,
        max: 1000,
        from: 200,
        to: 800,
        prefix: "$"
    });
    //End Prefix
    
    //Step
    $("#ise_step").ionRangeSlider({
        type: "double",
        grid: true,
        min: 0,
        max: 10000,
        from: 3000,
        to: 7000,
        step: 250
    });
    //End Step
    
    //Custom Values
    $("#ise_custom_values").ionRangeSlider({
        grid: true,
        from: 3,
        values: [
            "January", "February", "March",
            "April", "May", "June",
            "July", "August", "September",
            "October", "November", "December"
        ]
    });    
    //End Custom Values
    
    //Decorate
    $("#ise_decorate").ionRangeSlider({
        type: "double",
        min: 100,
        max: 200,
        from: 145,
        to: 155,
        prefix: "Weight: ",
        postfix: " million pounds",
        decorate_both: false
    });
    //End Decorate
    
    //Disabled
    $("#ise_disabled").ionRangeSlider({
        min: 0,
        max: 100,
        from: 30,
        disable: true
    });
    //End Disabled
    
    /* END ION Range Slider Samples */
});