using ISPCore.Models.Databases.Interface;
using ISPCore.Models.RequestsFilter.Domains.Log;
using ISPCore.Models.RequestsFilter.Domains.Rules;
using System.Collections.Generic;
using Base = ISPCore.Models.RequestsFilter.Base;

namespace ISPCore.Models.RequestsFilter.Domains
{
    public class Domain : IId
    {
        public int Id { get; set; }

        /// <summary>
        /// Основной домен (без www)
        /// </summary>
        public string host { get; set; }

        /// <summary>
        /// Статус защиты
        /// </summary>
        public Protection Protect { get; set; }

        /// <summary>
        /// Настройки локального антивируса
        /// </summary>
        public AntiVirus av { get; set; } = new AntiVirus();

        /// <summary>
        /// Настройки лимитирования запросов
        /// </summary>
        public LimitRequest limitRequest { get; set; } = new LimitRequest();

        /// <summary>
        /// Все запросы подходящие под правила не будут записаны в журналы
        /// </summary>
        public List<IgnoreToLog> IgnoreToLogs { get; set; } = new List<IgnoreToLog>();

        /// <summary>
        /// Настройки логирования запросов
        /// </summary>
        public ConfToLog confToLog { get; set; } = new ConfToLog();

        /// <summary>
        /// Список алиасов 
        /// </summary>
        public List<Alias> Aliases { get; set; } = new List<Alias>();

        /// <summary>
        /// Id шаблонов с дополнительными правилами
        /// </summary>
        public List<TemplateId> Templates { get; set; } = new List<TemplateId>();

        /// <summary>
        /// Список правил
        /// </summary>
        public List<Rule> Rules { get; set; } = new List<Rule>();

        /// <summary>
        /// Список правил для замены ответа
        /// </summary>
        public List<RuleReplace> RuleReplaces { get; set; } = new List<RuleReplace>();

        /// <summary>
        /// Список правил для переопределения
        /// </summary>
        public List<RuleOverride> RuleOverrides { get; set; } = new List<RuleOverride>();

        /// <summary>
        /// Список допустимых аргументов в правиле
        /// </summary>
        public List<RuleArg> RuleArgs { get; set; } = new List<RuleArg>();

        /// <summary>
        /// Блокировка IP в 'Брандмауэр' глобально или только для домена
        /// </summary>
        public TypeBlockIP typeBlockIP { get; set; } = TypeBlockIP.global;

        /// <summary>
        /// Защита сайта от Brute Force
        /// </summary>
        public Base.BruteForceType StopBruteForce { get; set; } = Base.BruteForceType.Not;

        /// <summary>
        /// Авторизация на страницы 2FA дает:
        /// AccessTo2FA - Доступ к страницам 2FA
        /// FullAccess  - Полный доступ к сайту
        /// </summary>
        public Base.Auth2faToAccess Auth2faToAccess { get; set; } = Base.Auth2faToAccess.AccessTo2FA;

        /// <summary>
        /// Защита сайта от ботов
        /// </summary>
        public AntiBot AntiBot { get; set; } = new AntiBot();
    }
}
