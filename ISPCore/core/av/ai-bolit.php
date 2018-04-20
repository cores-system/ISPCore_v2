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

//BEGIN_SIG 19/04/2018 09:45:21
$g_DBShe = unserialize(gzinflate(/*1524163521*/base64_decode("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")));
$gX_DBShe = unserialize(gzinflate(/*1524163521*/base64_decode("bVX9b+I4EP1XvDn2oFIpCUmgpNvqKLDtXT9V2L1dVVVkEhOsOB9rO7Swuvvbzx67tFodv5A8j59n3htPcDTwop80ck9ENIyclKxwQrhzQiNPIf0gcr5XDUpwiepGIoyKNERCclpmOqavYo4jp17XYk0Y05CvoEE/cj6ldIMShoU4dZasSnK0lNuaeM7Z+5VUVvUvyFIWzplmCgx5MhrtyUMD8XC4hwYm8dvFFKfpViNDu4+u90HHJujzC5BpZKQQT6X5N1miuQ5DS9jsaSE8TyuxoWw3f2XwQI4wci5xkpNURaMxW+ElkdVXymWDTZQV5NaX+7M93zLOZcWL4RtjYPHrKmnEOzy0qb1WeaTkhQVd6ihycCnpG/vQshRbwMgLSQA/NpmY2u5ywKBoZakO6ji8QF2OuiswEsrWmRPUpJhVAu1wxpsdlbCsqw/VKa2CCIEzgk4R4SSLOamZapiO8zGcfOz3nUPkmL/XwIMT2K918VTuteociZx5jQuSts8+LTlY3dcaBW7kCCKTqsop6SBd0Q8WP5NljNOClnEjCC/VPgdZUi2gr2wlTBC66qyaMpG0KmPyQoUUHQcEiUGRgwPYoaUNVRp0hTpUxAlmDC+ZSt8EIVym6IM6CnOOtxY9bKVU6CjNb3m0E35geDqtmvBCoN+R+zJxXfcAnZ7uH39COHikqkspR727C9T7BrC2yB8oljJhTUo6rXg+e/g6e3hsXy4W9/EX9RaPL2a3i/aTOXZk2njJ3WfhwyX1tW+Bqqi9lrJOj5KqXLUP25t1pRR4fUtWme4h/aQAal5gt2fr6NW8SnpiK3q50piw3hYXuFdLrryNRVLVBMK1i31fCb7BrLOijMQZkbEilaTUgusY3zaSkveZU2nk7W0w78FyYFuwdXF9dz6+nj+2Y/WDJTAnhJ17Y9ragvavxhhUNdk7ZwRwaGOUormrpIB74Gvt+wrKCkxZVxSy7tLyiB1lVZUxoiQqIMxOCNVt3HWh5/2RvVlTPQng2oPkgR2V07dRGXg29BHdr5WUL1v0BLgdB49JlRK+M5gWyNcD8kO3+5suBCVFeuq0wPPxZDK7Xzio2zUjMLCz4OmRV02ZdtyDJ5A5CF+HCi3GUr4lpwXwlIhTzPMJJ8+fOSVlCuIEQ5N43mfM91OAbNlX+ZXn+0OARpbhcjy5mk3R+Xc0X9w93MD4da17N+qKqW+CEQXIQ89uu+FH87Xq6DUuv3mjAaz1zeCaEppijv+F7g99eyn+omWtRlpV7AAOTKxXDv1Eem4JYGjPvVLfnXnOm1pd7MqcO/h/mqGhmZAyo7tLbGiODcj9jV9mQWkIRgY8v1b1frn9E5IbuHbUckmTnJE/nJN//gM=")));
$g_FlexDBShe = unserialize(gzinflate(/*1524163521*/base64_decode("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")));
$gX_FlexDBShe = unserialize(gzinflate(/*1524163521*/base64_decode("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")));
$gXX_FlexDBShe = unserialize(gzinflate(/*1524163521*/base64_decode("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")));
$g_ExceptFlex = unserialize(gzinflate(/*1524163521*/base64_decode("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")));
$g_AdwareSig = unserialize(gzinflate(/*1524163521*/base64_decode("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")));
$g_PhishingSig = unserialize(gzinflate(/*1524163521*/base64_decode("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")));
$g_JSVirSig = unserialize(gzinflate(/*1524163521*/base64_decode("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")));
$gX_JSVirSig = unserialize(gzinflate(/*1524163521*/base64_decode("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")));
$g_SusDB = unserialize(gzinflate(/*1524163521*/base64_decode("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")));
$g_SusDBPrio = unserialize(gzinflate(/*1524163521*/base64_decode("RdPLccMwDEXRloQvAaWaLFNDxr1nZOMiCw8wFHX4BFnft6jcvz/39fX5yVSdalN9akzNqWdqzb09VRZEFEjBFFBBFVjBlaJpkiHrZkVWZEVWZEVWZEVWMhsDMGTbMSAbsiEbsiEbspHZyezIjuw7YWRHdmRHdmRHDuRADuRAjn15yJHzpIEcyNHz9pNpJHIiJ3I6m/d/QeZEzmIPcz5kPshHuYR8nJWgyXHOYeWRnxsOmQu5hBUyF3KRuchcZK79NzONInMzjRYaMjdyI3fQkLkPDdPo/y/l2k7mgeTS7Wyv+q7Fdhwg134xV+3VPUP2DNkzZM94f5GfzndfbJfbPWe8d0jtWnPvfJivPw==")));
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
                              
define('AI_VERSION', '20180419');

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
