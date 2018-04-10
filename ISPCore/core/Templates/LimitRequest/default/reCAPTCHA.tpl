﻿<!DOCTYPE html>
<html lang='ru'>
<head>
    <title>Защита от ботов</title>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
    <meta name='referrer' content='no-referrer' />
    <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.css'>
    <link rel='stylesheet' href='/statics/antibot.css'>
    <script type='text/javascript' src='/statics/jquery.min.js'></script>
    <script src='https://www.google.com/recaptcha/api.js'></script>
</head>

<body class='payment payment-secuses'>
    <div class='text-center payment-box'>
        <div>
            <i id='FaWait' class='fa fa-reddit-alien payment-icon' style='display: none'></i>
            <i id='FaError' class='fa fa-wheelchair-alt payment-icon' style='color: #F04E51'></i>
        </div>
        <h3 class='payment-title'>AntiBot</h3>
        <p class='payment-text' id='InfoText'>У вас должен быть включён JavaScript и Cookie</p>

        <div class='block'>
            <div class='captcha'>
                <div class='g-recaptcha' data-sitekey='{isp:reCAPTCHASitekey}'></div>
            </div>
        </div>

        <div class='block' style='margin-top: 0px;'>
            <div class='copyright'>
                <div style='margin-bottom: 10px;'>
                    © 2018 <strong>ISPCore</strong>. All rights reserved.
                </div>
                <div>
                    <a href='/'>Главная сайта</a> / <a href='http://core-system.org/' target='_blank'>Core System</a>
                </div>
            </div>
        </div>
    </div>

    <script>
        $('#InfoText').text('Пожалуйста, подтвердите что Вы не робот!')
        $('#FaWait').show()
        $('#FaError').hide()
    </script>

    <script>
		var timer;
		timer = setInterval(function()
		{
			var key = $('.g-recaptcha-response').val();

			if(key){
				clearInterval(timer);

				$.post('{isp:CoreApiUrl}/check/recaptcha/limitrequest', { recaptchaKey: key, IP: '{isp:IP}', ExpiresToMinute: {isp:ExpiresToMinute}, hash: '{isp:hash}' }, function (data) 
				{
					var json = JSON.parse(JSON.stringify(data));
					if (json.result) {
						location.reload();
					}
					else {
						$('#InfoText').text('Упс! Кажется что-то пошло не так')
						$('#FaError').show()
						$('#FaWait').hide()
					}
				}).fail(function(){ 
					$('#InfoText').text('Упс! Кажется что-то пошло не так')
					$('#FaError').show()
					$('#FaWait').hide()
				});
			}
		},300)
    </script>


</body>
</html>