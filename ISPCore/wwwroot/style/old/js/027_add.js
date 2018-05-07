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
            $.sl('load', '/settings/remove/whitelist', { back: false, ignore: true, data: { id: newId }, dataType: 'json' })
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
		ReqMinuteToString: '',
		Id: 0
	}
	$.extend(data, option || {})

	var id = new Date().getTime() + Math.round(Math.random() * 1000);
	var code = $([
		'<tr id="c' + id + '">',
		'<input type="hidden" class="set-id-val" name="aliases[' + id + '].Id" value="' + data.Id + '" />',
		'<td></td>',
        '<td><div class="add-ads-input-light"><input class="name" name="aliases[' + id + '].host" value="' + data.host +'"></div></td>',
        '<td><div class="add-ads-input-light"><input class="name" name="aliases[' + id + '].Folder" value="'+(data.Folder || '')+'"></div></td>',
		'<td style="text-align: right;" class="table-products btn-icons">',
		'<a href="/requests-filter/monitoring?ShowHost=' + data.host + '" style="' + (data.host ? '' : 'display: none') + '" onclick="return loadPage(this)" class="btn"><i class="fa fa-bar-chart"></i></a>',
		'<a class="btn delete"><i class="fa fa-trash-o"></i></a></td>',
		'</tr>'
    ].join(''));
    
    var codeEmpty = $([
        '<tr id="c' + id + '">',
		'<td>' + (data.ReqMinuteToString || '0') + '</td>',
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


