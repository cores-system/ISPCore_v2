Blueprint.Worker.add('event',{
	params: {
		name: 'Событие',
		description: 'Событие',
		saturation: 'hsl(212, 100%, 65%)',
		alpha: 0.58,
		titleColor: '#ffffff',
		category: 'main',
		vars: {
			input: {
				path: {
					name: 'Path',
					color: '#ddd',
					placeholder: 'Категория триггера',
					display: true,
					disable: true
				},
				name: {
					name: 'Name',
					color: '#ddd',
					placeholder: 'Имя триггера',
					display: true,
					disable: true
				}
			},
			output: {
				output: {
				},
			}
		},
		userData: {
			
		}
	},
	on: {
		create: function(){
			
		},
		remove: function(){

		}
	},
	working: {
		start: function(){
			
		},
		
		build: function(){
			
		},
	}
});