using ISPCore.Engine;
using ISPCore.Models.Base.WhiteList;
using ISPCore.Models.Databases.json;
using ISPCore.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace ISPCore.Controllers
{
    public class ApiRemoveWhiteList : Controller
    {
        public JsonResult Users(int Id) {
            return new SettingsToUserWhiteList().Remove(Id);
        }

        public JsonResult Systems(int Id) {
            return new SettingsToSystemWhiteList().Remove(Id);
        }
    }
}
