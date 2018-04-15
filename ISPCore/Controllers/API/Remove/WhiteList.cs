using ISPCore.Engine;
using ISPCore.Models.Base.WhiteList;
using ISPCore.Models.Databases.json;
using ISPCore.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace ISPCore.Controllers
{
    public class ApiRemoveWhiteList : Controller
    {
        public JsonResult Base(int Id) {
            return new SettingsToWhiteListController().RemoveWhiteList(Id);
        }
    }
}
