using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.RequestsFilter.Templates;

namespace ISPCore.Controllers
{
    public class ApiAddTemplate : Controller
    {
        public JsonResult Base(string Name) => new RequestsFilterToTemplateController().Save(new Template() { Name = Name }, IsAPI: true);
    }
}
