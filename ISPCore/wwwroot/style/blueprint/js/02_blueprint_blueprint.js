Blueprint.classes.Blueprint = function(data){
	this.uid  = data.uid;
	this.data = data;
	
	this.blueprints = $('#blueprint-blueprints');

	this.initWindow();
}

Object.assign( Blueprint.classes.Blueprint.prototype, EventDispatcher.prototype, {
	
	initViewport: function(){
		this.contentBlueprint.Initialization.viewport();
		this.contentBlueprint.Data.set(Blueprint.Program.nodeData(this.uid))
		this.contentBlueprint.Initialization.nodes();
	},

	initWindow: function(){
		var self = this;

		$('iframe',this.blueprints).removeClass('active');

		this.blueprint = $('<iframe src="/style/blueprint/blueprint.html" class="active" id="'+this.uid+'"></iframe>')

		this.blueprint.on('load',function(){
			self.contents         = self.blueprint.contents()

			self.contentBlueprint = document.getElementById(self.uid).contentWindow.Blueprint; //не знаю, зато млин так работает, маджик!

			self.initViewport();
		})

		this.blueprints.append(this.blueprint)
	},

	remove: function(){
		this.blueprint.remove();

		this.dispatchEvent({type: 'remove'})
	}	
})