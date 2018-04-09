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

//BEGIN_SIG 25/03/2018 10:22:43
$g_DBShe = unserialize(gzinflate(/*1522005763*/base64_decode("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")));
$gX_DBShe = unserialize(gzinflate(/*1522005763*/base64_decode("bVX9b+I4EP1XvDn2oFIpCUmgpNvqKLDtXT9V2L1dVVVkEhOsOB9rO7Swuvvbzx67tFodv5A8j59n3htPcDTwop80ck9ENIyclKxwQrhzQiNPIf0gcr5XDUpwiepGIoyKNERCclpmOqavYo4jp17XYk0Y05CvoEE/cj6ldIMShoU4dZasSnK0lNuaeM7Z+5VUVvUvyFIWzplmCgx5MhrtyUMD8XC4hwYm8dvFFKfpViNDu4+u90HHJujzC5BpZKQQT6X5N1miuQ5DS9jsaSE8TyuxoWw3f2XwQI4wci5xkpNURaMxW+ElkdVXymWDTZQV5NaX+7M93zLOZcWL4RtjYPHrKmnEOzy0qb1WeaTkhQVd6ihycCnpG/vQshRbwMgLSQA/NpmY2u5ywKBoZakO6ji8QF2OuiswEsrWmRPUpJhVAu1wxpsdlbCsqw/VKa2CCIEzgk4R4SSLOamZapiO8zGcfOz3nUPkmL/XwIMT2K918VTuteociZx5jQuSts8+LTlY3dcaBW7kCCKTqsop6SBd0Q8WP5NljNOClnEjCC/VPgdZUi2gr2wlTBC66qyaMpG0KmPyQoUUHQcEiUGRgwPYoaUNVRp0hTpUxAlmDC+ZSt8EIVym6IM6CnOOtxY9bKVU6CjNb3m0E35geDqtmvBCoN+R+zJxXfcAnZ7uH39COHikqkspR727C9T7BrC2yB8oljJhTUo6rXg+e/g6e3hsXy4W9/EX9RaPL2a3i/aTOXZk2njJ3WfhwyX1tW+Bqqi9lrJOj5KqXLUP25t1pRR4fUtWme4h/aQAal5gt2fr6NW8SnpiK3q50piw3hYXuFdLrryNRVLVBMK1i31fCb7BrLOijMQZkbEilaTUgusY3zaSkveZU2nk7W0w78FyYFuwdXF9dz6+nj+2Y/WDJTAnhJ17Y9ragvavxhhUNdk7ZwRwaGOUormrpIB74Gvt+wrKCkxZVxSy7tLyiB1lVZUxoiQqIMxOCNVt3HWh5/2RvVlTPQng2oPkgR2V07dRGXg29BHdr5WUL1v0BLgdB49JlRK+M5gWyNcD8kO3+5suBCVFeuq0wPPxZDK7Xzio2zUjMLCz4OmRV02ZdtyDJ5A5CF+HCi3GUr4lpwXwlIhTzPMJJ8+fOSVlCuIEQ5N43mfM91OAbNlX+ZXn+0OARpbhcjy5mk3R+Xc0X9w93MD4da17N+qKqW+CEQXIQ89uu+FH87Xq6DUuv3mjAaz1zeCaEppijv+F7g99eyn+omWtRlpV7AAOTKxXDv1Eem4JYGjPvVLfnXnOm1pd7MqcO/h/mqGhmZAyo7tLbGiODcj9jV9mQWkIRgY8v1b1frn9E5IbuHbUckmTnJE/nJN//gM=")));
$g_FlexDBShe = unserialize(gzinflate(/*1522005763*/base64_decode("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")));
$gX_FlexDBShe = unserialize(gzinflate(/*1522005763*/base64_decode("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")));
$gXX_FlexDBShe = unserialize(gzinflate(/*1522005763*/base64_decode("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")));
$g_ExceptFlex = unserialize(gzinflate(/*1522005763*/base64_decode("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")));
$g_AdwareSig = unserialize(gzinflate(/*1522005763*/base64_decode("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")));
$g_PhishingSig = unserialize(gzinflate(/*1522005763*/base64_decode("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")));
$g_JSVirSig = unserialize(gzinflate(/*1522005763*/base64_decode("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")));
$gX_JSVirSig = unserialize(gzinflate(/*1522005763*/base64_decode("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")));
$g_SusDB = unserialize(gzinflate(/*1522005763*/base64_decode("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")));
$g_SusDBPrio = unserialize(gzinflate(/*1522005763*/base64_decode("RdPLccMwDEXRloQvAaWaLFNDxr1nZOMiCw8wFHX4BFnft6jcvz/39fX5yVSdalN9akzNqWdqzb09VRZEFEjBFFBBFVjBlaJpkiHrZkVWZEVWZEVWZEVWMhsDMGTbMSAbsiEbsiEbspHZyezIjuw7YWRHdmRHdmRHDuRADuRAjn15yJHzpIEcyNHz9pNpJHIiJ3I6m/d/QeZEzmIPcz5kPshHuYR8nJWgyXHOYeWRnxsOmQu5hBUyF3KRuchcZK79NzONInMzjRYaMjdyI3fQkLkPDdPo/y/l2k7mgeTS7Wyv+q7Fdhwg134xV+3VPUP2DNkzZM94f5GfzndfbJfbPWe8d0jtWnPvfJivPw==")));
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
                              
define('AI_VERSION', '20180325');

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
