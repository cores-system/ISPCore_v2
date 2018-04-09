using ISPCore.Engine.core.Cache.CheckLink;
using ISPCore.Models.Databases;
using ISPCore.Models.RequestsFilter.Domains;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ISPCore.Models.Response;
using ISPCore.Models.RequestsFilter.Domains.Log;
using ISPCore.Models.RequestsFilter.Base;

namespace ISPCore.Controllers
{
    public class ApiAddDomain : ControllerToDB
    {
        #region Domain
        public JsonResult Domain(string host, BruteForceType bruteForceType, Protection Protect, TypeBlockIP typeBlockIP = TypeBlockIP.global)
        {
            return new RequestsFilterToDomainBaseController().Save(new Domain()
            {
                host = host,
                Protect = Protect,
                typeBlockIP = typeBlockIP,
                StopBruteForce = bruteForceType,
            });
        }
        #endregion

        #region Aliases
        public JsonResult Aliases(int DomainId, IDictionary<string, Alias> aliases)
        {
            #region Проверка данных на правильность
            if (aliases != null)
            {
                foreach (var alias in aliases)
                {
                    if (string.IsNullOrWhiteSpace(alias.Value?.host))
                        continue;

                    if (!Regex.IsMatch(alias.Value.host, "^[a-z0-9-\\.]+$", RegexOptions.IgnoreCase))
                        return Json(new Text($"Алиас {alias.Value.host} не должен содержать тип протокола или url"));
                }
            }
            #endregion

            // Поиск домена
            if (coreDB.RequestsFilter_Domains.Where(i => i.Id == DomainId).Include(i => i.Aliases).FirstOrDefault() is Domain domain)
            {
                // Записываем новые алиасы и перезаписываем старые
                domain.Aliases.UpdateOrAddRange(aliases, out var NewAliases);

                // Сохраняем базу
                coreDB.SaveChanges();

                // Удаляем кеш для домена
                ISPCache.RemoveDomain(DomainId);

                // Отдаем сообщение и Id новых алиасов
                return Json(new UpdateToIds("accepted", domain.Id, NewAliases));
            }

            return Json(new Text("Домен не найден"));
        }
        #endregion

        #region TemplatesId
        public JsonResult TemplatesId(int DomainId, IDictionary<string, TemplateId> templates)
        {
            // Записываем новые шаблоны
            coreDB.RequestsFilter_Domain_TemplatesId.AddRange(DomainId, templates, out var NewTemplatesId);

            // Сохраняем базу
            coreDB.SaveChanges();

            // Удаляем кеш для домена
            ISPCache.RemoveDomain(DomainId);

            // Отдаем сообщение и Id новых шаблонов
            return Json(new UpdateToIds("accepted", DomainId, NewTemplatesId));
        }
        #endregion

        #region IgnoreLogs
        public JsonResult IgnoreLogs(int DomainId, IDictionary<string, IgnoreToLog> IgnoreToLogs)
        {
            // Записываем новые данные
            coreDB.RequestsFilter_Domain_IgnoreToLogs.AddRange(DomainId, IgnoreToLogs, out var NewIgnore);

            // Сохраняем базу
            coreDB.SaveChanges();

            // Удаляем кеш для домена
            ISPCache.RemoveDomain(DomainId);

            // Отдаем сообщение и Id новых шаблонов
            return Json(new UpdateToIds("accepted", DomainId, NewIgnore));
        }
        #endregion
    }
}
