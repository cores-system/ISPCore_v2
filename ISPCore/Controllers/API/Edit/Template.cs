using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using ISPCore.Models.Response;
using System.Linq;
using ISPCore.Engine.core.Cache.CheckLink;
using ISPCore.Models.RequestsFilter.Templates;
using Trigger = ISPCore.Models.Triggers.Events.RequestsFilter.Template;

namespace ISPCore.Controllers
{
    public class ApiEditTemplate : ControllerToDB
    {
        public JsonResult Base(int Id, string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                return Json(new Text("Имя шаблона не может быть пустым"));

            // Поиск шаблона
            if (coreDB.RequestsFilter_Templates.Where(i => i.Id == Id).FirstOrDefault() is Template item)
            {
                // Меняем имя
                item.Name = Name;

                // Сохраняем базу
                coreDB.SaveChanges();

                // Удаляем кеш для шаблона
                ISPCache.RemoveTemplate(Id);

                // 
                Trigger.OnChange((Id, 0));

                // Успех
                return Json(new TrueOrFalse(true));
            }

            return Json(new Text("Шаблон не найден"));
        }
    }
}
