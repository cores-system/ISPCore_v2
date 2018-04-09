using System;
using Microsoft.AspNetCore.Mvc;

namespace ISPCore.Controllers
{
    public class ApiRemoveTemplate : Controller
    {
        public JsonResult Base(int Id) => new RequestsFilterToTemplateController().Remove(Id);
    }
}
