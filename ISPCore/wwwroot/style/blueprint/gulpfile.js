var gulp           = require('gulp'), // Подключаем Gulp
	concat         = require('gulp-concat'), // Подключаем gulp-concat (для конкатенации файлов)
	rename         = require('gulp-rename'), // Подключаем библиотеку для переименования файлов
	del            = require('del'), // Подключаем библиотеку для удаления файлов и папок
	chokidar       = require('chokidar'),
	browserSync    = require('browser-sync'),
	fs             = require('fs');

var srcFolder = './';


gulp.task('merge', function(){
	return gulp.src([srcFolder+'js/*.js',srcFolder+'js/nodes/*.js']).pipe(concat('blueprint.js')).pipe(gulp.dest(srcFolder));
});


gulp.task('watch', function(){
	var watcher = chokidar.watch([srcFolder+'js'], { persistent: true});

		watcher.on('add', function(path) {
			console.log('File', path, 'has been added');

			gulp.start('merge');
		})
		.on('change', function(path) {
			console.log('File', path, 'has been changed');

			gulp.start('merge');
		})
		.on('unlink', function(path) {
			gulp.start('merge');
		})
});

gulp.task('browser-sync', function(){ // Создаем таск browser-sync
	browserSync({ // Выполняем browserSync
		server: { // Определяем параметры сервера
			baseDir: srcFolder // Директория для сервера - app
		},
		open: false,
		notify: false // Отключаем уведомления
	});
});

gulp.task('default', ['watch','browser-sync']);