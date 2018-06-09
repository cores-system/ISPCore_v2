var Blueprint = {
	classes: {}
};

Blueprint.Initialization = {
	viewport: function(){
		Blueprint.Render    = new Blueprint.classes.Render();
		Blueprint.Data      = new Blueprint.classes.Data();
		Blueprint.Drag      = new Blueprint.classes.Drag();
		Blueprint.Image     = new Blueprint.classes.Image();
		Blueprint.Shortcut  = new Blueprint.classes.Shortcut();
		Blueprint.Viewport  = new Blueprint.classes.Viewport();
		Blueprint.Selection = new Blueprint.classes.Selection();

		var selectNode, drag, has_focus, presed, dragCopy;
		var cursor = {x: 0, y: 0};
		var buffer = [];

		//drag and drop

		//начали тянуть
		Blueprint.Drag.addEventListener('start',function(event){
			selectNode = false;

			//если зажата одна из клавиш
			//то показывме мыделение
			if(presed.shiftKey || presed.altKey){
				this.enable = false;

				Blueprint.Selection.enable = true;
			} 

			parent.Blueprint.Menu.hide();

			drag = false;
		})

		//таскают
		Blueprint.Drag.addEventListener('drag',function(event){
			Blueprint.Render.draw()

			//отмеряем дистанцию, если было движение
			//то помечаем это и не показываем меню
			var a = event.drag.start,
				b = event.drag.move;

			var d = Math.sqrt(Math.pow(a.x-b.x,2) + Math.pow(a.y-b.y,2));

			if(d > 10) drag = true;

			//если зажали клавишу Alt и начали тянуть нод
			if(dragCopy){
				//отменяем выделение
				Blueprint.Selection.enable = false;
				//возобновляем драг
				Blueprint.Drag.enable      = true;
				//прячим выделение
				Blueprint.Selection.stop();

				var mouse = {},offset;

				var nodes = [], uids = [];

				var select = Blueprint.Selection.selection;

				//и так, для начала у всех нодов удалим драг
				for(var i = 0; i < Blueprint.Render.nodes.length; i++){
					Blueprint.Render.nodes[i].dragRemove();
				}

				//затем снимим выделение и скопируем дату
				for(var i = 0; i < select.length; i++){
					var node = select[i];

					node.node.removeClass('active')

					nodes.push(JSON.parse(JSON.stringify(node.data)));
				}

				//делаем клоны
				for(var i = 0; i < nodes.length; i++){
					var node = nodes[i],
						uid  = node.uid;

					//новый uid
					node.uid = Blueprint.Utility.uid();

					//запоминаем какие добавили
					uids.push(node.uid);

					//смотрим родителей и меняе на новый uid
					for(var a = 0; a < nodes.length; a++){
						var parents = nodes[a].parents;

						for(var b = 0; b < parents.length; b++){
							if(parents[b].uid == uid){
								parents[b].uid = node.uid;
							} 
						}
					}
					
					//пишим в бд
					Blueprint.Data.get().nodes[node.uid] = node;

					//создаем нод
					Blueprint.Render.addNode(node.uid).create();

					parent.Blueprint.Program.fireChangeEvent();
				}

				//чистим выделение
				Blueprint.Selection.clear();

				//делаем выделение новых нодов
				for(var i = 0; i < uids.length; i++){
					var uid = uids[i],
						node = Blueprint.Render.searchNode(uid);

					if(node){
						node.node.addClass('active');

						Blueprint.Selection.add(node);

						parent.Blueprint.Program.nodeOption({
							target: node,
							worker: node.data.worker,
							uid: node.uid,
							data: node.data
						})

						node.dragStart();
					}
				}

				//обновляем рендер
				Blueprint.Render.update();

				//помечам что больше копировать не нужно
				dragCopy = false;
			}

			//обновляем область если есть выделение
			if(Blueprint.Selection.enable) Blueprint.Selection.drag(event)
		})

		//перестали таскать
		Blueprint.Drag.addEventListener('stop',function(event){
			if(selectNode){
				cursor.x = event.drag.move.x;
				cursor.y = event.drag.move.y;

				//если из нода протянули линию то показать меню
				parent.Blueprint.Menu.show(cursor)
			} 
			else{
				Blueprint.Render.draw();
			}

			this.enable = true;

			//если было выделение, то прячим
			if(Blueprint.Selection.enable) Blueprint.Selection.stop();

			Blueprint.Selection.enable = false;
		})

		//обновляем рендер если был зум
		Blueprint.Viewport.addEventListener('zoom',Blueprint.Render.draw.bind(Blueprint.Render))


		//если жмекнули сохранить то говорим что нуно сохранить
		Blueprint.Shortcut.add('Ctrl+S',function(){
			parent.Shortcut.Fire('Ctrl+S');
		})

		Blueprint.Shortcut.add('Ctrl+C',function(){
			buffer = [];

			buffer = buffer.concat(Blueprint.Selection.selection);
		})

		Blueprint.Shortcut.add('Ctrl+V',function(){
			var mouse = {},offset;

			//смотрим где курсор
			mouse.x = presed.clientX / Blueprint.Viewport.scale - Blueprint.Viewport.position.x;
			mouse.y = presed.clientY / Blueprint.Viewport.scale - Blueprint.Viewport.position.y;

			var nodes = [], uids = [];

			//из буфера копируем данные нодов
			for(var i = 0; i < buffer.length; i++){
				nodes.push(JSON.parse(JSON.stringify(buffer[i].data)));
			}

			//начинаем копировать
			for(var i = 0; i < nodes.length; i++){
				var node = nodes[i],
					uid  = node.uid;

				//создаем новый uid
				node.uid = Blueprint.Utility.uid();

				//запоминаем новый uid
				uids.push(node.uid);

				//у родителей заменяем uid
				for(var a = 0; a < nodes.length; a++){
					var parents = nodes[a].parents;

					for(var b = 0; b < parents.length; b++){
						if(parents[b].uid == uid){
							parents[b].uid = node.uid;
						} 
					}
				}
				
				//надо узнать первый отступ
				if(!offset){
					offset = {
						x: mouse.x - node.position.x,
						y: mouse.y - node.position.y
					}
				}

				//к позиции добовляем отступ
				node.position = {
					x: node.position.x + offset.x,
					y: node.position.y + offset.y
				}

				//записываем в бд
				Blueprint.Data.get().nodes[node.uid] = node;

				//создаем нод
				Blueprint.Render.addNode(node.uid).create();

				parent.Blueprint.Program.fireChangeEvent();
				
			}

			//чистив выделение
			Blueprint.Selection.clear();

			//делаем активными то что создали
			for(var i = 0; i < uids.length; i++){
				var uid = uids[i],
					node = Blueprint.Render.searchNode(uid);

				if(node){
					node.node.addClass('active');

					Blueprint.Selection.add(node);
				}
			}

			//обновляем
			Blueprint.Render.update();
		})


		//выделение области
		Blueprint.Selection.addEventListener('drag',function(event){
			var v = event.viewport;
			var c = presed.altKey && presed.shiftKey;

			//если зажата клава шифт и альт то чистим все
			if(c) Blueprint.Selection.clear()

			//идем по списку
			$.each(Blueprint.Render.nodes,function(i,node){
				var p = node.data.position;

				//если попали в область
				if(p.x > v.left && p.x < v.left + v.width && p.y > v.top && p.y < v.top + v.height){
					if(c){
						node.node.addClass('active')

						Blueprint.Selection.add(node)
					}
					else if(presed.altKey){
						node.node.removeClass('active')

						Blueprint.Selection.remove(node)
					}
					else{
						node.node.addClass('active')

						Blueprint.Selection.add(node)
					}
				}
				else if(c){
					node.node.removeClass('active')

					Blueprint.Selection.remove(node)
				}
			})
		})

		//эвент на добовление нового нода
		Blueprint.Render.addEventListener('addNode',function(e){
			var node = e.node;

			//вешаем эвенты на сам нод

			//эвент удаления
			node.addEventListener('remove',function(){
				Blueprint.Render.removeNode(node);

				Blueprint.Selection.remove(node);

				parent.Blueprint.Program.dispatchEvent({type: 'nodeRemove', node: node});
			})

			//начали тянуть линию
			node.addEventListener('output',function(node){
				//там выше drag стирает значение
				//поэтому таймер поставил
				setTimeout(function(){
					selectNode = node.target;
				},0)
			})

			node.addEventListener('setValue',function(){
				Blueprint.Render.draw();

				parent.Blueprint.Program.fireChangeEvent();
			})

			//протянули линию на input переменную
			node.addEventListener('input',function(event){
				
				
				
				if(selectNode !== this && selectNode && event.entrance !== selectNode.selectEntrance){
					var selectVar = selectNode.selectVar;

					if(event.entrance !== 'input'){
						selectNode.data.parents.push({
							uid: this.data.uid,
							output: event.name,
							input: selectVar
						})
					}
					else{
						this.data.parents.push({
							uid: selectNode.data.uid,
							output: selectVar,
							input: event.name
						})
					}

					parent.Blueprint.Program.fireChangeEvent();
					
					Blueprint.Render.update();
				}

				selectNode = false;
			})

			//если удалили инпуты
			node.addEventListener('inputRemove',function(event){
				parent.Blueprint.Program.fireChangeEvent();

				Blueprint.Render.update();
			})

			//если удалили выходы
			node.addEventListener('outputRemove',function(event){
				parent.Blueprint.Program.fireChangeEvent();

				Blueprint.Render.update();
			})

			//если нод двигают
			node.addEventListener('drag',function(event){
				var selection = Blueprint.Selection.selection;

				//есть более одного выделения
				//а значит ташим их все
				if(selection.length > 1){
					for(var i = 0; i < selection.length; i++){
						var node = selection[i];

						//естественно кроме себя так как уже добавлен эвент
						if(node !== event.target) node.dragStart();
					}
				}
			})

			//если нод выбрали
			node.addEventListener('select',function(event){
				var selection = Blueprint.Selection.selection;

				//добовляем к обшиму выделению
				if(presed.shiftKey){
					Blueprint.Selection.add(event.target);

					event.target.node.addClass('active');
				}
				//снимаем выделение из обших
				else if(presed.altKey){
					Blueprint.Selection.remove(event.target);

					event.target.node.removeClass('active');
				}
				//снимаем все выделение и выделяем только его
				else if(!drag){
					Blueprint.Drag.clear();

					Blueprint.Selection.select(event.target)

					$('.blueprint-node').removeClass('active')

					event.target.node.addClass('active');

					parent.Blueprint.Program.nodeOption(event)

					//надо обновить переменную
					selection = Blueprint.Selection.selection
				}
			});

			node.addEventListener('showOptionAgain',function(event){
				parent.Blueprint.Program.nodeOption(event)
			})

			//опаньки, видать нужно к нему подключится
			//надо потом похимичить
			if(selectNode && selectNode.selectEntrance == 'output' && node.data.worker !== "event"){
				var first;

				for(var i in node.params.vars.input){
					if(!first){
						first = i; break;
					}
				}

				if(first){
					node.data.parents.push({
						uid: selectNode.data.uid,
						output: selectNode.selectVar,
						input: first
					})
				}

				Blueprint.Render.update();
			}

			selectNode = false;
		})

		//событие новый нод
		Blueprint.Render.addEventListener('newNode',function(e){
			//если новый то запускаем эвент создать
			e.node.create();

			parent.Blueprint.Program.fireChangeEvent();
		})

		//событие удаляем нод
		Blueprint.Render.addEventListener('removeNode',function(e){
			parent.Blueprint.Program.fireChangeEvent();
		})
		
		//если в меню был выбран нод то создаем его
		parent.Blueprint.Menu.addEventListener('select',function(event){
			if(has_focus){
				var node = {
					worker: event.name,
					position: {
						x: cursor.x,
						y: cursor.y
					}
				}

				Arrays.extend(node,event.data);

				Blueprint.Render.newNode(node);
			}
		})

		parent.Blueprint.Program.addEventListener('blueprintChangeParams',function(event){
			var nodes = Blueprint.Render.nodes;
			
			for(var i = 0; i < nodes.length; i++){
				var node = nodes[i];

				if(node.data.userData.blueprintUid == event.blueprint.uid){
					node.fire('blueprintChangeParams',event.node);
				}
			}
		})

		//ну понятно да?
		window.onblur = function(){  
		    has_focus = false;  
		}  
		window.onfocus = function(){  
		    has_focus = true;
		}

		//активация snaped
		$('.blueprint-snap').on('click',function(){
			$(this).toggleClass('active')

			Blueprint.snaped = false;

			if($(this).hasClass('active')){
				Blueprint.snaped = true;
			}
		}).click();

		//отслеживаем зажатые клавишы
		$(document).on('mousedown',function(e){
			presed = e;

			dragCopy = $(e.target).closest($('.blueprint-node')).length && presed.altKey;
		}).on('mousemove',function(e){
			presed = e;
		}).on('mouseup',function(e){
			presed = e;
		}).on('keydown',function(e){
			//если нажали Del
			if(e.keyCode == 46){
				var selection = Blueprint.Selection.selection;

				for(var i = selection.length; i--;) selection[i].remove()
			}
		})

		//снимаем выделение нодов
		$(document).on('click',function(event){
			if(!$(event.target).closest($('.blueprint-node')).length) {
				if(!drag){
					Blueprint.Selection.clear();

					$('.blueprint-node').removeClass('active')
				}
			}
		})

		//контекстное меню
		$(document).contextmenu(function(e){
			e.preventDefault();

			if(!drag){
				cursor = {
					x: e.pageX,
					y: e.pageY
				};

				if(!$(e.target).closest($('.blueprint-node')).length) parent.Blueprint.Menu.show(cursor);
			}
		})
	},

	//после установки данных и классов, создаем ноды
	nodes: function(){
		var nodes = Blueprint.Data.get().nodes;

		$.each(nodes,function(uid,params){
			if(Blueprint.Worker.get(params.worker)) Blueprint.Render.addNode(uid)
		})

		Blueprint.Render.update();
	},

	//наша программа
	program: function(){
		Blueprint.Menu    = new Blueprint.classes.Menu();
		Blueprint.Program = new Blueprint.classes.Program();
		
		
		Blueprint.Program.data.main.data.nodes = BlueprintData;


		//если было открыто blueprint окно
		Blueprint.Program.addEventListener('blueprintInit',function(program){
			var blueprint = program.blueprint;

			blueprint.addEventListener('close',function(){
				Blueprint.Program.blueprintClose(blueprint);
			});
		})

		Blueprint.Program.addEventListener('blueprintChangeParams',function(program){
			var uid  = program.blueprint.uid,
				name = program.node.name;

			$('#blueprint-tabs li[uid="'+uid+'"] span').text(name)
			$('#blueprint-menu li[uid="'+uid+'"] span').text(name)
			$('#blueprint-list li[uid="'+uid+'"] span a').text(name)
		})

		Blueprint.Program.addEventListener('nodeRemove',function(){
			$('#blueprint-node-option').empty();
		})

		//открываем окна
		Blueprint.Program.blueprintInit(Blueprint.Program.data['main']);
		
		
		$('#blueprint-save').on('click',function(){
			$(this).sl('load', '/trigger/nodes/save', { back: false, ignore:true, data: {Id: NodeId, nodes: JSON.stringify(Blueprint.Program.data.main.data.nodes)}, dataType: 'json' }, function (j){
				if (j.msg) $.sl('info', j.msg)
				else if (j.result) { }
				else $.sl('info', 'Неизвестная ошибка')
			})
		})
		
		var content = $('.blueprint-container');
		
		$(window).resize(function(){
			var offset = content.offset();
			
			var win_height = $(window).height();
			
			content.height(win_height - offset.top - 45);
		}).trigger('resize')
	}
}
