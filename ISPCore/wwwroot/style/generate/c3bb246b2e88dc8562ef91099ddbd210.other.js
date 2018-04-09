var IsConfirmDeleteElement = true;
/*! Copyright (c) 2013 Brandon Aaron (http://brandon.aaron.sh)
 * Licensed under the MIT License (LICENSE.txt).
 *
 * Version: 3.1.12
 *
 * Requires: jQuery 1.2.2+
 */
!function(a){"function"==typeof define&&define.amd?define(["jquery"],a):"object"==typeof exports?module.exports=a:a(jQuery)}(function(a){function b(b){var g=b||window.event,h=i.call(arguments,1),j=0,l=0,m=0,n=0,o=0,p=0;if(b=a.event.fix(g),b.type="mousewheel","detail"in g&&(m=-1*g.detail),"wheelDelta"in g&&(m=g.wheelDelta),"wheelDeltaY"in g&&(m=g.wheelDeltaY),"wheelDeltaX"in g&&(l=-1*g.wheelDeltaX),"axis"in g&&g.axis===g.HORIZONTAL_AXIS&&(l=-1*m,m=0),j=0===m?l:m,"deltaY"in g&&(m=-1*g.deltaY,j=m),"deltaX"in g&&(l=g.deltaX,0===m&&(j=-1*l)),0!==m||0!==l){if(1===g.deltaMode){var q=a.data(this,"mousewheel-line-height");j*=q,m*=q,l*=q}else if(2===g.deltaMode){var r=a.data(this,"mousewheel-page-height");j*=r,m*=r,l*=r}if(n=Math.max(Math.abs(m),Math.abs(l)),(!f||f>n)&&(f=n,d(g,n)&&(f/=40)),d(g,n)&&(j/=40,l/=40,m/=40),j=Math[j>=1?"floor":"ceil"](j/f),l=Math[l>=1?"floor":"ceil"](l/f),m=Math[m>=1?"floor":"ceil"](m/f),k.settings.normalizeOffset&&this.getBoundingClientRect){var s=this.getBoundingClientRect();o=b.clientX-s.left,p=b.clientY-s.top}return b.deltaX=l,b.deltaY=m,b.deltaFactor=f,b.offsetX=o,b.offsetY=p,b.deltaMode=0,h.unshift(b,j,l,m),e&&clearTimeout(e),e=setTimeout(c,200),(a.event.dispatch||a.event.handle).apply(this,h)}}function c(){f=null}function d(a,b){return k.settings.adjustOldDeltas&&"mousewheel"===a.type&&b%120===0}var e,f,g=["wheel","mousewheel","DOMMouseScroll","MozMousePixelScroll"],h="onwheel"in document||document.documentMode>=9?["wheel"]:["mousewheel","DomMouseScroll","MozMousePixelScroll"],i=Array.prototype.slice;if(a.event.fixHooks)for(var j=g.length;j;)a.event.fixHooks[g[--j]]=a.event.mouseHooks;var k=a.event.special.mousewheel={version:"3.1.12",setup:function(){if(this.addEventListener)for(var c=h.length;c;)this.addEventListener(h[--c],b,!1);else this.onmousewheel=b;a.data(this,"mousewheel-line-height",k.getLineHeight(this)),a.data(this,"mousewheel-page-height",k.getPageHeight(this))},teardown:function(){if(this.removeEventListener)for(var c=h.length;c;)this.removeEventListener(h[--c],b,!1);else this.onmousewheel=null;a.removeData(this,"mousewheel-line-height"),a.removeData(this,"mousewheel-page-height")},getLineHeight:function(b){var c=a(b),d=c["offsetParent"in a.fn?"offsetParent":"parent"]();return d.length||(d=a("body")),parseInt(d.css("fontSize"),10)||parseInt(c.css("fontSize"),10)||16},getPageHeight:function(b){return a(b).height()},settings:{adjustOldDeltas:!0,normalizeOffset:!0}};a.fn.extend({mousewheel:function(a){return a?this.bind("mousewheel",a):this.trigger("mousewheel")},unmousewheel:function(a){return this.unbind("mousewheel",a)}})});
/* == malihu jquery custom scrollbar plugin == Version: 3.0.8, License: MIT License (MIT) */
!function(e){"undefined"!=typeof module&&module.exports?module.exports=e:e(jQuery,window,document)}(function(e){!function(t){var o="function"==typeof define&&define.amd,a="undefined"!=typeof module&&module.exports,n="https:"==document.location.protocol?"https:":"http:",i="cdnjs.cloudflare.com/ajax/libs/jquery-mousewheel/3.1.12/jquery.mousewheel.min.js";o||(a?require("jquery-mousewheel")(e):e.event.special.mousewheel||e("head").append(decodeURI("%3Cscript src="+n+"//"+i+"%3E%3C/script%3E"))),t()}(function(){var t,o="mCustomScrollbar",a="mCS",n=".mCustomScrollbar",i={setTop:0,setLeft:0,axis:"y",scrollbarPosition:"inside",scrollInertia:950,autoDraggerLength:!0,alwaysShowScrollbar:0,snapOffset:0,mouseWheel:{enable:!0,scrollAmount:"auto",axis:"y",deltaFactor:"auto",disableOver:["select","option","keygen","datalist","textarea"]},scrollButtons:{scrollType:"stepless",scrollAmount:"auto"},keyboard:{enable:!0,scrollType:"stepless",scrollAmount:"auto"},contentTouchScroll:25,advanced:{autoScrollOnFocus:"input,textarea,select,button,datalist,keygen,a[tabindex],area,object,[contenteditable='true']",updateOnContentResize:!0,updateOnImageLoad:!0},theme:"light",callbacks:{onTotalScrollOffset:0,onTotalScrollBackOffset:0,alwaysTriggerOffsets:!0}},r=0,l={},s=window.attachEvent&&!window.addEventListener?1:0,c=!1,d=["mCSB_dragger_onDrag","mCSB_scrollTools_onDrag","mCS_img_loaded","mCS_disabled","mCS_destroyed","mCS_no_scrollbar","mCS-autoHide","mCS-dir-rtl","mCS_no_scrollbar_y","mCS_no_scrollbar_x","mCS_y_hidden","mCS_x_hidden","mCSB_draggerContainer","mCSB_buttonUp","mCSB_buttonDown","mCSB_buttonLeft","mCSB_buttonRight"],u={init:function(t){var t=e.extend(!0,{},i,t),o=f.call(this);if(t.live){var s=t.liveSelector||this.selector||n,c=e(s);if("off"===t.live)return void m(s);l[s]=setTimeout(function(){c.mCustomScrollbar(t),"once"===t.live&&c.length&&m(s)},500)}else m(s);return t.setWidth=t.set_width?t.set_width:t.setWidth,t.setHeight=t.set_height?t.set_height:t.setHeight,t.axis=t.horizontalScroll?"x":p(t.axis),t.scrollInertia=t.scrollInertia>0&&t.scrollInertia<17?17:t.scrollInertia,"object"!=typeof t.mouseWheel&&1==t.mouseWheel&&(t.mouseWheel={enable:!0,scrollAmount:"auto",axis:"y",preventDefault:!1,deltaFactor:"auto",normalizeDelta:!1,invert:!1}),t.mouseWheel.scrollAmount=t.mouseWheelPixels?t.mouseWheelPixels:t.mouseWheel.scrollAmount,t.mouseWheel.normalizeDelta=t.advanced.normalizeMouseWheelDelta?t.advanced.normalizeMouseWheelDelta:t.mouseWheel.normalizeDelta,t.scrollButtons.scrollType=g(t.scrollButtons.scrollType),h(t),e(o).each(function(){var o=e(this);if(!o.data(a)){o.data(a,{idx:++r,opt:t,scrollRatio:{y:null,x:null},overflowed:null,contentReset:{y:null,x:null},bindEvents:!1,tweenRunning:!1,sequential:{},langDir:o.css("direction"),cbOffsets:null,trigger:null});var n=o.data(a),i=n.opt,l=o.data("mcs-axis"),s=o.data("mcs-scrollbar-position"),c=o.data("mcs-theme");l&&(i.axis=l),s&&(i.scrollbarPosition=s),c&&(i.theme=c,h(i)),v.call(this),e("#mCSB_"+n.idx+"_container img:not(."+d[2]+")").addClass(d[2]),u.update.call(null,o)}})},update:function(t,o){var n=t||f.call(this);return e(n).each(function(){var t=e(this);if(t.data(a)){var n=t.data(a),i=n.opt,r=e("#mCSB_"+n.idx+"_container"),l=[e("#mCSB_"+n.idx+"_dragger_vertical"),e("#mCSB_"+n.idx+"_dragger_horizontal")];if(!r.length)return;n.tweenRunning&&V(t),t.hasClass(d[3])&&t.removeClass(d[3]),t.hasClass(d[4])&&t.removeClass(d[4]),S.call(this),_.call(this),"y"===i.axis||i.advanced.autoExpandHorizontalScroll||r.css("width",x(r.children())),n.overflowed=B.call(this),O.call(this),i.autoDraggerLength&&b.call(this),C.call(this),k.call(this);var s=[Math.abs(r[0].offsetTop),Math.abs(r[0].offsetLeft)];"x"!==i.axis&&(n.overflowed[0]?l[0].height()>l[0].parent().height()?T.call(this):(Q(t,s[0].toString(),{dir:"y",dur:0,overwrite:"none"}),n.contentReset.y=null):(T.call(this),"y"===i.axis?M.call(this):"yx"===i.axis&&n.overflowed[1]&&Q(t,s[1].toString(),{dir:"x",dur:0,overwrite:"none"}))),"y"!==i.axis&&(n.overflowed[1]?l[1].width()>l[1].parent().width()?T.call(this):(Q(t,s[1].toString(),{dir:"x",dur:0,overwrite:"none"}),n.contentReset.x=null):(T.call(this),"x"===i.axis?M.call(this):"yx"===i.axis&&n.overflowed[0]&&Q(t,s[0].toString(),{dir:"y",dur:0,overwrite:"none"}))),o&&n&&(2===o&&i.callbacks.onImageLoad&&"function"==typeof i.callbacks.onImageLoad?i.callbacks.onImageLoad.call(this):3===o&&i.callbacks.onSelectorChange&&"function"==typeof i.callbacks.onSelectorChange?i.callbacks.onSelectorChange.call(this):i.callbacks.onUpdate&&"function"==typeof i.callbacks.onUpdate&&i.callbacks.onUpdate.call(this)),X.call(this)}})},scrollTo:function(t,o){if("undefined"!=typeof t&&null!=t){var n=f.call(this);return e(n).each(function(){var n=e(this);if(n.data(a)){var i=n.data(a),r=i.opt,l={trigger:"external",scrollInertia:r.scrollInertia,scrollEasing:"mcsEaseInOut",moveDragger:!1,timeout:60,callbacks:!0,onStart:!0,onUpdate:!0,onComplete:!0},s=e.extend(!0,{},l,o),c=Y.call(this,t),d=s.scrollInertia>0&&s.scrollInertia<17?17:s.scrollInertia;c[0]=j.call(this,c[0],"y"),c[1]=j.call(this,c[1],"x"),s.moveDragger&&(c[0]*=i.scrollRatio.y,c[1]*=i.scrollRatio.x),s.dur=d,setTimeout(function(){null!==c[0]&&"undefined"!=typeof c[0]&&"x"!==r.axis&&i.overflowed[0]&&(s.dir="y",s.overwrite="all",Q(n,c[0].toString(),s)),null!==c[1]&&"undefined"!=typeof c[1]&&"y"!==r.axis&&i.overflowed[1]&&(s.dir="x",s.overwrite="none",Q(n,c[1].toString(),s))},s.timeout)}})}},stop:function(){var t=f.call(this);return e(t).each(function(){var t=e(this);t.data(a)&&V(t)})},disable:function(t){var o=f.call(this);return e(o).each(function(){var o=e(this);if(o.data(a)){{o.data(a)}X.call(this,"remove"),M.call(this),t&&T.call(this),O.call(this,!0),o.addClass(d[3])}})},destroy:function(){var t=f.call(this);return e(t).each(function(){var n=e(this);if(n.data(a)){var i=n.data(a),r=i.opt,l=e("#mCSB_"+i.idx),s=e("#mCSB_"+i.idx+"_container"),c=e(".mCSB_"+i.idx+"_scrollbar");r.live&&m(r.liveSelector||e(t).selector),X.call(this,"remove"),M.call(this),T.call(this),n.removeData(a),Z(this,"mcs"),c.remove(),s.find("img."+d[2]).removeClass(d[2]),l.replaceWith(s.contents()),n.removeClass(o+" _"+a+"_"+i.idx+" "+d[6]+" "+d[7]+" "+d[5]+" "+d[3]).addClass(d[4])}})}},f=function(){return"object"!=typeof e(this)||e(this).length<1?n:this},h=function(t){var o=["rounded","rounded-dark","rounded-dots","rounded-dots-dark"],a=["rounded-dots","rounded-dots-dark","3d","3d-dark","3d-thick","3d-thick-dark","inset","inset-dark","inset-2","inset-2-dark","inset-3","inset-3-dark"],n=["minimal","minimal-dark"],i=["minimal","minimal-dark"],r=["minimal","minimal-dark"];t.autoDraggerLength=e.inArray(t.theme,o)>-1?!1:t.autoDraggerLength,t.autoExpandScrollbar=e.inArray(t.theme,a)>-1?!1:t.autoExpandScrollbar,t.scrollButtons.enable=e.inArray(t.theme,n)>-1?!1:t.scrollButtons.enable,t.autoHideScrollbar=e.inArray(t.theme,i)>-1?!0:t.autoHideScrollbar,t.scrollbarPosition=e.inArray(t.theme,r)>-1?"outside":t.scrollbarPosition},m=function(e){l[e]&&(clearTimeout(l[e]),Z(l,e))},p=function(e){return"yx"===e||"xy"===e||"auto"===e?"yx":"x"===e||"horizontal"===e?"x":"y"},g=function(e){return"stepped"===e||"pixels"===e||"step"===e||"click"===e?"stepped":"stepless"},v=function(){var t=e(this),n=t.data(a),i=n.opt,r=i.autoExpandScrollbar?" "+d[1]+"_expand":"",l=["<div id='mCSB_"+n.idx+"_scrollbar_vertical' class='mCSB_scrollTools mCSB_"+n.idx+"_scrollbar mCS-"+i.theme+" mCSB_scrollTools_vertical"+r+"'><div class='"+d[12]+"'><div id='mCSB_"+n.idx+"_dragger_vertical' class='mCSB_dragger' style='position:absolute;' oncontextmenu='return false;'><div class='mCSB_dragger_bar' /></div><div class='mCSB_draggerRail' /></div></div>","<div id='mCSB_"+n.idx+"_scrollbar_horizontal' class='mCSB_scrollTools mCSB_"+n.idx+"_scrollbar mCS-"+i.theme+" mCSB_scrollTools_horizontal"+r+"'><div class='"+d[12]+"'><div id='mCSB_"+n.idx+"_dragger_horizontal' class='mCSB_dragger' style='position:absolute;' oncontextmenu='return false;'><div class='mCSB_dragger_bar' /></div><div class='mCSB_draggerRail' /></div></div>"],s="yx"===i.axis?"mCSB_vertical_horizontal":"x"===i.axis?"mCSB_horizontal":"mCSB_vertical",c="yx"===i.axis?l[0]+l[1]:"x"===i.axis?l[1]:l[0],u="yx"===i.axis?"<div id='mCSB_"+n.idx+"_container_wrapper' class='mCSB_container_wrapper' />":"",f=i.autoHideScrollbar?" "+d[6]:"",h="x"!==i.axis&&"rtl"===n.langDir?" "+d[7]:"";i.setWidth&&t.css("width",i.setWidth),i.setHeight&&t.css("height",i.setHeight),i.setLeft="y"!==i.axis&&"rtl"===n.langDir?"989999px":i.setLeft,t.addClass(o+" _"+a+"_"+n.idx+f+h).wrapInner("<div id='mCSB_"+n.idx+"' class='mCustomScrollBox mCS-"+i.theme+" "+s+"'><div id='mCSB_"+n.idx+"_container' class='mCSB_container' style='position:relative; top:"+i.setTop+"; left:"+i.setLeft+";' dir="+n.langDir+" /></div>");var m=e("#mCSB_"+n.idx),p=e("#mCSB_"+n.idx+"_container");"y"===i.axis||i.advanced.autoExpandHorizontalScroll||p.css("width",x(p.children())),"outside"===i.scrollbarPosition?("static"===t.css("position")&&t.css("position","relative"),t.css("overflow","visible"),m.addClass("mCSB_outside").after(c)):(m.addClass("mCSB_inside").append(c),p.wrap(u)),w.call(this);var g=[e("#mCSB_"+n.idx+"_dragger_vertical"),e("#mCSB_"+n.idx+"_dragger_horizontal")];g[0].css("min-height",g[0].height()),g[1].css("min-width",g[1].width())},x=function(t){return Math.max.apply(Math,t.map(function(){return e(this).outerWidth(!0)}).get())},_=function(){var t=e(this),o=t.data(a),n=o.opt,i=e("#mCSB_"+o.idx+"_container");n.advanced.autoExpandHorizontalScroll&&"y"!==n.axis&&i.css({position:"absolute",width:"auto"}).wrap("<div class='mCSB_h_wrapper' style='position:relative; left:0; width:999999px;' />").css({width:Math.ceil(i[0].getBoundingClientRect().right+.4)-Math.floor(i[0].getBoundingClientRect().left),position:"relative"}).unwrap()},w=function(){var t=e(this),o=t.data(a),n=o.opt,i=e(".mCSB_"+o.idx+"_scrollbar:first"),r=tt(n.scrollButtons.tabindex)?"tabindex='"+n.scrollButtons.tabindex+"'":"",l=["<a href='#' class='"+d[13]+"' oncontextmenu='return false;' "+r+" />","<a href='#' class='"+d[14]+"' oncontextmenu='return false;' "+r+" />","<a href='#' class='"+d[15]+"' oncontextmenu='return false;' "+r+" />","<a href='#' class='"+d[16]+"' oncontextmenu='return false;' "+r+" />"],s=["x"===n.axis?l[2]:l[0],"x"===n.axis?l[3]:l[1],l[2],l[3]];n.scrollButtons.enable&&i.prepend(s[0]).append(s[1]).next(".mCSB_scrollTools").prepend(s[2]).append(s[3])},S=function(){var t=e(this),o=t.data(a),n=e("#mCSB_"+o.idx),i=t.css("max-height")||"none",r=-1!==i.indexOf("%"),l=t.css("box-sizing");if("none"!==i){var s=r?t.parent().height()*parseInt(i)/100:parseInt(i);"border-box"===l&&(s-=t.innerHeight()-t.height()+(t.outerHeight()-t.innerHeight())),n.css("max-height",Math.round(s))}},b=function(){var t=e(this),o=t.data(a),n=e("#mCSB_"+o.idx),i=e("#mCSB_"+o.idx+"_container"),r=[e("#mCSB_"+o.idx+"_dragger_vertical"),e("#mCSB_"+o.idx+"_dragger_horizontal")],l=[n.height()/i.outerHeight(!1),n.width()/i.outerWidth(!1)],c=[parseInt(r[0].css("min-height")),Math.round(l[0]*r[0].parent().height()),parseInt(r[1].css("min-width")),Math.round(l[1]*r[1].parent().width())],d=s&&c[1]<c[0]?c[0]:c[1],u=s&&c[3]<c[2]?c[2]:c[3];r[0].css({height:d,"max-height":r[0].parent().height()-10}).find(".mCSB_dragger_bar").css({"line-height":c[0]+"px"}),r[1].css({width:u,"max-width":r[1].parent().width()-10})},C=function(){var t=e(this),o=t.data(a),n=e("#mCSB_"+o.idx),i=e("#mCSB_"+o.idx+"_container"),r=[e("#mCSB_"+o.idx+"_dragger_vertical"),e("#mCSB_"+o.idx+"_dragger_horizontal")],l=[i.outerHeight(!1)-n.height(),i.outerWidth(!1)-n.width()],s=[l[0]/(r[0].parent().height()-r[0].height()),l[1]/(r[1].parent().width()-r[1].width())];o.scrollRatio={y:s[0],x:s[1]}},y=function(e,t,o){var a=o?d[0]+"_expanded":"",n=e.closest(".mCSB_scrollTools");"active"===t?(e.toggleClass(d[0]+" "+a),n.toggleClass(d[1]),e[0]._draggable=e[0]._draggable?0:1):e[0]._draggable||("hide"===t?(e.removeClass(d[0]),n.removeClass(d[1])):(e.addClass(d[0]),n.addClass(d[1])))},B=function(){var t=e(this),o=t.data(a),n=e("#mCSB_"+o.idx),i=e("#mCSB_"+o.idx+"_container"),r=null==o.overflowed?i.height():i.outerHeight(!1),l=null==o.overflowed?i.width():i.outerWidth(!1);return[r>n.height(),l>n.width()]},T=function(){var t=e(this),o=t.data(a),n=o.opt,i=e("#mCSB_"+o.idx),r=e("#mCSB_"+o.idx+"_container"),l=[e("#mCSB_"+o.idx+"_dragger_vertical"),e("#mCSB_"+o.idx+"_dragger_horizontal")];if(V(t),("x"!==n.axis&&!o.overflowed[0]||"y"===n.axis&&o.overflowed[0])&&(l[0].add(r).css("top",0),Q(t,"_resetY")),"y"!==n.axis&&!o.overflowed[1]||"x"===n.axis&&o.overflowed[1]){var s=dx=0;"rtl"===o.langDir&&(s=i.width()-r.outerWidth(!1),dx=Math.abs(s/o.scrollRatio.x)),r.css("left",s),l[1].css("left",dx),Q(t,"_resetX")}},k=function(){function t(){r=setTimeout(function(){e.event.special.mousewheel?(clearTimeout(r),W.call(o[0])):t()},100)}var o=e(this),n=o.data(a),i=n.opt;if(!n.bindEvents){if(R.call(this),i.contentTouchScroll&&E.call(this),D.call(this),i.mouseWheel.enable){var r;t()}P.call(this),H.call(this),i.advanced.autoScrollOnFocus&&z.call(this),i.scrollButtons.enable&&U.call(this),i.keyboard.enable&&q.call(this),n.bindEvents=!0}},M=function(){var t=e(this),o=t.data(a),n=o.opt,i=a+"_"+o.idx,r=".mCSB_"+o.idx+"_scrollbar",l=e("#mCSB_"+o.idx+",#mCSB_"+o.idx+"_container,#mCSB_"+o.idx+"_container_wrapper,"+r+" ."+d[12]+",#mCSB_"+o.idx+"_dragger_vertical,#mCSB_"+o.idx+"_dragger_horizontal,"+r+">a"),s=e("#mCSB_"+o.idx+"_container");n.advanced.releaseDraggableSelectors&&l.add(e(n.advanced.releaseDraggableSelectors)),o.bindEvents&&(e(document).unbind("."+i),l.each(function(){e(this).unbind("."+i)}),clearTimeout(t[0]._focusTimeout),Z(t[0],"_focusTimeout"),clearTimeout(o.sequential.step),Z(o.sequential,"step"),clearTimeout(s[0].onCompleteTimeout),Z(s[0],"onCompleteTimeout"),o.bindEvents=!1)},O=function(t){var o=e(this),n=o.data(a),i=n.opt,r=e("#mCSB_"+n.idx+"_container_wrapper"),l=r.length?r:e("#mCSB_"+n.idx+"_container"),s=[e("#mCSB_"+n.idx+"_scrollbar_vertical"),e("#mCSB_"+n.idx+"_scrollbar_horizontal")],c=[s[0].find(".mCSB_dragger"),s[1].find(".mCSB_dragger")];"x"!==i.axis&&(n.overflowed[0]&&!t?(s[0].add(c[0]).add(s[0].children("a")).css("display","block"),l.removeClass(d[8]+" "+d[10])):(i.alwaysShowScrollbar?(2!==i.alwaysShowScrollbar&&c[0].css("display","none"),l.removeClass(d[10])):(s[0].css("display","none"),l.addClass(d[10])),l.addClass(d[8]))),"y"!==i.axis&&(n.overflowed[1]&&!t?(s[1].add(c[1]).add(s[1].children("a")).css("display","block"),l.removeClass(d[9]+" "+d[11])):(i.alwaysShowScrollbar?(2!==i.alwaysShowScrollbar&&c[1].css("display","none"),l.removeClass(d[11])):(s[1].css("display","none"),l.addClass(d[11])),l.addClass(d[9]))),n.overflowed[0]||n.overflowed[1]?o.removeClass(d[5]):o.addClass(d[5])},I=function(e){var t=e.type;switch(t){case"pointerdown":case"MSPointerDown":case"pointermove":case"MSPointerMove":case"pointerup":case"MSPointerUp":return e.target.ownerDocument!==document?[e.originalEvent.screenY,e.originalEvent.screenX,!1]:[e.originalEvent.pageY,e.originalEvent.pageX,!1];case"touchstart":case"touchmove":case"touchend":var o=e.originalEvent.touches[0]||e.originalEvent.changedTouches[0],a=e.originalEvent.touches.length||e.originalEvent.changedTouches.length;return e.target.ownerDocument!==document?[o.screenY,o.screenX,a>1]:[o.pageY,o.pageX,a>1];default:return[e.pageY,e.pageX,!1]}},R=function(){function t(e){var t=m.find("iframe");if(t.length){var o=e?"auto":"none";t.css("pointer-events",o)}}function o(e,t,o,a){if(m[0].idleTimer=u.scrollInertia<233?250:0,n.attr("id")===h[1])var i="x",r=(n[0].offsetLeft-t+a)*d.scrollRatio.x;else var i="y",r=(n[0].offsetTop-e+o)*d.scrollRatio.y;Q(l,r.toString(),{dir:i,drag:!0})}var n,i,r,l=e(this),d=l.data(a),u=d.opt,f=a+"_"+d.idx,h=["mCSB_"+d.idx+"_dragger_vertical","mCSB_"+d.idx+"_dragger_horizontal"],m=e("#mCSB_"+d.idx+"_container"),p=e("#"+h[0]+",#"+h[1]),g=u.advanced.releaseDraggableSelectors?p.add(e(u.advanced.releaseDraggableSelectors)):p;p.bind("mousedown."+f+" touchstart."+f+" pointerdown."+f+" MSPointerDown."+f,function(o){if(o.stopImmediatePropagation(),o.preventDefault(),$(o)){c=!0,s&&(document.onselectstart=function(){return!1}),t(!1),V(l),n=e(this);var a=n.offset(),d=I(o)[0]-a.top,f=I(o)[1]-a.left,h=n.height()+a.top,m=n.width()+a.left;h>d&&d>0&&m>f&&f>0&&(i=d,r=f),y(n,"active",u.autoExpandScrollbar)}}).bind("touchmove."+f,function(e){e.stopImmediatePropagation(),e.preventDefault();var t=n.offset(),a=I(e)[0]-t.top,l=I(e)[1]-t.left;o(i,r,a,l)}),e(document).bind("mousemove."+f+" pointermove."+f+" MSPointerMove."+f,function(e){if(n){var t=n.offset(),a=I(e)[0]-t.top,l=I(e)[1]-t.left;if(i===a)return;o(i,r,a,l)}}).add(g).bind("mouseup."+f+" touchend."+f+" pointerup."+f+" MSPointerUp."+f,function(){n&&(y(n,"active",u.autoExpandScrollbar),n=null),c=!1,s&&(document.onselectstart=null),t(!0)})},E=function(){function o(e){if(!et(e)||c||I(e)[2])return void(t=0);t=1,S=0,b=0;var o=M.offset();d=I(e)[0]-o.top,u=I(e)[1]-o.left,A=[I(e)[0],I(e)[1]]}function n(e){if(et(e)&&!c&&!I(e)[2]&&(e.stopImmediatePropagation(),!b||S)){p=J();var t=k.offset(),o=I(e)[0]-t.top,a=I(e)[1]-t.left,n="mcsLinearOut";if(R.push(o),E.push(a),A[2]=Math.abs(I(e)[0]-A[0]),A[3]=Math.abs(I(e)[1]-A[1]),y.overflowed[0])var i=O[0].parent().height()-O[0].height(),r=d-o>0&&o-d>-(i*y.scrollRatio.y)&&(2*A[3]<A[2]||"yx"===B.axis);if(y.overflowed[1])var l=O[1].parent().width()-O[1].width(),f=u-a>0&&a-u>-(l*y.scrollRatio.x)&&(2*A[2]<A[3]||"yx"===B.axis);r||f?(e.preventDefault(),S=1):b=1,_="yx"===B.axis?[d-o,u-a]:"x"===B.axis?[null,u-a]:[d-o,null],M[0].idleTimer=250,y.overflowed[0]&&s(_[0],D,n,"y","all",!0),y.overflowed[1]&&s(_[1],D,n,"x",W,!0)}}function i(e){if(!et(e)||c||I(e)[2])return void(t=0);t=1,e.stopImmediatePropagation(),V(C),m=J();var o=k.offset();f=I(e)[0]-o.top,h=I(e)[1]-o.left,R=[],E=[]}function r(e){if(et(e)&&!c&&!I(e)[2]){e.stopImmediatePropagation(),S=0,b=0,g=J();var t=k.offset(),o=I(e)[0]-t.top,a=I(e)[1]-t.left;if(!(g-p>30)){x=1e3/(g-m);var n="mcsEaseOut",i=2.5>x,r=i?[R[R.length-2],E[E.length-2]]:[0,0];v=i?[o-r[0],a-r[1]]:[o-f,a-h];var d=[Math.abs(v[0]),Math.abs(v[1])];x=i?[Math.abs(v[0]/4),Math.abs(v[1]/4)]:[x,x];var u=[Math.abs(M[0].offsetTop)-v[0]*l(d[0]/x[0],x[0]),Math.abs(M[0].offsetLeft)-v[1]*l(d[1]/x[1],x[1])];_="yx"===B.axis?[u[0],u[1]]:"x"===B.axis?[null,u[1]]:[u[0],null],w=[4*d[0]+B.scrollInertia,4*d[1]+B.scrollInertia];var C=parseInt(B.contentTouchScroll)||0;_[0]=d[0]>C?_[0]:0,_[1]=d[1]>C?_[1]:0,y.overflowed[0]&&s(_[0],w[0],n,"y",W,!1),y.overflowed[1]&&s(_[1],w[1],n,"x",W,!1)}}}function l(e,t){var o=[1.5*t,2*t,t/1.5,t/2];return e>90?t>4?o[0]:o[3]:e>60?t>3?o[3]:o[2]:e>30?t>8?o[1]:t>6?o[0]:t>4?t:o[2]:t>8?t:o[3]}function s(e,t,o,a,n,i){e&&Q(C,e.toString(),{dur:t,scrollEasing:o,dir:a,overwrite:n,drag:i})}var d,u,f,h,m,p,g,v,x,_,w,S,b,C=e(this),y=C.data(a),B=y.opt,T=a+"_"+y.idx,k=e("#mCSB_"+y.idx),M=e("#mCSB_"+y.idx+"_container"),O=[e("#mCSB_"+y.idx+"_dragger_vertical"),e("#mCSB_"+y.idx+"_dragger_horizontal")],R=[],E=[],D=0,W="yx"===B.axis?"none":"all",A=[],P=M.find("iframe"),z=["touchstart."+T+" pointerdown."+T+" MSPointerDown."+T,"touchmove."+T+" pointermove."+T+" MSPointerMove."+T,"touchend."+T+" pointerup."+T+" MSPointerUp."+T];M.bind(z[0],function(e){o(e)}).bind(z[1],function(e){n(e)}),k.bind(z[0],function(e){i(e)}).bind(z[2],function(e){r(e)}),P.length&&P.each(function(){e(this).load(function(){L(this)&&e(this.contentDocument||this.contentWindow.document).bind(z[0],function(e){o(e),i(e)}).bind(z[1],function(e){n(e)}).bind(z[2],function(e){r(e)})})})},D=function(){function o(){return window.getSelection?window.getSelection().toString():document.selection&&"Control"!=document.selection.type?document.selection.createRange().text:0}function n(e,t,o){d.type=o&&i?"stepped":"stepless",d.scrollAmount=10,F(r,e,t,"mcsLinearOut",o?60:null)}var i,r=e(this),l=r.data(a),s=l.opt,d=l.sequential,u=a+"_"+l.idx,f=e("#mCSB_"+l.idx+"_container"),h=f.parent();f.bind("mousedown."+u,function(){t||i||(i=1,c=!0)}).add(document).bind("mousemove."+u,function(e){if(!t&&i&&o()){var a=f.offset(),r=I(e)[0]-a.top+f[0].offsetTop,c=I(e)[1]-a.left+f[0].offsetLeft;r>0&&r<h.height()&&c>0&&c<h.width()?d.step&&n("off",null,"stepped"):("x"!==s.axis&&l.overflowed[0]&&(0>r?n("on",38):r>h.height()&&n("on",40)),"y"!==s.axis&&l.overflowed[1]&&(0>c?n("on",37):c>h.width()&&n("on",39)))}}).bind("mouseup."+u,function(){t||(i&&(i=0,n("off",null)),c=!1)})},W=function(){function t(t,a){if(V(o),!A(o,t.target)){var r="auto"!==i.mouseWheel.deltaFactor?parseInt(i.mouseWheel.deltaFactor):s&&t.deltaFactor<100?100:t.deltaFactor||100;if("x"===i.axis||"x"===i.mouseWheel.axis)var d="x",u=[Math.round(r*n.scrollRatio.x),parseInt(i.mouseWheel.scrollAmount)],f="auto"!==i.mouseWheel.scrollAmount?u[1]:u[0]>=l.width()?.9*l.width():u[0],h=Math.abs(e("#mCSB_"+n.idx+"_container")[0].offsetLeft),m=c[1][0].offsetLeft,p=c[1].parent().width()-c[1].width(),g=t.deltaX||t.deltaY||a;else var d="y",u=[Math.round(r*n.scrollRatio.y),parseInt(i.mouseWheel.scrollAmount)],f="auto"!==i.mouseWheel.scrollAmount?u[1]:u[0]>=l.height()?.9*l.height():u[0],h=Math.abs(e("#mCSB_"+n.idx+"_container")[0].offsetTop),m=c[0][0].offsetTop,p=c[0].parent().height()-c[0].height(),g=t.deltaY||a;"y"===d&&!n.overflowed[0]||"x"===d&&!n.overflowed[1]||(i.mouseWheel.invert&&(g=-g),i.mouseWheel.normalizeDelta&&(g=0>g?-1:1),(g>0&&0!==m||0>g&&m!==p||i.mouseWheel.preventDefault)&&(t.stopImmediatePropagation(),t.preventDefault()),Q(o,(h-g*f).toString(),{dir:d}))}}var o=e(this),n=o.data(a),i=n.opt,r=a+"_"+n.idx,l=e("#mCSB_"+n.idx),c=[e("#mCSB_"+n.idx+"_dragger_vertical"),e("#mCSB_"+n.idx+"_dragger_horizontal")],d=e("#mCSB_"+n.idx+"_container").find("iframe");n&&(d.length&&d.each(function(){e(this).load(function(){L(this)&&e(this.contentDocument||this.contentWindow.document).bind("mousewheel."+r,function(e,o){t(e,o)})})}),l.bind("mousewheel."+r,function(e,o){t(e,o)}))},L=function(e){var t=null;try{var o=e.contentDocument||e.contentWindow.document;t=o.body.innerHTML}catch(a){}return null!==t},A=function(t,o){var n=o.nodeName.toLowerCase(),i=t.data(a).opt.mouseWheel.disableOver,r=["select","textarea"];return e.inArray(n,i)>-1&&!(e.inArray(n,r)>-1&&!e(o).is(":focus"))},P=function(){var t=e(this),o=t.data(a),n=a+"_"+o.idx,i=e("#mCSB_"+o.idx+"_container"),r=i.parent(),l=e(".mCSB_"+o.idx+"_scrollbar ."+d[12]);l.bind("touchstart."+n+" pointerdown."+n+" MSPointerDown."+n,function(){c=!0}).bind("touchend."+n+" pointerup."+n+" MSPointerUp."+n,function(){c=!1}).bind("click."+n,function(a){if(e(a.target).hasClass(d[12])||e(a.target).hasClass("mCSB_draggerRail")){V(t);var n=e(this),l=n.find(".mCSB_dragger");if(n.parent(".mCSB_scrollTools_horizontal").length>0){if(!o.overflowed[1])return;var s="x",c=a.pageX>l.offset().left?-1:1,u=Math.abs(i[0].offsetLeft)-.9*c*r.width()}else{if(!o.overflowed[0])return;var s="y",c=a.pageY>l.offset().top?-1:1,u=Math.abs(i[0].offsetTop)-.9*c*r.height()}Q(t,u.toString(),{dir:s,scrollEasing:"mcsEaseInOut"})}})},z=function(){var t=e(this),o=t.data(a),n=o.opt,i=a+"_"+o.idx,r=e("#mCSB_"+o.idx+"_container"),l=r.parent();r.bind("focusin."+i,function(){var o=e(document.activeElement),a=r.find(".mCustomScrollBox").length,i=0;o.is(n.advanced.autoScrollOnFocus)&&(V(t),clearTimeout(t[0]._focusTimeout),t[0]._focusTimer=a?(i+17)*a:0,t[0]._focusTimeout=setTimeout(function(){var e=[ot(o)[0],ot(o)[1]],a=[r[0].offsetTop,r[0].offsetLeft],s=[a[0]+e[0]>=0&&a[0]+e[0]<l.height()-o.outerHeight(!1),a[1]+e[1]>=0&&a[0]+e[1]<l.width()-o.outerWidth(!1)],c="yx"!==n.axis||s[0]||s[1]?"all":"none";"x"===n.axis||s[0]||Q(t,e[0].toString(),{dir:"y",scrollEasing:"mcsEaseInOut",overwrite:c,dur:i}),"y"===n.axis||s[1]||Q(t,e[1].toString(),{dir:"x",scrollEasing:"mcsEaseInOut",overwrite:c,dur:i})},t[0]._focusTimer))})},H=function(){var t=e(this),o=t.data(a),n=a+"_"+o.idx,i=e("#mCSB_"+o.idx+"_container").parent();i.bind("scroll."+n,function(){(0!==i.scrollTop()||0!==i.scrollLeft())&&e(".mCSB_"+o.idx+"_scrollbar").css("visibility","hidden")})},U=function(){var t=e(this),o=t.data(a),n=o.opt,i=o.sequential,r=a+"_"+o.idx,l=".mCSB_"+o.idx+"_scrollbar",s=e(l+">a");s.bind("mousedown."+r+" touchstart."+r+" pointerdown."+r+" MSPointerDown."+r+" mouseup."+r+" touchend."+r+" pointerup."+r+" MSPointerUp."+r+" mouseout."+r+" pointerout."+r+" MSPointerOut."+r+" click."+r,function(a){function r(e,o){i.scrollAmount=n.snapAmount||n.scrollButtons.scrollAmount,F(t,e,o)}if(a.preventDefault(),$(a)){var l=e(this).attr("class");switch(i.type=n.scrollButtons.scrollType,a.type){case"mousedown":case"touchstart":case"pointerdown":case"MSPointerDown":if("stepped"===i.type)return;c=!0,o.tweenRunning=!1,r("on",l);break;case"mouseup":case"touchend":case"pointerup":case"MSPointerUp":case"mouseout":case"pointerout":case"MSPointerOut":if("stepped"===i.type)return;c=!1,i.dir&&r("off",l);break;case"click":if("stepped"!==i.type||o.tweenRunning)return;r("on",l)}}})},q=function(){function t(t){function a(e,t){r.type=i.keyboard.scrollType,r.scrollAmount=i.snapAmount||i.keyboard.scrollAmount,"stepped"===r.type&&n.tweenRunning||F(o,e,t)}switch(t.type){case"blur":n.tweenRunning&&r.dir&&a("off",null);break;case"keydown":case"keyup":var l=t.keyCode?t.keyCode:t.which,s="on";if("x"!==i.axis&&(38===l||40===l)||"y"!==i.axis&&(37===l||39===l)){if((38===l||40===l)&&!n.overflowed[0]||(37===l||39===l)&&!n.overflowed[1])return;"keyup"===t.type&&(s="off"),e(document.activeElement).is(u)||(t.preventDefault(),t.stopImmediatePropagation(),a(s,l))}else if(33===l||34===l){if((n.overflowed[0]||n.overflowed[1])&&(t.preventDefault(),t.stopImmediatePropagation()),"keyup"===t.type){V(o);var f=34===l?-1:1;if("x"===i.axis||"yx"===i.axis&&n.overflowed[1]&&!n.overflowed[0])var h="x",m=Math.abs(c[0].offsetLeft)-.9*f*d.width();else var h="y",m=Math.abs(c[0].offsetTop)-.9*f*d.height();Q(o,m.toString(),{dir:h,scrollEasing:"mcsEaseInOut"})}}else if((35===l||36===l)&&!e(document.activeElement).is(u)&&((n.overflowed[0]||n.overflowed[1])&&(t.preventDefault(),t.stopImmediatePropagation()),"keyup"===t.type)){if("x"===i.axis||"yx"===i.axis&&n.overflowed[1]&&!n.overflowed[0])var h="x",m=35===l?Math.abs(d.width()-c.outerWidth(!1)):0;else var h="y",m=35===l?Math.abs(d.height()-c.outerHeight(!1)):0;Q(o,m.toString(),{dir:h,scrollEasing:"mcsEaseInOut"})}}}var o=e(this),n=o.data(a),i=n.opt,r=n.sequential,l=a+"_"+n.idx,s=e("#mCSB_"+n.idx),c=e("#mCSB_"+n.idx+"_container"),d=c.parent(),u="input,textarea,select,datalist,keygen,[contenteditable='true']",f=c.find("iframe"),h=["blur."+l+" keydown."+l+" keyup."+l];f.length&&f.each(function(){e(this).load(function(){L(this)&&e(this.contentDocument||this.contentWindow.document).bind(h[0],function(e){t(e)})})}),s.attr("tabindex","0").bind(h[0],function(e){t(e)})},F=function(t,o,n,i,r){function l(e){var o="stepped"!==f.type,a=r?r:e?o?p/1.5:g:1e3/60,n=e?o?7.5:40:2.5,s=[Math.abs(h[0].offsetTop),Math.abs(h[0].offsetLeft)],d=[c.scrollRatio.y>10?10:c.scrollRatio.y,c.scrollRatio.x>10?10:c.scrollRatio.x],u="x"===f.dir[0]?s[1]+f.dir[1]*d[1]*n:s[0]+f.dir[1]*d[0]*n,m="x"===f.dir[0]?s[1]+f.dir[1]*parseInt(f.scrollAmount):s[0]+f.dir[1]*parseInt(f.scrollAmount),v="auto"!==f.scrollAmount?m:u,x=i?i:e?o?"mcsLinearOut":"mcsEaseInOut":"mcsLinear",_=e?!0:!1;return e&&17>a&&(v="x"===f.dir[0]?s[1]:s[0]),Q(t,v.toString(),{dir:f.dir[0],scrollEasing:x,dur:a,onComplete:_}),e?void(f.dir=!1):(clearTimeout(f.step),void(f.step=setTimeout(function(){l()},a)))}function s(){clearTimeout(f.step),Z(f,"step"),V(t)}var c=t.data(a),u=c.opt,f=c.sequential,h=e("#mCSB_"+c.idx+"_container"),m="stepped"===f.type?!0:!1,p=u.scrollInertia<26?26:u.scrollInertia,g=u.scrollInertia<1?17:u.scrollInertia;switch(o){case"on":if(f.dir=[n===d[16]||n===d[15]||39===n||37===n?"x":"y",n===d[13]||n===d[15]||38===n||37===n?-1:1],V(t),tt(n)&&"stepped"===f.type)return;l(m);break;case"off":s(),(m||c.tweenRunning&&f.dir)&&l(!0)}},Y=function(t){var o=e(this).data(a).opt,n=[];return"function"==typeof t&&(t=t()),t instanceof Array?n=t.length>1?[t[0],t[1]]:"x"===o.axis?[null,t[0]]:[t[0],null]:(n[0]=t.y?t.y:t.x||"x"===o.axis?null:t,n[1]=t.x?t.x:t.y||"y"===o.axis?null:t),"function"==typeof n[0]&&(n[0]=n[0]()),"function"==typeof n[1]&&(n[1]=n[1]()),n},j=function(t,o){if(null!=t&&"undefined"!=typeof t){var n=e(this),i=n.data(a),r=i.opt,l=e("#mCSB_"+i.idx+"_container"),s=l.parent(),c=typeof t;o||(o="x"===r.axis?"x":"y");var d="x"===o?l.outerWidth(!1):l.outerHeight(!1),f="x"===o?l[0].offsetLeft:l[0].offsetTop,h="x"===o?"left":"top";switch(c){case"function":return t();case"object":var m=t.jquery?t:e(t);if(!m.length)return;return"x"===o?ot(m)[1]:ot(m)[0];case"string":case"number":if(tt(t))return Math.abs(t);if(-1!==t.indexOf("%"))return Math.abs(d*parseInt(t)/100);if(-1!==t.indexOf("-="))return Math.abs(f-parseInt(t.split("-=")[1]));if(-1!==t.indexOf("+=")){var p=f+parseInt(t.split("+=")[1]);return p>=0?0:Math.abs(p)}if(-1!==t.indexOf("px")&&tt(t.split("px")[0]))return Math.abs(t.split("px")[0]);if("top"===t||"left"===t)return 0;if("bottom"===t)return Math.abs(s.height()-l.outerHeight(!1));if("right"===t)return Math.abs(s.width()-l.outerWidth(!1));if("first"===t||"last"===t){var m=l.find(":"+t);return"x"===o?ot(m)[1]:ot(m)[0]}return e(t).length?"x"===o?ot(e(t))[1]:ot(e(t))[0]:(l.css(h,t),void u.update.call(null,n[0]))}}},X=function(t){function o(){clearTimeout(h[0].autoUpdate),h[0].autoUpdate=setTimeout(function(){return f.advanced.updateOnSelectorChange&&(m=r(),m!==w)?(l(3),void(w=m)):(f.advanced.updateOnContentResize&&(p=[h.outerHeight(!1),h.outerWidth(!1),v.height(),v.width(),_()[0],_()[1]],(p[0]!==S[0]||p[1]!==S[1]||p[2]!==S[2]||p[3]!==S[3]||p[4]!==S[4]||p[5]!==S[5])&&(l(p[0]!==S[0]||p[1]!==S[1]),S=p)),f.advanced.updateOnImageLoad&&(g=n(),g!==b&&(h.find("img").each(function(){i(this)}),b=g)),void((f.advanced.updateOnSelectorChange||f.advanced.updateOnContentResize||f.advanced.updateOnImageLoad)&&o()))},60)}function n(){var e=0;return f.advanced.updateOnImageLoad&&(e=h.find("img").length),e}function i(t){function o(e,t){return function(){return t.apply(e,arguments)}}function a(){this.onload=null,e(t).addClass(d[2]),l(2)}if(e(t).hasClass(d[2]))return void l();var n=new Image;n.onload=o(n,a),n.src=t.src}function r(){f.advanced.updateOnSelectorChange===!0&&(f.advanced.updateOnSelectorChange="*");var t=0,o=h.find(f.advanced.updateOnSelectorChange);return f.advanced.updateOnSelectorChange&&o.length>0&&o.each(function(){t+=e(this).height()+e(this).width()}),t}function l(e){clearTimeout(h[0].autoUpdate),u.update.call(null,s[0],e)}var s=e(this),c=s.data(a),f=c.opt,h=e("#mCSB_"+c.idx+"_container");if(t)return clearTimeout(h[0].autoUpdate),void Z(h[0],"autoUpdate");var m,p,g,v=h.parent(),x=[e("#mCSB_"+c.idx+"_scrollbar_vertical"),e("#mCSB_"+c.idx+"_scrollbar_horizontal")],_=function(){return[x[0].is(":visible")?x[0].outerHeight(!0):0,x[1].is(":visible")?x[1].outerWidth(!0):0]},w=r(),S=[h.outerHeight(!1),h.outerWidth(!1),v.height(),v.width(),_()[0],_()[1]],b=n();o()},N=function(e,t,o){return Math.round(e/t)*t-o},V=function(t){var o=t.data(a),n=e("#mCSB_"+o.idx+"_container,#mCSB_"+o.idx+"_container_wrapper,#mCSB_"+o.idx+"_dragger_vertical,#mCSB_"+o.idx+"_dragger_horizontal");n.each(function(){K.call(this)})},Q=function(t,o,n){function i(e){return s&&c.callbacks[e]&&"function"==typeof c.callbacks[e]}function r(){return[c.callbacks.alwaysTriggerOffsets||_>=w[0]+b,c.callbacks.alwaysTriggerOffsets||-C>=_]}function l(){var e=[h[0].offsetTop,h[0].offsetLeft],o=[v[0].offsetTop,v[0].offsetLeft],a=[h.outerHeight(!1),h.outerWidth(!1)],i=[f.height(),f.width()];t[0].mcs={content:h,top:e[0],left:e[1],draggerTop:o[0],draggerLeft:o[1],topPct:Math.round(100*Math.abs(e[0])/(Math.abs(a[0])-i[0])),leftPct:Math.round(100*Math.abs(e[1])/(Math.abs(a[1])-i[1])),direction:n.dir}}var s=t.data(a),c=s.opt,d={trigger:"internal",dir:"y",scrollEasing:"mcsEaseOut",drag:!1,dur:c.scrollInertia,overwrite:"all",callbacks:!0,onStart:!0,onUpdate:!0,onComplete:!0},n=e.extend(d,n),u=[n.dur,n.drag?0:n.dur],f=e("#mCSB_"+s.idx),h=e("#mCSB_"+s.idx+"_container"),m=h.parent(),p=c.callbacks.onTotalScrollOffset?Y.call(t,c.callbacks.onTotalScrollOffset):[0,0],g=c.callbacks.onTotalScrollBackOffset?Y.call(t,c.callbacks.onTotalScrollBackOffset):[0,0];
if(s.trigger=n.trigger,(0!==m.scrollTop()||0!==m.scrollLeft())&&(e(".mCSB_"+s.idx+"_scrollbar").css("visibility","visible"),m.scrollTop(0).scrollLeft(0)),"_resetY"!==o||s.contentReset.y||(i("onOverflowYNone")&&c.callbacks.onOverflowYNone.call(t[0]),s.contentReset.y=1),"_resetX"!==o||s.contentReset.x||(i("onOverflowXNone")&&c.callbacks.onOverflowXNone.call(t[0]),s.contentReset.x=1),"_resetY"!==o&&"_resetX"!==o){switch(!s.contentReset.y&&t[0].mcs||!s.overflowed[0]||(i("onOverflowY")&&c.callbacks.onOverflowY.call(t[0]),s.contentReset.x=null),!s.contentReset.x&&t[0].mcs||!s.overflowed[1]||(i("onOverflowX")&&c.callbacks.onOverflowX.call(t[0]),s.contentReset.x=null),c.snapAmount&&(o=N(o,c.snapAmount,c.snapOffset)),n.dir){case"x":var v=e("#mCSB_"+s.idx+"_dragger_horizontal"),x="left",_=h[0].offsetLeft,w=[f.width()-h.outerWidth(!1),v.parent().width()-v.width()],S=[o,0===o?0:o/s.scrollRatio.x],b=p[1],C=g[1],B=b>0?b/s.scrollRatio.x:0,T=C>0?C/s.scrollRatio.x:0;break;case"y":var v=e("#mCSB_"+s.idx+"_dragger_vertical"),x="top",_=h[0].offsetTop,w=[f.height()-h.outerHeight(!1),v.parent().height()-v.height()],S=[o,0===o?0:o/s.scrollRatio.y],b=p[0],C=g[0],B=b>0?b/s.scrollRatio.y:0,T=C>0?C/s.scrollRatio.y:0}S[1]<0||0===S[0]&&0===S[1]?S=[0,0]:S[1]>=w[1]?S=[w[0],w[1]]:S[0]=-S[0],t[0].mcs||(l(),i("onInit")&&c.callbacks.onInit.call(t[0])),clearTimeout(h[0].onCompleteTimeout),(s.tweenRunning||!(0===_&&S[0]>=0||_===w[0]&&S[0]<=w[0]))&&(G(v[0],x,Math.round(S[1]),u[1],n.scrollEasing),G(h[0],x,Math.round(S[0]),u[0],n.scrollEasing,n.overwrite,{onStart:function(){n.callbacks&&n.onStart&&!s.tweenRunning&&(i("onScrollStart")&&(l(),c.callbacks.onScrollStart.call(t[0])),s.tweenRunning=!0,y(v),s.cbOffsets=r())},onUpdate:function(){n.callbacks&&n.onUpdate&&i("whileScrolling")&&(l(),c.callbacks.whileScrolling.call(t[0]))},onComplete:function(){if(n.callbacks&&n.onComplete){"yx"===c.axis&&clearTimeout(h[0].onCompleteTimeout);var e=h[0].idleTimer||0;h[0].onCompleteTimeout=setTimeout(function(){i("onScroll")&&(l(),c.callbacks.onScroll.call(t[0])),i("onTotalScroll")&&S[1]>=w[1]-B&&s.cbOffsets[0]&&(l(),c.callbacks.onTotalScroll.call(t[0])),i("onTotalScrollBack")&&S[1]<=T&&s.cbOffsets[1]&&(l(),c.callbacks.onTotalScrollBack.call(t[0])),s.tweenRunning=!1,h[0].idleTimer=0,y(v,"hide")},e)}}}))}},G=function(e,t,o,a,n,i,r){function l(){S.stop||(x||m.call(),x=J()-v,s(),x>=S.time&&(S.time=x>S.time?x+f-(x-S.time):x+f-1,S.time<x+1&&(S.time=x+1)),S.time<a?S.id=h(l):g.call())}function s(){a>0?(S.currVal=u(S.time,_,b,a,n),w[t]=Math.round(S.currVal)+"px"):w[t]=o+"px",p.call()}function c(){f=1e3/60,S.time=x+f,h=window.requestAnimationFrame?window.requestAnimationFrame:function(e){return s(),setTimeout(e,.01)},S.id=h(l)}function d(){null!=S.id&&(window.requestAnimationFrame?window.cancelAnimationFrame(S.id):clearTimeout(S.id),S.id=null)}function u(e,t,o,a,n){switch(n){case"linear":case"mcsLinear":return o*e/a+t;case"mcsLinearOut":return e/=a,e--,o*Math.sqrt(1-e*e)+t;case"easeInOutSmooth":return e/=a/2,1>e?o/2*e*e+t:(e--,-o/2*(e*(e-2)-1)+t);case"easeInOutStrong":return e/=a/2,1>e?o/2*Math.pow(2,10*(e-1))+t:(e--,o/2*(-Math.pow(2,-10*e)+2)+t);case"easeInOut":case"mcsEaseInOut":return e/=a/2,1>e?o/2*e*e*e+t:(e-=2,o/2*(e*e*e+2)+t);case"easeOutSmooth":return e/=a,e--,-o*(e*e*e*e-1)+t;case"easeOutStrong":return o*(-Math.pow(2,-10*e/a)+1)+t;case"easeOut":case"mcsEaseOut":default:var i=(e/=a)*e,r=i*e;return t+o*(.499999999999997*r*i+-2.5*i*i+5.5*r+-6.5*i+4*e)}}e._mTween||(e._mTween={top:{},left:{}});var f,h,r=r||{},m=r.onStart||function(){},p=r.onUpdate||function(){},g=r.onComplete||function(){},v=J(),x=0,_=e.offsetTop,w=e.style,S=e._mTween[t];"left"===t&&(_=e.offsetLeft);var b=o-_;S.stop=0,"none"!==i&&d(),c()},J=function(){return window.performance&&window.performance.now?window.performance.now():window.performance&&window.performance.webkitNow?window.performance.webkitNow():Date.now?Date.now():(new Date).getTime()},K=function(){var e=this;e._mTween||(e._mTween={top:{},left:{}});for(var t=["top","left"],o=0;o<t.length;o++){var a=t[o];e._mTween[a].id&&(window.requestAnimationFrame?window.cancelAnimationFrame(e._mTween[a].id):clearTimeout(e._mTween[a].id),e._mTween[a].id=null,e._mTween[a].stop=1)}},Z=function(e,t){try{delete e[t]}catch(o){e[t]=null}},$=function(e){return!(e.which&&1!==e.which)},et=function(e){var t=e.originalEvent.pointerType;return!(t&&"touch"!==t&&2!==t)},tt=function(e){return!isNaN(parseFloat(e))&&isFinite(e)},ot=function(e){var t=e.parents(".mCSB_container");return[e.offset().top-t.offset().top,e.offset().left-t.offset().left]};e.fn[o]=function(t){return u[t]?u[t].apply(this,Array.prototype.slice.call(arguments,1)):"object"!=typeof t&&t?void e.error("Method "+t+" does not exist"):u.init.apply(this,arguments)},e[o]=function(t){return u[t]?u[t].apply(this,Array.prototype.slice.call(arguments,1)):"object"!=typeof t&&t?void e.error("Method "+t+" does not exist"):u.init.apply(this,arguments)},e[o].defaults=i,window[o]=!0,e(window).load(function(){e(n)[o](),e.extend(e.expr[":"],{mcsInView:e.expr[":"].mcsInView||function(t){var o,a,n=e(t),i=n.parents(".mCSB_container");if(i.length)return o=i.parent(),a=[i[0].offsetTop,i[0].offsetLeft],a[0]+ot(n)[0]>=0&&a[0]+ot(n)[0]<o.height()-n.outerHeight(!1)&&a[1]+ot(n)[1]>=0&&a[1]+ot(n)[1]<o.width()-n.outerWidth(!1)},mcsOverflow:e.expr[":"].mcsOverflow||function(t){var o=e(t).data(a);if(o)return o.overflowed[0]||o.overflowed[1]}})})})});
//! moment.js
//! version : 2.9.0
//! authors : Tim Wood, Iskren Chernev, Moment.js contributors
//! license : MIT
//! momentjs.com

(function (undefined) {
    /************************************
        Constants
    ************************************/

    var moment,
        VERSION = '2.9.0',
        // the global-scope this is NOT the global object in Node.js
        globalScope = (typeof global !== 'undefined' && (typeof window === 'undefined' || window === global.window)) ? global : this,
        oldGlobalMoment,
        round = Math.round,
        hasOwnProperty = Object.prototype.hasOwnProperty,
        i,

        YEAR = 0,
        MONTH = 1,
        DATE = 2,
        HOUR = 3,
        MINUTE = 4,
        SECOND = 5,
        MILLISECOND = 6,

        // internal storage for locale config files
        locales = {},

        // extra moment internal properties (plugins register props here)
        momentProperties = [],

        // check for nodeJS
        hasModule = (typeof module !== 'undefined' && module && module.exports),

        // ASP.NET json date format regex
        aspNetJsonRegex = /^\/?Date\((\-?\d+)/i,
        aspNetTimeSpanJsonRegex = /(\-)?(?:(\d*)\.)?(\d+)\:(\d+)(?:\:(\d+)\.?(\d{3})?)?/,

        // from http://docs.closure-library.googlecode.com/git/closure_goog_date_date.js.source.html
        // somewhat more in line with 4.4.3.2 2004 spec, but allows decimal anywhere
        isoDurationRegex = /^(-)?P(?:(?:([0-9,.]*)Y)?(?:([0-9,.]*)M)?(?:([0-9,.]*)D)?(?:T(?:([0-9,.]*)H)?(?:([0-9,.]*)M)?(?:([0-9,.]*)S)?)?|([0-9,.]*)W)$/,

        // format tokens
        formattingTokens = /(\[[^\[]*\])|(\\)?(Mo|MM?M?M?|Do|DDDo|DD?D?D?|ddd?d?|do?|w[o|w]?|W[o|W]?|Q|YYYYYY|YYYYY|YYYY|YY|gg(ggg?)?|GG(GGG?)?|e|E|a|A|hh?|HH?|mm?|ss?|S{1,4}|x|X|zz?|ZZ?|.)/g,
        localFormattingTokens = /(\[[^\[]*\])|(\\)?(LTS|LT|LL?L?L?|l{1,4})/g,

        // parsing token regexes
        parseTokenOneOrTwoDigits = /\d\d?/, // 0 - 99
        parseTokenOneToThreeDigits = /\d{1,3}/, // 0 - 999
        parseTokenOneToFourDigits = /\d{1,4}/, // 0 - 9999
        parseTokenOneToSixDigits = /[+\-]?\d{1,6}/, // -999,999 - 999,999
        parseTokenDigits = /\d+/, // nonzero number of digits
        parseTokenWord = /[0-9]*['a-z\u00A0-\u05FF\u0700-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+|[\u0600-\u06FF\/]+(\s*?[\u0600-\u06FF]+){1,2}/i, // any word (or two) characters or numbers including two/three word month in arabic.
        parseTokenTimezone = /Z|[\+\-]\d\d:?\d\d/gi, // +00:00 -00:00 +0000 -0000 or Z
        parseTokenT = /T/i, // T (ISO separator)
        parseTokenOffsetMs = /[\+\-]?\d+/, // 1234567890123
        parseTokenTimestampMs = /[\+\-]?\d+(\.\d{1,3})?/, // 123456789 123456789.123

        //strict parsing regexes
        parseTokenOneDigit = /\d/, // 0 - 9
        parseTokenTwoDigits = /\d\d/, // 00 - 99
        parseTokenThreeDigits = /\d{3}/, // 000 - 999
        parseTokenFourDigits = /\d{4}/, // 0000 - 9999
        parseTokenSixDigits = /[+-]?\d{6}/, // -999,999 - 999,999
        parseTokenSignedNumber = /[+-]?\d+/, // -inf - inf

        // iso 8601 regex
        // 0000-00-00 0000-W00 or 0000-W00-0 + T + 00 or 00:00 or 00:00:00 or 00:00:00.000 + +00:00 or +0000 or +00)
        isoRegex = /^\s*(?:[+-]\d{6}|\d{4})-(?:(\d\d-\d\d)|(W\d\d$)|(W\d\d-\d)|(\d\d\d))((T| )(\d\d(:\d\d(:\d\d(\.\d+)?)?)?)?([\+\-]\d\d(?::?\d\d)?|\s*Z)?)?$/,

        isoFormat = 'YYYY-MM-DDTHH:mm:ssZ',

        isoDates = [
            ['YYYYYY-MM-DD', /[+-]\d{6}-\d{2}-\d{2}/],
            ['YYYY-MM-DD', /\d{4}-\d{2}-\d{2}/],
            ['GGGG-[W]WW-E', /\d{4}-W\d{2}-\d/],
            ['GGGG-[W]WW', /\d{4}-W\d{2}/],
            ['YYYY-DDD', /\d{4}-\d{3}/]
        ],

        // iso time formats and regexes
        isoTimes = [
            ['HH:mm:ss.SSSS', /(T| )\d\d:\d\d:\d\d\.\d+/],
            ['HH:mm:ss', /(T| )\d\d:\d\d:\d\d/],
            ['HH:mm', /(T| )\d\d:\d\d/],
            ['HH', /(T| )\d\d/]
        ],

        // timezone chunker '+10:00' > ['10', '00'] or '-1530' > ['-', '15', '30']
        parseTimezoneChunker = /([\+\-]|\d\d)/gi,

        // getter and setter names
        proxyGettersAndSetters = 'Date|Hours|Minutes|Seconds|Milliseconds'.split('|'),
        unitMillisecondFactors = {
            'Milliseconds' : 1,
            'Seconds' : 1e3,
            'Minutes' : 6e4,
            'Hours' : 36e5,
            'Days' : 864e5,
            'Months' : 2592e6,
            'Years' : 31536e6
        },

        unitAliases = {
            ms : 'millisecond',
            s : 'second',
            m : 'minute',
            h : 'hour',
            d : 'day',
            D : 'date',
            w : 'week',
            W : 'isoWeek',
            M : 'month',
            Q : 'quarter',
            y : 'year',
            DDD : 'dayOfYear',
            e : 'weekday',
            E : 'isoWeekday',
            gg: 'weekYear',
            GG: 'isoWeekYear'
        },

        camelFunctions = {
            dayofyear : 'dayOfYear',
            isoweekday : 'isoWeekday',
            isoweek : 'isoWeek',
            weekyear : 'weekYear',
            isoweekyear : 'isoWeekYear'
        },

        // format function strings
        formatFunctions = {},

        // default relative time thresholds
        relativeTimeThresholds = {
            s: 45,  // seconds to minute
            m: 45,  // minutes to hour
            h: 22,  // hours to day
            d: 26,  // days to month
            M: 11   // months to year
        },

        // tokens to ordinalize and pad
        ordinalizeTokens = 'DDD w W M D d'.split(' '),
        paddedTokens = 'M D H h m s w W'.split(' '),

        formatTokenFunctions = {
            M    : function () {
                return this.month() + 1;
            },
            MMM  : function (format) {
                return this.localeData().monthsShort(this, format);
            },
            MMMM : function (format) {
                return this.localeData().months(this, format);
            },
            D    : function () {
                return this.date();
            },
            DDD  : function () {
                return this.dayOfYear();
            },
            d    : function () {
                return this.day();
            },
            dd   : function (format) {
                return this.localeData().weekdaysMin(this, format);
            },
            ddd  : function (format) {
                return this.localeData().weekdaysShort(this, format);
            },
            dddd : function (format) {
                return this.localeData().weekdays(this, format);
            },
            w    : function () {
                return this.week();
            },
            W    : function () {
                return this.isoWeek();
            },
            YY   : function () {
                return leftZeroFill(this.year() % 100, 2);
            },
            YYYY : function () {
                return leftZeroFill(this.year(), 4);
            },
            YYYYY : function () {
                return leftZeroFill(this.year(), 5);
            },
            YYYYYY : function () {
                var y = this.year(), sign = y >= 0 ? '+' : '-';
                return sign + leftZeroFill(Math.abs(y), 6);
            },
            gg   : function () {
                return leftZeroFill(this.weekYear() % 100, 2);
            },
            gggg : function () {
                return leftZeroFill(this.weekYear(), 4);
            },
            ggggg : function () {
                return leftZeroFill(this.weekYear(), 5);
            },
            GG   : function () {
                return leftZeroFill(this.isoWeekYear() % 100, 2);
            },
            GGGG : function () {
                return leftZeroFill(this.isoWeekYear(), 4);
            },
            GGGGG : function () {
                return leftZeroFill(this.isoWeekYear(), 5);
            },
            e : function () {
                return this.weekday();
            },
            E : function () {
                return this.isoWeekday();
            },
            a    : function () {
                return this.localeData().meridiem(this.hours(), this.minutes(), true);
            },
            A    : function () {
                return this.localeData().meridiem(this.hours(), this.minutes(), false);
            },
            H    : function () {
                return this.hours();
            },
            h    : function () {
                return this.hours() % 12 || 12;
            },
            m    : function () {
                return this.minutes();
            },
            s    : function () {
                return this.seconds();
            },
            S    : function () {
                return toInt(this.milliseconds() / 100);
            },
            SS   : function () {
                return leftZeroFill(toInt(this.milliseconds() / 10), 2);
            },
            SSS  : function () {
                return leftZeroFill(this.milliseconds(), 3);
            },
            SSSS : function () {
                return leftZeroFill(this.milliseconds(), 3);
            },
            Z    : function () {
                var a = this.utcOffset(),
                    b = '+';
                if (a < 0) {
                    a = -a;
                    b = '-';
                }
                return b + leftZeroFill(toInt(a / 60), 2) + ':' + leftZeroFill(toInt(a) % 60, 2);
            },
            ZZ   : function () {
                var a = this.utcOffset(),
                    b = '+';
                if (a < 0) {
                    a = -a;
                    b = '-';
                }
                return b + leftZeroFill(toInt(a / 60), 2) + leftZeroFill(toInt(a) % 60, 2);
            },
            z : function () {
                return this.zoneAbbr();
            },
            zz : function () {
                return this.zoneName();
            },
            x    : function () {
                return this.valueOf();
            },
            X    : function () {
                return this.unix();
            },
            Q : function () {
                return this.quarter();
            }
        },

        deprecations = {},

        lists = ['months', 'monthsShort', 'weekdays', 'weekdaysShort', 'weekdaysMin'],

        updateInProgress = false;

    // Pick the first defined of two or three arguments. dfl comes from
    // default.
    function dfl(a, b, c) {
        switch (arguments.length) {
            case 2: return a != null ? a : b;
            case 3: return a != null ? a : b != null ? b : c;
            default: throw new Error('Implement me');
        }
    }

    function hasOwnProp(a, b) {
        return hasOwnProperty.call(a, b);
    }

    function defaultParsingFlags() {
        // We need to deep clone this object, and es5 standard is not very
        // helpful.
        return {
            empty : false,
            unusedTokens : [],
            unusedInput : [],
            overflow : -2,
            charsLeftOver : 0,
            nullInput : false,
            invalidMonth : null,
            invalidFormat : false,
            userInvalidated : false,
            iso: false
        };
    }

    function printMsg(msg) {
        if (moment.suppressDeprecationWarnings === false &&
                typeof console !== 'undefined' && console.warn) {
            console.warn('Deprecation warning: ' + msg);
        }
    }

    function deprecate(msg, fn) {
        var firstTime = true;
        return extend(function () {
            if (firstTime) {
                printMsg(msg);
                firstTime = false;
            }
            return fn.apply(this, arguments);
        }, fn);
    }

    function deprecateSimple(name, msg) {
        if (!deprecations[name]) {
            printMsg(msg);
            deprecations[name] = true;
        }
    }

    function padToken(func, count) {
        return function (a) {
            return leftZeroFill(func.call(this, a), count);
        };
    }
    function ordinalizeToken(func, period) {
        return function (a) {
            return this.localeData().ordinal(func.call(this, a), period);
        };
    }

    function monthDiff(a, b) {
        // difference in months
        var wholeMonthDiff = ((b.year() - a.year()) * 12) + (b.month() - a.month()),
            // b is in (anchor - 1 month, anchor + 1 month)
            anchor = a.clone().add(wholeMonthDiff, 'months'),
            anchor2, adjust;

        if (b - anchor < 0) {
            anchor2 = a.clone().add(wholeMonthDiff - 1, 'months');
            // linear across the month
            adjust = (b - anchor) / (anchor - anchor2);
        } else {
            anchor2 = a.clone().add(wholeMonthDiff + 1, 'months');
            // linear across the month
            adjust = (b - anchor) / (anchor2 - anchor);
        }

        return -(wholeMonthDiff + adjust);
    }

    while (ordinalizeTokens.length) {
        i = ordinalizeTokens.pop();
        formatTokenFunctions[i + 'o'] = ordinalizeToken(formatTokenFunctions[i], i);
    }
    while (paddedTokens.length) {
        i = paddedTokens.pop();
        formatTokenFunctions[i + i] = padToken(formatTokenFunctions[i], 2);
    }
    formatTokenFunctions.DDDD = padToken(formatTokenFunctions.DDD, 3);


    function meridiemFixWrap(locale, hour, meridiem) {
        var isPm;

        if (meridiem == null) {
            // nothing to do
            return hour;
        }
        if (locale.meridiemHour != null) {
            return locale.meridiemHour(hour, meridiem);
        } else if (locale.isPM != null) {
            // Fallback
            isPm = locale.isPM(meridiem);
            if (isPm && hour < 12) {
                hour += 12;
            }
            if (!isPm && hour === 12) {
                hour = 0;
            }
            return hour;
        } else {
            // thie is not supposed to happen
            return hour;
        }
    }

    /************************************
        Constructors
    ************************************/

    function Locale() {
    }

    // Moment prototype object
    function Moment(config, skipOverflow) {
        if (skipOverflow !== false) {
            checkOverflow(config);
        }
        copyConfig(this, config);
        this._d = new Date(+config._d);
        // Prevent infinite loop in case updateOffset creates new moment
        // objects.
        if (updateInProgress === false) {
            updateInProgress = true;
            moment.updateOffset(this);
            updateInProgress = false;
        }
    }

    // Duration Constructor
    function Duration(duration) {
        var normalizedInput = normalizeObjectUnits(duration),
            years = normalizedInput.year || 0,
            quarters = normalizedInput.quarter || 0,
            months = normalizedInput.month || 0,
            weeks = normalizedInput.week || 0,
            days = normalizedInput.day || 0,
            hours = normalizedInput.hour || 0,
            minutes = normalizedInput.minute || 0,
            seconds = normalizedInput.second || 0,
            milliseconds = normalizedInput.millisecond || 0;

        // representation for dateAddRemove
        this._milliseconds = +milliseconds +
            seconds * 1e3 + // 1000
            minutes * 6e4 + // 1000 * 60
            hours * 36e5; // 1000 * 60 * 60
        // Because of dateAddRemove treats 24 hours as different from a
        // day when working around DST, we need to store them separately
        this._days = +days +
            weeks * 7;
        // It is impossible translate months into days without knowing
        // which months you are are talking about, so we have to store
        // it separately.
        this._months = +months +
            quarters * 3 +
            years * 12;

        this._data = {};

        this._locale = moment.localeData();

        this._bubble();
    }

    /************************************
        Helpers
    ************************************/


    function extend(a, b) {
        for (var i in b) {
            if (hasOwnProp(b, i)) {
                a[i] = b[i];
            }
        }

        if (hasOwnProp(b, 'toString')) {
            a.toString = b.toString;
        }

        if (hasOwnProp(b, 'valueOf')) {
            a.valueOf = b.valueOf;
        }

        return a;
    }

    function copyConfig(to, from) {
        var i, prop, val;

        if (typeof from._isAMomentObject !== 'undefined') {
            to._isAMomentObject = from._isAMomentObject;
        }
        if (typeof from._i !== 'undefined') {
            to._i = from._i;
        }
        if (typeof from._f !== 'undefined') {
            to._f = from._f;
        }
        if (typeof from._l !== 'undefined') {
            to._l = from._l;
        }
        if (typeof from._strict !== 'undefined') {
            to._strict = from._strict;
        }
        if (typeof from._tzm !== 'undefined') {
            to._tzm = from._tzm;
        }
        if (typeof from._isUTC !== 'undefined') {
            to._isUTC = from._isUTC;
        }
        if (typeof from._offset !== 'undefined') {
            to._offset = from._offset;
        }
        if (typeof from._pf !== 'undefined') {
            to._pf = from._pf;
        }
        if (typeof from._locale !== 'undefined') {
            to._locale = from._locale;
        }

        if (momentProperties.length > 0) {
            for (i in momentProperties) {
                prop = momentProperties[i];
                val = from[prop];
                if (typeof val !== 'undefined') {
                    to[prop] = val;
                }
            }
        }

        return to;
    }

    function absRound(number) {
        if (number < 0) {
            return Math.ceil(number);
        } else {
            return Math.floor(number);
        }
    }

    // left zero fill a number
    // see http://jsperf.com/left-zero-filling for performance comparison
    function leftZeroFill(number, targetLength, forceSign) {
        var output = '' + Math.abs(number),
            sign = number >= 0;

        while (output.length < targetLength) {
            output = '0' + output;
        }
        return (sign ? (forceSign ? '+' : '') : '-') + output;
    }

    function positiveMomentsDifference(base, other) {
        var res = {milliseconds: 0, months: 0};

        res.months = other.month() - base.month() +
            (other.year() - base.year()) * 12;
        if (base.clone().add(res.months, 'M').isAfter(other)) {
            --res.months;
        }

        res.milliseconds = +other - +(base.clone().add(res.months, 'M'));

        return res;
    }

    function momentsDifference(base, other) {
        var res;
        other = makeAs(other, base);
        if (base.isBefore(other)) {
            res = positiveMomentsDifference(base, other);
        } else {
            res = positiveMomentsDifference(other, base);
            res.milliseconds = -res.milliseconds;
            res.months = -res.months;
        }

        return res;
    }

    // TODO: remove 'name' arg after deprecation is removed
    function createAdder(direction, name) {
        return function (val, period) {
            var dur, tmp;
            //invert the arguments, but complain about it
            if (period !== null && !isNaN(+period)) {
                deprecateSimple(name, 'moment().' + name  + '(period, number) is deprecated. Please use moment().' + name + '(number, period).');
                tmp = val; val = period; period = tmp;
            }

            val = typeof val === 'string' ? +val : val;
            dur = moment.duration(val, period);
            addOrSubtractDurationFromMoment(this, dur, direction);
            return this;
        };
    }

    function addOrSubtractDurationFromMoment(mom, duration, isAdding, updateOffset) {
        var milliseconds = duration._milliseconds,
            days = duration._days,
            months = duration._months;
        updateOffset = updateOffset == null ? true : updateOffset;

        if (milliseconds) {
            mom._d.setTime(+mom._d + milliseconds * isAdding);
        }
        if (days) {
            rawSetter(mom, 'Date', rawGetter(mom, 'Date') + days * isAdding);
        }
        if (months) {
            rawMonthSetter(mom, rawGetter(mom, 'Month') + months * isAdding);
        }
        if (updateOffset) {
            moment.updateOffset(mom, days || months);
        }
    }

    // check if is an array
    function isArray(input) {
        return Object.prototype.toString.call(input) === '[object Array]';
    }

    function isDate(input) {
        return Object.prototype.toString.call(input) === '[object Date]' ||
            input instanceof Date;
    }

    // compare two arrays, return the number of differences
    function compareArrays(array1, array2, dontConvert) {
        var len = Math.min(array1.length, array2.length),
            lengthDiff = Math.abs(array1.length - array2.length),
            diffs = 0,
            i;
        for (i = 0; i < len; i++) {
            if ((dontConvert && array1[i] !== array2[i]) ||
                (!dontConvert && toInt(array1[i]) !== toInt(array2[i]))) {
                diffs++;
            }
        }
        return diffs + lengthDiff;
    }

    function normalizeUnits(units) {
        if (units) {
            var lowered = units.toLowerCase().replace(/(.)s$/, '$1');
            units = unitAliases[units] || camelFunctions[lowered] || lowered;
        }
        return units;
    }

    function normalizeObjectUnits(inputObject) {
        var normalizedInput = {},
            normalizedProp,
            prop;

        for (prop in inputObject) {
            if (hasOwnProp(inputObject, prop)) {
                normalizedProp = normalizeUnits(prop);
                if (normalizedProp) {
                    normalizedInput[normalizedProp] = inputObject[prop];
                }
            }
        }

        return normalizedInput;
    }

    function makeList(field) {
        var count, setter;

        if (field.indexOf('week') === 0) {
            count = 7;
            setter = 'day';
        }
        else if (field.indexOf('month') === 0) {
            count = 12;
            setter = 'month';
        }
        else {
            return;
        }

        moment[field] = function (format, index) {
            var i, getter,
                method = moment._locale[field],
                results = [];

            if (typeof format === 'number') {
                index = format;
                format = undefined;
            }

            getter = function (i) {
                var m = moment().utc().set(setter, i);
                return method.call(moment._locale, m, format || '');
            };

            if (index != null) {
                return getter(index);
            }
            else {
                for (i = 0; i < count; i++) {
                    results.push(getter(i));
                }
                return results;
            }
        };
    }

    function toInt(argumentForCoercion) {
        var coercedNumber = +argumentForCoercion,
            value = 0;

        if (coercedNumber !== 0 && isFinite(coercedNumber)) {
            if (coercedNumber >= 0) {
                value = Math.floor(coercedNumber);
            } else {
                value = Math.ceil(coercedNumber);
            }
        }

        return value;
    }

    function daysInMonth(year, month) {
        return new Date(Date.UTC(year, month + 1, 0)).getUTCDate();
    }

    function weeksInYear(year, dow, doy) {
        return weekOfYear(moment([year, 11, 31 + dow - doy]), dow, doy).week;
    }

    function daysInYear(year) {
        return isLeapYear(year) ? 366 : 365;
    }

    function isLeapYear(year) {
        return (year % 4 === 0 && year % 100 !== 0) || year % 400 === 0;
    }

    function checkOverflow(m) {
        var overflow;
        if (m._a && m._pf.overflow === -2) {
            overflow =
                m._a[MONTH] < 0 || m._a[MONTH] > 11 ? MONTH :
                m._a[DATE] < 1 || m._a[DATE] > daysInMonth(m._a[YEAR], m._a[MONTH]) ? DATE :
                m._a[HOUR] < 0 || m._a[HOUR] > 24 ||
                    (m._a[HOUR] === 24 && (m._a[MINUTE] !== 0 ||
                                           m._a[SECOND] !== 0 ||
                                           m._a[MILLISECOND] !== 0)) ? HOUR :
                m._a[MINUTE] < 0 || m._a[MINUTE] > 59 ? MINUTE :
                m._a[SECOND] < 0 || m._a[SECOND] > 59 ? SECOND :
                m._a[MILLISECOND] < 0 || m._a[MILLISECOND] > 999 ? MILLISECOND :
                -1;

            if (m._pf._overflowDayOfYear && (overflow < YEAR || overflow > DATE)) {
                overflow = DATE;
            }

            m._pf.overflow = overflow;
        }
    }

    function isValid(m) {
        if (m._isValid == null) {
            m._isValid = !isNaN(m._d.getTime()) &&
                m._pf.overflow < 0 &&
                !m._pf.empty &&
                !m._pf.invalidMonth &&
                !m._pf.nullInput &&
                !m._pf.invalidFormat &&
                !m._pf.userInvalidated;

            if (m._strict) {
                m._isValid = m._isValid &&
                    m._pf.charsLeftOver === 0 &&
                    m._pf.unusedTokens.length === 0 &&
                    m._pf.bigHour === undefined;
            }
        }
        return m._isValid;
    }

    function normalizeLocale(key) {
        return key ? key.toLowerCase().replace('_', '-') : key;
    }

    // pick the locale from the array
    // try ['en-au', 'en-gb'] as 'en-au', 'en-gb', 'en', as in move through the list trying each
    // substring from most specific to least, but move to the next array item if it's a more specific variant than the current root
    function chooseLocale(names) {
        var i = 0, j, next, locale, split;

        while (i < names.length) {
            split = normalizeLocale(names[i]).split('-');
            j = split.length;
            next = normalizeLocale(names[i + 1]);
            next = next ? next.split('-') : null;
            while (j > 0) {
                locale = loadLocale(split.slice(0, j).join('-'));
                if (locale) {
                    return locale;
                }
                if (next && next.length >= j && compareArrays(split, next, true) >= j - 1) {
                    //the next array item is better than a shallower substring of this one
                    break;
                }
                j--;
            }
            i++;
        }
        return null;
    }

    function loadLocale(name) {
        var oldLocale = null;
        if (!locales[name] && hasModule) {
            try {
                oldLocale = moment.locale();
                require('./locale/' + name);
                // because defineLocale currently also sets the global locale, we want to undo that for lazy loaded locales
                moment.locale(oldLocale);
            } catch (e) { }
        }
        return locales[name];
    }

    // Return a moment from input, that is local/utc/utcOffset equivalent to
    // model.
    function makeAs(input, model) {
        var res, diff;
        if (model._isUTC) {
            res = model.clone();
            diff = (moment.isMoment(input) || isDate(input) ?
                    +input : +moment(input)) - (+res);
            // Use low-level api, because this fn is low-level api.
            res._d.setTime(+res._d + diff);
            moment.updateOffset(res, false);
            return res;
        } else {
            return moment(input).local();
        }
    }

    /************************************
        Locale
    ************************************/


    extend(Locale.prototype, {

        set : function (config) {
            var prop, i;
            for (i in config) {
                prop = config[i];
                if (typeof prop === 'function') {
                    this[i] = prop;
                } else {
                    this['_' + i] = prop;
                }
            }
            // Lenient ordinal parsing accepts just a number in addition to
            // number + (possibly) stuff coming from _ordinalParseLenient.
            this._ordinalParseLenient = new RegExp(this._ordinalParse.source + '|' + /\d{1,2}/.source);
        },

        _months : 'January_February_March_April_May_June_July_August_September_October_November_December'.split('_'),
        months : function (m) {
            return this._months[m.month()];
        },

        _monthsShort : 'Jan_Feb_Mar_Apr_May_Jun_Jul_Aug_Sep_Oct_Nov_Dec'.split('_'),
        monthsShort : function (m) {
            return this._monthsShort[m.month()];
        },

        monthsParse : function (monthName, format, strict) {
            var i, mom, regex;

            if (!this._monthsParse) {
                this._monthsParse = [];
                this._longMonthsParse = [];
                this._shortMonthsParse = [];
            }

            for (i = 0; i < 12; i++) {
                // make the regex if we don't have it already
                mom = moment.utc([2000, i]);
                if (strict && !this._longMonthsParse[i]) {
                    this._longMonthsParse[i] = new RegExp('^' + this.months(mom, '').replace('.', '') + '$', 'i');
                    this._shortMonthsParse[i] = new RegExp('^' + this.monthsShort(mom, '').replace('.', '') + '$', 'i');
                }
                if (!strict && !this._monthsParse[i]) {
                    regex = '^' + this.months(mom, '') + '|^' + this.monthsShort(mom, '');
                    this._monthsParse[i] = new RegExp(regex.replace('.', ''), 'i');
                }
                // test the regex
                if (strict && format === 'MMMM' && this._longMonthsParse[i].test(monthName)) {
                    return i;
                } else if (strict && format === 'MMM' && this._shortMonthsParse[i].test(monthName)) {
                    return i;
                } else if (!strict && this._monthsParse[i].test(monthName)) {
                    return i;
                }
            }
        },

        _weekdays : 'Sunday_Monday_Tuesday_Wednesday_Thursday_Friday_Saturday'.split('_'),
        weekdays : function (m) {
            return this._weekdays[m.day()];
        },

        _weekdaysShort : 'Sun_Mon_Tue_Wed_Thu_Fri_Sat'.split('_'),
        weekdaysShort : function (m) {
            return this._weekdaysShort[m.day()];
        },

        _weekdaysMin : 'Su_Mo_Tu_We_Th_Fr_Sa'.split('_'),
        weekdaysMin : function (m) {
            return this._weekdaysMin[m.day()];
        },

        weekdaysParse : function (weekdayName) {
            var i, mom, regex;

            if (!this._weekdaysParse) {
                this._weekdaysParse = [];
            }

            for (i = 0; i < 7; i++) {
                // make the regex if we don't have it already
                if (!this._weekdaysParse[i]) {
                    mom = moment([2000, 1]).day(i);
                    regex = '^' + this.weekdays(mom, '') + '|^' + this.weekdaysShort(mom, '') + '|^' + this.weekdaysMin(mom, '');
                    this._weekdaysParse[i] = new RegExp(regex.replace('.', ''), 'i');
                }
                // test the regex
                if (this._weekdaysParse[i].test(weekdayName)) {
                    return i;
                }
            }
        },

        _longDateFormat : {
            LTS : 'h:mm:ss A',
            LT : 'h:mm A',
            L : 'MM/DD/YYYY',
            LL : 'MMMM D, YYYY',
            LLL : 'MMMM D, YYYY LT',
            LLLL : 'dddd, MMMM D, YYYY LT'
        },
        longDateFormat : function (key) {
            var output = this._longDateFormat[key];
            if (!output && this._longDateFormat[key.toUpperCase()]) {
                output = this._longDateFormat[key.toUpperCase()].replace(/MMMM|MM|DD|dddd/g, function (val) {
                    return val.slice(1);
                });
                this._longDateFormat[key] = output;
            }
            return output;
        },

        isPM : function (input) {
            // IE8 Quirks Mode & IE7 Standards Mode do not allow accessing strings like arrays
            // Using charAt should be more compatible.
            return ((input + '').toLowerCase().charAt(0) === 'p');
        },

        _meridiemParse : /[ap]\.?m?\.?/i,
        meridiem : function (hours, minutes, isLower) {
            if (hours > 11) {
                return isLower ? 'pm' : 'PM';
            } else {
                return isLower ? 'am' : 'AM';
            }
        },


        _calendar : {
            sameDay : '[Today at] LT',
            nextDay : '[Tomorrow at] LT',
            nextWeek : 'dddd [at] LT',
            lastDay : '[Yesterday at] LT',
            lastWeek : '[Last] dddd [at] LT',
            sameElse : 'L'
        },
        calendar : function (key, mom, now) {
            var output = this._calendar[key];
            return typeof output === 'function' ? output.apply(mom, [now]) : output;
        },

        _relativeTime : {
            future : 'in %s',
            past : '%s ago',
            s : 'a few seconds',
            m : 'a minute',
            mm : '%d minutes',
            h : 'an hour',
            hh : '%d hours',
            d : 'a day',
            dd : '%d days',
            M : 'a month',
            MM : '%d months',
            y : 'a year',
            yy : '%d years'
        },

        relativeTime : function (number, withoutSuffix, string, isFuture) {
            var output = this._relativeTime[string];
            return (typeof output === 'function') ?
                output(number, withoutSuffix, string, isFuture) :
                output.replace(/%d/i, number);
        },

        pastFuture : function (diff, output) {
            var format = this._relativeTime[diff > 0 ? 'future' : 'past'];
            return typeof format === 'function' ? format(output) : format.replace(/%s/i, output);
        },

        ordinal : function (number) {
            return this._ordinal.replace('%d', number);
        },
        _ordinal : '%d',
        _ordinalParse : /\d{1,2}/,

        preparse : function (string) {
            return string;
        },

        postformat : function (string) {
            return string;
        },

        week : function (mom) {
            return weekOfYear(mom, this._week.dow, this._week.doy).week;
        },

        _week : {
            dow : 0, // Sunday is the first day of the week.
            doy : 6  // The week that contains Jan 1st is the first week of the year.
        },

        firstDayOfWeek : function () {
            return this._week.dow;
        },

        firstDayOfYear : function () {
            return this._week.doy;
        },

        _invalidDate: 'Invalid date',
        invalidDate: function () {
            return this._invalidDate;
        }
    });

    /************************************
        Formatting
    ************************************/


    function removeFormattingTokens(input) {
        if (input.match(/\[[\s\S]/)) {
            return input.replace(/^\[|\]$/g, '');
        }
        return input.replace(/\\/g, '');
    }

    function makeFormatFunction(format) {
        var array = format.match(formattingTokens), i, length;

        for (i = 0, length = array.length; i < length; i++) {
            if (formatTokenFunctions[array[i]]) {
                array[i] = formatTokenFunctions[array[i]];
            } else {
                array[i] = removeFormattingTokens(array[i]);
            }
        }

        return function (mom) {
            var output = '';
            for (i = 0; i < length; i++) {
                output += array[i] instanceof Function ? array[i].call(mom, format) : array[i];
            }
            return output;
        };
    }

    // format date using native date object
    function formatMoment(m, format) {
        if (!m.isValid()) {
            return m.localeData().invalidDate();
        }

        format = expandFormat(format, m.localeData());

        if (!formatFunctions[format]) {
            formatFunctions[format] = makeFormatFunction(format);
        }

        return formatFunctions[format](m);
    }

    function expandFormat(format, locale) {
        var i = 5;

        function replaceLongDateFormatTokens(input) {
            return locale.longDateFormat(input) || input;
        }

        localFormattingTokens.lastIndex = 0;
        while (i >= 0 && localFormattingTokens.test(format)) {
            format = format.replace(localFormattingTokens, replaceLongDateFormatTokens);
            localFormattingTokens.lastIndex = 0;
            i -= 1;
        }

        return format;
    }


    /************************************
        Parsing
    ************************************/


    // get the regex to find the next token
    function getParseRegexForToken(token, config) {
        var a, strict = config._strict;
        switch (token) {
        case 'Q':
            return parseTokenOneDigit;
        case 'DDDD':
            return parseTokenThreeDigits;
        case 'YYYY':
        case 'GGGG':
        case 'gggg':
            return strict ? parseTokenFourDigits : parseTokenOneToFourDigits;
        case 'Y':
        case 'G':
        case 'g':
            return parseTokenSignedNumber;
        case 'YYYYYY':
        case 'YYYYY':
        case 'GGGGG':
        case 'ggggg':
            return strict ? parseTokenSixDigits : parseTokenOneToSixDigits;
        case 'S':
            if (strict) {
                return parseTokenOneDigit;
            }
            /* falls through */
        case 'SS':
            if (strict) {
                return parseTokenTwoDigits;
            }
            /* falls through */
        case 'SSS':
            if (strict) {
                return parseTokenThreeDigits;
            }
            /* falls through */
        case 'DDD':
            return parseTokenOneToThreeDigits;
        case 'MMM':
        case 'MMMM':
        case 'dd':
        case 'ddd':
        case 'dddd':
            return parseTokenWord;
        case 'a':
        case 'A':
            return config._locale._meridiemParse;
        case 'x':
            return parseTokenOffsetMs;
        case 'X':
            return parseTokenTimestampMs;
        case 'Z':
        case 'ZZ':
            return parseTokenTimezone;
        case 'T':
            return parseTokenT;
        case 'SSSS':
            return parseTokenDigits;
        case 'MM':
        case 'DD':
        case 'YY':
        case 'GG':
        case 'gg':
        case 'HH':
        case 'hh':
        case 'mm':
        case 'ss':
        case 'ww':
        case 'WW':
            return strict ? parseTokenTwoDigits : parseTokenOneOrTwoDigits;
        case 'M':
        case 'D':
        case 'd':
        case 'H':
        case 'h':
        case 'm':
        case 's':
        case 'w':
        case 'W':
        case 'e':
        case 'E':
            return parseTokenOneOrTwoDigits;
        case 'Do':
            return strict ? config._locale._ordinalParse : config._locale._ordinalParseLenient;
        default :
            a = new RegExp(regexpEscape(unescapeFormat(token.replace('\\', '')), 'i'));
            return a;
        }
    }

    function utcOffsetFromString(string) {
        string = string || '';
        var possibleTzMatches = (string.match(parseTokenTimezone) || []),
            tzChunk = possibleTzMatches[possibleTzMatches.length - 1] || [],
            parts = (tzChunk + '').match(parseTimezoneChunker) || ['-', 0, 0],
            minutes = +(parts[1] * 60) + toInt(parts[2]);

        return parts[0] === '+' ? minutes : -minutes;
    }

    // function to convert string input to date
    function addTimeToArrayFromToken(token, input, config) {
        var a, datePartArray = config._a;

        switch (token) {
        // QUARTER
        case 'Q':
            if (input != null) {
                datePartArray[MONTH] = (toInt(input) - 1) * 3;
            }
            break;
        // MONTH
        case 'M' : // fall through to MM
        case 'MM' :
            if (input != null) {
                datePartArray[MONTH] = toInt(input) - 1;
            }
            break;
        case 'MMM' : // fall through to MMMM
        case 'MMMM' :
            a = config._locale.monthsParse(input, token, config._strict);
            // if we didn't find a month name, mark the date as invalid.
            if (a != null) {
                datePartArray[MONTH] = a;
            } else {
                config._pf.invalidMonth = input;
            }
            break;
        // DAY OF MONTH
        case 'D' : // fall through to DD
        case 'DD' :
            if (input != null) {
                datePartArray[DATE] = toInt(input);
            }
            break;
        case 'Do' :
            if (input != null) {
                datePartArray[DATE] = toInt(parseInt(
                            input.match(/\d{1,2}/)[0], 10));
            }
            break;
        // DAY OF YEAR
        case 'DDD' : // fall through to DDDD
        case 'DDDD' :
            if (input != null) {
                config._dayOfYear = toInt(input);
            }

            break;
        // YEAR
        case 'YY' :
            datePartArray[YEAR] = moment.parseTwoDigitYear(input);
            break;
        case 'YYYY' :
        case 'YYYYY' :
        case 'YYYYYY' :
            datePartArray[YEAR] = toInt(input);
            break;
        // AM / PM
        case 'a' : // fall through to A
        case 'A' :
            config._meridiem = input;
            // config._isPm = config._locale.isPM(input);
            break;
        // HOUR
        case 'h' : // fall through to hh
        case 'hh' :
            config._pf.bigHour = true;
            /* falls through */
        case 'H' : // fall through to HH
        case 'HH' :
            datePartArray[HOUR] = toInt(input);
            break;
        // MINUTE
        case 'm' : // fall through to mm
        case 'mm' :
            datePartArray[MINUTE] = toInt(input);
            break;
        // SECOND
        case 's' : // fall through to ss
        case 'ss' :
            datePartArray[SECOND] = toInt(input);
            break;
        // MILLISECOND
        case 'S' :
        case 'SS' :
        case 'SSS' :
        case 'SSSS' :
            datePartArray[MILLISECOND] = toInt(('0.' + input) * 1000);
            break;
        // UNIX OFFSET (MILLISECONDS)
        case 'x':
            config._d = new Date(toInt(input));
            break;
        // UNIX TIMESTAMP WITH MS
        case 'X':
            config._d = new Date(parseFloat(input) * 1000);
            break;
        // TIMEZONE
        case 'Z' : // fall through to ZZ
        case 'ZZ' :
            config._useUTC = true;
            config._tzm = utcOffsetFromString(input);
            break;
        // WEEKDAY - human
        case 'dd':
        case 'ddd':
        case 'dddd':
            a = config._locale.weekdaysParse(input);
            // if we didn't get a weekday name, mark the date as invalid
            if (a != null) {
                config._w = config._w || {};
                config._w['d'] = a;
            } else {
                config._pf.invalidWeekday = input;
            }
            break;
        // WEEK, WEEK DAY - numeric
        case 'w':
        case 'ww':
        case 'W':
        case 'WW':
        case 'd':
        case 'e':
        case 'E':
            token = token.substr(0, 1);
            /* falls through */
        case 'gggg':
        case 'GGGG':
        case 'GGGGG':
            token = token.substr(0, 2);
            if (input) {
                config._w = config._w || {};
                config._w[token] = toInt(input);
            }
            break;
        case 'gg':
        case 'GG':
            config._w = config._w || {};
            config._w[token] = moment.parseTwoDigitYear(input);
        }
    }

    function dayOfYearFromWeekInfo(config) {
        var w, weekYear, week, weekday, dow, doy, temp;

        w = config._w;
        if (w.GG != null || w.W != null || w.E != null) {
            dow = 1;
            doy = 4;

            // TODO: We need to take the current isoWeekYear, but that depends on
            // how we interpret now (local, utc, fixed offset). So create
            // a now version of current config (take local/utc/offset flags, and
            // create now).
            weekYear = dfl(w.GG, config._a[YEAR], weekOfYear(moment(), 1, 4).year);
            week = dfl(w.W, 1);
            weekday = dfl(w.E, 1);
        } else {
            dow = config._locale._week.dow;
            doy = config._locale._week.doy;

            weekYear = dfl(w.gg, config._a[YEAR], weekOfYear(moment(), dow, doy).year);
            week = dfl(w.w, 1);

            if (w.d != null) {
                // weekday -- low day numbers are considered next week
                weekday = w.d;
                if (weekday < dow) {
                    ++week;
                }
            } else if (w.e != null) {
                // local weekday -- counting starts from begining of week
                weekday = w.e + dow;
            } else {
                // default to begining of week
                weekday = dow;
            }
        }
        temp = dayOfYearFromWeeks(weekYear, week, weekday, doy, dow);

        config._a[YEAR] = temp.year;
        config._dayOfYear = temp.dayOfYear;
    }

    // convert an array to a date.
    // the array should mirror the parameters below
    // note: all values past the year are optional and will default to the lowest possible value.
    // [year, month, day , hour, minute, second, millisecond]
    function dateFromConfig(config) {
        var i, date, input = [], currentDate, yearToUse;

        if (config._d) {
            return;
        }

        currentDate = currentDateArray(config);

        //compute day of the year from weeks and weekdays
        if (config._w && config._a[DATE] == null && config._a[MONTH] == null) {
            dayOfYearFromWeekInfo(config);
        }

        //if the day of the year is set, figure out what it is
        if (config._dayOfYear) {
            yearToUse = dfl(config._a[YEAR], currentDate[YEAR]);

            if (config._dayOfYear > daysInYear(yearToUse)) {
                config._pf._overflowDayOfYear = true;
            }

            date = makeUTCDate(yearToUse, 0, config._dayOfYear);
            config._a[MONTH] = date.getUTCMonth();
            config._a[DATE] = date.getUTCDate();
        }

        // Default to current date.
        // * if no year, month, day of month are given, default to today
        // * if day of month is given, default month and year
        // * if month is given, default only year
        // * if year is given, don't default anything
        for (i = 0; i < 3 && config._a[i] == null; ++i) {
            config._a[i] = input[i] = currentDate[i];
        }

        // Zero out whatever was not defaulted, including time
        for (; i < 7; i++) {
            config._a[i] = input[i] = (config._a[i] == null) ? (i === 2 ? 1 : 0) : config._a[i];
        }

        // Check for 24:00:00.000
        if (config._a[HOUR] === 24 &&
                config._a[MINUTE] === 0 &&
                config._a[SECOND] === 0 &&
                config._a[MILLISECOND] === 0) {
            config._nextDay = true;
            config._a[HOUR] = 0;
        }

        config._d = (config._useUTC ? makeUTCDate : makeDate).apply(null, input);
        // Apply timezone offset from input. The actual utcOffset can be changed
        // with parseZone.
        if (config._tzm != null) {
            config._d.setUTCMinutes(config._d.getUTCMinutes() - config._tzm);
        }

        if (config._nextDay) {
            config._a[HOUR] = 24;
        }
    }

    function dateFromObject(config) {
        var normalizedInput;

        if (config._d) {
            return;
        }

        normalizedInput = normalizeObjectUnits(config._i);
        config._a = [
            normalizedInput.year,
            normalizedInput.month,
            normalizedInput.day || normalizedInput.date,
            normalizedInput.hour,
            normalizedInput.minute,
            normalizedInput.second,
            normalizedInput.millisecond
        ];

        dateFromConfig(config);
    }

    function currentDateArray(config) {
        var now = new Date();
        if (config._useUTC) {
            return [
                now.getUTCFullYear(),
                now.getUTCMonth(),
                now.getUTCDate()
            ];
        } else {
            return [now.getFullYear(), now.getMonth(), now.getDate()];
        }
    }

    // date from string and format string
    function makeDateFromStringAndFormat(config) {
        if (config._f === moment.ISO_8601) {
            parseISO(config);
            return;
        }

        config._a = [];
        config._pf.empty = true;

        // This array is used to make a Date, either with `new Date` or `Date.UTC`
        var string = '' + config._i,
            i, parsedInput, tokens, token, skipped,
            stringLength = string.length,
            totalParsedInputLength = 0;

        tokens = expandFormat(config._f, config._locale).match(formattingTokens) || [];

        for (i = 0; i < tokens.length; i++) {
            token = tokens[i];
            parsedInput = (string.match(getParseRegexForToken(token, config)) || [])[0];
            if (parsedInput) {
                skipped = string.substr(0, string.indexOf(parsedInput));
                if (skipped.length > 0) {
                    config._pf.unusedInput.push(skipped);
                }
                string = string.slice(string.indexOf(parsedInput) + parsedInput.length);
                totalParsedInputLength += parsedInput.length;
            }
            // don't parse if it's not a known token
            if (formatTokenFunctions[token]) {
                if (parsedInput) {
                    config._pf.empty = false;
                }
                else {
                    config._pf.unusedTokens.push(token);
                }
                addTimeToArrayFromToken(token, parsedInput, config);
            }
            else if (config._strict && !parsedInput) {
                config._pf.unusedTokens.push(token);
            }
        }

        // add remaining unparsed input length to the string
        config._pf.charsLeftOver = stringLength - totalParsedInputLength;
        if (string.length > 0) {
            config._pf.unusedInput.push(string);
        }

        // clear _12h flag if hour is <= 12
        if (config._pf.bigHour === true && config._a[HOUR] <= 12) {
            config._pf.bigHour = undefined;
        }
        // handle meridiem
        config._a[HOUR] = meridiemFixWrap(config._locale, config._a[HOUR],
                config._meridiem);
        dateFromConfig(config);
        checkOverflow(config);
    }

    function unescapeFormat(s) {
        return s.replace(/\\(\[)|\\(\])|\[([^\]\[]*)\]|\\(.)/g, function (matched, p1, p2, p3, p4) {
            return p1 || p2 || p3 || p4;
        });
    }

    // Code from http://stackoverflow.com/questions/3561493/is-there-a-regexp-escape-function-in-javascript
    function regexpEscape(s) {
        return s.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
    }

    // date from string and array of format strings
    function makeDateFromStringAndArray(config) {
        var tempConfig,
            bestMoment,

            scoreToBeat,
            i,
            currentScore;

        if (config._f.length === 0) {
            config._pf.invalidFormat = true;
            config._d = new Date(NaN);
            return;
        }

        for (i = 0; i < config._f.length; i++) {
            currentScore = 0;
            tempConfig = copyConfig({}, config);
            if (config._useUTC != null) {
                tempConfig._useUTC = config._useUTC;
            }
            tempConfig._pf = defaultParsingFlags();
            tempConfig._f = config._f[i];
            makeDateFromStringAndFormat(tempConfig);

            if (!isValid(tempConfig)) {
                continue;
            }

            // if there is any input that was not parsed add a penalty for that format
            currentScore += tempConfig._pf.charsLeftOver;

            //or tokens
            currentScore += tempConfig._pf.unusedTokens.length * 10;

            tempConfig._pf.score = currentScore;

            if (scoreToBeat == null || currentScore < scoreToBeat) {
                scoreToBeat = currentScore;
                bestMoment = tempConfig;
            }
        }

        extend(config, bestMoment || tempConfig);
    }

    // date from iso format
    function parseISO(config) {
        var i, l,
            string = config._i,
            match = isoRegex.exec(string);

        if (match) {
            config._pf.iso = true;
            for (i = 0, l = isoDates.length; i < l; i++) {
                if (isoDates[i][1].exec(string)) {
                    // match[5] should be 'T' or undefined
                    config._f = isoDates[i][0] + (match[6] || ' ');
                    break;
                }
            }
            for (i = 0, l = isoTimes.length; i < l; i++) {
                if (isoTimes[i][1].exec(string)) {
                    config._f += isoTimes[i][0];
                    break;
                }
            }
            if (string.match(parseTokenTimezone)) {
                config._f += 'Z';
            }
            makeDateFromStringAndFormat(config);
        } else {
            config._isValid = false;
        }
    }

    // date from iso format or fallback
    function makeDateFromString(config) {
        parseISO(config);
        if (config._isValid === false) {
            delete config._isValid;
            moment.createFromInputFallback(config);
        }
    }

    function map(arr, fn) {
        var res = [], i;
        for (i = 0; i < arr.length; ++i) {
            res.push(fn(arr[i], i));
        }
        return res;
    }

    function makeDateFromInput(config) {
        var input = config._i, matched;
        if (input === undefined) {
            config._d = new Date();
        } else if (isDate(input)) {
            config._d = new Date(+input);
        } else if ((matched = aspNetJsonRegex.exec(input)) !== null) {
            config._d = new Date(+matched[1]);
        } else if (typeof input === 'string') {
            makeDateFromString(config);
        } else if (isArray(input)) {
            config._a = map(input.slice(0), function (obj) {
                return parseInt(obj, 10);
            });
            dateFromConfig(config);
        } else if (typeof(input) === 'object') {
            dateFromObject(config);
        } else if (typeof(input) === 'number') {
            // from milliseconds
            config._d = new Date(input);
        } else {
            moment.createFromInputFallback(config);
        }
    }

    function makeDate(y, m, d, h, M, s, ms) {
        //can't just apply() to create a date:
        //http://stackoverflow.com/questions/181348/instantiating-a-javascript-object-by-calling-prototype-constructor-apply
        var date = new Date(y, m, d, h, M, s, ms);

        //the date constructor doesn't accept years < 1970
        if (y < 1970) {
            date.setFullYear(y);
        }
        return date;
    }

    function makeUTCDate(y) {
        var date = new Date(Date.UTC.apply(null, arguments));
        if (y < 1970) {
            date.setUTCFullYear(y);
        }
        return date;
    }

    function parseWeekday(input, locale) {
        if (typeof input === 'string') {
            if (!isNaN(input)) {
                input = parseInt(input, 10);
            }
            else {
                input = locale.weekdaysParse(input);
                if (typeof input !== 'number') {
                    return null;
                }
            }
        }
        return input;
    }

    /************************************
        Relative Time
    ************************************/


    // helper function for moment.fn.from, moment.fn.fromNow, and moment.duration.fn.humanize
    function substituteTimeAgo(string, number, withoutSuffix, isFuture, locale) {
        return locale.relativeTime(number || 1, !!withoutSuffix, string, isFuture);
    }

    function relativeTime(posNegDuration, withoutSuffix, locale) {
        var duration = moment.duration(posNegDuration).abs(),
            seconds = round(duration.as('s')),
            minutes = round(duration.as('m')),
            hours = round(duration.as('h')),
            days = round(duration.as('d')),
            months = round(duration.as('M')),
            years = round(duration.as('y')),

            args = seconds < relativeTimeThresholds.s && ['s', seconds] ||
                minutes === 1 && ['m'] ||
                minutes < relativeTimeThresholds.m && ['mm', minutes] ||
                hours === 1 && ['h'] ||
                hours < relativeTimeThresholds.h && ['hh', hours] ||
                days === 1 && ['d'] ||
                days < relativeTimeThresholds.d && ['dd', days] ||
                months === 1 && ['M'] ||
                months < relativeTimeThresholds.M && ['MM', months] ||
                years === 1 && ['y'] || ['yy', years];

        args[2] = withoutSuffix;
        args[3] = +posNegDuration > 0;
        args[4] = locale;
        return substituteTimeAgo.apply({}, args);
    }


    /************************************
        Week of Year
    ************************************/


    // firstDayOfWeek       0 = sun, 6 = sat
    //                      the day of the week that starts the week
    //                      (usually sunday or monday)
    // firstDayOfWeekOfYear 0 = sun, 6 = sat
    //                      the first week is the week that contains the first
    //                      of this day of the week
    //                      (eg. ISO weeks use thursday (4))
    function weekOfYear(mom, firstDayOfWeek, firstDayOfWeekOfYear) {
        var end = firstDayOfWeekOfYear - firstDayOfWeek,
            daysToDayOfWeek = firstDayOfWeekOfYear - mom.day(),
            adjustedMoment;


        if (daysToDayOfWeek > end) {
            daysToDayOfWeek -= 7;
        }

        if (daysToDayOfWeek < end - 7) {
            daysToDayOfWeek += 7;
        }

        adjustedMoment = moment(mom).add(daysToDayOfWeek, 'd');
        return {
            week: Math.ceil(adjustedMoment.dayOfYear() / 7),
            year: adjustedMoment.year()
        };
    }

    //http://en.wikipedia.org/wiki/ISO_week_date#Calculating_a_date_given_the_year.2C_week_number_and_weekday
    function dayOfYearFromWeeks(year, week, weekday, firstDayOfWeekOfYear, firstDayOfWeek) {
        var d = makeUTCDate(year, 0, 1).getUTCDay(), daysToAdd, dayOfYear;

        d = d === 0 ? 7 : d;
        weekday = weekday != null ? weekday : firstDayOfWeek;
        daysToAdd = firstDayOfWeek - d + (d > firstDayOfWeekOfYear ? 7 : 0) - (d < firstDayOfWeek ? 7 : 0);
        dayOfYear = 7 * (week - 1) + (weekday - firstDayOfWeek) + daysToAdd + 1;

        return {
            year: dayOfYear > 0 ? year : year - 1,
            dayOfYear: dayOfYear > 0 ?  dayOfYear : daysInYear(year - 1) + dayOfYear
        };
    }

    /************************************
        Top Level Functions
    ************************************/

    function makeMoment(config) {
        var input = config._i,
            format = config._f,
            res;

        config._locale = config._locale || moment.localeData(config._l);

        if (input === null || (format === undefined && input === '')) {
            return moment.invalid({nullInput: true});
        }

        if (typeof input === 'string') {
            config._i = input = config._locale.preparse(input);
        }

        if (moment.isMoment(input)) {
            return new Moment(input, true);
        } else if (format) {
            if (isArray(format)) {
                makeDateFromStringAndArray(config);
            } else {
                makeDateFromStringAndFormat(config);
            }
        } else {
            makeDateFromInput(config);
        }

        res = new Moment(config);
        if (res._nextDay) {
            // Adding is smart enough around DST
            res.add(1, 'd');
            res._nextDay = undefined;
        }

        return res;
    }

    moment = function (input, format, locale, strict) {
        var c;

        if (typeof(locale) === 'boolean') {
            strict = locale;
            locale = undefined;
        }
        // object construction must be done this way.
        // https://github.com/moment/moment/issues/1423
        c = {};
        c._isAMomentObject = true;
        c._i = input;
        c._f = format;
        c._l = locale;
        c._strict = strict;
        c._isUTC = false;
        c._pf = defaultParsingFlags();

        return makeMoment(c);
    };

    moment.suppressDeprecationWarnings = false;

    moment.createFromInputFallback = deprecate(
        'moment construction falls back to js Date. This is ' +
        'discouraged and will be removed in upcoming major ' +
        'release. Please refer to ' +
        'https://github.com/moment/moment/issues/1407 for more info.',
        function (config) {
            config._d = new Date(config._i + (config._useUTC ? ' UTC' : ''));
        }
    );

    // Pick a moment m from moments so that m[fn](other) is true for all
    // other. This relies on the function fn to be transitive.
    //
    // moments should either be an array of moment objects or an array, whose
    // first element is an array of moment objects.
    function pickBy(fn, moments) {
        var res, i;
        if (moments.length === 1 && isArray(moments[0])) {
            moments = moments[0];
        }
        if (!moments.length) {
            return moment();
        }
        res = moments[0];
        for (i = 1; i < moments.length; ++i) {
            if (moments[i][fn](res)) {
                res = moments[i];
            }
        }
        return res;
    }

    moment.min = function () {
        var args = [].slice.call(arguments, 0);

        return pickBy('isBefore', args);
    };

    moment.max = function () {
        var args = [].slice.call(arguments, 0);

        return pickBy('isAfter', args);
    };

    // creating with utc
    moment.utc = function (input, format, locale, strict) {
        var c;

        if (typeof(locale) === 'boolean') {
            strict = locale;
            locale = undefined;
        }
        // object construction must be done this way.
        // https://github.com/moment/moment/issues/1423
        c = {};
        c._isAMomentObject = true;
        c._useUTC = true;
        c._isUTC = true;
        c._l = locale;
        c._i = input;
        c._f = format;
        c._strict = strict;
        c._pf = defaultParsingFlags();

        return makeMoment(c).utc();
    };

    // creating with unix timestamp (in seconds)
    moment.unix = function (input) {
        return moment(input * 1000);
    };

    // duration
    moment.duration = function (input, key) {
        var duration = input,
            // matching against regexp is expensive, do it on demand
            match = null,
            sign,
            ret,
            parseIso,
            diffRes;

        if (moment.isDuration(input)) {
            duration = {
                ms: input._milliseconds,
                d: input._days,
                M: input._months
            };
        } else if (typeof input === 'number') {
            duration = {};
            if (key) {
                duration[key] = input;
            } else {
                duration.milliseconds = input;
            }
        } else if (!!(match = aspNetTimeSpanJsonRegex.exec(input))) {
            sign = (match[1] === '-') ? -1 : 1;
            duration = {
                y: 0,
                d: toInt(match[DATE]) * sign,
                h: toInt(match[HOUR]) * sign,
                m: toInt(match[MINUTE]) * sign,
                s: toInt(match[SECOND]) * sign,
                ms: toInt(match[MILLISECOND]) * sign
            };
        } else if (!!(match = isoDurationRegex.exec(input))) {
            sign = (match[1] === '-') ? -1 : 1;
            parseIso = function (inp) {
                // We'd normally use ~~inp for this, but unfortunately it also
                // converts floats to ints.
                // inp may be undefined, so careful calling replace on it.
                var res = inp && parseFloat(inp.replace(',', '.'));
                // apply sign while we're at it
                return (isNaN(res) ? 0 : res) * sign;
            };
            duration = {
                y: parseIso(match[2]),
                M: parseIso(match[3]),
                d: parseIso(match[4]),
                h: parseIso(match[5]),
                m: parseIso(match[6]),
                s: parseIso(match[7]),
                w: parseIso(match[8])
            };
        } else if (duration == null) {// checks for null or undefined
            duration = {};
        } else if (typeof duration === 'object' &&
                ('from' in duration || 'to' in duration)) {
            diffRes = momentsDifference(moment(duration.from), moment(duration.to));

            duration = {};
            duration.ms = diffRes.milliseconds;
            duration.M = diffRes.months;
        }

        ret = new Duration(duration);

        if (moment.isDuration(input) && hasOwnProp(input, '_locale')) {
            ret._locale = input._locale;
        }

        return ret;
    };

    // version number
    moment.version = VERSION;

    // default format
    moment.defaultFormat = isoFormat;

    // constant that refers to the ISO standard
    moment.ISO_8601 = function () {};

    // Plugins that add properties should also add the key here (null value),
    // so we can properly clone ourselves.
    moment.momentProperties = momentProperties;

    // This function will be called whenever a moment is mutated.
    // It is intended to keep the offset in sync with the timezone.
    moment.updateOffset = function () {};

    // This function allows you to set a threshold for relative time strings
    moment.relativeTimeThreshold = function (threshold, limit) {
        if (relativeTimeThresholds[threshold] === undefined) {
            return false;
        }
        if (limit === undefined) {
            return relativeTimeThresholds[threshold];
        }
        relativeTimeThresholds[threshold] = limit;
        return true;
    };

    moment.lang = deprecate(
        'moment.lang is deprecated. Use moment.locale instead.',
        function (key, value) {
            return moment.locale(key, value);
        }
    );

    // This function will load locale and then set the global locale.  If
    // no arguments are passed in, it will simply return the current global
    // locale key.
    moment.locale = function (key, values) {
        var data;
        if (key) {
            if (typeof(values) !== 'undefined') {
                data = moment.defineLocale(key, values);
            }
            else {
                data = moment.localeData(key);
            }

            if (data) {
                moment.duration._locale = moment._locale = data;
            }
        }

        return moment._locale._abbr;
    };

    moment.defineLocale = function (name, values) {
        if (values !== null) {
            values.abbr = name;
            if (!locales[name]) {
                locales[name] = new Locale();
            }
            locales[name].set(values);

            // backwards compat for now: also set the locale
            moment.locale(name);

            return locales[name];
        } else {
            // useful for testing
            delete locales[name];
            return null;
        }
    };

    moment.langData = deprecate(
        'moment.langData is deprecated. Use moment.localeData instead.',
        function (key) {
            return moment.localeData(key);
        }
    );

    // returns locale data
    moment.localeData = function (key) {
        var locale;

        if (key && key._locale && key._locale._abbr) {
            key = key._locale._abbr;
        }

        if (!key) {
            return moment._locale;
        }

        if (!isArray(key)) {
            //short-circuit everything else
            locale = loadLocale(key);
            if (locale) {
                return locale;
            }
            key = [key];
        }

        return chooseLocale(key);
    };

    // compare moment object
    moment.isMoment = function (obj) {
        return obj instanceof Moment ||
            (obj != null && hasOwnProp(obj, '_isAMomentObject'));
    };

    // for typechecking Duration objects
    moment.isDuration = function (obj) {
        return obj instanceof Duration;
    };

    for (i = lists.length - 1; i >= 0; --i) {
        makeList(lists[i]);
    }

    moment.normalizeUnits = function (units) {
        return normalizeUnits(units);
    };

    moment.invalid = function (flags) {
        var m = moment.utc(NaN);
        if (flags != null) {
            extend(m._pf, flags);
        }
        else {
            m._pf.userInvalidated = true;
        }

        return m;
    };

    moment.parseZone = function () {
        return moment.apply(null, arguments).parseZone();
    };

    moment.parseTwoDigitYear = function (input) {
        return toInt(input) + (toInt(input) > 68 ? 1900 : 2000);
    };

    moment.isDate = isDate;

    /************************************
        Moment Prototype
    ************************************/


    extend(moment.fn = Moment.prototype, {

        clone : function () {
            return moment(this);
        },

        valueOf : function () {
            return +this._d - ((this._offset || 0) * 60000);
        },

        unix : function () {
            return Math.floor(+this / 1000);
        },

        toString : function () {
            return this.clone().locale('en').format('ddd MMM DD YYYY HH:mm:ss [GMT]ZZ');
        },

        toDate : function () {
            return this._offset ? new Date(+this) : this._d;
        },

        toISOString : function () {
            var m = moment(this).utc();
            if (0 < m.year() && m.year() <= 9999) {
                if ('function' === typeof Date.prototype.toISOString) {
                    // native implementation is ~50x faster, use it when we can
                    return this.toDate().toISOString();
                } else {
                    return formatMoment(m, 'YYYY-MM-DD[T]HH:mm:ss.SSS[Z]');
                }
            } else {
                return formatMoment(m, 'YYYYYY-MM-DD[T]HH:mm:ss.SSS[Z]');
            }
        },

        toArray : function () {
            var m = this;
            return [
                m.year(),
                m.month(),
                m.date(),
                m.hours(),
                m.minutes(),
                m.seconds(),
                m.milliseconds()
            ];
        },

        isValid : function () {
            return isValid(this);
        },

        isDSTShifted : function () {
            if (this._a) {
                return this.isValid() && compareArrays(this._a, (this._isUTC ? moment.utc(this._a) : moment(this._a)).toArray()) > 0;
            }

            return false;
        },

        parsingFlags : function () {
            return extend({}, this._pf);
        },

        invalidAt: function () {
            return this._pf.overflow;
        },

        utc : function (keepLocalTime) {
            return this.utcOffset(0, keepLocalTime);
        },

        local : function (keepLocalTime) {
            if (this._isUTC) {
                this.utcOffset(0, keepLocalTime);
                this._isUTC = false;

                if (keepLocalTime) {
                    this.subtract(this._dateUtcOffset(), 'm');
                }
            }
            return this;
        },

        format : function (inputString) {
            var output = formatMoment(this, inputString || moment.defaultFormat);
            return this.localeData().postformat(output);
        },

        add : createAdder(1, 'add'),

        subtract : createAdder(-1, 'subtract'),

        diff : function (input, units, asFloat) {
            var that = makeAs(input, this),
                zoneDiff = (that.utcOffset() - this.utcOffset()) * 6e4,
                anchor, diff, output, daysAdjust;

            units = normalizeUnits(units);

            if (units === 'year' || units === 'month' || units === 'quarter') {
                output = monthDiff(this, that);
                if (units === 'quarter') {
                    output = output / 3;
                } else if (units === 'year') {
                    output = output / 12;
                }
            } else {
                diff = this - that;
                output = units === 'second' ? diff / 1e3 : // 1000
                    units === 'minute' ? diff / 6e4 : // 1000 * 60
                    units === 'hour' ? diff / 36e5 : // 1000 * 60 * 60
                    units === 'day' ? (diff - zoneDiff) / 864e5 : // 1000 * 60 * 60 * 24, negate dst
                    units === 'week' ? (diff - zoneDiff) / 6048e5 : // 1000 * 60 * 60 * 24 * 7, negate dst
                    diff;
            }
            return asFloat ? output : absRound(output);
        },

        from : function (time, withoutSuffix) {
            return moment.duration({to: this, from: time}).locale(this.locale()).humanize(!withoutSuffix);
        },

        fromNow : function (withoutSuffix) {
            return this.from(moment(), withoutSuffix);
        },

        calendar : function (time) {
            // We want to compare the start of today, vs this.
            // Getting start-of-today depends on whether we're locat/utc/offset
            // or not.
            var now = time || moment(),
                sod = makeAs(now, this).startOf('day'),
                diff = this.diff(sod, 'days', true),
                format = diff < -6 ? 'sameElse' :
                    diff < -1 ? 'lastWeek' :
                    diff < 0 ? 'lastDay' :
                    diff < 1 ? 'sameDay' :
                    diff < 2 ? 'nextDay' :
                    diff < 7 ? 'nextWeek' : 'sameElse';
            return this.format(this.localeData().calendar(format, this, moment(now)));
        },

        isLeapYear : function () {
            return isLeapYear(this.year());
        },

        isDST : function () {
            return (this.utcOffset() > this.clone().month(0).utcOffset() ||
                this.utcOffset() > this.clone().month(5).utcOffset());
        },

        day : function (input) {
            var day = this._isUTC ? this._d.getUTCDay() : this._d.getDay();
            if (input != null) {
                input = parseWeekday(input, this.localeData());
                return this.add(input - day, 'd');
            } else {
                return day;
            }
        },

        month : makeAccessor('Month', true),

        startOf : function (units) {
            units = normalizeUnits(units);
            // the following switch intentionally omits break keywords
            // to utilize falling through the cases.
            switch (units) {
            case 'year':
                this.month(0);
                /* falls through */
            case 'quarter':
            case 'month':
                this.date(1);
                /* falls through */
            case 'week':
            case 'isoWeek':
            case 'day':
                this.hours(0);
                /* falls through */
            case 'hour':
                this.minutes(0);
                /* falls through */
            case 'minute':
                this.seconds(0);
                /* falls through */
            case 'second':
                this.milliseconds(0);
                /* falls through */
            }

            // weeks are a special case
            if (units === 'week') {
                this.weekday(0);
            } else if (units === 'isoWeek') {
                this.isoWeekday(1);
            }

            // quarters are also special
            if (units === 'quarter') {
                this.month(Math.floor(this.month() / 3) * 3);
            }

            return this;
        },

        endOf: function (units) {
            units = normalizeUnits(units);
            if (units === undefined || units === 'millisecond') {
                return this;
            }
            return this.startOf(units).add(1, (units === 'isoWeek' ? 'week' : units)).subtract(1, 'ms');
        },

        isAfter: function (input, units) {
            var inputMs;
            units = normalizeUnits(typeof units !== 'undefined' ? units : 'millisecond');
            if (units === 'millisecond') {
                input = moment.isMoment(input) ? input : moment(input);
                return +this > +input;
            } else {
                inputMs = moment.isMoment(input) ? +input : +moment(input);
                return inputMs < +this.clone().startOf(units);
            }
        },

        isBefore: function (input, units) {
            var inputMs;
            units = normalizeUnits(typeof units !== 'undefined' ? units : 'millisecond');
            if (units === 'millisecond') {
                input = moment.isMoment(input) ? input : moment(input);
                return +this < +input;
            } else {
                inputMs = moment.isMoment(input) ? +input : +moment(input);
                return +this.clone().endOf(units) < inputMs;
            }
        },

        isBetween: function (from, to, units) {
            return this.isAfter(from, units) && this.isBefore(to, units);
        },

        isSame: function (input, units) {
            var inputMs;
            units = normalizeUnits(units || 'millisecond');
            if (units === 'millisecond') {
                input = moment.isMoment(input) ? input : moment(input);
                return +this === +input;
            } else {
                inputMs = +moment(input);
                return +(this.clone().startOf(units)) <= inputMs && inputMs <= +(this.clone().endOf(units));
            }
        },

        min: deprecate(
                 'moment().min is deprecated, use moment.min instead. https://github.com/moment/moment/issues/1548',
                 function (other) {
                     other = moment.apply(null, arguments);
                     return other < this ? this : other;
                 }
         ),

        max: deprecate(
                'moment().max is deprecated, use moment.max instead. https://github.com/moment/moment/issues/1548',
                function (other) {
                    other = moment.apply(null, arguments);
                    return other > this ? this : other;
                }
        ),

        zone : deprecate(
                'moment().zone is deprecated, use moment().utcOffset instead. ' +
                'https://github.com/moment/moment/issues/1779',
                function (input, keepLocalTime) {
                    if (input != null) {
                        if (typeof input !== 'string') {
                            input = -input;
                        }

                        this.utcOffset(input, keepLocalTime);

                        return this;
                    } else {
                        return -this.utcOffset();
                    }
                }
        ),

        // keepLocalTime = true means only change the timezone, without
        // affecting the local hour. So 5:31:26 +0300 --[utcOffset(2, true)]-->
        // 5:31:26 +0200 It is possible that 5:31:26 doesn't exist with offset
        // +0200, so we adjust the time as needed, to be valid.
        //
        // Keeping the time actually adds/subtracts (one hour)
        // from the actual represented time. That is why we call updateOffset
        // a second time. In case it wants us to change the offset again
        // _changeInProgress == true case, then we have to adjust, because
        // there is no such time in the given timezone.
        utcOffset : function (input, keepLocalTime) {
            var offset = this._offset || 0,
                localAdjust;
            if (input != null) {
                if (typeof input === 'string') {
                    input = utcOffsetFromString(input);
                }
                if (Math.abs(input) < 16) {
                    input = input * 60;
                }
                if (!this._isUTC && keepLocalTime) {
                    localAdjust = this._dateUtcOffset();
                }
                this._offset = input;
                this._isUTC = true;
                if (localAdjust != null) {
                    this.add(localAdjust, 'm');
                }
                if (offset !== input) {
                    if (!keepLocalTime || this._changeInProgress) {
                        addOrSubtractDurationFromMoment(this,
                                moment.duration(input - offset, 'm'), 1, false);
                    } else if (!this._changeInProgress) {
                        this._changeInProgress = true;
                        moment.updateOffset(this, true);
                        this._changeInProgress = null;
                    }
                }

                return this;
            } else {
                return this._isUTC ? offset : this._dateUtcOffset();
            }
        },

        isLocal : function () {
            return !this._isUTC;
        },

        isUtcOffset : function () {
            return this._isUTC;
        },

        isUtc : function () {
            return this._isUTC && this._offset === 0;
        },

        zoneAbbr : function () {
            return this._isUTC ? 'UTC' : '';
        },

        zoneName : function () {
            return this._isUTC ? 'Coordinated Universal Time' : '';
        },

        parseZone : function () {
            if (this._tzm) {
                this.utcOffset(this._tzm);
            } else if (typeof this._i === 'string') {
                this.utcOffset(utcOffsetFromString(this._i));
            }
            return this;
        },

        hasAlignedHourOffset : function (input) {
            if (!input) {
                input = 0;
            }
            else {
                input = moment(input).utcOffset();
            }

            return (this.utcOffset() - input) % 60 === 0;
        },

        daysInMonth : function () {
            return daysInMonth(this.year(), this.month());
        },

        dayOfYear : function (input) {
            var dayOfYear = round((moment(this).startOf('day') - moment(this).startOf('year')) / 864e5) + 1;
            return input == null ? dayOfYear : this.add((input - dayOfYear), 'd');
        },

        quarter : function (input) {
            return input == null ? Math.ceil((this.month() + 1) / 3) : this.month((input - 1) * 3 + this.month() % 3);
        },

        weekYear : function (input) {
            var year = weekOfYear(this, this.localeData()._week.dow, this.localeData()._week.doy).year;
            return input == null ? year : this.add((input - year), 'y');
        },

        isoWeekYear : function (input) {
            var year = weekOfYear(this, 1, 4).year;
            return input == null ? year : this.add((input - year), 'y');
        },

        week : function (input) {
            var week = this.localeData().week(this);
            return input == null ? week : this.add((input - week) * 7, 'd');
        },

        isoWeek : function (input) {
            var week = weekOfYear(this, 1, 4).week;
            return input == null ? week : this.add((input - week) * 7, 'd');
        },

        weekday : function (input) {
            var weekday = (this.day() + 7 - this.localeData()._week.dow) % 7;
            return input == null ? weekday : this.add(input - weekday, 'd');
        },

        isoWeekday : function (input) {
            // behaves the same as moment#day except
            // as a getter, returns 7 instead of 0 (1-7 range instead of 0-6)
            // as a setter, sunday should belong to the previous week.
            return input == null ? this.day() || 7 : this.day(this.day() % 7 ? input : input - 7);
        },

        isoWeeksInYear : function () {
            return weeksInYear(this.year(), 1, 4);
        },

        weeksInYear : function () {
            var weekInfo = this.localeData()._week;
            return weeksInYear(this.year(), weekInfo.dow, weekInfo.doy);
        },

        get : function (units) {
            units = normalizeUnits(units);
            return this[units]();
        },

        set : function (units, value) {
            var unit;
            if (typeof units === 'object') {
                for (unit in units) {
                    this.set(unit, units[unit]);
                }
            }
            else {
                units = normalizeUnits(units);
                if (typeof this[units] === 'function') {
                    this[units](value);
                }
            }
            return this;
        },

        // If passed a locale key, it will set the locale for this
        // instance.  Otherwise, it will return the locale configuration
        // variables for this instance.
        locale : function (key) {
            var newLocaleData;

            if (key === undefined) {
                return this._locale._abbr;
            } else {
                newLocaleData = moment.localeData(key);
                if (newLocaleData != null) {
                    this._locale = newLocaleData;
                }
                return this;
            }
        },

        lang : deprecate(
            'moment().lang() is deprecated. Instead, use moment().localeData() to get the language configuration. Use moment().locale() to change languages.',
            function (key) {
                if (key === undefined) {
                    return this.localeData();
                } else {
                    return this.locale(key);
                }
            }
        ),

        localeData : function () {
            return this._locale;
        },

        _dateUtcOffset : function () {
            // On Firefox.24 Date#getTimezoneOffset returns a floating point.
            // https://github.com/moment/moment/pull/1871
            return -Math.round(this._d.getTimezoneOffset() / 15) * 15;
        }

    });

    function rawMonthSetter(mom, value) {
        var dayOfMonth;

        // TODO: Move this out of here!
        if (typeof value === 'string') {
            value = mom.localeData().monthsParse(value);
            // TODO: Another silent failure?
            if (typeof value !== 'number') {
                return mom;
            }
        }

        dayOfMonth = Math.min(mom.date(),
                daysInMonth(mom.year(), value));
        mom._d['set' + (mom._isUTC ? 'UTC' : '') + 'Month'](value, dayOfMonth);
        return mom;
    }

    function rawGetter(mom, unit) {
        return mom._d['get' + (mom._isUTC ? 'UTC' : '') + unit]();
    }

    function rawSetter(mom, unit, value) {
        if (unit === 'Month') {
            return rawMonthSetter(mom, value);
        } else {
            return mom._d['set' + (mom._isUTC ? 'UTC' : '') + unit](value);
        }
    }

    function makeAccessor(unit, keepTime) {
        return function (value) {
            if (value != null) {
                rawSetter(this, unit, value);
                moment.updateOffset(this, keepTime);
                return this;
            } else {
                return rawGetter(this, unit);
            }
        };
    }

    moment.fn.millisecond = moment.fn.milliseconds = makeAccessor('Milliseconds', false);
    moment.fn.second = moment.fn.seconds = makeAccessor('Seconds', false);
    moment.fn.minute = moment.fn.minutes = makeAccessor('Minutes', false);
    // Setting the hour should keep the time, because the user explicitly
    // specified which hour he wants. So trying to maintain the same hour (in
    // a new timezone) makes sense. Adding/subtracting hours does not follow
    // this rule.
    moment.fn.hour = moment.fn.hours = makeAccessor('Hours', true);
    // moment.fn.month is defined separately
    moment.fn.date = makeAccessor('Date', true);
    moment.fn.dates = deprecate('dates accessor is deprecated. Use date instead.', makeAccessor('Date', true));
    moment.fn.year = makeAccessor('FullYear', true);
    moment.fn.years = deprecate('years accessor is deprecated. Use year instead.', makeAccessor('FullYear', true));

    // add plural methods
    moment.fn.days = moment.fn.day;
    moment.fn.months = moment.fn.month;
    moment.fn.weeks = moment.fn.week;
    moment.fn.isoWeeks = moment.fn.isoWeek;
    moment.fn.quarters = moment.fn.quarter;

    // add aliased format methods
    moment.fn.toJSON = moment.fn.toISOString;

    // alias isUtc for dev-friendliness
    moment.fn.isUTC = moment.fn.isUtc;

    /************************************
        Duration Prototype
    ************************************/


    function daysToYears (days) {
        // 400 years have 146097 days (taking into account leap year rules)
        return days * 400 / 146097;
    }

    function yearsToDays (years) {
        // years * 365 + absRound(years / 4) -
        //     absRound(years / 100) + absRound(years / 400);
        return years * 146097 / 400;
    }

    extend(moment.duration.fn = Duration.prototype, {

        _bubble : function () {
            var milliseconds = this._milliseconds,
                days = this._days,
                months = this._months,
                data = this._data,
                seconds, minutes, hours, years = 0;

            // The following code bubbles up values, see the tests for
            // examples of what that means.
            data.milliseconds = milliseconds % 1000;

            seconds = absRound(milliseconds / 1000);
            data.seconds = seconds % 60;

            minutes = absRound(seconds / 60);
            data.minutes = minutes % 60;

            hours = absRound(minutes / 60);
            data.hours = hours % 24;

            days += absRound(hours / 24);

            // Accurately convert days to years, assume start from year 0.
            years = absRound(daysToYears(days));
            days -= absRound(yearsToDays(years));

            // 30 days to a month
            // TODO (iskren): Use anchor date (like 1st Jan) to compute this.
            months += absRound(days / 30);
            days %= 30;

            // 12 months -> 1 year
            years += absRound(months / 12);
            months %= 12;

            data.days = days;
            data.months = months;
            data.years = years;
        },

        abs : function () {
            this._milliseconds = Math.abs(this._milliseconds);
            this._days = Math.abs(this._days);
            this._months = Math.abs(this._months);

            this._data.milliseconds = Math.abs(this._data.milliseconds);
            this._data.seconds = Math.abs(this._data.seconds);
            this._data.minutes = Math.abs(this._data.minutes);
            this._data.hours = Math.abs(this._data.hours);
            this._data.months = Math.abs(this._data.months);
            this._data.years = Math.abs(this._data.years);

            return this;
        },

        weeks : function () {
            return absRound(this.days() / 7);
        },

        valueOf : function () {
            return this._milliseconds +
              this._days * 864e5 +
              (this._months % 12) * 2592e6 +
              toInt(this._months / 12) * 31536e6;
        },

        humanize : function (withSuffix) {
            var output = relativeTime(this, !withSuffix, this.localeData());

            if (withSuffix) {
                output = this.localeData().pastFuture(+this, output);
            }

            return this.localeData().postformat(output);
        },

        add : function (input, val) {
            // supports only 2.0-style add(1, 's') or add(moment)
            var dur = moment.duration(input, val);

            this._milliseconds += dur._milliseconds;
            this._days += dur._days;
            this._months += dur._months;

            this._bubble();

            return this;
        },

        subtract : function (input, val) {
            var dur = moment.duration(input, val);

            this._milliseconds -= dur._milliseconds;
            this._days -= dur._days;
            this._months -= dur._months;

            this._bubble();

            return this;
        },

        get : function (units) {
            units = normalizeUnits(units);
            return this[units.toLowerCase() + 's']();
        },

        as : function (units) {
            var days, months;
            units = normalizeUnits(units);

            if (units === 'month' || units === 'year') {
                days = this._days + this._milliseconds / 864e5;
                months = this._months + daysToYears(days) * 12;
                return units === 'month' ? months : months / 12;
            } else {
                // handle milliseconds separately because of floating point math errors (issue #1867)
                days = this._days + Math.round(yearsToDays(this._months / 12));
                switch (units) {
                    case 'week': return days / 7 + this._milliseconds / 6048e5;
                    case 'day': return days + this._milliseconds / 864e5;
                    case 'hour': return days * 24 + this._milliseconds / 36e5;
                    case 'minute': return days * 24 * 60 + this._milliseconds / 6e4;
                    case 'second': return days * 24 * 60 * 60 + this._milliseconds / 1000;
                    // Math.floor prevents floating point math errors here
                    case 'millisecond': return Math.floor(days * 24 * 60 * 60 * 1000) + this._milliseconds;
                    default: throw new Error('Unknown unit ' + units);
                }
            }
        },

        lang : moment.fn.lang,
        locale : moment.fn.locale,

        toIsoString : deprecate(
            'toIsoString() is deprecated. Please use toISOString() instead ' +
            '(notice the capitals)',
            function () {
                return this.toISOString();
            }
        ),

        toISOString : function () {
            // inspired by https://github.com/dordille/moment-isoduration/blob/master/moment.isoduration.js
            var years = Math.abs(this.years()),
                months = Math.abs(this.months()),
                days = Math.abs(this.days()),
                hours = Math.abs(this.hours()),
                minutes = Math.abs(this.minutes()),
                seconds = Math.abs(this.seconds() + this.milliseconds() / 1000);

            if (!this.asSeconds()) {
                // this is the same as C#'s (Noda) and python (isodate)...
                // but not other JS (goog.date)
                return 'P0D';
            }

            return (this.asSeconds() < 0 ? '-' : '') +
                'P' +
                (years ? years + 'Y' : '') +
                (months ? months + 'M' : '') +
                (days ? days + 'D' : '') +
                ((hours || minutes || seconds) ? 'T' : '') +
                (hours ? hours + 'H' : '') +
                (minutes ? minutes + 'M' : '') +
                (seconds ? seconds + 'S' : '');
        },

        localeData : function () {
            return this._locale;
        },

        toJSON : function () {
            return this.toISOString();
        }
    });

    moment.duration.fn.toString = moment.duration.fn.toISOString;

    function makeDurationGetter(name) {
        moment.duration.fn[name] = function () {
            return this._data[name];
        };
    }

    for (i in unitMillisecondFactors) {
        if (hasOwnProp(unitMillisecondFactors, i)) {
            makeDurationGetter(i.toLowerCase());
        }
    }

    moment.duration.fn.asMilliseconds = function () {
        return this.as('ms');
    };
    moment.duration.fn.asSeconds = function () {
        return this.as('s');
    };
    moment.duration.fn.asMinutes = function () {
        return this.as('m');
    };
    moment.duration.fn.asHours = function () {
        return this.as('h');
    };
    moment.duration.fn.asDays = function () {
        return this.as('d');
    };
    moment.duration.fn.asWeeks = function () {
        return this.as('weeks');
    };
    moment.duration.fn.asMonths = function () {
        return this.as('M');
    };
    moment.duration.fn.asYears = function () {
        return this.as('y');
    };

    /************************************
        Default Locale
    ************************************/


    // Set default locale, other locale will inherit from English.
    moment.locale('en', {
        ordinalParse: /\d{1,2}(th|st|nd|rd)/,
        ordinal : function (number) {
            var b = number % 10,
                output = (toInt(number % 100 / 10) === 1) ? 'th' :
                (b === 1) ? 'st' :
                (b === 2) ? 'nd' :
                (b === 3) ? 'rd' : 'th';
            return number + output;
        }
    });

    /* EMBED_LOCALES */

    /************************************
        Exposing Moment
    ************************************/

    function makeGlobal(shouldDeprecate) {
        /*global ender:false */
        if (typeof ender !== 'undefined') {
            return;
        }
        oldGlobalMoment = globalScope.moment;
        if (shouldDeprecate) {
            globalScope.moment = deprecate(
                    'Accessing Moment through the global scope is ' +
                    'deprecated, and will be removed in an upcoming ' +
                    'release.',
                    moment);
        } else {
            globalScope.moment = moment;
        }
    }

    // CommonJS module is defined
    if (hasModule) {
        module.exports = moment;
    } else if (typeof define === 'function' && define.amd) {
        define(function (require, exports, module) {
            if (module.config && module.config() && module.config().noGlobal === true) {
                // release the global variable
                globalScope.moment = oldGlobalMoment;
            }

            return moment;
        });
        makeGlobal(true);
    } else {
        makeGlobal();
    }
}).call(this);
/*! Copyright 2012, Ben Lin (http://dreamerslab.com/)
 * Licensed under the MIT License (LICENSE.txt).
 *
 * Version: 1.0.16
 *
 * Requires: jQuery >= 1.2.3
 */
;( function ( factory ) {
if ( typeof define === 'function' && define.amd ) {
    // AMD. Register module depending on jQuery using requirejs define.
    define( ['jquery'], factory );
} else {
    // No AMD.
    factory( jQuery );
}
}( function ( $ ){
  $.fn.addBack = $.fn.addBack || $.fn.andSelf;

  $.fn.extend({

    actual : function ( method, options ){
      // check if the jQuery method exist
      if( !this[ method ]){
        throw '$.actual => The jQuery method "' + method + '" you called does not exist';
      }

      var defaults = {
        absolute      : false,
        clone         : false,
        includeMargin : false,
        display       : 'block'
      };

      var configs = $.extend( defaults, options );

      var $target = this.eq( 0 );
      var fix, restore;

      if( configs.clone === true ){
        fix = function (){
          var style = 'position: absolute !important; top: -1000 !important; ';

          // this is useful with css3pie
          $target = $target.
            clone().
            attr( 'style', style ).
            appendTo( 'body' );
        };

        restore = function (){
          // remove DOM element after getting the width
          $target.remove();
        };
      }else{
        var tmp   = [];
        var style = '';
        var $hidden;

        fix = function (){
          // get all hidden parents
          $hidden = $target.parents().addBack().filter( ':hidden' );
          style   += 'visibility: hidden !important; display: ' + configs.display + ' !important; ';

          if( configs.absolute === true ) style += 'position: absolute !important; ';

          // save the origin style props
          // set the hidden el css to be got the actual value later
          $hidden.each( function (){
            // Save original style. If no style was set, attr() returns undefined
            var $this     = $( this );
            var thisStyle = $this.attr( 'style' );

            tmp.push( thisStyle );
            // Retain as much of the original style as possible, if there is one
            $this.attr( 'style', thisStyle ? thisStyle + ';' + style : style );
          });
        };

        restore = function (){
          // restore origin style values
          $hidden.each( function ( i ){
            var $this = $( this );
            var _tmp  = tmp[ i ];

            if( _tmp === undefined ){
              $this.removeAttr( 'style' );
            }else{
              $this.attr( 'style', _tmp );
            }
          });
        };
      }

      fix();
      // get the actual value with user specific methed
      // it can be 'width', 'height', 'outerWidth', 'innerWidth'... etc
      // configs.includeMargin only works for 'outerWidth' and 'outerHeight'
      var actual = /(outer)/.test( method ) ?
        $target[ method ]( configs.includeMargin ) :
        $target[ method ]();

      restore();
      // IMPORTANT, this plugin only return the value of the first element
      return actual;
    }
  });
}));
// Ion.RangeSlider | version 2.0.0 | https://github.com/IonDen/ion.rangeSlider
(function(e,r,f,p,t){var s=0,n=function(){var a=p.userAgent,b=/msie\s\d+/i;return 0<a.search(b)&&(a=b.exec(a).toString(),a=a.split(" ")[1],9>a)?(e("html").addClass("lt-ie9"),!0):!1}(),k="ontouchstart"in f||0<p.msMaxTouchPoints,q=function(a,b,c){this.VERSION="2.0.0";this.input=a;this.plugin_count=c;this.old_to=this.old_from=this.calc_count=this.current_plugin=0;this.raf_id=null;this.is_update=this.is_key=this.force_redraw=this.dragging=!1;this.is_start=!0;this.is_active=!1;this.$cache={win:e(f),body:e(r.body),
input:e(a),cont:null,rs:null,min:null,max:null,from:null,to:null,single:null,bar:null,line:null,s_single:null,s_from:null,s_to:null,shad_single:null,shad_from:null,shad_to:null,grid:null,grid_labels:[]};a=this.$cache.input;a={min:a.data("min"),max:a.data("max"),from:a.data("from"),to:a.data("to"),step:a.data("step"),values:a.data("values"),type:a.data("type"),from_fixed:a.data("fromFixed"),from_min:a.data("fromMin"),from_max:a.data("fromMax"),from_shadow:a.data("fromShadow"),to_fixed:a.data("toFixed"),
to_min:a.data("toMin"),to_max:a.data("toMax"),to_shadow:a.data("toShadow"),prettify_enabled:a.data("prettifyEnabled"),prettify_separator:a.data("prettifySeparator"),force_edges:a.data("forceEdges"),keyboard:a.data("keyboard"),keyboard_step:a.data("keyboardStep"),grid:a.data("grid"),grid_margin:a.data("gridMargin"),grid_num:a.data("gridNum"),grid_snap:a.data("gridSnap"),hide_min_max:a.data("hideMinMax"),hide_from_to:a.data("hideFromTo"),prefix:a.data("prefix"),postfix:a.data("postfix"),max_postfix:a.data("maxPostfix"),
decorate_both:a.data("decorateBoth"),values_separator:a.data("valuesSeparator"),disable:a.data("disable")};a.values=a.values&&a.values.split(",");b=e.extend(a,b);this.options=e.extend({min:10,max:100,from:null,to:null,step:1,values:[],p_values:[],type:"single",from_fixed:!1,from_min:null,from_max:null,from_shadow:!1,to_fixed:!1,to_min:null,to_max:null,to_shadow:!1,prettify_enabled:!0,prettify_separator:" ",prettify:null,force_edges:!1,keyboard:!1,keyboard_step:5,grid:!1,grid_margin:!0,grid_num:4,
grid_snap:!1,hide_min_max:!1,hide_from_to:!1,prefix:"",postfix:"",max_postfix:"",decorate_both:!0,values_separator:" \u2014 ",disable:!1,onStart:null,onChange:null,onFinish:null,onUpdate:null},b);this.validate();this.result={input:this.$cache.input,slider:null,min:this.options.min,max:this.options.max,from:this.options.from,from_percent:0,from_value:null,to:this.options.to,to_percent:0,to_value:null};this.coords={x_gap:0,x_pointer:0,w_rs:0,w_rs_old:0,w_handle:0,p_gap:0,p_step:0,p_pointer:0,p_handle:0,
p_single:0,p_single_real:0,p_from:0,p_from_real:0,p_to:0,p_to_real:0,p_bar_x:0,p_bar_w:0,grid_gap:0,big_num:0,big:[],big_w:[],big_p:[],big_x:[]};this.labels={w_min:0,w_max:0,w_from:0,w_to:0,w_single:0,p_min:0,p_max:0,p_from:0,p_from_left:0,p_to:0,p_to_left:0,p_single:0,p_single_left:0};this.init()};q.prototype={init:function(a){this.coords.p_step=this.options.step/((this.options.max-this.options.min)/100);this.target="base";this.toggleInput();this.append();this.setMinMax();if(a){if(this.force_redraw=
!0,this.calc(!0),this.options.onUpdate&&"function"===typeof this.options.onUpdate)this.options.onUpdate(this.result)}else if(this.force_redraw=!0,this.calc(!0),this.options.onStart&&"function"===typeof this.options.onStart)this.options.onStart(this.result);this.raf_id=requestAnimationFrame(this.updateScene.bind(this))},append:function(){this.$cache.input.before('<span class="irs js-irs-'+this.plugin_count+'"></span>');this.$cache.cont=this.$cache.input.prev();this.result.slider=this.$cache.cont;this.$cache.cont.html('<span class="irs"><span class="irs-line"><span class="irs-line-left"></span><span class="irs-line-mid"></span><span class="irs-line-right"></span></span><span class="irs-min">0</span><span class="irs-max">1</span><span class="irs-from">0</span><span class="irs-to">0</span><span class="irs-single">0</span></span><span class="irs-grid"></span><span class="irs-bar"></span>');
this.$cache.rs=this.$cache.cont.find(".irs");this.$cache.min=this.$cache.cont.find(".irs-min");this.$cache.max=this.$cache.cont.find(".irs-max");this.$cache.from=this.$cache.cont.find(".irs-from");this.$cache.to=this.$cache.cont.find(".irs-to");this.$cache.single=this.$cache.cont.find(".irs-single");this.$cache.bar=this.$cache.cont.find(".irs-bar");this.$cache.line=this.$cache.cont.find(".irs-line");this.$cache.grid=this.$cache.cont.find(".irs-grid");"single"===this.options.type?(this.$cache.cont.append('<span class="irs-bar-edge"></span><span class="irs-shadow shadow-single"></span><span class="irs-slider single"></span>'),
this.$cache.s_single=this.$cache.cont.find(".single"),this.$cache.from[0].style.visibility="hidden",this.$cache.to[0].style.visibility="hidden",this.$cache.shad_single=this.$cache.cont.find(".shadow-single")):(this.$cache.cont.append('<span class="irs-shadow shadow-from"></span><span class="irs-shadow shadow-to"></span><span class="irs-slider from"></span><span class="irs-slider to"></span>'),this.$cache.s_from=this.$cache.cont.find(".from"),this.$cache.s_to=this.$cache.cont.find(".to"),this.$cache.shad_from=
this.$cache.cont.find(".shadow-from"),this.$cache.shad_to=this.$cache.cont.find(".shadow-to"));this.options.hide_from_to&&(this.$cache.from[0].style.display="none",this.$cache.to[0].style.display="none",this.$cache.single[0].style.display="none");this.appendGrid();this.options.disable?this.appendDisableMask():(this.$cache.cont.removeClass("irs-disabled"),this.bindEvents())},appendDisableMask:function(){this.$cache.cont.append('<span class="irs-disable-mask"></span>');this.$cache.cont.addClass("irs-disabled")},
remove:function(){this.$cache.cont.remove();this.$cache.cont=null;this.$cache.input.off("keydown.irs_"+this.plugin_count);k?(this.$cache.body.off("touchmove.irs_"+this.plugin_count),this.$cache.win.off("touchend.irs_"+this.plugin_count)):(this.$cache.body.off("mousemove.irs_"+this.plugin_count),this.$cache.win.off("mouseup.irs_"+this.plugin_count),n&&(this.$cache.body.off("mouseup.irs_"+this.plugin_count),this.$cache.body.off("mouseleave.irs_"+this.plugin_count)));this.$cache.grid_labels=[];this.coords.big=
[];this.coords.big_w=[];this.coords.big_p=[];this.coords.big_x=[];cancelAnimationFrame(this.raf_id)},bindEvents:function(){if(k)this.$cache.body.on("touchmove.irs_"+this.plugin_count,this.pointerMove.bind(this)),this.$cache.win.on("touchend.irs_"+this.plugin_count,this.pointerUp.bind(this)),this.$cache.line.on("touchstart.irs_"+this.plugin_count,this.pointerClick.bind(this,"click")),this.$cache.bar.on("touchstart.irs_"+this.plugin_count,this.pointerClick.bind(this,"click")),"single"===this.options.type?
(this.$cache.s_single.on("touchstart.irs_"+this.plugin_count,this.pointerDown.bind(this,"single")),this.$cache.shad_single.on("touchstart.irs_"+this.plugin_count,this.pointerClick.bind(this,"click"))):(this.$cache.s_from.on("touchstart.irs_"+this.plugin_count,this.pointerDown.bind(this,"from")),this.$cache.s_to.on("touchstart.irs_"+this.plugin_count,this.pointerDown.bind(this,"to")),this.$cache.shad_from.on("touchstart.irs_"+this.plugin_count,this.pointerClick.bind(this,"click")),this.$cache.shad_to.on("touchstart.irs_"+
this.plugin_count,this.pointerClick.bind(this,"click")));else{if(this.options.keyboard)this.$cache.input.on("keydown.irs_"+this.plugin_count,this.key.bind(this,"keyboard"));this.$cache.body.on("mousemove.irs_"+this.plugin_count,this.pointerMove.bind(this));this.$cache.win.on("mouseup.irs_"+this.plugin_count,this.pointerUp.bind(this));n&&(this.$cache.body.on("mouseup.irs_"+this.plugin_count,this.pointerUp.bind(this)),this.$cache.body.on("mouseleave.irs_"+this.plugin_count,this.pointerUp.bind(this)));
this.$cache.line.on("mousedown.irs_"+this.plugin_count,this.pointerClick.bind(this,"click"));this.$cache.bar.on("mousedown.irs_"+this.plugin_count,this.pointerClick.bind(this,"click"));"single"===this.options.type?(this.$cache.s_single.on("mousedown.irs_"+this.plugin_count,this.pointerDown.bind(this,"single")),this.$cache.shad_single.on("mousedown.irs_"+this.plugin_count,this.pointerClick.bind(this,"click"))):(this.$cache.s_from.on("mousedown.irs_"+this.plugin_count,this.pointerDown.bind(this,"from")),
this.$cache.s_to.on("mousedown.irs_"+this.plugin_count,this.pointerDown.bind(this,"to")),this.$cache.shad_from.on("mousedown.irs_"+this.plugin_count,this.pointerClick.bind(this,"click")),this.$cache.shad_to.on("mousedown.irs_"+this.plugin_count,this.pointerClick.bind(this,"click")))}},pointerMove:function(a){this.dragging&&(this.coords.x_pointer=(k?a.originalEvent.touches[0]:a).pageX-this.coords.x_gap,this.calc())},pointerUp:function(a){if(this.current_plugin===this.plugin_count&&this.is_active){this.is_active=
!1;var b=this.options.onFinish&&"function"===typeof this.options.onFinish;a=e.contains(this.$cache.cont[0],a.target)||this.dragging;if(b&&a)this.options.onFinish(this.result);this.force_redraw=!0;this.dragging=!1;n&&e("*").prop("unselectable",!1)}},pointerDown:function(a,b){b.preventDefault();var c=k?b.originalEvent.touches[0]:b;if(2!==b.button){this.current_plugin=this.plugin_count;this.target=a;this.dragging=this.is_active=!0;this.coords.x_gap=this.$cache.rs.offset().left;this.coords.x_pointer=
c.pageX-this.coords.x_gap;this.calcPointer();switch(a){case "single":this.coords.p_gap=this.toFixed(this.coords.p_pointer-this.coords.p_single);break;case "from":this.coords.p_gap=this.toFixed(this.coords.p_pointer-this.coords.p_from);this.$cache.s_from.addClass("type_last");this.$cache.s_to.removeClass("type_last");break;case "to":this.coords.p_gap=this.toFixed(this.coords.p_pointer-this.coords.p_to),this.$cache.s_to.addClass("type_last"),this.$cache.s_from.removeClass("type_last")}n&&e("*").prop("unselectable",
!0);this.$cache.input.trigger("focus")}},pointerClick:function(a,b){b.preventDefault();var c=k?b.originalEvent.touches[0]:b;2!==b.button&&(this.current_plugin=this.plugin_count,this.target=a,this.coords.x_gap=this.$cache.rs.offset().left,this.coords.x_pointer=+(c.pageX-this.coords.x_gap).toFixed(),this.force_redraw=!0,this.calc(),this.$cache.input.trigger("focus"))},key:function(a,b){if(!(this.current_plugin!==this.plugin_count||b.altKey||b.ctrlKey||b.shiftKey||b.metaKey)){switch(b.which){case 83:case 65:case 40:case 37:b.preventDefault();
this.moveByKey(!1);break;case 87:case 68:case 38:case 39:b.preventDefault(),this.moveByKey(!0)}return!0}},moveByKey:function(a){var b=this.coords.p_pointer,b=a?b+this.options.keyboard_step:b-this.options.keyboard_step;this.coords.x_pointer=this.toFixed(this.coords.w_rs/100*b);this.is_key=!0;this.calc()},setMinMax:function(){this.options.hide_min_max?(this.$cache.min[0].style.display="none",this.$cache.max[0].style.display="none"):(this.options.values.length?(this.$cache.min.html(this.decorate(this.options.p_values[this.options.min])),
this.$cache.max.html(this.decorate(this.options.p_values[this.options.max]))):(this.$cache.min.html(this.decorate(this._prettify(this.options.min),this.options.min)),this.$cache.max.html(this.decorate(this._prettify(this.options.max),this.options.max))),this.labels.w_min=this.$cache.min.outerWidth(!1),this.labels.w_max=this.$cache.max.outerWidth(!1))},calc:function(a){this.calc_count++;if(10===this.calc_count||a)this.calc_count=0,this.coords.w_rs=this.$cache.rs.outerWidth(!1),this.coords.w_handle=
"single"===this.options.type?this.$cache.s_single.outerWidth(!1):this.$cache.s_from.outerWidth(!1);if(this.coords.w_rs){this.calcPointer();this.coords.p_handle=this.toFixed(this.coords.w_handle/this.coords.w_rs*100);a=100-this.coords.p_handle;var b=this.toFixed(this.coords.p_pointer-this.coords.p_gap);"click"===this.target&&(b=this.toFixed(this.coords.p_pointer-this.coords.p_handle/2),this.target=this.chooseHandle(b));0>b?b=0:b>a&&(b=a);switch(this.target){case "base":b=(this.options.max-this.options.min)/
100;a=(this.result.from-this.options.min)/b;b=(this.result.to-this.options.min)/b;this.coords.p_single_real=this.toFixed(a);this.coords.p_from_real=this.toFixed(a);this.coords.p_to_real=this.toFixed(b);this.coords.p_single_real=this.checkDiapason(this.coords.p_single_real,this.options.from_min,this.options.from_max);this.coords.p_from_real=this.checkDiapason(this.coords.p_from_real,this.options.from_min,this.options.from_max);this.coords.p_to_real=this.checkDiapason(this.coords.p_to_real,this.options.to_min,
this.options.to_max);this.coords.p_single=this.toFixed(a-this.coords.p_handle/100*a);this.coords.p_from=this.toFixed(a-this.coords.p_handle/100*a);this.coords.p_to=this.toFixed(b-this.coords.p_handle/100*b);this.target=null;break;case "single":if(this.options.from_fixed)break;this.coords.p_single_real=this.calcWithStep(b/a*100);this.coords.p_single_real=this.checkDiapason(this.coords.p_single_real,this.options.from_min,this.options.from_max);this.coords.p_single=this.toFixed(this.coords.p_single_real/
100*a);break;case "from":if(this.options.from_fixed)break;this.coords.p_from_real=this.calcWithStep(b/a*100);this.coords.p_from_real>this.coords.p_to_real&&(this.coords.p_from_real=this.coords.p_to_real);this.coords.p_from_real=this.checkDiapason(this.coords.p_from_real,this.options.from_min,this.options.from_max);this.coords.p_from=this.toFixed(this.coords.p_from_real/100*a);break;case "to":if(this.options.to_fixed)break;this.coords.p_to_real=this.calcWithStep(b/a*100);this.coords.p_to_real<this.coords.p_from_real&&
(this.coords.p_to_real=this.coords.p_from_real);this.coords.p_to_real=this.checkDiapason(this.coords.p_to_real,this.options.to_min,this.options.to_max);this.coords.p_to=this.toFixed(this.coords.p_to_real/100*a)}"single"===this.options.type?(this.coords.p_bar_x=this.coords.p_handle/2,this.coords.p_bar_w=this.coords.p_single,this.result.from_percent=this.coords.p_single_real,this.result.from=this.calcReal(this.coords.p_single_real),this.options.values.length&&(this.result.from_value=this.options.values[this.result.from])):
(this.coords.p_bar_x=this.toFixed(this.coords.p_from+this.coords.p_handle/2),this.coords.p_bar_w=this.toFixed(this.coords.p_to-this.coords.p_from),this.result.from_percent=this.coords.p_from_real,this.result.from=this.calcReal(this.coords.p_from_real),this.result.to_percent=this.coords.p_to_real,this.result.to=this.calcReal(this.coords.p_to_real),this.options.values.length&&(this.result.from_value=this.options.values[this.result.from],this.result.to_value=this.options.values[this.result.to]));this.calcMinMax();
this.calcLabels()}},calcPointer:function(){this.coords.w_rs?(0>this.coords.x_pointer?this.coords.x_pointer=0:this.coords.x_pointer>this.coords.w_rs&&(this.coords.x_pointer=this.coords.w_rs),this.coords.p_pointer=this.toFixed(this.coords.x_pointer/this.coords.w_rs*100)):this.coords.p_pointer=0},chooseHandle:function(a){return"single"===this.options.type?"single":a>=this.coords.p_from_real+(this.coords.p_to_real-this.coords.p_from_real)/2?"to":"from"},calcMinMax:function(){this.coords.w_rs&&(this.labels.p_min=
this.labels.w_min/this.coords.w_rs*100,this.labels.p_max=this.labels.w_max/this.coords.w_rs*100)},calcLabels:function(){this.coords.w_rs&&!this.options.hide_from_to&&("single"===this.options.type?(this.labels.w_single=this.$cache.single.outerWidth(!1),this.labels.p_single=this.labels.w_single/this.coords.w_rs*100,this.labels.p_single_left=this.coords.p_single+this.coords.p_handle/2-this.labels.p_single/2):(this.labels.w_from=this.$cache.from.outerWidth(!1),this.labels.p_from=this.labels.w_from/this.coords.w_rs*
100,this.labels.p_from_left=this.coords.p_from+this.coords.p_handle/2-this.labels.p_from/2,this.labels.p_from_left=this.toFixed(this.labels.p_from_left),this.labels.p_from_left=this.checkEdges(this.labels.p_from_left,this.labels.p_from),this.labels.w_to=this.$cache.to.outerWidth(!1),this.labels.p_to=this.labels.w_to/this.coords.w_rs*100,this.labels.p_to_left=this.coords.p_to+this.coords.p_handle/2-this.labels.p_to/2,this.labels.p_to_left=this.toFixed(this.labels.p_to_left),this.labels.p_to_left=this.checkEdges(this.labels.p_to_left,
this.labels.p_to),this.labels.w_single=this.$cache.single.outerWidth(!1),this.labels.p_single=this.labels.w_single/this.coords.w_rs*100,this.labels.p_single_left=(this.labels.p_from_left+this.labels.p_to_left+this.labels.p_to)/2-this.labels.p_single/2,this.labels.p_single_left=this.toFixed(this.labels.p_single_left)),this.labels.p_single_left=this.checkEdges(this.labels.p_single_left,this.labels.p_single))},updateScene:function(){this.drawHandles();this.raf_id=requestAnimationFrame(this.updateScene.bind(this))},
drawHandles:function(){this.coords.w_rs=this.$cache.rs.outerWidth(!1);this.coords.w_rs!==this.coords.w_rs_old&&(this.target="base");if(this.coords.w_rs!==this.coords.w_rs_old||this.force_redraw)this.setMinMax(),this.calc(!0),this.drawLabels(),this.options.grid&&(this.calcGridMargin(),this.calcGridLabels()),this.force_redraw=!0,this.coords.w_rs_old=this.coords.w_rs,this.drawShadow();if(this.coords.w_rs&&(this.dragging||this.force_redraw||this.is_key)){if(this.old_from!==this.result.from||this.old_to!==
this.result.to||this.force_redraw||this.is_key){this.drawLabels();this.$cache.bar[0].style.left=this.coords.p_bar_x+"%";this.$cache.bar[0].style.width=this.coords.p_bar_w+"%";if("single"===this.options.type)this.$cache.s_single[0].style.left=this.coords.p_single+"%",this.$cache.single[0].style.left=this.labels.p_single_left+"%",this.options.values.length?(this.$cache.input.prop("value",this.result.from_value),this.$cache.input.data("from",this.result.from_value)):(this.$cache.input.prop("value",this.result.from),
this.$cache.input.data("from",this.result.from));else{this.$cache.s_from[0].style.left=this.coords.p_from+"%";this.$cache.s_to[0].style.left=this.coords.p_to+"%";if(this.old_from!==this.result.from||this.force_redraw)this.$cache.from[0].style.left=this.labels.p_from_left+"%";if(this.old_to!==this.result.to||this.force_redraw)this.$cache.to[0].style.left=this.labels.p_to_left+"%";this.$cache.single[0].style.left=this.labels.p_single_left+"%";this.options.values.length?(this.$cache.input.prop("value",
this.result.from_value+";"+this.result.to_value),this.$cache.input.data("from",this.result.from_value),this.$cache.input.data("to",this.result.to_value)):(this.$cache.input.prop("value",this.result.from+";"+this.result.to),this.$cache.input.data("from",this.result.from),this.$cache.input.data("to",this.result.to))}this.$cache.input.trigger("change");this.old_from=this.result.from;this.old_to=this.result.to;if(this.options.onChange&&"function"===typeof this.options.onChange&&!this.is_update&&!this.is_start)this.options.onChange(this.result);
this.is_update=!1}this.force_redraw=this.is_key=this.is_start=!1}},drawLabels:function(){var a=this.options.values.length,b=this.options.p_values,c;if(!this.options.hide_from_to)if("single"===this.options.type)a=a?this.decorate(b[this.result.from]):this.decorate(this._prettify(this.result.from),this.result.from),this.$cache.single.html(a),this.calcLabels(),this.$cache.min[0].style.visibility=this.labels.p_single_left<this.labels.p_min+1?"hidden":"visible",this.$cache.max[0].style.visibility=this.labels.p_single_left+
this.labels.p_single>100-this.labels.p_max-1?"hidden":"visible";else{a?(this.options.decorate_both?(a=this.decorate(b[this.result.from]),a+=this.options.values_separator,a+=this.decorate(b[this.result.to])):a=this.decorate(b[this.result.from]+this.options.values_separator+b[this.result.to]),c=this.decorate(b[this.result.from]),b=this.decorate(b[this.result.to])):(this.options.decorate_both?(a=this.decorate(this._prettify(this.result.from)),a+=this.options.values_separator,a+=this.decorate(this._prettify(this.result.to))):
a=this.decorate(this._prettify(this.result.from)+this.options.values_separator+this._prettify(this.result.to),this.result.from),c=this.decorate(this._prettify(this.result.from),this.result.from),b=this.decorate(this._prettify(this.result.to),this.result.to));this.$cache.single.html(a);this.$cache.from.html(c);this.$cache.to.html(b);this.calcLabels();b=Math.min(this.labels.p_single_left,this.labels.p_from_left);a=this.labels.p_single_left+this.labels.p_single;c=this.labels.p_to_left+this.labels.p_to;
var d=Math.max(a,c);this.labels.p_from_left+this.labels.p_from>=this.labels.p_to_left?(this.$cache.from[0].style.visibility="hidden",this.$cache.to[0].style.visibility="hidden",this.$cache.single[0].style.visibility="visible",this.result.from===this.result.to?(this.$cache.from[0].style.visibility="visible",this.$cache.single[0].style.visibility="hidden",d=c):(this.$cache.from[0].style.visibility="hidden",this.$cache.single[0].style.visibility="visible",d=Math.max(a,c))):(this.$cache.from[0].style.visibility=
"visible",this.$cache.to[0].style.visibility="visible",this.$cache.single[0].style.visibility="hidden");this.$cache.min[0].style.visibility=b<this.labels.p_min+1?"hidden":"visible";this.$cache.max[0].style.visibility=d>100-this.labels.p_max-1?"hidden":"visible"}},drawShadow:function(){var a=this.options,b=this.$cache,c,d;"single"===a.type?a.from_shadow&&(a.from_min||a.from_max)?(c=this.calcPercent(a.from_min||a.min),d=this.calcPercent(a.from_max||a.max)-c,c=this.toFixed(c-this.coords.p_handle/100*
c),d=this.toFixed(d-this.coords.p_handle/100*d),c+=this.coords.p_handle/2,b.shad_single[0].style.display="block",b.shad_single[0].style.left=c+"%",b.shad_single[0].style.width=d+"%"):b.shad_single[0].style.display="none":(a.from_shadow&&(a.from_min||a.from_max)?(c=this.calcPercent(a.from_min||a.min),d=this.calcPercent(a.from_max||a.max)-c,c=this.toFixed(c-this.coords.p_handle/100*c),d=this.toFixed(d-this.coords.p_handle/100*d),c+=this.coords.p_handle/2,b.shad_from[0].style.display="block",b.shad_from[0].style.left=
c+"%",b.shad_from[0].style.width=d+"%"):b.shad_from[0].style.display="none",a.to_shadow&&(a.to_min||a.to_max)?(c=this.calcPercent(a.to_min||a.min),a=this.calcPercent(a.to_max||a.max)-c,c=this.toFixed(c-this.coords.p_handle/100*c),a=this.toFixed(a-this.coords.p_handle/100*a),c+=this.coords.p_handle/2,b.shad_to[0].style.display="block",b.shad_to[0].style.left=c+"%",b.shad_to[0].style.width=a+"%"):b.shad_to[0].style.display="none")},toggleInput:function(){this.$cache.input.toggleClass("irs-hidden-input")},
calcPercent:function(a){return this.toFixed((a-this.options.min)/((this.options.max-this.options.min)/100))},calcReal:function(a){var b=this.options.min,c=this.options.max,d=0;0>b&&(d=Math.abs(b),b+=d,c+=d);a=(c-b)/100*a+b;(b=this.options.step.toString().split(".")[1])?a=+a.toFixed(b.length):(a/=this.options.step,a*=this.options.step,a=+a.toFixed(0));d&&(a-=d);a<this.options.min?a=this.options.min:a>this.options.max&&(a=this.options.max);return b?+a.toFixed(b.length):this.toFixed(a)},calcWithStep:function(a){var b=
Math.round(a/this.coords.p_step)*this.coords.p_step;100<b&&(b=100);100===a&&(b=100);return this.toFixed(b)},checkDiapason:function(a,b,c){if(!b&&!c)return a;a=this.calcReal(a);"number"===typeof b&&a<b&&(a=b);"number"===typeof c&&a>c&&(a=c);return this.calcPercent(a)},toFixed:function(a){a=a.toFixed(5);return+a},_prettify:function(a){return this.options.prettify_enabled?this.options.prettify&&"function"===typeof this.options.prettify?this.options.prettify(a):this.prettify(a):a},prettify:function(a){return a.toString().replace(/(\d{1,3}(?=(?:\d\d\d)+(?!\d)))/g,
"$1"+this.options.prettify_separator)},checkEdges:function(a,b){if(!this.options.force_edges)return this.toFixed(a);0>a?a=0:a>100-b&&(a=100-b);return this.toFixed(a)},validate:function(){var a=this.options,b=this.result,c=a.values,d=a.p_values,e=c.length,g,h;a.max<=a.min&&(a.max=a.min?2*a.min:a.min+1,a.step=1);if(e)for(a.min=0,a.max=e-1,a.step=1,a.grid_num=a.max,a.grid_snap=!0,h=0;h<e;h++)g=+c[h],isNaN(g)?g=c[h]:(c[h]=g,g=this._prettify(g)),d.push(g);if("number"!==typeof a.from||isNaN(a.from))a.from=
a.min;if("number"!==typeof a.to||isNaN(a.from))a.to=a.max;if(a.from<a.min||a.from>a.max)a.from=a.min;if(a.to>a.max||a.to<a.min)a.to=a.max;a.from>a.to&&(a.from=a.to);if("number"!==typeof a.step||isNaN(a.step)||!a.step||0>a.step)a.step=1;if("number"!==typeof a.keyboard_step||isNaN(a.keyboard_step)||!a.keyboard_step||0>a.keyboard_step)a.keyboard_step=5;a.from_min&&a.from<a.from_min&&(a.from=a.from_min);a.from_max&&a.from>a.from_max&&(a.from=a.from_max);a.to_min&&a.to<a.to_min&&(a.to=a.to_min);a.to_max&&
a.from>a.to_max&&(a.to=a.to_max);if(b){b.min!==a.min&&(b.min=a.min);b.max!==a.max&&(b.max=a.max);if(b.from<b.min||b.from>b.max)b.from=a.from;if(b.to<b.min||b.to>b.max)b.to=a.to}},decorate:function(a,b){var c="",d=this.options;d.prefix&&(c+=d.prefix);c+=a;d.max_postfix&&(d.values.length&&a===d.p_values[d.max]?(c+=d.max_postfix,d.postfix&&(c+=" ")):b===d.max&&(c+=d.max_postfix,d.postfix&&(c+=" ")));d.postfix&&(c+=d.postfix);return c},updateFrom:function(){this.result.from=this.options.from;this.result.from_percent=
this.calcPercent(this.result.from);this.options.values&&(this.result.from_value=this.options.values[this.result.from])},updateTo:function(){this.result.to=this.options.to;this.result.to_percent=this.calcPercent(this.result.to);this.options.values&&(this.result.to_value=this.options.values[this.result.to])},updateResult:function(a){this.result.min=this.options.min;this.result.max=this.options.max;a?(a.from&&this.updateFrom(),a.to&&this.updateTo()):(this.updateFrom(),this.updateTo())},appendGrid:function(){if(this.options.grid){var a=
this.options,b,c;b=a.max-a.min;var d=a.grid_num,e=0,g=0,h=4,f,k,l=0,m="";this.calcGridMargin();a.grid_snap?(d=b/a.step,e=this.toFixed(a.step/(b/100))):e=this.toFixed(100/d);4<d&&(h=3);7<d&&(h=2);14<d&&(h=1);28<d&&(h=0);for(b=0;b<d+1;b++){f=h;g=this.toFixed(e*b);100<g&&(g=100,f-=2,0>f&&(f=0));this.coords.big[b]=g;k=(g-e*(b-1))/(f+1);for(c=1;c<=f&&0!==g;c++)l=this.toFixed(g-k*c),m+='<span class="irs-grid-pol small" style="left: '+l+'%"></span>';m+='<span class="irs-grid-pol" style="left: '+g+'%"></span>';
l=this.calcReal(g);l=a.values.length?a.p_values[l]:this._prettify(l);m+='<span class="irs-grid-text js-grid-text-'+b+'" style="left: '+g+'%">'+l+"</span>"}this.coords.big_num=Math.ceil(d+1);this.$cache.cont.addClass("irs-with-grid");this.$cache.grid.html(m);this.cacheGridLabels()}},cacheGridLabels:function(){var a,b,c=this.coords.big_num;for(b=0;b<c;b++)a=this.$cache.grid.find(".js-grid-text-"+b),this.$cache.grid_labels.push(a);this.calcGridLabels()},calcGridLabels:function(){var a,b;b=[];var c=[],
d=this.coords.big_num;for(a=0;a<d;a++)this.coords.big_w[a]=this.$cache.grid_labels[a].outerWidth(!1),this.coords.big_p[a]=this.toFixed(this.coords.big_w[a]/this.coords.w_rs*100),this.coords.big_x[a]=this.toFixed(this.coords.big_p[a]/2),b[a]=this.toFixed(this.coords.big[a]-this.coords.big_x[a]),c[a]=this.toFixed(b[a]+this.coords.big_p[a]);this.options.force_edges&&(b[0]<this.coords.grid_gap&&(b[0]=this.coords.grid_gap,c[0]=this.toFixed(b[0]+this.coords.big_p[0]),this.coords.big_x[0]=this.coords.grid_gap),
c[d-1]>100-this.coords.grid_gap&&(c[d-1]=100-this.coords.grid_gap,b[d-1]=this.toFixed(c[d-1]-this.coords.big_p[d-1]),this.coords.big_x[d-1]=this.toFixed(this.coords.big_p[d-1]-this.coords.grid_gap)));this.calcGridCollision(2,b,c);this.calcGridCollision(4,b,c);for(a=0;a<d;a++)b=this.$cache.grid_labels[a][0],b.style.marginLeft=-this.coords.big_x[a]+"%"},calcGridCollision:function(a,b,c){var d,e,g,f=this.coords.big_num;for(d=0;d<f;d+=a){e=d+a/2;if(e>=f)break;g=this.$cache.grid_labels[e][0];g.style.visibility=
c[d]<=b[e]?"visible":"hidden"}},calcGridMargin:function(){this.options.grid_margin&&(this.coords.w_rs=this.$cache.rs.outerWidth(!1),this.coords.w_rs&&(this.coords.w_handle="single"===this.options.type?this.$cache.s_single.outerWidth(!1):this.$cache.s_from.outerWidth(!1),this.coords.p_handle=this.toFixed(this.coords.w_handle/this.coords.w_rs*100),this.coords.grid_gap=this.toFixed(this.coords.p_handle/2-.1),this.$cache.grid[0].style.width=this.toFixed(100-this.coords.p_handle)+"%",this.$cache.grid[0].style.left=
this.coords.grid_gap+"%"))},update:function(a){this.is_update=!0;this.options=e.extend(this.options,a);this.validate();this.updateResult(a);this.toggleInput();this.remove();this.init(!0)},reset:function(){this.updateResult();this.update()},destroy:function(){this.toggleInput();e.data(this.input,"ionRangeSlider",null);this.remove();this.options=this.input=null}};e.fn.ionRangeSlider=function(a){return this.each(function(){e.data(this,"ionRangeSlider")||e.data(this,"ionRangeSlider",new q(this,a,s++))})};
(function(){for(var a=0,b=["ms","moz","webkit","o"],c=0;c<b.length&&!f.requestAnimationFrame;++c)f.requestAnimationFrame=f[b[c]+"RequestAnimationFrame"],f.cancelAnimationFrame=f[b[c]+"CancelAnimationFrame"]||f[b[c]+"CancelRequestAnimationFrame"];f.requestAnimationFrame||(f.requestAnimationFrame=function(b,c){var e=(new Date).getTime(),h=Math.max(0,16-(e-a)),k=f.setTimeout(function(){b(e+h)},h);a=e+h;return k});f.cancelAnimationFrame||(f.cancelAnimationFrame=function(a){clearTimeout(a)})})()})(jQuery,
document,window,navigator);
/**!
 * easyPieChart
 * Lightweight plugin to render simple, animated and retina optimized pie charts
 *
 * @license Dual licensed under the MIT (http://www.opensource.org/licenses/mit-license.php) and GPL (http://www.opensource.org/licenses/gpl-license.php) licenses.
 * @author Robert Fleischmann <rendro87@gmail.com> (http://robert-fleischmann.de)
 * @version 2.1.3
 **/

(function(root, factory) {
    if(typeof exports === 'object') {
        module.exports = factory(require('jquery'));
    }
    else if(typeof define === 'function' && define.amd) {
        define('EasyPieChart', ['jquery'], factory);
    }
    else {
        factory(root.jQuery);
    }
}(this, function($) {
/**
 * Renderer to render the chart on a canvas object
 * @param {DOMElement} el      DOM element to host the canvas (root of the plugin)
 * @param {object}     options options object of the plugin
 */
var CanvasRenderer = function(el, options) {
	var cachedBackground;
	var canvas = document.createElement('canvas');

	if (typeof(G_vmlCanvasManager) !== 'undefined') {
		G_vmlCanvasManager.initElement(canvas);
	}

	var ctx = canvas.getContext('2d');

	canvas.width = canvas.height = options.size;

	el.appendChild(canvas);

	// canvas on retina devices
	var scaleBy = 1;
	if (window.devicePixelRatio > 1) {
		scaleBy = window.devicePixelRatio;
		canvas.style.width = canvas.style.height = [options.size, 'px'].join('');
		canvas.width = canvas.height = options.size * scaleBy;
		ctx.scale(scaleBy, scaleBy);
	}

	// move 0,0 coordinates to the center
	ctx.translate(options.size / 2, options.size / 2);

	// rotate canvas -90deg
	ctx.rotate((-1 / 2 + options.rotate / 180) * Math.PI);

	var radius = (options.size - options.lineWidth) / 2;
	if (options.scaleColor && options.scaleLength) {
		radius -= options.scaleLength + 2; // 2 is the distance between scale and bar
	}

	// IE polyfill for Date
	Date.now = Date.now || function() {
		return +(new Date());
	};

	/**
	 * Draw a circle around the center of the canvas
	 * @param {strong} color     Valid CSS color string
	 * @param {number} lineWidth Width of the line in px
	 * @param {number} percent   Percentage to draw (float between -1 and 1)
	 */
	var drawCircle = function(color, lineWidth, percent) {
		percent = Math.min(Math.max(-1, percent || 0), 1);
		var isNegative = percent <= 0 ? true : false;

		ctx.beginPath();
		ctx.arc(0, 0, radius, 0, Math.PI * 2 * percent, isNegative);

		ctx.strokeStyle = color;
		ctx.lineWidth = lineWidth;

		ctx.stroke();
	};

	/**
	 * Draw the scale of the chart
	 */
	var drawScale = function() {
		var offset;
		var length;
		var i = 24;

		ctx.lineWidth = 1
		ctx.fillStyle = options.scaleColor;

		ctx.save();
		for (var i = 24; i > 0; --i) {
			if (i%6 === 0) {
				length = options.scaleLength;
				offset = 0;
			} else {
				length = options.scaleLength * .6;
				offset = options.scaleLength - length;
			}
			ctx.fillRect(-options.size/2 + offset, 0, length, 1);
			ctx.rotate(Math.PI / 12);
		}
		ctx.restore();
	};

	/**
	 * Request animation frame wrapper with polyfill
	 * @return {function} Request animation frame method or timeout fallback
	 */
	var reqAnimationFrame = (function() {
		return  window.requestAnimationFrame ||
				window.webkitRequestAnimationFrame ||
				window.mozRequestAnimationFrame ||
				function(callback) {
					window.setTimeout(callback, 1000 / 60);
				};
	}());

	/**
	 * Draw the background of the plugin including the scale and the track
	 */
	var drawBackground = function() {
		options.scaleColor && drawScale();
		options.trackColor && drawCircle(options.trackColor, options.lineWidth, 1);
	};

	/**
	 * Clear the complete canvas
	 */
	this.clear = function() {
		ctx.clearRect(options.size / -2, options.size / -2, options.size, options.size);
	};

	/**
	 * Draw the complete chart
	 * @param {number} percent Percent shown by the chart between -100 and 100
	 */
	this.draw = function(percent) {
		// do we need to render a background
		if (!!options.scaleColor || !!options.trackColor) {
			// getImageData and putImageData are supported
			if (ctx.getImageData && ctx.putImageData) {
				if (!cachedBackground) {
					drawBackground();
					cachedBackground = ctx.getImageData(0, 0, options.size * scaleBy, options.size * scaleBy);
				} else {
					ctx.putImageData(cachedBackground, 0, 0);
				}
			} else {
				this.clear();
				drawBackground();
			}
		} else {
			this.clear();
		}

		ctx.lineCap = options.lineCap;

		// if barcolor is a function execute it and pass the percent as a value
		var color;
		if (typeof(options.barColor) === 'function') {
			color = options.barColor(percent);
		} else {
			color = options.barColor;
		}

		// draw bar
		drawCircle(color, options.lineWidth, percent / 100);
	}.bind(this);

	/**
	 * Animate from some percent to some other percentage
	 * @param {number} from Starting percentage
	 * @param {number} to   Final percentage
	 */
	this.animate = function(from, to) {
		var startTime = Date.now();
		options.onStart(from, to);
		var animation = function() {
			var process = Math.min(Date.now() - startTime, options.animate);
			var currentValue = options.easing(this, process, from, to - from, options.animate);
			this.draw(currentValue);
			options.onStep(from, to, currentValue);
			if (process >= options.animate) {
				options.onStop(from, to);
			} else {
				reqAnimationFrame(animation);
			}
		}.bind(this);

		reqAnimationFrame(animation);
	}.bind(this);
};

var EasyPieChart = function(el, opts) {
	var defaultOptions = {
		barColor: '#ef1e25',
		trackColor: '#f9f9f9',
		scaleColor: '#dfe0e0',
		scaleLength: 5,
		lineCap: 'round',
		lineWidth: 3,
		size: 110,
		rotate: 0,
		animate: 1000,
		easing: function (x, t, b, c, d) { // more can be found here: http://gsgd.co.uk/sandbox/jquery/easing/
			t = t / (d/2);
			if (t < 1) {
				return c / 2 * t * t + b;
			}
			return -c/2 * ((--t)*(t-2) - 1) + b;
		},
		onStart: function(from, to) {
			return;
		},
		onStep: function(from, to, currentValue) {
			return;
		},
		onStop: function(from, to) {
			return;
		}
	};

	// detect present renderer
	if (typeof(CanvasRenderer) !== 'undefined') {
		defaultOptions.renderer = CanvasRenderer;
	} else if (typeof(SVGRenderer) !== 'undefined') {
		defaultOptions.renderer = SVGRenderer;
	} else {
		throw new Error('Please load either the SVG- or the CanvasRenderer');
	}

	var options = {};
	var currentValue = 0;

	/**
	 * Initialize the plugin by creating the options object and initialize rendering
	 */
	var init = function() {
		this.el = el;
		this.options = options;

		// merge user options into default options
		for (var i in defaultOptions) {
			if (defaultOptions.hasOwnProperty(i)) {
				options[i] = opts && typeof(opts[i]) !== 'undefined' ? opts[i] : defaultOptions[i];
				if (typeof(options[i]) === 'function') {
					options[i] = options[i].bind(this);
				}
			}
		}

		// check for jQuery easing
		if (typeof(options.easing) === 'string' && typeof(jQuery) !== 'undefined' && jQuery.isFunction(jQuery.easing[options.easing])) {
			options.easing = jQuery.easing[options.easing];
		} else {
			options.easing = defaultOptions.easing;
		}

		// create renderer
		this.renderer = new options.renderer(el, options);

		// initial draw
		this.renderer.draw(currentValue);

		// initial update
		if (el.dataset && el.dataset.percent) {
			this.update(parseFloat(el.dataset.percent));
		} else if (el.getAttribute && el.getAttribute('data-percent')) {
			this.update(parseFloat(el.getAttribute('data-percent')));
		}
	}.bind(this);

	/**
	 * Update the value of the chart
	 * @param  {number} newValue Number between 0 and 100
	 * @return {object}          Instance of the plugin for method chaining
	 */
	this.update = function(newValue) {
		newValue = parseFloat(newValue);
		if (options.animate) {
			this.renderer.animate(currentValue, newValue);
		} else {
			this.renderer.draw(newValue);
		}
		currentValue = newValue;
		return this;
	}.bind(this);

	init();
};

$.fn.easyPieChart = function(options) {
	return this.each(function() {
		var instanceOptions;

		if (!$.data(this, 'easyPieChart')) {
			instanceOptions = $.extend({}, options, $(this).data());
			$.data(this, 'easyPieChart', new EasyPieChart(this, instanceOptions));
		}
	});
};

}));
(function ($) {
  'use strict';

  // Case insensitive search
  $.expr[':'].icontains = function (obj, index, meta) {
    return icontains($(obj).text(), meta[3]);
  };

  // Case and accent insensitive search
  $.expr[':'].aicontains = function (obj, index, meta) {
    return icontains($(obj).data('normalizedText') || $(obj).text(), meta[3]);
  };

  /**
   * Actual implementation of the case insensitive search.
   * @access private
   * @param {String} haystack
   * @param {String} needle
   * @returns {boolean}
   */
  function icontains(haystack, needle) {
    return haystack.toUpperCase().indexOf(needle.toUpperCase()) > -1;
  }

  /**
   * Remove all diatrics from the given text.
   * @access private
   * @param {String} text
   * @returns {String}
   */
  function normalizeToBase(text) {
    var rExps = [
      {re: /[\xC0-\xC6]/g, ch: "A"},
      {re: /[\xE0-\xE6]/g, ch: "a"},
      {re: /[\xC8-\xCB]/g, ch: "E"},
      {re: /[\xE8-\xEB]/g, ch: "e"},
      {re: /[\xCC-\xCF]/g, ch: "I"},
      {re: /[\xEC-\xEF]/g, ch: "i"},
      {re: /[\xD2-\xD6]/g, ch: "O"},
      {re: /[\xF2-\xF6]/g, ch: "o"},
      {re: /[\xD9-\xDC]/g, ch: "U"},
      {re: /[\xF9-\xFC]/g, ch: "u"},
      {re: /[\xC7-\xE7]/g, ch: "c"},
      {re: /[\xD1]/g, ch: "N"},
      {re: /[\xF1]/g, ch: "n"}
    ];
    $.each(rExps, function () {
      text = text.replace(this.re, this.ch);
    });
    return text;
  }


  function htmlEscape(html) {
    var escapeMap = {
      '&': '&amp;',
      '<': '&lt;',
      '>': '&gt;',
      '"': '&quot;',
      "'": '&#x27;',
      '`': '&#x60;'
    };
    var source = '(?:' + Object.keys(escapeMap).join('|') + ')',
        testRegexp = new RegExp(source),
        replaceRegexp = new RegExp(source, 'g'),
        string = html == null ? '' : '' + html;
    return testRegexp.test(string) ? string.replace(replaceRegexp, function (match) {
      return escapeMap[match];
    }) : string;
  }

  var Selectpicker = function (element, options, e) {
    if (e) {
      e.stopPropagation();
      e.preventDefault();
    }

    this.$element = $(element);
    this.$newElement = null;
    this.$button = null;
    this.$menu = null;
    this.$lis = null;
    this.options = options;

    // If we have no title yet, try to pull it from the html title attribute (jQuery doesnt' pick it up as it's not a
    // data-attribute)
    if (this.options.title === null) {
      this.options.title = this.$element.attr('title');
    }

    //Expose public methods
    this.val = Selectpicker.prototype.val;
    this.render = Selectpicker.prototype.render;
    this.refresh = Selectpicker.prototype.refresh;
    this.setStyle = Selectpicker.prototype.setStyle;
    this.selectAll = Selectpicker.prototype.selectAll;
    this.deselectAll = Selectpicker.prototype.deselectAll;
    this.destroy = Selectpicker.prototype.remove;
    this.remove = Selectpicker.prototype.remove;
    this.show = Selectpicker.prototype.show;
    this.hide = Selectpicker.prototype.hide;

    this.init();
  };

  Selectpicker.VERSION = '1.6.3';

  // part of this is duplicated in i18n/defaults-en_US.js. Make sure to update both.
  Selectpicker.DEFAULTS = {
    noneSelectedText: 'Nothing selected',
    noneResultsText: 'No results match',
    countSelectedText: function (numSelected, numTotal) {
      return (numSelected == 1) ? "{0} item selected" : "{0} items selected";
    },
    maxOptionsText: function (numAll, numGroup) {
      var arr = [];

      arr[0] = (numAll == 1) ? 'Limit reached ({n} item max)' : 'Limit reached ({n} items max)';
      arr[1] = (numGroup == 1) ? 'Group limit reached ({n} item max)' : 'Group limit reached ({n} items max)';

      return arr;
    },
    selectAllText: 'Select All',
    deselectAllText: 'Deselect All',
    multipleSeparator: ', ',
    style: 'btn-default',
    size: 'auto',
    title: null,
    selectedTextFormat: 'values',
    width: false,
    container: false,
    hideDisabled: false,
    showSubtext: false,
    showIcon: true,
    showContent: true,
    dropupAuto: true,
    header: false,
    liveSearch: false,
    actionsBox: false,
    iconBase: 'glyphicon',
    tickIcon: 'glyphicon-ok',
    maxOptions: false,
    mobile: false,
    selectOnTab: false,
    dropdownAlignRight: false,
    searchAccentInsensitive: false
  };

  Selectpicker.prototype = {

    constructor: Selectpicker,

    init: function () {
      var that = this,
          id = this.$element.attr('id');

      this.$element.hide();
      this.multiple = this.$element.prop('multiple');
      this.autofocus = this.$element.prop('autofocus');
      this.$newElement = this.createView();
      this.$element.after(this.$newElement);
      this.$menu = this.$newElement.find('> .dropdown-menu');
      this.$button = this.$newElement.find('> button');
      this.$searchbox = this.$newElement.find('input');

      if (this.options.dropdownAlignRight)
        this.$menu.addClass('dropdown-menu-right');

      if (typeof id !== 'undefined') {
        this.$button.attr('data-id', id);
        $('label[for="' + id + '"]').click(function (e) {
          e.preventDefault();
          that.$button.focus();
        });
      }

      this.checkDisabled();
      this.clickListener();
      if (this.options.liveSearch) this.liveSearchListener();
      this.render();
      this.liHeight();
      this.setStyle();
      this.setWidth();
      if (this.options.container) this.selectPosition();
      this.$menu.data('this', this);
      this.$newElement.data('this', this);
      if (this.options.mobile) this.mobile();
    },

    createDropdown: function () {
      // Options
      // If we are multiple, then add the show-tick class by default
      var multiple = this.multiple ? ' show-tick' : '',
          inputGroup = this.$element.parent().hasClass('input-group') ? ' input-group-btn' : '',
          autofocus = this.autofocus ? ' autofocus' : '',
          btnSize = this.$element.parents().hasClass('form-group-lg') ? ' btn-lg' : (this.$element.parents().hasClass('form-group-sm') ? ' btn-sm' : '');
      // Elements
      var header = this.options.header ? '<div class="popover-title"><button type="button" class="close" aria-hidden="true">&times;</button>' + this.options.header + '</div>' : '';
      var searchbox = this.options.liveSearch ? '<div class="bs-searchbox"><input type="text" class="input-block-level form-control" autocomplete="off" /></div>' : '';
      var actionsbox = this.options.actionsBox ? '<div class="bs-actionsbox">' +
      '<div class="btn-group btn-block">' +
      '<button class="actions-btn bs-select-all btn btn-sm btn-default">' +
      this.options.selectAllText +
      '</button>' +
      '<button class="actions-btn bs-deselect-all btn btn-sm btn-default">' +
      this.options.deselectAllText +
      '</button>' +
      '</div>' +
      '</div>' : '';
      var drop =
          '<div class="btn-group bootstrap-select' + multiple + inputGroup + '">' +
          '<button type="button" class="btn dropdown-toggle selectpicker' + btnSize + '" data-toggle="dropdown"' + autofocus + '>' +
          '<span class="filter-option pull-left"></span>&nbsp;' +
          '<span class="caret"></span>' +
          '</button>' +
          '<div class="dropdown-menu open">' +
          header +
          searchbox +
          actionsbox +
          '<ul class="dropdown-menu inner selectpicker" role="menu">' +
          '</ul>' +
          '</div>' +
          '</div>';

      return $(drop);
    },

    createView: function () {
      var $drop = this.createDropdown();
      var $li = this.createLi();
      $drop.find('ul').append($li);
      return $drop;
    },

    reloadLi: function () {
      //Remove all children.
      this.destroyLi();
      //Re build
      var $li = this.createLi();
      this.$menu.find('ul').append($li);
    },

    destroyLi: function () {
      this.$menu.find('li').remove();
    },

    createLi: function () {
      var that = this,
          _li = [],
          optID = 0;

      // Helper functions
      /**
       * @param content
       * @param [index]
       * @param [classes]
       * @returns {string}
       */
      var generateLI = function (content, index, classes) {
        return '<li' +
        (typeof classes !== 'undefined' ? ' class="' + classes + '"' : '') +
        (typeof index !== 'undefined' | null === index ? ' data-original-index="' + index + '"' : '') +
        '>' + content + '</li>';
      };

      /**
       * @param text
       * @param [classes]
       * @param [inline]
       * @param [optgroup]
       * @returns {string}
       */
      var generateA = function (text, classes, inline, optgroup) {
        var normText = normalizeToBase(htmlEscape(text));
        return '<a tabindex="0"' +
        (typeof classes !== 'undefined' ? ' class="' + classes + '"' : '') +
        (typeof inline !== 'undefined' ? ' style="' + inline + '"' : '') +
        (typeof optgroup !== 'undefined' ? 'data-optgroup="' + optgroup + '"' : '') +
        ' data-normalized-text="' + normText + '"' +
        '>' + text +
        '<span class="' + that.options.iconBase + ' ' + that.options.tickIcon + ' check-mark"></span>' +
        '</a>';
      };

      this.$element.find('option').each(function () {
        var $this = $(this);

        // Get the class and text for the option
        var optionClass = $this.attr('class') || '',
            inline = $this.attr('style'),
            text = $this.data('content') ? $this.data('content') : $this.html(),
            subtext = typeof $this.data('subtext') !== 'undefined' ? '<small class="muted text-muted">' + $this.data('subtext') + '</small>' : '',
            icon = typeof $this.data('icon') !== 'undefined' ? '<span class="' + that.options.iconBase + ' ' + $this.data('icon') + '"></span> ' : '',
            isDisabled = $this.is(':disabled') || $this.parent().is(':disabled'),
            index = $this[0].index;
        if (icon !== '' && isDisabled) {
          icon = '<span>' + icon + '</span>';
        }

        if (!$this.data('content')) {
          // Prepend any icon and append any subtext to the main text.
          text = icon + '<span class="text">' + text + subtext + '</span>';
        }

        if (that.options.hideDisabled && isDisabled) {
          return;
        }

        if ($this.parent().is('optgroup') && $this.data('divider') !== true) {
          if ($this.index() === 0) { // Is it the first option of the optgroup?
            optID += 1;

            // Get the opt group label
            var label = $this.parent().attr('label');
            var labelSubtext = typeof $this.parent().data('subtext') !== 'undefined' ? '<small class="muted text-muted">' + $this.parent().data('subtext') + '</small>' : '';
            var labelIcon = $this.parent().data('icon') ? '<span class="' + that.options.iconBase + ' ' + $this.parent().data('icon') + '"></span> ' : '';
            label = labelIcon + '<span class="text">' + label + labelSubtext + '</span>';

            if (index !== 0 && _li.length > 0) { // Is it NOT the first option of the select && are there elements in the dropdown?
              _li.push(generateLI('', null, 'divider'));
            }

            _li.push(generateLI(label, null, 'dropdown-header'));
          }

          _li.push(generateLI(generateA(text, 'opt ' + optionClass, inline, optID), index));
        } else if ($this.data('divider') === true) {
          _li.push(generateLI('', index, 'divider'));
        } else if ($this.data('hidden') === true) {
          _li.push(generateLI(generateA(text, optionClass, inline), index, 'hide is-hidden'));
        } else {
          _li.push(generateLI(generateA(text, optionClass, inline), index));
        }
      });

      //If we are not multiple, we don't have a selected item, and we don't have a title, select the first element so something is set in the button
      if (!this.multiple && this.$element.find('option:selected').length === 0 && !this.options.title) {
        this.$element.find('option').eq(0).prop('selected', true).attr('selected', 'selected');
      }

      return $(_li.join(''));
    },

    findLis: function () {
      if (this.$lis == null) this.$lis = this.$menu.find('li');
      return this.$lis;
    },

    /**
     * @param [updateLi] defaults to true
     */
    render: function (updateLi) {
      var that = this;

      //Update the LI to match the SELECT
      if (updateLi !== false) {
        this.$element.find('option').each(function (index) {
          that.setDisabled(index, $(this).is(':disabled') || $(this).parent().is(':disabled'));
          that.setSelected(index, $(this).is(':selected'));
        });
      }

      this.tabIndex();
      var notDisabled = this.options.hideDisabled ? ':not([disabled])' : '';
      var selectedItems = this.$element.find('option:selected' + notDisabled).map(function () {
        var $this = $(this);
        var icon = $this.data('icon') && that.options.showIcon ? '<i class="' + that.options.iconBase + ' ' + $this.data('icon') + '"></i> ' : '';
        var subtext;
        if (that.options.showSubtext && $this.attr('data-subtext') && !that.multiple) {
          subtext = ' <small class="muted text-muted">' + $this.data('subtext') + '</small>';
        } else {
          subtext = '';
        }
        if ($this.data('content') && that.options.showContent) {
          return $this.data('content');
        } else if (typeof $this.attr('title') !== 'undefined') {
          return $this.attr('title');
        } else {
          return icon + $this.html() + subtext;
        }
      }).toArray();

      //Fixes issue in IE10 occurring when no default option is selected and at least one option is disabled
      //Convert all the values into a comma delimited string
      var title = !this.multiple ? selectedItems[0] : selectedItems.join(this.options.multipleSeparator);

      //If this is multi select, and the selectText type is count, the show 1 of 2 selected etc..
      if (this.multiple && this.options.selectedTextFormat.indexOf('count') > -1) {
        var max = this.options.selectedTextFormat.split('>');
        if ((max.length > 1 && selectedItems.length > max[1]) || (max.length == 1 && selectedItems.length >= 2)) {
          notDisabled = this.options.hideDisabled ? ', [disabled]' : '';
          var totalCount = this.$element.find('option').not('[data-divider="true"], [data-hidden="true"]' + notDisabled).length,
              tr8nText = (typeof this.options.countSelectedText === 'function') ? this.options.countSelectedText(selectedItems.length, totalCount) : this.options.countSelectedText;
          title = tr8nText.replace('{0}', selectedItems.length.toString()).replace('{1}', totalCount.toString());
        }
      }

      this.options.title = this.$element.attr('title');

      if (this.options.selectedTextFormat == 'static') {
        title = this.options.title;
      }

      //If we dont have a title, then use the default, or if nothing is set at all, use the not selected text
      if (!title) {
        title = typeof this.options.title !== 'undefined' ? this.options.title : this.options.noneSelectedText;
      }

      this.$button.attr('title', htmlEscape(title));
      this.$newElement.find('.filter-option').html(title);
    },

    /**
     * @param [style]
     * @param [status]
     */
    setStyle: function (style, status) {
      if (this.$element.attr('class')) {
        this.$newElement.addClass(this.$element.attr('class').replace(/selectpicker|mobile-device|validate\[.*\]/gi, ''));
      }

      var buttonClass = style ? style : this.options.style;

      if (status == 'add') {
        this.$button.addClass(buttonClass);
      } else if (status == 'remove') {
        this.$button.removeClass(buttonClass);
      } else {
        this.$button.removeClass(this.options.style);
        this.$button.addClass(buttonClass);
      }
    },

    liHeight: function () {
      if (this.options.size === false) return;

      var $selectClone = this.$menu.parent().clone().find('> .dropdown-toggle').prop('autofocus', false).end().appendTo('body'),
          $menuClone = $selectClone.addClass('open').find('> .dropdown-menu'),
          liHeight = $menuClone.find('li').not('.divider').not('.dropdown-header').filter(':visible').children('a').outerHeight(),
          headerHeight = this.options.header ? $menuClone.find('.popover-title').outerHeight() : 0,
          searchHeight = this.options.liveSearch ? $menuClone.find('.bs-searchbox').outerHeight() : 0,
          actionsHeight = this.options.actionsBox ? $menuClone.find('.bs-actionsbox').outerHeight() : 0;

      $selectClone.remove();

      this.$newElement
          .data('liHeight', liHeight)
          .data('headerHeight', headerHeight)
          .data('searchHeight', searchHeight)
          .data('actionsHeight', actionsHeight);
    },

    setSize: function () {
      this.findLis();
      var that = this,
          menu = this.$menu,
          menuInner = menu.find('.inner'),
          selectHeight = this.$newElement.outerHeight(),
          liHeight = this.$newElement.data('liHeight'),
          headerHeight = this.$newElement.data('headerHeight'),
          searchHeight = this.$newElement.data('searchHeight'),
          actionsHeight = this.$newElement.data('actionsHeight'),
          divHeight = this.$lis.filter('.divider').outerHeight(true),
          menuPadding = parseInt(menu.css('padding-top')) +
              parseInt(menu.css('padding-bottom')) +
              parseInt(menu.css('border-top-width')) +
              parseInt(menu.css('border-bottom-width')),
          notDisabled = this.options.hideDisabled ? ', .disabled' : '',
          $window = $(window),
          menuExtras = menuPadding + parseInt(menu.css('margin-top')) + parseInt(menu.css('margin-bottom')) + 2,
          menuHeight,
          selectOffsetTop,
          selectOffsetBot,
          posVert = function () {
            // JQuery defines a scrollTop function, but in pure JS it's a property
            //noinspection JSValidateTypes
            selectOffsetTop = that.$newElement.offset().top - $window.scrollTop();
            selectOffsetBot = $window.height() - selectOffsetTop - selectHeight;
          };
      posVert();
      if (this.options.header) menu.css('padding-top', 0);

      if (this.options.size == 'auto') {
        var getSize = function () {
          var minHeight,
              lisVis = that.$lis.not('.hide');

          posVert();
          menuHeight = selectOffsetBot - menuExtras;

          if (that.options.dropupAuto) {
            that.$newElement.toggleClass('dropup', (selectOffsetTop > selectOffsetBot) && ((menuHeight - menuExtras) < menu.height()));
          }
          if (that.$newElement.hasClass('dropup')) {
            menuHeight = selectOffsetTop - menuExtras;
          }

          if ((lisVis.length + lisVis.filter('.dropdown-header').length) > 3) {
            minHeight = liHeight * 3 + menuExtras - 2;
          } else {
            minHeight = 0;
          }

          menu.css({
            'max-height': menuHeight + 'px',
            'overflow': 'hidden',
            'min-height': minHeight + headerHeight + searchHeight + actionsHeight + 'px'
          });
          menuInner.css({
            'max-height': menuHeight - headerHeight - searchHeight - actionsHeight - menuPadding + 'px',
            'overflow-y': 'auto',
            'min-height': Math.max(minHeight - menuPadding, 0) + 'px'
          });
        };
        getSize();
        this.$searchbox.off('input.getSize propertychange.getSize').on('input.getSize propertychange.getSize', getSize);
        $(window).off('resize.getSize').on('resize.getSize', getSize);
        $(window).off('scroll.getSize').on('scroll.getSize', getSize);
      } else if (this.options.size && this.options.size != 'auto' && menu.find('li' + notDisabled).length > this.options.size) {
        var optIndex = this.$lis.not('.divider' + notDisabled).find(' > *').slice(0, this.options.size).last().parent().index();
        var divLength = this.$lis.slice(0, optIndex + 1).filter('.divider').length;
        menuHeight = liHeight * this.options.size + divLength * divHeight + menuPadding;
        if (that.options.dropupAuto) {
          //noinspection JSUnusedAssignment
          this.$newElement.toggleClass('dropup', (selectOffsetTop > selectOffsetBot) && (menuHeight < menu.height()));
        }
        menu.css({'max-height': menuHeight + headerHeight + searchHeight + actionsHeight + 'px', 'overflow': 'hidden'});
        menuInner.css({'max-height': menuHeight - menuPadding + 'px', 'overflow-y': 'auto'});
      }
    },

    setWidth: function () {
      if (this.options.width == 'auto') {
        this.$menu.css('min-width', '0');

        // Get correct width if element hidden
        var selectClone = this.$newElement.clone().appendTo('body');
        var ulWidth = selectClone.find('> .dropdown-menu').css('width');
        var btnWidth = selectClone.css('width', 'auto').find('> button').css('width');
        selectClone.remove();

        // Set width to whatever's larger, button title or longest option
        this.$newElement.css('width', Math.max(parseInt(ulWidth), parseInt(btnWidth)) + 'px');
      } else if (this.options.width == 'fit') {
        // Remove inline min-width so width can be changed from 'auto'
        this.$menu.css('min-width', '');
        this.$newElement.css('width', '').addClass('fit-width');
      } else if (this.options.width) {
        // Remove inline min-width so width can be changed from 'auto'
        this.$menu.css('min-width', '');
        this.$newElement.css('width', this.options.width);
      } else {
        // Remove inline min-width/width so width can be changed
        this.$menu.css('min-width', '');
        this.$newElement.css('width', '');
      }
      // Remove fit-width class if width is changed programmatically
      if (this.$newElement.hasClass('fit-width') && this.options.width !== 'fit') {
        this.$newElement.removeClass('fit-width');
      }
    },

    selectPosition: function () {
      var that = this,
          drop = '<div />',
          $drop = $(drop),
          pos,
          actualHeight,
          getPlacement = function ($element) {
            $drop.addClass($element.attr('class').replace(/form-control/gi, '')).toggleClass('dropup', $element.hasClass('dropup'));
            pos = $element.offset();
            actualHeight = $element.hasClass('dropup') ? 0 : $element[0].offsetHeight;
            $drop.css({
              'top': pos.top + actualHeight,
              'left': pos.left,
              'width': $element[0].offsetWidth,
              'position': 'absolute'
            });
          };
      this.$newElement.on('click', function () {
        if (that.isDisabled()) {
          return;
        }
        getPlacement($(this));
        $drop.appendTo(that.options.container);
        $drop.toggleClass('open', !$(this).hasClass('open'));
        $drop.append(that.$menu);
      });
      $(window).resize(function () {
        getPlacement(that.$newElement);
      });
      $(window).on('scroll', function () {
        getPlacement(that.$newElement);
      });
      $('html').on('click', function (e) {
        if ($(e.target).closest(that.$newElement).length < 1) {
          $drop.removeClass('open');
        }
      });
    },

    setSelected: function (index, selected) {
      this.findLis();
      this.$lis.filter('[data-original-index="' + index + '"]').toggleClass('selected', selected);
    },

    setDisabled: function (index, disabled) {
      this.findLis();
      if (disabled) {
        this.$lis.filter('[data-original-index="' + index + '"]').addClass('disabled').find('a').attr('href', '#').attr('tabindex', -1);
      } else {
        this.$lis.filter('[data-original-index="' + index + '"]').removeClass('disabled').find('a').removeAttr('href').attr('tabindex', 0);
      }
    },

    isDisabled: function () {
      return this.$element.is(':disabled');
    },

    checkDisabled: function () {
      var that = this;

      if (this.isDisabled()) {
        this.$button.addClass('disabled').attr('tabindex', -1);
      } else {
        if (this.$button.hasClass('disabled')) {
          this.$button.removeClass('disabled');
        }

        if (this.$button.attr('tabindex') == -1) {
          if (!this.$element.data('tabindex')) this.$button.removeAttr('tabindex');
        }
      }

      this.$button.click(function () {
        return !that.isDisabled();
      });
    },

    tabIndex: function () {
      if (this.$element.is('[tabindex]')) {
        this.$element.data('tabindex', this.$element.attr('tabindex'));
        this.$button.attr('tabindex', this.$element.data('tabindex'));
      }
    },

    clickListener: function () {
      var that = this;

      this.$newElement.on('touchstart.dropdown', '.dropdown-menu', function (e) {
        e.stopPropagation();
      });

      this.$newElement.on('click', function () {
        that.setSize();
        if (!that.options.liveSearch && !that.multiple) {
          setTimeout(function () {
            that.$menu.find('.selected a').focus();
          }, 10);
        }
      });

      this.$menu.on('click', 'li a', function (e) {
        var $this = $(this),
            clickedIndex = $this.parent().data('originalIndex'),
            prevValue = that.$element.val(),
            prevIndex = that.$element.prop('selectedIndex');

        // Don't close on multi choice menu
        if (that.multiple) {
          e.stopPropagation();
        }

        e.preventDefault();

        //Don't run if we have been disabled
        if (!that.isDisabled() && !$this.parent().hasClass('disabled')) {
          var $options = that.$element.find('option'),
              $option = $options.eq(clickedIndex),
              state = $option.prop('selected'),
              $optgroup = $option.parent('optgroup'),
              maxOptions = that.options.maxOptions,
              maxOptionsGrp = $optgroup.data('maxOptions') || false;

          if (!that.multiple) { // Deselect all others if not multi select box
            $options.prop('selected', false);
            $option.prop('selected', true);
            that.$menu.find('.selected').removeClass('selected');
            that.setSelected(clickedIndex, true);
          } else { // Toggle the one we have chosen if we are multi select.
            $option.prop('selected', !state);
            that.setSelected(clickedIndex, !state);
            $this.blur();

            if ((maxOptions !== false) || (maxOptionsGrp !== false)) {
              var maxReached = maxOptions < $options.filter(':selected').length,
                  maxReachedGrp = maxOptionsGrp < $optgroup.find('option:selected').length;

              if ((maxOptions && maxReached) || (maxOptionsGrp && maxReachedGrp)) {
                if (maxOptions && maxOptions == 1) {
                  $options.prop('selected', false);
                  $option.prop('selected', true);
                  that.$menu.find('.selected').removeClass('selected');
                  that.setSelected(clickedIndex, true);
                } else if (maxOptionsGrp && maxOptionsGrp == 1) {
                  $optgroup.find('option:selected').prop('selected', false);
                  $option.prop('selected', true);
                  var optgroupID = $this.data('optgroup');

                  that.$menu.find('.selected').has('a[data-optgroup="' + optgroupID + '"]').removeClass('selected');

                  that.setSelected(clickedIndex, true);
                } else {
                  var maxOptionsArr = (typeof that.options.maxOptionsText === 'function') ?
                          that.options.maxOptionsText(maxOptions, maxOptionsGrp) : that.options.maxOptionsText,
                      maxTxt = maxOptionsArr[0].replace('{n}', maxOptions),
                      maxTxtGrp = maxOptionsArr[1].replace('{n}', maxOptionsGrp),
                      $notify = $('<div class="notify"></div>');
                  // If {var} is set in array, replace it
                  /** @deprecated */
                  if (maxOptionsArr[2]) {
                    maxTxt = maxTxt.replace('{var}', maxOptionsArr[2][maxOptions > 1 ? 0 : 1]);
                    maxTxtGrp = maxTxtGrp.replace('{var}', maxOptionsArr[2][maxOptionsGrp > 1 ? 0 : 1]);
                  }

                  $option.prop('selected', false);

                  that.$menu.append($notify);

                  if (maxOptions && maxReached) {
                    $notify.append($('<div>' + maxTxt + '</div>'));
                    that.$element.trigger('maxReached.bs.select');
                  }

                  if (maxOptionsGrp && maxReachedGrp) {
                    $notify.append($('<div>' + maxTxtGrp + '</div>'));
                    that.$element.trigger('maxReachedGrp.bs.select');
                  }

                  setTimeout(function () {
                    that.setSelected(clickedIndex, false);
                  }, 10);

                  $notify.delay(750).fadeOut(300, function () {
                    $(this).remove();
                  });
                }
              }
            }
          }

          if (!that.multiple) {
            that.$button.focus();
          } else if (that.options.liveSearch) {
            that.$searchbox.focus();
          }

          // Trigger select 'change'
          if ((prevValue != that.$element.val() && that.multiple) || (prevIndex != that.$element.prop('selectedIndex') && !that.multiple)) {
            that.$element.change();
          }
        }
      });

      this.$menu.on('click', 'li.disabled a, .popover-title, .popover-title :not(.close)', function (e) {
        if (e.target == this) {
          e.preventDefault();
          e.stopPropagation();
          if (!that.options.liveSearch) {
            that.$button.focus();
          } else {
            that.$searchbox.focus();
          }
        }
      });

      this.$menu.on('click', 'li.divider, li.dropdown-header', function (e) {
        e.preventDefault();
        e.stopPropagation();
        if (!that.options.liveSearch) {
          that.$button.focus();
        } else {
          that.$searchbox.focus();
        }
      });

      this.$menu.on('click', '.popover-title .close', function () {
        that.$button.focus();
      });

      this.$searchbox.on('click', function (e) {
        e.stopPropagation();
      });


      this.$menu.on('click', '.actions-btn', function (e) {
        if (that.options.liveSearch) {
          that.$searchbox.focus();
        } else {
          that.$button.focus();
        }

        e.preventDefault();
        e.stopPropagation();

        if ($(this).is('.bs-select-all')) {
          that.selectAll();
        } else {
          that.deselectAll();
        }
        that.$element.change();
      });

      this.$element.change(function () {
        that.render(false);
      });
    },

    liveSearchListener: function () {
      var that = this,
          no_results = $('<li class="no-results"></li>');

      this.$newElement.on('click.dropdown.data-api touchstart.dropdown.data-api', function () {
        that.$menu.find('.active').removeClass('active');
        if (!!that.$searchbox.val()) {
          that.$searchbox.val('');
          that.$lis.not('.is-hidden').removeClass('hide');
          if (!!no_results.parent().length) no_results.remove();
        }
        if (!that.multiple) that.$menu.find('.selected').addClass('active');
        setTimeout(function () {
          that.$searchbox.focus();
        }, 10);
      });

      this.$searchbox.on('click.dropdown.data-api focus.dropdown.data-api touchend.dropdown.data-api', function (e) {
        e.stopPropagation();
      });

      this.$searchbox.on('input propertychange', function () {
        if (that.$searchbox.val()) {

          if (that.options.searchAccentInsensitive) {
            that.$lis.not('.is-hidden').removeClass('hide').find('a').not(':aicontains(' + normalizeToBase(that.$searchbox.val()) + ')').parent().addClass('hide');
          } else {
            that.$lis.not('.is-hidden').removeClass('hide').find('a').not(':icontains(' + that.$searchbox.val() + ')').parent().addClass('hide');
          }

          if (!that.$menu.find('li').filter(':visible:not(.no-results)').length) {
            if (!!no_results.parent().length) no_results.remove();
            no_results.html(that.options.noneResultsText + ' "' + htmlEscape(that.$searchbox.val()) + '"').show();
            that.$menu.find('li').last().after(no_results);
          } else if (!!no_results.parent().length) {
            no_results.remove();
          }

        } else {
          that.$lis.not('.is-hidden').removeClass('hide');
          if (!!no_results.parent().length) no_results.remove();
        }

        that.$menu.find('li.active').removeClass('active');
        that.$menu.find('li').filter(':visible:not(.divider)').eq(0).addClass('active').find('a').focus();
        $(this).focus();
      });
    },

    val: function (value) {
      if (typeof value !== 'undefined') {
        this.$element.val(value);
        this.render();

        return this.$element;
      } else {
        return this.$element.val();
      }
    },

    selectAll: function () {
      this.findLis();
      this.$lis.not('.divider').not('.disabled').not('.selected').filter(':visible').find('a').click();
    },

    deselectAll: function () {
      this.findLis();
      this.$lis.not('.divider').not('.disabled').filter('.selected').filter(':visible').find('a').click();
    },

    keydown: function (e) {
      var $this = $(this),
          $parent = ($this.is('input')) ? $this.parent().parent() : $this.parent(),
          $items,
          that = $parent.data('this'),
          index,
          next,
          first,
          last,
          prev,
          nextPrev,
          prevIndex,
          isActive,
          keyCodeMap = {
            32: ' ',
            48: '0',
            49: '1',
            50: '2',
            51: '3',
            52: '4',
            53: '5',
            54: '6',
            55: '7',
            56: '8',
            57: '9',
            59: ';',
            65: 'a',
            66: 'b',
            67: 'c',
            68: 'd',
            69: 'e',
            70: 'f',
            71: 'g',
            72: 'h',
            73: 'i',
            74: 'j',
            75: 'k',
            76: 'l',
            77: 'm',
            78: 'n',
            79: 'o',
            80: 'p',
            81: 'q',
            82: 'r',
            83: 's',
            84: 't',
            85: 'u',
            86: 'v',
            87: 'w',
            88: 'x',
            89: 'y',
            90: 'z',
            96: '0',
            97: '1',
            98: '2',
            99: '3',
            100: '4',
            101: '5',
            102: '6',
            103: '7',
            104: '8',
            105: '9'
          };

      if (that.options.liveSearch) $parent = $this.parent().parent();

      if (that.options.container) $parent = that.$menu;

      $items = $('[role=menu] li a', $parent);

      isActive = that.$menu.parent().hasClass('open');

      if (!isActive && /([0-9]|[A-z])/.test(String.fromCharCode(e.keyCode))) {
        if (!that.options.container) {
          that.setSize();
          that.$menu.parent().addClass('open');
          isActive = true;
        } else {
          that.$newElement.trigger('click');
        }
        that.$searchbox.focus();
      }

      if (that.options.liveSearch) {
        if (/(^9$|27)/.test(e.keyCode.toString(10)) && isActive && that.$menu.find('.active').length === 0) {
          e.preventDefault();
          that.$menu.parent().removeClass('open');
          that.$button.focus();
        }
        $items = $('[role=menu] li:not(.divider):not(.dropdown-header):visible', $parent);
        if (!$this.val() && !/(38|40)/.test(e.keyCode.toString(10))) {
          if ($items.filter('.active').length === 0) {
            if (that.options.searchAccentInsensitive) {
              $items = that.$newElement.find('li').filter(':aicontains(' + normalizeToBase(keyCodeMap[e.keyCode]) + ')');
            } else {
              $items = that.$newElement.find('li').filter(':icontains(' + keyCodeMap[e.keyCode] + ')');
            }
          }
        }
      }

      if (!$items.length) return;

      if (/(38|40)/.test(e.keyCode.toString(10))) {
        index = $items.index($items.filter(':focus'));
        first = $items.parent(':not(.disabled):visible').first().index();
        last = $items.parent(':not(.disabled):visible').last().index();
        next = $items.eq(index).parent().nextAll(':not(.disabled):visible').eq(0).index();
        prev = $items.eq(index).parent().prevAll(':not(.disabled):visible').eq(0).index();
        nextPrev = $items.eq(next).parent().prevAll(':not(.disabled):visible').eq(0).index();

        if (that.options.liveSearch) {
          $items.each(function (i) {
            if ($(this).is(':not(.disabled)')) {
              $(this).data('index', i);
            }
          });
          index = $items.index($items.filter('.active'));
          first = $items.filter(':not(.disabled):visible').first().data('index');
          last = $items.filter(':not(.disabled):visible').last().data('index');
          next = $items.eq(index).nextAll(':not(.disabled):visible').eq(0).data('index');
          prev = $items.eq(index).prevAll(':not(.disabled):visible').eq(0).data('index');
          nextPrev = $items.eq(next).prevAll(':not(.disabled):visible').eq(0).data('index');
        }

        prevIndex = $this.data('prevIndex');

        if (e.keyCode == 38) {
          if (that.options.liveSearch) index -= 1;
          if (index != nextPrev && index > prev) index = prev;
          if (index < first) index = first;
          if (index == prevIndex) index = last;
        }

        if (e.keyCode == 40) {
          if (that.options.liveSearch) index += 1;
          if (index == -1) index = 0;
          if (index != nextPrev && index < next) index = next;
          if (index > last) index = last;
          if (index == prevIndex) index = first;
        }

        $this.data('prevIndex', index);

        if (!that.options.liveSearch) {
          $items.eq(index).focus();
        } else {
          e.preventDefault();
          if (!$this.is('.dropdown-toggle')) {
            $items.removeClass('active');
            $items.eq(index).addClass('active').find('a').focus();
            $this.focus();
          }
        }

      } else if (!$this.is('input')) {
        var keyIndex = [],
            count,
            prevKey;

        $items.each(function () {
          if ($(this).parent().is(':not(.disabled)')) {
            if ($.trim($(this).text().toLowerCase()).substring(0, 1) == keyCodeMap[e.keyCode]) {
              keyIndex.push($(this).parent().index());
            }
          }
        });

        count = $(document).data('keycount');
        count++;
        $(document).data('keycount', count);

        prevKey = $.trim($(':focus').text().toLowerCase()).substring(0, 1);

        if (prevKey != keyCodeMap[e.keyCode]) {
          count = 1;
          $(document).data('keycount', count);
        } else if (count >= keyIndex.length) {
          $(document).data('keycount', 0);
          if (count > keyIndex.length) count = 1;
        }

        $items.eq(keyIndex[count - 1]).focus();
      }

      // Select focused option if "Enter", "Spacebar" or "Tab" (when selectOnTab is true) are pressed inside the menu.
      if ((/(13|32)/.test(e.keyCode.toString(10)) || (/(^9$)/.test(e.keyCode.toString(10)) && that.options.selectOnTab)) && isActive) {
        if (!/(32)/.test(e.keyCode.toString(10))) e.preventDefault();
        if (!that.options.liveSearch) {
          $(':focus').click();
        } else if (!/(32)/.test(e.keyCode.toString(10))) {
          that.$menu.find('.active a').click();
          $this.focus();
        }
        $(document).data('keycount', 0);
      }

      if ((/(^9$|27)/.test(e.keyCode.toString(10)) && isActive && (that.multiple || that.options.liveSearch)) || (/(27)/.test(e.keyCode.toString(10)) && !isActive)) {
        that.$menu.parent().removeClass('open');
        that.$button.focus();
      }
    },

    mobile: function () {
      this.$element.addClass('mobile-device').appendTo(this.$newElement);
      if (this.options.container) this.$menu.hide();
    },

    refresh: function () {
      this.$lis = null;
      this.reloadLi();
      this.render();
      this.setWidth();
      this.setStyle();
      this.checkDisabled();
      this.liHeight();
    },

    update: function () {
      this.reloadLi();
      this.setWidth();
      this.setStyle();
      this.checkDisabled();
      this.liHeight();
    },

    hide: function () {
      this.$newElement.hide();
    },

    show: function () {
      this.$newElement.show();
    },

    remove: function () {
      this.$newElement.remove();
      this.$element.remove();
    }
  };

  // SELECTPICKER PLUGIN DEFINITION
  // ==============================
  function Plugin(option, event) {
    // get the args of the outer function..
    var args = arguments;
    // The arguments of the function are explicitly re-defined from the argument list, because the shift causes them
    // to get lost
    //noinspection JSDuplicatedDeclaration
    var _option = option,
        option = args[0],
        event = args[1];
    [].shift.apply(args);

    // This fixes a bug in the js implementation on android 2.3 #715
    if (typeof option == 'undefined') {
      option = _option;
    }

    var value;
    var chain = this.each(function () {
      var $this = $(this);
      if ($this.is('select')) {
        var data = $this.data('selectpicker'),
            options = typeof option == 'object' && option;

        if (!data) {
          var config = $.extend({}, Selectpicker.DEFAULTS, $.fn.selectpicker.defaults || {}, $this.data(), options);
          $this.data('selectpicker', (data = new Selectpicker(this, config, event)));
        } else if (options) {
          for (var i in options) {
            if (options.hasOwnProperty(i)) {
              data.options[i] = options[i];
            }
          }
        }

        if (typeof option == 'string') {
          if (data[option] instanceof Function) {
            value = data[option].apply(data, args);
          } else {
            value = data.options[option];
          }
        }
      }
    });

    if (typeof value !== 'undefined') {
      //noinspection JSUnusedAssignment
      return value;
    } else {
      return chain;
    }
  }

  var old = $.fn.selectpicker;
  $.fn.selectpicker = Plugin;
  $.fn.selectpicker.Constructor = Selectpicker;

  // SELECTPICKER NO CONFLICT
  // ========================
  $.fn.selectpicker.noConflict = function () {
    $.fn.selectpicker = old;
    return this;
  };

  $(document)
      .data('keycount', 0)
      .on('keydown', '.bootstrap-select [data-toggle=dropdown], .bootstrap-select [role=menu], .bs-searchbox input', Selectpicker.prototype.keydown)
      .on('focusin.modal', '.bootstrap-select [data-toggle=dropdown], .bootstrap-select [role=menu], .bs-searchbox input', function (e) {
        e.stopPropagation();
      });

  // SELECTPICKER DATA-API
  // =====================
  $(window).on('load.bs.select.data-api', function () {
    $('.selectpicker').each(function () {
      var $selectpicker = $(this);
      Plugin.call($selectpicker, $selectpicker.data());
    })
  });
})(jQuery);
/* Inspired by Lee Byron's test data generator. */
function stream_layers(n, m, o) {
    if (arguments.length < 3)
        o = 0;
    function bump(a) {
        var x = 1 / (.1 + Math.random()),
                y = 2 * Math.random() - .5,
                z = 10 / (.1 + Math.random());
        for (var i = 0; i < m; i++) {
            var w = (i / m - y) * z;
            a[i] += x * Math.exp(-w * w);
        }
    }
    return d3.range(n).map(function () {
        var a = [], i;
        for (i = 0; i < m; i++)
            a[i] = o + o * Math.random();
        for (i = 0; i < 5; i++)
            bump(a);
        return a.map(stream_index);
    });
}

/* Another layer generator using gamma distributions. */
function stream_waves(n, m) {
    return d3.range(n).map(function (i) {
        return d3.range(m).map(function (j) {
            var x = 20 * j / m - i / 3;
            return 2 * x * Math.exp(-.5 * x);
        }).map(stream_index);
    });
}

function stream_index(d, i) {
    return {x: i, y: Math.max(0, d)};
}
/**
*
* jquery.sparkline.js
*
* v2.1.2
* (c) Splunk, Inc
* Contact: Gareth Watts (gareth@splunk.com)
* http://omnipotent.net/jquery.sparkline/
*
* Generates inline sparkline charts from data supplied either to the method
* or inline in HTML
*
* Compatible with Internet Explorer 6.0+ and modern browsers equipped with the canvas tag
* (Firefox 2.0+, Safari, Opera, etc)
*
* License: New BSD License
*
* Copyright (c) 2012, Splunk Inc.
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without modification,
* are permitted provided that the following conditions are met:
*
*     * Redistributions of source code must retain the above copyright notice,
*       this list of conditions and the following disclaimer.
*     * Redistributions in binary form must reproduce the above copyright notice,
*       this list of conditions and the following disclaimer in the documentation
*       and/or other materials provided with the distribution.
*     * Neither the name of Splunk Inc nor the names of its contributors may
*       be used to endorse or promote products derived from this software without
*       specific prior written permission.
*
* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY
* EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
* OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT
* SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
* SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT
* OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
* HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
* OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
* SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*
*
* Usage:
*  $(selector).sparkline(values, options)
*
* If values is undefined or set to 'html' then the data values are read from the specified tag:
*   <p>Sparkline: <span class="sparkline">1,4,6,6,8,5,3,5</span></p>
*   $('.sparkline').sparkline();
* There must be no spaces in the enclosed data set
*
* Otherwise values must be an array of numbers or null values
*    <p>Sparkline: <span id="sparkline1">This text replaced if the browser is compatible</span></p>
*    $('#sparkline1').sparkline([1,4,6,6,8,5,3,5])
*    $('#sparkline2').sparkline([1,4,6,null,null,5,3,5])
*
* Values can also be specified in an HTML comment, or as a values attribute:
*    <p>Sparkline: <span class="sparkline"><!--1,4,6,6,8,5,3,5 --></span></p>
*    <p>Sparkline: <span class="sparkline" values="1,4,6,6,8,5,3,5"></span></p>
*    $('.sparkline').sparkline();
*
* For line charts, x values can also be specified:
*   <p>Sparkline: <span class="sparkline">1:1,2.7:4,3.4:6,5:6,6:8,8.7:5,9:3,10:5</span></p>
*    $('#sparkline1').sparkline([ [1,1], [2.7,4], [3.4,6], [5,6], [6,8], [8.7,5], [9,3], [10,5] ])
*
* By default, options should be passed in as teh second argument to the sparkline function:
*   $('.sparkline').sparkline([1,2,3,4], {type: 'bar'})
*
* Options can also be set by passing them on the tag itself.  This feature is disabled by default though
* as there's a slight performance overhead:
*   $('.sparkline').sparkline([1,2,3,4], {enableTagOptions: true})
*   <p>Sparkline: <span class="sparkline" sparkType="bar" sparkBarColor="red">loading</span></p>
* Prefix all options supplied as tag attribute with "spark" (configurable by setting tagOptionPrefix)
*
* Supported options:
*   lineColor - Color of the line used for the chart
*   fillColor - Color used to fill in the chart - Set to '' or false for a transparent chart
*   width - Width of the chart - Defaults to 3 times the number of values in pixels
*   height - Height of the chart - Defaults to the height of the containing element
*   chartRangeMin - Specify the minimum value to use for the Y range of the chart - Defaults to the minimum value supplied
*   chartRangeMax - Specify the maximum value to use for the Y range of the chart - Defaults to the maximum value supplied
*   chartRangeClip - Clip out of range values to the max/min specified by chartRangeMin and chartRangeMax
*   chartRangeMinX - Specify the minimum value to use for the X range of the chart - Defaults to the minimum value supplied
*   chartRangeMaxX - Specify the maximum value to use for the X range of the chart - Defaults to the maximum value supplied
*   composite - If true then don't erase any existing chart attached to the tag, but draw
*           another chart over the top - Note that width and height are ignored if an
*           existing chart is detected.
*   tagValuesAttribute - Name of tag attribute to check for data values - Defaults to 'values'
*   enableTagOptions - Whether to check tags for sparkline options
*   tagOptionPrefix - Prefix used for options supplied as tag attributes - Defaults to 'spark'
*   disableHiddenCheck - If set to true, then the plugin will assume that charts will never be drawn into a
*           hidden dom element, avoding a browser reflow
*   disableInteraction - If set to true then all mouseover/click interaction behaviour will be disabled,
*       making the plugin perform much like it did in 1.x
*   disableTooltips - If set to true then tooltips will be disabled - Defaults to false (tooltips enabled)
*   disableHighlight - If set to true then highlighting of selected chart elements on mouseover will be disabled
*       defaults to false (highlights enabled)
*   highlightLighten - Factor to lighten/darken highlighted chart values by - Defaults to 1.4 for a 40% increase
*   tooltipContainer - Specify which DOM element the tooltip should be rendered into - defaults to document.body
*   tooltipClassname - Optional CSS classname to apply to tooltips - If not specified then a default style will be applied
*   tooltipOffsetX - How many pixels away from the mouse pointer to render the tooltip on the X axis
*   tooltipOffsetY - How many pixels away from the mouse pointer to render the tooltip on the r axis
*   tooltipFormatter  - Optional callback that allows you to override the HTML displayed in the tooltip
*       callback is given arguments of (sparkline, options, fields)
*   tooltipChartTitle - If specified then the tooltip uses the string specified by this setting as a title
*   tooltipFormat - A format string or SPFormat object  (or an array thereof for multiple entries)
*       to control the format of the tooltip
*   tooltipPrefix - A string to prepend to each field displayed in a tooltip
*   tooltipSuffix - A string to append to each field displayed in a tooltip
*   tooltipSkipNull - If true then null values will not have a tooltip displayed (defaults to true)
*   tooltipValueLookups - An object or range map to map field values to tooltip strings
*       (eg. to map -1 to "Lost", 0 to "Draw", and 1 to "Win")
*   numberFormatter - Optional callback for formatting numbers in tooltips
*   numberDigitGroupSep - Character to use for group separator in numbers "1,234" - Defaults to ","
*   numberDecimalMark - Character to use for the decimal point when formatting numbers - Defaults to "."
*   numberDigitGroupCount - Number of digits between group separator - Defaults to 3
*
* There are 7 types of sparkline, selected by supplying a "type" option of 'line' (default),
* 'bar', 'tristate', 'bullet', 'discrete', 'pie' or 'box'
*    line - Line chart.  Options:
*       spotColor - Set to '' to not end each line in a circular spot
*       minSpotColor - If set, color of spot at minimum value
*       maxSpotColor - If set, color of spot at maximum value
*       spotRadius - Radius in pixels
*       lineWidth - Width of line in pixels
*       normalRangeMin
*       normalRangeMax - If set draws a filled horizontal bar between these two values marking the "normal"
*                      or expected range of values
*       normalRangeColor - Color to use for the above bar
*       drawNormalOnTop - Draw the normal range above the chart fill color if true
*       defaultPixelsPerValue - Defaults to 3 pixels of width for each value in the chart
*       highlightSpotColor - The color to use for drawing a highlight spot on mouseover - Set to null to disable
*       highlightLineColor - The color to use for drawing a highlight line on mouseover - Set to null to disable
*       valueSpots - Specify which points to draw spots on, and in which color.  Accepts a range map
*
*   bar - Bar chart.  Options:
*       barColor - Color of bars for postive values
*       negBarColor - Color of bars for negative values
*       zeroColor - Color of bars with zero values
*       nullColor - Color of bars with null values - Defaults to omitting the bar entirely
*       barWidth - Width of bars in pixels
*       colorMap - Optional mappnig of values to colors to override the *BarColor values above
*                  can be an Array of values to control the color of individual bars or a range map
*                  to specify colors for individual ranges of values
*       barSpacing - Gap between bars in pixels
*       zeroAxis - Centers the y-axis around zero if true
*
*   tristate - Charts values of win (>0), lose (<0) or draw (=0)
*       posBarColor - Color of win values
*       negBarColor - Color of lose values
*       zeroBarColor - Color of draw values
*       barWidth - Width of bars in pixels
*       barSpacing - Gap between bars in pixels
*       colorMap - Optional mappnig of values to colors to override the *BarColor values above
*                  can be an Array of values to control the color of individual bars or a range map
*                  to specify colors for individual ranges of values
*
*   discrete - Options:
*       lineHeight - Height of each line in pixels - Defaults to 30% of the graph height
*       thesholdValue - Values less than this value will be drawn using thresholdColor instead of lineColor
*       thresholdColor
*
*   bullet - Values for bullet graphs msut be in the order: target, performance, range1, range2, range3, ...
*       options:
*       targetColor - The color of the vertical target marker
*       targetWidth - The width of the target marker in pixels
*       performanceColor - The color of the performance measure horizontal bar
*       rangeColors - Colors to use for each qualitative range background color
*
*   pie - Pie chart. Options:
*       sliceColors - An array of colors to use for pie slices
*       offset - Angle in degrees to offset the first slice - Try -90 or +90
*       borderWidth - Width of border to draw around the pie chart, in pixels - Defaults to 0 (no border)
*       borderColor - Color to use for the pie chart border - Defaults to #000
*
*   box - Box plot. Options:
*       raw - Set to true to supply pre-computed plot points as values
*             values should be: low_outlier, low_whisker, q1, median, q3, high_whisker, high_outlier
*             When set to false you can supply any number of values and the box plot will
*             be computed for you.  Default is false.
*       showOutliers - Set to true (default) to display outliers as circles
*       outlierIQR - Interquartile range used to determine outliers.  Default 1.5
*       boxLineColor - Outline color of the box
*       boxFillColor - Fill color for the box
*       whiskerColor - Line color used for whiskers
*       outlierLineColor - Outline color of outlier circles
*       outlierFillColor - Fill color of the outlier circles
*       spotRadius - Radius of outlier circles
*       medianColor - Line color of the median line
*       target - Draw a target cross hair at the supplied value (default undefined)
*
*
*
*   Examples:
*   $('#sparkline1').sparkline(myvalues, { lineColor: '#f00', fillColor: false });
*   $('.barsparks').sparkline('html', { type:'bar', height:'40px', barWidth:5 });
*   $('#tristate').sparkline([1,1,-1,1,0,0,-1], { type:'tristate' }):
*   $('#discrete').sparkline([1,3,4,5,5,3,4,5], { type:'discrete' });
*   $('#bullet').sparkline([10,12,12,9,7], { type:'bullet' });
*   $('#pie').sparkline([1,1,2], { type:'pie' });
*/

/*jslint regexp: true, browser: true, jquery: true, white: true, nomen: false, plusplus: false, maxerr: 500, indent: 4 */

(function(document, Math, undefined) { // performance/minified-size optimization
(function(factory) {
    if(typeof define === 'function' && define.amd) {
        define(['jquery'], factory);
    } else if (jQuery && !jQuery.fn.sparkline) {
        factory(jQuery);
    }
}
(function($) {
    'use strict';

    var UNSET_OPTION = {},
        getDefaults, createClass, SPFormat, clipval, quartile, normalizeValue, normalizeValues,
        remove, isNumber, all, sum, addCSS, ensureArray, formatNumber, RangeMap,
        MouseHandler, Tooltip, barHighlightMixin,
        line, bar, tristate, discrete, bullet, pie, box, defaultStyles, initStyles,
        VShape, VCanvas_base, VCanvas_canvas, VCanvas_vml, pending, shapeCount = 0;

    /**
     * Default configuration settings
     */
    getDefaults = function () {
        return {
            // Settings common to most/all chart types
            common: {
                type: 'line',
                lineColor: '#00f',
                fillColor: '#cdf',
                defaultPixelsPerValue: 3,
                width: 'auto',
                height: 'auto',
                composite: false,
                tagValuesAttribute: 'values',
                tagOptionsPrefix: 'spark',
                enableTagOptions: false,
                enableHighlight: true,
                highlightLighten: 1.1,
                tooltipSkipNull: true,
                tooltipPrefix: '',
                tooltipSuffix: '',
                disableHiddenCheck: false,
                numberFormatter: false,
                numberDigitGroupCount: 3,
                numberDigitGroupSep: ',',
                numberDecimalMark: '.',
                disableTooltips: false,
                disableInteraction: false
            },
            // Defaults for line charts
            line: {
                spotColor: '#f80',
                highlightSpotColor: '#5f5',
                highlightLineColor: '#f22',
                spotRadius: 1.5,
                minSpotColor: '#f80',
                maxSpotColor: '#f80',
                lineWidth: 1,
                normalRangeMin: undefined,
                normalRangeMax: undefined,
                normalRangeColor: '#ccc',
                drawNormalOnTop: false,
                chartRangeMin: undefined,
                chartRangeMax: undefined,
                chartRangeMinX: undefined,
                chartRangeMaxX: undefined,
                tooltipFormat: new SPFormat('<span style="color: {{color}}">&#9679;</span> {{prefix}}{{y}}{{suffix}}')
            },
            // Defaults for bar charts
            bar: {
                barColor: '#3366cc',
                negBarColor: '#f44',
                stackedBarColor: ['#3366cc', '#dc3912', '#ff9900', '#109618', '#66aa00',
                    '#dd4477', '#0099c6', '#990099'],
                zeroColor: undefined,
                nullColor: undefined,
                zeroAxis: true,
                barWidth: 4,
                barSpacing: 1,
                chartRangeMax: undefined,
                chartRangeMin: undefined,
                chartRangeClip: false,
                colorMap: undefined,
                tooltipFormat: new SPFormat('<span style="color: {{color}}">&#9679;</span> {{prefix}}{{value}}{{suffix}}')
            },
            // Defaults for tristate charts
            tristate: {
                barWidth: 4,
                barSpacing: 1,
                posBarColor: '#6f6',
                negBarColor: '#f44',
                zeroBarColor: '#999',
                colorMap: {},
                tooltipFormat: new SPFormat('<span style="color: {{color}}">&#9679;</span> {{value:map}}'),
                tooltipValueLookups: { map: { '-1': 'Loss', '0': 'Draw', '1': 'Win' } }
            },
            // Defaults for discrete charts
            discrete: {
                lineHeight: 'auto',
                thresholdColor: undefined,
                thresholdValue: 0,
                chartRangeMax: undefined,
                chartRangeMin: undefined,
                chartRangeClip: false,
                tooltipFormat: new SPFormat('{{prefix}}{{value}}{{suffix}}')
            },
            // Defaults for bullet charts
            bullet: {
                targetColor: '#f33',
                targetWidth: 3, // width of the target bar in pixels
                performanceColor: '#33f',
                rangeColors: ['#d3dafe', '#a8b6ff', '#7f94ff'],
                base: undefined, // set this to a number to change the base start number
                tooltipFormat: new SPFormat('{{fieldkey:fields}} - {{value}}'),
                tooltipValueLookups: { fields: {r: 'Range', p: 'Performance', t: 'Target'} }
            },
            // Defaults for pie charts
            pie: {
                offset: 0,
                sliceColors: ['#3366cc', '#dc3912', '#ff9900', '#109618', '#66aa00',
                    '#dd4477', '#0099c6', '#990099'],
                borderWidth: 0,
                borderColor: '#000',
                tooltipFormat: new SPFormat('<span style="color: {{color}}">&#9679;</span> {{value}} ({{percent.1}}%)')
            },
            // Defaults for box plots
            box: {
                raw: false,
                boxLineColor: '#000',
                boxFillColor: '#cdf',
                whiskerColor: '#000',
                outlierLineColor: '#333',
                outlierFillColor: '#fff',
                medianColor: '#f00',
                showOutliers: true,
                outlierIQR: 1.5,
                spotRadius: 1.5,
                target: undefined,
                targetColor: '#4a2',
                chartRangeMax: undefined,
                chartRangeMin: undefined,
                tooltipFormat: new SPFormat('{{field:fields}}: {{value}}'),
                tooltipFormatFieldlistKey: 'field',
                tooltipValueLookups: { fields: { lq: 'Lower Quartile', med: 'Median',
                    uq: 'Upper Quartile', lo: 'Left Outlier', ro: 'Right Outlier',
                    lw: 'Left Whisker', rw: 'Right Whisker'} }
            }
        };
    };

    // You can have tooltips use a css class other than jqstooltip by specifying tooltipClassname
    // tooltip modified by westilian:jaman
    defaultStyles = '.jqstooltip { ' +
            'position: absolute;' +
            'left: 30px;' +
            'top: 0px;' +
            'display: block;' +
            'visibility: hidden;' +
            'background: rgb(0, 0, 0) transparent;' +
            'background-color: rgba(0,0,0,0.6);' +
            'filter:progid:DXImageTransform.Microsoft.gradient(startColorstr=#99000000, endColorstr=#99000000);' +
            '-ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr=#99000000, endColorstr=#99000000)";' +
            'color: white;' +
            'font: 10px arial, san serif;' +
            'text-align: left;' +
            'white-space: nowrap;' +
            
            'border: 0px solid white;' +
            'border-radius: 3px;' +
            '-webkit-border-radius: 3px;' +
            'z-index: 10000;' +
            '}' +
            '.jqsfield { ' +
            'color: white;' +
            'padding: 5px 5px 5px 5px;' +
            'font: 10px arial, san serif;' +
            'text-align: left;' +
            '}';

    /**
     * Utilities
     */

    createClass = function (/* [baseclass, [mixin, ...]], definition */) {
        var Class, args;
        Class = function () {
            this.init.apply(this, arguments);
        };
        if (arguments.length > 1) {
            if (arguments[0]) {
                Class.prototype = $.extend(new arguments[0](), arguments[arguments.length - 1]);
                Class._super = arguments[0].prototype;
            } else {
                Class.prototype = arguments[arguments.length - 1];
            }
            if (arguments.length > 2) {
                args = Array.prototype.slice.call(arguments, 1, -1);
                args.unshift(Class.prototype);
                $.extend.apply($, args);
            }
        } else {
            Class.prototype = arguments[0];
        }
        Class.prototype.cls = Class;
        return Class;
    };

    /**
     * Wraps a format string for tooltips
     * {{x}}
     * {{x.2}
     * {{x:months}}
     */
    $.SPFormatClass = SPFormat = createClass({
        fre: /\{\{([\w.]+?)(:(.+?))?\}\}/g,
        precre: /(\w+)\.(\d+)/,

        init: function (format, fclass) {
            this.format = format;
            this.fclass = fclass;
        },

        render: function (fieldset, lookups, options) {
            var self = this,
                fields = fieldset,
                match, token, lookupkey, fieldvalue, prec;
            return this.format.replace(this.fre, function () {
                var lookup;
                token = arguments[1];
                lookupkey = arguments[3];
                match = self.precre.exec(token);
                if (match) {
                    prec = match[2];
                    token = match[1];
                } else {
                    prec = false;
                }
                fieldvalue = fields[token];
                if (fieldvalue === undefined) {
                    return '';
                }
                if (lookupkey && lookups && lookups[lookupkey]) {
                    lookup = lookups[lookupkey];
                    if (lookup.get) { // RangeMap
                        return lookups[lookupkey].get(fieldvalue) || fieldvalue;
                    } else {
                        return lookups[lookupkey][fieldvalue] || fieldvalue;
                    }
                }
                if (isNumber(fieldvalue)) {
                    if (options.get('numberFormatter')) {
                        fieldvalue = options.get('numberFormatter')(fieldvalue);
                    } else {
                        fieldvalue = formatNumber(fieldvalue, prec,
                            options.get('numberDigitGroupCount'),
                            options.get('numberDigitGroupSep'),
                            options.get('numberDecimalMark'));
                    }
                }
                return fieldvalue;
            });
        }
    });

    // convience method to avoid needing the new operator
    $.spformat = function(format, fclass) {
        return new SPFormat(format, fclass);
    };

    clipval = function (val, min, max) {
        if (val < min) {
            return min;
        }
        if (val > max) {
            return max;
        }
        return val;
    };

    quartile = function (values, q) {
        var vl;
        if (q === 2) {
            vl = Math.floor(values.length / 2);
            return values.length % 2 ? values[vl] : (values[vl-1] + values[vl]) / 2;
        } else {
            if (values.length % 2 ) { // odd
                vl = (values.length * q + q) / 4;
                return vl % 1 ? (values[Math.floor(vl)] + values[Math.floor(vl) - 1]) / 2 : values[vl-1];
            } else { //even
                vl = (values.length * q + 2) / 4;
                return vl % 1 ? (values[Math.floor(vl)] + values[Math.floor(vl) - 1]) / 2 :  values[vl-1];

            }
        }
    };

    normalizeValue = function (val) {
        var nf;
        switch (val) {
            case 'undefined':
                val = undefined;
                break;
            case 'null':
                val = null;
                break;
            case 'true':
                val = true;
                break;
            case 'false':
                val = false;
                break;
            default:
                nf = parseFloat(val);
                if (val == nf) {
                    val = nf;
                }
        }
        return val;
    };

    normalizeValues = function (vals) {
        var i, result = [];
        for (i = vals.length; i--;) {
            result[i] = normalizeValue(vals[i]);
        }
        return result;
    };

    remove = function (vals, filter) {
        var i, vl, result = [];
        for (i = 0, vl = vals.length; i < vl; i++) {
            if (vals[i] !== filter) {
                result.push(vals[i]);
            }
        }
        return result;
    };

    isNumber = function (num) {
        return !isNaN(parseFloat(num)) && isFinite(num);
    };

    formatNumber = function (num, prec, groupsize, groupsep, decsep) {
        var p, i;
        num = (prec === false ? parseFloat(num).toString() : num.toFixed(prec)).split('');
        p = (p = $.inArray('.', num)) < 0 ? num.length : p;
        if (p < num.length) {
            num[p] = decsep;
        }
        for (i = p - groupsize; i > 0; i -= groupsize) {
            num.splice(i, 0, groupsep);
        }
        return num.join('');
    };

    // determine if all values of an array match a value
    // returns true if the array is empty
    all = function (val, arr, ignoreNull) {
        var i;
        for (i = arr.length; i--; ) {
            if (ignoreNull && arr[i] === null) continue;
            if (arr[i] !== val) {
                return false;
            }
        }
        return true;
    };

    // sums the numeric values in an array, ignoring other values
    sum = function (vals) {
        var total = 0, i;
        for (i = vals.length; i--;) {
            total += typeof vals[i] === 'number' ? vals[i] : 0;
        }
        return total;
    };

    ensureArray = function (val) {
        return $.isArray(val) ? val : [val];
    };

    // http://paulirish.com/2008/bookmarklet-inject-new-css-rules/
    addCSS = function(css) {
        var tag;
        //if ('\v' == 'v') /* ie only */ {
        if (document.createStyleSheet) {
            document.createStyleSheet().cssText = css;
        } else {
            tag = document.createElement('style');
            tag.type = 'text/css';
            document.getElementsByTagName('head')[0].appendChild(tag);
            tag[(typeof document.body.style.WebkitAppearance == 'string') /* webkit only */ ? 'innerText' : 'innerHTML'] = css;
        }
    };

    // Provide a cross-browser interface to a few simple drawing primitives
    $.fn.simpledraw = function (width, height, useExisting, interact) {
        var target, mhandler;
        if (useExisting && (target = this.data('_jqs_vcanvas'))) {
            return target;
        }

        if ($.fn.sparkline.canvas === false) {
            // We've already determined that neither Canvas nor VML are available
            return false;

        } else if ($.fn.sparkline.canvas === undefined) {
            // No function defined yet -- need to see if we support Canvas or VML
            var el = document.createElement('canvas');
            if (!!(el.getContext && el.getContext('2d'))) {
                // Canvas is available
                $.fn.sparkline.canvas = function(width, height, target, interact) {
                    return new VCanvas_canvas(width, height, target, interact);
                };
            } else if (document.namespaces && !document.namespaces.v) {
                // VML is available
                document.namespaces.add('v', 'urn:schemas-microsoft-com:vml', '#default#VML');
                $.fn.sparkline.canvas = function(width, height, target, interact) {
                    return new VCanvas_vml(width, height, target);
                };
            } else {
                // Neither Canvas nor VML are available
                $.fn.sparkline.canvas = false;
                return false;
            }
        }

        if (width === undefined) {
            width = $(this).innerWidth();
        }
        if (height === undefined) {
            height = $(this).innerHeight();
        }

        target = $.fn.sparkline.canvas(width, height, this, interact);

        mhandler = $(this).data('_jqs_mhandler');
        if (mhandler) {
            mhandler.registerCanvas(target);
        }
        return target;
    };

    $.fn.cleardraw = function () {
        var target = this.data('_jqs_vcanvas');
        if (target) {
            target.reset();
        }
    };

    $.RangeMapClass = RangeMap = createClass({
        init: function (map) {
            var key, range, rangelist = [];
            for (key in map) {
                if (map.hasOwnProperty(key) && typeof key === 'string' && key.indexOf(':') > -1) {
                    range = key.split(':');
                    range[0] = range[0].length === 0 ? -Infinity : parseFloat(range[0]);
                    range[1] = range[1].length === 0 ? Infinity : parseFloat(range[1]);
                    range[2] = map[key];
                    rangelist.push(range);
                }
            }
            this.map = map;
            this.rangelist = rangelist || false;
        },

        get: function (value) {
            var rangelist = this.rangelist,
                i, range, result;
            if ((result = this.map[value]) !== undefined) {
                return result;
            }
            if (rangelist) {
                for (i = rangelist.length; i--;) {
                    range = rangelist[i];
                    if (range[0] <= value && range[1] >= value) {
                        return range[2];
                    }
                }
            }
            return undefined;
        }
    });

    // Convenience function
    $.range_map = function(map) {
        return new RangeMap(map);
    };

    MouseHandler = createClass({
        init: function (el, options) {
            var $el = $(el);
            this.$el = $el;
            this.options = options;
            this.currentPageX = 0;
            this.currentPageY = 0;
            this.el = el;
            this.splist = [];
            this.tooltip = null;
            this.over = false;
            this.displayTooltips = !options.get('disableTooltips');
            this.highlightEnabled = !options.get('disableHighlight');
        },

        registerSparkline: function (sp) {
            this.splist.push(sp);
            if (this.over) {
                this.updateDisplay();
            }
        },

        registerCanvas: function (canvas) {
            var $canvas = $(canvas.canvas);
            this.canvas = canvas;
            this.$canvas = $canvas;
            $canvas.mouseenter($.proxy(this.mouseenter, this));
            $canvas.mouseleave($.proxy(this.mouseleave, this));
            $canvas.click($.proxy(this.mouseclick, this));
        },

        reset: function (removeTooltip) {
            this.splist = [];
            if (this.tooltip && removeTooltip) {
                this.tooltip.remove();
                this.tooltip = undefined;
            }
        },

        mouseclick: function (e) {
            var clickEvent = $.Event('sparklineClick');
            clickEvent.originalEvent = e;
            clickEvent.sparklines = this.splist;
            this.$el.trigger(clickEvent);
        },

        mouseenter: function (e) {
            $(document.body).unbind('mousemove.jqs');
            $(document.body).bind('mousemove.jqs', $.proxy(this.mousemove, this));
            this.over = true;
            this.currentPageX = e.pageX;
            this.currentPageY = e.pageY;
            this.currentEl = e.target;
            if (!this.tooltip && this.displayTooltips) {
                this.tooltip = new Tooltip(this.options);
                this.tooltip.updatePosition(e.pageX, e.pageY);
            }
            this.updateDisplay();
        },

        mouseleave: function () {
            $(document.body).unbind('mousemove.jqs');
            var splist = this.splist,
                 spcount = splist.length,
                 needsRefresh = false,
                 sp, i;
            this.over = false;
            this.currentEl = null;

            if (this.tooltip) {
                this.tooltip.remove();
                this.tooltip = null;
            }

            for (i = 0; i < spcount; i++) {
                sp = splist[i];
                if (sp.clearRegionHighlight()) {
                    needsRefresh = true;
                }
            }

            if (needsRefresh) {
                this.canvas.render();
            }
        },

        mousemove: function (e) {
            this.currentPageX = e.pageX;
            this.currentPageY = e.pageY;
            this.currentEl = e.target;
            if (this.tooltip) {
                this.tooltip.updatePosition(e.pageX, e.pageY);
            }
            this.updateDisplay();
        },

        updateDisplay: function () {
            var splist = this.splist,
                 spcount = splist.length,
                 needsRefresh = false,
                 offset = this.$canvas.offset(),
                 localX = this.currentPageX - offset.left,
                 localY = this.currentPageY - offset.top,
                 tooltiphtml, sp, i, result, changeEvent;
            if (!this.over) {
                return;
            }
            for (i = 0; i < spcount; i++) {
                sp = splist[i];
                result = sp.setRegionHighlight(this.currentEl, localX, localY);
                if (result) {
                    needsRefresh = true;
                }
            }
            if (needsRefresh) {
                changeEvent = $.Event('sparklineRegionChange');
                changeEvent.sparklines = this.splist;
                this.$el.trigger(changeEvent);
                if (this.tooltip) {
                    tooltiphtml = '';
                    for (i = 0; i < spcount; i++) {
                        sp = splist[i];
                        tooltiphtml += sp.getCurrentRegionTooltip();
                    }
                    this.tooltip.setContent(tooltiphtml);
                }
                if (!this.disableHighlight) {
                    this.canvas.render();
                }
            }
            if (result === null) {
                this.mouseleave();
            }
        }
    });


    Tooltip = createClass({
        sizeStyle: 'position: static !important;' +
            'display: block !important;' +
            'visibility: hidden !important;' +
            'float: left !important;',

        init: function (options) {
            var tooltipClassname = options.get('tooltipClassname', 'jqstooltip'),
                sizetipStyle = this.sizeStyle,
                offset;
            this.container = options.get('tooltipContainer') || document.body;
            this.tooltipOffsetX = options.get('tooltipOffsetX', 10);
            this.tooltipOffsetY = options.get('tooltipOffsetY', 12);
            // remove any previous lingering tooltip
            $('#jqssizetip').remove();
            $('#jqstooltip').remove();
            this.sizetip = $('<div/>', {
                id: 'jqssizetip',
                style: sizetipStyle,
                'class': tooltipClassname
            });
            this.tooltip = $('<div/>', {
                id: 'jqstooltip',
                'class': tooltipClassname
            }).appendTo(this.container);
            // account for the container's location
            offset = this.tooltip.offset();
            this.offsetLeft = offset.left;
            this.offsetTop = offset.top;
            this.hidden = true;
            $(window).unbind('resize.jqs scroll.jqs');
            $(window).bind('resize.jqs scroll.jqs', $.proxy(this.updateWindowDims, this));
            this.updateWindowDims();
        },

        updateWindowDims: function () {
            this.scrollTop = $(window).scrollTop();
            this.scrollLeft = $(window).scrollLeft();
            this.scrollRight = this.scrollLeft + $(window).width();
            this.updatePosition();
        },

        getSize: function (content) {
            this.sizetip.html(content).appendTo(this.container);
            this.width = this.sizetip.width() + 1;
            this.height = this.sizetip.height();
            this.sizetip.remove();
        },

        setContent: function (content) {
            if (!content) {
                this.tooltip.css('visibility', 'hidden');
                this.hidden = true;
                return;
            }
            this.getSize(content);
            this.tooltip.html(content)
                .css({
                    'width': this.width,
                    'height': this.height,
                    'visibility': 'visible'
                });
            if (this.hidden) {
                this.hidden = false;
                this.updatePosition();
            }
        },

        updatePosition: function (x, y) {
            if (x === undefined) {
                if (this.mousex === undefined) {
                    return;
                }
                x = this.mousex - this.offsetLeft;
                y = this.mousey - this.offsetTop;

            } else {
                this.mousex = x = x - this.offsetLeft;
                this.mousey = y = y - this.offsetTop;
            }
            if (!this.height || !this.width || this.hidden) {
                return;
            }

            y -= this.height + this.tooltipOffsetY;
            x += this.tooltipOffsetX;

            if (y < this.scrollTop) {
                y = this.scrollTop;
            }
            if (x < this.scrollLeft) {
                x = this.scrollLeft;
            } else if (x + this.width > this.scrollRight) {
                x = this.scrollRight - this.width;
            }

            this.tooltip.css({
                'left': x,
                'top': y
            });
        },

        remove: function () {
            this.tooltip.remove();
            this.sizetip.remove();
            this.sizetip = this.tooltip = undefined;
            $(window).unbind('resize.jqs scroll.jqs');
        }
    });

    initStyles = function() {
        addCSS(defaultStyles);
    };

    $(initStyles);

    pending = [];
    $.fn.sparkline = function (userValues, userOptions) {
        return this.each(function () {
            var options = new $.fn.sparkline.options(this, userOptions),
                 $this = $(this),
                 render, i;
            render = function () {
                var values, width, height, tmp, mhandler, sp, vals;
                if (userValues === 'html' || userValues === undefined) {
                    vals = this.getAttribute(options.get('tagValuesAttribute'));
                    if (vals === undefined || vals === null) {
                        vals = $this.html();
                    }
                    values = vals.replace(/(^\s*<!--)|(-->\s*$)|\s+/g, '').split(',');
                } else {
                    values = userValues;
                }

                width = options.get('width') === 'auto' ? values.length * options.get('defaultPixelsPerValue') : options.get('width');
                if (options.get('height') === 'auto') {
                    if (!options.get('composite') || !$.data(this, '_jqs_vcanvas')) {
                        // must be a better way to get the line height
                        tmp = document.createElement('span');
                        tmp.innerHTML = 'a';
                        $this.html(tmp);
                        height = $(tmp).innerHeight() || $(tmp).height();
                        $(tmp).remove();
                        tmp = null;
                    }
                } else {
                    height = options.get('height');
                }

                if (!options.get('disableInteraction')) {
                    mhandler = $.data(this, '_jqs_mhandler');
                    if (!mhandler) {
                        mhandler = new MouseHandler(this, options);
                        $.data(this, '_jqs_mhandler', mhandler);
                    } else if (!options.get('composite')) {
                        mhandler.reset();
                    }
                } else {
                    mhandler = false;
                }

                if (options.get('composite') && !$.data(this, '_jqs_vcanvas')) {
                    if (!$.data(this, '_jqs_errnotify')) {
                        alert('Attempted to attach a composite sparkline to an element with no existing sparkline');
                        $.data(this, '_jqs_errnotify', true);
                    }
                    return;
                }

                sp = new $.fn.sparkline[options.get('type')](this, values, options, width, height);

                sp.render();

                if (mhandler) {
                    mhandler.registerSparkline(sp);
                }
            };
            if (($(this).html() && !options.get('disableHiddenCheck') && $(this).is(':hidden')) || !$(this).parents('body').length) {
                if (!options.get('composite') && $.data(this, '_jqs_pending')) {
                    // remove any existing references to the element
                    for (i = pending.length; i; i--) {
                        if (pending[i - 1][0] == this) {
                            pending.splice(i - 1, 1);
                        }
                    }
                }
                pending.push([this, render]);
                $.data(this, '_jqs_pending', true);
            } else {
                render.call(this);
            }
        });
    };

    $.fn.sparkline.defaults = getDefaults();


    $.sparkline_display_visible = function () {
        var el, i, pl;
        var done = [];
        for (i = 0, pl = pending.length; i < pl; i++) {
            el = pending[i][0];
            if ($(el).is(':visible') && !$(el).parents().is(':hidden')) {
                pending[i][1].call(el);
                $.data(pending[i][0], '_jqs_pending', false);
                done.push(i);
            } else if (!$(el).closest('html').length && !$.data(el, '_jqs_pending')) {
                // element has been inserted and removed from the DOM
                // If it was not yet inserted into the dom then the .data request
                // will return true.
                // removing from the dom causes the data to be removed.
                $.data(pending[i][0], '_jqs_pending', false);
                done.push(i);
            }
        }
        for (i = done.length; i; i--) {
            pending.splice(done[i - 1], 1);
        }
    };


    /**
     * User option handler
     */
    $.fn.sparkline.options = createClass({
        init: function (tag, userOptions) {
            var extendedOptions, defaults, base, tagOptionType;
            this.userOptions = userOptions = userOptions || {};
            this.tag = tag;
            this.tagValCache = {};
            defaults = $.fn.sparkline.defaults;
            base = defaults.common;
            this.tagOptionsPrefix = userOptions.enableTagOptions && (userOptions.tagOptionsPrefix || base.tagOptionsPrefix);

            tagOptionType = this.getTagSetting('type');
            if (tagOptionType === UNSET_OPTION) {
                extendedOptions = defaults[userOptions.type || base.type];
            } else {
                extendedOptions = defaults[tagOptionType];
            }
            this.mergedOptions = $.extend({}, base, extendedOptions, userOptions);
        },


        getTagSetting: function (key) {
            var prefix = this.tagOptionsPrefix,
                val, i, pairs, keyval;
            if (prefix === false || prefix === undefined) {
                return UNSET_OPTION;
            }
            if (this.tagValCache.hasOwnProperty(key)) {
                val = this.tagValCache.key;
            } else {
                val = this.tag.getAttribute(prefix + key);
                if (val === undefined || val === null) {
                    val = UNSET_OPTION;
                } else if (val.substr(0, 1) === '[') {
                    val = val.substr(1, val.length - 2).split(',');
                    for (i = val.length; i--;) {
                        val[i] = normalizeValue(val[i].replace(/(^\s*)|(\s*$)/g, ''));
                    }
                } else if (val.substr(0, 1) === '{') {
                    pairs = val.substr(1, val.length - 2).split(',');
                    val = {};
                    for (i = pairs.length; i--;) {
                        keyval = pairs[i].split(':', 2);
                        val[keyval[0].replace(/(^\s*)|(\s*$)/g, '')] = normalizeValue(keyval[1].replace(/(^\s*)|(\s*$)/g, ''));
                    }
                } else {
                    val = normalizeValue(val);
                }
                this.tagValCache.key = val;
            }
            return val;
        },

        get: function (key, defaultval) {
            var tagOption = this.getTagSetting(key),
                result;
            if (tagOption !== UNSET_OPTION) {
                return tagOption;
            }
            return (result = this.mergedOptions[key]) === undefined ? defaultval : result;
        }
    });


    $.fn.sparkline._base = createClass({
        disabled: false,

        init: function (el, values, options, width, height) {
            this.el = el;
            this.$el = $(el);
            this.values = values;
            this.options = options;
            this.width = width;
            this.height = height;
            this.currentRegion = undefined;
        },

        /**
         * Setup the canvas
         */
        initTarget: function () {
            var interactive = !this.options.get('disableInteraction');
            if (!(this.target = this.$el.simpledraw(this.width, this.height, this.options.get('composite'), interactive))) {
                this.disabled = true;
            } else {
                this.canvasWidth = this.target.pixelWidth;
                this.canvasHeight = this.target.pixelHeight;
            }
        },

        /**
         * Actually render the chart to the canvas
         */
        render: function () {
            if (this.disabled) {
                this.el.innerHTML = '';
                return false;
            }
            return true;
        },

        /**
         * Return a region id for a given x/y co-ordinate
         */
        getRegion: function (x, y) {
        },

        /**
         * Highlight an item based on the moused-over x,y co-ordinate
         */
        setRegionHighlight: function (el, x, y) {
            var currentRegion = this.currentRegion,
                highlightEnabled = !this.options.get('disableHighlight'),
                newRegion;
            if (x > this.canvasWidth || y > this.canvasHeight || x < 0 || y < 0) {
                return null;
            }
            newRegion = this.getRegion(el, x, y);
            if (currentRegion !== newRegion) {
                if (currentRegion !== undefined && highlightEnabled) {
                    this.removeHighlight();
                }
                this.currentRegion = newRegion;
                if (newRegion !== undefined && highlightEnabled) {
                    this.renderHighlight();
                }
                return true;
            }
            return false;
        },

        /**
         * Reset any currently highlighted item
         */
        clearRegionHighlight: function () {
            if (this.currentRegion !== undefined) {
                this.removeHighlight();
                this.currentRegion = undefined;
                return true;
            }
            return false;
        },

        renderHighlight: function () {
            this.changeHighlight(true);
        },

        removeHighlight: function () {
            this.changeHighlight(false);
        },

        changeHighlight: function (highlight)  {},

        /**
         * Fetch the HTML to display as a tooltip
         */
        getCurrentRegionTooltip: function () {
            var options = this.options,
                header = '',
                entries = [],
                fields, formats, formatlen, fclass, text, i,
                showFields, showFieldsKey, newFields, fv,
                formatter, format, fieldlen, j;
            if (this.currentRegion === undefined) {
                return '';
            }
            fields = this.getCurrentRegionFields();
            formatter = options.get('tooltipFormatter');
            if (formatter) {
                return formatter(this, options, fields);
            }
            if (options.get('tooltipChartTitle')) {
                header += '<div class="jqs jqstitle">' + options.get('tooltipChartTitle') + '</div>\n';
            }
            formats = this.options.get('tooltipFormat');
            if (!formats) {
                return '';
            }
            if (!$.isArray(formats)) {
                formats = [formats];
            }
            if (!$.isArray(fields)) {
                fields = [fields];
            }
            showFields = this.options.get('tooltipFormatFieldlist');
            showFieldsKey = this.options.get('tooltipFormatFieldlistKey');
            if (showFields && showFieldsKey) {
                // user-selected ordering of fields
                newFields = [];
                for (i = fields.length; i--;) {
                    fv = fields[i][showFieldsKey];
                    if ((j = $.inArray(fv, showFields)) != -1) {
                        newFields[j] = fields[i];
                    }
                }
                fields = newFields;
            }
            formatlen = formats.length;
            fieldlen = fields.length;
            for (i = 0; i < formatlen; i++) {
                format = formats[i];
                if (typeof format === 'string') {
                    format = new SPFormat(format);
                }
                fclass = format.fclass || 'jqsfield';
                for (j = 0; j < fieldlen; j++) {
                    if (!fields[j].isNull || !options.get('tooltipSkipNull')) {
                        $.extend(fields[j], {
                            prefix: options.get('tooltipPrefix'),
                            suffix: options.get('tooltipSuffix')
                        });
                        text = format.render(fields[j], options.get('tooltipValueLookups'), options);
                        entries.push('<div class="' + fclass + '">' + text + '</div>');
                    }
                }
            }
            if (entries.length) {
                return header + entries.join('\n');
            }
            return '';
        },

        getCurrentRegionFields: function () {},

        calcHighlightColor: function (color, options) {
            var highlightColor = options.get('highlightColor'),
                lighten = options.get('highlightLighten'),
                parse, mult, rgbnew, i;
            if (highlightColor) {
                return highlightColor;
            }
            if (lighten) {
                // extract RGB values
                parse = /^#([0-9a-f])([0-9a-f])([0-9a-f])$/i.exec(color) || /^#([0-9a-f]{2})([0-9a-f]{2})([0-9a-f]{2})$/i.exec(color);
                if (parse) {
                    rgbnew = [];
                    mult = color.length === 4 ? 16 : 1;
                    for (i = 0; i < 3; i++) {
                        rgbnew[i] = clipval(Math.round(parseInt(parse[i + 1], 16) * mult * lighten), 0, 255);
                    }
                    return 'rgb(' + rgbnew.join(',') + ')';
                }

            }
            return color;
        }

    });

    barHighlightMixin = {
        changeHighlight: function (highlight) {
            var currentRegion = this.currentRegion,
                target = this.target,
                shapeids = this.regionShapes[currentRegion],
                newShapes;
            // will be null if the region value was null
            if (shapeids) {
                newShapes = this.renderRegion(currentRegion, highlight);
                if ($.isArray(newShapes) || $.isArray(shapeids)) {
                    target.replaceWithShapes(shapeids, newShapes);
                    this.regionShapes[currentRegion] = $.map(newShapes, function (newShape) {
                        return newShape.id;
                    });
                } else {
                    target.replaceWithShape(shapeids, newShapes);
                    this.regionShapes[currentRegion] = newShapes.id;
                }
            }
        },

        render: function () {
            var values = this.values,
                target = this.target,
                regionShapes = this.regionShapes,
                shapes, ids, i, j;

            if (!this.cls._super.render.call(this)) {
                return;
            }
            for (i = values.length; i--;) {
                shapes = this.renderRegion(i);
                if (shapes) {
                    if ($.isArray(shapes)) {
                        ids = [];
                        for (j = shapes.length; j--;) {
                            shapes[j].append();
                            ids.push(shapes[j].id);
                        }
                        regionShapes[i] = ids;
                    } else {
                        shapes.append();
                        regionShapes[i] = shapes.id; // store just the shapeid
                    }
                } else {
                    // null value
                    regionShapes[i] = null;
                }
            }
            target.render();
        }
    };

    /**
     * Line charts
     */
    $.fn.sparkline.line = line = createClass($.fn.sparkline._base, {
        type: 'line',

        init: function (el, values, options, width, height) {
            line._super.init.call(this, el, values, options, width, height);
            this.vertices = [];
            this.regionMap = [];
            this.xvalues = [];
            this.yvalues = [];
            this.yminmax = [];
            this.hightlightSpotId = null;
            this.lastShapeId = null;
            this.initTarget();
        },

        getRegion: function (el, x, y) {
            var i,
                regionMap = this.regionMap; // maps regions to value positions
            for (i = regionMap.length; i--;) {
                if (regionMap[i] !== null && x >= regionMap[i][0] && x <= regionMap[i][1]) {
                    return regionMap[i][2];
                }
            }
            return undefined;
        },

        getCurrentRegionFields: function () {
            var currentRegion = this.currentRegion;
            return {
                isNull: this.yvalues[currentRegion] === null,
                x: this.xvalues[currentRegion],
                y: this.yvalues[currentRegion],
                color: this.options.get('lineColor'),
                fillColor: this.options.get('fillColor'),
                offset: currentRegion
            };
        },

        renderHighlight: function () {
            var currentRegion = this.currentRegion,
                target = this.target,
                vertex = this.vertices[currentRegion],
                options = this.options,
                spotRadius = options.get('spotRadius'),
                highlightSpotColor = options.get('highlightSpotColor'),
                highlightLineColor = options.get('highlightLineColor'),
                highlightSpot, highlightLine;

            if (!vertex) {
                return;
            }
            if (spotRadius && highlightSpotColor) {
                highlightSpot = target.drawCircle(vertex[0], vertex[1],
                    spotRadius, undefined, highlightSpotColor);
                this.highlightSpotId = highlightSpot.id;
                target.insertAfterShape(this.lastShapeId, highlightSpot);
            }
            if (highlightLineColor) {
                highlightLine = target.drawLine(vertex[0], this.canvasTop, vertex[0],
                    this.canvasTop + this.canvasHeight, highlightLineColor);
                this.highlightLineId = highlightLine.id;
                target.insertAfterShape(this.lastShapeId, highlightLine);
            }
        },

        removeHighlight: function () {
            var target = this.target;
            if (this.highlightSpotId) {
                target.removeShapeId(this.highlightSpotId);
                this.highlightSpotId = null;
            }
            if (this.highlightLineId) {
                target.removeShapeId(this.highlightLineId);
                this.highlightLineId = null;
            }
        },

        scanValues: function () {
            var values = this.values,
                valcount = values.length,
                xvalues = this.xvalues,
                yvalues = this.yvalues,
                yminmax = this.yminmax,
                i, val, isStr, isArray, sp;
            for (i = 0; i < valcount; i++) {
                val = values[i];
                isStr = typeof(values[i]) === 'string';
                isArray = typeof(values[i]) === 'object' && values[i] instanceof Array;
                sp = isStr && values[i].split(':');
                if (isStr && sp.length === 2) { // x:y
                    xvalues.push(Number(sp[0]));
                    yvalues.push(Number(sp[1]));
                    yminmax.push(Number(sp[1]));
                } else if (isArray) {
                    xvalues.push(val[0]);
                    yvalues.push(val[1]);
                    yminmax.push(val[1]);
                } else {
                    xvalues.push(i);
                    if (values[i] === null || values[i] === 'null') {
                        yvalues.push(null);
                    } else {
                        yvalues.push(Number(val));
                        yminmax.push(Number(val));
                    }
                }
            }
            if (this.options.get('xvalues')) {
                xvalues = this.options.get('xvalues');
            }

            this.maxy = this.maxyorg = Math.max.apply(Math, yminmax);
            this.miny = this.minyorg = Math.min.apply(Math, yminmax);

            this.maxx = Math.max.apply(Math, xvalues);
            this.minx = Math.min.apply(Math, xvalues);

            this.xvalues = xvalues;
            this.yvalues = yvalues;
            this.yminmax = yminmax;

        },

        processRangeOptions: function () {
            var options = this.options,
                normalRangeMin = options.get('normalRangeMin'),
                normalRangeMax = options.get('normalRangeMax');

            if (normalRangeMin !== undefined) {
                if (normalRangeMin < this.miny) {
                    this.miny = normalRangeMin;
                }
                if (normalRangeMax > this.maxy) {
                    this.maxy = normalRangeMax;
                }
            }
            if (options.get('chartRangeMin') !== undefined && (options.get('chartRangeClip') || options.get('chartRangeMin') < this.miny)) {
                this.miny = options.get('chartRangeMin');
            }
            if (options.get('chartRangeMax') !== undefined && (options.get('chartRangeClip') || options.get('chartRangeMax') > this.maxy)) {
                this.maxy = options.get('chartRangeMax');
            }
            if (options.get('chartRangeMinX') !== undefined && (options.get('chartRangeClipX') || options.get('chartRangeMinX') < this.minx)) {
                this.minx = options.get('chartRangeMinX');
            }
            if (options.get('chartRangeMaxX') !== undefined && (options.get('chartRangeClipX') || options.get('chartRangeMaxX') > this.maxx)) {
                this.maxx = options.get('chartRangeMaxX');
            }

        },

        drawNormalRange: function (canvasLeft, canvasTop, canvasHeight, canvasWidth, rangey) {
            var normalRangeMin = this.options.get('normalRangeMin'),
                normalRangeMax = this.options.get('normalRangeMax'),
                ytop = canvasTop + Math.round(canvasHeight - (canvasHeight * ((normalRangeMax - this.miny) / rangey))),
                height = Math.round((canvasHeight * (normalRangeMax - normalRangeMin)) / rangey);
            this.target.drawRect(canvasLeft, ytop, canvasWidth, height, undefined, this.options.get('normalRangeColor')).append();
        },

        render: function () {
            var options = this.options,
                target = this.target,
                canvasWidth = this.canvasWidth,
                canvasHeight = this.canvasHeight,
                vertices = this.vertices,
                spotRadius = options.get('spotRadius'),
                regionMap = this.regionMap,
                rangex, rangey, yvallast,
                canvasTop, canvasLeft,
                vertex, path, paths, x, y, xnext, xpos, xposnext,
                last, next, yvalcount, lineShapes, fillShapes, plen,
                valueSpots, hlSpotsEnabled, color, xvalues, yvalues, i;

            if (!line._super.render.call(this)) {
                return;
            }

            this.scanValues();
            this.processRangeOptions();

            xvalues = this.xvalues;
            yvalues = this.yvalues;

            if (!this.yminmax.length || this.yvalues.length < 2) {
                // empty or all null valuess
                return;
            }

            canvasTop = canvasLeft = 0;

            rangex = this.maxx - this.minx === 0 ? 1 : this.maxx - this.minx;
            rangey = this.maxy - this.miny === 0 ? 1 : this.maxy - this.miny;
            yvallast = this.yvalues.length - 1;

            if (spotRadius && (canvasWidth < (spotRadius * 4) || canvasHeight < (spotRadius * 4))) {
                spotRadius = 0;
            }
            if (spotRadius) {
                // adjust the canvas size as required so that spots will fit
                hlSpotsEnabled = options.get('highlightSpotColor') &&  !options.get('disableInteraction');
                if (hlSpotsEnabled || options.get('minSpotColor') || (options.get('spotColor') && yvalues[yvallast] === this.miny)) {
                    canvasHeight -= Math.ceil(spotRadius);
                }
                if (hlSpotsEnabled || options.get('maxSpotColor') || (options.get('spotColor') && yvalues[yvallast] === this.maxy)) {
                    canvasHeight -= Math.ceil(spotRadius);
                    canvasTop += Math.ceil(spotRadius);
                }
                if (hlSpotsEnabled ||
                     ((options.get('minSpotColor') || options.get('maxSpotColor')) && (yvalues[0] === this.miny || yvalues[0] === this.maxy))) {
                    canvasLeft += Math.ceil(spotRadius);
                    canvasWidth -= Math.ceil(spotRadius);
                }
                if (hlSpotsEnabled || options.get('spotColor') ||
                    (options.get('minSpotColor') || options.get('maxSpotColor') &&
                        (yvalues[yvallast] === this.miny || yvalues[yvallast] === this.maxy))) {
                    canvasWidth -= Math.ceil(spotRadius);
                }
            }


            canvasHeight--;

            if (options.get('normalRangeMin') !== undefined && !options.get('drawNormalOnTop')) {
                this.drawNormalRange(canvasLeft, canvasTop, canvasHeight, canvasWidth, rangey);
            }

            path = [];
            paths = [path];
            last = next = null;
            yvalcount = yvalues.length;
            for (i = 0; i < yvalcount; i++) {
                x = xvalues[i];
                xnext = xvalues[i + 1];
                y = yvalues[i];
                xpos = canvasLeft + Math.round((x - this.minx) * (canvasWidth / rangex));
                xposnext = i < yvalcount - 1 ? canvasLeft + Math.round((xnext - this.minx) * (canvasWidth / rangex)) : canvasWidth;
                next = xpos + ((xposnext - xpos) / 2);
                regionMap[i] = [last || 0, next, i];
                last = next;
                if (y === null) {
                    if (i) {
                        if (yvalues[i - 1] !== null) {
                            path = [];
                            paths.push(path);
                        }
                        vertices.push(null);
                    }
                } else {
                    if (y < this.miny) {
                        y = this.miny;
                    }
                    if (y > this.maxy) {
                        y = this.maxy;
                    }
                    if (!path.length) {
                        // previous value was null
                        path.push([xpos, canvasTop + canvasHeight]);
                    }
                    vertex = [xpos, canvasTop + Math.round(canvasHeight - (canvasHeight * ((y - this.miny) / rangey)))];
                    path.push(vertex);
                    vertices.push(vertex);
                }
            }

            lineShapes = [];
            fillShapes = [];
            plen = paths.length;
            for (i = 0; i < plen; i++) {
                path = paths[i];
                if (path.length) {
                    if (options.get('fillColor')) {
                        path.push([path[path.length - 1][0], (canvasTop + canvasHeight)]);
                        fillShapes.push(path.slice(0));
                        path.pop();
                    }
                    // if there's only a single point in this path, then we want to display it
                    // as a vertical line which means we keep path[0]  as is
                    if (path.length > 2) {
                        // else we want the first value
                        path[0] = [path[0][0], path[1][1]];
                    }
                    lineShapes.push(path);
                }
            }

            // draw the fill first, then optionally the normal range, then the line on top of that
            plen = fillShapes.length;
            for (i = 0; i < plen; i++) {
                target.drawShape(fillShapes[i],
                    options.get('fillColor'), options.get('fillColor')).append();
            }

            if (options.get('normalRangeMin') !== undefined && options.get('drawNormalOnTop')) {
                this.drawNormalRange(canvasLeft, canvasTop, canvasHeight, canvasWidth, rangey);
            }

            plen = lineShapes.length;
            for (i = 0; i < plen; i++) {
                target.drawShape(lineShapes[i], options.get('lineColor'), undefined,
                    options.get('lineWidth')).append();
            }

            if (spotRadius && options.get('valueSpots')) {
                valueSpots = options.get('valueSpots');
                if (valueSpots.get === undefined) {
                    valueSpots = new RangeMap(valueSpots);
                }
                for (i = 0; i < yvalcount; i++) {
                    color = valueSpots.get(yvalues[i]);
                    if (color) {
                        target.drawCircle(canvasLeft + Math.round((xvalues[i] - this.minx) * (canvasWidth / rangex)),
                            canvasTop + Math.round(canvasHeight - (canvasHeight * ((yvalues[i] - this.miny) / rangey))),
                            spotRadius, undefined,
                            color).append();
                    }
                }

            }
            if (spotRadius && options.get('spotColor') && yvalues[yvallast] !== null) {
                target.drawCircle(canvasLeft + Math.round((xvalues[xvalues.length - 1] - this.minx) * (canvasWidth / rangex)),
                    canvasTop + Math.round(canvasHeight - (canvasHeight * ((yvalues[yvallast] - this.miny) / rangey))),
                    spotRadius, undefined,
                    options.get('spotColor')).append();
            }
            if (this.maxy !== this.minyorg) {
                if (spotRadius && options.get('minSpotColor')) {
                    x = xvalues[$.inArray(this.minyorg, yvalues)];
                    target.drawCircle(canvasLeft + Math.round((x - this.minx) * (canvasWidth / rangex)),
                        canvasTop + Math.round(canvasHeight - (canvasHeight * ((this.minyorg - this.miny) / rangey))),
                        spotRadius, undefined,
                        options.get('minSpotColor')).append();
                }
                if (spotRadius && options.get('maxSpotColor')) {
                    x = xvalues[$.inArray(this.maxyorg, yvalues)];
                    target.drawCircle(canvasLeft + Math.round((x - this.minx) * (canvasWidth / rangex)),
                        canvasTop + Math.round(canvasHeight - (canvasHeight * ((this.maxyorg - this.miny) / rangey))),
                        spotRadius, undefined,
                        options.get('maxSpotColor')).append();
                }
            }

            this.lastShapeId = target.getLastShapeId();
            this.canvasTop = canvasTop;
            target.render();
        }
    });

    /**
     * Bar charts
     */
    $.fn.sparkline.bar = bar = createClass($.fn.sparkline._base, barHighlightMixin, {
        type: 'bar',

        init: function (el, values, options, width, height) {
            var barWidth = parseInt(options.get('barWidth'), 10),
                barSpacing = parseInt(options.get('barSpacing'), 10),
                chartRangeMin = options.get('chartRangeMin'),
                chartRangeMax = options.get('chartRangeMax'),
                chartRangeClip = options.get('chartRangeClip'),
                stackMin = Infinity,
                stackMax = -Infinity,
                isStackString, groupMin, groupMax, stackRanges,
                numValues, i, vlen, range, zeroAxis, xaxisOffset, min, max, clipMin, clipMax,
                stacked, vlist, j, slen, svals, val, yoffset, yMaxCalc, canvasHeightEf;
            bar._super.init.call(this, el, values, options, width, height);

            // scan values to determine whether to stack bars
            for (i = 0, vlen = values.length; i < vlen; i++) {
                val = values[i];
                isStackString = typeof(val) === 'string' && val.indexOf(':') > -1;
                if (isStackString || $.isArray(val)) {
                    stacked = true;
                    if (isStackString) {
                        val = values[i] = normalizeValues(val.split(':'));
                    }
                    val = remove(val, null); // min/max will treat null as zero
                    groupMin = Math.min.apply(Math, val);
                    groupMax = Math.max.apply(Math, val);
                    if (groupMin < stackMin) {
                        stackMin = groupMin;
                    }
                    if (groupMax > stackMax) {
                        stackMax = groupMax;
                    }
                }
            }

            this.stacked = stacked;
            this.regionShapes = {};
            this.barWidth = barWidth;
            this.barSpacing = barSpacing;
            this.totalBarWidth = barWidth + barSpacing;
            this.width = width = (values.length * barWidth) + ((values.length - 1) * barSpacing);

            this.initTarget();

            if (chartRangeClip) {
                clipMin = chartRangeMin === undefined ? -Infinity : chartRangeMin;
                clipMax = chartRangeMax === undefined ? Infinity : chartRangeMax;
            }

            numValues = [];
            stackRanges = stacked ? [] : numValues;
            var stackTotals = [];
            var stackRangesNeg = [];
            for (i = 0, vlen = values.length; i < vlen; i++) {
                if (stacked) {
                    vlist = values[i];
                    values[i] = svals = [];
                    stackTotals[i] = 0;
                    stackRanges[i] = stackRangesNeg[i] = 0;
                    for (j = 0, slen = vlist.length; j < slen; j++) {
                        val = svals[j] = chartRangeClip ? clipval(vlist[j], clipMin, clipMax) : vlist[j];
                        if (val !== null) {
                            if (val > 0) {
                                stackTotals[i] += val;
                            }
                            if (stackMin < 0 && stackMax > 0) {
                                if (val < 0) {
                                    stackRangesNeg[i] += Math.abs(val);
                                } else {
                                    stackRanges[i] += val;
                                }
                            } else {
                                stackRanges[i] += Math.abs(val - (val < 0 ? stackMax : stackMin));
                            }
                            numValues.push(val);
                        }
                    }
                } else {
                    val = chartRangeClip ? clipval(values[i], clipMin, clipMax) : values[i];
                    val = values[i] = normalizeValue(val);
                    if (val !== null) {
                        numValues.push(val);
                    }
                }
            }
            this.max = max = Math.max.apply(Math, numValues);
            this.min = min = Math.min.apply(Math, numValues);
            this.stackMax = stackMax = stacked ? Math.max.apply(Math, stackTotals) : max;
            this.stackMin = stackMin = stacked ? Math.min.apply(Math, numValues) : min;

            if (options.get('chartRangeMin') !== undefined && (options.get('chartRangeClip') || options.get('chartRangeMin') < min)) {
                min = options.get('chartRangeMin');
            }
            if (options.get('chartRangeMax') !== undefined && (options.get('chartRangeClip') || options.get('chartRangeMax') > max)) {
                max = options.get('chartRangeMax');
            }

            this.zeroAxis = zeroAxis = options.get('zeroAxis', true);
            if (min <= 0 && max >= 0 && zeroAxis) {
                xaxisOffset = 0;
            } else if (zeroAxis == false) {
                xaxisOffset = min;
            } else if (min > 0) {
                xaxisOffset = min;
            } else {
                xaxisOffset = max;
            }
            this.xaxisOffset = xaxisOffset;

            range = stacked ? (Math.max.apply(Math, stackRanges) + Math.max.apply(Math, stackRangesNeg)) : max - min;

            // as we plot zero/min values a single pixel line, we add a pixel to all other
            // values - Reduce the effective canvas size to suit
            this.canvasHeightEf = (zeroAxis && min < 0) ? this.canvasHeight - 2 : this.canvasHeight - 1;

            if (min < xaxisOffset) {
                yMaxCalc = (stacked && max >= 0) ? stackMax : max;
                yoffset = (yMaxCalc - xaxisOffset) / range * this.canvasHeight;
                if (yoffset !== Math.ceil(yoffset)) {
                    this.canvasHeightEf -= 2;
                    yoffset = Math.ceil(yoffset);
                }
            } else {
                yoffset = this.canvasHeight;
            }
            this.yoffset = yoffset;

            if ($.isArray(options.get('colorMap'))) {
                this.colorMapByIndex = options.get('colorMap');
                this.colorMapByValue = null;
            } else {
                this.colorMapByIndex = null;
                this.colorMapByValue = options.get('colorMap');
                if (this.colorMapByValue && this.colorMapByValue.get === undefined) {
                    this.colorMapByValue = new RangeMap(this.colorMapByValue);
                }
            }

            this.range = range;
        },

        getRegion: function (el, x, y) {
            var result = Math.floor(x / this.totalBarWidth);
            return (result < 0 || result >= this.values.length) ? undefined : result;
        },

        getCurrentRegionFields: function () {
            var currentRegion = this.currentRegion,
                values = ensureArray(this.values[currentRegion]),
                result = [],
                value, i;
            for (i = values.length; i--;) {
                value = values[i];
                result.push({
                    isNull: value === null,
                    value: value,
                    color: this.calcColor(i, value, currentRegion),
                    offset: currentRegion
                });
            }
            return result;
        },

        calcColor: function (stacknum, value, valuenum) {
            var colorMapByIndex = this.colorMapByIndex,
                colorMapByValue = this.colorMapByValue,
                options = this.options,
                color, newColor;
            if (this.stacked) {
                color = options.get('stackedBarColor');
            } else {
                color = (value < 0) ? options.get('negBarColor') : options.get('barColor');
            }
            if (value === 0 && options.get('zeroColor') !== undefined) {
                color = options.get('zeroColor');
            }
            if (colorMapByValue && (newColor = colorMapByValue.get(value))) {
                color = newColor;
            } else if (colorMapByIndex && colorMapByIndex.length > valuenum) {
                color = colorMapByIndex[valuenum];
            }
            return $.isArray(color) ? color[stacknum % color.length] : color;
        },

        /**
         * Render bar(s) for a region
         */
        renderRegion: function (valuenum, highlight) {
            var vals = this.values[valuenum],
                options = this.options,
                xaxisOffset = this.xaxisOffset,
                result = [],
                range = this.range,
                stacked = this.stacked,
                target = this.target,
                x = valuenum * this.totalBarWidth,
                canvasHeightEf = this.canvasHeightEf,
                yoffset = this.yoffset,
                y, height, color, isNull, yoffsetNeg, i, valcount, val, minPlotted, allMin;

            vals = $.isArray(vals) ? vals : [vals];
            valcount = vals.length;
            val = vals[0];
            isNull = all(null, vals);
            allMin = all(xaxisOffset, vals, true);

            if (isNull) {
                if (options.get('nullColor')) {
                    color = highlight ? options.get('nullColor') : this.calcHighlightColor(options.get('nullColor'), options);
                    y = (yoffset > 0) ? yoffset - 1 : yoffset;
                    return target.drawRect(x, y, this.barWidth - 1, 0, color, color);
                } else {
                    return undefined;
                }
            }
            yoffsetNeg = yoffset;
            for (i = 0; i < valcount; i++) {
                val = vals[i];

                if (stacked && val === xaxisOffset) {
                    if (!allMin || minPlotted) {
                        continue;
                    }
                    minPlotted = true;
                }

                if (range > 0) {
                    height = Math.floor(canvasHeightEf * ((Math.abs(val - xaxisOffset) / range))) + 1;
                } else {
                    height = 1;
                }
                if (val < xaxisOffset || (val === xaxisOffset && yoffset === 0)) {
                    y = yoffsetNeg;
                    yoffsetNeg += height;
                } else {
                    y = yoffset - height;
                    yoffset -= height;
                }
                color = this.calcColor(i, val, valuenum);
                if (highlight) {
                    color = this.calcHighlightColor(color, options);
                }
                result.push(target.drawRect(x, y, this.barWidth - 1, height - 1, color, color));
            }
            if (result.length === 1) {
                return result[0];
            }
            return result;
        }
    });

    /**
     * Tristate charts
     */
    $.fn.sparkline.tristate = tristate = createClass($.fn.sparkline._base, barHighlightMixin, {
        type: 'tristate',

        init: function (el, values, options, width, height) {
            var barWidth = parseInt(options.get('barWidth'), 10),
                barSpacing = parseInt(options.get('barSpacing'), 10);
            tristate._super.init.call(this, el, values, options, width, height);

            this.regionShapes = {};
            this.barWidth = barWidth;
            this.barSpacing = barSpacing;
            this.totalBarWidth = barWidth + barSpacing;
            this.values = $.map(values, Number);
            this.width = width = (values.length * barWidth) + ((values.length - 1) * barSpacing);

            if ($.isArray(options.get('colorMap'))) {
                this.colorMapByIndex = options.get('colorMap');
                this.colorMapByValue = null;
            } else {
                this.colorMapByIndex = null;
                this.colorMapByValue = options.get('colorMap');
                if (this.colorMapByValue && this.colorMapByValue.get === undefined) {
                    this.colorMapByValue = new RangeMap(this.colorMapByValue);
                }
            }
            this.initTarget();
        },

        getRegion: function (el, x, y) {
            return Math.floor(x / this.totalBarWidth);
        },

        getCurrentRegionFields: function () {
            var currentRegion = this.currentRegion;
            return {
                isNull: this.values[currentRegion] === undefined,
                value: this.values[currentRegion],
                color: this.calcColor(this.values[currentRegion], currentRegion),
                offset: currentRegion
            };
        },

        calcColor: function (value, valuenum) {
            var values = this.values,
                options = this.options,
                colorMapByIndex = this.colorMapByIndex,
                colorMapByValue = this.colorMapByValue,
                color, newColor;

            if (colorMapByValue && (newColor = colorMapByValue.get(value))) {
                color = newColor;
            } else if (colorMapByIndex && colorMapByIndex.length > valuenum) {
                color = colorMapByIndex[valuenum];
            } else if (values[valuenum] < 0) {
                color = options.get('negBarColor');
            } else if (values[valuenum] > 0) {
                color = options.get('posBarColor');
            } else {
                color = options.get('zeroBarColor');
            }
            return color;
        },

        renderRegion: function (valuenum, highlight) {
            var values = this.values,
                options = this.options,
                target = this.target,
                canvasHeight, height, halfHeight,
                x, y, color;

            canvasHeight = target.pixelHeight;
            halfHeight = Math.round(canvasHeight / 2);

            x = valuenum * this.totalBarWidth;
            if (values[valuenum] < 0) {
                y = halfHeight;
                height = halfHeight - 1;
            } else if (values[valuenum] > 0) {
                y = 0;
                height = halfHeight - 1;
            } else {
                y = halfHeight - 1;
                height = 2;
            }
            color = this.calcColor(values[valuenum], valuenum);
            if (color === null) {
                return;
            }
            if (highlight) {
                color = this.calcHighlightColor(color, options);
            }
            return target.drawRect(x, y, this.barWidth - 1, height - 1, color, color);
        }
    });

    /**
     * Discrete charts
     */
    $.fn.sparkline.discrete = discrete = createClass($.fn.sparkline._base, barHighlightMixin, {
        type: 'discrete',

        init: function (el, values, options, width, height) {
            discrete._super.init.call(this, el, values, options, width, height);

            this.regionShapes = {};
            this.values = values = $.map(values, Number);
            this.min = Math.min.apply(Math, values);
            this.max = Math.max.apply(Math, values);
            this.range = this.max - this.min;
            this.width = width = options.get('width') === 'auto' ? values.length * 2 : this.width;
            this.interval = Math.floor(width / values.length);
            this.itemWidth = width / values.length;
            if (options.get('chartRangeMin') !== undefined && (options.get('chartRangeClip') || options.get('chartRangeMin') < this.min)) {
                this.min = options.get('chartRangeMin');
            }
            if (options.get('chartRangeMax') !== undefined && (options.get('chartRangeClip') || options.get('chartRangeMax') > this.max)) {
                this.max = options.get('chartRangeMax');
            }
            this.initTarget();
            if (this.target) {
                this.lineHeight = options.get('lineHeight') === 'auto' ? Math.round(this.canvasHeight * 0.3) : options.get('lineHeight');
            }
        },

        getRegion: function (el, x, y) {
            return Math.floor(x / this.itemWidth);
        },

        getCurrentRegionFields: function () {
            var currentRegion = this.currentRegion;
            return {
                isNull: this.values[currentRegion] === undefined,
                value: this.values[currentRegion],
                offset: currentRegion
            };
        },

        renderRegion: function (valuenum, highlight) {
            var values = this.values,
                options = this.options,
                min = this.min,
                max = this.max,
                range = this.range,
                interval = this.interval,
                target = this.target,
                canvasHeight = this.canvasHeight,
                lineHeight = this.lineHeight,
                pheight = canvasHeight - lineHeight,
                ytop, val, color, x;

            val = clipval(values[valuenum], min, max);
            x = valuenum * interval;
            ytop = Math.round(pheight - pheight * ((val - min) / range));
            color = (options.get('thresholdColor') && val < options.get('thresholdValue')) ? options.get('thresholdColor') : options.get('lineColor');
            if (highlight) {
                color = this.calcHighlightColor(color, options);
            }
            return target.drawLine(x, ytop, x, ytop + lineHeight, color);
        }
    });

    /**
     * Bullet charts
     */
    $.fn.sparkline.bullet = bullet = createClass($.fn.sparkline._base, {
        type: 'bullet',

        init: function (el, values, options, width, height) {
            var min, max, vals;
            bullet._super.init.call(this, el, values, options, width, height);

            // values: target, performance, range1, range2, range3
            this.values = values = normalizeValues(values);
            // target or performance could be null
            vals = values.slice();
            vals[0] = vals[0] === null ? vals[2] : vals[0];
            vals[1] = values[1] === null ? vals[2] : vals[1];
            min = Math.min.apply(Math, values);
            max = Math.max.apply(Math, values);
            if (options.get('base') === undefined) {
                min = min < 0 ? min : 0;
            } else {
                min = options.get('base');
            }
            this.min = min;
            this.max = max;
            this.range = max - min;
            this.shapes = {};
            this.valueShapes = {};
            this.regiondata = {};
            this.width = width = options.get('width') === 'auto' ? '4.0em' : width;
            this.target = this.$el.simpledraw(width, height, options.get('composite'));
            if (!values.length) {
                this.disabled = true;
            }
            this.initTarget();
        },

        getRegion: function (el, x, y) {
            var shapeid = this.target.getShapeAt(el, x, y);
            return (shapeid !== undefined && this.shapes[shapeid] !== undefined) ? this.shapes[shapeid] : undefined;
        },

        getCurrentRegionFields: function () {
            var currentRegion = this.currentRegion;
            return {
                fieldkey: currentRegion.substr(0, 1),
                value: this.values[currentRegion.substr(1)],
                region: currentRegion
            };
        },

        changeHighlight: function (highlight) {
            var currentRegion = this.currentRegion,
                shapeid = this.valueShapes[currentRegion],
                shape;
            delete this.shapes[shapeid];
            switch (currentRegion.substr(0, 1)) {
                case 'r':
                    shape = this.renderRange(currentRegion.substr(1), highlight);
                    break;
                case 'p':
                    shape = this.renderPerformance(highlight);
                    break;
                case 't':
                    shape = this.renderTarget(highlight);
                    break;
            }
            this.valueShapes[currentRegion] = shape.id;
            this.shapes[shape.id] = currentRegion;
            this.target.replaceWithShape(shapeid, shape);
        },

        renderRange: function (rn, highlight) {
            var rangeval = this.values[rn],
                rangewidth = Math.round(this.canvasWidth * ((rangeval - this.min) / this.range)),
                color = this.options.get('rangeColors')[rn - 2];
            if (highlight) {
                color = this.calcHighlightColor(color, this.options);
            }
            return this.target.drawRect(0, 0, rangewidth - 1, this.canvasHeight - 1, color, color);
        },

        renderPerformance: function (highlight) {
            var perfval = this.values[1],
                perfwidth = Math.round(this.canvasWidth * ((perfval - this.min) / this.range)),
                color = this.options.get('performanceColor');
            if (highlight) {
                color = this.calcHighlightColor(color, this.options);
            }
            return this.target.drawRect(0, Math.round(this.canvasHeight * 0.3), perfwidth - 1,
                Math.round(this.canvasHeight * 0.4) - 1, color, color);
        },

        renderTarget: function (highlight) {
            var targetval = this.values[0],
                x = Math.round(this.canvasWidth * ((targetval - this.min) / this.range) - (this.options.get('targetWidth') / 2)),
                targettop = Math.round(this.canvasHeight * 0.10),
                targetheight = this.canvasHeight - (targettop * 2),
                color = this.options.get('targetColor');
            if (highlight) {
                color = this.calcHighlightColor(color, this.options);
            }
            return this.target.drawRect(x, targettop, this.options.get('targetWidth') - 1, targetheight - 1, color, color);
        },

        render: function () {
            var vlen = this.values.length,
                target = this.target,
                i, shape;
            if (!bullet._super.render.call(this)) {
                return;
            }
            for (i = 2; i < vlen; i++) {
                shape = this.renderRange(i).append();
                this.shapes[shape.id] = 'r' + i;
                this.valueShapes['r' + i] = shape.id;
            }
            if (this.values[1] !== null) {
                shape = this.renderPerformance().append();
                this.shapes[shape.id] = 'p1';
                this.valueShapes.p1 = shape.id;
            }
            if (this.values[0] !== null) {
                shape = this.renderTarget().append();
                this.shapes[shape.id] = 't0';
                this.valueShapes.t0 = shape.id;
            }
            target.render();
        }
    });

    /**
     * Pie charts
     */
    $.fn.sparkline.pie = pie = createClass($.fn.sparkline._base, {
        type: 'pie',

        init: function (el, values, options, width, height) {
            var total = 0, i;

            pie._super.init.call(this, el, values, options, width, height);

            this.shapes = {}; // map shape ids to value offsets
            this.valueShapes = {}; // maps value offsets to shape ids
            this.values = values = $.map(values, Number);

            if (options.get('width') === 'auto') {
                this.width = this.height;
            }

            if (values.length > 0) {
                for (i = values.length; i--;) {
                    total += values[i];
                }
            }
            this.total = total;
            this.initTarget();
            this.radius = Math.floor(Math.min(this.canvasWidth, this.canvasHeight) / 2);
        },

        getRegion: function (el, x, y) {
            var shapeid = this.target.getShapeAt(el, x, y);
            return (shapeid !== undefined && this.shapes[shapeid] !== undefined) ? this.shapes[shapeid] : undefined;
        },

        getCurrentRegionFields: function () {
            var currentRegion = this.currentRegion;
            return {
                isNull: this.values[currentRegion] === undefined,
                value: this.values[currentRegion],
                percent: this.values[currentRegion] / this.total * 100,
                color: this.options.get('sliceColors')[currentRegion % this.options.get('sliceColors').length],
                offset: currentRegion
            };
        },

        changeHighlight: function (highlight) {
            var currentRegion = this.currentRegion,
                 newslice = this.renderSlice(currentRegion, highlight),
                 shapeid = this.valueShapes[currentRegion];
            delete this.shapes[shapeid];
            this.target.replaceWithShape(shapeid, newslice);
            this.valueShapes[currentRegion] = newslice.id;
            this.shapes[newslice.id] = currentRegion;
        },

        renderSlice: function (valuenum, highlight) {
            var target = this.target,
                options = this.options,
                radius = this.radius,
                borderWidth = options.get('borderWidth'),
                offset = options.get('offset'),
                circle = 2 * Math.PI,
                values = this.values,
                total = this.total,
                next = offset ? (2*Math.PI)*(offset/360) : 0,
                start, end, i, vlen, color;

            vlen = values.length;
            for (i = 0; i < vlen; i++) {
                start = next;
                end = next;
                if (total > 0) {  // avoid divide by zero
                    end = next + (circle * (values[i] / total));
                }
                if (valuenum === i) {
                    color = options.get('sliceColors')[i % options.get('sliceColors').length];
                    if (highlight) {
                        color = this.calcHighlightColor(color, options);
                    }

                    return target.drawPieSlice(radius, radius, radius - borderWidth, start, end, undefined, color);
                }
                next = end;
            }
        },

        render: function () {
            var target = this.target,
                values = this.values,
                options = this.options,
                radius = this.radius,
                borderWidth = options.get('borderWidth'),
                shape, i;

            if (!pie._super.render.call(this)) {
                return;
            }
            if (borderWidth) {
                target.drawCircle(radius, radius, Math.floor(radius - (borderWidth / 2)),
                    options.get('borderColor'), undefined, borderWidth).append();
            }
            for (i = values.length; i--;) {
                if (values[i]) { // don't render zero values
                    shape = this.renderSlice(i).append();
                    this.valueShapes[i] = shape.id; // store just the shapeid
                    this.shapes[shape.id] = i;
                }
            }
            target.render();
        }
    });

    /**
     * Box plots
     */
    $.fn.sparkline.box = box = createClass($.fn.sparkline._base, {
        type: 'box',

        init: function (el, values, options, width, height) {
            box._super.init.call(this, el, values, options, width, height);
            this.values = $.map(values, Number);
            this.width = options.get('width') === 'auto' ? '4.0em' : width;
            this.initTarget();
            if (!this.values.length) {
                this.disabled = 1;
            }
        },

        /**
         * Simulate a single region
         */
        getRegion: function () {
            return 1;
        },

        getCurrentRegionFields: function () {
            var result = [
                { field: 'lq', value: this.quartiles[0] },
                { field: 'med', value: this.quartiles[1] },
                { field: 'uq', value: this.quartiles[2] }
            ];
            if (this.loutlier !== undefined) {
                result.push({ field: 'lo', value: this.loutlier});
            }
            if (this.routlier !== undefined) {
                result.push({ field: 'ro', value: this.routlier});
            }
            if (this.lwhisker !== undefined) {
                result.push({ field: 'lw', value: this.lwhisker});
            }
            if (this.rwhisker !== undefined) {
                result.push({ field: 'rw', value: this.rwhisker});
            }
            return result;
        },

        render: function () {
            var target = this.target,
                values = this.values,
                vlen = values.length,
                options = this.options,
                canvasWidth = this.canvasWidth,
                canvasHeight = this.canvasHeight,
                minValue = options.get('chartRangeMin') === undefined ? Math.min.apply(Math, values) : options.get('chartRangeMin'),
                maxValue = options.get('chartRangeMax') === undefined ? Math.max.apply(Math, values) : options.get('chartRangeMax'),
                canvasLeft = 0,
                lwhisker, loutlier, iqr, q1, q2, q3, rwhisker, routlier, i,
                size, unitSize;

            if (!box._super.render.call(this)) {
                return;
            }

            if (options.get('raw')) {
                if (options.get('showOutliers') && values.length > 5) {
                    loutlier = values[0];
                    lwhisker = values[1];
                    q1 = values[2];
                    q2 = values[3];
                    q3 = values[4];
                    rwhisker = values[5];
                    routlier = values[6];
                } else {
                    lwhisker = values[0];
                    q1 = values[1];
                    q2 = values[2];
                    q3 = values[3];
                    rwhisker = values[4];
                }
            } else {
                values.sort(function (a, b) { return a - b; });
                q1 = quartile(values, 1);
                q2 = quartile(values, 2);
                q3 = quartile(values, 3);
                iqr = q3 - q1;
                if (options.get('showOutliers')) {
                    lwhisker = rwhisker = undefined;
                    for (i = 0; i < vlen; i++) {
                        if (lwhisker === undefined && values[i] > q1 - (iqr * options.get('outlierIQR'))) {
                            lwhisker = values[i];
                        }
                        if (values[i] < q3 + (iqr * options.get('outlierIQR'))) {
                            rwhisker = values[i];
                        }
                    }
                    loutlier = values[0];
                    routlier = values[vlen - 1];
                } else {
                    lwhisker = values[0];
                    rwhisker = values[vlen - 1];
                }
            }
            this.quartiles = [q1, q2, q3];
            this.lwhisker = lwhisker;
            this.rwhisker = rwhisker;
            this.loutlier = loutlier;
            this.routlier = routlier;

            unitSize = canvasWidth / (maxValue - minValue + 1);
            if (options.get('showOutliers')) {
                canvasLeft = Math.ceil(options.get('spotRadius'));
                canvasWidth -= 2 * Math.ceil(options.get('spotRadius'));
                unitSize = canvasWidth / (maxValue - minValue + 1);
                if (loutlier < lwhisker) {
                    target.drawCircle((loutlier - minValue) * unitSize + canvasLeft,
                        canvasHeight / 2,
                        options.get('spotRadius'),
                        options.get('outlierLineColor'),
                        options.get('outlierFillColor')).append();
                }
                if (routlier > rwhisker) {
                    target.drawCircle((routlier - minValue) * unitSize + canvasLeft,
                        canvasHeight / 2,
                        options.get('spotRadius'),
                        options.get('outlierLineColor'),
                        options.get('outlierFillColor')).append();
                }
            }

            // box
            target.drawRect(
                Math.round((q1 - minValue) * unitSize + canvasLeft),
                Math.round(canvasHeight * 0.1),
                Math.round((q3 - q1) * unitSize),
                Math.round(canvasHeight * 0.8),
                options.get('boxLineColor'),
                options.get('boxFillColor')).append();
            // left whisker
            target.drawLine(
                Math.round((lwhisker - minValue) * unitSize + canvasLeft),
                Math.round(canvasHeight / 2),
                Math.round((q1 - minValue) * unitSize + canvasLeft),
                Math.round(canvasHeight / 2),
                options.get('lineColor')).append();
            target.drawLine(
                Math.round((lwhisker - minValue) * unitSize + canvasLeft),
                Math.round(canvasHeight / 4),
                Math.round((lwhisker - minValue) * unitSize + canvasLeft),
                Math.round(canvasHeight - canvasHeight / 4),
                options.get('whiskerColor')).append();
            // right whisker
            target.drawLine(Math.round((rwhisker - minValue) * unitSize + canvasLeft),
                Math.round(canvasHeight / 2),
                Math.round((q3 - minValue) * unitSize + canvasLeft),
                Math.round(canvasHeight / 2),
                options.get('lineColor')).append();
            target.drawLine(
                Math.round((rwhisker - minValue) * unitSize + canvasLeft),
                Math.round(canvasHeight / 4),
                Math.round((rwhisker - minValue) * unitSize + canvasLeft),
                Math.round(canvasHeight - canvasHeight / 4),
                options.get('whiskerColor')).append();
            // median line
            target.drawLine(
                Math.round((q2 - minValue) * unitSize + canvasLeft),
                Math.round(canvasHeight * 0.1),
                Math.round((q2 - minValue) * unitSize + canvasLeft),
                Math.round(canvasHeight * 0.9),
                options.get('medianColor')).append();
            if (options.get('target')) {
                size = Math.ceil(options.get('spotRadius'));
                target.drawLine(
                    Math.round((options.get('target') - minValue) * unitSize + canvasLeft),
                    Math.round((canvasHeight / 2) - size),
                    Math.round((options.get('target') - minValue) * unitSize + canvasLeft),
                    Math.round((canvasHeight / 2) + size),
                    options.get('targetColor')).append();
                target.drawLine(
                    Math.round((options.get('target') - minValue) * unitSize + canvasLeft - size),
                    Math.round(canvasHeight / 2),
                    Math.round((options.get('target') - minValue) * unitSize + canvasLeft + size),
                    Math.round(canvasHeight / 2),
                    options.get('targetColor')).append();
            }
            target.render();
        }
    });

    // Setup a very simple "virtual canvas" to make drawing the few shapes we need easier
    // This is accessible as $(foo).simpledraw()

    VShape = createClass({
        init: function (target, id, type, args) {
            this.target = target;
            this.id = id;
            this.type = type;
            this.args = args;
        },
        append: function () {
            this.target.appendShape(this);
            return this;
        }
    });

    VCanvas_base = createClass({
        _pxregex: /(\d+)(px)?\s*$/i,

        init: function (width, height, target) {
            if (!width) {
                return;
            }
            this.width = width;
            this.height = height;
            this.target = target;
            this.lastShapeId = null;
            if (target[0]) {
                target = target[0];
            }
            $.data(target, '_jqs_vcanvas', this);
        },

        drawLine: function (x1, y1, x2, y2, lineColor, lineWidth) {
            return this.drawShape([[x1, y1], [x2, y2]], lineColor, lineWidth);
        },

        drawShape: function (path, lineColor, fillColor, lineWidth) {
            return this._genShape('Shape', [path, lineColor, fillColor, lineWidth]);
        },

        drawCircle: function (x, y, radius, lineColor, fillColor, lineWidth) {
            return this._genShape('Circle', [x, y, radius, lineColor, fillColor, lineWidth]);
        },

        drawPieSlice: function (x, y, radius, startAngle, endAngle, lineColor, fillColor) {
            return this._genShape('PieSlice', [x, y, radius, startAngle, endAngle, lineColor, fillColor]);
        },

        drawRect: function (x, y, width, height, lineColor, fillColor) {
            return this._genShape('Rect', [x, y, width, height, lineColor, fillColor]);
        },

        getElement: function () {
            return this.canvas;
        },

        /**
         * Return the most recently inserted shape id
         */
        getLastShapeId: function () {
            return this.lastShapeId;
        },

        /**
         * Clear and reset the canvas
         */
        reset: function () {
            alert('reset not implemented');
        },

        _insert: function (el, target) {
            $(target).html(el);
        },

        /**
         * Calculate the pixel dimensions of the canvas
         */
        _calculatePixelDims: function (width, height, canvas) {
            // XXX This should probably be a configurable option
            var match;
            match = this._pxregex.exec(height);
            if (match) {
                this.pixelHeight = match[1];
            } else {
                this.pixelHeight = $(canvas).height();
            }
            match = this._pxregex.exec(width);
            if (match) {
                this.pixelWidth = match[1];
            } else {
                this.pixelWidth = $(canvas).width();
            }
        },

        /**
         * Generate a shape object and id for later rendering
         */
        _genShape: function (shapetype, shapeargs) {
            var id = shapeCount++;
            shapeargs.unshift(id);
            return new VShape(this, id, shapetype, shapeargs);
        },

        /**
         * Add a shape to the end of the render queue
         */
        appendShape: function (shape) {
            alert('appendShape not implemented');
        },

        /**
         * Replace one shape with another
         */
        replaceWithShape: function (shapeid, shape) {
            alert('replaceWithShape not implemented');
        },

        /**
         * Insert one shape after another in the render queue
         */
        insertAfterShape: function (shapeid, shape) {
            alert('insertAfterShape not implemented');
        },

        /**
         * Remove a shape from the queue
         */
        removeShapeId: function (shapeid) {
            alert('removeShapeId not implemented');
        },

        /**
         * Find a shape at the specified x/y co-ordinates
         */
        getShapeAt: function (el, x, y) {
            alert('getShapeAt not implemented');
        },

        /**
         * Render all queued shapes onto the canvas
         */
        render: function () {
            alert('render not implemented');
        }
    });

    VCanvas_canvas = createClass(VCanvas_base, {
        init: function (width, height, target, interact) {
            VCanvas_canvas._super.init.call(this, width, height, target);
            this.canvas = document.createElement('canvas');
            if (target[0]) {
                target = target[0];
            }
            $.data(target, '_jqs_vcanvas', this);
            $(this.canvas).css({ display: 'inline-block', width: width, height: height, verticalAlign: 'top' });
            this._insert(this.canvas, target);
            this._calculatePixelDims(width, height, this.canvas);
            this.canvas.width = this.pixelWidth;
            this.canvas.height = this.pixelHeight;
            this.interact = interact;
            this.shapes = {};
            this.shapeseq = [];
            this.currentTargetShapeId = undefined;
            $(this.canvas).css({width: this.pixelWidth, height: this.pixelHeight});
        },

        _getContext: function (lineColor, fillColor, lineWidth) {
            var context = this.canvas.getContext('2d');
            if (lineColor !== undefined) {
                context.strokeStyle = lineColor;
            }
            context.lineWidth = lineWidth === undefined ? 1 : lineWidth;
            if (fillColor !== undefined) {
                context.fillStyle = fillColor;
            }
            return context;
        },

        reset: function () {
            var context = this._getContext();
            context.clearRect(0, 0, this.pixelWidth, this.pixelHeight);
            this.shapes = {};
            this.shapeseq = [];
            this.currentTargetShapeId = undefined;
        },

        _drawShape: function (shapeid, path, lineColor, fillColor, lineWidth) {
            var context = this._getContext(lineColor, fillColor, lineWidth),
                i, plen;
            context.beginPath();
            context.moveTo(path[0][0] + 0.5, path[0][1] + 0.5);
            for (i = 1, plen = path.length; i < plen; i++) {
                context.lineTo(path[i][0] + 0.5, path[i][1] + 0.5); // the 0.5 offset gives us crisp pixel-width lines
            }
            if (lineColor !== undefined) {
                context.stroke();
            }
            if (fillColor !== undefined) {
                context.fill();
            }
            if (this.targetX !== undefined && this.targetY !== undefined &&
                context.isPointInPath(this.targetX, this.targetY)) {
                this.currentTargetShapeId = shapeid;
            }
        },

        _drawCircle: function (shapeid, x, y, radius, lineColor, fillColor, lineWidth) {
            var context = this._getContext(lineColor, fillColor, lineWidth);
            context.beginPath();
            context.arc(x, y, radius, 0, 2 * Math.PI, false);
            if (this.targetX !== undefined && this.targetY !== undefined &&
                context.isPointInPath(this.targetX, this.targetY)) {
                this.currentTargetShapeId = shapeid;
            }
            if (lineColor !== undefined) {
                context.stroke();
            }
            if (fillColor !== undefined) {
                context.fill();
            }
        },

        _drawPieSlice: function (shapeid, x, y, radius, startAngle, endAngle, lineColor, fillColor) {
            var context = this._getContext(lineColor, fillColor);
            context.beginPath();
            context.moveTo(x, y);
            context.arc(x, y, radius, startAngle, endAngle, false);
            context.lineTo(x, y);
            context.closePath();
            if (lineColor !== undefined) {
                context.stroke();
            }
            if (fillColor) {
                context.fill();
            }
            if (this.targetX !== undefined && this.targetY !== undefined &&
                context.isPointInPath(this.targetX, this.targetY)) {
                this.currentTargetShapeId = shapeid;
            }
        },

        _drawRect: function (shapeid, x, y, width, height, lineColor, fillColor) {
            return this._drawShape(shapeid, [[x, y], [x + width, y], [x + width, y + height], [x, y + height], [x, y]], lineColor, fillColor);
        },

        appendShape: function (shape) {
            this.shapes[shape.id] = shape;
            this.shapeseq.push(shape.id);
            this.lastShapeId = shape.id;
            return shape.id;
        },

        replaceWithShape: function (shapeid, shape) {
            var shapeseq = this.shapeseq,
                i;
            this.shapes[shape.id] = shape;
            for (i = shapeseq.length; i--;) {
                if (shapeseq[i] == shapeid) {
                    shapeseq[i] = shape.id;
                }
            }
            delete this.shapes[shapeid];
        },

        replaceWithShapes: function (shapeids, shapes) {
            var shapeseq = this.shapeseq,
                shapemap = {},
                sid, i, first;

            for (i = shapeids.length; i--;) {
                shapemap[shapeids[i]] = true;
            }
            for (i = shapeseq.length; i--;) {
                sid = shapeseq[i];
                if (shapemap[sid]) {
                    shapeseq.splice(i, 1);
                    delete this.shapes[sid];
                    first = i;
                }
            }
            for (i = shapes.length; i--;) {
                shapeseq.splice(first, 0, shapes[i].id);
                this.shapes[shapes[i].id] = shapes[i];
            }

        },

        insertAfterShape: function (shapeid, shape) {
            var shapeseq = this.shapeseq,
                i;
            for (i = shapeseq.length; i--;) {
                if (shapeseq[i] === shapeid) {
                    shapeseq.splice(i + 1, 0, shape.id);
                    this.shapes[shape.id] = shape;
                    return;
                }
            }
        },

        removeShapeId: function (shapeid) {
            var shapeseq = this.shapeseq,
                i;
            for (i = shapeseq.length; i--;) {
                if (shapeseq[i] === shapeid) {
                    shapeseq.splice(i, 1);
                    break;
                }
            }
            delete this.shapes[shapeid];
        },

        getShapeAt: function (el, x, y) {
            this.targetX = x;
            this.targetY = y;
            this.render();
            return this.currentTargetShapeId;
        },

        render: function () {
            var shapeseq = this.shapeseq,
                shapes = this.shapes,
                shapeCount = shapeseq.length,
                context = this._getContext(),
                shapeid, shape, i;
            context.clearRect(0, 0, this.pixelWidth, this.pixelHeight);
            for (i = 0; i < shapeCount; i++) {
                shapeid = shapeseq[i];
                shape = shapes[shapeid];
                this['_draw' + shape.type].apply(this, shape.args);
            }
            if (!this.interact) {
                // not interactive so no need to keep the shapes array
                this.shapes = {};
                this.shapeseq = [];
            }
        }

    });

    VCanvas_vml = createClass(VCanvas_base, {
        init: function (width, height, target) {
            var groupel;
            VCanvas_vml._super.init.call(this, width, height, target);
            if (target[0]) {
                target = target[0];
            }
            $.data(target, '_jqs_vcanvas', this);
            this.canvas = document.createElement('span');
            $(this.canvas).css({ display: 'inline-block', position: 'relative', overflow: 'hidden', width: width, height: height, margin: '0px', padding: '0px', verticalAlign: 'top'});
            this._insert(this.canvas, target);
            this._calculatePixelDims(width, height, this.canvas);
            this.canvas.width = this.pixelWidth;
            this.canvas.height = this.pixelHeight;
            groupel = '<v:group coordorigin="0 0" coordsize="' + this.pixelWidth + ' ' + this.pixelHeight + '"' +
                    ' style="position:absolute;top:0;left:0;width:' + this.pixelWidth + 'px;height=' + this.pixelHeight + 'px;"></v:group>';
            this.canvas.insertAdjacentHTML('beforeEnd', groupel);
            this.group = $(this.canvas).children()[0];
            this.rendered = false;
            this.prerender = '';
        },

        _drawShape: function (shapeid, path, lineColor, fillColor, lineWidth) {
            var vpath = [],
                initial, stroke, fill, closed, vel, plen, i;
            for (i = 0, plen = path.length; i < plen; i++) {
                vpath[i] = '' + (path[i][0]) + ',' + (path[i][1]);
            }
            initial = vpath.splice(0, 1);
            lineWidth = lineWidth === undefined ? 1 : lineWidth;
            stroke = lineColor === undefined ? ' stroked="false" ' : ' strokeWeight="' + lineWidth + 'px" strokeColor="' + lineColor + '" ';
            fill = fillColor === undefined ? ' filled="false"' : ' fillColor="' + fillColor + '" filled="true" ';
            closed = vpath[0] === vpath[vpath.length - 1] ? 'x ' : '';
            vel = '<v:shape coordorigin="0 0" coordsize="' + this.pixelWidth + ' ' + this.pixelHeight + '" ' +
                 ' id="jqsshape' + shapeid + '" ' +
                 stroke +
                 fill +
                ' style="position:absolute;left:0px;top:0px;height:' + this.pixelHeight + 'px;width:' + this.pixelWidth + 'px;padding:0px;margin:0px;" ' +
                ' path="m ' + initial + ' l ' + vpath.join(', ') + ' ' + closed + 'e">' +
                ' </v:shape>';
            return vel;
        },

        _drawCircle: function (shapeid, x, y, radius, lineColor, fillColor, lineWidth) {
            var stroke, fill, vel;
            x -= radius;
            y -= radius;
            stroke = lineColor === undefined ? ' stroked="false" ' : ' strokeWeight="' + lineWidth + 'px" strokeColor="' + lineColor + '" ';
            fill = fillColor === undefined ? ' filled="false"' : ' fillColor="' + fillColor + '" filled="true" ';
            vel = '<v:oval ' +
                 ' id="jqsshape' + shapeid + '" ' +
                stroke +
                fill +
                ' style="position:absolute;top:' + y + 'px; left:' + x + 'px; width:' + (radius * 2) + 'px; height:' + (radius * 2) + 'px"></v:oval>';
            return vel;

        },

        _drawPieSlice: function (shapeid, x, y, radius, startAngle, endAngle, lineColor, fillColor) {
            var vpath, startx, starty, endx, endy, stroke, fill, vel;
            if (startAngle === endAngle) {
                return '';  // VML seems to have problem when start angle equals end angle.
            }
            if ((endAngle - startAngle) === (2 * Math.PI)) {
                startAngle = 0.0;  // VML seems to have a problem when drawing a full circle that doesn't start 0
                endAngle = (2 * Math.PI);
            }

            startx = x + Math.round(Math.cos(startAngle) * radius);
            starty = y + Math.round(Math.sin(startAngle) * radius);
            endx = x + Math.round(Math.cos(endAngle) * radius);
            endy = y + Math.round(Math.sin(endAngle) * radius);

            if (startx === endx && starty === endy) {
                if ((endAngle - startAngle) < Math.PI) {
                    // Prevent very small slices from being mistaken as a whole pie
                    return '';
                }
                // essentially going to be the entire circle, so ignore startAngle
                startx = endx = x + radius;
                starty = endy = y;
            }

            if (startx === endx && starty === endy && (endAngle - startAngle) < Math.PI) {
                return '';
            }

            vpath = [x - radius, y - radius, x + radius, y + radius, startx, starty, endx, endy];
            stroke = lineColor === undefined ? ' stroked="false" ' : ' strokeWeight="1px" strokeColor="' + lineColor + '" ';
            fill = fillColor === undefined ? ' filled="false"' : ' fillColor="' + fillColor + '" filled="true" ';
            vel = '<v:shape coordorigin="0 0" coordsize="' + this.pixelWidth + ' ' + this.pixelHeight + '" ' +
                 ' id="jqsshape' + shapeid + '" ' +
                 stroke +
                 fill +
                ' style="position:absolute;left:0px;top:0px;height:' + this.pixelHeight + 'px;width:' + this.pixelWidth + 'px;padding:0px;margin:0px;" ' +
                ' path="m ' + x + ',' + y + ' wa ' + vpath.join(', ') + ' x e">' +
                ' </v:shape>';
            return vel;
        },

        _drawRect: function (shapeid, x, y, width, height, lineColor, fillColor) {
            return this._drawShape(shapeid, [[x, y], [x, y + height], [x + width, y + height], [x + width, y], [x, y]], lineColor, fillColor);
        },

        reset: function () {
            this.group.innerHTML = '';
        },

        appendShape: function (shape) {
            var vel = this['_draw' + shape.type].apply(this, shape.args);
            if (this.rendered) {
                this.group.insertAdjacentHTML('beforeEnd', vel);
            } else {
                this.prerender += vel;
            }
            this.lastShapeId = shape.id;
            return shape.id;
        },

        replaceWithShape: function (shapeid, shape) {
            var existing = $('#jqsshape' + shapeid),
                vel = this['_draw' + shape.type].apply(this, shape.args);
            existing[0].outerHTML = vel;
        },

        replaceWithShapes: function (shapeids, shapes) {
            // replace the first shapeid with all the new shapes then toast the remaining old shapes
            var existing = $('#jqsshape' + shapeids[0]),
                replace = '',
                slen = shapes.length,
                i;
            for (i = 0; i < slen; i++) {
                replace += this['_draw' + shapes[i].type].apply(this, shapes[i].args);
            }
            existing[0].outerHTML = replace;
            for (i = 1; i < shapeids.length; i++) {
                $('#jqsshape' + shapeids[i]).remove();
            }
        },

        insertAfterShape: function (shapeid, shape) {
            var existing = $('#jqsshape' + shapeid),
                 vel = this['_draw' + shape.type].apply(this, shape.args);
            existing[0].insertAdjacentHTML('afterEnd', vel);
        },

        removeShapeId: function (shapeid) {
            var existing = $('#jqsshape' + shapeid);
            this.group.removeChild(existing[0]);
        },

        getShapeAt: function (el, x, y) {
            var shapeid = el.id.substr(8);
            return shapeid;
        },

        render: function () {
            if (!this.rendered) {
                // batch the intial render into a single repaint
                this.group.innerHTML = this.prerender;
                this.rendered = true;
            }
        }
    });

}))}(document, Math));
// Generated by CoffeeScript 1.6.2
/*
jQuery Waypoints - v2.0.3
Copyright (c) 2011-2013 Caleb Troughton
Dual licensed under the MIT license and GPL license.
https://github.com/imakewebthings/jquery-waypoints/blob/master/licenses.txt
*/
(function(){var t=[].indexOf||function(t){for(var e=0,n=this.length;e<n;e++){if(e in this&&this[e]===t)return e}return-1},e=[].slice;(function(t,e){if(typeof define==="function"&&define.amd){return define("waypoints",["jquery"],function(n){return e(n,t)})}else{return e(t.jQuery,t)}})(this,function(n,r){var i,o,l,s,f,u,a,c,h,d,p,y,v,w,g,m;i=n(r);c=t.call(r,"ontouchstart")>=0;s={horizontal:{},vertical:{}};f=1;a={};u="waypoints-context-id";p="resize.waypoints";y="scroll.waypoints";v=1;w="waypoints-waypoint-ids";g="waypoint";m="waypoints";o=function(){function t(t){var e=this;this.$element=t;this.element=t[0];this.didResize=false;this.didScroll=false;this.id="context"+f++;this.oldScroll={x:t.scrollLeft(),y:t.scrollTop()};this.waypoints={horizontal:{},vertical:{}};t.data(u,this.id);a[this.id]=this;t.bind(y,function(){var t;if(!(e.didScroll||c)){e.didScroll=true;t=function(){e.doScroll();return e.didScroll=false};return r.setTimeout(t,n[m].settings.scrollThrottle)}});t.bind(p,function(){var t;if(!e.didResize){e.didResize=true;t=function(){n[m]("refresh");return e.didResize=false};return r.setTimeout(t,n[m].settings.resizeThrottle)}})}t.prototype.doScroll=function(){var t,e=this;t={horizontal:{newScroll:this.$element.scrollLeft(),oldScroll:this.oldScroll.x,forward:"right",backward:"left"},vertical:{newScroll:this.$element.scrollTop(),oldScroll:this.oldScroll.y,forward:"down",backward:"up"}};if(c&&(!t.vertical.oldScroll||!t.vertical.newScroll)){n[m]("refresh")}n.each(t,function(t,r){var i,o,l;l=[];o=r.newScroll>r.oldScroll;i=o?r.forward:r.backward;n.each(e.waypoints[t],function(t,e){var n,i;if(r.oldScroll<(n=e.offset)&&n<=r.newScroll){return l.push(e)}else if(r.newScroll<(i=e.offset)&&i<=r.oldScroll){return l.push(e)}});l.sort(function(t,e){return t.offset-e.offset});if(!o){l.reverse()}return n.each(l,function(t,e){if(e.options.continuous||t===l.length-1){return e.trigger([i])}})});return this.oldScroll={x:t.horizontal.newScroll,y:t.vertical.newScroll}};t.prototype.refresh=function(){var t,e,r,i=this;r=n.isWindow(this.element);e=this.$element.offset();this.doScroll();t={horizontal:{contextOffset:r?0:e.left,contextScroll:r?0:this.oldScroll.x,contextDimension:this.$element.width(),oldScroll:this.oldScroll.x,forward:"right",backward:"left",offsetProp:"left"},vertical:{contextOffset:r?0:e.top,contextScroll:r?0:this.oldScroll.y,contextDimension:r?n[m]("viewportHeight"):this.$element.height(),oldScroll:this.oldScroll.y,forward:"down",backward:"up",offsetProp:"top"}};return n.each(t,function(t,e){return n.each(i.waypoints[t],function(t,r){var i,o,l,s,f;i=r.options.offset;l=r.offset;o=n.isWindow(r.element)?0:r.$element.offset()[e.offsetProp];if(n.isFunction(i)){i=i.apply(r.element)}else if(typeof i==="string"){i=parseFloat(i);if(r.options.offset.indexOf("%")>-1){i=Math.ceil(e.contextDimension*i/100)}}r.offset=o-e.contextOffset+e.contextScroll-i;if(r.options.onlyOnScroll&&l!=null||!r.enabled){return}if(l!==null&&l<(s=e.oldScroll)&&s<=r.offset){return r.trigger([e.backward])}else if(l!==null&&l>(f=e.oldScroll)&&f>=r.offset){return r.trigger([e.forward])}else if(l===null&&e.oldScroll>=r.offset){return r.trigger([e.forward])}})})};t.prototype.checkEmpty=function(){if(n.isEmptyObject(this.waypoints.horizontal)&&n.isEmptyObject(this.waypoints.vertical)){this.$element.unbind([p,y].join(" "));return delete a[this.id]}};return t}();l=function(){function t(t,e,r){var i,o;r=n.extend({},n.fn[g].defaults,r);if(r.offset==="bottom-in-view"){r.offset=function(){var t;t=n[m]("viewportHeight");if(!n.isWindow(e.element)){t=e.$element.height()}return t-n(this).outerHeight()}}this.$element=t;this.element=t[0];this.axis=r.horizontal?"horizontal":"vertical";this.callback=r.handler;this.context=e;this.enabled=r.enabled;this.id="waypoints"+v++;this.offset=null;this.options=r;e.waypoints[this.axis][this.id]=this;s[this.axis][this.id]=this;i=(o=t.data(w))!=null?o:[];i.push(this.id);t.data(w,i)}t.prototype.trigger=function(t){if(!this.enabled){return}if(this.callback!=null){this.callback.apply(this.element,t)}if(this.options.triggerOnce){return this.destroy()}};t.prototype.disable=function(){return this.enabled=false};t.prototype.enable=function(){this.context.refresh();return this.enabled=true};t.prototype.destroy=function(){delete s[this.axis][this.id];delete this.context.waypoints[this.axis][this.id];return this.context.checkEmpty()};t.getWaypointsByElement=function(t){var e,r;r=n(t).data(w);if(!r){return[]}e=n.extend({},s.horizontal,s.vertical);return n.map(r,function(t){return e[t]})};return t}();d={init:function(t,e){var r;if(e==null){e={}}if((r=e.handler)==null){e.handler=t}this.each(function(){var t,r,i,s;t=n(this);i=(s=e.context)!=null?s:n.fn[g].defaults.context;if(!n.isWindow(i)){i=t.closest(i)}i=n(i);r=a[i.data(u)];if(!r){r=new o(i)}return new l(t,r,e)});n[m]("refresh");return this},disable:function(){return d._invoke(this,"disable")},enable:function(){return d._invoke(this,"enable")},destroy:function(){return d._invoke(this,"destroy")},prev:function(t,e){return d._traverse.call(this,t,e,function(t,e,n){if(e>0){return t.push(n[e-1])}})},next:function(t,e){return d._traverse.call(this,t,e,function(t,e,n){if(e<n.length-1){return t.push(n[e+1])}})},_traverse:function(t,e,i){var o,l;if(t==null){t="vertical"}if(e==null){e=r}l=h.aggregate(e);o=[];this.each(function(){var e;e=n.inArray(this,l[t]);return i(o,e,l[t])});return this.pushStack(o)},_invoke:function(t,e){t.each(function(){var t;t=l.getWaypointsByElement(this);return n.each(t,function(t,n){n[e]();return true})});return this}};n.fn[g]=function(){var t,r;r=arguments[0],t=2<=arguments.length?e.call(arguments,1):[];if(d[r]){return d[r].apply(this,t)}else if(n.isFunction(r)){return d.init.apply(this,arguments)}else if(n.isPlainObject(r)){return d.init.apply(this,[null,r])}else if(!r){return n.error("jQuery Waypoints needs a callback function or handler option.")}else{return n.error("The "+r+" method does not exist in jQuery Waypoints.")}};n.fn[g].defaults={context:r,continuous:true,enabled:true,horizontal:false,offset:0,triggerOnce:false};h={refresh:function(){return n.each(a,function(t,e){return e.refresh()})},viewportHeight:function(){var t;return(t=r.innerHeight)!=null?t:i.height()},aggregate:function(t){var e,r,i;e=s;if(t){e=(i=a[n(t).data(u)])!=null?i.waypoints:void 0}if(!e){return[]}r={horizontal:[],vertical:[]};n.each(r,function(t,i){n.each(e[t],function(t,e){return i.push(e)});i.sort(function(t,e){return t.offset-e.offset});r[t]=n.map(i,function(t){return t.element});return r[t]=n.unique(r[t])});return r},above:function(t){if(t==null){t=r}return h._filter(t,"vertical",function(t,e){return e.offset<=t.oldScroll.y})},below:function(t){if(t==null){t=r}return h._filter(t,"vertical",function(t,e){return e.offset>t.oldScroll.y})},left:function(t){if(t==null){t=r}return h._filter(t,"horizontal",function(t,e){return e.offset<=t.oldScroll.x})},right:function(t){if(t==null){t=r}return h._filter(t,"horizontal",function(t,e){return e.offset>t.oldScroll.x})},enable:function(){return h._invoke("enable")},disable:function(){return h._invoke("disable")},destroy:function(){return h._invoke("destroy")},extendFn:function(t,e){return d[t]=e},_invoke:function(t){var e;e=n.extend({},s.vertical,s.horizontal);return n.each(e,function(e,n){n[t]();return true})},_filter:function(t,e,r){var i,o;i=a[n(t).data(u)];if(!i){return[]}o=[];n.each(i.waypoints[e],function(t,e){if(r(i,e)){return o.push(e)}});o.sort(function(t,e){return t.offset-e.offset});return n.map(o,function(t){return t.element})}};n[m]=function(){var t,n;n=arguments[0],t=2<=arguments.length?e.call(arguments,1):[];if(h[n]){return h[n].apply(null,t)}else{return h.aggregate.call(null,n)}};n[m].settings={resizeThrottle:100,scrollThrottle:30};return i.load(function(){return n[m]("refresh")})})}).call(this);
/*!
* jquery.counterup.js 1.0
*
* Copyright 2013, Benjamin Intal http://gambit.ph @bfintal
* Released under the GPL v2 License
*
* Date: Nov 26, 2013
*/(function(e){"use strict";e.fn.counterUp=function(t){var n=e.extend({time:400,delay:10},t);return this.each(function(){var t=e(this),r=n,i=function(){var e=[],n=r.time/r.delay,i=t.text(),s=/[0-9]+,[0-9]+/.test(i);i=i.replace(/,/g,"");var o=/^[0-9]+$/.test(i),u=/^[0-9]+\.[0-9]+$/.test(i),a=u?(i.split(".")[1]||[]).length:0;for(var f=n;f>=1;f--){var l=parseInt(i/n*f);u&&(l=parseFloat(i/n*f).toFixed(a));if(s)while(/(\d+)(\d{3})/.test(l.toString()))l=l.toString().replace(/(\d+)(\d{3})/,"$1,$2");e.unshift(l)}t.data("counterup-nums",e);t.text("0");var c=function(){t.text(t.data("counterup-nums").shift());if(t.data("counterup-nums").length)setTimeout(t.data("counterup-func"),r.delay);else{delete t.data("counterup-nums");t.data("counterup-nums",null);t.data("counterup-func",null)}};t.data("counterup-func",c);setTimeout(t.data("counterup-func"),r.delay)};t.waypoint(i,{offset:"100%",triggerOnce:!0})})}})(jQuery);
/*

	jQuery Tags Input Plugin 1.3.3

	Copyright (c) 2011 XOXCO, Inc

	Documentation for this plugin lives here:
	http://xoxco.com/clickable/jquery-tags-input

	Licensed under the MIT license:
	http://www.opensource.org/licenses/mit-license.php

	ben@xoxco.com

*/

(function($) {

	var delimiter = new Array();
	var tags_callbacks = new Array();
	$.fn.doAutosize = function(o){
	    var minWidth = $(this).data('minwidth'),
	        maxWidth = $(this).data('maxwidth'),
	        val = '',
	        input = $(this),
	        testSubject = $('#'+$(this).data('tester_id'));

	    if (val === (val = input.val())) {return;}

	    // Enter new content into testSubject
	    var escaped = val.replace(/&/g, '&amp;').replace(/\s/g,' ').replace(/</g, '&lt;').replace(/>/g, '&gt;');
	    testSubject.html(escaped);
	    // Calculate new width + whether to change
	    var testerWidth = testSubject.width(),
	        newWidth = (testerWidth + o.comfortZone) >= minWidth ? testerWidth + o.comfortZone : minWidth,
	        currentWidth = input.width(),
	        isValidWidthChange = (newWidth < currentWidth && newWidth >= minWidth)
	                             || (newWidth > minWidth && newWidth < maxWidth);

	    // Animate width
	    if (isValidWidthChange) {
	        input.width(newWidth);
	    }


  };
  $.fn.resetAutosize = function(options){
    // alert(JSON.stringify(options));
    var minWidth =  $(this).data('minwidth') || options.minInputWidth || $(this).width(),
        maxWidth = $(this).data('maxwidth') || options.maxInputWidth || ($(this).closest('.tagsinput').width() - options.inputPadding),
        val = '',
        input = $(this),
        testSubject = $('<tester/>').css({
            position: 'absolute',
            top: -9999,
            left: -9999,
            width: 'auto',
            fontSize: input.css('fontSize'),
            fontFamily: input.css('fontFamily'),
            fontWeight: input.css('fontWeight'),
            letterSpacing: input.css('letterSpacing'),
            whiteSpace: 'nowrap'
        }),
        testerId = $(this).attr('id')+'_autosize_tester';
    if(! $('#'+testerId).length > 0){
      testSubject.attr('id', testerId);
      testSubject.appendTo('body');
    }

    input.data('minwidth', minWidth);
    input.data('maxwidth', maxWidth);
    input.data('tester_id', testerId);
    input.css('width', minWidth);
  };

	$.fn.addTag = function(value,options,noCallback) {
			options = jQuery.extend({focus:false,callback:true},options);
			this.each(function() {
				var id = $(this).attr('id');

				var tagslist = $(this).val().split(delimiter[id]);
				if (tagslist[0] == '') {
					tagslist = new Array();
				}

				value = jQuery.trim(value);

				if (options.unique) {
					var skipTag = $(this).tagExist(value);
					if(skipTag == true) {
					    //Marks fake input as not_valid to let styling it
    				    $('#'+id+'_tag').addClass('not_valid');
    				}
				} else {
					var skipTag = false;
				}

				if (value !='' && skipTag != true) {
                    $('<span>').addClass('tag').append(
                        $('<span>').text(value).append('&nbsp;&nbsp;'),
                        $('<a>', {
                            href  : '#',
                            title : 'Removing tag',
                            text  : 'x'
                        }).click(function () {
                            return $('#' + id).removeTag(escape(value));
                        })
                    ).insertBefore('#' + id + '_addTag');

					tagslist.push(value);

					$('#'+id+'_tag').val('');
					if (options.focus) {
						$('#'+id+'_tag').focus();
					} else {
						$('#'+id+'_tag').blur();
					}

					$.fn.tagsInput.updateTagsField(this,tagslist);

					if (options.callback && tags_callbacks[id] && tags_callbacks[id]['onAddTag']) {
						var f = tags_callbacks[id]['onAddTag'];
						f.call(this, value);
					}
					if(!noCallback && tags_callbacks[id] && tags_callbacks[id]['onChange'])
					{
						var i = tagslist.length;
						var f = tags_callbacks[id]['onChange'];
						f.call(this, $(this), tagslist[i-1]);
					}
				}

			});

			return false;
		};

	$.fn.removeTag = function(value) {
			value = unescape(value);
			this.each(function() {
				var id = $(this).attr('id');

				var old = $(this).val().split(delimiter[id]);

				$('#'+id+'_tagsinput .tag').remove();
				str = '';
				for (i=0; i< old.length; i++) {
					if (old[i]!=value) {
						str = str + delimiter[id] +old[i];
					}
				}

				$.fn.tagsInput.importTags(this,str);

				if (tags_callbacks[id] && tags_callbacks[id]['onRemoveTag']) {
					var f = tags_callbacks[id]['onRemoveTag'];
					f.call(this, value);
				}
			});

			return false;
		};

	$.fn.tagExist = function(val) {
		var id = $(this).attr('id');
		var tagslist = $(this).val().split(delimiter[id]);
		return (jQuery.inArray(val, tagslist) >= 0); //true when tag exists, false when not
	};

   // clear all existing tags and import new ones from a string
   $.fn.importTags = function(str) {
      var id = $(this).attr('id');
      $('#'+id+'_tagsinput .tag').remove();
      $.fn.tagsInput.importTags(this,str);
   }

	$.fn.tagsInput = function(options) {
    var settings = jQuery.extend({
      interactive:true,
      defaultText:'add a tag',
      minChars:0,
      width:'300px',
      height:'100px',
      autocomplete: {selectFirst: false },
      hide:true,
      delimiter: ',',
      unique:true,
      removeWithBackspace:true,
      placeholderColor:'#666666',
      autosize: true,
      comfortZone: 20,
      inputPadding: 6*2
    },options);

    	var uniqueIdCounter = 0;

		this.each(function() {
         // If we have already initialized the field, do not do it again
         if (typeof $(this).attr('data-tagsinput-init') !== 'undefined') {
            return;
         }

         // Mark the field as having been initialized
         $(this).attr('data-tagsinput-init', true);

			if (settings.hide) {
				$(this).hide();
			}
			var id = $(this).attr('id');
			if (!id || delimiter[$(this).attr('id')]) {
				id = $(this).attr('id', 'tags' + new Date().getTime() + (uniqueIdCounter++)).attr('id');
			}

			var data = jQuery.extend({
				pid:id,
				real_input: '#'+id,
				holder: '#'+id+'_tagsinput',
				input_wrapper: '#'+id+'_addTag',
				fake_input: '#'+id+'_tag'
			},settings);

			delimiter[id] = data.delimiter;

			if (settings.onAddTag || settings.onRemoveTag || settings.onChange) {
				tags_callbacks[id] = new Array();
				tags_callbacks[id]['onAddTag'] = settings.onAddTag;
				tags_callbacks[id]['onRemoveTag'] = settings.onRemoveTag;
				tags_callbacks[id]['onChange'] = settings.onChange;
			}

			var markup = '<div id="'+id+'_tagsinput" class="tagsinput"><div id="'+id+'_addTag">';

			if (settings.interactive) {
				markup = markup + '<input id="'+id+'_tag" value="" data-default="'+settings.defaultText+'" />';
			}

			markup = markup + '</div><div class="tags_clear"></div></div>';

			$(markup).insertAfter(this);

			$(data.holder).css('width',settings.width);
			$(data.holder).css('min-height',settings.height);
			$(data.holder).css('height',settings.height);

			if ($(data.real_input).val()!='') {
				$.fn.tagsInput.importTags($(data.real_input),$(data.real_input).val());
			}
			if (settings.interactive) {
				
				$(data.fake_input).val($(data.fake_input).attr('data-default'));
				$(data.fake_input).css('color',settings.placeholderColor);
		        $(data.fake_input).resetAutosize(settings);

				$(data.holder).bind('click',data,function(event) {
					$(event.data.fake_input).focus();
				});

				$(data.fake_input).bind('focus',data,function(event) {
					if ($(event.data.fake_input).val()==$(event.data.fake_input).attr('data-default')) {
						$(event.data.fake_input).val('');
					}
					$(event.data.fake_input).css('color','#000000');
				});

				if (settings.autocomplete_url != undefined) {
					autocomplete_options = {source: settings.autocomplete_url};
					for (attrname in settings.autocomplete) {
						autocomplete_options[attrname] = settings.autocomplete[attrname];
					}

					if (jQuery.Autocompleter !== undefined) {
						$(data.fake_input).autocomplete(settings.autocomplete_url, settings.autocomplete);
						$(data.fake_input).bind('result',data,function(event,data,formatted) {
							if (data) {
								$('#'+id).addTag(data[0] + "",{focus:true,unique:(settings.unique)});
							}
					  	});
					} else if (jQuery.ui.autocomplete !== undefined) {
						
						$(data.fake_input).autocomplete(autocomplete_options);
						$(data.fake_input).bind('autocompleteselect',data,function(event,ui) {
							$(event.data.real_input).addTag(ui.item.value,{focus:true,unique:(settings.unique)});
							return false;
						});
					}


				} else {
						$(data.fake_input).autocomplete({source: settings.autocomplete.source});
						$(data.fake_input).bind('autocompleteselect',data,function(event,ui) {
							$(event.data.real_input).addTag(ui.item.value,{focus:true,unique:(settings.unique)});
							return false;
						});
					
						// if a user tabs out of the field, create a new tag
						// this is only available if autocomplete is not used.
						$(data.fake_input).bind('blur',data,function(event) {
							var d = $(this).attr('data-default');
							if ($(event.data.fake_input).val()!='' && $(event.data.fake_input).val()!=d) {
								if( (event.data.minChars <= $(event.data.fake_input).val().length) && (!event.data.maxChars || (event.data.maxChars >= $(event.data.fake_input).val().length)) )
									$(event.data.real_input).addTag($(event.data.fake_input).val(),{focus:true,unique:(settings.unique)});
							} else {
								$(event.data.fake_input).val($(event.data.fake_input).attr('data-default'));
								$(event.data.fake_input).css('color',settings.placeholderColor);
							}
							return false;
						});

				}
				// if user types a default delimiter like comma,semicolon and then create a new tag
				$(data.fake_input).bind('keypress',data,function(event) {
					if (_checkDelimiter(event)) {
					    event.preventDefault();
						if( (event.data.minChars <= $(event.data.fake_input).val().length) && (!event.data.maxChars || (event.data.maxChars >= $(event.data.fake_input).val().length)) )
							$(event.data.real_input).addTag($(event.data.fake_input).val(),{focus:true,unique:(settings.unique)});
					  	$(event.data.fake_input).resetAutosize(settings);
						return false;
					} else if (event.data.autosize) {
			            $(event.data.fake_input).doAutosize(settings);

          			}
				});
				//Delete last tag on backspace
				data.removeWithBackspace && $(data.fake_input).bind('keydown', function(event)
				{
					if(event.keyCode == 8 && $(this).val() == '')
					{
						 event.preventDefault();
						 var last_tag = $(this).closest('.tagsinput').find('.tag:last').text();
						 var id = $(this).attr('id').replace(/_tag$/, '');
						 last_tag = last_tag.replace(/[\s]+x$/, '');
						 $('#' + id).removeTag(escape(last_tag));
						 $(this).trigger('focus');
					}
				});
				$(data.fake_input).blur(function(){
					$(this).removeClass('not_valid');
				});

				//Removes the not_valid class when user changes the value of the fake input
				if(data.unique) {
				    $(data.fake_input).keydown(function(event){
				        if(event.keyCode == 8 || String.fromCharCode(event.which).match(/\w+|[áéíóúÁÉÍÓÚñÑ,/]+/)) {
				            $(this).removeClass('not_valid');
				        }
				    });
				}
			} // if settings.interactive
		});

		return this;

	};

	$.fn.tagsInput.updateTagsField = function(obj,tagslist) {
		var id = $(obj).attr('id');
		$(obj).val(tagslist.join(delimiter[id]));
	};

	$.fn.tagsInput.importTags = function(obj,val) {
		$(obj).val('');
		var id = $(obj).attr('id');
		var tags = val.split(delimiter[id]);
		for (i=0; i<tags.length; i++) {
			$(obj).addTag(tags[i],{focus:false,callback:false},true);
		}
		if(tags_callbacks[id] && tags_callbacks[id]['onChange'])
		{
			var f = tags_callbacks[id]['onChange'];
			f.call(obj, obj, tags[i]);
		}
	};

   /**
     * check delimiter Array
     * @param event
     * @returns {boolean}
     * @private
     */
   var _checkDelimiter = function(event){
      var found = false;
      if (event.which == 13) {
         return true;
      }

      if (typeof event.data.delimiter === 'string') {
         if (event.which == event.data.delimiter.charCodeAt(0)) {
            found = true;
         }
      } else {
         $.each(event.data.delimiter, function(index, delimiter) {
            if (event.which == delimiter.charCodeAt(0)) {
               found = true;
            }
         });
      }

      return found;
   }
})(jQuery);
(function(root,factory){if(typeof define==="function"&&define.amd){define(["d3"],function(d3){return root.Rickshaw=factory(d3)})}else if(typeof exports==="object"){module.exports=factory(require("d3"))}else{root.Rickshaw=factory(d3)}})(this,function(d3){var Rickshaw={namespace:function(namespace,obj){var parts=namespace.split(".");var parent=Rickshaw;for(var i=1,length=parts.length;i<length;i++){var currentPart=parts[i];parent[currentPart]=parent[currentPart]||{};parent=parent[currentPart]}return parent},keys:function(obj){var keys=[];for(var key in obj)keys.push(key);return keys},extend:function(destination,source){for(var property in source){destination[property]=source[property]}return destination},clone:function(obj){return JSON.parse(JSON.stringify(obj))}};(function(globalContext){var _toString=Object.prototype.toString,NULL_TYPE="Null",UNDEFINED_TYPE="Undefined",BOOLEAN_TYPE="Boolean",NUMBER_TYPE="Number",STRING_TYPE="String",OBJECT_TYPE="Object",FUNCTION_CLASS="[object Function]";function isFunction(object){return _toString.call(object)===FUNCTION_CLASS}function extend(destination,source){for(var property in source)if(source.hasOwnProperty(property))destination[property]=source[property];return destination}function keys(object){if(Type(object)!==OBJECT_TYPE){throw new TypeError}var results=[];for(var property in object){if(object.hasOwnProperty(property)){results.push(property)}}return results}function Type(o){switch(o){case null:return NULL_TYPE;case void 0:return UNDEFINED_TYPE}var type=typeof o;switch(type){case"boolean":return BOOLEAN_TYPE;case"number":return NUMBER_TYPE;case"string":return STRING_TYPE}return OBJECT_TYPE}function isUndefined(object){return typeof object==="undefined"}var slice=Array.prototype.slice;function argumentNames(fn){var names=fn.toString().match(/^[\s\(]*function[^(]*\(([^)]*)\)/)[1].replace(/\/\/.*?[\r\n]|\/\*(?:.|[\r\n])*?\*\//g,"").replace(/\s+/g,"").split(",");return names.length==1&&!names[0]?[]:names}function wrap(fn,wrapper){var __method=fn;return function(){var a=update([bind(__method,this)],arguments);return wrapper.apply(this,a)}}function update(array,args){var arrayLength=array.length,length=args.length;while(length--)array[arrayLength+length]=args[length];return array}function merge(array,args){array=slice.call(array,0);return update(array,args)}function bind(fn,context){if(arguments.length<2&&isUndefined(arguments[0]))return this;var __method=fn,args=slice.call(arguments,2);return function(){var a=merge(args,arguments);return __method.apply(context,a)}}var emptyFunction=function(){};var Class=function(){var IS_DONTENUM_BUGGY=function(){for(var p in{toString:1}){if(p==="toString")return false}return true}();function subclass(){}function create(){var parent=null,properties=[].slice.apply(arguments);if(isFunction(properties[0]))parent=properties.shift();function klass(){this.initialize.apply(this,arguments)}extend(klass,Class.Methods);klass.superclass=parent;klass.subclasses=[];if(parent){subclass.prototype=parent.prototype;klass.prototype=new subclass;try{parent.subclasses.push(klass)}catch(e){}}for(var i=0,length=properties.length;i<length;i++)klass.addMethods(properties[i]);if(!klass.prototype.initialize)klass.prototype.initialize=emptyFunction;klass.prototype.constructor=klass;return klass}function addMethods(source){var ancestor=this.superclass&&this.superclass.prototype,properties=keys(source);if(IS_DONTENUM_BUGGY){if(source.toString!=Object.prototype.toString)properties.push("toString");if(source.valueOf!=Object.prototype.valueOf)properties.push("valueOf")}for(var i=0,length=properties.length;i<length;i++){var property=properties[i],value=source[property];if(ancestor&&isFunction(value)&&argumentNames(value)[0]=="$super"){var method=value;value=wrap(function(m){return function(){return ancestor[m].apply(this,arguments)}}(property),method);value.valueOf=bind(method.valueOf,method);value.toString=bind(method.toString,method)}this.prototype[property]=value}return this}return{create:create,Methods:{addMethods:addMethods}}}();if(globalContext.exports){globalContext.exports.Class=Class}else{globalContext.Class=Class}})(Rickshaw);Rickshaw.namespace("Rickshaw.Compat.ClassList");Rickshaw.Compat.ClassList=function(){if(typeof document!=="undefined"&&!("classList"in document.createElement("a"))){(function(view){"use strict";var classListProp="classList",protoProp="prototype",elemCtrProto=(view.HTMLElement||view.Element)[protoProp],objCtr=Object,strTrim=String[protoProp].trim||function(){return this.replace(/^\s+|\s+$/g,"")},arrIndexOf=Array[protoProp].indexOf||function(item){var i=0,len=this.length;for(;i<len;i++){if(i in this&&this[i]===item){return i}}return-1},DOMEx=function(type,message){this.name=type;this.code=DOMException[type];this.message=message},checkTokenAndGetIndex=function(classList,token){if(token===""){throw new DOMEx("SYNTAX_ERR","An invalid or illegal string was specified")}if(/\s/.test(token)){throw new DOMEx("INVALID_CHARACTER_ERR","String contains an invalid character")}return arrIndexOf.call(classList,token)},ClassList=function(elem){var trimmedClasses=strTrim.call(elem.className),classes=trimmedClasses?trimmedClasses.split(/\s+/):[],i=0,len=classes.length;for(;i<len;i++){this.push(classes[i])}this._updateClassName=function(){elem.className=this.toString()}},classListProto=ClassList[protoProp]=[],classListGetter=function(){return new ClassList(this)};DOMEx[protoProp]=Error[protoProp];classListProto.item=function(i){return this[i]||null};classListProto.contains=function(token){token+="";return checkTokenAndGetIndex(this,token)!==-1};classListProto.add=function(token){token+="";if(checkTokenAndGetIndex(this,token)===-1){this.push(token);this._updateClassName()}};classListProto.remove=function(token){token+="";var index=checkTokenAndGetIndex(this,token);if(index!==-1){this.splice(index,1);this._updateClassName()}};classListProto.toggle=function(token){token+="";if(checkTokenAndGetIndex(this,token)===-1){this.add(token)}else{this.remove(token)}};classListProto.toString=function(){return this.join(" ")};if(objCtr.defineProperty){var classListPropDesc={get:classListGetter,enumerable:true,configurable:true};try{objCtr.defineProperty(elemCtrProto,classListProp,classListPropDesc)}catch(ex){if(ex.number===-2146823252){classListPropDesc.enumerable=false;objCtr.defineProperty(elemCtrProto,classListProp,classListPropDesc)}}}else if(objCtr[protoProp].__defineGetter__){elemCtrProto.__defineGetter__(classListProp,classListGetter)}})(window)}};if(typeof RICKSHAW_NO_COMPAT!=="undefined"&&!RICKSHAW_NO_COMPAT||typeof RICKSHAW_NO_COMPAT==="undefined"){new Rickshaw.Compat.ClassList}Rickshaw.namespace("Rickshaw.Graph");Rickshaw.Graph=function(args){var self=this;this.initialize=function(args){if(!args.element)throw"Rickshaw.Graph needs a reference to an element";if(args.element.nodeType!==1)throw"Rickshaw.Graph element was defined but not an HTML element";this.element=args.element;this.series=args.series;this.window={};this.updateCallbacks=[];this.configureCallbacks=[];this.defaults={interpolation:"cardinal",offset:"zero",min:undefined,max:undefined,preserve:false,xScale:undefined,yScale:undefined,stack:true};this._loadRenderers();this.configure(args);this.validateSeries(args.series);this.series.active=function(){return self.series.filter(function(s){return!s.disabled})};this.setSize({width:args.width,height:args.height});this.element.classList.add("rickshaw_graph");this.vis=d3.select(this.element).append("svg:svg").attr("width",this.width).attr("height",this.height);this.discoverRange()};this._loadRenderers=function(){for(var name in Rickshaw.Graph.Renderer){if(!name||!Rickshaw.Graph.Renderer.hasOwnProperty(name))continue;var r=Rickshaw.Graph.Renderer[name];if(!r||!r.prototype||!r.prototype.render)continue;self.registerRenderer(new r({graph:self}))}};this.validateSeries=function(series){if(!Array.isArray(series)&&!(series instanceof Rickshaw.Series)){var seriesSignature=Object.prototype.toString.apply(series);throw"series is not an array: "+seriesSignature}var pointsCount;series.forEach(function(s){if(!(s instanceof Object)){throw"series element is not an object: "+s}if(!s.data){throw"series has no data: "+JSON.stringify(s)}if(!Array.isArray(s.data)){throw"series data is not an array: "+JSON.stringify(s.data)}if(s.data.length>0){var x=s.data[0].x;var y=s.data[0].y;if(typeof x!="number"||typeof y!="number"&&y!==null){throw"x and y properties of points should be numbers instead of "+typeof x+" and "+typeof y}}if(s.data.length>=3){if(s.data[2].x<s.data[1].x||s.data[1].x<s.data[0].x||s.data[s.data.length-1].x<s.data[0].x){throw"series data needs to be sorted on x values for series name: "+s.name}}},this)};this.dataDomain=function(){var data=this.series.map(function(s){return s.data});var min=d3.min(data.map(function(d){return d[0].x}));var max=d3.max(data.map(function(d){return d[d.length-1].x}));return[min,max]};this.discoverRange=function(){var domain=this.renderer.domain();this.x=(this.xScale||d3.scale.linear()).copy().domain(domain.x).range([0,this.width]);this.y=(this.yScale||d3.scale.linear()).copy().domain(domain.y).range([this.height,0]);this.x.magnitude=d3.scale.linear().domain([domain.x[0]-domain.x[0],domain.x[1]-domain.x[0]]).range([0,this.width]);this.y.magnitude=d3.scale.linear().domain([domain.y[0]-domain.y[0],domain.y[1]-domain.y[0]]).range([0,this.height])};this.render=function(){var stackedData=this.stackData();this.discoverRange();this.renderer.render();this.updateCallbacks.forEach(function(callback){callback()})};this.update=this.render;this.stackData=function(){var data=this.series.active().map(function(d){return d.data}).map(function(d){return d.filter(function(d){return this._slice(d)},this)},this);var preserve=this.preserve;if(!preserve){this.series.forEach(function(series){if(series.scale){preserve=true}})}data=preserve?Rickshaw.clone(data):data;this.series.active().forEach(function(series,index){if(series.scale){var seriesData=data[index];if(seriesData){seriesData.forEach(function(d){d.y=series.scale(d.y)})}}});this.stackData.hooks.data.forEach(function(entry){data=entry.f.apply(self,[data])});var stackedData;if(!this.renderer.unstack){this._validateStackable();var layout=d3.layout.stack();layout.offset(self.offset);stackedData=layout(data)}stackedData=stackedData||data;if(this.renderer.unstack){stackedData.forEach(function(seriesData){seriesData.forEach(function(d){d.y0=d.y0===undefined?0:d.y0})})}this.stackData.hooks.after.forEach(function(entry){stackedData=entry.f.apply(self,[data])});var i=0;this.series.forEach(function(series){if(series.disabled)return;series.stack=stackedData[i++]});this.stackedData=stackedData;return stackedData};this._validateStackable=function(){var series=this.series;var pointsCount;series.forEach(function(s){pointsCount=pointsCount||s.data.length;if(pointsCount&&s.data.length!=pointsCount){throw"stacked series cannot have differing numbers of points: "+pointsCount+" vs "+s.data.length+"; see Rickshaw.Series.fill()"}},this)};this.stackData.hooks={data:[],after:[]};this._slice=function(d){if(this.window.xMin||this.window.xMax){var isInRange=true;if(this.window.xMin&&d.x<this.window.xMin)isInRange=false;if(this.window.xMax&&d.x>this.window.xMax)isInRange=false;return isInRange}return true};this.onUpdate=function(callback){this.updateCallbacks.push(callback)};this.onConfigure=function(callback){this.configureCallbacks.push(callback)};this.registerRenderer=function(renderer){this._renderers=this._renderers||{};this._renderers[renderer.name]=renderer};this.configure=function(args){this.config=this.config||{};if(args.width||args.height){this.setSize(args)}Rickshaw.keys(this.defaults).forEach(function(k){this.config[k]=k in args?args[k]:k in this?this[k]:this.defaults[k]},this);Rickshaw.keys(this.config).forEach(function(k){this[k]=this.config[k]},this);if("stack"in args)args.unstack=!args.stack;var renderer=args.renderer||this.renderer&&this.renderer.name||"stack";this.setRenderer(renderer,args);this.configureCallbacks.forEach(function(callback){callback(args)})};this.setRenderer=function(r,args){if(typeof r=="function"){this.renderer=new r({graph:self});this.registerRenderer(this.renderer)}else{if(!this._renderers[r]){throw"couldn't find renderer "+r}this.renderer=this._renderers[r]}if(typeof args=="object"){this.renderer.configure(args)}};this.setSize=function(args){args=args||{};if(typeof window!==undefined){var style=window.getComputedStyle(this.element,null);var elementWidth=parseInt(style.getPropertyValue("width"),10);var elementHeight=parseInt(style.getPropertyValue("height"),10)}this.width=args.width||elementWidth||400;this.height=args.height||elementHeight||250;this.vis&&this.vis.attr("width",this.width).attr("height",this.height)};this.initialize(args)};Rickshaw.namespace("Rickshaw.Fixtures.Color");Rickshaw.Fixtures.Color=function(){this.schemes={};this.schemes.spectrum14=["#ecb796","#dc8f70","#b2a470","#92875a","#716c49","#d2ed82","#bbe468","#a1d05d","#e7cbe6","#d8aad6","#a888c2","#9dc2d3","#649eb9","#387aa3"].reverse();this.schemes.spectrum2000=["#57306f","#514c76","#646583","#738394","#6b9c7d","#84b665","#a7ca50","#bfe746","#e2f528","#fff726","#ecdd00","#d4b11d","#de8800","#de4800","#c91515","#9a0000","#7b0429","#580839","#31082b"];this.schemes.spectrum2001=["#2f243f","#3c2c55","#4a3768","#565270","#6b6b7c","#72957f","#86ad6e","#a1bc5e","#b8d954","#d3e04e","#ccad2a","#cc8412","#c1521d","#ad3821","#8a1010","#681717","#531e1e","#3d1818","#320a1b"];this.schemes.classic9=["#423d4f","#4a6860","#848f39","#a2b73c","#ddcb53","#c5a32f","#7d5836","#963b20","#7c2626","#491d37","#2f254a"].reverse();this.schemes.httpStatus={503:"#ea5029",502:"#d23f14",500:"#bf3613",410:"#efacea",409:"#e291dc",403:"#f457e8",408:"#e121d2",401:"#b92dae",405:"#f47ceb",404:"#a82a9f",400:"#b263c6",301:"#6fa024",302:"#87c32b",307:"#a0d84c",304:"#28b55c",200:"#1a4f74",206:"#27839f",201:"#52adc9",202:"#7c979f",203:"#a5b8bd",204:"#c1cdd1"};this.schemes.colorwheel=["#b5b6a9","#858772","#785f43","#96557e","#4682b4","#65b9ac","#73c03a","#cb513a"].reverse();this.schemes.cool=["#5e9d2f","#73c03a","#4682b4","#7bc3b8","#a9884e","#c1b266","#a47493","#c09fb5"];this.schemes.munin=["#00cc00","#0066b3","#ff8000","#ffcc00","#330099","#990099","#ccff00","#ff0000","#808080","#008f00","#00487d","#b35a00","#b38f00","#6b006b","#8fb300","#b30000","#bebebe","#80ff80","#80c9ff","#ffc080","#ffe680","#aa80ff","#ee00cc","#ff8080","#666600","#ffbfff","#00ffcc","#cc6699","#999900"]};Rickshaw.namespace("Rickshaw.Fixtures.RandomData");Rickshaw.Fixtures.RandomData=function(timeInterval){var addData;timeInterval=timeInterval||1;var lastRandomValue=200;var timeBase=Math.floor((new Date).getTime()/1e3);this.addData=function(data){var randomValue=Math.random()*100+15+lastRandomValue;var index=data[0].length;var counter=1;data.forEach(function(series){var randomVariance=Math.random()*20;var v=randomValue/25+counter++ +(Math.cos(index*counter*11/960)+2)*15+(Math.cos(index/7)+2)*7+(Math.cos(index/17)+2)*1;series.push({x:index*timeInterval+timeBase,y:v+randomVariance})});lastRandomValue=randomValue*.85};this.removeData=function(data){data.forEach(function(series){series.shift()});timeBase+=timeInterval}};Rickshaw.namespace("Rickshaw.Fixtures.Time");Rickshaw.Fixtures.Time=function(){var self=this;this.months=["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"];this.units=[{name:"decade",seconds:86400*365.25*10,formatter:function(d){return parseInt(d.getUTCFullYear()/10,10)*10}},{name:"year",seconds:86400*365.25,formatter:function(d){return d.getUTCFullYear()}},{name:"month",seconds:86400*30.5,formatter:function(d){return self.months[d.getUTCMonth()]}},{name:"week",seconds:86400*7,formatter:function(d){return self.formatDate(d)}},{name:"day",seconds:86400,formatter:function(d){return d.getUTCDate()}},{name:"6 hour",seconds:3600*6,formatter:function(d){return self.formatTime(d)}},{name:"hour",seconds:3600,formatter:function(d){return self.formatTime(d)}},{name:"15 minute",seconds:60*15,formatter:function(d){return self.formatTime(d)}},{name:"minute",seconds:60,formatter:function(d){return d.getUTCMinutes()}},{name:"15 second",seconds:15,formatter:function(d){return d.getUTCSeconds()+"s"}},{name:"second",seconds:1,formatter:function(d){return d.getUTCSeconds()+"s"}},{name:"decisecond",seconds:1/10,formatter:function(d){return d.getUTCMilliseconds()+"ms"}},{name:"centisecond",seconds:1/100,formatter:function(d){return d.getUTCMilliseconds()+"ms"}}];this.unit=function(unitName){return this.units.filter(function(unit){return unitName==unit.name}).shift()};this.formatDate=function(d){return d3.time.format("%b %e")(d)};this.formatTime=function(d){return d.toUTCString().match(/(\d+:\d+):/)[1]};this.ceil=function(time,unit){var date,floor,year;if(unit.name=="month"){date=new Date(time*1e3);floor=Date.UTC(date.getUTCFullYear(),date.getUTCMonth())/1e3;if(floor==time)return time;year=date.getUTCFullYear();var month=date.getUTCMonth();if(month==11){month=0;year=year+1}else{month+=1}return Date.UTC(year,month)/1e3}if(unit.name=="year"){date=new Date(time*1e3);floor=Date.UTC(date.getUTCFullYear(),0)/1e3;if(floor==time)return time;year=date.getUTCFullYear()+1;return Date.UTC(year,0)/1e3}return Math.ceil(time/unit.seconds)*unit.seconds}};Rickshaw.namespace("Rickshaw.Fixtures.Time.Local");Rickshaw.Fixtures.Time.Local=function(){var self=this;this.months=["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"];this.units=[{name:"decade",seconds:86400*365.25*10,formatter:function(d){return parseInt(d.getFullYear()/10,10)*10}},{name:"year",seconds:86400*365.25,formatter:function(d){return d.getFullYear()}},{name:"month",seconds:86400*30.5,formatter:function(d){return self.months[d.getMonth()]}},{name:"week",seconds:86400*7,formatter:function(d){return self.formatDate(d)}},{name:"day",seconds:86400,formatter:function(d){return d.getDate()}},{name:"6 hour",seconds:3600*6,formatter:function(d){return self.formatTime(d)}},{name:"hour",seconds:3600,formatter:function(d){return self.formatTime(d)}},{name:"15 minute",seconds:60*15,formatter:function(d){return self.formatTime(d)}},{name:"minute",seconds:60,formatter:function(d){return d.getMinutes()}},{name:"15 second",seconds:15,formatter:function(d){return d.getSeconds()+"s"}},{name:"second",seconds:1,formatter:function(d){return d.getSeconds()+"s"}},{name:"decisecond",seconds:1/10,formatter:function(d){return d.getMilliseconds()+"ms"}},{name:"centisecond",seconds:1/100,formatter:function(d){return d.getMilliseconds()+"ms"}}];this.unit=function(unitName){return this.units.filter(function(unit){return unitName==unit.name}).shift()};this.formatDate=function(d){return d3.time.format("%b %e")(d)};this.formatTime=function(d){return d.toString().match(/(\d+:\d+):/)[1]};this.ceil=function(time,unit){var date,floor,year;if(unit.name=="day"){var nearFuture=new Date((time+unit.seconds-1)*1e3);var rounded=new Date(0);rounded.setMilliseconds(0);rounded.setSeconds(0);rounded.setMinutes(0);rounded.setHours(0);rounded.setDate(nearFuture.getDate());rounded.setMonth(nearFuture.getMonth());rounded.setFullYear(nearFuture.getFullYear());return rounded.getTime()/1e3}if(unit.name=="month"){date=new Date(time*1e3);floor=new Date(date.getFullYear(),date.getMonth()).getTime()/1e3;if(floor==time)return time;year=date.getFullYear();var month=date.getMonth();if(month==11){month=0;year=year+1}else{month+=1}return new Date(year,month).getTime()/1e3}if(unit.name=="year"){date=new Date(time*1e3);floor=new Date(date.getUTCFullYear(),0).getTime()/1e3;if(floor==time)return time;year=date.getFullYear()+1;return new Date(year,0).getTime()/1e3}return Math.ceil(time/unit.seconds)*unit.seconds}};Rickshaw.namespace("Rickshaw.Fixtures.Number");Rickshaw.Fixtures.Number.formatKMBT=function(y){var abs_y=Math.abs(y);if(abs_y>=1e12){return y/1e12+"T"}else if(abs_y>=1e9){return y/1e9+"B"}else if(abs_y>=1e6){return y/1e6+"M"}else if(abs_y>=1e3){return y/1e3+"K"}else if(abs_y<1&&y>0){return y.toFixed(2)}else if(abs_y===0){return""}else{return y}};Rickshaw.Fixtures.Number.formatBase1024KMGTP=function(y){var abs_y=Math.abs(y);if(abs_y>=0x4000000000000){return y/0x4000000000000+"P"}else if(abs_y>=1099511627776){return y/1099511627776+"T"}else if(abs_y>=1073741824){return y/1073741824+"G"}else if(abs_y>=1048576){return y/1048576+"M"}else if(abs_y>=1024){return y/1024+"K"}else if(abs_y<1&&y>0){return y.toFixed(2)}else if(abs_y===0){return""}else{return y}};Rickshaw.namespace("Rickshaw.Color.Palette");Rickshaw.Color.Palette=function(args){var color=new Rickshaw.Fixtures.Color;args=args||{};this.schemes={};this.scheme=color.schemes[args.scheme]||args.scheme||color.schemes.colorwheel;this.runningIndex=0;this.generatorIndex=0;if(args.interpolatedStopCount){var schemeCount=this.scheme.length-1;var i,j,scheme=[];for(i=0;i<schemeCount;i++){scheme.push(this.scheme[i]);var generator=d3.interpolateHsl(this.scheme[i],this.scheme[i+1]);for(j=1;j<args.interpolatedStopCount;j++){scheme.push(generator(1/args.interpolatedStopCount*j))}}scheme.push(this.scheme[this.scheme.length-1]);this.scheme=scheme}this.rotateCount=this.scheme.length;this.color=function(key){return this.scheme[key]||this.scheme[this.runningIndex++]||this.interpolateColor()||"#808080"};this.interpolateColor=function(){if(!Array.isArray(this.scheme))return;var color;if(this.generatorIndex==this.rotateCount*2-1){color=d3.interpolateHsl(this.scheme[this.generatorIndex],this.scheme[0])(.5);this.generatorIndex=0;this.rotateCount*=2}else{color=d3.interpolateHsl(this.scheme[this.generatorIndex],this.scheme[this.generatorIndex+1])(.5);this.generatorIndex++}this.scheme.push(color);return color}};Rickshaw.namespace("Rickshaw.Graph.Ajax");Rickshaw.Graph.Ajax=Rickshaw.Class.create({initialize:function(args){this.dataURL=args.dataURL;this.onData=args.onData||function(d){return d};this.onComplete=args.onComplete||function(){};this.onError=args.onError||function(){};this.args=args;this.request()},request:function(){jQuery.ajax({url:this.dataURL,dataType:"json",success:this.success.bind(this),error:this.error.bind(this)})},error:function(){console.log("error loading dataURL: "+this.dataURL);this.onError(this)},success:function(data,status){data=this.onData(data);this.args.series=this._splice({data:data,series:this.args.series});this.graph=this.graph||new Rickshaw.Graph(this.args);this.graph.render();this.onComplete(this)},_splice:function(args){var data=args.data;var series=args.series;if(!args.series)return data;series.forEach(function(s){var seriesKey=s.key||s.name;if(!seriesKey)throw"series needs a key or a name";data.forEach(function(d){var dataKey=d.key||d.name;if(!dataKey)throw"data needs a key or a name";if(seriesKey==dataKey){var properties=["color","name","data"];properties.forEach(function(p){if(d[p])s[p]=d[p]})}})});return series}});Rickshaw.namespace("Rickshaw.Graph.Annotate");Rickshaw.Graph.Annotate=function(args){var graph=this.graph=args.graph;this.elements={timeline:args.element};var self=this;this.data={};this.elements.timeline.classList.add("rickshaw_annotation_timeline");this.add=function(time,content,end_time){self.data[time]=self.data[time]||{boxes:[]};self.data[time].boxes.push({content:content,end:end_time})};this.update=function(){Rickshaw.keys(self.data).forEach(function(time){var annotation=self.data[time];var left=self.graph.x(time);if(left<0||left>self.graph.x.range()[1]){if(annotation.element){annotation.line.classList.add("offscreen");annotation.element.style.display="none"}annotation.boxes.forEach(function(box){if(box.rangeElement)box.rangeElement.classList.add("offscreen")});return}if(!annotation.element){var element=annotation.element=document.createElement("div");element.classList.add("annotation");this.elements.timeline.appendChild(element);element.addEventListener("click",function(e){element.classList.toggle("active");annotation.line.classList.toggle("active");annotation.boxes.forEach(function(box){if(box.rangeElement)box.rangeElement.classList.toggle("active")})},false)}annotation.element.style.left=left+"px";annotation.element.style.display="block";annotation.boxes.forEach(function(box){var element=box.element;if(!element){element=box.element=document.createElement("div");element.classList.add("content");element.innerHTML=box.content;annotation.element.appendChild(element);annotation.line=document.createElement("div");annotation.line.classList.add("annotation_line");self.graph.element.appendChild(annotation.line);if(box.end){box.rangeElement=document.createElement("div");box.rangeElement.classList.add("annotation_range");self.graph.element.appendChild(box.rangeElement)}}if(box.end){var annotationRangeStart=left;var annotationRangeEnd=Math.min(self.graph.x(box.end),self.graph.x.range()[1]);if(annotationRangeStart>annotationRangeEnd){annotationRangeEnd=left;annotationRangeStart=Math.max(self.graph.x(box.end),self.graph.x.range()[0])}var annotationRangeWidth=annotationRangeEnd-annotationRangeStart;box.rangeElement.style.left=annotationRangeStart+"px";box.rangeElement.style.width=annotationRangeWidth+"px";box.rangeElement.classList.remove("offscreen")}annotation.line.classList.remove("offscreen");annotation.line.style.left=left+"px"})},this)};this.graph.onUpdate(function(){self.update()})};Rickshaw.namespace("Rickshaw.Graph.Axis.Time");Rickshaw.Graph.Axis.Time=function(args){var self=this;this.graph=args.graph;this.elements=[];this.ticksTreatment=args.ticksTreatment||"plain";this.fixedTimeUnit=args.timeUnit;var time=args.timeFixture||new Rickshaw.Fixtures.Time;this.appropriateTimeUnit=function(){var unit;var units=time.units;var domain=this.graph.x.domain();var rangeSeconds=domain[1]-domain[0];units.forEach(function(u){if(Math.floor(rangeSeconds/u.seconds)>=2){unit=unit||u}});return unit||time.units[time.units.length-1]};this.tickOffsets=function(){var domain=this.graph.x.domain();var unit=this.fixedTimeUnit||this.appropriateTimeUnit();var count=Math.ceil((domain[1]-domain[0])/unit.seconds);var runningTick=domain[0];var offsets=[];for(var i=0;i<count;i++){var tickValue=time.ceil(runningTick,unit);runningTick=tickValue+unit.seconds/2;offsets.push({value:tickValue,unit:unit})}return offsets};this.render=function(){this.elements.forEach(function(e){e.parentNode.removeChild(e)});this.elements=[];var offsets=this.tickOffsets();offsets.forEach(function(o){if(self.graph.x(o.value)>self.graph.x.range()[1])return;var element=document.createElement("div");element.style.left=self.graph.x(o.value)+"px";element.classList.add("x_tick");element.classList.add(self.ticksTreatment);var title=document.createElement("div");title.classList.add("title");title.innerHTML=o.unit.formatter(new Date(o.value*1e3));element.appendChild(title);self.graph.element.appendChild(element);self.elements.push(element)})};this.graph.onUpdate(function(){self.render()})};Rickshaw.namespace("Rickshaw.Graph.Axis.X");Rickshaw.Graph.Axis.X=function(args){var self=this;var berthRate=.1;this.initialize=function(args){this.graph=args.graph;this.orientation=args.orientation||"top";this.pixelsPerTick=args.pixelsPerTick||75;if(args.ticks)this.staticTicks=args.ticks;if(args.tickValues)this.tickValues=args.tickValues;this.tickSize=args.tickSize||4;this.ticksTreatment=args.ticksTreatment||"plain";if(args.element){this.element=args.element;this._discoverSize(args.element,args);this.vis=d3.select(args.element).append("svg:svg").attr("height",this.height).attr("width",this.width).attr("class","rickshaw_graph x_axis_d3");this.element=this.vis[0][0];this.element.style.position="relative";this.setSize({width:args.width,height:args.height})}else{this.vis=this.graph.vis}this.graph.onUpdate(function(){self.render()})};this.setSize=function(args){args=args||{};if(!this.element)return;this._discoverSize(this.element.parentNode,args);this.vis.attr("height",this.height).attr("width",this.width*(1+berthRate));var berth=Math.floor(this.width*berthRate/2);this.element.style.left=-1*berth+"px"};this.render=function(){if(this._renderWidth!==undefined&&this.graph.width!==this._renderWidth)this.setSize({auto:true});var axis=d3.svg.axis().scale(this.graph.x).orient(this.orientation);axis.tickFormat(args.tickFormat||function(x){return x});if(this.tickValues)axis.tickValues(this.tickValues);this.ticks=this.staticTicks||Math.floor(this.graph.width/this.pixelsPerTick);var berth=Math.floor(this.width*berthRate/2)||0;var transform;if(this.orientation=="top"){var yOffset=this.height||this.graph.height;transform="translate("+berth+","+yOffset+")"}else{transform="translate("+berth+", 0)"}if(this.element){this.vis.selectAll("*").remove()}this.vis.append("svg:g").attr("class",["x_ticks_d3",this.ticksTreatment].join(" ")).attr("transform",transform).call(axis.ticks(this.ticks).tickSubdivide(0).tickSize(this.tickSize));var gridSize=(this.orientation=="bottom"?1:-1)*this.graph.height;this.graph.vis.append("svg:g").attr("class","x_grid_d3").call(axis.ticks(this.ticks).tickSubdivide(0).tickSize(gridSize)).selectAll("text").each(function(){this.parentNode.setAttribute("data-x-value",this.textContent)});this._renderHeight=this.graph.height};this._discoverSize=function(element,args){if(typeof window!=="undefined"){var style=window.getComputedStyle(element,null);var elementHeight=parseInt(style.getPropertyValue("height"),10);if(!args.auto){var elementWidth=parseInt(style.getPropertyValue("width"),10)}}this.width=(args.width||elementWidth||this.graph.width)*(1+berthRate);this.height=args.height||elementHeight||40};this.initialize(args)};Rickshaw.namespace("Rickshaw.Graph.Axis.Y");Rickshaw.Graph.Axis.Y=Rickshaw.Class.create({initialize:function(args){this.graph=args.graph;this.orientation=args.orientation||"right";this.pixelsPerTick=args.pixelsPerTick||75;if(args.ticks)this.staticTicks=args.ticks;if(args.tickValues)this.tickValues=args.tickValues;this.tickSize=args.tickSize||4;this.ticksTreatment=args.ticksTreatment||"plain";this.tickFormat=args.tickFormat||function(y){return y};this.berthRate=.1;if(args.element){this.element=args.element;this.vis=d3.select(args.element).append("svg:svg").attr("class","rickshaw_graph y_axis");this.element=this.vis[0][0];this.element.style.position="relative";this.setSize({width:args.width,height:args.height})}else{this.vis=this.graph.vis}var self=this;this.graph.onUpdate(function(){self.render()})},setSize:function(args){args=args||{};if(!this.element)return;if(typeof window!=="undefined"){var style=window.getComputedStyle(this.element.parentNode,null);var elementWidth=parseInt(style.getPropertyValue("width"),10);if(!args.auto){var elementHeight=parseInt(style.getPropertyValue("height"),10)}}this.width=args.width||elementWidth||this.graph.width*this.berthRate;this.height=args.height||elementHeight||this.graph.height;this.vis.attr("width",this.width).attr("height",this.height*(1+this.berthRate));var berth=this.height*this.berthRate;if(this.orientation=="left"){this.element.style.top=-1*berth+"px"}},render:function(){if(this._renderHeight!==undefined&&this.graph.height!==this._renderHeight)this.setSize({auto:true});this.ticks=this.staticTicks||Math.floor(this.graph.height/this.pixelsPerTick);var axis=this._drawAxis(this.graph.y);this._drawGrid(axis);this._renderHeight=this.graph.height},_drawAxis:function(scale){var axis=d3.svg.axis().scale(scale).orient(this.orientation);axis.tickFormat(this.tickFormat);if(this.tickValues)axis.tickValues(this.tickValues);if(this.orientation=="left"){var berth=this.height*this.berthRate;var transform="translate("+this.width+", "+berth+")"}if(this.element){this.vis.selectAll("*").remove()}this.vis.append("svg:g").attr("class",["y_ticks",this.ticksTreatment].join(" ")).attr("transform",transform).call(axis.ticks(this.ticks).tickSubdivide(0).tickSize(this.tickSize));return axis},_drawGrid:function(axis){var gridSize=(this.orientation=="right"?1:-1)*this.graph.width;this.graph.vis.append("svg:g").attr("class","y_grid").call(axis.ticks(this.ticks).tickSubdivide(0).tickSize(gridSize)).selectAll("text").each(function(){this.parentNode.setAttribute("data-y-value",this.textContent)
})}});Rickshaw.namespace("Rickshaw.Graph.Axis.Y.Scaled");Rickshaw.Graph.Axis.Y.Scaled=Rickshaw.Class.create(Rickshaw.Graph.Axis.Y,{initialize:function($super,args){if(typeof args.scale==="undefined"){throw new Error("Scaled requires scale")}this.scale=args.scale;if(typeof args.grid==="undefined"){this.grid=true}else{this.grid=args.grid}$super(args)},_drawAxis:function($super,scale){var domain=this.scale.domain();var renderDomain=this.graph.renderer.domain().y;var extents=[Math.min.apply(Math,domain),Math.max.apply(Math,domain)];var extentMap=d3.scale.linear().domain([0,1]).range(extents);var adjExtents=[extentMap(renderDomain[0]),extentMap(renderDomain[1])];var adjustment=d3.scale.linear().domain(extents).range(adjExtents);var adjustedScale=this.scale.copy().domain(domain.map(adjustment)).range(scale.range());return $super(adjustedScale)},_drawGrid:function($super,axis){if(this.grid){$super(axis)}}});Rickshaw.namespace("Rickshaw.Graph.Behavior.Series.Highlight");Rickshaw.Graph.Behavior.Series.Highlight=function(args){this.graph=args.graph;this.legend=args.legend;var self=this;var colorSafe={};var activeLine=null;var disabledColor=args.disabledColor||function(seriesColor){return d3.interpolateRgb(seriesColor,d3.rgb("#d8d8d8"))(.8).toString()};this.addHighlightEvents=function(l){l.element.addEventListener("mouseover",function(e){if(activeLine)return;else activeLine=l;self.legend.lines.forEach(function(line){if(l===line){if(self.graph.renderer.unstack&&(line.series.renderer?line.series.renderer.unstack:true)){var seriesIndex=self.graph.series.indexOf(line.series);line.originalIndex=seriesIndex;var series=self.graph.series.splice(seriesIndex,1)[0];self.graph.series.push(series)}return}colorSafe[line.series.name]=colorSafe[line.series.name]||line.series.color;line.series.color=disabledColor(line.series.color)});self.graph.update()},false);l.element.addEventListener("mouseout",function(e){if(!activeLine)return;else activeLine=null;self.legend.lines.forEach(function(line){if(l===line&&line.hasOwnProperty("originalIndex")){var series=self.graph.series.pop();self.graph.series.splice(line.originalIndex,0,series);delete line.originalIndex}if(colorSafe[line.series.name]){line.series.color=colorSafe[line.series.name]}});self.graph.update()},false)};if(this.legend){this.legend.lines.forEach(function(l){self.addHighlightEvents(l)})}};Rickshaw.namespace("Rickshaw.Graph.Behavior.Series.Order");Rickshaw.Graph.Behavior.Series.Order=function(args){this.graph=args.graph;this.legend=args.legend;var self=this;if(typeof window.jQuery=="undefined"){throw"couldn't find jQuery at window.jQuery"}if(typeof window.jQuery.ui=="undefined"){throw"couldn't find jQuery UI at window.jQuery.ui"}jQuery(function(){jQuery(self.legend.list).sortable({containment:"parent",tolerance:"pointer",update:function(event,ui){var series=[];jQuery(self.legend.list).find("li").each(function(index,item){if(!item.series)return;series.push(item.series)});for(var i=self.graph.series.length-1;i>=0;i--){self.graph.series[i]=series.shift()}self.graph.update()}});jQuery(self.legend.list).disableSelection()});this.graph.onUpdate(function(){var h=window.getComputedStyle(self.legend.element).height;self.legend.element.style.height=h})};Rickshaw.namespace("Rickshaw.Graph.Behavior.Series.Toggle");Rickshaw.Graph.Behavior.Series.Toggle=function(args){this.graph=args.graph;this.legend=args.legend;var self=this;this.addAnchor=function(line){var anchor=document.createElement("a");anchor.innerHTML="&#10004;";anchor.classList.add("action");line.element.insertBefore(anchor,line.element.firstChild);anchor.onclick=function(e){if(line.series.disabled){line.series.enable();line.element.classList.remove("disabled")}else{if(this.graph.series.filter(function(s){return!s.disabled}).length<=1)return;line.series.disable();line.element.classList.add("disabled")}self.graph.update()}.bind(this);var label=line.element.getElementsByTagName("span")[0];label.onclick=function(e){var disableAllOtherLines=line.series.disabled;if(!disableAllOtherLines){for(var i=0;i<self.legend.lines.length;i++){var l=self.legend.lines[i];if(line.series===l.series){}else if(l.series.disabled){}else{disableAllOtherLines=true;break}}}if(disableAllOtherLines){line.series.enable();line.element.classList.remove("disabled");self.legend.lines.forEach(function(l){if(line.series===l.series){}else{l.series.disable();l.element.classList.add("disabled")}})}else{self.legend.lines.forEach(function(l){l.series.enable();l.element.classList.remove("disabled")})}self.graph.update()}};if(this.legend){var $=jQuery;if(typeof $!="undefined"&&$(this.legend.list).sortable){$(this.legend.list).sortable({start:function(event,ui){ui.item.bind("no.onclick",function(event){event.preventDefault()})},stop:function(event,ui){setTimeout(function(){ui.item.unbind("no.onclick")},250)}})}this.legend.lines.forEach(function(l){self.addAnchor(l)})}this._addBehavior=function(){this.graph.series.forEach(function(s){s.disable=function(){if(self.graph.series.length<=1){throw"only one series left"}s.disabled=true};s.enable=function(){s.disabled=false}})};this._addBehavior();this.updateBehaviour=function(){this._addBehavior()}};Rickshaw.namespace("Rickshaw.Graph.HoverDetail");Rickshaw.Graph.HoverDetail=Rickshaw.Class.create({initialize:function(args){var graph=this.graph=args.graph;this.xFormatter=args.xFormatter||function(x){return new Date(x*1e3).toUTCString()};this.yFormatter=args.yFormatter||function(y){return y===null?y:y.toFixed(2)};var element=this.element=document.createElement("div");element.className="detail";this.visible=true;graph.element.appendChild(element);this.lastEvent=null;this._addListeners();this.onShow=args.onShow;this.onHide=args.onHide;this.onRender=args.onRender;this.formatter=args.formatter||this.formatter},formatter:function(series,x,y,formattedX,formattedY,d){return series.name+":&nbsp;"+formattedY},update:function(e){e=e||this.lastEvent;if(!e)return;this.lastEvent=e;if(!e.target.nodeName.match(/^(path|svg|rect|circle)$/))return;var graph=this.graph;var eventX=e.offsetX||e.layerX;var eventY=e.offsetY||e.layerY;var j=0;var points=[];var nearestPoint;this.graph.series.active().forEach(function(series){var data=this.graph.stackedData[j++];if(!data.length)return;var domainX=graph.x.invert(eventX);var domainIndexScale=d3.scale.linear().domain([data[0].x,data.slice(-1)[0].x]).range([0,data.length-1]);var approximateIndex=Math.round(domainIndexScale(domainX));if(approximateIndex==data.length-1)approximateIndex--;var dataIndex=Math.min(approximateIndex||0,data.length-1);for(var i=approximateIndex;i<data.length-1;){if(!data[i]||!data[i+1])break;if(data[i].x<=domainX&&data[i+1].x>domainX){dataIndex=Math.abs(domainX-data[i].x)<Math.abs(domainX-data[i+1].x)?i:i+1;break}if(data[i+1].x<=domainX){i++}else{i--}}if(dataIndex<0)dataIndex=0;var value=data[dataIndex];var distance=Math.sqrt(Math.pow(Math.abs(graph.x(value.x)-eventX),2)+Math.pow(Math.abs(graph.y(value.y+value.y0)-eventY),2));var xFormatter=series.xFormatter||this.xFormatter;var yFormatter=series.yFormatter||this.yFormatter;var point={formattedXValue:xFormatter(value.x),formattedYValue:yFormatter(series.scale?series.scale.invert(value.y):value.y),series:series,value:value,distance:distance,order:j,name:series.name};if(!nearestPoint||distance<nearestPoint.distance){nearestPoint=point}points.push(point)},this);if(!nearestPoint)return;nearestPoint.active=true;var domainX=nearestPoint.value.x;var formattedXValue=nearestPoint.formattedXValue;this.element.innerHTML="";this.element.style.left=graph.x(domainX)+"px";this.visible&&this.render({points:points,detail:points,mouseX:eventX,mouseY:eventY,formattedXValue:formattedXValue,domainX:domainX})},hide:function(){this.visible=false;this.element.classList.add("inactive");if(typeof this.onHide=="function"){this.onHide()}},show:function(){this.visible=true;this.element.classList.remove("inactive");if(typeof this.onShow=="function"){this.onShow()}},render:function(args){var graph=this.graph;var points=args.points;var point=points.filter(function(p){return p.active}).shift();if(point.value.y===null)return;var formattedXValue=point.formattedXValue;var formattedYValue=point.formattedYValue;this.element.innerHTML="";this.element.style.left=graph.x(point.value.x)+"px";var xLabel=document.createElement("div");xLabel.className="x_label";xLabel.innerHTML=formattedXValue;this.element.appendChild(xLabel);var item=document.createElement("div");item.className="item";var series=point.series;var actualY=series.scale?series.scale.invert(point.value.y):point.value.y;item.innerHTML=this.formatter(series,point.value.x,actualY,formattedXValue,formattedYValue,point);item.style.top=this.graph.y(point.value.y0+point.value.y)+"px";this.element.appendChild(item);var dot=document.createElement("div");dot.className="dot";dot.style.top=item.style.top;dot.style.borderColor=series.color;this.element.appendChild(dot);if(point.active){item.classList.add("active");dot.classList.add("active")}var alignables=[xLabel,item];alignables.forEach(function(el){el.classList.add("left")});this.show();var leftAlignError=this._calcLayoutError(alignables);if(leftAlignError>0){alignables.forEach(function(el){el.classList.remove("left");el.classList.add("right")});var rightAlignError=this._calcLayoutError(alignables);if(rightAlignError>leftAlignError){alignables.forEach(function(el){el.classList.remove("right");el.classList.add("left")})}}if(typeof this.onRender=="function"){this.onRender(args)}},_calcLayoutError:function(alignables){var parentRect=this.element.parentNode.getBoundingClientRect();var error=0;var alignRight=alignables.forEach(function(el){var rect=el.getBoundingClientRect();if(!rect.width){return}if(rect.right>parentRect.right){error+=rect.right-parentRect.right}if(rect.left<parentRect.left){error+=parentRect.left-rect.left}});return error},_addListeners:function(){this.graph.element.addEventListener("mousemove",function(e){this.visible=true;this.update(e)}.bind(this),false);this.graph.onUpdate(function(){this.update()}.bind(this));this.graph.element.addEventListener("mouseout",function(e){if(e.relatedTarget&&!(e.relatedTarget.compareDocumentPosition(this.graph.element)&Node.DOCUMENT_POSITION_CONTAINS)){this.hide()}}.bind(this),false)}});Rickshaw.namespace("Rickshaw.Graph.JSONP");Rickshaw.Graph.JSONP=Rickshaw.Class.create(Rickshaw.Graph.Ajax,{request:function(){jQuery.ajax({url:this.dataURL,dataType:"jsonp",success:this.success.bind(this),error:this.error.bind(this)})}});Rickshaw.namespace("Rickshaw.Graph.Legend");Rickshaw.Graph.Legend=Rickshaw.Class.create({className:"rickshaw_legend",initialize:function(args){this.element=args.element;this.graph=args.graph;this.naturalOrder=args.naturalOrder;this.element.classList.add(this.className);this.list=document.createElement("ul");this.element.appendChild(this.list);this.render();this.graph.onUpdate(function(){})},render:function(){var self=this;while(this.list.firstChild){this.list.removeChild(this.list.firstChild)}this.lines=[];var series=this.graph.series.map(function(s){return s});if(!this.naturalOrder){series=series.reverse()}series.forEach(function(s){self.addLine(s)})},addLine:function(series){var line=document.createElement("li");line.className="line";if(series.disabled){line.className+=" disabled"}if(series.className){d3.select(line).classed(series.className,true)}var swatch=document.createElement("div");swatch.className="swatch";swatch.style.backgroundColor=series.color;line.appendChild(swatch);var label=document.createElement("span");label.className="label";label.innerHTML=series.name;line.appendChild(label);this.list.appendChild(line);line.series=series;if(series.noLegend){line.style.display="none"}var _line={element:line,series:series};if(this.shelving){this.shelving.addAnchor(_line);this.shelving.updateBehaviour()}if(this.highlighter){this.highlighter.addHighlightEvents(_line)}this.lines.push(_line);return line}});Rickshaw.namespace("Rickshaw.Graph.RangeSlider");Rickshaw.Graph.RangeSlider=Rickshaw.Class.create({initialize:function(args){var element=this.element=args.element;var graph=this.graph=args.graph;this.slideCallbacks=[];this.build();graph.onUpdate(function(){this.update()}.bind(this))},build:function(){var element=this.element;var graph=this.graph;var $=jQuery;var domain=graph.dataDomain();var self=this;$(function(){$(element).slider({range:true,min:domain[0],max:domain[1],values:[domain[0],domain[1]],slide:function(event,ui){if(ui.values[1]<=ui.values[0])return;graph.window.xMin=ui.values[0];graph.window.xMax=ui.values[1];graph.update();var domain=graph.dataDomain();if(domain[0]==ui.values[0]){graph.window.xMin=undefined}if(domain[1]==ui.values[1]){graph.window.xMax=undefined}self.slideCallbacks.forEach(function(callback){callback(graph,graph.window.xMin,graph.window.xMax)})}})});$(element)[0].style.width=graph.width+"px"},update:function(){var element=this.element;var graph=this.graph;var $=jQuery;var values=$(element).slider("option","values");var domain=graph.dataDomain();$(element).slider("option","min",domain[0]);$(element).slider("option","max",domain[1]);if(graph.window.xMin==null){values[0]=domain[0]}if(graph.window.xMax==null){values[1]=domain[1]}$(element).slider("option","values",values)},onSlide:function(callback){this.slideCallbacks.push(callback)}});Rickshaw.namespace("Rickshaw.Graph.RangeSlider.Preview");Rickshaw.Graph.RangeSlider.Preview=Rickshaw.Class.create({initialize:function(args){if(!args.element)throw"Rickshaw.Graph.RangeSlider.Preview needs a reference to an element";if(!args.graph&&!args.graphs)throw"Rickshaw.Graph.RangeSlider.Preview needs a reference to an graph or an array of graphs";this.element=args.element;this.element.style.position="relative";this.graphs=args.graph?[args.graph]:args.graphs;this.defaults={height:75,width:400,gripperColor:undefined,frameTopThickness:3,frameHandleThickness:10,frameColor:"#d4d4d4",frameOpacity:1,minimumFrameWidth:0,heightRatio:.2};this.heightRatio=args.heightRatio||this.defaults.heightRatio;this.defaults.gripperColor=d3.rgb(this.defaults.frameColor).darker().toString();this.configureCallbacks=[];this.slideCallbacks=[];this.previews=[];if(!args.width)this.widthFromGraph=true;if(!args.height)this.heightFromGraph=true;if(this.widthFromGraph||this.heightFromGraph){this.graphs[0].onConfigure(function(){this.configure(args);this.render()}.bind(this))}args.width=args.width||this.graphs[0].width||this.defaults.width;args.height=args.height||this.graphs[0].height*this.heightRatio||this.defaults.height;this.configure(args);this.render()},onSlide:function(callback){this.slideCallbacks.push(callback)},onConfigure:function(callback){this.configureCallbacks.push(callback)},configure:function(args){this.config=this.config||{};this.configureCallbacks.forEach(function(callback){callback(args)});Rickshaw.keys(this.defaults).forEach(function(k){this.config[k]=k in args?args[k]:k in this.config?this.config[k]:this.defaults[k]},this);if("width"in args||"height"in args){if(this.widthFromGraph){this.config.width=this.graphs[0].width}if(this.heightFromGraph){this.config.height=this.graphs[0].height*this.heightRatio;this.previewHeight=this.config.height}this.previews.forEach(function(preview){var height=this.previewHeight/this.graphs.length-this.config.frameTopThickness*2;var width=this.config.width-this.config.frameHandleThickness*2;preview.setSize({width:width,height:height});if(this.svg){var svgHeight=height+this.config.frameHandleThickness*2;var svgWidth=width+this.config.frameHandleThickness*2;this.svg.style("width",svgWidth+"px");this.svg.style("height",svgHeight+"px")}},this)}},render:function(){var self=this;this.svg=d3.select(this.element).selectAll("svg.rickshaw_range_slider_preview").data([null]);this.previewHeight=this.config.height-this.config.frameTopThickness*2;this.previewWidth=this.config.width-this.config.frameHandleThickness*2;this.currentFrame=[0,this.previewWidth];var buildGraph=function(parent,index){var graphArgs=Rickshaw.extend({},parent.config);var height=self.previewHeight/self.graphs.length;var renderer=parent.renderer.name;Rickshaw.extend(graphArgs,{element:this.appendChild(document.createElement("div")),height:height,width:self.previewWidth,series:parent.series,renderer:renderer});var graph=new Rickshaw.Graph(graphArgs);self.previews.push(graph);parent.onUpdate(function(){graph.render();self.render()});parent.onConfigure(function(args){delete args.height;args.width=args.width-self.config.frameHandleThickness*2;graph.configure(args);graph.render()});graph.render()};var graphContainer=d3.select(this.element).selectAll("div.rickshaw_range_slider_preview_container").data(this.graphs);var translateCommand="translate("+this.config.frameHandleThickness+"px, "+this.config.frameTopThickness+"px)";graphContainer.enter().append("div").classed("rickshaw_range_slider_preview_container",true).style("-webkit-transform",translateCommand).style("-moz-transform",translateCommand).style("-ms-transform",translateCommand).style("transform",translateCommand).each(buildGraph);graphContainer.exit().remove();var masterGraph=this.graphs[0];var domainScale=d3.scale.linear().domain([0,this.previewWidth]).range(masterGraph.dataDomain());var currentWindow=[masterGraph.window.xMin,masterGraph.window.xMax];this.currentFrame[0]=currentWindow[0]===undefined?0:Math.round(domainScale.invert(currentWindow[0]));if(this.currentFrame[0]<0)this.currentFrame[0]=0;this.currentFrame[1]=currentWindow[1]===undefined?this.previewWidth:domainScale.invert(currentWindow[1]);if(this.currentFrame[1]-this.currentFrame[0]<self.config.minimumFrameWidth){this.currentFrame[1]=(this.currentFrame[0]||0)+self.config.minimumFrameWidth}this.svg.enter().append("svg").classed("rickshaw_range_slider_preview",true).style("height",this.config.height+"px").style("width",this.config.width+"px").style("position","absolute").style("top",0);this._renderDimming();this._renderFrame();this._renderGrippers();this._renderHandles();this._renderMiddle();this._registerMouseEvents()},_renderDimming:function(){var element=this.svg.selectAll("path.dimming").data([null]);element.enter().append("path").attr("fill","white").attr("fill-opacity","0.7").attr("fill-rule","evenodd").classed("dimming",true);var path="";path+=" M "+this.config.frameHandleThickness+" "+this.config.frameTopThickness;path+=" h "+this.previewWidth;path+=" v "+this.previewHeight;path+=" h "+-this.previewWidth;path+=" z ";path+=" M "+Math.max(this.currentFrame[0],this.config.frameHandleThickness)+" "+this.config.frameTopThickness;path+=" H "+Math.min(this.currentFrame[1]+this.config.frameHandleThickness*2,this.previewWidth+this.config.frameHandleThickness);path+=" v "+this.previewHeight;path+=" H "+Math.max(this.currentFrame[0],this.config.frameHandleThickness);path+=" z";element.attr("d",path)},_renderFrame:function(){var element=this.svg.selectAll("path.frame").data([null]);element.enter().append("path").attr("stroke","white").attr("stroke-width","1px").attr("stroke-linejoin","round").attr("fill",this.config.frameColor).attr("fill-opacity",this.config.frameOpacity).attr("fill-rule","evenodd").classed("frame",true);var path="";path+=" M "+this.currentFrame[0]+" 0";path+=" H "+(this.currentFrame[1]+this.config.frameHandleThickness*2);path+=" V "+this.config.height;path+=" H "+this.currentFrame[0];path+=" z";path+=" M "+(this.currentFrame[0]+this.config.frameHandleThickness)+" "+this.config.frameTopThickness;path+=" H "+(this.currentFrame[1]+this.config.frameHandleThickness);path+=" v "+this.previewHeight;path+=" H "+(this.currentFrame[0]+this.config.frameHandleThickness);path+=" z";element.attr("d",path)},_renderGrippers:function(){var gripper=this.svg.selectAll("path.gripper").data([null]);gripper.enter().append("path").attr("stroke",this.config.gripperColor).classed("gripper",true);var path="";[.4,.6].forEach(function(spacing){path+=" M "+Math.round(this.currentFrame[0]+this.config.frameHandleThickness*spacing)+" "+Math.round(this.config.height*.3);path+=" V "+Math.round(this.config.height*.7);path+=" M "+Math.round(this.currentFrame[1]+this.config.frameHandleThickness*(1+spacing))+" "+Math.round(this.config.height*.3);path+=" V "+Math.round(this.config.height*.7)}.bind(this));gripper.attr("d",path)},_renderHandles:function(){var leftHandle=this.svg.selectAll("rect.left_handle").data([null]);leftHandle.enter().append("rect").attr("width",this.config.frameHandleThickness).style("cursor","ew-resize").style("fill-opacity","0").classed("left_handle",true);leftHandle.attr("x",this.currentFrame[0]).attr("height",this.config.height);var rightHandle=this.svg.selectAll("rect.right_handle").data([null]);rightHandle.enter().append("rect").attr("width",this.config.frameHandleThickness).style("cursor","ew-resize").style("fill-opacity","0").classed("right_handle",true);rightHandle.attr("x",this.currentFrame[1]+this.config.frameHandleThickness).attr("height",this.config.height)},_renderMiddle:function(){var middleHandle=this.svg.selectAll("rect.middle_handle").data([null]);middleHandle.enter().append("rect").style("cursor","move").style("fill-opacity","0").classed("middle_handle",true);middleHandle.attr("width",Math.max(0,this.currentFrame[1]-this.currentFrame[0])).attr("x",this.currentFrame[0]+this.config.frameHandleThickness).attr("height",this.config.height)},_registerMouseEvents:function(){var element=d3.select(this.element);var drag={target:null,start:null,stop:null,left:false,right:false,rigid:false};var self=this;function onMousemove(datum,index){drag.stop=self._getClientXFromEvent(d3.event,drag);var distanceTraveled=drag.stop-drag.start;var frameAfterDrag=self.frameBeforeDrag.slice(0);var minimumFrameWidth=self.config.minimumFrameWidth;if(drag.rigid){minimumFrameWidth=self.frameBeforeDrag[1]-self.frameBeforeDrag[0]}if(drag.left){frameAfterDrag[0]=Math.max(frameAfterDrag[0]+distanceTraveled,0)}if(drag.right){frameAfterDrag[1]=Math.min(frameAfterDrag[1]+distanceTraveled,self.previewWidth)}var currentFrameWidth=frameAfterDrag[1]-frameAfterDrag[0];if(currentFrameWidth<=minimumFrameWidth){if(drag.left){frameAfterDrag[0]=frameAfterDrag[1]-minimumFrameWidth}if(drag.right){frameAfterDrag[1]=frameAfterDrag[0]+minimumFrameWidth}if(frameAfterDrag[0]<=0){frameAfterDrag[1]-=frameAfterDrag[0];frameAfterDrag[0]=0}if(frameAfterDrag[1]>=self.previewWidth){frameAfterDrag[0]-=frameAfterDrag[1]-self.previewWidth;frameAfterDrag[1]=self.previewWidth}}self.graphs.forEach(function(graph){var domainScale=d3.scale.linear().interpolate(d3.interpolateNumber).domain([0,self.previewWidth]).range(graph.dataDomain());var windowAfterDrag=[domainScale(frameAfterDrag[0]),domainScale(frameAfterDrag[1])];self.slideCallbacks.forEach(function(callback){callback(graph,windowAfterDrag[0],windowAfterDrag[1])});if(frameAfterDrag[0]===0){windowAfterDrag[0]=undefined}if(frameAfterDrag[1]===self.previewWidth){windowAfterDrag[1]=undefined}graph.window.xMin=windowAfterDrag[0];graph.window.xMax=windowAfterDrag[1];graph.update()})}function onMousedown(){drag.target=d3.event.target;drag.start=self._getClientXFromEvent(d3.event,drag);self.frameBeforeDrag=self.currentFrame.slice();d3.event.preventDefault?d3.event.preventDefault():d3.event.returnValue=false;d3.select(document).on("mousemove.rickshaw_range_slider_preview",onMousemove);d3.select(document).on("mouseup.rickshaw_range_slider_preview",onMouseup);d3.select(document).on("touchmove.rickshaw_range_slider_preview",onMousemove);d3.select(document).on("touchend.rickshaw_range_slider_preview",onMouseup);d3.select(document).on("touchcancel.rickshaw_range_slider_preview",onMouseup)}function onMousedownLeftHandle(datum,index){drag.left=true;onMousedown()}function onMousedownRightHandle(datum,index){drag.right=true;onMousedown()}function onMousedownMiddleHandle(datum,index){drag.left=true;drag.right=true;drag.rigid=true;onMousedown()}function onMouseup(datum,index){d3.select(document).on("mousemove.rickshaw_range_slider_preview",null);d3.select(document).on("mouseup.rickshaw_range_slider_preview",null);d3.select(document).on("touchmove.rickshaw_range_slider_preview",null);d3.select(document).on("touchend.rickshaw_range_slider_preview",null);d3.select(document).on("touchcancel.rickshaw_range_slider_preview",null);delete self.frameBeforeDrag;drag.left=false;drag.right=false;drag.rigid=false}element.select("rect.left_handle").on("mousedown",onMousedownLeftHandle);element.select("rect.right_handle").on("mousedown",onMousedownRightHandle);element.select("rect.middle_handle").on("mousedown",onMousedownMiddleHandle);element.select("rect.left_handle").on("touchstart",onMousedownLeftHandle);element.select("rect.right_handle").on("touchstart",onMousedownRightHandle);element.select("rect.middle_handle").on("touchstart",onMousedownMiddleHandle)},_getClientXFromEvent:function(event,drag){switch(event.type){case"touchstart":case"touchmove":var touchList=event.changedTouches;var touch=null;for(var touchIndex=0;touchIndex<touchList.length;touchIndex++){if(touchList[touchIndex].target===drag.target){touch=touchList[touchIndex];break}}return touch!==null?touch.clientX:undefined;default:return event.clientX}}});Rickshaw.namespace("Rickshaw.Graph.Renderer");Rickshaw.Graph.Renderer=Rickshaw.Class.create({initialize:function(args){this.graph=args.graph;this.tension=args.tension||this.tension;this.configure(args)},seriesPathFactory:function(){},seriesStrokeFactory:function(){},defaults:function(){return{tension:.8,strokeWidth:2,unstack:true,padding:{top:.01,right:0,bottom:.01,left:0},stroke:false,fill:false}},domain:function(data){var stackedData=data||this.graph.stackedData||this.graph.stackData();var xMin=+Infinity;var xMax=-Infinity;var yMin=+Infinity;var yMax=-Infinity;stackedData.forEach(function(series){series.forEach(function(d){if(d.y==null)return;var y=d.y+d.y0;if(y<yMin)yMin=y;if(y>yMax)yMax=y});if(!series.length)return;if(series[0].x<xMin)xMin=series[0].x;if(series[series.length-1].x>xMax)xMax=series[series.length-1].x});xMin-=(xMax-xMin)*this.padding.left;xMax+=(xMax-xMin)*this.padding.right;yMin=this.graph.min==="auto"?yMin:this.graph.min||0;yMax=this.graph.max===undefined?yMax:this.graph.max;if(this.graph.min==="auto"||yMin<0){yMin-=(yMax-yMin)*this.padding.bottom}if(this.graph.max===undefined){yMax+=(yMax-yMin)*this.padding.top}return{x:[xMin,xMax],y:[yMin,yMax]}},render:function(args){args=args||{};var graph=this.graph;var series=args.series||graph.series;var vis=args.vis||graph.vis;vis.selectAll("*").remove();var data=series.filter(function(s){return!s.disabled}).map(function(s){return s.stack});var pathNodes=vis.selectAll("path.path").data(data).enter().append("svg:path").classed("path",true).attr("d",this.seriesPathFactory());if(this.stroke){var strokeNodes=vis.selectAll("path.stroke").data(data).enter().append("svg:path").classed("stroke",true).attr("d",this.seriesStrokeFactory())}var i=0;series.forEach(function(series){if(series.disabled)return;series.path=pathNodes[0][i];if(this.stroke)series.stroke=strokeNodes[0][i];this._styleSeries(series);i++},this)},_styleSeries:function(series){var fill=this.fill?series.color:"none";var stroke=this.stroke?series.color:"none";series.path.setAttribute("fill",fill);series.path.setAttribute("stroke",stroke);series.path.setAttribute("stroke-width",this.strokeWidth);if(series.className){d3.select(series.path).classed(series.className,true)}if(series.className&&this.stroke){d3.select(series.stroke).classed(series.className,true)}},configure:function(args){args=args||{};Rickshaw.keys(this.defaults()).forEach(function(key){if(!args.hasOwnProperty(key)){this[key]=this[key]||this.graph[key]||this.defaults()[key];return}if(typeof this.defaults()[key]=="object"){Rickshaw.keys(this.defaults()[key]).forEach(function(k){this[key][k]=args[key][k]!==undefined?args[key][k]:this[key][k]!==undefined?this[key][k]:this.defaults()[key][k]},this)}else{this[key]=args[key]!==undefined?args[key]:this[key]!==undefined?this[key]:this.graph[key]!==undefined?this.graph[key]:this.defaults()[key]}},this)},setStrokeWidth:function(strokeWidth){if(strokeWidth!==undefined){this.strokeWidth=strokeWidth}},setTension:function(tension){if(tension!==undefined){this.tension=tension}}});Rickshaw.namespace("Rickshaw.Graph.Renderer.Line");Rickshaw.Graph.Renderer.Line=Rickshaw.Class.create(Rickshaw.Graph.Renderer,{name:"line",defaults:function($super){return Rickshaw.extend($super(),{unstack:true,fill:false,stroke:true})},seriesPathFactory:function(){var graph=this.graph;var factory=d3.svg.line().x(function(d){return graph.x(d.x)}).y(function(d){return graph.y(d.y)}).interpolate(this.graph.interpolation).tension(this.tension);factory.defined&&factory.defined(function(d){return d.y!==null});return factory}});Rickshaw.namespace("Rickshaw.Graph.Renderer.Stack");Rickshaw.Graph.Renderer.Stack=Rickshaw.Class.create(Rickshaw.Graph.Renderer,{name:"stack",defaults:function($super){return Rickshaw.extend($super(),{fill:true,stroke:false,unstack:false})},seriesPathFactory:function(){var graph=this.graph;var factory=d3.svg.area().x(function(d){return graph.x(d.x)}).y0(function(d){return graph.y(d.y0)}).y1(function(d){return graph.y(d.y+d.y0)}).interpolate(this.graph.interpolation).tension(this.tension);factory.defined&&factory.defined(function(d){return d.y!==null});return factory}});Rickshaw.namespace("Rickshaw.Graph.Renderer.Bar");Rickshaw.Graph.Renderer.Bar=Rickshaw.Class.create(Rickshaw.Graph.Renderer,{name:"bar",defaults:function($super){var defaults=Rickshaw.extend($super(),{gapSize:.05,unstack:false});delete defaults.tension;return defaults},initialize:function($super,args){args=args||{};this.gapSize=args.gapSize||this.gapSize;$super(args)},domain:function($super){var domain=$super();var frequentInterval=this._frequentInterval(this.graph.stackedData.slice(-1).shift());domain.x[1]+=Number(frequentInterval.magnitude);return domain},barWidth:function(series){var frequentInterval=this._frequentInterval(series.stack);var barWidth=this.graph.x.magnitude(frequentInterval.magnitude)*(1-this.gapSize);return barWidth},render:function(args){args=args||{};var graph=this.graph;var series=args.series||graph.series;var vis=args.vis||graph.vis;vis.selectAll("*").remove();var barWidth=this.barWidth(series.active()[0]);var barXOffset=0;var activeSeriesCount=series.filter(function(s){return!s.disabled}).length;var seriesBarWidth=this.unstack?barWidth/activeSeriesCount:barWidth;var transform=function(d){var matrix=[1,0,0,d.y<0?-1:1,0,d.y<0?graph.y.magnitude(Math.abs(d.y))*2:0];return"matrix("+matrix.join(",")+")"};series.forEach(function(series){if(series.disabled)return;var barWidth=this.barWidth(series);var nodes=vis.selectAll("path").data(series.stack.filter(function(d){return d.y!==null})).enter().append("svg:rect").attr("x",function(d){return graph.x(d.x)+barXOffset}).attr("y",function(d){return graph.y(d.y0+Math.abs(d.y))*(d.y<0?-1:1)}).attr("width",seriesBarWidth).attr("height",function(d){return graph.y.magnitude(Math.abs(d.y))}).attr("transform",transform);Array.prototype.forEach.call(nodes[0],function(n){n.setAttribute("fill",series.color)});if(this.unstack)barXOffset+=seriesBarWidth},this)},_frequentInterval:function(data){var intervalCounts={};for(var i=0;i<data.length-1;i++){var interval=data[i+1].x-data[i].x;intervalCounts[interval]=intervalCounts[interval]||0;intervalCounts[interval]++}var frequentInterval={count:0,magnitude:1};Rickshaw.keys(intervalCounts).forEach(function(i){if(frequentInterval.count<intervalCounts[i]){frequentInterval={count:intervalCounts[i],magnitude:i}}});return frequentInterval}});Rickshaw.namespace("Rickshaw.Graph.Renderer.Area");Rickshaw.Graph.Renderer.Area=Rickshaw.Class.create(Rickshaw.Graph.Renderer,{name:"area",defaults:function($super){return Rickshaw.extend($super(),{unstack:false,fill:false,stroke:false})},seriesPathFactory:function(){var graph=this.graph;var factory=d3.svg.area().x(function(d){return graph.x(d.x)}).y0(function(d){return graph.y(d.y0)}).y1(function(d){return graph.y(d.y+d.y0)}).interpolate(graph.interpolation).tension(this.tension);
factory.defined&&factory.defined(function(d){return d.y!==null});return factory},seriesStrokeFactory:function(){var graph=this.graph;var factory=d3.svg.line().x(function(d){return graph.x(d.x)}).y(function(d){return graph.y(d.y+d.y0)}).interpolate(graph.interpolation).tension(this.tension);factory.defined&&factory.defined(function(d){return d.y!==null});return factory},render:function(args){args=args||{};var graph=this.graph;var series=args.series||graph.series;var vis=args.vis||graph.vis;vis.selectAll("*").remove();var method=this.unstack?"append":"insert";var data=series.filter(function(s){return!s.disabled}).map(function(s){return s.stack});var nodes=vis.selectAll("path").data(data).enter()[method]("svg:g","g");nodes.append("svg:path").attr("d",this.seriesPathFactory()).attr("class","area");if(this.stroke){nodes.append("svg:path").attr("d",this.seriesStrokeFactory()).attr("class","line")}var i=0;series.forEach(function(series){if(series.disabled)return;series.path=nodes[0][i++];this._styleSeries(series)},this)},_styleSeries:function(series){if(!series.path)return;d3.select(series.path).select(".area").attr("fill",series.color);if(this.stroke){d3.select(series.path).select(".line").attr("fill","none").attr("stroke",series.stroke||d3.interpolateRgb(series.color,"black")(.125)).attr("stroke-width",this.strokeWidth)}if(series.className){series.path.setAttribute("class",series.className)}}});Rickshaw.namespace("Rickshaw.Graph.Renderer.ScatterPlot");Rickshaw.Graph.Renderer.ScatterPlot=Rickshaw.Class.create(Rickshaw.Graph.Renderer,{name:"scatterplot",defaults:function($super){return Rickshaw.extend($super(),{unstack:true,fill:true,stroke:false,padding:{top:.01,right:.01,bottom:.01,left:.01},dotSize:4})},initialize:function($super,args){$super(args)},render:function(args){args=args||{};var graph=this.graph;var series=args.series||graph.series;var vis=args.vis||graph.vis;var dotSize=this.dotSize;vis.selectAll("*").remove();series.forEach(function(series){if(series.disabled)return;var nodes=vis.selectAll("path").data(series.stack.filter(function(d){return d.y!==null})).enter().append("svg:circle").attr("cx",function(d){return graph.x(d.x)}).attr("cy",function(d){return graph.y(d.y)}).attr("r",function(d){return"r"in d?d.r:dotSize});if(series.className){nodes.classed(series.className,true)}Array.prototype.forEach.call(nodes[0],function(n){n.setAttribute("fill",series.color)})},this)}});Rickshaw.namespace("Rickshaw.Graph.Renderer.Multi");Rickshaw.Graph.Renderer.Multi=Rickshaw.Class.create(Rickshaw.Graph.Renderer,{name:"multi",initialize:function($super,args){$super(args)},defaults:function($super){return Rickshaw.extend($super(),{unstack:true,fill:false,stroke:true})},configure:function($super,args){args=args||{};this.config=args;$super(args)},domain:function($super){this.graph.stackData();var domains=[];var groups=this._groups();this._stack(groups);groups.forEach(function(group){var data=group.series.filter(function(s){return!s.disabled}).map(function(s){return s.stack});if(!data.length)return;var domain=null;if(group.renderer&&group.renderer.domain){domain=group.renderer.domain(data)}else{domain=$super(data)}domains.push(domain)});var xMin=d3.min(domains.map(function(d){return d.x[0]}));var xMax=d3.max(domains.map(function(d){return d.x[1]}));var yMin=d3.min(domains.map(function(d){return d.y[0]}));var yMax=d3.max(domains.map(function(d){return d.y[1]}));return{x:[xMin,xMax],y:[yMin,yMax]}},_groups:function(){var graph=this.graph;var renderGroups={};graph.series.forEach(function(series){if(series.disabled)return;if(!renderGroups[series.renderer]){var ns="http://www.w3.org/2000/svg";var vis=document.createElementNS(ns,"g");graph.vis[0][0].appendChild(vis);var renderer=graph._renderers[series.renderer];var config={};var defaults=[this.defaults(),renderer.defaults(),this.config,this.graph];defaults.forEach(function(d){Rickshaw.extend(config,d)});renderer.configure(config);renderGroups[series.renderer]={renderer:renderer,series:[],vis:d3.select(vis)}}renderGroups[series.renderer].series.push(series)},this);var groups=[];Object.keys(renderGroups).forEach(function(key){var group=renderGroups[key];groups.push(group)});return groups},_stack:function(groups){groups.forEach(function(group){var series=group.series.filter(function(series){return!series.disabled});var data=series.map(function(series){return series.stack});if(!group.renderer.unstack){var layout=d3.layout.stack();var stackedData=Rickshaw.clone(layout(data));series.forEach(function(series,index){series._stack=Rickshaw.clone(stackedData[index])})}},this);return groups},render:function(){this.graph.series.forEach(function(series){if(!series.renderer){throw new Error("Each series needs a renderer for graph 'multi' renderer")}});this.graph.vis.selectAll("*").remove();var groups=this._groups();groups=this._stack(groups);groups.forEach(function(group){var series=group.series.filter(function(series){return!series.disabled});series.active=function(){return series};group.renderer.render({series:series,vis:group.vis});series.forEach(function(s){s.stack=s._stack||s.stack||s.data})})}});Rickshaw.namespace("Rickshaw.Graph.Renderer.LinePlot");Rickshaw.Graph.Renderer.LinePlot=Rickshaw.Class.create(Rickshaw.Graph.Renderer,{name:"lineplot",defaults:function($super){return Rickshaw.extend($super(),{unstack:true,fill:false,stroke:true,padding:{top:.01,right:.01,bottom:.01,left:.01},dotSize:3,strokeWidth:2})},seriesPathFactory:function(){var graph=this.graph;var factory=d3.svg.line().x(function(d){return graph.x(d.x)}).y(function(d){return graph.y(d.y)}).interpolate(this.graph.interpolation).tension(this.tension);factory.defined&&factory.defined(function(d){return d.y!==null});return factory},render:function(args){args=args||{};var graph=this.graph;var series=args.series||graph.series;var vis=args.vis||graph.vis;var dotSize=this.dotSize;vis.selectAll("*").remove();var data=series.filter(function(s){return!s.disabled}).map(function(s){return s.stack});var nodes=vis.selectAll("path").data(data).enter().append("svg:path").attr("d",this.seriesPathFactory());var i=0;series.forEach(function(series){if(series.disabled)return;series.path=nodes[0][i++];this._styleSeries(series)},this);series.forEach(function(series){if(series.disabled)return;var nodes=vis.selectAll("x").data(series.stack.filter(function(d){return d.y!==null})).enter().append("svg:circle").attr("cx",function(d){return graph.x(d.x)}).attr("cy",function(d){return graph.y(d.y)}).attr("r",function(d){return"r"in d?d.r:dotSize});Array.prototype.forEach.call(nodes[0],function(n){if(!n)return;n.setAttribute("data-color",series.color);n.setAttribute("fill","white");n.setAttribute("stroke",series.color);n.setAttribute("stroke-width",this.strokeWidth)}.bind(this))},this)}});Rickshaw.namespace("Rickshaw.Graph.Smoother");Rickshaw.Graph.Smoother=Rickshaw.Class.create({initialize:function(args){this.graph=args.graph;this.element=args.element;this.aggregationScale=1;this.build();this.graph.stackData.hooks.data.push({name:"smoother",orderPosition:50,f:this.transformer.bind(this)})},build:function(){var self=this;var $=jQuery;if(this.element){$(function(){$(self.element).slider({min:1,max:100,slide:function(event,ui){self.setScale(ui.value)}})})}},setScale:function(scale){if(scale<1){throw"scale out of range: "+scale}this.aggregationScale=scale;this.graph.update()},transformer:function(data){if(this.aggregationScale==1)return data;var aggregatedData=[];data.forEach(function(seriesData){var aggregatedSeriesData=[];while(seriesData.length){var avgX=0,avgY=0;var slice=seriesData.splice(0,this.aggregationScale);slice.forEach(function(d){avgX+=d.x/slice.length;avgY+=d.y/slice.length});aggregatedSeriesData.push({x:avgX,y:avgY})}aggregatedData.push(aggregatedSeriesData)}.bind(this));return aggregatedData}});Rickshaw.namespace("Rickshaw.Graph.Socketio");Rickshaw.Graph.Socketio=Rickshaw.Class.create(Rickshaw.Graph.Ajax,{request:function(){var socket=io.connect(this.dataURL);var self=this;socket.on("rickshaw",function(data){self.success(data)})}});Rickshaw.namespace("Rickshaw.Series");Rickshaw.Series=Rickshaw.Class.create(Array,{initialize:function(data,palette,options){options=options||{};this.palette=new Rickshaw.Color.Palette(palette);this.timeBase=typeof options.timeBase==="undefined"?Math.floor((new Date).getTime()/1e3):options.timeBase;var timeInterval=typeof options.timeInterval=="undefined"?1e3:options.timeInterval;this.setTimeInterval(timeInterval);if(data&&typeof data=="object"&&Array.isArray(data)){data.forEach(function(item){this.addItem(item)},this)}},addItem:function(item){if(typeof item.name==="undefined"){throw"addItem() needs a name"}item.color=item.color||this.palette.color(item.name);item.data=item.data||[];if(item.data.length===0&&this.length&&this.getIndex()>0){this[0].data.forEach(function(plot){item.data.push({x:plot.x,y:0})})}else if(item.data.length===0){item.data.push({x:this.timeBase-(this.timeInterval||0),y:0})}this.push(item);if(this.legend){this.legend.addLine(this.itemByName(item.name))}},addData:function(data,x){var index=this.getIndex();Rickshaw.keys(data).forEach(function(name){if(!this.itemByName(name)){this.addItem({name:name})}},this);this.forEach(function(item){item.data.push({x:x||(index*this.timeInterval||1)+this.timeBase,y:data[item.name]||0})},this)},getIndex:function(){return this[0]&&this[0].data&&this[0].data.length?this[0].data.length:0},itemByName:function(name){for(var i=0;i<this.length;i++){if(this[i].name==name)return this[i]}},setTimeInterval:function(iv){this.timeInterval=iv/1e3},setTimeBase:function(t){this.timeBase=t},dump:function(){var data={timeBase:this.timeBase,timeInterval:this.timeInterval,items:[]};this.forEach(function(item){var newItem={color:item.color,name:item.name,data:[]};item.data.forEach(function(plot){newItem.data.push({x:plot.x,y:plot.y})});data.items.push(newItem)});return data},load:function(data){if(data.timeInterval){this.timeInterval=data.timeInterval}if(data.timeBase){this.timeBase=data.timeBase}if(data.items){data.items.forEach(function(item){this.push(item);if(this.legend){this.legend.addLine(this.itemByName(item.name))}},this)}}});Rickshaw.Series.zeroFill=function(series){Rickshaw.Series.fill(series,0)};Rickshaw.Series.fill=function(series,fill){var x;var i=0;var data=series.map(function(s){return s.data});while(i<Math.max.apply(null,data.map(function(d){return d.length}))){x=Math.min.apply(null,data.filter(function(d){return d[i]}).map(function(d){return d[i].x}));data.forEach(function(d){if(!d[i]||d[i].x!=x){d.splice(i,0,{x:x,y:fill})}});i++}};Rickshaw.namespace("Rickshaw.Series.FixedDuration");Rickshaw.Series.FixedDuration=Rickshaw.Class.create(Rickshaw.Series,{initialize:function(data,palette,options){options=options||{};if(typeof options.timeInterval==="undefined"){throw new Error("FixedDuration series requires timeInterval")}if(typeof options.maxDataPoints==="undefined"){throw new Error("FixedDuration series requires maxDataPoints")}this.palette=new Rickshaw.Color.Palette(palette);this.timeBase=typeof options.timeBase==="undefined"?Math.floor((new Date).getTime()/1e3):options.timeBase;this.setTimeInterval(options.timeInterval);if(this[0]&&this[0].data&&this[0].data.length){this.currentSize=this[0].data.length;this.currentIndex=this[0].data.length}else{this.currentSize=0;this.currentIndex=0}this.maxDataPoints=options.maxDataPoints;if(data&&typeof data=="object"&&Array.isArray(data)){data.forEach(function(item){this.addItem(item)},this);this.currentSize+=1;this.currentIndex+=1}this.timeBase-=(this.maxDataPoints-this.currentSize)*this.timeInterval;if(typeof this.maxDataPoints!=="undefined"&&this.currentSize<this.maxDataPoints){for(var i=this.maxDataPoints-this.currentSize-1;i>1;i--){this.currentSize+=1;this.currentIndex+=1;this.forEach(function(item){item.data.unshift({x:((i-1)*this.timeInterval||1)+this.timeBase,y:0,i:i})},this)}}},addData:function($super,data,x){$super(data,x);this.currentSize+=1;this.currentIndex+=1;if(this.maxDataPoints!==undefined){while(this.currentSize>this.maxDataPoints){this.dropData()}}},dropData:function(){this.forEach(function(item){item.data.splice(0,1)});this.currentSize-=1},getIndex:function(){return this.currentIndex}});return Rickshaw});

// jQuery Scin Plugin
//
// Version 1.03
//
// Author Korner
// Sl SYSTEM
// 06 June 2012
//
// Visit http://sl-cms.com for more information
//
// Usage: $.scin('method',options)
//
// TERMS OF USE
// 
// This plugin is dual-licensed under the GNU General Public License and the MIT License and
// is copyright 2012 Sl SYSTEM, LLC. 
 
(function($){
    
    var methods = {
        init : function() {
            return this;
        },
        /**
         * _INPUT
         */
        input: function(n){
            if(typeof n === 'string') n = {name:n};
            
            if(arguments.length > 1){
                n = {name:n.name,value:arguments[1]};
                if(typeof arguments[2] === 'object') n = $.extend(n,arguments[2]);
            }
            
            n = $.extend({
                name: 'none',
                value: '',
                holder: '',
                type: 'input',
                attr: {}
            }, n),data = '',o = [];
            
            n['attr']['class'] = n['attr']['class'] ? n['attr']['class']+' sl_input'+(n['invisible'] ? ' invisible' : '') : 'sl_input'+(n['invisible'] ? ' invisible' : '');
            $.each(n['attr'],function(j,v){
                o[o.length] = j+'="'+v+'"';   
            });
            data = '<div '+o.join(' ')+'><div><input type="'+(n.type == 'password' ? 'password' : 'input')+'" value="'+n.value+'" name="'+n.name+'" placeholder="'+n.holder+'" spellcheck="'+(n.check ? 'true' : 'false')+'"'+(n.regex ? ' onkeyup="this.value = this.value.replace(/'+n.regex+'/gi,\'\');"' : '')+' /></div>'+(n.bigedit ? '<div class="bigedit"></div>' : '')+'</div>';
            
            if(this == false) return data;
            else $(this).html(data);
        },
        /**
         * _AREA
         */
        textarea: function(n){
            if(typeof n === 'string') n = {name:n};
            
            if(arguments.length > 1){
                n = {name:n.name,value:arguments[1]};
                if(typeof arguments[2] === 'object') n = $.extend(n,arguments[2]);
            }
            
            n = $.extend({
                name: 'none',
                value: '',
                attr: {}
            }, n),data = '',o = [];
            
            n['attr']['class'] = n['attr']['class'] ? n['attr']['class']+' sl_textarea'+(n['invisible'] ? ' invisible' : '') : 'sl_textarea'+(n['invisible'] ? ' invisible' : '');
            n['attr']['style'] = n['attr']['style'] ? 'width:100%; height:inherit;'+n['attr']['style'] : 'width:100%; height:inherit;';
            $.each(n['attr'],function(j,v){
                o[o.length] = j+'="'+v+'"';   
            });
            
            data = '<div '+o.join(' ')+'><div><textarea name="'+n.name+'" spellcheck="'+(n.check ? 'true' : 'false')+'">'+n.value+'</textarea></div>'+(n.bigedit ? '<div class="bigedit"></div>' : '')+'</div>';
            
            if(this == false) return data;
            else $(this).html(data);
        },
        /**
         * _BTN
         */
        btn: function(n){
            if(typeof n === 'string') n = {name:n};
            
            if(arguments.length > 1){
                if(typeof arguments[1] === 'object') n = $.extend(n,arguments[1]);
                else n['callback'] = arguments[1]
            }
            
            n = $.extend({
                name: 'none',
                callback: '',
                attr: {}
            }, n),data = '',o = [];
            
            n['attr']['class'] = n['attr']['class'] ? n['attr']['class']+' sl_btn' : 'sl_btn';
            n['attr']['onclick'] = n['callback'] ?  n['callback']+"('"+n.name+"');"+(n['attr']['onclick'] || '') : (n['attr']['onclick'] || '');
            $.each(n['attr'],function(j,v){
                o[o.length] = j+'="'+v+'"';   
            });
            data = '<div '+o.join(' ')+'>'+n.name+'</div>';
            
            if(this == false) return data;
            else $(this).html(data);
        },
        /**
         * _RADIO
         */
        radio: function(n){
            if(typeof n === 'string') n = {name:n};
            
            if(arguments.length > 1){
                n = {name:n.name,val:arguments[1],value:arguments[2]}
                if(typeof arguments[3] === 'object') n = $.extend(n,arguments[3]);
                else n['callback'] = arguments[3]
            }
            
            n = $.extend({
                name: 'none',
                value: 'on',
                val: ['on','off'],
                type: 'line',
                callback: '',
                attr: {}
            }, n),data = '',o = [],s = [],r = Math.floor(Math.random() * (999 - 100 + 1)) + 100;
            
            $.each(n['val'],function(c,v){
                s = n['value'] == v ? [' checked','cb-enable selected'] : ['','cb-disable'];
                data += '<input type="radio" name="'+n.name+'" value="'+v+'" id="radio_'+r+'_'+c+'"'+s[0]+' /><label'+(n['callback'] ? ' onclick="'+n['callback']+'(\''+v+'\','+c+')"' : '')+' for="radio_'+r+'" class="'+s[1]+'"><span>'+v+'</span></label>';
            })
        
            n['attr']['class'] = n['attr']['class'] ? n['attr']['class']+' sl_radio '+n['type'] : 'sl_radio '+n['type'];
            $.each(n['attr'],function(j,v){
                o[o.length] = j+'="'+v+'"';   
            });
            data = '<div '+o.join(' ')+'>'+data+'</div>';
            
            if(this == false) return data;
            else $(this).html(data);
        },
        /**
         * _CHECKBOX
         */
        checkbox: function(n){
            if(typeof n === 'string') n = {name:n};
            
            if(arguments.length > 1){
                n = {name:n.name,value:arguments[1]}
                if(typeof arguments[2] === 'object') n = $.extend(n,arguments[2]);
                else n['callback'] = arguments[2]
            }
            
            n = $.extend({
                name: 'none',
                value: '0',
                callback: '',
                attr: {}
            }, n),data = '',o = [];
        
            n['attr']['class'] = n['attr']['class'] ? n['attr']['class']+' sl_checkbox' : 'sl_checkbox';
            n['attr']['class'] += n['value'] > 0 ? ' active': '';
            n['attr']['onclick'] = n['callback'] ?  n['callback']+"('"+n.name+"',$(this).find('input').val());"+(n['attr']['onclick'] || '') : (n['attr']['onclick'] || '');
            $.each(n['attr'],function(j,v){
                o[o.length] = j+'="'+v+'"';   
            });
            data = '<div '+o.join(' ')+'><input type="hidden" name="'+n.name+'" value="'+n.value+'" /></div>';
            
            if(this == false) return data;
            else $(this).html(data);
        },
        /**
         * _SELECT
         */
        select: function(n){
            if(typeof n === 'string') n = {name:n};
            
            if(arguments.length > 1){
                n = {name:n.name,val:arguments[1],value:arguments[2]}
                if(typeof arguments[3] === 'object') n = $.extend(n,arguments[3]);
                else n['callback'] = arguments[3]
            }
            
            n = $.extend({
                name: 'none',
                value: 'on',
                val: ['on','off'],
                callback: '',
                attr: {}
            }, n),data = '',o = [],d = [],ih = false;
            
            function array_all( input ) {	
            	var a = [[],[]];
            
            	for ( key in input ){
            		a[0][a[0].length] = input[key];
                    a[1][a[1].length] = key;
            	}
            	return a;
            }
    
            d = array_all(n.val);
            
            if(typeof n.callback === 'object'){
                var callName = n.callback[0];
                n.callback.shift();
            }
            else var callName = n.callback[0];
            
            $.each(n.val,function(c,v){
                data += '<li'+(n.callback ? ' onclick="'+(typeof n.callback === 'object' ? callName : n.callback)+'(\''+c+'\''+(typeof n.callback === 'object' ? ','+n.callback.join(',') : '')+')"' : '')+' val="'+c+'" name="'+v+'"'+(c == n.value ? ' class="selected"' : '')+'><span>'+v+'</span></li>';
            })
        
            n['attr']['class']= n['attr']['class'] ? n['attr']['class']+' sl_select' : 'sl_select';
            $.each(n['attr'],function(j,v){
                o[o.length] = j+'="'+v+'"';   
            });
            
            data = '<div '+o.join(' ')+'><input type="hidden" name="'+n.name+'" value="'+(n.value ? n.value : d[1][0])+'" /><div class="_data"><ul>'+data+'</ul></div><div class="_display">'+(n['val'][n['value']] ? n['val'][n['value']] : d[0][0])+'</div></div>';

            if(this == false) return data;
            else $(this).html(data);
        },
        /**
         * _SLIDE
         */
        slide: function(n){
            if(typeof n === 'string') n = [n];
            
            var j = 0,i = t= '',cnt = 0;

            for (var b in n) b && cnt++;

            $.each(n,function(c,v){
                i += '<div class="page win_h_size scrollbarInit" style="left:'+(j*100)+'%"><div class="s_data">'+v+'</div></div>';
                t += '<li style="width:'+(100 / cnt)+'%" rel="'+j+'"'+(j == 0 ? ' class="active"' : '')+'>'+c+'</li>';
                j++;
            })
            
            return '<div class="sl_slide win_h_size">'+i+'<ul class="title">'+t+'</ul></div>';
        }
    
    };
    
    $.fn.scin = function( method ) {
    
        if ( methods[method] ) {
          return methods[ method ].apply( this, Array.prototype.slice.call( arguments, 1 ));
        } else if ( typeof method === 'object' || ! method ) {
          return methods.init.apply( this, arguments );
        } else {
          return methods.init.apply( this );
        }   
    
    };
    
    $.scin = function( method ) {
    
        if ( methods[method] ) {
          return methods[ method ].apply( false,Array.prototype.slice.call( arguments, 1 ));
        } else if ( typeof method === 'object' || ! method ) {
          return methods.init.apply( false, arguments );
        } else {
          return methods.init();
        }   
    
    };
  

})(jQuery);
/*!
 * Tiny Scrollbar 1.66
 * http://www.baijs.nl/tinyscrollbar/
 *
 * Copyright 2010, Maarten Baijs
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.opensource.org/licenses/gpl-2.0.php
 *
 * Date: 13 / 11 / 2011
 * Depends on library: jQuery
 * 
 * Мodified Korner
 */

(function($){
	$.tiny = $.tiny || { };
	
    var winSize = [
        $(window).width(),
        $(window).height()
    ];
    
	$.tiny.scrollbar = {
		options: {	
			axis: 'y', // vertical or horizontal scrollbar? ( x || y ).
			wheel: 40,  //how many pixels must the mouswheel scroll at a time.
			scroll: true, //enable or disable the mousewheel;
			size: 'auto', //set the size of the scrollbar to auto or a fixed number.
			sizethumb: 'auto' //set the size of the thumb to auto or a fixed number.
		}
	};
	
	$.fn.tinyscrollbar = function(options) {
        init.apply(this,[options]); return this;
	};
	$.fn.tinyscrollbar_update = function(sScroll) {
	   init.apply(this,[{},sScroll]); return this;
    };
    $.fn.tinyscrollbar_remove = function() {
       this.each(function(){ $(this).removeClass('scrollbarContent').removeData()}); return this;
    };
	
    function init(options,sScroll){
        var options = $.extend({}, $.tiny.scrollbar.options, options),
            viewport = '.viewport:eq(0)',
            overview = '.overview:eq(0)';
        
        $(this).each(function(){
            var _this = $(this);
            
            !_this.hasClass('scrollbarContent') && _this.addClass('scrollbarContent').wrapInner('<div class="viewport"><div class="overview">').append('<div class="scrollbar"><div class="track"><div class="thumb"><div class="end"></div></div></div></div>');
            
            if(_this.is(':visible')){
                $(viewport,_this).height(_this.outerHeight(true));
                $(overview+','+viewport,_this).width(_this.outerWidth(true));
            } 
            
            if(_this.data('tsb')){
                var isHide = _this.is(':hidden') ? 1 : 0;
                
                if(isHide){
                    sScroll = 'relative';
                    $(viewport,_this).height(_this.outerHeight(true)).unbind('mouseenter').bind('mouseenter',function(){
                        _this.data('tsb').update(sScroll);
                        $(this).unbind('mouseenter');
                    });
                }
                _this.data('tsb').update(sScroll,isHide);
            } 
            else _this.data('tsb', new Scrollbar(_this, options));
        });
    }
    
        
	function Scrollbar(root, options){
		var oSelf = this;
		var oWrapper = root;
		var NameVar,oViewport,oContent,oScrollbar,oTrack,oThumb;
		var sAxis = options.axis == 'x', sDirection = sAxis ? 'left' : 'top', sSize = sAxis ? 'Width' : 'Height';
		var iScroll, iPosition = { start: 0, now: 0 }, iMouse = {};

		function initialize() {
			oSelf.update();
			setEvents();
			return oSelf;
		}
        function initVar(){
            oViewport = { obj: $('.viewport', root) };
    		oContent = { obj: $('.overview', root) };
    		oScrollbar = { obj: $('.scrollbar', root) };
    		oTrack = { obj: $('.track', oScrollbar.obj) };
    		oThumb = { obj: $('.thumb', oScrollbar.obj) };
        }
        
		this.update = function(sScroll,isHide){
            initVar();
            
            oViewport[options.axis] = sAxis ? $('.viewport', root).width() : $('.viewport', root).height();
            
            oContent[options.axis] = isHide ? $('.overview', root).data('h')[options.axis] : (sAxis ? $('.overview', root).width() : $('.overview', root).height());
            
            !isHide && $('.overview', root).data('h',{x:$('.overview', root).outerWidth(true),y:$('.overview', root).outerHeight(true)});
            
			oContent.ratio = oViewport[options.axis] / oContent[options.axis];
            
			oScrollbar.obj.toggleClass('disable', oContent.ratio >= 1);
			oTrack[options.axis] = options.size == 'auto' ? oViewport[options.axis] : options.size;
			oThumb[options.axis] = Math.min(oTrack[options.axis], Math.max(0, ( options.sizethumb == 'auto' ? (oTrack[options.axis] * oContent.ratio) : options.sizethumb )));
			oScrollbar.ratio = options.sizethumb == 'auto' ? (oContent[options.axis] / oTrack[options.axis]) : (oContent[options.axis] - oViewport[options.axis]) / (oTrack[options.axis] - oThumb[options.axis]);
            
			iScroll = (sScroll == 'relative' && oContent.ratio <= 1) ? Math.min((oContent[options.axis] - oViewport[options.axis]), Math.max(0, iScroll)) : 0;
			iScroll = (sScroll == 'bottom' && oContent.ratio <= 1) ? (oContent[options.axis] - oViewport[options.axis]) : isNaN(parseInt(sScroll)) ? iScroll : parseInt(sScroll);
			setSize();
		};
		function setSize(){
			oThumb.obj.css(sDirection, iScroll / oScrollbar.ratio);
			oContent.obj.css(sDirection, -iScroll);
			iMouse['start'] = oThumb.obj.offset()[sDirection];
			var sCssSize = sSize.toLowerCase(); 
			oScrollbar.obj.css(sCssSize, oTrack[options.axis]);
			oTrack.obj.css(sCssSize, oTrack[options.axis]);
			oThumb.obj.css(sCssSize, oThumb[options.axis]);		
		};		
		function setEvents(){
			oThumb.obj.bind('mousedown', start);
            
			oContent.obj[0].ontouchstart = function(oEvent){
				//oEvent.preventDefault();
				oThumb.obj.unbind('mousedown');
				start(oEvent.touches[0]);
				
			};
            
            
			oTrack.obj.bind('mouseup', drag);
			if(options.scroll && this.addEventListener){
				oWrapper[0].addEventListener('DOMMouseScroll', wheel, false);
				oWrapper[0].addEventListener('mousewheel', wheel, false );
			}
			else if(options.scroll){oWrapper[0].onmousewheel = wheel;}
		};
		function start(oEvent){
			iMouse.start = sAxis ? oEvent.pageX : oEvent.pageY;
			var oThumbDir = parseInt(oThumb.obj.css(sDirection));
			iPosition.start = oThumbDir == 'auto' ? 0 : oThumbDir;
			$(document).bind('mousemove', drag);
            
			oViewport.obj[0].ontouchmove = function(oEvent){
				$(document).unbind('mousemove');
				drag(oEvent.touches[0],true);
			};
            
			$(document).bind('mouseup', end);
			oThumb.obj.bind('mouseup', end);
			oContent.obj[0].ontouchend = document.ontouchend = function(oEvent){
				$(document).unbind('mouseup');
				oThumb.obj.unbind('mouseup');
				end(oEvent.touches[0]);
			};
			return false;
		};		
		function wheel(oEvent){
			if(!(oContent.ratio >= 1)){
				var oEvent = oEvent || window.event;
				var iDelta = oEvent.wheelDelta ? oEvent.wheelDelta/120 : -oEvent.detail/3;
				iScroll -= iDelta * options.wheel;
				iScroll = Math.min((oContent[options.axis] - oViewport[options.axis]), Math.max(0, iScroll));
				oThumb.obj.css(sDirection, iScroll / oScrollbar.ratio);
				oContent.obj.css(sDirection, -iScroll);
				
				oEvent = $.event.fix(oEvent);
				oEvent.preventDefault();
			};
		};
		function end(oEvent){
			$(document).unbind('mousemove', drag);
			$(document).unbind('mouseup', end);
			oThumb.obj.unbind('mouseup', end);
			document.ontouchmove = oThumb.obj[0].ontouchend = document.ontouchend = null;
			return false;
		};
		function drag(oEvent,touch){
			if(!(oContent.ratio >= 1)){
                iPosition.now = Math.min((oTrack[options.axis] - oThumb[options.axis]), Math.max(0, (touch ? (iPosition.start - (((sAxis ? oEvent.pageX : oEvent.pageY) - iMouse.start) / oScrollbar.ratio) ) : (iPosition.start + ((sAxis ? oEvent.pageX : oEvent.pageY) - iMouse.start)) ) ));

				iScroll = iPosition.now * oScrollbar.ratio;
				oContent.obj.css(sDirection, -iScroll);
				oThumb.obj.css(sDirection, iPosition.now);
			}
			return false;
		};
		
		return initialize();
	};
})(jQuery);
"use strict";

var dev_loaders_page_loader = {    
    open: function(){
        /* close opened loaders */
        this.close();
        /* ./end */
        
        /* new loader */
        var loading_layer = $("<div></div>").addClass("dev-page-loading");
        /* ./end */
        
        /* append loader */
        $("body").append(loading_layer).find(".dev-page-loading").animate({opacity: 1},200,"linear");
        /* ./end */
    },
    close: function(){
        /* close opened loader */
        if($(".dev-page-loading").length > 0){            
            $(".dev-page-loading").animate({opacity: 0},300,"linear",function(){
                $(this).remove();
            });                        
        }        
        /* ./close opened loader */
    }
}

var dev_loaders_circle = {        
    show: function(element) {        
    /* loader circle */
        /* build loader html */
        var loader = $("<div></div>").addClass("dev-loader-circle dev-loader-circle-active").html("<div></div><div></div><div></div><div></div>");        
        /* ./end */        
        /* show loader */
        element.append(loader);
        /* ./end */
    /* ./loader circle */
    },
    hide: function(element){
    /* hide loader */
        element.find(".dev-loader-circle").remove();
    /* ./hide loader */
    }
};

var dev_loaders_default = {
    show: function(element) {
    /* loader circle */    
        /* build loader html */
        var loader = $("<div></div>").addClass("dev-loader-default").html("<div></div>");
        /* ./end */
        /* show loader */
        element.append(loader);
        /* ./end */        
    /* ./loader circle */
    },
    hide: function(element){
    /* hide loader */
        element.find(".dev-loader-default").remove();
    /* ./hide loader */    
    }
};

$(function(){
    
    setTimeout(function(){
        dev_loaders_page_loader.close();
    },1000);    
});
"use strict";

var dev_layout_alpha_settings = {
    unit: 5,
    headerHeight: $(".dev-page .dev-page-header").height(),
    footerHeight: 40,
    footerHeightCurrent: 0,
    containerPadding: 30,
    screenSmall: 992,
    navAnimateSpeed: 300,
    sbAnimateSpeed: 100,
    footerContainerDisable: 0,
    contentHeight: 0,
    rightbarChatTemplate: "<li class=\"sent\">{message} <span>{date}</span></li>",
    rightbarHeight: 0
};

var dev_layout_alpha_content = {
    init: function(settings){
    /* content height control */
        
        /* add state loaded to dev-page */
        $(".dev-page").addClass("dev-page-loaded");
        /* ./end */
        
        /* check avail of footer */
        if($(".dev-page .dev-page-footer").length === 0){
            $(".dev-page").addClass("dev-page-no-footer");
            
        }
        if($(".dev-page").hasClass("dev-page-no-footer"))
            dev_layout_alpha_settings.footerHeightCurrent = 0;            
        else
            dev_layout_alpha_settings.footerHeightCurrent = settings.footerHeight;
        /* ./end */
        
        /* remove height param from containers */
        $(".dev-page-container .dev-page-content, .dev-page-sidebar, .dev-page-rightbar, .rightbar-chat, .rightbar-chat .rightbar-chat-frame-contacts, .rightbar-chat .rightbar-chat-frame-chat").css("height","");
        /* ./end */
        
        /* do not set height for containers in mobile mode */
		
        if(window.innerWidth < settings.screenSmall){           
            /* remove active class from navigation */            
            $(".dev-page-navigation li").removeClass("active");
            $(".dev-page-navigation ul").css("max-height","");
            /* ./end */                        
            
            //$(".dev-page-sidebar-collapse").addClass("active");
            //$(".dev-page").addClass("dev-page-sidebar-collapsed").removeClass("dev-page-sidebar-minimized");
            
            return false;
        }
		
        /* ./end */
		
		settings.headerHeight = $(".dev-page .dev-page-header").height();
        
        /* get height of other elements to get minus value */                
        var minus = settings.headerHeight + dev_layout_alpha_settings.footerHeightCurrent;
        /* ./end */

        /* get height of main container (sidebar and content) */
        var content_height = $(".dev-page-container .dev-page-content > .container").height() + settings.containerPadding;            
        var sidebar_height = $(".dev-page-container .dev-page-sidebar").height();                

        var height = content_height > sidebar_height ? content_height : sidebar_height;
            height = height + minus;
        /* ./end */

        /* compare it with window height and get new height value */
        var new_height  = window.innerHeight > height ? window.innerHeight - minus : height - minus - 30; // i dont know why i -30px... probably its padding... :(                
        /* ./end */
		
        /* set height for inner boxes */
        $(".dev-page-container .dev-page-sidebar,.dev-page-container .dev-page-content").height(new_height);
        /* ./end */

        
        return new_height;
        
    /* ./content height control */
    },
    sidebar_minimize: function(settings) {
    /* minimize sidebar control */
        
        /* add event handler on .dev-page-sidebar-minimize */
        $(".dev-page-sidebar-minimize").on("click",function(){
            
            /* change icon */
            var icon = $(this).find(".fa");
            if(icon.hasClass("fa-outdent"))
                icon.removeClass("fa-outdent").addClass("fa-indent");
            else
                icon.removeClass("fa-indent").addClass("fa-outdent");
            /* ./end */
            
            /* remove active class from navigation */            
            $(".dev-page-navigation li").removeClass("active");
            $(".dev-page-navigation ul").css("max-height","");
            /* ./end */
            
            /* remove collappsed state if avail */
            $(".dev-page").removeClass("dev-page-sidebar-collapsed");
            $(".dev-page-sidebar-collapse").removeClass("active");
            /* ./end */
                        
            
            /* toggle dev-page-sidebar-minimized class on dev-page */
            if($(".dev-page").hasClass("dev-page-sidebar-minimized")){                
                $(".dev-page").removeClass("dev-page-sidebar-minimized");
                $(".dev-page-sidebar").mCustomScrollbar("update");
            }else{
                $(".dev-page").addClass("dev-page-sidebar-minimized");
                $(".dev-page-sidebar").mCustomScrollbar("disable",true);                
            }            
            /* ./end */
            
            /* add animation class to navigation on minimize */
            $(".dev-page-navigation").removeClass("dev-page-navigation-effect").addClass("dev-page-navigation-effect");
            setTimeout(function(){
                $(".dev-page-navigation").removeClass("dev-page-navigation-effect");
            },settings.navAnimateSpeed);
            /* ./end */
            
            return false;            
        });
        /* ./end */
        
    /* ./minimize sidebar control */    
    },
    sidebar_collapse: function(settings) {
    /* collapse sidebar control */
        
        var self = this;
        
        /* add event handler on .dev-page-sidebar-collapse */
        $(".dev-page-sidebar-collapse").on("click",function(){
            
            /* close search container */
            self.search_container.close();
            self.rightbar.close();
            /* ./end */
            
            /* close opened elements in minimized mode */          
            if($(".dev-page").hasClass("dev-page-sidebar-minimized")){
                $(".dev-page-navigation li").removeClass("active");
                $(".dev-page-navigation ul").css("max-height","");
            }
            /* ./end */
            
            /* toggle dev-page-sidebar-collapsed class on dev-page */
            if($(".dev-page").hasClass("dev-page-sidebar-collapsed")){
                $(".dev-page").removeClass("dev-page-sidebar-collapsed");
                $(this).removeClass("active");
            }else{
                $(".dev-page").addClass("dev-page-sidebar-collapsed");
                $(this).addClass("active");
            }
            
            /* ./end */
            
            /* add animation class to navigation on collapse */
            $(".dev-page-navigation").removeClass("dev-page-navigation-effect").addClass("dev-page-navigation-effect");
            setTimeout(function(){
                $(".dev-page-navigation").removeClass("dev-page-navigation-effect");
                $(window).trigger('resize');
            },settings.navAnimateSpeed);
            /* ./end */            
            
            return false;
        });
        /* ./end */
    
    /* ./collapse sidebar control */
    },
    rightbar: {
    /* collappse rightbar control */        
        init: function(settings){    
            
            var self = this;
            
        /* add event handler on .dev-page-rightbar-collapse */                        
            $(".dev-page-rightbar-toggle-no").on("click",function(){

                /* close other */        
                dev_layout_alpha_content.search_container.close();
                //dev_layout_alpha_content.footer_container.closeAll();
                /* ./end */
                
                self.calcHeight();
            
                $(".dev-page").toggleClass("dev-page-rightbar-open");
                
                return false;
            });
        /* ./end */            
        /* add close rightbar handler */
            $(".rightbar-close").on("click",function(){
                self.close();
            });
        /* ./end */        
        },
        calcHeight: function(){
            var rightbar_height = window.innerHeight;
            /* set height */
            $(".dev-page-rightbar, .rightbar-chat, .rightbar-chat .rightbar-chat-frame-contacts, .rightbar-chat .rightbar-chat-frame-chat").height(rightbar_height);
            /* ./end */
        },
        close: function(){
        /* close rightbar function */    
            $(".dev-page").removeClass("dev-page-rightbar-open");
        /* ./end */
        }                
    /* ./collappse rightbar control */        
    },
    footer: function(){
    /* footer collapse control */
        var self = this;

        /* fix footer control */
        $(".dev-page-footer-fix").on("click",function(){
            $(".dev-page-footer").toggleClass("dev-page-footer-fixed");
        });
        /* ./end */

        $(".dev-page-footer-collapse").on("click",function(){            
            /* close other elements */
            self.rightbar.close();
            /* ./end */
            
            /* remove class dev-page-footer-closed */
            $(".dev-page-footer").removeClass("dev-page-footer-closed");
            /* ./end */
            
            /* change collapse footer icon */
            var icon = $(this).find(".fa");
            
            if(icon.hasClass("fa-dot-circle-o")){
                icon.removeClass("fa-dot-circle-o").addClass("fa-circle-o");
            }else{
                icon.removeClass("fa-circle-o").addClass("fa-dot-circle-o");
            }
            /* ./end */
            
            /* close all opened container */
            self.footer_container.closeAll();
            /* ./end */            
                        
            if($(".dev-page-footer").hasClass("dev-page-footer-collapsed")){
                $(".dev-page-footer").removeClass("dev-page-footer-effect").removeClass("dev-page-footer-collapsed").addClass("dev-page-footer-effect");
                $(".dev-page").removeClass("dev-page-no-footer");                
            }else{
                $(".dev-page-footer").removeClass("dev-page-footer-effect").addClass("dev-page-footer-collapsed").addClass("dev-page-footer-effect");
                $(".dev-page").addClass("dev-page-no-footer");
            }
            /* update sidebar scroll */
            //$(".dev-page-sidebar").mCustomScrollbar("update");
            /* ./update sidebar scroll */
            
            return false;
        });
    /* ./footer collapse control */
    },
    footer_buttons: function(settings) {
    /* footer buttons width control */
    
        /* check footer settings */        
        if($(".dev-page-footer").hasClass("dev-page-footer-closed")){
            $(".dev-page-footer-collapse").click();            
        }
        /* ./end */
    
        var buttonsWidth = $(".dev-page-footer-buttons li").length * (settings.footerHeight + settings.unit) + settings.unit;
        $(".dev-page-footer-buttons").css({width: buttonsWidth, "margin-left": -buttonsWidth/2}).addClass("dev-page-footer-buttons-effect");            
    /* ./footer buttons width control */
    },
    footer_container_open: function(){
        var self = this;
        
        /* add event listener to dev-page-footer-buttons */
        $(".dev-page-footer-container-open").on("click",function(e){
            
            if(dev_layout_alpha_settings.footerContainerDisable) return false;
            
            if($(this).hasClass("active")){
                /* on twice click on button - close all */                
                self.footer_container.closeAll();
                /* ./end */
            }else{
                /* remove active buttons */
                $(".dev-page-footer-buttons a").removeClass("active");
                /* ./end */
                
                /* open contant by id */
                if(self.footer_container.open($(this).attr("href"))){
                    $(this).addClass("active");
                }
                /* ./end */
            }

            return false;
        });
        /* ./end */        
        
    },
    footer_container: {
    /* footer container controls */            
        init: function(){
        /* add close event listener on class .dev-layout-container-close */            
            var self = this;            
            $(".dev-page-footer-container-layer-button").on("click",function(){
                self.closeAll();
                return false;
            });            
        /* ./add close event listener on class .dev-layout-container-close */
        },
        open: function(contentID){
        /* open content in container by id */
            
            if(dev_layout_alpha_settings.footerContainerDisable) return false;
            
            /* close other */
            dev_layout_alpha_content.rightbar.close();
            /* ./close other */
            
            if($(contentID).length > 0){
               dev_layout_alpha_settings.footerContainerDisable = 1;
               
                $(".dev-page-footer-container .dev-page-footer-container-content").css("display","");
                $(".dev-page-footer-container").addClass("dev-page-footer-container-opened");
                
                dev_loaders_default.show($(".dev-page-footer-container"));
                                
                /* TEMP FOR DEMO */
                setTimeout(function(){                    
                    $(contentID).fadeIn("slow");                                        
                    dev_loaders_default.hide($(".dev-page-footer-container"));
                    dev_layout_alpha_settings.footerContainerDisable = 0;
                    
                    $(contentID).width(window.innerWidth);
                    $(contentID).mCustomScrollbar({axis:"x", autoHideScrollbar: true, scrollInertia: 200, advanced: {autoScrollOnFocus: false,autoExpandHorizontalScroll:true}});
                    
                },1000);                
                /* ./TEMP FOR DEMO */
                
                return true;
            }else
                return false;
            
        /* ./open content in container by id */    
        },
        closeAll: function(){
        /* close all contents */
            $(".dev-page-footer-buttons a").removeClass("active");
            $(".dev-page-footer-container .dev-page-footer-container-content").css("display","");
            $(".dev-page-footer-container").removeClass("dev-page-footer-container-opened");
            $(".dev-page-footer-container").each(function(){
                $(this).mCustomScrollbar('destroy');
            });
        /* ./close all contents */
        }            
    /* ./footer container controls */
    },
    search_container: {
        init: function(){
        
        var self = this;
        
        /* toggle search container */
            $(".dev-page-search-toggle").on("click",function(){      
                
                if($(".dev-page").hasClass("dev-page-search-active")){
                    self.close();
                }else{
                    self.open();
                }
                
                return false;
            });                        
        /* ./end toggle search container */
        },
        open: function(){
            /* close other elements */
                $(".dev-page").removeClass("dev-page-rightbar-open");
            /* ./end */
                
            /* temp search results */
            //$.get( "assets/search_result.html", function(data) {
                //$(".dev-search .dev-search-results").html(data);
            //});
            /* ./temp search results */
                
            /* open search container */
                $(".dev-page").addClass("dev-page-search-active");
                //$(".dev-search .dev-search-field input").focus().val($(".dev-search .dev-search-field input").val());
            /* ./open search container */
        },
        close: function(){
        /* close search container */
            serverLogsOpen = 0;
            
            $(".dev-page").removeClass("dev-page-search-active");
        /* ./close search container */
        }
    },
    rightbar_chat: {        
        init: function(settings){
            var self = this;

            /* temp, using to show chat after click on contacts item */
            $(".rightbar-chat .contacts a").on("click",function(){
                self.open();
                return false;
            });
            /* ./end */        
            
            /* add close chat event listener */
            $(".rightbar-chat-close").on("click",function(){
                self.close();
                return false;
            });
            /* ./end */
            
            /* init chat */
            self.messages.init(settings);
            /* ./end */            
        },
        open: function(){
        /* open chat window */
            $(".rightbar-chat").addClass("rightbar-chat-opened");
        /* ./open chat window */    
        /* set focus to message field */
            setTimeout(function(){
                /* set height */
                $(".rightbar-chat .chat-wrapper").height($(".rightbar-chat-frame-chat").height() - 245);
                /* ./end */
                
                //$("#rightbar_chat_form").find("input[name=message]").focus();
            },200);            
        /* ./end */
        },        
        close: function(){
        /* close chat window */
            $(".rightbar-chat").removeClass("rightbar-chat-opened");
        /* ./close chat window */
        },
        messages: {
            init: function(settings){                
                var self = this;                
                
                /* add submit form listener */
                $("#rightbar_chat_form").on("submit",function(){                    
                    var message = $(this).find("input[name=message]");                    
                    
                    if(message.val().length > 0)
                        self.post(settings,message.val());
                    
                    /* clear field */
                    message.val("");
                    
                    return false;
                });
                /* ./end */
            },
            post: function(settings,message){       
                /* build message using template from settings */                
                var date = "1 min ago"; /* temp */
                
                var message = settings.rightbarChatTemplate.replace("{message}", message);
                    message = message.replace("{date}", date);
                /* ./end */
                
                /* append message to chat */
                $("#rightbar_chat").append(message);
                /* ./end */
            }
        }
    }
};

$(function(){
//$(document).ready(function(){
    
    setTimeout(function(){
        dev_layout_alpha_settings.contentHeight = dev_layout_alpha_content.init(dev_layout_alpha_settings);
    },500);
    
    dev_layout_alpha_content.sidebar_minimize(dev_layout_alpha_settings);
    dev_layout_alpha_content.sidebar_collapse(dev_layout_alpha_settings);
    
    dev_layout_alpha_content.rightbar.init(dev_layout_alpha_settings);
    
    dev_layout_alpha_content.footer();
    dev_layout_alpha_content.footer_buttons(dev_layout_alpha_settings);
    dev_layout_alpha_content.footer_container_open();
    dev_layout_alpha_content.footer_container.init();
    dev_layout_alpha_content.search_container.init();
    dev_layout_alpha_content.rightbar_chat.init(dev_layout_alpha_settings);
    
    
    $(".dev-page-rightbar").on("click",function(e){
        e.stopPropagation();
    });
        
    $("body").on("click",function(){
        if(!Modernizr.touch){
            dev_layout_alpha_content.rightbar.close();
        }
    });
	
	if(window.innerWidth < dev_layout_alpha_settings.screenSmall){           
		/* remove active class from navigation */            
		$(".dev-page-navigation li").removeClass("active");
		$(".dev-page-navigation ul").css("max-height","");
		/* ./end */                        
		
		$(".dev-page-sidebar-collapse").addClass("active");
		$(".dev-page").addClass("dev-page-sidebar-collapsed").removeClass("dev-page-sidebar-minimized");
		
		return false;
	}
		
    
});

$(window).resize(function(e){
    
    if(Modernizr.touch) {
        e.preventDefault();
        
        /* recalculate righbar height */
        dev_layout_alpha_content.rightbar.calcHeight();
        /* ./end */        
    }else{ 
        /* close rightbar on window resize */
        dev_layout_alpha_content.rightbar.close();
        /* ./end */

        /* close rightbar chat on window resize */
        dev_layout_alpha_content.rightbar_chat.close();
        /* ./end */                
        
        setTimeout(function(){
            dev_layout_alpha_settings.contentHeight = dev_layout_alpha_content.init(dev_layout_alpha_settings);
        },100);    
    }
    
    /* close all footer containers */
    dev_layout_alpha_content.footer_container.closeAll();
    /* ./close all footer containers */
});
"use strict";

var demo_form_validation = {
    init: function() {
        
        if($("#validate").length > 0){
            var validate = $("#validate").validate({                
                    errorClass:"has-error",
                    validClass:"has-success",
                    errorElement:"span",
                    ignore: [],
                    errorPlacement: function(error,element){
                        $(element).after(error);                    
                        $(element).parents(".form-group").addClass("has-error");
                    },
                    highlight: function(element, errorClass){
                        $(element).parents(".form-group").removeClass("has-success").addClass(errorClass);
                        dev_layout_alpha_content.init(dev_layout_alpha_settings);
                    },
                    unhighlight: function(element, errorClass, validClass){                    
                        $(element).parents(".form-group").removeClass(errorClass).addClass(validClass);
                        dev_layout_alpha_content.init(dev_layout_alpha_settings);
                    },
                    rules: {                                            
                            login:          {required: true, minlength: 2, maxlength: 8},
                            password:       {required: true, minlength: 5, maxlength: 10},
                            're-password':  {required: true, minlength: 5, maxlength: 10, equalTo: "#password2"},
                            age:            {required: true, min: 18, max: 100},
                            email:          {required: true, email: true},
                            date:           {required: true, date: true},
                            credit:         {required: true, creditcard: true},
                            site:           {required: true, url: true}
                    }                                        
            });      

            $(".hide-prompts").on("click",function(){
                validate.resetForm();
            });
        }
        
        if($("#nestable").length > 0){
            $("#nestable").nestable({group: 1});
            $("#nestable2").nestable({group: 1});
            
            $("#nestable3").nestable();
            
            $('.dd').on('change', function() {
                dev_layout_alpha_content.init(dev_layout_alpha_settings);
            });
            $(".dd-item > button").on("click",function(){
                setTimeout(function(){
                    dev_layout_alpha_content.init(dev_layout_alpha_settings);
                },200);                
            });
        }
        
        // jsTree 
        if($("#jstree_default").length > 0){
            

            // Default
            $("#jstree_default").jstree();

            // Checkbox Plugin
            $("#jstree_checkbox").jstree({
                "checkbox" : {
                    "keep_selected_style" : false
                },
                "plugins" : [ "checkbox" ]
            });

            // Drag & drop plugin    
            $("#jstree_dnd").jstree({
                "core" : {"check_callback" : true},
                "plugins" : [ "dnd" ]
            });    
            
            $(".jstree-icon.jstree-ocl").on("click",function(){
                setTimeout(function(){
                    dev_layout_alpha_content.init(dev_layout_alpha_settings);
                },200);                
            });
            
        }
        //jstree_end
        
    }
}

var demo_tocify = {
    update: function(){
        if($("#tocify").length > 0){
            setTimeout(function(){
                $("#tocify").width($("#tocify").parent("div").width()-20);
            },500);
        }
    },
    init: function(){        
        if($("#tocify").length > 0){            
            $("#tocify").tocify({context: ".tocify-content", showEffect: "fadeIn",extendPage:false,selectors: "h3, h4, h5" });
            this.update();
        }
    }
};


$(function(){    
    demo_form_validation.init();    
    demo_tocify.init();    
});

$(window).resize(function(){
    demo_tocify.update();
});
"use strict";

var dev_app_settings = {
    dev_page_loader: false// use false to disablle page loader    
};

var dev_custom = {
    init: function(){
        // New selector case insensivity        
        $.expr[':'].containsi = function(a, i, m) {
            return jQuery(a).text().toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
        };
    }
}

var dev_header = {
    init: function(){
    /* header actions */    
    
        /* add click outside event */
        $("body").on("click",function(){
            $(".dph-buttons > li").removeClass("active");
        });
        /* ./end */
    
        /* add listener to header buttons */
        $(".dph-buttons > li").on("click",function(e){
            e.stopPropagation();
            
            if($(this).find(".dev-popup").length > 0){
                $(this).toggleClass("active"); 
                return false;
            }            
        });
        /* ./end */
        
        /* popup click stoppropagation */
        $(".dev-page-header .dev-popup").on("click",function(e){
            e.stopPropagation();
        });
        /* ./end */
        
                
    /* ./header actions */    
    }
};

var dev_sidebar_navigation = {
    init: function() {
    /* navigation functions */
        
        if($(".dev-page-sidebar").length === 0) return false;
        
        /* navigation element height */
        var elmHeight = 55;
        /* ./end */
        
        /* add click outside event */
        $("body").on("click",function(){
            $(".dev-page-sidebar-minimized .dev-page-navigation li").removeClass("active");
            $(".dev-page-sidebar-minimized .dev-page-navigation ul").css("max-height","");
        });
        /* ./end  */
        
        /* check default settings */
        if($(".dev-page").hasClass("dev-page-sidebar-collapsed"))
            $(".dev-page-sidebar-collapse").addClass("active");                
        /* ./end */
        
        /* add class .has-child to all elements that have childs */
        $(".dev-page-navigation li").each(function(){
            if($(this).find(" > ul").length > 0){
                $(this).addClass("has-child");
            }
            
            if($(this).hasClass("active")){
                var ul = $(this).children("ul");
                
                var maxHeight = ul.find("li").length * elmHeight;                
                ul.css("max-height",maxHeight);
            }
        });
        /* ./end */
        
        /* add event listener to navigation link */
        $(".dev-page-navigation li > a").on("click",function(e){            
            e.stopPropagation();
                       
            var li = $(this).parent("li");
            var ul = li.children("ul");
            
            if(ul.length > 0){                
               /* close other opened elements in minimized mode */
                if($(".dev-page").hasClass("dev-page-sidebar-minimized")){
                    var other = li.parent("ul").find("> li").not(li);

                    other.each(function(){
                        $(this).removeClass("active");
                        $(this).find("li").removeClass("active");
                        $(this).find("ul").css("max-height",0);
                    });
                }
                /* ./end */
                
                /* toggle active class */
                if(li.hasClass("active")){
                    li.removeClass("active");
                    ul.css("max-height",0);
                }else{
                    var maxHeight = ul.find("li").length * elmHeight;
                    li.addClass("active");
                    ul.css("max-height",maxHeight);
                    
                    li.parent("ul").find("li").not(li).each(function(){
                        $(this).removeClass("active");
                        $(this).children("ul").css("max-height",0);
                    });
                }
                /* ./end */                                
                
                /* call window resize function to fix navigation height*/
                if(window.innerWidth > 992) $(window).resize();
                /* ./end */
                
                return false;
            }else{
                /* get url from href */
                var url = $(this).attr("href");
                /* ./end */
                
                /* check it */
                if(url !== "#" && dev_app_settings.dev_page_loader === true){
                    /* start loading layer */
                    dev_loaders_page_loader.open();
                    /* ./end */

                    /* wait .5sec and reload page */
                    setTimeout(function(){
                        window.location.href = url;
                    },500);
                    /* ./end */

                    return false;                    
                }
                /* ./check it */                
            }
            
        });
        /* ./end */
        
        $(".dev-page-sidebar").mCustomScrollbar({axis:"y", autoHideScrollbar: true, scrollInertia: 200, advanced: {autoScrollOnFocus: false}});
        
        if($(".dev-page").hasClass("dev-page-sidebar-minimized"))
            $(".dev-page-sidebar").mCustomScrollbar("disable",true);
        
    /* ./navigation functions */    
    }
};

var dev_header_navigation = {
    init: function() {
        
        if($(".dev-page-header .dev-page-navigation").length === 0) return false;
        
        /* add class .has-child to all elements that have childs */
        $(".dev-page-navigation li").each(function(){
            if($(this).find(" > ul").length > 0){
                $(this).addClass("has-child");
            }            
        });
        /* ./end */      
        
        $(".dev-page-navigation li").removeClass("active");
        
        if(window.innerWidth > 992){
            $(".dev-page-navigation li > a").unbind("click");            
            
            $(".dev-page-navigation li").hover(function(){
                $(this).addClass("active");                  
            },function(){            
                $(this).removeClass("active");            
            });
        }else{            
            $(".dev-page-navigation li").unbind('mouseenter mouseleave');
            
            $(".dev-page-navigation li > a").on("click",function(){
                $(this).parent("li").toggleClass("active");
                
                if($(this).parent("li").hasClass("has-child")) return false;
            });
        }
        
        $(".dev-page-navigation-toggle").on("click",function(){
            $(".dev-page-navigation li").removeClass("active");
            $(".dev-page-navigation").toggleClass("show");
            return false;
        });
        
    }
};

var dev_container_tabbed = {
    init: function(){
        
        if($(".container-tabbed").length === 0) return false;
        
        $(".container-tabbed .container-tabs a").on("click",function(){
            var link = $(this).attr("href");
            if($(link).length > 0){
                $(".container-tabbed .container-tabs a").removeClass("active");
                $(".container-tabbed .container-tab").removeClass("active");
                
                $(this).addClass("active");
                $(link).addClass("active");
            }
            
            return false;
        });        
        
    }    
};

var dev_panels = {
    init: function(){
        
        /* add collapse panel handler */
        $(".panel-collapse").on("click",function(){
            var panel = $(this).parents(".panel");
            
            if(panel.hasClass("panel-collapsed")){
                panel.removeClass("panel-collapsed");
                $(this).find(".fa").removeClass("fa-angle-up").addClass("fa-angle-down");
            }else{
                panel.addClass("panel-collapsed");
                $(this).find(".fa").removeClass("fa-angle-down").addClass("fa-angle-up");                
            }
            
            dev_layout_alpha_content.init(dev_layout_alpha_settings);
            
            return false;
        });
        /* ./end */
        
    }
};

/* remove panel function */
function dev_panel_remove(p,c){    
    if(typeof c === "function") c(); // run callback         
     
    /* remove panel */
    p.animate({opacity: "0"},200,"linear",function(){
        $(this).remove();
        dev_layout_alpha_content.init(dev_layout_alpha_settings);
    });        
    
    return false;
}
/* ./remove panel function */

/* panel fullscreen mode */
function dev_panel_fullscreen(panel){    
    
    if(panel.hasClass("panel-fullscreened")){
        panel.removeClass("panel-fullscreened").unwrap();
        panel.find(".panel-body").css("height","");
        panel.find(".panel-fullscreen .fa").removeClass("fa-compress").addClass("fa-expand");
        $(".dev-page").removeClass("dev-page-fullscreen");
        dev_layout_alpha_content.init(dev_layout_alpha_settings);
    }else{
        var head    = panel.find(".panel-heading");
        var body    = panel.find(".panel-body");
        var footer  = panel.find(".panel-footer");
        var hplus   = 10;
        
        if(body.hasClass("panel-body-table") || body.hasClass("padding-0")){
            hplus = 0;
        }
        if(head.length > 0){
            hplus += head.height()+21;
        } 
        if(footer.length > 0){
            hplus += footer.height()+21;
        } 

        panel.find(".panel-body,.chart-holder").height($(window).height() - hplus);
        
        
        panel.addClass("panel-fullscreened").wrap('<div class="panel-fullscreen-wrap"></div>');        
        panel.find(".panel-fullscreen .fa").removeClass("fa-expand").addClass("fa-compress");
        $(".dev-page").addClass("dev-page-fullscreen");
        
        dev_layout_alpha_content.init(dev_layout_alpha_settings);
    }
}
/* ./panel fullscreen mode */

var dev_login = {
    init: function(){
        
        $(".dev-page-login-block input").focus(function(){
            $(this).parents(".form-group").addClass("focus");
        }).blur(function(){
            $(this).parents(".form-group").removeClass("focus");
        });
        
    }
};

var dev_forms = {
    init: function(){
    /* default form functions */
    
        /* close on click outside */
        $("html").on("click",function() {
            $(".form-group-custom").not(".disabled").removeClass("active");
        });
        /* ./end */
        
        /* add disabled clas to custom form group if there any disabled input */
        $(".form-group-custom input:disabled").each(function(){
            $(this).parents(".form-group-custom").addClass("disabled");
        });
        /* ./end */

        /* spy click on form group */
        $(".form-group-custom").not(".disabled").on("click",function(e){            
            
            $(".form-group-custom").not($(this)).removeClass("active");            
            $(this).addClass("active");
            
            $(this).find("input,textarea,select").focus();            
            
            e.stopPropagation();
        });
        /* ./spy click on form group */
        
        /* spy focus on form elements */
        $(".form-group-custom input, .form-group-custom textarea, .form-group-custom select").focus(function(e){
            $(this).parent(".form-group-custom").not(".disabled").addClass("active");
            e.stopPropagation();
        }).blur(function(){            
            $(this).parent(".form-group-custom").not(".disabled").removeClass("active");           
        });
        /* ./spy focus on form elements */
        
        /* clear field function */
        $(".form-control-clear").each(function(){
            /* add class if no label */
            if($(this).find("label").length === 0)
                $(this).addClass("form-control-clear-no-head");
            /* ./add class if no label */
            
            /* wrap each form element in group */
            $(this).find("input,select,textarea").each(function(){                
                $(this).data("default-value",$(this).val());        
                $(this).wrap("<div class=\"form-control-clear-wrap\"></div>");
                $(this).parent("div").append("<a class=\"form-control-clear-button\"><span class=\"glyphicon glyphicon-remove\"></span></a>");                
            });
            /* ./wrap each form element in group */
        });
        
        $(".form-control-clear-button").on("click",function(){
            var input = $(this).prev("input,select,textarea");
            input.val("");
            //input.val(input.data("default-value"))); // use this code if you need to restore default value
        });
        /* ./clear field function */
        
    /* ./default form functions */

        /* file input */
        $("input.file").each(function(){
            var if_title = typeof $(this).attr("title") === "undefined" ? "Browse" : $(this).attr("title");
            var if_class = $(this).attr("class").replace("file","");

            if_class = if_class === "" ? " btn-default" : if_class;
            if_class = $(this).is(":disabled") ? if_class+" disabled" : if_class;

            $(this).wrap("<a href=\"#\" class=\"file-input btn"+if_class+"\"></a>").parent("a").append("<span>"+if_title+"</span>");
            $(this).parent("a").after("<span class=\"file-input-name\"></span>");
        });

        $("input.file").on("change",function(){
            $(this).parent("a").next(".file-input-name").html($(this).val().split('/').pop().split('\\').pop());
        });
        /* ./file input */        

        /* bootstrap select */
        if($(".selectpicker").length > 0)
            $(".selectpicker").selectpicker();        
        /* ./bootstra select */

        /* bootstrap datepicker */
        if($(".datetimepicker").length > 0)
            $(".datetimepicker").datetimepicker();
        
        if($(".datepicker").length > 0)
            $(".datepicker").datetimepicker({format: "MM/DD/YYYY"});
        
        if($(".timepicker").length > 0)
            $(".timepicker").datetimepicker({format: "HH:mm"});
        
        if($(".datepickeryears").length > 0)
            $(".datepickeryears").datetimepicker({viewMode: 'years'});
        /* ./bootstrap datepicker */

        /* specturm colorpicker */
        if($(".colorpicker").length > 0){
            $(".colorpicker").spectrum({showAlpha: true, showPalette: true, showSelectionPalette: true, palette: [], localStorageKey: "spectrum.homepage", showInput: true,preferredFormat: "hex", maxSelectionSize: 20});            
        }
        /* ./specturm colorpicker */

        /* tags input */
        if($("input.tags").length > 0)
            $("input.tags").tagsInput({width:'auto',height:'auto',defaultText: "Add a tag"});
        /* ./tags input */

        /* masked inputs */
        if($("input[class*='mask_']").length > 0){        
            $("input.mask_tin").mask('99-9999999');
            $("input.mask_ssn").mask('999-99-9999');        
            $("input.mask_date").mask('9999-99-99');
            $("input.mask_product").mask('a*-999-a999');
            $("input.mask_phone").mask('99 (999) 999-99-99');
            $("input.mask_phone_ext").mask('99 (999) 999-9999? x99999');
            $("input.mask_credit").mask('9999-9999-9999-9999');        
            $("input.mask_percent").mask('99%');
            $("input.mask_price").mask('$9.99');
        }
        /* ./masked inputs */
        
        /* tooltip */        
        $(".tip").tooltip();
        /* ./tooltip */
        
        /* summernote */
        if($(".summernote").length > 0)
            $(".summernote").summernote({'width': '100%', 'height': '300px'});
        /* ./summernote */            
        
        /* popover */
        $("[data-toggle='popover']").popover();
        /* ./popover */        
    }
};

// Counter-up
var counter = {
    init: function(){
        // init Counter-up
        if($(".counter").length > 0)
           $(".counter").counterUp({delay: 10,time: 1000});        
        // ./end
    }
};
// ./Counter-up

// Start Smart Wizard
var smartWizard = {    
    init: function(){

        if($(".wizard").length > 0){

            //check count of steps in each wizard
            $(".wizard > ul").each(function(){
                $(this).addClass("steps_"+$(this).children("li").length);
            });// ./end            
            
            // this is demo, can be removed if wizard is not using
            if($("#wizard-validation").length > 0){
                
                var validate =  $("#wizard-validation").validate({
                        errorClass:"has-error",
                        validClass:"has-success",
                        errorElement:"span",
                        ignore: [],                        
                        errorPlacement: function(error,element){
                            $(element).after(error);                    
                            $(element).parents(".form-group").addClass("has-error");
                        },
                        highlight: function(element, errorClass){
                            $(element).parents(".form-group").removeClass("has-success").addClass(errorClass);
                            dev_layout_alpha_content.init(dev_layout_alpha_settings);
                        },
                        unhighlight: function(element, errorClass, validClass){                    
                            $(element).parents(".form-group").removeClass(errorClass).addClass(validClass);
                            dev_layout_alpha_content.init(dev_layout_alpha_settings);
                        },
                        rules: {
                            login:      {required: true, minlength: 2, maxlength: 8},
                            password:   {required: true, minlength: 5, maxlength: 10},
                            repassword: {required: true, minlength: 5, maxlength: 10, equalTo: "#password"},
                            email:      {required: true, email: true},
                            name:       {required: true, maxlength: 10},
                            adress:     {required: true}
                        }
                });
                
            }// ./end of demo
            
            
            // init wizard plugin
            $(".wizard").smartWizard({
                // This part (using for wizard validation) of code can be removed FROM 
                onLeaveStep: function(obj){
                    var wizard = obj.parents(".wizard");

                    if(wizard.hasClass("wizard-validation")){

                        var valid = true;

                        $('input,textarea',$(obj.attr("href"))).each(function(i,v){
                            valid = validate.element(v) && valid;
                        });

                        if(!valid){
                            wizard.find(".stepContainer").removeAttr("style");
                            validate.focusInvalid();                                
                            return false;
                        }         
                    }    

                    dev_layout_alpha_content.init(dev_layout_alpha_settings);

                    return true;
                },// <-- TO
                //this is important part of wizard init
                onShowStep: function(obj){
                    var wizard = obj.parents(".wizard");

                    if(wizard.hasClass("show-submit")){

                        var step_num = obj.attr('rel');
                        var step_max = obj.parents(".anchor").find("li").length;

                        if(step_num == step_max){                             
                            obj.parents(".wizard").find(".actionBar .btn-primary").css("display","block");
                        }                         
                    }
                    
                    dev_layout_alpha_content.init(dev_layout_alpha_settings);
                    
                    return true;                         
                }// ./end
            });
            
            /*
            $(".modal").on('show.bs.modal', function (e) {
                $(this).find(".wizard").smartWizard("fixHeight");                
            });*/
        }            

    }
};//./start smart wizard

// dev tables 
var dev_tables = {
    init: function(){
        
        $(".table-controls").each(function(){
            
            
            var table = $(this).find("table");
            var thead = table.find("thead");
            var tbody = table.find("tbody");
            
            
            /* Buil list of headers */
            var list    = $("<select></select>").attr("multiple",true).addClass("form-control selectpicker");
            
            table.find("thead th").each(function(){               
               var option = $("<option></option>").attr("selected",true).val($(this).index()).html($(this).html());
                list.append(option);
            });            
            
            $(this).find(".table-controls-block").append(list);            
            /* ./Buil list of headers */
            
            /* spy change select values */
            list.on("change",function(){
                $(this).find("option").each(function(){
                    
                    var index = parseInt($(this).val()) + 1;
                    
                    if($(this).is(":selected")){
                        thead.find("tr th:nth-child("+index+")").show();
                        tbody.find("tr td:nth-child("+index+")").show();                        
                    }else{
                        thead.find("tr th:nth-child("+index+")").hide();                        
                        tbody.find("tr td:nth-child("+index+")").hide();                        
                    }
                });
                if($(this).find("option").length === $(this).find("option:not(:selected)").length){
                    tbody.append("<tr class=\"table-no-columns\"><td>No columns selected.</td></tr>");
                }else
                    tbody.find(".table-no-columns").remove();
            });
            /* ./spy change select values */
        });
        
    }
};

var dev_table = {
    init: function(){
        
        $(".dev-table .dev-row").each(function(){
            var cols = $(this).find(".dev-col");
            var count = cols.length;
            var width = Math.floor($(this).width() / count);
            
            cols.each(function(){
                $(this).width(width);
            });
            
        });
        
    }    
};

var sparkLineStat = {};

var charts = {
    init: function(){
        // Sparkline                    
        //if($(".sparkline").length > 0)
           //$(".sparkline").sparkline('html', { enableTagOptions: true,disableHiddenCheck: true});              
       // End sparkline
       
       /*== SPARKLINE==*/
        if ($.fn.sparkline) {

            $(".d-pending").sparkline([3, 1], {
                type: 'pie',
                width: '40',
                height: '40',
                sliceColors: ['#e1e1e1', '#8175c9']
            });



            var sparkLine = function () {
                $(".sparkline").each(function () {
                    var $data = $(this).data();
                    ($data.type == 'pie') && $data.sliceColors && ($data.sliceColors = eval($data.sliceColors));
                    ($data.type == 'bar') && $data.stackedBarColor && ($data.stackedBarColor = eval($data.stackedBarColor));

                    $data.valueSpots = {
                        '0:': $data.spotColor
                    };
                    
                    $(this).sparkline($data.data || "html", $data);
                    
                    var date = new Date(),
                        minu = (60-date.getMinutes())+1;
                    
                    var scope = $(this),
                        name  = $data.name;
                    
                    var update = function(){
                        if(sparkLineStat[name]) scope.sparkline(sparkLineStat[name],$data);
                    }
                    
                    setTimeout(function(){
                        update();
                        
                        setInterval(update,1000*60*60)
                    },1000*60*minu)
                    
                    if ($(this).data("compositeData")) {
                        $spdata.composite = true;
                        $spdata.minSpotColor = false;
                        $spdata.maxSpotColor = false;
                        $spdata.valueSpots = {
                            '0:': $spdata.spotColor
                        };
                        $(this).sparkline($(this).data("compositeData"), $spdata);
                    };
                });
            };

            var sparkResize;
            $(window).resize(function (e) {
                clearTimeout(sparkResize);
                sparkResize = setTimeout(function () {
                    sparkLine(true)
                }, 500);
            });
            sparkLine(false);



        }
        
        
        /*==Easy Pie chart ==*/
        if ($.fn.easyPieChart) {

            $('.notification-pie-chart').easyPieChart({
                onStep: function (from, to, percent) {
                    $(this.el).find('.percent').text(Math.round(percent));
                },
                barColor: "#39b6ac",
                lineWidth: 3,
                size: 50,
                trackColor: "#efefef",
                scaleColor: "#cccccc"

            });
            
            $('.pc-epie-chart').each(function(){
                var scope = $(this);
                var update = function(){
                    scope.data('easyPieChart').update(parseFloat(scope.find('.percent').text()));
                }
                
                scope.easyPieChart({
                    onStep: function(from, to, percent) {
                        //$(this.el).find('.percent').text(Math.round(percent));
                    },
                    barColor: "#F8A20F",
                    lineWidth: 5,
                    size:80,
                    trackColor: "#EFEFEF",
                    scaleColor:"#cccccc"
    
                })
                
                setInterval(update,30000)
            })

        }
    }
};

var datatables = {
    init: function(){
        
        if($(".table-sortable").length > 0){
            /* init default sortable table */
            $(".table-sortable").DataTable({
                "fnInitComplete": function() {
                    $(".dataTables_wrapper").find("select,input").addClass("form-control");
                }
            });            
            /* ./init default sortable table */
            
            /* udate page content on page change */
            $(".table-sortable").on('page.dt',function() {
                setTimeout(function(){
                    dev_layout_alpha_content.init(dev_layout_alpha_settings);
                },100);                
            });
            /* ./udate page content on page change */                
            
            /* update page content on search */
            $(".table-sortable").on( 'search.dt', function() {
                setTimeout(function(){
                    dev_layout_alpha_content.init(dev_layout_alpha_settings);
                },200);                
            });
            /* ./update page content on search */
            
            /* uppdate page content on change length */
            $('.table-sortable').on('length.dt', function() {
                setTimeout(function(){
                    dev_layout_alpha_content.init(dev_layout_alpha_settings);
                },100);                
            });
            /* ./uppdate page content on change length */
        }
        
    }
};

var dev_accordion = {
    init: function(){
        
        $(".accordion .panel-title > a").on("click",function(){
            
            var accordion   = $(this).parents(".accordion");
            var noCollapse  = accordion.hasClass("accordion-dc");
            
            var panel       = $(this).parents(".panel");
            var pbody       = $($(this).attr("href"));
            
            if(pbody.length > 0){
                
                if(panel.hasClass("panel-opened")){
                    
                    pbody.slideUp(200,function(){
                        panel.removeClass("panel-opened");
                    });
                    
                }else{
                    
                    pbody.slideDown(200,function(){
                        panel.addClass("panel-opened");
                    });
                    
                }
                
                if(!noCollapse){
                    accordion.find(".panel").not(panel).find(".panel-body").slideUp(200,function(){
                        $(this).parents(".panel").removeClass("panel-opened");                        
                    });                                           
                }
                
                setTimeout(function(){
                    dev_layout_alpha_content.init(dev_layout_alpha_settings);
                },500);                
                return false;
            }
            
        });                

    }
};

var widget_tabbed = {
    init: function(){
        
        $(".widget-tabbed").each(function(){
            
            var widget = $(this);
            var active = widget.find(".widget-tabs > li.active > a").attr("href");
            
            widget.find(".widget-tab").removeClass("active");
            $(active).addClass("active");
            
            setTimeout(function(){
                dev_layout_alpha_content.init(dev_layout_alpha_settings);
            },200);            
            
            $(this).find(".widget-tabs li a").click(function(){
                widget.find(".widget-tab").removeClass("active");
                widget.find(".widget-tabs li.active").removeClass("active");
                
                $($(this).attr("href")).addClass("active");
                $(this).parents(".widget-tabs").find("li").removeClass("active");
                $(this).parent("li").addClass("active");
                
                setTimeout(function(){
                    dev_layout_alpha_content.init(dev_layout_alpha_settings);
                },200);
                
                return false;
            });
            
        });        
    }
};

var list_tasks = {
    init: function(){
        /*
        $(".list-tasks .list-tasks-item").on("click",function(){
            
            if($(this).find(".checkbox > input").prop("checked")){
                $(this).removeClass("active");
                $(this).find(".checkbox > input").prop("checked",false);
            }else{
                $(this).addClass("active");
                $(this).find(".checkbox > input").prop("checked",true);
            }
            
        });
        */
    }
}

var tabs = {
    init: function(){
        $('a[data-toggle="tab"]').on('shown.bs.tab', function () {
            dev_layout_alpha_content.init(dev_layout_alpha_settings);
        });
    }
};

var knob = {
    init: function(){
        if($(".knob").length > 0)
            $(".knob").knob();        
    }
};

var scroll = {    
    init: function(){        
        if($(".scroll").length > 0){
            $(".scroll").mCustomScrollbar({axis:"y", autoHideScrollbar: true, scrollInertia: 200, advanced: {autoScrollOnFocus: false}});
        }
    }    
};

$(function(){    
    dev_custom.init();
    dev_sidebar_navigation.init();
    dev_header_navigation.init();
    dev_header.init();    
    dev_tables.init();
    counter.init();
    knob.init();
    smartWizard.init();
    dev_forms.init();
    dev_accordion.init();
    tabs.init();
    datatables.init();    
    scroll.init();
    dev_panels.init();
    dev_login.init();
    charts.init();
    widget_tabbed.init();
    list_tasks.init();
    dev_container_tabbed.init();
});

$(window).resize(function(){
    dev_header_navigation.init();
});
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
/*

highlight v4

Highlights arbitrary terms.

<http://johannburkard.de/blog/programming/javascript/highlight-javascript-text-higlighting-jquery-plugin.html>

MIT license.

Johann Burkard
<http://johannburkard.de>
<mailto:jb@eaio.com>

*/

jQuery.fn.highlight = function(pat) {
 function innerHighlight(node, pat) {
  var skip = 0;
  if (node.nodeType == 3) {
   var pos = node.data.toUpperCase().indexOf(pat);
   if (pos >= 0) {
    var spannode = document.createElement('span');
    spannode.className = 'faq-highlight';
    var middlebit = node.splitText(pos);
    var endbit = middlebit.splitText(pat.length);
    var middleclone = middlebit.cloneNode(true);
    spannode.appendChild(middleclone);
    middlebit.parentNode.replaceChild(spannode, middlebit);
    skip = 1;
   }
  }
  else if (node.nodeType == 1 && node.childNodes && !/(script|style)/i.test(node.tagName)) {
   for (var i = 0; i < node.childNodes.length; ++i) {
    i += innerHighlight(node.childNodes[i], pat);
   }
  }
  return skip;
 }
 return this.length && pat && pat.length ? this.each(function() {
  innerHighlight(this, pat.toUpperCase());
 }) : this;
};

jQuery.fn.removeHighlight = function() {
 return this.find("span.label").each(function() {
  this.parentNode.firstChild.nodeName;
  with (this.parentNode) {
   replaceChild(this.firstChild, this);
   normalize();
  }
 }).end();
};

"use strict";

var faq = {
    init: function(){
        
        $(".faq .faq-item .faq-title").click(function(){        
            var item = $(this).parent('.faq-item');

            if(item.hasClass("active"))
                $(this).find(".fa").removeClass("fa-angle-up").addClass("fa-angle-down");
            else
                $(this).find(".fa").removeClass("fa-angle-down").addClass("fa-angle-up");

            item.toggleClass("active");
            
            setTimeout(function(){
                dev_layout_alpha_content.init(dev_layout_alpha_settings);
            },200);            
        });
        
        $("#dev-faq-keyword").on("keydown",function(e){
			if(e.keyCode == 13){
				
				var keyword = $("#dev-faq-keyword").val();

				if(keyword.length >= 3){
					$(".faq .faq-item").removeClass("active");

					$("#dev-faq-search-result").html("");
					$(".faq").removeHighlight();

					var items = $(".faq .faq-text:containsi('"+keyword+"')");

					items.highlight(keyword);

					items.each(function(){
						$(this).parent(".faq-item").addClass("active");
					});

					setTimeout(function(){
						dev_layout_alpha_content.init(dev_layout_alpha_settings);
					},200);            

					$("#dev-faq-search-result").html("<strong>Search result:</strong>: <span class='text-success'>Found in "+items.length+" answers</span>");            
				}else
					$("#dev-faq-search-result").html("<strong>Search result:</strong>: <span class='text-error'>Minimum 3 chars required</span>");
				
				return false;
			}
        })
        
        $("#dev-faq-open").click(function(){
            $(".faq .faq-item").addClass("active");
            
            setTimeout(function(){
                dev_layout_alpha_content.init(dev_layout_alpha_settings);
            },200);
        });

        $("#dev-faq-close").click(function(){
            $(".faq .faq-item").removeClass("active");
            
            setTimeout(function(){
                dev_layout_alpha_content.init(dev_layout_alpha_settings);
            },200);            
        });

        $("#dev-faq-remove-highlights").click(function(){
            var hl = $(".faq").find(".faq-highlight");
            hl.each(function(){
                var txt = $(this).html();
                $(this).after(txt);
                $(this).remove();
            });
        });
        
    }
};

$(function(){
    faq.init();
});
/*
function loadPage(a,href){
    var url = href || $(a).attr('href');
    
    $('#page-content').animate({opacity: 0},150,function(){
		url += url.indexOf('?') !== -1 ? '&' : '?';
		url += 'ajax=true';
		
        $.sl('load', url, { type: 'GET', ignore: true }, function (content) {
            $('#page-content').html(content).animate({opacity: 1},150)
			
			widget_tabbed.init();
			initTabs();
			
			$(window).trigger('resize')
			
			$('.selectpicker').selectpicker('refresh');
        })
    })
    
    
    
    return false;
}
*/

function loadPage(a,href){
	if($('#sl_info').length) $('#sl_info').fadeOut(100);
	
    var url = href || $(a).attr('href');

	url += url.indexOf('?') !== -1 ? '&' : '?';
	url += 'ajax=true';
	
	$('#page-content').animate({opacity: 0},70)
	
	
	var loading = $('<div class="content-loading"></div>');
		loading.appendTo('.dev-page-content')
	
	$.get(url,function(content){
		
		$('#page-content').stop().animate({opacity: 1},120).html(content)
		
		loading.remove();
		
		widget_tabbed.init();
		initTabs();
		
		faq.init();
		
		$(window).trigger('resize')
		
		$('.selectpicker').selectpicker('refresh');
		
		// Массовое уделание элементов
		IsConfirmDeleteElement = true;
	})
    
    
    return false;
}


function saveEditSite(self,href) {
	$(self).sl('load', href, { back: false, ignore:true, data: $('#form').serializeArray(), dataType: 'json' }, function (j) 
	{
		if(j.updateToIds){
			$.each(j.updateToIds,function(i,row){
				$('#c'+row.key + ' input.set-id-val').val(row.id)
			})
		}
		
		if (j.msg) $.sl('info', j.msg)
		else if (j.id > 0) {
            $('#itemID').val(j.id);
			$.sl('info', 'Данные созданы')
        }
	    else if (j.result) { }
		else $.sl('info', 'Неизвестная ошибка')
    })
}


function saveAndRedirect(self,href, hrefToRedirect) { 
	$(self).sl('load', href, { back: false, ignore:true, data: $('#form').serializeArray(), dataType: 'json' }, function (j) {
		if (j.msg) $.sl('info', j.msg)
		else if (j.id > 0) {
			loadPage(false, hrefToRedirect + '?Id=' + j.id);
		}
		else {
			loadPage(false, hrefToRedirect);
		}
	})
}



function cmdToAV(self, startStop) { 
	$(self).sl('load', '/security/antivirus/' + startStop, { back: false, ignore:true, data: $('#form').serializeArray(), dataType: 'json' }, function (j) {
		if (j.msg) $.sl('info', j.msg)
		else {
			loadPage(false, '/security/antivirus');
		}
	})
}





function initTabs(){
	$('a[data-toggle="tab"]').on('click',function(){
		$(window).trigger('resize')
	})
}

function addIgnoreToIP(value) {

	var id = new Date().getTime() + Math.round(Math.random() * 1000);
	var code = $([
		'<tr id="c' + id + '">',
		'<td><input type="hidden" name="IgnoreToIP[' + id + '].name" value="IgnoreToIP"><div class="add-ads-input-light"><textarea rows="1" style="height: 24px; overflow: hidden; resize: none;" class="name alias" name="IgnoreToIP[' + id + '].value">'+value+'</textarea></div></td>',
		'<td style="text-align: right;" class="table-products"><a class="btn delete"><i class="fa fa-trash-o"></i></a></td>',
		'</tr>'
	].join(''));

	$('.delete', code).click(function () {
		$('#c' + id).remove();
	})


	code.appendTo('#tab-IgnoreToIP');
	$(window).trigger('resize');
}


function addWhiteList(option) {
    var data = {
        Description: '',
        Value: '',
        Type: 2,
        Id: 0
    }
    $.extend(data, option || {})

	var id = new Date().getTime() + Math.round(Math.random() * 1000);
	var code = $([
		'<tr id="c' + id + '">',
        '<td>',
        '<input type="hidden" class="set-id-val" name="whiteList[' + id + '].Id" value="' + data.Id + '" />',
		'<div class="form-group" style="margin-bottom: 0px;">',
		'<select class="form-control selectpicker method set-type-val" name="whiteList[' + id + '].Type">',
        '<option value="IPv4Or6" ' + (data.Type == '2' ? 'selected' : '') + '>IP-адрес</option>',
        '<option value="PTR" ' + (data.Type == '1' ? 'selected' : '') + '>PTR</option>',
        '<option value="UserAgent" ' + (data.Type == '3' ? 'selected' : '') + '>User Agent</option>',
		'</select>',
		'</div> ',
		'</td>',
        '<td><div class="add-ads-input-light"><textarea rows="1" style="height: 24px; overflow: hidden; resize: none;" class="name" name="whiteList[' + id + '].Description">' + data.Description + '</textarea></div></td>',
        '<td><div class="add-ads-input-light"><textarea rows="1" style="height: 24px; overflow: hidden; resize: none;" class="name set-value-val" name="whiteList[' + id + '].Value">' + data.Value + '</textarea></div></td>',
		'<td class="table-products btn-icons text-right"><a class="btn delete"><i class="fa fa-trash-o"></i></a></td>',
		'</tr>'
    ].join(''));

    var codeEmpty = $([
        '<tr id="c' + id + '">',
        '<td>' + (data.Type == 1 ? 'PTR' : data.Type == 2 ? 'IPv4Or6' : 'User-Agent') + '</td>',
        '<td>' + (data.Description || '') + '</td>',
        '<td>' + (data.Value || '') + '</td>',
        '<td class="table-products btn-icons text-right"><a class="btn edit"><i class="fa fa-gear"></i></a></td>',
        '</tr>'
    ].join(''));

    if (option) codeEmpty.appendTo('#site-whitelist');
    else {
        code.appendTo('#site-whitelist');
        $(window).trigger('resize');
    }

    $('.edit', codeEmpty).on('click', function () {
        codeEmpty.after(code);
        codeEmpty.remove();
    })

    $('.delete', code).click(function () {
        $('#c' + id).remove();

        var newId = $('.set-id-val', code).val();
        if (newId > 0)
            $.sl('load', '/settings/remove/whiteList', { back: false, ignore: true, data: { id: newId }, dataType: 'json' })
    })
	
	$('.selectpicker', code).selectpicker('refresh');
	$('.selectpicker', code).parent().on('show.bs.dropdown', function () {
		$('.table-visible').css({overflow: 'inherit'});
	}).on('hide.bs.dropdown', function () {
		$('.table-visible').css({overflow: 'auto'});
	})
}


function addIgnoreToLog(value) {

	var id = new Date().getTime() + Math.round(Math.random() * 1000);
	var code = $([
		'<tr id="c' + id + '">',
		'<td><div class="add-ads-input-light"><textarea rows="1" style="height: 24px; overflow: hidden; resize: none;" class="name" name="IgnoreToLogs[' + id + '].rule">'+value+'</textarea></div></td>',
		'<td style="text-align: right;" class="table-products"><a class="btn delete"><i class="fa fa-trash-o"></i></a></td>',
		'</tr>'
	].join(''));

	$('.delete', code).click(function () {
		$('#c' + id).remove();
	})


	code.appendTo('#tab-IgnoreToLog');
	$(window).trigger('resize');
}


function addTemplateToId(options){
	var params = jQuery.extend({
		selector: '.tags-selective',
		tagsAll: [],
		tagsSelect: []
    },options);
	
	var tagsExist = [];

	for (var i = 0; i < params.tagsAll.length; i++) {
		tagsExist.push(params.tagsAll[i].Name);
	}
	
	var hiddenContainer = $('<div></div>');
	$(params.selector).after(hiddenContainer);

	$("input.tags-df").tagsInput({
		width: 'auto',
		height: 'auto',
		defaultText: "",
		interactive: true,
		autocomplete: {
			source: tagsExist
		},
		onAddTag: function (name) {
			if (tagsExist.indexOf(name) == -1) $(this).removeTag(name);
		},
		onChange: function () {
			hiddenContainer.empty()
			for (var i = 0; i < params.tagsAll.length; i++) {
				if ($(this).tagExist(params.tagsAll[i].Name)) {
					var id = new Date().getTime() + Math.round(Math.random() * 1000);
					hiddenContainer.append('<input type="hidden" name="templates[' + id + '].Template" value="' + params.tagsAll[i].Id + '" />');
				}
			}
		}
	});

	for (var i = 0; i < params.tagsSelect.length; i++) {
		$(params.selector).addTag(params.tagsSelect[i])
	}
}


function addIgnoreToFileOrFolders(value) {

	var id = new Date().getTime() + Math.round(Math.random() * 1000);
	var code = $([
		'<tr id="c' + id + '">',
		'<td><div class="add-ads-input-light"><textarea rows="1" style="height: 24px; overflow: hidden; resize: none;" class="name alias" name="ignr[' + id + '].Patch">'+value+'</textarea></div></td>',
		'<td style="text-align: right;" class="table-products"><a class="btn delete"><i class="fa fa-trash-o"></i></a></td>',
		'</tr>'
	].join(''));

	$('.delete', code).click(function () {
		$('#c' + id).remove();
	})


	code.appendTo('#tab-IgnoreToFileOrFolders');
	$(window).trigger('resize');
}

function addNewAlias(DomainId, option) {
	var data = {
        host: '',
        Folder: '',
		Id: 0
	}
	$.extend(data, option || {})

	var id = new Date().getTime() + Math.round(Math.random() * 1000);
	var code = $([
		'<tr id="c' + id + '">',
		'<input type="hidden" class="set-id-val" name="aliases[' + id + '].Id" value="' + data.Id + '" />',
        '<td><div class="add-ads-input-light"><input class="name" name="aliases[' + id + '].host" value="' + data.host +'"></div></td>',
        '<td><div class="add-ads-input-light"><input class="name" name="aliases[' + id + '].Folder" value="'+(data.Folder || '')+'"></div></td>',
		'<td style="text-align: right;" class="table-products btn-icons">',
		'<a href="/requests-filter/monitoring?ShowHost=' + data.host + '" style="' + (data.host ? '' : 'display: none') + '" onclick="return loadPage(this)" class="btn"><i class="fa fa-bar-chart"></i></a>',
		'<a class="btn delete"><i class="fa fa-trash-o"></i></a></td>',
		'</tr>'
    ].join(''));
    
    var codeEmpty = $([
        '<tr id="c' + id + '">',
        '<td>' + data.host + '</td>',
        '<td>' + (data.Folder || 'не указана') + '</td>',
        '<td class="table-products btn-icons text-right">',
        '<a href="/requests-filter/monitoring?ShowHost=' + data.host + '" style="' + (data.host ? '' : 'display: none') + '" onclick="return loadPage(this)" class="btn"><i class="fa fa-bar-chart"></i></a>',
        '<a class="btn edit"><i class="fa fa-gear"></i></a>',
        '</td>',
        '</tr>'
    ].join(''));

    if (option) codeEmpty.appendTo('#site-aliases');
    else {
        code.appendTo('#site-aliases');
        $(window).trigger('resize');
    }

    $('.edit', codeEmpty).on('click', function () {
        codeEmpty.after(code);
        codeEmpty.remove();
    })

	$('.delete', code).click(function () 
	{
		var newId = $('.set-id-val',code).val();
		if (newId > 0) {
			deleteRule(id, '/requests-filter/common/remove/alias', {DomainId:DomainId,id:newId});
		}
		else { $('#c' + id).remove(); }
	})
}


function addNewRull(DomainId, TemplateId, content, option) {
	var data = {
		IsActive: 1,
		order: 0,
		Id: 0
	}

	$.extend(data, option || {})

	var id = new Date().getTime() + Math.round(Math.random() * 1000);
	var code = $([
		'<tr id="c' + id + '">',
		'<input type="hidden" class="set-id-val" name="rules[' + id + '].Id" value="' + data.Id + '" />',
		'<input type="hidden" name="rules[' + id + '].order" value="' + data.order + '" />',
		'<td><div class="checkbox checkbox-inline"><input name="rules[' + id + '].IsActive" class="status" type="checkbox" ' + (data.IsActive ? 'checked="true" value="true"' : 'value="false"') + ' id="check_' + id + '" /> <label for="check_' + id + '"></label></div></td>',
		'<td>',
		'<div class="form-group" style="margin-bottom: 0px;">',
		'<select class="form-control selectpicker method" name="rules[' + id + '].Method">',
		'<option value="0" selected>Любой</option>',
		'<option value="1" >POST</option>',
		'<option value="2" >GET</option>',
		'</select>',
		'</div> ',
		'</td>',
		'<td><div class="add-ads-input-light"><textarea rows="1" style="height: 24px; overflow: hidden; resize: none;" class="name rull" name="rules[' + id + '].rule"></textarea></div></td>',
		'<td class="table-products btn-icons text-right"><a class="btn delete"><i class="fa fa-trash-o"></i></a></td>',
		'</tr>'
    ].join(''));

    var codeEmpty = $([
        '<tr id="c' + id + '">',
        '<td>' + (data.IsActive ? 'On' : 'Off') + '</td>',
        '<td>' + (data.Method == 0 ? 'Любой' : data.Method == 1 ? 'POST' : 'GET') + '</td>',
        '<td class="name">' + (data.rule || '') + '</td>',
        '<td class="table-products btn-icons text-right"><a class="btn edit"><i class="fa fa-gear"></i></a></td>',
        '</tr>'
    ].join(''));

    if (data.Id != 0) codeEmpty.appendTo('#site-' + content);
    else {
        code.prependTo('#site-' + content);
        $(window).trigger('resize');
    }

    $('.edit', codeEmpty).on('click', function () {
        codeEmpty.after(code);
        codeEmpty.remove();
    })

	$(".status", code).on('change', function () {
		if ($(this).is(':checked')) {
			$(this).attr('value', 'true');
		} else {
			$(this).attr('value', 'false');
		}
	})

	$('.delete', code).click(function () 
	{
		var newId = $('.set-id-val',code).val();
		if (newId > 0) {
			deleteRule(id, '/requests-filter/common/remove/rule', {DomainId:DomainId,TemplateId:TemplateId,id:newId});
		}
		else { $('#c' + id).remove(); }
	})

	$('.method', code).val(data.Method)
	$('.rull', code).val(data.rule)
    

	$('.selectpicker', code).selectpicker('refresh');
	$('.selectpicker', code).parent().on('show.bs.dropdown', function () {
		$('.table-visible').css({overflow: 'inherit'});
	}).on('hide.bs.dropdown', function () {
		$('.table-visible').css({overflow: 'auto'});
	})
}



function addNewRullReplace(DomainId, TemplateId, option) {
	var data = {
		IsActive: 1,
		Id: 0,
		uri: '',
		GetArgs: '',
		PostArgs: '',
		RegexWhite: '',
		TypeResponse: 0,
		ResponceUri: '',
		ContentType: '',
		kode: '',
	}
	$.extend(data, option || {})

	var id = new Date().getTime() + Math.round(Math.random() * 1000);
	var code = $([
		'<tr id="c' + id + '">',
		'<input type="hidden" class="set-id-val" name="RuleReplaces[' + id + '].Id" value="' + data.Id + '" />',
		'<input type="hidden" class="set-type-val" name="RuleReplaces[' + id + '].TypeResponse" value="' + data.TypeResponse + '" />',
		'<td>' + 
			'<textarea style="display: none" class="set-GetArgs-val" name="RuleReplaces[' + id + '].GetArgs"></textarea>' + 
			'<textarea style="display: none" class="set-PostArgs-val" name="RuleReplaces[' + id + '].PostArgs"></textarea>' + 
			'<textarea style="display: none" class="set-RegexWhite-val" name="RuleReplaces[' + id + '].RegexWhite"></textarea>' + 
			'<textarea style="display: none" class="set-ResponceUri-val" name="RuleReplaces[' + id + '].ResponceUri"></textarea>' + 
			'<textarea style="display: none" class="set-ContentType-val" name="RuleReplaces[' + id + '].ContentType"></textarea>' + 
			'<textarea style="display: none" class="set-kode-val" name="RuleReplaces[' + id + '].kode"></textarea>' + 
			'<div class="checkbox checkbox-inline">' + 
				'<input name="RuleReplaces[' + id + '].IsActive" class="status" type="checkbox" ' + (data.IsActive ? 'checked="true" value="true"' : 'value="false"') + ' id="check_' + id + '" />' + 
				'<label for="check_' + id + '"></label>' + 
				'</div>' + 
		'</td>',
		'<td><div class="add-ads-input-light"><textarea rows="1" style="height: 24px; overflow: hidden; resize: none;" class="name set-uri-val" name="RuleReplaces[' + id + '].uri"></textarea></div></td>',
		'<td class="table-products btn-icons text-right"><a class="btn settings"><i class="fa fa-gear"></i></a><a class="btn delete"><i class="fa fa-trash-o"></i></a></td>',
		'</tr>',
    ].join(''));

    var codeEmpty = $([
        '<tr id="c' + id + '">',
        '<td>' + (data.IsActive ? 'On' : 'Off') + '</td>',
        '<td class="name">' + (data.uri || '') + '</td>',
        '<td class="table-products btn-icons text-right"><a class="btn edit"><i class="fa fa-gear"></i></a></td>',
        '</tr>'
    ].join(''));

    if (data.Id != 0) codeEmpty.appendTo('#site-ruls-replace');
    else {
        code.prependTo('#site-ruls-replace');
        $(window).trigger('resize');
    }

    $('.edit', codeEmpty).on('click', function () {
        codeEmpty.after(code);
        codeEmpty.remove();
    })

	$(".status", code).on('change', function () {
		if ($(this).is(':checked')) {
			$(this).attr('value', 'true');
		} else {
			$(this).attr('value', 'false');
		}
	})

	$('.delete', code).click(function () 
	{
		var newId = $('.set-id-val',code).val();
		if (newId > 0) {
			deleteRule(id, '/requests-filter/common/remove/rulereplace', {DomainId:DomainId,TemplateId:TemplateId,id:newId});
		}
		else { $('#c' + id).remove(); }
	})
	
	function showType(type){
		$('#sb-rulrpc-id-ResponceUri,#sb-rulrpc-id-ContentType,#sb-rulrpc-id-kode').hide();
		
		if(type == 0){
			$('#sb-rulrpc-id-ResponceUri').show();
			$('#sb-rulrpc-id-ContentType').hide();
			$('#sb-rulrpc-id-kode').hide();
		}
		else{
			$('#sb-rulrpc-id-ResponceUri').hide();
			$('#sb-rulrpc-id-ContentType').show();
			$('#sb-rulrpc-id-kode').show();
		}
	}
	
	$('.settings',code).click(function () 
	{
		$('#settings-ruls-replace-some').modal('show');
		
		var type = $('.set-type-val',code).val();
		showType(type);
		
		$('#sb-rulrpc-TypeResponse').unbind().on('change',function(){
			showType($(this).val());
		})
		
		$('#sb-rulrpc-TypeResponse').val(type).selectpicker('refresh');
		$('#sb-rulrpc-GetArgs').val($('.set-GetArgs-val',code).val());
		$('#sb-rulrpc-PostArgs').val($('.set-PostArgs-val',code).val());
		$('#sb-rulrpc-RegexWhite').val($('.set-RegexWhite-val',code).val());
		$('#sb-rulrpc-ResponceUri').val($('.set-ResponceUri-val',code).val());
		$('#sb-rulrpc-ContentType').val($('.set-ContentType-val',code).val());
		$('#sb-rulrpc-kode').val($('.set-kode-val',code).val());
		
		$('#sb-ruls-replace-btn').unbind().on('click',function(){
			$('.set-GetArgs-val',code).val($('#sb-rulrpc-GetArgs').val());
			$('.set-PostArgs-val',code).val($('#sb-rulrpc-PostArgs').val());
			$('.set-RegexWhite-val',code).val($('#sb-rulrpc-RegexWhite').val());
			$('.set-type-val',code).val($('#sb-rulrpc-TypeResponse').val());
			$('.set-ResponceUri-val',code).val($('#sb-rulrpc-ResponceUri').val());
			$('.set-ContentType-val',code).val($('#sb-rulrpc-ContentType').val());
			$('.set-kode-val',code).val($('#sb-rulrpc-kode').val());
			
			$('#settings-ruls-replace-some').modal('hide');
		});
	})
	
	
	$('.set-uri-val', code).val(data.uri);
	$('.set-GetArgs-val', code).val(data.GetArgs);
	$('.set-PostArgs-val', code).val(data.PostArgs);
	$('.set-RegexWhite-val', code).val(data.RegexWhite);
	$('.set-ResponceUri-val', code).val(data.ResponceUri);
	$('.set-ContentType-val', code).val(data.ContentType);
	$('.set-kode-val', code).val(data.kode);

	$('.selectpicker', code).selectpicker('refresh');
	$('.selectpicker', code).parent().on('show.bs.dropdown', function () {
		$('.table-visible').css({overflow: 'inherit'});
	}).on('hide.bs.dropdown', function () {
		$('.table-visible').css({overflow: 'auto'});
	})
}


function addNewRullOverride(DomainId, TemplateId, option) {
	var data = {
		IsActive: 1,
		order: 0,
		Id: 0
	}

	$.extend(data, option || {})

	var id = new Date().getTime() + Math.round(Math.random() * 1000);
	var code = $([
		'<tr id="c' + id + '">',
		'<input type="hidden" class="set-id-val" name="RuleOverrides[' + id + '].Id" value="' + data.Id + '" />',
		'<td><div class="checkbox checkbox-inline"><input name="RuleOverrides[' + id + '].IsActive" class="status" type="checkbox" ' + (data.IsActive ? 'checked="true" value="true"' : 'value="false"') + ' id="check_' + id + '" /> <label for="check_' + id + '"></label></div></td>',
		'<td>',
		'<div class="form-group" style="margin-bottom: 0px;">',
		'<select class="form-control selectpicker method" name="RuleOverrides[' + id + '].Method">',
		'<option value="0" selected>Любой</option>',
		'<option value="1" >POST</option>',
		'<option value="2" >GET</option>',
		'</select>',
		'</div> ',
		'</td>',
		'<td>',
		'<div class="form-group" style="margin-bottom: 0px;">',
		'<select class="form-control selectpicker order" name="RuleOverrides[' + id + '].order">',
		'<option value="0" selected>Allow</option>',
		'<option value="1" >Deny</option>',
		'<option value="2" >2FA</option>',
		'</select>',
		'</div> ',
		'</td>',
		'<td><div class="add-ads-input-light"><textarea rows="1" style="height: 24px; overflow: hidden; resize: none;" class="name rull" name="RuleOverrides[' + id + '].rule"></textarea></div></td>',
		'<td class="table-products btn-icons text-right"><a class="btn delete"><i class="fa fa-trash-o"></i></a></td>',
		'</tr>'
    ].join(''));

    var codeEmpty = $([
        '<tr id="c' + id + '">',
        '<td>' + (data.IsActive ? 'On' : 'Off') + '</td>',
        '<td>' + (data.Method == 0 ? 'Любой' : data.Method == 1 ? 'POST' : 'GET') + '</td>',
        '<td>' + (data.order == 0 ? 'Allow' : data.order == 1 ? 'Deny' : '2FA') + '</td>',
        '<td class="name">' + (data.rule || '') + '</td>',
        '<td class="table-products btn-icons text-right"><a class="btn edit"><i class="fa fa-gear"></i></a></td>',
        '</tr>'
    ].join(''));

    if (data.Id != 0) codeEmpty.appendTo('#site-ruls-override');
    else {
        code.prependTo('#site-ruls-override');
        $(window).trigger('resize');
    }

    $('.edit', codeEmpty).on('click', function () {
        codeEmpty.after(code);
        codeEmpty.remove();
    })

	$(".status", code).on('change', function () {
		if ($(this).is(':checked')) {
			$(this).attr('value', 'true');
		} else {
			$(this).attr('value', 'false');
		}
	})

	$('.delete', code).click(function () 
	{
		var newId = $('.set-id-val',code).val();
		if (newId > 0) {
			deleteRule(id, '/requests-filter/common/remove/ruleoverride', {DomainId:DomainId,TemplateId:TemplateId,id:newId});
		}
		else { $('#c' + id).remove(); }
	})

	$('.method', code).val(data.Method)
	$('.order', code).val(data.order)
	$('.rull', code).val(data.rule)

	$('.selectpicker', code).selectpicker('refresh');
	$('.selectpicker', code).parent().on('show.bs.dropdown', function () {
		$('.table-visible').css({overflow: 'inherit'});
	}).on('hide.bs.dropdown', function () {
		$('.table-visible').css({overflow: 'auto'});
	})
}


function addNewRullArg(DomainId, TemplateId, option) {
	var data = {
		Name: '',
		rule: '',
		Id: 0
	}
	$.extend(data, option || {})

	var id = new Date().getTime() + Math.round(Math.random() * 1000);
	var code = $([
		'<tr id="c' + id + '">',
		'<input type="hidden" class="set-id-val" name="RuleArgs[' + id + '].Id" value="' + data.Id + '" />',
		'<td><div class="form-group" style="margin-bottom: 0px;">',
		'<select class="form-control selectpicker method" name="RuleArgs[' + id + '].Method">',
		'<option value="2" selected>GET</option>',
		'<option value="1">POST</option>',
		'<option value="0">Любой</option>',
		'</select>',
		'</div></td>',
		'<td><div class="add-ads-input-light"><textarea rows="1" style="height: 24px; overflow: hidden; resize: none;" class="name Name" name="RuleArgs[' + id + '].Name"></textarea></div></td>',
		'<td><div class="add-ads-input-light"><textarea rows="1" style="height: 24px; overflow: hidden; resize: none;" class="name rule" name="RuleArgs[' + id + '].rule"></textarea></div></td>',
		'<td style="text-align: right;" class="table-products"><a class="btn delete"><i class="fa fa-trash-o"></i></a></td>',
		'</tr>'
	].join(''));

    var codeEmpty = $([
        '<tr id="c' + id + '">',
        '<td>' + (data.Method == 0 ? 'Любой' : data.Method == 1 ? 'POST' : 'GET') + '</td>',
        '<td class="name">' + (data.Name || '') + '</td>',
        '<td>' + (data.rule || '') + '</td>',
        '<td class="table-products btn-icons text-right"><a class="btn edit"><i class="fa fa-gear"></i></a></td>',
        '</tr>'
    ].join(''));

    if (data.Id != 0) codeEmpty.appendTo('#site-ruls-args');
    else {
        code.prependTo('#site-ruls-args');
        $(window).trigger('resize');
    }

    $('.edit', codeEmpty).on('click', function () {
        codeEmpty.after(code);
        codeEmpty.remove();
    })

	$('.delete', code).click(function () 
	{
		var newId = $('.set-id-val',code).val();
		if (newId > 0) {
			deleteRule(id, '/requests-filter/common/remove/rulearg', {DomainId:DomainId,TemplateId:TemplateId,id:newId});
		}
		else { $('#c' + id).remove(); }
	})

	$('.method', code).val(data.Method)
	$('.Name', code).val(data.Name)
	$('.rule', code).val(data.rule)

	
	$('.selectpicker', code).selectpicker('refresh');
	$('.selectpicker', code).parent().on('show.bs.dropdown', function () {
		$('.table-visible').css({overflow: 'inherit'});
	}).on('hide.bs.dropdown', function () {
		$('.table-visible').css({overflow: 'auto'});
	})
}


function addAccessIpToSite() {
	$.sl('load', '/requests-filter/access/open', { back: false, data: $('#form_AccessIP_to_site').serializeArray(), dataType: 'json' }, function (j) {
		if (j.msg) {
			$.sl('info', j.msg)
			
			if (j.result) {
				$('#modal_form_AccessIP_to_site').modal('hide');
			}
		}
		else $.sl('info', 'Неизвестная ошибка')
	})
}
	
	
	
function showCTRChar(name){
    $('#ctr_char_modal_content').empty();
    
    $.sl('load','/ajax/stat/show_ctr_char/'+name,{mode:'hide',dataType:'json'},function(data){
        var ctr = new CTRChar('#ctr_char_modal_content',data);
            ctr.height = 250;
            ctr.inProcent = false;
            //ctr.average = false;
            ctr.init();
    })
}
function getData(url,callback,jsonType){
    $.ajax({dataType: jsonType ? 'json' : false,url:url}).done(function(data) {
        callback(data);
    }).fail(function() {
        callback(jsonType ? {} : '')
    })
}
function switch_onoff(_this,table_name,row_name,id,callback){
    var status = parseInt($(_this).attr('status')),
        status = status ? 0 : 1;

    $(_this).attr('status',status);
        
    $.sl('load','/ajax/stat/switch_onoff/'+table_name+'/'+row_name+'/'+id+'/'+status,{mode:'hide'},function(){
        callback && callback(status);
    })
}
function switch_name(_this,id,name,status_name){
    switch_onoff(_this,name,status_name ? status_name : 'status',id,function(status){
        if(status){
            $(_this).removeClass('label-danger');
            $(_this).addClass('label-success');
        } 
        else{
            $(_this).addClass('label-danger');
            $(_this).removeClass('label-success');
        } 
    })
}
function switch_server(_this,id){
    switch_onoff(_this,'servers','status',id,function(status){
        $('span',_this).text(status ? 'остановить' : 'запусить');
        
        var bgElem = $('.server_id_'+id).find('.widget-bg-stat');
        
        if(status) bgElem.removeClass('bg-gray');
        else bgElem.addClass('bg-gray');
    })
}

function server_call(method,id){
    $.sl('load','/ajax/servers/server_'+method+'/'+id,{mode:'hide'});
}





function deleteRule(id, url,params){
	var btn = {'Да всегда':function(w){
		IsConfirmDeleteElement = false;
		
		deleteRuleToConfirm(id, url,params);
	}}
	
	if(IsConfirmDeleteElement){
		$.sl('_confirm', 'Вы действительно хотите выполнить это действие ?', { h: 70, error: 1, title: 'Информация', btn: btn  }, function (wn) {
			deleteRuleToConfirm(id, url,params);
		});
	}
	else{
		deleteRuleToConfirm(id, url,params);
	}

    return false;
}

function deleteRuleToConfirm(id, url,params){
	$.sl('load', url, { mode: 'hide', data: params, dataType: 'json' }, function (response) { 
		if (response.msg) $.sl('info', response.msg)
		else if (response.result) $('#c' + id).remove();
		else $.sl('info', 'Неизвестная ошибка') 
	});
}





function deleteElement(_this,url,params){
	var btn = {'Да всегда':function(w){
		IsConfirmDeleteElement = false;
		
		deleteElementToConfirm(_this,url,params);
	}}
	
	if(IsConfirmDeleteElement){
		$.sl('_confirm', 'Вы действительно хотите выполнить это действие ?', { h: 70, error: 1, title: 'Информация', btn: btn  }, function (wn) {
			deleteElementToConfirm(_this,url,params);
		});
	}
	else{
		deleteElementToConfirm(_this,url,params);
	}

    return false;
}

function deleteElementToConfirm(_this,url,params){
	$.sl('load', url, { mode: 'hide', data: params, dataType: 'json' }, function (response) { 
		if (response.msg) $.sl('info', response.msg)
		else if (response.result) $(_this).parents('.elemDelete').remove(); 
		else $.sl('info', 'Неизвестная ошибка') 
	});
}

function bunDomain(_this,id){
    getData('/ajax/domains/bun/'+id,function(data){
        $(_this).parent().removeClass('bun');
        
        if(data.status) $(_this).parent().addClass('bun');
    },true)
}

var serverLogsOpen;

function server_logs(server_id,click){
    if(click){
        serverLogsOpen = server_id;
        
        $(".dev-page-search-toggle").click();
    }
    
    getData('/ajax/servers/server_logs/'+server_id,function(data){
        
        if(data) $(".dev-search .dev-search-results").html(data);
        
        if(serverLogsOpen == server_id){
            setTimeout(function(){
                server_logs(server_id)
            },1000);
        }
    })
}

var idServerToDetete;

function realyDeleteServer(){
    $('.server_id_'+idServerToDetete).remove();
    
    $.sl('load','/ajax/servers/delete/'+idServerToDetete,{mode:'hide'});
}
function number_format(number, decimals, dec_point, thousands_sep) {
    number = (number + '').replace(/[^0-9+\-Ee.]/g, '');
    
    var n = !isFinite(+number) ? 0 : +number,
        prec = !isFinite(+decimals) ? 0 : Math.abs(decimals),
        sep = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep,
        dec = (typeof dec_point === 'undefined') ? '.' : dec_point,
        s = '',
        toFixedFix = function(n, prec) {
            var k = Math.pow(10, prec);
            return '' + (Math.round(n * k) / k).toFixed(prec);
        };
        
        s = (prec ? toFixedFix(n, prec) : '' + Math.round(n)).split('.');
        
        if (s[0].length > 3) {
            s[0] = s[0].replace(/\B(?=(?:\d{3})+(?!\d))/g, sep);
        }
        if ((s[1] || '').length < prec) {
            s[1] = s[1] || '';
            s[1] += new Array(prec - s[1].length + 1).join('0');
        }
        return s.join(dec);
}


function updateStatClassName(name,value){
    var elems = $('.'+name);
    
    elems.each(function(){
        var elem = $(this);
        
        if(elem.attr('progress-bar')){
            elem.css('width',number_format(value)+'%');
        }
        else if(elem.attr('status-button')){
            var status_true  = elem.attr('status-true'),
                status_false = elem.attr('status-false'),
                status       = parseInt(number_format(value));
                
            if(status){
                elem.removeClass(status_false);
                elem.addClass(status_true);
            }
            else{
                elem.removeClass(status_true);
                elem.addClass(status_false);
            }
            
            elem.attr('status',status);
        }
        else{
            var fom = number_format(value),
                str = value+'',
                spl = value ? (str.split('.').length > 1 ? 1 : 0) : 0,
                num = fom < 10 && spl ? (fom ? parseFloat(value).toFixed(3) : fom) : fom;
            
            elem.html(str.length > 10 ? str : num);
        }
    })
}
function updateStatWhile(data,name){
    if(data){
        for(var id in data){
            var ob = data[id];
            
            if(typeof ob == 'object') updateStatWhile(ob,name+'_'+id);
            else updateStatClassName(name+'_'+id,data[id]);
        }
    }
}
function updateStatSelect(json,name){
    if(json[name]){
        for(var id in json[name]){
            updateStatWhile(json[name][id],name+'_'+id);
        }
    }
}
function updateStatInDom(json){
    if(json.site_stat){
        for(var site_id in json.site_stat){
            updateStatWhile(json.site_stat[site_id],'site_stat_'+site_id);
            
            if(json.site_code_stat[site_id]){
                for(var code_id in json.site_code_stat[site_id]){
                    updateStatWhile(json.site_code_stat[site_id][code_id],'site_code_stat_'+site_id+'_'+code_id);
                }
            }
        }
    }
    
    updateStatSelect(json,'code');
    updateStatSelect(json,'site');
    updateStatSelect(json,'parthner');
    updateStatSelect(json,'code_stat');
    updateStatSelect(json,'parthner_stat');
    updateStatSelect(json,'server_stat');
    updateStatSelect(json,'code_option');
    updateStatSelect(json,'main');
    updateStatSelect(json,'adsids');
    
    dashboardChartsData['site'] = json.dashboard_chart_site;
    dashboardChartsData['code'] = json.dashboard_chart_code;
    dashboardChartsData['server'] = json.dashboard_chart_server;
    dashboardChartsData['parthner'] = json.dashboard_chart_parthner;
    dashboardChartsData['parthner_new_install'] = json.dashboard_chart_parthner_new_install;
    
    sparkLineStat = json.hours_stat;
    
    for(var id in json.parthner){
        dashboardChartsData['parthner_'+id] = json['dashboard_chart_parthner_'+id];
    }
    
    try{
        for(var id in json.rickshaw_chart) RickshawGraphData[id].update(json.rickshaw_chart[id]);
    }
    catch(e){}
    
    var trigerResize = false;
    
    $('.trigerResize').each(function(){
        var elem = $(this),
            oldHeight = elem.data('height'),
            height = elem.height();
        
        if(oldHeight !== height){
            elem.data('height',height);
            
            trigerResize = true;
        }
    })
    
    if(trigerResize) $(window).trigger('resize');
    
    if(updateStat){
        console.log('dush')
        for(var i = 0; i < dashboardChartUpdate.length; i++){
            dashboardChartUpdate[i]();
        }
    }
}

var windowFocus = true;


window.addEventListener("focus", function(event) {
    windowFocus = true;
}, false);
window.addEventListener("blur", function(event) { 
    windowFocus = false; 
}, false);



// jQuery SL Plugin
//
// Version 1.34
//
// Author Korner
// Sl SYSTEM
// 06 June 2012
//
// Visit http://sl-cms.com for more information
//
// Usage: $.sl('method',options)
//
// TERMS OF USE
// 
// This plugin is dual-licensed under the GNU General Public License and the MIT License and
// is copyright 2012 Sl SYSTEM, LLC. 

(function($){
    var cursor_point = {x:0,y:0};
    var window_size = {w:0,h:0};
    var m_o = {
            awc: '#all_window_load',
            cn: 'active'
        };
    var gop = {
        theme:'/'
    };
    var lang = [
        'Сохранить',
        'Загрузка',
        'Обновить',
        'Закрыть',
        'Ошибка в скрипте',
        'Всего ошибок',
        'Загрузка страницы',
        'Ошибка сервера',	
        'Да',
        'Вы действительно хотите удалить',
        'Удаление',
        'Не указан адрес запроса для удаления'
    ];
    
    var methods = {
        init : function() {
            
            $(document).on('mousemove',function(e){
                cursor_point.x = e.pageX;
                cursor_point.y = e.pageY;
            });
            
            window_size.w = window.innerWidth;
            window_size.h = window.innerHeight;
            
            $(window).resize(function() {
                
                window_size.w = window.innerWidth;
                window_size.h = window.innerHeight;
                
                var im = $(document).data('top_menu') || false;
                
                $('.shell_window,.shell_window .win_h_size,.shell_window .preload,.win_h_size_shell').each(function(){
                    $(this).css({
                        width: ($(this).hasClass('win_h_size_shell') ? false : window_size.w),
                        height: (im ? window_size.h - im : window_size.h) - $(this).closest('.shell_window').find('.shell_menu').height()
                    })
                    
                    if($(this).attr('minus')) $(this).height($(this).height() - $(this).attr('minus'));
                });
                
                $.sl('update_scroll');
            });
            
            $('.sl_checkbox').on('mousedown',function(){
                $(this).hasClass('active') ? $(this).removeClass('active').find('input').val(0) : $(this).addClass('active').find('input').val(1);
            });
            
            $(".sl_radio label").on('click',function(){
                var parent = $(this).closest('.sl_radio');
                $('label',parent).removeClass('selected');
                $(this).addClass('selected');
            });
            
            $('.bigedit').on('click',function(){
                var parent = $(this).closest('.sl_input,.sl_textarea'),btn = {};
                btn[lang[0]] = function(w,v){
                    $('input,textarea',parent).val(v[0]['value']);
                }
                $.sl('_area',{value:$('input,textarea',parent).val(),btn:btn});
            });
            
            $('.sl_slide ul.title li').on('click',function(){
                var parent = $(this).closest('.sl_slide');
                var p = $(this).attr('rel') * 100;
                $('.title li',parent).removeClass('active');
                $(this).addClass('active');
                $('.page',parent).css({'-webkit-transform': 'translateX(-'+p+'%)','-moz-transform': 'translateX(-'+p+'%)','-o-transform': 'translateX(-'+p+'%)','transform': 'translateX(-'+p+'%)'});
            })
            
            $(document).mousedown(function(){
                $('#sl_select_box').fadeOut(function(){
                    $(this).remove();
                })
            })
            
            $('table tr.sl_add_row').on('click',function(){
                var call = $(this).attr('callback'),
                    url = $(this).attr('url'),
                    top = $(this).attr('top'),
                    _this = $(this),
                    parent = $(this).closest('table'),
                    scroll_p = $(this).closest('.scrollbarInit');
                
                function bi(data,c){
                    top ? parent.find('tr:eq('+top+')').before(data) : _this.before(data);
                    scroll_p.length && scroll_p.tinyscrollbar_update('bottom');
                    c && c();
                }
                
                if(call){
                    if(url){
                        $.sl('load','/ajax/'+call,function(d){
                           bi(d);
                        },{mode:'quiet'});
                    }
                    else window[call](function(data,callback){
                        bi(data,function(){
                            callback && callback();
                        })
                    },_this);
                }
            })
            
            $(".sl_select ._display").on('click',function(){
                var parent = $(this).closest('.sl_select'),
                    data   = $('._data ul',parent).html(),
                    w      = parent.outerWidth(),
                    h      = parent.outerHeight(),
                    of     = parent.offset(),
                    wh     = $(window).height(),x,y,bh,bh2,overhide,tp,g = 30,v = '',n = '';
                    
                $box = $([
                    '<div id="sl_select_box">',
                        '<div class="scroll"><ul>',
                            data,
                        '</ul></div>',
                    '</div>'
                ].join(''));
                
                bh = $box.hide().appendTo('body').fadeIn().find('ul').height();
                
                x = of.left;
                y = of.top;
                
                wh += $(document).scrollTop();
                
                bh  = bh+g > wh ? wh-g : bh;
                bh2 = bh/2+15;
                
                $box.find('div.scroll').height(bh);
                
                tp = (bh2 > y) ? 15 : y - bh2 + (h/2) + 15;
                
                tp = (y+bh2 > wh) ? y - bh2-(y+bh2 - wh)+ (h/2): tp;
                
                $box.css({left:x,top:tp,width:w,height:overhide}).find('.scroll').tinyscrollbar();
                
                $box.find('li').click(function(){
                    v = $(this).attr('val');
                    n = $(this).attr('name');
                    $('._data li',parent).removeClass('selected');
                    $('._data li[val='+v+']',parent).eq(0).addClass('selected');
                    $('._display',parent).html(n);
                    $('input',parent).val(v);
                });
            });
            
            
            $('table.sortable').on('mouseenter', function() {
                $(this).die('mouseenter').sortable({
                    distance:15,
                    items:'tr.row_2,tr.row_1', 
                    opacity:0.6,
                    start:function(event,ui){
                        ui.item.addClass('hideTd');
                    },
                    stop:function(event,ui){
                        ui.item.removeClass('hideTd');
                        $.sl('sort_table');
                    },
                    axis:'y'
                });
            });
            
            
            return this;
        },
        /**
         * Lang Translate
         */
        lang: function(fn,op,st){
            
            var o = '',f,_this,obj;
            this == false ? _this = false : _this = $(this);
            
            o = _this ? _this.text().substr(0,500) : '';

            $.sl('type',[fn,op,st],'function',function(is,n){
                f = is[n];
            })
            
            $.sl('type',[fn,op,st],'string',function(is,n){
                is[n] !== '' && (o =  is[n]);
            })
            
            $.sl('type',[fn,op,st],'object',function(is,n){
                o = is[n];
            })
            
            $.sl('load','/ajax/fn/lang',{data:{lang:JSON.stringify(o)},mode:'hide',ignore:true},function(j){
                if(typeof o == 'object') obj = jQuery.parseJSON(j);
                f && f(obj ? obj : j);
                _this && !obj && _this.text(j);
            });
            
            return this == false ? false : this;
        },
        options: function(options){
            if(typeof options == 'string') return gop[options]; 
            else gop = jQuery.extend(gop, options);
        },
        /**
         * Update Scroll
         */
        update_scroll : function() {
            $('.scrollbarInit').length && $('.scrollbarInit').tinyscrollbar_update('relative');
        },
        /**
         * Resize
         */
        resize : function(fn,op,st) {
            var o = [],f,s,ah = 0,i=0,w=0;
            
            $.sl('type',[fn,op,st],'object',function(is,n){
                o = is[n];
            })
            
            $.sl('type',[fn,op,st],'function',function(is,n){
                f = is[n];
            })
            
            $.sl('type',[fn,op,st],'string',function(is,n){
                is[n] !== '' && (s =  is[n]);
            })
            
            function stin(){
                ah = 0,h = window.innerHeight,w = window.innerWidth;
                for(i = 0; i < o.length; i++) ah += $(o[i]).outerHeight();
                s && $(s).height(h-ah);
                f && f(h,h-ah,w);
            }
            
            $(window).resize(function() {
                stin();
            });
            
            stin();
            
            return this == false ? false : this;
        },
        /**
         * Sort Table
         */
        sort_table : function() {
            $('table tr:even').not('tr.header,tr.sl_add_row').removeClass('row_2').addClass('row_1');
            $('table tr:odd').not('tr.header,tr.sl_add_row').removeClass('row_1').addClass('row_2');
            return this == false ? false : this;
        },
        /**
         * Scroll menu
         */
        scroll_menu : function(fn,op,st) {
            if(this == false) return;
            
            var o = {
                menu: [],
                load: false,
            },f,s,btn = '',btns;
            
            $.sl('type',[fn,op,st],'object',function(is,n){
                o = $.extend(o, is[n]);
            })
            
            $.sl('type',[fn,op,st],'function',function(is,n){
                f = is[n];
            })
            
            $.sl('type',[fn,op,st],'string',function(is,n){
                is[n] !== '' && (s =  is[n]);
            })
            $.sl('type',[fn,op,st],'number',function(is,n){
                is[n] !== '' && (s =  is[n]);
            })
            
            var _this = $(this),
                w      = _this.outerWidth(),
                h      = _this.outerHeight(),
                of     = _this.offset(),
                wh     = $(window).height(),x,y,bh,bh2,overhide,tp,g = 30,wq = w,ib = 0;
            
            function bin(menu){
                menu && (o.menu = menu);
                
                $.each(o.menu,function(k,v){ btn += '<li'+(k == s ? ' class="selected"' : '')+'><span>'+v+'</span></li>'; });
                    
                $box = $([
                    '<div id="sl_select_box">',
                        '<div class="scroll"><ul>',
                            btn,
                        '</ul></div>',
                    '</div>'
                ].join(''));
                
                bh = $box.hide().appendTo('body').fadeIn().find('ul').height();
                
                w = w < 100 ? 100 : w;
                x = of.left - (w/2) + (wq/2);
                y = of.top;
                
                wh += $(document).scrollTop();
                
                bh  = bh+g > wh ? wh-g : bh;
                bh2 = bh/2+15;
                
                $box.find('div.scroll').height(bh);
                
                tp = (bh2 > y) ? 15 : y - bh2 + (h/2) + 15;
                
                tp = (y+bh2 > wh) ? y - bh2-(y+bh2 - wh)+ (h/2): tp;
                
                $box.css({left:x,top:tp,width:w}).find('.scroll').tinyscrollbar();
                
                btns = $box.find('li');
                
                $.each(o.menu,function(nb,obj){
        			btns.eq(ib++).click(function(){
                        if(o.module) $.sl('load',o.module[0],{mode:(o.module[1] || false),data:{0:nb,1:obj}},function(data){
                            f && f.apply(_this,[nb,obj,data]);
                        })
                        else f && f.apply(_this,[nb,obj]);
        			});
        		});
            }
            
            if(o.load){
                $(this).sl('load',(typeof o.load == 'object' ? o.load[0] : o.load),{mode:(typeof o.load == 'object' ? o.load[1] : 'content'),dataType:'json',back:false},function(menu){
                    bin(menu);
                })
            }
            else bin();
        },
        /**
         * BIG Select
         */
        big_select : function(fn,op,st) {
            var o = {
                menu: [],
                load: false,
            },f,s,t,btn = '',btns;
            
            $.sl('type',[fn,op,st],'object',function(is,n){
                o = $.extend(o, is[n]);
            })
            
            $.sl('type',[fn,op,st],'function',function(is,n){
                f = is[n];
            })
            
            $.sl('type',[fn,op,st],'string',function(is,n){
                is[n] !== '' && (t =  is[n]);
            })
            $.sl('type',[fn,op,st],'number',function(is,n){
                is[n] !== '' && (s =  is[n]);
            })

            var wh = window_size.h,ww = window_size.w,w,x,y,bh,g = 30,nh = 58,ib = 0;
            
            function bin(menu){
                menu && (o.menu = menu);
                
                $.each(o.menu,function(k,vob){ btn += '<li'+(k == s ? ' class="selected"' : '')+'><h2>'+vob[0]+'</h2><p>'+vob[1]+'</p></li>'; });
                    
                $box = $([
                    '<div id="sl_big_select_box">',
                        '<div class="boxConteiner"><h3><span>',t,'</span></h3>',
                        '<div class="scroll"><ul>',
                            btn,
                        '</ul></div>',
                        '</div>',
                    '</div>'
                ].join(''));
                
                bh = $box.click(function(){ $(this).fadeOut(function(){ $(this).remove() }) }).hide().appendTo('body').fadeIn().find('ul').height();
                
                w = $('.boxConteiner',$box).width();
                w = w < 300 ? 300 : w;
                x = (ww/2) - (w/2);
                
                bh  = bh+g+nh > wh ? wh-g-nh : bh;
                
                $('div.scroll',$box).height(bh);
                
                y = (wh/2)-((bh+nh)/2);
                
                $('.boxConteiner',$box).css({left:x,top:y}).find('.scroll').tinyscrollbar();
                
                btns = $box.find('li');
                
                $.each(o.menu,function(nb,obj){
        			btns.eq(ib++).click(function(){
                        if(o.module) $.sl('load',o.module[0],{mode:(o.module[1] || false),data:{0:nb}},function(data){
                            f && f(nb,data);
                        })
                        else f && f(nb);
        			});
        		});
            }
            
            if(o.load){
                $(this == false ? false : this).sl('load',(typeof o.load == 'object' ? o.load[0] : o.load),{mode:(typeof o.load == 'object' ? o.load[1] : 'content'),dataType:'json',back:false},function(menu){
                    bin(menu);
                })
            }
            else bin();
        },
        /**
         * Small Menu
         */
        menu : function(btn,options) {
            
            var btn = jQuery.extend({}, btn);
            var o = jQuery.extend({
                id: 'sl_menu',
                parent: 'body',
                width: 120,
                offset: 10,
                position: 'auto', // or cursor
                conteiner: document
            }, options);
            
            var ww,wh,r_m = '',mw,mh,l = 0,t = 0,cnt = 0,callb,w,h,_this = this;
            
            o.position == 'cursor' ? (p = {left:cursor_point.x,top:cursor_point.y},w = 0,h = 0) : (p = $(this).offset(),w = $(this).outerWidth(),h = $(this).outerHeight());
            
            ww = $(o.conteiner).width();
            wh = $(o.conteiner).height();
            
            $.each(btn,function(k,v){
                r_m = r_m + '<li'+(typeof v == 'string' ? ' onclick="'+v+'"' : '')+'>'+k+'</li>',cnt += 1;
            })
            
            if(cnt == 0) return this;
            
            if($('#'+o.id).length) $('#'+o.id).fadeOut(function(){ $(this).remove() });
            
            $men = $([
                '<div id="',o.id,'"',(o.zIndex ? ' style="z-index: ' + o.zIndex + '"' : ''),'>',
                    '<div class="m_con" style="width: ',o.width,'px;">',
                        '<ul>',
                            ,r_m,
                        '</ul>',
                        '<div class="l"></div>',
                        '<div class="t"></div>',
                        '<div class="r"></div>',
                        '<div class="b"></div>',
                    '</div>',
                '</div>'
            ].join(''));
            
            $men.hide().appendTo(o.parent).fadeIn();
            
            callb = $('ul li',$men),cnt = 0;
            
            $.each(btn,function(c,v){
                if(typeof v == 'function'){
                    callb.eq(cnt++).click(function(){
                        v && v.apply(_this);
        				return false;
        			});
                }
                else cnt++;
            });
            
            mw = $men.width();
            mh = $men.height();
            
            l = o.position == 'cursor' ? p.left : p.left + (w/2) - (mw/2);
            
            if(p.top + (h/2) + (mh/2) > wh){
                t  = p.top - mh - o.offset,show  = 'b';
            }
            else if(p.top + (h/2) - (mh/2) < 0){
                t = p.top + h + o.offset,show  = 't';
            }
            else{
                if(p.left + w + o.width + o.offset > ww) l = p.left - mw - o.offset,show  = 'r';
                else l = p.left + w + o.offset,show  = 'l';
                t  = p.top + (h/2) - (mh/2);
            }
            
            $men.css({left:l,top:t}).find('.'+show).show();
            
            !$(document).data('menu') && $(document).on('mousedown',function(){ $men.fadeOut(function(){ $(this).remove(); }); }).data('menu',true);
            
            return this;
        },
        /**
         * Tip
         */
        tip : function(options) {
            
            if(this == false)  return;
            
            var on = o = jQuery.extend({
                offset: 10,
                s: false
            }, options);
            
            if(!$(document).data('tip')){
                
                $tip = $([
                    '<div id="tip">',
                        '<div class="m_con">',
                            '<div id="tip_con"></div>',
                            '<div class="l"></div>',
                            '<div class="t"></div>',
                            '<div class="r"></div>',
                            '<div class="b"></div>',
                        '</div>',
                    '</div>'
                ].join(''));
                
                $tip.hide().appendTo('body');
            
                $(this).on('mouseover mouseout',function(event){
                    event.type == 'mouseover' ? (on.s = 'show',$(this).sl('tip',on)) : (on.s = 'hide',$(this).sl('tip',on));
                });
                
                $(document).data('tip',true);
            }
            
            if(!o.s)  return false;
            
            var ww,wh,r_d = '',mw,mh,l = 0,t = 0,$tip = $('#tip'),p,show;
            
            if(o.s == 'hide') { $tip.stop().fadeOut(100); return false; }
            
            p = $(this).offset(),w = $(this).outerWidth(),h = $(this).outerHeight();
                
            ww = $(document).width();
            wh = $(document).height();
            
            $('#tip_con').html($(this).attr('tip')),$tip.find('.l,.t,.r,.b').hide();
            
            mw = $tip.width();
            mh = $tip.height();
            
            l = p.left + (w/2) - (mw/2);
            t  = p.top - mh - o.offset;
            
            if(p.top - (mh/2) - o.offset < 0){
                t = p.top + h + o.offset,show  = 't';
            }
            else{
                if(p.left + w + mw + o.offset > ww) l = p.left - mw - o.offset,show  = 'r',t = p.top + (h/2) - (mh/2);
                else if(p.left + (w/2) - w + o.offset < 0) l = p.left + w + o.offset,show  = 'l',t = p.top + (h/2) - (mh/2);
                else show  = 'b';
            }
            
            o.s == 'show' ? $tip.stop().css({left:l,top:t-1,opacity:1}).fadeIn(100).find('.'+show).show() : $tip.stop().fadeOut(100);
            
            return this;
        },
        /**
         * Top Panel
         */
        top_panel: function(options){
            var o = jQuery.extend({
                id: 'top_panel',
                login: "korner", 
                fun: "$.sl('shell',{name: 'admin_menu'})",
                logout: "$.sl('load','/ajax/auth/logout',function(){ window.location = document.URL })"
            }, options);
            
            $tpm = $([
                '<div id="',o.id,'">',
                    '<div class="fn_panel" onclick="',o.fun,'"></div>',
                    '<div class="fn_window"><ul id="all_window_load"></ul></div>',
                    '<div class="fn_logout" onclick="',o.logout,'"></div>',
                    '<div class="fn_login">',o.login,'</div>',
                '</div>'
            ].join(''));  
            
            $('body #'+o.id).length && $('body #'+o.id).remove();
                  
            $tpm.appendTo(this == false ? 'body' : this).animate({opacity:1,top:0},700);
            
            $(document).data('top_menu',$tpm.height());
            
            return this;
        },
        /**
         * Preload
         */
        preload: function(){
            if(this == false) return false;
            
            var img = $(this).find('img'),
                src = '',i;
            
            var sec = function(_this){
                $(_this).delay(500).animate({opacity:1},function(){
                    $(this).unwrap();
                })
            }
            
            var onl = function(_this,src){
                i = new Image(); 
                i.onload = function(){
                    sec(_this);
                }
                i.onerror = function(){
                    sec(_this);
                }
                i.src = src; 
            }
            
            $.each(img,function(){
                src = $(this).attr('src');
                
                $(this).wrap('<div class="load_img"/>').css({opacity:0});
                
                onl($(this),src);
            })
        }
        ,
        /**
         * Window
         */
        window: function(options,fn_call){
            var o = jQuery.extend({
                w: 300,
                h: 140,
                name: 'default',
                status: 'none',
                panel_height: 32,
                title: 'Window',
                resize: false,
                containment: '#wrap',
                drag: false,
                data: '',
                size: false,
                btn: {},
                autoclose: true,
                bg: true,
                error: false,
                preload: true,
                scroll:0
            }, options);
            
            var win_all = '.sl_window,.shell_window',w_s = $('#'+o.name),ww2,wh2,cor = [100,70],pf,slaw,wt = 0,ws = 0,btn = '',ib = 0,dn,
                w2 = window_size.w/2,
                h2 = window_size.h/2,
                scTp = $(document).scrollTop(),
                sSi = o.status,
                me = ['none','show','hide','show_hide','close','backsize','fullsize','resize','index','data'];
                //     0      1      2      3           4       5          6          7        8       9
            
            o.w = o.w < 100 ? 100 : o.w > window_size.w ? window_size.w : o.w;
            o.h = o.h < 60 ? 60 : o.h > window_size.h ? window_size.h : o.h;
            
            o.status = $.inArray(o.status, me), o.status > 0 ? o.status = me[o.status] : o.status = me[0];
            
            w_s.length && (wt = 1);
            wt && w_s.is(':visible') && (ws = 1)
            
            if(!wt && o.status == 'close'){
                fn_call && fn_call();
                return;
            } 
            
            !wt && (o.status = me[0]);
            
            if(wt && o.status == me[9]){
                w_s.find('.win_data .overview').html(o.data);
                w_s.find('.scrollbarInit').tinyscrollbar_update(o.scroll == 0 ? 'relative' : o.scroll == 1 ? 'top' : 'bottom');
                o.preload && $(w_s).find('.win_data').sl('preload');
                return this == false ? false : this;
            } 
            
            wt && ws && !w_s.hasClass('window_stack') && (o.status == me[3] || o.status == me[1] || o.status == me[2]) && (o.status = me[8]);
            
            o.status == me[3] && ( o.status = ws ? o.status = me[2] : o.status = me[1] );
            
            wt && o.status == me[0] && ( o.status = me[1] );
            
            dn = wt ? w_s.data('name') : o.name;
            
            o.status !==  me[0] ? (o.w = w_s.data('size').width,o.h = w_s.data('size').height) : o.status ;
            
            o.status ==  me[7] && (w_s.hasClass(me[6]) ? o.status = me[5] : o.status = me[6]);
            o.status ==  me[1] && (w_s.hasClass(me[6]) ? o.status = me[6] : o.status = me[5]);

            ww2 = o.w / 2;
            wh2 = o.h / 2;
            
            o.status ==  me[8] && w_a_h();
            
            pf = {
                wm:o.w-cor[0],
                hm:o.h-cor[1],
                cw: (cor[0] / 2),
                ch: (cor[1] / 2),
            }
            
            cssStart = {
                width:pf.wm,
                height:pf.hm,
                left:w2-ww2+pf.cw,
                top:h2-wh2+pf.ch+scTp,
                opacity:0
            }
                
            function w_a(obj){
                if(sSi == me[6]){
                    obj.css(cssStart);
                    w_fullSize(true);
                    return;
                }
                obj.css(cssStart).animate({
                    width:o.w,
                    height:o.h,
                    left:w2-ww2,
                    top:h2-wh2+scTp,
                    opacity:1
                },300,function(){
                    w_s_c(obj,o.h);
                    obj.find('.scrollbarInit').tinyscrollbar();
                    fn_call && fn_call();
                });
            }
            
            function w_s_c(obj,h,callback){
                obj.show().find('.win_h_size').css({height:h-o.panel_height});
                obj.find('.win_conteiner').fadeIn(100,function(){
                    Cufon.replace(".smooth");
                    callback && callback();
                });
                
                $.each($('.win_h_size,.win_h_size_shell',obj),function(){
                    if($(this).attr('minus')) $(this).height($(this).height() - $(this).attr('minus'));
                })
            }
            function w_h_c(obj,callback){
                obj.show().find('.win_conteiner').fadeOut(100,function(){
                    callback && callback();
                });
            }
            function w_a_h(hi){
                $.sl('win_tab',dn,{prefix:'win',title:o.title,status:(hi && 'hide'),select:'#'+dn,fun:'$.sl(\'window\',{status:\'show_hide\',name:\''+dn+'\'})'});
            }
            function w_h_s(obj,h){
                obj.find('.win_h_size').css({height:h-o.panel_height});
            }
            function w_fullSize(is_new){
                ifmenu = $(document).data('top_menu') || false;
                
                w_h_c(w_s,function(){
                    w_s.resizable("disable").animate({
                        width:window_size.w,
                        height:ifmenu ? window_size.h - ifmenu : window_size.h,
                        left:0,
                        top:ifmenu ? ifmenu : 0,
                        opacity:1
                    },300,function(){
                        w_s.removeClass('border glow');
                        w_s_c(w_s,ifmenu ? window_size.h - ifmenu : window_size.h);
                        is_new ? w_s.find('.scrollbarInit').tinyscrollbar() : w_s.find('.scrollbarInit').tinyscrollbar_update('relative');
                        fn_call && fn_call();
                    }).addClass(o.status);
                });
                w_s.data('size',{width:o.w,height:o.h});
                is_new && w_s.addClass('fullsize');
            }
            
            if(!w_s.length){
                
                o.btn[lang[3]] = function(){
                    $.sl('window',{status:'close',name:dn})
                }
                
                $.each(o.btn,function(name,fn){ btn += '<div class="win_btn">'+name+'</div>' });
    
                $win = $([
                    '<div id="',o.name,'" class="sl_window window_layer',(o.error ? ' window_error' : ''),' glow border">',
                        '<div class="win_conteiner" style="display:none">',
                            '<div class="win_panel"',(o.size && ' ondblclick="$.sl(\'window\',{status:\'resize\',name:\''+o.name+'\'})"'),'>',
                                '<div class="t_p_r t_h_i">',
                                    '<div class="title">',o.title,'</div>',
                                    '<div class="win_buttons">',btn,'</div>',
                                '</div>',
                            '</div>',
                            '<div class="win_h_size">',
                                '<div class="win_data win_h_size scrollbarInit">',(this == false ? o.data : $(this).html()),'</div>',
                            '</div>',
                        '</div>',
                    '</div>'
                ].join(''));
                
                o.containment = $(o.containment).length ? o.containment : 'body';
                
                $win.appendTo(o.containment).data('position',{left:w2-ww2,top:h2-wh2}).data('size',{width:o.w,height:o.h}).data('name',dn);
                
                if(o.bg) {
                    $bg = $('<div class="win_bg window_layer w_b_win_'+dn+'"></div>').hide().fadeIn();
                    $('#'+o.name).before($bg);
                }
                
                w_s = $win,w_a($win),w_a_h();
                o.preload && $(w_s).find('.win_data').sl('preload');
                
                if(o.drag){
                    $win.draggable({
                        containment: 'parent',
                        opacity:'0.7',
                        handle: '.win_panel',
                        stop: function(event, ui){
                            $(this).data('position',{left:ui.offset.left,top:ui.offset.top});
                        }
                    })
                }
                if(o.resize){
                    $win.resizable({
                        containment: 'parent',
                        minWidth: 100,
                        minHeight: 70,
                        resize: function(event, ui){
                            w_h_s($(this),ui.size.height);
                            $(this).find('.scrollbarInit').tinyscrollbar_update('relative');
                        },
                        stop: function(event, ui){
                            $(this).data('size',{width:ui.size.width,height:ui.size.height});
                        }
                    });
                }
                $win.bind('mousedown',function(){
                    w_a_h();
                });
                
                btns = $win.find('.win_panel .win_btn');

        		$.each(o.btn,function(nb,obj){
        		  
        			btns.eq(ib++).click(function(){
                        obj && obj.apply( this,[dn]);
                        o.autoclose && $.sl('window',{status:'close',name:dn});
        				return false;
        			});
        		});
                
                
            }
            else if(o.status == me[4] || o.status == me[2]){
                $('.w_b_win_'+dn).fadeOut(function(){
                    if(o.status == me[4]) $(this).remove();
                });
                
                w_h_c(w_s,function(){
                    
                    w_s.animate({
                        width:pf.wm,
                        height:pf.hm,
                        left:w_s.data('position').left + pf.cw,
                        top:w_s.data('position').top + pf.ch + scTp,
                        opacity: 0
                    },300,function(){
                        if(o.status == me[4]){
                            $(this).remove();
                            $.sl('win_tab',dn,{status:'close',prefix:'win'});
                        } 
                        else{
                            $(this).hide();
                            
                            !$(document).data('top_menu') && $.sl('top_panel');
                            
                            w_a_h(true);
                        }
                        
                        fn_call && fn_call();
                    });
                })
            }
            else if(o.status == me[5]){
                $('.w_b_win_'+dn).fadeIn();
                
                w_h_c(w_s,function(){
                    w_s.resizable("enable").addClass('border glow').animate({
                        width:o.w,
                        height:o.h,
                        left:w_s.data('position').left || w2 - ww2,
                        top:w_s.data('position').top || h2 - wh2,
                        opacity:1
                    },300,function(){
                        w_s_c(w_s,o.h),w_a_h();
                        w_s.find('.scrollbarInit').tinyscrollbar_update('relative');
                        fn_call && fn_call();
                    }).removeClass(me[6]);
                })
            }
            else if(o.status == me[6]){
                w_fullSize();
            }
            

        },
        /**
         * Info
         */
        info : function(string) {
            
            if(!$('#sl_info').length){
                var $info = $([
                    '<div id="sl_info" onclick="$(this).fadeOut()">',
                        '<div class="info_bg"></div>',
                        '<div class="info_text"><div></div></div>',
                    '</div>'
        	    ].join(''));
        
                $info.hide().appendTo('body');
            }
            else $info = $('#sl_info');
    
    
            function sh(){
                $info.fadeIn(300).find('.info_bg').hide().css({opacity:1}).delay(300).fadeIn(300).animate({opacity:0.5},400);
                $info.find('.info_text > div').hide().delay(400).fadeIn(100).html(string);
            }
            
            function sr(){
                $info.find('.info_bg').animate({opacity:1},200).animate({opacity:0.5},400);
                $info.find('.info_text > div').html(string);
            }
            
            $info.is(':visible') ? sr() : sh();
        },
        /**
         * Win Tab
         */
        win_tab: function(name,options){
            var o = $.extend({
                status: 'active',
                addclass: '',
                fun: '',
                prefix: 'win',
                title: 'window',
                select: '.win_default',
                ico: false
            }, options);
            
            var m_o = {
                awc: '#all_window_load',
                cn: 'active',
                alw: '.window_layer',
                ws: 'window_stack'
            };
            var slaw = $(m_o.awc);
            
            if(name == 'hide_all'){
                slaw.find('li').removeClass('active');
            }
            else if(name == 'last'){
                eval(slaw.find('li:last').addClass('active').attr('onclick'));
            }
            else if(o.status == 'active' || o.status == 'hide'){
                slaw.length && slaw.find('li').removeClass(m_o.cn).closest('ul').find('li[name='+o.prefix+'_'+name+']').addClass(o.status == 'active' &&  m_o.cn);

                !slaw.find('li[name='+o.prefix+'_'+name+']').length && $('<li name="'+o.prefix+'_'+name+'" class="active '+o.addclass+'" onclick="'+o.fun+'"><span>'+(o.ico ? '<img src="'+o.ico+'" />' : '')+o.title+'</span></li>').appendTo(slaw);
                
                if(o.status == 'active'){
                    $(m_o.alw).removeClass(m_o.ws);
                    $(o.select).addClass(m_o.ws);
                    $('.w_b_'+o.prefix+'_'+name).addClass(m_o.ws);
                }
            }
            else if(o.status == 'close'){
                slaw.find('li[name='+o.prefix+'_'+name+']').css({width:0});
                setTimeout(function(){
                    slaw.find('li[name='+o.prefix+'_'+name+']').remove();
                },500)
            } 
        },
        /**
         * Shell
         */
        shell: function(fn,op,st){
            var o = {
                name: 'default',
                title: lang[1],
                add_param: '',
                post: false,
                method: false
            },f = function(){};
            
            $.sl('type',[fn,op,st],'object',function(is,n){
                o = $.extend(o, is[n]);
            })
            
            $.sl('type',[fn,op,st],'function',function(is,n){
                f = is[n];
            })
            
            $.sl('type',[fn,op,st],'string',function(is,n){
                is[n] !== '' && (st =  is[n]);
            })
            
            var elem = $('#shell_'+o.name),
                sh = false,
                im,dn,ws,css;
                
            elem.length && (sh = elem);
            
            sh && sh.is(':visible') && (ws = 1);
            
            st == 'hide_all' || st == 'hide_all_not' && $.sl('win_tab','hide_all');
                
            if(st == 'hide_all'){
                $('.shell_window').hide(),f();
                return;
            }
            
            if(st == 'hide_all_not'){
                $('.shell_window').not(sh).hide();
                $.sl('shell',{name:o.name},function(){
                    f();
                });
                return;
            }
            
            im = $(document).data('top_menu') || false;
            dn = sh ? sh.data('name') : o.name;
            
            function w_a_h(hi,ico){
                $.sl('win_tab',dn,{prefix:'shell',status:(hi && 'hide'),title:dn,select:'#shell_'+dn,addclass:'shell',fun:'$.sl(\'shell\',\'hide_all_not\',{name:\''+dn+'\'})',ico:ico});
            }
            
            function l_c(elem,obj){
                var dt = elem.data('jn')['shell'] || 'show';
                    dt = o.method ? o.method : dt;
                if(obj) $(obj).sl('load','/ajax/'+o.name+'/'+dt+'/'+o.add_param,{data:o.post},function(d){
                   elem.find('.win_data').html(d),h_p_l(elem);
                   Cufon.replace(".smooth");
                   f();
                },{back:false,error:function(){
                    $.sl('shell',{name:o.name},'close');
                }});
                else $.sl('load','/ajax/'+o.name+'/'+dt+'/'+o.add_param,{data:o.post},function(d){
                   elem.find('.win_data').html(d),h_p_l(elem);
                   Cufon.replace(".smooth");
                   f();
                },{error:function(){
                    $.sl('shell',{name:o.name},'close');
                }});
            }
            
            function h_p_l(obj){
                i_s(obj);
                var ch_sh = obj.find('.shell_iframe');
                var ch_pr = obj.find('.preload');
                if(ch_sh.length){
                    ch_sh.load(function(){
                        ch_pr.fadeOut(300);
                    })
                }
                else{
                    obj.find('.win_data').removeClass('scrollbarContent').tinyscrollbar().sl('preload');
                    obj.find('.scrollbarInit').tinyscrollbar();
                    ch_pr.fadeOut(300);
                }
            }
            
            function i_s(obj){
                
                obj.find('.win_data,.win_h_size,.shell_iframe').css({
                    width:window_size.w,
                    height:(im ? window_size.h - im : window_size.h) - $('.shell_menu',obj).height()
                });
                
                obj.find('.win_h_size_shell').height((im ? window_size.h - im : window_size.h) - $('.shell_menu',obj).height());
                
                $.each($('.win_h_size,.win_h_size_shell',obj),function(){
                    if($(this).attr('minus')) $(this).height($(this).height() - $(this).attr('minus'));
                })
            }
            
            if(!sh){
                $.sl('load','/ajax/fn/infomod/'+o.name,{dataType:'json'},function(nq){
                    
                    $sh = $([
                        '<div id="shell_',dn,'" class="shell_window window_layer window_stack">',
                            '<div class="win_h_size">',
                                '<div class="win_data win_h_size scrollbarInit ',(nq.style == 'aero' ? 'aero_style_bg' : ''),'"></div>',
                            '</div>',
                            '<div class="preload ',(nq.style == 'aero' ? 'aero_preload' : ''),'"></div>',
                            '<div class="shell_menu ',(nq.style == 'aero' ? 'aero_style_top aero_shadow' : ''),'">',
                                '<div class="pad"><div class="t_left"><span class="bold">',o.name,'</span> - <span class="title">',nq.title,'</span><br /><span>'+nq.style+'</span></div>',
                                '<div class="t_right"><div class="btn" onclick="$(this).sl(\'shell\',{name:\'',o.name,'\'},\'update\')">',lang[2],'</div><div class="btn" onclick="$.sl(\'shell\',{name:\'',o.name,'\'},\'close\')">',lang[3],'</div></div>',
                                '</div>',
                            '</div>',
                        '</div>'
                    ].join(''));
                    
                    nq['shell'] = o.action && o.action !== '' ? o.action : nq['shell'];
                    nq['ico_img'] = nq['ico_img'] ? '/modules/'+o.name+'/ico.png' : false;
                    
                    css = {
                        width:window_size.w,
                        height:im ? window_size.h - im : window_size.h,
                        top:im ? im : 0
                    }
                    
                    $sh.hide().appendTo('body').data('name',o.name).data('jn',nq).css(css).fadeIn(300,function(){
                        w_a_h(true,nq['ico_img']);
                    });
                    $sh.find('.preload').css(css).show();
                    
                    $sh.bind('mousedown',function(){
                        w_a_h();
                    });
                    
                    this == false ? l_c($sh) : l_c($sh,this);
                })
                
            }
            else if(st == 'close'){
                sh.fadeOut(300,function(){
                    sh.remove(),$.sl('win_tab',dn,{status:'close',prefix:'shell'}),f();
                })
            }
            else if(st == 'update'){
                sh.find('.preload').fadeIn(200);
                this == false ? l_c(sh) : l_c(sh,this);
            }
            else if(st == 'hide'){
                 sh.fadeOut(),w_a_h(true),f();
            }
            else if(sh && ws && sh.hasClass('window_stack')){
                sh.fadeOut(),w_a_h(true);
            }
            else if(!ws){
                sh.fadeIn(),w_a_h();
            }
            else{
                w_a_h();
            }
        },
        /**
         * Type
         */
        type: function(agrs,tp,call){
            agrs = $.extend({}, agrs);
            
            $.each(agrs,function(k,v){
                typeof v == tp && (call && call(agrs,k));
            })
        },
        /**
         * Count Array
         */
        count: function(array){
            var cnt=0;
        	for (var i in array) { if (i) cnt++ }
        	return cnt;
        }
        ,
        /**
         * Loading
         */
        loading: function(fn,op,st){
            var f=function(){},o,ob,obo = {p:[],w:0,h:0},s = 'show',q = {},c = 0;
            
            this == false ? ob = false : ob = $(this);
            
            o = {
                zIndex: 'auto',
                mode: 'content',
                name: 'default',
                tip: lang[1]
            };
            
            $.sl('type',[fn,op,st],'object',function(is,n){
                o = $.extend(o, is[n]);
            })
            
            $.sl('type',[fn,op,st],'function',function(is,n){
                f = is[n];
            })
            
            $.sl('type',[fn,op,st],'string',function(is,n){
                is[n] !== '' && (s =  is[n]);
            })
            
            function _lc(){
                
                var elem = $('#lc_'+o.name);
                
                ob && (obo.p = ob.offset(),obo.w = ob.outerWidth(),obo.h = ob.outerHeight());
                
                if(s == 'show' && ob && elem.length) elem.hide().css({left:obo.p.left,top:obo.p.top,width:obo.w,height:obo.h}).fadeIn(function(){
                        f();
                    });
                else if(ob && !elem.length && s == 'show') $('<div class="sl_loading_content" id="lc_'+o.name+'"></div>').hide().css({left:obo.p.left,top:obo.p.top,width:obo.w,height:obo.h}).appendTo('body').fadeIn(function(){
                        f();
                    });
                else if(elem.length) elem.fadeOut(function(){ $(this).remove(),f() });
                else f();
            }

            function _lq(){
                var dg = $(document).data('loading');
                
                dg && (c = $.sl('count',dg.q));
                
                var elem = $('.sl_loading_quiet');
                
                if(!elem.length && s == 'show') $('<div class="sl_loading_quiet">1</div>').hide().appendTo('body').fadeIn(function(){
                        q[o.name]=1,$(document).data('loading',{q:q}),f();
                    });
                else if(elem.is(':visible') && s == 'show') !dg.q[o.name] && (dg.q[o.name] = 1,elem.text(c+1),f());
                else if(elem.is(':hidden') && s == 'show') elem.fadeIn(function(){ f() });
                else if(s == 'hide'){
                    delete dg.q[o.name];
                    c < 2 ? elem.fadeOut().text(1) :  elem.text(c-1);
                } 
            }
            
            function _ls(){
                var elem = $('.sl_loading_show');
                
                if(!elem.length && s == 'show') $('<div class="sl_loading_show"></div>').hide().appendTo('body').fadeIn(function(){
                        f();
                    });
                else if(s == 'hide') elem.fadeOut(function(){ $(this).remove(),f() });
            }
            
            function _ly(){
                if(s == 'show') $('body').addClass('sl_loading_cursor'),f();
                else if(s == 'hide') $('body').removeClass('sl_loading_cursor'),f();
            }
            
            function _lp(){
                
                var elem = $('#lp_'+o.name);
                
                if(s == 'show' && elem.length) elem.hide().css({left:cursor_point.x,top:cursor_point.y}).fadeIn(function(){
                        f();
                    });
                else if(!elem.length && s == 'show') $('<div class="sl_loading_point" id="lp_'+o.name+'"></div>').hide().css({left:cursor_point.x,top:cursor_point.y}).appendTo('body').fadeIn(function(){
                        f();
                    });
                else if(elem.length) elem.fadeOut(function(){ $(this).remove(),f() });
                else f();
            }
            
            o.mode == 'content' ? _lc() : o.mode == 'quiet' ? _lq() : o.mode == 'show' ? _ls() : o.mode == 'cursor' ? _ly() : o.mode == 'point' ? _lp() : f();
            
        },
        /**
         * Operator
         */
        operator: function(data,callback,status,error,ignore){
            if(ignore){
                callback && callback(); return;
            }
            
            var r = '',e,i = 0,h,mas;
            
            function strpos( haystack, needle){
                if(typeof haystack == 'object') return false;
            	return haystack.indexOf( needle ) >= 0 ? true : false;
            }
            
            mas = ['Parse error:','Fatal error:','Warning:','Catchable fatal error:'];
            
            $.each(mas,function(fi,vi){
                if(strpos(data, vi)){
                    var hj = data.match(new RegExp(vi+'(.*?)\n','ig'));
                    
                    if(hj){
                        $.each(hj,function(k,v){
                            r += v+'<div class="t_sep"></div>',i++;
                        })
                    }
                }
            })
            
            r !== '' && ($.sl('window',{name:'error',error:1,title:lang[4],w:400,data:'<div class="t_p_10">'+r+lang[5]+': <b>'+i+'</b></div>'}),e = 1,error && error());
            
            if(strpos(data, '{"error":') && !e){
        
                e = data.match(/\{"error":(.*?)\}\n/g)[0];
                e = JSON.parse(e).error;
                e && ($.sl('info',e),h=1);
                error && error();
            }
            
            callback && status && !e && callback();
            callback && status && !e && h && callback();
        },
        /**
         * Load
         */
        load: function(url,fn,op,st){
			if($('#sl_info').length) $('#sl_info').fadeOut(100);
				
            var f = function(){},o,ob,hash,dt;
            
            this == false ? ob = false : ob = $(this);
            
            o = {
                type: 'POST',
                data: ob ? (ob.closest('form').length ? ob.closest('form').serializeArray() : {}): {}, 
                done: function(){},
                mode: 'content',
                back: true,
                win: false,
                form: false,
                shell: false,
                error: function(){},
                ignore: false
            };
            
            $.sl('type',[fn,op,st],'object',function(is,n){
                o = $.extend(o, is[n]);
            })
            
            $.sl('type',[fn,op,st],'function',function(is,n){
                f = is[n];
            })
            
            $.sl('type',[fn,op,st],'string',function(is,n){
                is[n] !== '' && (o.mode =  is[n]);
            })
            
            !ob.length && (o.mode == 'content' || !o.mode) && (o.mode = 'quiet');
            
            hash = Math.floor(Math.random() * (999 - 100 + 1)) + 100;
            
            dt = {name:hash,mode:o.mode,tip:lang[6]+': '+url};
            
            function _getload(){
                $.ajax({
                    type: o.type,
                    url: url,
                    data: o.data,
                    complete:function(){
                        $.sl('loading','hide',dt);
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown){
                        !o.ignore && $.sl('info',lang[7]+': [Page: <b>'+url+'</b> ] [Error: <b>'+errorThrown+'</b> ]');
                         o.error && o.error();
                    },
                    dataType: o.dataType ? o.dataType : 'html',
                    timeout: o.timeout ? o.timeout * 1000 : false
                }).done(function( d ) {
                    $.sl('operator',d,function(){
                        o.done && o.done(d);
                        ob && o.back && ob.html(d);
                        f && f.apply( ob,[d]);
                        o.win && $.sl('window',$.extend({status:'data',data:d},o.win));
                        o.shell && $.sl('shell',o.shell);
                    },true,function(){
                        o.error && o.error();
                    },o.ignore);
                    
                });
            }
            
            ob ? $(this).sl('loading',dt,function(){ _getload() }) : $.sl('loading',dt,function(){ _getload() });
        },
        /**
         * _PROMT
         */
        _promt: function(options){
            var o = $.extend({
                w: 400,
                h: 60,
                name: 'promt_'+Math.floor(Math.random() * (999 - 100 + 1)) + 100,
                btn: {},
                input: [],
                module: false,
                load: false
            }, options),nbtn = {},data = '',i=0,_this = this,dt;
            
            function bin(inp){
                o.input = inp ? inp : o.input;
                if(inp) o.btn[lang[0]] = function(){}
                inp && (o.autoclose = false);
                
                $.each(o.btn,function(name,fn){
                    nbtn[name] = function(wn){
                        dt = $('#form_'+o.name).serializeArray();
                        
                        if(o.module) $(this).sl('load',o.module[0],{data:dt,mode:(o.module[1] || 'content'),back:false},function(result){
                            
                            $.sl('window',{name:wn,status:'close'},function(){
                                o.module[2] && o.module[2].apply( _this,[dt,result]);
                                fn && fn.apply( _this,[wn,dt,result]);
                            });
                            
                        })
                        else fn && fn.apply( this,[wn,dt]);
                    }
                })
                
                $.each(o.input,function(h,q){
                    q['name'] = (q.name || h);
                    data = data +  $.scin('input',q);
                    i++;
                })
                
                o.data = '<form id="form_'+o.name+'" class="t_p_10" method="POST" action="javascript:this.preventDefault()">'+data+'</form>';
                o.btn = nbtn;
                o.h = o.h + ((i > 5 ? 5 : i) * 29);
                $.sl('window',o)
            }
            
            if(o.load){
                $(this == false ? false : this).sl('load',(typeof o.load == 'object' ? o.load[0] : o.load),{mode:(typeof o.load == 'object' ? o.load[1] : 'content'),dataType:'json',back:false},function(inp){
                    bin(inp);
                })
            }
            else bin();
        },
        /**
         * _AREA
         */
        _area: function(options){
            var o = $.extend({
                w: 500,
                h: 300,
                name: 'area_'+Math.floor(Math.random() * (999 - 100 + 1)) + 100,
                btn: {},
                module: false,
                area_name: 'area'
            }, options),nbtn = {},data = '',i=0,_this = this,dt;
            
            $.each(o.btn,function(name,fn){
                nbtn[name] = function(wn){
                    dt = $('#form_'+o.name).serializeArray();
                    
                    if(o.module) $(this).sl('load',o.module[0],{data:dt,mode:(o.module[1] || 'content'),back:false},function(result){
                        $.sl('window',{name:wn,status:'close'},function(){
                            o.module[2] && o.module[2].apply( _this,[dt,result]);
                            fn && fn.apply( _this,[wn,dt,result]);
                        });
                    })
                    else fn && fn.apply( this,[wn,dt]);
                }
            })
            
            function _show_area(ov){
                data = $.scin('textarea',{name:o.area_name,value:(ov || ''),check:(o.check || false),attr:{style:'margin:0'}});
            
                o.data = '<form id="form_'+o.name+'" class="win_h_size" method="POST" action="javascript:this.preventDefault()">'+data+'</form>';
                o.btn = nbtn;
                $.sl('window',o)
            }
            
            if(typeof o.value === 'object') $(this == false ? false : this).sl('load',o.value[0],{mode:o.value[1],back:false},function(data){ _show_area(data) });
            else _show_area(o.value);
        },
        /**
         * _CONFIRM
         */
        _confirm: function(fn,op,st){            
            var o = {
                w: 400,
                h: 100,
                name: 'confirm_'+Math.floor(Math.random() * (999 - 100 + 1)) + 100,
                btn: {},
                info: ''
            },nbtn = {},data = '',i=0;
            
            $.sl('type',[fn,op,st],'object',function(is,n){
                o = $.extend(o, is[n]);
            })
            
            $.sl('type',[fn,op,st],'function',function(is,n){
                f = is[n];
            })
            
            $.sl('type',[fn,op,st],'string',function(is,n){
                is[n] !== '' && (o.info =  is[n]);
            })
            
            if($.sl('count',o.btn) >= 0 && f) o.btn[lang[8]] = f;
            
            $.each(o.btn,function(name,fn){
                nbtn[name] = function(wn){
                    fn && fn.apply( this,[wn]);
                }
            })
            
            o.data = '<div class="t_p_10">'+o.info+'</div>';
            o.btn = nbtn;
            $.sl('window',o)
        },
        /**
         * _Table delete tr
         */
        _tbl_del_tr: function(op){
            if(this == false) return;
            
            var _this = $(this),
                parent = $(this).closest('tr'),
                scroll_p = $(this).closest('.scrollbarInit');
            
            function bi(data,c){
                parent.fadeOut(function(){
                    $(this).remove();
                    scroll_p.length && scroll_p.tinyscrollbar_update('bottom');
                    $.sl('sort_table');
                    c && c();
                })
            }
            
            if(typeof op === 'object'){
                $(this == false ? false : this).sl('load',op[0],function(data){
                    bi(data,function(){
                        op[2] && op[2]();
                    });
                },{mode:(op[1]||'content'),back:false});
            }
            else if(op) window[op](function(data,callback){
                bi(data,function(){
                    callback && callback();
                })
            });
            else bi();
        },
        /**
         * _DEL_CONFIRM
         */
        _del_confirm: function(fn,op,st){
            var o = {
                w: 400,
                h: 70,
                name: 'del_confirm_'+Math.floor(Math.random() * (999 - 100 + 1)) + 100,
                btn: {},
                info: lang[9]+'?',
                module: false,
                title: lang[10],
                autoclose: false
            },nbtn = {},data = '',_this = this;
            
            $.sl('type',[fn,op,st],'object',function(is,n){
                o = $.extend(o, is[n]);
            })
            
            $.sl('type',[fn,op,st],'function',function(is,n){
                f = is[n];
            })
            
            $.sl('type',[fn,op,st],'string',function(is,n){
                is[n] !== '' && (o.module =  is[n]);
            })
            
            if(!o.module) $.sl('info',lang[11]);
            
            o.btn[lang[8]] = function(wn){
                if(o.module){
                    $(this).sl('load',(typeof o.module == 'object' ? o.module[0] : o.module),{mode:(typeof o.module == 'object' ? o.module[1] : 'content'),back:false},function(data){
                        $.sl('window',{name:wn,status:'close'},function(){
                            f && f.apply( _this,[data]);
                        });
                    })
                }
                else f && f.apply( _this);
            }
            
            $.each(o.btn,function(name,fn){
                nbtn[name] = function(wn){
                    fn && fn.apply( this,[wn]);
                }
            })
            
            o.data = '<div class="t_p_10">'+o.info+'</div>';
            o.btn = nbtn;
            $.sl('window',o)
        },
        /**
         * _WINDOW_SETTING
         */
        _window_setting: function(fn,op,st){
            var o = {
                name: 'window_setting_'+Math.floor(Math.random() * (999 - 100 + 1)) + 100,
                btn: {},
                module: [],
                load: [],
                data: '',
                autoclose: false
            },_this = this;
            
            $.sl('type',[fn,op,st],'object',function(is,n){
                o = $.extend(o, is[n]);
            })
            
            $.sl('type',[fn,op,st],'function',function(is,n){
                f = is[n];
            })
            
            $.sl('type',[fn,op,st],'string',function(is,n){
                is[n] !== '' && (o.module =  is[n]);
            })
            
            o.btn[lang[0]] = function(){
                $.sl('load',o.module[0],{mode:o.module[1],data:$('form#form_'+o.name).serializeArray()},function(re){
                    o.module[2] && o.module[2](re);
                    $.sl('window',{name:o.name,status:'close'});
                })
            }
            
            $.sl('load',o.load[0],{mod:o.load[1]},function(data){
                o.data = '<form id="form_'+o.name+'">'+data+'</form>';
                
                $.sl('window',o);
            })
        },
        /**
         * _WINDOW_SETTING
         */
        install: function(fn,op,st){
            var o = {},f,s,css,_this = this;
            
            $.sl('type',[fn,op,st],'object',function(is,n){
                o = $.extend(o, is[n]);
            })
            
            $.sl('type',[fn,op,st],'function',function(is,n){
                f = is[n];
            })
            
            $.sl('type',[fn,op,st],'string',function(is,n){
                is[n] !== '' && (s =  is[n]);
            })
            
            $ins = $(['<div class="sl_install"><div class="t_p_r"><div class="loa"></div></div></div>'].join(''));
            
            function hiins(cl){
                $ins.fadeOut(300,function(){
                    $ins.remove();
                    cl && cl();
                })
            }
            
            $ins.hide().appendTo('body').fadeIn(300,function(){
                $.sl('load',s,{mode:'hide',data:o,error:function(){
                    hiins();
                }},function(data){
                    hiins(function(){
                        f && f(data);
                    });
                })
            });
        }
    };
    
    $.fn.sl = function( method ) {
    
        if ( methods[method] ) {
          return methods[ method ].apply( this, Array.prototype.slice.call( arguments, 1 ));
        } else if ( typeof method === 'object' || ! method ) {
          return methods.init.apply( this, arguments );
        } else {
          return methods.init.apply( this );
        }   
    
    };
    
    $.sl = function( method ) {
    
        if ( methods[method] ) {
          return methods[ method ].apply( false,Array.prototype.slice.call( arguments, 1 ));
        } else if ( typeof method === 'object' || ! method ) {
          return methods.init.apply( false, arguments );
        } else {
          return methods.init();
        }   
    
    };
  

})(jQuery);

$(document).ready(function(){
    $.sl();
})
/*
 * Copyright 2015, Michael Brook, All rights reserved.
 * http://simpleupload.michaelcbrook.com/
 *
 * simpleUpload.js is an extremely simple yet powerful jQuery file upload plugin.
 * It is free to use under the MIT License (http://opensource.org/licenses/MIT)
 *
 * https://github.com/michaelcbrook/simpleUpload.js
 * @michaelcbrook
 */
function simpleUpload(e,l,n){function t(){if("object"==typeof n&&null!==n){if("boolean"==typeof n.forceIframe&&(U=n.forceIframe),"function"==typeof n.init&&(I=n.init),"function"==typeof n.start&&(L=n.start),"function"==typeof n.progress&&(q=n.progress),"function"==typeof n.success&&(W=n.success),"function"==typeof n.error&&(_=n.error),"function"==typeof n.cancel&&(M=n.cancel),"function"==typeof n.complete&&(D=n.complete),"function"==typeof n.finish&&(N=n.finish),"string"==typeof n.hashWorker&&""!=n.hashWorker&&(S=n.hashWorker),"function"==typeof n.hashComplete&&(z=n.hashComplete),"object"==typeof n.data&&null!==n.data)for(var e in n.data)F[e]=n.data[e];if("number"==typeof n.limit&&y(n.limit)&&n.limit>0&&(w=n.limit),"number"==typeof n.maxFileSize&&y(n.maxFileSize)&&n.maxFileSize>0&&(x=n.maxFileSize),"object"==typeof n.allowedExts&&null!==n.allowedExts)for(var e in n.allowedExts)j.push(n.allowedExts[e]);if("object"==typeof n.allowedTypes&&null!==n.allowedTypes)for(var e in n.allowedTypes)k.push(n.allowedTypes[e]);if("string"==typeof n.expect&&""!=n.expect){var t=n.expect.toLowerCase(),o=["auto","json","xml","html","script","text"];for(var e in o)if(o[e]==t){E=t;break}}if("object"==typeof n.xhrFields&&null!==n.xhrFields)for(var e in n.xhrFields)T[e]=n.xhrFields[e]}if("object"==typeof l&&null!==l&&l instanceof jQuery){if(!(l.length>0))return!1;l=l.get(0)}if(!U&&window.File&&window.FileReader&&window.FileList&&window.Blob&&("object"==typeof n&&null!==n&&"object"==typeof n.files&&null!==n.files?b=n.files:"object"==typeof l&&null!==l&&"object"==typeof l.files&&null!==l.files&&(b=l.files)),("object"!=typeof l||null===l)&&null==b)return!1;"object"==typeof n&&null!==n&&"string"==typeof n.name&&""!=n.name?C=n.name.replace(/\[\s*\]/g,"[0]"):"object"==typeof l&&null!==l&&"string"==typeof l.name&&""!=l.name&&(C=l.name.replace(/\[\s*\]/g,"[0]"));var r=0;if(null!=b?b.length>0&&(r=b.length>1&&window.FormData&&$.ajaxSettings.xhr().upload?w>0&&b.length>w?w:b.length:1):""!=l.value&&(r=1),r>0){if("object"==typeof l&&null!==l){var i=$(l);J=$("<form>").hide().attr("enctype","multipart/form-data").attr("method","post").appendTo("body"),i.after(i.clone(!0).val("")).removeAttr("onchange").off().removeAttr("id").attr("name",C).appendTo(J)}for(var s=0;r>s;s++)!function(e){R[e]={state:0,hashWorker:null,xhr:null,iframe:null},A[e]={upload:{index:e,state:"init",file:null!=b?b[e]:{name:l.value.split(/(\\|\/)/g).pop()},cancel:function(){if(0==u(e))d(e,4);else{if(1!=u(e))return!1;d(e,4),null!=R[e].hashWorker&&(R[e].hashWorker.terminate(),R[e].hashWorker=null),null!=R[e].xhr&&(R[e].xhr.abort(),R[e].xhr=null),null!=R[e].iframe&&($("iframe[name=simpleUpload_iframe_"+R[e].iframe+"]").attr("src","javascript:false;"),simpleUpload.dequeueIframe(R[e].iframe),R[e].iframe=null),P(e)}return!0}}}}(s);var p=H(r);if(p!==!1){var f=r;if("number"==typeof p&&y(p)&&p>=0&&r>p){f=p;for(var m=f;r>m;m++)d(m,4)}for(var c=[],v=0;f>v;v++)X(v,A[v].upload.file)!==!1&&(c[c.length]=v);c.length>0?(B=c.length,simpleUpload.queueUpload(c,function(e){a(e)}),simpleUpload.uploadNext()):Z()}else{for(var m in A)d(m,4);Z()}}}function a(e){if(1==u(e)){var n=null;if(null!=b){if(void 0==b[e]||null==b[e])return void K(e,{name:"InternalError",message:"There was an error uploading the file"});n=b[e]}else if(""==l.value)return void K(e,{name:"InternalError",message:"There was an error uploading the file"});return j.length>0&&!c(j,n)?void K(e,{name:"InvalidFileExtensionError",message:"That file format is not allowed"}):k.length>0&&!v(k,n)?void K(e,{name:"InvalidFileTypeError",message:"That file format is not allowed"}):x>0&&!h(x,n)?void K(e,{name:"MaxFileSizeError",message:"That file is too big"}):void(null!=S&&null!=z?o(e):i(e))}}function o(e){if(null!=b&&void 0!=b[e]&&null!=b[e]&&window.Worker){var l=b[e];if(void 0!=l.size&&null!=l.size&&""!=l.size&&y(l.size)&&(l.slice||l.webkitSlice||l.mozSlice))try{var n=new Worker(S);n.addEventListener("error",function(){n.terminate(),R[e].hashWorker=null,i(e)},!1),n.addEventListener("message",function(l){if(l.data.result){var t=l.data.result;n.terminate(),R[e].hashWorker=null,r(e,t)}},!1);var t,a,o,s,p,f;return f=function(e){n.postMessage({message:e.target.result,block:a})},p=function(){a.end!==l.size&&(a.start+=t,a.end+=t,a.end>l.size&&(a.end=l.size),o=new FileReader,o.onload=f,l.slice?s=l.slice(a.start,a.end):l.webkitSlice?s=l.webkitSlice(a.start,a.end):l.mozSlice&&(s=l.mozSlice(a.start,a.end)),o.readAsArrayBuffer(s))},t=1048576,a={file_size:l.size,start:0},a.end=t>l.size?l.size:t,n.addEventListener("message",p,!1),o=new FileReader,o.onload=f,l.slice?s=l.slice(a.start,a.end):l.webkitSlice?s=l.webkitSlice(a.start,a.end):l.mozSlice&&(s=l.mozSlice(a.start,a.end)),o.readAsArrayBuffer(s),void(R[e].hashWorker=n)}catch(u){}}i(e)}function r(e,l){if(1==u(e)){var n=!1,t=function(l){return 1!=u(e)?!1:n?!1:(n=!0,Y(e,100),G(e,l),!0)},a=function(){return 1!=u(e)?!1:n?!1:(n=!0,i(e),!0)},o=function(l){return 1!=u(e)?!1:n?!1:(n=!0,K(e,{name:"HashError",message:l}),!0)};z.call(A[e],l,{success:t,proceed:a,error:o})}}function i(n){if(1==u(n)){if(null!=b){if(void 0==b[n]||null==b[n])return void K(n,{name:"InternalError",message:"There was an error uploading the file"});if(window.FormData){var t=$.ajaxSettings.xhr();if(t.upload){var a=b[n],o=new FormData;f(o,F),o.append(C,a);var r={url:e,data:o,type:"post",cache:!1,xhrFields:T,beforeSend:function(e){R[n].xhr=e},xhr:function(){return t.upload.addEventListener("progress",function(e){e.lengthComputable&&Y(n,e.loaded/e.total*100)},!1),t},error:function(){R[n].xhr=null,K(n,{name:"RequestError",message:"Could not get response from server"})},success:function(e){R[n].xhr=null,Y(n,100),G(n,e)},contentType:!1,processData:!1};return"auto"!=E&&(r.dataType=E),void $.ajax(r)}}}"object"==typeof l&&null!==l?s(n):K(n,{name:"UnsupportedError",message:"Your browser does not support this upload method"})}}function s(l){if(0==l){var n=simpleUpload.queueIframe({origin:g(e),expect:E,complete:function(e){1==u(l)&&(R[l].iframe=null,simpleUpload.dequeueIframe(n),Y(l,100),G(l,e))},error:function(e){1==u(l)&&(R[l].iframe=null,simpleUpload.dequeueIframe(n),K(l,{name:"RequestError",message:e}))}});R[l].iframe=n;var t=p(F);J.attr("action",e+(-1==e.lastIndexOf("?")?"?":"&")+"_iframeUpload="+n+"&_="+(new Date).getTime()).attr("target","simpleUpload_iframe_"+n).prepend(t).submit()}else K(l,{name:"UnsupportedError",message:"Multiple file uploads not supported"})}function p(e,l){(void 0===l||null===l||""===l)&&(l=null);var n="";for(var t in e)void 0===e[t]||null===e[t]?n+=$("<div>").append($('<input type="hidden">').attr("name",null==l?t+"":l+"["+t+"]").val("")).html():"object"==typeof e[t]?n+=p(e[t],null==l?t+"":l+"["+t+"]"):"boolean"==typeof e[t]?n+=$("<div>").append($('<input type="hidden">').attr("name",null==l?t+"":l+"["+t+"]").val(e[t]?"true":"false")).html():"number"==typeof e[t]?n+=$("<div>").append($('<input type="hidden">').attr("name",null==l?t+"":l+"["+t+"]").val(e[t]+"")).html():"string"==typeof e[t]&&(n+=$("<div>").append($('<input type="hidden">').attr("name",null==l?t+"":l+"["+t+"]").val(e[t])).html());return n}function f(e,l,n){(void 0===n||null===n||""===n)&&(n=null);for(var t in l)void 0===l[t]||null===l[t]?e.append(null==n?t+"":n+"["+t+"]",""):"object"==typeof l[t]?f(e,l[t],null==n?t+"":n+"["+t+"]"):"boolean"==typeof l[t]?e.append(null==n?t+"":n+"["+t+"]",l[t]?"true":"false"):"number"==typeof l[t]?e.append(null==n?t+"":n+"["+t+"]",l[t]+""):"string"==typeof l[t]&&e.append(null==n?t+"":n+"["+t+"]",l[t])}function u(e){return R[e].state}function d(e,l){var n="";if(0==l)n="init";else if(1==l)n="uploading";else if(2==l)n="success";else if(3==l)n="error";else{if(4!=l)return!1;n="cancel"}R[e].state=l,A[e].upload.state=n}function m(e){var l=e.lastIndexOf(".");return-1!=l?e.substr(l+1):""}function c(e,n){if(void 0!=n&&null!=n){var t=n.name;if(void 0!=t&&null!=t&&""!=t){var a=m(t).toLowerCase();if(""!=a){var o=!1;for(var r in e)if(e[r].toLowerCase()==a){o=!0;break}return o?!0:!1}return!1}}if("object"!=typeof l||null===l)return!0;var i=l.value;if(""!=i){var a=m(i).toLowerCase();if(""!=a){var o=!1;for(var r in e)if(e[r].toLowerCase()==a){o=!0;break}if(o)return!0}}return!1}function v(e,l){if(void 0!=l&&null!=l){var n=l.type;if(void 0!=n&&null!=n&&""!=n){n=n.toLowerCase();var t=!1;for(var a in e)if(e[a].toLowerCase()==n){t=!0;break}return t?!0:!1}}return!0}function h(e,l){if(void 0!=l&&null!=l){var n=l.size;if(void 0!=n&&null!=n&&""!=n&&y(n))return e>=n?!0:!1}return!0}function y(e){return isNaN(e)||parseInt(e)+""!=e?!1:!0}function g(e){var l=document.createElement("a");l.href=e;var n=l.host,t=l.protocol;return""==n&&(n=window.location.host),(""==t||":"==t)&&(t=window.location.protocol),t.replace(/\:$/,"")+"://"+n}var U=!1,b=null,w=0,x=0,j=[],k=[],E="auto",S=null,z=null,C="file",F={},T={},I=function(){},L=function(){},q=function(){},W=function(){},_=function(){},M=function(){},D=function(){},N=function(){},A=[],R=[],O={files:A},B=0,J=null,Q=function(e,l){V(e,l),B--,0==B&&Z(),simpleUpload.activeUploads--,simpleUpload.uploadNext()},H=function(e){return I.call(O,e)},X=function(e,l){return u(e)>0?!1:L.call(A[e],l)===!1?(d(e,4),!1):u(e)>0?!1:void d(e,1)},Y=function(e,l){1==u(e)&&q.call(A[e],l)},G=function(e,l){1==u(e)&&(d(e,2),W.call(A[e],l),Q(e,"success"))},K=function(e,l){1==u(e)&&(d(e,3),_.call(A[e],l),Q(e,"error"))},P=function(e){M.call(A[e]),Q(e,"cancel")},V=function(e,l){D.call(A[e],l)},Z=function(){N.call(O),null!=J&&J.remove()};t()}simpleUpload.maxUploads=10,simpleUpload.activeUploads=0,simpleUpload.uploads=[],simpleUpload.iframes={},simpleUpload.iframeCount=0,simpleUpload.queueUpload=function(e,l){simpleUpload.uploads[simpleUpload.uploads.length]={uploads:e,callback:l}},simpleUpload.uploadNext=function(){if(simpleUpload.uploads.length>0&&simpleUpload.activeUploads<simpleUpload.maxUploads){var e=simpleUpload.uploads[0],l=e.callback,n=e.uploads.splice(0,1)[0];0==e.uploads.length&&simpleUpload.uploads.splice(0,1),simpleUpload.activeUploads++,l(n),simpleUpload.uploadNext()}},simpleUpload.queueIframe=function(e){for(var l=0;0==l||l in simpleUpload.iframes;)l=Math.floor(999999999*Math.random()+1);return simpleUpload.iframes[l]=e,simpleUpload.iframeCount++,$("body").append('<iframe name="simpleUpload_iframe_'+l+'" style="display: none;"></iframe>'),l},simpleUpload.dequeueIframe=function(e){e in simpleUpload.iframes&&($("iframe[name=simpleUpload_iframe_"+e+"]").remove(),delete simpleUpload.iframes[e],simpleUpload.iframeCount--)},simpleUpload.convertDataType=function(e,l,n){var t="auto";if("auto"==e){if("string"==typeof l&&""!=l){var a=l.toLowerCase(),o=["json","xml","html","script","text"];for(var r in o)if(o[r]==a){t=a;break}}}else t=e;if("auto"==t)return"undefined"==typeof n?"":"object"==typeof n?n:String(n);if("json"==t){if("undefined"==typeof n||null===n)return null;if("object"==typeof n)return n;if("string"==typeof n)try{return $.parseJSON(n)}catch(i){return!1}return!1}if("xml"==t){if("undefined"==typeof n||null===n)return null;if("string"==typeof n)try{return $.parseXML(n)}catch(i){return!1}return!1}if("script"==t){if("undefined"==typeof n)return"";if("string"==typeof n)try{return $.globalEval(n),n}catch(i){return!1}return!1}return"undefined"==typeof n?"":String(n)},simpleUpload.iframeCallback=function(e){if("object"==typeof e&&null!==e){var l=e.id;if(l in simpleUpload.iframes){var n=simpleUpload.convertDataType(simpleUpload.iframes[l].expect,e.type,e.data);n!==!1?simpleUpload.iframes[l].complete(n):simpleUpload.iframes[l].error("Could not get response from server")}}},simpleUpload.postMessageCallback=function(e){try{var l=e.message?"message":"data",n=e[l];if("string"==typeof n&&""!=n&&(n=$.parseJSON(n),"object"==typeof n&&null!==n&&"string"==typeof n.namespace&&"simpleUpload"==n.namespace)){var t=n.id;if(t in simpleUpload.iframes&&e.origin===simpleUpload.iframes[t].origin){var a=simpleUpload.convertDataType(simpleUpload.iframes[t].expect,n.type,n.data);a!==!1?simpleUpload.iframes[t].complete(a):simpleUpload.iframes[t].error("Could not get response from server")}}}catch(e){}},window.addEventListener?window.addEventListener("message",simpleUpload.postMessageCallback,!1):window.attachEvent("onmessage",simpleUpload.postMessageCallback),function(e){"function"==typeof define&&define.amd?define(["jquery"],e):"object"==typeof exports?module.exports=e(require("jquery")):e(jQuery)}(function(e){e.fn.simpleUpload=function(l,n){return 0==e(this).length&&"object"==typeof n&&null!==n&&"object"==typeof n.files&&null!==n.files?(new simpleUpload(l,null,n),this):this.each(function(){new simpleUpload(l,this,n)})},e.fn.simpleUpload.maxSimultaneousUploads=function(e){return"undefined"==typeof e?simpleUpload.maxUploads:"number"==typeof e&&e>0?(simpleUpload.maxUploads=e,this):void 0}});
/*! jQuery UI - v1.12.1 - 2017-09-05
* http://jqueryui.com
* Includes: widget.js, position.js, keycode.js, unique-id.js, widgets/autocomplete.js, widgets/menu.js
* Copyright jQuery Foundation and other contributors; Licensed MIT */

(function( factory ) {
	if ( typeof define === "function" && define.amd ) {

		// AMD. Register as an anonymous module.
		define([ "jquery" ], factory );
	} else {

		// Browser globals
		factory( jQuery );
	}
}(function( $ ) {

$.ui = $.ui || {};

var version = $.ui.version = "1.12.1";


/*!
 * jQuery UI Widget 1.12.1
 * http://jqueryui.com
 *
 * Copyright jQuery Foundation and other contributors
 * Released under the MIT license.
 * http://jquery.org/license
 */

//>>label: Widget
//>>group: Core
//>>description: Provides a factory for creating stateful widgets with a common API.
//>>docs: http://api.jqueryui.com/jQuery.widget/
//>>demos: http://jqueryui.com/widget/



var widgetUuid = 0;
var widgetSlice = Array.prototype.slice;

$.cleanData = ( function( orig ) {
	return function( elems ) {
		var events, elem, i;
		for ( i = 0; ( elem = elems[ i ] ) != null; i++ ) {
			try {

				// Only trigger remove when necessary to save time
				events = $._data( elem, "events" );
				if ( events && events.remove ) {
					$( elem ).triggerHandler( "remove" );
				}

			// Http://bugs.jquery.com/ticket/8235
			} catch ( e ) {}
		}
		orig( elems );
	};
} )( $.cleanData );

$.widget = function( name, base, prototype ) {
	var existingConstructor, constructor, basePrototype;

	// ProxiedPrototype allows the provided prototype to remain unmodified
	// so that it can be used as a mixin for multiple widgets (#8876)
	var proxiedPrototype = {};

	var namespace = name.split( "." )[ 0 ];
	name = name.split( "." )[ 1 ];
	var fullName = namespace + "-" + name;

	if ( !prototype ) {
		prototype = base;
		base = $.Widget;
	}

	if ( $.isArray( prototype ) ) {
		prototype = $.extend.apply( null, [ {} ].concat( prototype ) );
	}

	// Create selector for plugin
	$.expr[ ":" ][ fullName.toLowerCase() ] = function( elem ) {
		return !!$.data( elem, fullName );
	};

	$[ namespace ] = $[ namespace ] || {};
	existingConstructor = $[ namespace ][ name ];
	constructor = $[ namespace ][ name ] = function( options, element ) {

		// Allow instantiation without "new" keyword
		if ( !this._createWidget ) {
			return new constructor( options, element );
		}

		// Allow instantiation without initializing for simple inheritance
		// must use "new" keyword (the code above always passes args)
		if ( arguments.length ) {
			this._createWidget( options, element );
		}
	};

	// Extend with the existing constructor to carry over any static properties
	$.extend( constructor, existingConstructor, {
		version: prototype.version,

		// Copy the object used to create the prototype in case we need to
		// redefine the widget later
		_proto: $.extend( {}, prototype ),

		// Track widgets that inherit from this widget in case this widget is
		// redefined after a widget inherits from it
		_childConstructors: []
	} );

	basePrototype = new base();

	// We need to make the options hash a property directly on the new instance
	// otherwise we'll modify the options hash on the prototype that we're
	// inheriting from
	basePrototype.options = $.widget.extend( {}, basePrototype.options );
	$.each( prototype, function( prop, value ) {
		if ( !$.isFunction( value ) ) {
			proxiedPrototype[ prop ] = value;
			return;
		}
		proxiedPrototype[ prop ] = ( function() {
			function _super() {
				return base.prototype[ prop ].apply( this, arguments );
			}

			function _superApply( args ) {
				return base.prototype[ prop ].apply( this, args );
			}

			return function() {
				var __super = this._super;
				var __superApply = this._superApply;
				var returnValue;

				this._super = _super;
				this._superApply = _superApply;

				returnValue = value.apply( this, arguments );

				this._super = __super;
				this._superApply = __superApply;

				return returnValue;
			};
		} )();
	} );
	constructor.prototype = $.widget.extend( basePrototype, {

		// TODO: remove support for widgetEventPrefix
		// always use the name + a colon as the prefix, e.g., draggable:start
		// don't prefix for widgets that aren't DOM-based
		widgetEventPrefix: existingConstructor ? ( basePrototype.widgetEventPrefix || name ) : name
	}, proxiedPrototype, {
		constructor: constructor,
		namespace: namespace,
		widgetName: name,
		widgetFullName: fullName
	} );

	// If this widget is being redefined then we need to find all widgets that
	// are inheriting from it and redefine all of them so that they inherit from
	// the new version of this widget. We're essentially trying to replace one
	// level in the prototype chain.
	if ( existingConstructor ) {
		$.each( existingConstructor._childConstructors, function( i, child ) {
			var childPrototype = child.prototype;

			// Redefine the child widget using the same prototype that was
			// originally used, but inherit from the new version of the base
			$.widget( childPrototype.namespace + "." + childPrototype.widgetName, constructor,
				child._proto );
		} );

		// Remove the list of existing child constructors from the old constructor
		// so the old child constructors can be garbage collected
		delete existingConstructor._childConstructors;
	} else {
		base._childConstructors.push( constructor );
	}

	$.widget.bridge( name, constructor );

	return constructor;
};

$.widget.extend = function( target ) {
	var input = widgetSlice.call( arguments, 1 );
	var inputIndex = 0;
	var inputLength = input.length;
	var key;
	var value;

	for ( ; inputIndex < inputLength; inputIndex++ ) {
		for ( key in input[ inputIndex ] ) {
			value = input[ inputIndex ][ key ];
			if ( input[ inputIndex ].hasOwnProperty( key ) && value !== undefined ) {

				// Clone objects
				if ( $.isPlainObject( value ) ) {
					target[ key ] = $.isPlainObject( target[ key ] ) ?
						$.widget.extend( {}, target[ key ], value ) :

						// Don't extend strings, arrays, etc. with objects
						$.widget.extend( {}, value );

				// Copy everything else by reference
				} else {
					target[ key ] = value;
				}
			}
		}
	}
	return target;
};

$.widget.bridge = function( name, object ) {
	var fullName = object.prototype.widgetFullName || name;
	$.fn[ name ] = function( options ) {
		var isMethodCall = typeof options === "string";
		var args = widgetSlice.call( arguments, 1 );
		var returnValue = this;

		if ( isMethodCall ) {

			// If this is an empty collection, we need to have the instance method
			// return undefined instead of the jQuery instance
			if ( !this.length && options === "instance" ) {
				returnValue = undefined;
			} else {
				this.each( function() {
					var methodValue;
					var instance = $.data( this, fullName );

					if ( options === "instance" ) {
						returnValue = instance;
						return false;
					}

					if ( !instance ) {
						return $.error( "cannot call methods on " + name +
							" prior to initialization; " +
							"attempted to call method '" + options + "'" );
					}

					if ( !$.isFunction( instance[ options ] ) || options.charAt( 0 ) === "_" ) {
						return $.error( "no such method '" + options + "' for " + name +
							" widget instance" );
					}

					methodValue = instance[ options ].apply( instance, args );

					if ( methodValue !== instance && methodValue !== undefined ) {
						returnValue = methodValue && methodValue.jquery ?
							returnValue.pushStack( methodValue.get() ) :
							methodValue;
						return false;
					}
				} );
			}
		} else {

			// Allow multiple hashes to be passed on init
			if ( args.length ) {
				options = $.widget.extend.apply( null, [ options ].concat( args ) );
			}

			this.each( function() {
				var instance = $.data( this, fullName );
				if ( instance ) {
					instance.option( options || {} );
					if ( instance._init ) {
						instance._init();
					}
				} else {
					$.data( this, fullName, new object( options, this ) );
				}
			} );
		}

		return returnValue;
	};
};

$.Widget = function( /* options, element */ ) {};
$.Widget._childConstructors = [];

$.Widget.prototype = {
	widgetName: "widget",
	widgetEventPrefix: "",
	defaultElement: "<div>",

	options: {
		classes: {},
		disabled: false,

		// Callbacks
		create: null
	},

	_createWidget: function( options, element ) {
		element = $( element || this.defaultElement || this )[ 0 ];
		this.element = $( element );
		this.uuid = widgetUuid++;
		this.eventNamespace = "." + this.widgetName + this.uuid;

		this.bindings = $();
		this.hoverable = $();
		this.focusable = $();
		this.classesElementLookup = {};

		if ( element !== this ) {
			$.data( element, this.widgetFullName, this );
			this._on( true, this.element, {
				remove: function( event ) {
					if ( event.target === element ) {
						this.destroy();
					}
				}
			} );
			this.document = $( element.style ?

				// Element within the document
				element.ownerDocument :

				// Element is window or document
				element.document || element );
			this.window = $( this.document[ 0 ].defaultView || this.document[ 0 ].parentWindow );
		}

		this.options = $.widget.extend( {},
			this.options,
			this._getCreateOptions(),
			options );

		this._create();

		if ( this.options.disabled ) {
			this._setOptionDisabled( this.options.disabled );
		}

		this._trigger( "create", null, this._getCreateEventData() );
		this._init();
	},

	_getCreateOptions: function() {
		return {};
	},

	_getCreateEventData: $.noop,

	_create: $.noop,

	_init: $.noop,

	destroy: function() {
		var that = this;

		this._destroy();
		$.each( this.classesElementLookup, function( key, value ) {
			that._removeClass( value, key );
		} );

		// We can probably remove the unbind calls in 2.0
		// all event bindings should go through this._on()
		this.element
			.off( this.eventNamespace )
			.removeData( this.widgetFullName );
		this.widget()
			.off( this.eventNamespace )
			.removeAttr( "aria-disabled" );

		// Clean up events and states
		this.bindings.off( this.eventNamespace );
	},

	_destroy: $.noop,

	widget: function() {
		return this.element;
	},

	option: function( key, value ) {
		var options = key;
		var parts;
		var curOption;
		var i;

		if ( arguments.length === 0 ) {

			// Don't return a reference to the internal hash
			return $.widget.extend( {}, this.options );
		}

		if ( typeof key === "string" ) {

			// Handle nested keys, e.g., "foo.bar" => { foo: { bar: ___ } }
			options = {};
			parts = key.split( "." );
			key = parts.shift();
			if ( parts.length ) {
				curOption = options[ key ] = $.widget.extend( {}, this.options[ key ] );
				for ( i = 0; i < parts.length - 1; i++ ) {
					curOption[ parts[ i ] ] = curOption[ parts[ i ] ] || {};
					curOption = curOption[ parts[ i ] ];
				}
				key = parts.pop();
				if ( arguments.length === 1 ) {
					return curOption[ key ] === undefined ? null : curOption[ key ];
				}
				curOption[ key ] = value;
			} else {
				if ( arguments.length === 1 ) {
					return this.options[ key ] === undefined ? null : this.options[ key ];
				}
				options[ key ] = value;
			}
		}

		this._setOptions( options );

		return this;
	},

	_setOptions: function( options ) {
		var key;

		for ( key in options ) {
			this._setOption( key, options[ key ] );
		}

		return this;
	},

	_setOption: function( key, value ) {
		if ( key === "classes" ) {
			this._setOptionClasses( value );
		}

		this.options[ key ] = value;

		if ( key === "disabled" ) {
			this._setOptionDisabled( value );
		}

		return this;
	},

	_setOptionClasses: function( value ) {
		var classKey, elements, currentElements;

		for ( classKey in value ) {
			currentElements = this.classesElementLookup[ classKey ];
			if ( value[ classKey ] === this.options.classes[ classKey ] ||
					!currentElements ||
					!currentElements.length ) {
				continue;
			}

			// We are doing this to create a new jQuery object because the _removeClass() call
			// on the next line is going to destroy the reference to the current elements being
			// tracked. We need to save a copy of this collection so that we can add the new classes
			// below.
			elements = $( currentElements.get() );
			this._removeClass( currentElements, classKey );

			// We don't use _addClass() here, because that uses this.options.classes
			// for generating the string of classes. We want to use the value passed in from
			// _setOption(), this is the new value of the classes option which was passed to
			// _setOption(). We pass this value directly to _classes().
			elements.addClass( this._classes( {
				element: elements,
				keys: classKey,
				classes: value,
				add: true
			} ) );
		}
	},

	_setOptionDisabled: function( value ) {
		this._toggleClass( this.widget(), this.widgetFullName + "-disabled", null, !!value );

		// If the widget is becoming disabled, then nothing is interactive
		if ( value ) {
			this._removeClass( this.hoverable, null, "ui-state-hover" );
			this._removeClass( this.focusable, null, "ui-state-focus" );
		}
	},

	enable: function() {
		return this._setOptions( { disabled: false } );
	},

	disable: function() {
		return this._setOptions( { disabled: true } );
	},

	_classes: function( options ) {
		var full = [];
		var that = this;

		options = $.extend( {
			element: this.element,
			classes: this.options.classes || {}
		}, options );

		function processClassString( classes, checkOption ) {
			var current, i;
			for ( i = 0; i < classes.length; i++ ) {
				current = that.classesElementLookup[ classes[ i ] ] || $();
				if ( options.add ) {
					current = $( $.unique( current.get().concat( options.element.get() ) ) );
				} else {
					current = $( current.not( options.element ).get() );
				}
				that.classesElementLookup[ classes[ i ] ] = current;
				full.push( classes[ i ] );
				if ( checkOption && options.classes[ classes[ i ] ] ) {
					full.push( options.classes[ classes[ i ] ] );
				}
			}
		}

		this._on( options.element, {
			"remove": "_untrackClassesElement"
		} );

		if ( options.keys ) {
			processClassString( options.keys.match( /\S+/g ) || [], true );
		}
		if ( options.extra ) {
			processClassString( options.extra.match( /\S+/g ) || [] );
		}

		return full.join( " " );
	},

	_untrackClassesElement: function( event ) {
		var that = this;
		$.each( that.classesElementLookup, function( key, value ) {
			if ( $.inArray( event.target, value ) !== -1 ) {
				that.classesElementLookup[ key ] = $( value.not( event.target ).get() );
			}
		} );
	},

	_removeClass: function( element, keys, extra ) {
		return this._toggleClass( element, keys, extra, false );
	},

	_addClass: function( element, keys, extra ) {
		return this._toggleClass( element, keys, extra, true );
	},

	_toggleClass: function( element, keys, extra, add ) {
		add = ( typeof add === "boolean" ) ? add : extra;
		var shift = ( typeof element === "string" || element === null ),
			options = {
				extra: shift ? keys : extra,
				keys: shift ? element : keys,
				element: shift ? this.element : element,
				add: add
			};
		options.element.toggleClass( this._classes( options ), add );
		return this;
	},

	_on: function( suppressDisabledCheck, element, handlers ) {
		var delegateElement;
		var instance = this;

		// No suppressDisabledCheck flag, shuffle arguments
		if ( typeof suppressDisabledCheck !== "boolean" ) {
			handlers = element;
			element = suppressDisabledCheck;
			suppressDisabledCheck = false;
		}

		// No element argument, shuffle and use this.element
		if ( !handlers ) {
			handlers = element;
			element = this.element;
			delegateElement = this.widget();
		} else {
			element = delegateElement = $( element );
			this.bindings = this.bindings.add( element );
		}

		$.each( handlers, function( event, handler ) {
			function handlerProxy() {

				// Allow widgets to customize the disabled handling
				// - disabled as an array instead of boolean
				// - disabled class as method for disabling individual parts
				if ( !suppressDisabledCheck &&
						( instance.options.disabled === true ||
						$( this ).hasClass( "ui-state-disabled" ) ) ) {
					return;
				}
				return ( typeof handler === "string" ? instance[ handler ] : handler )
					.apply( instance, arguments );
			}

			// Copy the guid so direct unbinding works
			if ( typeof handler !== "string" ) {
				handlerProxy.guid = handler.guid =
					handler.guid || handlerProxy.guid || $.guid++;
			}

			var match = event.match( /^([\w:-]*)\s*(.*)$/ );
			var eventName = match[ 1 ] + instance.eventNamespace;
			var selector = match[ 2 ];

			if ( selector ) {
				delegateElement.on( eventName, selector, handlerProxy );
			} else {
				element.on( eventName, handlerProxy );
			}
		} );
	},

	_off: function( element, eventName ) {
		eventName = ( eventName || "" ).split( " " ).join( this.eventNamespace + " " ) +
			this.eventNamespace;
		element.off( eventName ).off( eventName );

		// Clear the stack to avoid memory leaks (#10056)
		this.bindings = $( this.bindings.not( element ).get() );
		this.focusable = $( this.focusable.not( element ).get() );
		this.hoverable = $( this.hoverable.not( element ).get() );
	},

	_delay: function( handler, delay ) {
		function handlerProxy() {
			return ( typeof handler === "string" ? instance[ handler ] : handler )
				.apply( instance, arguments );
		}
		var instance = this;
		return setTimeout( handlerProxy, delay || 0 );
	},

	_hoverable: function( element ) {
		this.hoverable = this.hoverable.add( element );
		this._on( element, {
			mouseenter: function( event ) {
				this._addClass( $( event.currentTarget ), null, "ui-state-hover" );
			},
			mouseleave: function( event ) {
				this._removeClass( $( event.currentTarget ), null, "ui-state-hover" );
			}
		} );
	},

	_focusable: function( element ) {
		this.focusable = this.focusable.add( element );
		this._on( element, {
			focusin: function( event ) {
				this._addClass( $( event.currentTarget ), null, "ui-state-focus" );
			},
			focusout: function( event ) {
				this._removeClass( $( event.currentTarget ), null, "ui-state-focus" );
			}
		} );
	},

	_trigger: function( type, event, data ) {
		var prop, orig;
		var callback = this.options[ type ];

		data = data || {};
		event = $.Event( event );
		event.type = ( type === this.widgetEventPrefix ?
			type :
			this.widgetEventPrefix + type ).toLowerCase();

		// The original event may come from any element
		// so we need to reset the target on the new event
		event.target = this.element[ 0 ];

		// Copy original event properties over to the new event
		orig = event.originalEvent;
		if ( orig ) {
			for ( prop in orig ) {
				if ( !( prop in event ) ) {
					event[ prop ] = orig[ prop ];
				}
			}
		}

		this.element.trigger( event, data );
		return !( $.isFunction( callback ) &&
			callback.apply( this.element[ 0 ], [ event ].concat( data ) ) === false ||
			event.isDefaultPrevented() );
	}
};

$.each( { show: "fadeIn", hide: "fadeOut" }, function( method, defaultEffect ) {
	$.Widget.prototype[ "_" + method ] = function( element, options, callback ) {
		if ( typeof options === "string" ) {
			options = { effect: options };
		}

		var hasOptions;
		var effectName = !options ?
			method :
			options === true || typeof options === "number" ?
				defaultEffect :
				options.effect || defaultEffect;

		options = options || {};
		if ( typeof options === "number" ) {
			options = { duration: options };
		}

		hasOptions = !$.isEmptyObject( options );
		options.complete = callback;

		if ( options.delay ) {
			element.delay( options.delay );
		}

		if ( hasOptions && $.effects && $.effects.effect[ effectName ] ) {
			element[ method ]( options );
		} else if ( effectName !== method && element[ effectName ] ) {
			element[ effectName ]( options.duration, options.easing, callback );
		} else {
			element.queue( function( next ) {
				$( this )[ method ]();
				if ( callback ) {
					callback.call( element[ 0 ] );
				}
				next();
			} );
		}
	};
} );

var widget = $.widget;


/*!
 * jQuery UI Position 1.12.1
 * http://jqueryui.com
 *
 * Copyright jQuery Foundation and other contributors
 * Released under the MIT license.
 * http://jquery.org/license
 *
 * http://api.jqueryui.com/position/
 */

//>>label: Position
//>>group: Core
//>>description: Positions elements relative to other elements.
//>>docs: http://api.jqueryui.com/position/
//>>demos: http://jqueryui.com/position/


( function() {
var cachedScrollbarWidth,
	max = Math.max,
	abs = Math.abs,
	rhorizontal = /left|center|right/,
	rvertical = /top|center|bottom/,
	roffset = /[\+\-]\d+(\.[\d]+)?%?/,
	rposition = /^\w+/,
	rpercent = /%$/,
	_position = $.fn.position;

function getOffsets( offsets, width, height ) {
	return [
		parseFloat( offsets[ 0 ] ) * ( rpercent.test( offsets[ 0 ] ) ? width / 100 : 1 ),
		parseFloat( offsets[ 1 ] ) * ( rpercent.test( offsets[ 1 ] ) ? height / 100 : 1 )
	];
}

function parseCss( element, property ) {
	return parseInt( $.css( element, property ), 10 ) || 0;
}

function getDimensions( elem ) {
	var raw = elem[ 0 ];
	if ( raw.nodeType === 9 ) {
		return {
			width: elem.width(),
			height: elem.height(),
			offset: { top: 0, left: 0 }
		};
	}
	if ( $.isWindow( raw ) ) {
		return {
			width: elem.width(),
			height: elem.height(),
			offset: { top: elem.scrollTop(), left: elem.scrollLeft() }
		};
	}
	if ( raw.preventDefault ) {
		return {
			width: 0,
			height: 0,
			offset: { top: raw.pageY, left: raw.pageX }
		};
	}
	return {
		width: elem.outerWidth(),
		height: elem.outerHeight(),
		offset: elem.offset()
	};
}

$.position = {
	scrollbarWidth: function() {
		if ( cachedScrollbarWidth !== undefined ) {
			return cachedScrollbarWidth;
		}
		var w1, w2,
			div = $( "<div " +
				"style='display:block;position:absolute;width:50px;height:50px;overflow:hidden;'>" +
				"<div style='height:100px;width:auto;'></div></div>" ),
			innerDiv = div.children()[ 0 ];

		$( "body" ).append( div );
		w1 = innerDiv.offsetWidth;
		div.css( "overflow", "scroll" );

		w2 = innerDiv.offsetWidth;

		if ( w1 === w2 ) {
			w2 = div[ 0 ].clientWidth;
		}

		div.remove();

		return ( cachedScrollbarWidth = w1 - w2 );
	},
	getScrollInfo: function( within ) {
		var overflowX = within.isWindow || within.isDocument ? "" :
				within.element.css( "overflow-x" ),
			overflowY = within.isWindow || within.isDocument ? "" :
				within.element.css( "overflow-y" ),
			hasOverflowX = overflowX === "scroll" ||
				( overflowX === "auto" && within.width < within.element[ 0 ].scrollWidth ),
			hasOverflowY = overflowY === "scroll" ||
				( overflowY === "auto" && within.height < within.element[ 0 ].scrollHeight );
		return {
			width: hasOverflowY ? $.position.scrollbarWidth() : 0,
			height: hasOverflowX ? $.position.scrollbarWidth() : 0
		};
	},
	getWithinInfo: function( element ) {
		var withinElement = $( element || window ),
			isWindow = $.isWindow( withinElement[ 0 ] ),
			isDocument = !!withinElement[ 0 ] && withinElement[ 0 ].nodeType === 9,
			hasOffset = !isWindow && !isDocument;
		return {
			element: withinElement,
			isWindow: isWindow,
			isDocument: isDocument,
			offset: hasOffset ? $( element ).offset() : { left: 0, top: 0 },
			scrollLeft: withinElement.scrollLeft(),
			scrollTop: withinElement.scrollTop(),
			width: withinElement.outerWidth(),
			height: withinElement.outerHeight()
		};
	}
};

$.fn.position = function( options ) {
	if ( !options || !options.of ) {
		return _position.apply( this, arguments );
	}

	// Make a copy, we don't want to modify arguments
	options = $.extend( {}, options );

	var atOffset, targetWidth, targetHeight, targetOffset, basePosition, dimensions,
		target = $( options.of ),
		within = $.position.getWithinInfo( options.within ),
		scrollInfo = $.position.getScrollInfo( within ),
		collision = ( options.collision || "flip" ).split( " " ),
		offsets = {};

	dimensions = getDimensions( target );
	if ( target[ 0 ].preventDefault ) {

		// Force left top to allow flipping
		options.at = "left top";
	}
	targetWidth = dimensions.width;
	targetHeight = dimensions.height;
	targetOffset = dimensions.offset;

	// Clone to reuse original targetOffset later
	basePosition = $.extend( {}, targetOffset );

	// Force my and at to have valid horizontal and vertical positions
	// if a value is missing or invalid, it will be converted to center
	$.each( [ "my", "at" ], function() {
		var pos = ( options[ this ] || "" ).split( " " ),
			horizontalOffset,
			verticalOffset;

		if ( pos.length === 1 ) {
			pos = rhorizontal.test( pos[ 0 ] ) ?
				pos.concat( [ "center" ] ) :
				rvertical.test( pos[ 0 ] ) ?
					[ "center" ].concat( pos ) :
					[ "center", "center" ];
		}
		pos[ 0 ] = rhorizontal.test( pos[ 0 ] ) ? pos[ 0 ] : "center";
		pos[ 1 ] = rvertical.test( pos[ 1 ] ) ? pos[ 1 ] : "center";

		// Calculate offsets
		horizontalOffset = roffset.exec( pos[ 0 ] );
		verticalOffset = roffset.exec( pos[ 1 ] );
		offsets[ this ] = [
			horizontalOffset ? horizontalOffset[ 0 ] : 0,
			verticalOffset ? verticalOffset[ 0 ] : 0
		];

		// Reduce to just the positions without the offsets
		options[ this ] = [
			rposition.exec( pos[ 0 ] )[ 0 ],
			rposition.exec( pos[ 1 ] )[ 0 ]
		];
	} );

	// Normalize collision option
	if ( collision.length === 1 ) {
		collision[ 1 ] = collision[ 0 ];
	}

	if ( options.at[ 0 ] === "right" ) {
		basePosition.left += targetWidth;
	} else if ( options.at[ 0 ] === "center" ) {
		basePosition.left += targetWidth / 2;
	}

	if ( options.at[ 1 ] === "bottom" ) {
		basePosition.top += targetHeight;
	} else if ( options.at[ 1 ] === "center" ) {
		basePosition.top += targetHeight / 2;
	}

	atOffset = getOffsets( offsets.at, targetWidth, targetHeight );
	basePosition.left += atOffset[ 0 ];
	basePosition.top += atOffset[ 1 ];

	return this.each( function() {
		var collisionPosition, using,
			elem = $( this ),
			elemWidth = elem.outerWidth(),
			elemHeight = elem.outerHeight(),
			marginLeft = parseCss( this, "marginLeft" ),
			marginTop = parseCss( this, "marginTop" ),
			collisionWidth = elemWidth + marginLeft + parseCss( this, "marginRight" ) +
				scrollInfo.width,
			collisionHeight = elemHeight + marginTop + parseCss( this, "marginBottom" ) +
				scrollInfo.height,
			position = $.extend( {}, basePosition ),
			myOffset = getOffsets( offsets.my, elem.outerWidth(), elem.outerHeight() );

		if ( options.my[ 0 ] === "right" ) {
			position.left -= elemWidth;
		} else if ( options.my[ 0 ] === "center" ) {
			position.left -= elemWidth / 2;
		}

		if ( options.my[ 1 ] === "bottom" ) {
			position.top -= elemHeight;
		} else if ( options.my[ 1 ] === "center" ) {
			position.top -= elemHeight / 2;
		}

		position.left += myOffset[ 0 ];
		position.top += myOffset[ 1 ];

		collisionPosition = {
			marginLeft: marginLeft,
			marginTop: marginTop
		};

		$.each( [ "left", "top" ], function( i, dir ) {
			if ( $.ui.position[ collision[ i ] ] ) {
				$.ui.position[ collision[ i ] ][ dir ]( position, {
					targetWidth: targetWidth,
					targetHeight: targetHeight,
					elemWidth: elemWidth,
					elemHeight: elemHeight,
					collisionPosition: collisionPosition,
					collisionWidth: collisionWidth,
					collisionHeight: collisionHeight,
					offset: [ atOffset[ 0 ] + myOffset[ 0 ], atOffset [ 1 ] + myOffset[ 1 ] ],
					my: options.my,
					at: options.at,
					within: within,
					elem: elem
				} );
			}
		} );

		if ( options.using ) {

			// Adds feedback as second argument to using callback, if present
			using = function( props ) {
				var left = targetOffset.left - position.left,
					right = left + targetWidth - elemWidth,
					top = targetOffset.top - position.top,
					bottom = top + targetHeight - elemHeight,
					feedback = {
						target: {
							element: target,
							left: targetOffset.left,
							top: targetOffset.top,
							width: targetWidth,
							height: targetHeight
						},
						element: {
							element: elem,
							left: position.left,
							top: position.top,
							width: elemWidth,
							height: elemHeight
						},
						horizontal: right < 0 ? "left" : left > 0 ? "right" : "center",
						vertical: bottom < 0 ? "top" : top > 0 ? "bottom" : "middle"
					};
				if ( targetWidth < elemWidth && abs( left + right ) < targetWidth ) {
					feedback.horizontal = "center";
				}
				if ( targetHeight < elemHeight && abs( top + bottom ) < targetHeight ) {
					feedback.vertical = "middle";
				}
				if ( max( abs( left ), abs( right ) ) > max( abs( top ), abs( bottom ) ) ) {
					feedback.important = "horizontal";
				} else {
					feedback.important = "vertical";
				}
				options.using.call( this, props, feedback );
			};
		}

		elem.offset( $.extend( position, { using: using } ) );
	} );
};

$.ui.position = {
	fit: {
		left: function( position, data ) {
			var within = data.within,
				withinOffset = within.isWindow ? within.scrollLeft : within.offset.left,
				outerWidth = within.width,
				collisionPosLeft = position.left - data.collisionPosition.marginLeft,
				overLeft = withinOffset - collisionPosLeft,
				overRight = collisionPosLeft + data.collisionWidth - outerWidth - withinOffset,
				newOverRight;

			// Element is wider than within
			if ( data.collisionWidth > outerWidth ) {

				// Element is initially over the left side of within
				if ( overLeft > 0 && overRight <= 0 ) {
					newOverRight = position.left + overLeft + data.collisionWidth - outerWidth -
						withinOffset;
					position.left += overLeft - newOverRight;

				// Element is initially over right side of within
				} else if ( overRight > 0 && overLeft <= 0 ) {
					position.left = withinOffset;

				// Element is initially over both left and right sides of within
				} else {
					if ( overLeft > overRight ) {
						position.left = withinOffset + outerWidth - data.collisionWidth;
					} else {
						position.left = withinOffset;
					}
				}

			// Too far left -> align with left edge
			} else if ( overLeft > 0 ) {
				position.left += overLeft;

			// Too far right -> align with right edge
			} else if ( overRight > 0 ) {
				position.left -= overRight;

			// Adjust based on position and margin
			} else {
				position.left = max( position.left - collisionPosLeft, position.left );
			}
		},
		top: function( position, data ) {
			var within = data.within,
				withinOffset = within.isWindow ? within.scrollTop : within.offset.top,
				outerHeight = data.within.height,
				collisionPosTop = position.top - data.collisionPosition.marginTop,
				overTop = withinOffset - collisionPosTop,
				overBottom = collisionPosTop + data.collisionHeight - outerHeight - withinOffset,
				newOverBottom;

			// Element is taller than within
			if ( data.collisionHeight > outerHeight ) {

				// Element is initially over the top of within
				if ( overTop > 0 && overBottom <= 0 ) {
					newOverBottom = position.top + overTop + data.collisionHeight - outerHeight -
						withinOffset;
					position.top += overTop - newOverBottom;

				// Element is initially over bottom of within
				} else if ( overBottom > 0 && overTop <= 0 ) {
					position.top = withinOffset;

				// Element is initially over both top and bottom of within
				} else {
					if ( overTop > overBottom ) {
						position.top = withinOffset + outerHeight - data.collisionHeight;
					} else {
						position.top = withinOffset;
					}
				}

			// Too far up -> align with top
			} else if ( overTop > 0 ) {
				position.top += overTop;

			// Too far down -> align with bottom edge
			} else if ( overBottom > 0 ) {
				position.top -= overBottom;

			// Adjust based on position and margin
			} else {
				position.top = max( position.top - collisionPosTop, position.top );
			}
		}
	},
	flip: {
		left: function( position, data ) {
			var within = data.within,
				withinOffset = within.offset.left + within.scrollLeft,
				outerWidth = within.width,
				offsetLeft = within.isWindow ? within.scrollLeft : within.offset.left,
				collisionPosLeft = position.left - data.collisionPosition.marginLeft,
				overLeft = collisionPosLeft - offsetLeft,
				overRight = collisionPosLeft + data.collisionWidth - outerWidth - offsetLeft,
				myOffset = data.my[ 0 ] === "left" ?
					-data.elemWidth :
					data.my[ 0 ] === "right" ?
						data.elemWidth :
						0,
				atOffset = data.at[ 0 ] === "left" ?
					data.targetWidth :
					data.at[ 0 ] === "right" ?
						-data.targetWidth :
						0,
				offset = -2 * data.offset[ 0 ],
				newOverRight,
				newOverLeft;

			if ( overLeft < 0 ) {
				newOverRight = position.left + myOffset + atOffset + offset + data.collisionWidth -
					outerWidth - withinOffset;
				if ( newOverRight < 0 || newOverRight < abs( overLeft ) ) {
					position.left += myOffset + atOffset + offset;
				}
			} else if ( overRight > 0 ) {
				newOverLeft = position.left - data.collisionPosition.marginLeft + myOffset +
					atOffset + offset - offsetLeft;
				if ( newOverLeft > 0 || abs( newOverLeft ) < overRight ) {
					position.left += myOffset + atOffset + offset;
				}
			}
		},
		top: function( position, data ) {
			var within = data.within,
				withinOffset = within.offset.top + within.scrollTop,
				outerHeight = within.height,
				offsetTop = within.isWindow ? within.scrollTop : within.offset.top,
				collisionPosTop = position.top - data.collisionPosition.marginTop,
				overTop = collisionPosTop - offsetTop,
				overBottom = collisionPosTop + data.collisionHeight - outerHeight - offsetTop,
				top = data.my[ 1 ] === "top",
				myOffset = top ?
					-data.elemHeight :
					data.my[ 1 ] === "bottom" ?
						data.elemHeight :
						0,
				atOffset = data.at[ 1 ] === "top" ?
					data.targetHeight :
					data.at[ 1 ] === "bottom" ?
						-data.targetHeight :
						0,
				offset = -2 * data.offset[ 1 ],
				newOverTop,
				newOverBottom;
			if ( overTop < 0 ) {
				newOverBottom = position.top + myOffset + atOffset + offset + data.collisionHeight -
					outerHeight - withinOffset;
				if ( newOverBottom < 0 || newOverBottom < abs( overTop ) ) {
					position.top += myOffset + atOffset + offset;
				}
			} else if ( overBottom > 0 ) {
				newOverTop = position.top - data.collisionPosition.marginTop + myOffset + atOffset +
					offset - offsetTop;
				if ( newOverTop > 0 || abs( newOverTop ) < overBottom ) {
					position.top += myOffset + atOffset + offset;
				}
			}
		}
	},
	flipfit: {
		left: function() {
			$.ui.position.flip.left.apply( this, arguments );
			$.ui.position.fit.left.apply( this, arguments );
		},
		top: function() {
			$.ui.position.flip.top.apply( this, arguments );
			$.ui.position.fit.top.apply( this, arguments );
		}
	}
};

} )();

var position = $.ui.position;


/*!
 * jQuery UI Keycode 1.12.1
 * http://jqueryui.com
 *
 * Copyright jQuery Foundation and other contributors
 * Released under the MIT license.
 * http://jquery.org/license
 */

//>>label: Keycode
//>>group: Core
//>>description: Provide keycodes as keynames
//>>docs: http://api.jqueryui.com/jQuery.ui.keyCode/


var keycode = $.ui.keyCode = {
	BACKSPACE: 8,
	COMMA: 188,
	DELETE: 46,
	DOWN: 40,
	END: 35,
	ENTER: 13,
	ESCAPE: 27,
	HOME: 36,
	LEFT: 37,
	PAGE_DOWN: 34,
	PAGE_UP: 33,
	PERIOD: 190,
	RIGHT: 39,
	SPACE: 32,
	TAB: 9,
	UP: 38
};


/*!
 * jQuery UI Unique ID 1.12.1
 * http://jqueryui.com
 *
 * Copyright jQuery Foundation and other contributors
 * Released under the MIT license.
 * http://jquery.org/license
 */

//>>label: uniqueId
//>>group: Core
//>>description: Functions to generate and remove uniqueId's
//>>docs: http://api.jqueryui.com/uniqueId/



var uniqueId = $.fn.extend( {
	uniqueId: ( function() {
		var uuid = 0;

		return function() {
			return this.each( function() {
				if ( !this.id ) {
					this.id = "ui-id-" + ( ++uuid );
				}
			} );
		};
	} )(),

	removeUniqueId: function() {
		return this.each( function() {
			if ( /^ui-id-\d+$/.test( this.id ) ) {
				$( this ).removeAttr( "id" );
			}
		} );
	}
} );



var safeActiveElement = $.ui.safeActiveElement = function( document ) {
	var activeElement;

	// Support: IE 9 only
	// IE9 throws an "Unspecified error" accessing document.activeElement from an <iframe>
	try {
		activeElement = document.activeElement;
	} catch ( error ) {
		activeElement = document.body;
	}

	// Support: IE 9 - 11 only
	// IE may return null instead of an element
	// Interestingly, this only seems to occur when NOT in an iframe
	if ( !activeElement ) {
		activeElement = document.body;
	}

	// Support: IE 11 only
	// IE11 returns a seemingly empty object in some cases when accessing
	// document.activeElement from an <iframe>
	if ( !activeElement.nodeName ) {
		activeElement = document.body;
	}

	return activeElement;
};


/*!
 * jQuery UI Menu 1.12.1
 * http://jqueryui.com
 *
 * Copyright jQuery Foundation and other contributors
 * Released under the MIT license.
 * http://jquery.org/license
 */

//>>label: Menu
//>>group: Widgets
//>>description: Creates nestable menus.
//>>docs: http://api.jqueryui.com/menu/
//>>demos: http://jqueryui.com/menu/
//>>css.structure: ../../themes/base/core.css
//>>css.structure: ../../themes/base/menu.css
//>>css.theme: ../../themes/base/theme.css



var widgetsMenu = $.widget( "ui.menu", {
	version: "1.12.1",
	defaultElement: "<ul>",
	delay: 300,
	options: {
		icons: {
			submenu: "ui-icon-caret-1-e"
		},
		items: "> *",
		menus: "ul",
		position: {
			my: "left top",
			at: "right top"
		},
		role: "menu",

		// Callbacks
		blur: null,
		focus: null,
		select: null
	},

	_create: function() {
		this.activeMenu = this.element;

		// Flag used to prevent firing of the click handler
		// as the event bubbles up through nested menus
		this.mouseHandled = false;
		this.element
			.uniqueId()
			.attr( {
				role: this.options.role,
				tabIndex: 0
			} );

		this._addClass( "ui-menu", "ui-widget ui-widget-content" );
		this._on( {

			// Prevent focus from sticking to links inside menu after clicking
			// them (focus should always stay on UL during navigation).
			"mousedown .ui-menu-item": function( event ) {
				event.preventDefault();
			},
			"click .ui-menu-item": function( event ) {
				var target = $( event.target );
				var active = $( $.ui.safeActiveElement( this.document[ 0 ] ) );
				if ( !this.mouseHandled && target.not( ".ui-state-disabled" ).length ) {
					this.select( event );

					// Only set the mouseHandled flag if the event will bubble, see #9469.
					if ( !event.isPropagationStopped() ) {
						this.mouseHandled = true;
					}

					// Open submenu on click
					if ( target.has( ".ui-menu" ).length ) {
						this.expand( event );
					} else if ( !this.element.is( ":focus" ) &&
							active.closest( ".ui-menu" ).length ) {

						// Redirect focus to the menu
						this.element.trigger( "focus", [ true ] );

						// If the active item is on the top level, let it stay active.
						// Otherwise, blur the active item since it is no longer visible.
						if ( this.active && this.active.parents( ".ui-menu" ).length === 1 ) {
							clearTimeout( this.timer );
						}
					}
				}
			},
			"mouseenter .ui-menu-item": function( event ) {

				// Ignore mouse events while typeahead is active, see #10458.
				// Prevents focusing the wrong item when typeahead causes a scroll while the mouse
				// is over an item in the menu
				if ( this.previousFilter ) {
					return;
				}

				var actualTarget = $( event.target ).closest( ".ui-menu-item" ),
					target = $( event.currentTarget );

				// Ignore bubbled events on parent items, see #11641
				if ( actualTarget[ 0 ] !== target[ 0 ] ) {
					return;
				}

				// Remove ui-state-active class from siblings of the newly focused menu item
				// to avoid a jump caused by adjacent elements both having a class with a border
				this._removeClass( target.siblings().children( ".ui-state-active" ),
					null, "ui-state-active" );
				this.focus( event, target );
			},
			mouseleave: "collapseAll",
			"mouseleave .ui-menu": "collapseAll",
			focus: function( event, keepActiveItem ) {

				// If there's already an active item, keep it active
				// If not, activate the first item
				var item = this.active || this.element.find( this.options.items ).eq( 0 );

				if ( !keepActiveItem ) {
					this.focus( event, item );
				}
			},
			blur: function( event ) {
				this._delay( function() {
					var notContained = !$.contains(
						this.element[ 0 ],
						$.ui.safeActiveElement( this.document[ 0 ] )
					);
					if ( notContained ) {
						this.collapseAll( event );
					}
				} );
			},
			keydown: "_keydown"
		} );

		this.refresh();

		// Clicks outside of a menu collapse any open menus
		this._on( this.document, {
			click: function( event ) {
				if ( this._closeOnDocumentClick( event ) ) {
					this.collapseAll( event );
				}

				// Reset the mouseHandled flag
				this.mouseHandled = false;
			}
		} );
	},

	_destroy: function() {
		var items = this.element.find( ".ui-menu-item" )
				.removeAttr( "role aria-disabled" ),
			submenus = items.children( ".ui-menu-item-wrapper" )
				.removeUniqueId()
				.removeAttr( "tabIndex role aria-haspopup" );

		// Destroy (sub)menus
		this.element
			.removeAttr( "aria-activedescendant" )
			.find( ".ui-menu" ).addBack()
				.removeAttr( "role aria-labelledby aria-expanded aria-hidden aria-disabled " +
					"tabIndex" )
				.removeUniqueId()
				.show();

		submenus.children().each( function() {
			var elem = $( this );
			if ( elem.data( "ui-menu-submenu-caret" ) ) {
				elem.remove();
			}
		} );
	},

	_keydown: function( event ) {
		var match, prev, character, skip,
			preventDefault = true;

		switch ( event.keyCode ) {
		case $.ui.keyCode.PAGE_UP:
			this.previousPage( event );
			break;
		case $.ui.keyCode.PAGE_DOWN:
			this.nextPage( event );
			break;
		case $.ui.keyCode.HOME:
			this._move( "first", "first", event );
			break;
		case $.ui.keyCode.END:
			this._move( "last", "last", event );
			break;
		case $.ui.keyCode.UP:
			this.previous( event );
			break;
		case $.ui.keyCode.DOWN:
			this.next( event );
			break;
		case $.ui.keyCode.LEFT:
			this.collapse( event );
			break;
		case $.ui.keyCode.RIGHT:
			if ( this.active && !this.active.is( ".ui-state-disabled" ) ) {
				this.expand( event );
			}
			break;
		case $.ui.keyCode.ENTER:
		case $.ui.keyCode.SPACE:
			this._activate( event );
			break;
		case $.ui.keyCode.ESCAPE:
			this.collapse( event );
			break;
		default:
			preventDefault = false;
			prev = this.previousFilter || "";
			skip = false;

			// Support number pad values
			character = event.keyCode >= 96 && event.keyCode <= 105 ?
				( event.keyCode - 96 ).toString() : String.fromCharCode( event.keyCode );

			clearTimeout( this.filterTimer );

			if ( character === prev ) {
				skip = true;
			} else {
				character = prev + character;
			}

			match = this._filterMenuItems( character );
			match = skip && match.index( this.active.next() ) !== -1 ?
				this.active.nextAll( ".ui-menu-item" ) :
				match;

			// If no matches on the current filter, reset to the last character pressed
			// to move down the menu to the first item that starts with that character
			if ( !match.length ) {
				character = String.fromCharCode( event.keyCode );
				match = this._filterMenuItems( character );
			}

			if ( match.length ) {
				this.focus( event, match );
				this.previousFilter = character;
				this.filterTimer = this._delay( function() {
					delete this.previousFilter;
				}, 1000 );
			} else {
				delete this.previousFilter;
			}
		}

		if ( preventDefault ) {
			event.preventDefault();
		}
	},

	_activate: function( event ) {
		if ( this.active && !this.active.is( ".ui-state-disabled" ) ) {
			if ( this.active.children( "[aria-haspopup='true']" ).length ) {
				this.expand( event );
			} else {
				this.select( event );
			}
		}
	},

	refresh: function() {
		var menus, items, newSubmenus, newItems, newWrappers,
			that = this,
			icon = this.options.icons.submenu,
			submenus = this.element.find( this.options.menus );

		this._toggleClass( "ui-menu-icons", null, !!this.element.find( ".ui-icon" ).length );

		// Initialize nested menus
		newSubmenus = submenus.filter( ":not(.ui-menu)" )
			.hide()
			.attr( {
				role: this.options.role,
				"aria-hidden": "true",
				"aria-expanded": "false"
			} )
			.each( function() {
				var menu = $( this ),
					item = menu.prev(),
					submenuCaret = $( "<span>" ).data( "ui-menu-submenu-caret", true );

				that._addClass( submenuCaret, "ui-menu-icon", "ui-icon " + icon );
				item
					.attr( "aria-haspopup", "true" )
					.prepend( submenuCaret );
				menu.attr( "aria-labelledby", item.attr( "id" ) );
			} );

		this._addClass( newSubmenus, "ui-menu", "ui-widget ui-widget-content ui-front" );

		menus = submenus.add( this.element );
		items = menus.find( this.options.items );

		// Initialize menu-items containing spaces and/or dashes only as dividers
		items.not( ".ui-menu-item" ).each( function() {
			var item = $( this );
			if ( that._isDivider( item ) ) {
				that._addClass( item, "ui-menu-divider", "ui-widget-content" );
			}
		} );

		// Don't refresh list items that are already adapted
		newItems = items.not( ".ui-menu-item, .ui-menu-divider" );
		newWrappers = newItems.children()
			.not( ".ui-menu" )
				.uniqueId()
				.attr( {
					tabIndex: -1,
					role: this._itemRole()
				} );
		this._addClass( newItems, "ui-menu-item" )
			._addClass( newWrappers, "ui-menu-item-wrapper" );

		// Add aria-disabled attribute to any disabled menu item
		items.filter( ".ui-state-disabled" ).attr( "aria-disabled", "true" );

		// If the active item has been removed, blur the menu
		if ( this.active && !$.contains( this.element[ 0 ], this.active[ 0 ] ) ) {
			this.blur();
		}
	},

	_itemRole: function() {
		return {
			menu: "menuitem",
			listbox: "option"
		}[ this.options.role ];
	},

	_setOption: function( key, value ) {
		if ( key === "icons" ) {
			var icons = this.element.find( ".ui-menu-icon" );
			this._removeClass( icons, null, this.options.icons.submenu )
				._addClass( icons, null, value.submenu );
		}
		this._super( key, value );
	},

	_setOptionDisabled: function( value ) {
		this._super( value );

		this.element.attr( "aria-disabled", String( value ) );
		this._toggleClass( null, "ui-state-disabled", !!value );
	},

	focus: function( event, item ) {
		var nested, focused, activeParent;
		this.blur( event, event && event.type === "focus" );

		this._scrollIntoView( item );

		this.active = item.first();

		focused = this.active.children( ".ui-menu-item-wrapper" );
		this._addClass( focused, null, "ui-state-active" );

		// Only update aria-activedescendant if there's a role
		// otherwise we assume focus is managed elsewhere
		if ( this.options.role ) {
			this.element.attr( "aria-activedescendant", focused.attr( "id" ) );
		}

		// Highlight active parent menu item, if any
		activeParent = this.active
			.parent()
				.closest( ".ui-menu-item" )
					.children( ".ui-menu-item-wrapper" );
		this._addClass( activeParent, null, "ui-state-active" );

		if ( event && event.type === "keydown" ) {
			this._close();
		} else {
			this.timer = this._delay( function() {
				this._close();
			}, this.delay );
		}

		nested = item.children( ".ui-menu" );
		if ( nested.length && event && ( /^mouse/.test( event.type ) ) ) {
			this._startOpening( nested );
		}
		this.activeMenu = item.parent();

		this._trigger( "focus", event, { item: item } );
	},

	_scrollIntoView: function( item ) {
		var borderTop, paddingTop, offset, scroll, elementHeight, itemHeight;
		if ( this._hasScroll() ) {
			borderTop = parseFloat( $.css( this.activeMenu[ 0 ], "borderTopWidth" ) ) || 0;
			paddingTop = parseFloat( $.css( this.activeMenu[ 0 ], "paddingTop" ) ) || 0;
			offset = item.offset().top - this.activeMenu.offset().top - borderTop - paddingTop;
			scroll = this.activeMenu.scrollTop();
			elementHeight = this.activeMenu.height();
			itemHeight = item.outerHeight();

			if ( offset < 0 ) {
				this.activeMenu.scrollTop( scroll + offset );
			} else if ( offset + itemHeight > elementHeight ) {
				this.activeMenu.scrollTop( scroll + offset - elementHeight + itemHeight );
			}
		}
	},

	blur: function( event, fromFocus ) {
		if ( !fromFocus ) {
			clearTimeout( this.timer );
		}

		if ( !this.active ) {
			return;
		}

		this._removeClass( this.active.children( ".ui-menu-item-wrapper" ),
			null, "ui-state-active" );

		this._trigger( "blur", event, { item: this.active } );
		this.active = null;
	},

	_startOpening: function( submenu ) {
		clearTimeout( this.timer );

		// Don't open if already open fixes a Firefox bug that caused a .5 pixel
		// shift in the submenu position when mousing over the caret icon
		if ( submenu.attr( "aria-hidden" ) !== "true" ) {
			return;
		}

		this.timer = this._delay( function() {
			this._close();
			this._open( submenu );
		}, this.delay );
	},

	_open: function( submenu ) {
		var position = $.extend( {
			of: this.active
		}, this.options.position );

		clearTimeout( this.timer );
		this.element.find( ".ui-menu" ).not( submenu.parents( ".ui-menu" ) )
			.hide()
			.attr( "aria-hidden", "true" );

		submenu
			.show()
			.removeAttr( "aria-hidden" )
			.attr( "aria-expanded", "true" )
			.position( position );
	},

	collapseAll: function( event, all ) {
		clearTimeout( this.timer );
		this.timer = this._delay( function() {

			// If we were passed an event, look for the submenu that contains the event
			var currentMenu = all ? this.element :
				$( event && event.target ).closest( this.element.find( ".ui-menu" ) );

			// If we found no valid submenu ancestor, use the main menu to close all
			// sub menus anyway
			if ( !currentMenu.length ) {
				currentMenu = this.element;
			}

			this._close( currentMenu );

			this.blur( event );

			// Work around active item staying active after menu is blurred
			this._removeClass( currentMenu.find( ".ui-state-active" ), null, "ui-state-active" );

			this.activeMenu = currentMenu;
		}, this.delay );
	},

	// With no arguments, closes the currently active menu - if nothing is active
	// it closes all menus.  If passed an argument, it will search for menus BELOW
	_close: function( startMenu ) {
		if ( !startMenu ) {
			startMenu = this.active ? this.active.parent() : this.element;
		}

		startMenu.find( ".ui-menu" )
			.hide()
			.attr( "aria-hidden", "true" )
			.attr( "aria-expanded", "false" );
	},

	_closeOnDocumentClick: function( event ) {
		return !$( event.target ).closest( ".ui-menu" ).length;
	},

	_isDivider: function( item ) {

		// Match hyphen, em dash, en dash
		return !/[^\-\u2014\u2013\s]/.test( item.text() );
	},

	collapse: function( event ) {
		var newItem = this.active &&
			this.active.parent().closest( ".ui-menu-item", this.element );
		if ( newItem && newItem.length ) {
			this._close();
			this.focus( event, newItem );
		}
	},

	expand: function( event ) {
		var newItem = this.active &&
			this.active
				.children( ".ui-menu " )
					.find( this.options.items )
						.first();

		if ( newItem && newItem.length ) {
			this._open( newItem.parent() );

			// Delay so Firefox will not hide activedescendant change in expanding submenu from AT
			this._delay( function() {
				this.focus( event, newItem );
			} );
		}
	},

	next: function( event ) {
		this._move( "next", "first", event );
	},

	previous: function( event ) {
		this._move( "prev", "last", event );
	},

	isFirstItem: function() {
		return this.active && !this.active.prevAll( ".ui-menu-item" ).length;
	},

	isLastItem: function() {
		return this.active && !this.active.nextAll( ".ui-menu-item" ).length;
	},

	_move: function( direction, filter, event ) {
		var next;
		if ( this.active ) {
			if ( direction === "first" || direction === "last" ) {
				next = this.active
					[ direction === "first" ? "prevAll" : "nextAll" ]( ".ui-menu-item" )
					.eq( -1 );
			} else {
				next = this.active
					[ direction + "All" ]( ".ui-menu-item" )
					.eq( 0 );
			}
		}
		if ( !next || !next.length || !this.active ) {
			next = this.activeMenu.find( this.options.items )[ filter ]();
		}

		this.focus( event, next );
	},

	nextPage: function( event ) {
		var item, base, height;

		if ( !this.active ) {
			this.next( event );
			return;
		}
		if ( this.isLastItem() ) {
			return;
		}
		if ( this._hasScroll() ) {
			base = this.active.offset().top;
			height = this.element.height();
			this.active.nextAll( ".ui-menu-item" ).each( function() {
				item = $( this );
				return item.offset().top - base - height < 0;
			} );

			this.focus( event, item );
		} else {
			this.focus( event, this.activeMenu.find( this.options.items )
				[ !this.active ? "first" : "last" ]() );
		}
	},

	previousPage: function( event ) {
		var item, base, height;
		if ( !this.active ) {
			this.next( event );
			return;
		}
		if ( this.isFirstItem() ) {
			return;
		}
		if ( this._hasScroll() ) {
			base = this.active.offset().top;
			height = this.element.height();
			this.active.prevAll( ".ui-menu-item" ).each( function() {
				item = $( this );
				return item.offset().top - base + height > 0;
			} );

			this.focus( event, item );
		} else {
			this.focus( event, this.activeMenu.find( this.options.items ).first() );
		}
	},

	_hasScroll: function() {
		return this.element.outerHeight() < this.element.prop( "scrollHeight" );
	},

	select: function( event ) {

		// TODO: It should never be possible to not have an active item at this
		// point, but the tests don't trigger mouseenter before click.
		this.active = this.active || $( event.target ).closest( ".ui-menu-item" );
		var ui = { item: this.active };
		if ( !this.active.has( ".ui-menu" ).length ) {
			this.collapseAll( event, true );
		}
		this._trigger( "select", event, ui );
	},

	_filterMenuItems: function( character ) {
		var escapedCharacter = character.replace( /[\-\[\]{}()*+?.,\\\^$|#\s]/g, "\\$&" ),
			regex = new RegExp( "^" + escapedCharacter, "i" );

		return this.activeMenu
			.find( this.options.items )

				// Only match on items, not dividers or other content (#10571)
				.filter( ".ui-menu-item" )
					.filter( function() {
						return regex.test(
							$.trim( $( this ).children( ".ui-menu-item-wrapper" ).text() ) );
					} );
	}
} );


/*!
 * jQuery UI Autocomplete 1.12.1
 * http://jqueryui.com
 *
 * Copyright jQuery Foundation and other contributors
 * Released under the MIT license.
 * http://jquery.org/license
 */

//>>label: Autocomplete
//>>group: Widgets
//>>description: Lists suggested words as the user is typing.
//>>docs: http://api.jqueryui.com/autocomplete/
//>>demos: http://jqueryui.com/autocomplete/
//>>css.structure: ../../themes/base/core.css
//>>css.structure: ../../themes/base/autocomplete.css
//>>css.theme: ../../themes/base/theme.css



$.widget( "ui.autocomplete", {
	version: "1.12.1",
	defaultElement: "<input>",
	options: {
		appendTo: null,
		autoFocus: false,
		delay: 300,
		minLength: 1,
		position: {
			my: "left top",
			at: "left bottom",
			collision: "none"
		},
		source: null,

		// Callbacks
		change: null,
		close: null,
		focus: null,
		open: null,
		response: null,
		search: null,
		select: null
	},

	requestIndex: 0,
	pending: 0,

	_create: function() {

		// Some browsers only repeat keydown events, not keypress events,
		// so we use the suppressKeyPress flag to determine if we've already
		// handled the keydown event. #7269
		// Unfortunately the code for & in keypress is the same as the up arrow,
		// so we use the suppressKeyPressRepeat flag to avoid handling keypress
		// events when we know the keydown event was used to modify the
		// search term. #7799
		var suppressKeyPress, suppressKeyPressRepeat, suppressInput,
			nodeName = this.element[ 0 ].nodeName.toLowerCase(),
			isTextarea = nodeName === "textarea",
			isInput = nodeName === "input";

		// Textareas are always multi-line
		// Inputs are always single-line, even if inside a contentEditable element
		// IE also treats inputs as contentEditable
		// All other element types are determined by whether or not they're contentEditable
		this.isMultiLine = isTextarea || !isInput && this._isContentEditable( this.element );

		this.valueMethod = this.element[ isTextarea || isInput ? "val" : "text" ];
		this.isNewMenu = true;

		this._addClass( "ui-autocomplete-input" );
		this.element.attr( "autocomplete", "off" );

		this._on( this.element, {
			keydown: function( event ) {
				if ( this.element.prop( "readOnly" ) ) {
					suppressKeyPress = true;
					suppressInput = true;
					suppressKeyPressRepeat = true;
					return;
				}

				suppressKeyPress = false;
				suppressInput = false;
				suppressKeyPressRepeat = false;
				var keyCode = $.ui.keyCode;
				switch ( event.keyCode ) {
				case keyCode.PAGE_UP:
					suppressKeyPress = true;
					this._move( "previousPage", event );
					break;
				case keyCode.PAGE_DOWN:
					suppressKeyPress = true;
					this._move( "nextPage", event );
					break;
				case keyCode.UP:
					suppressKeyPress = true;
					this._keyEvent( "previous", event );
					break;
				case keyCode.DOWN:
					suppressKeyPress = true;
					this._keyEvent( "next", event );
					break;
				case keyCode.ENTER:

					// when menu is open and has focus
					if ( this.menu.active ) {

						// #6055 - Opera still allows the keypress to occur
						// which causes forms to submit
						suppressKeyPress = true;
						event.preventDefault();
						this.menu.select( event );
					}
					break;
				case keyCode.TAB:
					if ( this.menu.active ) {
						this.menu.select( event );
					}
					break;
				case keyCode.ESCAPE:
					if ( this.menu.element.is( ":visible" ) ) {
						if ( !this.isMultiLine ) {
							this._value( this.term );
						}
						this.close( event );

						// Different browsers have different default behavior for escape
						// Single press can mean undo or clear
						// Double press in IE means clear the whole form
						event.preventDefault();
					}
					break;
				default:
					suppressKeyPressRepeat = true;

					// search timeout should be triggered before the input value is changed
					this._searchTimeout( event );
					break;
				}
			},
			keypress: function( event ) {
				if ( suppressKeyPress ) {
					suppressKeyPress = false;
					if ( !this.isMultiLine || this.menu.element.is( ":visible" ) ) {
						event.preventDefault();
					}
					return;
				}
				if ( suppressKeyPressRepeat ) {
					return;
				}

				// Replicate some key handlers to allow them to repeat in Firefox and Opera
				var keyCode = $.ui.keyCode;
				switch ( event.keyCode ) {
				case keyCode.PAGE_UP:
					this._move( "previousPage", event );
					break;
				case keyCode.PAGE_DOWN:
					this._move( "nextPage", event );
					break;
				case keyCode.UP:
					this._keyEvent( "previous", event );
					break;
				case keyCode.DOWN:
					this._keyEvent( "next", event );
					break;
				}
			},
			input: function( event ) {
				if ( suppressInput ) {
					suppressInput = false;
					event.preventDefault();
					return;
				}
				this._searchTimeout( event );
			},
			focus: function() {
				this.selectedItem = null;
				this.previous = this._value();
			},
			blur: function( event ) {
				if ( this.cancelBlur ) {
					delete this.cancelBlur;
					return;
				}

				clearTimeout( this.searching );
				this.close( event );
				this._change( event );
			}
		} );

		this._initSource();
		this.menu = $( "<ul>" )
			.appendTo( this._appendTo() )
			.menu( {

				// disable ARIA support, the live region takes care of that
				role: null
			} )
			.hide()
			.menu( "instance" );

		this._addClass( this.menu.element, "ui-autocomplete", "ui-front" );
		this._on( this.menu.element, {
			mousedown: function( event ) {

				// prevent moving focus out of the text field
				event.preventDefault();

				// IE doesn't prevent moving focus even with event.preventDefault()
				// so we set a flag to know when we should ignore the blur event
				this.cancelBlur = true;
				this._delay( function() {
					delete this.cancelBlur;

					// Support: IE 8 only
					// Right clicking a menu item or selecting text from the menu items will
					// result in focus moving out of the input. However, we've already received
					// and ignored the blur event because of the cancelBlur flag set above. So
					// we restore focus to ensure that the menu closes properly based on the user's
					// next actions.
					if ( this.element[ 0 ] !== $.ui.safeActiveElement( this.document[ 0 ] ) ) {
						this.element.trigger( "focus" );
					}
				} );
			},
			menufocus: function( event, ui ) {
				var label, item;

				// support: Firefox
				// Prevent accidental activation of menu items in Firefox (#7024 #9118)
				if ( this.isNewMenu ) {
					this.isNewMenu = false;
					if ( event.originalEvent && /^mouse/.test( event.originalEvent.type ) ) {
						this.menu.blur();

						this.document.one( "mousemove", function() {
							$( event.target ).trigger( event.originalEvent );
						} );

						return;
					}
				}

				item = ui.item.data( "ui-autocomplete-item" );
				if ( false !== this._trigger( "focus", event, { item: item } ) ) {

					// use value to match what will end up in the input, if it was a key event
					if ( event.originalEvent && /^key/.test( event.originalEvent.type ) ) {
						this._value( item.value );
					}
				}

				// Announce the value in the liveRegion
				label = ui.item.attr( "aria-label" ) || item.value;
				if ( label && $.trim( label ).length ) {
					this.liveRegion.children().hide();
					$( "<div>" ).text( label ).appendTo( this.liveRegion );
				}
			},
			menuselect: function( event, ui ) {
				var item = ui.item.data( "ui-autocomplete-item" ),
					previous = this.previous;

				// Only trigger when focus was lost (click on menu)
				if ( this.element[ 0 ] !== $.ui.safeActiveElement( this.document[ 0 ] ) ) {
					this.element.trigger( "focus" );
					this.previous = previous;

					// #6109 - IE triggers two focus events and the second
					// is asynchronous, so we need to reset the previous
					// term synchronously and asynchronously :-(
					this._delay( function() {
						this.previous = previous;
						this.selectedItem = item;
					} );
				}

				if ( false !== this._trigger( "select", event, { item: item } ) ) {
					this._value( item.value );
				}

				// reset the term after the select event
				// this allows custom select handling to work properly
				this.term = this._value();

				this.close( event );
				this.selectedItem = item;
			}
		} );

		this.liveRegion = $( "<div>", {
			role: "status",
			"aria-live": "assertive",
			"aria-relevant": "additions"
		} )
			.appendTo( this.document[ 0 ].body );

		this._addClass( this.liveRegion, null, "ui-helper-hidden-accessible" );

		// Turning off autocomplete prevents the browser from remembering the
		// value when navigating through history, so we re-enable autocomplete
		// if the page is unloaded before the widget is destroyed. #7790
		this._on( this.window, {
			beforeunload: function() {
				this.element.removeAttr( "autocomplete" );
			}
		} );
	},

	_destroy: function() {
		clearTimeout( this.searching );
		this.element.removeAttr( "autocomplete" );
		this.menu.element.remove();
		this.liveRegion.remove();
	},

	_setOption: function( key, value ) {
		this._super( key, value );
		if ( key === "source" ) {
			this._initSource();
		}
		if ( key === "appendTo" ) {
			this.menu.element.appendTo( this._appendTo() );
		}
		if ( key === "disabled" && value && this.xhr ) {
			this.xhr.abort();
		}
	},

	_isEventTargetInWidget: function( event ) {
		var menuElement = this.menu.element[ 0 ];

		return event.target === this.element[ 0 ] ||
			event.target === menuElement ||
			$.contains( menuElement, event.target );
	},

	_closeOnClickOutside: function( event ) {
		if ( !this._isEventTargetInWidget( event ) ) {
			this.close();
		}
	},

	_appendTo: function() {
		var element = this.options.appendTo;

		if ( element ) {
			element = element.jquery || element.nodeType ?
				$( element ) :
				this.document.find( element ).eq( 0 );
		}

		if ( !element || !element[ 0 ] ) {
			element = this.element.closest( ".ui-front, dialog" );
		}

		if ( !element.length ) {
			element = this.document[ 0 ].body;
		}

		return element;
	},

	_initSource: function() {
		var array, url,
			that = this;
		if ( $.isArray( this.options.source ) ) {
			array = this.options.source;
			this.source = function( request, response ) {
				response( $.ui.autocomplete.filter( array, request.term ) );
			};
		} else if ( typeof this.options.source === "string" ) {
			url = this.options.source;
			this.source = function( request, response ) {
				if ( that.xhr ) {
					that.xhr.abort();
				}
				that.xhr = $.ajax( {
					url: url,
					data: request,
					dataType: "json",
					success: function( data ) {
						response( data );
					},
					error: function() {
						response( [] );
					}
				} );
			};
		} else {
			this.source = this.options.source;
		}
	},

	_searchTimeout: function( event ) {
		clearTimeout( this.searching );
		this.searching = this._delay( function() {

			// Search if the value has changed, or if the user retypes the same value (see #7434)
			var equalValues = this.term === this._value(),
				menuVisible = this.menu.element.is( ":visible" ),
				modifierKey = event.altKey || event.ctrlKey || event.metaKey || event.shiftKey;

			if ( !equalValues || ( equalValues && !menuVisible && !modifierKey ) ) {
				this.selectedItem = null;
				this.search( null, event );
			}
		}, this.options.delay );
	},

	search: function( value, event ) {
		value = value != null ? value : this._value();

		// Always save the actual value, not the one passed as an argument
		this.term = this._value();

		if ( value.length < this.options.minLength ) {
			return this.close( event );
		}

		if ( this._trigger( "search", event ) === false ) {
			return;
		}

		return this._search( value );
	},

	_search: function( value ) {
		this.pending++;
		this._addClass( "ui-autocomplete-loading" );
		this.cancelSearch = false;

		this.source( { term: value }, this._response() );
	},

	_response: function() {
		var index = ++this.requestIndex;

		return $.proxy( function( content ) {
			if ( index === this.requestIndex ) {
				this.__response( content );
			}

			this.pending--;
			if ( !this.pending ) {
				this._removeClass( "ui-autocomplete-loading" );
			}
		}, this );
	},

	__response: function( content ) {
		if ( content ) {
			content = this._normalize( content );
		}
		this._trigger( "response", null, { content: content } );
		if ( !this.options.disabled && content && content.length && !this.cancelSearch ) {
			this._suggest( content );
			this._trigger( "open" );
		} else {

			// use ._close() instead of .close() so we don't cancel future searches
			this._close();
		}
	},

	close: function( event ) {
		this.cancelSearch = true;
		this._close( event );
	},

	_close: function( event ) {

		// Remove the handler that closes the menu on outside clicks
		this._off( this.document, "mousedown" );

		if ( this.menu.element.is( ":visible" ) ) {
			this.menu.element.hide();
			this.menu.blur();
			this.isNewMenu = true;
			this._trigger( "close", event );
		}
	},

	_change: function( event ) {
		if ( this.previous !== this._value() ) {
			this._trigger( "change", event, { item: this.selectedItem } );
		}
	},

	_normalize: function( items ) {

		// assume all items have the right format when the first item is complete
		if ( items.length && items[ 0 ].label && items[ 0 ].value ) {
			return items;
		}
		return $.map( items, function( item ) {
			if ( typeof item === "string" ) {
				return {
					label: item,
					value: item
				};
			}
			return $.extend( {}, item, {
				label: item.label || item.value,
				value: item.value || item.label
			} );
		} );
	},

	_suggest: function( items ) {
		var ul = this.menu.element.empty();
		this._renderMenu( ul, items );
		this.isNewMenu = true;
		this.menu.refresh();

		// Size and position menu
		ul.show();
		this._resizeMenu();
		ul.position( $.extend( {
			of: this.element
		}, this.options.position ) );

		if ( this.options.autoFocus ) {
			this.menu.next();
		}

		// Listen for interactions outside of the widget (#6642)
		this._on( this.document, {
			mousedown: "_closeOnClickOutside"
		} );
	},

	_resizeMenu: function() {
		var ul = this.menu.element;
		ul.outerWidth( Math.max(

			// Firefox wraps long text (possibly a rounding bug)
			// so we add 1px to avoid the wrapping (#7513)
			ul.width( "" ).outerWidth() + 1,
			this.element.outerWidth()
		) );
	},

	_renderMenu: function( ul, items ) {
		var that = this;
		$.each( items, function( index, item ) {
			that._renderItemData( ul, item );
		} );
	},

	_renderItemData: function( ul, item ) {
		return this._renderItem( ul, item ).data( "ui-autocomplete-item", item );
	},

	_renderItem: function( ul, item ) {
		return $( "<li>" )
			.append( $( "<div>" ).text( item.label ) )
			.appendTo( ul );
	},

	_move: function( direction, event ) {
		if ( !this.menu.element.is( ":visible" ) ) {
			this.search( null, event );
			return;
		}
		if ( this.menu.isFirstItem() && /^previous/.test( direction ) ||
				this.menu.isLastItem() && /^next/.test( direction ) ) {

			if ( !this.isMultiLine ) {
				this._value( this.term );
			}

			this.menu.blur();
			return;
		}
		this.menu[ direction ]( event );
	},

	widget: function() {
		return this.menu.element;
	},

	_value: function() {
		return this.valueMethod.apply( this.element, arguments );
	},

	_keyEvent: function( keyEvent, event ) {
		if ( !this.isMultiLine || this.menu.element.is( ":visible" ) ) {
			this._move( keyEvent, event );

			// Prevents moving cursor to beginning/end of the text field in some browsers
			event.preventDefault();
		}
	},

	// Support: Chrome <=50
	// We should be able to just use this.element.prop( "isContentEditable" )
	// but hidden elements always report false in Chrome.
	// https://code.google.com/p/chromium/issues/detail?id=313082
	_isContentEditable: function( element ) {
		if ( !element.length ) {
			return false;
		}

		var editable = element.prop( "contentEditable" );

		if ( editable === "inherit" ) {
		  return this._isContentEditable( element.parent() );
		}

		return editable === "true";
	}
} );

$.extend( $.ui.autocomplete, {
	escapeRegex: function( value ) {
		return value.replace( /[\-\[\]{}()*+?.,\\\^$|#\s]/g, "\\$&" );
	},
	filter: function( array, term ) {
		var matcher = new RegExp( $.ui.autocomplete.escapeRegex( term ), "i" );
		return $.grep( array, function( value ) {
			return matcher.test( value.label || value.value || value );
		} );
	}
} );

// Live region extension, adding a `messages` option
// NOTE: This is an experimental API. We are still investigating
// a full solution for string manipulation and internationalization.
$.widget( "ui.autocomplete", $.ui.autocomplete, {
	options: {
		messages: {
			noResults: "No search results.",
			results: function( amount ) {
				return amount + ( amount > 1 ? " results are" : " result is" ) +
					" available, use up and down arrow keys to navigate.";
			}
		}
	},

	__response: function( content ) {
		var message;
		this._superApply( arguments );
		if ( this.options.disabled || this.cancelSearch ) {
			return;
		}
		if ( content && content.length ) {
			message = this.options.messages.results( content.length );
		} else {
			message = this.options.messages.noResults;
		}
		this.liveRegion.children().hide();
		$( "<div>" ).text( message ).appendTo( this.liveRegion );
	}
} );

var widgetsAutocomplete = $.ui.autocomplete;




}));
