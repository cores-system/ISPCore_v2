using ISPCore.Engine;
using ISPCore.Engine.Base;
using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Models.Base.WhiteList;
using ISPCore.Models.Databases.json;
using ISPCore.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace ISPCore.Controllers
{
    public class ApiAddWhiteList : Controller
    {
        public JsonResult Base(WhiteListType type, string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(value) && type == WhiteListType.Not)
                return Json(new TrueOrFalse(false));

            // База
            JsonDB jsonDB = Service.Get<JsonDB>();

            // Добавляем значение
            var md = new WhiteListModel(name, value, type);
            md.Id = int.Parse(Generate.Passwd(6, true));
            jsonDB.WhiteList.Add(md);

            // Успех
            jsonDB.Save();
            WhiteUserList.UpdateCache();
            return Json(new TrueOrFalse(true));
        }
    }
}
