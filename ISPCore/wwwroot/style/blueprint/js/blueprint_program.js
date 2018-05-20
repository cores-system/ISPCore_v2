Blueprint.classes.Program = function(){
	this.data        = {
		main: {}
	};

	this._blueprints = [];

	this._empty = {
		uid: 'main',
		name: 'Main',
		change: 0,
		data: {}
	};

	Arrays.extend(this.data.main, this._empty);
	
	
	this.editor = ace.edit("ace-editor-code");
	this.editor.setTheme("ace/theme/crimson_editor");

	var CssMode = ace.require("ace/mode/csharp").Mode;

	this.editor.session.setMode(new CssMode());
	this.editor.session.setUseWorker(false)
	this.editor.setShowPrintMargin(false);

	this.editor.$blockScrolling = Infinity;
}

Object.assign( Blueprint.classes.Program.prototype, EventDispatcher.prototype, {
	_getBlueprint: function(uid){
		for(var i = 0; i < this._blueprints.length; i++){
			var blueprint = this._blueprints[i];

			if(blueprint.uid == uid){
				return blueprint; break;
			}
		}
		
	},
	fireChangeEvent: function(){

	},
	/**
	 * Из блюпринта вытаскивае все дынные
	 * @param {string} uid - айди блюпринта
	 */
	nodeData: function(uid){
		return this.data[uid].data;
	},
	/**
	 * Показываем какие параметры есть у нода
	 * @param {Object} event
	 * @param {Class}  event.target - ссылка на нод в блюпринте
	 * @param {Object} event.data   - все данные о ноде
	 * @param {string} event.uid    - айди нода
	 */
	nodeOption: function(event){
		var options = $('<div></div>'),
			worker  = Blueprint.Worker.get(event.data.worker),
			html;
			
		var self = this;

		var field = function(entrance, group, name, params){
			if((name == 'input' || name == 'output') && !params.enableChange) return;

			if(params.disableChange) return;

			if(typeof params.type == 'function'){
				arguments.push = [].push;
				arguments.push(event)

				//запускаем функцию, но не из параметров а из воркера, потому что так надо чуваки!
				html = worker.params.vars[entrance][name].type.apply(null, arguments);
			}
			else{
				if(params.type == 'fileSave' || params.type == 'fileOpen' || params.type == 'fileDir' || params.type == 'fileOpenMultiple'){
					html = $([
						'<div class="form-field form-field-icon m-b-5">',
							'<div>',
								'<div class="form-btn form-btn-icon m-r-5"><img src="style/img/icons-panel/search.png" /></div>',
							'</div>',
							'<div>',
								'<div class="form-input">',
                                    '<input type="text" name="background-position" value="" disabled placeholder="" />',
                                '</div>',
							'</div>',
                        '</div>',
					].join(''))

					var path  = event.target.getValue(entrance, name);
					var input = $('input',html).val(path);

					$('.form-btn',html).on('click',function(){
			            File.Choise(params.type,function(file){
			            	event.target.setValue(entrance, name, file);

			                input.val(file)
			            },nw.path.dirname(path))
			        });
				}
				else if(params.type == 'code'){
					$('#ace-editor-save').unbind().on('click',function(){
						event.target.setValue(entrance, name, self.editor.getValue());
						
						$('#modal-editor').modal('hide');
					})
					
					html = $([
						'<div class="m-b-5">',
	                        '<button class="btn btn-success btn-block">edit code</button>',
		                '</div>',
					].join(''))
					
					$('.btn',html).on('click',function(){
						$('#modal-editor').modal('show');
					
						self.editor.setValue(event.target.getValue(entrance, name), -1) // moves cursor to the start
						self.editor.setValue(event.target.getValue(entrance, name), 1) // moves cursor to the end
					})
				}
				else{
					
					
					var input    = '<input type="text" class="form-control" name="'+name+'" placeholder="'+(params.placeholder || (params.name || name))+'" />';
					var textarea = '<textarea class="form-control" rows="4" placeholder="'+(params.placeholder || (params.name || name))+'"></textarea>';
					
					var field = params.textarea ? textarea : input;
					
					html = $([
						'<div class="m-b-5">',
	                        '<div>',
								field,
	                        '</div>',
		                '</div>',
					].join(''))
					
					$('.form-control',html).val(event.target.getValue(entrance, name));

					var change = function(inputName, inputValue){
						if(inputValue == undefined) inputValue= '';

						event.target.setValue(entrance, inputName, inputValue);
					}
					
					$('.form-control',html).on('change',function(){
						change(name,$(this).val())
					})

					//Form.InputChangeEvent($('.form-input',html), change, change);
				}
			}

			group.append(html);
		}

		var group = function(entrance){
			var html = $([
				'<div class="form-group group-'+entrance+' p-b-5">',
                    '<div class="form-name">Значения ('+ Blueprint.Utility.capitalizeFirstLetter(entrance)+')</div>',
                    '<div class="form-content">',
                        
                    '</div>',
                '</div>',
			].join(''));

			options.append(html)

			return $('.form-content',html);
		}


		var vars        = event.target.params.vars;
		var description = $('<p class="text-center m-b-20">'+(event.target.params.description || '-')+'</p>')

		options.append(description)

		//формируем группы
		var input  = group('input'),
			output = group('output');

		$.each(vars.input,function(name,params){
			field('input', input, name, params);
		})

		$.each(vars.output,function(name,params){
			field('output', output, name, params);
		})

		//если в блоках пусто то зачем их показывать а?
		if(!$('*',input).length)  $('.group-input',options).remove();
		if(!$('*',output).length) $('.group-output',options).remove();

		$('#blueprint-node-option').empty().append(options);
		
	},

	_processStart: function(){
		$('.blueprint-process').addClass('active');

		var uids = [];

		for(var i in this.data){
			var nodes = this.data[i].data.nodes;

			for(var a in nodes){
				var node = nodes[a];

				if(node.worker == 'blueprint'){
					var uid = node.userData.blueprintUid;

					if(uids.indexOf(uid) == -1) uids.push(uid);
				}
			}
		}

		uids.push('main');

		$('.blueprint-process-title').text('Процесс 0 из '+uids.length);

		this._process_total = uids.length;
		this._process_uids  = uids;
	},

	_processComplite: function(uid){
		Arrays.remove(this._process_uids, uid);

		var now = this._process_total - this._process_uids.length;

		$('.blueprint-process-title').text('Процесс '+now+' из '+this._process_total)

		$('.blueprint-process-bar-inside').css({
			width: Math.round(now / this._process_total * 100) + '%'
		});
	},

	_processEnd: function(){
		//поcтавил таймер да бы видеть что была компиляция
		setTimeout(function(){
			$('.blueprint-process').removeClass('active');
		},10)
	},

	//строим
	projectBuild: function(){
		this._processStart();

		this.blueprintBuild('main');

		this._processEnd();
	},

	//если сработал эвент сохранения то тоже сохраняем
	projectSave: function(){
		this.projectBuild();
	},
	/**
	 * Строим и строим
	 * @param {string} uid
	 */
	blueprintBuild: function(uid){
		try{
			this._processComplite(uid);
			
			Blueprint.Worker.build(uid, this.data[uid].data.nodes);
		}
		catch(err){
			Console.Add(err)
		}
	},
	/**
	 * Инициализируем блюпринт
	 * @param {Object} node
	 */
	blueprintInit: function(node){
		var blueprint = new Blueprint.classes.Blueprint(node);

		this._blueprints.push(blueprint)

		this.dispatchEvent({type: 'blueprintInit', blueprint: blueprint, node: node})
	},
	/**
	 * Закрываем блюпринт
	 * @param {Class} blueprint - класс ифрейм окошка
	 */
	blueprintClose: function(blueprint){
		this._blueprints[0].close();
	},
	/**
	 * Создаем новый блюпринт
	 */
	blueprintNew: function(){
		var uid  = 'blueprint_'+Blueprint.Utility.uid();
		
		this.data[uid] = {
			uid: uid,
			name: 'Blueprint',
			change: 0,
			data: {}
		};

		this.reloadBlueprints();
	},
	/**
	 * Нстройки блюпринта
	 * @param {Class} node
	 */
	blueprintSettings: function(blueprint){
		var self = this;

		var data  = blueprint.data;
		var group = $([
			'<div class="form-group">',
                '<div class="form-name">Название</div>',
                '<div class="form-content">',
                    '<div class="form-input">',
                        '<input type="text" name="name" value="'+(data.name || '')+'" placeholder="" />',
                    '</div>',
                '</div>',
            '</div>',
		].join(''));

		var changeName = function(n,value){
			data.name = value;

			self.dispatchEvent({type: 'blueprintChangeParams', blueprint: blueprint, node: data})
		}

		Form.InputChangeEvent($('.form-input',group),changeName,changeName)
		
		$('#blueprint-settings').empty().append(group)
	}
})