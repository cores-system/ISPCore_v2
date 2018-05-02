using System;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using ISPCore.Models.api;
using ISPCore.Engine.Base;
using ISPCore.Models.Security;
using ISPCore.Models.core.Cache.CheckLink;
using ISPCore.Models.Base.WhiteList;
using ISPCore.Models.Base;

namespace ISPCore.Models.Databases.json
{
    public class JsonDB
    {
        public Base Base { get; set; } = new Base();
        public API API { get; set; } = new API();
        public Security Security { get; set; } = new Security();
        public List<WhiteListModel> WhiteList { get; set; } = new List<WhiteListModel>();
        public AntiDdos AntiDdos { get; set; } = new AntiDdos();
        public AntiVirus AntiVirus { get; set; } = new AntiVirus();
        public AntiBot AntiBot { get; set; } = new AntiBot();
        public ServiceBot ServiceBot { get; set; } = new ServiceBot();
        public BruteForceConf BruteForceConf { get; set; } = new BruteForceConf();
        public List<ProjectNews> ProjectNews { get; set; } = new List<ProjectNews>();
        public List<ProjectChange> ProjectChange { get; set; } = new List<ProjectChange>();

        public void Save()
        {
            jsonDB = this;
            File.WriteAllText($"{Folders.Databases}/ISPCore.json", JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        private static JsonDB jsonDB = null;
        static JsonDB()
        {
            if (File.Exists($"{Folders.Databases}/ISPCore.json")) {
                jsonDB = JsonConvert.DeserializeObject<JsonDB>(File.ReadAllText($"{Folders.Databases}/ISPCore.json"));
            }
        }

        public JsonDB()
        {
            if (jsonDB != null)
            {
                Base = jsonDB.Base;
                API = jsonDB.API;
                Security = jsonDB.Security;
                WhiteList = jsonDB.WhiteList;
                AntiDdos = jsonDB.AntiDdos;
                AntiBot = jsonDB.AntiBot;
                AntiVirus = jsonDB.AntiVirus;
                ServiceBot = jsonDB.ServiceBot;
                BruteForceConf = jsonDB.BruteForceConf;
                ProjectNews = jsonDB.ProjectNews;
                ProjectChange = jsonDB.ProjectChange;
            }
        }
    }
    

    public class Base
    {
        private string _CoreAPI;
        private int _CountParallel;

        /// <summary>
        /// Адрес к Core API
        /// </summary>
        public string CoreAPI
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CoreAPI))
                    return "/core";

                return Regex.Replace(_CoreAPI.Trim(), "/+$", "");
            }
            set { _CoreAPI = value; }
        }

        /// <summary>
        /// Количиство потоков в Parallel.ForEach
        /// </summary>
        public int CountParallel
        {
            get
            {
                if (_CountParallel < 1)
                    return 1;

                return _CountParallel;
            }
            set { _CountParallel = value; }
        }

        /// <summary>
        /// Автоматическое обновление ISPCore
        /// </summary>
        public bool AutoUpdate { get; set; } = true;

        /// <summary>
        /// Режим дебага
        /// </summary>
        public bool DebugEnabled { get; set; }

        /// <summary>
        /// Глобально останавлиывает запись любых логов
        /// </summary>
        public bool DisableWriteLog { get; set; }

        /// <summary>
        /// Показывать заглушку для доменов которые не закреплены
        /// /core/check/link
        /// </summary>
        public bool EnableToDomainNotFound { get; set; } = true;

        /// <summary>
        /// Авторизация 2FA
        /// </summary>
        public bool EnableTo2FA { get; set; }

        /// <summary>
        /// Количиство новых уведомлений
        /// </summary>
        public int CountNotification { get; set; }
    }

    public class Security
    {
        private int _CountAccess, _BlockingTime;

        /// <summary>
        /// Количество попыток авторизации за 10 минут
        /// </summary>
        public int CountAccess
        {
            get { return _CountAccess == 0 ? 5 : _CountAccess; }
            set { _CountAccess = value; }
        }

        /// <summary>
        /// Время блокировки в минутах
        /// </summary>
        public int BlockingTime
        {
            get { return _BlockingTime == 0 ? 40 : _BlockingTime; }
            set { _BlockingTime = value; }
        }

        /// <summary>
        /// https://www.google.com/recaptcha/admin
        /// </summary>
        public string reCAPTCHASecret { get; set; }

        /// <summary>
        /// https://www.google.com/recaptcha/admin
        /// </summary>
        public string reCAPTCHASitekey { get; set; }
    }

    public class AntiDdos
    {
        private string _CheckPorts, _Interface;
        private int _BlockingTime, _numberOfRequestsIn120Second, _maximumBurstSize;

        /// <summary>
        /// Включить/Выключить AntiDdos
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Интерфейс (eth0/ens33)
        /// </summary>
        public string Interface
        {
            get
            {
                if (_Interface != null)
                    return _Interface;

                return @"any";
            }
            set { _Interface = value; }

        }
        /// <summary>
        /// Список проверяемых портов
        /// </summary>
        public string CheckPorts
        {
            get
            {
                if (_CheckPorts != null)
                    return _CheckPorts;

                return @"80,443,53";
            }
            set { _CheckPorts = value; }
        }

        /// <summary>
        /// Вести журнал заблокированых IP
        /// </summary>
        public bool Jurnal { get; set; }

        /// <summary>
        /// Записывать информацию GeoIP об IP
        /// </summary>
        public bool GeoIP { get; set; }

        /// <summary>
        /// Проверять DNSLookup на белый список
        /// </summary>
        public bool DNSLookupEnabled { get; set; }

        /// <summary>
        /// Выполнять блокировку в IPtables или нет
        /// </summary>
        public bool BlockToIPtables { get; set; } = true;

        /// <summary>
        /// Время блокировки в минутах
        /// </summary>
        public int BlockingTime
        {
            get { return _BlockingTime == 0 ? 15 : _BlockingTime; }
            set { _BlockingTime = value; }
        }

        /// <summary>
        /// Режим активной блокировки
        /// </summary>
        public bool ActiveLockMode { get; set; } = true;

        /// <summary>
        /// Максимальное количиство запросов за 120 секунд
        /// </summary>
        public int NumberOfRequestsIn120Second
        {
            get { return _numberOfRequestsIn120Second == 0 ? 400 : _numberOfRequestsIn120Second; }
            set { _numberOfRequestsIn120Second = value; }
        }

        /// <summary>
        /// Максимально допустимый размер 'TCP/UDP' 
        /// </summary>
        public int MaximumBurstSize
        {
            get { return _maximumBurstSize == 0 ? 80 : _maximumBurstSize; }
            set { _maximumBurstSize = value; }
        }
    }

    public class AntiBot : AntiBotBase
    {
        /// <summary>
        /// Включить/Выключить
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Список доменов где можно вывести капчу если "type = reCAPTCHA"
        /// </summary>
        public string DomainsToreCAPTCHA { get; set; }

        /// <summary>
        /// Настройки лимитирования запросов
        /// </summary>
        public LimitRequest limitRequest { get; set; } = new LimitRequest();

        /// <summary>
        /// Время обновления настроек
        /// </summary>
        public DateTime LastUpdateToConf { get; set; } = DateTime.Now;

        /// <summary>
        /// Клонирование обьекта
        /// </summary>
        public AntiBot Clone()
        {
            return (AntiBot)this.MemberwiseClone();
        }
    }

    public class API
    {
        /// <summary>
        /// Доступ к API открыт
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Логин 
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// IP для которого не нужна авторизация 
        /// </summary>
        public string WhiteIP { get; set; }
    }

    public class BruteForceConf
    {
        private int _minuteLimit, _hourLimit, _dayLimit;

        /// <summary>
        /// Максимальное количиство запросов в минуту 
        /// </summary>
        public int MinuteLimit
        {
            get
            {
                if (_minuteLimit <= 0)
                    return 10;

                return _minuteLimit;
            }
            set { _minuteLimit = value; }
        }

        /// <summary>
        /// Максимальное количиство запросов за час 
        /// </summary>
        public int HourLimit
        {
            get
            {
                if (_hourLimit <= 0)
                    return 30;

                return _hourLimit;
            }
            set { _hourLimit = value; }
        }

        /// <summary>
        /// Максимальное количиство запросов за сутки 
        /// </summary>
        public int DayLimit
        {
            get
            {
                if (_dayLimit <= 0)
                    return 120;

                return _dayLimit;
            }
            set { _dayLimit = value; }
        }
    }


    public class ServiceBot
    {
        public TelegramBot Telegram { get; set; } = new TelegramBot();
        public EmailBot Email { get; set; } = new EmailBot();
        public SmsBot SMS { get; set; } = new SmsBot();
    }

    public class TelegramBot
    {
        /// <summary>
        /// Токен
        /// </summary>
        public string Token { get; set; }
    }

    public class EmailBot
    {
        /// <summary>
        /// Адрес почтового сервера
        /// </summary>
        public string ConnectUrl { get; set; }

        /// <summary>
        /// Порт почтового сервера
        /// </summary>
        public int ConnectPort { get; set; }

        /// <summary>
        /// Безопасное подключение
        /// </summary>
        public bool useSsl { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Passwd { get; set; }
    }

    public class SmsBot
    {
        /// <summary>
        /// https://smspilot.ru/my-settings.php?tab=acc
        /// </summary>
        public string apikey { get; set; }
    }
}
