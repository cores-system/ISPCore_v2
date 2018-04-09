using ISPCore.Models.RequestsFilter.Base;
using ISPCore.Models.RequestsFilter.Domains;
using ISPCore.Models.RequestsFilter.Domains.Log;
using System.Collections.Generic;
using ModelCache = ISPCore.Models.core.Cache.CheckLink.Rules;

namespace ISPCore.Models.core.Cache.CheckLink
{
    public class Domain
    {
        /// <summary>
        /// Если данные взяты с кеша
        /// </summary>
        public bool IsCache { get; set; }

        /// <summary>
        /// ID привязаных шаблонов
        /// </summary>
        public List<int> TemplateIds { get; set; } = new List<int>();

        /// <summary>
        /// Список правил для игнорирования логов
        /// </summary>
        public string IgnoreLogToRegex { get; set; } = "^$";

        /// <summary>
        /// Замена ответа
        /// </summary>
        public ModelCache.RuleReplaces RuleReplaces { get; set; } = new ModelCache.RuleReplaces();

        /// <summary>
        /// Список переопределенных правил "Разрешено"
        /// </summary>
        public ModelCache.Rule RuleOverrideAllow { get; set; } = new ModelCache.Rule();

        /// <summary>
        /// Список переопределенных правил "2FA"
        /// </summary>
        public ModelCache.Rule RuleOverride2FA { get; set; } = new ModelCache.Rule();

        /// <summary>
        /// Список переопределенных правил "Запрещено"
        /// </summary>
        public ModelCache.Rule RuleOverrideDeny { get; set; } = new ModelCache.Rule();

        /// <summary>
        /// Список правил "Разрешено"
        /// </summary>
        public ModelCache.Rule RuleAllow { get; set; } = new ModelCache.Rule();

        /// <summary>
        /// Список правил 2FA
        /// </summary>
        public ModelCache.Rule Rule2FA { get; set; } = new ModelCache.Rule();

        /// <summary>
        /// Список правил "Запрещено"
        /// </summary>
        public ModelCache.Rule RuleDeny { get; set; } = new ModelCache.Rule();

        /// <summary>
        /// Блокировка IP в 'Брандмауэр' глобально или только для домена
        /// </summary>
        public TypeBlockIP typeBlockIP { get; set; }

        /// <summary>
        /// Настройки логирования запросов
        /// </summary>
        public ConfToLog confToLog { get; set; } = new ConfToLog();

        /// <summary>
        /// Настройки лимитирования запросов
        /// </summary>
        public LimitRequest limitRequest { get; set; } = new LimitRequest();

        /// <summary>
        /// Защита сайта от BruteForce
        /// </summary>
        public BruteForceType StopBruteForce { get; set; }

        /// <summary>
        /// Авторизация на страницы 2FA дает:
        /// AccessTo2FA - Доступ к страницам 2FA
        /// FullAccess  - Полный доступ к сайту
        /// </summary>
        public Auth2faToAccess Auth2faToAccess { get; set; } = Auth2faToAccess.AccessTo2FA;

        /// <summary>
        /// Защита сайта от ботов
        /// </summary>
        public AntiBot AntiBot { get; set; } = new AntiBot();
    }
} 
