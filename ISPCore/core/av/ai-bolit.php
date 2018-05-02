<?php
///////////////////////////////////////////////////////////////////////////
// Created and developed by Greg Zemskov, Revisium Company
// Email: audit@revisium.com, http://revisium.com/ai/

// Commercial usage is not allowed without a license purchase or written permission of the author
// Source code and signatures usage is not allowed

// Certificated in Federal Institute of Industrial Property in 2012
// http://revisium.com/ai/i/mini_aibolit.jpg

////////////////////////////////////////////////////////////////////////////
// Запрещено использование скрипта в коммерческих целях без приобретения лицензии.
// Запрещено использование исходного кода скрипта и сигнатур.
//
// По вопросам приобретения лицензии обращайтесь в компанию "Ревизиум": http://www.revisium.com
// audit@revisium.com
// На скрипт получено авторское свидетельство в Роспатенте
// http://revisium.com/ai/i/mini_aibolit.jpg
///////////////////////////////////////////////////////////////////////////
ini_set('memory_limit', '1G');
ini_set('xdebug.max_nesting_level', 500);

$int_enc = @ini_get('mbstring.internal_encoding');
        
define('SHORT_PHP_TAG', strtolower(ini_get('short_open_tag')) == 'on' || strtolower(ini_get('short_open_tag')) == 1 ? true : false);

// Put any strong password to open the script from web
// Впишите вместо put_any_strong_password_here сложный пароль	 

define('PASS', '????????????????'); 

//////////////////////////////////////////////////////////////////////////

if (isCli()) {
	if (strpos('--eng', $argv[$argc - 1]) !== false) {
		  define('LANG', 'EN');  
	}
} else {
   define('NEED_REPORT', true);
}
	
if (!defined('LANG')) {
   define('LANG', 'RU');  
}	

// put 1 for expert mode, 0 for basic check and 2 for paranoic mode
// установите 1 для режима "Эксперта", 0 для быстрой проверки и 2 для параноидальной проверки (для лечения сайта) 
define('AI_EXPERT_MODE', 1); 

define('REPORT_MASK_PHPSIGN', 1);
define('REPORT_MASK_SPAMLINKS', 2);
define('REPORT_MASK_DOORWAYS', 4);
define('REPORT_MASK_SUSP', 8);
define('REPORT_MASK_CANDI', 16);
define('REPORT_MASK_WRIT', 32);
define('REPORT_MASK_FULL', REPORT_MASK_PHPSIGN | REPORT_MASK_DOORWAYS | REPORT_MASK_SUSP
/* <-- remove this line to enable "recommendations"  

| REPORT_MASK_SPAMLINKS 

 remove this line to enable "recommendations" --> */
);

define('AI_HOSTER', 0); 

define('AI_EXTRA_WARN', 0);

$defaults = array(
	'path' => dirname(__FILE__),
	'scan_all_files' => (AI_EXPERT_MODE == 2), // full scan (rather than just a .js, .php, .html, .htaccess)
	'scan_delay' => 0, // delay in file scanning to reduce system load
	'max_size_to_scan' => '600K',
	'site_url' => '', // website url
	'no_rw_dir' => 0,
    	'skip_ext' => '',
        'skip_cache' => false,
	'report_mask' => REPORT_MASK_FULL
);

define('DEBUG_MODE', 0);
define('DEBUG_PERFORMANCE', 0);

define('AIBOLIT_START_TIME', time());
define('START_TIME', microtime(true));

define('DIR_SEPARATOR', '/');

define('AIBOLIT_MAX_NUMBER', 200);

define('DOUBLECHECK_FILE', 'AI-BOLIT-DOUBLECHECK.php');

if ((isset($_SERVER['OS']) && stripos('Win', $_SERVER['OS']) !== false)/* && stripos('CygWin', $_SERVER['OS']) === false)*/) {
   define('DIR_SEPARATOR', '\\');
}

$g_SuspiciousFiles = array('cgi', 'pl', 'o', 'so', 'py', 'sh', 'phtml', 'php3', 'php4', 'php5', 'php6', 'php7', 'pht', 'shtml');
$g_SensitiveFiles = array_merge(array('php', 'js', 'htaccess', 'html', 'htm', 'tpl', 'inc', 'css', 'txt', 'sql', 'ico', '', 'susp', 'suspected', 'zip', 'tar'), $g_SuspiciousFiles);
$g_CriticalFiles = array('php', 'htaccess', 'cgi', 'pl', 'o', 'so', 'py', 'sh', 'phtml', 'php3', 'php4', 'php5', 'php6', 'php7', 'pht', 'shtml', 'susp', 'suspected', 'infected', 'vir', 'ico', '');
$g_CriticalEntries = '^\s*<\?php|^\s*<\?=|^#!/usr|^#!/bin|\beval|assert|base64_decode|\bsystem|create_function|\bexec|\bpopen|\bfwrite|\bfputs|file_get_|call_user_func|file_put_|\$_REQUEST|ob_start|\$_GET|\$_POST|\$_SERVER|\$_FILES|\bmove|\bcopy|\barray_|reg_replace|\bmysql_|\bchr|fsockopen|\$GLOBALS|sqliteCreateFunction';
$g_VirusFiles = array('js', 'html', 'htm', 'suspicious');
$g_VirusEntries = '<\s*script|<\s*iframe|<\s*object|<\s*embed|fromCharCode|setTimeout|setInterval|location\.|document\.|window\.|navigator\.|\$(this)\.';
$g_PhishFiles = array('js', 'html', 'htm', 'suspected', 'php', 'pht', 'php7');
$g_PhishEntries = '<\s*title|<\s*html|<\s*form|<\s*body|bank|account';
$g_ShortListExt = array('php', 'php3', 'php4', 'php5', 'php6', 'php7', 'pht', 'html', 'htm', 'phtml', 'shtml', 'khtml', '', 'ico', 'txt');

if (LANG == 'RU') {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// RUSSIAN INTERFACE
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
$msg1 = "\"Отображать по _MENU_ записей\"";
$msg2 = "\"Ничего не найдено\"";
$msg3 = "\"Отображается c _START_ по _END_ из _TOTAL_ файлов\"";
$msg4 = "\"Нет файлов\"";
$msg5 = "\"(всего записей _MAX_)\"";
$msg6 = "\"Поиск:\"";
$msg7 = "\"Первая\"";
$msg8 = "\"Предыдущая\"";
$msg9 = "\"Следующая\"";
$msg10 = "\"Последняя\"";
$msg11 = "\": активировать для сортировки столбца по возрастанию\"";
$msg12 = "\": активировать для сортировки столбцов по убыванию\"";

define('AI_STR_001', 'Отчет сканера <a href="https://revisium.com/ai/">AI-Bolit</a> v@@VERSION@@:');
define('AI_STR_002', 'Обращаем внимание на то, что большинство CMS <b>без дополнительной защиты</b> рано или поздно <b>взламывают</b>.<p> Компания <a href="https://revisium.com/">"Ревизиум"</a> предлагает услугу превентивной защиты сайта от взлома с использованием уникальной <b>процедуры "цементирования сайта"</b>. Подробно на <a href="https://revisium.com/ru/client_protect/">странице услуги</a>. <p>Лучшее лечение &mdash; это профилактика.');
define('AI_STR_003', 'Не оставляйте файл отчета на сервере, и не давайте на него прямых ссылок с других сайтов. Информация из отчета может быть использована злоумышленниками для взлома сайта, так как содержит информацию о настройках сервера, файлах и каталогах.');
define('AI_STR_004', 'Путь');
define('AI_STR_005', 'Изменение свойств');
define('AI_STR_006', 'Изменение содержимого');
define('AI_STR_007', 'Размер');
define('AI_STR_008', 'Конфигурация PHP');
define('AI_STR_009', "Вы установили слабый пароль на скрипт AI-BOLIT. Укажите пароль не менее 8 символов, содержащий латинские буквы в верхнем и нижнем регистре, а также цифры. Например, такой <b>%s</b>");
define('AI_STR_010', "Сканер AI-Bolit запускается с паролем. Если это первый запуск сканера, вам нужно придумать сложный пароль и вписать его в файле ai-bolit.php в строке №34. <p>Например, <b>define('PASS', '%s');</b><p>
После этого откройте сканер в браузере, указав пароль в параметре \"p\". <p>Например, так <b>http://mysite.ru/ai-bolit.php?p=%s</b>. ");
define('AI_STR_011', 'Текущая директория не доступна для чтения скрипту. Пожалуйста, укажите права на доступ <b>rwxr-xr-x</b> или с помощью командной строки <b>chmod +r имя_директории</b>');
define('AI_STR_012', "Затрачено времени: <b>%s</b>. Сканирование начато %s, сканирование завершено %s");
define('AI_STR_013', 'Всего проверено %s директорий и %s файлов.');
define('AI_STR_014', '<div class="rep" style="color: #0000A0">Внимание, скрипт выполнил быструю проверку сайта. Проверяются только наиболее критические файлы, но часть вредоносных скриптов может быть не обнаружена. Пожалуйста, запустите скрипт из командной строки для выполнения полного тестирования. Подробнее смотрите в <a href="https://revisium.com/ai/faq.php">FAQ вопрос №10</a>.</div>');
define('AI_STR_015', '<div class="title">Критические замечания</div>');
define('AI_STR_016', 'Эти файлы могут быть вредоносными или хакерскими скриптами');
define('AI_STR_017', 'Вирусы и вредоносные скрипты не обнаружены.');
define('AI_STR_018', 'Эти файлы могут быть javascript вирусами');
define('AI_STR_019', 'Обнаружены сигнатуры исполняемых файлов unix и нехарактерных скриптов. Они могут быть вредоносными файлами');
define('AI_STR_020', 'Двойное расширение, зашифрованный контент или подозрение на вредоносный скрипт. Требуется дополнительный анализ');
define('AI_STR_021', 'Подозрение на вредоносный скрипт');
define('AI_STR_022', 'Символические ссылки (symlinks)');
define('AI_STR_023', 'Скрытые файлы');
define('AI_STR_024', 'Возможно, каталог с дорвеем');
define('AI_STR_025', 'Не найдено директорий c дорвеями');
define('AI_STR_026', 'Предупреждения');
define('AI_STR_027', 'Подозрение на мобильный редирект, подмену расширений или автовнедрение кода');
define('AI_STR_028', 'В не .php файле содержится стартовая сигнатура PHP кода. Возможно, там вредоносный код');
define('AI_STR_029', 'Дорвеи, реклама, спам-ссылки, редиректы');
define('AI_STR_030', 'Непроверенные файлы - ошибка чтения');
define('AI_STR_031', 'Невидимые ссылки. Подозрение на ссылочный спам');
define('AI_STR_032', 'Невидимые ссылки');
define('AI_STR_033', 'Отображены только первые ');
define('AI_STR_034', 'Подозрение на дорвей');
define('AI_STR_035', 'Скрипт использует код, который часто встречается во вредоносных скриптах');
define('AI_STR_036', 'Директории из файла .adirignore были пропущены при сканировании');
define('AI_STR_037', 'Версии найденных CMS');
define('AI_STR_038', 'Большие файлы (больше чем %s). Пропущено');
define('AI_STR_039', 'Не найдено файлов больше чем %s');
define('AI_STR_040', 'Временные файлы или файлы(каталоги) - кандидаты на удаление по ряду причин');
define('AI_STR_041', 'Потенциально небезопасно! Директории, доступные скрипту на запись');
define('AI_STR_042', 'Не найдено директорий, доступных на запись скриптом');
define('AI_STR_043', 'Использовано памяти при сканировании: ');
define('AI_STR_044', 'Просканированы только файлы, перечисленные в ' . DOUBLECHECK_FILE . '. Для полного сканирования удалите файл ' . DOUBLECHECK_FILE . ' и запустите сканер повторно.');
define('AI_STR_045', '<div class="rep">Внимание! Выполнена экспресс-проверка сайта. Просканированы только файлы с расширением .php, .js, .html, .htaccess. В этом режиме могут быть пропущены вирусы и хакерские скрипты в файлах с другими расширениями. Чтобы выполнить более тщательное сканирование, поменяйте значение настройки на <b>\'scan_all_files\' => 1</b> в строке 50 или откройте сканер в браузере с параметром full: <b><a href="ai-bolit.php?p=' . PASS . '&full">ai-bolit.php?p=' . PASS . '&full</a></b>. <p>Не забудьте перед повторным запуском удалить файл ' . DOUBLECHECK_FILE . '</div>');
define('AI_STR_050', 'Замечания и предложения по работе скрипта и не обнаруженные вредоносные скрипты присылайте на <a href="mailto:ai@revisium.com">ai@revisium.com</a>.<p>Также будем чрезвычайно благодарны за любые упоминания скрипта AI-Bolit на вашем сайте, в блоге, среди друзей, знакомых и клиентов. Ссылочку можно поставить на <a href="https://revisium.com/ai/">https://revisium.com/ai/</a>. <p>Если будут вопросы - пишите <a href="mailto:ai@revisium.com">ai@revisium.com</a>. ');
define('AI_STR_051', 'Отчет по ');
define('AI_STR_052', 'Эвристический анализ обнаружил подозрительные файлы. Проверьте их на наличие вредоносного кода.');
define('AI_STR_053', 'Много косвенных вызовов функции');
define('AI_STR_054', 'Подозрение на обфусцированные переменные');
define('AI_STR_055', 'Подозрительное использование массива глобальных переменных');
define('AI_STR_056', 'Дробление строки на символы');
define('AI_STR_057', 'Сканирование выполнено в экспресс-режиме. Многие вредоносные скрипты могут быть не обнаружены.<br> Рекомендуем проверить сайт в режиме "Эксперт" или "Параноидальный". Подробно описано в <a href="https://revisium.com/ai/faq.php">FAQ</a> и инструкции к скрипту.');
define('AI_STR_058', 'Обнаружены фишинговые страницы');

define('AI_STR_059', 'Мобильных редиректов');
define('AI_STR_060', 'Вредоносных скриптов');
define('AI_STR_061', 'JS Вирусов');
define('AI_STR_062', 'Фишинговых страниц');
define('AI_STR_063', 'Исполняемых файлов');
define('AI_STR_064', 'IFRAME вставок');
define('AI_STR_065', 'Пропущенных больших файлов');
define('AI_STR_066', 'Ошибок чтения файлов');
define('AI_STR_067', 'Зашифрованных файлов');
define('AI_STR_068', 'Подозрительных (эвристика)');
define('AI_STR_069', 'Символических ссылок');
define('AI_STR_070', 'Скрытых файлов');
define('AI_STR_072', 'Рекламных ссылок и кодов');
define('AI_STR_073', 'Пустых ссылок');
define('AI_STR_074', 'Сводный отчет');
define('AI_STR_075', 'Сканер бесплатный только для личного некоммерческого использования. Информация по <a href="https://revisium.com/ai/faq.php#faq11" target=_blank>коммерческой лицензии</a> (пункт №11). <a href="https://revisium.com/images/mini_aibolit.jpg">Авторское свидетельство</a> о гос. регистрации в РосПатенте №2012619254 от 12 октября 2012 г.');

$tmp_str = <<<HTML_FOOTER
   <div class="disclaimer"><span class="vir">[!]</span> Отказ от гарантий: невозможно гарантировать обнаружение всех вредоносных скриптов. Поэтому разработчик сканера не несет ответственности за возможные последствия работы сканера AI-Bolit или неоправданные ожидания пользователей относительно функциональности и возможностей.
   </div>
   <div class="thanx">
      Замечания и предложения по работе скрипта, а также не обнаруженные вредоносные скрипты вы можете присылать на <a href="mailto:ai@revisium.com">ai@revisium.com</a>.<br/>
      Также будем чрезвычайно благодарны за любые упоминания сканера AI-Bolit на вашем сайте, в блоге, среди друзей, знакомых и клиентов. <br/>Ссылку можно поставить на страницу <a href="https://revisium.com/ai/">https://revisium.com/ai/</a>.<br/> 
     <p>Получить консультацию или задать вопросы можно по email <a href="mailto:ai@revisium.com">ai@revisium.com</a>.</p> 
	</div>
HTML_FOOTER;

define('AI_STR_076', $tmp_str);
define('AI_STR_077', "Подозрительные параметры времени изменения файла");
define('AI_STR_078', "Подозрительные атрибуты файла");
define('AI_STR_079', "Подозрительное местоположение файла");
define('AI_STR_080', "Обращаем внимание, что обнаруженные файлы не всегда являются вирусами и хакерскими скриптами. Сканер минимизирует число ложных обнаружений, но это не всегда возможно, так как найденный фрагмент может встречаться как во вредоносных скриптах, так и в обычных.<p>Для диагностического сканирования без ложных срабатываний мы разработали специальную версию <u><a href=\"https://revisium.com/ru/blog/ai-bolit-4-ISP.html\" target=_blank style=\"background: none; color: #303030\">сканера для хостинг-компаний</a></u>.");
define('AI_STR_081', "Уязвимости в скриптах");
define('AI_STR_082', "Добавленные файлы");
define('AI_STR_083', "Измененные файлы");
define('AI_STR_084', "Удаленные файлы");
define('AI_STR_085', "Добавленные каталоги");
define('AI_STR_086', "Удаленные каталоги");
define('AI_STR_087', "Изменения в файловой структуре");

$l_Offer =<<<OFFER
    <div>
	 <div class="crit" style="font-size: 17px; margin-bottom: 20px"><b>Внимание! Наш сканер обнаружил подозрительный или вредоносный код</b>.</div> 
	 <p>Возможно, ваш сайт был взломан. Рекомендуем срочно <a href="https://revisium.com/ru/order/#fform" target=_blank>проконсультироваться со специалистами</a> по данному отчету.</p>
	 <p><hr size=1></p>
	 <p>Рекомендуем также проверить сайт бесплатным <b><a href="https://rescan.pro/?utm=aibolit" target=_blank>онлайн-сканером ReScan.Pro</a></b>.</p>
	 <p><hr size=1></p>
         <div class="caution">@@CAUTION@@</div>
    </div>
OFFER;

$l_Offer2 =<<<OFFER2
	   <b>Наши продукты:</b><br/>
              <ul>
               <li style="margin-top: 10px"><font color=red><sup>[new]</sup></font><b><a href="https://revisium.com/ru/blog/revisium-antivirus-for-plesk.html" target=_blank>Антивирус для Plesk</a> Onyx 17.x</b> &mdash;  сканирование и лечение сайтов прямо в панели хостинга</li>
               <li style="margin-top: 10px"><b><a href="https://cloudscan.pro/ru/" target=_blank>Облачный антивирус CloudScan.Pro</a> для веб-специалистов</b> &mdash; лечение сайтов в один клик</li>
               <li style="margin-top: 10px"><b><a href="https://revisium.com/ru/antivirus-server/" target=_blank>Антивирус для сервера</a></b> &mdash; для хостин-компаний, веб-студий и агентств.</li>
              </ul>  
	</div>
OFFER2;

} else {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// ENGLISH INTERFACE
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
$msg1 = "\"Display _MENU_ records\"";
$msg2 = "\"Not found\"";
$msg3 = "\"Display from _START_ to _END_ of _TOTAL_ files\"";
$msg4 = "\"No files\"";
$msg5 = "\"(total _MAX_)\"";
$msg6 = "\"Filter/Search:\"";
$msg7 = "\"First\"";
$msg8 = "\"Previous\"";
$msg9 = "\"Next\"";
$msg10 = "\"Last\"";
$msg11 = "\": activate to sort row ascending order\"";
$msg12 = "\": activate to sort row descending order\"";

define('AI_STR_001', 'AI-Bolit v@@VERSION@@ Scan Report:');
define('AI_STR_002', '');
define('AI_STR_003', 'Caution! Do not leave either ai-bolit.php or report file on server and do not provide direct links to the report file. Report file contains sensitive information about your website which could be used by hackers. So keep it in safe place and don\'t leave on website!');
define('AI_STR_004', 'Path');
define('AI_STR_005', 'iNode Changed');
define('AI_STR_006', 'Modified');
define('AI_STR_007', 'Size');
define('AI_STR_008', 'PHP Info');
define('AI_STR_009', "Your password for AI-BOLIT is too weak. Password must be more than 8 character length, contain both latin letters in upper and lower case, and digits. E.g. <b>%s</b>");
define('AI_STR_010', "Open AI-BOLIT with password specified in the beggining of file in PASS variable. <br/>E.g. http://you_website.com/ai-bolit.php?p=<b>%s</b>");
define('AI_STR_011', 'Current folder is not readable. Please change permission for <b>rwxr-xr-x</b> or using command line <b>chmod +r folder_name</b>');
define('AI_STR_012', "<div class=\"rep\">%s malicious signatures known, %s virus signatures and other malicious code. Elapsed: <b>%s</b
>.<br/>Started: %s. Stopped: %s</div> ");
define('AI_STR_013', 'Scanned %s folders and %s files.');
define('AI_STR_014', '<div class="rep" style="color: #0000A0">Attention! Script has performed quick scan. It scans only .html/.js/.php files  in quick scan mode so some of malicious scripts might not be detected. <br>Please launch script from a command line thru SSH to perform full scan.');
define('AI_STR_015', '<div class="title">Critical</div>');
define('AI_STR_016', 'Shell script signatures detected. Might be a malicious or hacker\'s scripts');
define('AI_STR_017', 'Shell scripts signatures not detected.');
define('AI_STR_018', 'Javascript virus signatures detected:');
define('AI_STR_019', 'Unix executables signatures and odd scripts detected. They might be a malicious binaries or rootkits:');
define('AI_STR_020', 'Suspicious encoded strings, extra .php extention or external includes detected in PHP files. Might be a malicious or hacker\'s script:');
define('AI_STR_021', 'Might be a malicious or hacker\'s script:');
define('AI_STR_022', 'Symlinks:');
define('AI_STR_023', 'Hidden files:');
define('AI_STR_024', 'Files might be a part of doorway:');
define('AI_STR_025', 'Doorway folders not detected');
define('AI_STR_026', 'Warnings');
define('AI_STR_027', 'Malicious code in .htaccess (redirect to external server, extention handler replacement or malicious code auto-append):');
define('AI_STR_028', 'Non-PHP file has PHP signature. Check for malicious code:');
define('AI_STR_029', 'This script has black-SEO links or linkfarm. Check if it was installed by yourself:');
define('AI_STR_030', 'Reading error. Skipped.');
define('AI_STR_031', 'These files have invisible links, might be black-seo stuff:');
define('AI_STR_032', 'List of invisible links:');
define('AI_STR_033', 'Displayed first ');
define('AI_STR_034', 'Folders contained too many .php or .html files. Might be a doorway:');
define('AI_STR_035', 'Suspicious code detected. It\'s usually used in malicious scrips:');
define('AI_STR_036', 'The following list of files specified in .adirignore has been skipped:');
define('AI_STR_037', 'CMS found:');
define('AI_STR_038', 'Large files (greater than %s! Skipped:');
define('AI_STR_039', 'Files greater than %s not found');
define('AI_STR_040', 'Files recommended to be remove due to security reason:');
define('AI_STR_041', 'Potentially unsafe! Folders which are writable for scripts:');
define('AI_STR_042', 'Writable folders not found');
define('AI_STR_043', 'Memory used: ');
define('AI_STR_044', 'Quick scan through the files from ' . DOUBLECHECK_FILE . '. For full scan remove ' . DOUBLECHECK_FILE . ' and launch scanner once again.');
define('AI_STR_045', '<div class="notice"><span class="vir">[!]</span> Ai-BOLIT is working in quick scan mode, only .php, .html, .htaccess files will be checked. Change the following setting \'scan_all_files\' => 1 to perform full scanning.</b>. </div>');
define('AI_STR_050', "I'm sincerely appreciate reports for any bugs you may found in the script. Please email me: <a href=\"mailto:audit@revisium.com\">audit@revisium.com</a>.<p> Also I appriciate any reference to the script in your blog or forum posts. Thank you for the link to download page: <a href=\"https://revisium.com/aibo/\">https://revisium.com/aibo/</a>");
define('AI_STR_051', 'Report for ');
define('AI_STR_052', 'Heuristic Analyzer has detected suspicious files. Check if they are malware.');
define('AI_STR_053', 'Function called by reference');
define('AI_STR_054', 'Suspected for obfuscated variables');
define('AI_STR_055', 'Suspected for $GLOBAL array usage');
define('AI_STR_056', 'Abnormal split of string');
define('AI_STR_057', 'Scanning has been done in simple mode. It is strongly recommended to perform scanning in "Expert" mode. See readme.txt for details.');
define('AI_STR_058', 'Phishing pages detected:');

define('AI_STR_059', 'Mobile redirects');
define('AI_STR_060', 'Malware');
define('AI_STR_061', 'JS viruses');
define('AI_STR_062', 'Phishing pages');
define('AI_STR_063', 'Unix executables');
define('AI_STR_064', 'IFRAME injections');
define('AI_STR_065', 'Skipped big files');
define('AI_STR_066', 'Reading errors');
define('AI_STR_067', 'Encrypted files');
define('AI_STR_068', 'Suspicious (heuristics)');
define('AI_STR_069', 'Symbolic links');
define('AI_STR_070', 'Hidden files');
define('AI_STR_072', 'Adware and spam links');
define('AI_STR_073', 'Empty links');
define('AI_STR_074', 'Summary');
define('AI_STR_075', 'For non-commercial use only. In order to purchase the commercial license of the scanner contact us at ai@revisium.com');

$tmp_str =<<<HTML_FOOTER
		   <div class="disclaimer"><span class="vir">[!]</span> Disclaimer: We're not liable to you for any damages, including general, special, incidental or consequential damages arising out of the use or inability to use the script (including but not limited to loss of data or report being rendered inaccurate or failure of the script). There's no warranty for the program. Use at your own risk. 
		   </div>
		   <div class="thanx">
		      We're greatly appreciate for any references in the social medias, forums or blogs to our scanner AI-BOLIT <a href="https://revisium.com/aibo/">https://revisium.com/aibo/</a>.<br/> 
		     <p>Contact us via email if you have any questions regarding the scanner or need report analysis: <a href="mailto:ai@revisium.com">ai@revisium.com</a>.</p> 
			</div>
HTML_FOOTER;
define('AI_STR_076', $tmp_str);
define('AI_STR_077', "Suspicious file mtime and ctime");
define('AI_STR_078', "Suspicious file permissions");
define('AI_STR_079', "Suspicious file location");
define('AI_STR_081', "Vulnerable Scripts");
define('AI_STR_082', "Added files");
define('AI_STR_083', "Modified files");
define('AI_STR_084', "Deleted files");
define('AI_STR_085', "Added directories");
define('AI_STR_086', "Deleted directories");
define('AI_STR_087', "Integrity Check Report");

$l_Offer =<<<HTML_OFFER_EN
<div>
 <div class="crit" style="font-size: 17px;"><b>Attention! The scanner has detected suspicious or malicious files.</b></div> 
 <br/>Most likely the website has been compromised. Please, <a href="https://revisium.com/en/contacts/" target=_blank>contact website security experts</a> from Revisium to check the report or clean the malware.
 <p><hr size=1></p>
 Also check your website for viruses with our free <b><a href="http://rescan.pro/?en&utm=aibo" target=_blank>online scanner ReScan.Pro</a></b>.
</div>
<br/>
<div>
   Revisium contacts: <a href="mailto:ai@revisium.com">ai@revisium.com</a>, <a href="https://revisium.com/en/contacts/">https://revisium.com/en/home/</a>
</div>
<div class="caution">@@CAUTION@@</div>
HTML_OFFER_EN;

$l_Offer2 = '<b>Special Offers:</b><br/>
              <ul>
               <li style="margin-top: 10px"><font color=red><sup>[new]</sup></font><b><a href="http://ext.plesk.com/packages/b71916cf-614e-4b11-9644-a5fe82060aaf-revisium-antivirus">Antivirus for Plesk Onyx</a></b> hosting panel with one-click malware cleanup and scheduled website scanning.</li>
               <li style="margin-top: 10px">Professional malware cleanup and web-protection service with 6 month guarantee for only $99 (one-time payment): <a href="https://revisium.com/en/home/#order_form">https://revisium.com/en/home/</a>.</li>
              </ul>  
	</div>';

define('AI_STR_080', "Notice! Some of detected files may not contain malicious code. Scanner tries to minimize a number of false positives, but sometimes it's impossible, because same piece of code may be used either in malware or in normal scripts.");
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

$l_Template =<<<MAIN_PAGE
<html>
<head>
<!-- revisium.com/ai/ -->
<meta http-equiv="Content-Type" content="text/html;charset=utf-8" >
<META NAME="ROBOTS" CONTENT="NOINDEX,NOFOLLOW">
<title>@@HEAD_TITLE@@</title>
<style type="text/css" title="currentStyle">
	@import "https://cdn.revisium.com/ai/media/css/demo_page2.css";
	@import "https://cdn.revisium.com/ai/media/css/jquery.dataTables2.css";
</style>

<script type="text/javascript" language="javascript" src="https://cdn.revisium.com/ai/jquery.js"></script>
<script type="text/javascript" language="javascript" src="https://cdn.revisium.com/ai/datatables.min.js"></script>

<style type="text/css">
 body 
 {
   font-family: Tahoma;
   color: #5a5a5a;
   background: #FFFFFF;
   font-size: 14px;
   margin: 20px;
   padding: 0;
 }

.header
 {
   font-size: 34px;
   margin: 0 0 10px 0;
 }

 .hidd
 {
    display: none;
 }
 
 .ok
 {
    color: green;
 }
 
 .line_no
 {
   -webkit-border-radius: 4px;
   -moz-border-radius: 4px;
   border-radius: 4px;

   background: #DAF2C1;
   padding: 2px 5px 2px 5px;
   margin: 0 5px 0 5px;
 }
 
 .credits_header 
 {
  -webkit-border-radius: 4px;
   -moz-border-radius: 4px;
   border-radius: 4px;

   background: #F2F2F2;
   padding: 10px;
   font-size: 11px;
    margin: 0 0 10px 0;
 }
 
 .marker
 {
    color: #FF0090;
	font-weight: 100;
	background: #FF0090;
	padding: 2px 0px 2px 0px;
	width: 2px;
 }
 
 .title
 {
   font-size: 24px;
   margin: 20px 0 10px 0;
   color: #9CA9D1;
}

.summary 
{
  float: left;
  width: 500px;
}

.summary TD
{
  font-size: 12px;
  border-bottom: 1px solid #F0F0F0;
  font-weight: 700;
  padding: 10px 0 10px 0;
}
 
.crit, .vir
{
  color: #D84B55;
}

.intitem
{
  color:#4a6975;
}

.spacer
{
   margin: 0 0 50px 0;
   clear:both;
}

.warn
{
  color: #F6B700;
}

.clear
{
   clear: both;
}

.offer
{
  -webkit-border-radius: 4px;
   -moz-border-radius: 4px;
   border-radius: 4px;

   width: 500px;
   background: #F2F2F2;
   color: #747474;
   font-family: Helvetica, Arial;
   padding: 30px;
   margin: 20px 0 0 550px;
   font-size: 14px;
}

.offer2
{
  -webkit-border-radius: 4px;
   -moz-border-radius: 4px;
   border-radius: 4px;

   width: 500px;
   background: #f6f5e0;
   color: #747474;
   font-family: Helvetica, Arial;
   padding: 30px;
   margin: 20px 0 0 550px;
   font-size: 14px;
}


HR {
  margin-top: 15px;
  margin-bottom: 15px;
  opacity: .2;
}
 
.flist
{
   font-family: Henvetica, Arial;
}

.flist TD
{
   font-size: 11px;
   padding: 5px;
}

.flist TH
{
   font-size: 12px;
   height: 30px;
   padding: 5px;
   background: #CEE9EF;
}


.it
{
   font-size: 14px;
   font-weight: 100;
   margin-top: 10px;
}

.crit .it A {
   color: #E50931; 
   line-height: 25px;
   text-decoration: none;
}

.warn .it A {
   color: #F2C900; 
   line-height: 25px;
   text-decoration: none;
}



.details
{
   font-family: Calibri;
   font-size: 12px;
   margin: 10px 10px 10px 0px;
}

.crit .details
{
   color: #A08080;
}

.warn .details
{
   color: #808080;
}

.details A
{
  color: #FFF;
  font-weight: 700;
  text-decoration: none;
  padding: 2px;
  background: #E5CEDE;
  -webkit-border-radius: 7px;
   -moz-border-radius: 7px;
   border-radius: 7px;
}

.details A:hover
{
   background: #A0909B;
}

.ctd
{
   margin: 10px 0px 10px 0;
   align:center;
}

.ctd A 
{
   color: #0D9922;
}

.disclaimer
{
   color: darkgreen;
   margin: 10px 10px 10px 0;
}

.note_vir
{
   margin: 10px 0 10px 0;
   //padding: 10px;
   color: #FF4F4F;
   font-size: 15px;
   font-weight: 700;
   clear:both;
  
}

.note_warn
{
   margin: 10px 0 10px 0;
   color: #F6B700;
   font-size: 15px;
   font-weight: 700;
   clear:both;
}

.note_int
{
   margin: 10px 0 10px 0;
   color: #60b5d6;
   font-size: 15px;
   font-weight: 700;
   clear:both;
}

.updateinfo
{
  color: #FFF;
  text-decoration: none;
  background: #E5CEDE;
  -webkit-border-radius: 7px;
   -moz-border-radius: 7px;
   border-radius: 7px;

  margin: 10px 0 10px 0px;   
  padding: 10px;
}


.caution
{
  color: #EF7B75;
  text-decoration: none;
  margin: 20px 0 0px 0px;   
  font-size: 12px;
}

.footer
{
  color: #303030;
  text-decoration: none;
  background: #F4F4F4;
  -webkit-border-radius: 7px;
   -moz-border-radius: 7px;
   border-radius: 7px;

  margin: 80px 0 10px 0px;   
  padding: 10px;
}

.rep
{
  color: #303030;
  text-decoration: none;
  background: #94DDDB;
  -webkit-border-radius: 7px;
   -moz-border-radius: 7px;
   border-radius: 7px;

  margin: 10px 0 10px 0px;   
  padding: 10px;
  font-size: 12px;
}

</style>

</head>
<body>

<div class="header">@@MAIN_TITLE@@ @@PATH_URL@@ (@@MODE@@)</div>
<div class="credits_header">@@CREDITS@@</div>
<div class="details_header">
   @@STAT@@<br/>
   @@SCANNED@@ @@MEMORY@@.
 </div>

 @@WARN_QUICK@@
 
 <div class="summary">
@@SUMMARY@@
 </div>
 
 <div class="offer">
@@OFFER@@
 </div>

 <div class="offer2">
@@OFFER2@@
 </div> 
 
 <div class="clear"></div>
 
 @@MAIN_CONTENT@@
 
	<div class="footer">
	@@FOOTER@@
	</div>
	
<script language="javascript">

function hsig(id) {
  var divs = document.getElementsByTagName("tr");
  for(var i = 0; i < divs.length; i++){
     
     if (divs[i].getAttribute('o') == id) {
        divs[i].innerHTML = '';
     }
  }

  return false;
}


$(document).ready(function(){
    $('#table_crit').dataTable({
       "aLengthMenu": [[100 , 500, -1], [100, 500, "All"]],
       "aoColumns": [
                                     {"iDataSort": 7, "width":"70%"},
                                     {"iDataSort": 5},
                                     {"iDataSort": 6},
                                     {"bSortable": true},
                                     {"bVisible": false},
                                     {"bVisible": false},
                                     {"bVisible": false},
                                     {"bVisible": false}
                     ],
		"paging": true,
       "iDisplayLength": 500,
		"oLanguage": {
			"sLengthMenu": $msg1,
			"sZeroRecords": $msg2,
			"sInfo": $msg3,
			"sInfoEmpty": $msg4,
			"sInfoFiltered": $msg5,
			"sSearch":       $msg6,
			"sUrl":          "",
			"oPaginate": {
				"sFirst": $msg7,
				"sPrevious": $msg8,
				"sNext": $msg9,
				"sLast": $msg10
			},
			"oAria": {
				"sSortAscending": $msg11,
				"sSortDescending": $msg12	
			}
		}

     } );

});

$(document).ready(function(){
    $('#table_vir').dataTable({
       "aLengthMenu": [[100 , 500, -1], [100, 500, "All"]],
		"paging": true,
       "aoColumns": [
                                     {"iDataSort": 7, "width":"70%"},
                                     {"iDataSort": 5},
                                     {"iDataSort": 6},
                                     {"bSortable": true},
                                     {"bVisible": false},
                                     {"bVisible": false},
                                     {"bVisible": false},
                                     {"bVisible": false}
                     ],
       "iDisplayLength": 500,
		"oLanguage": {
			"sLengthMenu": $msg1,
			"sZeroRecords": $msg2,
			"sInfo": $msg3,
			"sInfoEmpty": $msg4,
			"sInfoFiltered": $msg5,
			"sSearch":       $msg6,
			"sUrl":          "",
			"oPaginate": {
				"sFirst": $msg7,
				"sPrevious": $msg8,
				"sNext": $msg9,
				"sLast": $msg10
			},
			"oAria": {
				"sSortAscending":  $msg11,
				"sSortDescending": $msg12	
			}
		},

     } );

});

if ($('#table_warn0')) {
    $('#table_warn0').dataTable({
       "aLengthMenu": [[100 , 500, -1], [100, 500, "All"]],
		"paging": true,
       "aoColumns": [
                                     {"iDataSort": 7, "width":"70%"},
                                     {"iDataSort": 5},
                                     {"iDataSort": 6},
                                     {"bSortable": true},
                                     {"bVisible": false},
                                     {"bVisible": false},
                                     {"bVisible": false},
                                     {"bVisible": false}
                     ],
			         "iDisplayLength": 500,
			  		"oLanguage": {
			  			"sLengthMenu": $msg1,
			  			"sZeroRecords": $msg2,
			  			"sInfo": $msg3,
			  			"sInfoEmpty": $msg4,
			  			"sInfoFiltered": $msg5,
			  			"sSearch":       $msg6,
			  			"sUrl":          "",
			  			"oPaginate": {
			  				"sFirst": $msg7,
			  				"sPrevious": $msg8,
			  				"sNext": $msg9,
			  				"sLast": $msg10
			  			},
			  			"oAria": {
			  				"sSortAscending":  $msg11,
			  				"sSortDescending": $msg12	
			  			}
		}

     } );
}

if ($('#table_warn1')) {
    $('#table_warn1').dataTable({
       "aLengthMenu": [[100 , 500, -1], [100, 500, "All"]],
		"paging": true,
       "aoColumns": [
                                     {"iDataSort": 7, "width":"70%"},
                                     {"iDataSort": 5},
                                     {"iDataSort": 6},
                                     {"bSortable": true},
                                     {"bVisible": false},
                                     {"bVisible": false},
                                     {"bVisible": false},
                                     {"bVisible": false}
                     ],
			         "iDisplayLength": 500,
			  		"oLanguage": {
			  			"sLengthMenu": $msg1,
			  			"sZeroRecords": $msg2,
			  			"sInfo": $msg3,
			  			"sInfoEmpty": $msg4,
			  			"sInfoFiltered": $msg5,
			  			"sSearch":       $msg6,
			  			"sUrl":          "",
			  			"oPaginate": {
			  				"sFirst": $msg7,
			  				"sPrevious": $msg8,
			  				"sNext": $msg9,
			  				"sLast": $msg10
			  			},
			  			"oAria": {
			  				"sSortAscending":  $msg11,
			  				"sSortDescending": $msg12	
			  			}
		}

     } );
}


</script>
<!-- @@SERVICE_INFO@@  -->
 </body>
</html>
MAIN_PAGE;

$g_AiBolitAbsolutePath = dirname(__FILE__);

if (file_exists($g_AiBolitAbsolutePath . '/ai-design.html')) {
  $l_Template = file_get_contents($g_AiBolitAbsolutePath . '/ai-design.html');
}

$l_Template = str_replace('@@MAIN_TITLE@@', AI_STR_001, $l_Template);

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//BEGIN_SIG 02/05/2018 12:03:33
$g_DBShe = unserialize(gzinflate(/*1525208613*/base64_decode("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")));
$gX_DBShe = unserialize(gzinflate(/*1525208613*/base64_decode("bVX9b+I4EP1XvDn2oFIpCUmgpNvqKLDtXT9V2L1dVVVkEhOsOB9rO7Swuvvbzx67tFodv5A8j59n3htPcDTwop80ck9ENIyclKxwQrhzQiNPIf0gcr5XDUpwiepGIoyKNERCclpmOqavYo4jp17XYk0Y05CvoEE/cj6ldIMShoU4dZasSnK0lNuaeM7Z+5VUVvUvyFIWzplmCgx5MhrtyUMD8XC4hwYm8dvFFKfpViNDu4+u90HHJujzC5BpZKQQT6X5N1miuQ5DS9jsaSE8TyuxoWw3f2XwQI4wci5xkpNURaMxW+ElkdVXymWDTZQV5NaX+7M93zLOZcWL4RtjYPHrKmnEOzy0qb1WeaTkhQVd6ihycCnpG/vQshRbwMgLSQA/NpmY2u5ywKBoZakO6ji8QF2OuiswEsrWmRPUpJhVAu1wxpsdlbCsqw/VKa2CCIEzgk4R4SSLOamZapiO8zGcfOz3nUPkmL/XwIMT2K918VTuteociZx5jQuSts8+LTlY3dcaBW7kCCKTqsop6SBd0Q8WP5NljNOClnEjCC/VPgdZUi2gr2wlTBC66qyaMpG0KmPyQoUUHQcEiUGRgwPYoaUNVRp0hTpUxAlmDC+ZSt8EIVym6IM6CnOOtxY9bKVU6CjNb3m0E35geDqtmvBCoN+R+zJxXfcAnZ7uH39COHikqkspR727C9T7BrC2yB8oljJhTUo6rXg+e/g6e3hsXy4W9/EX9RaPL2a3i/aTOXZk2njJ3WfhwyX1tW+Bqqi9lrJOj5KqXLUP25t1pRR4fUtWme4h/aQAal5gt2fr6NW8SnpiK3q50piw3hYXuFdLrryNRVLVBMK1i31fCb7BrLOijMQZkbEilaTUgusY3zaSkveZU2nk7W0w78FyYFuwdXF9dz6+nj+2Y/WDJTAnhJ17Y9ragvavxhhUNdk7ZwRwaGOUormrpIB74Gvt+wrKCkxZVxSy7tLyiB1lVZUxoiQqIMxOCNVt3HWh5/2RvVlTPQng2oPkgR2V07dRGXg29BHdr5WUL1v0BLgdB49JlRK+M5gWyNcD8kO3+5suBCVFeuq0wPPxZDK7Xzio2zUjMLCz4OmRV02ZdtyDJ5A5CF+HCi3GUr4lpwXwlIhTzPMJJ8+fOSVlCuIEQ5N43mfM91OAbNlX+ZXn+0OARpbhcjy5mk3R+Xc0X9w93MD4da17N+qKqW+CEQXIQ89uu+FH87Xq6DUuv3mjAaz1zeCaEppijv+F7g99eyn+omWtRlpV7AAOTKxXDv1Eem4JYGjPvVLfnXnOm1pd7MqcO/h/mqGhmZAyo7tLbGiODcj9jV9mQWkIRgY8v1b1frn9E5IbuHbUckmTnJE/nJN//gM=")));
$g_FlexDBShe = unserialize(gzinflate(/*1525208613*/base64_decode("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")));
$gX_FlexDBShe = unserialize(gzinflate(/*1525208613*/base64_decode("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")));
$gXX_FlexDBShe = unserialize(gzinflate(/*1525208613*/base64_decode("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")));
$g_ExceptFlex = unserialize(gzinflate(/*1525208613*/base64_decode("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")));
$g_AdwareSig = unserialize(gzinflate(/*1525208613*/base64_decode("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")));
$g_PhishingSig = unserialize(gzinflate(/*1525208613*/base64_decode("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")));
$g_JSVirSig = unserialize(gzinflate(/*1525208613*/base64_decode("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")));
$gX_JSVirSig = unserialize(gzinflate(/*1525208613*/base64_decode("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")));
$g_SusDB = unserialize(gzinflate(/*1525208613*/base64_decode("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")));
$g_SusDBPrio = unserialize(gzinflate(/*1525208613*/base64_decode("RdPLccMwDEXRloQvAaWaLFNDxr1nZOMiCw8wFHX4BFnft6jcvz/39fX5yVSdalN9akzNqWdqzb09VRZEFEjBFFBBFVjBlaJpkiHrZkVWZEVWZEVWZEVWMhsDMGTbMSAbsiEbsiEbspHZyezIjuw7YWRHdmRHdmRHDuRADuRAjn15yJHzpIEcyNHz9pNpJHIiJ3I6m/d/QeZEzmIPcz5kPshHuYR8nJWgyXHOYeWRnxsOmQu5hBUyF3KRuchcZK79NzONInMzjRYaMjdyI3fQkLkPDdPo/y/l2k7mgeTS7Wyv+q7Fdhwg134xV+3VPUP2DNkzZM94f5GfzndfbJfbPWe8d0jtWnPvfJivPw==")));
$g_DeMapper = unserialize(base64_decode("YTo1OntzOjEwOiJ3aXphcmQucGhwIjtzOjM3OiJjbGFzcyBXZWxjb21lU3RlcCBleHRlbmRzIENXaXphcmRTdGVwIjtzOjE3OiJ1cGRhdGVfY2xpZW50LnBocCI7czozNzoieyBDVXBkYXRlQ2xpZW50OjpBZGRNZXNzYWdlMkxvZygiZXhlYyI7czoxMToiaW5jbHVkZS5waHAiO3M6NDg6IkdMT0JBTFNbIlVTRVIiXS0+SXNBdXRob3JpemVkKCkgJiYgJGFyQXV0aFJlc3VsdCI7czo5OiJzdGFydC5waHAiO3M6NjA6IkJYX1JPT1QuJy9tb2R1bGVzL21haW4vY2xhc3Nlcy9nZW5lcmFsL3VwZGF0ZV9kYl91cGRhdGVyLnBocCI7czoxMDoiaGVscGVyLnBocCI7czo1ODoiSlBsdWdpbkhlbHBlcjo6Z2V0UGx1Z2luKCJzeXN0ZW0iLCJvbmVjbGlja2NoZWNrb3V0X3ZtMyIpOyI7fQ=="));

//END_SIG
////////////////////////////////////////////////////////////////////////////
if (!isCli() && !isset($_SERVER['HTTP_USER_AGENT'])) {
  echo "#####################################################\n";
  echo "# Error: cannot run on php-cgi. Requires php as cli #\n";
  echo "#                                                   #\n";
  echo "# See FAQ: http://revisium.com/ai/faq.php           #\n";
  echo "#####################################################\n";
  exit;
}


if (version_compare(phpversion(), '5.3.1', '<')) {
  echo "#####################################################\n";
  echo "# Warning: PHP Version < 5.3.1                      #\n";
  echo "# Some function might not work properly             #\n";
  echo "# See FAQ: http://revisium.com/ai/faq.php           #\n";
  echo "#####################################################\n";
  exit;
}

if (!(function_exists("file_put_contents") && is_callable("file_put_contents"))) {
    echo "#####################################################\n";
	echo "file_put_contents() is disabled. Cannot proceed.\n";
    echo "#####################################################\n";	
    exit;
}
                              
define('AI_VERSION', '20180501');

////////////////////////////////////////////////////////////////////////////

$l_Res = '';

$g_Structure = array();
$g_Counter = 0;

$g_SpecificExt = false;

$g_UpdatedJsonLog = 0;
$g_NotRead = array();
$g_FileInfo = array();
$g_Iframer = array();
$g_PHPCodeInside = array();
$g_CriticalJS = array();
$g_Phishing = array();
$g_Base64 = array();
$g_HeuristicDetected = array();
$g_HeuristicType = array();
$g_UnixExec = array();
$g_SkippedFolders = array();
$g_UnsafeFilesFound = array();
$g_CMS = array();
$g_SymLinks = array();
$g_HiddenFiles = array();
$g_Vulnerable = array();

$g_RegExpStat = array();

$g_TotalFolder = 0;
$g_TotalFiles = 0;

$g_FoundTotalDirs = 0;
$g_FoundTotalFiles = 0;

if (!isCli()) {
   $defaults['site_url'] = 'http://' . $_SERVER['HTTP_HOST'] . '/'; 
}

define('CRC32_LIMIT', pow(2, 31) - 1);
define('CRC32_DIFF', CRC32_LIMIT * 2 -2);

error_reporting(E_ALL ^ E_NOTICE ^ E_WARNING);
srand(time());

set_time_limit(0);
ini_set('max_execution_time', '900000');
ini_set('realpath_cache_size','16M');
ini_set('realpath_cache_ttl','1200');
ini_set('pcre.backtrack_limit','1000000');
ini_set('pcre.recursion_limit','200000');
ini_set('pcre.jit','1');

if (!function_exists('stripos')) {
	function stripos($par_Str, $par_Entry, $Offset = 0) {
		return strpos(strtolower($par_Str), strtolower($par_Entry), $Offset);
	}
}

define('CMS_BITRIX', 'Bitrix');
define('CMS_WORDPRESS', 'Wordpress');
define('CMS_JOOMLA', 'Joomla');
define('CMS_DLE', 'Data Life Engine');
define('CMS_IPB', 'Invision Power Board');
define('CMS_WEBASYST', 'WebAsyst');
define('CMS_OSCOMMERCE', 'OsCommerce');
define('CMS_DRUPAL', 'Drupal');
define('CMS_MODX', 'MODX');
define('CMS_INSTANTCMS', 'Instant CMS');
define('CMS_PHPBB', 'PhpBB');
define('CMS_VBULLETIN', 'vBulletin');
define('CMS_SHOPSCRIPT', 'PHP ShopScript Premium');

define('CMS_VERSION_UNDEFINED', '0.0');

class CmsVersionDetector {
    private $root_path;
    private $versions;
    private $types;

    public function __construct($root_path = '.') {
        $this->root_path = $root_path;
        $this->versions = array();
        $this->types = array();

        $version = '';

        $dir_list = $this->getDirList($root_path);
        $dir_list[] = $root_path;

        foreach ($dir_list as $dir) {
            if ($this->checkBitrix($dir, $version)) {
               $this->addCms(CMS_BITRIX, $version);
            }

            if ($this->checkWordpress($dir, $version)) {
               $this->addCms(CMS_WORDPRESS, $version);
            }

            if ($this->checkJoomla($dir, $version)) {
               $this->addCms(CMS_JOOMLA, $version);
            }

            if ($this->checkDle($dir, $version)) {
               $this->addCms(CMS_DLE, $version);
            }

            if ($this->checkIpb($dir, $version)) {
               $this->addCms(CMS_IPB, $version);
            }

            if ($this->checkWebAsyst($dir, $version)) {
               $this->addCms(CMS_WEBASYST, $version);
            }

            if ($this->checkOsCommerce($dir, $version)) {
               $this->addCms(CMS_OSCOMMERCE, $version);
            }

            if ($this->checkDrupal($dir, $version)) {
               $this->addCms(CMS_DRUPAL, $version);
            }

            if ($this->checkMODX($dir, $version)) {
               $this->addCms(CMS_MODX, $version);
            }

            if ($this->checkInstantCms($dir, $version)) {
               $this->addCms(CMS_INSTANTCMS, $version);
            }

            if ($this->checkPhpBb($dir, $version)) {
               $this->addCms(CMS_PHPBB, $version);
            }

            if ($this->checkVBulletin($dir, $version)) {
               $this->addCms(CMS_VBULLETIN, $version);
            }

            if ($this->checkPhpShopScript($dir, $version)) {
               $this->addCms(CMS_SHOPSCRIPT, $version);
            }

        }
    }

    function getDirList($target) {
       $remove = array('.', '..'); 
       $directories = array_diff(scandir($target), $remove);

       $res = array();
           
       foreach($directories as $value) 
       { 
          if(is_dir($target . '/' . $value)) 
          {
             $res[] = $target . '/' . $value; 
          } 
       }

       return $res;
    }

    function isCms($name, $version) {
		for ($i = 0; $i < count($this->types); $i++) {
			if ((strpos($this->types[$i], $name) !== false) 
				&& 
			    (strpos($this->versions[$i], $version) !== false)) {
				return true;
			}
		}
    	
		return false;
    }

    function getCmsList() {
      return $this->types;
    }

    function getCmsVersions() {
      return $this->versions;
    }

    function getCmsNumber() {
      return count($this->types);
    }

    function getCmsName($index = 0) {
      return $this->types[$index];
    }

    function getCmsVersion($index = 0) {
      return $this->versions[$index];
    }

    private function addCms($type, $version) {
       $this->types[] = $type;
       $this->versions[] = $version;
    }

    private function checkBitrix($dir, &$version) {
       $version = CMS_VERSION_UNDEFINED;
       $res = false;

       if (file_exists($dir .'/bitrix')) {
          $res = true;

          $tmp_content = @file_get_contents($this->root_path .'/bitrix/modules/main/classes/general/version.php');
          if (preg_match('|define\("SM_VERSION","(.+?)"\)|smi', $tmp_content, $tmp_ver)) {
             $version = $tmp_ver[1];
          }

       }

       return $res;
    }

    private function checkWordpress($dir, &$version) {
       $version = CMS_VERSION_UNDEFINED;
       $res = false;

       if (file_exists($dir .'/wp-admin')) {
          $res = true;

          $tmp_content = @file_get_contents($dir .'/wp-includes/version.php');
          if (preg_match('|\$wp_version\s*=\s*\'(.+?)\'|smi', $tmp_content, $tmp_ver)) {
             $version = $tmp_ver[1];
          }
       }

       return $res;
    }

    private function checkJoomla($dir, &$version) {
       $version = CMS_VERSION_UNDEFINED;
       $res = false;

       if (file_exists($dir .'/libraries/joomla')) {
          $res = true;

          // for 1.5.x
          $tmp_content = @file_get_contents($dir .'/libraries/joomla/version.php');
          if (preg_match('|var\s+\$RELEASE\s*=\s*\'(.+?)\'|smi', $tmp_content, $tmp_ver)) {
             $version = $tmp_ver[1];

             if (preg_match('|var\s+\$DEV_LEVEL\s*=\s*\'(.+?)\'|smi', $tmp_content, $tmp_ver)) {
                $version .= '.' . $tmp_ver[1];
             }
          }

          // for 1.7.x
          $tmp_content = @file_get_contents($dir .'/includes/version.php');
          if (preg_match('|public\s+\$RELEASE\s*=\s*\'(.+?)\'|smi', $tmp_content, $tmp_ver)) {
             $version = $tmp_ver[1];

             if (preg_match('|public\s+\$DEV_LEVEL\s*=\s*\'(.+?)\'|smi', $tmp_content, $tmp_ver)) {
                $version .= '.' . $tmp_ver[1];
             }
          }


	  // for 2.5.x and 3.x 
          $tmp_content = @file_get_contents($dir . '/libraries/cms/version/version.php');
   
          if (preg_match('|const\s+RELEASE\s*=\s*\'(.+?)\'|smi', $tmp_content, $tmp_ver)) {
	      $version = $tmp_ver[1];
 
             if (preg_match('|const\s+DEV_LEVEL\s*=\s*\'(.+?)\'|smi', $tmp_content, $tmp_ver)) { 
		$version .= '.' . $tmp_ver[1];
             }
          }

       }

       return $res;
    }

    private function checkDle($dir, &$version) {
       $version = CMS_VERSION_UNDEFINED;
       $res = false;

       if (file_exists($dir .'/engine/engine.php')) {
          $res = true;

          $tmp_content = @file_get_contents($dir . '/engine/data/config.php');
          if (preg_match('|\'version_id\'\s*=>\s*"(.+?)"|smi', $tmp_content, $tmp_ver)) {
             $version = $tmp_ver[1];
          }

          $tmp_content = @file_get_contents($dir . '/install.php');
          if (preg_match('|\'version_id\'\s*=>\s*"(.+?)"|smi', $tmp_content, $tmp_ver)) {
             $version = $tmp_ver[1];
          }

       }

       return $res;
    }

    private function checkIpb($dir, &$version) {
       $version = CMS_VERSION_UNDEFINED;
       $res = false;

       if (file_exists($dir . '/ips_kernel')) {
          $res = true;

          $tmp_content = @file_get_contents($dir . '/ips_kernel/class_xml.php');
          if (preg_match('|IP.Board\s+v([0-9\.]+)|si', $tmp_content, $tmp_ver)) {
             $version = $tmp_ver[1];
          }

       }

       return $res;
    }

    private function checkWebAsyst($dir, &$version) {
       $version = CMS_VERSION_UNDEFINED;
       $res = false;

       if (file_exists($dir . '/wbs/installer')) {
          $res = true;

          $tmp_content = @file_get_contents($dir . '/license.txt');
          if (preg_match('|v([0-9\.]+)|si', $tmp_content, $tmp_ver)) {
             $version = $tmp_ver[1];
          }

       }

       return $res;
    }

    private function checkOsCommerce($dir, &$version) {
       $version = CMS_VERSION_UNDEFINED;
       $res = false;

       if (file_exists($dir . '/includes/version.php')) {
          $res = true;

          $tmp_content = @file_get_contents($dir . '/includes/version.php');
          if (preg_match('|([0-9\.]+)|smi', $tmp_content, $tmp_ver)) {
             $version = $tmp_ver[1];
          }

       }

       return $res;
    }

    private function checkDrupal($dir, &$version) {
       $version = CMS_VERSION_UNDEFINED;
       $res = false;

       if (file_exists($dir . '/sites/all')) {
          $res = true;

          $tmp_content = @file_get_contents($dir . '/CHANGELOG.txt');
          if (preg_match('|Drupal\s+([0-9\.]+)|smi', $tmp_content, $tmp_ver)) {
             $version = $tmp_ver[1];
          }

       }

       if (file_exists($dir . '/core/lib/Drupal.php')) {
          $res = true;

          $tmp_content = @file_get_contents($dir . '/core/lib/Drupal.php');
          if (preg_match('|VERSION\s*=\s*\'(\d+\.\d+\.\d+)\'|smi', $tmp_content, $tmp_ver)) {
             $version = $tmp_ver[1];
          }

       }

       if (file_exists($dir . 'modules/system/system.info')) {
          $res = true;

          $tmp_content = @file_get_contents($dir . 'modules/system/system.info');
          if (preg_match('|version\s*=\s*"\d+\.\d+"|smi', $tmp_content, $tmp_ver)) {
             $version = $tmp_ver[1];
          }

       }

       return $res;
    }

    private function checkMODX($dir, &$version) {
       $version = CMS_VERSION_UNDEFINED;
       $res = false;

       if (file_exists($dir . '/manager/assets')) {
          $res = true;

          // no way to pick up version
       }

       return $res;
    }

    private function checkInstantCms($dir, &$version) {
       $version = CMS_VERSION_UNDEFINED;
       $res = false;

       if (file_exists($dir . '/plugins/p_usertab')) {
          $res = true;

          $tmp_content = @file_get_contents($dir . '/index.php');
          if (preg_match('|InstantCMS\s+v([0-9\.]+)|smi', $tmp_content, $tmp_ver)) {
             $version = $tmp_ver[1];
          }

       }

       return $res;
    }

    private function checkPhpBb($dir, &$version) {
       $version = CMS_VERSION_UNDEFINED;
       $res = false;

       if (file_exists($dir . '/includes/acp')) {
          $res = true;

          $tmp_content = @file_get_contents($dir . '/config.php');
          if (preg_match('|phpBB\s+([0-9\.x]+)|smi', $tmp_content, $tmp_ver)) {
             $version = $tmp_ver[1];
          }

       }

       return $res;
    }

    private function checkVBulletin($dir, &$version) {
          $version = CMS_VERSION_UNDEFINED;
          $res = false;
          if (file_exists($dir . '/core/includes/md5_sums_vbulletin.php'))
          {
                $res = true;
                require_once($dir . '/core/includes/md5_sums_vbulletin.php');
                $version = $md5_sum_versions['vb5_connect'];
          }
          else if(file_exists($dir . '/includes/md5_sums_vbulletin.php'))
          {
                $res = true;
                require_once($dir . '/includes/md5_sums_vbulletin.php');
                $version = $md5_sum_versions['vbulletin'];
          }
          return $res;
       }

    private function checkPhpShopScript($dir, &$version) {
       $version = CMS_VERSION_UNDEFINED;
       $res = false;

       if (file_exists($dir . '/install/consts.php')) {
          $res = true;

          $tmp_content = @file_get_contents($dir . '/install/consts.php');
          if (preg_match('|STRING_VERSION\',\s*\'(.+?)\'|smi', $tmp_content, $tmp_ver)) {
             $version = $tmp_ver[1];
          }

       }

       return $res;
    }
}

/**
 * Print file
*/
function printFile() {
	$l_FileName = $_GET['fn'];
	$l_CRC = isset($_GET['c']) ? (int)$_GET['c'] : 0;
	$l_Content = file_get_contents($l_FileName);
	$l_FileCRC = realCRC($l_Content);
	if ($l_FileCRC != $l_CRC) {
		echo 'Доступ запрещен.';
		exit;
	}
	
	echo '<pre>' . htmlspecialchars($l_Content) . '</pre>';
}

/**
 *
 */
function realCRC($str_in, $full = false)
{
        $in = crc32( $full ? normal($str_in) : $str_in );
        return ($in > CRC32_LIMIT) ? ($in - CRC32_DIFF) : $in;
}


/**
 * Determine php script is called from the command line interface
 * @return bool
 */
function isCli()
{
	return php_sapi_name() == 'cli';
}

function myCheckSum($str) {
   return hash('crc32b', $str);
}

 function generatePassword ($length = 9)
  {

    // start with a blank password
    $password = "";

    // define possible characters - any character in this string can be
    // picked for use in the password, so if you want to put vowels back in
    // or add special characters such as exclamation marks, this is where
    // you should do it
    $possible = "2346789bcdfghjkmnpqrtvwxyzBCDFGHJKLMNPQRTVWXYZ";

    // we refer to the length of $possible a few times, so let's grab it now
    $maxlength = strlen($possible);
  
    // check for length overflow and truncate if necessary
    if ($length > $maxlength) {
      $length = $maxlength;
    }
	
    // set up a counter for how many characters are in the password so far
    $i = 0; 
    
    // add random characters to $password until $length is reached
    while ($i < $length) { 

      // pick a random character from the possible ones
      $char = substr($possible, mt_rand(0, $maxlength-1), 1);
        
      // have we already used this character in $password?
      if (!strstr($password, $char)) { 
        // no, so it's OK to add it onto the end of whatever we've already got...
        $password .= $char;
        // ... and increase the counter by one
        $i++;
      }

    }

    // done!
    return $password;

  }

/**
 * Print to console
 * @param mixed $text
 * @param bool $add_lb Add line break
 * @return void
 */
function stdOut($text, $add_lb = true)
{
	if (!isCli())
		return;
		
	if (is_bool($text))
	{
		$text = $text ? 'true' : 'false';
	}
	else if (is_null($text))
	{
		$text = 'null';
	}
	if (!is_scalar($text))
	{
		$text = print_r($text, true);
	}

 	if (!BOOL_RESULT)
 	{
 		@fwrite(STDOUT, $text . ($add_lb ? "\n" : ''));
 	}
}

/**
 * Print progress
 * @param int $num Current file
 */
function printProgress($num, &$par_File)
{
	global $g_CriticalPHP, $g_Base64, $g_Phishing, $g_CriticalJS, $g_Iframer, $g_UpdatedJsonLog, 
               $g_AddPrefix, $g_NoPrefix;

	$total_files = $GLOBALS['g_FoundTotalFiles'];
	$elapsed_time = microtime(true) - START_TIME;
	$percent = number_format($total_files ? $num * 100 / $total_files : 0, 1);
	$stat = '';
	if ($elapsed_time >= 1)
	{
		$elapsed_seconds = round($elapsed_time, 0);
		$fs = floor($num / $elapsed_seconds);
		$left_files = $total_files - $num;
		if ($fs > 0) 
		{
		   $left_time = ($left_files / $fs); //ceil($left_files / $fs);
		   $stat = ' [Avg: ' . round($fs,2) . ' files/s' . ($left_time > 0  ? ' Left: ' . seconds2Human($left_time) : '') . '] [Mlw:' . (count($g_CriticalPHP) + count($g_Base64))  . '|' . (count($g_CriticalJS) + count($g_Iframer) + count($g_Phishing)) . ']';
        }
	}

        $l_FN = $g_AddPrefix . str_replace($g_NoPrefix, '', $par_File); 
	$l_FN = substr($par_File, -60);

	$text = "$percent% [$l_FN] $num of {$total_files}. " . $stat;
	$text = str_pad($text, 160, ' ', STR_PAD_RIGHT);
	stdOut(str_repeat(chr(8), 160) . $text, false);


      	$data = array('self' => __FILE__, 'started' => AIBOLIT_START_TIME, 'updated' => time(), 
                            'progress' => $percent, 'time_elapsed' => $elapsed_seconds, 
                            'time_left' => round($left_time), 'files_left' => $left_files, 
                            'files_total' => $total_files, 'current_file' => substr($g_AddPrefix . str_replace($g_NoPrefix, '', $par_File), -160));

        if (function_exists('aibolit_onProgressUpdate')) { aibolit_onProgressUpdate($data); }

	if (defined('PROGRESS_LOG_FILE') && 
           (time() - $g_UpdatedJsonLog > 1)) {
                if (function_exists('json_encode')) {
             	   file_put_contents(PROGRESS_LOG_FILE, json_encode($data));
                } else {
             	   file_put_contents(PROGRESS_LOG_FILE, serialize($data));
                }

		$g_UpdatedJsonLog = time();
        }
}

/**
 * Seconds to human readable
 * @param int $seconds
 * @return string
 */
function seconds2Human($seconds)
{
	$r = '';
	$_seconds = floor($seconds);
	$ms = $seconds - $_seconds;
	$seconds = $_seconds;
	if ($hours = floor($seconds / 3600))
	{
		$r .= $hours . (isCli() ? ' h ' : ' час ');
		$seconds = $seconds % 3600;
	}

	if ($minutes = floor($seconds / 60))
	{
		$r .= $minutes . (isCli() ? ' m ' : ' мин ');
		$seconds = $seconds % 60;
	}

	if ($minutes < 3) $r .= ' ' . $seconds + ($ms > 0 ? round($ms) : 0) . (isCli() ? ' s' : ' сек'); 

	return $r;
}

if (isCli())
{

	$cli_options = array(
                'c:' => 'avdb:',
		'm:' => 'memory:',
		's:' => 'size:',
		'a' => 'all',
		'd:' => 'delay:',
		'l:' => 'list:',
		'r:' => 'report:',
		'f' => 'fast',
		'j:' => 'file:',
		'p:' => 'path:',
		'q' => 'quite',
		'e:' => 'cms:',
		'x:' => 'mode:',
		'k:' => 'skip:',
		'i:' => 'idb:',
		'n' => 'sc',
		'o:' => 'json_report:',
		't:' => 'php_report:',
		'z:' => 'progress:',
		'g:' => 'handler:',
		'b' => 'smart',
		'u:' => 'username:',
		'h' => 'help',
	);

	$cli_longopts = array(
		'avdb:',
		'cmd:',
		'noprefix:',
		'addprefix:',
		'scan:',
		'one-pass',
		'smart',
		'quarantine',
		'with-2check',
		'skip-cache',
		'username:', 
		'imake',
		'icheck'
	);
	
	$cli_longopts = array_merge($cli_longopts, array_values($cli_options));

	$options = getopt(implode('', array_keys($cli_options)), $cli_longopts);

	if (isset($options['h']) OR isset($options['help']))
	{
		$memory_limit = ini_get('memory_limit');
		echo <<<HELP
Revisium AI-Bolit - Intelligent Malware File Scanner for Websites.

Usage: php {$_SERVER['PHP_SELF']} [OPTIONS] [PATH]
Current default path is: {$defaults['path']}

  -j, --file=FILE      		Full path to single file to check
  -l, --list=FILE      		Full path to create plain text file with a list of found malware
  -o, --json_report=FILE	Full path to create json-file with a list of found malware
  -p, --path=PATH      		Directory path to scan, by default the file directory is used
                       		Current path: {$defaults['path']}
  -m, --memory=SIZE    		Maximum amount of memory a script may consume. Current value: $memory_limit
                       		Can take shorthand byte values (1M, 1G...)
  -s, --size=SIZE      		Scan files are smaller than SIZE. 0 - All files. Current value: {$defaults['max_size_to_scan']}
  -a, --all            		Scan all files (by default scan. js,. php,. html,. htaccess)
  -d, --delay=INT      		Delay in milliseconds when scanning files to reduce load on the file system (Default: 1)
  -x, --mode=INT       		Set scan mode. 0 - for basic, 1 - for expert and 2 for paranoic.
  -k, --skip=jpg,...   		Skip specific extensions. E.g. --skip=jpg,gif,png,xls,pdf
      --scan=php,...   		Scan only specific extensions. E.g. --scan=php,htaccess,js
  -r, --report=PATH/EMAILS
  -z, --progress=FILE  		Runtime progress of scanning, saved to the file, full path required. 
  -u, --username=<username>  	Run scanner with specific user id and group id, e.g. --username=www-data
  -g, --hander=FILE    		External php handler for different events, full path to php file required.
      --cmd="command [args...]"
      --smart                   Enable smart mode (skip cache files and optimize scanning)
                       		Run command after scanning
      --one-pass       		Do not calculate remaining time
      --quarantine     		Archive all malware from report
      --with-2check    		Create or use AI-BOLIT-DOUBLECHECK.php file
      --imake
      --icheck
      --idb=file	   	Integrity Check database file

      --help           		Display this help and exit

* Mandatory arguments listed below are required for both full and short way of usage.

HELP;
		exit;
	}

	$l_FastCli = false;
	
	if (
		(isset($options['memory']) AND !empty($options['memory']) AND ($memory = $options['memory']))
		OR (isset($options['m']) AND !empty($options['m']) AND ($memory = $options['m']))
	)
	{
		$memory = getBytes($memory);
		if ($memory > 0)
		{
			$defaults['memory_limit'] = $memory;
			ini_set('memory_limit', $memory);
		}
	}


	$avdb = '';
	if (
		(isset($options['avdb']) AND !empty($options['avdb']) AND ($avdb = $options['avdb']))
		OR (isset($options['c']) AND !empty($options['c']) AND ($avdb = $options['c']))
	)
	{
		if (file_exists($avdb))
		{
			$defaults['avdb'] = $avdb;
		}
	}

	if (
		(isset($options['file']) AND !empty($options['file']) AND ($file = $options['file']) !== false)
		OR (isset($options['j']) AND !empty($options['j']) AND ($file = $options['j']) !== false)
	)
	{
		define('SCAN_FILE', $file);
	}


	if (
		(isset($options['list']) AND !empty($options['list']) AND ($file = $options['list']) !== false)
		OR (isset($options['l']) AND !empty($options['l']) AND ($file = $options['l']) !== false)
	)
	{

		define('PLAIN_FILE', $file);
	}

	if (
		(isset($options['json_report']) AND !empty($options['json_report']) AND ($file = $options['json_report']) !== false)
		OR (isset($options['o']) AND !empty($options['o']) AND ($file = $options['o']) !== false)
	)
	{
		define('JSON_FILE', $file);
	}

	if (
		(isset($options['php_report']) AND !empty($options['php_report']) AND ($file = $options['php_report']) !== false)
		OR (isset($options['t']) AND !empty($options['t']) AND ($file = $options['t']) !== false)
	)
	{
		define('PHP_FILE', $file);
	}

	if (isset($options['smart']) OR isset($options['b']))
	{
		define('SMART_SCAN', 1);
	}

	if (
		(isset($options['handler']) AND !empty($options['handler']) AND ($file = $options['handler']) !== false)
		OR (isset($options['g']) AND !empty($options['g']) AND ($file = $options['g']) !== false)
	)
	{
	        if (file_exists($file)) {
		   define('AIBOLIT_EXTERNAL_HANDLER', $file);
                }
	}

	if (
		(isset($options['progress']) AND !empty($options['progress']) AND ($file = $options['progress']) !== false)
		OR (isset($options['z']) AND !empty($options['z']) AND ($file = $options['z']) !== false)
	)
	{
		define('PROGRESS_LOG_FILE', $file);
	}

	if (
		(isset($options['size']) AND !empty($options['size']) AND ($size = $options['size']) !== false)
		OR (isset($options['s']) AND !empty($options['s']) AND ($size = $options['s']) !== false)
	)
	{
		$size = getBytes($size);
		$defaults['max_size_to_scan'] = $size > 0 ? $size : 0;
	}

	if (
		(isset($options['username']) AND !empty($options['username']) AND ($username = $options['username']) !== false)
		OR (isset($options['u']) AND !empty($options['u']) AND ($username = $options['u']) !== false)
	)
	{

                if (!empty($username) && ($info = posix_getpwnam($username)) !== false) {
                    posix_setgid($info['gid']);
                    posix_setuid($info['uid']);
                    $defaults['userid'] = $info['uid'];
                    $defaults['groupid'] = $info['gid'];
                } else {
                    echo('Invalid username');
                    exit(-1);
                }               
	}

 	if (
 		(isset($options['file']) AND !empty($options['file']) AND ($file = $options['file']) !== false)
 		OR (isset($options['j']) AND !empty($options['j']) AND ($file = $options['j']) !== false)
 		AND (isset($options['q'])) 
 	
 	)
 	{
 		$BOOL_RESULT = true;
 	}
 
	if (isset($options['f'])) 
	{
	   $l_FastCli = true;
	}
		
	if (isset($options['q']) || isset($options['quite'])) 
	{
 	    $BOOL_RESULT = true;
	}

        if (isset($options['x'])) {
            define('AI_EXPERT', $options['x']);
        } else if (isset($options['mode'])) {
            define('AI_EXPERT', $options['mode']);
        } else {
            define('AI_EXPERT', AI_EXPERT_MODE); 
        }

        if (AI_EXPERT < 2) {
           $g_SpecificExt = true;
           $defaults['scan_all_files'] = false;
        } else {
           $defaults['scan_all_files'] = true;
        }	

	define('BOOL_RESULT', $BOOL_RESULT);

	if (
		(isset($options['delay']) AND !empty($options['delay']) AND ($delay = $options['delay']) !== false)
		OR (isset($options['d']) AND !empty($options['d']) AND ($delay = $options['d']) !== false)
	)
	{
		$delay = (int) $delay;
		if (!($delay < 0))
		{
			$defaults['scan_delay'] = $delay;
		}
	}

	if (
		(isset($options['skip']) AND !empty($options['skip']) AND ($ext_list = $options['skip']) !== false)
		OR (isset($options['k']) AND !empty($options['k']) AND ($ext_list = $options['k']) !== false)
	)
	{
		$defaults['skip_ext'] = $ext_list;
	}

	if (isset($options['n']) OR isset($options['skip-cache']))
	{
		$defaults['skip_cache'] = true;
	}

	if (isset($options['scan']))
	{
		$ext_list = strtolower(trim($options['scan'], " ,\t\n\r\0\x0B"));
		if ($ext_list != '')
		{
			$l_FastCli = true;
			$g_SensitiveFiles = explode(",", $ext_list);
			for ($i = 0; $i < count($g_SensitiveFiles); $i++) {
			   if ($g_SensitiveFiles[$i] == '.') {
                              $g_SensitiveFiles[$i] = '';
                           }
                        }

			$g_SpecificExt = true;
		}
	}


    if (isset($options['all']) OR isset($options['a']))
    {
    	$defaults['scan_all_files'] = true;
        $g_SpecificExt = false;
    }

    if (isset($options['cms'])) {
        define('CMS', $options['cms']);
    } else if (isset($options['e'])) {
        define('CMS', $options['e']);
    }


    if (!defined('SMART_SCAN')) {
       define('SMART_SCAN', 1);
    }


	$l_SpecifiedPath = false;
	if (
		(isset($options['path']) AND !empty($options['path']) AND ($path = $options['path']) !== false)
		OR (isset($options['p']) AND !empty($options['p']) AND ($path = $options['p']) !== false)
	)
	{
		$defaults['path'] = $path;
		$l_SpecifiedPath = true;
	}

	if (
		isset($options['noprefix']) AND !empty($options['noprefix']) AND ($g_NoPrefix = $options['noprefix']) !== false)
		
	{
	} else {
		$g_NoPrefix = '';
	}

	if (
		isset($options['addprefix']) AND !empty($options['addprefix']) AND ($g_AddPrefix = $options['addprefix']) !== false)
		
	{
	} else {
		$g_AddPrefix = '';
	}



	$l_SuffixReport = str_replace('/var/www', '', $defaults['path']);
	$l_SuffixReport = str_replace('/home', '', $l_SuffixReport);
        $l_SuffixReport = preg_replace('#[/\\\.\s]#', '_', $l_SuffixReport);
	$l_SuffixReport .=  "-" . rand(1, 999999);
		
	if (
		(isset($options['report']) AND ($report = $options['report']) !== false)
		OR (isset($options['r']) AND ($report = $options['r']) !== false)
	)
	{
		$report = str_replace('@PATH@', $l_SuffixReport, $report);
		$report = str_replace('@RND@', rand(1, 999999), $report);
		$report = str_replace('@DATE@', date('d-m-Y-h-i'), $report);
		define('REPORT', $report);
		define('NEED_REPORT', true);
	}

	if (
		(isset($options['idb']) AND ($ireport = $options['idb']) !== false)
	)
	{
		$ireport = str_replace('@PATH@', $l_SuffixReport, $ireport);
		$ireport = str_replace('@RND@', rand(1, 999999), $ireport);
		$ireport = str_replace('@DATE@', date('d-m-Y-h-i'), $ireport);
		define('INTEGRITY_DB_FILE', $ireport);
	}

  
	defined('REPORT') OR define('REPORT', 'AI-BOLIT-REPORT-' . $l_SuffixReport . '-' . date('d-m-Y_H-i') . '.html');
	
	defined('INTEGRITY_DB_FILE') OR define('INTEGRITY_DB_FILE', 'AINTEGRITY-' . $l_SuffixReport . '-' . date('d-m-Y_H-i'));

	$last_arg = max(1, sizeof($_SERVER['argv']) - 1);
	if (isset($_SERVER['argv'][$last_arg]))
	{
		$path = $_SERVER['argv'][$last_arg];
		if (
			substr($path, 0, 1) != '-'
			AND (substr($_SERVER['argv'][$last_arg - 1], 0, 1) != '-' OR array_key_exists(substr($_SERVER['argv'][$last_arg - 1], -1), $cli_options)))
		{
			$defaults['path'] = $path;
		}
	}	
	
	
	define('ONE_PASS', isset($options['one-pass']));

	define('IMAKE', isset($options['imake']));
	define('ICHECK', isset($options['icheck']));

	if (IMAKE && ICHECK) die('One of the following options must be used --imake or --icheck.');

} else {
   define('AI_EXPERT', AI_EXPERT_MODE); 
   define('ONE_PASS', true);
}


if (isset($defaults['avdb']) && file_exists($defaults['avdb'])) {
   $avdb = explode("\n", gzinflate(base64_decode(str_rot13(strrev(trim(file_get_contents($defaults['avdb'])))))));

   $g_DBShe = explode("\n", base64_decode($avdb[0]));
   $gX_DBShe = explode("\n", base64_decode($avdb[1]));
   $g_FlexDBShe = explode("\n", base64_decode($avdb[2]));
   $gX_FlexDBShe = explode("\n", base64_decode($avdb[3]));
   $gXX_FlexDBShe = explode("\n", base64_decode($avdb[4]));
   $g_ExceptFlex = explode("\n", base64_decode($avdb[5]));
   $g_AdwareSig = explode("\n", base64_decode($avdb[6]));
   $g_PhishingSig = explode("\n", base64_decode($avdb[7]));
   $g_JSVirSig = explode("\n", base64_decode($avdb[8]));
   $gX_JSVirSig = explode("\n", base64_decode($avdb[9]));
   $g_SusDB = explode("\n", base64_decode($avdb[10]));
   $g_SusDBPrio = explode("\n", base64_decode($avdb[11]));
   $g_DeMapper = array_combine(explode("\n", base64_decode($avdb[12])), explode("\n", base64_decode($avdb[13])));

   if (count($g_DBShe) <= 1) {
      $g_DBShe = array();
   }

   if (count($gX_DBShe) <= 1) {
      $gX_DBShe = array();
   }

   if (count($g_FlexDBShe) <= 1) {
      $g_FlexDBShe = array();
   }

   if (count($gX_FlexDBShe) <= 1) {
      $gX_FlexDBShe = array();
   }

   if (count($gXX_FlexDBShe) <= 1) {
      $gXX_FlexDBShe = array();
   }

   if (count($g_ExceptFlex) <= 1) {
      $g_ExceptFlex = array();
   }

   if (count($g_AdwareSig) <= 1) {
      $g_AdwareSig = array();
   }

   if (count($g_PhishingSig) <= 1) {
      $g_PhishingSig = array();
   }

   if (count($gX_JSVirSig) <= 1) {
      $gX_JSVirSig = array();
   }

   if (count($g_JSVirSig) <= 1) {
      $g_JSVirSig = array();
   }

   if (count($g_SusDB) <= 1) {
      $g_SusDB = array();
   }

   if (count($g_SusDBPrio) <= 1) {
      $g_SusDBPrio = array();
   }

   stdOut('Loaded external signatures from ' . $defaults['avdb']);
}

// use only basic signature subset
if (AI_EXPERT < 2) {
   $gX_FlexDBShe = array();
   $gXX_FlexDBShe = array();
   $gX_JSVirSig = array();
}

if (isset($defaults['userid'])) {
   stdOut('Running from ' . $defaults['userid'] . ':' . $defaults['groupid']);
}

stdOut('Malware signatures: ' . (count($g_JSVirSig) + count($gX_JSVirSig) + count($g_DBShe) + count($gX_DBShe) + count($gX_DBShe) + count($g_FlexDBShe) + count($gX_FlexDBShe) + count($gXX_FlexDBShe)));

if ($g_SpecificExt) {
  stdOut("Scan specific extensions: " . implode(',', $g_SensitiveFiles));
}

if (!DEBUG_PERFORMANCE) {
   OptimizeSignatures();
} else {
   stdOut("Debug Performance Scan");
}

$g_DBShe  = array_map('strtolower', $g_DBShe);
$gX_DBShe = array_map('strtolower', $gX_DBShe);

if (!defined('PLAIN_FILE')) { define('PLAIN_FILE', ''); }

// Init
define('MAX_ALLOWED_PHP_HTML_IN_DIR', 600);
define('BASE64_LENGTH', 69);
define('MAX_PREVIEW_LEN', 80);
define('MAX_EXT_LINKS', 1001);

if (defined('AIBOLIT_EXTERNAL_HANDLER')) {
   include_once(AIBOLIT_EXTERNAL_HANDLER);
   stdOut("\nLoaded external handler: " . AIBOLIT_EXTERNAL_HANDLER . "\n");
   if (function_exists("aibolit_onStart")) { aibolit_onStart(); }
}

// Perform full scan when running from command line
if (isset($_GET['full'])) {
  $defaults['scan_all_files'] = 1;
}

if ($l_FastCli) {
  $defaults['scan_all_files'] = 0; 
}

if (!isCli()) {
  	define('ICHECK', isset($_GET['icheck']));
  	define('IMAKE', isset($_GET['imake']));
	
	define('INTEGRITY_DB_FILE', 'ai-integrity-db');
}

define('SCAN_ALL_FILES', (bool) $defaults['scan_all_files']);
define('SCAN_DELAY', (int) $defaults['scan_delay']);
define('MAX_SIZE_TO_SCAN', getBytes($defaults['max_size_to_scan']));

if ($defaults['memory_limit'] AND ($defaults['memory_limit'] = getBytes($defaults['memory_limit'])) > 0) {
	ini_set('memory_limit', $defaults['memory_limit']);
    stdOut("Changed memory limit to " . $defaults['memory_limit']);
}

define('ROOT_PATH', realpath($defaults['path']));

if (!ROOT_PATH)
{
    if (isCli())  {
		die(stdOut("Directory '{$defaults['path']}' not found!"));
	}
}
elseif(!is_readable(ROOT_PATH))
{
        if (isCli())  {
		die2(stdOut("Cannot read directory '" . ROOT_PATH . "'!"));
	}
}

define('CURRENT_DIR', getcwd());
chdir(ROOT_PATH);

if (isCli() AND REPORT !== '' AND !getEmails(REPORT))
{
	$report = str_replace('\\', '/', REPORT);
	$abs = strpos($report, '/') === 0 ? DIR_SEPARATOR : '';
	$report = array_values(array_filter(explode('/', $report)));
	$report_file = array_pop($report);
	$report_path = realpath($abs . implode(DIR_SEPARATOR, $report));

	define('REPORT_FILE', $report_file);
	define('REPORT_PATH', $report_path);

	if (REPORT_FILE AND REPORT_PATH AND is_file(REPORT_PATH . DIR_SEPARATOR . REPORT_FILE))
	{
		@unlink(REPORT_PATH . DIR_SEPARATOR . REPORT_FILE);
	}
}

if (defined('REPORT_PATH')) {
   $l_ReportDirName = REPORT_PATH;
}

define('QUEUE_FILENAME', ($l_ReportDirName != '' ? $l_ReportDirName . '/' : '') . 'AI-BOLIT-QUEUE-' . md5($defaults['path']) . '-' . rand(1000,9999) . '.txt');

if (function_exists('phpinfo')) {
   ob_start();
   phpinfo();
   $l_PhpInfo = ob_get_contents();
   ob_end_clean();

   $l_PhpInfo = str_replace('border: 1px', '', $l_PhpInfo);
   preg_match('|<body>(.*)</body>|smi', $l_PhpInfo, $l_PhpInfoBody);
}

////////////////////////////////////////////////////////////////////////////
$l_Template = str_replace("@@MODE@@", AI_EXPERT . '/' . SMART_SCAN, $l_Template);

if (AI_EXPERT == 0) {
   $l_Result .= '<div class="rep">' . AI_STR_057 . '</div>'; 
} else {
}

$l_Template = str_replace('@@HEAD_TITLE@@', AI_STR_051 . $g_AddPrefix . str_replace($g_NoPrefix, '', ROOT_PATH), $l_Template);

define('QCR_INDEX_FILENAME', 'fn');
define('QCR_INDEX_TYPE', 'type');
define('QCR_INDEX_WRITABLE', 'wr');
define('QCR_SVALUE_FILE', '1');
define('QCR_SVALUE_FOLDER', '0');

/**
 * Extract emails from the string
 * @param string $email
 * @return array of strings with emails or false on error
 */
function getEmails($email)
{
	$email = preg_split('#[,\s;]#', $email, -1, PREG_SPLIT_NO_EMPTY);
	$r = array();
	for ($i = 0, $size = sizeof($email); $i < $size; $i++)
	{
	        if (function_exists('filter_var')) {
   		   if (filter_var($email[$i], FILTER_VALIDATE_EMAIL))
   		   {
   		   	$r[] = $email[$i];
    		   }
                } else {
                   // for PHP4
                   if (strpos($email[$i], '@') !== false) {
   		   	$r[] = $email[$i];
                   }
                }
	}
	return empty($r) ? false : $r;
}

/**
 * Get bytes from shorthand byte values (1M, 1G...)
 * @param int|string $val
 * @return int
 */
function getBytes($val)
{
	$val = trim($val);
	$last = strtolower($val{strlen($val) - 1});
	switch($last) {
		case 't':
			$val *= 1024;
		case 'g':
			$val *= 1024;
		case 'm':
			$val *= 1024;
		case 'k':
			$val *= 1024;
	}
	return intval($val);
}

/**
 * Format bytes to human readable
 * @param int $bites
 * @return string
 */
function bytes2Human($bites)
{
	if ($bites < 1024)
	{
		return $bites . ' b';
	}
	elseif (($kb = $bites / 1024) < 1024)
	{
		return number_format($kb, 2) . ' Kb';
	}
	elseif (($mb = $kb / 1024) < 1024)
	{
		return number_format($mb, 2) . ' Mb';
	}
	elseif (($gb = $mb / 1024) < 1024)
	{
		return number_format($gb, 2) . ' Gb';
	}
	else
	{
		return number_format($gb / 1024, 2) . 'Tb';
	}
}

///////////////////////////////////////////////////////////////////////////
function needIgnore($par_FN, $par_CRC) {
  global $g_IgnoreList;
  
  for ($i = 0; $i < count($g_IgnoreList); $i++) {
     if (strpos($par_FN, $g_IgnoreList[$i][0]) !== false) {
		if ($par_CRC == $g_IgnoreList[$i][1]) {
			return true;
		}
	 }
  }
  
  return false;
}

///////////////////////////////////////////////////////////////////////////
function makeSafeFn($par_Str, $replace_path = false) {
  global $g_AddPrefix, $g_NoPrefix;
  if ($replace_path) {
     $lines = explode("\n", $par_Str);
     array_walk($lines, function(&$n) {
          global $g_AddPrefix, $g_NoPrefix;
          $n = $g_AddPrefix . str_replace($g_NoPrefix, '', $n); 
     }); 

     $par_Str = implode("\n", $lines);
  }
 
  return htmlspecialchars($par_Str, ENT_SUBSTITUTE | ENT_QUOTES);
}

function replacePathArray($par_Arr) {
  global $g_AddPrefix, $g_NoPrefix;
     array_walk($par_Arr, function(&$n) {
          global $g_AddPrefix, $g_NoPrefix;
          $n = $g_AddPrefix . str_replace($g_NoPrefix, '', $n); 
     }); 

  return $par_Arr;
}

///////////////////////////////////////////////////////////////////////////
function getRawJsonVuln($par_List) {
  global $g_Structure, $g_NoPrefix, $g_AddPrefix;
   $results = array();
   $l_Src = array('&quot;', '&lt;', '&gt;', '&amp;', '&#039;', '<' . '?php.');
   $l_Dst = array('"',      '<',    '>',    '&', '\'',         '<' . '?php ');

   for ($i = 0; $i < count($par_List); $i++) {
      $l_Pos = $par_List[$i]['ndx'];
      $res['fn'] = $g_AddPrefix . str_replace($g_NoPrefix, '', $g_Structure['n'][$l_Pos]);
      $res['sig'] = $par_List[$i]['id'];

      $res['ct'] = $g_Structure['c'][$l_Pos];
      $res['mt'] = $g_Structure['m'][$l_Pos];
      $res['sz'] = $g_Structure['s'][$l_Pos];
      $res['sigid'] = 'vuln_' . md5($g_Structure['n'][$l_Pos] . $par_List[$i]['id']);

      $results[] = $res; 
   }

   return $results;
}

///////////////////////////////////////////////////////////////////////////
function getRawJson($par_List, $par_Details = null, $par_SigId = null) {
  global $g_Structure, $g_NoPrefix, $g_AddPrefix;
   $results = array();
   $l_Src = array('&quot;', '&lt;', '&gt;', '&amp;', '&#039;', '<' . '?php.');
   $l_Dst = array('"',      '<',    '>',    '&', '\'',         '<' . '?php ');

   for ($i = 0; $i < count($par_List); $i++) {
       if ($par_SigId != null) {
          $l_SigId = 'id_' . $par_SigId[$i];
       } else {
          $l_SigId = 'id_n' . rand(1000000, 9000000);
       }
       


      $l_Pos = $par_List[$i];
      $res['fn'] = $g_AddPrefix . str_replace($g_NoPrefix, '', $g_Structure['n'][$l_Pos]);
      if ($par_Details != null) {
         $res['sig'] = preg_replace('|(L\d+).+__AI_MARKER__|smi', '[$1]: ...', $par_Details[$i]);
         $res['sig'] = preg_replace('/[^\x20-\x7F]/', '.', $res['sig']);
         $res['sig'] = preg_replace('/__AI_LINE1__(\d+)__AI_LINE2__/', '[$1] ', $res['sig']);
         $res['sig'] = preg_replace('/__AI_MARKER__/', ' @!!!>', $res['sig']);
         $res['sig'] = str_replace($l_Src, $l_Dst, $res['sig']);
      }

      $res['ct'] = $g_Structure['c'][$l_Pos];
      $res['mt'] = $g_Structure['m'][$l_Pos];
      $res['sz'] = $g_Structure['s'][$l_Pos];
      $res['sigid'] = $l_SigId;

      $results[] = $res; 
   }

   return $results;
}

///////////////////////////////////////////////////////////////////////////
function printList($par_List, $par_Details = null, $par_NeedIgnore = false, $par_SigId = null, $par_TableName = null) {
  global $g_Structure, $g_NoPrefix, $g_AddPrefix;
  
  $i = 0;

  if ($par_TableName == null) {
     $par_TableName = 'table_' . rand(1000000,9000000);
  }

  $l_Result = '';
  $l_Result .= "<div class=\"flist\"><table cellspacing=1 cellpadding=4 border=0 id=\"" . $par_TableName . "\">";

  $l_Result .= "<thead><tr class=\"tbgh" . ( $i % 2 ). "\">";
  $l_Result .= "<th width=70%>" . AI_STR_004 . "</th>";
  $l_Result .= "<th>" . AI_STR_005 . "</th>";
  $l_Result .= "<th>" . AI_STR_006 . "</th>";
  $l_Result .= "<th width=90>" . AI_STR_007 . "</th>";
  $l_Result .= "<th width=0 class=\"hidd\">CRC32</th>";
  $l_Result .= "<th width=0 class=\"hidd\"></th>";
  $l_Result .= "<th width=0 class=\"hidd\"></th>";
  $l_Result .= "<th width=0 class=\"hidd\"></th>";
  
  $l_Result .= "</tr></thead><tbody>";

  for ($i = 0; $i < count($par_List); $i++) {
    if ($par_SigId != null) {
       $l_SigId = 'id_' . $par_SigId[$i];
    } else {
       $l_SigId = 'id_z' . rand(1000000,9000000);
    }
    
    $l_Pos = $par_List[$i];
        if ($par_NeedIgnore) {
         	if (needIgnore($g_Structure['n'][$par_List[$i]], $g_Structure['crc'][$l_Pos])) {
         		continue;
         	}
        }
  
     $l_Creat = $g_Structure['c'][$l_Pos] > 0 ? date("d/m/Y H:i:s", $g_Structure['c'][$l_Pos]) : '-';
     $l_Modif = $g_Structure['m'][$l_Pos] > 0 ? date("d/m/Y H:i:s", $g_Structure['m'][$l_Pos]) : '-';
     $l_Size = $g_Structure['s'][$l_Pos] > 0 ? bytes2Human($g_Structure['s'][$l_Pos]) : '-';

     if ($par_Details != null) {
        $l_WithMarker = preg_replace('|__AI_MARKER__|smi', '<span class="marker">&nbsp;</span>', $par_Details[$i]);
        $l_WithMarker = preg_replace('|__AI_LINE1__|smi', '<span class="line_no">', $l_WithMarker);
        $l_WithMarker = preg_replace('|__AI_LINE2__|smi', '</span>', $l_WithMarker);
		
        $l_Body = '<div class="details">';

        if ($par_SigId != null) {
           $l_Body .= '<a href="#" onclick="return hsig(\'' . $l_SigId . '\')">[x]</a> ';
        }

        $l_Body .= $l_WithMarker . '</div>';
     } else {
        $l_Body = '';
     }

     $l_Result .= '<tr class="tbg' . ( $i % 2 ). '" o="' . $l_SigId .'">';
	 
	 if (is_file($g_Structure['n'][$l_Pos])) {
//		$l_Result .= '<td><div class="it"><a class="it" target="_blank" href="'. $defaults['site_url'] . 'ai-bolit.php?fn=' .
//	              $g_Structure['n'][$l_Pos] . '&ph=' . realCRC(PASS) . '&c=' . $g_Structure['crc'][$l_Pos] . '">' . $g_Structure['n'][$l_Pos] . '</a></div>' . $l_Body . '</td>';
		$l_Result .= '<td><div class="it"><a class="it">' . makeSafeFn($g_AddPrefix . str_replace($g_NoPrefix, '', $g_Structure['n'][$l_Pos])) . '</a></div>' . $l_Body . '</td>';
	 } else {
		$l_Result .= '<td><div class="it"><a class="it">' . makeSafeFn($g_AddPrefix . str_replace($g_NoPrefix, '', $g_Structure['n'][$par_List[$i]])) . '</a></div></td>';
	 }
	 
     $l_Result .= '<td align=center><div class="ctd">' . $l_Creat . '</div></td>';
     $l_Result .= '<td align=center><div class="ctd">' . $l_Modif . '</div></td>';
     $l_Result .= '<td align=center><div class="ctd">' . $l_Size . '</div></td>';
     $l_Result .= '<td class="hidd"><div class="hidd">' . $g_Structure['crc'][$l_Pos] . '</div></td>';
     $l_Result .= '<td class="hidd"><div class="hidd">' . 'x' . '</div></td>';
     $l_Result .= '<td class="hidd"><div class="hidd">' . $g_Structure['m'][$l_Pos] . '</div></td>';
     $l_Result .= '<td class="hidd"><div class="hidd">' . $l_SigId . '</div></td>';
     $l_Result .= '</tr>';

  }

  $l_Result .= "</tbody></table></div><div class=clear style=\"margin: 20px 0 0 0\"></div>";

  return $l_Result;
}

///////////////////////////////////////////////////////////////////////////
function printPlainList($par_List, $par_Details = null, $par_NeedIgnore = false, $par_SigId = null, $par_TableName = null) {
  global $g_Structure, $g_NoPrefix, $g_AddPrefix;
  
  $l_Result = "";

  $l_Src = array('&quot;', '&lt;', '&gt;', '&amp;', '&#039;');
  $l_Dst = array('"',      '<',    '>',    '&', '\'');

  for ($i = 0; $i < count($par_List); $i++) {
    $l_Pos = $par_List[$i];
        if ($par_NeedIgnore) {
         	if (needIgnore($g_Structure['n'][$par_List[$i]], $g_Structure['crc'][$l_Pos])) {
         		continue;
         	}                      
        }
  

     if ($par_Details != null) {

        $l_Body = preg_replace('|(L\d+).+__AI_MARKER__|smi', '$1: ...', $par_Details[$i]);
        $l_Body = preg_replace('/[^\x20-\x7F]/', '.', $l_Body);
        $l_Body = str_replace($l_Src, $l_Dst, $l_Body);

     } else {
        $l_Body = '';
     }

	 if (is_file($g_Structure['n'][$l_Pos])) {		 
		$l_Result .= $g_AddPrefix . str_replace($g_NoPrefix, '', $g_Structure['n'][$l_Pos]) . "\t\t\t" . $l_Body . "\n";
	 } else {
		$l_Result .= $g_AddPrefix . str_replace($g_NoPrefix, '', $g_Structure['n'][$par_List[$i]]) . "\n";
	 }
	 
  }

  return $l_Result;
}

///////////////////////////////////////////////////////////////////////////
function extractValue(&$par_Str, $par_Name) {
  if (preg_match('|<tr><td class="e">\s*'.$par_Name.'\s*</td><td class="v">(.+?)</td>|sm', $par_Str, $l_Result)) {
     return str_replace('no value', '', strip_tags($l_Result[1]));
  }
}

///////////////////////////////////////////////////////////////////////////
function QCR_ExtractInfo($par_Str) {
   $l_PhpInfoSystem = extractValue($par_Str, 'System');
   $l_PhpPHPAPI = extractValue($par_Str, 'Server API');
   $l_AllowUrlFOpen = extractValue($par_Str, 'allow_url_fopen');
   $l_AllowUrlInclude = extractValue($par_Str, 'allow_url_include');
   $l_DisabledFunction = extractValue($par_Str, 'disable_functions');
   $l_DisplayErrors = extractValue($par_Str, 'display_errors');
   $l_ErrorReporting = extractValue($par_Str, 'error_reporting');
   $l_ExposePHP = extractValue($par_Str, 'expose_php');
   $l_LogErrors = extractValue($par_Str, 'log_errors');
   $l_MQGPC = extractValue($par_Str, 'magic_quotes_gpc');
   $l_MQRT = extractValue($par_Str, 'magic_quotes_runtime');
   $l_OpenBaseDir = extractValue($par_Str, 'open_basedir');
   $l_RegisterGlobals = extractValue($par_Str, 'register_globals');
   $l_SafeMode = extractValue($par_Str, 'safe_mode');


   $l_DisabledFunction = ($l_DisabledFunction == '' ? '-?-' : $l_DisabledFunction);
   $l_OpenBaseDir = ($l_OpenBaseDir == '' ? '-?-' : $l_OpenBaseDir);

   $l_Result = '<div class="title">' . AI_STR_008 . ': ' . phpversion() . '</div>';
   $l_Result .= 'System Version: <span class="php_ok">' . $l_PhpInfoSystem . '</span><br/>';
   $l_Result .= 'PHP API: <span class="php_ok">' . $l_PhpPHPAPI. '</span><br/>';
   $l_Result .= 'allow_url_fopen: <span class="php_' . ($l_AllowUrlFOpen == 'On' ? 'bad' : 'ok') . '">' . $l_AllowUrlFOpen. '</span><br/>';
   $l_Result .= 'allow_url_include: <span class="php_' . ($l_AllowUrlInclude == 'On' ? 'bad' : 'ok') . '">' . $l_AllowUrlInclude. '</span><br/>';
   $l_Result .= 'disable_functions: <span class="php_' . ($l_DisabledFunction == '-?-' ? 'bad' : 'ok') . '">' . $l_DisabledFunction. '</span><br/>';
   $l_Result .= 'display_errors: <span class="php_' . ($l_DisplayErrors == 'On' ? 'ok' : 'bad') . '">' . $l_DisplayErrors. '</span><br/>';
   $l_Result .= 'error_reporting: <span class="php_ok">' . $l_ErrorReporting. '</span><br/>';
   $l_Result .= 'expose_php: <span class="php_' . ($l_ExposePHP == 'On' ? 'bad' : 'ok') . '">' . $l_ExposePHP. '</span><br/>';
   $l_Result .= 'log_errors: <span class="php_' . ($l_LogErrors == 'On' ? 'ok' : 'bad') . '">' . $l_LogErrors . '</span><br/>';
   $l_Result .= 'magic_quotes_gpc: <span class="php_' . ($l_MQGPC == 'On' ? 'ok' : 'bad') . '">' . $l_MQGPC. '</span><br/>';
   $l_Result .= 'magic_quotes_runtime: <span class="php_' . ($l_MQRT == 'On' ? 'bad' : 'ok') . '">' . $l_MQRT. '</span><br/>';
   $l_Result .= 'register_globals: <span class="php_' . ($l_RegisterGlobals == 'On' ? 'bad' : 'ok') . '">' . $l_RegisterGlobals . '</span><br/>';
   $l_Result .= 'open_basedir: <span class="php_' . ($l_OpenBaseDir == '-?-' ? 'bad' : 'ok') . '">' . $l_OpenBaseDir . '</span><br/>';
   
   if (phpversion() < '5.3.0') {
      $l_Result .= 'safe_mode (PHP < 5.3.0): <span class="php_' . ($l_SafeMode == 'On' ? 'ok' : 'bad') . '">' . $l_SafeMode. '</span><br/>';
   }

   return $l_Result . '<p>';
}

///////////////////////////////////////////////////////////////////////////
   function addSlash($dir) {
      return rtrim($dir, DIRECTORY_SEPARATOR) . DIRECTORY_SEPARATOR;
   }

///////////////////////////////////////////////////////////////////////////
function QCR_Debug($par_Str = "") {
  if (!DEBUG_MODE) {
     return;
  }

  $l_MemInfo = ' ';  
  if (function_exists('memory_get_usage')) {
     $l_MemInfo .= ' curmem=' .  bytes2Human(memory_get_usage());
  }

  if (function_exists('memory_get_peak_usage')) {
     $l_MemInfo .= ' maxmem=' .  bytes2Human(memory_get_peak_usage());
  }

  stdOut("\n" . date('H:i:s') . ': ' . $par_Str . $l_MemInfo . "\n");
}


///////////////////////////////////////////////////////////////////////////
function QCR_ScanDirectories($l_RootDir)
{
	global $g_Structure, $g_Counter, $g_Doorway, $g_FoundTotalFiles, $g_FoundTotalDirs, 
			$defaults, $g_SkippedFolders, $g_UrlIgnoreList, $g_DirIgnoreList, $g_UnsafeDirArray, 
                        $g_UnsafeFilesFound, $g_SymLinks, $g_HiddenFiles, $g_UnixExec, $g_IgnoredExt, $g_SensitiveFiles, 
						$g_SuspiciousFiles, $g_ShortListExt, $l_SkipSample;

	static $l_Buffer = '';

	$l_DirCounter = 0;
	$l_DoorwayFilesCounter = 0;
	$l_SourceDirIndex = $g_Counter - 1;

        $l_SkipSample = array();

	QCR_Debug('Scan ' . $l_RootDir);

        $l_QuotedSeparator = quotemeta(DIR_SEPARATOR); 
 	if ($l_DIRH = @opendir($l_RootDir))
	{
		while (($l_FileName = readdir($l_DIRH)) !== false)
		{
			if ($l_FileName == '.' || $l_FileName == '..') continue;

			$l_FileName = $l_RootDir . DIR_SEPARATOR . $l_FileName;

			$l_Type = filetype($l_FileName);
            if ($l_Type == "link") 
            {
                $g_SymLinks[] = $l_FileName;
                continue;
            } else			
			if ($l_Type != "file" && $l_Type != "dir" ) {
			        if (!in_array($l_FileName, $g_UnixExec)) {
				   $g_UnixExec[] = $l_FileName;
				}

				continue;
			}	
						
			$l_Ext = strtolower(pathinfo($l_FileName, PATHINFO_EXTENSION));
			$l_IsDir = is_dir($l_FileName);

			if (in_array($l_Ext, $g_SuspiciousFiles)) 
			{
			        if (!in_array($l_FileName, $g_UnixExec)) {
                		   $g_UnixExec[] = $l_FileName;
                                } 
            		}

			// which files should be scanned
			$l_NeedToScan = SCAN_ALL_FILES || (in_array($l_Ext, $g_SensitiveFiles));

			if (in_array(strtolower($l_Ext), $g_IgnoredExt)) {    
		           $l_NeedToScan = false;
                        }

      			// if folder in ignore list
      			$l_Skip = false;
      			for ($dr = 0; $dr < count($g_DirIgnoreList); $dr++) {
      				if (($g_DirIgnoreList[$dr] != '') &&
      				   preg_match('#' . $g_DirIgnoreList[$dr] . '#', $l_FileName, $l_Found)) {
      				   if (!in_array($g_DirIgnoreList[$dr], $l_SkipSample)) {
                                      $l_SkipSample[] = $g_DirIgnoreList[$dr];
                                   } else {
        		             $l_Skip = true;
                                     $l_NeedToScan = false;
                                   }
      				}
      			}


			if ($l_IsDir)
			{
				// skip on ignore
				if ($l_Skip) {
				   $g_SkippedFolders[] = $l_FileName;
				   continue;
				}
				
				$l_BaseName = basename($l_FileName);

				if ((strpos($l_BaseName, '.') === 0) && ($l_BaseName != '.htaccess')) {
	               $g_HiddenFiles[] = $l_FileName;
	            }

//				$g_Structure['d'][$g_Counter] = $l_IsDir;
//				$g_Structure['n'][$g_Counter] = $l_FileName;
				if (ONE_PASS) {
					$g_Structure['n'][$g_Counter] = $l_FileName . DIR_SEPARATOR;
				} else {
					$l_Buffer .= $l_FileName . DIR_SEPARATOR . "\n";
				}

				$l_DirCounter++;

				if ($l_DirCounter > MAX_ALLOWED_PHP_HTML_IN_DIR)
				{
					$g_Doorway[] = $l_SourceDirIndex;
					$l_DirCounter = -655360;
				}

				$g_Counter++;
				$g_FoundTotalDirs++;

				QCR_ScanDirectories($l_FileName);
			} else
			{
				if ($l_NeedToScan)
				{
					$g_FoundTotalFiles++;
					if (in_array($l_Ext, $g_ShortListExt)) 
					{
						$l_DoorwayFilesCounter++;
						
						if ($l_DoorwayFilesCounter > MAX_ALLOWED_PHP_HTML_IN_DIR)
						{
							$g_Doorway[] = $l_SourceDirIndex;
							$l_DoorwayFilesCounter = -655360;
						}
					}

					if (ONE_PASS) {
						QCR_ScanFile($l_FileName, $g_Counter++);
					} else {
						$l_Buffer .= $l_FileName."\n";
					}

					$g_Counter++;
				}
			}

			if (strlen($l_Buffer) > 32000)
			{ 
				file_put_contents(QUEUE_FILENAME, $l_Buffer, FILE_APPEND) or die2("Cannot write to file ".QUEUE_FILENAME);
				$l_Buffer = '';
			}

		}

		closedir($l_DIRH);
	}
	
	if (($l_RootDir == ROOT_PATH) && !empty($l_Buffer)) {
		file_put_contents(QUEUE_FILENAME, $l_Buffer, FILE_APPEND) or die2("Cannot write to file " . QUEUE_FILENAME);
		$l_Buffer = '';                                                                            
	}

}


///////////////////////////////////////////////////////////////////////////
function getFragment($par_Content, $par_Pos) {
  $l_MaxChars = MAX_PREVIEW_LEN;
  $l_MaxLen = strlen($par_Content);
  $l_RightPos = min($par_Pos + $l_MaxChars, $l_MaxLen); 
  $l_MinPos = max(0, $par_Pos - $l_MaxChars);

  $l_FoundStart = substr($par_Content, 0, $par_Pos);
  $l_FoundStart = str_replace("\r", '', $l_FoundStart);
  $l_LineNo = strlen($l_FoundStart) - strlen(str_replace("\n", '', $l_FoundStart)) + 1;

  $par_Content = preg_replace('/[\x00-\x1F\x80-\xFF]/', '~', $par_Content);

  $l_Res = '__AI_LINE1__' . $l_LineNo . "__AI_LINE2__  " . ($l_MinPos > 0 ? '…' : '') . substr($par_Content, $l_MinPos, $par_Pos - $l_MinPos) . 
           '__AI_MARKER__' . substr($par_Content, $par_Pos, $l_RightPos - $par_Pos - 1);

  $l_Res = makeSafeFn(UnwrapObfu($l_Res));
  $l_Res = str_replace('~', '·', $l_Res);
  $l_Res = preg_replace('/\s+/smi', ' ', $l_Res);
  $l_Res = str_replace('' . '?php', '' . '?php ', $l_Res);

  return $l_Res;
}

///////////////////////////////////////////////////////////////////////////
function escapedHexToHex($escaped)
{ $GLOBALS['g_EncObfu']++; return chr(hexdec($escaped[1])); }
function escapedOctDec($escaped)
{ $GLOBALS['g_EncObfu']++; return chr(octdec($escaped[1])); }
function escapedDec($escaped)
{ $GLOBALS['g_EncObfu']++; return chr($escaped[1]); }

///////////////////////////////////////////////////////////////////////////
if (!defined('T_ML_COMMENT')) {
   define('T_ML_COMMENT', T_COMMENT);
} else {
   define('T_DOC_COMMENT', T_ML_COMMENT);
}
          	
function UnwrapObfu($par_Content) {
  $GLOBALS['g_EncObfu'] = 0;
  
  $search  = array( ' ;', ' =', ' ,', ' .', ' (', ' )', ' {', ' }', '; ', '= ', ', ', '. ', '( ', '( ', '{ ', '} ', ' !', ' >', ' <', ' _', '_ ', '< ',  '> ', ' $', ' %',   '% ', '# ', ' #', '^ ', ' ^', ' &', '& ', ' ?', '? ');
  $replace = array(  ';',  '=',  ',',  '.',  '(',  ')',  '{',  '}', ';',  '=',  ',',  '.',  '(',   ')', '{',  '}',   '!',  '>',  '<',  '_', '_',  '<',   '>',   '$',  '%',   '%',  '#',   '#', '^',   '^',  '&', '&',   '?', '?');
  $par_Content = str_replace('@', '', $par_Content);
  $par_Content = preg_replace('~\s+~smi', ' ', $par_Content);
  $par_Content = str_replace($search, $replace, $par_Content);
  $par_Content = preg_replace_callback('~\bchr\(\s*([0-9a-fA-FxX]+)\s*\)~', function ($m) { return "'".chr(intval($m[1], 0))."'"; }, $par_Content );

  $par_Content = preg_replace_callback('/\\\\x([a-fA-F0-9]{1,2})/i','escapedHexToHex', $par_Content);
  $par_Content = preg_replace_callback('/\\\\([0-9]{1,3})/i','escapedOctDec', $par_Content);

  $par_Content = preg_replace('/[\'"]\s*?\.+\s*?[\'"]/smi', '', $par_Content);
  $par_Content = preg_replace('/[\'"]\s*?\++\s*?[\'"]/smi', '', $par_Content);

  $content = str_replace('<?$', '<?php$', $content);
  $content = str_replace('<?php', '<?php ', $content);

  return $par_Content;
}

///////////////////////////////////////////////////////////////////////////
// Unicode BOM is U+FEFF, but after encoded, it will look like this.
define ('UTF32_BIG_ENDIAN_BOM'   , chr(0x00) . chr(0x00) . chr(0xFE) . chr(0xFF));
define ('UTF32_LITTLE_ENDIAN_BOM', chr(0xFF) . chr(0xFE) . chr(0x00) . chr(0x00));
define ('UTF16_BIG_ENDIAN_BOM'   , chr(0xFE) . chr(0xFF));
define ('UTF16_LITTLE_ENDIAN_BOM', chr(0xFF) . chr(0xFE));
define ('UTF8_BOM'               , chr(0xEF) . chr(0xBB) . chr(0xBF));

function detect_utf_encoding($text) {
    $first2 = substr($text, 0, 2);
    $first3 = substr($text, 0, 3);
    $first4 = substr($text, 0, 3);
    
    if ($first3 == UTF8_BOM) return 'UTF-8';
    elseif ($first4 == UTF32_BIG_ENDIAN_BOM) return 'UTF-32BE';
    elseif ($first4 == UTF32_LITTLE_ENDIAN_BOM) return 'UTF-32LE';
    elseif ($first2 == UTF16_BIG_ENDIAN_BOM) return 'UTF-16BE';
    elseif ($first2 == UTF16_LITTLE_ENDIAN_BOM) return 'UTF-16LE';

    return false;
}

///////////////////////////////////////////////////////////////////////////
function QCR_SearchPHP($src)
{
  if (preg_match("/(<\?php[\w\s]{5,})/smi", $src, $l_Found, PREG_OFFSET_CAPTURE)) {
	  return $l_Found[0][1];
  }

  if (preg_match("/(<script[^>]*language\s*=\s*)('|\"|)php('|\"|)([^>]*>)/i", $src, $l_Found, PREG_OFFSET_CAPTURE)) {
    return $l_Found[0][1];
  }

  return false;
}


///////////////////////////////////////////////////////////////////////////
function knowUrl($par_URL) {
  global $g_UrlIgnoreList;

  for ($jk = 0; $jk < count($g_UrlIgnoreList); $jk++) {
     if  (stripos($par_URL, $g_UrlIgnoreList[$jk]) !== false) {
     	return true;
     }
  }

  return false;
}

///////////////////////////////////////////////////////////////////////////

function makeSummary($par_Str, $par_Number, $par_Style) {
   return '<tr><td class="' . $par_Style . '" width=400>' . $par_Str . '</td><td class="' . $par_Style . '">' . $par_Number . '</td></tr>';
}

///////////////////////////////////////////////////////////////////////////

function CheckVulnerability($par_Filename, $par_Index, $par_Content) {
    global $g_Vulnerable, $g_CmsListDetector;
	

	$l_Vuln = array();

        $par_Filename = strtolower($par_Filename);

	if (
	    (strpos($par_Filename, 'libraries/joomla/session/session.php') !== false) &&
		(strpos($par_Content, '&& filter_var($_SERVER[\'HTTP_X_FORWARDED_FOR') === false)
		) 
	{		
			$l_Vuln['id'] = 'RCE : https://docs.joomla.org/Security_hotfixes_for_Joomla_EOL_versions';
			$l_Vuln['ndx'] = $par_Index;
			$g_Vulnerable[] = $l_Vuln;
			return true;
	}

	if (
	    (strpos($par_Filename, 'administrator/components/com_media/helpers/media.php') !== false) &&
		(strpos($par_Content, '$format == \'\' || $format == false ||') === false)
		) 
	{		
		if ($g_CmsListDetector->isCms(CMS_JOOMLA, '1.5')) {
			$l_Vuln['id'] = 'AFU : https://docs.joomla.org/Security_hotfixes_for_Joomla_EOL_versions';
			$l_Vuln['ndx'] = $par_Index;
			$g_Vulnerable[] = $l_Vuln;
			return true;
		}
		
		return false;
	}

	if (
	    (strpos($par_Filename, 'joomla/filesystem/file.php') !== false) &&
		(strpos($par_Content, '$file = rtrim($file, \'.\');') === false)
		) 
	{		
		if ($g_CmsListDetector->isCms(CMS_JOOMLA, '1.5')) {
			$l_Vuln['id'] = 'AFU : https://docs.joomla.org/Security_hotfixes_for_Joomla_EOL_versions';
			$l_Vuln['ndx'] = $par_Index;
			$g_Vulnerable[] = $l_Vuln;
			return true;
		}
		
		return false;
	}

	if ((strpos($par_Filename, 'editor/filemanager/upload/test.html') !== false) ||
		(stripos($par_Filename, 'editor/filemanager/browser/default/connectors/php/') !== false) ||
		(stripos($par_Filename, 'editor/filemanager/connectors/uploadtest.html') !== false) ||
	   (strpos($par_Filename, 'editor/filemanager/browser/default/connectors/test.html') !== false)) {
		$l_Vuln['id'] = 'AFU : FCKEDITOR : http://www.exploit-db.com/exploits/17644/ & /exploit/249';
		$l_Vuln['ndx'] = $par_Index;
		$g_Vulnerable[] = $l_Vuln;
		return true;
	}

	if ((strpos($par_Filename, 'inc_php/image_view.class.php') !== false) ||
	    (strpos($par_Filename, '/inc_php/framework/image_view.class.php') !== false)) {
		if (strpos($par_Content, 'showImageByID') === false) {
			$l_Vuln['id'] = 'AFU : REVSLIDER : http://www.exploit-db.com/exploits/35385/';
			$l_Vuln['ndx'] = $par_Index;
			$g_Vulnerable[] = $l_Vuln;
			return true;
		}
		
		return false;
	}

	if ((strpos($par_Filename, 'elfinder/php/connector.php') !== false) ||
	    (strpos($par_Filename, 'elfinder/elfinder.') !== false)) {
			$l_Vuln['id'] = 'AFU : elFinder';
			$l_Vuln['ndx'] = $par_Index;
			$g_Vulnerable[] = $l_Vuln;
			return true;
	}

	if (strpos($par_Filename, 'includes/database/database.inc') !== false) {
		if (strpos($par_Content, 'foreach ($data as $i => $value)') !== false) {
			$l_Vuln['id'] = 'SQLI : DRUPAL : CVE-2014-3704';
			$l_Vuln['ndx'] = $par_Index;
			$g_Vulnerable[] = $l_Vuln;
			return true;
		}
		
		return false;
	}

	if (strpos($par_Filename, 'engine/classes/min/index.php') !== false) {
		if (strpos($par_Content, 'tr_replace(chr(0)') === false) {
			$l_Vuln['id'] = 'AFD : MINIFY : CVE-2013-6619';
			$l_Vuln['ndx'] = $par_Index;
			$g_Vulnerable[] = $l_Vuln;
			return true;
		}
		
		return false;
	}

	if (( strpos($par_Filename, 'timthumb.php') !== false ) || 
	    ( strpos($par_Filename, 'thumb.php') !== false ) || 
	    ( strpos($par_Filename, 'cache.php') !== false ) || 
	    ( strpos($par_Filename, '_img.php') !== false )) {
		if (strpos($par_Content, 'code.google.com/p/timthumb') !== false && strpos($par_Content, '2.8.14') === false ) {
			$l_Vuln['id'] = 'RCE : TIMTHUMB : CVE-2011-4106,CVE-2014-4663';
			$l_Vuln['ndx'] = $par_Index;
			$g_Vulnerable[] = $l_Vuln;
			return true;
		}
		
		return false;
	}

	if (strpos($par_Filename, 'components/com_rsform/helpers/rsform.php') !== false) {
		if (strpos($par_Content, 'eval($form->ScriptDisplay);') !== false) {
			$l_Vuln['id'] = 'RCE : RSFORM : rsform.php, LINE 1605';
			$l_Vuln['ndx'] = $par_Index;
			$g_Vulnerable[] = $l_Vuln;
			return true;
		}
		
		return false;
	}

	if (strpos($par_Filename, 'fancybox-for-wordpress/fancybox.php') !== false) {
		if (strpos($par_Content, '\'reset\' == $_REQUEST[\'action\']') !== false) {
			$l_Vuln['id'] = 'CODE INJECTION : FANCYBOX';
			$l_Vuln['ndx'] = $par_Index;
			$g_Vulnerable[] = $l_Vuln;
			return true;
		}
		
		return false;
	}


	if (strpos($par_Filename, 'cherry-plugin/admin/import-export/upload.php') !== false) {
		if (strpos($par_Content, 'verify nonce') === false) {
			$l_Vuln['id'] = 'AFU : Cherry Plugin';
			$l_Vuln['ndx'] = $par_Index;
			$g_Vulnerable[] = $l_Vuln;
			return true;
		}
		
		return false;
	}
	
	
	if (strpos($par_Filename, 'tiny_mce/plugins/tinybrowser/tinybrowser.php') !== false) {	
		$l_Vuln['id'] = 'AFU : TINYMCE : http://www.exploit-db.com/exploits/9296/';
		$l_Vuln['ndx'] = $par_Index;
		$g_Vulnerable[] = $l_Vuln;
		
		return true;
	}

	if (strpos($par_Filename, '/bx_1c_import.php') !== false) {	
		if (strpos($par_Content, '$_GET[\'action\']=="getfiles"') !== false) {
   		   $l_Vuln['id'] = 'AFD : https://habrahabr.ru/company/dsec/blog/326166/';
   		   $l_Vuln['ndx'] = $par_Index;
   		   $g_Vulnerable[] = $l_Vuln;
   		
   		   return true;
                }
	}

	if (strpos($par_Filename, 'scripts/setup.php') !== false) {		
		if (strpos($par_Content, 'PMA_Config') !== false) {
			$l_Vuln['id'] = 'CODE INJECTION : PHPMYADMIN : http://1337day.com/exploit/5334';
			$l_Vuln['ndx'] = $par_Index;
			$g_Vulnerable[] = $l_Vuln;
			return true;
		}
		
		return false;
	}

	if (strpos($par_Filename, '/uploadify.php') !== false) {		
		if (strpos($par_Content, 'move_uploaded_file($tempFile,$targetFile') !== false) {
			$l_Vuln['id'] = 'AFU : UPLOADIFY : CVE: 2012-1153';
			$l_Vuln['ndx'] = $par_Index;
			$g_Vulnerable[] = $l_Vuln;
			return true;
		}
		
		return false;
	}

	if (strpos($par_Filename, 'com_adsmanager/controller.php') !== false) {		
		if (strpos($par_Content, 'move_uploaded_file($file[\'tmp_name\'], $tempPath.\'/\'.basename($file[') !== false) {
			$l_Vuln['id'] = 'AFU : https://revisium.com/ru/blog/adsmanager_afu.html';
			$l_Vuln['ndx'] = $par_Index;
			$g_Vulnerable[] = $l_Vuln;
			return true;
		}
		
		return false;
	}

	if (strpos($par_Filename, 'wp-content/plugins/wp-mobile-detector/resize.php') !== false) {		
		if (strpos($par_Content, 'file_put_contents($path, file_get_contents($_REQUEST[\'src\']));') !== false) {
			$l_Vuln['id'] = 'AFU : https://www.pluginvulnerabilities.com/2016/05/31/aribitrary-file-upload-vulnerability-in-wp-mobile-detector/';
			$l_Vuln['ndx'] = $par_Index;
			$g_Vulnerable[] = $l_Vuln;
			return true;
		}
		
		return false;
	}


	if (strpos($par_Filename, 'core/lib/drupal.php') !== false) {		
                $version = '';
                if (preg_match('|VERSION\s*=\s*\'(8\.\d+\.\d+)\'|smi', $par_Content, $tmp_ver)) {
                   $version = $tmp_ver[1];
                }

		if (($version !== '') && (version_compare($version, '8.5.1', '<'))) {
			$l_Vuln['id'] = 'Drupageddon 2 : SA-CORE-2018–002';
			$l_Vuln['ndx'] = $par_Index;
			$g_Vulnerable[] = $l_Vuln;
			return true;
		}
		

		return false;
	}

	if (strpos($par_Filename, 'changelog.txt') !== false) {		
                $version = '';
                if (preg_match('|Drupal\s+(7\.\d+),|smi', $par_Content, $tmp_ver)) {
                   $version = $tmp_ver[1];
                }

		if (($version !== '') && (version_compare($version, '7.58', '<'))) {
			$l_Vuln['id'] = 'Drupageddon 2 : SA-CORE-2018–002';
			$l_Vuln['ndx'] = $par_Index;
			$g_Vulnerable[] = $l_Vuln;
			return true;
		}
		
		return false;
	}

	if (strpos($par_Filename, 'phpmailer.php') !== false) {		
		if (strpos($par_Content, 'PHPMailer') !== false) {
                        $l_Found = preg_match('~Version:\s*(\d+)\.(\d+)\.(\d+)~', $par_Content, $l_Match);

                        if ($l_Found) {
                           $l_Version = $l_Match[1] * 1000 + $l_Match[2] * 100 + $l_Match[3];

                           if ($l_Version < 2520) {
                              $l_Found = false;
                           }
                        }

                        if (!$l_Found) {

                           $l_Found = preg_match('~Version\s*=\s*\'(\d+)\.*(\d+)\.(\d+)~', $par_Content, $l_Match);
                           if ($l_Found) {
                              $l_Version = $l_Match[1] * 1000 + $l_Match[2] * 100 + $l_Match[3];
                              if ($l_Version < 5220) {
                                 $l_Found = false;
                              }
                           }
			}


		        if (!$l_Found) {
	   		   $l_Vuln['id'] = 'RCE : CVE-2016-10045, CVE-2016-10031';
			   $l_Vuln['ndx'] = $par_Index;
			   $g_Vulnerable[] = $l_Vuln;
			   return true;
                        }
		}
		
		return false;
	}




}

///////////////////////////////////////////////////////////////////////////
function QCR_GoScan($par_Offset)
{
	global $g_IframerFragment, $g_Iframer, $g_Redirect, $g_Doorway, $g_EmptyLink, $g_Structure, $g_Counter, 
		   $g_HeuristicType, $g_HeuristicDetected, $g_TotalFolder, $g_TotalFiles, $g_WarningPHP, $g_AdwareList,
		   $g_CriticalPHP, $g_Phishing, $g_CriticalJS, $g_UrlIgnoreList, $g_CriticalJSFragment, $g_PHPCodeInside, $g_PHPCodeInsideFragment, 
		   $g_NotRead, $g_WarningPHPFragment, $g_WarningPHPSig, $g_BigFiles, $g_RedirectPHPFragment, $g_EmptyLinkSrc, $g_CriticalPHPSig, $g_CriticalPHPFragment, 
           $g_Base64Fragment, $g_UnixExec, $g_PhishingSigFragment, $g_PhishingFragment, $g_PhishingSig, $g_CriticalJSSig, $g_IframerFragment, $g_CMS, $defaults, $g_AdwareListFragment, $g_KnownList,$g_Vulnerable;

    QCR_Debug('QCR_GoScan ' . $par_Offset);

	$i = 0;
	
	try {
		$s_file = new SplFileObject(QUEUE_FILENAME);
		$s_file->setFlags(SplFileObject::READ_AHEAD | SplFileObject::SKIP_EMPTY | SplFileObject::DROP_NEW_LINE);

		foreach ($s_file as $l_Filename) {
			QCR_ScanFile($l_Filename, $i++);
		}
		
		unset($s_file);	
	}
	catch (Exception $e) { QCR_Debug( $e->getMessage() ); }
}

///////////////////////////////////////////////////////////////////////////
function QCR_ScanFile($l_Filename, $i = 0)
{
	global $g_IframerFragment, $g_Iframer, $g_Redirect, $g_Doorway, $g_EmptyLink, $g_Structure, $g_Counter, 
		   $g_HeuristicType, $g_HeuristicDetected, $g_TotalFolder, $g_TotalFiles, $g_WarningPHP, $g_AdwareList,
		   $g_CriticalPHP, $g_Phishing, $g_CriticalJS, $g_UrlIgnoreList, $g_CriticalJSFragment, $g_PHPCodeInside, $g_PHPCodeInsideFragment, 
		   $g_NotRead, $g_WarningPHPFragment, $g_WarningPHPSig, $g_BigFiles, $g_RedirectPHPFragment, $g_EmptyLinkSrc, $g_CriticalPHPSig, $g_CriticalPHPFragment, 
           $g_Base64Fragment, $g_UnixExec, $g_PhishingSigFragment, $g_PhishingFragment, $g_PhishingSig, $g_CriticalJSSig, $g_IframerFragment, $g_CMS, $defaults, $g_AdwareListFragment, 
           $g_KnownList,$g_Vulnerable, $g_CriticalFiles, $g_DeMapper;

	global $g_CRC;
	static $_files_and_ignored = 0;

			$l_CriticalDetected = false;
			$l_Stat = stat($l_Filename);

			if (substr($l_Filename, -1) == DIR_SEPARATOR) {
				// FOLDER
				$g_Structure['n'][$i] = $l_Filename;
				$g_TotalFolder++;
				printProgress($_files_and_ignored, $l_Filename);
				return;
			}

			QCR_Debug('Scan file ' . $l_Filename);
			printProgress(++$_files_and_ignored, $l_Filename);

     			// ignore itself
     			if ($l_Filename == __FILE__) {
     				return;
     			}

			// FILE
			if ((MAX_SIZE_TO_SCAN > 0 AND $l_Stat['size'] > MAX_SIZE_TO_SCAN) || ($l_Stat['size'] < 0))
			{
				$g_BigFiles[] = $i;

                                if (function_exists('aibolit_onBigFile')) { aibolit_onBigFile($l_Filename); }

				AddResult($l_Filename, $i);

		                $l_Ext = strtolower(pathinfo($l_Filename, PATHINFO_EXTENSION));
                                if ((!AI_HOSTER) && in_array($l_Ext, $g_CriticalFiles)) {
				    $g_CriticalPHP[] = $i;
				    $g_CriticalPHPFragment[] = "BIG FILE. SKIPPED.";
				    $g_CriticalPHPSig[] = "big_1";
                                }
			}
			else
			{
				$g_TotalFiles++;

			$l_TSStartScan = microtime(true);

		$l_Ext = strtolower(pathinfo($l_Filename, PATHINFO_EXTENSION));
		if (filetype($l_Filename) == 'file') {
                   $l_Content = @file_get_contents($l_Filename);
		   if (SHORT_PHP_TAG) {
//                      $l_Content = preg_replace('|<\?\s|smiS', '<?php ', $l_Content); 
                   }

                   $l_Unwrapped = @php_strip_whitespace($l_Filename);
                }

		
                if ((($l_Content == '') || ($l_Unwrapped == '')) && ($l_Stat['size'] > 0)) {
                   $g_NotRead[] = $i;
                   if (function_exists('aibolit_onReadError')) { aibolit_onReadError($l_Filename, 'io'); }
                   AddResult('[io] ' . $l_Filename, $i);
                   return;
                }

				// unix executables
				if (strpos($l_Content, chr(127) . 'ELF') !== false) 
				{
			        	if (!in_array($l_Filename, $g_UnixExec)) {
                    				$g_UnixExec[] = $l_Filename;
					}

				        return;
                		}

				$g_CRC = _hash_($l_Unwrapped);

				$l_UnicodeContent = detect_utf_encoding($l_Content);
				//$l_Unwrapped = $l_Content;

				// check vulnerability in files
				$l_CriticalDetected = CheckVulnerability($l_Filename, $i, $l_Content);				

				if ($l_UnicodeContent !== false) {
       				   if (function_exists('iconv')) {
				      $l_Unwrapped = iconv($l_UnicodeContent, "CP1251//IGNORE", $l_Unwrapped);
//       			   if (function_exists('mb_convert_encoding')) {
//                                    $l_Unwrapped = mb_convert_encoding($l_Unwrapped, $l_UnicodeContent, "CP1251");
                                   } else {
                                      $g_NotRead[] = $i;
                                      if (function_exists('aibolit_onReadError')) { aibolit_onReadError($l_Filename, 'ec'); }
                                      AddResult('[ec] ' . $l_Filename, $i);
				   }
                                }

				// critical
				$g_SkipNextCheck = false;

                                $l_DeobfType = '';
				if (!AI_HOSTER) {
                                   $l_DeobfType = getObfuscateType($l_Unwrapped);
                                }

                                if ($l_DeobfType != '') {
                                   $l_Unwrapped = deobfuscate($l_Unwrapped);
				   $g_SkipNextCheck = checkFalsePositives($l_Filename, $l_Unwrapped, $l_DeobfType);
                                } else {
     				   if (DEBUG_MODE) {
				      stdOut("\n...... NOT OBFUSCATED\n");
				   }
				}

				$l_Unwrapped = UnwrapObfu($l_Unwrapped);
				
				if ((!$g_SkipNextCheck) && CriticalPHP($l_Filename, $i, $l_Unwrapped, $l_Pos, $l_SigId))
				{
				        if ($l_Ext == 'js') {
 					   $g_CriticalJS[] = $i;
 					   $g_CriticalJSFragment[] = getFragment($l_Unwrapped, $l_Pos);
 					   $g_CriticalJSSig[] = $l_SigId;
                                        } else {
       					   $g_CriticalPHP[] = $i;
       					   $g_CriticalPHPFragment[] = getFragment($l_Unwrapped, $l_Pos);
      					   $g_CriticalPHPSig[] = $l_SigId;
                                        }

					$g_SkipNextCheck = true;
				} else {
         				if ((!$g_SkipNextCheck) && CriticalPHP($l_Filename, $i, $l_Content, $l_Pos, $l_SigId))
         				{
					        if ($l_Ext == 'js') {
         					   $g_CriticalJS[] = $i;
         					   $g_CriticalJSFragment[] = getFragment($l_Content, $l_Pos);
         					   $g_CriticalJSSig[] = $l_SigId;
                                                } else {
               					   $g_CriticalPHP[] = $i;
               					   $g_CriticalPHPFragment[] = getFragment($l_Content, $l_Pos);
      						   $g_CriticalPHPSig[] = $l_SigId;
                                                }

         					$g_SkipNextCheck = true;
         				}
				}

				$l_TypeDe = 0;
			    if ((!$g_SkipNextCheck) && HeuristicChecker($l_Content, $l_TypeDe, $l_Filename)) {
					$g_HeuristicDetected[] = $i;
					$g_HeuristicType[] = $l_TypeDe;
					$l_CriticalDetected = true;
				}

				// critical JS
				if (!$g_SkipNextCheck) {
					$l_Pos = CriticalJS($l_Filename, $i, $l_Unwrapped, $l_SigId);
					if ($l_Pos !== false)
					{
					        if ($l_Ext == 'js') {
         					   $g_CriticalJS[] = $i;
         					   $g_CriticalJSFragment[] = getFragment($l_Unwrapped, $l_Pos);
         					   $g_CriticalJSSig[] = $l_SigId;
                                                } else {
               					   $g_CriticalPHP[] = $i;
               					   $g_CriticalPHPFragment[] = getFragment($l_Unwrapped, $l_Pos);
      						   $g_CriticalPHPSig[] = $l_SigId;
                                                }

						$g_SkipNextCheck = true;
					}
			    }

				// phishing
				if (!$g_SkipNextCheck) {
					$l_Pos = Phishing($l_Filename, $i, $l_Unwrapped, $l_SigId);
					if ($l_Pos === false) {
                                            $l_Pos = Phishing($l_Filename, $i, $l_Content, $l_SigId);
                                        }

					if ($l_Pos !== false)
					{
						$g_Phishing[] = $i;
						$g_PhishingFragment[] = getFragment($l_Unwrapped, $l_Pos);
						$g_PhishingSigFragment[] = $l_SigId;
						$g_SkipNextCheck = true;
					}
				}

			
			if (!$g_SkipNextCheck) {
				if (SCAN_ALL_FILES || stripos($l_Filename, 'index.'))
				{
					// check iframes
					if (preg_match_all('|<iframe[^>]+src.+?>|smi', $l_Unwrapped, $l_Found, PREG_SET_ORDER)) 
					{
						for ($kk = 0; $kk < count($l_Found); $kk++) {
						    $l_Pos = stripos($l_Found[$kk][0], 'http://');
						    $l_Pos = $l_Pos || stripos($l_Found[$kk][0], 'https://');
						    $l_Pos = $l_Pos || stripos($l_Found[$kk][0], 'ftp://');
							if  (($l_Pos !== false ) && (!knowUrl($l_Found[$kk][0]))) {
         						$g_Iframer[] = $i;
         						$g_IframerFragment[] = getFragment($l_Found[$kk][0], $l_Pos);
         						$l_CriticalDetected = true;
							}
						}
					}

					// check empty links
					if ((($defaults['report_mask'] & REPORT_MASK_SPAMLINKS) == REPORT_MASK_SPAMLINKS) &&
					   (preg_match_all('|<a[^>]+href([^>]+?)>(.*?)</a>|smi', $l_Unwrapped, $l_Found, PREG_SET_ORDER)))
					{
						for ($kk = 0; $kk < count($l_Found); $kk++) {
							if  ((stripos($l_Found[$kk][1], 'http://') !== false) &&
                                                            (trim(strip_tags($l_Found[$kk][2])) == '')) {

								$l_NeedToAdd = true;

							    if  ((stripos($l_Found[$kk][1], $defaults['site_url']) !== false)
                                                                 || knowUrl($l_Found[$kk][1])) {
										$l_NeedToAdd = false;
								}
								
								if ($l_NeedToAdd && (count($g_EmptyLink) < MAX_EXT_LINKS)) {
									$g_EmptyLink[] = $i;
									$g_EmptyLinkSrc[$i][] = substr($l_Found[$kk][0], 0, MAX_PREVIEW_LEN);
									$l_CriticalDetected = true;
								}
							}
						}
					}
				}

				// check for PHP code inside any type of file
				if (stripos($l_Ext, 'ph') === false)
				{
					$l_Pos = QCR_SearchPHP($l_Content);
					if ($l_Pos !== false)
					{
						$g_PHPCodeInside[] = $i;
						$g_PHPCodeInsideFragment[] = getFragment($l_Unwrapped, $l_Pos);
						$l_CriticalDetected = true;
					}
				}

				// htaccess
				if (stripos($l_Filename, '.htaccess'))
				{
				
					if (stripos($l_Content, 'index.php?name=$1') !== false ||
						stripos($l_Content, 'index.php?m=1') !== false
					)
					{
						$g_SuspDir[] = $i;
					}

					$l_HTAContent = preg_replace('|^\s*#.+$|m', '', $l_Content);

					$l_Pos = stripos($l_Content, 'auto_prepend_file');
					if ($l_Pos !== false) {
						$g_Redirect[] = $i;
						$g_RedirectPHPFragment[] = getFragment($l_Content, $l_Pos);
						$l_CriticalDetected = true;
					}
					
					$l_Pos = stripos($l_Content, 'auto_append_file');
					if ($l_Pos !== false) {
						$g_Redirect[] = $i;
						$g_RedirectPHPFragment[] = getFragment($l_Content, $l_Pos);
						$l_CriticalDetected = true;
					}

					$l_Pos = stripos($l_Content, '^(%2d|-)[^=]+$');
					if ($l_Pos !== false)
					{
						$g_Redirect[] = $i;
                        			$g_RedirectPHPFragment[] = getFragment($l_Content, $l_Pos);
						$l_CriticalDetected = true;
					}

					if (!$l_CriticalDetected) {
						$l_Pos = stripos($l_Content, '%{HTTP_USER_AGENT}');
						if ($l_Pos !== false)
						{
							$g_Redirect[] = $i;
							$g_RedirectPHPFragment[] = getFragment($l_Content, $l_Pos);
							$l_CriticalDetected = true;
						}
					}

					if (!$l_CriticalDetected) {
						if (
							preg_match_all("|RewriteRule\s+.+?\s+http://(.+?)/.+\s+\[.*R=\d+.*\]|smi", $l_HTAContent, $l_Found, PREG_SET_ORDER)
						)
						{
							$l_Host = str_replace('www.', '', $_SERVER['HTTP_HOST']);
							for ($j = 0; $j < sizeof($l_Found); $j++)
							{
								$l_Found[$j][1] = str_replace('www.', '', $l_Found[$j][1]);
								if ($l_Found[$j][1] != $l_Host)
								{
									$g_Redirect[] = $i;
									$l_CriticalDetected = true;
									break;
								}
							}
						}
					}

					unset($l_HTAContent);
			    }
			

			    // warnings
				$l_Pos = '';
				
			    if (WarningPHP($l_Filename, $l_Unwrapped, $l_Pos, $l_SigId))
				{       
					$l_Prio = 1;
					if (strpos($l_Filename, '.ph') !== false) {
					   $l_Prio = 0;
					}
					
					$g_WarningPHP[$l_Prio][] = $i;
					$g_WarningPHPFragment[$l_Prio][] = getFragment($l_Unwrapped, $l_Pos);
					$g_WarningPHPSig[] = $l_SigId;

					$l_CriticalDetected = true;
				}
				

				// adware
				if (Adware($l_Filename, $l_Unwrapped, $l_Pos))
				{
					$g_AdwareList[] = $i;
					$g_AdwareListFragment[] = getFragment($l_Unwrapped, $l_Pos);
					$l_CriticalDetected = true;
				}

				// articles
				if (stripos($l_Filename, 'article_index'))
				{
					$g_AdwareList[] = $i;
					$l_CriticalDetected = true;
				}
			}
		} // end of if (!$g_SkipNextCheck) {
			
			unset($l_Unwrapped);
			unset($l_Content);
			
			//printProgress(++$_files_and_ignored, $l_Filename);

			$l_TSEndScan = microtime(true);
                        if ($l_TSEndScan - $l_TSStartScan >= 0.5) {
			   			   usleep(SCAN_DELAY * 1000);
                        }

			if ($g_SkipNextCheck || $l_CriticalDetected) {
				AddResult($l_Filename, $i);
			}
}

function AddResult($l_Filename, $i)
{
	global $g_Structure, $g_CRC;
	
	$l_Stat = stat($l_Filename);
	$g_Structure['n'][$i] = $l_Filename;
	$g_Structure['s'][$i] = $l_Stat['size'];
	$g_Structure['c'][$i] = $l_Stat['ctime'];
	$g_Structure['m'][$i] = $l_Stat['mtime'];
	$g_Structure['crc'][$i] = $g_CRC;
}

///////////////////////////////////////////////////////////////////////////
function WarningPHP($l_FN, $l_Content, &$l_Pos, &$l_SigId)
{
	   global $g_SusDB,$g_ExceptFlex, $gXX_FlexDBShe, $gX_FlexDBShe, $g_FlexDBShe, $gX_DBShe, $g_DBShe, $g_Base64, $g_Base64Fragment;

  $l_Res = false;

  if (AI_EXTRA_WARN) {
  	foreach ($g_SusDB as $l_Item) {
    	if (preg_match('#' . $l_Item . '#smiS', $l_Content, $l_Found, PREG_OFFSET_CAPTURE)) {
       	 	if (!CheckException($l_Content, $l_Found)) {
           	 	$l_Pos = $l_Found[0][1];
           	 	//$l_SigId = myCheckSum($l_Item);
           	 	$l_SigId = getSigId($l_Found);
           	 	return true;
       	 	}
    	}
  	}
  }

  if (AI_EXPERT < 2) {
    	foreach ($gXX_FlexDBShe as $l_Item) {
      		if (preg_match('#' . $l_Item . '#smiS', $l_Content, $l_Found, PREG_OFFSET_CAPTURE)) {
             	$l_Pos = $l_Found[0][1];
           	    //$l_SigId = myCheckSum($l_Item);
           	    $l_SigId = getSigId($l_Found);
        	    return true;
	  		}
    	}

	}

    if (AI_EXPERT < 1) {
    	foreach ($gX_FlexDBShe as $l_Item) {
      		if (preg_match('#' . $l_Item . '#smiS', $l_Content, $l_Found, PREG_OFFSET_CAPTURE)) {
             	$l_Pos = $l_Found[0][1];
           	 	//$l_SigId = myCheckSum($l_Item);
           	 	$l_SigId = getSigId($l_Found);
        	    return true;
	  		}
    	}

	    $l_Content_lo = strtolower($l_Content);

	    foreach ($gX_DBShe as $l_Item) {
	      $l_Pos = strpos($l_Content_lo, $l_Item);
	      if ($l_Pos !== false) {
	         $l_SigId = myCheckSum($l_Item);
	         return true;
	      }
		}
	}

}

///////////////////////////////////////////////////////////////////////////
function Adware($l_FN, $l_Content, &$l_Pos)
{
  global $g_AdwareSig;

  $l_Res = false;

foreach ($g_AdwareSig as $l_Item) {
    $offset = 0;
    while (preg_match('#' . $l_Item . '#smi', $l_Content, $l_Found, PREG_OFFSET_CAPTURE, $offset)) {
       if (!CheckException($l_Content, $l_Found)) {
           $l_Pos = $l_Found[0][1];
           return true;
       }

       $offset = $l_Found[0][1] + 1;
    }
  }

  return $l_Res;
}

///////////////////////////////////////////////////////////////////////////
function CheckException(&$l_Content, &$l_Found) {
  global $g_ExceptFlex, $gX_FlexDBShe, $gXX_FlexDBShe, $g_FlexDBShe, $gX_DBShe, $g_DBShe, $g_Base64, $g_Base64Fragment;
   $l_FoundStrPlus = substr($l_Content, max($l_Found[0][1] - 10, 0), 70);

   foreach ($g_ExceptFlex as $l_ExceptItem) {
      if (@preg_match('#' . $l_ExceptItem . '#smi', $l_FoundStrPlus, $l_Detected)) {
//         print("\n\nEXCEPTION FOUND\n[" . $l_ExceptItem .  "]\n" . $l_Content . "\n\n----------\n\n");
         return true;
      }
   }

   return false;
}

///////////////////////////////////////////////////////////////////////////
function Phishing($l_FN, $l_Index, $l_Content, &$l_SigId)
{
  global $g_PhishingSig, $g_PhishFiles, $g_PhishEntries;

  $l_Res = false;

  // need check file (by extension) ?
  $l_SkipCheck = SMART_SCAN;

if ($l_SkipCheck) {
  	foreach($g_PhishFiles as $l_Ext) {
  		  if (strpos($l_FN, $l_Ext) !== false) {
		  			$l_SkipCheck = false;
		  		  	break;
  	  	  }
  	  }
  }

  // need check file (by signatures) ?
  if ($l_SkipCheck && preg_match('~' . $g_PhishEntries . '~smiS', $l_Content, $l_Found)) {
	  $l_SkipCheck = false;
  }

  if ($l_SkipCheck && SMART_SCAN) {
      if (DEBUG_MODE) {
         echo "Skipped phs file, not critical.\n";
      }

	  return false;
  }


  foreach ($g_PhishingSig as $l_Item) {
    $offset = 0;
    while (preg_match('#' . $l_Item . '#smi', $l_Content, $l_Found, PREG_OFFSET_CAPTURE, $offset)) {
       if (!CheckException($l_Content, $l_Found)) {
           $l_Pos = $l_Found[0][1];
//           $l_SigId = myCheckSum($l_Item);
           $l_SigId = getSigId($l_Found);

           if (DEBUG_MODE) {
              echo "Phis: $l_FN matched [$l_Item] in $l_Pos\n";
           }

           return $l_Pos;
       }
       $offset = $l_Found[0][1] + 1;

    }
  }

  return $l_Res;
}

///////////////////////////////////////////////////////////////////////////
function CriticalJS($l_FN, $l_Index, $l_Content, &$l_SigId)
{
  global $g_JSVirSig, $gX_JSVirSig, $g_VirusFiles, $g_VirusEntries, $g_RegExpStat;

  $l_Res = false;
  
    // need check file (by extension) ?
    $l_SkipCheck = SMART_SCAN;
	
	if ($l_SkipCheck) {
       	   foreach($g_VirusFiles as $l_Ext) {
    		  if (strpos($l_FN, $l_Ext) !== false) {
  		  			$l_SkipCheck = false;
  		  		  	break;
    	  	  }
    	  }
	  }
  
    // need check file (by signatures) ?
    if ($l_SkipCheck && preg_match('~' . $g_VirusEntries . '~smiS', $l_Content, $l_Found)) {
  	  $l_SkipCheck = false;
    }
  
    if ($l_SkipCheck && SMART_SCAN) {
        if (DEBUG_MODE) {
           echo "Skipped js file, not critical.\n";
        }

  	  return false;
    }
  

  foreach ($g_JSVirSig as $l_Item) {
    $offset = 0;
    if (DEBUG_PERFORMANCE) { 
       $stat_start = microtime(true);
    }

    while (preg_match('#' . $l_Item . '#smi', $l_Content, $l_Found, PREG_OFFSET_CAPTURE, $offset)) {

       if (!CheckException($l_Content, $l_Found)) {
           $l_Pos = $l_Found[0][1];
//           $l_SigId = myCheckSum($l_Item);
           $l_SigId = getSigId($l_Found);

           if (DEBUG_MODE) {
              echo "JS: $l_FN matched [$l_Item] in $l_Pos\n";
           }

           return $l_Pos;
       }

       $offset = $l_Found[0][1] + 1;

    }

    if (DEBUG_PERFORMANCE) { 
       $stat_stop = microtime(true);
       $g_RegExpStat[$l_Item] += $stat_stop - $stat_start;
    }
//   if (pcre_error($l_FN, $l_Index)) {  }

  }

if (AI_EXPERT > 1) {
  foreach ($gX_JSVirSig as $l_Item) {
    if (DEBUG_PERFORMANCE) { 
       $stat_start = microtime(true);
    }

    if (preg_match('#' . $l_Item . '#smi', $l_Content, $l_Found, PREG_OFFSET_CAPTURE)) {
       if (!CheckException($l_Content, $l_Found)) {
           $l_Pos = $l_Found[0][1];
           //$l_SigId = myCheckSum($l_Item);
           $l_SigId = getSigId($l_Found);

           if (DEBUG_MODE) {
              echo "JS PARA: $l_FN matched [$l_Item] in $l_Pos\n";
           }

           return $l_Pos;
       }
    }

    if (DEBUG_PERFORMANCE) { 
       $stat_stop = microtime(true);
       $g_RegExpStat[$l_Item] += $stat_stop - $stat_start;
    }

//   if (pcre_error($l_FN, $l_Index)) {  }

  }
}

  return $l_Res;
}

////////////////////////////////////////////////////////////////////////////
function pcre_error($par_FN, $par_Index) {
   global $g_NotRead, $g_Structure;

   $err = preg_last_error();
   if (($err == PREG_BACKTRACK_LIMIT_ERROR) || ($err == PREG_RECURSION_LIMIT_ERROR)) {
      if (!in_array($par_Index, $g_NotRead)) {
         if (function_exists('aibolit_onReadError')) { aibolit_onReadError($l_Filename, 're'); }
         $g_NotRead[] = $par_Index;
         AddResult('[re] ' . $par_FN, $par_Index);
      }
 
      return true;
   }

   return false;
}



////////////////////////////////////////////////////////////////////////////
define('SUSP_MTIME', 1); // suspicious mtime (greater than ctime)
define('SUSP_PERM', 2); // suspicious permissions 
define('SUSP_PHP_IN_UPLOAD', 3); // suspicious .php file in upload or image folder 

  function get_descr_heur($type) {
     switch ($type) {
	     case SUSP_MTIME: return AI_STR_077; 
	     case SUSP_PERM: return AI_STR_078;  
	     case SUSP_PHP_IN_UPLOAD: return AI_STR_079; 
	 }
	 
	 return "---";
  }

  ///////////////////////////////////////////////////////////////////////////
  function HeuristicChecker($l_Content, &$l_Type, $l_Filename) {
     $res = false;
	 
	 $l_Stat = stat($l_Filename);
	 // most likely changed by touch
	 if ($l_Stat['ctime'] < $l_Stat['mtime']) {
	     $l_Type = SUSP_MTIME;
		 return true;
	 }

	 	 
	 $l_Perm = fileperms($l_Filename) & 0777;
	 if (($l_Perm & 0400 != 0400) || // not readable by owner
		($l_Perm == 0000) ||
		($l_Perm == 0404) ||
		($l_Perm == 0505))
	 {
		 $l_Type = SUSP_PERM;
		 return true;
	 }

	 
     if ((strpos($l_Filename, '.ph')) && (
	     strpos($l_Filename, '/images/stories/') ||
	     //strpos($l_Filename, '/img/') ||
		 //strpos($l_Filename, '/images/') ||
	     //strpos($l_Filename, '/uploads/') ||
		 strpos($l_Filename, '/wp-content/upload/') 
	    )	    
	 ) {
		$l_Type = SUSP_PHP_IN_UPLOAD;
	 	return true;
	 }

     return false;
  }

///////////////////////////////////////////////////////////////////////////
function CriticalPHP($l_FN, $l_Index, $l_Content, &$l_Pos, &$l_SigId)
{
  global $g_ExceptFlex, $gXX_FlexDBShe, $gX_FlexDBShe, $g_FlexDBShe, $gX_DBShe, $g_DBShe, $g_Base64, $g_Base64Fragment,
  $g_CriticalFiles, $g_CriticalEntries, $g_RegExpStat;

  // need check file (by extension) ?
  $l_SkipCheck = SMART_SCAN;

  if ($l_SkipCheck) {
	  foreach($g_CriticalFiles as $l_Ext) {
  	  	if ((strpos($l_FN, $l_Ext) !== false) && (strpos($l_FN, '.js') === false)) {
		   $l_SkipCheck = false;
		   break;
  	  	}
  	  }
  }
  
  // need check file (by signatures) ?
  if ($l_SkipCheck && preg_match('~' . $g_CriticalEntries . '~smiS', $l_Content, $l_Found)) {
     $l_SkipCheck = false;
  }
  
  
  // if not critical - skip it 
  if ($l_SkipCheck && SMART_SCAN) {
      if (DEBUG_MODE) {
         echo "Skipped file, not critical.\n";
      }

	  return false;
  }

  foreach ($g_FlexDBShe as $l_Item) {
    $offset = 0;

    if (DEBUG_PERFORMANCE) { 
       $stat_start = microtime(true);
    }

    while (preg_match('#' . $l_Item . '#smiS', $l_Content, $l_Found, PREG_OFFSET_CAPTURE, $offset)) {
       if (!CheckException($l_Content, $l_Found)) {
           $l_Pos = $l_Found[0][1];
           //$l_SigId = myCheckSum($l_Item);
           $l_SigId = getSigId($l_Found);

           if (DEBUG_MODE) {
              echo "CRIT 1: $l_FN matched [$l_Item] in $l_Pos\n";
           }

           return true;
       }

       $offset = $l_Found[0][1] + 1;

    }

    if (DEBUG_PERFORMANCE) { 
       $stat_stop = microtime(true);
       $g_RegExpStat[$l_Item] += $stat_stop - $stat_start;
    }

//   if (pcre_error($l_FN, $l_Index)) {  }

  }

if (AI_EXPERT > 0) {
  foreach ($gX_FlexDBShe as $l_Item) {
    if (DEBUG_PERFORMANCE) { 
       $stat_start = microtime(true);
    }

    if (preg_match('#' . $l_Item . '#smiS', $l_Content, $l_Found, PREG_OFFSET_CAPTURE)) {
       if (!CheckException($l_Content, $l_Found)) {
           $l_Pos = $l_Found[0][1];
           //$l_SigId = myCheckSum($l_Item);
           $l_SigId = getSigId($l_Found);

           if (DEBUG_MODE) {
              echo "CRIT 3: $l_FN matched [$l_Item] in $l_Pos\n";
           }

           return true;
       }
    }

    if (DEBUG_PERFORMANCE) { 
       $stat_stop = microtime(true);
       $g_RegExpStat[$l_Item] += $stat_stop - $stat_start;
    }

//   if (pcre_error($l_FN, $l_Index)) {  }
  }
}

if (AI_EXPERT > 1) {
  foreach ($gXX_FlexDBShe as $l_Item) {
    if (DEBUG_PERFORMANCE) { 
       $stat_start = microtime(true);
    }

    if (preg_match('#' . $l_Item . '#smiS', $l_Content, $l_Found, PREG_OFFSET_CAPTURE)) {
       if (!CheckException($l_Content, $l_Found)) {
           $l_Pos = $l_Found[0][1];
           //$l_SigId = myCheckSum($l_Item);
           $l_SigId = getSigId($l_Found);

           if (DEBUG_MODE) {
              echo "CRIT 2: $l_FN matched [$l_Item] in $l_Pos\n";
           }

           return true;
       }
    }

    if (DEBUG_PERFORMANCE) { 
       $stat_stop = microtime(true);
       $g_RegExpStat[$l_Item] += $stat_stop - $stat_start;
    }

//   if (pcre_error($l_FN, $l_Index)) {  }
  }
}

  $l_Content_lo = strtolower($l_Content);

  foreach ($g_DBShe as $l_Item) {
    $l_Pos = strpos($l_Content_lo, $l_Item);
    if ($l_Pos !== false) {
       $l_SigId = myCheckSum($l_Item);

       if (DEBUG_MODE) {
          echo "CRIT 4: $l_FN matched [$l_Item] in $l_Pos\n";
       }

       return true;
    }
  }

if (AI_EXPERT > 0) {
  foreach ($gX_DBShe as $l_Item) {
    $l_Pos = strpos($l_Content_lo, $l_Item);
    if ($l_Pos !== false) {
       $l_SigId = myCheckSum($l_Item);

       if (DEBUG_MODE) {
          echo "CRIT 5: $l_FN matched [$l_Item] in $l_Pos\n";
       }

       return true;
    }
  }
}

if (AI_HOSTER) return false;

if (AI_EXPERT > 0) {
  if ((strpos($l_Content, 'GIF89') === 0) && (strpos($l_FN, '.php') !== false )) {
     $l_Pos = 0;

     if (DEBUG_MODE) {
          echo "CRIT 6: $l_FN matched [$l_Item] in $l_Pos\n";
     }

     return true;
  }
}

  // detect uploaders / droppers
if (AI_EXPERT > 1) {
  $l_Found = null;
  if (
     (filesize($l_FN) < 1024) &&
     (strpos($l_FN, '.ph') !== false) &&
     (
       (($l_Pos = strpos($l_Content, 'multipart/form-data')) > 0) || 
       (($l_Pos = strpos($l_Content, '$_FILE[') > 0)) ||
       (($l_Pos = strpos($l_Content, 'move_uploaded_file')) > 0) ||
       (preg_match('|\bcopy\s*\(|smi', $l_Content, $l_Found, PREG_OFFSET_CAPTURE))
     )
     ) {
       if ($l_Found != null) {
          $l_Pos = $l_Found[0][1];
       } 
     if (DEBUG_MODE) {
          echo "CRIT 7: $l_FN matched [$l_Item] in $l_Pos\n";
     }

     return true;
  }
}

  return false;
}

///////////////////////////////////////////////////////////////////////////
if (!isCli()) {
   header('Content-type: text/html; charset=utf-8');
}

if (!isCli()) {

  $l_PassOK = false;
  if (strlen(PASS) > 8) {
     $l_PassOK = true;   
  } 

  if ($l_PassOK && preg_match('|[0-9]|', PASS, $l_Found) && preg_match('|[A-Z]|', PASS, $l_Found) && preg_match('|[a-z]|', PASS, $l_Found) ) {
     $l_PassOK = true;   
  }
  
  if (!$l_PassOK) {  
    echo sprintf(AI_STR_009, generatePassword());
    exit;
  }

  if (isset($_GET['fn']) && ($_GET['ph'] == crc32(PASS))) {
     printFile();
     exit;
  }

  if ($_GET['p'] != PASS) {
    $generated_pass = generatePassword(); 
    echo sprintf(AI_STR_010, $generated_pass, $generated_pass);
    exit;
  }
}

if (!is_readable(ROOT_PATH)) {
  echo AI_STR_011;
  exit;
}

if (isCli()) {
	if (defined('REPORT_PATH') AND REPORT_PATH)
	{
		if (!is_writable(REPORT_PATH))
		{
			die2("\nCannot write report. Report dir " . REPORT_PATH . " is not writable.");
		}

		else if (!REPORT_FILE)
		{
			die2("\nCannot write report. Report filename is empty.");
		}

		else if (($file = REPORT_PATH . DIR_SEPARATOR . REPORT_FILE) AND is_file($file) AND !is_writable($file))
		{
			die2("\nCannot write report. Report file '$file' exists but is not writable.");
		}
	}
}


// detect version CMS
$g_KnownCMS = array();
$tmp_cms = array();
$g_CmsListDetector = new CmsVersionDetector(ROOT_PATH);
$l_CmsDetectedNum = $g_CmsListDetector->getCmsNumber();
for ($tt = 0; $tt < $l_CmsDetectedNum; $tt++) {
    $g_CMS[] = $g_CmsListDetector->getCmsName($tt) . ' v' . makeSafeFn($g_CmsListDetector->getCmsVersion($tt));
    $tmp_cms[strtolower($g_CmsListDetector->getCmsName($tt))] = 1;
}

if (count($tmp_cms) > 0) {
   $g_KnownCMS = array_keys($tmp_cms);
   $len = count($g_KnownCMS);
   for ($i = 0; $i < $len; $i++) {
      if ($g_KnownCMS[$i] == strtolower(CMS_WORDPRESS)) $g_KnownCMS[] = 'wp';
      if ($g_KnownCMS[$i] == strtolower(CMS_WEBASYST)) $g_KnownCMS[] = 'shopscript';
      if ($g_KnownCMS[$i] == strtolower(CMS_IPB)) $g_KnownCMS[] = 'ipb';
      if ($g_KnownCMS[$i] == strtolower(CMS_DLE)) $g_KnownCMS[] = 'dle';
      if ($g_KnownCMS[$i] == strtolower(CMS_INSTANTCMS)) $g_KnownCMS[] = 'instantcms';
      if ($g_KnownCMS[$i] == strtolower(CMS_SHOPSCRIPT)) $g_KnownCMS[] = 'shopscript';
      if ($g_KnownCMS[$i] == strtolower(CMS_DRUPAL)) $g_KnownCMS[] = 'drupal';
   }
}


$g_DirIgnoreList = array();
$g_IgnoreList = array();
$g_UrlIgnoreList = array();
$g_KnownList = array();

$l_IgnoreFilename = $g_AiBolitAbsolutePath . '/.aignore';
$l_DirIgnoreFilename = $g_AiBolitAbsolutePath . '/.adirignore';
$l_UrlIgnoreFilename = $g_AiBolitAbsolutePath . '/.aurlignore';

if (file_exists($l_IgnoreFilename)) {
    $l_IgnoreListRaw = file($l_IgnoreFilename);
    for ($i = 0; $i < count($l_IgnoreListRaw); $i++) 
    {
    	$g_IgnoreList[] = explode("\t", trim($l_IgnoreListRaw[$i]));
    }
    unset($l_IgnoreListRaw);
}

if (file_exists($l_DirIgnoreFilename)) {
    $g_DirIgnoreList = file($l_DirIgnoreFilename);
	
	for ($i = 0; $i < count($g_DirIgnoreList); $i++) {
		$g_DirIgnoreList[$i] = trim($g_DirIgnoreList[$i]);
	}
}

if (file_exists($l_UrlIgnoreFilename)) {
    $g_UrlIgnoreList = file($l_UrlIgnoreFilename);
	
	for ($i = 0; $i < count($g_UrlIgnoreList); $i++) {
		$g_UrlIgnoreList[$i] = trim($g_UrlIgnoreList[$i]);
	}
}


$l_SkipMask = array(
            '/template_\w{32}.css',
            '/cache/templates/.{1,150}\.tpl\.php',
	    '/system/cache/templates_c/\w{1,40}\.php',
	    '/assets/cache/rss/\w{1,60}',
            '/cache/minify/minify_\w{32}',
            '/cache/page/\w{32}\.php',
            '/cache/object/\w{1,10}/\w{1,10}/\w{1,10}/\w{32}\.php',
            '/cache/wp-cache-\d{32}\.php',
            '/cache/page/\w{32}\.php_expire',
	    '/cache/page/\w{32}-cache-page-\w{32}\.php',
	    '\w{32}-cache-com_content-\w{32}\.php',
	    '\w{32}-cache-mod_custom-\w{32}\.php',
	    '\w{32}-cache-mod_templates-\w{32}\.php',
            '\w{32}-cache-_system-\w{32}\.php',
            '/cache/twig/\w{1,32}/\d+/\w{1,100}\.php', 
            '/autoptimize/js/autoptimize_\w{32}\.js',
            '/bitrix/cache/\w{32}\.php',
            '/bitrix/cache/.+/\w{32}\.php',
            '/bitrix/cache/iblock_find/',
            '/bitrix/managed_cache/MYSQL/user_option/[^/]+/',
            '/bitrix/cache/s1/bitrix/catalog\.section/',
            '/bitrix/cache/s1/bitrix/catalog\.element/',
            '/bitrix/cache/s1/bitrix/menu/',
            '/catalog.element/[^/]+/[^/]+/\w{32}\.php',
            '/bitrix/managed\_cache/.*/\.\w{32}\.php',
            '/core/cache/mgr/smarty/default/.{1,100}\.tpl\.php',
            '/core/cache/resource/web/resources/[0-9]{1,50}\.cache\.php',
            '/smarty/compiled/SC/.*/%%.*\.php',
            '/smarty/.{1,150}\.tpl\.php',
            '/smarty/compile/.{1,150}\.tpl\.cache\.php',
            '/files/templates_c/.{1,150}\.html\.php',
            '/uploads/javascript_global/.{1,150}\.js',
            '/assets/cache/rss/\w{32}',
	    '/assets/cache/docid_\d+_\w{32}\.pageCache\.php',
            '/t3-assets/dev/t3/.*-cache-\w{1,20}-.{1,150}\.php',
	    '/t3-assets/js/js-\w{1,30}\.js',
            '/temp/cache/SC/.*/\.cache\..*\.php',
            '/tmp/sess\_\w{32}$',
            '/assets/cache/docid\_.*\.pageCache\.php',
            '/stat/usage\_\w+\.html',
            '/stat/site\_\w+\.html',
            '/gallery/item/list/\w+\.cache\.php',
            '/core/cache/registry/.*/ext-.*\.php',
            '/core/cache/resource/shk\_/\w+\.cache\.php',
            '/webstat/awstats.*\.txt',
            '/awstats/awstats.*\.txt',
            '/awstats/.{1,80}\.pl',
            '/awstats/.{1,80}\.html',
            '/inc/min/styles_\w+\.min\.css',
            '/inc/min/styles_\w+\.min\.js',
            '/logs/error\_log\..*',
            '/logs/xferlog\..*',
            '/logs/access_log\..*',
            '/logs/cron\..*',
            '/logs/exceptions/.+\.log$',
            '/hyper-cache/[^/]+/[^/]+/[^/]+/index\.html',
            '/mail/new/[^,]+,S=[^,]+,W=.+',
            '/mail/new/[^,]=,S=.+',
            '/application/logs/\d+/\d+/\d+\.php',
            '/sites/default/files/js/js_\w{32}\.js',
            '/yt-assets/\w{32}\.css',
);

$l_SkipSample = array();

if (SMART_SCAN) {
   $g_DirIgnoreList = array_merge($g_DirIgnoreList, $l_SkipMask);
}

QCR_Debug();

// Load custom signatures

try {
	$s_file = new SplFileObject($g_AiBolitAbsolutePath."/ai-bolit.sig");
	$s_file->setFlags(SplFileObject::READ_AHEAD | SplFileObject::SKIP_EMPTY | SplFileObject::DROP_NEW_LINE);
	foreach ($s_file as $line) {
		$g_FlexDBShe[] = preg_replace('~\G(?:[^#\\\\]+|\\\\.)*+\K#~', '\\#', $line); // escaping #
	}
	stdOut("Loaded " . $s_file->key() . " signatures from ai-bolit.sig");
	$s_file = null; // file handler is closed
} catch (Exception $e) { QCR_Debug( "Import ai-bolit.sig " . $e->getMessage() ); }

QCR_Debug();

	$defaults['skip_ext'] = strtolower(trim($defaults['skip_ext']));
         if ($defaults['skip_ext'] != '') {
	    $g_IgnoredExt = explode(',', $defaults['skip_ext']);
	    for ($i = 0; $i < count($g_IgnoredExt); $i++) {
                $g_IgnoredExt[$i] = trim($g_IgnoredExt[$i]);
             }

	    QCR_Debug('Skip files with extensions: ' . implode(',', $g_IgnoredExt));
	    stdOut('Skip extensions: ' . implode(',', $g_IgnoredExt));
         } 

// scan single file
if (defined('SCAN_FILE')) {
   if (file_exists(SCAN_FILE) && is_file(SCAN_FILE) && is_readable(SCAN_FILE)) {
       stdOut("Start scanning file '" . SCAN_FILE . "'.");
       QCR_ScanFile(SCAN_FILE); 
   } else { 
       stdOut("Error:" . SCAN_FILE . " either is not a file or readable");
   }
} else {
	if (isset($_GET['2check'])) {
		$options['with-2check'] = 1;
	}
   
   // scan list of files from file
   if (!(ICHECK || IMAKE) && isset($options['with-2check']) && file_exists(DOUBLECHECK_FILE)) {
      stdOut("Start scanning the list from '" . DOUBLECHECK_FILE . "'.\n");
      $lines = file(DOUBLECHECK_FILE);
      for ($i = 0, $size = count($lines); $i < $size; $i++) {
         $lines[$i] = trim($lines[$i]);
         if (empty($lines[$i])) unset($lines[$i]);
      }
      /* skip first line with <?php die("Forbidden"); ?> */
      unset($lines[0]);
      $g_FoundTotalFiles = count($lines);
      $i = 1;
      foreach ($lines as $l_FN) {
         is_dir($l_FN) && $g_TotalFolder++;
         printProgress( $i++, $l_FN);
         $BOOL_RESULT = true; // display disable
         is_file($l_FN) && QCR_ScanFile($l_FN, $i);
         $BOOL_RESULT = false; // display enable
      }

      $g_FoundTotalDirs = $g_TotalFolder;
      $g_FoundTotalFiles = $g_TotalFiles;

   } else {
      // scan whole file system
      stdOut("Start scanning '" . ROOT_PATH . "'.\n");
      
      file_exists(QUEUE_FILENAME) && unlink(QUEUE_FILENAME);
      if (ICHECK || IMAKE) {
      // INTEGRITY CHECK
        IMAKE and unlink(INTEGRITY_DB_FILE);
        ICHECK and load_integrity_db();
        QCR_IntegrityCheck(ROOT_PATH);
        stdOut("Found $g_FoundTotalFiles files in $g_FoundTotalDirs directories.");
        if (IMAKE) exit(0);
        if (ICHECK) {
            $i = $g_Counter;
            $g_CRC = 0;
            $changes = array();
            $ref =& $g_IntegrityDB;
            foreach ($g_IntegrityDB as $l_FileName => $type) {
                unset($g_IntegrityDB[$l_FileName]);
                $l_Ext2 = substr(strstr(basename($l_FileName), '.'), 1);
                if (in_array(strtolower($l_Ext2), $g_IgnoredExt)) {
                    continue;
                }
                for ($dr = 0; $dr < count($g_DirIgnoreList); $dr++) {
                    if (($g_DirIgnoreList[$dr] != '') && preg_match('#' . $g_DirIgnoreList[$dr] . '#', $l_FileName, $l_Found)) {
                        continue 2;
                    }
                }
                $type = in_array($type, array('added', 'modified')) ? $type : 'deleted';
                $type .= substr($l_FileName, -1) == '/' ? 'Dirs' : 'Files';
                $changes[$type][] = ++$i;
                AddResult($l_FileName, $i);
            }
            $g_FoundTotalFiles = count($changes['addedFiles']) + count($changes['modifiedFiles']);
            stdOut("Found changes " . count($changes['modifiedFiles']) . " files and added " . count($changes['addedFiles']) . " files.");
        }
        
      } else {
      QCR_ScanDirectories(ROOT_PATH);
      stdOut("Found $g_FoundTotalFiles files in $g_FoundTotalDirs directories.");
      }

      QCR_Debug();
      stdOut(str_repeat(' ', 160),false);
      QCR_GoScan(0);
      unlink(QUEUE_FILENAME);
      if (defined('PROGRESS_LOG_FILE') && file_exists(PROGRESS_LOG_FILE)) @unlink(PROGRESS_LOG_FILE);
   }
}

QCR_Debug();

if (true) {
   $g_HeuristicDetected = array();
   $g_Iframer = array();
   $g_Base64 = array();
}


// whitelist

$snum = 0;
$list = check_whitelist($g_Structure['crc'], $snum);

foreach (array('g_CriticalPHP', 'g_CriticalJS', 'g_Iframer', 'g_Base64', 'g_Phishing', 'g_AdwareList', 'g_Redirect') as $p) {
	if (empty($$p)) continue;
	
	$p_Fragment = $p . "Fragment";
	$p_Sig = $p . "Sig";
	if ($p == 'g_Redirect') $p_Fragment = $p . "PHPFragment";
	if ($p == 'g_Phishing') $p_Sig = $p . "SigFragment";

	$count = count($$p);
	for ($i = 0; $i < $count; $i++) {
		$id = "{${$p}[$i]}";
		if (in_array($g_Structure['crc'][$id], $list)) {
			unset($GLOBALS[$p][$i]);
			unset($GLOBALS[$p_Sig][$i]);
			unset($GLOBALS[$p_Fragment][$i]);
		}
	}

	$$p = array_values($$p);
	$$p_Fragment = array_values($$p_Fragment);
	if (!empty($$p_Sig)) $$p_Sig = array_values($$p_Sig);
}


////////////////////////////////////////////////////////////////////////////
if (AI_HOSTER) {
   $g_IframerFragment = array();
   $g_Iframer = array();
   $g_Redirect = array();
   $g_Doorway = array();
   $g_EmptyLink = array();
   $g_HeuristicType = array();
   $g_HeuristicDetected = array();
   $g_WarningPHP = array();
   $g_AdwareList = array();
   $g_Phishing = array(); 
   $g_PHPCodeInside = array();
   $g_PHPCodeInsideFragment = array();
   //$g_NotRead = array();
   $g_WarningPHPFragment = array();
   $g_WarningPHPSig = array();
   $g_BigFiles = array();
   $g_RedirectPHPFragment = array();
   $g_EmptyLinkSrc = array();
   $g_Base64Fragment = array();
   $g_UnixExec = array();
   $g_PhishingSigFragment = array();
   $g_PhishingFragment = array();
   $g_PhishingSig = array();
   $g_IframerFragment = array();
   $g_CMS = array();
   $g_AdwareListFragment = array(); 
   //$g_Vulnerable = array();
}

 if (BOOL_RESULT && (!defined('NEED_REPORT'))) {
  if ((count($g_CriticalPHP) > 0) OR (count($g_CriticalJS) > 0) OR (count($g_Base64) > 0) OR  (count($g_Iframer) > 0) OR  (count($g_UnixExec) > 0))
  {
  echo "1\n";
  exit(0);
  }
 }
////////////////////////////////////////////////////////////////////////////
$l_Template = str_replace("@@SERVICE_INFO@@", htmlspecialchars("[" . $int_enc . "][" . $snum . "]"), $l_Template);

$l_Template = str_replace("@@PATH_URL@@", (isset($_SERVER['HTTP_HOST']) ? $_SERVER['HTTP_HOST'] : $g_AddPrefix . str_replace($g_NoPrefix, '', addSlash(ROOT_PATH))), $l_Template);

$time_taken = seconds2Human(microtime(true) - START_TIME);

$l_Template = str_replace("@@SCANNED@@", sprintf(AI_STR_013, $g_TotalFolder, $g_TotalFiles), $l_Template);

$l_ShowOffer = false;

stdOut("\nBuilding report [ mode = " . AI_EXPERT . " ]\n");

//stdOut("\nLoaded signatures: " . count($g_FlexDBShe) . " / " . count($g_JSVirSig) . "\n");

////////////////////////////////////////////////////////////////////////////
// save 
if (!(ICHECK || IMAKE))
if (isset($options['with-2check']) || isset($options['quarantine']))
if ((count($g_CriticalPHP) > 0) OR (count($g_CriticalJS) > 0) OR (count($g_Base64) > 0) OR 
   (count($g_Iframer) > 0) OR  (count($g_UnixExec))) 
{
  if (!file_exists(DOUBLECHECK_FILE)) {	  
      if ($l_FH = fopen(DOUBLECHECK_FILE, 'w')) {
         fputs($l_FH, '<?php die("Forbidden"); ?>' . "\n");

         $l_CurrPath = dirname(__FILE__);
		 
		 if (!isset($g_CriticalPHP)) { $g_CriticalPHP = array(); }
		 if (!isset($g_CriticalJS)) { $g_CriticalJS = array(); }
		 if (!isset($g_Iframer)) { $g_Iframer = array(); }
		 if (!isset($g_Base64)) { $g_Base64 = array(); }
		 if (!isset($g_Phishing)) { $g_Phishing = array(); }
		 if (!isset($g_AdwareList)) { $g_AdwareList = array(); }
		 if (!isset($g_Redirect)) { $g_Redirect = array(); }
		 
         $tmpIndex = array_merge($g_CriticalPHP, $g_CriticalJS, $g_Phishing, $g_Base64, $g_Iframer, $g_AdwareList, $g_Redirect);
         $tmpIndex = array_values(array_unique($tmpIndex));

         for ($i = 0; $i < count($tmpIndex); $i++) {
             $tmpIndex[$i] = str_replace($l_CurrPath, '.', $g_Structure['n'][$tmpIndex[$i]]);
         }

         for ($i = 0; $i < count($g_UnixExec); $i++) {
             $tmpIndex[] = str_replace($l_CurrPath, '.', $g_UnixExec[$i]);
         }

         $tmpIndex = array_values(array_unique($tmpIndex));

         for ($i = 0; $i < count($tmpIndex); $i++) {
             fputs($l_FH, $tmpIndex[$i] . "\n");
         }

         fclose($l_FH);
      } else {
         stdOut("Error! Cannot create " . DOUBLECHECK_FILE);
      }      
  } else {
      stdOut(DOUBLECHECK_FILE . ' already exists.');
      if (AI_STR_044 != '') $l_Result .= '<div class="rep">' . AI_STR_044 . '</div>';
  }
 
}

////////////////////////////////////////////////////////////////////////////

$l_Summary = '<div class="title">' . AI_STR_074 . '</div>';
$l_Summary .= '<table cellspacing=0 border=0>';

if (count($g_Redirect) > 0) {
   $l_Summary .= makeSummary(AI_STR_059, count($g_Redirect), "crit");
}

if (count($g_CriticalPHP) > 0) {
   $l_Summary .= makeSummary(AI_STR_060, count($g_CriticalPHP), "crit");
}

if (count($g_CriticalJS) > 0) {
   $l_Summary .= makeSummary(AI_STR_061, count($g_CriticalJS), "crit");
}

if (count($g_Phishing) > 0) {
   $l_Summary .= makeSummary(AI_STR_062, count($g_Phishing), "crit");
}

if (count($g_UnixExec) > 0) {
   $l_Summary .= makeSummary(AI_STR_063, count($g_UnixExec), (AI_EXPERT > 1 ? 'crit' : 'warn'));
}

if (count($g_Iframer) > 0) {
   $l_Summary .= makeSummary(AI_STR_064, count($g_Iframer), "crit");
}

if (count($g_NotRead) > 0) {
   $l_Summary .= makeSummary(AI_STR_066, count($g_NotRead), "crit");
}

if (count($g_Base64) > 0) {
   $l_Summary .= makeSummary(AI_STR_067, count($g_Base64), (AI_EXPERT > 1 ? 'crit' : 'warn'));
}

if (count($g_BigFiles) > 0) {
   $l_Summary .= makeSummary(AI_STR_065, count($g_BigFiles), "warn");
}

if (count($g_HeuristicDetected) > 0) {
   $l_Summary .= makeSummary(AI_STR_068, count($g_HeuristicDetected), "warn");
}

if (count($g_SymLinks) > 0) {
   $l_Summary .= makeSummary(AI_STR_069, count($g_SymLinks), "warn");
}

if (count($g_HiddenFiles) > 0) {
   $l_Summary .= makeSummary(AI_STR_070, count($g_HiddenFiles), "warn");
}

if (count($g_AdwareList) > 0) {
   $l_Summary .= makeSummary(AI_STR_072, count($g_AdwareList), "warn");
}

if (count($g_EmptyLink) > 0) {
   $l_Summary .= makeSummary(AI_STR_073, count($g_EmptyLink), "warn");
}

 $l_Summary .= "</table>";

$l_ArraySummary = array();
$l_ArraySummary["redirect"] = count($g_Redirect);
$l_ArraySummary["critical_php"] = count($g_CriticalPHP);
$l_ArraySummary["critical_js"] = count($g_CriticalJS);
$l_ArraySummary["phishing"] = count($g_Phishing);
$l_ArraySummary["unix_exec"] = count($g_UnixExec);
$l_ArraySummary["iframes"] = count($g_Iframer);
$l_ArraySummary["not_read"] = count($g_NotRead);
$l_ArraySummary["base64"] = count($g_Base64);
$l_ArraySummary["heuristics"] = count($g_HeuristicDetected);
$l_ArraySummary["symlinks"] = count($g_SymLinks);
$l_ArraySummary["big_files_skipped"] = count($g_BigFiles);

 if (function_exists('json_encode')) { $l_Summary .= "<!--[json]" . json_encode($l_ArraySummary) . "[/json]-->"; }

 $l_Summary .= "<div class=details style=\"margin: 20px 20px 20px 0\">" . AI_STR_080 . "</div>\n";

 $l_Template = str_replace("@@SUMMARY@@", $l_Summary, $l_Template);


 $l_Result .= AI_STR_015;
 
 $l_Template = str_replace("@@VERSION@@", AI_VERSION, $l_Template);
 
////////////////////////////////////////////////////////////////////////////



if (function_exists("gethostname") && is_callable("gethostname")) {
  $l_HostName = gethostname();
} else {
  $l_HostName = '???';
}

$l_PlainResult = "# Malware list detected by AI-Bolit (https://revisium.com/ai/) on " . date("d/m/Y H:i:s", time()) . " " . $l_HostName .  "\n\n";

$l_RawReport = array();

$l_RawReport['summary'] = array(
  'scan_path' => $defaults['path'],
  'report_time' => time(),
  'scan_time' => round(microtime(true) - START_TIME, 1),
  'total_files' => $g_FoundTotalFiles,
  'counters' => $l_ArraySummary,
  'ai_version' => AI_VERSION,
);

if (!AI_HOSTER) {
   stdOut("Building list of vulnerable scripts " . count($g_Vulnerable));

   if (count($g_Vulnerable) > 0) {
       $l_Result .= '<div class="note_vir">' . AI_STR_081 . ' (' . count($g_Vulnerable) . ')</div><div class="crit">';
    	foreach ($g_Vulnerable as $l_Item) {
   	    $l_Result .= '<li>' . makeSafeFn($g_Structure['n'][$l_Item['ndx']], true) . ' - ' . $l_Item['id'] . '</li>';
               $l_PlainResult .= '[VULNERABILITY] ' . replacePathArray($g_Structure['n'][$l_Item['ndx']]) . ' - ' . $l_Item['id'] . "\n";
    	}
   	
     $l_Result .= '</div><p>' . PHP_EOL;
     $l_PlainResult .= "\n";
   }
}


stdOut("Building list of shells " . count($g_CriticalPHP));

$l_RawReport['vulners'] = getRawJsonVuln($g_Vulnerable);

if (count($g_CriticalPHP) > 0) {
  $g_CriticalPHP = array_slice($g_CriticalPHP, 0, 15000);
  $l_RawReport['php_malware'] = getRawJson($g_CriticalPHP, $g_CriticalPHPFragment, $g_CriticalPHPSig);
  $l_Result .= '<div class="note_vir">' . AI_STR_016 . ' (' . count($g_CriticalPHP) . ')</div><div class="crit">';
  $l_Result .= printList($g_CriticalPHP, $g_CriticalPHPFragment, true, $g_CriticalPHPSig, 'table_crit');
  $l_PlainResult .= '[SERVER MALWARE]' . "\n" . printPlainList($g_CriticalPHP, $g_CriticalPHPFragment, true, $g_CriticalPHPSig, 'table_crit') . "\n";
  $l_Result .= '</div>' . PHP_EOL;

  $l_ShowOffer = true;
} else {
  $l_Result .= '<div class="ok"><b>' . AI_STR_017. '</b></div>';
}

stdOut("Building list of js " . count($g_CriticalJS));

if (count($g_CriticalJS) > 0) {
  $g_CriticalJS = array_slice($g_CriticalJS, 0, 15000);
  $l_RawReport['js_malware'] = getRawJson($g_CriticalJS, $g_CriticalJSFragment, $g_CriticalJSSig);
  $l_Result .= '<div class="note_vir">' . AI_STR_018 . ' (' . count($g_CriticalJS) . ')</div><div class="crit">';
  $l_Result .= printList($g_CriticalJS, $g_CriticalJSFragment, true, $g_CriticalJSSig, 'table_vir');
  $l_PlainResult .= '[CLIENT MALWARE / JS]'  . "\n" . printPlainList($g_CriticalJS, $g_CriticalJSFragment, true, $g_CriticalJSSig, 'table_vir') . "\n";
  $l_Result .= "</div>" . PHP_EOL;

  $l_ShowOffer = true;
}

stdOut("Building list of unread files " . count($g_NotRead));

if (count($g_NotRead) > 0) {
   $g_NotRead = array_slice($g_NotRead, 0, AIBOLIT_MAX_NUMBER);
   $l_RawReport['not_read'] = $g_NotRead;
   $l_Result .= '<div class="note_vir">' . AI_STR_030 . ' (' . count($g_NotRead) . ')</div><div class="crit">';
   $l_Result .= printList($g_NotRead);
   $l_Result .= "</div><div class=\"spacer\"></div>" . PHP_EOL;
   $l_PlainResult .= '[SCAN ERROR / SKIPPED]' . "\n" . printPlainList($g_NotRead) . "\n\n";
}

if (!AI_HOSTER) {
   stdOut("Building phishing pages " . count($g_Phishing));

   if (count($g_Phishing) > 0) {
     $l_RawReport['phishing'] = getRawJson($g_Phishing, $g_PhishingFragment, $g_PhishingSigFragment);
     $l_Result .= '<div class="note_vir">' . AI_STR_058 . ' (' . count($g_Phishing) . ')</div><div class="crit">';
     $l_Result .= printList($g_Phishing, $g_PhishingFragment, true, $g_PhishingSigFragment, 'table_vir');
     $l_PlainResult .= '[PHISHING]'  . "\n" . printPlainList($g_Phishing, $g_PhishingFragment, true, $g_PhishingSigFragment, 'table_vir') . "\n";
     $l_Result .= "</div>". PHP_EOL;

     $l_ShowOffer = true;
   }

   stdOut("Building list of iframes " . count($g_Iframer));

   if (count($g_Iframer) > 0) {
     $l_RawReport['iframer'] = getRawJson($g_Iframer, $g_IframerFragment);
     $l_ShowOffer = true;
     $l_Result .= '<div class="note_vir">' . AI_STR_021 . ' (' . count($g_Iframer) . ')</div><div class="crit">';
     $l_Result .= printList($g_Iframer, $g_IframerFragment, true);
     $l_Result .= "</div>" . PHP_EOL;
   }

   stdOut("Building list of base64s " . count($g_Base64));

   if (count($g_Base64) > 0) {
     $l_RawReport['warn_enc'] = getRawJson($g_Base64, $g_Base64Fragment);
     if (AI_EXPERT > 1) $l_ShowOffer = true;
     
     $l_Result .= '<div class="note_' . (AI_EXPERT > 1 ? 'vir' : 'warn') . '">' . AI_STR_020 . ' (' . count($g_Base64) . ')</div><div class="' . (AI_EXPERT > 1 ? 'crit' : 'warn') . '">';
     $l_Result .= printList($g_Base64, $g_Base64Fragment, true);
     $l_PlainResult .= '[ENCODED / SUSP_EXT]' . "\n" . printPlainList($g_Base64, $g_Base64Fragment, true) . "\n";
     $l_Result .= "</div>" . PHP_EOL;

   }

   stdOut("Building list of redirects " . count($g_Redirect));
   if (count($g_Redirect) > 0) {
     $l_RawReport['redirect'] = getRawJson($g_Redirect, $g_RedirectPHPFragment);
     $l_ShowOffer = true;
     $l_Result .= '<div class="note_vir">' . AI_STR_027 . ' (' . count($g_Redirect) . ')</div><div class="crit">';
     $l_Result .= printList($g_Redirect, $g_RedirectPHPFragment, true);
     $l_Result .= "</div>" . PHP_EOL;
   }

   stdOut("Building list of symlinks " . count($g_SymLinks));

   if (count($g_SymLinks) > 0) {
     $g_SymLinks = array_slice($g_SymLinks, 0, AIBOLIT_MAX_NUMBER);
     $l_RawReport['sym_links'] = $g_SymLinks;
     $l_Result .= '<div class="note_vir">' . AI_STR_022 . ' (' . count($g_SymLinks) . ')</div><div class="crit">';
     $l_Result .= nl2br(makeSafeFn(implode("\n", $g_SymLinks), true));
     $l_Result .= "</div><div class=\"spacer\"></div>";
   }

   stdOut("Building list of unix executables and odd scripts " . count($g_UnixExec));

   if (count($g_UnixExec) > 0) {
     $g_UnixExec = array_slice($g_UnixExec, 0, AIBOLIT_MAX_NUMBER);
     $l_RawReport['unix_exec'] = $g_UnixExec;
     $l_Result .= '<div class="note_' . (AI_EXPERT > 1 ? 'vir' : 'warn') . '">' . AI_STR_019 . ' (' . count($g_UnixExec) . ')</div><div class="' . (AI_EXPERT > 1 ? 'crit' : 'warn') . '">';
     $l_Result .= nl2br(makeSafeFn(implode("\n", $g_UnixExec), true));
     $l_PlainResult .= '[UNIX EXEC]' . "\n" . implode("\n", replacePathArray($g_UnixExec)) . "\n\n";
     $l_Result .= "</div>" . PHP_EOL;

     if (AI_EXPERT > 1) $l_ShowOffer = true;
   }
}

////////////////////////////////////
if (!AI_HOSTER) {
   $l_WarningsNum = count($g_HeuristicDetected) + count($g_HiddenFiles) + count($g_BigFiles) + count($g_PHPCodeInside) + count($g_AdwareList) + count($g_EmptyLink) + count($g_Doorway) + (count($g_WarningPHP[0]) + count($g_WarningPHP[1]) + count($g_SkippedFolders));

   if ($l_WarningsNum > 0) {
   	$l_Result .= "<div style=\"margin-top: 20px\" class=\"title\">" . AI_STR_026 . "</div>";
   }

   stdOut("Building list of links/adware " . count($g_AdwareList));

   if (count($g_AdwareList) > 0) {
     $l_RawReport['adware'] = getRawJson($g_AdwareList, $g_AdwareListFragment);
     $l_Result .= '<div class="note_warn">' . AI_STR_029 . '</div><div class="warn">';
     $l_Result .= printList($g_AdwareList, $g_AdwareListFragment, true);
     $l_PlainResult .= '[ADWARE]' . "\n" . printPlainList($g_AdwareList, $g_AdwareListFragment, true) . "\n";
     $l_Result .= "</div>" . PHP_EOL;

   }

   stdOut("Building list of heuristics " . count($g_HeuristicDetected));

   if (count($g_HeuristicDetected) > 0) {
     $l_RawReport['heuristic'] = $g_HeuristicDetected;
     $l_Result .= '<div class="note_warn">' . AI_STR_052 . ' (' . count($g_HeuristicDetected) . ')</div><div class="warn">';
     for ($i = 0; $i < count($g_HeuristicDetected); $i++) {
   	   $l_Result .= '<li>' . makeSafeFn($g_Structure['n'][$g_HeuristicDetected[$i]], true) . ' (' . get_descr_heur($g_HeuristicType[$i]) . ')</li>';
     }
     
     $l_Result .= '</ul></div><div class=\"spacer\"></div>' . PHP_EOL;
   }

   stdOut("Building list of hidden files " . count($g_HiddenFiles));
   if (count($g_HiddenFiles) > 0) {
     $g_HiddenFiles = array_slice($g_HiddenFiles, 0, AIBOLIT_MAX_NUMBER);
     $l_RawReport['hidden'] = $g_HiddenFiles;
     $l_Result .= '<div class="note_warn">' . AI_STR_023 . ' (' . count($g_HiddenFiles) . ')</div><div class="warn">';
     $l_Result .= nl2br(makeSafeFn(implode("\n", $g_HiddenFiles), true));
     $l_Result .= "</div><div class=\"spacer\"></div>" . PHP_EOL;
     $l_PlainResult .= '[HIDDEN]' . "\n" . implode("\n", replacePathArray($g_HiddenFiles)) . "\n\n";
   }

   stdOut("Building list of bigfiles " . count($g_BigFiles));
   $max_size_to_scan = getBytes(MAX_SIZE_TO_SCAN);
   $max_size_to_scan = $max_size_to_scan > 0 ? $max_size_to_scan : getBytes('1m');

   if (count($g_BigFiles) > 0) {
     $g_BigFiles = array_slice($g_BigFiles, 0, AIBOLIT_MAX_NUMBER);
     $l_RawReport['big_files'] = getRawJson($g_BigFiles);
     $l_Result .= "<div class=\"note_warn\">" . sprintf(AI_STR_038, bytes2Human($max_size_to_scan)) . '</div><div class="warn">';
     $l_Result .= printList($g_BigFiles);
     $l_Result .= "</div>";
     $l_PlainResult .= '[BIG FILES / SKIPPED]' . "\n" . printPlainList($g_BigFiles) . "\n\n";
   } 

   stdOut("Building list of php inj " . count($g_PHPCodeInside));

   if ((count($g_PHPCodeInside) > 0) && (($defaults['report_mask'] & REPORT_MASK_PHPSIGN) == REPORT_MASK_PHPSIGN)) {
     $l_Result .= '<div class="note_warn">' . AI_STR_028 . '</div><div class="warn">';
     $l_Result .= printList($g_PHPCodeInside, $g_PHPCodeInsideFragment, true);
     $l_Result .= "</div>" . PHP_EOL;

   }

   stdOut("Building list of empty links " . count($g_EmptyLink));
   if (count($g_EmptyLink) > 0) {
     $g_EmptyLink = array_slice($g_EmptyLink, 0, AIBOLIT_MAX_NUMBER);
     $l_Result .= '<div class="note_warn">' . AI_STR_031 . '</div><div class="warn">';
     $l_Result .= printList($g_EmptyLink, '', true);

     $l_Result .= AI_STR_032 . '<br/>';
     
     if (count($g_EmptyLink) == MAX_EXT_LINKS) {
         $l_Result .= '(' . AI_STR_033 . MAX_EXT_LINKS . ')<br/>';
       }
      
     for ($i = 0; $i < count($g_EmptyLink); $i++) {
   	$l_Idx = $g_EmptyLink[$i];
       for ($j = 0; $j < count($g_EmptyLinkSrc[$l_Idx]); $j++) {
         $l_Result .= '<span class="details">' . makeSafeFn($g_Structure['n'][$g_EmptyLink[$i]], true) . ' &rarr; ' . htmlspecialchars($g_EmptyLinkSrc[$l_Idx][$j]) . '</span><br/>';
   	}
     }

     $l_Result .= "</div>";

   }

   stdOut("Building list of doorways " . count($g_Doorway));

   if ((count($g_Doorway) > 0) && (($defaults['report_mask'] & REPORT_MASK_DOORWAYS) == REPORT_MASK_DOORWAYS)) {
     $g_Doorway = array_slice($g_Doorway, 0, AIBOLIT_MAX_NUMBER);
     $l_RawReport['doorway'] = getRawJson($g_Doorway);
     $l_Result .= '<div class="note_warn">' . AI_STR_034 . '</div><div class="warn">';
     $l_Result .= printList($g_Doorway);
     $l_Result .= "</div>" . PHP_EOL;

   }

   stdOut("Building list of php warnings " . (count($g_WarningPHP[0]) + count($g_WarningPHP[1])));

   if (($defaults['report_mask'] & REPORT_MASK_SUSP) == REPORT_MASK_SUSP) {
      if ((count($g_WarningPHP[0]) + count($g_WarningPHP[1])) > 0) {
        $g_WarningPHP[0] = array_slice($g_WarningPHP[0], 0, AIBOLIT_MAX_NUMBER);
        $g_WarningPHP[1] = array_slice($g_WarningPHP[1], 0, AIBOLIT_MAX_NUMBER);
        $l_Result .= '<div class="note_warn">' . AI_STR_035 . '</div><div class="warn">';

        for ($i = 0; $i < count($g_WarningPHP); $i++) {
            if (count($g_WarningPHP[$i]) > 0) 
               $l_Result .= printList($g_WarningPHP[$i], $g_WarningPHPFragment[$i], true, $g_WarningPHPSig, 'table_warn' . $i);
        }                                                                                                                    
        $l_Result .= "</div>" . PHP_EOL;

      } 
   }

   stdOut("Building list of skipped dirs " . count($g_SkippedFolders));
   if (count($g_SkippedFolders) > 0) {
        $l_Result .= '<div class="note_warn">' . AI_STR_036 . '</div><div class="warn">';
        $l_Result .= nl2br(makeSafeFn(implode("\n", $g_SkippedFolders), true));   
        $l_Result .= "</div>" . PHP_EOL;
    }

    if (count($g_CMS) > 0) {
         $l_RawReport['cms'] = $g_CMS;
         $l_Result .= "<div class=\"note_warn\">" . AI_STR_037 . "<br/>";
         $l_Result .= nl2br(makeSafeFn(implode("\n", $g_CMS)));
         $l_Result .= "</div>";
    }
}

if (ICHECK) {
	$l_Result .= "<div style=\"margin-top: 20px\" class=\"title\">" . AI_STR_087 . "</div>";
	
    stdOut("Building list of added files " . count($changes['addedFiles']));
    if (count($changes['addedFiles']) > 0) {
      $l_Result .= '<div class="note_int">' . AI_STR_082 . ' (' . count($changes['addedFiles']) . ')</div><div class="intitem">';
      $l_Result .= printList($changes['addedFiles']);
      $l_Result .= "</div>" . PHP_EOL;
    }

    stdOut("Building list of modified files " . count($changes['modifiedFiles']));
    if (count($changes['modifiedFiles']) > 0) {
      $l_Result .= '<div class="note_int">' . AI_STR_083 . ' (' . count($changes['modifiedFiles']) . ')</div><div class="intitem">';
      $l_Result .= printList($changes['modifiedFiles']);
      $l_Result .= "</div>" . PHP_EOL;
    }

    stdOut("Building list of deleted files " . count($changes['deletedFiles']));
    if (count($changes['deletedFiles']) > 0) {
      $l_Result .= '<div class="note_int">' . AI_STR_084 . ' (' . count($changes['deletedFiles']) . ')</div><div class="intitem">';
      $l_Result .= printList($changes['deletedFiles']);
      $l_Result .= "</div>" . PHP_EOL;
    }

    stdOut("Building list of added dirs " . count($changes['addedDirs']));
    if (count($changes['addedDirs']) > 0) {
      $l_Result .= '<div class="note_int">' . AI_STR_085 . ' (' . count($changes['addedDirs']) . ')</div><div class="intitem">';
      $l_Result .= printList($changes['addedDirs']);
      $l_Result .= "</div>" . PHP_EOL;
    }

    stdOut("Building list of deleted dirs " . count($changes['deletedDirs']));
    if (count($changes['deletedDirs']) > 0) {
      $l_Result .= '<div class="note_int">' . AI_STR_086 . ' (' . count($changes['deletedDirs']) . ')</div><div class="intitem">';
      $l_Result .= printList($changes['deletedDirs']);
      $l_Result .= "</div>" . PHP_EOL;
    }
}

if (!isCli()) {
   $l_Result .= QCR_ExtractInfo($l_PhpInfoBody[1]);
}


if (function_exists('memory_get_peak_usage')) {
  $l_Template = str_replace("@@MEMORY@@", AI_STR_043 . bytes2Human(memory_get_peak_usage()), $l_Template);
}

$l_Template = str_replace('@@WARN_QUICK@@', ((SCAN_ALL_FILES || $g_SpecificExt) ? '' : AI_STR_045), $l_Template);

if ($l_ShowOffer) {
	$l_Template = str_replace('@@OFFER@@', $l_Offer, $l_Template);
} else {
	$l_Template = str_replace('@@OFFER@@', AI_STR_002, $l_Template);
}

$l_Template = str_replace('@@OFFER2@@', $l_Offer2, $l_Template);

$l_Template = str_replace('@@CAUTION@@', AI_STR_003, $l_Template);

$l_Template = str_replace('@@CREDITS@@', AI_STR_075, $l_Template);

$l_Template = str_replace('@@FOOTER@@', AI_STR_076, $l_Template);

$l_Template = str_replace('@@STAT@@', sprintf(AI_STR_012, $time_taken, date('d-m-Y в H:i:s', floor(START_TIME)) , date('d-m-Y в H:i:s')), $l_Template);

////////////////////////////////////////////////////////////////////////////
$l_Template = str_replace("@@MAIN_CONTENT@@", $l_Result, $l_Template);

if (!isCli())
{
    echo $l_Template;
    exit;
}

if (!defined('REPORT') OR REPORT === '')
{
	die2('Report not written.');
}
 
// write plain text result
if (PLAIN_FILE != '') {
	
    $l_PlainResult = preg_replace('|__AI_LINE1__|smi', '[', $l_PlainResult);
    $l_PlainResult = preg_replace('|__AI_LINE2__|smi', '] ', $l_PlainResult);
    $l_PlainResult = preg_replace('|__AI_MARKER__|smi', ' %> ', $l_PlainResult);

   if ($l_FH = fopen(PLAIN_FILE, "w")) {
      fputs($l_FH, $l_PlainResult);
      fclose($l_FH);
   }
}

// write json result
if (defined('JSON_FILE')) {	
   if ($l_FH = fopen(JSON_FILE, "w")) {
      fputs($l_FH, json_encode($l_RawReport));
      fclose($l_FH);
   }
}

// write serialized result
if (defined('PHP_FILE')) {	
   if ($l_FH = fopen(PHP_FILE, "w")) {
      fputs($l_FH, serialize($l_RawReport));
      fclose($l_FH);
   }
}

$emails = getEmails(REPORT);

if (!$emails) {
	if ($l_FH = fopen($file, "w")) {
	   fputs($l_FH, $l_Template);
	   fclose($l_FH);
	   stdOut("\nReport written to '$file'.");
	} else {
		stdOut("\nCannot create '$file'.");
	}
}	else	{
		$headers = array(
			'MIME-Version: 1.0',
			'Content-type: text/html; charset=UTF-8',
			'From: ' . ($defaults['email_from'] ? $defaults['email_from'] : 'AI-Bolit@myhost')
		);

		for ($i = 0, $size = sizeof($emails); $i < $size; $i++)
		{
			mail($emails[$i], 'AI-Bolit Report ' . date("d/m/Y H:i", time()), $l_Result, implode("\r\n", $headers));
		}

		stdOut("\nReport sended to " . implode(', ', $emails));
}


$time_taken = microtime(true) - START_TIME;
$time_taken = number_format($time_taken, 5);


stdOut("Scanning complete! Time taken: " . seconds2Human($time_taken));

if (DEBUG_PERFORMANCE) {
   $keys = array_keys($g_RegExpStat);
   for ($i = 0; $i < count($keys); $i++) {
       $g_RegExpStat[$keys[$i]] = round($g_RegExpStat[$keys[$i]] * 1000000);
   }

   arsort($g_RegExpStat);

   foreach ($g_RegExpStat as $r => $v) {
      echo $v . "\t\t" . $r . "\n";
   }

   die();
}

stdOut("\n\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
stdOut("Attention! DO NOT LEAVE either ai-bolit.php or AI-BOLIT-REPORT-<xxxx>-<yy>.html \nfile on server. COPY it locally then REMOVE from server. ");
stdOut("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

if (isset($options['quarantine'])) {
	Quarantine();
}

if (isset($options['cmd'])) {
	stdOut("Run \"{$options['cmd']}\" ");
	system($options['cmd']);
}

QCR_Debug();

# exit with code

$l_EC1 = count($g_CriticalPHP);
$l_EC2 = count($g_CriticalJS) + count($g_Phishing) + count($g_WarningPHP[0]) + count($g_WarningPHP[1]);
$code = 0;

if ($l_EC1 > 0) {
	$code = 2;
} else {
	if ($l_EC2 > 0) {
		$code = 1;
	}
}

$stat = array('php_malware' => count($g_CriticalPHP), 'js_malware' => count($g_CriticalJS), 'phishing' => count($g_Phishing));

if (function_exists('aibolit_onComplete')) { aibolit_onComplete($code, $stat); }

stdOut('Exit code ' . $code);
exit($code);

############################################# END ###############################################

function Quarantine()
{
	if (!file_exists(DOUBLECHECK_FILE)) {
		return;
	}
	
	$g_QuarantinePass = 'aibolit';
	
	$archive = "AI-QUARANTINE-" .rand(100000, 999999) . ".zip";
	$infoFile = substr($archive, 0, -3) . "txt";
	$report = REPORT_PATH . DIR_SEPARATOR . REPORT_FILE;
	

	foreach (file(DOUBLECHECK_FILE) as $file) {
		$file = trim($file);
		if (!is_file($file)) continue;
	
		$lStat = stat($file);
		
		// skip files over 300KB
		if ($lStat['size'] > 300*1024) continue;

		// http://www.askapache.com/security/chmod-stat.html
		$p = $lStat['mode'];
		$perm ='-';
		$perm.=(($p&0x0100)?'r':'-').(($p&0x0080)?'w':'-');
		$perm.=(($p&0x0040)?(($p&0x0800)?'s':'x'):(($p&0x0800)?'S':'-'));
		$perm.=(($p&0x0020)?'r':'-').(($p&0x0010)?'w':'-');
		$perm.=(($p&0x0008)?(($p&0x0400)?'s':'x'):(($p&0x0400)?'S':'-'));
		$perm.=(($p&0x0004)?'r':'-').(($p&0x0002)?'w':'-');
		$perm.=(($p&0x0001)?(($p&0x0200)?'t':'x'):(($p&0x0200)?'T':'-'));
		
		$owner = (function_exists('posix_getpwuid'))? @posix_getpwuid($lStat['uid']) : array('name' => $lStat['uid']);
		$group = (function_exists('posix_getgrgid'))? @posix_getgrgid($lStat['gid']) : array('name' => $lStat['uid']);

		$inf['permission'][] = $perm;
		$inf['owner'][] = $owner['name'];
		$inf['group'][] = $group['name'];
		$inf['size'][] = $lStat['size'] > 0 ? bytes2Human($lStat['size']) : '-';
		$inf['ctime'][] = $lStat['ctime'] > 0 ? date("d/m/Y H:i:s", $lStat['ctime']) : '-';
		$inf['mtime'][] = $lStat['mtime'] > 0 ? date("d/m/Y H:i:s", $lStat['mtime']) : '-';
		$files[] = strpos($file, './') === 0 ? substr($file, 2) : $file;
	}
	
	// get config files for cleaning
	$configFilesRegex = 'config(uration|\.in[ic])?\.php$|dbconn\.php$';
	$configFiles = preg_grep("~$configFilesRegex~", $files);

	// get columns width
	$width = array();
	foreach (array_keys($inf) as $k) {
		$width[$k] = strlen($k);
		for ($i = 0; $i < count($inf[$k]); ++$i) {
			$len = strlen($inf[$k][$i]);
			if ($len > $width[$k])
				$width[$k] = $len;
		}
	}

	// headings of columns
	$info = '';
	foreach (array_keys($inf) as $k) {
		$info .= str_pad($k, $width[$k], ' ', STR_PAD_LEFT). ' ';
	}
	$info .= "name\n";
	
	for ($i = 0; $i < count($files); ++$i) {
		foreach (array_keys($inf) as $k) {
			$info .= str_pad($inf[$k][$i], $width[$k], ' ', STR_PAD_LEFT). ' ';
		}
		$info .= $files[$i]."\n";
	}
	unset($inf, $width);

	exec("zip -v 2>&1", $output,$code);

	if ($code == 0) {
		$filter = '';
		if ($configFiles && exec("grep -V 2>&1", $output, $code) && $code == 0) {
			$filter = "|grep -v -E '$configFilesRegex'";
		}

		exec("cat AI-BOLIT-DOUBLECHECK.php $filter |zip -@ --password $g_QuarantinePass $archive", $output, $code);
		if ($code == 0) {
			file_put_contents($infoFile, $info);
			$m = array();
			if (!empty($filter)) {
				foreach ($configFiles as $file) {
					$tmp = file_get_contents($file);
					// remove  passwords
					$tmp = preg_replace('~^.*?pass.*~im', '', $tmp);
					// new file name
					$file = preg_replace('~.*/~', '', $file) . '-' . rand(100000, 999999);
					file_put_contents($file, $tmp);
					$m[] = $file;
				}
			}

			exec("zip -j --password $g_QuarantinePass $archive $infoFile $report " . DOUBLECHECK_FILE . ' ' . implode(' ', $m));
			stdOut("\nCreate archive '" . realpath($archive) . "'");
			stdOut("This archive have password '$g_QuarantinePass'");
			foreach ($m as $file) unlink($file);
			unlink($infoFile);
			return;
		}
	}
	
	$zip = new ZipArchive;
	
	if ($zip->open($archive, ZIPARCHIVE::CREATE | ZIPARCHIVE::OVERWRITE) === false) {
		stdOut("Cannot create '$archive'.");
		return;
	}

	foreach ($files as $file) {
		if (in_array($file, $configFiles)) {
			$tmp = file_get_contents($file);
			// remove  passwords
			$tmp = preg_replace('~^.*?pass.*~im', '', $tmp);
			$zip->addFromString($file, $tmp);
		} else {
			$zip->addFile($file);
		}
	}
	$zip->addFile(DOUBLECHECK_FILE, DOUBLECHECK_FILE);
	$zip->addFile($report, REPORT_FILE);
	$zip->addFromString($infoFile, $info);
	$zip->close();

	stdOut("\nCreate archive '" . realpath($archive) . "'.");
	stdOut("This archive has no password!");
}



///////////////////////////////////////////////////////////////////////////
function QCR_IntegrityCheck($l_RootDir)
{
	global $g_Structure, $g_Counter, $g_Doorway, $g_FoundTotalFiles, $g_FoundTotalDirs, 
			$defaults, $g_SkippedFolders, $g_UrlIgnoreList, $g_DirIgnoreList, $g_UnsafeDirArray, 
                        $g_UnsafeFilesFound, $g_SymLinks, $g_HiddenFiles, $g_UnixExec, $g_IgnoredExt, $g_SuspiciousFiles, $l_SkipSample;
	global $g_IntegrityDB, $g_ICheck;
	static $l_Buffer = '';
	
	$l_DirCounter = 0;
	$l_DoorwayFilesCounter = 0;
	$l_SourceDirIndex = $g_Counter - 1;
	
	QCR_Debug('Check ' . $l_RootDir);

 	if ($l_DIRH = @opendir($l_RootDir))
	{
		while (($l_FileName = readdir($l_DIRH)) !== false)
		{
			if ($l_FileName == '.' || $l_FileName == '..') continue;

			$l_FileName = $l_RootDir . DIR_SEPARATOR . $l_FileName;

			$l_Type = filetype($l_FileName);
			$l_IsDir = ($l_Type == "dir");
            if ($l_Type == "link") 
            {
				$g_SymLinks[] = $l_FileName;
                continue;
            } else 
			if ($l_Type != "file" && (!$l_IsDir)) {
				$g_UnixExec[] = $l_FileName;
				continue;
			}	
						
			$l_Ext = substr($l_FileName, strrpos($l_FileName, '.') + 1);

			$l_NeedToScan = true;
			$l_Ext2 = substr(strstr(basename($l_FileName), '.'), 1);
			if (in_array(strtolower($l_Ext2), $g_IgnoredExt)) {
                           $l_NeedToScan = false;
            		}

      			// if folder in ignore list
      			$l_Skip = false;
      			for ($dr = 0; $dr < count($g_DirIgnoreList); $dr++) {
      				if (($g_DirIgnoreList[$dr] != '') &&
      				   preg_match('#' . $g_DirIgnoreList[$dr] . '#', $l_FileName, $l_Found)) {
      				   if (!in_array($g_DirIgnoreList[$dr], $l_SkipSample)) {
                                      $l_SkipSample[] = $g_DirIgnoreList[$dr];
                                   } else {
        		             $l_Skip = true;
                                     $l_NeedToScan = false;
                                   }
      				}
      			}
      					
			if (getRelativePath($l_FileName) == "./" . INTEGRITY_DB_FILE) $l_NeedToScan = false;

			if ($l_IsDir)
			{
				// skip on ignore
				if ($l_Skip) {
				   $g_SkippedFolders[] = $l_FileName;
				   continue;
				}
				
				$l_BaseName = basename($l_FileName);

				$l_DirCounter++;

				$g_Counter++;
				$g_FoundTotalDirs++;

				QCR_IntegrityCheck($l_FileName);

			} else
			{
				if ($l_NeedToScan)
				{
					$g_FoundTotalFiles++;
					$g_Counter++;
				}
			}
			
			if (!$l_NeedToScan) continue;

			if (IMAKE) {
				write_integrity_db_file($l_FileName);
				continue;
			}

			// ICHECK
			// skip if known and not modified.
			if (icheck($l_FileName)) continue;
			
			$l_Buffer .= getRelativePath($l_FileName);
			$l_Buffer .= $l_IsDir ? DIR_SEPARATOR . "\n" : "\n";

			if (strlen($l_Buffer) > 32000)
			{
				file_put_contents(QUEUE_FILENAME, $l_Buffer, FILE_APPEND) or die2("Cannot write to file " . QUEUE_FILENAME);
				$l_Buffer = '';
			}

		}

		closedir($l_DIRH);
	}
	
	if (($l_RootDir == ROOT_PATH) && !empty($l_Buffer)) {
		file_put_contents(QUEUE_FILENAME, $l_Buffer, FILE_APPEND) or die2("Cannot write to file " . QUEUE_FILENAME);
		$l_Buffer = '';
	}

	if (($l_RootDir == ROOT_PATH)) {
		write_integrity_db_file();
	}

}


function getRelativePath($l_FileName) {
	return "./" . substr($l_FileName, strlen(ROOT_PATH) + 1) . (is_dir($l_FileName) ? DIR_SEPARATOR : '');
}
/**
 *
 * @return true if known and not modified
 */
function icheck($l_FileName) {
	global $g_IntegrityDB, $g_ICheck;
	static $l_Buffer = '';
	static $l_status = array( 'modified' => 'modified', 'added' => 'added' );
    
	$l_RelativePath = getRelativePath($l_FileName);
	$l_known = isset($g_IntegrityDB[$l_RelativePath]);

	if (is_dir($l_FileName)) {
		if ( $l_known ) {
			unset($g_IntegrityDB[$l_RelativePath]);
		} else {
			$g_IntegrityDB[$l_RelativePath] =& $l_status['added'];
		}
		return $l_known;
	}

	if ($l_known == false) {
		$g_IntegrityDB[$l_RelativePath] =& $l_status['added'];
		return false;
	}

	$hash = is_file($l_FileName) ? hash_file('sha1', $l_FileName) : '';
	
	if ($g_IntegrityDB[$l_RelativePath] != $hash) {
		$g_IntegrityDB[$l_RelativePath] =& $l_status['modified'];
		return false;
	}

	unset($g_IntegrityDB[$l_RelativePath]);
	return true;
}

function write_integrity_db_file($l_FileName = '') {
	static $l_Buffer = '';

	if (empty($l_FileName)) {
		empty($l_Buffer) or file_put_contents('compress.zlib://' . INTEGRITY_DB_FILE, $l_Buffer, FILE_APPEND) or die2("Cannot write to file " . INTEGRITY_DB_FILE);
		$l_Buffer = '';
		return;
	}

	$l_RelativePath = getRelativePath($l_FileName);
		
	$hash = is_file($l_FileName) ? hash_file('sha1', $l_FileName) : '';

	$l_Buffer .= "$l_RelativePath|$hash\n";
	
	if (strlen($l_Buffer) > 32000)
	{
		file_put_contents('compress.zlib://' . INTEGRITY_DB_FILE, $l_Buffer, FILE_APPEND) or die2("Cannot write to file " . INTEGRITY_DB_FILE);
		$l_Buffer = '';
	}
}

function load_integrity_db() {
	global $g_IntegrityDB;
	file_exists(INTEGRITY_DB_FILE) or die2('Not found ' . INTEGRITY_DB_FILE);

	$s_file = new SplFileObject('compress.zlib://'.INTEGRITY_DB_FILE);
	$s_file->setFlags(SplFileObject::READ_AHEAD | SplFileObject::SKIP_EMPTY | SplFileObject::DROP_NEW_LINE);

	foreach ($s_file as $line) {
		$i = strrpos($line, '|');
		if (!$i) continue;
		$g_IntegrityDB[substr($line, 0, $i)] = substr($line, $i+1);
	}

	$s_file = null;
}


function OptimizeSignatures()
{
	global $g_DBShe, $g_FlexDBShe, $gX_FlexDBShe, $gXX_FlexDBShe;
	global $g_JSVirSig, $gX_JSVirSig;
	global $g_AdwareSig;
	global $g_PhishingSig;
	global $g_ExceptFlex, $g_SusDBPrio, $g_SusDB;

	(AI_EXPERT == 2) && ($g_FlexDBShe = array_merge($g_FlexDBShe, $gX_FlexDBShe, $gXX_FlexDBShe));
	(AI_EXPERT == 1) && ($g_FlexDBShe = array_merge($g_FlexDBShe, $gX_FlexDBShe));
	$gX_FlexDBShe = $gXX_FlexDBShe = array();

	(AI_EXPERT == 2) && ($g_JSVirSig = array_merge($g_JSVirSig, $gX_JSVirSig));
	$gX_JSVirSig = array();

	$count = count($g_FlexDBShe);

	for ($i = 0; $i < $count; $i++) {
		if ($g_FlexDBShe[$i] == '[a-zA-Z0-9_]+?\(\s*[a-zA-Z0-9_]+?=\s*\)') $g_FlexDBShe[$i] = '\((?<=[a-zA-Z0-9_].)\s*[a-zA-Z0-9_]++=\s*\)';
		if ($g_FlexDBShe[$i] == '([^\?\s])\({0,1}\.[\+\*]\){0,1}\2[a-z]*e') $g_FlexDBShe[$i] = '(?J)\.[+*](?<=(?<d>[^\?\s])\(..|(?<d>[^\?\s])..)\)?\g{d}[a-z]*e';
		if ($g_FlexDBShe[$i] == '$[a-zA-Z0-9_]\{\d+\}\s*\.$[a-zA-Z0-9_]\{\d+\}\s*\.$[a-zA-Z0-9_]\{\d+\}\s*\.') $g_FlexDBShe[$i] = '\$[a-zA-Z0-9_]\{\d+\}\s*\.\$[a-zA-Z0-9_]\{\d+\}\s*\.\$[a-zA-Z0-9_]\{\d+\}\s*\.';

		$g_FlexDBShe[$i] = str_replace('http://.+?/.+?\.php\?a', 'http://[^?\s]++(?<=\.php)\?a', $g_FlexDBShe[$i]);
		$g_FlexDBShe[$i] = preg_replace('~\[a-zA-Z0-9_\]\+\K\?~', '+', $g_FlexDBShe[$i]);
		$g_FlexDBShe[$i] = preg_replace('~^\\\\[d]\+&@~', '&@(?<=\d..)', $g_FlexDBShe[$i]);
		$g_FlexDBShe[$i] = str_replace('\s*[\'"]{0,1}.+?[\'"]{0,1}\s*', '.+?', $g_FlexDBShe[$i]);
		$g_FlexDBShe[$i] = str_replace('[\'"]{0,1}.+?[\'"]{0,1}', '.+?', $g_FlexDBShe[$i]);

		$g_FlexDBShe[$i] = preg_replace('~^\[\'"\]\{0,1\}\.?|^@\*|^\\\\s\*~', '', $g_FlexDBShe[$i]);
		$g_FlexDBShe[$i] = preg_replace('~^\[\'"\]\{0,1\}\.?|^@\*|^\\\\s\*~', '', $g_FlexDBShe[$i]);
	}

	optSig($g_FlexDBShe);

	optSig($g_JSVirSig);
	optSig($g_AdwareSig);
	optSig($g_PhishingSig);
        optSig($g_SusDB);
        //optSig($g_SusDBPrio);
        //optSig($g_ExceptFlex);

        // convert exception rules
        $cnt = count($g_ExceptFlex);
        for ($i = 0; $i < $cnt; $i++) {                		
            $g_ExceptFlex[$i] = trim(UnwrapObfu($g_ExceptFlex[$i]));
            if (!strlen($g_ExceptFlex[$i])) unset($g_ExceptFlex[$i]);
        }

        $g_ExceptFlex = array_values($g_ExceptFlex);
}

function optSig(&$sigs)
{
	$sigs = array_unique($sigs);

	// Add SigId
	foreach ($sigs as &$s) {
		$s .= '(?<X' . myCheckSum($s) . '>)';
	}
	unset($s);
	
	$fix = array(
		'([^\?\s])\({0,1}\.[\+\*]\){0,1}\2[a-z]*e' => '(?J)\.[+*](?<=(?<d>[^\?\s])\(..|(?<d>[^\?\s])..)\)?\g{d}[a-z]*e',
		'http://.+?/.+?\.php\?a' => 'http://[^?\s]++(?<=\.php)\?a',
		'\s*[\'"]{0,1}.+?[\'"]{0,1}\s*' => '.+?',
		'[\'"]{0,1}.+?[\'"]{0,1}' => '.+?'
	);

	$sigs = str_replace(array_keys($fix), array_values($fix), $sigs);
	
	$fix = array(
		'~^\\\\[d]\+&@~' => '&@(?<=\d..)',
		'~^((\[\'"\]|\\\\s|@)(\{0,1\}\.?|[?*]))+~' => ''
	);

	$sigs = preg_replace(array_keys($fix), array_values($fix), $sigs);

	optSigCheck($sigs);

	$tmp = array();
	foreach ($sigs as $i => $s) {
		if (!preg_match('#^(?>(?!\.[*+]|\\\\\d)(?:\\\\.|\[.+?\]|.))+$#', $s)) {
			unset($sigs[$i]);
			$tmp[] = $s;
		}
	}
	
	usort($sigs, 'strcasecmp');
	$txt = implode("\n", $sigs);

	for ($i = 24; $i >= 1; ($i > 4 ) ? $i -= 4 : --$i) {
	    $txt = preg_replace_callback('#^((?>(?:\\\\.|\\[.+?\\]|[^(\n]|\((?:\\\\.|[^)(\n])++\))(?:[*?+]\+?|\{\d+(?:,\d*)?\}[+?]?|)){' . $i . ',})[^\n]*+(?:\\n\\1(?![{?*+]).+)+#im', 'optMergePrefixes', $txt);
	}

	$sigs = array_merge(explode("\n", $txt), $tmp);
	
	optSigCheck($sigs);
}

function optMergePrefixes($m)
{
	$limit = 8000;
	
	$prefix = $m[1];
	$prefix_len = strlen($prefix);

	$len = $prefix_len;
	$r = array();

	$suffixes = array();
	foreach (explode("\n", $m[0]) as $line) {
	
	  if (strlen($line)>$limit) {
	    $r[] = $line;
	    continue;
	  }
	
	  $s = substr($line, $prefix_len);
	  $len += strlen($s);
	  if ($len > $limit) {
	    if (count($suffixes) == 1) {
	      $r[] = $prefix . $suffixes[0];
	    } else {
	      $r[] = $prefix . '(?:' . implode('|', $suffixes) . ')';
	    }
	    $suffixes = array();
	    $len = $prefix_len + strlen($s);
	  }
	  $suffixes[] = $s;
	}

	if (!empty($suffixes)) {
	  if (count($suffixes) == 1) {
	    $r[] = $prefix . $suffixes[0];
	  } else {
	    $r[] = $prefix . '(?:' . implode('|', $suffixes) . ')';
	  }
	}
	
	return implode("\n", $r);
}

function optMergePrefixes_Old($m)
{
	$prefix = $m[1];
	$prefix_len = strlen($prefix);

	$suffixes = array();
	foreach (explode("\n", $m[0]) as $line) {
	  $suffixes[] = substr($line, $prefix_len);
	}

	return $prefix . '(?:' . implode('|', $suffixes) . ')';
}

/*
 * Checking errors in pattern
 */
function optSigCheck(&$sigs)
{
	$result = true;

	foreach ($sigs as $k => $sig) {
                if (trim($sig) == "") {
                   if (DEBUG_MODE) {
                      echo("************>>>>> EMPTY\n     pattern: " . $sig . "\n");
                   }
	           unset($sigs[$k]);
		   $result = false;
                }

		if (@preg_match('#' . $sig . '#smiS', '') === false) {
			$error = error_get_last();
                        if (DEBUG_MODE) {
			   echo("************>>>>> " . $error['message'] . "\n     pattern: " . $sig . "\n");
                        }
			unset($sigs[$k]);
			$result = false;
		}
	}
	
	return $result;
}

function _hash_($text)
{
	static $r;
	
	if (empty($r)) {
		for ($i = 0; $i < 256; $i++) {
			if ($i < 33 OR $i > 127 ) $r[chr($i)] = '';
		}
	}

	return sha1(strtr($text, $r));
}

function check_whitelist($list, &$snum) 
{
	if (empty($list)) return array();
	
	$file = dirname(__FILE__) . '/AIBOLIT-WHITELIST.db';

	$snum = max(0, @filesize($file) - 1024) / 20;
	stdOut("\nLoaded " . ceil($snum) . " known files\n");
	
	sort($list);

	$hash = reset($list);
	
	$fp = @fopen($file, 'rb');
	
	if (false === $fp) return array();
	
	$header = unpack('V256', fread($fp, 1024));
	
	$result = array();
	
	foreach ($header as $chunk_id => $chunk_size) {
		if ($chunk_size > 0) {
			$str = fread($fp, $chunk_size);
			
			do {
				$raw = pack("H*", $hash);
				$id = ord($raw[0]) + 1;
				
				if ($chunk_id == $id AND binarySearch($str, $raw)) {
					$result[] = $hash;
				}
				
			} while ($chunk_id >= $id AND $hash = next($list));
			
			if ($hash === false) break;
		}
	}
	
	fclose($fp);

	return $result;
}


function binarySearch($str, $item)
{
	$item_size = strlen($item);	
	if ( $item_size == 0 ) return false;
	
	$first = 0;

	$last = floor(strlen($str) / $item_size);
	
	while ($first < $last) {
		$mid = $first + (($last - $first) >> 1);
		$b = substr($str, $mid * $item_size, $item_size);
		if (strcmp($item, $b) <= 0)
			$last = $mid;
		else
			$first = $mid + 1;
	}

	$b = substr($str, $last * $item_size, $item_size);
	if ($b == $item) {
		return true;
	} else {
		return false;
	}
}

function getSigId($l_Found)
{
	foreach ($l_Found as $key => &$v) {
		if (is_string($key) AND $v[1] != -1 AND strlen($key) == 9) {
			return substr($key, 1);
		}
	}
	
	return null;
}

function die2($str) {
  if (function_exists('aibolit_onFatalError')) { aibolit_onFatalError($str); }
  die($str);
}

function checkFalsePositives($l_Filename, $l_Unwrapped, $l_DeobfType) {
  global $g_DeMapper;

  if ($l_DeobfType != '') {
     if (DEBUG_MODE) {
       stdOut("\n-----------------------------------------------------------------------------\n");
       stdOut("[DEBUG]" . $l_Filename . "\n");
       var_dump(getFragment($l_Unwrapped, $l_Pos));
       stdOut("\n...... $l_DeobfType ...........\n");
       var_dump($l_Unwrapped);
       stdOut("\n");
     }

     switch ($l_DeobfType) {
        case '_GLOBALS_': 
           foreach ($g_DeMapper as $fkey => $fvalue) {
              if (DEBUG_MODE) {
                 stdOut("[$fkey] => [$fvalue]\n");
              }

              if ((strpos($l_Filename, $fkey) !== false) &&
                  (strpos($l_Unwrapped, $fvalue) !== false)) {
                 if (DEBUG_MODE) {
                    stdOut("\n[DEBUG] *** SKIP: False Positive\n");
                 } 

                 return true;
              }
           }
        break;
     }


     return false;
  }
}

function deobfuscate_bitrix($str)
{
	global $varname,$funclist,$strlist;
	$res = $str;
	$funclist = array();
	$strlist = array();
	$res = preg_replace("|'\s*\.\s*'|smi", '', $res);
	$res = preg_replace_callback(
		'|(round\((.+?)\))|smi',
		function ($matches) {
		   return round($matches[2]);
		},
		$res
	);
	$res = preg_replace_callback(
			'|base64_decode\(\'(.*?)\'\)|smi',
			function ($matches) {
				return "'" . base64_decode($matches[1]) . "'";
			},
			$res
	);

	$res = preg_replace_callback(
			'|\'(.*?)\'|sm',
			function ($matches) {
				$temp = base64_decode($matches[1]);
				if (base64_encode($temp) === $matches[1] && preg_match('#^[ -~]*$#', $temp)) { 
				   return "'" . $temp . "'";
				} else {
				   return "'" . $matches[1] . "'";
				}
			},
			$res
	);	

	if (preg_match_all('|\$GLOBALS\[\'(.+?)\'\]\s*=\s*Array\((.+?)\);|smi', $res, $founds, PREG_SET_ORDER)) {
   	foreach($founds as $found)
   	{
   		$varname = $found[1];
   		$funclist[$varname] = explode(',', $found[2]);
   		$funclist[$varname] = array_map(function($value) { return trim($value, "'"); }, $funclist[$varname]);

   		$res = preg_replace_callback(
   				'|\$GLOBALS\[\'' . $varname . '\'\]\[(\d+)\]|smi',
   				function ($matches) {
   				   global $varname, $funclist;
   				   return $funclist[$varname][$matches[1]];
   				},
   				$res
   		);
   		
     	        $res = preg_replace('~' . quotemeta(str_replace('~', '.', $found[0])) . '~smi', '', $res);
   	}
        }
		

	if (preg_match_all('|function _+(\d+)\(\$i\){\$a=Array\((.+?)\);[^}]+}|smi', $res, $founds, PREG_SET_ORDER)) {
	foreach($founds as $found)
	{
		$strlist = explode(',', $found[2]);

		$res = preg_replace_callback(
				'|_' . $found[1] . '\((\d+)\)|smi',
				function ($matches) {
				   global $strlist;
				   return $strlist[$matches[1]];
				},
				$res
		);

  	        $res = preg_replace('~' . quotemeta(str_replace('~', '\\~', $found[0])) . '~smi', '', $res);
	}
        }

  	$res = preg_replace('~<\?(php)?\s*\?>~smi', '', $res);

	preg_match_all('~function (_+(.+?))\(\$[_0-9]+\)\{\s*static\s*\$([_0-9]+)\s*=\s*(true|false);.*?\$\3=array\((.*?)\);\s*return\s*base64_decode\(\$\3~smi', $res, $founds,PREG_SET_ORDER);
	foreach($founds as $found)
	{
		$strlist = explode("',",$found[5]);
		$res = preg_replace_callback(
				'|' . $found[1] . '\((\d+)\)|sm',
				function ($matches) {
				   global $strlist;
				   return $strlist[$matches[1]]."'";
				},
				$res
		);
				
	}

	$res = preg_replace('|;|sm', ";\n", $res);

	return $res;
}

function my_eval($matches)
{
    $string = $matches[0];
    $string = substr($string, 5, strlen($string) - 7);
    return decode($string);
}

function decode($string, $level = 0)
{
    if (trim($string) == '') return '';
    if ($level > 100) return '';

    if (($string[0] == '\'') || ($string[0] == '"')) {
        return substr($string, 1, strlen($string) - 2); //
	} elseif ($string[0] == '$') {
        return $string; //
    } else {
        $pos      = strpos($string, '(');
        $function = substr($string, 0, $pos);
		
        $arg      = decode(substr($string, $pos + 1), $level + 1);
    	if ($function == 'base64_decode') return @base64_decode($arg);
    	else if ($function == 'gzinflate') return @gzinflate($arg);
		else if ($function == 'gzuncompress') return @gzuncompress($arg);
    	else if ($function == 'strrev')  return @strrev($arg);
    	else if ($function == 'str_rot13')  return @str_rot13($arg);
    	else return $arg;
    }    
}
    
function deobfuscate_eval($str)
{
    $res = preg_replace_callback('~eval\((base64_decode|gzinflate|strrev|str_rot13|gzuncompress).*?\);~ms', "my_eval", $str);
    return $res;
}

function getEvalCode($string)
{
    preg_match("/eval\((.*?)\);/", $string, $matches);
    return (empty($matches)) ? '' : end($matches);
}
function getTextInsideQuotes($string)
{
    preg_match('/("(.*?)")/', $string, $matches);
    return (empty($matches)) ? '' : end($matches);
}

function deobfuscate_lockit($str)
{    
    $obfPHP        = $str;
    $phpcode       = base64_decode(getTextInsideQuotes(getEvalCode($obfPHP)));
    $hexvalues     = getHexValues($phpcode);
    $tmp_point     = getHexValues($obfPHP);
    $pointer1      = hexdec($tmp_point[0]);
    $pointer2      = hexdec($hexvalues[0]);
    $pointer3      = hexdec($hexvalues[1]);
    $needles       = getNeedles($phpcode);
    $needle        = $needles[count($needles) - 2];
    $before_needle = end($needles);
    
    
    $phpcode = base64_decode(strtr(substr($obfPHP, $pointer2 + $pointer3, $pointer1), $needle, $before_needle));
    return "<?php {$phpcode} ?>";
}


    function getNeedles($string)
    {
        preg_match_all("/'(.*?)'/", $string, $matches);
        
        return (empty($matches)) ? array() : $matches[1];
    }
    function getHexValues($string)
    {
        preg_match_all('/0x[a-fA-F0-9]{1,8}/', $string, $matches);
        return (empty($matches)) ? array() : $matches[0];
    }

function deobfuscate_als($str)
{
	preg_match('~__FILE__;\$[O0]+=[0-9a-fx]+;eval\(\$[O0]+\(\'([^\']+)\'\)\);return;~msi',$str,$layer1);

	preg_match('~\$[O0]+=(\$[O0]+\()+\$[O0]+,[0-9a-fx]+\),\'([^\']+)\',\'([^\']+)\'\)\);eval\(~msi',base64_decode($layer1[1]),$layer2);
    $res = explode("?>", $str);
	if (strlen($res[1])>0)
	{
		$res = substr($res[1], 380);
		$res = base64_decode(strtr($res, $layer2[2], $layer2[3]));
	}
    return "<?php {$res} ?>";
}

function deobfuscate_byterun($str)
{
	preg_match('~\$_F=__FILE__;\$_X=\'([^\']+)\';eval\(~ms',$str,$matches);
	$res = base64_decode($matches[1]);
	$res = strtr($res,'123456aouie','aouie123456');
    return "<?php {$res} ?>";
}

function deobfuscate_urldecode($str)
{
	preg_match('~(\$[O0_]+)=urldecode\("([%0-9a-f]+)"\);((\$[O0_]+=(\1\{\d+\}\.?)+;)+)~msi',$str,$matches);
	$alph = urldecode($matches[2]);
	$funcs=$matches[3];
	for($i = 0; $i < strlen($alph); $i++)
	{
		$funcs = str_replace($matches[1].'{'.$i.'}.',$alph[$i],$funcs);
		$funcs = str_replace($matches[1].'{'.$i.'}',$alph[$i],$funcs);
	}

	$str = str_replace($matches[3], $funcs, $str);
	$funcs = explode(';', $funcs);
	foreach($funcs as $func)
	{
		$func_arr = explode("=", $func);
		if (count($func_arr) == 2)
		{
			$func_arr[0] = str_replace('$', '', $func_arr[0]);
			$str = str_replace('${"GLOBALS"}["' . $func_arr[0] . '"]', $func_arr[1], $str);
		}			
	}

	return $str;
}


function formatPHP($string)
{
    $string = str_replace('<?php', '', $string);
    $string = str_replace('?>', '', $string);
    $string = str_replace(PHP_EOL, "", $string);
    $string = str_replace(";", ";\n", $string);
    return $string;
}

function deobfuscate_fopo($str)
{
    $phpcode = formatPHP($str);
    $phpcode = base64_decode(getTextInsideQuotes(getEvalCode($phpcode)));
    @$phpcode = gzinflate(base64_decode(str_rot13(getTextInsideQuotes(end(explode(':', $phpcode))))));
    $old = '';
    while (($old != $phpcode) && (strlen(strstr($phpcode, '@eval($')) > 0)) {
        $old = $phpcode;
        $funcs = explode(';', $phpcode);
		if (count($funcs) == 5) $phpcode = gzinflate(base64_decode(str_rot13(getTextInsideQuotes(getEvalCode($phpcode)))));
		else if (count($funcs) == 4) $phpcode = gzinflate(base64_decode(getTextInsideQuotes(getEvalCode($phpcode))));
    }
    
    return substr($phpcode, 2);
}

function getObfuscateType($str)
{
if (preg_match('~eval\((base64_decode|gzinflate|strrev|str_rot13|gzuncompress)~ms', $str))
        return "eval";
    if (preg_match('~\$GLOBALS\[\'_+\d+\'\]=\s*array\(base64_decode\(~msi', $str))
        return "_GLOBALS_";
    if (preg_match('~function _+\d+\(\$i\){\$a=Array~msi', $str))
        return "_GLOBALS_";
    if (preg_match('~__FILE__;\$[O0]+=[0-9a-fx]+;eval\(\$[O0]+\(\'([^\']+)\'\)\);return;~msi', $str))
        return "ALS-Fullsite";
    if (preg_match('~\$[O0]*=urldecode\(\'%66%67%36%73%62%65%68%70%72%61%34%63%6f%5f%74%6e%64\'\);\s*\$GLOBALS\[\'[O0]*\'\]=\$[O0]*~msi', $str))
        return "LockIt!";
    if (preg_match('~\$\w+="(\\\x?[0-9a-f]+){13}";@eval\(\$\w+\(~msi', $str))
        return "FOPO";
	if (preg_match('~\$_F=__FILE__;\$_X=\'([^\']+\');eval\(~ms', $str))
        return "ByteRun";
    if (preg_match('~(\$[O0_]+)=urldecode\("([%0-9a-f]+)"\);((\$[O0_]+=(\1\{\d+\}\.?)+;)+)~msi', $str))
        return "urldecode_globals";
	
}

function deobfuscate($str)
{
    switch (getObfuscateType($str)) {
        case '_GLOBALS_':
            $str = deobfuscate_bitrix($str);
            break;
        case 'eval':
            $str = deobfuscate_eval($str);
            break;
        case 'ALS-Fullsite':
            $str = deobfuscate_als($str);
            break;
        case 'LockIt!':
            $str = deobfuscate_lockit($str);
            break;
        case 'FOPO':
            $str = deobfuscate_fopo($str);
            break;
	case 'ByteRun':
            $str = deobfuscate_byterun($str);
            break;
	case 'urldecode_globals' :
            $str = deobfuscate_urldecode($str);
	    break;
    }
    
    return $str;
}
