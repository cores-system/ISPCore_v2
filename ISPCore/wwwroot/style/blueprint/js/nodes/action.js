Blueprint.Worker.add('action',{
	params: {
		name: 'Действие',
		description: 'Действие',
		saturation: 'hsl(59, 84%, 48%)',
		alpha: 0.52,
		titleColor: '#3e3729',
		category: 'main',
		type: 'round',
		vars: {
			input: {
				input: {
					
				},
				code: {
					disableVisible: true,
					type: 'code',
				},
				namespaces: {
					name: 'Namespaces',
					placeholder: 'Пространства имён (namespace)',
					color: '#ddd',
					disableVisible: true,
					textarea: true
				},
				references: {
					name: 'References',
					color: '#ddd',
					disableVisible: true,
					placeholder: 'Список DLL библиотек',
					textarea: true
				}
			},
			output: {
				output: {
					enableChange: true,
					placeholder: 'Имя действия',
					display: true
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